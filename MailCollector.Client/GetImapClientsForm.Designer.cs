namespace MailCollector.Client
{
    partial class GetImapClientsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxLogin = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoImapServersNames = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonCheckImapConn = new System.Windows.Forms.Button();
            this.buttonAddClient = new System.Windows.Forms.Button();
            this.checkBoxHidePass = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listBoxImapClients = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(497, 58);
            this.label1.TabIndex = 17;
            this.label1.Text = "Добавьте клиентов, вписав параметры для подключения к почтовым ящикам с которых В" +
    "ы хотите собирать письма.";
            // 
            // textBoxLogin
            // 
            this.textBoxLogin.Location = new System.Drawing.Point(14, 33);
            this.textBoxLogin.Name = "textBoxLogin";
            this.textBoxLogin.Size = new System.Drawing.Size(251, 26);
            this.textBoxLogin.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(11, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(254, 21);
            this.label4.TabIndex = 22;
            this.label4.Text = "Логин:";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(13, 95);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(251, 26);
            this.textBoxPassword.TabIndex = 1;
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(10, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 21);
            this.label5.TabIndex = 24;
            this.label5.Text = "Пароль:";
            // 
            // comboBoImapServersNames
            // 
            this.comboBoImapServersNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoImapServersNames.FormattingEnabled = true;
            this.comboBoImapServersNames.Location = new System.Drawing.Point(15, 159);
            this.comboBoImapServersNames.Name = "comboBoImapServersNames";
            this.comboBoImapServersNames.Size = new System.Drawing.Size(250, 28);
            this.comboBoImapServersNames.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(11, 135);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(254, 21);
            this.label6.TabIndex = 26;
            this.label6.Text = "Почтовый сервер (Imap):";
            // 
            // buttonCheckImapConn
            // 
            this.buttonCheckImapConn.Location = new System.Drawing.Point(15, 205);
            this.buttonCheckImapConn.Name = "buttonCheckImapConn";
            this.buttonCheckImapConn.Size = new System.Drawing.Size(250, 35);
            this.buttonCheckImapConn.TabIndex = 4;
            this.buttonCheckImapConn.Text = "Проверить подключение";
            this.buttonCheckImapConn.UseVisualStyleBackColor = true;
            this.buttonCheckImapConn.Click += new System.EventHandler(this.ButtonCheckImapConn_Click);
            // 
            // buttonAddClient
            // 
            this.buttonAddClient.Location = new System.Drawing.Point(42, 253);
            this.buttonAddClient.Name = "buttonAddClient";
            this.buttonAddClient.Size = new System.Drawing.Size(194, 35);
            this.buttonAddClient.TabIndex = 5;
            this.buttonAddClient.Text = "Добавить";
            this.buttonAddClient.UseVisualStyleBackColor = true;
            this.buttonAddClient.Click += new System.EventHandler(this.ButtonAddClient_Click);
            // 
            // checkBoxHidePass
            // 
            this.checkBoxHidePass.AutoSize = true;
            this.checkBoxHidePass.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxHidePass.Checked = true;
            this.checkBoxHidePass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHidePass.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxHidePass.Location = new System.Drawing.Point(138, 74);
            this.checkBoxHidePass.Name = "checkBoxHidePass";
            this.checkBoxHidePass.Size = new System.Drawing.Size(126, 21);
            this.checkBoxHidePass.TabIndex = 29;
            this.checkBoxHidePass.Text = "Скрыть пароль";
            this.checkBoxHidePass.UseVisualStyleBackColor = true;
            this.checkBoxHidePass.CheckedChanged += new System.EventHandler(this.CheckBoxHidePass_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.checkBoxHidePass);
            this.panel1.Controls.Add(this.buttonAddClient);
            this.panel1.Controls.Add(this.buttonCheckImapConn);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.comboBoImapServersNames);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.textBoxPassword);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.textBoxLogin);
            this.panel1.Location = new System.Drawing.Point(16, 102);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(280, 304);
            this.panel1.TabIndex = 18;
            // 
            // listBoxImapClients
            // 
            this.listBoxImapClients.FormattingEnabled = true;
            this.listBoxImapClients.ItemHeight = 20;
            this.listBoxImapClients.Location = new System.Drawing.Point(318, 102);
            this.listBoxImapClients.Name = "listBoxImapClients";
            this.listBoxImapClients.Size = new System.Drawing.Size(191, 304);
            this.listBoxImapClients.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(284, 21);
            this.label2.TabIndex = 20;
            this.label2.Text = "Форма добавления клиентов";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(314, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(195, 21);
            this.label3.TabIndex = 21;
            this.label3.Text = "Лист клиентов";
            // 
            // GetImapClientsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 419);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBoxImapClients);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Name = "GetImapClientsForm";
            this.Text = "Добавление клиентов для подключения к почте";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxLogin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoImapServersNames;
        private System.Windows.Forms.Label label6;
        internal System.Windows.Forms.Button buttonCheckImapConn;
        internal System.Windows.Forms.Button buttonAddClient;
        private System.Windows.Forms.CheckBox checkBoxHidePass;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listBoxImapClients;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}