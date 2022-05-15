namespace MailCollector.Setup
{
    partial class GetTelegramBotTokenForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetTelegramBotTokenForm));
            this.labelInfo = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelSelectServicePath = new System.Windows.Forms.Panel();
            this.buttonConfirmToken = new System.Windows.Forms.Button();
            this.buttonPaste = new System.Windows.Forms.Button();
            this.textBoxToken = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelSelectServicePath.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelInfo
            // 
            this.labelInfo.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelInfo.Location = new System.Drawing.Point(12, 11);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(412, 69);
            this.labelInfo.TabIndex = 19;
            this.labelInfo.Text = "Выберите папку установки (надписи меняются в зависимости от опций в конструкторе)" +
    "";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(424, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(90, 90);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            // 
            // panelSelectServicePath
            // 
            this.panelSelectServicePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSelectServicePath.Controls.Add(this.buttonConfirmToken);
            this.panelSelectServicePath.Controls.Add(this.buttonPaste);
            this.panelSelectServicePath.Controls.Add(this.textBoxToken);
            this.panelSelectServicePath.Controls.Add(this.label1);
            this.panelSelectServicePath.Location = new System.Drawing.Point(12, 213);
            this.panelSelectServicePath.Name = "panelSelectServicePath";
            this.panelSelectServicePath.Size = new System.Drawing.Size(510, 118);
            this.panelSelectServicePath.TabIndex = 20;
            // 
            // buttonConfirmToken
            // 
            this.buttonConfirmToken.Location = new System.Drawing.Point(144, 77);
            this.buttonConfirmToken.Name = "buttonConfirmToken";
            this.buttonConfirmToken.Size = new System.Drawing.Size(218, 32);
            this.buttonConfirmToken.TabIndex = 22;
            this.buttonConfirmToken.Text = "Подтвердить токен";
            this.buttonConfirmToken.UseVisualStyleBackColor = true;
            this.buttonConfirmToken.Click += new System.EventHandler(this.ButtonConfirmToken_Click);
            // 
            // buttonPaste
            // 
            this.buttonPaste.Location = new System.Drawing.Point(348, 5);
            this.buttonPaste.Name = "buttonPaste";
            this.buttonPaste.Size = new System.Drawing.Size(152, 32);
            this.buttonPaste.TabIndex = 21;
            this.buttonPaste.Text = "Вставить";
            this.buttonPaste.UseVisualStyleBackColor = true;
            this.buttonPaste.Click += new System.EventHandler(this.ButtonPaste_Click);
            // 
            // textBoxToken
            // 
            this.textBoxToken.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBoxToken.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxToken.Location = new System.Drawing.Point(10, 44);
            this.textBoxToken.Name = "textBoxToken";
            this.textBoxToken.Size = new System.Drawing.Size(491, 27);
            this.textBoxToken.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(344, 28);
            this.label1.TabIndex = 19;
            this.label1.Text = "Скопируйте токен в это поле:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GetTelegramBotTokenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 511);
            this.Controls.Add(this.panelSelectServicePath);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.pictureBox1);
            this.Name = "GetTelegramBotTokenForm";
            this.Text = "Впишите токен к Telegram-боту";
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            this.Controls.SetChildIndex(this.labelInfo, 0);
            this.Controls.SetChildIndex(this.panelSelectServicePath, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelSelectServicePath.ResumeLayout(false);
            this.panelSelectServicePath.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelSelectServicePath;
        private System.Windows.Forms.TextBox textBoxToken;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button buttonPaste;
        internal System.Windows.Forms.Button buttonConfirmToken;
    }
}