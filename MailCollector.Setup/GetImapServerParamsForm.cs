using MailKit.Net.Imap;
using MailCollector.Kit.ImapKit.Models;
using System;
using System.Windows.Forms;

namespace MailCollector.Setup
{
    public partial class GetImapServerParamsForm : TemplateForm
    {
        internal ImapServerParams ImapServerParams { get; private set; }

        public GetImapServerParamsForm() : base()
        {
            InitializeComponent();

            DialogResult = DialogResult.Cancel;

            buttonNext.Visible = false;
            buttonBack.Visible = false;
            buttonSkip.Visible = false;
        }

        private async void ButtonCheckImapServerConn_Click(object sender, EventArgs e)
        {
            buttonAddImapServer.Enabled = false;
            buttonCheckImapServerConn.Enabled = false;

            var imapServerParams = GetImapServerParams();
            if (imapServerParams == null)
                return;

            try
            {
                var client = new ImapClient();
                client.Timeout = Constants.ImapConnectTimeout;
                await client.ConnectAsync(imapServerParams.Uri, imapServerParams.Port
                        , imapServerParams.UseSsl);

                await client.DisconnectAsync(true);
                client.Dispose();

                ShowSuccessBox("Подключение успешно установлено!");
            } catch (Exception ex)
            {
                ShowWarningBox($"Текст ошибки: {ex}", "Не удалось подключиться под указанными данными");
            }

            buttonAddImapServer.Enabled = true;
            buttonCheckImapServerConn.Enabled = true;
        }

        private void ButtonAddImapServer_Click(object sender, EventArgs e)
        {
            var imapServerParams = GetImapServerParams();
            if (imapServerParams == null)
                return;

            ImapServerParams = imapServerParams;
            DialogResult = DialogResult.OK;
            Close();
        }

        private ImapServerParams GetImapServerParams()
        {
            try
            {
                var serverUri = textBoxServer.Text.Trim();
                if (string.IsNullOrEmpty(serverUri))
                {
                    ShowWarningBox("Адрес IMAP-сервера не введён.");
                    return null;
                }
                var port = Convert.ToInt32(numericUpDownPort.Value);
                if (port == 993)
                    port = 0;
                var useSsl = checkBoxUseSsl.Checked;

                return new ImapServerParams(serverUri, port, useSsl);
            }
            catch (Exception ex)
            {
                ShowWarningBox(ex.ToString(), isError: true);
                return null;
            }
        }

        protected override void TemplateForm_FormClosed(object sender, FormClosedEventArgs e) { }
    }
}
