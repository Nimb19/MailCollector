using System;
using System.IO;
using System.Reflection;

namespace MailCollector.Kit.Logger
{
    public sealed class FileLogger : AbstractLogger
    {
        public static readonly FileLogger Instance = new FileLogger();
        public string ModuleName = null;

        private readonly object _fileLock = new object();

        /// <summary> По умолчанию сбрасывает в папку с приложением </summary>
        public string PathToLogs { get; private set; }

        protected override void PrivateWrite(string fullMsg)
        {
            lock (_fileLock)
            {
                using (var sw = new StreamWriter(PathToLogs, append: true, CommonExtensions.Encoding))
                    sw.WriteLine(fullMsg);
            }
        }

        public FileLogger() 
        {
            Constructor();
        }

        public FileLogger(string moduleName)
        {
            ModuleName = string.IsNullOrWhiteSpace(moduleName) ? null : moduleName.Trim();
            Constructor();
        }

        private void Constructor()
        {
            NeedWriteFullDate = false;
            SetPathToLogs();
        }

        private void SetPathToLogs()
        {
            var fileName =  $"{ModuleName ?? "AppLog"}_{DateTime.Now.ToString("dd.MM.yyyy")}.log";
            var location = Assembly.GetEntryAssembly()?.Location;
            if (location != null) {
                var pathToLogs = new FileInfo(location)?.DirectoryName;
                PathToLogs = Path.Combine(pathToLogs, fileName);
            }
            else
            {
                PathToLogs = fileName;
            }
        }
    }
}
