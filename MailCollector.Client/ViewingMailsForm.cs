using MailCollector.Kit;
using MailCollector.Kit.SqlKit;
using MailCollector.Kit.SqlKit.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MailCollector.Client
{
    public partial class ViewingMailsForm : TemplateForm
    {
        private SqlServerShell _sqlServerShell;
        private readonly List<MailControl> _mailsControls = new List<MailControl>();
        private readonly List<MailControl> _mailsFilteredControls;

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

        public void TryGetAndAddNewMails()
        {
            TryCatch(() =>
            {
                const string orderBy = "ORDER BY " + nameof(Mail.Date) + " DESC"; 
                var mails = _sqlServerShell.GetArrayOf<Mail>(Mail.TableName, orderBy: orderBy);

                var newMails = mails.Length - _mailsControls.Count;
                if (newMails == 0)
                    return;
                AddNewMails(mails);

                if (newMails > 1)
                    ShowInfoBox($"Пришли новые письма в количестве: {newMails}");
                else if (newMails == 1)
                    ShowInfoBox($"Пришло новое письмо");
                else
                    ShowInfoBox($"По какой то причине письма из БД были удалены. " +
                        $"{Environment.NewLine}До этого было {_mailsControls.Count}, стало {mails.Length}");
            });
        }

        public void AddNewMails(Mail[] sortedMails)
        {
            TryCatch(() =>
            {
                _mailsControls.Clear();

                var height = new MailControl().Size.Height;
                for (int i = 0; i < sortedMails.Length; i++)
                {
                    var newMailControl = new MailControl(sortedMails[i])
                    {
                        Location = new Point(0, (i * height) + 1),
                    };
                    _mailsControls.Insert(0, newMailControl);
                }
            }, "Ошибка во время добавления новых писем", true);
        }

        private void ButtonUpdateMails_Click(object sender, EventArgs e)
        {
            TryGetAndAddNewMails();
        }

        private void ButtonGetBackAllMails_Click(object sender, EventArgs e)
        {

        }

        private void ButtonFindByKeyWord_Click(object sender, EventArgs e)
        {
            if (_mailsControls.Count == 0)
                return;
            var keyWord = textBoxKeyWord.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyWord))
                return;

            
            var properties = typeof(Mail).GetProperties();

            //foreach (var mail in mails)
            //{
            // TODO:
            //}
        }
    }
}
