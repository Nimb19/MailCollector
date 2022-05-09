using MailCollector.Kit.SqlKit.Models;

namespace MailCollector.Kit.ImapKit.Models
{
    public class ImapServerParams
    {
        public string Uri { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }

        public ImapServerParams(string uri, int port, bool useSsl)
        {
            Uri = uri;
            Port = port;
            UseSsl = useSsl;
        }

        public ImapServerParams(ImapServer server)
        {
            Uri = server.Uri;
            Port = server.Port;
            UseSsl = server.UseSsl;
        }
    }
}
