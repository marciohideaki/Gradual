using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Monitores.Risco.Lib;
using Gradual.Monitores.Risco.Enum;
using System.Globalization;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.OMS.ContaCorrente.Lib.Info;
using Gradual.OMS.Monitor.Custodia.Lib;
using Gradual.OMS.Monitor.Custodia.Lib.Info;
using Gradual.OMS.Monitor.Custodia.Lib.Mensageria;
using Gradual.OMS.Monitor.Custodia.Lib.Util;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteRiscoMonitoramentoLucrosPrejuizos 
    {
        #region | Estruturas

        public struct LucrosPrejuizosInfo
        {
            public string Status { get; set; }

            public string Codigo { get; set; }

            public string CodigoBmf { get; set; }

            public string Cliente { get; set; }

            public string NmCliente { get; set; }

            public string Assessor { get; set; }

            public string NomeAssessor { get; set; }

            public string ContaCorrenteAbertura { get; set; }

            public string CustodiaAbertura { get; set; }

            public string DtAtualizacao { get; set; }

            public string LucroPrejuizoBMF { get; set; }

            public string LucroPrejuizoBOVESPA { get; set; }

            public string LucroPrejuizoTOTAL { get; set; }

            public string NetOperacoes { get; set; }

            public string PatrimonioLiquidoTempoReal { get; set; }

            public string PLAberturaBMF { get; set; }

            public string PLAberturaBovespa { get; set; }

            public string SaldoBMF { get; set; }

            public string SaldoContaMargem { get; set; }

            public string TotalContaCorrenteTempoReal { get; set; }

            public string TotalGarantias { get; set; }

            public string LimiteAVista { get; set; }

            public string LimiteDisponivel { get; set; }

            public string LimiteOpcoes { get; set; }

            public string LimiteTotal { get; set; }

            public string Semaforo { get; set; }

            public string Prejuizo { get; set; }

            public string VolumeTotalFinaceiroBov { get; set; }

            public string VolumeTotalFinaceiroBmf { get; set; }
        }

        public struct OperacoesClienteInfo
        {
            public string  Cliente { get; set; }
            
            public string Cotacao { get; set; }
            
            public string FinanceiroAbertura { get; set; }
            
            public string FinanceiroComprado { get; set; }
            
            public string FinanceiroVendido { get; set; }
            
            public string Instrumento { get; set; }
            
            public string LucroPrejuizo { get; set; }
            
            public string NetOperacao { get; set; }
            
            public string PrecoReversao { get; set; }
            
            public string QtdeAber { get; set; }
            
            public string QtdeAtual { get; set; }
            
            public string QtdeComprada { get; set; }
            
            public string QtdeVendida { get; set; }
            
            public string QtReversao { get; set; }
            
            public string TipoMercado { get; set; }
            
            public string VLMercadoCompra { get; set; }
            
            public string VLMercadoVenda { get; set; }
            
            public string VLNegocioCompra { get; set; }
            
            public string VLNegocioVenda { get; set; }

            public string SubtotalCompra { get; set; }

            public string SubtotalVenda { get; set; }

            public string PrecoMedio { get; set; }

            public string Portas { get; set; }
        }

        public class OperacoesBTCClienteInfo
        {
            public string Carteira { get; set; }
            
            public string CodigoCliente { get; set; }

            public string DataAbertura { get; set; }

            public string DataVencimento { get; set; }
            
            public string Instrumento { get; set; }

            public string PrecoMedio { get; set; }

            public string PrecoMercado { get; set; }

            public string Quantidade { get; set; }

            public string Remuneracao { get; set; }

            public string Taxa { get; set; }
            
            public string TipoContrato { get; set; }

            public string SubtotalQuantidade { get; set; }

            public string SubtotalValor { get; set; }
        }

        public class OperacoesBTCClienteDetalhesInfo
        {
            public string Carteira { get; set; }

            public string CodigoCliente { get; set; }

            public string DataAbertura { get; set; }

            public string DataVencimento { get; set; }

            public string Instrumento { get; set; }

            public string PrecoMedio { get; set; }

            public string PrecoMercado { get; set; }

            public string Quantidade { get; set; }

            public string Remuneracao { get; set; }

            public string Taxa { get; set; }

            public string TipoContrato { get; set; }
        }

        public class OperacoesTermoClienteInfo
        {
            public string DataExecucao { get; set; }

            public string DataVencimento { get; set; }

            public string CodigoCliente { get; set; }

            public string Instrumento { get; set; }

            public string LucroPrejuizo { get; set; }

            public string PrecoExecucao { get; set; }

            public string PrecoMercado { get; set; }

            public string Quantidade { get; set; }

            public string SubtotalLucroPrejuizo { get; set; }

            public string SubtotalQuantidade { get; set; }

            public string SubtotalValor { get; set; }

            public string PrecoMedio { get; set; }

            public string lCount { get; set; }

            public string SubtotalContrato { get; set; }
        }

        public class OperacoesTermoClienteDetalhesInfo
        {
            public string DataExecucao { get; set; }

            public string DataVencimento { get; set; }

            public string DataRolagem { get; set; }

            public string CodigoCliente { get; set; }

            public string Instrumento { get; set; }

            public string LucroPrejuizo { get; set; }

            public string PrecoExecucao { get; set; }

            public string PrecoMercado { get; set; }

            public string Quantidade { get; set; }

            public string SubtotalOperacao { get; set; }
        }

        public class OperacoesDetalhesClienteInfo
        {
            public string Cliente { get; set; }
            
            public string Instrumento { get; set; }
            
            public string LucroPrejuiso { get; set; }
            
            public string PrecoMercado { get; set; }
            
            public string PrecoNegocio { get; set; }
            
            public string Quantidade { get; set; }
            
            public string Sentido { get; set; }
            
            public string TotalMercado { get; set; }
            
            public string TotalNegocio { get; set; }

            public string Porta { get; set; }
        }

        public class MonitoramentoContaCorrenteLimiteGeral
        {
            public string CdCliente { get; set; }

            public string NmCliente { get; set; }

            public string Assessor { get; set; }

            public string DataAtual { get; set; }

            public string StatusBovespa { get; set; }

            public string StatusBmf { get; set; }

            public string CustodiaSubTotalAvista { get; set; }

            public string CustodiaSubTotalOpcoes { get; set; }

            public string CustodiaSubTotalTermo { get; set; }

            public string CustodiaSubTotalBmf { get; set; }

            public string CustodiaSubTotalBmfD_1 { get; set; }

            public string CustodiaSubTotalTesouroDireto { get; set; }

            public string CustodiaSubTotalBTC { get; set; }

            public string CustodiaSubTotalRendaFixa { get; set; }

            public string ContaCorrenteD0 { get; set; }

            public string ContaCorrenteD1 { get; set; }

            public string ContaCorrenteD2 { get; set; }

            public string ContaCorrenteD3 { get; set; }

            public string ContaCorrenteDTotal { get; set; }

            public string ContaCorrenteContaMargem { get; set; }

            public string ContaCorrenteProjetado { get; set; }

            public string ContaCorrenteCompraAvista { get; set; }

            public string ContaCorrenteCompraOpcao { get; set; }

            public string ContaCorrenteVendaOpcao { get; set; }

            public string ContaCorrenteVendaAvista { get; set; }

            public string ContaCorrenteDisponivelAvista { get; set; }

            public string ContaCorrenteDisponivelOpcoes { get; set; }

            public string ContaCorrenteDisponivelBmf { get; set; }

            public string ContaCorrenteGarantiaBmf { get; set; }

            public string ContaCorrenteMargemRequeriada { get; set; }

            public string ContaCorrenteMargemRequeridaBovespa { get; set; }

            public string ContaCorrenteDisponivelBovespa { get; set; }

            public string ContaCorrenteGarantiaBovespa { get; set; }

            public string ContaCorrenteTotalLucroPrejuizo { get; set; }

            public string Financeiro_SFP { get; set; }

            public string OperacoesSubtotalTermo { get; set; }

            public string OperacoesSubtotalBTC { get; set; }

            public List<MonitorRiscoFinanceiroGridInfo> ListaGarantiasBMF { get; set; }

            public List<MonitorRiscoFinanceiroGridInfo> ListaGarantiasBOV { get; set; }

            public MonitorRiscoExtratoLiquidacao ExtratoLiquidacao { get; set; }

            public string DigitoCodigoCliente { get; set; }

            public string ListaPortas { get; set; }

            public string CustodiaSubTotalClubesFundos { get; set; }
        }
        public struct MonitorRiscoFinanceiroGridInfo
        {
            public string  ValorGarantiaDeposito { get; set; }

            public string DescricaoGarantia { get; set; }

            public string DtDeposito { get; set; }

            public string FinalidadeGarantia { get; set; }

            public string CodigoAtividade { get; set; }

            public string CodigoIsin { get; set; }

            public string CodigoDistribuicao { get; set; }

            public string NomeEmpresa { get; set; }

            public string Quantidade { get; set; }
        }

        public struct MonitorRiscoExtratoLiquidacao
        {
            public List<MonitorRiscoExtratoLiquidacaoMovimento> ListaExtratoMovimento { get; set; }

            public string ValorDisponivel { get; set; }

            public string TotalCliente { get; set; }

            public string SaldoAnterior { get; set; }
        }

        public class MonitorRiscoExtratoLiquidacaoMovimento
        {
            public MonitorRiscoExtratoLiquidacaoMovimento() { }

            public MonitorRiscoExtratoLiquidacaoMovimento(ContaCorrenteMovimentoInfo pInfo) 
            {
                this.DataMovimento      = pInfo.DataMovimento.ToString("dd/MM/yyyy");
                this.DataLiquidacao     = pInfo.DataLiquidacao.ToString("dd/MM/yyyy");
                this.DescricaoHistorico = pInfo.Historico;
                this.ValorDebito        = pInfo.ValorDebito.ToString("N2");
                this.ValorCredito       = pInfo.ValorCredito.ToString("N2");
                this.ValorSaldo         = pInfo.ValorSaldo.ToString("N2");
            }

            

            public string DataMovimento { get; set; }

            public string DataLiquidacao { get; set; }

            public string DescricaoHistorico { get; set; }

            public string ValorDebito { get; set; }

            public string ValorCredito { get; set; }

            public string ValorSaldo { get; set; }
        }

        public struct PDLOperacaoGridInfo
        {
            public string CodigoCliente { get; set; }

            public string Contraparte { get; set; }

            public string Criticidade { get; set; }
            
            public string DescricaoCriticidade { get; set; }

            public string HR_NEGOCIO { get; set; }
            
            public string IntencaoPLD { get; set; }
            
            public string Intrumento { get; set; }

            public string LucroPrejuiso { get; set; }

            public string MinutosRestantesPLD { get; set; }

            public string NumeroNegocio { get; set; }

            public string PrecoMercado { get; set; }

            public string PrecoNegocio { get; set; }

            public string QT_CASADA { get; set; }

            public string Quantidade { get; set; }
            
            public string Sentido { get; set; }

            public string Seq { get; set; }
            
            public string STATUS { get; set; }

            public string TipoRegistro { get; set; }

            public string UltimaAtualizacao { get; set; }
        }

        public struct OrdensExecultadasBmfGridInfo
        {
            public string Cliente	                  { get; set; }
		                                                          
            public string Contrato                    { get; set; }
                                                                  
            public string FatorMultiplicador          { get; set; }
                                                                  
            public string DiferencialPontos           { get; set; }
                                                                  
            public string LucroPrejuizoContrato	      { get; set; }
		                                                          
            public string PrecoAquisicaoContrato	  { get; set; }
		                                                          
            public string PrecoContatoMercado	      { get; set; }
		                                                          
            public string QuantidadeContato           { get; set; }

            public string Sentido                     { get; set; }

            public string PrecoMedio                  { get; set; }

            public string Count                       { get; set; }

            public string SubtotalCompra              { get; set; }

            public string SubtotalVenda               { get; set; }

            public string SaldoAnterior               { get; set; }

            public string SaldoAtual                  { get; set; }

            public string Porta                       { get; set; }

            public string CodigoSerie                 { get; set; }

        }

        public struct OrdensExecultadasBmfGridDetalhesInfo
        {
            //public string Id { get; set; }

            public string Sentido { get; set; }

            public string Cliente { get; set; }

            public string Contrato { get; set; }

            public string DataOperacao { get; set; }

            public string DiferencialPontos { get; set; }

            public string FatorMultiplicador { get; set; }

            public string LucroPrejuizoContrato { get; set; }

            public string PrecoAquisicaoContrato { get; set; }

            public string PrecoContatoMercado { get; set; }

            public string QuantidadeContato { get; set; }

            public string Contraparte { get; set; }

        }

        public struct TotalLucroPrejuizoOperacoes
        {
            public string SubtotalVenda { get; set; }

            public string SubtotalCompra { get; set; }

            public string TotalLucroPrejuizo{ get; set; }
            
            public string QuantidadeContratos { get; set; }
            
            public string PrecoMedio { get; set; }
        }
        #endregion

        private CultureInfo gCultura = new CultureInfo("pt-BR");

        #region | Métodos de Tradução
        public static TotalLucroPrejuizoOperacoes TraduzirLucroPrejuizo(decimal pLucroPrejuizo,  decimal pQuantidadeContratos, decimal pPrecoMedio)
        {
            TotalLucroPrejuizoOperacoes lRetorno = new TotalLucroPrejuizoOperacoes();

            lRetorno.PrecoMedio                  = pPrecoMedio.ToString("N2");

            lRetorno.QuantidadeContratos         = pQuantidadeContratos.ToString("N2");

            lRetorno.TotalLucroPrejuizo          = pLucroPrejuizo.ToString("N2");

            return lRetorno;
        }
        public static TotalLucroPrejuizoOperacoes TraduzirLucroPrejuizo(decimal pLucroPrejuizo, int pQuantidadeContratos, decimal pPrecoMedio)
        {
            TotalLucroPrejuizoOperacoes lRetorno = new TotalLucroPrejuizoOperacoes();

            lRetorno.PrecoMedio                  = pPrecoMedio.ToString("N2");
            
            lRetorno.QuantidadeContratos         = pQuantidadeContratos.ToString();

            lRetorno.TotalLucroPrejuizo          = pLucroPrejuizo.ToString("N2");

            return lRetorno;
        }

        public List<OperacoesBTCClienteDetalhesInfo> TraduzirLista(List<BTCInfo> pParametros, string pInstrumento)
        {
            var lRetorno = new List<OperacoesBTCClienteDetalhesInfo>();

            var lLista = pParametros.FindAll(btc => btc.Instrumento == pInstrumento);

            if (null != pParametros && pParametros.Count > 0)
                lLista.ForEach(btc =>
                {
                    OperacoesBTCClienteDetalhesInfo lInfo = new OperacoesBTCClienteDetalhesInfo();

                    lInfo.Carteira       = btc.Carteira.ToString();
                    lInfo.DataVencimento = btc.DataVencimento.ToString("dd/MM/yyyy hh:mm:ss");
                    lInfo.DataAbertura   = btc.DataAbertura.ToString("dd/MM/yyyy hh:mm:ss");
                    lInfo.CodigoCliente  = btc.CodigoCliente.ToString();
                    lInfo.Instrumento    = btc.Instrumento;
                    lInfo.Remuneracao    = btc.Remuneracao.ToString("N2");
                    lInfo.PrecoMedio     = btc.PrecoMedio.ToString("N2");
                    lInfo.PrecoMercado   = btc.PrecoMercado.ToString();
                    lInfo.Quantidade     = string.Format("{0:#,0}", btc.Quantidade);
                    lInfo.Taxa           = btc.Taxa.ToString("N2");
                    lInfo.TipoContrato   = btc.TipoContrato.ToString();

                    lRetorno.Add(lInfo);
                });

            return lRetorno;
        }

        public List<OperacoesBTCClienteInfo> TraduzirLista(List<BTCInfo> pParametros)
        {
            var lRetorno = new List<OperacoesBTCClienteInfo>();

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

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(btc =>
                {
                    var lTemp = lRetorno.Find(ordem => ordem.Instrumento == btc.Instrumento);

                    if (lTemp != null && !string.IsNullOrEmpty(lTemp.Instrumento))
                    {
                        lRetorno.Remove(lTemp);

                        decimal lTempDecimal = Convert.ToDecimal(lTemp.Remuneracao) + btc.Remuneracao;

                        int lTempInt = Convert.ToInt32(lTemp.Quantidade.Replace(".",string.Empty)) + btc.Quantidade;

                        lTemp.Carteira       = btc.Carteira.ToString();
                        lTemp.DataVencimento = btc.DataVencimento.ToString("dd/MM/yyyy hh:mm:ss");
                        lTemp.DataAbertura   = btc.DataAbertura.ToString("dd/MM/yyyy hh:mm:ss");
                        lTemp.CodigoCliente  = btc.CodigoCliente.ToString();
                        lTemp.Instrumento    = btc.Instrumento;
                        lTemp.Remuneracao    = lTempDecimal.ToString("N2");
                        lTemp.PrecoMedio     = btc.PrecoMedio.ToString("N2");
                        lTemp.PrecoMercado   = btc.PrecoMercado.ToString();
                        lTemp.Quantidade     = string.Format ("{0:#,0}",lTempInt);
                        lTemp.Taxa           = btc.Taxa.ToString("N2");
                        lTemp.TipoContrato   = btc.TipoContrato.ToString();

                        lTempDecimal = 0.0M;

                        lTempDecimal = decimal.Parse(lTemp.SubtotalValor) + (btc.PrecoMercado * btc.Quantidade);

                        if (lstFatorCotacao1000.Contains(btc.Instrumento))
                        {
                            lTemp.SubtotalValor = (lTempDecimal / 1000).ToString("N2");
                        }
                        else
                        {
                            lTemp.SubtotalValor = lTempDecimal.ToString("N2");
                        }
                        lTemp.SubtotalQuantidade = string.Format ("{0:#,0}",lTempInt);

                        lRetorno.Add(lTemp);
                    }
                    else
                    {
                        OperacoesBTCClienteInfo lInfo = new OperacoesBTCClienteInfo();

                        lInfo.Carteira           = btc.Carteira.ToString();
                        lInfo.DataVencimento     = btc.DataVencimento.ToString("dd/MM/yyyy hh:mm:ss");
                        lInfo.DataAbertura       = btc.DataAbertura.ToString("dd/MM/yyyy hh:mm:ss");
                        lInfo.CodigoCliente      = btc.CodigoCliente.ToString();
                        lInfo.Instrumento        = btc.Instrumento;
                        lInfo.Remuneracao        = btc.Remuneracao.ToString("N2");
                        lInfo.PrecoMedio         = btc.PrecoMedio.ToString("N2");
                        lInfo.PrecoMercado       = btc.PrecoMercado.ToString();
                        lInfo.Quantidade         = string.Format ("{0:#,0}",btc.Quantidade);
                        lInfo.Taxa               = btc.Taxa.ToString("N2");
                        lInfo.TipoContrato       = btc.TipoContrato.ToString();
                        
                        if (lstFatorCotacao1000.Contains(btc.Instrumento))
                        {
                            lInfo.SubtotalValor = ((btc.PrecoMercado * btc.Quantidade) /1000).ToString("N2");
                        }
                        else
                        {
                            lInfo.SubtotalValor = (btc.PrecoMercado * btc.Quantidade).ToString("N2");
                        }

                        lInfo.SubtotalQuantidade = string.Format ("{0:#,0}",btc.Quantidade);

                        lRetorno.Add(lInfo);
                    }
                });

            return lRetorno;
        }

        public List<OperacoesTermoClienteInfo> TraduzirLista(List<PosicaoTermoInfo> pParametros)
        {
            var lRetorno     = new List<OperacoesTermoClienteInfo>();
            var lRetornoTemp = new List<OperacoesTermoClienteInfo>();

            int lCount = 0;

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(termo =>
                {
                    var lTemp = lRetornoTemp.Find(ordem => ordem.Instrumento == termo.Instrumento);

                    if (lTemp !=null && !string.IsNullOrEmpty(lTemp.Instrumento))
                    {
                        lRetornoTemp.Remove(lTemp);

                        decimal lTempDecimal = 0.0M;

                        int lTempInt = 0;

                        lCount = int.Parse(lTemp.lCount )+1;

                        lTemp.DataExecucao   = termo.DataExecucao.ToString("dd/MM/yyyy hh:mm:ss");
                        lTemp.DataVencimento = termo.DataVencimento.ToString("dd/MM/yyyy hh:mm:ss");
                        lTemp.CodigoCliente  = termo.IDCliente.ToString();
                        lTemp.Instrumento    = termo.Instrumento;
                        //lTemp.LucroPrejuizo  = termo.LucroPrejuizo.ToString("N2");
                        lTemp.PrecoExecucao  = termo.PrecoExecucao.ToString();
                        lTemp.PrecoMercado   = termo.PrecoMercado.ToString();

                        lTempInt = int.Parse(lTemp.SubtotalQuantidade.Replace(".",string.Empty)) + termo.Quantidade;

                        lTemp.SubtotalQuantidade = string.Format("{0:#,0}", lTempInt);

                        lTempDecimal = 0.0M;

                        lTempDecimal = decimal.Parse(lTemp.SubtotalValor) + termo.PrecoExecucao;

                        lTemp.SubtotalValor = lTempDecimal.ToString("N2");

                        lTempDecimal = 0.0M;

                        lTempDecimal = Convert.ToDecimal(lTemp.SubtotalLucroPrejuizo) + termo.LucroPrejuizo;

                        lTemp.SubtotalLucroPrejuizo = lTempDecimal.ToString("N2");

                        decimal ltempSubTotalContrato = (termo.Quantidade * termo.PrecoExecucao);

                        lTemp.SubtotalContrato = (Convert.ToDecimal(lTemp.SubtotalContrato, gCultura) + ltempSubTotalContrato).ToString("N2");

                        lTemp.lCount = lCount.ToString();

                        lRetornoTemp.Add(lTemp);

                        
                    }
                    else
                    {
                        OperacoesTermoClienteInfo lInfo = new OperacoesTermoClienteInfo();

                        lCount = 1;

                        lInfo.DataExecucao          = termo.DataExecucao.ToString("dd/MM/yyyy hh:mm:ss");
                        lInfo.DataVencimento        = termo.DataVencimento.ToString("dd/MM/yyyy hh:mm:ss");
                        lInfo.CodigoCliente         = termo.IDCliente.ToString();
                        lInfo.Instrumento           = termo.Instrumento;
                        lInfo.LucroPrejuizo         = termo.LucroPrejuizo.ToString("N2");
                        lInfo.PrecoExecucao         = termo.PrecoExecucao.ToString();
                        lInfo.PrecoMercado          = termo.PrecoMercado.ToString();
                        lInfo.Quantidade            = termo.Quantidade.ToString();
                        lInfo.SubtotalValor         = termo.PrecoExecucao.ToString("N2");
                        lInfo.SubtotalLucroPrejuizo = termo.LucroPrejuizo.ToString("N2");
                        lInfo.SubtotalQuantidade    = termo.Quantidade.ToString();
                        lInfo.lCount                = lCount.ToString();
                        lInfo.SubtotalContrato      = (termo.Quantidade * termo.PrecoExecucao).ToString("N2");
                        lRetornoTemp.Add(lInfo);
                    }
                });

            for (int i = 0; i < lRetornoTemp.Count; i++)
            {
                OperacoesTermoClienteInfo termo = new OperacoesTermoClienteInfo();

                termo = lRetornoTemp[i];

                termo.PrecoMedio = (Math.Round(Convert.ToDecimal(lRetornoTemp[i].SubtotalValor) / Convert.ToInt32(lRetornoTemp[i].lCount),2)).ToString();

                lRetorno.Add(termo);
            }

            return lRetorno;
        }

        public List<OperacoesTermoClienteDetalhesInfo> TraduzirLista(List<PosicaoTermoInfo> pParametros, string pInstrumento, List<DateTime> lFeriados)
        {
            var lRetorno = new List<OperacoesTermoClienteDetalhesInfo>();

            var lLista = pParametros.FindAll(termo => termo.Instrumento== pInstrumento);

            if (null != pParametros && pParametros.Count > 0)
                lLista.ForEach(termo =>
                {
                    OperacoesTermoClienteDetalhesInfo lInfo = new OperacoesTermoClienteDetalhesInfo();

                    lInfo.DataExecucao   = termo.DataExecucao.ToString("dd/MM/yyyy");
                    lInfo.DataVencimento = termo.DataVencimento.ToString("dd/MM/yyyy");

                    DateTime lDataVencimento = termo.DataVencimento;

                    DateTime lDataRolagem = termo.DataVencimento;//.AddDays(-3);

                    bool lEFeriado = lFeriados.Contains(lDataRolagem);

                    lEFeriado = lFeriados.Contains(lDataRolagem);

                    bool lEhDiaUtilIntervaloValido = false;

                    int lDiasUteis = 0;

                    //Primeiro elelimina sabados domigos e feriados do intervalo
                    while (lDiasUteis < 3)
                    {
                        lDataRolagem = lDataRolagem.AddDays(-1);

                        lEhDiaUtilIntervaloValido = lDataRolagem.DayOfWeek == DayOfWeek.Saturday ||
                            lDataRolagem.DayOfWeek == DayOfWeek.Sunday || lFeriados.Contains(lDataRolagem);

                        if (!lEhDiaUtilIntervaloValido)
                            lDiasUteis++;
                    }

                    lEFeriado = lFeriados.Contains(lDataVencimento);

                    while (lEFeriado || lDataVencimento.DayOfWeek == DayOfWeek.Saturday || 
                        lDataVencimento.DayOfWeek == DayOfWeek.Sunday ||
                        (dateDiff('d', lDataRolagem, lDataVencimento) < 2))
                    {
                        lDataVencimento = lDataVencimento.AddDays(1);

                        lEFeriado = lFeriados.Contains(lDataVencimento);
                    }
                    
                    lInfo.DataRolagem      = lDataRolagem.ToString("dd/MM/yyyy");
                    lInfo.CodigoCliente    = termo.IDCliente.ToString();
                    lInfo.Instrumento      = termo.Instrumento;
                    lInfo.LucroPrejuizo    = termo.LucroPrejuizo.ToString("N2");
                    lInfo.PrecoExecucao    = termo.PrecoExecucao.ToString();
                    lInfo.PrecoMercado     = termo.PrecoMercado.ToString();
                    lInfo.Quantidade       = string.Format("{0:#,0}",termo.Quantidade);
                    lInfo.SubtotalOperacao = (termo.Quantidade * termo.PrecoExecucao).ToString("N2");

                    lRetorno.Add(lInfo);
                });

            return lRetorno;
        }
        
        public int dateDiff(char charInterval, DateTime dttFromDate, DateTime dttToDate)
        {
            TimeSpan tsDuration;
            tsDuration = dttToDate - dttFromDate;

            if (charInterval == 'd')
            {
                // Resultado em Dias
                return tsDuration.Days;
            }
            else if (charInterval == 'm')
            {
                // Resultado em Meses
                double dblValue = 12 * (dttFromDate.Year - dttToDate.Year) + dttFromDate.Month - dttToDate.Month;
                return Convert.ToInt32(Math.Abs(dblValue));
            }
            else if (charInterval == 'y')
            {
                // Resultado em Anos
                return Convert.ToInt32((tsDuration.Days) / 365);
            }
            else
            {
                return 0;
            }
        }

        public List<OrdensExecultadasBmfGridDetalhesInfo> TraduzirLista(List<PosicaoBmfInfo> pParametros, string lInstrumento)
        {
            var lRetorno = new List<OrdensExecultadasBmfGridDetalhesInfo>();

            int lCont = 0;

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(pld =>
                {
                    lRetorno.Add(new OrdensExecultadasBmfGridDetalhesInfo()
                    {
                        //Id                       = string.Format(pld.Contrato, "_", lCont),
                        Cliente	                 = pld.Cliente                 .ToString(),
                        Contrato                 = pld.Contrato                .ToString(),
                        DataOperacao	         = pld.DataOperacao            .ToString("dd/MM/yyyy hh:mm:ss"),
                        DiferencialPontos        = pld.DiferencialPontos       .ToString(),
                        FatorMultiplicador       = pld.FatorMultiplicador      .ToString(),
                        LucroPrejuizoContrato	 = pld.LucroPrejuizoContrato   .ToString(),
                        PrecoAquisicaoContrato   = pld.PrecoAquisicaoContrato  .ToString(),
                        PrecoContatoMercado	     = pld.PrecoContatoMercado     .ToString(),
                        QuantidadeContato        = pld.QuantidadeContato       .ToString(),
                        Sentido                  = pld.Sentido                 .ToString(),
                        Contraparte              = pld.Contraparte             .ToString()
                    });

                    lCont++;
                });

            return lRetorno;
        }

        public List<OrdensExecultadasBmfGridInfo> TraduzirLista(List<PosicaoBmfInfo> pParametros)
        {
            var lTempRetorno = new List<OrdensExecultadasBmfGridInfo>();
            var lRetorno = new List<OrdensExecultadasBmfGridInfo>();

            int lCont = 0;

            if (null != pParametros && pParametros.Count > 0)
            {
                pParametros.ForEach(pld =>
                {
                    var lTemp = lTempRetorno.Find(ordem => ordem.Contrato == pld.Contrato);

                    lCont++;

                    if (!string.IsNullOrEmpty(lTemp.Contrato))
                    {
                        lTempRetorno.Remove(lTemp);

                        lTemp.PrecoMedio = (decimal.Parse(lTemp.PrecoMedio) + (pld.PrecoAquisicaoContrato * pld.QuantidadeContato)).ToString("N2");

                        if (pld.Sentido == "V")
                        {
                            lTemp.SubtotalVenda         = ((Convert.ToDecimal(lTemp.SubtotalVenda ) + pld.LucroPrejuizoContrato)).ToString("N2");
                            decimal lTempDecimal        = Convert.ToDecimal(lTemp.LucroPrejuizoContrato) + pld.LucroPrejuizoContrato;
                            lTemp.LucroPrejuizoContrato = lTempDecimal.ToString("N2");
                            lTempDecimal                = Convert.ToDecimal(lTemp.QuantidadeContato) - pld.QuantidadeContato;
                            lTemp.QuantidadeContato     = lTempDecimal.ToString();
                            
                        }
                        else if (pld.Sentido == "C")
                        {
                            lTemp.SubtotalCompra        = (Convert.ToDecimal(lTemp.SubtotalCompra) + pld.LucroPrejuizoContrato).ToString("N2");
                            decimal lTempDecimal        = Convert.ToDecimal(lTemp.QuantidadeContato) + pld.QuantidadeContato;
                            lTemp.QuantidadeContato     = lTempDecimal.ToString();
                            lTempDecimal                = Convert.ToDecimal(lTemp.LucroPrejuizoContrato) + pld.LucroPrejuizoContrato;
                            lTemp.LucroPrejuizoContrato = lTempDecimal.ToString("N2");
                        }

                        lTemp.Count = (int.Parse(lTemp.Count) + pld.QuantidadeContato).ToString();

                        lTemp.DiferencialPontos = (pld.PrecoContatoMercado - Convert.ToDecimal(lTemp.PrecoMedio, new CultureInfo("pt-BR"))).ToString("N2");

                        lTempRetorno.Add(lTemp);

                    }
                    else
                    {
                        lCont = 0;

                        var lOrdem = new OrdensExecultadasBmfGridInfo();

                        lOrdem.Cliente                = pld.Cliente.ToString()                 ;
                        lOrdem.Contrato               = pld.Contrato.ToString()                ;
                        lOrdem.FatorMultiplicador     = pld.FatorMultiplicador.ToString()      ;
                        lOrdem.LucroPrejuizoContrato  = pld.LucroPrejuizoContrato.ToString("N2")   ;
                        lOrdem.PrecoAquisicaoContrato = pld.PrecoAquisicaoContrato.ToString("N2")  ;
                        lOrdem.PrecoContatoMercado    = pld.PrecoContatoMercado.ToString("N2")     ;
                        lOrdem.Sentido                = pld.Sentido;
                        lOrdem.SubtotalCompra         = "0,00";
                        lOrdem.SubtotalVenda          = "0,00";

                        if (pld.Sentido == "C")
                        {
                            lOrdem.QuantidadeContato    = pld.QuantidadeContato.ToString();
                            lOrdem.SubtotalCompra       = (pld.LucroPrejuizoContrato).ToString("N2");
                        }
                        else if (pld.Sentido == "V")
                        {
                            lOrdem.QuantidadeContato = "-" + pld.QuantidadeContato.ToString();
                            lOrdem.SubtotalVenda     = pld.LucroPrejuizoContrato.ToString("N2");
                        }

                        lOrdem.PrecoMedio        = (pld.PrecoAquisicaoContrato * pld.QuantidadeContato).ToString();
                        lOrdem.Count             = pld.QuantidadeContato.ToString();
                        lOrdem.CodigoSerie       = pld.CodigoSerie;
                        lOrdem.DiferencialPontos = (pld.PrecoContatoMercado - Convert.ToDecimal(lOrdem.PrecoMedio)).ToString("N2");

                        lTempRetorno.Add(lOrdem);
                    }
                });

                    for (int i = 0; i < lTempRetorno.Count; i++ )
                    {
                        OrdensExecultadasBmfGridInfo bmf = new OrdensExecultadasBmfGridInfo();
                        bmf                              = lTempRetorno[i];
                        bmf.PrecoMedio                   = (Convert.ToDecimal(lTempRetorno[i].PrecoMedio) / int.Parse(lTempRetorno[i].Count)).ToString("N2");
                        bmf.DiferencialPontos            = (Convert.ToDecimal(lTempRetorno[i].PrecoContatoMercado) - Convert.ToDecimal(bmf.PrecoMedio)).ToString();
                        //bmf.SubtotalCompra               = (Convert.ToDecimal(bmf.SubtotalCompra) * -1).ToString("N2");
                        lRetorno.Add(bmf);
                    }
            }

            return lRetorno;
        }

        public List<PDLOperacaoGridInfo> TraduzirLista(List<PLDOperacaoInfo> pParametros)
        {
            var lRetorno = new List<PDLOperacaoGridInfo>();

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(pld =>
                {
                    lRetorno.Add(new PDLOperacaoGridInfo()
                    {
                        CodigoCliente         = pld.CodigoCliente          .ToString(),
                        Contraparte           = pld.Contraparte            .ToString(),
                        Criticidade           = pld.Criticidade            .ToString(),
                        DescricaoCriticidade  = pld.DescricaoCriticidade              ,
                        HR_NEGOCIO            = pld.HR_NEGOCIO             .ToString("dd/MM/yyyy hh:mm:ss"),
                        IntencaoPLD           = pld.IntencaoPLD            .ToString(),
                        Intrumento            = pld.Intrumento             .ToString(),
                        LucroPrejuiso         = pld.LucroPrejuiso          .ToString(),
                        MinutosRestantesPLD   = pld.MinutosRestantesPLD    .ToString(),
                        NumeroNegocio         = pld.NumeroNegocio          .ToString(),
                        PrecoMercado          = pld.PrecoMercado           .ToString("N2"),
                        PrecoNegocio          = pld.PrecoNegocio           .ToString("N2"),
                        QT_CASADA             = pld.QT_CASADA              .ToString(),
                        Quantidade            = pld.Quantidade             .ToString(),
                        Sentido               = pld.Sentido                .ToString(),
                        Seq                   = pld.Seq                    .ToString(),
                        STATUS                = pld.STATUS                 .ToString(),
                        TipoRegistro          = pld.TipoRegistro           .ToString(),
                        UltimaAtualizacao     = pld.UltimaAtualizacao      .ToString("dd/MM/yyyy hh:mm:ss"),

                    });
                });

            return lRetorno;
        }

        public List<OperacoesDetalhesClienteInfo> TraduzirLista(List<OperacoesInfo> pParametros)
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

            var lRetorno = new List<OperacoesDetalhesClienteInfo>();

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(plm =>
                {
                    lRetorno.Add(new OperacoesDetalhesClienteInfo()
                    {
                        Cliente       = plm.Cliente.ToString(),
                        Instrumento   = plm.Instrumento,
                        LucroPrejuiso = plm.LucroPrejuiso.ToString("N2"),
                        PrecoMercado  = plm.PrecoMercado.ToString("N2"),
                        PrecoNegocio  = plm.PrecoNegocio.ToString("N2"),
                        Quantidade    = (lstFatorCotacao1000.Contains(plm.Instrumento)) ? (plm.Quantidade * 1000 ).ToString(): plm.Quantidade.ToString(),
                        Sentido       = plm.Sentido,
                        TotalMercado  = plm.TotalMercado.ToString("N2"),
                        TotalNegocio  = plm.TotalNegocio.ToString("N2"),
                        Porta         = plm.Porta
                    });
                });

            return lRetorno;
        }

        public List<LucrosPrejuizosInfo> TraduzirLista(List<ExposicaoClienteInfo> pParametros)
        {
            var lRetorno = new List<LucrosPrejuizosInfo>();

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(plm =>
                {
                    if (plm.Cliente != null)
                    {
                        lRetorno.Add(new LucrosPrejuizosInfo()
                        {
                            Assessor                    = plm.Assessor,
                            NomeAssessor                = plm.NomeAssessor,
                            Cliente                     = plm.Cliente.ToStringFormatoNome(),
                            NmCliente                   = plm.Cliente.ToString(),
                            CodigoBmf                   = plm.CodigoBMF.ToString(),
                            Codigo                      = plm.CodigoBovespa.ToString(),
                            ContaCorrenteAbertura       = plm.ContaCorrenteAbertura.ToString("N2"),
                            CustodiaAbertura            = plm.CustodiaAbertura.ToString("N2"),
                            DtAtualizacao               = plm.DtAtualizacao.ToString("dd/MM/yyyy"),
                            LucroPrejuizoBMF            = plm.LucroPrejuizoBMF.ToString("N2"),
                            LucroPrejuizoBOVESPA        = plm.LucroPrejuizoBOVESPA.ToString("N2"),
                            LucroPrejuizoTOTAL          = plm.LucroPrejuizoTOTAL.ToString("N2"),
                            NetOperacoes                = plm.NetOperacoes.ToString("N2"),
                            PatrimonioLiquidoTempoReal  = plm.PatrimonioLiquidoTempoReal.ToString("N2"),
                            PLAberturaBMF               = plm.PLAberturaBMF.ToString("N2"),
                            PLAberturaBovespa           = plm.PLAberturaBovespa.ToString("N2"),
                            SaldoBMF                    = plm.SaldoBMF.ToString("N2"),
                            SaldoContaMargem            = plm.SaldoContaMargem.ToString("N2"),
                            TotalContaCorrenteTempoReal = plm.TotalContaCorrenteTempoReal.ToString("N2"),
                            TotalGarantias              = plm.TotalGarantias.ToString("N2"),
                            LimiteAVista                = plm.LimitesOperacionais != null ? plm.LimitesOperacionais.LimiteAVista.ToString("N2") : "0",
                            LimiteDisponivel            = plm.LimitesOperacionais != null ? plm.LimitesOperacionais.LimiteDisponivel.ToString("N2") : "0",
                            LimiteOpcoes                = plm.LimitesOperacionais != null ? plm.LimitesOperacionais.LimiteOpcoes.ToString("N2") : "0",
                            LimiteTotal                 = plm.LimitesOperacionais != null ? plm.LimitesOperacionais.LimiteTotal.ToString("N2") : "0",
                            Semaforo                    = plm.Semaforo.ToString(),
                            Prejuizo                    = string.Concat(plm.PercentualPrejuiso.ToString("N2"), " %"),
                            VolumeTotalFinaceiroBmf     = plm.VolumeTotalFinanceiroBmf.ToString("N2"),
                            VolumeTotalFinaceiroBov     = plm.VolumeTotalFinanceiroBov.ToString("N2")
                        });
                    }
                });

            return lRetorno;
        }

        public List<OperacoesClienteInfo> TraduzirLista(List<ClienteRiscoResumo> pParametros, List<OperacoesInfo> pOrdensExecutadas)
        {
            var lRetorno = new List<OperacoesClienteInfo>();

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

            if (null != pParametros && pParametros.Count > 0)
            {
                pParametros.ForEach(plm =>
                {
                    OperacoesClienteInfo lInfo = new OperacoesClienteInfo();

                    lInfo.Cliente            = plm.Cliente.ToString();
                    lInfo.Cotacao            = plm.Cotacao.ToString("N2");
                    lInfo.FinanceiroAbertura = plm.FinanceiroAbertura.ToString("N2");
                    lInfo.FinanceiroComprado = plm.FinanceiroComprado.ToString("N2");
                    lInfo.FinanceiroVendido  = plm.FinanceiroVendido.ToString("N2");
                    lInfo.Instrumento        = plm.Instrumento;
                    lInfo.LucroPrejuizo      = plm.LucroPrejuizo.ToString("N2");
                    lInfo.NetOperacao        = plm.NetOperacao.ToString("N2");
                    lInfo.PrecoReversao      = plm.PrecoReversao.ToString("N2");
                    lInfo.QtdeAber           = (plm.QtdeAber.ToString() == string.Empty) ? "0" : String.Format(gCultura, "{0:#,0}",plm.QtdeAber);
                    
                    if (!lstFatorCotacao1000.Contains(plm.Instrumento))
                    {
                        lInfo.QtdeAtual    = String.Format(gCultura,"{0:#,0}", plm.QtdeAtual);
                        lInfo.QtdeComprada = String.Format(gCultura,"{0:#,0}", plm.QtdeComprada);
                        lInfo.QtdeVendida  = String.Format(gCultura,"{0:#,0}", plm.QtdeVendida); 
                        lInfo.QtReversao   = String.Format(gCultura,"{0:#,0}", plm.QtReversao);
                    }
                    else
                    {
                        lInfo.QtdeAtual    = String.Format(gCultura,"{0:#,0}", (plm.QtdeAtual    * 1000 ));
                        lInfo.QtdeComprada = String.Format(gCultura,"{0:#,0}", (plm.QtdeComprada * 1000 ));
                        lInfo.QtdeVendida  = String.Format(gCultura,"{0:#,0}", (plm.QtdeVendida  * 1000 ));
                        lInfo.QtReversao   = String.Format(gCultura,"{0:#,0}", (plm.QtReversao   * 1000 ));
                    }

                    lInfo.TipoMercado        = plm.TipoMercado;
                    lInfo.VLMercadoCompra    = plm.VLMercadoCompra.ToString("N2");
                    lInfo.VLMercadoVenda     = plm.VLMercadoVenda.ToString("N2");
                    lInfo.VLNegocioVenda     = plm.VLNegocioVenda.ToString("N2");

                    List<OperacoesInfo> lOrdens = pOrdensExecutadas.FindAll(ordens => ordens.Instrumento == lInfo.Instrumento);

                    decimal lSubtotalCompra = 0.0M;

                    decimal lSubtotalVenda = 0.0M;

                    decimal lPrecoMedio = 0.0M;

                    decimal lQuantPrecoMedio = 0.0M;

                    lOrdens.ForEach(ordens =>
                    {
                        if (ordens.Sentido == "V")
                        {
                            lSubtotalVenda += (ordens.Quantidade * (ordens.PrecoMercado - ordens.PrecoNegocio )) *-1;
                        }
                        else if (ordens.Sentido == "C")
                        {
                            lSubtotalCompra += (ordens.Quantidade * (ordens.PrecoMercado - ordens.PrecoNegocio));
                        }

                        lPrecoMedio += ordens.Quantidade * ordens.PrecoNegocio;

                        lQuantPrecoMedio += ordens.Quantidade;

                        string lPorta = ordens.Porta;//.Substring(0,ordens.Porta.IndexOf(' '));

                        if (lInfo.Portas == null)
                        {
                            lInfo.Portas = string.Empty;
                        }

                        if (!lInfo.Portas.Contains(lPorta))
                        {
                            lInfo.Portas += string.Concat(lPorta,",");
                        }
                        if (!string.IsNullOrEmpty(lInfo.Portas) && lInfo.Portas.LastIndexOf(',') > -1)
                        {
                            lInfo.Portas = lInfo.Portas.Remove(lInfo.Portas.LastIndexOf(','));
                        }
                    });

                    lInfo.PrecoMedio = (lQuantPrecoMedio > 0) ? (lPrecoMedio / lQuantPrecoMedio).ToString("N2") : "0,00";

                    lInfo.SubtotalCompra = lSubtotalCompra.ToString("N2");

                    lInfo.SubtotalVenda = lSubtotalVenda.ToString("N2");

                    lRetorno.Add(lInfo);

                });
            }

            return lRetorno;
        }
        
        private string RecuperarSemaforoPerda(decimal pPl, decimal pLucroPrejuizo)
        {
            var lRetorno = "SemaforoAmarelo";

            if (pLucroPrejuizo > 0 || Math.Abs(pLucroPrejuizo) < (pPl * .7m))
            {
                lRetorno = "SemaforoVerde";
            }
            else if (Math.Abs(pLucroPrejuizo) >= pPl)
            {
                lRetorno = "SemaforoVermelho";
            }

            return lRetorno;
        }

        #endregion

        
    }
}
