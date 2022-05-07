using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
