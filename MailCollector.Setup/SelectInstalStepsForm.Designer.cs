namespace MailCollector.Setup
{
    partial class SelectInstalStepsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectInstalStepsForm));
            this.checkBoxCreateDb = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxInstallService = new System.Windows.Forms.CheckBox();
            this.checkBoxConfigureTgBot = new System.Windows.Forms.CheckBox();
            this.checkBoxInstallClient = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBoxCreateDb
            // 
            this.checkBoxCreateDb.Checked = true;
            this.checkBoxCreateDb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCreateDb.Location = new System.Drawing.Point(21, 141);
            this.checkBoxCreateDb.Name = "checkBoxCreateDb";
            this.checkBoxCreateDb.Size = new System.Drawing.Size(492, 67);
            this.checkBoxCreateDb.TabIndex = 2;
            this.checkBoxCreateDb.Text = "Создание БД \'MailCollectorStorage\' (обязтельно нужна, если не была установлена ра" +
    "нее)";
            this.checkBoxCreateDb.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(424, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(90, 90);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(412, 69);
            this.label1.TabIndex = 15;
            this.label1.Text = "Выберите компоненты программы, которые Вы хотите установить. Нажмите кнопку \'Дале" +
    "е\' для продолжения.";
            // 
            // checkBoxInstallService
            // 
            this.checkBoxInstallService.Checked = true;
            this.checkBoxInstallService.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxInstallService.Location = new System.Drawing.Point(21, 204);
            this.checkBoxInstallService.Name = "checkBoxInstallService";
            this.checkBoxInstallService.Size = new System.Drawing.Size(492, 67);
            this.checkBoxInstallService.TabIndex = 16;
            this.checkBoxInstallService.Text = "Основной сервис \'MailCollector.Service\' (обязательно нужен, если не был установле" +
    "н ранее)";
            this.checkBoxInstallService.UseVisualStyleBackColor = true;
            // 
            // checkBoxConfigureTgBot
            // 
            this.checkBoxConfigureTgBot.Checked = true;
            this.checkBoxConfigureTgBot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxConfigureTgBot.Location = new System.Drawing.Point(21, 261);
            this.checkBoxConfigureTgBot.Name = "checkBoxConfigureTgBot";
            this.checkBoxConfigureTgBot.Size = new System.Drawing.Size(492, 67);
            this.checkBoxConfigureTgBot.TabIndex = 17;
            this.checkBoxConfigureTgBot.Text = "Подключение телеграм-бота для оповещений";
            this.checkBoxConfigureTgBot.UseVisualStyleBackColor = true;
            // 
            // checkBoxInstallClient
            // 
            this.checkBoxInstallClient.Checked = true;
            this.checkBoxInstallClient.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxInstallClient.Location = new System.Drawing.Point(21, 313);
            this.checkBoxInstallClient.Name = "checkBoxInstallClient";
            this.checkBoxInstallClient.Size = new System.Drawing.Size(492, 67);
            this.checkBoxInstallClient.TabIndex = 18;
            this.checkBoxInstallClient.Text = "Клиентская форма для работы и перенастройки сервиса";
            this.checkBoxInstallClient.UseVisualStyleBackColor = true;
            // 
            // SelectInstalStepsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 511);
            this.Controls.Add(this.checkBoxInstallClient);
            this.Controls.Add(this.checkBoxConfigureTgBot);
            this.Controls.Add(this.checkBoxInstallService);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.checkBoxCreateDb);
            this.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.Name = "SelectInstalStepsForm";
            this.Text = "Выбор компонентов для установки";
            this.Controls.SetChildIndex(this.checkBoxCreateDb, 0);
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.checkBoxInstallService, 0);
            this.Controls.SetChildIndex(this.checkBoxConfigureTgBot, 0);
            this.Controls.SetChildIndex(this.checkBoxInstallClient, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxCreateDb;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxInstallService;
        private System.Windows.Forms.CheckBox checkBoxConfigureTgBot;
        private System.Windows.Forms.CheckBox checkBoxInstallClient;
    }
}