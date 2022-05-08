using System;
using System.Collections.Generic;
using System.Text;

namespace MailCollector.Kit.Logger
{
    public sealed class MultiLogger : ILogger
    {
        public List<ILogger> Loggers { get; set; }
        public LogLevel LogLevel { get; set; } = LogLevel.Debug;

        public MultiLogger(params ILogger[] loggers)
        {
            Loggers = new List<ILogger>(loggers);
        }

        public void Error(Exception exception)
        {
            foreach (var logger in Loggers)
            {
                logger.Error(exception);
            }
        }

        public void Write(LogLevel logLevel, string msg)
        {
            foreach (var logger in Loggers)
            {
                logger.Write(logLevel, msg);
            }
        }
    }
}
