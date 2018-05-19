using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.OMS.AtivacaoPlanoCliente;
using Gradual.OMS.AtivacaoPlanoCliente.Lib;

namespace Gradual.OMS.AtivacaoPlanoCliente.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAtivar_Click(object sender, EventArgs e)
        {
            Gradual.OMS.AtivacaoPlanoCliente.ServicoAtivacaoPlanoCliente lServico = new Gradual.OMS.AtivacaoPlanoCliente.ServicoAtivacaoPlanoCliente();

            GerarArquivoRequest lRequest = new GerarArquivoRequest();

            lRequest.StSituacao = 'A';

            //lServico.GerarArquivo(lRequest);
            
            lServico.GerarArquivoTravelCard(lRequest);

            //lServico.VerificaPosicaoClienteSinacor(null,true);
            
            

            //lServico.VerificaPosicaoClienteSinacor(null, true);//.GerarArquivo(lRequest);

            //lServico.GerarArquivoPeriodico(null, true);
        }
    }
}
