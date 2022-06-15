using MailCollector.Kit.ImapKit;
using MailCollector.Kit.ImapKit.Models;
using MailCollector.Kit.Logger;
using MailCollector.Kit.SqlKit;
using MailCollector.Kit.SqlKit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailCollector.Client
{
    public partial class GetImapClientsForm : TemplateForm
    {
        private List<ImapClientParams> _resultImapClientParams = new List<ImapClientParams>();
        private readonly SqlServerShell _sqlServerShell;

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

        public GetImapClientsForm(SqlServerShell sqlServerShell) : base()
        {
            InitializeComponent();
            _sqlServerShell = sqlServerShell;

            comboBoImapServersNames.Items.Clear();
            var keys = _imapServers.Keys.ToArray();
            comboBoImapServersNames.Items.AddRange(keys);
            comboBoImapServersNames.SelectedIndex = 0;
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
            var clientParams = GetImapClientParam();
            if (clientParams == null)
                return;

            buttonCheckImapConn.Enabled = false;
            buttonAddClient.Enabled = false;
            textBoxLogin.Enabled = false;
            textBoxPassword.Enabled = false;
            comboBoImapServersNames.Enabled = false;

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

                    ShowInfoBox("Клиент успешно прошёл аутентификацию!", "Успешно!");
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
                var clients = _sqlServerShell.GetArrayOf<ImapClient>(ImapClient.TableName);
                var servers = _sqlServerShell.GetArrayOf<ImapServer>(ImapServer.TableName);
                (ImapClient Client, ImapServer Server)[] clientWithServer = 
                    clients.Select(x => (x, servers.First(y => y.Uid == x.ImapServerUid))).ToArray();
                var fClient = clientWithServer.FirstOrDefault(x => StringEquals(x.Client.Login, clientParams.Login)
                    && StringEquals(x.Server.Uri, clientParams.ImapServerParams.Uri)
                    && x.Server.UseSsl == clientParams.ImapServerParams.UseSsl
                    && x.Server.Port == clientParams.ImapServerParams.Port);
                if (fClient == default)
                {
                    var serverUid = Guid.NewGuid();
                    var server = new ImapServer()
                    {
                        Uid = serverUid,
                        Uri = clientParams.ImapServerParams.Uri,
                        Port = clientParams.ImapServerParams.Port,
                        UseSsl = clientParams.ImapServerParams.UseSsl,
                    };
                    var client = new ImapClient()
                    {
                        Uid = Guid.NewGuid(),
                        ImapServerUid = serverUid,
                        Login = clientParams.Login,
                        Password = clientParams.Password,
                        IsWorking = null,
                    };
                    _sqlServerShell.Insert(server, ImapServer.TableName);
                    _sqlServerShell.Insert(client, ImapClient.TableName);

                    listBoxImapClients.Items.Add(clientName);
                    _resultImapClientParams.Add(clientParams);
                }
                else
                {
                    ShowWarningBox($"Клиент {clientName} уже существует в БД.");
                }
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
