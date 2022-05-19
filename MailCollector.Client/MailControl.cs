using MailCollector.Kit.SqlKit.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MailCollector.Client
{
    public partial class MailControl : UserControl
    {
        public static readonly Color DefaultColor = SystemColors.InactiveBorder;
        public static readonly Color MouseEnterColor = Color.White; 
        public static readonly Color ActiveColor = Color.Linen; 

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
            ClickPanel.BackColor = ActiveColor;
            RefreshLabels();
            ClickPanel.BorderStyle = BorderStyle.None;
        }

        private void RefreshLabels()
        {
            foreach (var control in ClickPanel.Controls)
            {
                if (control is Label cL)
                    cL.Refresh();
            }
        }

        public void OnPanelDeactivated()
        {
            Activated = false;

            ClickPanel.BackColor = DefaultColor;
            RefreshLabels();
            ClickPanel.BorderStyle = BorderStyle.FixedSingle;
        }

        private void InitMailData()
        {
            labelFrom.Text = Mail.AccsToString(Mail.MFrom);
            labelTo.Text = Mail.AccsToString(Mail.MTo);
            labelSubject.Text = Mail.Subject;
            labelDate.Text = Mail.Date.ToLocalTime().ToString("f");
        }

        private void Panel_MouseLeave(object sender, EventArgs e)
        {
            if (!Activated)
            {
                ClickPanel.BackColor = DefaultColor;
                RefreshLabels();
            }
        }

        private void Panel_MouseEnter(object sender, EventArgs e)
        {
            if (!Activated)
            {
                ClickPanel.BackColor = MouseEnterColor;
                RefreshLabels();
            }
        }
    }
}
