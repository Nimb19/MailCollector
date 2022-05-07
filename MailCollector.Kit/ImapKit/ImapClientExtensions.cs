using MailCollector.Kit.ImapKit.Models;
using MailKit.Net.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MailCollector.Kit.ImapKit
{
    public static class ImapClientExtensions
    {
        public static ImapClient Connect(this ImapClientParams imapClientParams,
            CancellationToken cancellationToken = default, int timeout = 8000)
        {
            var client = new ImapClient();
            client.Timeout = timeout;
            client.Connect(imapClientParams.ImapServerParams.Uri, imapClientParams.ImapServerParams.Port
                    , imapClientParams.ImapServerParams.UseSsl, cancellationToken);
            client.Authenticate(imapClientParams.Login, imapClientParams.Password, cancellationToken);
            return client;
        }
    }
}
