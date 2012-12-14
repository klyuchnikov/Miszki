using System;
using System.Collections.Generic;
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
            var bytes = Encoding.UTF8.GetBytes(str);
            var data = new BigInteger(bytes);
            var res = BigIntegerExtensions.GenRSAValues(16);

            var rr = RSAEx.EnCrypt(data, res.Item1, res.Item2);
            var ww = RSAEx.DeCrypt(rr, res.Item3, res.Item4);
            var sss = Encoding.UTF8.GetString(ww.ToByteArray());
        }
    }
}
