using MailCollector.Kit.Logger;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailCollector.Setup
{
    public partial class SelectInstalStepsForm : TemplateForm
    {
        public SelectInstalStepsForm() : base()
        {
            InitializeComponent();
        }

        public SelectInstalStepsForm(ILogger logger, Form parentForm, SetupSettings setupSettings)
            : base(logger, parentForm, setupSettings)
        {
            InitializeComponent();
            buttonNext.Click += ButtonNext_Click;
        }

        private async void ButtonNext_Click(object sender, EventArgs e)
        {
            var createDb = checkBoxCreateDb.Checked;
            var installService = checkBoxInstallService.Checked;
            var configureTgBot = checkBoxConfigureTgBot.Checked;
            var installClient = checkBoxInstallClient.Checked;

            var setupSteps = SetupSteps.None;
            if (createDb)
                setupSteps = setupSteps | SetupSteps.CreateDb;
            if (installService)
                setupSteps = setupSteps | SetupSteps.InstallService;
            if (configureTgBot)
                setupSteps = setupSteps | SetupSteps.ConfigureTgBot;
            if (installClient)
                setupSteps = setupSteps | SetupSteps.InstallClient;

            if (setupSteps == 0)
            {
                ShowWarningBox("Вы не отметили ни одного компонента для установки");
                return;
            }
            InstallerSettings.InstallSteps = setupSteps;

            if (NextForm == null)
                NextForm = new GetSqlInfoForm(Logger, this, InstallerSettings);
            NextForm.Show();
            await Task.Delay(Constants.DelayAfterFormHide);
            this.Hide();
        }
    }
}
