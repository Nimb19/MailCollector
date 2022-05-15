using MailCollector.Kit.Logger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailCollector.Setup
{
    public partial class InstallationForm : TemplateForm
    {
        private CancellationTokenSource _cts;

        public InstallationForm() : base()
        {
            InitializeComponent();
        }

        public InstallationForm(ILogger logger, Form parentForm, InstallerSettings setupSettings)
            : base(logger, parentForm, setupSettings)
        {
            InitializeComponent();
            _cts = new CancellationTokenSource();
            buttonNext.Click += ButtonStart_Click;
            buttonNext.Text = "Начать";
            buttonSkip.Click += ButtonSkip_Click;
        }

        private void ButtonSkip_Click(object sender, EventArgs e)
        {
            _cts.Cancel();
        }

        private async void ButtonStart_Click(object sender, EventArgs e)
        {
            buttonNext.Enabled = false;
            buttonBack.Enabled = false;
            buttonSkip.Enabled = true;

            try
            {
                var textBoxLogger = new TextBoxLogger(textBoxLog);
                (Logger as MultiLogger).Loggers.Add(textBoxLogger);

                var installer = new MailCollectorInstaller(InstallerSettings, Logger);
                await installer.StartInstall(_cts.Token);

                buttonNext.Text = "Готово!";
                buttonNext.Enabled = true;
                buttonNext.Click -= ButtonStart_Click;
                buttonNext.Click += ButtonFinish_Click;
                buttonSkip.Enabled = false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                ShowWarningBox(ex.ToString(), "Ошибка во время установки", true);
                buttonSkip.Enabled = false;
            }
        }

        private void ButtonFinish_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
