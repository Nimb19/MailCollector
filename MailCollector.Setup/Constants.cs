namespace MailCollector.Setup
{
    public static class Constants
    {
        public const string ModuleName = "MailCollector.Setup";
        /// <summary>
        ///     Что бы переключение форм было плавным и без мерцания, добавлена небольшая задержка
        /// </summary>
        public const int DelayAfterFormHide = 100;

        /// <summary>
        ///     Таймаут подключения к IMAP-серверу. В миллисекундах
        /// </summary>
        public const int ImapConnectTimeout = 7000;
    }
}
