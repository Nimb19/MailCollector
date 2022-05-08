using MailCollector.Kit;
using MailCollector.Kit.Logger;
using MailCollector.Kit.ServiceKit;
using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace MailCollector.Service
{
    public partial class Service1 : ServiceBase
    {
        private ServiceWorker _serviceWorker;
        
        private const string ConfigFileName = "Configs\\Settings.json";

        private readonly ILogger _logger = new MultiLogger(FileLogger.Instance);
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                var config = CommonExtensions.DeserializeFile<Settings>(ConfigFileName);
                _serviceWorker = new ServiceWorker(config.SqlServerSettings, _logger, _cts.Token);
            }catch (Exception ex)
            {
                _logger.Error(ex);
                Stop();
            }
        }

        protected override void OnStop()
        {
            _cts.Cancel();

            var timeeStartCancel = DateTime.Now;
            const int seconsForWaitDispose = 10;

            do
            {
                Task.Delay(200).Wait();
            } while (!_serviceWorker.IsAllTasksCanceled || DateTime.Now - timeeStartCancel < TimeSpan.FromSeconds(seconsForWaitDispose));

            if (!_serviceWorker.IsAllTasksCanceled)
                _logger.Warning($"За {seconsForWaitDispose} секунд таски не успели освободиться");

            _serviceWorker.TryDispose(_logger);
            _cts.TryDispose(_logger);
        }
    }
}
