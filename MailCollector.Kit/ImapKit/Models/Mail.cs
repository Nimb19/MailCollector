using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailCollector.Kit.ImapKit.Models
{
    public class Mail
    {
        /// <summary> Ставиться между адресом и именем аккаунта. </summary>
        public const string EnvelopeSeparator = "=";

        public int Index { get; set; }
        public string FolderName { get; set; }
        public DateTimeOffset Date { get; set; }
        public string[] From { get; set; }
        public string[] To { get; set; }
        public string[] Cc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
