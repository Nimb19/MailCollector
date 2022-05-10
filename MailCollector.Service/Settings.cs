using MailCollector.Kit.SqlKit.Models;

namespace MailCollector.Service
{
    public class Settings
    {
        public SqlServerSettings SqlServerSettings { get; set; }
        public string TelegramBotApiToken { get; set; }
    }
}
