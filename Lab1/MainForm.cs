using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Lab1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            var hash = CommonLibrary.MaHash8v64.GetHashCode("привет");
            var gen = new CommonLibrary.CongruentialGenerator(hash);
            var res = gen.Next(255);

        }
    }
}
