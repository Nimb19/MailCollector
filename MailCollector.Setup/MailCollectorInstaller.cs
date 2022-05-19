using MailCollector.Kit;
using MailCollector.Kit.Logger;
using MailCollector.Kit.ServiceKit;
using MailCollector.Kit.SqlKit;
using MailCollector.Kit.SqlKit.Models;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MailCollector.Setup
{
    public class MailCollectorInstaller
    {
        private const string InstallUtilPath = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe";
        private const string MailServiceName = ServiceInfo.ServiceName;
        private const string MailClientName = "MailCollector.Client";

        private readonly ILogger _logger;
        private readonly InstallerSettings _installerSettings;
        private bool _isCreateDb;
        private bool _isSetupService;
        private bool _isSetupClient;
        private bool _isAddTgBot;

        public MailCollectorInstaller(InstallerSettings installerSettings, ILogger logger)
        {
            _logger = logger;
            _installerSettings = installerSettings;
        }

        public async Task StartInstall(CancellationToken cancellationToken) 
        {
            cancellationToken.ThrowIfCancellationRequested();

            _isCreateDb = _installerSettings.InstallSteps.HasFlag(SetupSteps.CreateDb);
            _isSetupService = _installerSettings.InstallSteps.HasFlag(SetupSteps.InstallService);
            _isSetupClient = _installerSettings.InstallSteps.HasFlag(SetupSteps.InstallClient);
            _isAddTgBot = _installerSettings.InstallSteps.HasFlag(SetupSteps.ConfigureTgBot);

            _logger.WriteLine("Начало установки проекта MailCollector!");
            _logger.WriteLine("Были выбраны следующие параметры установки:");
            _logger.WriteLine($"\tСоздание БД '{KitConstants.DbName}': {_isCreateDb}");
            _logger.WriteLine($"\tУстановка сервиса '{MailServiceName}': {_isSetupService}");
            _logger.WriteLine($"\tУстановка клиента: {_isSetupClient}");
            _logger.WriteLine($"\tДобавление Telegram-бота: {_isAddTgBot}");
            _logger.WriteLine($"");

            if (_isCreateDb)
            {
                await CreateDb(cancellationToken);
            }

            if (_isSetupService)
            {
                await SetupService(cancellationToken);
            }

            if (_isSetupClient)
            {
                await SetupClient(cancellationToken);
            }

            if (!_isSetupService && _isAddTgBot)
            {
                AddTelegramBotToken(cancellationToken);
            }

            _logger.WriteLine("Установка успешно завершена!");
        }

        private void AddTelegramBotToken(CancellationToken cancellationToken)
        {
            _logger.WriteLine($"Начало добавления Telegram-бота");
            if (string.IsNullOrWhiteSpace(_installerSettings.TelegramBotToken))
            {
                _logger.WriteLine("Установка Telegram-бота была пропущена, так как вы не указали токен для подключения");
                _logger.WriteLine($"");
                return;
            }
            cancellationToken.ThrowIfCancellationRequested();

            // По пути сервиса находится конфиг, он десериализуется и добавляется токен тг-бота.
            var configPath = Path.Combine(_installerSettings.InstallServicePath, "Config.json");
            var config = CommonExtensions.DeserializeFile<ServiceConfig>(configPath);
            config.TelegramBotApiToken = _installerSettings.TelegramBotToken;

            // Далее сохраняем конфиг с новым токеном к боту
            CommonExtensions.SerializeToFile(config, configPath);

            _logger.WriteLine($"Telegram-бот успешно добавлен");
            _logger.WriteLine($"");
        }

        private async Task SetupClient(CancellationToken cancellationToken)
        {
            _logger.WriteLine($"Начало установки клиента '{MailClientName}'");
            cancellationToken.ThrowIfCancellationRequested();

            // Копируется билд клиента в выбранный путь
            await Task.Run(() =>
                CommonExtensions.CopyDir(_installerSettings.PathToClientBuild, _installerSettings.InstallClientPath));

            // Генерация и выкладывание конфига клиенту
            var serviceConfig = new ClientConfig()
            {
                SqlServerSettings = _installerSettings.SqlServerSettings,
            };
            var configPath = Path.Combine(_installerSettings.InstallClientPath, "Config.json");
            CommonExtensions.SerializeToFile(serviceConfig, configPath);
            _logger.WriteLine($"Клиент успешно скопирован, конфиг к нему успешно сгенерирован и выложен");

            _logger.WriteLine($"Клиент '{MailClientName}' успешно установлен");
            _logger.WriteLine($"");
        }

        private async Task SetupService(CancellationToken cancellationToken)
        {
            _logger.WriteLine($"Начало установки сервиса '{MailServiceName}'");
            cancellationToken.ThrowIfCancellationRequested();

            // Копируется билд сервиса в выбранный путь
            await Task.Run(() =>
                CommonExtensions.CopyDir(_installerSettings.PathToServiceBuild, _installerSettings.InstallServicePath));

            // Генерация и выкладывание конфига к сервису
            var serviceConfig = new ServiceConfig()
            {
                SqlServerSettings = _installerSettings.SqlServerSettings,
            };
            if (_isAddTgBot)
                serviceConfig.TelegramBotApiToken = _installerSettings.TelegramBotToken;
            var configPath = Path.Combine(_installerSettings.InstallServicePath, "Config.json");
            CommonExtensions.SerializeToFile(serviceConfig, configPath);
            _logger.WriteLine($"Сервис успешно скопирован, конфиг к нему успешно сгенерирован и выложен");

            // Регистрация службы
            cancellationToken.ThrowIfCancellationRequested();
            var isInstalled = ServiceInstaller.ServiceIsInstalled(MailServiceName);
            if (!isInstalled)
            {
                var servicePath = Path.Combine(_installerSettings.InstallServicePath, $"{MailServiceName}.exe");
                ServiceInstaller.Install(ServiceInfo.ServiceName, ServiceInfo.ServiceDisplayName, servicePath);
            }
            _logger.WriteLine($"Сервис успешно зарегистрирован");

            // Пробуем запустить службу
            // (3 попытки, у каждой по 4 секунды ожидания статуса Running у сервиса, после запуска + интервал секунда между попытками)
            // Итог задержка дистра на 15 секунд, если сервис не рабочий, но в проде должно за секунды 3 запуститься
            var trys = 3;
            var isStarted = false;
            Exception exception = null;
            _logger.WriteLine($"Пробуем запустить сервис '{MailServiceName}'...");
            for (int i = 0; i < trys; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                isStarted = await Task.Run(() => CommonExtensions.StartService(MailServiceName, out exception));

                cancellationToken.ThrowIfCancellationRequested();
                if (!isStarted)
                    await Task.Delay(1000);
                else
                    break;
            }
            if (!isStarted)
            {
                _logger.Warning($"Не удалось запустить сервис '{MailServiceName}' за {trys} попытки." +
                    $"{Environment.NewLine}{exception}");
            }
            else
            {
                _logger.WriteLine($"Сервис '{MailServiceName}' был успешно запущен");
            }

            _logger.WriteLine($"Сервис '{MailServiceName}' успешно установлен " +
                $"и помещён в автоматический запуск в системе");
            _logger.WriteLine($"");
        }

        private async Task CreateDb(CancellationToken cancellationToken)
        {
            _logger.WriteLine($"Начало создания БД '{KitConstants.DbName}'");
            cancellationToken.ThrowIfCancellationRequested();

            // Создание БД
            var sqlShell = new SqlServerShell(_installerSettings.SqlServerSettings
                , _logger, Constants.ModuleName, null);
            var cmdCreate = CommonExtensions.ReadFile(_installerSettings.PathToCreateDbScript);
            var commands = cmdCreate.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var command in commands)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Run(() => sqlShell.ExecuteNonQuery(command));
            }
            sqlShell.DbName = KitConstants.DbName;
            _logger.WriteLine($"БД '{KitConstants.DbName}' успешно создана");

            // Добавляю клиентов с их imap серверами
            if (_installerSettings.ImapClients != null && _installerSettings.ImapClients.Length != 0)
            {
                foreach (var imapClientData in _installerSettings.ImapClients)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var serverUid = Guid.NewGuid();
                    var server = new ImapServer()
                    {
                        Uid = serverUid,
                        Uri = imapClientData.ImapServerParams.Uri,
                        Port = imapClientData.ImapServerParams.Port,
                        UseSsl = imapClientData.ImapServerParams.UseSsl,
                    };
                    var client = new ImapClient()
                    {
                        Uid = Guid.NewGuid(),
                        ImapServerUid = serverUid,
                        Login = imapClientData.Login,
                        Password = imapClientData.Password,
                        IsWorking = null,
                    };
                    sqlShell.Insert(server, ImapServer.TableName);
                    sqlShell.Insert(client, ImapClient.TableName);
                }
                _logger.WriteLine($"Указанные IMAP-клиенты успешно добавлены в БД");
            }

            // Если установки сервиса с генерацией конфига не будет,
            // то прописываем существующему новые параметры подключения к БД
            if (!_isSetupService)
            {
                var serviceConfig = CommonExtensions.DeserializeFile<ServiceConfig>(_installerSettings.InstallServicePath);
                serviceConfig.SqlServerSettings = _installerSettings.SqlServerSettings;
                CommonExtensions.SerializeToFile(serviceConfig, _installerSettings.InstallServicePath);
                _logger.WriteLine($"В конфиге сервиса успешно добавлены новые настройки для подключения к БД");
            }

            _logger.WriteLine($"Шаг установки БД '{KitConstants.DbName}' успешно завершён");
            _logger.WriteLine($"");
        }
    }
}
