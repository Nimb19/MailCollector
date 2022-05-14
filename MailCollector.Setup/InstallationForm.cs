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
    public partial class InstallationForm : TemplateForm
    {
        public InstallationForm() : base()
        {
            InitializeComponent();
        }

        public InstallationForm(ILogger logger, Form parentForm, InstallerSettings setupSettings)
            : base(logger, parentForm, setupSettings)
        {
            InitializeComponent();
            buttonNext.Click += ButtonFinish_Click;
            buttonNext.Text = "Готово!";

            buttonNext.Enabled = false;
            buttonNext.Enabled = false;
        }

        private async void ButtonFinish_Click(object sender, EventArgs e)
        {
            if (NextForm == null)
                NextForm = new ChooseInstallPathForm(Logger, this, InstallerSettings);
            NextForm.Show();
            await Task.Delay(Constants.DelayAfterFormHide);
            this.Hide();
        }

        // TODO: Создать БД
        // TODO: sqlShell.CreateUsers; // создать учётку системы админом

        //TODO: Описать рекомендации по тому что за СУБД должна быть
    }
}
