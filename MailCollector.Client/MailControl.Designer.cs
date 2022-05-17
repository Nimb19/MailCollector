namespace MailCollector.Client
{
    partial class MailControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.ClickPanel = new System.Windows.Forms.Panel();
            this.labelDate = new System.Windows.Forms.Label();
            this.labelSubject = new System.Windows.Forms.Label();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelFrom = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ClickPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ClickPanel
            // 
            this.ClickPanel.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClickPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ClickPanel.Controls.Add(this.labelDate);
            this.ClickPanel.Controls.Add(this.labelSubject);
            this.ClickPanel.Controls.Add(this.labelTo);
            this.ClickPanel.Controls.Add(this.labelFrom);
            this.ClickPanel.Controls.Add(this.label4);
            this.ClickPanel.Controls.Add(this.label3);
            this.ClickPanel.Controls.Add(this.label2);
            this.ClickPanel.Controls.Add(this.label1);
            this.ClickPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClickPanel.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ClickPanel.Location = new System.Drawing.Point(0, 0);
            this.ClickPanel.Margin = new System.Windows.Forms.Padding(4);
            this.ClickPanel.Name = "ClickPanel";
            this.ClickPanel.Size = new System.Drawing.Size(418, 88);
            this.ClickPanel.TabIndex = 0;
            this.ClickPanel.MouseEnter += new System.EventHandler(this.Panel_MouseEnter);
            this.ClickPanel.MouseLeave += new System.EventHandler(this.Panel_MouseLeave);
            // 
            // labelDate
            // 
            this.labelDate.AutoSize = true;
            this.labelDate.Location = new System.Drawing.Point(70, 60);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(0, 20);
            this.labelDate.TabIndex = 7;
            this.labelDate.MouseEnter += new System.EventHandler(this.Panel_MouseEnter);
            this.labelDate.MouseLeave += new System.EventHandler(this.Panel_MouseLeave);
            // 
            // labelSubject
            // 
            this.labelSubject.AutoSize = true;
            this.labelSubject.Location = new System.Drawing.Point(70, 40);
            this.labelSubject.Name = "labelSubject";
            this.labelSubject.Size = new System.Drawing.Size(0, 20);
            this.labelSubject.TabIndex = 6;
            this.labelSubject.MouseEnter += new System.EventHandler(this.Panel_MouseEnter);
            this.labelSubject.MouseLeave += new System.EventHandler(this.Panel_MouseLeave);
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(70, 20);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(0, 20);
            this.labelTo.TabIndex = 5;
            this.labelTo.MouseEnter += new System.EventHandler(this.Panel_MouseEnter);
            this.labelTo.MouseLeave += new System.EventHandler(this.Panel_MouseLeave);
            // 
            // labelFrom
            // 
            this.labelFrom.AutoSize = true;
            this.labelFrom.Location = new System.Drawing.Point(70, 0);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(0, 20);
            this.labelFrom.TabIndex = 4;
            this.labelFrom.MouseEnter += new System.EventHandler(this.Panel_MouseEnter);
            this.labelFrom.MouseLeave += new System.EventHandler(this.Panel_MouseLeave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "Дата: ";
            this.label4.MouseEnter += new System.EventHandler(this.Panel_MouseEnter);
            this.label4.MouseLeave += new System.EventHandler(this.Panel_MouseLeave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Тема: ";
            this.label3.MouseEnter += new System.EventHandler(this.Panel_MouseEnter);
            this.label3.MouseLeave += new System.EventHandler(this.Panel_MouseLeave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Кому: ";
            this.label2.MouseEnter += new System.EventHandler(this.Panel_MouseEnter);
            this.label2.MouseLeave += new System.EventHandler(this.Panel_MouseLeave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "От: ";
            this.label1.MouseEnter += new System.EventHandler(this.Panel_MouseEnter);
            this.label1.MouseLeave += new System.EventHandler(this.Panel_MouseLeave);
            // 
            // MailControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.ClickPanel);
            this.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MailControl";
            this.Size = new System.Drawing.Size(418, 88);
            this.ClickPanel.ResumeLayout(false);
            this.ClickPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel ClickPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.Label labelSubject;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Label labelFrom;
    }
}
