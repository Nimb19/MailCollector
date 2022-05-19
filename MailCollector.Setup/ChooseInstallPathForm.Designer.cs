namespace MailCollector.Setup
{
    partial class ChooseInstallPathForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseInstallPathForm));
            this.labelInfo = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelSelectServicePath = new System.Windows.Forms.Panel();
            this.buttonSelectServicePath = new System.Windows.Forms.Button();
            this.textBoxServicePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelSelectClientPath = new System.Windows.Forms.Panel();
            this.buttonSelectClientPath = new System.Windows.Forms.Button();
            this.textBoxClientPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelSelectServicePath.SuspendLayout();
            this.panelSelectClientPath.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelInfo
            // 
            this.labelInfo.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelInfo.Location = new System.Drawing.Point(12, 11);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(412, 69);
            this.labelInfo.TabIndex = 17;
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
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // panelSelectServicePath
            // 
            this.panelSelectServicePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSelectServicePath.Controls.Add(this.buttonSelectServicePath);
            this.panelSelectServicePath.Controls.Add(this.textBoxServicePath);
            this.panelSelectServicePath.Controls.Add(this.label1);
            this.panelSelectServicePath.Location = new System.Drawing.Point(12, 146);
            this.panelSelectServicePath.Name = "panelSelectServicePath";
            this.panelSelectServicePath.Size = new System.Drawing.Size(510, 85);
            this.panelSelectServicePath.TabIndex = 18;
            // 
            // buttonSelectServicePath
            // 
            this.buttonSelectServicePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelectServicePath.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSelectServicePath.Location = new System.Drawing.Point(454, 29);
            this.buttonSelectServicePath.Name = "buttonSelectServicePath";
            this.buttonSelectServicePath.Size = new System.Drawing.Size(40, 40);
            this.buttonSelectServicePath.TabIndex = 21;
            this.buttonSelectServicePath.Text = "...";
            this.buttonSelectServicePath.UseVisualStyleBackColor = true;
            this.buttonSelectServicePath.Click += new System.EventHandler(this.ButtonSelectServicePath_Click);
            // 
            // textBoxServicePath
            // 
            this.textBoxServicePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxServicePath.Font = new System.Drawing.Font("Corbel", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxServicePath.Location = new System.Drawing.Point(9, 41);
            this.textBoxServicePath.Name = "textBoxServicePath";
            this.textBoxServicePath.Size = new System.Drawing.Size(427, 28);
            this.textBoxServicePath.TabIndex = 20;
            this.textBoxServicePath.Text = "C:\\ProgramData\\MailCollector\\Service";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(427, 28);
            this.label1.TabIndex = 19;
            this.label1.Text = "Расположение сервиса:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelSelectClientPath
            // 
            this.panelSelectClientPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSelectClientPath.Controls.Add(this.buttonSelectClientPath);
            this.panelSelectClientPath.Controls.Add(this.textBoxClientPath);
            this.panelSelectClientPath.Controls.Add(this.label2);
            this.panelSelectClientPath.Location = new System.Drawing.Point(12, 262);
            this.panelSelectClientPath.Name = "panelSelectClientPath";
            this.panelSelectClientPath.Size = new System.Drawing.Size(510, 85);
            this.panelSelectClientPath.TabIndex = 22;
            // 
            // buttonSelectClientPath
            // 
            this.buttonSelectClientPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelectClientPath.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSelectClientPath.Location = new System.Drawing.Point(454, 28);
            this.buttonSelectClientPath.Name = "buttonSelectClientPath";
            this.buttonSelectClientPath.Size = new System.Drawing.Size(40, 40);
            this.buttonSelectClientPath.TabIndex = 21;
            this.buttonSelectClientPath.Text = "...";
            this.buttonSelectClientPath.UseVisualStyleBackColor = true;
            this.buttonSelectClientPath.Click += new System.EventHandler(this.ButtonSelectClientPath_Click);
            // 
            // textBoxClientPath
            // 
            this.textBoxClientPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxClientPath.Font = new System.Drawing.Font("Corbel", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxClientPath.Location = new System.Drawing.Point(9, 40);
            this.textBoxClientPath.Name = "textBoxClientPath";
            this.textBoxClientPath.Size = new System.Drawing.Size(427, 28);
            this.textBoxClientPath.TabIndex = 20;
            this.textBoxClientPath.Text = "C:\\ProgramData\\MailCollector\\Client";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(427, 28);
            this.label2.TabIndex = 19;
            this.label2.Text = "Расположение клиентской части:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChooseInstallPathForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 511);
            this.Controls.Add(this.panelSelectClientPath);
            this.Controls.Add(this.panelSelectServicePath);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ChooseInstallPathForm";
            this.Text = "Выберите путь устаноки";
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            this.Controls.SetChildIndex(this.labelInfo, 0);
            this.Controls.SetChildIndex(this.panelSelectServicePath, 0);
            this.Controls.SetChildIndex(this.panelSelectClientPath, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelSelectServicePath.ResumeLayout(false);
            this.panelSelectServicePath.PerformLayout();
            this.panelSelectClientPath.ResumeLayout(false);
            this.panelSelectClientPath.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelSelectServicePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxServicePath;
        private System.Windows.Forms.Button buttonSelectServicePath;
        private System.Windows.Forms.Panel panelSelectClientPath;
        private System.Windows.Forms.Button buttonSelectClientPath;
        private System.Windows.Forms.TextBox textBoxClientPath;
        private System.Windows.Forms.Label label2;
    }
}