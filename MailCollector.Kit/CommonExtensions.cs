using MailCollector.Kit.Logger;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace MailCollector.Kit
{
    public static class CommonExtensions
    {
        public static readonly Encoding Encoding = Encoding.UTF8;

        public static void TryDispose<T>(this T obj, ILogger logger) where T: IDisposable
        {
            try
            {
                obj?.Dispose();
            }catch (Exception ex)
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
    }
}
