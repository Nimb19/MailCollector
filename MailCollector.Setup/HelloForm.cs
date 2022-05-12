using MailCollector.Kit.Logger;
using System;
using System.Threading.Tasks;

namespace MailCollector.Setup
{
    public partial class HelloForm : TemplateForm
    {
        public HelloForm(ILogger logger, SetupSettings setupSettings)
            : base(logger, null, setupSettings)
        {
            InitializeComponent();
            buttonNext.Click += ButtonNext_Click;

            buttonBack.Enabled = false;
        }

        private async void ButtonNext_Click(object sender, EventArgs e)
        {
            if (NextForm == null)
                NextForm = new SelectInstalStepsForm(Logger, this, InstallerSettings);
            NextForm.Show();
            await Task.Delay(Constants.DelayAfterFormHide);
            this.Hide();
        }
    }
}
