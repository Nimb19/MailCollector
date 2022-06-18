using MailCollector.Kit.ImapKit.Models;
using MailCollector.Kit.Logger;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using System;
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

        public static ImapClient Connect(this SqlKit.Models.ImapClient imapClient, SqlKit.Models.ImapServer imapServer,
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

        public static bool TryConnect(this SqlKit.Models.ImapClient clientParams, SqlKit.Models.ImapServer serverParams
            , CancellationToken cancellationToken, int connTimeoutInMs
            , out ImapClient imapClient, out Exception lastException)
        {
            var trys = 4;
            var isConnected = false;
            lastException = null;
            imapClient = null;
            for (int i = 0; i < trys; i++)
            {
                try
                {
                    imapClient = clientParams.Connect(serverParams, cancellationToken, connTimeoutInMs);

                    isConnected = true;
                    break;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                }
            }

            return isConnected;
        }

        public static ImapMailParams[] FetchLastMailsReverse(this IMailFolder mailFolder, IMessageSummary[] messagesSummary
            , int startIndex, int endIndex, CancellationToken cancellationToken, ILogger logger)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var mails = new List<ImapMailParams>();

            var isTrace = logger != null && logger.LogLevel >= LogLevel.Trace;
            for (int i = startIndex; i >= endIndex; i--)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var message = mailFolder.GetMessage(i, cancellationToken);
                var to = message.To.MailAddressToString();
                var mail = new ImapMailParams() 
                {
                    Index = messagesSummary[i].Index,
                    Date = message.Date,
                    Subject = message.Subject,
                    Folder = mailFolder,
                    HtmlBody = message.HtmlBody?.ToString(),
                    From = message.From.MailAddressToString(),
                    To = to,
                    Cc = message.Cc.MailAddressToString(),
                };
                mails.Add(mail);

                if (isTrace)
                    logger.Trace($"Сохранено в временный лист письмо с индексом {i} на почту(ы): '{to}'. Его тема: '{mail.Subject}'");
            }

            return mails.ToArray();
        }

        public static string MailAddressToString(this InternetAddressList internetAddressList)
        {
            var addreses = internetAddressList.Mailboxes.Select(x => $"{x.Address}" +
                $"{(string.IsNullOrWhiteSpace(x.Name) ? string.Empty : $"{SqlKit.Models.Mail.EmailNamesSeparator}{x.Name}")}")
            .ToArray();

            return string.Join(SqlKit.Models.Mail.EmailsSeparator, addreses);
        }
    }
}
