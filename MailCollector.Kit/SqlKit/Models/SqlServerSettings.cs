namespace MailCollector.Kit.SqlKit.Models
{
    public class SqlServerSettings
    {
        public string ServerName { get; set; }
        public bool? IntegratedSecurity { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int? CommandTimeoutInSeconds { get; set; }
    }
}
