using MailCollector.Kit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MailCollector.Kit.Logger
{
    public sealed class FileLogger : AbstractLogger
    {
        public static readonly FileLogger Instance = new FileLogger();

        public string ModuleName = null;

        /// <summary> По умолчанию сбрасывает в папку с приложением. </summary>
        public string PathToLogs { get; private set; }
        public override LogLevel LogLevel { get; set; } = LogLevel.Debug;

        protected override void PrivateWrite(string fullMsg)
        {
            using (var sw = new StreamWriter(PathToLogs, append: true, CommonExtensions.Encoding))
                sw.WriteLine(fullMsg);
        }

        public FileLogger() 
        {
            SetPathToLogs();
        }

        public FileLogger(string moduleName)
        {
            ModuleName = string.IsNullOrWhiteSpace(moduleName) ? null : moduleName.Trim();
            SetPathToLogs();
        }

        private void SetPathToLogs()
        {
            PathToLogs = $"{ModuleName ?? "AppLog"}_{DateTime.Now.ToString("dd.MM.yyyy")}.log";
        }
    }
}
