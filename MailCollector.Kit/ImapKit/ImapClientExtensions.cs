using MailCollector.Kit.ImapKit.Models;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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

        public static Mail[] FetchLastMails(this IMailFolder mailFolder, int startIndex, CancellationToken cancellationToken)
        {
            var mails = new List<Mail>();

            var j = 0;
            var messageSummary = mailFolder.Fetch(startIndex, -1, MessageSummaryItems.InternalDate, cancellationToken).ToArray();

            for (int i = startIndex; i < mailFolder.Count; i++)
            {
                var message = mailFolder.GetMessage(i, cancellationToken);
                var mail = new Mail() 
                {
                    Index = messageSummary[j].Index,
                    Date = message.Date,
                    Subject = message.Subject,
                    FolderName = mailFolder.FullName,
                    Body = message.HtmlBody?.ToString(),
                    From = message.From.MailAddressToString(),
                    To = message.To.MailAddressToString(),
                    Cc = message.Cc.MailAddressToString(),
                };
                mails.Add(mail);

                j++;
            }

            return mails.ToArray();
        }

        public static string[] MailAddressToString(this InternetAddressList internetAddressList)
        {
            return internetAddressList.Mailboxes.Select(x => $"{x.Address}" +
                $"{(string.IsNullOrWhiteSpace(x.Name) ? string.Empty : $"{Mail.EnvelopeSeparator}{x.Name}")}")
            .ToArray();
        }
    }
}
