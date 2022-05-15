using MailCollector.Kit.SqlKit.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MailCollector.Client
{
    public partial class MailControl : UserControl
    {
        public Mail Mail { get; }
        public bool Activated { get; private set; }

        public MailControl()
        {
            InitializeComponent();
        }

        public MailControl(Mail mail) : this()
        {
            Mail = mail;
            InitMailData();
        }

        public void OnPanelActivated()
        {
            Activated = true;
            ClickPanel.BackColor = Color.Linen;
            ClickPanel.BorderStyle = BorderStyle.None;
        }

        public void OnPanelDeactivated()
        {
            Activated = false;
            ClickPanel.BackColor = SystemColors.InactiveBorder;
            ClickPanel.BorderStyle = BorderStyle.FixedSingle;
        }

        private void InitMailData()
        {
            labelFrom.Text = Mail.AccsToString(Mail.MFrom);
            labelTo.Text = Mail.AccsToString(Mail.MTo);
            labelSubject.Text = Mail.Subject;
            labelDate.Text = Mail.Date.Date.ToLocalTime().ToString("f");
        }

        private void Panel_MouseLeave(object sender, EventArgs e)
        {
            if (!Activated)
                ClickPanel.BackColor = SystemColors.InactiveBorder;
        }

        private void Panel_MouseEnter(object sender, EventArgs e)
        {
            if (!Activated)
                ClickPanel.BackColor = Color.White;
        }
    }
}
