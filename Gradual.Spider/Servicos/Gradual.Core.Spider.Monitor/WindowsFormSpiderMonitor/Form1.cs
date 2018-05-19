using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.Core.Spider.Monitor.Lib;
using Gradual.OMS.Library.Servicos;




namespace WindowsFormSpiderMonitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IMonitorFix mntFix = Ativador.Get<IMonitorFix>();
            mntFix.DummyFunction();
            string aa = mntFix.GetFixMonitorListJson();
            MessageBox.Show(aa);
        }
    }
}
