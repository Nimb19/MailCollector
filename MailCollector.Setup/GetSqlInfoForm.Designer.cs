namespace MailCollector.Setup
{
    partial class GetSqlInfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetSqlInfoForm));
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSqlServerConnStr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxLogin = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.buttonCheckSqlConn = new System.Windows.Forms.Button();
            this.checkBoxIntegratedSecurity = new System.Windows.Forms.CheckBox();
            this.labelSqlConnIsSuccess = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(432, 69);
            this.label1.TabIndex = 3;
            this.label1.Text = "Впишите данные для подключения к СУБД. \r\nВ ней будет размещена БД \'MailCollectorS" +
    "torage\', она будет нужна для работы сервиса.";
            // 
            // textBoxSqlServerConnStr
            // 
            this.textBoxSqlServerConnStr.Location = new System.Drawing.Point(48, 130);
            this.textBoxSqlServerConnStr.Name = "textBoxSqlServerConnStr";
            this.textBoxSqlServerConnStr.Size = new System.Drawing.Size(432, 26);
            this.textBoxSqlServerConnStr.TabIndex = 4;
            this.textBoxSqlServerConnStr.Text = "localhost";
            this.textBoxSqlServerConnStr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(4, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(518, 27);
            this.label2.TabIndex = 5;
            this.label2.Text = "Строка подключения к СУБД:";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(4, 206);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(518, 27);
            this.label3.TabIndex = 7;
            this.label3.Text = "Логин:";
            // 
            // textBoxLogin
            // 
            this.textBoxLogin.Enabled = false;
            this.textBoxLogin.Location = new System.Drawing.Point(48, 236);
            this.textBoxLogin.Name = "textBoxLogin";
            this.textBoxLogin.Size = new System.Drawing.Size(432, 26);
            this.textBoxLogin.TabIndex = 6;
            this.textBoxLogin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(4, 276);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(518, 27);
            this.label4.TabIndex = 9;
            this.label4.Text = "Пароль:";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Enabled = false;
            this.textBoxPassword.Location = new System.Drawing.Point(48, 306);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(432, 26);
            this.textBoxPassword.TabIndex = 8;
            this.textBoxPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // buttonCheckSqlConn
            // 
            this.buttonCheckSqlConn.Location = new System.Drawing.Point(89, 358);
            this.buttonCheckSqlConn.Name = "buttonCheckSqlConn";
            this.buttonCheckSqlConn.Size = new System.Drawing.Size(355, 35);
            this.buttonCheckSqlConn.TabIndex = 10;
            this.buttonCheckSqlConn.Text = "Проверить подключение";
            this.buttonCheckSqlConn.UseVisualStyleBackColor = true;
            this.buttonCheckSqlConn.Click += new System.EventHandler(this.ButtonCheckSqlConn_Click);
            // 
            // checkBoxIntegratedSecurity
            // 
            this.checkBoxIntegratedSecurity.AutoSize = true;
            this.checkBoxIntegratedSecurity.Checked = true;
            this.checkBoxIntegratedSecurity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxIntegratedSecurity.Location = new System.Drawing.Point(68, 165);
            this.checkBoxIntegratedSecurity.Name = "checkBoxIntegratedSecurity";
            this.checkBoxIntegratedSecurity.Size = new System.Drawing.Size(401, 24);
            this.checkBoxIntegratedSecurity.TabIndex = 11;
            this.checkBoxIntegratedSecurity.Text = "Подключиться с помощью вашей учётной записи";
            this.checkBoxIntegratedSecurity.UseVisualStyleBackColor = true;
            this.checkBoxIntegratedSecurity.CheckedChanged += new System.EventHandler(this.CheckBoxUseIntegratedSecurity_CheckedChanged);
            // 
            // labelSqlConnIsSuccess
            // 
            this.labelSqlConnIsSuccess.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelSqlConnIsSuccess.ForeColor = System.Drawing.Color.DimGray;
            this.labelSqlConnIsSuccess.Location = new System.Drawing.Point(48, 396);
            this.labelSqlConnIsSuccess.Name = "labelSqlConnIsSuccess";
            this.labelSqlConnIsSuccess.Size = new System.Drawing.Size(432, 34);
            this.labelSqlConnIsSuccess.TabIndex = 12;
            this.labelSqlConnIsSuccess.Text = "Статуст: нет подключения";
            this.labelSqlConnIsSuccess.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(424, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(90, 90);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // GetSqlInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 511);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelSqlConnIsSuccess);
            this.Controls.Add(this.checkBoxIntegratedSecurity);
            this.Controls.Add(this.buttonCheckSqlConn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxLogin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxSqlServerConnStr);
            this.Controls.Add(this.label1);
            this.Name = "GetSqlInfoForm";
            this.Text = "Установщик сервиса MailCollector";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.textBoxSqlServerConnStr, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.textBoxLogin, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.textBoxPassword, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.buttonCheckSqlConn, 0);
            this.Controls.SetChildIndex(this.checkBoxIntegratedSecurity, 0);
            this.Controls.SetChildIndex(this.labelSqlConnIsSuccess, 0);
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSqlServerConnStr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxLogin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxPassword;
        internal System.Windows.Forms.Button buttonCheckSqlConn;
        private System.Windows.Forms.CheckBox checkBoxIntegratedSecurity;
        private System.Windows.Forms.Label labelSqlConnIsSuccess;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}