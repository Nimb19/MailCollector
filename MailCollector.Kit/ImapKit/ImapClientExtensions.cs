using MailCollector.Kit.ImapKit.Models;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MailCollector.Kit.ImapKit
{
    public static class ImapClientExtensions
    {
        public static ImapClient Connect(this ImapClientParams imapClientParams,
            CancellationToken cancellationToken = default, int timeout = 8000)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var client = new ImapClient();
            client.Timeout = timeout;
            client.Connect(imapClientParams.ImapServerParams.Uri, imapClientParams.ImapServerParams.Port
                    , imapClientParams.ImapServerParams.UseSsl, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            client.Authenticate(imapClientParams.Login, imapClientParams.Password, cancellationToken);
            return client;
        }

        public static async Task<ImapClient> ConnectAsync(this ImapClientParams imapClientParams,
            CancellationToken cancellationToken = default, int timeout = 8000)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var client = new ImapClient();
            client.Timeout = timeout;
            await client.ConnectAsync(imapClientParams.ImapServerParams.Uri, imapClientParams.ImapServerParams.Port
                    , imapClientParams.ImapServerParams.UseSsl, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            await client.AuthenticateAsync(imapClientParams.Login, imapClientParams.Password, cancellationToken);
            return client;
        }

        public static ImapClient Connect(SqlKit.Models.ImapClient imapClient, SqlKit.Models.ImapServer imapServer,
            CancellationToken cancellationToken = default, int timeout = 8000)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var client = new ImapClient();
            client.Timeout = timeout;
            client.Connect(imapServer.Uri, imapServer.Port
                    , imapServer.UseSsl, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            client.Authenticate(imapClient.Login, imapClient.Password, cancellationToken);
            return client;
        }

        public static ImapMailParams[] FetchLastMails(this IMailFolder mailFolder, int startIndex, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var mails = new List<ImapMailParams>();

            var j = 0;
            var messageSummary = mailFolder.Fetch(startIndex, -1, MessageSummaryItems.InternalDate, cancellationToken).ToArray();

            for (int i = startIndex; i < mailFolder.Count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var message = mailFolder.GetMessage(i, cancellationToken); 
                var mail = new ImapMailParams() 
                {
                    Index = messageSummary[j].Index,
                    Date = message.Date,
                    Subject = message.Subject,
                    Folder = mailFolder,
                    HtmlBody = message.HtmlBody?.ToString(),
                    From = message.From.MailAddressToString(),
                    To = message.To.MailAddressToString(),
                    Cc = message.Cc.MailAddressToString(),
                };
                mails.Add(mail);

                j++;
            }

            return mails.ToArray();
        }

        public static string MailAddressToString(this InternetAddressList internetAddressList)
        {
            var addreses = internetAddressList.Mailboxes.Select(x => $"{x.Address}" +
                $"{(string.IsNullOrWhiteSpace(x.Name) ? string.Empty : $"{SqlKit.Models.Mail.EmailNamesSeparator}{x.Name}")}")
            .ToArray();

            return string.Join(";", addreses);
        }
    }
}
