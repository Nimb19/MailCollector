using MailCollector.Kit.SqlKit.Models;
using System;
using System.Drawing;
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

        public void OnPanelActivated(object sender, EventArgs e)
        {
            panel1.BorderStyle = BorderStyle.None;
        }

        public void OnPanelDeactivated(object sender, EventArgs e)
        {
            panel1.BorderStyle = BorderStyle.FixedSingle;
        }

        private void InitMailData()
        {
            labelFrom.Text = Mail.AccsToString(Mail.MFrom);
            labelTo.Text = Mail.AccsToString(Mail.MTo);
            labelSubject.Text = Mail.Subject;
            labelDate.Text = Mail.Date.Date.ToLocalTime().ToString("f");
        }

        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            panel1.BackColor = Color.White;
        }

        private void Panel_MouseLeave(object sender, EventArgs e)
        {
            panel1.BackColor = SystemColors.InactiveBorder;
        }
    }
}
