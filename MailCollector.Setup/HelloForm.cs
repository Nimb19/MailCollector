using MailCollector.Kit.Logger;
using System;

namespace MailCollector.Setup
{
    public partial class HelloForm : TemplateForm
    {
        public HelloForm(ILogger logger) : base(logger, null)
        {
            InitializeComponent();
            buttonNext.Click += ButtonNext_Click;

            buttonBack.Enabled = false;
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            this.Hide();
            var sqlForm = new GetSqlInfoForm(Logger, this);
            sqlForm.Show();
        }
    }
}
