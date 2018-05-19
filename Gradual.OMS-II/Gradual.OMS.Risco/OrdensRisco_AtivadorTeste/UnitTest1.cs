using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net;
using Gradual.OMS.Contratos.CadastroPapeis.Mensageria;
using Gradual.OMS.Contratos.CadastroPapeis.Info;
using Gradual.OMS.Sistemas.CadastroPapeis;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Custodia.Lib.Mensageria;
using Gradual.OMS.Custodia.Lib.Info;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.Ordens.Lib.Mensageria;
using Gradual.OMS.Ordens.Lib.Info;
using Gradual.OMS.Ordens;
using Gradual.OMS.Ordens.Lib.Dados;
using Gradual.OMS.ContaCorrente;
using Gradual.OMS.Risco.Custodia;

namespace OrdensRisco_AtivadorTeste
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {

        public UnitTest1()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        // Esse tipo de declaracao é preferivel sobre a outra
        // classes derivadas de MyClass automaticamente gravarao no log
        // com o nome da classe corrigido
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Com grana
        //public int IdCliente
        //{
        //    get { return 33969; }
        //}

        // Sem grana
        public int IdCliente
        {
            get { return 35978; }
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

        [TestMethod]
        public void TestMethod1()
        {
            EnviarOrdemVendaRisco();
        }

        private void ObterSaldoContaCorrenteUtilizandoLimite()
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

        private void EnviarOrdemRisco()
        {
            DateTime Inicio = DateTime.Now;

            EnviarOrdemRequest OrdemRequest = new EnviarOrdemRequest
            {
                OrdemInfo = new OrdemInfo
                {
                    Account = IdCliente,
                    ChannelID = 1,
                    ExecBroker = "227",
                    ExpireDate = DateTime.Now.AddDays(30),
                    OrderQty = 1800,
                    OrdType = OrdemTipoEnum.OnClose,
                    OrdStatus = OrdemStatusEnum.NOVA,
                    Price = 35.00,
                    RegisterTime = DateTime.Now,
                    Side = OrdemDirecaoEnum.Compra,
                    Symbol = "PETRH42",
                    TimeInForce = OrdemValidadeEnum.ValidaAteSerCancelada,
                    Description = "Envio de ordem de compra de opções."
                }
            };

            IServicoOrdens servicoOrdem = Ativador.Get<IServicoOrdens>();
            EnviarOrdemResponse response = servicoOrdem.EnviarOrdem(OrdemRequest);

            TimeSpan TempoGasto = (Inicio - DateTime.Now);

        }

        private void EnviarOrdemVendaRisco()
        {

            DateTime Inicio = DateTime.Now;

            EnviarOrdemRequest OrdemRequest = new EnviarOrdemRequest
            {
                OrdemInfo = new OrdemInfo
                {
                    Account = IdCliente,
                    ChannelID = 1,
                    ExecBroker = "227",
                    ExpireDate = DateTime.Now.AddDays(30),
                    OrderQty = 500,
                    OrdType = OrdemTipoEnum.OnClose,
                    OrdStatus = OrdemStatusEnum.NOVA,
                    Price = 35.00,
                    RegisterTime = DateTime.Now,
                    Side = OrdemDirecaoEnum.Compra,
                    Symbol = "VALEI40",
                    TimeInForce = OrdemValidadeEnum.ValidaAteSerCancelada,
                    Description = "Envio de ordem de venda de ações."
                }
            };

            EnviarOrdemResponse response =
                new ServicoOrdens().EnviarOrdem(OrdemRequest);

            TimeSpan TempoGasto = (Inicio - DateTime.Now);

            EnviarOrdemRisco();

        }

        private void ObterDadosInstrumento(string Instrumento)
        {
            CadastroPapeisRequest request = new CadastroPapeisRequest()
            {
                Instrumento = Instrumento
            };

            CadastroPapeisResponse<CadastroPapelBovespaInfo> Info =
                new ServicoCadastroPapeis().ObterInformacoesIntrumento(request);
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
