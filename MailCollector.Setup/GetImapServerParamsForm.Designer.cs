namespace MailCollector.Setup
{
    partial class GetImapServerParamsForm
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
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxServer = new System.Windows.Forms.TextBox();
            this.checkBoxUseSsl = new System.Windows.Forms.CheckBox();
            this.numericUpDownPort = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonAddImapServer = new System.Windows.Forms.Button();
            this.buttonCheckImapServerConn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(23, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 21);
            this.label5.TabIndex = 33;
            this.label5.Text = "Порт:";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(20, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(303, 21);
            this.label4.TabIndex = 32;
            this.label4.Text = "IMAP-сервер:";
            // 
            // textBoxServer
            // 
            this.textBoxServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxServer.Location = new System.Drawing.Point(23, 46);
            this.textBoxServer.Name = "textBoxServer";
            this.textBoxServer.Size = new System.Drawing.Size(308, 26);
            this.textBoxServer.TabIndex = 30;
            // 
            // checkBoxUseSsl
            // 
            this.checkBoxUseSsl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxUseSsl.AutoSize = true;
            this.checkBoxUseSsl.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxUseSsl.Location = new System.Drawing.Point(175, 96);
            this.checkBoxUseSsl.Name = "checkBoxUseSsl";
            this.checkBoxUseSsl.Size = new System.Drawing.Size(156, 25);
            this.checkBoxUseSsl.TabIndex = 34;
            this.checkBoxUseSsl.Text = "SSL-соединение";
            this.checkBoxUseSsl.UseVisualStyleBackColor = true;
            // 
            // numericUpDownPort
            // 
            this.numericUpDownPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownPort.Location = new System.Drawing.Point(78, 95);
            this.numericUpDownPort.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDownPort.Name = "numericUpDownPort";
            this.numericUpDownPort.Size = new System.Drawing.Size(76, 26);
            this.numericUpDownPort.TabIndex = 35;
            this.numericUpDownPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownPort.Value = new decimal(new int[] {
            993,
            0,
            0,
            0});
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.buttonAddImapServer);
            this.panel1.Controls.Add(this.buttonCheckImapServerConn);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.numericUpDownPort);
            this.panel1.Controls.Add(this.textBoxServer);
            this.panel1.Controls.Add(this.checkBoxUseSsl);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(358, 243);
            this.panel1.TabIndex = 36;
            // 
            // buttonAddImapServer
            // 
            this.buttonAddImapServer.Location = new System.Drawing.Point(85, 189);
            this.buttonAddImapServer.Name = "buttonAddImapServer";
            this.buttonAddImapServer.Size = new System.Drawing.Size(194, 35);
            this.buttonAddImapServer.TabIndex = 37;
            this.buttonAddImapServer.Text = "Добавить";
            this.buttonAddImapServer.UseVisualStyleBackColor = true;
            this.buttonAddImapServer.Click += new System.EventHandler(this.ButtonAddImapServer_Click);
            // 
            // buttonCheckImapServerConn
            // 
            this.buttonCheckImapServerConn.Location = new System.Drawing.Point(58, 141);
            this.buttonCheckImapServerConn.Name = "buttonCheckImapServerConn";
            this.buttonCheckImapServerConn.Size = new System.Drawing.Size(250, 35);
            this.buttonCheckImapServerConn.TabIndex = 36;
            this.buttonCheckImapServerConn.Text = "Проверить подключение";
            this.buttonCheckImapServerConn.UseVisualStyleBackColor = true;
            this.buttonCheckImapServerConn.Click += new System.EventHandler(this.ButtonCheckImapServerConn_Click);
            // 
            // GetImapServerParamsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 267);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.Name = "GetImapServerParamsForm";
            this.Text = "Добавление постового сервера";
            this.Controls.SetChildIndex(this.panel1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxServer;
        private System.Windows.Forms.CheckBox checkBoxUseSsl;
        private System.Windows.Forms.NumericUpDown numericUpDownPort;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Button buttonAddImapServer;
        internal System.Windows.Forms.Button buttonCheckImapServerConn;
    }
}