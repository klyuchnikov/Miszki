using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
using System.IO;
using System.ComponentModel;
using System.Threading;
using Microsoft.Win32;

namespace Lab3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var str = "qwe";
            var bytes = Encoding.ASCII.GetBytes(str);
            /*var data = new BigInteger(bytes);
            var res = BigIntegerExtensions.GenRSAValues(16);

            var rr = RSAEx.EnCrypt(data, res.Item1, res.Item2);
            var ww = RSAEx.DeCrypt(rr, res.Item3, res.Item4);
            var sss = Encoding.ASCII.GetString(ww.ToByteArray());*/
            encryptMGrid.Visibility = Visibility.Collapsed;
            encryptPassGrid.Visibility = Visibility.Collapsed;
            decryptPassGrid.Visibility = Visibility.Collapsed;
            decryptMGrid.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.SelectedPath = Environment.CurrentDirectory;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                workFolderTB.Text = dialog.SelectedPath;
        }

        private string SenderPath;
        private string RecipientPath;
        private Tuple<BigInteger, BigInteger, BigInteger, BigInteger> keys;
        private void GenRSAKeys(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(workFolderTB.Text))
            {
                MessageBox.Show("Данной папки не существует!");
                return;
            }
            prog.IsIndeterminate = true;
            GenRSAKeysB.IsEnabled = false;
            SenderPath = workFolderTB.Text + @"\Sender";
            RecipientPath = workFolderTB.Text + @"\Recipient";
            if (Directory.Exists(SenderPath))
                Directory.Delete(SenderPath, true);
            if (Directory.Exists(RecipientPath))
                Directory.Delete(RecipientPath, true);
            Directory.CreateDirectory(SenderPath);
            Directory.CreateDirectory(RecipientPath);
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
                    File.WriteAllLines(RecipientPath + @"\close_key.txt", new string[]
                        {
                            string.Join(" ",keys.Item3.ToByteArray().Select(a => a.ToString("X"))),
                            string.Join(" ",keys.Item4.ToByteArray().Select(a => a.ToString("X")))
                        }); Dispatcher.InvokeAsync(new Action(() =>
                        {
                            prog.IsIndeterminate = false;
                            GenRSAKeysB.IsEnabled = true;
                            encryptPassGrid.Visibility = Visibility.Visible;
                        }));
                };
            bw.RunWorkerAsync();

        }

        private void EncryptPass(object sender, RoutedEventArgs e)
        {
            var bytes = File.ReadAllBytes(pathPassTB.Text);
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
            File.WriteAllLines(RecipientPath + @"\pass.cod", cryptArr.Select(intg => string.Join(" ", intg.ToByteArray().Select(a => a.ToString("X")))));
            decryptPassGrid.Visibility = Visibility.Visible;
        }

        private void DecryptPass(object sender, RoutedEventArgs e)
        {
            var lines = File.ReadAllLines(pathCodPassTB.Text);
            var close_key = File.ReadAllLines(RecipientPath + @"\close_key.txt");
            var E = new BigInteger(close_key[0].Split(' ').Select(a => byte.Parse(a.ToString(), NumberStyles.HexNumber)).ToArray());
            var N = new BigInteger(close_key[1].Split(' ').Select(a => byte.Parse(a.ToString(), NumberStyles.HexNumber)).ToArray());
            var cryptArr = new BigInteger[lines.Length];
            for (int i = 0, k = 0; i < lines.Length; i++)
            {
                cryptArr[k++] = RSAEx.DeCrypt(new BigInteger(lines[i].Split(' ').Select(a => byte.Parse(a.ToString(), NumberStyles.HexNumber)).ToArray()), E, N);
            }
            var bytes = cryptArr.SelectMany(a => a.ToByteArray()).ToArray();
            File.WriteAllBytes(RecipientPath + @"\pass.txt", bytes);
            encryptMGrid.Visibility = Visibility.Visible;
            ;
        }

        private void DecryptFile(object sender, RoutedEventArgs e)
        {
            Process.Start(workFolderTB.Text + @"\Lab2.exe",
                string.Format("{0} {1} {2} {3}", File.ReadAllText(pathPassTB.Text), 2, RecipientPath + @"\message.cod", RecipientPath + @"\message.txt"));
        }

        private void EncryptFile(object sender, RoutedEventArgs e)
        {
            Process.Start(workFolderTB.Text + @"\Lab2.exe",
               string.Format("{0} {1} {2} {3}", File.ReadAllText(pathPassTB.Text), 1, SenderPath + @"\message.txt", RecipientPath + @"\message.cod"));
            decryptMGrid.Visibility = Visibility.Visible;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { InitialDirectory = Environment.CurrentDirectory };
            if (dialog.ShowDialog() == true)
                pathPassTB.Text = dialog.FileName;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { InitialDirectory = Environment.CurrentDirectory };
            if (dialog.ShowDialog() == true)
                pathCodPassTB.Text = dialog.FileName;
        }
    }
}
