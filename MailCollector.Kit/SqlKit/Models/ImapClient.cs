using System;

namespace MailCollector.Kit.SqlKit.Models
{
#pragma warning disable CS0659 // Тип переопределяет Object.Equals(object o), но не переопределяет Object.GetHashCode()

    public class ImapClient
    {
        internal const string TableName = "ImapClients";

        public Guid Uid { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Guid ImapServerUid { get; set; }
        public bool? IsWorking { get; set; }

        public override string ToString()
        {
            return $"{Login} (Uid={Uid})";
        }

        public override bool Equals(object obj)
        {
            var objT = obj as ImapClient;
            if (objT == null)
                return false;

            if (Uid == objT.Uid && Login == objT.Login 
                && Password == objT.Password 
                && ImapServerUid == objT.ImapServerUid)
                return true;
            else
                return false;
        }
    }
}
