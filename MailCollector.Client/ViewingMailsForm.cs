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
        private const int MaxMailssCountOnPage = 100;
        private int _currentPage = 1;
        private int _pagesCount;
        private Mail[] _lastMails;

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

        private void NumericUpDownPageNum_ValueChanged(object sender, EventArgs e)
        {
            _currentPage = (int)numericUpDownPageNum.Value;
            AddCurrentPageMails();
        }

        private void ButtonBackPage_Click(object sender, EventArgs e)
        {
            BackPage();
        }

        private void BackPage()
        {
            if (_currentPage == 1)
                return;

            _currentPage--;
            numericUpDownPageNum.Value = _currentPage;
            AddCurrentPageMails();
        }

        private void ButtonNextPage_Click(object sender, EventArgs e)
        {
            NextPage();
        }

        private void NextPage()
        {
            if (_currentPage == _pagesCount)
                return;

            _currentPage++;
            numericUpDownPageNum.Value = _currentPage;
            AddCurrentPageMails();
        }

        public int TryGetAndAddNewMails()
        {
            int newMails = 0;
            TryCatch(() =>
            {
                const string orderBy = "ORDER BY " + nameof(Mail.Date) + " DESC"; 
                var mails = _sqlServerShell.GetArrayOf<Mail>(Mail.TableName, orderBy: orderBy);

                if (mails.Length == 0)
                    return;

                newMails = mails.Length - _lastMails.Length;
                if (newMails == 0)
                    return;

                _lastMails = mails;
                _pagesCount = (int)Math.Ceiling((double)_lastMails.Length / (double)MaxMailssCountOnPage);
                labelMailsCount.Text = _lastMails.Length.ToString();
                numericUpDownPageNum.Maximum = _pagesCount;
                AddCurrentPageMails();

            });
            return newMails;
        }

        public void AddCurrentPageMails()
        {
            TryCatch(() =>
            {
                var start = (_currentPage - 1) * MaxMailssCountOnPage;
                var ostatok = _lastMails.Length - start;
                var end = ostatok >= MaxMailssCountOnPage
                    ? start + MaxMailssCountOnPage - 1
                    : start + ostatok - 1;

                GenerateMailControls(start, end, _lastMails);
            }, "Ошибка во время добавления писем", true);
        }

        private void GenerateMailControls(int start, int end, Mail[] mails)
        {
            panelMailsBox.Controls.Clear();
            _mailsControls.Clear();

            var height = new MailControl().Size.Height;
            var count = 0;
            for (int i = start; i <= end; i++)
            {
                var newMailControl = new MailControl(mails[i])
                {
                    Location = new Point(0, (count++ * height) + 1),
                    Width = panelMailsBox.Width - 18,
                };
                _mailsControls.Insert(0, newMailControl);
                panelMailsBox.Controls.Add(newMailControl);
                newMailControl.ClickPanel.Click += NewMailControl_Click;
                foreach (var control in newMailControl.ClickPanel.Controls)
                {
                    if (control is Label cL)
                        cL.Click += NewMailControl_Click;
                }
            }
        }

        public void AddSelectedMails(Mail[] sortedMails)
        {
            TryCatch(() =>
            {
                GenerateMailControls(0, sortedMails.Length, sortedMails);
            }, "Ошибка во время добавления новых писем", true);
        }

        private void NewMailControl_Click(object sender, EventArgs e)
        {
            TryCatch(() =>
            {
                if (sender is Label label)
                    sender = label.Parent;

                if (sender is Panel panel)
                {
                    var mailControl = (panel.Parent as MailControl) ?? throw new Exception();
                    if (mailControl.Activated)
                        return;

                    _activeMailControl?.OnPanelDeactivated();
                    mailControl.OnPanelActivated();
                    _activeMailControl = mailControl;

                    OpenMailInWebBrowser(mailControl.Mail);
                }
            }, "Ошибка во время открытия письма", isErr: true);
        }

        private void OpenMailInWebBrowser(Mail mail)
        {
            webBrowser.Navigate("about:blank");

            labelSubject.Text = mail.Subject;
            labelFrom.Text = Mail.AccsToString(mail.MFrom, false, false);
            labelTo.Text = Mail.AccsToString(mail.MTo, false, false);
            labelCc.Text = Mail.AccsToString(mail.MCc, false, false);
            labelDate.Text = mail.Date.ToLocalTime().ToString("f");

            webBrowser.Document.OpenNew(false);
            webBrowser.Document.Write(mail.HtmlBody ?? string.Empty);
            webBrowser.Refresh();
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
                    if (val == null)
                        continue;
                    else if (val is Guid)
                        continue;
                    var isContains = false;
                    
                    if((val is string valS && valS.ToLower().Contains(keyWord)))
                        isContains = true;
                    else if (val is DateTimeOffset valD && valD.ToString("f").ToLower().Contains(keyWord))
                        isContains = true;
                    else if (val is DateTime valDt && valDt.ToString("f").ToLower().Contains(keyWord))
                        isContains = true;
                    else if (val.ToString().ToLower().Contains(keyWord))
                        isContains = true;

                    if (isContains)
                    {
                        mailsFilteredControls.Add(mail);
                        break;
                    }
                }
            }

            AddSelectedMails(mailsFilteredControls.OrderByDescending(x => x.Date).ToArray());
        }

        private void ЗакрытьПрограммуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sqlServerShell.TryDispose(Logger);
            Environment.Exit(0);
        }

        private void ДобавитьКлиентовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var getImapClientsForm = new GetImapClientsForm(_sqlServerShell);
            getImapClientsForm.ShowDialog();
        }

        private void ОбновитьПисьмаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ButtonUpdateMails_Click(sender, e);
        }
    }
}
