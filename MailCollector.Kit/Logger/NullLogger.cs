using System;

namespace MailCollector.Kit.Logger
{
    public class NullLogger : ILogger
    {
        public static readonly NullLogger Instance = new NullLogger();

        public LogLevel LogLevel { get; set; }

        public void Error(Exception exception)
        {
            return;
        }

        public void Write(LogLevel logLevel, string msg)
        {
            return;
        }
    }
}
