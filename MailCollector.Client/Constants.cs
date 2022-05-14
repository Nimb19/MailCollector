using MailCollector.Kit.Logger;

namespace MailCollector.Client
{
    public static class Constants
    {
        public static readonly ILogger Logger = new MultiLogger(FileLogger.Instance);
    }
}
