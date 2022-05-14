using MailCollector.Kit.Logger;
using System;
using System.Windows.Forms;

namespace MailCollector.Client
{
    public partial class TemplateForm : Form
    {
        public ILogger Logger { get; }

        public TemplateForm()
        {
            Logger = Constants.Logger;
            InitializeComponent();
        }

        protected void ShowWarningBox(string text, string header = "Ошибка во время выполнения команды"
            , bool isError = false)
        {
            MessageBox.Show(text, header, MessageBoxButtons.OK
                , isError ? MessageBoxIcon.Error : MessageBoxIcon.Warning);
        }

        protected void ShowInfoBox(string text, string header = "Информация")
        {
            MessageBox.Show(text, header, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected bool TryCatch(Action action, string errHeader = null, bool isErr = false)
        {
            try
            {
                action.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                if (errHeader != null)
                    ShowWarningBox(ex.ToString(), errHeader, isErr);
                else
                    ShowWarningBox(ex.ToString(), isError: isErr);
                return false;
            }
        }
    }
}
