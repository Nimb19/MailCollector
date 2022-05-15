using MailCollector.Kit.Logger;
using System;
using System.Windows.Forms;

namespace MailCollector.Setup
{
    public class TextBoxLogger : AbstractLogger
    {
        public TextBox TextBox { get; }

        public TextBoxLogger(TextBox textBox)
        {
            TextBox = textBox;
        }

        protected override void PrivateWrite(string fullMsg)
        {
            TextBox.Text +=  fullMsg + Environment.NewLine;
        }
    }
}
