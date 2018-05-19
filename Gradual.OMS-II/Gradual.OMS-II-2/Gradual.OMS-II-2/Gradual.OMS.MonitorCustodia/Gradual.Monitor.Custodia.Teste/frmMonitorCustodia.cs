using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.Monitor.Custodia;
using System.Threading;
using System.ServiceModel;
using Gradual.Monitor.Custodia.Lib.Mensageria;

namespace Gradual.Monitor.Custodia.Teste
{
    public partial class frmMonitorCustodia : Form
    {

        MonitorCustodiaServico lServico = new MonitorCustodiaServico();

        public frmMonitorCustodia()
        {
            InitializeComponent();
            
            lServico._bKeepRunning = true;
            lServico.StartMonitor(null);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                MonitorCustodiaResponse lResponse = lServico.ObterMonitorCustodiaMemoria(new Lib.Mensageria.MonitorCustodiaRequest() { CodigoCliente = int.Parse(txtCliente.Text) });

                if (lResponse.MonitorCustodia != null)
                {
                    MessageBox.Show("Retornou dados", "Retornou dados", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}
