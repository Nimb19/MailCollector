using MailCollector.Kit.ImapKit;
using MailCollector.Kit.ImapKit.Models;
using MailKit;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MailCollector.Debug
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var cts = new CancellationTokenSource();

                var login = "TroyHelper@yandex.ru";
                var pass = "ornqvoiqyinetlxw"; // "QRnVGrTqbxM0j9Q2WshN"; // TODO: uberi!!!!!!!!!!!!!!!!

                var imapClient = new ImapClientParams(login, pass, SupportedImapServers.YandexParams);

                var task = ReceiveMails(imapClient, cts.Token);

                while (true)
                {
                    var text = Console.ReadLine();

                    if (task.IsCompleted)
                    {
                        cts.Cancel();
                        break;
                    }

                    if (text == "stop")
                    {
                        cts.Cancel();
                        task.Dispose();
                        break;
                    }
                }

                cts.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Работа завершена");
            Console.ReadLine();
        }

        private static async Task ReceiveMails(ImapClientParams imapKitClient, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var client = imapKitClient.Connect(cancellationToken))
                    {
                        var allFolders = client.GetFolders(new FolderNamespace('0', ""), cancellationToken: cancellationToken)
                            .Where(x => x.Attributes.HasFlag(FolderAttributes.Marked)).ToArray();

                        foreach (var folder in allFolders)
                        {
                            folder.Open(FolderAccess.ReadOnly);

                            var mails = folder.FetchLastMails(0, cancellationToken);
                        }

                        client.Disconnect(true);
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc);
                }
            });
        }
    }
}
