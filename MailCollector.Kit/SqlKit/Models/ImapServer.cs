using System;

namespace MailCollector.Kit.SqlKit.Models
{
    public class ImapServer
    {
        internal const string TableName = "ImapServers";

        public Guid Uid { get; set; }
        public string Uri { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
    }
}
