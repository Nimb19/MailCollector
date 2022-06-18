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
                var lastIndex = shell.GetLastMailIndexFromFolder(sqlFolder.Uid);
                var messagesSummary = folder.Fetch(lastIndex + 1, -1, MessageSummaryItems.InternalDate, cancellationToken)
                    .Reverse().ToArray();

                if (messagesSummary.Length == 1 && lastIndex >= messagesSummary[0].Index)
                    continue;
                if (messagesSummary.Length == 0)
                    continue;

                var packetsCount = (int)Math.Ceiling((double)messagesSummary.Length / (double)MaxMailsPacketCount);
                var lastSummaryIndex = messagesSummary.Length - 1;
                var maxMailsCount__ = MaxMailsPacketCount - 1;

                var allMails = new List<ImapMailParams>();
                for (int i = 0; i < packetsCount; i++)
                {
                    var start = lastSummaryIndex - i * MaxMailsPacketCount;
                    var end = start >= maxMailsCount__
                        ? start - maxMailsCount__
                        : 0;

                    logger.Debug($"У клиента '{shell.SqlClient}' будет сохранена пачка писем с индексом: {i} " +
                        $"(пачка равна след. кол-ву писем: {(start - end) + 1}). " +
                        $"Не считая эту пачку, осталось сохранить: {end}");
                    var mails = folder.FetchLastMailsReverse(messagesSummary, start, end, cancellationToken, logger);

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
