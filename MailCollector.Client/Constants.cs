using MailCollector.Kit.Logger;
using MailCollector.Kit.ServiceKit;

namespace MailCollector.Client
{
    public static class Constants
    {
        public static readonly ILogger Logger = new MultiLogger(FileLogger.Instance);
        public static ClientConfig Config = null; // Перед загрузкой 1 формы инициализируется
        public const string PathToConfig = "Config.json";
        public const int DefaultUpdatemailsTime = 10000;
        public const string ModuleInfo = "MailCollector.Client";
    }
}
