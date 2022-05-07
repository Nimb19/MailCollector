using MailCollector.Kit.ImapKit;
using MailCollector.Kit.ImapKit.Models;
using MailKit;
using MailKit.Net.Imap;
using System;
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

                var login = "123";
                var pass = "123";

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
                        var inbox = client.GetFolder("Inbox");
                        inbox.Open(FolderAccess.ReadOnly);

                        Console.WriteLine("Total messages: {0}", inbox.Count);
                        Console.WriteLine("Recent messages: {0}", inbox.Recent);

                        for (int i = 0; i < inbox.Count; i++)
                        {
                            var message = inbox.GetMessage(i);
                            Console.WriteLine("Subject: {0}", message.Subject);
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
