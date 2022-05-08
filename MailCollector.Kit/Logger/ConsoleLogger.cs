using System;
using System.Collections.Generic;
using System.Text;

namespace MailCollector.Kit.Logger
{
    public class ConsoleLogger : AbstractLogger
    {
        public static readonly ConsoleLogger Instance = new ConsoleLogger();

        public override LogLevel LogLevel { get; set; } = LogLevel.Trace;

        protected override void PrivateWrite(string fullMsg)
        {
            Console.WriteLine(fullMsg);
        }
    }
}
