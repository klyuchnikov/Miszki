using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommonLibrary;
using Microsoft.Win32;

namespace Lab3Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Client.Current.MainWindow = this;
        }


        private void browserkey(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { CheckFileExists = false, CheckPathExists = false, InitialDirectory = Environment.CurrentDirectory };
            if (dialog.ShowDialog() == true)
                pathkeyTB.Text = dialog.FileName;
        }

        private void browserFile(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { CheckFileExists = false, CheckPathExists = false, InitialDirectory = Environment.CurrentDirectory };
            if (dialog.ShowDialog() == true)
                pathFileTB.Text = dialog.FileName;
        }

        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            var lines = File.ReadAllLines(pathFileTB.Text);
            var close_key = File.ReadAllLines(pathkeyTB.Text);
            var E = new BigInteger(close_key[0].Split(' ').Select(a => byte.Parse(a.ToString(), NumberStyles.HexNumber)).ToArray());
            var N = new BigInteger(close_key[1].Split(' ').Select(a => byte.Parse(a.ToString(), NumberStyles.HexNumber)).ToArray());
            var cryptArr = new BigInteger[lines.Length];
            for (int i = 0, k = 0; i < lines.Length; i++)
            {
                cryptArr[k++] = RSAEx.DeCrypt(new BigInteger(lines[i].Split(' ').Select(a => byte.Parse(a.ToString(), NumberStyles.HexNumber)).ToArray()), E, N);
            }
            var bytes = cryptArr.SelectMany(a => a.ToByteArray()).ToArray();
            File.WriteAllBytes(pathFiledecTB.Text, bytes);
            MessageBox.Show("Расшифрование завершено!");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (serverAddressTB.Text == "")
            {
                MessageBox.Show("Задайте адрес сервера");
                return;
            }
            DnsEndPoint endPoint;
            try
            {
                endPoint = new DnsEndPoint(serverAddressTB.Text, int.Parse(serverPortTB.Text));
            }
            catch
            {
                MessageBox.Show("Неверный адрес сервера");
                return;
            }
            Client.Current.ConnectAsync(endPoint.Host, endPoint.Port);
        }

        private void browserdecFile(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { CheckFileExists = false, CheckPathExists = false, InitialDirectory = Environment.CurrentDirectory };
            if (dialog.ShowDialog() == true)
                pathFiledecTB.Text = dialog.FileName;
        }
    }
}
