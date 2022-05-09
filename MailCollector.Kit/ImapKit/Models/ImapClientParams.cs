using MailCollector.Kit.SqlKit.Models;

namespace MailCollector.Kit.ImapKit.Models
{
    public class ImapClientParams
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public ImapServerParams ImapServerParams { get; set; }

        public ImapClientParams(string login, string password, ImapServerParams imapServerParams)
        {
            Login = login;
            Password = password;
            ImapServerParams = imapServerParams;
        }

        public ImapClientParams(ImapClient client, ImapServer server)
        {
            Login = client.Login;
            Password = client.Password;
            ImapServerParams = new ImapServerParams(server);
        }
    }
}
