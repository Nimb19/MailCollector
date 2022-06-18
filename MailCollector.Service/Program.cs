using MailCollector.Kit;
using MailCollector.Kit.Logger;
using MailCollector.Kit.ServiceKit;
using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace MailCollector.Service
{
    public class Program
    {
        private static ServiceWorker _serviceWorker;

        private static readonly MultiLogger _logger = new MultiLogger(new FileLogger(Constants.ModuleName));
        private static readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public static void Main(string[] args)
        {
            try
            {
                _logger.WriteLine("");
                _logger.WriteLine("");
                _logger.WriteLine("Start program");
                if (ServiceLauncher.IsService())
                {
                    using (var service = new Service())
                        ServiceBase.Run(service);
                }
                else
                {
                    StartAsConsoleApp();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        private static void StartAsConsoleApp()
        {
            const string cmdToStop = "stop";
            Console.WriteLine($"Впишите '{cmdToStop}', что бы отключить сервис");
            _logger.Loggers.Add(ConsoleLogger.Instance);
            StartWorker();

            while (true)
            {
                var text = Console.ReadLine();

                if (text == cmdToStop)
                {
                    StopWorker();
                    break;
                }
            }

            _cts.Dispose();

            Console.WriteLine("Работа завершена");
            Console.ReadLine();
        }

        private static bool StartWorker()
        {
            try
            {
                var configPath = Path.Combine(new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName, Constants.ConfigFileName);
                var config = CommonExtensions.DeserializeFile<ServiceConfig>(configPath);
                _logger.LogLevel = config.LogLevel == LogLevel.None ? LogLevel.Info : config.LogLevel;

                _serviceWorker = new ServiceWorker(config, _logger, Constants.ModuleName, _cts.Token);

                _serviceWorker.Start();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return false;
            }
        }

        private static void StopWorker()
        {
            if (!_cts.IsCancellationRequested)
                _cts.Cancel();

            var timeeStartCancel = DateTime.Now;
            const int seconsForWaitDispose = 10;

            _cts.TryDispose(_logger);
            if (_serviceWorker == null)
                return;

            do
            {
                Task.Delay(200).Wait();
            } while (!_serviceWorker.IsAllTasksCompleted
            || DateTime.Now - timeeStartCancel < TimeSpan.FromSeconds(seconsForWaitDispose));

            if (!_serviceWorker.IsAllTasksCompleted)
                _logger.Warning($"За {seconsForWaitDispose} секунд потоки не успели освободиться");

            _serviceWorker.TryDispose(_logger);
        }

        public class Service : ServiceBase
        {
            public Service()
            {
                ServiceName = ServiceInfo.ServiceName;
            }

            protected override void OnStart(string[] args)
            {
                var isWork = StartWorker();
                if (!isWork)
                    Stop();
            }

            protected override void OnStop()
            {
                StopWorker();
            }
        }

        //private static void StartAsService()
        //{
        //    ServiceLauncher.SetServiceStatus(ServiceState.StartPending);
        //    ServiceLauncher.SetServiceStatus(ServiceState.Running);
        //    StartWorker();
        //    ServiceLauncher.ReadMessages();
        //    ServiceLauncher.SetServiceStatus(ServiceState.StartPending);
        //    StopWorker();
        //    ServiceLauncher.SetServiceStatus(ServiceState.Stopped);
        //}
    }

    [RunInstaller(true)]
    public class MyServiceInstaller : Installer
    {
        public MyServiceInstaller()
        {
            var spi = new ServiceProcessInstaller();
            var si = new System.ServiceProcess.ServiceInstaller();
        
            spi.Account = ServiceAccount.LocalSystem;
            spi.Username = null;
            spi.Password = null;
        
            si.DisplayName = ServiceInfo.ServiceName;
            si.ServiceName = ServiceInfo.ServiceName;
            si.StartType = ServiceStartMode.Automatic;
        
            Installers.Add(spi);
            Installers.Add(si);
        }
    }
}
