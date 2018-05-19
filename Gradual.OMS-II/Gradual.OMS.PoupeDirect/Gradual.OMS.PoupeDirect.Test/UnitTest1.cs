using Gradual.OMS.PoupeDirect.Lib.Mensagens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Gradual.OMS.PoupeDirect.Lib.Util;
using Gradual.OMS.PoupeDirect.DB;

namespace Gradual.OMS.PoupeDirect.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //new PersistenciaDbExporcacaoClientePoupe().ExportarClientePoupe(new ExportarClientePoupeRequest());



            //custodiavalorizadarequest request = new custodiavalorizadarequest();

            //request.custodiavalorizada = new lib.dados.custodiavalorizadainfo();
            //request.custodiavalorizada.codigocliente = 342089;

            //custodiavalorizadaresponse response = new persistenciadb().selecionarcustodiavalorizada(request);

            //ProdutoClienteRequest request = new ProdutoClienteRequest();
            //request.ProdutoCliente = new Lib.Dados.ProdutoClienteInfo();
            //request.ProdutoCliente.CheckOperador = true;
            //request.ProdutoCliente.DataFim = new DateTime(2011, 12, 11); 
            //request.ProdutoCliente.DataInicio = new DateTime(2011, 03, 11);

            //var lretorno = new ServicoPoupeDirect().InserirClienteVencimentoHistorico();


            //new ServicoPoupeDirect().InserirClienteVencimentoHistorico(); teste vencimento 
            new ServicoPoupeDirect().InserirMovimentoClienteRetroativo();


            //if (lretorno.StatusResposta == Library.MensagemResponseStatusEnum.OK)
            //{

            //}

            //ProdutoClienteRequest request = new ProdutoClienteRequest();
            //request.ProdutoCliente = new Lib.Dados.ProdutoClienteInfo();


            //new ServicoPoupeDirect().ListarLinhasArquivoCCOUPoupe();

        }
    }
}
