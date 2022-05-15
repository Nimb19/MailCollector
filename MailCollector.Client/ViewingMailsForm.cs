using MailCollector.Kit;
using MailCollector.Kit.SqlKit;
using MailCollector.Kit.SqlKit.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MailCollector.Client
{
    public partial class ViewingMailsForm : TemplateForm
    {
        private SqlServerShell _sqlServerShell;
        private MailControl _activeMailControl;
        private readonly List<MailControl> _mailsControls = new List<MailControl>();
        private bool _isFiltered = false;

        public ViewingMailsForm()
        {
            InitializeComponent();

            var isSqlGetted = TryCatch(() =>
            {
                _sqlServerShell = new SqlServerShell(Constants.Config.SqlServerSettings, Logger
                    , Constants.ModuleInfo, KitConstants.DbName);
            }, $"Ошибка при подключении к БД '{KitConstants.DbName}'", isErr: true);
            
            if (!isSqlGetted)
                Environment.Exit(1);

            TryGetAndAddNewMails();
        }

        public int TryGetAndAddNewMails()
        {
            int newMails = 0;
            TryCatch(() =>
            {
                const string orderBy = "ORDER BY " + nameof(Mail.Date) + " DESC"; 
                var mails = _sqlServerShell.GetArrayOf<Mail>(Mail.TableName, orderBy: orderBy);

                newMails = mails.Length - _mailsControls.Count;
                if (newMails == 0)
                    return;
                AddNewMails(mails);

            });
            return newMails;
        }

        public void AddNewMails(Mail[] sortedMails)
        {
            TryCatch(() =>
            {
                panelMailsBox.Controls.Clear();
                _mailsControls.Clear();

                var height = new MailControl().Size.Height;
                for (int i = 0; i < sortedMails.Length; i++)
                {
                    var newMailControl = new MailControl(sortedMails[i])
                    {
                        Location = new Point(0, (i * height) + 1),
                    };
                    _mailsControls.Insert(0, newMailControl);
                    panelMailsBox.Controls.Add(newMailControl);
                    newMailControl.ClickPanel.Click += NewMailControl_Click;
                }

                labelMailsCount.Text = _mailsControls.Count.ToString();
            }, "Ошибка во время добавления новых писем", true);
        }

        private void NewMailControl_Click(object sender, EventArgs e)
        {
            TryCatch(() =>
            {
                if (sender is Panel panel)
                {
                    var mailControl = (panel.Parent as MailControl) ?? throw new Exception();
                    _activeMailControl?.OnPanelDeactivated();
                    mailControl.OnPanelActivated();
                    _activeMailControl = mailControl;

                    OpenMailInWebBrowser(mailControl.Mail);
                }
            }, "Ошибка во время открытия письма", isErr: true);
        }

        private void OpenMailInWebBrowser(Mail mail)
        {
            labelSubject.Text = mail.Subject;
            labelFrom.Text = Mail.AccsToString(mail.MFrom, false, true);
            labelTo.Text = Mail.AccsToString(mail.MTo, false, true);
            labelCc.Text = Mail.AccsToString(mail.MCc, false, true);
            labelDate.Text = mail.Date.ToLocalTime().ToString("f");

            webBrowser.DocumentText = mail.HtmlBody;
        }

        private void ButtonUpdateMails_Click(object sender, EventArgs e)
        {
            var newMails = TryGetAndAddNewMails();

            if (!_isFiltered)
            {
                if (newMails == 0)
                    return;

                if (newMails > 1)
                    ShowInfoBox($"Пришли новые письма в количестве: {newMails}");
                else if (newMails == 1)
                    ShowInfoBox($"Пришло новое письмо");
                else
                    ShowInfoBox($"По какой то причине количество писем стало меньше");
            }

            _isFiltered = false;
        }

        private void ButtonFindByKeyWord_Click(object sender, EventArgs e)
        {
            if (_mailsControls.Count == 0)
                return;
            var keyWord = textBoxKeyWord.Text?.Trim()?.ToLower();
            if (string.IsNullOrWhiteSpace(keyWord))
                return;

            _isFiltered = true;
            var properties = typeof(Mail).GetProperties();
            var mailsFilteredControls = new List<Mail>();

            foreach (var mail in _mailsControls.Select(x => x.Mail))
            {
                foreach (var prop in properties)
                {
                    var val = prop.GetValue(mail);
                    if ((val is string valS && valS.ToLower().Contains(keyWord)))
                    {
                        mailsFilteredControls.Add(mail);
                        break;
                    }
                    else if (val.ToString().ToLower().Contains(keyWord))
                    {
                        mailsFilteredControls.Add(mail);
                        break;
                    }
                }
            }

            AddNewMails(mailsFilteredControls.OrderByDescending(x => x.Date).ToArray());
        }
    }
}
