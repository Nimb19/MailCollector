using MailCollector.Kit.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailCollector.Setup
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ILogger logger = new MultiLogger(FileLogger.Instance);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var installerSettings = new InstallerSettings()
            {
                PathToClientBuild = Constants.PathToClientBuild,
                PathToServiceBuild = Constants.PathToServiceBuild,
                PathToCreateDbScript = Constants.PathToCreateDbScript,
            };
            Application.Run(new HelloForm(logger, installerSettings));
        }
    }
}
