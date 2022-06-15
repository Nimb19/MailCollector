using System;
using System.Collections.Generic;
using System.Linq;

namespace MailCollector.Kit.Logger
{
    public sealed class MultiLogger : ILogger
    {
        public List<ILogger> Loggers { get; set; }
        public LogLevel LogLevel 
        {
            get
            {
                var fl = Loggers.FirstOrDefault();
                if (fl == null)
                    return LogLevel.Info;

                return fl.LogLevel;
            }
            set
            {
                foreach (var logger in Loggers)
                {
                    if (logger.LogLevel != value)
                        logger.LogLevel = value;
                }
            } 
        }

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
