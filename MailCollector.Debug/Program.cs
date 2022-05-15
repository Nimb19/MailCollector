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
                var pass = "ornqvoiqyinetlxw"; // "QRnVGrTqbxM0j9Q2WshN"
                // 5398105719:AAGGG10I3-PWkeBf-6BK3TDGC9ghvqnZ44s

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
