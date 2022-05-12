using MailCollector.Kit.Logger;
using System;
using System.Threading.Tasks;

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

        private async void ButtonNext_Click(object sender, EventArgs e)
        {
            if (NextForm == null)
                NextForm = new GetSqlInfoForm(Logger, this);
            NextForm.Show();
            await Task.Delay(Constants.DelayAfterFormHide);
            this.Hide();
        }
    }
}
