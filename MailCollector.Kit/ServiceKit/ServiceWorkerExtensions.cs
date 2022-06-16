using MailCollector.Kit.ImapKit;
using MailCollector.Kit.ImapKit.Models;
using MailCollector.Kit.Logger;
using MailCollector.Kit.SqlKit;
using MailCollector.Kit.TelegramBotKit;
using MailKit;
using MailKit.Net.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MailCollector.Kit.ServiceKit
{
    public static class ServiceWorkerExtensions
    {
        private const int MaxMailsPacketCount = 25;

        /// <summary>
        ///     Подгрузить для каждой папки только их новые письма, если они были.
        /// </summary>
        public static void FetchAndSaveLastMailsFromAllFolders(this ImapClient client
            , MailCollectorSqlAdapter shell, MailTelegramBot tgBot, ILogger logger
            , CancellationToken cancellationToken, bool isInited = true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var markedFolders = client.GetFolders(new FolderNamespace(' ', ""), cancellationToken: cancellationToken)
                            .Where(x => (x.Attributes.HasFlag(FolderAttributes.Marked)
                            || x.Attributes == FolderAttributes.None
                            || x.Attributes.HasFlag(FolderAttributes.Inbox)) 
                           && !x.Attributes.HasFlag(FolderAttributes.Sent) 
                           && !x.Attributes.HasFlag(FolderAttributes.Drafts)).ToArray();
            if (markedFolders.Length == 0)
                throw new Exception($"С почты '{shell.SqlClient.Login}' не поступает ни одной папки");

            foreach (var folder in markedFolders)
            {
                cancellationToken.ThrowIfCancellationRequested();

                folder.Open(FolderAccess.ReadOnly);

                var sqlFolder = shell.Folders.FirstOrDefault(x => x.FullName == folder.FullName);
                if (sqlFolder == null)
                {
                    sqlFolder = shell.CreateFolder(folder);
                }
                var startIndex = shell.GetLastMailIndexFromFolder(sqlFolder.Uid) + 1;

                var messagesSummary = folder.Fetch(startIndex, -1, MessageSummaryItems.InternalDate, cancellationToken).ToArray();

                if (messagesSummary.Length == 0)
                    continue;

                var packetsCount = (int)Math.Ceiling((double)messagesSummary.Length / (double)MaxMailsPacketCount);

                var allMails = new List<ImapMailParams>();
                for (int i = 0; i < packetsCount; i++)
                {
                    var start = i * MaxMailsPacketCount + startIndex;
                    var ostatok = messagesSummary.Length - (packetsCount * MaxMailsPacketCount);
                    var end = ostatok >= MaxMailsPacketCount
                        ? start + MaxMailsPacketCount - 1
                        : start + ostatok - 1;

                    logger.Debug($"У клиента '{shell.SqlClient}' будет сохранена пачка писем с индексом: {i} (пачка равна {MaxMailsPacketCount} письмам). " +
                        $"Всего нужно сохранить ещё {ostatok}");
                    var mails = folder.FetchLastMails(messagesSummary, start, end, cancellationToken, logger);

                    foreach (var mail in mails)
                    {
                        shell.SaveMail(mail);
                    }
                    allMails.AddRange(mails);
                }

                if (isInited)
                    tgBot?.SendMessageToAllSubsAboutNewMails(allMails, shell.SqlClient);

                logger.WriteLine($"С почты '{shell.SqlClient.Login}' были успешно сохранены сообщения" +
                    $" из папки '{sqlFolder.FullName}', в количестве: {allMails.Count}. " +
                    $"{(isInited ? string.Empty : "Клиент был первый раз инициализирован")}");
            }

            if (!isInited)
                tgBot?.SendMessageToAllSubsAboutInitComplete(shell.SqlClient);
        }
    }
}
