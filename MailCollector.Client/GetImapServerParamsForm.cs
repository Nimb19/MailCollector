using MailKit.Net.Imap;
using MailCollector.Kit.ImapKit.Models;
using System;
using System.Windows.Forms;

namespace MailCollector.Client
{
    public partial class GetImapServerParamsForm : TemplateForm
    {
        internal ImapServerParams ImapServerParams { get; private set; }

        public GetImapServerParamsForm() : base()
        {
            InitializeComponent();

            DialogResult = DialogResult.Cancel;
        }

        private async void ButtonCheckImapServerConn_Click(object sender, EventArgs e)
        {
            buttonAddImapServer.Enabled = false;
            buttonCheckImapServerConn.Enabled = false;

            var imapServerParams = GetImapServerParams();
            if (imapServerParams == null)
                return;

            var trys = 4;
            var isConnected = false;
            Exception exception = null;
            for (int i = 0; i < trys; i++)
            {
                try
                {
                    var client = new ImapClient();
                    client.Timeout = Constants.ImapConnectTimeout;
                    await client.ConnectAsync(imapServerParams.Uri, imapServerParams.Port
                            , imapServerParams.UseSsl);

                    await client.DisconnectAsync(true);
                    client.Dispose();

                    ShowInfoBox("Подключение успешно установлено!");
                    isConnected = true;
                    break;
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }
            if (!isConnected)
            {
                ShowWarningBox(exception.ToString(), $"Не удалось подключиться под указанными данными. Попыток подключения: {trys}");
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
    }
}
