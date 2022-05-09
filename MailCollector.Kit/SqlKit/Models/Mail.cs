﻿using MailCollector.Kit.ImapKit.Models;
using System;

namespace MailCollector.Kit.SqlKit.Models
{
    public class Mail
    {
        internal const string TableName = "Mails";

        /// <summary> 
        ///     Ставиться между адресом и именем аккаунта. 
        ///     Получается; например: "noreply@mail.yandex.ru=Яндекс.Почта". 
        /// </summary>
        public const string EmailNamesSeparator = "=";

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
    }
}