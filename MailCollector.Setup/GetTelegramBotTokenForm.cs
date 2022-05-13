using MailCollector.Kit.Logger;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailCollector.Setup
{
    public partial class GetTelegramBotTokenForm : TemplateForm
    {
        public GetTelegramBotTokenForm() : base()
        {
            InitializeComponent();
        }

        public GetTelegramBotTokenForm(ILogger logger, Form parentForm, InstallerSettings setupSettings)
            : base(logger, parentForm, setupSettings)
        {
            InitializeComponent();
            buttonNext.Click += ButtonNext_Click;
            buttonSkip.Click += ButtonSkip_Click;

            buttonNext.Enabled = false;
            buttonSkip.Visible = true;
        }

        private void ButtonSkip_Click(object sender, EventArgs e)
        {
            InstallerSettings.TelegramBotToken = null;
            buttonNext.Enabled = false;
            ToNextForm();
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            ToNextForm();
        }

        private async void ToNextForm()
        {
            if (base.NextForm == null)
                base.NextForm = new InstallationForm(Logger, this, InstallerSettings);
            base.NextForm.Show();
            await Task.Delay(Constants.DelayAfterFormHide);
            this.Hide();
        }

        private void ButtonPaste_Click(object sender, EventArgs e)
        {
            textBoxToken.Text = Clipboard.GetText();
        }

        private void ButtonConfirmToken_Click(object sender, EventArgs e)
        {
            buttonNext.Enabled = false;

            var token = textBoxToken.Text.Trim();
            if (!string.IsNullOrWhiteSpace(token))
            {
                InstallerSettings.TelegramBotToken = token;
                buttonNext.Enabled = true;
            }
            else
            {
                ShowWarningBox("Заполните поле с токеном или пропустите этот шаг.");
                return;
            }
        }
    }
}
