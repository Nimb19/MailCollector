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
    public partial class HelloForm : TemplateForm
    {
        public HelloForm() : base()
        {
            InitializeComponent();

            buttonBack.Enabled = false;
        }
    }
}
