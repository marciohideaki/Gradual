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
using Gradual.OMS.InvXX.Fundos.DbLib.ITAUUNIBANCO;
using Gradual.OMS.InvXX.Fundos.ITAUUNIBANCO;

namespace Gradual.OMS.InvXX.Fundos.Teste
{
    public partial class frmImportacaoITAUUNIBANCO : Form
    {
        public frmImportacaoITAUUNIBANCO()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //AplicacaoResgateServico lServico = new AplicacaoResgateServico();
                //CotistaPosicaoItau lServico = new CotistaPosicaoItau();
                //lServico.ThreadAplicacaoResgate();
                //lServico.ThreadMonitorAplicacaoResgate();
                //lServico.ThreadAplicacaoResgate();
                //lServico.ThreadAplicacaoResgate
                //lServico.StartCotistaPosicaoItau()
                //lServico.ThreadImportacaoPosicaoItau();
                var lServico = new AplicacaoResgateServico();
                lServico.ThreadMonitorAplicacaoResgate(this, false);

            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        private void btnTestarFinancial_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        private void btnTestarTD_Click(object sender, EventArgs e)
        {
            try
            {
                WSVendaTD.venda lServico = new WSVendaTD.venda();

                lServico.hdSegurancaValue = new WSVendaTD.hdSeguranca()
                    {
                        strContratoSenha    =  "33R3BBM88",
                        strContratoHash     =  "-8w-5-gAyJRN",
                        strLoginNome = "227WebServ",
                        strLoginSenha = "Serv*%$227", 
                    };


                string message = lServico.VendaVerifCondVenda("02324180804", 3003);

                MessageBox.Show(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }
    }
}
