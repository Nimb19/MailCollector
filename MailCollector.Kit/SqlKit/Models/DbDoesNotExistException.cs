using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailCollector.Kit.SqlKit.Models
{
    public class DbDoesNotExistException : Exception
    {
        public DbDoesNotExistException(string msg) : base(msg)
        {

        }
    }
}
