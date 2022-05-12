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
        protected readonly Form ParForm;
        protected Form NextForm;

        public TemplateForm()
        {
            InitializeComponent();
            buttonBack.Click += ButtonBack_Click;
            FormClosed += TemplateForm_FormClosed;

            this.MaximizeBox = false;
        }

        public TemplateForm(ILogger logger, Form parentForm) : this()
        {
            Logger = logger;
            ParForm = parentForm;
        }

        protected virtual void TemplateForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        protected virtual void ButtonBack_Click(object sender, EventArgs e)
        {
            ParForm.Show();
            Task.Delay(50).Wait();
            this.Hide();
        }
    }
}
