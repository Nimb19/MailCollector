using System;

namespace MailCollector.Kit.SqlKit.Models
{
    public class ImapClient
    {
        internal const string TableName = "ImapClients";

        public Guid Uid { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Guid ImapServerUid { get; set; }
        public bool? IsWorking { get; set; }

        public override string ToString()
        {
            return $"{Login} (Uid={Uid})";
        }
    }
}
