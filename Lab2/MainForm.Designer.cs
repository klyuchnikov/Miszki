namespace Lab2
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.brouseInputFileB = new System.Windows.Forms.Button();
            this.lenthBlockTB = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ecbRB = new System.Windows.Forms.RadioButton();
            this.feistelRB = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.roundTB = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.decryptRB = new System.Windows.Forms.RadioButton();
            this.encryptRB = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.subblockTB = new System.Windows.Forms.TextBox();
            this.inputBrowseTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.outputBrowseTB = new System.Windows.Forms.TextBox();
            this.brouseOutputFileB = new System.Windows.Forms.Button();
            this.startB = new System.Windows.Forms.Button();
            this.rb = new System.Windows.Forms.ProgressBar();
            this.passTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(108, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Длина блока";
            // 
            // brouseInputFileB
            // 
            this.brouseInputFileB.Location = new System.Drawing.Point(490, 19);
            this.brouseInputFileB.Name = "brouseInputFileB";
            this.brouseInputFileB.Size = new System.Drawing.Size(75, 23);
            this.brouseInputFileB.TabIndex = 1;
            this.brouseInputFileB.Text = "Обзор";
            this.brouseInputFileB.UseVisualStyleBackColor = true;
            this.brouseInputFileB.Click += new System.EventHandler(this.brouseInputFileB_Click);
            // 
            // lenthBlockTB
            // 
            this.lenthBlockTB.Location = new System.Drawing.Point(108, 37);
            this.lenthBlockTB.Name = "lenthBlockTB";
            this.lenthBlockTB.Size = new System.Drawing.Size(73, 20);
            this.lenthBlockTB.TabIndex = 2;
            this.lenthBlockTB.Text = "10";
            this.lenthBlockTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.onlyDigit_KeyPress);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ecbRB);
            this.groupBox1.Controls.Add(this.feistelRB);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(148, 69);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Режим блочного шифра";
            // 
            // ecbRB
            // 
            this.ecbRB.AutoSize = true;
            this.ecbRB.Location = new System.Drawing.Point(6, 42);
            this.ecbRB.Name = "ecbRB";
            this.ecbRB.Size = new System.Drawing.Size(46, 17);
            this.ecbRB.TabIndex = 1;
            this.ecbRB.TabStop = true;
            this.ecbRB.Text = "ECB";
            this.ecbRB.UseVisualStyleBackColor = true;
            // 
            // feistelRB
            // 
            this.feistelRB.AutoSize = true;
            this.feistelRB.Checked = true;
            this.feistelRB.Location = new System.Drawing.Point(6, 19);
            this.feistelRB.Name = "feistelRB";
            this.feistelRB.Size = new System.Drawing.Size(104, 17);
            this.feistelRB.TabIndex = 0;
            this.feistelRB.TabStop = true;
            this.feistelRB.Text = "Сеть Фейстеля";
            this.feistelRB.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Кол-во иттераций";
            // 
            // roundTB
            // 
            this.roundTB.Location = new System.Drawing.Point(6, 37);
            this.roundTB.Name = "roundTB";
            this.roundTB.Size = new System.Drawing.Size(96, 20);
            this.roundTB.TabIndex = 2;
            this.roundTB.Text = "1";
            this.roundTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.onlyDigit_KeyPress);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.decryptRB);
            this.groupBox2.Controls.Add(this.encryptRB);
            this.groupBox2.Location = new System.Drawing.Point(166, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(118, 69);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Режим работы";
            // 
            // decryptRB
            // 
            this.decryptRB.AutoSize = true;
            this.decryptRB.Location = new System.Drawing.Point(6, 42);
            this.decryptRB.Name = "decryptRB";
            this.decryptRB.Size = new System.Drawing.Size(104, 17);
            this.decryptRB.TabIndex = 1;
            this.decryptRB.TabStop = true;
            this.decryptRB.Text = "Дешифрование";
            this.decryptRB.UseVisualStyleBackColor = true;
            // 
            // encryptRB
            // 
            this.encryptRB.AutoSize = true;
            this.encryptRB.Checked = true;
            this.encryptRB.Location = new System.Drawing.Point(6, 19);
            this.encryptRB.Name = "encryptRB";
            this.encryptRB.Size = new System.Drawing.Size(90, 17);
            this.encryptRB.TabIndex = 0;
            this.encryptRB.TabStop = true;
            this.encryptRB.Text = "Шифрование";
            this.encryptRB.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.subblockTB);
            this.groupBox3.Controls.Add(this.lenthBlockTB);
            this.groupBox3.Controls.Add(this.roundTB);
            this.groupBox3.Location = new System.Drawing.Point(290, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(293, 69);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Настройки";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(187, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Число подблоков";
            // 
            // subblockTB
            // 
            this.subblockTB.Location = new System.Drawing.Point(187, 37);
            this.subblockTB.Name = "subblockTB";
            this.subblockTB.Size = new System.Drawing.Size(96, 20);
            this.subblockTB.TabIndex = 2;
            this.subblockTB.TextChanged += new System.EventHandler(this.subblockTB_TextChanged);
            this.subblockTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.onlyDigit_KeyPress);
            // 
            // inputBrowseTB
            // 
            this.inputBrowseTB.Location = new System.Drawing.Point(46, 19);
            this.inputBrowseTB.Name = "inputBrowseTB";
            this.inputBrowseTB.Size = new System.Drawing.Size(438, 20);
            this.inputBrowseTB.TabIndex = 2;
            this.inputBrowseTB.Text = "D:\\Documents\\расписание весенний семестр 2011.rar";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Путь";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.inputBrowseTB);
            this.groupBox4.Controls.Add(this.brouseInputFileB);
            this.groupBox4.Location = new System.Drawing.Point(12, 87);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(571, 53);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Исходный файл";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.outputBrowseTB);
            this.groupBox5.Controls.Add(this.brouseOutputFileB);
            this.groupBox5.Location = new System.Drawing.Point(12, 146);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(571, 53);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Выходной файл";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Путь";
            // 
            // outputBrowseTB
            // 
            this.outputBrowseTB.Location = new System.Drawing.Point(46, 19);
            this.outputBrowseTB.Name = "outputBrowseTB";
            this.outputBrowseTB.Size = new System.Drawing.Size(438, 20);
            this.outputBrowseTB.TabIndex = 2;
            this.outputBrowseTB.Text = "D:\\Documents\\outqq.rar";
            // 
            // brouseOutputFileB
            // 
            this.brouseOutputFileB.Location = new System.Drawing.Point(490, 17);
            this.brouseOutputFileB.Name = "brouseOutputFileB";
            this.brouseOutputFileB.Size = new System.Drawing.Size(75, 23);
            this.brouseOutputFileB.TabIndex = 1;
            this.brouseOutputFileB.Text = "Обзор";
            this.brouseOutputFileB.UseVisualStyleBackColor = true;
            this.brouseOutputFileB.Click += new System.EventHandler(this.brouseOutputFileB_Click);
            // 
            // startB
            // 
            this.startB.Location = new System.Drawing.Point(490, 11);
            this.startB.Name = "startB";
            this.startB.Size = new System.Drawing.Size(75, 23);
            this.startB.TabIndex = 5;
            this.startB.Text = "Старт";
            this.startB.UseVisualStyleBackColor = true;
            this.startB.Click += new System.EventHandler(this.startB_Click);
            // 
            // rb
            // 
            this.rb.Location = new System.Drawing.Point(164, 11);
            this.rb.Maximum = 1000;
            this.rb.Name = "rb";
            this.rb.Size = new System.Drawing.Size(320, 23);
            this.rb.Step = 1;
            this.rb.TabIndex = 6;
            // 
            // passTB
            // 
            this.passTB.Location = new System.Drawing.Point(58, 13);
            this.passTB.Name = "passTB";
            this.passTB.Size = new System.Drawing.Size(100, 20);
            this.passTB.TabIndex = 7;
            this.passTB.Text = "qwe";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Пароль";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.passTB);
            this.groupBox6.Controls.Add(this.startB);
            this.groupBox6.Controls.Add(this.rb);
            this.groupBox6.Location = new System.Drawing.Point(12, 205);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(571, 40);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 256);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "№2. Блочные шифры";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button brouseInputFileB;
        private System.Windows.Forms.TextBox lenthBlockTB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton ecbRB;
        private System.Windows.Forms.RadioButton feistelRB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox roundTB;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton decryptRB;
        private System.Windows.Forms.RadioButton encryptRB;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox inputBrowseTB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox outputBrowseTB;
        private System.Windows.Forms.Button brouseOutputFileB;
        private System.Windows.Forms.Button startB;
        private System.Windows.Forms.ProgressBar rb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox subblockTB;
        private System.Windows.Forms.TextBox passTB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox6;
    }
}

