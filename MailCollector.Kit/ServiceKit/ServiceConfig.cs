using MailCollector.Kit.SqlKit.Models;

namespace MailCollector.Kit.ServiceKit
{
    public class ServiceConfig
    {
        public SqlServerSettings SqlServerSettings { get; set; }
        public string TelegramBotApiToken { get; set; }
    }
}
