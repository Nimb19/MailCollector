using System;
using System.Collections.Generic;
using System.Text;

namespace MailCollector.Kit.Logger
{
    public interface ILogger
    {
        LogLevel LogLevel { get; set; }
        void Write(LogLevel logLevel, string msg);
        void Error(Exception exception);
    }

    public enum LogLevel
    {
        Error = 0,
        Warning = 1,
        Info = 2,
        Debug = 3,
        Trace = 4,
    }
}
