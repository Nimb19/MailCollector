using MailCollector.Kit.ImapKit.Models;
using System;
using System.Linq;

namespace MailCollector.Kit.SqlKit.Models
{
    public class Mail
    {
        internal const string TableName = "Mails";

        /// <summary> 
        ///     Ставится между адресом и именем аккаунта. 
        ///     Получается; например: "noreply@mail.yandex.ru=Яндекс.Почта". 
        /// </summary>
        public const string EmailNamesSeparator = "=";

        /// <summary> 
        ///     Ставится между адресами. 
        ///     Получается; например: "noreply@mail.yandex.ru;somenewacc@mail.ru". 
        /// </summary>
        public const string EmailsSeparator = ";";

        public Guid Uid { get; set; }
        public Guid FolderUid { get; set; }
        public int IndexInFolder { get; set; }
        public DateTimeOffset Date { get; set; }
        public string MFrom { get; set; }
        public string MTo { get; set; }
        public string MCc { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }

        public Mail() { }

        public Mail(ImapMailParams imapMailParams, Guid folderUid)
        {
            Uid = Guid.NewGuid();
            IndexInFolder = imapMailParams.Index;
            Subject = imapMailParams.Subject;
            Date = imapMailParams.Date;
            MFrom = imapMailParams.From;
            MTo = imapMailParams.To;
            MCc = imapMailParams.Cc;
            HtmlBody = imapMailParams.HtmlBody;
            FolderUid = folderUid;
        }

        /// <summary>
        ///     В базе хранятся мало приятные глазу строки с адресами. 
        ///     Тут одна такая строку форматируется в красивый глазу текст.
        /// </summary>
        public static string AccsToString(string str, bool onlyFirstAcc = true, bool shortName= true)
        {
            if (onlyFirstAcc)
            {
                var firstAcc = new string(str.TakeWhile(x => x != ';').ToArray());
                if (shortName)
                    return firstAcc.Split(EmailNamesSeparator.First()).Last();
                else
                    return firstAcc.Replace(EmailNamesSeparator, " ");
            }
            else
            {
                if (shortName)
                    return string.Join("; ", str.Split(EmailsSeparator.First())
                        .Select(x => x.Split(EmailNamesSeparator.First()).Last()));
                else
                    return str.Replace(EmailNamesSeparator, " ").Replace(EmailsSeparator, $"{EmailsSeparator} ");
            }
        }
    }
}
