using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gradual.OMS.Risco;
using System.Threading;
using System.Runtime.CompilerServices;
using log4net;
using Gradual.OMS.Custodia.Lib.Mensageria;
using Gradual.OMS.Custodia.Lib.Info;
using Gradual.OMS.Risco.Custodia;
using Gradual.OMS.Ordens.Lib.Mensageria;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.OMS.Ordens.Lib.Info;
using Gradual.OMS.Ordens;
using Gradual.OMS.ContaCorrente;
using Gradual.OMS.Risco.Lib.Mensageria;
using Gradual.OMS.Risco.Enum;
using Gradual.OMS.Risco.Lib.Info;
using Gradual.OMS.Ordens.Lib.Enum;
using Gradual.OMS.CadastroPapeis.Lib.Mensageria;
using Gradual.OMS.CadastroPapeis;
using Gradual.OMS.CadastroPapeis.Lib.Info;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.Proventos;

//ChamadaHomeBroker

namespace RiskTest
{
    [TestClass]
    public class ChamadaHomeBroker
    {
        [TestMethod]
        public void TestMethod1()
        {
            TestarCancelamentoDeOrdem();
        }

        private void TestarCancelamentoDeOrdem()
        {
            var lRetorno = new ServicoOrdens().CancelarOrdem(new EnviarCancelamentoOrdemRequest()
             {
                 ClienteCancelamentoInfo = new ClienteCancelamentoInfo()
                 {
                     OrderID = "10102011030457-74049898",
                 }
             });

            if (lRetorno.StatusResposta == Gradual.OMS.Ordens.Lib.Enum.CriticaRiscoEnum.Sucesso)
            {

            }
        }

        // LogForNet
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Com grana
        //public int IdCliente
        //{
        //    get { return 7041; }
        //}

        // Sem grana
        public int IdCliente
        {
            get { return 42228; }
        }

        //Cliente posicionado em opção
        //public int IdCliente
        //{
        //    get { return 20866; }
        //}

        private TestContext testContextInstance;


        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        private void ObterSaldoContaCorrenteBMF()
        {
            SaldoContaCorrenteRequest ContaCorrenteRequest = new SaldoContaCorrenteRequest()
            {
                IdCliente = 38360
            };

            SaldoContaCorrenteResponse<ContaCorrenteBMFInfo> ContaCorrenteBMF = new ServicoContaCorrente().ObterSaldoContaCorrenteBMF(ContaCorrenteRequest);

        }

        private void ObterCustodiaBMF()
        {

            SaldoCustodiaRequest _SaldoRequest = new SaldoCustodiaRequest()
            {
                IdCliente = 108651
            };

            SaldoCustodiaResponse<CustodiaClienteInfo> resultData =
                new ServicoCustodia().ObterCustodiaCliente(_SaldoRequest);

            SaldoCustodiaResponse<CustodiaClienteBMFInfo> resposta =
                new ServicoCustodia().ObterCustodiaClienteBMF(new SaldoCustodiaRequest()
            {
                IdCliente = 108651
            });

        }

        public void IncluirBloqueioCliente()
        {
            ClienteCustodiaBloqueioRequest<ClienteCustodiaBloqueioInfo> request = new ClienteCustodiaBloqueioRequest<ClienteCustodiaBloqueioInfo>();

            request.Objeto = new ClienteCustodiaBloqueioInfo();

            request.Objeto.IdCliente = 35978;
            request.Objeto.IdOrdem = 10;
            request.Objeto.Instrumento = "PETR4";
            // request.Objeto.QtdBloqueada = 5000;

            ClienteCustodiaBloqueioResponse<ClienteCustodiaBloqueioInfo> response = new ServicoCustodia().InserirBloqueioCliente(
                request);

        }

        private void ObterVencimentoOpcoes()
        {
            VencimentoInstrumentoResponse response = new ServicoRisco().ObterVencimentoInstrumentos(
                new VencimentoInstrumentoRequest()
                );

            VencimentoInstrumentoInfo.htInstrumentos = response.Objeto;
        }

        private void ObterSaldoCustodia()
        {
            SaldoCustodiaRequest _SaldoRequest = new SaldoCustodiaRequest()
            {
                IdCliente = IdCliente
            };

            SaldoCustodiaResponse<CustodiaClienteInfo> resultData =
                new ServicoCustodia().ObterCustodiaCliente(_SaldoRequest);

        }

        private void ObterSaldoContaCorrente()
        {

            SaldoContaCorrenteRequest _SaldoRequest = new SaldoContaCorrenteRequest()
            {
                IdCliente = IdCliente
            };

            SaldoContaCorrenteResponse<ContaCorrenteInfo> resultData =
                new ServicoContaCorrente().ObterSaldoContaCorrente(_SaldoRequest);
        }


        [TestMethod]
        private void EnviarOrdemRisco()
        {


            EnviarOrdemRequest OrdemRequest = new EnviarOrdemRequest
            {
                ClienteOrdemInfo = new ClienteOrdemInfo
                {
                    CodigoCliente = 31940,//31940, //28643,// IdCliente,
                    DataHoraSolicitacao = DateTime.Now,
                    DataValidade = new DateTime(2011, 01, 10, 23, 59, 59),
                    DirecaoOrdem = OrdemDirecaoEnum.Compra,
                    Instrumento = "PETR4",
                    Preco = 25,
                    Quantidade = 50,
                    TipoDeOrdem = OrdemTipoEnum.Limitada,
                    ValidadeOrdem = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaParaODia
                }
            };

            DateTime dataInicio = DateTime.Now;

            EnviarOrdemResponse response =
               new ServicoOrdens().EnviarOrdem(OrdemRequest);

            TimeSpan datafinal = (DateTime.Now - dataInicio);

            bool solicitacao = true;

        }

        private void TestarPermissaoCliente()
        {
            ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> Resposta =
                new ServicoRisco().VerificarPermissaoCliente(
                            new ClienteParametrosPermissoesRequest()
                            {
                                IdCliente = 31940,
                                ParametroPermissaoEnum = Gradual.OMS.Risco.Lib.Enum.ParametroPermissaoRiscoEnum.PERMISSAO_OPERAR_STOPSTART

                            });

            bool valor = true;
        }

        private void ObterDadosInstrumento(string Instrumento)
        {
            CadastroPapeisRequest request = new CadastroPapeisRequest()
            {
                Instrumento = Instrumento
            };

            CadastroPapeisResponse<CadastroPapelInfo> Info =
                new ServicoCadastroPapeis().ObterInformacoesIntrumento(request);
        }

        private void CancelarOrdem()
        {
            ClienteCancelamentoInfo ClienteCancelamentoInfo = new ClienteCancelamentoInfo()
            {
                OrderID = "2192010184602-3552158"
            };

            EnviarCancelamentoOrdemRequest request = new EnviarCancelamentoOrdemRequest()
            {
                ClienteCancelamentoInfo = ClienteCancelamentoInfo
            };

            EnviarCancelamentoOrdemResponse response =
                new ServicoOrdens().CancelarOrdem(request);

            bool solicitacao = true;

        }

        /// <summary>
        /// Rotina responsável por carregar em memória todos os possíveis pipelines que uma ordem 
        /// devera percorrer ate efetivamente ser enviada para o roteador de ordens.       
        /// </summary>        
        private void CarregarEstruraPipeLineRisco()
        {

            /* PipeLineRiscoRequest Request
                 = new PipeLineRiscoRequest()
                 {
                     IdAcao = (int)TipoAcaoRiscoEnum.CompraAcoes
                 };

            
               //Trecho referente ao pipeline de compra de ações
               //Inicio

               PipeLineRiscoResponse<PipeLineAcaoInfo> _ParamRisco =
                   new ServicoRisco().CarregarPipeLineCompraAcoes(Request);

               if (_ParamRisco.StatusResposta == Gradual.OMS.Contratos.Risco.Enum.CriticaMensagemEnum.OK)
               {
                   var IEnumerableCompraAcoes =
                       from c in _ParamRisco.ColecaoObjeto
                       orderby c.OrdemDisparo ascending
                       select c;


                   SharedPipeLineRisco.AddValue(
                       TipoAcaoRiscoEnum.CompraAcoes, IEnumerableCompraAcoes
                       );
               }

               // Fim - Trecho compra de ações     

               // Techo referente a Venda de ações
               // Inicio
               Request
                   = new PipeLineRiscoRequest()
                   {
                       IdAcao = (int)TipoAcaoRiscoEnum.VendaAcoes
                   };


               _ParamRisco =
                   new ServicoRisco().CarregarPipeLineCompraAcoes(
                   Request
                   );

               if (_ParamRisco.StatusResposta == CriticaMensagemEnum.OK)
               {
                   var IEnumerableVendaAcoes =
                     from c in _ParamRisco.ColecaoObjeto
                     orderby c.OrdemDisparo ascending
                     select c;

                   SharedPipeLineRisco.AddValue(
                       TipoAcaoRiscoEnum.VendaAcoes, IEnumerableVendaAcoes
                       );
               }

               // Fim - Trecho Venda de ações
             * */

            Console.Read();

        }

    }
}
