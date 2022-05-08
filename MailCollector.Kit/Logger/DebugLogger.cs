using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MailCollector.Kit.Logger
{
    public sealed class DebugLogger : AbstractLogger
    {
        public static readonly DebugLogger Instance = new DebugLogger();

        public override LogLevel LogLevel { get; set; } = LogLevel.Trace;

        protected override void PrivateWrite(string fullMsg)
        {
            Debug.WriteLine(fullMsg);
        }
    }
}
