using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonLibrary;

namespace Lab1
{
    public partial class MainForm : Form
    {
        private CongruentialGenerator generator = null;
        public MainForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (passTB.Text == "")
            {
                hashTB.Text = "";
                return;
            }
            var hash = MaHash8v64.GetHashCode(passTB.Text);
            hashTB.Text = hash + "";
        }

        private void genB_Click(object sender, EventArgs e)
        {
            if (hashTB.Text == "")
            {
                MessageBox.Show("Задайте пароль!");
                return;
            }
            if (generator == null)
                generator = new CongruentialGenerator(uint.Parse(hashTB.Text));
            try
            {
                ulong res = 0;
                if (minValueTB.Text != "")
                {
                    if (maxValueTB.Text == "")
                    {
                        MessageBox.Show("Задайте максимальное значение");
                        return;
                    }
                    res = generator.Next(ulong.Parse(minValueTB.Text), ulong.Parse(maxValueTB.Text));
                }
                else
                {
                    res = maxValueTB.Text != "" ? generator.Next(ulong.Parse(maxValueTB.Text)) : generator.Next();
                }
                randomTB.Text = res + "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void onlyDigit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar)) return;
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != '-')
                e.Handled = true;
            else if (e.KeyChar == '-')
                if ((sender as TextBox).Text.Length != 0)
                    e.Handled = true;
        }
    }
}
