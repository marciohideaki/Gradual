using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.Monitor.Custodia.Lib.Mensageria;
using Gradual.Monitores.Risco.Info;
using Gradual.Monitores.Risco.Lib;
using Gradual.OMS.Monitor.Custodia.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using System.Resources;
using System.Reflection;
using Gradual.Site.DbLib.Mensagens;
using Gradual.Site.Www.PosicaoCotista;
using Gradual.Servico.FichaCadastral.Dados;
using Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.Site.DbLib.Dados.MinhaConta;
using Gradual.Site.DbLib.Persistencias.MinhaConta.Fundos;
using Gradual.OMS.ContaCorrente.Lib.Info;
using Gradual.OMS.Risco.Lib.Enum;
using Gradual.Site.DbLib;
using Gradual.Site.DbLib.Dados;

namespace Gradual.Site.Www.MinhaConta.Financeiro
{
    public partial class MinhaConta : PaginaBase
    {
        public decimal CurrentTotalGeral { get; set; }

        Dictionary<String, decimal> lListaValores = new Dictionary<string, decimal>();

        List<Garantia> lListaGarantia = new List<Garantia>();
        
        List<Gradual.Site.DbLib.Dados.Garantia> lListaGarantias = new List<Gradual.Site.DbLib.Dados.Garantia>();
        List<Gradual.Site.DbLib.Dados.GarantiaBMF> lListaGarantiasBMF = new List<Gradual.Site.DbLib.Dados.GarantiaBMF>();

        List<CustodiaBTC> lListaBTC = new List<CustodiaBTC>();
        List<CustodiaTermo> lListaCustodiaTermo = new List<CustodiaTermo>();
        List<CustodiaTermo> lListaCustodiaTermoALiquidar = new List<CustodiaTermo>();
        List<Gradual.Site.DbLib.Dados.CustodiaTesouro> lListaCustodiaTesouro = new List<Gradual.Site.DbLib.Dados.CustodiaTesouro>();
        List<Gradual.Site.DbLib.Dados.CustodiaTesouro> lListaCustodiaTesouroMinicom = new List<Gradual.Site.DbLib.Dados.CustodiaTesouro>();
        List<Gradual.Site.DbLib.Dados.Provento> lListaProventos = new List<Gradual.Site.DbLib.Dados.Provento>();
        List<Gradual.Site.DbLib.Dados.ResgateFundo> lListaResgates = new List<Gradual.Site.DbLib.Dados.ResgateFundo>();
        List<Gradual.Site.DbLib.Dados.PosicaoFundo> lListaPosicoes = new List<Gradual.Site.DbLib.Dados.PosicaoFundo>();
        List<FundoPosicao> lListaFundoRendaFixa = new List<FundoPosicao>();
        List<FundoPosicao> lListaFundoMultimercado = new List<FundoPosicao>();
        List<FundoPosicao> lListaFundoAcao = new List<FundoPosicao>();
        List<FundoPosicao> lListaFundoOutros = new List<FundoPosicao>();

        DateTime lUltimoDiaUtil;

        SaldoContaCorrenteResponse<Gradual.OMS.ContaCorrente.Lib.ContaCorrenteInfo> lResponseSaldoCC;
        SaldoContaCorrenteResponse<ContaCorrenteBMFInfo> lResponseSaldoBMF;

        FinanceiroExtratoInfo lResponseExtrato = new FinanceiroExtratoInfo();

        ContaCorrenteExtratoRequest lRequestExtratoCC = new ContaCorrenteExtratoRequest();
        ContaCorrenteExtratoResponse lResponseExtratoCC = new ContaCorrenteExtratoResponse();

        MonitorCustodiaRequest lRequestCustodia = new MonitorCustodiaRequest();
        MonitorCustodiaResponse lResponseCustodia = new MonitorCustodiaResponse();

        MonitorLucroPrejuizoRequest lRequestLucroPreju = new MonitorLucroPrejuizoRequest();
        MonitorLucroPrejuizoResponse lResponseLucroPreju = new MonitorLucroPrejuizoResponse();

        SaldoContaCorrenteRequest lRequestSaldoCC = new SaldoContaCorrenteRequest();
        SaldoContaCorrenteRequest lRequestSaldoBMF = new SaldoContaCorrenteRequest();

        FundoRequest lRequestFundo = new FundoRequest();
        FundoResponse lResponseFundo = new FundoResponse();

        RendaFixaInfo lRequestRendaFixa = new RendaFixaInfo();
        List<RendaFixaInfo> lResponseRendaFixa = new List<RendaFixaInfo>();

        PosicaoCotistaViewModel[] lResponseRendaVariavel;

        List<ChamadaMargem> lChamadaMargem = new List<ChamadaMargem>();

        IServicoPersistenciaSite lServicoSite;

        DateTime DataD1;
        DateTime DataD2;
        DateTime DataD3;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Session["MinhaContaCC"]             = null;
                Session["SaldoAbertura"]            = null;
                Session["Extrato"]                  = null;
                Session["CustodiaAcao"]             = null;
                Session["CustodiaOpcao"]            = null;
                Session["CustodiaTermo"]            = null;
                Session["CustodiaTesouro"]          = null;
                Session["CustodiaFundo"]            = null;
                Session["CustodiaBTC"]              = null;
                Session["FundoPosicaoRendaFixa"]    = null;
                Session["FundoPosicaoMultimercado"] = null;
                Session["FundoPosicaoAcao"]         = null;
                Session["TituloPrivado"]            = null;
                Session["Garantia"]                 = null;
                Session["UltimoDiaUtil"]            = null;
                Session["LucroPrejuizo"]            = null;
                Session["ChamadaMargem"]            = null;

                if (base.ValidarSessao())
                {
                    if (!this.IsPostBack)
                    {
                        if (string.IsNullOrEmpty(base.SessaoClienteLogado.CodigoPrincipal))
                        {
                            base.ExibirMensagemJsOnLoad("I", "Você ainda não possui o código de conta na Gradual. <br/>Para acessar essa área, finalize seu Cadastro.");
                        }

                        this.CarregarDados();

                        //RodarJavascriptOnLoad("MinhaConta_GerarGraficoPatrimonio();");
                    }
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
            }
        }

        private void CarregarDados()
        {
            lServicoSite = InstanciarServicoDoAtivador<IServicoPersistenciaSite>();

            Int32 lCodigo = base.SessaoClienteLogado.CodigoPrincipal.DBToInt32();

            if (!String.IsNullOrEmpty(Request.Params["Codigo"]))
            {
                lCodigo = Int32.Parse(Request.Params["Codigo"]);
            }

            lUltimoDiaUtil = lServicoSite.SelecionaUltimoDiaUtil();

            lRequestCustodia.CodigoCliente  = lCodigo;
            lRequestLucroPreju.Cliente      = lCodigo;
            lRequestLucroPreju.CodigoLogin  = lCodigo;
            lRequestSaldoCC.IdCliente       = lCodigo;
            lRequestSaldoBMF.IdCliente      = lCodigo;
            lRequestFundo.CpfDoCliente      = "";
            lRequestRendaFixa.CodigoCliente = lCodigo;

            try
            {
                decimal lResponse           = 0;
                lResponse                   = lServicoSite.ObterSaldoAbertura(lCodigo);
                Session["SaldoAbertura"]    = lResponse;
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("CarregarDadaos -> ObterSaldoAbertura: {0}-{1}", ex.Message, ex.StackTrace);
            }

            #region Datas de pregão
            try
            {
                DataD1 = lServicoSite.ObterDataPregao(1);
                DataD2 = lServicoSite.ObterDataPregao(2);
                DataD3 = lServicoSite.ObterDataPregao(3);

                Session["DataD1"] = DataD1;
                Session["DataD2"] = DataD2;
                Session["DataD3"] = DataD3;
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("CarregarDadaos -> ObterdataPregao: {0}-{1}", ex.Message, ex.StackTrace);
            }
            #endregion

            #region Saldo de Conta Corrente
            try
            {
                lResponseSaldoCC = this.ObterSaldoContaCorrente(lRequestSaldoCC);

                if (lResponseSaldoCC.StatusResposta.Equals(OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK))
                {
                    this.CarregarSaldoContaCorrente(lResponseSaldoCC);

                }
                else
                {
                    throw new Exception(string.Format("{0}-{1}", lResponseSaldoCC.DescricaoResposta, lResponseSaldoCC.StackTrace));
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("CarregarDadaos -> ObterSaldoAbertura: {0}-{1}", ex.Message, ex.StackTrace);
            }
            #endregion

            #region Saldo Conta Corrente BMF
            try
            {
                lResponseSaldoBMF = this.ObterSaldoContaCorrenteBMF(lRequestSaldoBMF);

                if (lResponseSaldoBMF.StatusResposta.Equals(OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK))
                {
                    this.CarregarSaldoContaCorrenteBMF(lResponseSaldoBMF);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("CarregarDadaos -> ObterSaldoContaCorrentBMF: {0}-{1}", ex.Message, ex.StackTrace);
            }
            #endregion

            #region Lucro / Prejuizo
            try
            {
                lResponseLucroPreju = this.ObterLucroPrejuizo(lRequestLucroPreju);

                if (lResponseLucroPreju.TotalRegistros > 0)
                {
                    this.CarregarLucroPrejuizo(lResponseLucroPreju);
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("CarregarDadaos -> ObterLucroPrejuizo: {0}-{1}", ex.Message, ex.StackTrace);
            }
            #endregion

            #region Renda Variavel
            try
            {
                //lResponseRendaVariavel = this.ObterPosicaoRendaVariavel(lCodigo);

                //if (lResponseRendaVariavel.Count() > 0)
                //{
                //    this.CarregarPosicaoRendaVariavel(lResponseRendaVariavel);
                //}
                
                lListaPosicoes = lServicoSite.ObterPosicaoFundo(lCodigo);

                lListaResgates.AddRange(lServicoSite.ObterResgateFundo(lCodigo));

                if (lListaResgates.Count > 0)
                {
                    foreach (PosicaoFundo lFundoPosicao in lListaPosicoes)
                    {
                        foreach (ResgateFundo lResgate in lListaResgates)
                        {
                            if (lFundoPosicao.IdCarteira.Equals(lResgate.IdCarteira))
                            {
                                if (lFundoPosicao.DataProcessamento <= lResgate.DataAgendamento)
                                {
                                    lFundoPosicao.ValorLiquido -= lResgate.ValorLiquido;
                                }
                            }
                        }
                    }

                    Session["Resgates"] = lListaResgates;

                    CurrentTotalGeral += lListaResgates.AsEnumerable().Sum(x => x.ValorLiquido);
                }


                this.CarregarPosicaoRendaVariavel(lListaPosicoes);

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("CarregarDadaos -> ObterPosicaoRendaVariavel: {0}-{1}", ex.Message, ex.StackTrace);
            }
            #endregion

            #region Renda Fixa
            try
            {
                lResponseRendaFixa = this.ObterPosicaoRendaFixaTitulosPrivados(lRequestRendaFixa);

                if (lResponseRendaFixa.Count() > 0)
                {
                    this.CarregarPosicaoRendaFixa(lResponseRendaFixa);
                }
                else
                {
                    Session["TituloPrivado"] = new List<CustodiaTesouro>();
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("CarregarDadaos -> ObterPosicaoRendaFixa: {0}-{1}", ex.Message, ex.StackTrace);
            }
            #endregion 

            #region Termo à Liquidar
            try
            {
                lListaCustodiaTermoALiquidar = lServicoSite.ObterTermoALiquidar(lCodigo);
                Session["CustodiaTermoALiquidar"] = lListaCustodiaTermoALiquidar;
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("CarregarDadaos -> ObterTermoALiquidar: {0}-{1}", ex.Message, ex.StackTrace);
            }
            #endregion

            #region Termo
            try
            {
                lListaCustodiaTermo = lServicoSite.ObterTermo(lCodigo);
                Session["CustodiaTermo"] = lListaCustodiaTermo;
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("CarregarDadaos -> ObterTermo: {0}-{1}", ex.Message, ex.StackTrace);
            }
            #endregion 

            #region Tesouro
            try
            {
                lListaCustodiaTesouro = lServicoSite.ObterTesouroDireto(lCodigo);

                lListaCustodiaTesouroMinicom = this.ObterPoscaoRendaFixaTitulosPublicos(lRequestRendaFixa);

                lListaCustodiaTesouro.AddRange(lListaCustodiaTesouroMinicom);

                Session["CustodiaTesouro"] = lListaCustodiaTesouro;

                if (lListaCustodiaTesouro.Count > 0)
                {
                    CurrentTotalGeral += lListaCustodiaTesouro.AsEnumerable().Sum(x => x.ValorPosicao);
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("CarregarDadaos -> ObterTermo: {0}-{1}", ex.Message, ex.StackTrace);
            }
            #endregion

            #region Margem
            try
            {
                lChamadaMargem = lServicoSite.ObterChamadaMargem(lCodigo);

                Session["ChamadaMargem"] = lChamadaMargem;
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("CarregarDadaos -> ObterChamadaMargem: {0}-{1}", ex.Message, ex.StackTrace);
            }
            #endregion

            #region Garantias
            try
            {
                lListaGarantias = lServicoSite.ObterGarantias(lCodigo);

                //if (lListaGarantias.Count > 0)
                //{
                //    CurrentTotalGeral += lListaGarantias.AsEnumerable().Sum(x => x.Valor);
                //}

                //Session["Garantias"] = lListaBTC;
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("CarregarDadaos -> ObterBTC: {0}-{1}", ex.Message, ex.StackTrace);
            }
            #endregion

            #region Garantias
            try
            {
                lListaGarantiasBMF = lServicoSite.ObterGarantiasBMF(lCodigo);

                //if (lListaGarantias.Count > 0)
                //{
                //    CurrentTotalGeral += lListaGarantias.AsEnumerable().Sum(x => x.Valor);
                //}

                //Session["Garantias"] = lListaBTC;
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("CarregarDadaos -> ObterBTC: {0}-{1}", ex.Message, ex.StackTrace);
            }
            #endregion

            #region Custodia
            try
            {
                lResponseCustodia = this.ObterCustodia(lRequestCustodia);

                if (lResponseCustodia.StatusResposta.Equals(OMS.Library.MensagemResponseStatusEnum.OK))
                {
                    this.CarregarCustodia(lResponseCustodia);
                    
                    //this.CarregarGarantia(lResponseCustodia);
                    //this.CarregarGarantiaBMF(lResponseCustodia);
                    
                    this.CarregarGarantia(lListaGarantias);

                    this.CarregarGarantiaBMF(lListaGarantiasBMF);

                    if (lListaGarantia.Count > 0)
                    {
                        CurrentTotalGeral += lListaGarantia.AsEnumerable().Sum(x => x.Valor);
                    }
                }
                else
                {
                    gLogger.ErrorFormat("CarregarDadaos -> ObterCustodia: erro no serviço");
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("CarregarDadaos -> ObterCustodia: {0}-{1}", ex.Message, ex.StackTrace);
            }
            #endregion

            #region BTC
            try
            {
                lListaBTC = lServicoSite.ObterBTC(lCodigo);

                if (lListaBTC.Count > 0)
                {
                    CurrentTotalGeral += lListaBTC.AsEnumerable().Sum(x => x.Remuneracao);
                    CurrentTotalGeral += lListaBTC.AsEnumerable().Sum(x => x.Financeiro);
                }

                Session["CustodiaBTC"] = lListaBTC;
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("CarregarDadaos -> ObterBTC: {0}-{1}", ex.Message, ex.StackTrace);
            }
            #endregion

            #region Proventos
            try
            {
                lListaProventos = lServicoSite.ObterProventos(lCodigo);

                if (lListaProventos.Count > 0)
                {
                    CurrentTotalGeral += lListaProventos.AsEnumerable().Sum(x => x.Valor);
                }
                
                Session["Proventos"] = lListaProventos;
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("CarregarDadaos -> ObterProventos: {0}-{1}", ex.Message, ex.StackTrace);
            }
            #endregion

            #region Proventos em Garantia
            try
            {
                
                lListaProventos.AddRange(lServicoSite.ObterGarantiasDividendos(lCodigo));

                if (lListaProventos.Count > 0)
                {
                    Session["Proventos"] = lListaProventos;

                    CurrentTotalGeral += lListaProventos.AsEnumerable().Sum(x => x.Valor);
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("CarregarDadaos -> ObterGarantiasDividendos: {0}-{1}", ex.Message, ex.StackTrace);
            }
            #endregion

            Session["UltimoDiaUtil"] = lUltimoDiaUtil;
        }

        public SaldoContaCorrenteResponse<Gradual.OMS.ContaCorrente.Lib.ContaCorrenteInfo> ObterSaldoContaCorrente(SaldoContaCorrenteRequest pRequest)
        {
            IServicoContaCorrente lServicoCC = Ativador.Get<IServicoContaCorrente>();
            return lServicoCC.ObterSaldoContaCorrente(pRequest);
        }

        public bool CarregarSaldoContaCorrente(SaldoContaCorrenteResponse<Gradual.OMS.ContaCorrente.Lib.ContaCorrenteInfo> pParametro)
        {
            bool lRetorno = true;

            try
            {
                /* Saldo em Conta Corrente */
                MinhaContaCC lMinhaContaCC = new MinhaContaCC();

                /* Saldo D0 */
                lMinhaContaCC.SaldoD0_Rotulo = String.Format(Resource.SaldoD0_Rotulo, DateTime.Now.ToString("dd/MM/yyyy"));
                lMinhaContaCC.SaldoD0_Valor = pParametro.Objeto.SaldoD0;

                /* Saldo D1 */
                lMinhaContaCC.SaldoD1_Rotulo = String.Format(Resource.SaldoD1_Rotulo, DataD1.ToString("dd/MM/yyyy"));
                lMinhaContaCC.SaldoD1_Valor = pParametro.Objeto.SaldoD1;

                /* Saldo D2 */
                lMinhaContaCC.SaldoD2_Rotulo = String.Format(Resource.SaldoD2_Rotulo, DataD2.ToString("dd/MM/yyyy"));
                lMinhaContaCC.SaldoD2_Valor = pParametro.Objeto.SaldoD2;

                /* Saldo D3 */
                lMinhaContaCC.SaldoD3_Rotulo = String.Format(Resource.SaldoD3_Rotulo, DataD3.ToString("dd/MM/yyyy"));
                lMinhaContaCC.SaldoD3_Valor = pParametro.Objeto.SaldoD3;

                Session["MinhaContaCC"] = lMinhaContaCC;

                CurrentTotalGeral += lMinhaContaCC.SaldoD0_Valor;
                CurrentTotalGeral += lMinhaContaCC.SaldoD1_Valor;
                CurrentTotalGeral += lMinhaContaCC.SaldoD2_Valor;
                CurrentTotalGeral += lMinhaContaCC.SaldoD3_Valor;
            }
            catch (Exception ex)
            {
                lRetorno = false;

                gLogger.ErrorFormat("CarregarSaldoContaCorrente: {0}-{1}", ex.Message, ex.StackTrace);

                throw new Exception(string.Format("{0}-{1}", ex.Message, ex.StackTrace));
            }

            return lRetorno;

        }

        public SaldoContaCorrenteResponse<ContaCorrenteBMFInfo> ObterSaldoContaCorrenteBMF(SaldoContaCorrenteRequest pRequest)
        {
            IServicoContaCorrente lServicoCC = Ativador.Get<IServicoContaCorrente>();
            return lServicoCC.ObterSaldoContaCorrenteBMF(pRequest);
        }

        public bool CarregarSaldoContaCorrenteBMF(SaldoContaCorrenteResponse<ContaCorrenteBMFInfo> pParametro)
        {

            bool lRetorno = true;

            return lRetorno;
        }

        public FinanceiroExtratoInfo ObterExtratoFinanceiro(Int32 pCodigo)
        {
            var lServicoAtivador = Ativador.Get<IServicoExtratos>();

            var lRespostaBusca = lServicoAtivador.ConsultarExtratoFinanceiro(new FinanceiroExtratoRequest()
            {
                ConsultaCodigoCliente = pCodigo,
                ConsultaNomeCliente = ""//this.GetNomeCliente,
            });

            if (Gradual.OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK.Equals(lRespostaBusca.StatusResposta))
            {
                return lRespostaBusca.Relatorio;
            }
            else
            {
                throw new Exception(string.Format("{0}-{1}", lRespostaBusca.StatusResposta, lRespostaBusca.StackTrace));
            }
        }

        public bool CarregarExtratoFinanceiro(FinanceiroExtratoInfo pParametro)
        {

            bool lRetorno = true;

            Session["Extrato"] = pParametro;

            return lRetorno;
        }

        public MonitorCustodiaResponse ObterCustodia(MonitorCustodiaRequest pRequest)
        {
            IServicoMonitorCustodia lServicoCustodia = Ativador.Get<IServicoMonitorCustodia>();
            MonitorCustodiaResponse lResponse = lServicoCustodia.ObterMonitorCustodiaMemoria(pRequest);

            return lResponse;
        }

        public bool CarregarCustodia(MonitorCustodiaResponse pParametro)
        {
            bool lRetorno = true;

            List<CustodiaAcao> lListaCustodiaAcao = new List<CustodiaAcao>();
            List<CustodiaOpcao> lListaCustodiaOpcao = new List<CustodiaOpcao>();

            List<CustodiaFI> lListaCustodiaFundo = new List<CustodiaFI>();
            List<CustodiaTesouro> lListaCustodiaTesouro = new List<CustodiaTesouro>();

            foreach (Gradual.OMS.Monitor.Custodia.Lib.Info.MonitorCustodiaInfo.CustodiaPosicao lCustodia in pParametro.MonitorCustodia.ListaCustodia)
            {
                if (!lCustodia.CodigoCarteira.Equals(22012))
                {
                    if (lCustodia.TipoGrupo != null)
                    {
                        if (lCustodia.TipoGrupo.Equals("ACAO"))
                        {
                            if (lCustodia.NomeEmpresa.Contains("FII"))
                            {
                                CustodiaFI lCustodiaFI = new CustodiaFI();

                                lCustodiaFI.Instrumento     = lCustodia.CodigoInstrumento;
                                lCustodiaFI.Quantidade      = lCustodia.QtdeAtual.DBToInt32();
                                lCustodiaFI.Preco           = lCustodia.Cotacao;
                                lCustodiaFI.ValorPosicao    = (lCustodia.QtdeAtual * lCustodia.Cotacao);

                                lListaCustodiaFundo.Add(lCustodiaFI);
                            }
                            else if (lCustodia.TipoMercado.Equals("VIS"))
                            {
                                CustodiaAcao lCustodiaAcao = new CustodiaAcao();

                                lCustodiaAcao.Instrumento   = lCustodia.CodigoInstrumento;
                                lCustodiaAcao.Quantidade    = lCustodia.QtdeAtual.DBToInt32();
                                lCustodiaAcao.Preco         = lCustodia.Cotacao;
                                lCustodiaAcao.ValorPosicao  = (lCustodiaAcao.Quantidade * lCustodia.Cotacao);
                                lCustodiaAcao.Cotacao       = lCustodia.Cotacao;
                                lListaCustodiaAcao.Add(lCustodiaAcao);
                            }
                        }

                        if (lCustodia.TipoMercado.Equals("OPC") || lCustodia.TipoMercado.Equals("OPV"))
                        {
                            CustodiaOpcao lCustodiaOpcao = new CustodiaOpcao();

                            lCustodiaOpcao.Instrumento  = lCustodia.CodigoInstrumento;
                            lCustodiaOpcao.Quantidade   = lCustodia.QtdeAtual.DBToInt32();
                            lCustodiaOpcao.Preco        = lCustodia.VlPrecoMedio == null ? 0 : (decimal)lCustodia.VlPrecoMedio;
                            lCustodiaOpcao.ValorPosicao = (lCustodia.QtdeAtual * lCustodia.Cotacao);

                            lListaCustodiaOpcao.Add(lCustodiaOpcao);
                        }

                        if (lCustodia.TipoGrupo.Equals("FUNDO"))
                        {
                            CustodiaFI lCustodiaFundo = new CustodiaFI();

                            lCustodiaFundo.Instrumento = lCustodia.CodigoInstrumento;
                            lCustodiaFundo.Quantidade = lCustodia.QtdeAtual.DBToInt32();
                            lCustodiaFundo.Preco = lCustodia.VlPrecoMedio == null ? 0 : (decimal)lCustodia.VlPrecoMedio;

                            lListaCustodiaFundo.Add(lCustodiaFundo);
                        }
                    }
                }
            }

            lListaCustodiaAcao = AgruparCustodia(lListaCustodiaAcao);
            lListaCustodiaAcao = AbaterLiquidacaoTermo(lListaCustodiaAcao);

            Session["CustodiaAcao"]     = lListaCustodiaAcao;
            Session["CustodiaOpcao"]    = lListaCustodiaOpcao;
            Session["CustodiaFundo"]    = lListaCustodiaFundo;

            if (lListaCustodiaAcao.Count > 0)
            {
                CurrentTotalGeral += lListaCustodiaAcao.AsEnumerable().Sum(x => x.ValorPosicao);
            }

            if (lListaCustodiaOpcao.Count > 0)
            {
                CurrentTotalGeral += lListaCustodiaOpcao.AsEnumerable().Sum(x => x.ValorPosicao);
            }

            if (lListaCustodiaTermo.Count > 0)
            {
                CurrentTotalGeral += lListaCustodiaTermo.AsEnumerable().Sum(x => x.ResultadoTermo);
            }

            if (lListaCustodiaTesouro.Count > 0)
            {
                CurrentTotalGeral += lListaCustodiaTesouro.AsEnumerable().Sum(x => x.ValorPosicao);
            }

            if (lListaCustodiaFundo.Count > 0)
            {
                CurrentTotalGeral += lListaCustodiaFundo.AsEnumerable().Sum(x => x.ValorPosicao);
            }

            return lRetorno;

        }

        public List<CustodiaAcao> AgruparCustodia(List<CustodiaAcao> pCustodia)
        {
            List<CustodiaAcao> lRetorno = new List<CustodiaAcao>();

            foreach (CustodiaAcao lCustodia in pCustodia)
            {
                List<CustodiaAcao> lOcorrencias = lRetorno.AsEnumerable().Where(x => x.Instrumento.Equals(String.Format("{0}", lCustodia.Instrumento))).ToList();
                
                CustodiaAcao lOcorrencia;

                if (lOcorrencias.Count > 0)
                {
                    lOcorrencia = lOcorrencias[0];
                    lOcorrencia.Quantidade += lCustodia.Quantidade;
                    lOcorrencia.ValorPosicao = lOcorrencia.Quantidade * lOcorrencia.Cotacao;
                }
                else
                {
                    lRetorno.Add(lCustodia);
                }
            }

            return lRetorno;
        }

        public List<CustodiaAcao> AbaterLiquidacaoTermo(List<CustodiaAcao> pCustodia)
        {

            foreach (CustodiaTermo lCustodiaTermo in lListaCustodiaTermoALiquidar)
            {
                String lInstrumento = lCustodiaTermo.CodigoNegocio.Substring(0, lCustodiaTermo.CodigoNegocio.Length - 1);
                List<CustodiaAcao> lOcorrencias = pCustodia.AsEnumerable().Where(x => x.Instrumento.Equals(lInstrumento)).ToList();

                if (lOcorrencias.Count > 0)
                {
                    lOcorrencias[0].Quantidade += lCustodiaTermo.QuantidadeDisponivel;
                    lOcorrencias[0].ValorPosicao = (lOcorrencias[0].Quantidade * lOcorrencias[0].Cotacao);
                }
            }


            List<CustodiaAcao> lRetorno = pCustodia.AsEnumerable().Where(x => x.Quantidade != 0).ToList();

            return lRetorno;
        }

        public bool CarregarGarantia(List<Gradual.Site.DbLib.Dados.Garantia> pParametro)
        {
            bool lRetorno = true;

            Garantia lGarantiaItem = new Garantia();

            foreach (Gradual.Site.DbLib.Dados.Garantia lGarantia in pParametro)
            {
                lGarantiaItem.CodigoCliente = "";
                lGarantiaItem.Descricao = "Garantia BOV";
                lGarantiaItem.Valor += lGarantia.Valor;
            }

            if (lGarantiaItem.Valor > 0)
            {
                lListaGarantia.Add(lGarantiaItem);
            }

            if (lChamadaMargem.AsEnumerable().Where(c => c.Bolsa == "BOV").ToList().Count > 0)
            {
                if (lListaGarantia.AsEnumerable().Where(c => c.Descricao.Contains("BOV")).ToList().Count > 0)
                {
                    Garantia lGarantiaBOV    = lListaGarantia.AsEnumerable().Where(c => c.Descricao.Contains("BOV")).ToList()[0];
                    lGarantiaBOV.Valor      += lChamadaMargem.AsEnumerable().Where(c => c.Bolsa == "BOV").ToList()[0].ValorDebito;
                    lGarantiaBOV.Valor      -= lChamadaMargem.AsEnumerable().Where(c => c.Bolsa == "BOV").ToList()[0].ValorCredito;
                }
                else
                {
                    Garantia lGarantiaBOV = new Garantia();

                    lGarantiaBOV.Descricao = "Garantia BOV";
                    lGarantiaBOV.Valor = 0;

                    lGarantiaBOV.Valor += lChamadaMargem.AsEnumerable().Where(c => c.Bolsa == "BOV").ToList()[0].ValorDebito;
                    lGarantiaBOV.Valor -= lChamadaMargem.AsEnumerable().Where(c => c.Bolsa == "BOV").ToList()[0].ValorCredito;

                    lListaGarantia.Add(lGarantiaBOV);
                }
            }

            Session["Garantia"] = lListaGarantia;

            return lRetorno;
        }

        public bool CarregarGarantiaBMF(List<Gradual.Site.DbLib.Dados.GarantiaBMF> pParametro)
        {
            bool lRetorno = true;

            Garantia lGarantiaItem = new Garantia();

            foreach (Gradual.Site.DbLib.Dados.GarantiaBMF lGarantia in pParametro)
            {
                lGarantiaItem.Descricao = "Garantia BMF";
                lGarantiaItem.Valor += lGarantia.Valor;
            }

            if (lGarantiaItem.Valor > 0)
            {
                lListaGarantia.Add(lGarantiaItem);
            }

            if (lChamadaMargem.Count > 0)
            {
                if (lChamadaMargem.AsEnumerable().Where(c => c.Bolsa == "BMF").ToList().Count > 0)
                {
                    if (lListaGarantia.AsEnumerable().Where(c => c.Descricao.Contains("BMF")).ToList().Count > 0)
                    {
                        Garantia lGarantiaBMF = lListaGarantia.AsEnumerable().Where(c => c.Descricao.Contains("BMF")).ToList()[0];
                        lGarantiaBMF.Valor += lChamadaMargem.AsEnumerable().Where(c => c.Bolsa == "BMF").ToList()[0].ValorDebito;
                        lGarantiaBMF.Valor -= lChamadaMargem.AsEnumerable().Where(c => c.Bolsa == "BMF").ToList()[0].ValorCredito;
                    }
                    else
                    {
                        Garantia lGarantiaBMF = new Garantia();

                        lGarantiaBMF.CodigoCliente = "";
                        lGarantiaBMF.Descricao = "Garantia BMF";
                        lGarantiaBMF.Valor = 0;
                        
                        lGarantiaBMF.Valor += lChamadaMargem.AsEnumerable().Where(c => c.Bolsa == "BMF").ToList()[0].ValorDebito;
                        lGarantiaBMF.Valor -= lChamadaMargem.AsEnumerable().Where(c => c.Bolsa == "BMF").ToList()[0].ValorCredito;

                        lListaGarantia.Add(lGarantiaBMF);
                    }

                }
            }

            Session["Garantia"] = lListaGarantia;

            return lRetorno;
        }

        public MonitorLucroPrejuizoResponse ObterLucroPrejuizo(MonitorLucroPrejuizoRequest pRequest)
        {
            IServicoMonitorRisco lServicoRisco = Ativador.Get<IServicoMonitorRisco>();
            return lServicoRisco.ObterMonitorLucroPrejuizo(pRequest);
        }


        public bool CarregarLucroPrejuizo(MonitorLucroPrejuizoResponse pParametro)
        {
            List<string> lstFatorCotacao1000 = new List<string>();

            lstFatorCotacao1000.Add("CEGR3");
            lstFatorCotacao1000.Add("CAFE3");
            lstFatorCotacao1000.Add("CAFE4");
            lstFatorCotacao1000.Add("CBEE3");
            lstFatorCotacao1000.Add("SGEN4");
            lstFatorCotacao1000.Add("PMET6");
            lstFatorCotacao1000.Add("EBTP3");
            lstFatorCotacao1000.Add("EBTP4");
            lstFatorCotacao1000.Add("TOYB3");
            lstFatorCotacao1000.Add("TOYB4");
            lstFatorCotacao1000.Add("FNAM11");
            lstFatorCotacao1000.Add("FNOR11");
            lstFatorCotacao1000.Add("NORD3");

            bool lRetorno = true;

            //List<PosicaoBmfInfo> llist = pParametro.Monitor[0].OrdensBMF;
            Session["LucroPrejuizo"] = pParametro;

            //if (pParametro.Monitor.Count > 0)
            //{
            //    if (pParametro.Monitor[0].OrdensBTC.Count > 0)
            //    {

            //        foreach(BTCInfo lBTC in pParametro.Monitor[0].OrdensBTC)
            //        {
            //            CustodiaBTC lCustodia = new CustodiaBTC();

            //            lCustodia.DataAbertura      = lBTC.DataAbertura.DBToDateTime().ToString("dd/MM/yyyy");
            //            lCustodia.DataVencimento    = lBTC.DataVencimento.DBToDateTime().ToString("dd/MM/yyyy");
            //            lCustodia.CodigoNegocio     = lBTC.Instrumento;
            //            lCustodia.PrecoMedio        = lBTC.PrecoMedio;
            //            lCustodia.                  = lBTC.PrecoMercado;
            //            lCustodia.QuantidadeAcoes   = lBTC.Quantidade;
            //            lCustodia.Remu              = lBTC.Remuneracao;
            //            lCustodia.TaxaRemuneracao   = lBTC.Taxa;
            //            lCustodia.TipoContrato      = lBTC.TipoContrato.ToString();

            //            lListaBTC.Add(lCustodia);
            //        }

            //        decimal SubTotal1 = lListaBTC.AsEnumerable().Where(c => lstFatorCotacao1000.Contains(c.Instrumento)).Sum(c => (c.PrecoMercado * c.Quantidade) / 1000);
            //        decimal SubTotal2 = lListaBTC.AsEnumerable().Where(c => !lstFatorCotacao1000.Contains(c.Instrumento)).Sum(c => (c.PrecoMercado * c.Quantidade));
            //        //CurrentTotalGeral -= SubTotal1 + SubTotal2;
            //    }
            //}

            //Session["CustodiaBTC"] = lListaBTC;

            return lRetorno;

        }

        public PosicaoCotistaViewModel[] ObterPosicaoRendaVariavel(Int32 pCodigo)
        {
            PosicaoCotista.ValidateLogin lLogin = new PosicaoCotista.ValidateLogin();
            lLogin.Username = ConfiguracoesValidadas.UsuarioFinancial;
            lLogin.Password = ConfiguracoesValidadas.SenhaFinancial;

            PosicaoCotistaWSSoapClient lServicoFinancial = new PosicaoCotistaWSSoapClient();
            return lServicoFinancial.Exporta(lLogin, null, 57617, null);
        }

        //public bool CarregarPosicaoRendaVariavel(PosicaoCotistaViewModel[] pParametro)
        public bool CarregarPosicaoRendaVariavel(List<PosicaoFundo> pParametro)
        {
            //bool lRetorno = true;

            //List<FundoPosicao> lListaFundoRendaFixa = new List<FundoPosicao>();
            //List<FundoPosicao> lListaFundoMultimercado = new List<FundoPosicao>();
            //List<FundoPosicao> lListaFundoAcao = new List<FundoPosicao>();

            //PosicaoCotista.PosicaoCotistaViewModel[] lListaTemp = pParametro.OrderByDescending(posicao => posicao.DataConversao).ToArray();

            //for (int i = 0; i < lListaTemp.Count(); i++)
            //{
            //    FundoPosicao lPosicao = new FundoPosicao();

            //    if (lListaTemp[i].ValorBruto == 0) continue;

            //    if (lListaTemp[i].CodigoAnbima == string.Empty) continue;

            //    IntegracaoFundosInfo lFundo = new IntegracaoFundosDbLib().GetNomeRiscoFundo(lListaTemp[i].CodigoAnbima, 0);

            //    if (!lFundo.CodigoFundoItau.Equals(string.Empty)) continue;

            //    lPosicao.DataAtualizacao = lListaTemp[i].DataConversao.ToString("dd/MM/yyyy");
            //    lPosicao.NomeFundo = lListaTemp[i].CodigoAnbima == "" ? "Sem Código ANBIMA" : lFundo.NomeProduto;
            //    lPosicao.IR = lListaTemp[i].ValorIR.ToString("N2");
            //    lPosicao.QtdCotas = lListaTemp[i].Quantidade.ToString("N8");
            //    lPosicao.Risco = lFundo.Risco;
            //    lPosicao.ValorBruto = lListaTemp[i].ValorBruto;
            //    lPosicao.ValorCota = lListaTemp[i].CotaDia;
            //    lPosicao.ValorLiquido = lListaTemp[i].ValorLiquido;
            //    lPosicao.CodigoAnbima = lListaTemp[i].CodigoAnbima;
            //    lPosicao.IdFundo = lFundo.IdProduto.ToString();
            //    lPosicao.IOF = lListaTemp[i].ValorIOF.ToString("N2");
            //    lPosicao.DataProcessamento = lServicoSite.ObterDataProcessamentoFundo(Int32.Parse(lPosicao.IdFundo));

            //    lPosicao.DataPosicao = lServicoSite.ObterDataPosicaoFundo(lPosicao.CodigoAnbima, lPosicao.ValorCota);

            //    if (lFundo.Categoria.IdCategoria.Equals(1))
            //    {
            //        lListaFundoRendaFixa.Add(lPosicao);
            //    }

            //    if (lFundo.Categoria.IdCategoria.Equals(2))
            //    {
            //        lListaFundoMultimercado.Add(lPosicao);
            //    }

            //    if (lFundo.Categoria.IdCategoria.Equals(3))
            //    {
            //        lListaFundoAcao.Add(lPosicao);
            //    }
            //}

            //Session["FundoPosicaoRendaFixa"] = lListaFundoRendaFixa.Count > 0 ? lListaFundoRendaFixa : null;
            //Session["FundoPosicaoMultimercado"] = lListaFundoMultimercado.Count > 0 ? lListaFundoMultimercado : null;
            //Session["FundoPosicaoAcao"] = lListaFundoAcao.Count > 0 ? lListaFundoAcao : null;

            //if (lListaFundoMultimercado.Count > 0)
            //{
            //    CurrentTotalGeral += lListaFundoMultimercado.AsEnumerable().Sum(x => x.ValorLiquido);
            //    //lListaValores
            //}

            //if (lListaFundoRendaFixa.Count > 0)
            //{
            //    CurrentTotalGeral += lListaFundoRendaFixa.AsEnumerable().Sum(x => x.ValorLiquido);
            //}

            //if (lListaFundoAcao.Count > 0)
            //{
            //    CurrentTotalGeral += lListaFundoAcao.AsEnumerable().Sum(x => x.ValorLiquido);
            //}

            //return lRetorno;

            bool lRetorno = true;



            //PosicaoCotista.PosicaoCotistaViewModel[] lListaTemp = pParametro.OrderByDescending(posicao => posicao.DataConversao).ToArray();

            List<PosicaoFundo> lListaTemp = pParametro.OrderByDescending(posicao => posicao.DataConversao).ToList();

            //for (int i = 0; i < lListaTemp.Count(); i++)
            foreach(PosicaoFundo lPosicaoFundo in lListaTemp)
            {
                FundoPosicao lPosicao = new FundoPosicao();

                if (lPosicaoFundo.ValorBruto == 0) continue;

                //if (lPosicaoFundo.CodigoAnbima == string.Empty) continue;
                //if (!lFundo.CodigoFundoItau.Equals(string.Empty)) continue;


                IntegracaoFundosInfo lFundo = new IntegracaoFundosInfo();
                if (!String.IsNullOrEmpty(lPosicaoFundo.CodigoAnbima))
                {
                    lFundo                      = new IntegracaoFundosDbLib().GetNomeRiscoFundo(lPosicaoFundo.CodigoAnbima, 0);
                    lPosicao.IdFundo            = lFundo.IdProduto.ToString();
                    lPosicao.Risco              = lFundo.Risco;
                    
                }

                lPosicao.DataAtualizacao    = lPosicaoFundo.DataConversao.ToString("dd/MM/yyyy");
                //lPosicao.NomeFundo          = lPosicaoFundo.CodigoAnbima == "" ? "Sem Código ANBIMA" : lFundo.NomeProduto;
                lPosicao.NomeFundo          = lPosicaoFundo.Nome;
                lPosicao.IR                 = lPosicaoFundo.ValorIR.ToString("N2");
                lPosicao.QtdCotas           = lPosicaoFundo.Quantidade.ToString("N8");
                lPosicao.ValorBruto         = lPosicaoFundo.ValorBruto;
                lPosicao.ValorCota          = lPosicaoFundo.CotaDia;
                lPosicao.ValorLiquido       = lPosicaoFundo.ValorLiquido;
                lPosicao.CodigoAnbima       = lPosicaoFundo.CodigoAnbima;
                lPosicao.IOF                = lPosicaoFundo.ValorIOF.ToString("N2");
                //lPosicao.DataProcessamento  = lServicoSite.ObterDataProcessamentoFundo(Int32.Parse(lPosicao.IdFundo));                
                lPosicao.Tipo               = lPosicaoFundo.Tipo;
                lPosicao.DataProcessamento = lServicoSite.ObterDataProcessamentoFundo(lPosicaoFundo.IdCarteira);

                lPosicao.DataPosicao = lServicoSite.ObterDataPosicaoFundo(lPosicao.CodigoAnbima, lPosicao.ValorCota);

                //if (!String.IsNullOrEmpty(lPosicaoFundo.CodigoAnbima))
                //{
                //    if (lFundo.Categoria.IdCategoria.Equals(1))
                //    {
                //        lListaFundoRendaFixa.Add(lPosicao);
                //    }

                //    if (lFundo.Categoria.IdCategoria.Equals(2))
                //    {
                //        lListaFundoMultimercado.Add(lPosicao);
                //    }

                //    if (lFundo.Categoria.IdCategoria.Equals(3))
                //    {
                //        lListaFundoAcao.Add(lPosicao);
                //    }
                //}
                //else
                //{
                //    lListaFundoRendaFixa.Add(lPosicao);
                //}


                switch (lPosicaoFundo.Tipo)
                {
                    case 1:
                        lListaFundoAcao.Add(lPosicao);
                        break;
                    case 2:
                        lListaFundoRendaFixa.Add(lPosicao);
                        break;
                    case 3:
                        lListaFundoRendaFixa.Add(lPosicao);
                        break;
                    case 4:
                        lListaFundoMultimercado.Add(lPosicao);
                        break;
                    case 5:
                        lListaFundoOutros.Add(lPosicao);
                        break;
                    case 6:
                        lListaFundoOutros.Add(lPosicao);
                        break;
                    case 7:
                        lListaFundoOutros.Add(lPosicao);
                        break;
                    case 8:
                        lListaFundoOutros.Add(lPosicao);
                        break;
                    case 9:
                        lListaFundoOutros.Add(lPosicao);
                        break;
                    case 10:
                        lListaFundoAcao.Add(lPosicao);
                        break;
                    case 11:
                        lListaFundoOutros.Add(lPosicao);
                        break;
                    case 12:
                        lListaFundoOutros.Add(lPosicao);
                        break;
                    case 13:
                        lListaFundoAcao.Add(lPosicao);
                        break;
                    default:
                        lListaFundoOutros.Add(lPosicao);
                        break;
                }
            }

            Session["FundoPosicaoRendaFixa"] = lListaFundoRendaFixa.Count > 0 ? lListaFundoRendaFixa : null;
            Session["FundoPosicaoMultimercado"] = lListaFundoMultimercado.Count > 0 ? lListaFundoMultimercado : null;
            Session["FundoPosicaoAcao"] = lListaFundoAcao.Count > 0 ? lListaFundoAcao : null;
            Session["FundoPosicaoOutros"] = lListaFundoOutros.Count > 0 ? lListaFundoOutros : null;

            if (lListaFundoMultimercado.Count > 0)
            {
                CurrentTotalGeral += lListaFundoMultimercado.AsEnumerable().Sum(x => x.ValorLiquido);
            }

            if (lListaFundoRendaFixa.Count > 0)
            {
                CurrentTotalGeral += lListaFundoRendaFixa.AsEnumerable().Sum(x => x.ValorLiquido);
            }

            if (lListaFundoAcao.Count > 0)
            {
                CurrentTotalGeral += lListaFundoAcao.AsEnumerable().Sum(x => x.ValorLiquido);
            }
            
            if (lListaFundoOutros.Count > 0)
            {
                CurrentTotalGeral += lListaFundoOutros.AsEnumerable().Sum(x => x.ValorLiquido);
            }

            return lRetorno;
        }

        public List<RendaFixaInfo> ObterPosicaoRendaFixaTitulosPrivados(RendaFixaInfo pRequest)
        {
            return new IntegracaoFundosDbLib().ConsultarRendaFixaTitulosPrivados(pRequest);
        }

        public List<Gradual.Site.DbLib.Dados.CustodiaTesouro> ObterPoscaoRendaFixaTitulosPublicos(RendaFixaInfo pRequest)
        {
            return new IntegracaoFundosDbLib().ConsultarRendaFixaTitulosPublicos(pRequest);
        }

        public bool CarregarPosicaoRendaFixa(List<RendaFixaInfo> pParametro)
        {
            bool lRetorno = true;

            List<CustodiaTesouro> lListaTituloPrivado = new List<CustodiaTesouro>();

            for (int i = 0; i < pParametro.Count(); i++)
            {
                CustodiaTesouro lPosicao = new CustodiaTesouro();

                lPosicao.Instrumento    = pParametro[i].Titulo.ToString();
                lPosicao.Quantidade     = pParametro[i].Quantidade.DBToInt32();
                lPosicao.Preco          = (pParametro[i].ValorOriginal / pParametro[i].Quantidade);
                lPosicao.DataPosicao    = pParametro[i].Aplicacao.DBToDateTime();
                //lPosicao.Emissor        = "n/a";
                lPosicao.Emissor        = pParametro[i].Emissor;
                lPosicao.ValorPosicao   = pParametro[i].ValorOriginal;
                lPosicao.ValorBruto     = pParametro[i].SaldoBruto;
                lPosicao.IR             = pParametro[i].IRRF.ToString();
                lPosicao.IOF            = pParametro[i].IOF.ToString();
                lPosicao.ValorLiquido   = pParametro[i].SaldoLiquido;

                lListaTituloPrivado.Add(lPosicao);
            }

            Session["TituloPrivado"] = lListaTituloPrivado.Count > 0 ? lListaTituloPrivado : null;

            if (lListaTituloPrivado.Count > 0)
            {
                CurrentTotalGeral += lListaTituloPrivado.AsEnumerable().Sum(x => x.ValorBruto);
            }

            return lRetorno;
        }

        public List<ResgateFundo> ObterResgatesFundo(Int32 pRequest)
        {

            List<ResgateFundo> lRetorno = new List<ResgateFundo>();

            try
            {
                lRetorno = lServicoSite.ObterResgateFundo(pRequest);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("CarregarDadaos -> ObterResgatesFundo: {0}-{1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }
    }

    #region Classes auxiliares

    public class MinhaContaCC
    {
        public String SaldoD0_Rotulo    { get; set; }
        public decimal SaldoD0_Valor    { get; set; }

        public String SaldoD1_Rotulo    { get; set; }
        public decimal SaldoD1_Valor    { get; set; }

        public String SaldoD2_Rotulo    { get; set; }
        public decimal SaldoD2_Valor    { get; set; }

        public String SaldoD3_Rotulo    { get; set; }
        public decimal SaldoD3_Valor    { get; set; }

        public MinhaContaCC()
        {
            SaldoD0_Rotulo  = String.Empty;
            SaldoD0_Valor   = 0;

            SaldoD1_Rotulo  = String.Empty;
            SaldoD1_Valor   = 0;

            SaldoD2_Rotulo  = String.Empty;
            SaldoD2_Valor   = 0;

            SaldoD3_Rotulo  = String.Empty;
            SaldoD3_Valor   = 0;
        }
    }

    public class Custodia
    {
        public String Instrumento       { get; set; }
        public decimal Quantidade        { get; set; }
        public decimal PrecoMedio       { get; set; }
        public decimal Preco            { get; set; }
    }

    public class CustodiaAcao: Custodia
    {
        public decimal ValorPosicao     { get; set; }
        public decimal Cotacao          { get; set; }
    }

    //public class CustodiaTermo : Custodia
    //{
    //    public decimal PrecoPagar       { get; set; }
    //    public decimal ValorPagar       { get; set; }
    //    public String Vencimento         { get; set; }
    //    public decimal PrecoMercado     { get; set; }
    //    public decimal ValorMercado     { get; set; }
    //    public decimal ValorPosicao     { get; set; }
    //}

    public class CustodiaOpcao : Custodia
    {
        public decimal ValorPosicao     { get; set; }
    }

    public class CustodiaFI : Custodia
    {
        public decimal ValorPosicao     { get; set; }
    }

    //public class CustodiaBTC : Custodia 
    //{
    //    public String DataVencimento    { get; set; }
    //}

    public class CustodiaFA : Custodia
    {
        public decimal DataPosicao      { get; set; }
        public decimal ValorPosicao     { get; set; }	
        public String IR                { get; set; }	
        public String IOF	            { get; set; }
        public decimal ValorLíquido     { get; set; }
    }

    public class CustodiaRF : Custodia
    {
        public String DataPosicao       { get; set; }
        public decimal ValorPosicao     { get; set; }
        public String IR                { get; set; }
        public String IOF               { get; set; }
        public decimal ValorLíquido     { get; set; }
    }

    public class CustodiaMI : Custodia
    {
        public String DataPosicao       { get; set; }
        public decimal ValorPosicao     { get; set; }
        public String IR                { get; set; }
        public String IOF               { get; set; }
        public decimal ValorLíquido     { get; set; }
    }

    public class CustodiaTesouro : Custodia
    {
        public DateTime DataPosicao     { get; set; }
        public String Emissor           { get; set; }
        public decimal ValorPosicao     { get; set; }
        public decimal ValorBruto       { get; set; }
        public String IR                { get; set; }
        public String IOF               { get; set; }
        public decimal ValorLiquido     { get; set; }
    }

    public class CustodiaTituloPrivado : Custodia
    {
        public DateTime DataPosicao     { get; set; }
        public String Emissor           { get; set; }
        public decimal ValorPosicao     { get; set; }
        public decimal ValorBruto       { get; set; }
        public String IR                { get; set; }
        public String IOF               { get; set; }
        public decimal ValorLiquido     { get; set; }
    }


   

    //public class Garantia
    //{
    //    public String Data              { get; set; }
    //    public String Finalidade        { get; set; }
    //    public String Instrumento       { get; set; }
    //    public String Distribuicao      { get; set; }
    //    public String Quantidade        { get; set; }
    //    public decimal Valor            { get; set; }
    //}

    //public class GarantiaBMF
    //{
    //    public String Codigocliente     { get; set; }
    //    public String Descricao         { get; set; }
    //    public decimal Valor            { get; set; }
    //}

    public class Garantia
    {
        public String CodigoCliente     { get; set; }
        public String Descricao         { get; set; }
        public decimal Valor            { get; set; }
    }

    public enum TipoRendaVariavel
    {
        Acao,
        Termo,
        Opcao
    }

    public class RendaVariavel
    {
        public String Instrumento       { get; set; }
        public String Quantidade        { get; set; }
        public String Preco             { get; set; }
        public decimal Valor            { get; set; }
    }

    public class FundoPosicao
    {
        public String DataAtualizacao       { get; set; }
        public String NomeFundo             { get; set; }
        public String IR                    { get; set; }
        public String QtdCotas              { get; set; }
        public String Risco                 { get; set; }
        public decimal ValorBruto           { get; set; }
        public decimal ValorCota            { get; set; }
        public decimal ValorLiquido         { get; set; }
        public String CodigoAnbima          { get; set; }
        public String IdFundo               { get; set; }
        public String IOF                   { get; set; }
        public DateTime? DataPosicao        { get; set; }
        public DateTime? DataProcessamento  { get; set; }
        public Int32 Tipo                   { get; set; }
    }

    #endregion
}