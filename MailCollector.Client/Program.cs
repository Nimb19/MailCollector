using MailCollector.Kit;
using MailCollector.Kit.Logger;
using MailCollector.Kit.ServiceKit;
using System;
using System.Windows.Forms;

namespace MailCollector.Client
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            GetConfigSettings();

            Application.Run(new ViewingMailsForm());
        }

        private static void GetConfigSettings()
        {
            var errorMsg = $"Ошибка во время инициализации конфига '{Constants.Config}'";
            try
            {
                var config = CommonExtensions.DeserializeFile<ClientConfig>(Constants.PathToConfig)
                    ?? throw new ArgumentNullException(errorMsg);

                if (config.SqlServerSettings == null)
                    throw new ArgumentNullException($"Укажите параметры подключения к sql серверу с БД '{KitConstants.DbName}'");

                if (config.UpdateMailsTimeImMs == default)
                    config.UpdateMailsTimeImMs = Constants.DefaultUpdatemailsTime;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, errorMsg
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }
    }
}
