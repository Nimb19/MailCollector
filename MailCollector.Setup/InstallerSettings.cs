using MailCollector.Kit.SqlKit.Models;

namespace MailCollector.Setup
{
    public class InstallerSettings
    {
        public SetupSteps InstallSteps { get; set; }
        public SqlServerSettings SqlServerSettings { get; set; }
        public string TelegramBotToken { get; set; }
        public string InstallServicePath { get; set; }
        public string InstallClientPath { get; set; }
    }
}
