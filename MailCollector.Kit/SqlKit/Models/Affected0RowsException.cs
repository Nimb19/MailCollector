using System;

namespace MailCollector.Kit.SqlKit.Models
{
    public class Affected0RowsException : Exception
    {
        public Affected0RowsException() : base()
        {
        }

        public Affected0RowsException(string msg) : base(msg)
        {
        }
    }
}
