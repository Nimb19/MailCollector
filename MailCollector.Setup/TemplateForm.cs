using MailCollector.Kit.Logger;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailCollector.Setup
{
    public partial class TemplateForm : Form
    {
        protected readonly ILogger Logger;
        protected readonly Form ParForm;
        protected readonly InstallerSettings InstallerSettings;
        protected Form NextForm;

        public TemplateForm()
        {
            InitializeComponent();
            buttonBack.Click += ButtonBack_Click;
            FormClosed += TemplateForm_FormClosed;

            this.MaximizeBox = false;
        }

        public TemplateForm(ILogger logger, Form parentForm, InstallerSettings setupSettings) : this()
        {
            Logger = logger;
            ParForm = parentForm;
            InstallerSettings = setupSettings;
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

        protected void ShowWarningBox(string text, string header = "Ошибка во время проверки значений"
            , bool isError = false)
        {
            MessageBox.Show(text, header, MessageBoxButtons.OK
                , isError ? MessageBoxIcon.Error : MessageBoxIcon.Warning);
        }

        protected void ShowSuccessBox(string text, string header = "Успешно!")
        {
            MessageBox.Show(text, header, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
