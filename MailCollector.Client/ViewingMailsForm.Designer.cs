namespace MailCollector.Client
{
    partial class ViewingMailsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonGetBackAllMails = new System.Windows.Forms.Button();
            this.buttonUpdateMails = new System.Windows.Forms.Button();
            this.buttonFindByKeyWord = new System.Windows.Forms.Button();
            this.textBoxKeyWord = new System.Windows.Forms.TextBox();
            this.panelMailsBox = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.buttonGetBackAllMails);
            this.panel1.Controls.Add(this.buttonUpdateMails);
            this.panel1.Controls.Add(this.buttonFindByKeyWord);
            this.panel1.Controls.Add(this.textBoxKeyWord);
            this.panel1.Controls.Add(this.panelMailsBox);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(426, 694);
            this.panel1.TabIndex = 0;
            // 
            // buttonGetBackAllMails
            // 
            this.buttonGetBackAllMails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGetBackAllMails.Location = new System.Drawing.Point(242, 78);
            this.buttonGetBackAllMails.Name = "buttonGetBackAllMails";
            this.buttonGetBackAllMails.Size = new System.Drawing.Size(177, 32);
            this.buttonGetBackAllMails.TabIndex = 13;
            this.buttonGetBackAllMails.Text = "Вернуть всю почту";
            this.buttonGetBackAllMails.UseVisualStyleBackColor = true;
            this.buttonGetBackAllMails.Click += new System.EventHandler(this.ButtonGetBackAllMails_Click);
            // 
            // buttonUpdateMails
            // 
            this.buttonUpdateMails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpdateMails.Location = new System.Drawing.Point(5, 3);
            this.buttonUpdateMails.Name = "buttonUpdateMails";
            this.buttonUpdateMails.Size = new System.Drawing.Size(414, 32);
            this.buttonUpdateMails.TabIndex = 12;
            this.buttonUpdateMails.Text = "Обновить почту";
            this.buttonUpdateMails.UseVisualStyleBackColor = true;
            this.buttonUpdateMails.Click += new System.EventHandler(this.ButtonUpdateMails_Click);
            // 
            // buttonFindByKeyWord
            // 
            this.buttonFindByKeyWord.Location = new System.Drawing.Point(5, 78);
            this.buttonFindByKeyWord.Name = "buttonFindByKeyWord";
            this.buttonFindByKeyWord.Size = new System.Drawing.Size(231, 32);
            this.buttonFindByKeyWord.TabIndex = 11;
            this.buttonFindByKeyWord.Text = "Найти по ключевому слову";
            this.buttonFindByKeyWord.UseVisualStyleBackColor = true;
            this.buttonFindByKeyWord.Click += new System.EventHandler(this.ButtonFindByKeyWord_Click);
            // 
            // textBoxKeyWord
            // 
            this.textBoxKeyWord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxKeyWord.Location = new System.Drawing.Point(5, 46);
            this.textBoxKeyWord.Name = "textBoxKeyWord";
            this.textBoxKeyWord.Size = new System.Drawing.Size(414, 26);
            this.textBoxKeyWord.TabIndex = 5;
            this.textBoxKeyWord.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panelMailsBox
            // 
            this.panelMailsBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMailsBox.AutoScroll = true;
            this.panelMailsBox.Location = new System.Drawing.Point(3, 116);
            this.panelMailsBox.Name = "panelMailsBox";
            this.panelMailsBox.Size = new System.Drawing.Size(418, 573);
            this.panelMailsBox.TabIndex = 0;
            // 
            // ViewingMailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1153, 718);
            this.Controls.Add(this.panel1);
            this.Name = "ViewingMailsForm";
            this.Text = "Форма просмотра писем";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelMailsBox;
        private System.Windows.Forms.TextBox textBoxKeyWord;
        internal System.Windows.Forms.Button buttonFindByKeyWord;
        internal System.Windows.Forms.Button buttonUpdateMails;
        internal System.Windows.Forms.Button buttonGetBackAllMails;
    }
}