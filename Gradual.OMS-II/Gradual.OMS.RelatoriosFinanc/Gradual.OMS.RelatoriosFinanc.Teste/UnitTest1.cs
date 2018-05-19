using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;

namespace Gradual.OMS.RelatoriosFinanc.Teste
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //this.ConsutlarCustodia();
            //this.ConsultarCustodiaTesouroDireto();
            //this.ConsultarSaldoProjetado();
            //ConsultarDadosMonitorRisco();
            ConsultarNCBmf();

        }

        private void ConsultarNCBmf()
        {
            var lRetorno = new ServicoRelatoriosFinanceiros().ConsultarNotaDeCorretagemBmf(new NotaDeCorretagemExtratoRequest()
                {
                    ConsultaCodigoCliente = 31217,
                    ConsultaDataMovimento = new DateTime(2014, 04, 17),
                    
                });

            if (lRetorno.StatusResposta == Library.MensagemResponseStatusEnum.OK)
            {

            }
        }

        private void ConsultarDadosMonitorRisco()
        {
            var lRetorno = new ServicoRelatoriosFinanceiros().ConsultarDadosMonitorRisco(new MonitorRiscoRequest ()
            {
                //CodigoClienteBmf = 39556,
                CodigoCliente = 39556

            });

            if (lRetorno.StatusResposta == Library.MensagemResponseStatusEnum.OK)
            {

            }

        }

        private void ConsultarCustodiaTesouroDireto()
        {
            var lRetorno = new ServicoRelatoriosFinanceiros().ConsultarCustodiaComTesouro(new PosicaoCustodiaTesouroDiretoRequest()
            {
                ConsultaCdClienteBMF = 7041
            });

            if (lRetorno.StatusResposta == Library.MensagemResponseStatusEnum.OK)
            {

            }
        }

        private void ConsultarSaldoProjetado()
        {
            var lRetorno = new ServicoRelatoriosFinanceiros().ConsultarSaldoProjecoesEmContaCorrente(new SaldoProjetadoCCRequest()
            {
                ConsultaCdAssesso = "403"
                ,
                ConsultaDataOperacao = DateTime.Now.Date
            });

            if (lRetorno.StatusResposta == Library.MensagemResponseStatusEnum.OK)
            {

            }
        }

        private void ConsutlarCustodia()
        {
            var lRetorno = new ServicoRelatoriosFinanceiros().ConsultarCustodia(new PosicaoCustodiaRequest()
            {
                 ConsultaCdClienteBovespa = 7041
            });

            if (lRetorno.StatusResposta == Library.MensagemResponseStatusEnum.OK)
            {

            }
        }
    }
}
