using MailCollector.Kit.Logger;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailCollector.Setup
{
    public partial class ChoosePathInstallForm : TemplateForm
    {
        private static readonly Point FirstFileDialogLocation = new Point(12, 146);

        private bool _hasFlagInstService;
        private bool _hasFlagInstClient;

        public ChoosePathInstallForm() : base()
        {
            InitializeComponent();
        }

        public ChoosePathInstallForm(ILogger logger, Form parentForm, SetupSettings setupSettings)
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
                ShowWarningBox("Открылась форма ввода пути установки, но устанавливать нечего"
                    , "Внутренняя ошибка", true);
                Close();
            }
        }

        private async void ButtonNext_Click(object sender, EventArgs e)
        {
            // TODO: Check path
            // TODO: Set path to settings

            if (NextForm == null)
                NextForm = new InstallationForm(Logger, this, InstallerSettings);
            NextForm.Show();
            await Task.Delay(Constants.DelayAfterFormHide);
            this.Hide();
        }

        private void ButtonSelectServicePath_Click(object sender, EventArgs e)
        {
            var folderDesc = "сервиса";
            var installFodler = FolderOpenDialog(folderDesc);
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
