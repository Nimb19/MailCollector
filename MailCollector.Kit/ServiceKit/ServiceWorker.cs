using MailCollector.Kit.ImapKit;
using MailCollector.Kit.Logger;
using MailCollector.Kit.SqlKit;
using MailCollector.Kit.SqlKit.Models;
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

        /// <summary>
        ///     Под каждого клиента создаётся свой отдельый бесконечный поток. 
        ///     Такие рабочие потоки фиксируются в этот список.
        /// </summary>
        private readonly List<(Task Worker, CancellationTokenSource Cts, ImapClient Client)> _workingClients;
        /// <summary>
        ///     Отключённые клиенты (отключаются во время ошибок в своих потоках)
        /// </summary>
        private readonly List<(ImapClient Client, ImapServer Server)> _diabledClients;

        private readonly object _workingClientsListLock = new object();
        private readonly object _diabledClientsListLock = new object();

        /// <summary>
        ///     Поток, который запускает, смотрит и управляет клиентскими потоками. 
        ///     Если какой то падает с ошибкой, то убирает, ставит галку что клиент не рабочий 
        ///     и пытается перезапускать его с определённым интервалом.
        ///     Если поступила команда на остановку сервиса, то освобождает клиентские потоки.
        /// </summary>
        private Task _clientsWorkersChecker;

        public bool IsAllTasksCompleted { get; private set; } // TODO: 
        public bool IsStarted { get; private set; }

        public ServiceWorker(SqlServerSettings sqlServerSettings, ILogger logger, CancellationToken cancellationToken)
        {
            _logger = logger;
            _sqlServerShell = new SqlServerShell(sqlServerSettings);
            _cancellationToken = cancellationToken;

            _logger.WriteLine("ServiceWorker успешно инициализирован");
        }

        public void Start()
        {
            if (IsStarted)
                return;

            _clientsWorkersChecker = Task.Factory.StartNew((a) => CheckerWorker()
                , TaskContinuationOptions.LongRunning, _cancellationToken);
        }

        private void CheckerWorker()
        {
            var clientsWithServers = SqlServerShellAdapter.GetAllClientsWithServers(_sqlServerShell, _cancellationToken);
            StartClientsWorkers(clientsWithServers);

            while (true)
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    CancelAllClientsTasks();
                    break;
                }

                CheckDisabledClients();

                //TODO: Обновить инфу о текущих клиентов и вырубить те, что были убраны из БД

                Task.Delay(TimeSpan.FromSeconds(3)).Wait(_cancellationToken);
            }
        }

        /// <summary>
        ///     Отмена всех клиенстких потоков.
        /// </summary>
        private void CancelAllClientsTasks()
        {
            _logger.WriteLine("Поступила команда отмены работы сервиса. Начинаю освобождение клиентских потоков");
            // TODO: Продумать освобождение
            lock (_workingClientsListLock)
            {
                foreach (var clientWorker in _workingClients)
                {
                    clientWorker.Cts.Cancel();
                }

                _workingClients.ForEach(x =>
                {
                    var waitSecondsForDispose = TimeSpan.FromSeconds(1);
                    if (!x.Worker.Wait(waitSecondsForDispose))
                    {
                        _logger.Warning($"Поток с клиентом {x.Client} слишком долго отменяется" +
                            $", поэтому сервис не будет ждать его завершения. Количество секунд ожидания: {waitSecondsForDispose.TotalSeconds}");
                    }
                });
            }
            _logger.WriteLine("Клиенсткие потоки успешно завершены");
            IsAllTasksCompleted = true;
        }

        private void StartClientsWorkers(IEnumerable<(ImapClient Client, ImapServer Server)> clientsWithServers)
        {
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
        private void CheckDisabledClients()
        {
            lock (_diabledClientsListLock)
            {
                foreach (var disClient in _diabledClients)
                {
                    bool isNotNull = false;
                    lock (_workingClients)
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
            }
        }

        private void TryUpdateClientIsWorking(ImapClient client, bool? isWorkingNow)
        {
            try
            {
                SqlServerShellAdapter.UpdateClientIsWorking(_sqlServerShell, isWorkingNow, client.Uid);
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
                SqlServerShellAdapter.UpdateClientIsWorking(_sqlServerShell, true, clientWithServer.Client.Uid);

                var clientSqlShell = new SqlServerShellAdapter(_sqlServerShell, clientWithServer.Client, _logger, _cancellationToken);

                while (true)
                {
                    imapClient.FetchAndSaveLastMailsFromAllFolders(clientSqlShell, _logger, _cancellationToken);

                    Task.Delay(TimeSpan.FromSeconds(5)).Wait(_cancellationToken);
                    // TODO: Отладить и попробовать ловить событие о изменении кол-ва писем в папке

                    _cancellationToken.ThrowIfCancellationRequested();
                }
            }
            catch (TaskCanceledException cex) 
            {
                _logger.WriteLine($"Поток для клиента '{clientWithServer.Client}' был отменён. Message: '{cex.Message}'");
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка в потоке клиента '{clientWithServer.Client.Login}' (Uid={clientWithServer.Client.Uid}). {ex}");
                lock (_diabledClientsListLock)
                {
                    _diabledClients.Add(clientWithServer);
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
