using MailCollector.Kit;
using MailCollector.Kit.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MailCollector.Setup
{
    public class MailCollectorInstaller
    {
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
            _logger.WriteLine($"\tУстановка сервиса 'MailCollector.Service': {_isSetupService}");
            _logger.WriteLine($"\tУстановка клиента: {_isSetupClient}");
            _logger.WriteLine($"\tДобавление Telegram-бота: {_isAddTgBot}");
            _logger.WriteLine($"");

            if (_isCreateDb)
            {
                _logger.WriteLine($"Начало создания БД '{KitConstants.DbName}'");

                // TODO: Подключаюсь к СУБД, создаю БД
                // TODO: Создаю системного юзера админом, добавляю текущему прав
                // TODO: Добавлю клиентов с их imap серверами

                _logger.WriteLine($"БД '{KitConstants.DbName}' успешно установлена");
                _logger.WriteLine($"");
            }

            if (_isSetupService)
            {
                _logger.WriteLine($"Начало установки сервиса 'MailCollector.Service'");

                // TODO: Скопировать в выбранный путь
                // TODO: Создать конфиг // TODO: В конфиг успешно добавлен тг-бот
                // TODO: Зарегистрировать службу

                _logger.WriteLine($"Сервис 'MailCollector.Service' успешно установлен " +
                    $"и помещён в автоматический запуск в системе");
                _logger.WriteLine($"");
            }

            if (_isSetupClient)
            {
                _logger.WriteLine($"Начало установки клиента 'MailCollector.Client'");

                // TODO: Скопировать в выбранный путь
                // TODO: Создать конфиг

                _logger.WriteLine($"Клиент 'MailCollector.Client' успешно установлен");
                _logger.WriteLine($"");
            }

            if (!_isSetupService && _isAddTgBot)
            {
                _logger.WriteLine($"Начало добавления Telegram-бота");

                // TODO: По пути сервиса найти конфиг, десериализовать и добавить токен тг-бота.
                    // Сериализовать и сохранить

                _logger.WriteLine($"Telegram-бот успешно добавлен");
                _logger.WriteLine($"");
            }

            _logger.WriteLine("Установка успешно завершена!");
        }
    }
}
