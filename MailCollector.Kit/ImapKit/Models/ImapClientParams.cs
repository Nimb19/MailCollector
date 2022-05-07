using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
