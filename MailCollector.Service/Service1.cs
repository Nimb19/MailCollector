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

        private readonly ILogger _logger = new MultiLogger(new FileLogger(Constants.ModuleName));
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                var config = CommonExtensions.DeserializeFile<Settings>(Constants.ConfigFileName);
                _serviceWorker = new ServiceWorker(config.SqlServerSettings, config.TelegramBotApiToken
                    , _logger, Constants.ModuleName, _cts.Token);
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
            } while (!_serviceWorker.IsAllTasksCompleted || DateTime.Now - timeeStartCancel < TimeSpan.FromSeconds(seconsForWaitDispose));

            if (!_serviceWorker.IsAllTasksCompleted)
                _logger.Warning($"За {seconsForWaitDispose} секунд таски не успели освободиться");

            _serviceWorker.TryDispose(_logger);
            _cts.TryDispose(_logger);
        }
    }
}
