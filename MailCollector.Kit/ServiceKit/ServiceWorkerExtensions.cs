using MailCollector.Kit.ImapKit;
using MailCollector.Kit.Logger;
using MailCollector.Kit.SqlKit;
using MailKit;
using MailKit.Net.Imap;
using System.Linq;
using System.Threading;

namespace MailCollector.Kit.ServiceKit
{
    public static class ServiceWorkerExtensions
    {
        public static void FetchAndSaveLastMailsFromAllFolders(this ImapClient client
            , SqlServerShellAdapter shell, ILogger logger, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var markedFolders = client.GetFolders(new FolderNamespace('0', ""), cancellationToken: cancellationToken)
                            .Where(x => x.Attributes.HasFlag(FolderAttributes.Marked)).ToArray();

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
                    cancellationToken.ThrowIfCancellationRequested();
                    shell.SaveMail(mail);
                }

                logger.WriteLine($"С почты '{shell.SqlClient.Login}' были успешно сохранены сообщения" +
                    $" из папки '{sqlFolder.FullName}', в количестве: {mails.Length}");
            }
        }
    }
}
