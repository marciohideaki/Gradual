using Gradual.OMS.Library;
using Gradual.OMS.TesouroDireto.AcessoWS;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Consultas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.TesouroDireto.Lib;
using System;
using System.Collections.Generic;
using Gradual.OMS.TesouroDireto.Lib.Dados;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Venda;

namespace Gradual.OMS.TesouroDireto.Teste
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //var lRetorno = new TesouroDeiretoConsulta().ConsultarMercado(new Lib.Mensagens.Consultas.ConsultasConsultaMercadoRequest());

            TestarConsultaExtrato();

            //if (lRetorno.StatusResposta == MensagemResponseStatusEnum.OK)
            //{

            //}
        }

        private void TestarConsultaExtrato()
        {
            /*
            var lRetorno = new TesouroDiretoConsulta().ConsultarProtocolo(new ConsultasConsultaTaxaProtocoloRequest()
            {
                CodigoCesta = 1607857,
                 CodigoTitulo = 010115
            });

            if (lRetorno.StatusResposta == MensagemResponseStatusEnum.OK)
            {

            }
             * */

            ConsultasConsultaExtratoMensalResponse lRetorno = new TesouroDiretoConsulta().ConsultarExtratoMensal(new ConsultasConsultaExtratoMensalRequest() 
            {
                CPFNegociador = "01221123858"
            });


            Console.Write(lRetorno);
        }
    }
}
