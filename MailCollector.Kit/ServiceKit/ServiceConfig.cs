using MailCollector.Kit.Logger;
using MailCollector.Kit.SqlKit.Models;

namespace MailCollector.Kit.ServiceKit
{
    public class ServiceConfig
    {
        public SqlServerSettings SqlServerSettings { get; set; }
        public string TelegramBotApiToken { get; set; }
        public bool WhetherEnableMailFilter { get; set; }
        public string[] MailsFilterStrings { get; set; }
        public LogLevel LogLevel { get; set; } = LogLevel.Info;
    }
}
