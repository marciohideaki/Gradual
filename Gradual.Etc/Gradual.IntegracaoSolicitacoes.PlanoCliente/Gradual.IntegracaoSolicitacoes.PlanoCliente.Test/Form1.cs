#region Includes
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.IntegracaoSolicitacoes.PlanoCliente;
using Gradual.IntegracaoSolicitacoes.PlanoCliente.Lib;
#endregion


namespace Gradual.IntegracaoSolicitacoes.PlanoCliente.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnTestar_Click(object sender, EventArgs e)
        {
            try
            {
                ServicoIntegracaoSolicitacoes lServico = new ServicoIntegracaoSolicitacoes();

                //GerarArquivoRequest lRequest = new GerarArquivoRequest();

                //lRequest.StAtivo = 'S';

                lServico.EnviarEmailAviso();// GerarArquivo(lRequest);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }
    }
}
