using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.OMS.Monitor.Posicao.Servico;

namespace Gradual.OMS.Monitor.Posicao.Teste
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MonitorPosicaoServico lServico = new MonitorPosicaoServico();

            lServico._bKeepRunning = true;
            lServico.StartMonitorPosicao(null);
        }
    }
}
