using MailCollector.Kit.Logger;
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
    }
}
