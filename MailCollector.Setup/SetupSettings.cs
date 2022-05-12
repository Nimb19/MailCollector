using MailCollector.Kit.SqlKit.Models;

namespace MailCollector.Setup
{
    public class SetupSettings
    {
        public SetupSteps InstallSteps { get; set; }
        public SqlServerSettings SqlServerSettings { get; set; }
        public string TelegramBotToken { get; set; }
    }
}
