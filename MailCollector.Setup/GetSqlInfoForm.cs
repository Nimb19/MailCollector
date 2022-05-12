using MailCollector.Kit.Logger;
using MailCollector.Kit.SqlKit;
using MailCollector.Kit.SqlKit.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        private SqlServerSettings _sqlServerSettings = null;
        private bool _canPressNext = false;

        public GetSqlInfoForm(ILogger logger, Form parentForm) : base(logger, parentForm)
        {
            InitializeComponent();
            buttonNext.Click += ButtonNext_Click;

            buttonNext.Enabled = false;
            labelSqlConnIsSuccess.Enabled = false;
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            this.Hide();
            var sqlForm = new InstallationForm(Logger, this);
            sqlForm.Show();
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
            try
            {
                if (!TryGetSqlServerSettings(out var sqlServerSettings))
                    return;

                await Task.Run(() =>
                {
                    // Чисто тест подключения
                    var sqlShell = new SqlServerShell(sqlServerSettings, Logger, Constants.ModuleName, null);
                });

                labelSqlConnIsSuccess.Text = _lableSqlConnStateSuccessText;
                labelSqlConnIsSuccess.ForeColor = _lableSqlConnStateSuccessColor;
            }
            catch (Exception ex)
            {
                ShowWarningBox($"Текст ошибки: {ex}", "Ошибка во время соединения подключения");

                labelSqlConnIsSuccess.Text = _lableSqlConnStateFailedText;
                labelSqlConnIsSuccess.ForeColor = _lableSqlConnStateFailedColor;
            }
            finally
            {
                buttonCheckSqlConn.Enabled = true;
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

        private void ShowWarningBox(string text, string header = "Ошибка во время проверки значений")
        {
            MessageBox.Show(text, header, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
