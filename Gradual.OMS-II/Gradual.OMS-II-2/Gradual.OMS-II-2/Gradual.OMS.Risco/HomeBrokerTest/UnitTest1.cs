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
using Gradual.OMS.Risco.Lib;
using Gradual.OMS.Custodia.Lib;
using Gradual.OMS.Ordens.Lib;
using Gradual.OMS.Proventos;
using Gradual.OMS.Cotacao.Lib;
using Gradual.OMS.Cotacao;
using Gradual.OMS.Cotacao.Lib.Mensageria;
using Gradual.OMS.Basket.Lib.Mensageria;
using Gradual.OMS.Basket;
using Gradual.OMS.Basket.Lib;
using Gradual.OMS.Termo;
using Gradual.OMS.Termo.Lib;
using Gradual.OMS.Termo.Lib.Mensageria;
using System.Data.OleDb;
using System.Data;
using Gradual.OMS.Termo.Lib.Info;
using System.Diagnostics;

namespace HomeBrokerTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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


        private void ObterSaldoContaCorrente()
        {
            SaldoContaCorrenteRequest CC = new SaldoContaCorrenteRequest();
            CC.IdCliente = 45383;

            SaldoContaCorrenteResponse<ContaCorrenteInfo> SaldoCC =
                new ServicoContaCorrente().ObterSaldoContaCorrente(CC);
        }

        private void ObterStrikeOpcao()
        {
            string Instrumento = "PETRK30";
            string Strike = Instrumento.Substring(Instrumento.Length - 2, 2);
        }

        private EnviarCotacaoResponse ObterCotacao(string Papel)
        {
            EnviarCotacaoRequest _request = new EnviarCotacaoRequest();
            _request.CotacaoInfo.Ativo = Papel;

            ServicoCotacaoOMS _ServicoCotacaoOMS = new ServicoCotacaoOMS();
            return _ServicoCotacaoOMS.ObterCotacaoInstrumento(_request);            
        }

        private void ObterSaldoCustodia()
        {

            ServicoCustodia custodia = new ServicoCustodia();
            SaldoCustodiaRequest RE = new SaldoCustodiaRequest();

            RE.IdCliente = 31940;

            var Resposta = custodia.ObterCustodiaConsolidada(RE);

            SaldoCustodiaRequest _SaldoRequest = new SaldoCustodiaRequest(){
                IdCliente = 31940
            };

            SaldoCustodiaResponse<CustodiaClienteInfo> resultData =
                new ServicoCustodia().ObterCustodiaCliente(_SaldoRequest);
        }

        [TestMethod]
        public void TestMethod1()
        {
            // this.AlterarStatusSolicitacao();
            // InserirOrdemTermo();

            //InserirOrdemTermo();

            //AcompanhamentoOrdemConsolidado();

            ObterPosicaoTermo();

            //AcompanhamentoOrdemTermo();
            // AcompanhamentoOrdemTermo();
            // AtualizarOrdemTermo();           
            // CancelarOrdem();
            // AdicionaBasket();

        }

        private void ObterTaxaTermo()
        {
            var response = new ServicoTermo().ConsultarTaxaTermoDia();
        }


        private void AlocarGarantiasTermo()
        {
           // ServicoTermo _ServicoTermo = new ServicoTermo();

            IServicoTermo _ServicoTermo = Ativador.Get<IServicoTermo>();

            ClienteGarantiaRequest request = new ClienteGarantiaRequest();

            request.ClienteGarantiaInfo.IdCliente = 31940;
            request.ClienteGarantiaInfo.Instrumento = "PETR4";
            request.ClienteGarantiaInfo.Quantidade = 100;
            request.ClienteGarantiaInfo.DataSolicitacao = DateTime.Now;

            var response = _ServicoTermo.AlocarGarantiasTermo(request);
        }

        private void ConsultarTaxaTermo()
        {
            var response = new ServicoTermo().ConsultarTaxaTermoDia();
        }

        private void InserirTaxaTermo()
        {

            TaxaTermoRequest _request = new TaxaTermoRequest();

            _request.TaxaTermoInfo.IdTaxa = 3;
            _request.TaxaTermoInfo.NumeroDias = 80;
            _request.TaxaTermoInfo.ValorRolagem = 24;
            _request.TaxaTermoInfo.ValorTaxa = 18;

            TaxaTermoResponse _resposta = new ServicoTermo().InserirTaxaTermo(_request);
        }
        private void InserirOrdemTermo()
        {
            try
            {

               //IServicoTermo _ServicoTermo = Ativador.Get<IServicoTermo>();

                OrdemTermoInfo _OrdemTermoInfo = new OrdemTermoInfo();
                ServicoTermo _ServicoTermo = new ServicoTermo();

                OrdemTermoRequest _request = new OrdemTermoRequest();

              //  _OrdemTermoInfo.IdOrdemTermo = 6;
                _OrdemTermoInfo.IdCliente = 31940;
                _OrdemTermoInfo.IdTaxa = 1;
                _OrdemTermoInfo.Instrumento = "PETR4T";                
                _OrdemTermoInfo.Quantidade = 200;
                _OrdemTermoInfo.StatusOrdemTermo = EnumStatusTermo.SolicitacaoEnviada;
                _OrdemTermoInfo.precoDireto = 0;
                _OrdemTermoInfo.PrecoLimite = 40;
                _OrdemTermoInfo.precoMaximo = 0;
                _OrdemTermoInfo.precoMinimo = 0;
                _OrdemTermoInfo.TipoSolicitacao = EnumTipoSolicitacao.NovoTermo;

                _request.OrdemTermoInfo = _OrdemTermoInfo;

                var response = _ServicoTermo.InserirOrdemTermo(_request);
            }
            catch (Exception ex)
            {

            }
            
        }

        private void AcompanhamentoOrdemTermo()
        {
            AcompanhamentoOrdemTermoRequest _request = new AcompanhamentoOrdemTermoRequest();
            _request.AcompanhamentoOrdemTermoInfo.IdCliente = 31940;

            AcompanhamentoOrdemTermoResponse ObterAcompanhamentoOrdemTermo =
                new ServicoTermo().ObterAcompanhamentoOrdemTermoSinacor(_request);
        }

        private void AcompanhamentoOrdemConsolidado()
        {
            try
            {

                //IServicoTermo _ServicoTermo = Ativador.Get<IServicoTermo>();

                ServicoTermo _ServicoTermo = new ServicoTermo();

                AcompanhamentoConsolidadoOrdemTermoRequest _request = new AcompanhamentoConsolidadoOrdemTermoRequest();
                _request.CodigoCliente = 31940;

                //AcompanhamentoConsolidadoOrdemTermoResponse ObterAcompanhamentoOrdemTermo =
                //    _ServicoTermo.AcompanhamentoOrdensConsolidadoSolicitacoes(_request);


                AcompanhamentoConsolidadoOrdemTermoResponse ObterAcompanhamentoOrdemTermo =
               new ServicoTermo().AcompanhamentoOrdensConsolidadoSolicitacoes(_request);

                Debugger.Log(0, "AcompanhamentoOrdemConsolidado", string.Format("[{0}] acompanhamentos encontrados", ObterAcompanhamentoOrdemTermo.ListaAcompanhamentoConsolidado.Count));
            }
            catch (Exception ex)
            {
                Debugger.Log(0, "Erro_AcompanhamentoOrdemConsolidado", ex.Message);
            }

        }

        private void AlterarStatusSolicitacao()
        {
            try
            {

                OrdemTermoInfo    _OrdemTermoInfo = new OrdemTermoInfo();
                ServicoTermo      _ServicoTermo   = new ServicoTermo();
                OrdemTermoRequest _request        = new OrdemTermoRequest();

                _OrdemTermoInfo.IdOrdemTermo = 24;
                _OrdemTermoInfo.IdCliente         = 31940;
                _OrdemTermoInfo.StatusOrdemTermo  = EnumStatusTermo.SolicitacaoExecutada;
                _OrdemTermoInfo.TipoSolicitacao   = EnumTipoSolicitacao.Cancelamento;
 

                _request.OrdemTermoInfo = _OrdemTermoInfo;

                var response = _ServicoTermo.AlterarStatusSolicitacaoTermo(_request);
            }
            catch (Exception ex)
            {

            }

        }

        private void AtualizarOrdemTermo()
        {
            try
            {
                OrdemTermoInfo    _OrdemTermoInfo  = new OrdemTermoInfo();
                ServicoTermo      _ServicoTermo    = new ServicoTermo();
                OrdemTermoRequest _request         = new OrdemTermoRequest();

                _OrdemTermoInfo.IdOrdemTermo     = 11;
                _OrdemTermoInfo.IdCliente        = 31940;
                _OrdemTermoInfo.StatusOrdemTermo = EnumStatusTermo.SolicitacaoEnviada;
                _OrdemTermoInfo.TipoSolicitacao  = EnumTipoSolicitacao.Liquidacao;
                _OrdemTermoInfo.precoMaximo      = 39;
                _OrdemTermoInfo.precoMinimo      = 38;
                _OrdemTermoInfo.PrecoLimite      = 39;

               // _OrdemTermoInfo.precoNegocio = 28;

                _request.OrdemTermoInfo = _OrdemTermoInfo;
                var response            = _ServicoTermo.AtualizarOrdemTermo(_request);

            }
            catch (Exception ex)
            {

            }

        }

        private void ObterPosicaoTermo()
        {
            ServicoTermo _servicoTermo = new ServicoTermo();

            _servicoTermo.ObterSaldoDisponivelTermo(31940);
        }





        private void Teste()
        {
            var CadastroPapeis = new ServicoCadastroPapeis().ObterInformacoesIntrumento(
                                                                                      new CadastroPapeisRequest()
                                                                                      {
                                                                                          Instrumento = "TOYB4"
                                                                                      });

            int QuantidadeOrdem = 150;            

            int LoteNegociacao = int.Parse(CadastroPapeis.Objeto.LoteNegociacao);
            int FatorCotacao = int.Parse(CadastroPapeis.Objeto.FatorCotacao);

            if (QuantidadeOrdem < LoteNegociacao)
            {

            }
              
        }

        [TestMethod]
        private void Proventos()
        {
           
        }

        private void CancelarOrdem()
        {
            ClienteCancelamentoInfo info = new ClienteCancelamentoInfo();

            info.OrderID = "30032012030412-36938417";            
            EnviarCancelamentoOrdemRequest request = new EnviarCancelamentoOrdemRequest();
            request.ClienteCancelamentoInfo = info;
            EnviarCancelamentoOrdemResponse response = new EnviarCancelamentoOrdemResponse();

            response = new ServicoOrdens().CancelarOrdem(request);            
        }

        private void AdicionaBasket()
        {

            ServicoBasket _ServicoBasket = new ServicoBasket();

            EnviarAtualizaoPrecoGlobalRequest req = new EnviarAtualizaoPrecoGlobalRequest();
            req.PrecoBasket = 5;
            req.IndicadorOscilacao = 'N';
            req.CodigoBasket = 1;

            _ServicoBasket.EnviarAtualizaoPrecoGlobal(req);
        }


        private void ServicoTermo()
        {

            DateTime dataInicio = DateTime.Now;
            IServicoTermo gServico = Ativador.Get<IServicoTermo>();

            AcompanhamentoConsolidadoOrdemTermoRequest _request = new AcompanhamentoConsolidadoOrdemTermoRequest();
            _request.CodigoCliente = 31940;

             var response = gServico.ConsultarTaxaTermoDia();

        }

        private void EnviarOrdemRisco()
        {
            DateTime dataInicio = DateTime.Now;
            IServicoOrdens _gServico = Ativador.Get<IServicoOrdens>();



            EnviarOrdemRequest OrdemRequest = new EnviarOrdemRequest
            {
                ClienteOrdemInfo = new ClienteOrdemInfo
                {
                    CodigoCliente = 31940,
                   // NumeroControleOrdem = "16042012123433-94912441",
                    DataHoraSolicitacao = DateTime.Now,
                    DataValidade = new DateTime(2012, 05, 28, 23, 59, 59),
                    DirecaoOrdem = OrdemDirecaoEnum.Compra,
                    Instrumento = "ICFU12",
                    PortaControleOrdem = "0",
                    Preco = 100,
                    Quantidade = 1,
                    QuantidadeAparente = 0,
                    QuantidadeMinima = 0,
                    TipoDeOrdem = OrdemTipoEnum.Limitada,
                    ValidadeOrdem = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaParaODia,
                }
            };

           // EnviarOrdemResponse response = _gServico.EnviarOrdem(OrdemRequest);
            EnviarOrdemResponse response = new ServicoOrdens().EnviarOrdem(OrdemRequest);
            Thread.Sleep(1000);


            TimeSpan datafinal = (DateTime.Now - dataInicio);

            bool solicitacao = true;

        }
    }
}
