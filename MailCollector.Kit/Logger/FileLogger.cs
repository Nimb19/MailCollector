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

        /// <summary> По умолчанию сбрасывает в папку с приложением. </summary>
        public string PathToLogs { get; set; } = $"AILogs_{DateTime.Now.ToString("dd.MM.yyyy")}.txt";
        public override LogLevel LogLevel { get; set; } = LogLevel.Debug;

        protected override void PrivateWrite(string fullMsg)
        {
            using (var sw = new StreamWriter(PathToLogs, append: true, CommonExtensions.Encoding))
                sw.WriteLine(fullMsg);
        }
    }
}
