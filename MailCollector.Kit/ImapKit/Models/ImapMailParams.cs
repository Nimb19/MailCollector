using MailKit;
using System;

namespace MailCollector.Kit.ImapKit.Models
{
    public class ImapMailParams
    {
        public IMailFolder Folder { get; set; }
        public int Index { get; set; }
        public DateTimeOffset Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
    }
}
