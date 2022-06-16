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
            this.buttonNextPage = new System.Windows.Forms.Button();
            this.buttonBackPage = new System.Windows.Forms.Button();
            this.labelPageNumber = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelMailsCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonUpdateMails = new System.Windows.Forms.Button();
            this.buttonFindByKeyWord = new System.Windows.Forms.Button();
            this.textBoxKeyWord = new System.Windows.Forms.TextBox();
            this.panelMailsBox = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelCc = new System.Windows.Forms.Label();
            this.labelDate = new System.Windows.Forms.Label();
            this.labelSubject = new System.Windows.Forms.Label();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelFrom = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.действияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьКлиентовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обновитьПисьмаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.закрытьПрограммуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.buttonNextPage);
            this.panel1.Controls.Add(this.buttonBackPage);
            this.panel1.Controls.Add(this.labelPageNumber);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.labelMailsCount);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonUpdateMails);
            this.panel1.Controls.Add(this.buttonFindByKeyWord);
            this.panel1.Controls.Add(this.textBoxKeyWord);
            this.panel1.Controls.Add(this.panelMailsBox);
            this.panel1.Location = new System.Drawing.Point(12, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(449, 863);
            this.panel1.TabIndex = 0;
            // 
            // buttonNextPage
            // 
            this.buttonNextPage.Location = new System.Drawing.Point(242, 140);
            this.buttonNextPage.Name = "buttonNextPage";
            this.buttonNextPage.Size = new System.Drawing.Size(200, 32);
            this.buttonNextPage.TabIndex = 18;
            this.buttonNextPage.Text = "Следующая";
            this.buttonNextPage.UseVisualStyleBackColor = true;
            this.buttonNextPage.Click += new System.EventHandler(this.ButtonNextPage_Click);
            // 
            // buttonBackPage
            // 
            this.buttonBackPage.Location = new System.Drawing.Point(7, 140);
            this.buttonBackPage.Name = "buttonBackPage";
            this.buttonBackPage.Size = new System.Drawing.Size(200, 32);
            this.buttonBackPage.TabIndex = 17;
            this.buttonBackPage.Text = "Предыдующая";
            this.buttonBackPage.UseVisualStyleBackColor = true;
            this.buttonBackPage.Click += new System.EventHandler(this.ButtonBackPage_Click);
            // 
            // labelPageNumber
            // 
            this.labelPageNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPageNumber.AutoSize = true;
            this.labelPageNumber.Location = new System.Drawing.Point(346, 117);
            this.labelPageNumber.Name = "labelPageNumber";
            this.labelPageNumber.Size = new System.Drawing.Size(17, 20);
            this.labelPageNumber.TabIndex = 16;
            this.labelPageNumber.Text = "1";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 117);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(346, 20);
            this.label7.TabIndex = 15;
            this.label7.Text = "Отображается макс. 100 писем. Страница: ";
            // 
            // labelMailsCount
            // 
            this.labelMailsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMailsCount.AutoSize = true;
            this.labelMailsCount.Location = new System.Drawing.Point(404, 84);
            this.labelMailsCount.Name = "labelMailsCount";
            this.labelMailsCount.Size = new System.Drawing.Size(0, 20);
            this.labelMailsCount.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(238, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "Общее количество:";
            // 
            // buttonUpdateMails
            // 
            this.buttonUpdateMails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpdateMails.Location = new System.Drawing.Point(5, 3);
            this.buttonUpdateMails.Name = "buttonUpdateMails";
            this.buttonUpdateMails.Size = new System.Drawing.Size(437, 32);
            this.buttonUpdateMails.TabIndex = 12;
            this.buttonUpdateMails.Text = "Обновить список писем";
            this.buttonUpdateMails.UseVisualStyleBackColor = true;
            this.buttonUpdateMails.Click += new System.EventHandler(this.ButtonUpdateMails_Click);
            // 
            // buttonFindByKeyWord
            // 
            this.buttonFindByKeyWord.Location = new System.Drawing.Point(5, 78);
            this.buttonFindByKeyWord.Name = "buttonFindByKeyWord";
            this.buttonFindByKeyWord.Size = new System.Drawing.Size(227, 32);
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
            this.textBoxKeyWord.Size = new System.Drawing.Size(437, 26);
            this.textBoxKeyWord.TabIndex = 5;
            this.textBoxKeyWord.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panelMailsBox
            // 
            this.panelMailsBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMailsBox.AutoScroll = true;
            this.panelMailsBox.Location = new System.Drawing.Point(3, 178);
            this.panelMailsBox.Name = "panelMailsBox";
            this.panelMailsBox.Size = new System.Drawing.Size(441, 680);
            this.panelMailsBox.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.labelCc);
            this.panel2.Controls.Add(this.labelDate);
            this.panel2.Controls.Add(this.labelSubject);
            this.panel2.Controls.Add(this.labelTo);
            this.panel2.Controls.Add(this.labelFrom);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Location = new System.Drawing.Point(467, 37);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(945, 863);
            this.panel2.TabIndex = 1;
            // 
            // labelCc
            // 
            this.labelCc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCc.AutoSize = true;
            this.labelCc.Location = new System.Drawing.Point(77, 68);
            this.labelCc.Name = "labelCc";
            this.labelCc.Size = new System.Drawing.Size(0, 20);
            this.labelCc.TabIndex = 24;
            // 
            // labelDate
            // 
            this.labelDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDate.AutoSize = true;
            this.labelDate.Location = new System.Drawing.Point(77, 90);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(0, 20);
            this.labelDate.TabIndex = 23;
            // 
            // labelSubject
            // 
            this.labelSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSubject.AutoSize = true;
            this.labelSubject.Location = new System.Drawing.Point(77, 3);
            this.labelSubject.Name = "labelSubject";
            this.labelSubject.Size = new System.Drawing.Size(0, 20);
            this.labelSubject.TabIndex = 22;
            // 
            // labelTo
            // 
            this.labelTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(77, 45);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(0, 20);
            this.labelTo.TabIndex = 21;
            // 
            // labelFrom
            // 
            this.labelFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFrom.AutoSize = true;
            this.labelFrom.Location = new System.Drawing.Point(77, 24);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(0, 20);
            this.labelFrom.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 20);
            this.label6.TabIndex = 19;
            this.label6.Text = "Копия:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 20);
            this.label5.TabIndex = 18;
            this.label5.Text = "Дата:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 20);
            this.label4.TabIndex = 17;
            this.label4.Text = "Тема:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 20);
            this.label3.TabIndex = 16;
            this.label3.Text = "Кому:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 20);
            this.label2.TabIndex = 15;
            this.label2.Text = "От:";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.webBrowser);
            this.panel3.Location = new System.Drawing.Point(-1, 116);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(945, 746);
            this.panel3.TabIndex = 2;
            // 
            // webBrowser
            // 
            this.webBrowser.AllowNavigation = false;
            this.webBrowser.AllowWebBrowserDrop = false;
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(943, 744);
            this.webBrowser.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.действияToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1424, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // действияToolStripMenuItem
            // 
            this.действияToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьКлиентовToolStripMenuItem,
            this.обновитьПисьмаToolStripMenuItem,
            this.закрытьПрограммуToolStripMenuItem});
            this.действияToolStripMenuItem.Name = "действияToolStripMenuItem";
            this.действияToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.действияToolStripMenuItem.Text = "Действия";
            // 
            // добавитьКлиентовToolStripMenuItem
            // 
            this.добавитьКлиентовToolStripMenuItem.Name = "добавитьКлиентовToolStripMenuItem";
            this.добавитьКлиентовToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.добавитьКлиентовToolStripMenuItem.Text = "Добавить клиентов";
            this.добавитьКлиентовToolStripMenuItem.Click += new System.EventHandler(this.ДобавитьКлиентовToolStripMenuItem_Click);
            // 
            // обновитьПисьмаToolStripMenuItem
            // 
            this.обновитьПисьмаToolStripMenuItem.Name = "обновитьПисьмаToolStripMenuItem";
            this.обновитьПисьмаToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.обновитьПисьмаToolStripMenuItem.Text = "Обновить письма";
            this.обновитьПисьмаToolStripMenuItem.Click += new System.EventHandler(this.ОбновитьПисьмаToolStripMenuItem_Click);
            // 
            // закрытьПрограммуToolStripMenuItem
            // 
            this.закрытьПрограммуToolStripMenuItem.Name = "закрытьПрограммуToolStripMenuItem";
            this.закрытьПрограммуToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.закрытьПрограммуToolStripMenuItem.Text = "Закрыть программу";
            this.закрытьПрограммуToolStripMenuItem.Click += new System.EventHandler(this.ЗакрытьПрограммуToolStripMenuItem_Click);
            // 
            // ViewingMailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1424, 912);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ViewingMailsForm";
            this.Text = "Форма просмотра писем";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelMailsBox;
        private System.Windows.Forms.TextBox textBoxKeyWord;
        internal System.Windows.Forms.Button buttonFindByKeyWord;
        internal System.Windows.Forms.Button buttonUpdateMails;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelMailsCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelCc;
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.Label labelSubject;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem действияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьКлиентовToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem обновитьПисьмаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem закрытьПрограммуToolStripMenuItem;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.Label labelPageNumber;
        private System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Button buttonBackPage;
        internal System.Windows.Forms.Button buttonNextPage;
    }
}