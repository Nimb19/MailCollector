using MailCollector.Kit.Logger;
using MailCollector.Kit.ServiceKit;

namespace MailCollector.Client
{
    public static class Constants
    {
        public static readonly ILogger Logger = new MultiLogger(FileLogger.Instance);
        public static ClientConfig Config = null; // Инициализируется в Program.cs
        public const string PathToConfig = "Config.json";
        public const string ModuleInfo = "MailCollector.Client";

        /// <summary>
        ///     Таймаут подключения к IMAP-серверу. В миллисекундах
        /// </summary>
        public const int ImapConnectTimeout = 7000;
    }
}
