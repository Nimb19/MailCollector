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
        private readonly ILogger _logger;

        private const string _lableSqlConnStateSuccessText = "Подключение успешно установлено!";
        private static readonly Color _lableSqlConnStateSuccessColor = Color.Green;
        private const string _lableSqlConnStateFailedText = "Не удалось подключиться к СУБД";
        private static readonly Color _lableSqlConnStateFailedColor = Color.Maroon;

        private SqlServerSettings _sqlServerSettings = null;
        private bool _canPressNext = false;

        public GetSqlInfoForm(ILogger logger) : base()
        {
            _logger = logger;

            InitializeComponent();

            buttonNext.Enabled = false;
            labelSqlConnIsSuccess.Enabled = false;
        }

        private void CheckBoxUseNtAuth_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox checkBoxUseNtAuth)
            {
                if (checkBoxUseNtAuth.Checked)
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

        private void ButtonCheckSqlConn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TryGetSqlServerSettings(out var sqlServerSettings))
                    return;

                var sqlShell = new SqlServerShell(sqlServerSettings, _logger, Constants.ModuleName, null);
                //sqlShell.CreateUsers; // TODO: создать учётку для сервиса и научить сервис под ней запускаться
                // TODO: Создать БД

                //TODO: Описать рекомендации по тому что за СУБД должна быть
            }
            catch (Exception ex)
            {
                ShowWarningBox($"Текст ошибки: {ex}", "Ошибка во время соединения подключения");
            }
        }

        private bool TryGetSqlServerSettings(out SqlServerSettings sqlServerSettings)
        {
            sqlServerSettings = null;
            var sqlConnString = textBoxSqlServerConnStr.Text.Trim();
            if (string.IsNullOrWhiteSpace(sqlConnString))
            {
                ShowWarningBox($"Поле логина было пустым");
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

        private void ShowWarningBox(string text, string header = "Поле было пустым")
        {
            MessageBox.Show(text, header, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
