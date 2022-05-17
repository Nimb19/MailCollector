using MailCollector.Kit.ImapKit;
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
        /// <summary>
        ///     Подгрузить для каждой папки только их новые письма, если они были.
        /// </summary>
        public static void FetchAndSaveLastMailsFromAllFolders(this ImapClient client
            , MailCollectorSqlAdapter shell, MailTelegramBot tgBot, ILogger logger, CancellationToken cancellationToken, bool isInited = true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var markedFolders = client.GetFolders(new FolderNamespace(' ', ""), cancellationToken: cancellationToken)
                            .Where(x => (x.Attributes.HasFlag(FolderAttributes.Marked)
                            || x.Attributes == FolderAttributes.None
                            || x.Attributes.HasFlag(FolderAttributes.Inbox)) 
                           && !x.Attributes.HasFlag(FolderAttributes.Sent)).ToArray();
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
                var mails = folder.FetchLastMails(lastIndex + 1, cancellationToken);
                if (mails.Length == 0)
                    continue;

                foreach (var mail in mails)
                {
                    shell.SaveMail(mail);
                }

                if (isInited)
                    tgBot?.SendMessageToAllSubsAboutNewMails(mails, shell.SqlClient);

                logger.WriteLine($"С почты '{shell.SqlClient.Login}' были успешно сохранены сообщения" +
                    $" из папки '{sqlFolder.FullName}', в количестве: {mails.Length}. Был ли клиент инициализирован: {isInited}");
            }

            if (!isInited)
                tgBot?.SendMessageToAllSubsAboutInitComplete(shell.SqlClient);
        }
    }
}
