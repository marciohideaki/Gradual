using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.Generico.Dados;
using Gradual.Generico.Geral;
using Gradual.OMS.Monitor.Custodia.Lib;
using Gradual.OMS.Monitor.Custodia.DB;
using Gradual.OMS.Monitor.Custodia.Servico;
using Gradual.OMS.Monitor.Custodia.Lib.Mensageria;
using Gradual.OMS.Library;
using System.Configuration;
using Gradual.OMS.Monitor.Custodia.Lib.Info;

namespace Gradual.OMS.Monitor.Custodia.Teste
{
    public partial class Form1 : Form
    {
        MonitorCustodiaServico lServico = new MonitorCustodiaServico();
        public Form1()
        {
            InitializeComponent();

            //lServico.IniciarServico();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var lRequest = new MonitorCustodiaRequest();

            lRequest.CodigoCliente = int.Parse(textBox1.Text);

            //var lRequest = new MonitorCustodiaRequest();
            //var lRequest = new MonitorCustodiaRequest();

            DateTime time = DateTime.Now;

            var lResponse = lServico.ObterMonitorCustodiaMemoria(lRequest);

            if (lResponse != null)
            {
                double lSeconds = (DateTime.Now - time).TotalSeconds;
                MessageBox.Show("Segundos que demorou -> " + lSeconds);
            }

            lServico.ClearMonitorCustodiaMemoria();

            
        }
    }
}
