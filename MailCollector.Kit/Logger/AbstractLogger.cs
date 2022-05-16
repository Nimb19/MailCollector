using System;
using System.Collections.Generic;
using System.Text;

namespace MailCollector.Kit.Logger
{
    public abstract class AbstractLogger : ILogger
    {
        public virtual LogLevel LogLevel { get; set; } = LogLevel.Trace;

        protected abstract void PrivateWrite(string fullMsg);

        public virtual void Write(LogLevel logLevel, string msg)
        {
            if (LogLevel >= logLevel)
            {
                var logLevelToString = string.Empty;
                switch (logLevel)
                {
                    case LogLevel.Trace: logLevelToString = "TRC"; break;
                    case LogLevel.Info: logLevelToString = "INF"; break;
                    case LogLevel.Error: logLevelToString = "ERR"; break;
                    case LogLevel.Warning: logLevelToString = "WRN"; break;
                    case LogLevel.Debug: logLevelToString = "DBG"; break;
                }

                var fullMasg = $"{DateTime.Now} [{logLevelToString}] {msg}";
                PrivateWrite(fullMasg);
            }
        }

        public virtual void Error(Exception exception)
        {
            Write(LogLevel.Error, $"{exception.Message} ({exception.InnerException}).{Environment.NewLine}{exception.StackTrace}");
        }

        public override string ToString()
        {
            return $"Logger={this.GetType().Name}: LogLevel={LogLevel}";
        }
    }
}
