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

        public MailCollectorInstaller(InstallerSettings installerSettings, ILogger logger)
        {
            _logger = logger;
            _installerSettings = installerSettings;
        }

        public void StartInstall(CancellationToken cancellationToken) 
        {
            cancellationToken.ThrowIfCancellationRequested();


        }
    }
}
