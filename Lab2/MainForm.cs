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
        private FeistelNetwork crypter;
        private Timer timer;

        public MainForm()
        {
            InitializeComponent();
            timer = new Timer { Interval = 100 };
            timer.Tick += new EventHandler(timer_Tick);

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            rb.Maximum = crypter.MaxValueProcess;
            rb.Minimum = 0;
            rb.Value = crypter.CurrentValueProcess;
            if (crypter.CurrentValueProcess == crypter.MaxValueProcess)
                timer.Stop();
        }

        private void onlyDigit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar)) return;
            if (e.KeyChar != (char)Keys.Back)
                e.Handled = true;
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
            if (subblockTB.Text == "" || lenthBlockTB.Text == "")
                return;
            if (int.Parse(subblockTB.Text) <= int.Parse(lenthBlockTB.Text)) return;
            MessageBox.Show("Количество подблоков не может быть больше длины блока.");
            subblockTB.Text = "";
        }

        private void startB_Click(object sender, EventArgs e)
        {
            if (passTB.Text == "")
            {
                MessageBox.Show("Задайте пароль.");
                return;
            }
            var lenthBlock = byte.Parse(lenthBlockTB.Text);
            var round = byte.Parse(roundTB.Text);
            crypter = new FeistelNetwork(lenthBlock, round);

            timer.Start();
            crypter.Encrypt(inputBrowseTB.Text, outputBrowseTB.Text, passTB.Text);


        }
    }
}
