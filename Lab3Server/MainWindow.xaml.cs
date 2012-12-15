using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
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

namespace Lab3Server
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Server.Current.Start(4444);
        }


        private void file_browse(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { CheckFileExists = false, CheckPathExists = false, InitialDirectory = Environment.CurrentDirectory };
            if (dialog.ShowDialog() == true)
                filePath.Text = dialog.FileName;
        }

        private void filePathEncrypt_browse(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { CheckFileExists = false, CheckPathExists = false, InitialDirectory = Environment.CurrentDirectory };
            if (dialog.ShowDialog() == true)
                filePathEncrypt.Text = dialog.FileName;
        }
        private void filePathOpen_browse(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { CheckFileExists = false, CheckPathExists = false, InitialDirectory = Environment.CurrentDirectory };
            if (dialog.ShowDialog() == true)
                filePathOpen.Text = dialog.FileName;
        }
        private void GenRSAKeys(object sender, RoutedEventArgs e)
        {
            prog.IsIndeterminate = true;
            GenRSAKeysB.IsEnabled = false;
            SenderPath = Environment.CurrentDirectory + @"\Sender";
            if (Directory.Exists(SenderPath))
                Directory.Delete(SenderPath, true);
            Directory.CreateDirectory(SenderPath);
            var bw = new BackgroundWorker();
            bw.DoWork += delegate
            {
                var keys = BigIntegerExtensions.GenRSAValues(16);
                this.keys = keys;
                File.WriteAllLines(SenderPath + @"\open_key.txt", new string[]
                        {
                            string.Join(" ",keys.Item1.ToByteArray().Select(a => a.ToString("X"))),
                            string.Join(" ",keys.Item2.ToByteArray().Select(a => a.ToString("X")))
                        });
                File.WriteAllLines(SenderPath + @"\close_key.txt", new string[]
                        {
                            string.Join(" ",keys.Item3.ToByteArray().Select(a => a.ToString("X"))),
                            string.Join(" ",keys.Item4.ToByteArray().Select(a => a.ToString("X")))
                        }); Dispatcher.InvokeAsync(new Action(() =>
                            {
                                MessageBox.Show("Открытый ключ - open_key.txt, Закрытый ключ - close_key.txt");
                                prog.IsIndeterminate = false;
                                GenRSAKeysB.IsEnabled = true;
                                SendCloseKeyB.IsEnabled = true;
                            }));
            };
            bw.RunWorkerAsync();
        }

        private void SendCloseKey(object sender, RoutedEventArgs e)
        {
            var data = new List<byte> { 2 };
            data.AddRange(File.ReadAllBytes(SenderPath + @"\close_key.txt"));
            foreach (var clientConnection in Server.Current.ListConnection)
            {
                clientConnection.SendBytes(data.ToArray());
            }
        }

        public Tuple<BigInteger, BigInteger, BigInteger, BigInteger> keys { get; set; }

        public string SenderPath { get; set; }

        private void encryptFile(object sender, RoutedEventArgs e)
        {
            var bytes = File.ReadAllBytes(filePathOpen.Text);
            var open_key = File.ReadAllLines(SenderPath + @"\open_key.txt");
            var D = new BigInteger(open_key[0].Split(' ').Select(a => byte.Parse(a.ToString(), NumberStyles.HexNumber)).ToArray());
            var N = new BigInteger(open_key[1].Split(' ').Select(a => byte.Parse(a.ToString(), NumberStyles.HexNumber)).ToArray());
            var cryptArr = new BigInteger[bytes.Length / N.ToByteArray().Length + 1];
            for (int i = 0, k = 0; i < bytes.Length; i = i)
            {
                var data = bytes.Skip(i).Take(N.ToByteArray().Length).ToArray();
                cryptArr[k++] = RSAEx.EnCrypt(new BigInteger(data), D, N);
                i += data.Length;
            }
            File.WriteAllLines(filePathEncrypt.Text, cryptArr.Select(intg => string.Join(" ", intg.ToByteArray().Select(a => a.ToString("X")))));
            MessageBox.Show("Шифрование завершено");
        }

        private void SendFileToClients(object sender, RoutedEventArgs e)
        {
            var data = new List<byte> { 2 };
            data.AddRange(File.ReadAllBytes(filePath.Text));
            foreach (var clientConnection in Server.Current.ListConnection)
            {
                clientConnection.SendBytes(data.ToArray());
            }
        }
    }
}
