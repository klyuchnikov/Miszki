namespace Lab1
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
            this.passTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.genB = new System.Windows.Forms.Button();
            this.hashTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.randomTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.maxValueTB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.minValueTB = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // passTB
            // 
            this.passTB.Location = new System.Drawing.Point(6, 25);
            this.passTB.Name = "passTB";
            this.passTB.Size = new System.Drawing.Size(113, 20);
            this.passTB.TabIndex = 0;
            this.passTB.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Введите ключ-пароль";
            // 
            // genB
            // 
            this.genB.Location = new System.Drawing.Point(125, 22);
            this.genB.Name = "genB";
            this.genB.Size = new System.Drawing.Size(114, 23);
            this.genB.TabIndex = 2;
            this.genB.Text = "Сгенерировать";
            this.genB.UseVisualStyleBackColor = true;
            this.genB.Click += new System.EventHandler(this.genB_Click);
            // 
            // hashTB
            // 
            this.hashTB.Location = new System.Drawing.Point(6, 64);
            this.hashTB.Name = "hashTB";
            this.hashTB.ReadOnly = true;
            this.hashTB.Size = new System.Drawing.Size(113, 20);
            this.hashTB.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Хеш-значение";
            // 
            // randomTB
            // 
            this.randomTB.Location = new System.Drawing.Point(126, 64);
            this.randomTB.Name = "randomTB";
            this.randomTB.ReadOnly = true;
            this.randomTB.Size = new System.Drawing.Size(113, 20);
            this.randomTB.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(243, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Максимальное значение";
            // 
            // maxValueTB
            // 
            this.maxValueTB.Location = new System.Drawing.Point(246, 25);
            this.maxValueTB.Name = "maxValueTB";
            this.maxValueTB.Size = new System.Drawing.Size(131, 20);
            this.maxValueTB.TabIndex = 0;
            this.maxValueTB.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.maxValueTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.onlyDigit_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(243, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Минимальное значение";
            // 
            // minValueTB
            // 
            this.minValueTB.Location = new System.Drawing.Point(246, 63);
            this.minValueTB.Name = "minValueTB";
            this.minValueTB.Size = new System.Drawing.Size(131, 20);
            this.minValueTB.TabIndex = 0;
            this.minValueTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.onlyDigit_KeyPress);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 95);
            this.Controls.Add(this.randomTB);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.hashTB);
            this.Controls.Add(this.genB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.minValueTB);
            this.Controls.Add(this.maxValueTB);
            this.Controls.Add(this.passTB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "№1. Хеширование паролей. Генераторы случайных чисел";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox passTB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button genB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox randomTB;
        private System.Windows.Forms.TextBox hashTB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox maxValueTB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox minValueTB;
    }
}

