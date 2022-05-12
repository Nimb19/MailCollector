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
        public InstallationForm(ILogger logger, Form parentForm) : base(logger, parentForm)
        {
            InitializeComponent();
        }

        // TODO: Создать БД
        // TODO: sqlShell.CreateUsers; // создать учётку системы админом

        //TODO: Описать рекомендации по тому что за СУБД должна быть
    }
}
