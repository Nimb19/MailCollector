using MailCollector.Kit.Logger;
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
    public partial class TemplateForm : Form
    {
        protected readonly ILogger Logger;
        protected new readonly Form ParentForm;

        public TemplateForm(ILogger logger, Form parentForm)
        {
            InitializeComponent();
            buttonBack.Click += ButtonBack_Click;
            FormClosed += TemplateForm_FormClosed;

            this.MaximizeBox = false;

            Logger = logger;
            ParentForm = parentForm;
        }

        private void TemplateForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        protected virtual void ButtonBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            ParentForm.Show();
        }
    }
}
