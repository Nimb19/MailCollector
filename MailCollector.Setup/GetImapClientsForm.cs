using MailCollector.Kit.ImapKit;
using MailCollector.Kit.ImapKit.Models;
using MailCollector.Kit.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailCollector.Setup
{
    public partial class GetImapClientsForm : TemplateForm
    {
        private List<ImapClientParams> _resultImapClientParams = new List<ImapClientParams>();

        private Dictionary<string, ImapServerParams> _imapServers = new Dictionary<string, ImapServerParams>()
        {
            { "Яндекс.Почта", SupportedImapServers.YandexParams },
            { "Mail.ru почта", SupportedImapServers.MailRuParams },
            { "Gmail почта", SupportedImapServers.GmailParams },
            { "Ввести другую почту", null },
        };

        public GetImapClientsForm() : base()
        {
            InitializeComponent();
        }

        public GetImapClientsForm(ILogger logger, Form parentForm, InstallerSettings setupSettings)
            : base(logger, parentForm, setupSettings)
        {
            InitializeComponent();
            buttonNext.Click += ButtonNext_Click;
            buttonSkip.Click += ButtonSkip_Click; ;

            buttonNext.Enabled = false;
            buttonSkip.Visible = true;

            comboBoImapServersNames.Items.Clear();
            var keys = _imapServers.Keys.ToArray();
            comboBoImapServersNames.Items.AddRange(keys);
            comboBoImapServersNames.SelectedIndex = 0;
        }

        private void ButtonSkip_Click(object sender, EventArgs e)
        {
            InstallerSettings.ImapClients = null;
            ToNextForm();
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            InstallerSettings.ImapClients = _resultImapClientParams.ToArray();
            ToNextForm();
        }

        private async void ToNextForm()
        {
            if (InstallerSettings.InstallSteps.HasFlag(SetupSteps.CreateDb)
                    || InstallerSettings.InstallSteps.HasFlag(SetupSteps.InstallService)
                    || InstallerSettings.InstallSteps.HasFlag(SetupSteps.InstallClient))
            {
                NextForm = new GetSqlInfoForm(Logger, this, InstallerSettings);
            }
            NextForm.Show();
            await Task.Delay(Constants.DelayAfterFormHide);
            this.Hide();
        }

        private void CheckBoxHidePass_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox checkBoxHidePass)
            {
                if (checkBoxHidePass.Checked)
                {
                    textBoxPassword.UseSystemPasswordChar = true;
                }
                else
                {
                    textBoxPassword.UseSystemPasswordChar = false;
                }
            }
        }

        private async void ButtonCheckImapConn_Click(object sender, EventArgs e)
        {
            var buttonNextActive = buttonNext.Enabled;
            if (buttonNextActive)
                buttonNext.Enabled = false;

            buttonBack.Enabled = false;
            buttonCheckImapConn.Enabled = false;
            buttonAddClient.Enabled = false;
            textBoxLogin.Enabled = false;
            textBoxPassword.Enabled = false;
            comboBoImapServersNames.Enabled = false;

            var clientParams = GetImapClientParam();
            if (clientParams == null)
                return;

            var trys = 4;
            var isConnected = false;
            Exception exception = null;
            for (int i = 0; i < trys; i++)
            {
                try
                {
                    var imapClient = await clientParams.ConnectAsync(default, Constants.ImapConnectTimeout);
                    await imapClient.DisconnectAsync(true);
                    imapClient.Dispose();

                    ShowSuccessBox("Клиент успешно прошёл аутентификацию!");
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
                ShowWarningBox(exception.ToString(), $"Аутентификация прошла с ошибкой. Попыток подключения: {trys}");
            }

            if (buttonNextActive)
                buttonNext.Enabled = true;
            buttonBack.Enabled = true;
            buttonCheckImapConn.Enabled = true;
            buttonAddClient.Enabled = true;
            textBoxLogin.Enabled = true;
            textBoxPassword.Enabled = true;
            comboBoImapServersNames.Enabled = true;
        }

        private void ButtonAddClient_Click(object sender, EventArgs e)
        {
            var clientParams = GetImapClientParam();
            if (clientParams == null)
                return;

            var clientName = clientParams.Login;
            var isExist = listBoxImapClients.Items
                .OfType<string>()
                .Any(x => string.Equals(clientName, x, StringComparison.OrdinalIgnoreCase));

            if (!isExist)
            {
                listBoxImapClients.Items.Add(clientName);
                _resultImapClientParams.Add(clientParams);
                buttonNext.Enabled = true;
            }
            else
            {
                ShowWarningBox($"Клиент {clientName} уже добавлен в список.");
            }
        }

        private ImapClientParams GetImapClientParam()
        {
            try
            {
                var login = textBoxLogin.Text.Trim();
                if (string.IsNullOrEmpty(login))
                {
                    ShowWarningBox("Пустое поле для логина.");
                    return null;
                }
                var password = textBoxPassword.Text.Trim();
                var imapServerName = comboBoImapServersNames.Text;
                var imapServer = _imapServers[imapServerName];

                if (imapServer == null)
                {
                    var getImapServerParamsForm = new GetImapServerParamsForm();
                    var dialogResult = getImapServerParamsForm.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                    {
                        imapServer = getImapServerParamsForm.ImapServerParams;
                        var imapSName = imapServer.Uri;
                        _imapServers.Add(imapSName, imapServer);
                        comboBoImapServersNames.Items.Insert(0, imapSName);
                        comboBoImapServersNames.SelectedIndex = 0;
                    }
                    else
                    {
                        return null;
                    }
                }

                return new ImapClientParams(login, password, imapServer);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                ShowWarningBox(ex.ToString());
            }

            return null;
        }
    }
}
