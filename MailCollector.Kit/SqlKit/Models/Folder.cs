using System;

namespace MailCollector.Kit.SqlKit.Models
{
    public class Folder
    {
        internal const string TableName = "Folders";

        public Guid Uid { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public Guid ImapClientUid { get; set; }
    }
}
