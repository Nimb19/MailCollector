using MailCollector.Kit.Logger;
using MailCollector.Kit.SqlKit;
using MailCollector.Kit.SqlKit.Models;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailCollector.Setup
{
    public partial class GetSqlInfoForm : TemplateForm
    {
        private const string _lableSqlConnStateSuccessText = "Подключение успешно установлено!";
        private static readonly Color _lableSqlConnStateSuccessColor = Color.Green;
        private const string _lableSqlConnStateFailedText = "Не удалось подключиться к СУБД";
        private static readonly Color _lableSqlConnStateFailedColor = Color.Maroon;

        public GetSqlInfoForm() : base()
        {
            InitializeComponent();
        }

        public GetSqlInfoForm(ILogger logger, Form parentForm, SetupSettings setupSettings) 
            : base(logger, parentForm, setupSettings)
        {
            InitializeComponent();
            buttonNext.Click += ButtonNext_Click;

            buttonNext.Enabled = false;
        }

        private async void ButtonNext_Click(object sender, EventArgs e)
        {
            if (NextForm == null)
                NextForm = new ChoosePathInstallForm(Logger, this, InstallerSettings);
            NextForm.Show();
            await Task.Delay(Constants.DelayAfterFormHide);
            this.Hide();
        }

        private void CheckBoxUseIntegratedSecurity_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox checkBoxIntegratedSecurity)
            {
                if (checkBoxIntegratedSecurity.Checked)
                {
                    textBoxLogin.Enabled = false;
                    textBoxPassword.Enabled = false;
                }
                else
                {
                    textBoxLogin.Enabled = true;
                    textBoxPassword.Enabled = true;
                }
            }
        }

        private async void ButtonCheckSqlConn_Click(object sender, EventArgs e)
        {
            buttonCheckSqlConn.Enabled = false;
            buttonNext.Enabled = false;
            buttonBack.Enabled = false;

            try
            {
                if (!TryGetSqlServerSettings(out var sqlServerSettings))
                    return;

                await Task.Run(() =>
                {
                    // Тест подключения
                    var sqlShell = new SqlServerShell(sqlServerSettings, Logger, Constants.ModuleName, null);
                    sqlShell.Dispose();
                });

                InstallerSettings.SqlServerSettings = sqlServerSettings;

                labelSqlConnIsSuccess.ForeColor = _lableSqlConnStateSuccessColor;
                labelSqlConnIsSuccess.Text = _lableSqlConnStateSuccessText;

                buttonNext.Enabled = true;
            }
            catch (Exception ex)
            {
                ShowWarningBox($"Текст ошибки: {ex}", "Ошибка во время соединения подключения");

                buttonNext.Enabled = false;
                labelSqlConnIsSuccess.Text = _lableSqlConnStateFailedText;
                labelSqlConnIsSuccess.ForeColor = _lableSqlConnStateFailedColor;
            }
            finally
            {
                buttonCheckSqlConn.Enabled = true;
                buttonBack.Enabled = true;
            }
        }

        private bool TryGetSqlServerSettings(out SqlServerSettings sqlServerSettings)
        {
            sqlServerSettings = null;
            var sqlConnString = textBoxSqlServerConnStr.Text.Trim();
            if (string.IsNullOrWhiteSpace(sqlConnString))
            {
                ShowWarningBox($"Поле строки подключения к СУБД было пустым");
                return false;
            }

            var integratedSecurity = checkBoxIntegratedSecurity.Checked;
            string login = null, password = null;
            if (!integratedSecurity)
            {
                login = textBoxLogin.Text.Trim();
                if (string.IsNullOrWhiteSpace(login))
                {
                    ShowWarningBox($"Поле логина было пустым");
                    return false;
                }
                password = textBoxLogin.Text.Trim();
                if (string.IsNullOrWhiteSpace(password))
                {
                    ShowWarningBox($"Поле пароля было пустым");
                    return false;
                }
            }

            sqlServerSettings = new SqlServerSettings()
            {
                ServerName = sqlConnString,
                IntegratedSecurity = integratedSecurity,
                Login = login,
                Password = password,
            };
            return true;
        }
    }
}
