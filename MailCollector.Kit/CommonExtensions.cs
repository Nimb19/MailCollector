using MailCollector.Kit.Logger;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Text;

namespace MailCollector.Kit
{
    public static class CommonExtensions
    {
        public static readonly Encoding Encoding = Encoding.UTF8;

        public static void TryDispose<T>(this T obj, ILogger logger) where T : IDisposable
        {
            try
            {
                obj?.Dispose();
            }
            catch (Exception ex)
            {
                logger.Error($"Ошибка во время освобождения объекта класса '{obj.GetType().FullName}': {ex}");
            }
        }

        public static string FormatToDate(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        public static int ValidateInt(string text, string fieldName)
        {
            return int.TryParse(ValidateString(text, fieldName), out var result)
                ? result
                : throw new ArgumentNullException(fieldName);
        }

        public static string ValidateString(string tbtext, string fieldName)
        {
            return string.IsNullOrWhiteSpace(tbtext)
                ? throw new ArgumentNullException(fieldName)
                : tbtext.Trim();
        }

        public static string ReadFile(string pathToFile)
        {
            using (var sr = new StreamReader(pathToFile, Encoding))
                return sr.ReadToEnd();
        }

        public static void WriteToFile(string text, string pathToFile, bool append = false)
        {
            using (var sw = new StreamWriter(pathToFile, append, Encoding))
                sw.Write(text);
        }

        public static T DeserializeFile<T>(string pathToFile)
        {
            var fileText = ReadFile(pathToFile);
            return JsonConvert.DeserializeObject<T>(fileText);
        }

        public static void SerializeToFile<T>(T obj, string pathToFile)
        {
            var text = JsonConvert.SerializeObject(obj, Formatting.Indented);
            WriteToFile(text, pathToFile, append: false);
        }

        public static void CopyDir(string fromDir, string toDir)
        {
            Directory.CreateDirectory(toDir);
            foreach (string s1 in Directory.GetFiles(fromDir))
            {
                string s2 = toDir + "\\" + Path.GetFileName(s1);
                File.Copy(s1, s2);
            }
            foreach (string s in Directory.GetDirectories(fromDir))
            {
                CopyDir(s, toDir + "\\" + Path.GetFileName(s));
            }
        }

        private const string OutputPrefix = "[ConsoleOutput]";

        /// <summary> 
        ///     Стартовать процесс 
        /// </summary>
        /// <returns> ExitCode </returns>
        public static int StartProcess(ILogger logger, string args, string workingDirectory
            , string fileName = "cmd.exe ", bool logOnlyErrors = false, Encoding encoding = null, bool useAdminRights = false)
        {
            logger = logger == null ? (ILogger)NullLogger.Instance : (ILogger)new PrefixLogger(logger, $"[ConsoleStarter] ");

            var internalEncoding = encoding ?? Encoding.UTF8;
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = fileName,
                Arguments = args,
                UseShellExecute = false, // The Process object must have the UseShellExecute property set to false in order to redirect IO streams
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = internalEncoding,
                StandardErrorEncoding = internalEncoding,
            };
            if (!string.IsNullOrWhiteSpace(workingDirectory))
                processStartInfo.WorkingDirectory = workingDirectory;
            if (useAdminRights)
                processStartInfo.Verb = "runas";

            var process = new Process() { StartInfo = processStartInfo };

            if (!logOnlyErrors)
                process.OutputDataReceived += (s, e) => { if (!string.IsNullOrWhiteSpace(e.Data)) Console.WriteLine($"{OutputPrefix} {e.Data}"); };
            process.ErrorDataReceived += (s, e) => { if (!string.IsNullOrWhiteSpace(e.Data)) Console.WriteLine($"{OutputPrefix} [ERR] {e.Data}"); };

            if (!logOnlyErrors)
                logger.WriteLine("Начало чтения консоли дочернего процесса: ");

            process.Start();

            if (!logOnlyErrors)
                process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();
            var exitCode = process.ExitCode;
            process.Close();

            if (exitCode != 0)
            {
                logger.Error($"Процесс завершился с кодом {exitCode}.{Environment.NewLine}\tАргументы: {args}");
                return exitCode;
            }

            if (!logOnlyErrors)
                logger.WriteLine($"Конец чтения консоли дочернего процесса. Код выхода: {exitCode}");

            return exitCode;
        }

        /// <summary>
        ///     Запускает указанную службу
        /// </summary>
        /// <param name="serviceName"> Название службы </param>
        /// <param name="exception"> Ошибка, если была </param>
        /// <param name="waitStartTimeoutInSeconds"> Таймаут ожидания старта службы в секундах </param>
        /// <returns> Была ли служба успешно запущена </returns>
        public static bool StartService(string serviceName, out Exception exception, int waitStartTimeoutInSeconds = 4)
        {
            exception = null;
            try
            {
                var service = new ServiceController(serviceName);
                if (service.Status != ServiceControllerStatus.Running)
                {

                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(waitStartTimeoutInSeconds));
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                return false;
            }
        }
    }
}
