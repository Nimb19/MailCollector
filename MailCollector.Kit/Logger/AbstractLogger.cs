using System;

namespace MailCollector.Kit.Logger
{
    public abstract class AbstractLogger : ILogger
    {
        public bool NeedWriteFullDate { get; set; } = true;
        public virtual LogLevel LogLevel { get; set; } = LogLevel.Info;

        protected abstract void PrivateWrite(string fullMsg);

        public virtual void Write(LogLevel logLevel, string msg)
        {
            if (logLevel == LogLevel.None)
            {
                logLevel = LogLevel.Info;
            }

            if (LogLevel >= logLevel)
            {
                string logLevelToString = null;
                switch (logLevel)
                {
                    case LogLevel.Trace: logLevelToString = "TRC"; break;
                    case LogLevel.Info: logLevelToString = "INF"; break;
                    case LogLevel.Error: logLevelToString = "ERR"; break;
                    case LogLevel.Warning: logLevelToString = "WRN"; break;
                    case LogLevel.Debug: logLevelToString = "DBG"; break;
                    default: logLevelToString = "default"; break;
                }

                var datetimestr = NeedWriteFullDate 
                    ? DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") 
                    : DateTime.Now.ToString("HH:mm:ss:fff");
                var fullMasg = $"{datetimestr} [{logLevelToString}] {msg}";
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
