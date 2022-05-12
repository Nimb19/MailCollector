namespace MailCollector.Setup
{
    /// <summary>
    ///     Шаги, выбранные для установки
    /// </summary>
    public enum SetupSteps
    {
        None = 0x0000,
        CreateDb = 0x0001,
        InstallService = 0x0010,
        ConfigureTgBot = 0x0100,
        InstallClient = 0x1000,
    }
}
