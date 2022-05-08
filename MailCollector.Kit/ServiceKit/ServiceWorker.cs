using MailCollector.Kit.Logger;
using MailCollector.Kit.SqlKit;
using MailCollector.Kit.SqlKit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MailCollector.Kit.ServiceKit
{
    public class ServiceWorker : IDisposable
    {
        private readonly ILogger _logger;
        private readonly SqlServerShell _sqlServerShell;
        private readonly CancellationToken _cancellationToken;

        public bool IsAllTasksCanceled { get; private set; } // TODO: 

        public ServiceWorker(SqlServerSettings sqlServerSettings, ILogger logger, CancellationToken cancellationToken)
        {
            _logger = logger;
            _sqlServerShell = new SqlServerShell(sqlServerSettings);
            _cancellationToken = cancellationToken;

            _logger.WriteLine("ServiceWorker успешно инициализирован");
        }

        public void Dispose()
        {
            _sqlServerShell?.Dispose();
        }
    }
}
