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

namespace MailCollector.Client
{
    public partial class MailControl : UserControl
    {
        public Mail Mail { get; }

        public MailControl()
        {
            InitializeComponent();
        }

        public MailControl(Mail mail) : this()
        {
            Mail = mail;

            InitMailData();
        }

        private void InitMailData()
        {
            labelDate.Text = Mail.Date.Date.ToLocalTime().ToString("D");
            labelFrom.Text = Mail.AccsToString(Mail.MFrom);
            labelTo.Text = Mail.AccsToString(Mail.MTo);
            labelSubject.Text = Mail.Subject;
        }
    }
}
