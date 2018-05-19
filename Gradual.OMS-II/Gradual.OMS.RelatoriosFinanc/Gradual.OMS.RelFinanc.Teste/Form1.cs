using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.OMS.RelatoriosFinanc;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;

namespace Gradual.OMS.RelFinanc.Teste
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCarregaDados_Click(object sender, EventArgs e)
        {
            //ConsultarNCBovespa();
            ConsultarNCBmf();
            //ConsultarFaxBovespa();
            //ConsultarFaxBmf();
            //ConsultarFaxBmfVolatilidade();
            //ConsultarExtratoOrdensBovespa();
            //ConsultarNCBovespa();
            //ConsultarNCBovespa();
            
        }

        private void ConsultarExtratoOrdensBovespa()
        {
            var lRetorno = new ServicoRelatoriosFinanceiros().ObterExtratoOperacoesBovespa(new ExtratoOperacaoRequest()
            {
                ConsultaCodigoCliente = 27887,
                ConsultaDataMovimento = new DateTime(2014, 9, 16, 0, 0, 0),

            });

            if (lRetorno.StatusResposta == Library.MensagemResponseStatusEnum.OK)
            {

            }
        }

        private void ConsultarFaxBmfVolatilidade()
        {
            var lRetorno = new ServicoRelatoriosFinanceiros().ObterFaxBmfVolatilidade(new FaxRequest()
            {
                ConsultaCodigoClienteBmf = 140587,
                ConsultaDataMovimento = new DateTime(2014, 5, 13, 0, 0, 0),
            });

            if (lRetorno.StatusResposta == Library.MensagemResponseStatusEnum.OK)
            {

            }
        }
        private void ConsultarFaxBmf()
        {
            var lRetorno = new ServicoRelatoriosFinanceiros().ObterFaxBmf(new FaxRequest()
                {
                    ConsultaCodigoClienteBmf = 31217,
                    ConsultaDataMovimento = new DateTime(2014, 5, 8, 0, 0, 0),
                });

            if (lRetorno.StatusResposta == Library.MensagemResponseStatusEnum.OK)
            {

            }
        }

        private void ConsultarFaxBovespa()
        {
            var lRetorno = new ServicoRelatoriosFinanceiros().ObterFaxBovespa(new FaxRequest()
            {
                ConsultaCodigoCliente = 6134,
                ConsultaDataMovimento = new DateTime(2015, 3, 6, 0, 0, 0),

            });

            if (lRetorno.StatusResposta == Library.MensagemResponseStatusEnum.OK)
            {

            }
        }

        private void ConsultarNCBovespa()
        {
            var lRetorno = new ServicoRelatoriosFinanceiros().ConsultarNotaDeCorretagem(new NotaDeCorretagemExtratoRequest()
            {
                ConsultaCodigoCliente = 51452,
                ConsultaDataMovimento = new DateTime(2015, 02, 3, 0, 0, 0),
                ConsultaTipoDeMercado = "VIS",
                ConsultaCodigoCorretora = 271
            });

            if (lRetorno.StatusResposta == Library.MensagemResponseStatusEnum.OK)
            {

            }
        }
        private void ConsultarNCBmf()
        {
            var lRetorno = new ServicoRelatoriosFinanceiros().ConsultarNotaDeCorretagemBmf(new NotaDeCorretagemExtratoRequest()
            {
                ConsultaCodigoCliente = 54843 ,
                ConsultaDataMovimento = new DateTime(2015, 3, 25, 0, 0, 0),
                ConsultaTipoDeMercado = "VIS",
                ConsultaCodigoCorretora = 2271,
                ConsultaCodigoClienteBmf = 54843

            });

            if (lRetorno.StatusResposta == Library.MensagemResponseStatusEnum.OK)
            {

            }
        }
    }
}
