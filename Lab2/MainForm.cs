using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonLibrary;
using System.IO;

namespace Lab2
{
    public partial class MainForm : Form
    {
        private CrypterBlocks crypter;
        private Timer timer;
        private DateTime oldTime;

        public MainForm()
        {
            InitializeComponent();
            timer = new Timer { Interval = 10 };
            timer.Tick += new EventHandler(timer_Tick);
            rb.Minimum = 0;
            rb.Maximum = 1000;
        }

        public MainForm(string[] args)
            : this()
        {
            passTB.Text = args[0];
            encryptRB.Checked = args[1] == "1";
            decryptRB.Checked = args[1] == "2";
            inputBrowseTB.Text = args[2];
            outputBrowseTB.Text = args[3];
            roundTB.Text = 3 + "";
            lenthBlockTB.Text = 10 + "";
            subblockTB.Text = 3 + "";
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (crypter.MaxValueProcess == 0)
                return;
            var diff = DateTime.Now - oldTime;
            SpeedLabel.Text = string.Format("Скорость: {0:N} симв/сек", (crypter.CurrentValueProcess / diff.TotalSeconds));
            rb.Value = (int)((crypter.CurrentValueProcess / (double)crypter.MaxValueProcess) * 1000);

        }

        private void onlyDigit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar)) return;
            if (e.KeyChar != (char)Keys.Back)
                e.Handled = true;
        }

        private void onlyByte_TextChange(object sender, EventArgs e)
        {
            var textbox = sender as TextBox;
            byte res;
            if (!byte.TryParse(textbox.Text, out res) && textbox.Text != "")
            {
                MessageBox.Show("Значение должно находиться в диапазоне от 0 до 254.");
                textbox.Text = "";
                return;
            }
        }

        private void brouseInputFileB_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            inputBrowseTB.Text = openFileDialog.FileName;
        }

        private void brouseOutputFileB_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            outputBrowseTB.Text = saveFileDialog.FileName;
        }

        private void subblockTB_TextChanged(object sender, EventArgs e)
        {
            onlyByte_TextChange(sender, e);
            if (subblockTB.Text == "")
                return;
            if (lenthBlockTB.Text == "")
            {
                MessageBox.Show("Задайте сначала длину блока.");
                subblockTB.Text = "";
                return;
            }
            if (int.Parse(subblockTB.Text) <= int.Parse(lenthBlockTB.Text)) return;
            MessageBox.Show("Количество подблоков не может быть больше поливины длины блока.");
            subblockTB.Text = "";
        }

        private void startB_Click(object sender, EventArgs e)
        {
            if (passTB.Text == "")
            {
                MessageBox.Show("Задайте пароль.");
                return;
            }
            if (subblockTB.Text == "")
            {
                MessageBox.Show("Задайте количество подблоков.");
                return;
            }
            if (lenthBlockTB.Text == "")
            {
                MessageBox.Show("Задайте длину блока.");
                return;
            }
            if (lenthBlockTB.Text == "")
            {
                MessageBox.Show("Задайте колличество иттераций.");
                return;
            }
            if (inputBrowseTB.Text == "")
            {
                MessageBox.Show("Задайте входной файл.");
                return;
            }
            if (outputBrowseTB.Text == "")
            {
                MessageBox.Show("Задайте выходной файл.");
                return;
            }
            var lenthBlock = byte.Parse(lenthBlockTB.Text);
            var round = byte.Parse(roundTB.Text);
            var subblocks = byte.Parse(subblockTB.Text);
            if (ecbRB.Checked)
                crypter = new ECB(lenthBlock, subblocks, passTB.Text);
            else
                crypter = new FeistelNetwork(lenthBlock, subblocks, passTB.Text, round);

            timer.Stop();
            timer.Start();

            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;
            groupBox5.Enabled = false;
            groupBox6.Enabled = false;
            oldTime = DateTime.Now;
            crypter.DecryptComplitedEvent += crypter_DecryptComplitedEvent;
            crypter.EncryptComplitedEvent += crypter_DecryptComplitedEvent;
            if (encryptRB.Checked)
                crypter.EncryptAsync(inputBrowseTB.Text, outputBrowseTB.Text);
            else if (decryptRB.Checked)
                crypter.DecryptAsync(inputBrowseTB.Text, outputBrowseTB.Text);


        }

        void crypter_DecryptComplitedEvent(CryptResult cryptResult, string password)
        {
            if (cryptResult == CryptResult.MismatchValueParameters)
                MessageBox.Show("Заданные параметры дешифрации не соответствуют параметрам при шифровании", "Ошибка");
            timer.Stop();
            rb.Maximum = 1000;
            rb.Minimum = 0;
            rb.Value = rb.Maximum;
            if (cryptResult == CryptResult.Success)
                SpeedLabel.Text = string.Format("Время выполнения: {0} секунд, {1:N} символов, средняя скорость: {2:N} симв/сек",
                    (DateTime.Now - oldTime).TotalSeconds, crypter.MaxValueProcess, crypter.MaxValueProcess / (DateTime.Now - oldTime).TotalSeconds);
            else if (cryptResult == CryptResult.MismatchValueParameters)
                SpeedLabel.Text = "Ошибка. Заданные параметры дешифрации не соответствуют параметрам при шифровании!";
            else if (cryptResult == CryptResult.MismatchСhecksum)
                SpeedLabel.Text = "Ошибка. Контрольная сумма не совпадает!";

            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            groupBox4.Enabled = true;
            groupBox5.Enabled = true;
            groupBox6.Enabled = true;
        }

        private void ecbRB_CheckedChanged(object sender, EventArgs e)
        {
            label2.Enabled = !ecbRB.Checked;
            roundTB.Enabled = !ecbRB.Checked;
        }

        private void ExistsFile_TextChanged(object sender, EventArgs e)
        {
            if (!File.Exists((sender as TextBox).Text))
            {
                MessageBox.Show("По данному пути файла не существует. Задайте другой путь.");
                (sender as TextBox).Text = "";
                return;
            }
        }
    }
}
