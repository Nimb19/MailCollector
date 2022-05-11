using MailCollector.Kit.ImapKit;
using MailCollector.Kit.Logger;
using MailCollector.Kit.SqlKit;
using MailCollector.Kit.SqlKit.Models;
using MailCollector.Kit.TelegramBotKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MailCollector.Kit.ServiceKit
{
    /// <summary>
    ///     Вся логика сервиса запускается методом Start(). 
    ///     Вынес всю её сюда, чтобы была возможность отладки 
    ///     (сервисы не запустить под отладкой, поэтому, для отладки, стартую воркер из консольного отладочного проекта).
    /// </summary>
    public class ServiceWorker : IDisposable
    {
        private readonly ILogger _logger;
        private readonly SqlServerShell _sqlServerShell;
        private readonly CancellationToken _cancellationToken;
        private readonly MailTelegramBot _mailTelegramBot = null;

        private DateTime _lastReconnectTry = DateTime.Now;

        private static readonly TimeSpan _checkerUpdateDelay = TimeSpan.FromSeconds(3);
        private static readonly TimeSpan _checkNewMailsDelay = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan _checkIfCancelationRequestedDelay = TimeSpan.FromMilliseconds(500);
        private static readonly TimeSpan _delayBeforeTryReconnectDisabledClients = TimeSpan.FromMinutes(10);

        /// <summary>
        ///     Под каждого клиента создаётся свой отдельый бесконечный поток. 
        ///     Такие рабочие потоки фиксируются в этот список.
        /// </summary>
        private readonly List<(Task Worker, CancellationTokenSource Cts, ImapClient Client)> _workingClients;
        /// <summary>
        ///     Отключённые клиенты (отключаются во время ошибок в своих потоках)
        /// </summary>
        private readonly List<(ImapClient Client, ImapServer Server)> _disabledClients;

        // Если нужно использовать оба лока, но не хочешь дэдлока - нужно лочить сначала _disabledClientsListLock
        private readonly object _disabledClientsListLock = new object();
        private readonly object _workingClientsListLock = new object();

        /// <summary>
        ///     Поток, который запускает, смотрит и управляет клиентскими потоками. 
        ///     Если какой то падает с ошибкой, то убирает, ставит галку что клиент не рабочий 
        ///     и пытается перезапускать его с определённым интервалом.
        ///     Если поступила команда на остановку сервиса, то освобождает клиентские потоки.
        /// </summary>
        private Task _clientsWorkersChecker;

        public bool IsAllTasksCompleted { get; private set; }
        public bool IsStarted { get; private set; }

        public ServiceWorker(SqlServerSettings sqlServerSettings, string tgBotToken
            , ILogger logger, string moduleName, CancellationToken cancellationToken)
        {
            _logger = logger;
            _sqlServerShell = new SqlServerShell(sqlServerSettings, logger, moduleName, KitConstants.DbName);
            _cancellationToken = cancellationToken;
            
            _workingClients = new List<(Task Worker, CancellationTokenSource Cts, ImapClient Client)>();
            _disabledClients = new List<(ImapClient Client, ImapServer Server)>();

            if (!string.IsNullOrWhiteSpace(tgBotToken))
                _mailTelegramBot = new MailTelegramBot(tgBotToken, _sqlServerShell, logger);

            _mailTelegramBot.Start(_cancellationToken);

            _logger.WriteLine("ServiceWorker успешно инициализирован");
        }

        /// <summary>
        ///     Старт сервиса. Завершается отменой токена или командой стоп.
        /// </summary>
        public void Start()
        {
            if (IsStarted)
                return;

            _logger.WriteLine("Подана команда старта сервиса. Клиенты скоро будут запущены");

            _clientsWorkersChecker = Task.Factory.StartNew((a) => CheckerWorker()
                , TaskContinuationOptions.LongRunning);

            IsStarted = true;
        }
        
        public void Stop()
        {
            if (IsStarted && !_cancellationToken.IsCancellationRequested)
            {
                _logger.WriteLine("Поступила команда остановки сервиса. Токен отменён не был");
                IsStarted = false;
            }
        }

        /// <summary>
        ///     Запуск дирижёра клиентских потоков. 
        /// </summary>
        private void CheckerWorker()
        {
            var clientsWithServers = MailCollectorSqlAdapter.GetAllClientsWithServers(_sqlServerShell, _cancellationToken);
            StartClientsWorkers(clientsWithServers);

            var work = Task.Factory.StartNew(() =>
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    CheckDisabledClientsAndUpdates();
                    TryReconnectDisabledClients();

                    Task.Delay(_checkerUpdateDelay).Wait(_cancellationToken);
                }
            }, TaskCreationOptions.LongRunning);

            Task.Factory.StartNew(() =>
            {
                while (!_cancellationToken.IsCancellationRequested || !IsStarted)
                {
                    Task.Delay(_checkIfCancelationRequestedDelay).Wait();
                }
            }, TaskCreationOptions.LongRunning).Wait();

            CancelAllClientsTasks();
        }

        /// <summary>
        ///     Попытка включения отключённых клиентов.
        /// </summary>
        private void TryReconnectDisabledClients()
        {
            if (DateTime.Now - _lastReconnectTry > _delayBeforeTryReconnectDisabledClients)
            {
                lock (_disabledClients)
                {
                    _logger.WriteLine($"Выполняется попытка перезагрузки" +
                        $" отключённх клиентов: {string.Join(";", _disabledClients)}" +
                        $"{Environment.NewLine}Каждая попытка проходит" +
                        $" один раз в {_delayBeforeTryReconnectDisabledClients.TotalMinutes} минут");
                    StartClientsWorkers(_disabledClients);
                }
            }
        }

        /// <summary>
        ///     Отмена всех клиенстких потоков.
        /// </summary>
        private void CancelAllClientsTasks()
        {
            lock (_workingClients)
            {
                CancelSelectedClientsTask(_workingClients, "Поступила команда отмены работы сервиса");
            }
            IsAllTasksCompleted = true;
        }

        /// <summary>
        ///     Отмена указанных клиенстких потоков.
        /// </summary>
        private void CancelSelectedClientsTask(
            List<(Task Worker, CancellationTokenSource Cts, ImapClient Client)> workingClientsForCancel
            , string argument)
        {
            _logger.WriteLine($"Поступила команда отмены и удаления потоков клиентов: {string.Join(";", workingClientsForCancel)}" +
                $"{Environment.NewLine}Причина: {argument}");

            foreach (var clientWorker in workingClientsForCancel)
            {
                clientWorker.Cts.Cancel();
            }

            workingClientsForCancel.ForEach(x =>
            {
                var waitSecondsForDispose = TimeSpan.FromSeconds(1);
                if (!x.Worker.Wait(waitSecondsForDispose))
                {
                    _logger.Warning($"Поток с клиентом {x.Client} слишком долго отменяется" +
                        $", поэтому сервис не будет ждать его завершения. Количество секунд ожидания было: {waitSecondsForDispose.TotalSeconds}");
                }
            });

            _logger.WriteLine("Клиенсткие потоки успешно отменены");
            IsAllTasksCompleted = true;
        }

        /// <summary>
        ///     Старт указанных клиентских потоков.
        /// </summary>
        private void StartClientsWorkers(IList<(ImapClient Client, ImapServer Server)> clientsWithServers)
        {
            if (clientsWithServers.Count == 0)
            {
                _logger.Warning($"Пришло нулевое количество клиентов на их старт");
                return;
            }

            IsAllTasksCompleted = false;
            foreach (var clientWithServer in clientsWithServers)
            {
                var cts = new CancellationTokenSource();
                var clientWorkerTask = Task.Factory.StartNew((a) => ClientWorker(clientWithServer)
                    , TaskContinuationOptions.LongRunning, cts.Token);

                lock (_workingClientsListLock)
                {
                    _workingClients.Add((clientWorkerTask, cts, clientWithServer.Client));
                }
            }
        }

        /// <summary>
        ///     Проверка, были ли клиенты отключены, и занесение таких в список.
        /// </summary>
        private void CheckDisabledClientsAndUpdates()
        {
            List<(ImapClient Client, ImapServer Server)> clientsForStart;

            lock (_disabledClientsListLock)
            {
                foreach (var disClient in _disabledClients)
                {
                    bool isNotNull = false;
                    lock (_workingClientsListLock)
                    {
                        var notWorkingClient = _workingClients.FirstOrDefault(x => x.Client.Uid == disClient.Client.Uid);
                        if (notWorkingClient.Client?.Uid != null)
                        {
                            isNotNull = true;
                            _workingClients.Remove(notWorkingClient);
                        }
                    }

                    if (isNotNull)
                    {
                        TryUpdateClientIsWorking(disClient.Client, false);
                    }
                }

                clientsForStart = UpdateClientsInfo();
                
            }

            if (clientsForStart.Count != 0)
                StartClientsWorkers(clientsForStart);

            return;

            /// <summary>
            ///    Обновление списков клентов. 
            ///    Если появились новые, то запускает их. 
            ///    Если каких то клиентов больше нет, то убивает их потоки и убирает из списков. 
            ///    
            ///    Должен запускаться после выравнивания клиентов по спискам путём метода выше 
            ///    + под локом от отключённого списка клиентов.
            /// </summary>
            List<(ImapClient Client, ImapServer Server)> UpdateClientsInfo()
            {
                var clientsWithServers = MailCollectorSqlAdapter.GetAllClientsWithServers(_sqlServerShell, _cancellationToken);
                var clientsForStartInternal = new List<(ImapClient Client, ImapServer Server)>();

                lock (_workingClientsListLock)
                {
                    // Сначала добавляются из отключённого списка
                    // true, если в лист добавлен с рабочего списка
                    List<(ImapClient Client, bool IsInWorkingList)> allClients = _disabledClients.Select(x => (x.Client, false)).ToList();
                    allClients.AddRange(_workingClients.Select(x => (x.Client, true)));
                    var clientsToStart = AddNewClients(allClients);
                    RemoveOldClients(allClients);
                }

                return clientsForStartInternal;

                IReadOnlyList<(ImapClient Client, ImapServer Server)> AddNewClients(IReadOnlyList<(ImapClient Client, bool IsInWorkingList)> allClients)
                {
                    foreach (var maybeNewClient in clientsWithServers)
                    {
                        var foundedClient = allClients.FirstOrDefault(x => x.Client.Uid == maybeNewClient.Client.Uid);

                        if (foundedClient.Client != null)
                        {
                            // Если параметры для авторизации клиента были изменены
                            if (!foundedClient.Client.Equals(maybeNewClient))
                            {
                                const string removeArgumentToLog = "параметры клиента были изменены." +
                                    " Скоро запуститься поток этого клиента с новыми параметрами";
                                RemoveClient(foundedClient, removeArgumentToLog);
                                clientsForStartInternal.Add(maybeNewClient);
                            }
                        }
                        else
                        {
                            clientsForStartInternal.Add(maybeNewClient);
                        }
                    }
                    return clientsForStartInternal;
                }

                void RemoveOldClients(IReadOnlyList<(ImapClient Client, bool IsInWorkingList)> allClients)
                {
                    foreach (var maybeOldClient in allClients)
                    {
                        var foundedClient = clientsWithServers.FirstOrDefault(x => x.Client.Uid == maybeOldClient.Client.Uid);
                        if (foundedClient.Client == null)
                        {
                            const string removeArgumentToLog = "клиент был удалён из БД";
                            RemoveClient(maybeOldClient, removeArgumentToLog);
                        }
                    }
                }

                void RemoveClient((ImapClient Client, bool IsInWorkingList) foundedClientInternal, string removeArgumentToLog)
                {
                    if (!foundedClientInternal.IsInWorkingList)
                    {
                        _logger.WriteLine($"Клиент {foundedClientInternal.Client}" +
                            $" будет остановлен удалён по причине: {removeArgumentToLog}");
                        var iAm = _disabledClients
                                    .FirstOrDefault(x => x.Client.Uid == foundedClientInternal.Client.Uid);
                        _disabledClients.Remove(iAm);
                    }
                    else
                    {
                        var clientForRemove = _workingClients.First(x => x.Client.Uid == foundedClientInternal.Client.Uid);
                        CancelSelectedClientsTask(
                            new List<(Task Worker, CancellationTokenSource Cts, ImapClient Client)>() { clientForRemove }
                            , removeArgumentToLog);
                    }
                }
            }
        }

        private void TryUpdateClientIsWorking(ImapClient client, bool? isWorkingNow)
        {
            try
            {
                MailCollectorSqlAdapter.UpdateClientIsWorking(_sqlServerShell, isWorkingNow, client);
            }
            catch (Exception ex)
            {
                _logger.Error($"Во время обновления поля {nameof(ImapClient.IsWorking)}" +
                    $", для клента {client} на значение {isWorkingNow?.ToString() ?? "NULL"}, была выброшена ошибка: {ex}");
            }
        }

        private void ClientWorker((ImapClient Client, ImapServer Server) clientWithServer)
        {
            MailKit.Net.Imap.ImapClient imapClient = null;
            try
            {
                imapClient = ImapClientExtensions.Connect(clientWithServer.Client
                    , clientWithServer.Server, _cancellationToken);
                TryUpdateClientIsWorking(clientWithServer.Client, true);

                var clientSqlShell = new MailCollectorSqlAdapter(_sqlServerShell
                    , clientWithServer.Client, _cancellationToken);

                var isInited = clientSqlShell.Folders.Any();
                if (!isInited)
                {
                    imapClient.FetchAndSaveLastMailsFromAllFolders(clientSqlShell, _mailTelegramBot
                        , _logger, _cancellationToken, isInited: false);
                    Task.Delay(_checkNewMailsDelay).Wait(_cancellationToken);
                }

                while (true)
                {
                    imapClient.FetchAndSaveLastMailsFromAllFolders(clientSqlShell, _mailTelegramBot
                        , _logger, _cancellationToken);
                    Task.Delay(_checkNewMailsDelay).Wait(_cancellationToken); 
                    // Нет необходимости тратить время на поимку событий
                    // о новых письмах и отладку такого кода. Ресурсов хватает

                    _cancellationToken.ThrowIfCancellationRequested();
                }
            }
            catch (TaskCanceledException cex)
            {
                _logger.WriteLine($"Поток для клиента {clientWithServer.Client} был отменён и удалён. Message: '{cex.Message}'");
                lock (_workingClientsListLock)
                {
                    var iAm = _workingClients.FirstOrDefault(x => x.Client.Uid == clientWithServer.Client.Uid);
                    _workingClients.Remove(iAm);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка в потоке клиента {clientWithServer.Client}. {ex}");
                lock (_disabledClientsListLock)
                {
                    _disabledClients.Add(clientWithServer);
                }
                lock (_workingClientsListLock)
                {
                    var iAm = _workingClients.FirstOrDefault(x => x.Client.Uid == clientWithServer.Client.Uid);
                    _workingClients.Remove(iAm);
                }
            }
            finally
            {
                imapClient.TryDispose(_logger);
            }
        }

        public void Dispose()
        {
            _sqlServerShell?.Dispose();
        }
    }
}
