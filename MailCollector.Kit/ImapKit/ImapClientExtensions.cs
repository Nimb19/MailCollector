using MailCollector.Kit.ImapKit.Models;
using MailCollector.Kit.SqlKit.Models;
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
        public static MailKit.Net.Imap.ImapClient Connect(this ImapClientParams imapClientParams,
            CancellationToken cancellationToken = default, int timeout = 8000)
        {
            var client = new MailKit.Net.Imap.ImapClient();
            client.Timeout = timeout;
            client.Connect(imapClientParams.ImapServerParams.Uri, imapClientParams.ImapServerParams.Port
                    , imapClientParams.ImapServerParams.UseSsl, cancellationToken);
            client.Authenticate(imapClientParams.Login, imapClientParams.Password, cancellationToken);
            return client;
        }

        public static Dictionary<IMailFolder, ImapMailParams[]> FetchLastMailsFromAllFolders(MailKit.Net.Imap.ImapClient client, CancellationToken cancellationToken)
        {
            var markedFolders = client.GetFolders(new FolderNamespace('0', ""), cancellationToken: cancellationToken)
                            .Where(x => x.Attributes.HasFlag(FolderAttributes.Marked)).ToArray();

            var result = new Dictionary<IMailFolder, ImapMailParams[]>();

            foreach (var folder in markedFolders)
            {
                folder.Open(FolderAccess.ReadOnly);
                var mails = folder.FetchLastMails(0, cancellationToken);
                result.Add(folder, mails);
            }

            return result;
        }

        public static ImapMailParams[] FetchLastMails(this IMailFolder mailFolder, int startIndex, CancellationToken cancellationToken)
        {
            var mails = new List<ImapMailParams>();

            var j = 0;
            var messageSummary = mailFolder.Fetch(startIndex, -1, MessageSummaryItems.InternalDate, cancellationToken).ToArray();

            for (int i = startIndex; i < mailFolder.Count; i++)
            {
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
                $"{(string.IsNullOrWhiteSpace(x.Name) ? string.Empty : $"{Mail.EmailNamesSeparator}{x.Name}")}")
            .ToArray();

            return string.Join(";", addreses);
        }
    }
}
