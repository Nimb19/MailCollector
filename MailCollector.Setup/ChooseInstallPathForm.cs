using MailCollector.Kit.Logger;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailCollector.Setup
{
    public partial class ChooseInstallPathForm : TemplateForm
    {
        private static readonly Point FirstFileDialogLocation = new Point(12, 146);

        private bool _hasFlagInstService;
        private bool _hasFlagInstClient;
        private bool _hasConfigureTgBotOrCreateDb;

        public ChooseInstallPathForm() : base()
        {
            InitializeComponent();
        }

        public ChooseInstallPathForm(ILogger logger, Form parentForm, InstallerSettings setupSettings)
            : base(logger, parentForm, setupSettings)
        {
            InitializeComponent();
            buttonNext.Click += ButtonNext_Click;

            _hasFlagInstService = setupSettings.InstallSteps.HasFlag(SetupSteps.InstallService);
            _hasFlagInstClient = setupSettings.InstallSteps.HasFlag(SetupSteps.InstallClient);

            if (_hasFlagInstService || _hasFlagInstClient)
            {
                if (_hasFlagInstService == _hasFlagInstClient)
                {
                    Text = "Выберите путь установки сервиса и клиентской части";
                    labelInfo.Text = "Выберите папку для установки сервиса и клиентской части:";
                }
                else
                {
                    if (_hasFlagInstService)
                    {
                        Text = "Выберите путь установки сервиса";
                        labelInfo.Text = "Выберите папку установки сервиса:";

                        panelSelectClientPath.Visible = false;
                    }
                    if (_hasFlagInstClient)
                    {
                        Text = "Выберите путь установки клиентской части";
                        labelInfo.Text = "Выберите папку установки клиентской части:";

                        panelSelectServicePath.Visible = false;
                        panelSelectClientPath.Location = FirstFileDialogLocation;
                    }
                }
            }
            else
            {
                _hasConfigureTgBotOrCreateDb = setupSettings.InstallSteps.HasFlag(SetupSteps.ConfigureTgBot)
                    || setupSettings.InstallSteps.HasFlag(SetupSteps.CreateDb);
                if (_hasConfigureTgBotOrCreateDb)
                {
                    Text = "Выберите путь к установленному сервису";
                    labelInfo.Text = "Выберите папку, где был установлен сервис:";

                    panelSelectClientPath.Visible = false;
                }
                else
                {
                    ShowWarningBox("Открылась форма ввода пути установки, но устанавливать нечего."
                        , "Внутренняя ошибка", true);
                    Close();
                }
            }
        }

        private async void ButtonNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (_hasFlagInstService || _hasConfigureTgBotOrCreateDb)
                {
                    var servicePath = textBoxServicePath.Text.Trim();

                    if (!string.IsNullOrWhiteSpace(servicePath))
                    {
                        DirectoryInfo dirInfo;

                        if (_hasFlagInstService)
                        {
                            dirInfo = Directory.CreateDirectory(servicePath);
                        }
                        else
                        {
                            dirInfo = new DirectoryInfo(servicePath);
                            if (!dirInfo.Exists)
                            {
                                ShowWarningBox("Путь к папке сервиса указан не верно, папки не существует.");
                                return;
                            }
                        }
                        InstallerSettings.InstallServicePath = dirInfo.FullName;
                    }
                    else
                    {
                        if (_hasFlagInstService)
                            ShowWarningBox("Путь для установки сервиса не указан.");
                        else
                            ShowWarningBox("Путь к папке сервиса не указан.");
                        return;
                    }
                }
                if (_hasFlagInstClient)
                {
                    var clientPath = textBoxClientPath.Text.Trim();

                    if (!string.IsNullOrWhiteSpace(clientPath))
                    {
                        var dirInfo = Directory.CreateDirectory(clientPath);
                        InstallerSettings.InstallClientPath = dirInfo.FullName;
                    }
                    else
                    {
                        ShowWarningBox("Путь для установки клиентской части не указан.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowWarningBox(ex.ToString());
                return;
            }

            if (NextForm == null)
            {
                if (InstallerSettings.InstallSteps.HasFlag(SetupSteps.ConfigureTgBot))
                    NextForm = new GetTelegramBotTokenForm(Logger, this, InstallerSettings);
                else
                    NextForm = new InstallationForm(Logger, this, InstallerSettings);
            }
            NextForm.Show();
            await Task.Delay(Constants.DelayAfterFormHide);
            this.Hide();
        }

        private void ButtonSelectServicePath_Click(object sender, EventArgs e)
        {
            var folderDesc = "сервиса";
            var installFodler = FolderOpenDialog(folderDesc);
            if (!string.IsNullOrWhiteSpace(installFodler))
                textBoxServicePath.Text = installFodler;
        }

        private void ButtonSelectClientPath_Click(object sender, EventArgs e)
        {
            var folderDesc = "клиентской части";
            var installFodler = FolderOpenDialog(folderDesc);
            textBoxClientPath.Text = installFodler;
        }

        private static string FolderOpenDialog(string folderDesc)
        {
            var folderBrowserDialog = new FolderBrowserDialog
            {
                Description = $"Выберите папку для установки {folderDesc}",
                ShowNewFolderButton = true,
            };

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                return folderBrowserDialog.SelectedPath;
            }

            return null;
        }
    }
}
