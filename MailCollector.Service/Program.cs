using System.ServiceProcess;

namespace MailCollector.Service
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MailCollectorService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
