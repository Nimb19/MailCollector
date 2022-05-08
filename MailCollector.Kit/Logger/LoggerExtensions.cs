using System;
using System.Collections.Generic;
using System.Text;

namespace MailCollector.Kit.Logger
{
    public static class LoggerExtensions
    {
        public static ILogger Error(this ILogger logger, string msg)
        {
            logger.Write(LogLevel.Error, msg);
            return logger;
        }

        public static ILogger Warning(this ILogger logger, string msg)
        {
            logger.Write(LogLevel.Warning, msg);
            return logger;
        }

        public static ILogger WriteLine(this ILogger logger, string msg)
        {
            logger.Write(LogLevel.Info, msg);
            return logger;
        }

        public static ILogger Debug(this ILogger logger, string msg)
        {
            logger.Write(LogLevel.Debug, msg);
            return logger;
        }

        public static ILogger Trace(this ILogger logger, string msg)
        {
            logger.Write(LogLevel.Trace, msg);
            return logger;
        }
    }
}
