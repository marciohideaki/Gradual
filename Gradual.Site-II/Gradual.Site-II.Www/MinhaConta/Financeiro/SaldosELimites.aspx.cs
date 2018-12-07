using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.Risco.Regra.Lib.Mensagens;
using Gradual.Site.DbLib.Mensagens;
using Gradual.OMS.Risco.Regra;
using Gradual.OMS.ContaCorrente.Lib;
using System.Text;
using Gradual.OMS.Custodia.Lib;
using Gradual.OMS.Custodia.Lib.Mensageria;
using Gradual.OMS.Custodia.Lib.Info;
using System.Globalization;
using Gradual.OMS.Cotacao.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.Site.DbLib.Persistencias.MinhaConta;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.OMS.Monitor.Custodia.Lib.Mensageria;
using Gradual.OMS.Monitor.Custodia.Lib;
using Gradual.OMS.Monitor.Custodia.Lib.Info;
using Gradual.Site.DbLib.Dados.MinhaConta;
using Gradual.OMS.Library;
using Microsoft.Reporting.WebForms;
using Gradual.OMS.ContaCorrente.Lib.Info;
using Gradual.OMS.ContaCorrente.Lib.Enum;
using Gradual.OMS.Email.Lib;


namespace Gradual.Site.Www.MinhaConta.Financeiro
{
    public partial class SaldosELimites : PaginaBase
    {
        #region Globais

        private List<RiscoLimiteAlocadoInfo> gRetornoLimitePorCliente;
        private List<TransporteConfigurarLimites> gTransporteConfigurarLimites;

        protected string cssLimiteAcoes = "";
        protected string cssLimiteAcoesD0 = "";
        protected string cssLimiteAcoesD1 = "";
        protected string cssLimiteAcoesD2 = "";
        protected string cssLimiteAcoesD3 = "";
        protected string cssSaldoProjetado = "";
        protected string cssAcoesLimiteTotalAVista = "";
        protected string cssLimiteAcoesCompraAlocado = "";
        protected string cssLimiteAcoesCompra = "";
        protected string cssLimiteAcoesCompraTotal = "";
        protected string cssLimiteOpcoesCompraAlocado = "";
        protected string cssLimiteOpcoesCompra = "";
        protected string cssLimiteOpcoesCompraTotal = "";
        protected string cssLimiteAcoesVendaAlocado = "";
        protected string cssLimiteAcoesVenda = "";
        protected string cssLimiteAcoesVendaTotal = "";
        protected string cssLimiteOpcoesVendaAlocado = "";
        protected string cssLimiteOpcoesVenda = "";
        protected string cssLimiteOpcoesVendaTotal = "";



        protected string cssOpcoesLimiteTotal = "";
        protected string cssAcoesContaMargem = "";
        protected string cssAcoesLimiteCompra = "";
        protected string cssAcoesLimiteVenda = "";
        protected string cssAcoesSaldoBloqueado = "";
        protected string cssLimiteOpcoes = "";
        protected string cssLimiteOpcoesD0 = "";
        protected string cssLimiteOpcoesD1 = "";
        protected string cssOpcoesLimiteCompra = "";
        protected string cssOpcoesLimiteVenda = "";
        protected string cssOpcoesSaldoBloqueado = "";
        protected string cssAcoesLimiteParaCompra = "";
        protected string cssAcoesLimiteParaVenda = "";
        protected string cssOpcoesLimiteParaCompra = "";
        protected string cssOpcoesLimiteParaVenda = "";
        protected string cssSaldoTotal = "";
        protected string cssSomatoriaCustodia = "";

        #endregion

        #region Propriedades

        protected TransporteSaldoDeConta TransporteSaldoDeConta { get; set; }

        
        #endregion

        #region Metodos Private
        
        private List<TransporteCustodiaInfo> BuscarCustodiasDoCliente()
        {
            MonitorCustodiaRequest lRequest          = new MonitorCustodiaRequest();

            MonitorCustodiaResponse lResponse        = new MonitorCustodiaResponse();
            
            IServicoMonitorCustodia gServicoCustodia = Ativador.Get<IServicoMonitorCustodia>();
            
            lRequest.CodigoCliente                   = base.SessaoClienteLogado.CodigoPrincipal.DBToInt32();
            
            lResponse                                = gServicoCustodia.ObterMonitorCustodiaMemoria(lRequest);

            List<TransporteCustodiaInfo> lRetorno = new List<TransporteCustodiaInfo>();

            IEnumerable<MonitorCustodiaInfo.CustodiaPosicao> Lista = from a in lResponse.MonitorCustodia.ListaCustodia orderby a.Resultado descending select a;

            lRetorno = TransporteCustodiaInfo.TraduzirCustodiaInfo(Lista.ToList());

            return lRetorno;
        }

        private Dictionary<string, decimal> CarregarCustodiaComoPorcentagens()
        {
            List<TransporteCustodiaInfo> lCustodia = BuscarCustodiasDoCliente();

            Dictionary<string, decimal> lRetorno = new Dictionary<string, decimal>();
            Dictionary<string, decimal> lAgrupadoPorPapel = new Dictionary<string, decimal>();
            Dictionary<string, decimal> lPrecosPorPapel = new Dictionary<string, decimal>();

            CultureInfo lBR = new CultureInfo("pt-BR");

            int lQuantidade;

            decimal lValorTotal = 0;
            decimal lPrecoPapel;

            string lPapel, lCotacao;

            TransporteMensagemDeNegocio lMsgCotacao;

            IServicoCotacao lServico = InstanciarServicoDoAtivador<IServicoCotacao>();

            //Roda todos os itens da custódia pegando o preço atual de cada papel:

            foreach (TransporteCustodiaInfo lTransporte in lCustodia)
            {
                try
                {
                    lTransporte.CodigoNegocio = lTransporte.CodigoNegocio.Trim();

                    if (!lPrecosPorPapel.ContainsKey(lTransporte.CodigoNegocio))
                        lPrecosPorPapel.Add(lTransporte.CodigoNegocio, 0);

                    lCotacao = lServico.ReceberTickerCotacao(lTransporte.CodigoNegocio);

                    lMsgCotacao = new TransporteMensagemDeNegocio(lCotacao); //verificar por que est dando erro

                    //lMsgCotacao = new TransporteMensagemDeNegocio(); //verificar por que est dando erro

                    if (decimal.TryParse(lMsgCotacao.Preco, NumberStyles.Any, lBR, out lPrecoPapel))
                    {
                        lPrecosPorPapel[lTransporte.CodigoNegocio] = lPrecoPapel;
                    }
                }
                catch { }
            }

            foreach (TransporteCustodiaInfo lTransporte in lCustodia)
            {
                if (int.TryParse(lTransporte.QtdAtual, out lQuantidade))
                {
                    if (lQuantidade > 0)
                    {
                        lPapel = lTransporte.CodigoNegocio;

                        if (lPrecosPorPapel[lPapel] > 0)
                        {
                            if (!lAgrupadoPorPapel.ContainsKey(lPapel))
                                lAgrupadoPorPapel.Add(lPapel, 0);

                            lPrecoPapel = lPrecosPorPapel[lPapel] * lQuantidade;

                            lAgrupadoPorPapel[lPapel] += lPrecoPapel;

                            lValorTotal += lPrecoPapel;
                        }
                    }
                }
            }

            foreach (string lPapelAgrupado in lAgrupadoPorPapel.Keys)
            {
                lRetorno.Add(string.Format("{0} {1:P} (R$ {2:n})"
                                           , lPapelAgrupado
                                           , (lAgrupadoPorPapel[lPapelAgrupado] / lValorTotal)
                                           , lAgrupadoPorPapel[lPapelAgrupado])
                                           , lAgrupadoPorPapel[lPapelAgrupado]);
            }

            lblComposicaoCarteira.Text = string.Format("{0:n}", lValorTotal);

            TransporteSaldoDeConta.SomatoriaCustodia += decimal.Parse(lblComposicaoCarteira.Text, gCultureInfoBR);

            return lRetorno;
        }

        public struct PreencheGraficoCarteiraAux
        {
            public string Key { get; set; }
            public string Color { get; set; }
            public decimal Value { get; set; }
        }

        public string DefinirCorDoValor(object pParametro)
        {
            decimal lParametro = default(decimal);

            decimal.TryParse(pParametro.ToString(), out lParametro);

            if (lParametro > 0L)
            {
                return "ValorPositivo_Azul";
            }
            else if (lParametro < 0L)
            {
                return "ValorNegativo_Vermelho";
            }
            else
            {
                return string.Empty;
            }
        }

        /*private void MontarExcel()
        {
            StringBuilder lBuilder = new StringBuilder();

            lBuilder.AppendLine("Saldos\tConta Depósito\t\r\n");
            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Para Compra de Ações"  , TransporteSaldoDeConta.Acoes_LimiteTotalAVista.ToString("N2")   );
            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "   Saldo em D0 "       , TransporteSaldoDeConta.Acoes_SaldoD0.ToString("N2")             );
            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "   Saldo em D1 "       , TransporteSaldoDeConta.Acoes_SaldoD1.ToString("N2")             );
            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "   Saldo em D2 "       , TransporteSaldoDeConta.Acoes_SaldoD2.ToString("N2")             );
            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "   Saldo em D3 "       , TransporteSaldoDeConta.Acoes_SaldoD3.ToString("N2")             );
            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Conta Margem "         , TransporteSaldoDeConta.Acoes_SaldoContaMargem.ToString("N2")    );
            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Limite para Compra "   , this.lblLimiteAcoesCompra.Text                                       );
            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Limite para Venda "    , this.lblLimiteAcoesVenda.Text                                        );
            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Saldo Bloqueado "      , TransporteSaldoDeConta.SaldoBloqueado.ToString("N2")            );
            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Para Compra de Opções" , TransporteSaldoDeConta.Opcoes_LimiteTotal.ToString("N2")        );
            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "   Saldo em D0 "       , TransporteSaldoDeConta.Opcoes_SaldoD0.ToString("N2")            );
            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "   Saldo em D1 "       , TransporteSaldoDeConta.SaldoBloqueado.ToString("N2")            );

            lBuilder.AppendLine("\t\r\n");

            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Saldo Projetado em C/C",           TransporteSaldoDeConta.Acoes_LimiteTotalAVista.ToString("N2")  );
            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Saldo em Ações (limite incluso)", (TransporteSaldoDeConta.Acoes_LimiteTotalAVista).ToString("N2"));
            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Saldo em Opções (limite incluso)", TransporteSaldoDeConta.Opcoes_LimiteTotal.ToString("N2")       );
            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Total em Fundos",                  TransporteSaldoDeConta.TotalFundos.ToString("N2")              );
            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Saldo Total",                      TransporteSaldoDeConta.SaldoTotal.ToString("N2")               );
            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Composição Carteira",              TransporteSaldoDeConta.SomatoriaCustodia.ToString("N2")        );

            lBuilder.AppendLine("\t\r\n");
            lBuilder.AppendLine("Limite Operacional de Compras\t\r\n");

            gTransporteConfigurarLimites.ForEach(delegate(TransporteConfigurarLimites tcl)
            {
                if (tcl.DsParametro.Contains("compra"))
                {
                    if (tcl.DsParametro.Contains("vista"))
                    {
                        lBuilder.AppendLine("Compra Ações\t\r\n");
                        lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\r\n", "Valor Alocado:", tcl.ValorAlocado , "Valor Disponível:", tcl.ValorDisponivel , "Valor Total:", tcl.ValorLimite );
                    }
                    else
                    {
                        lBuilder.AppendLine("Compra Opções\t\r\n");
                        lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\r\n", "Valor Alocado:", tcl.ValorAlocado , "Valor Disponível:", tcl.ValorDisponivel , "Valor Total:", tcl.ValorLimite );
                    }
                }
                else
                {
                    if (tcl.DsParametro.Contains("vista"))
                    {
                        lBuilder.AppendLine("Venda Ações\t\r\n");
                        lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\r\n", "Valor Alocado:", tcl.ValorAlocado , "Valor Disponível:", tcl.ValorDisponivel , "Valor Total:", tcl.ValorLimite );
                    }
                    else
                    {
                        lBuilder.AppendLine("Venda Opções\t\r\n");
                        lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\r\n", "Valor Alocado:", tcl.ValorAlocado , "Valor Disponível:", tcl.ValorDisponivel , "Valor Total:", tcl.ValorLimite );
                    }
                }
            });
            
            lBuilder.AppendLine("\t\r\n");

            Response.Clear();

            Response.ContentType = "text/xls";

            Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

            Response.Charset = "iso-8859-1";

            Response.AddHeader("content-disposition", "attachment;filename=RelatorioPosicaoAtual.xls");

            Response.Write(lBuilder.ToString());

            Response.End();
        }*/

        private byte[] GerarRelatorio(
            string pCaminhoRelatorio,
            string pCaminhoDoArquivo,
            List<RiscoLimiteAlocadoInfo> pRisco,
            FinanceiroExtratoInfo pSaldo, 
            out string pMymeType)
        {
            string lNome = base.SessaoClienteLogado.Nome;

            LocalReport lReport = new LocalReport();

            //Endereço
            lReport.ReportPath = pCaminhoRelatorio;

            //Parametro
            List<ReportParameter> lParametros = new List<ReportParameter>();

            ReportParameter lParamDataSaldosLimites = new ReportParameter("pDataSaldoLimites", DateTime.Now.ToString("dd/MM/yyyy"));
            lParametros.Add(lParamDataSaldosLimites);

            ReportParameter lParamCpfCnpjCliente = new ReportParameter("pCpfCnpj", base.SessaoClienteLogado.CpfCnpj);
            lParametros.Add(lParamCpfCnpjCliente);

            ReportParameter lParamCliente = new ReportParameter("pCliente", base.SessaoClienteLogado.CodigoPrincipal + "-" + base.SessaoClienteLogado.Nome);
            lParametros.Add(lParamCliente);

            ReportParameter lParamValorDia = new ReportParameter("pValorParaDia", pSaldo.SaldoDisponivel_ValorParaDia.ToString("N2"));
            lParametros.Add(lParamValorDia);

            ReportParameter lParamValorResgateDia = new ReportParameter("pResgateDia", pSaldo.SaldoDisponivel_ResgateParaDia.ToString("N2"));
            lParametros.Add(lParamValorResgateDia);

            foreach (var risco in pRisco)
            {
                if (risco.DsParametro.Contains("maximo")) continue;

                if (risco.DsParametro.Contains("vista"))
                {
                    if (risco.DsParametro.Contains("compra"))
                    {
                        ReportParameter lParamLimiteCreditoVista = new ReportParameter("pLimiteTotalCreditoVista", risco.VlParametro.ToString("N2"));
                        lParametros.Add(lParamLimiteCreditoVista);

                        ReportParameter lParamLimiteDisponivelVista = new ReportParameter("pLimiteDisponivelVista", risco.VlDisponivel.ToString("N2"));
                        lParametros.Add(lParamLimiteDisponivelVista);
                    }
                }
                else
                {
                    if (risco.DsParametro.Contains("compra"))
                    {
                        ReportParameter lParamLimiteCreditoOpcoes = new ReportParameter("pLimiteTotalCreditoOpcao", risco.VlParametro.ToString("N2"));
                        lParametros.Add(lParamLimiteCreditoOpcoes);

                        ReportParameter lParamLimiteDisponivelOpcoes = new ReportParameter("pLimiteDisponivelOpcoes", risco.VlDisponivel.ToString("N2"));
                        lParametros.Add(lParamLimiteDisponivelOpcoes);
                    }
                }

            }
            ReportParameter lParamSaldoD1 = new ReportParameter("pSaldoProjetadoD1", pSaldo.SaldoD1.ToString("N2"));
            lParametros.Add(lParamSaldoD1);

            ReportParameter lParamSaldoD2 = new ReportParameter("pSaldoProjetadoD2", pSaldo.SaldoD2.ToString("N2"));
            lParametros.Add(lParamSaldoD2);

            ReportParameter lParamSaldoD3 = new ReportParameter("pSaldoProjetadoD3", pSaldo.SaldoD3.ToString("N2"));
            lParametros.Add(lParamSaldoD3);

            ReportParameter lParamSaldoContaMargem = new ReportParameter("pSaldoContaMargem", pSaldo.SaldoContaMargem.Value.ToString("N2"));
            lParametros.Add(lParamSaldoContaMargem);

            ReportParameter lParamSaldoProjetado = new ReportParameter("pTotalProjetado", pSaldo.SaldoProjetado.Value.ToString("N2"));
            lParametros.Add(lParamSaldoProjetado);

            ReportParameter lParamComprasExecVista = new ReportParameter("pComprasExecVista", pSaldo.OperacoesRealizadasDoDia_ComprasExecutadas.ToString("N2"));
            lParametros.Add(lParamComprasExecVista);

            ReportParameter lParamVendasExecVista = new ReportParameter("pVendasExecVista", pSaldo.OperacoesRealizadasDoDia_VendasExecutadas.ToString("N2"));
            lParametros.Add(lParamVendasExecVista);

            ReportParameter lParamTotalaVista = new ReportParameter("pTotalExecVista", pSaldo.OperacoesRealizadasDoDia_TotalAVista.ToString("N2"));
            lParametros.Add(lParamTotalaVista);

            ReportParameter lParamComprasExecOpcoes = new ReportParameter("pComprasExecOpcoes", pSaldo.OperacoesRealizadasDoDia_ComprasDeOpcoes.ToString("N2"));
            lParametros.Add(lParamComprasExecOpcoes);

            ReportParameter lParamVendasExecOpcoes = new ReportParameter("pVendasExecOpcoes", pSaldo.OperacoesRealizadasDoDia_VendasDeOpcoes.ToString("N2"));
            lParametros.Add(lParamVendasExecOpcoes);

            ReportParameter lParamTotalOpcoes = new ReportParameter("pTotalExecOpcoes", pSaldo.OperacoesRealizadasDoDia_TotalDeOpcoes.ToString("N2"));
            lParametros.Add(lParamTotalOpcoes);

            ReportParameter lParamComprasEmAberto = new ReportParameter("pComprasEmAberto", pSaldo.OperacoesNaoRealizadasDoDia_ComprasEmAberto.ToString("N2"));
            lParametros.Add(lParamComprasEmAberto);

            ReportParameter lParamVendasemAberto = new ReportParameter("pVendasEmAberto", pSaldo.OperacoesNaoRealizadasDoDia_VendasEmAberto.ToString("N2"));
            lParametros.Add(lParamVendasemAberto);

            ReportParameter lParamTotalEmAberto = new ReportParameter("pTotalEmAberto", pSaldo.OperacoesNaoRealizadasDoDia_TotalEmAberto.ToString("N2"));
            lParametros.Add(lParamTotalEmAberto);

            ReportParameter lParamSaldoProjetadoTotal = new ReportParameter("pSaldoProjetado", pSaldo.SaldoProjetado.Value.ToString("N2"));
            lParametros.Add(lParamSaldoProjetadoTotal);

            ReportParameter lParamSaldoTotalContaCorrente = new ReportParameter("pSaldoTotalContaCorrente", pSaldo.SaldoTotalEmContaCorrente.ToString("N2"));
            lParametros.Add(lParamSaldoTotalContaCorrente);

            lReport.SetParameters(lParametros);

            string lReportType, lEncoding, lFileNameExtension, lFileName, lDeviceInfo;

            Warning[] lWarnings;
            string[] lStreams;
            byte[] lRenderedBytes;

            lReportType = "PDF";
            lFileName = pCaminhoDoArquivo;

            lDeviceInfo =
            "<DeviceInfo> <OutputFormat>PDF</OutputFormat> <PageWidth>9.5in</PageWidth> <PageHeight>11in</PageHeight> <MarginTop>0.5in</MarginTop> <MarginLeft>0.5in</MarginLeft> <MarginRight>0.5in</MarginRight> <MarginBottom>0.5in</MarginBottom> </DeviceInfo>";

            //Render the report
            lRenderedBytes = lReport.Render(lReportType, lDeviceInfo, out pMymeType, out lEncoding, out lFileNameExtension, out lStreams, out lWarnings);

            return lRenderedBytes;
        }

        private void CarregarLimitesViaServico()
        {
            RiscoLimiteAlocadoInfo lRequest = new RiscoLimiteAlocadoInfo();

            lRequest.ConsultaIdCliente = base.SessaoClienteLogado.CodigoPrincipal.DBToInt32();

            this.gRetornoLimitePorCliente = new PersistenciaMinhaConta().ConsultarRiscoLimiteAlocadoPorClienteNovoOMS(lRequest);
        }

        private void CarregarSaldoDeConta()
        {
            //TransporteSaldoDeConta TransporteSaldoDeConta;

            SaldoContaCorrenteRequest lRequest = new SaldoContaCorrenteRequest();

            lRequest.IdCliente = base.SessaoClienteLogado.CodigoPrincipal.DBToInt32();

            var lServico = Ativador.Get<IServicoContaCorrente>();

            SaldoContaCorrenteResponse<Gradual.OMS.ContaCorrente.Lib.ContaCorrenteInfo> lResponse = lServico.ObterSaldoContaCorrente(lRequest);

            TransporteSaldoDeConta = new TransporteSaldoDeConta(lResponse.Objeto);

            //this.lblAtualizacao.Text = "Última Atualização em: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            this.cssLimiteAcoes            = this.DefinirCorDoValor(TransporteSaldoDeConta.SaldoAtual);
            this.cssLimiteAcoesD0          = this.DefinirCorDoValor(TransporteSaldoDeConta.Acoes_SaldoD0);
            this.cssLimiteAcoesD1          = this.DefinirCorDoValor(TransporteSaldoDeConta.Acoes_SaldoD1);
            this.cssLimiteAcoesD2          = this.DefinirCorDoValor(TransporteSaldoDeConta.Acoes_SaldoD2);
            this.cssLimiteAcoesD3          = this.DefinirCorDoValor(TransporteSaldoDeConta.Acoes_SaldoD3);
            this.cssSaldoProjetado         = this.DefinirCorDoValor(TransporteSaldoDeConta.SaldoProjetado);
            this.cssAcoesLimiteTotalAVista = this.DefinirCorDoValor(TransporteSaldoDeConta.Acoes_LimiteTotalAVista);
            this.cssOpcoesLimiteTotal      = this.DefinirCorDoValor(TransporteSaldoDeConta.Opcoes_LimiteTotal);

            this.cssAcoesSaldoBloqueado    = this.DefinirCorDoValor(TransporteSaldoDeConta.SaldoBloqueado);
            this.cssAcoesContaMargem       = this.DefinirCorDoValor(TransporteSaldoDeConta.Acoes_SaldoContaMargem);
            this.cssLimiteOpcoes           = this.DefinirCorDoValor(TransporteSaldoDeConta.Opcoes_LimiteTotal);
            this.cssLimiteOpcoesD0         = this.DefinirCorDoValor(TransporteSaldoDeConta.Opcoes_SaldoD0);
            this.cssLimiteOpcoesD1         = this.DefinirCorDoValor(TransporteSaldoDeConta.Opcoes_SaldoD1);
            this.cssOpcoesSaldoBloqueado   = this.DefinirCorDoValor(TransporteSaldoDeConta.SaldoBloqueado);

            
            //TransporteSaldoDeConta.Acoes_LimiteTotalAVista += Convert.ToDecimal(this.lblLimiteAcoesCompra.Text, gCultureInfoBR);
            //TransporteSaldoDeConta.Opcoes_LimiteTotal      += Convert.ToDecimal(this.lblLimiteOpcoesCompra.Text, gCultureInfoBR);

            //TransporteSaldoDeConta.TotalFundos = this.BuscarFundosClubes(lRequest.IdCliente);

            TransporteSaldoDeConta.SaldoTotal = //TransporteSaldoDeConta.TotalFundos +
                TransporteSaldoDeConta.Acoes_SaldoD0 +
                TransporteSaldoDeConta.Acoes_SaldoD1 +
                TransporteSaldoDeConta.Acoes_SaldoD2 +
                TransporteSaldoDeConta.Acoes_SaldoD3;

             //TransporteSaldoDeConta.SaldoTotal + TransporteSaldoDeConta.TotalFundos;

            this.CarregarPosicaoFundos();

            Dictionary<string, decimal> lCustodia = CarregarCustodiaComoPorcentagens();

            this.cssSaldoTotal = this.DefinirCorDoValor(TransporteSaldoDeConta.SaldoTotal);
            this.cssSomatoriaCustodia = this.DefinirCorDoValor(TransporteSaldoDeConta.SomatoriaCustodia);

            //lCustodia = from a in lCustodia orderby a.Value descending select a;

            List<PreencheGraficoCarteiraAux> lListaCustodia = new List<PreencheGraficoCarteiraAux>();

            string[] CoresPreferencias = { "#524646", "#B4837A", "#D1D2D3", "#EE9620", "#F6BC15" };

            int lCount = 0;

            var lRandom = new Random();

            foreach (KeyValuePair<string, decimal> kvp in lCustodia)
            {
                var lCust = new PreencheGraficoCarteiraAux();

                lCust.Color = (lCount <= 4) ? CoresPreferencias[lCount] : String.Format("#{0:X6}", lRandom.Next(0x1000000));
                lCust.Value = kvp.Value;
                lCust.Key = kvp.Key;

                lListaCustodia.Add(lCust);
                lCount++;
            }

            

            rptGrafico_Carteira.DataSource = lListaCustodia;
            rptGrafico_Carteira.DataBind();

        }

        private void ConfigurarLimitesNaTela(List<TransporteConfigurarLimites> pParametrosLimites)
        {
            gTransporteConfigurarLimites = pParametrosLimites;

            foreach (TransporteConfigurarLimites tcl in pParametrosLimites)
            {
                if (tcl.DsParametro.Contains("maximo"))
                    continue;

                if (tcl.DsParametro.Contains("compra"))
                {
                    if (tcl.DsParametro.Contains("vista"))
                    {
                        this.lblLimiteAcoesCompra.Text        = tcl.ValorDisponivel;
                        this.lblLimiteAcoesCompra.CssClass    = this.DefinirCorDoValor(tcl.ValorDisponivel);
                        
                        this.lblLimiteAcoesCompraAlocado.Text = tcl.ValorAlocado;
                        this.lblLimiteAcoesCompraAlocado.CssClass = this.DefinirCorDoValor(tcl.ValorAlocado);
                        
                        this.lblLimiteAcoesCompraTotal.Text   = tcl.ValorLimite;
                        this.lblLimiteAcoesCompraTotal.CssClass = this.DefinirCorDoValor(tcl.ValorLimite);
                    }
                    else
                    {
                        this.lblLimiteOpcoesCompra.Text        = tcl.ValorDisponivel;
                        this.lblLimiteOpcoesCompra.CssClass    = this.DefinirCorDoValor(tcl.ValorDisponivel);

                        this.lblLimiteOpcoesCompraAlocado.Text = tcl.ValorAlocado;
                        this.lblLimiteOpcoesCompraAlocado.CssClass = this.DefinirCorDoValor(tcl.ValorAlocado);

                        this.lblLimiteOpcoesCompraTotal.Text   = tcl.ValorLimite;
                        this.lblLimiteOpcoesCompraTotal.CssClass   = this.DefinirCorDoValor(tcl.ValorLimite);
                    }
                }
                else
                {
                    if (tcl.DsParametro.Contains("vista"))
                    {
                        this.lblLimiteAcoesVenda.Text            = tcl.ValorDisponivel;
                        this.lblLimiteAcoesVenda.CssClass        = this.DefinirCorDoValor(tcl.ValorDisponivel);

                        this.lblLimiteAcoesVendaAlocado.Text     = tcl.ValorAlocado;
                        this.lblLimiteAcoesVendaAlocado.CssClass = this.DefinirCorDoValor(tcl.ValorAlocado);
                        
                        this.lblLimiteAcoesVendaTotal.Text     = tcl.ValorLimite;
                        this.lblLimiteAcoesVendaTotal.CssClass = this.DefinirCorDoValor(tcl.ValorLimite);
                    }
                    else
                    {
                        this.lblLimiteOpcoesVenda.Text        = tcl.ValorDisponivel;
                        this.lblLimiteOpcoesVenda.CssClass    = this.DefinirCorDoValor(tcl.ValorDisponivel);

                        this.lblLimiteOpcoesVendaAlocado.Text     = tcl.ValorAlocado;
                        this.lblLimiteOpcoesVendaAlocado.CssClass = this.DefinirCorDoValor(tcl.ValorAlocado);

                        this.lblLimiteOpcoesVendaTotal.Text     = tcl.ValorLimite;
                        this.lblLimiteOpcoesVendaTotal.CssClass = this.DefinirCorDoValor(tcl.ValorLimite);
                    }
                }
            }
        }

        private FinanceiroExtratoInfo CarregarRelatorio()
        {
            var lServicoAtivador = Ativador.Get<IServicoExtratos>();

            var lRespostaBusca = lServicoAtivador.ConsultarExtratoFinanceiro(new FinanceiroExtratoRequest()
            {
                ConsultaCodigoCliente = int.Parse( base.SessaoClienteLogado.CodigoPrincipal),
                //ConsultaNomeCliente = this.GetNomeCliente,
            });

            if (CriticaMensagemEnum.OK.Equals(lRespostaBusca.StatusResposta))
            {

                return lRespostaBusca.Relatorio;
            }
            else
            {
                throw new Exception(string.Format("{0}-{1}", lRespostaBusca.StatusResposta, lRespostaBusca.StackTrace));
            }
        }

        private void CarregarPosicaoFundos()
        {
            var lServico = new PaginaFundos();

            var lPosicao = lServico.PosicaoFundosSumarizada();

            decimal lTotalFundos = 0.0M;

            foreach (var posicao in lPosicao)
            {
                lTotalFundos += decimal.Parse( posicao.ValorLiquido, gCultureInfoBR);
            }

            TransporteSaldoDeConta.TotalFundos = lTotalFundos;

            TransporteSaldoDeConta.SaldoTotal += lTotalFundos;
        }

        #endregion

        #region Event Handlers

        protected string lCodigoClienteformatado = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (base.ValidarSessao())
                {
                    if (!this.IsPostBack)
                    {
                        if (string.IsNullOrEmpty(base.SessaoClienteLogado.CodigoPrincipal))
                        {
                            base.ExibirMensagemJsOnLoad("I", "Você ainda não possui o código de conta na Gradual. <br/>Para acessar essa área, finalize seu Cadastro.");
                            TransporteSaldoDeConta = new TransporteSaldoDeConta();
                            return;
                        }
                    }

                    this.CarregarLimitesViaServico();

                    this.ConfigurarLimitesNaTela(new TransporteConfigurarLimites().TraduzirListaSaldo(this.gRetornoLimitePorCliente));

                    this.CarregarSaldoDeConta();

                    lCodigoClienteformatado = this.SessaoClienteLogado.CodigoPrincipal.ToCodigoClienteFormatado();

                    RodarJavascriptOnLoad("MinhaConta_GerarGrafico(); GradSite_VerificarPositivoseNegativos('#tblSaldo td.ValorNumerico');");
                }
                else
                {
                    if (ValidaApenasSessao())
                    {
                        base.ExibirMensagemJsOnLoad("I", "Você ainda não possui o código de conta na Gradual. <br/>Para acessar essa área, finalize seu Cadastro.");
                        TransporteSaldoDeConta = new TransporteSaldoDeConta();
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
            }
        }

        protected new void Page_Init(object sender, EventArgs e)
        {
            this.PaginaMaster.BreadCrumbVisible = true;

            this.PaginaMaster.Crumb1Text = "Minha Conta";
            this.PaginaMaster.Crumb2Text = "Financeiro";
            this.PaginaMaster.Crumb3Text = "Saldos e Limites";
        }

        public void btnImprimirSaldosLimitesPDF_Click(object sender, EventArgs e)
        {
            try
            {
                var lSaldo = CarregarRelatorio();

                RiscoLimiteAlocadoInfo lRequest = new RiscoLimiteAlocadoInfo();

                lRequest.ConsultaIdCliente = base.SessaoClienteLogado.CodigoPrincipal.DBToInt32();

                List<RiscoLimiteAlocadoInfo> lRiscoInfo = new PersistenciaMinhaConta().ConsultarRiscoLimiteAlocadoPorClienteNovoOMS(lRequest);

                string lNomeDoArquivo = string.Format("SaldosLimites_{0}_{1}", base.SessaoClienteLogado.CodigoPrincipal, DateTime.Now.ToString("yyyyMMddHHmmss"));

                string lMimeType;

                byte[] lRenderedBytes = this.GerarRelatorio(
                        Server.MapPath(@"..\Reports\SaldosLimites.rdlc"),
                        lNomeDoArquivo,
                        lRiscoInfo,
                        lSaldo,
                        out  lMimeType);

                Response.Clear();
                Response.ContentType = lMimeType;
                Response.AddHeader("content-disposition", "attachment; filename=" + lNomeDoArquivo + ".pdf");
                Response.BinaryWrite(lRenderedBytes);
                Response.End();


            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao gerar PDF de Saldos e Limites.");
            }
        }

        public void btnImprimirSaldosLimitesExcell_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder lBuilder = new StringBuilder();

                var lSaldo = CarregarRelatorio();

                RiscoLimiteAlocadoInfo lRequest = new RiscoLimiteAlocadoInfo();

                lRequest.ConsultaIdCliente = base.SessaoClienteLogado.CodigoPrincipal.DBToInt32();

                List<RiscoLimiteAlocadoInfo> lRiscoInfo = new PersistenciaMinhaConta().ConsultarRiscoLimiteAlocadoPorClienteNovoOMS(lRequest);

                lBuilder.AppendFormat("{0}\t{1}\t\r\n", "SALDO EM", DateTime.Now.ToString("dd/MM/yyyy"));

                lBuilder.AppendFormat("{0}\t\r\n", "SALDO DISPONÍVEL");

                lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Valor para o Dia: R$ ", lSaldo.SaldoDisponivel_ValorParaDia.ToString("N2"));

                lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Resgate para o Dia: R$ ", lSaldo.SaldoDisponivel_ResgateParaDia.ToString("N2"));

                lBuilder.AppendFormat("{0}\t\r\n", "LIMITE");

                foreach (var lRisco in lRiscoInfo)
                {
                    if (lRisco.DsParametro.Contains("maximo")) continue;

                    if (lRisco.DsParametro.Contains("vista"))
                    {
                        if (lRisco.DsParametro.Contains("compra"))
                        {
                            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Limite Total de Crédito à Vista: R$ ", lRisco.VlParametro.ToString("N2"));

                            lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Limite Disponível à Vista: R$ ", lRisco.VlDisponivel.ToString("N2"));

                        }
                    }
                    else
                    {
                        lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Limite Total de Crédito para Opções: R$ ", lRisco.VlParametro.ToString("N2"));

                        lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Limite Disponível para Opções: R$ ", lRisco.VlDisponivel.ToString("N2"));
                    }
                }

                lBuilder.AppendFormat("{0}\t\r\n", "LANÇAMENTOS PROJETADOS PARA");

                lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Projetado D+1: R$ ", lSaldo.SaldoD1.ToString("N2"));

                lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Projetado D+2: R$ ", lSaldo.SaldoD2.ToString("N2"));

                lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Projetado D+3: R$ ", lSaldo.SaldoD3.ToString("N2"));

                if (lSaldo.SaldoContaMargem.HasValue)
                {
                    lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Saldo Conta Margem: R$ ", lSaldo.SaldoContaMargem.Value.ToString("N2"));
                }

                if (lSaldo.SaldoProjetado.HasValue)
                {
                    lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Total Projetado: R$ ", lSaldo.SaldoProjetado.Value.ToString("N2"));
                }

                lBuilder.AppendFormat("{0}\t\r\n", "OPERAÇÕES REALIZADAS NO DIA");

                lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Compras executadas Á Vista: R$ ", lSaldo.OperacoesRealizadasDoDia_ComprasExecutadas.ToString("N2"));

                lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Vendas Executadas Á Vista: R$ ", lSaldo.OperacoesRealizadasDoDia_VendasExecutadas.ToString("N2"));

                lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Total Á Vista:  R$ ", lSaldo.OperacoesRealizadasDoDia_TotalAVista.ToString("N2"));

                lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Compras executadas de Opções: R$ ", lSaldo.OperacoesRealizadasDoDia_ComprasDeOpcoes.ToString("N2"));
                
                lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Vendas executadas de Opções: R$ ", lSaldo.OperacoesRealizadasDoDia_VendasDeOpcoes.ToString("N2"));
                
                lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Total de Opções: R$ ", lSaldo.OperacoesRealizadasDoDia_TotalDeOpcoes.ToString("N2"));

                lBuilder.AppendFormat("{0}\t\r\n", "OPERAÇÕES NÃO REALIZADAS DO DIA");

                lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Compras Em Aberto: R$ ", lSaldo.OperacoesNaoRealizadasDoDia_ComprasEmAberto.ToString("N2"));

                lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Vendas Em Aberto : R$ ", lSaldo.OperacoesNaoRealizadasDoDia_VendasEmAberto.ToString("N2"));

                lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Total Em Aberto :  R$ ", lSaldo.OperacoesNaoRealizadasDoDia_TotalEmAberto.ToString("N2"));

                if (lSaldo.SaldoProjetado.HasValue)
                {
                    lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Saldo Projetado: R$ ", lSaldo.SaldoProjetado.Value.ToString("N2"));
                }

                lBuilder.AppendFormat("{0}\t{1}\t\r\n", "Saldo Total em Conta Corrente: R$ ", lSaldo.SaldoTotalEmContaCorrente.ToString("N2"));


                Response.Clear();

                Response.ContentType = "text/xls";

                Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

                Response.Charset = "iso-8859-1";

                Response.AddHeader("content-disposition", "attachment;filename=RelatorioSaldosLimites.xls");

                Response.Write(lBuilder.ToString());

                Response.End();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao gerar Excel de Saldos e Limites.");
            }
        }

        public void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {
                var lSaldo = CarregarRelatorio();

                RiscoLimiteAlocadoInfo lRequest = new RiscoLimiteAlocadoInfo();

                lRequest.ConsultaIdCliente = base.SessaoClienteLogado.CodigoPrincipal.DBToInt32();

                List<RiscoLimiteAlocadoInfo> lRiscoInfo = new PersistenciaMinhaConta().ConsultarRiscoLimiteAlocadoPorClienteNovoOMS(lRequest);

                string lNomeDoArquivo = string.Format("SaldosLimites_{0}_{1}", base.SessaoClienteLogado.CodigoPrincipal, DateTime.Now.ToString("yyyyMMddHHmmss"));

                string lMimeType;

                byte[] lRenderedBytes = this.GerarRelatorio(
                        Server.MapPath(@"..\Reports\SaldosLimites.rdlc"),
                        lNomeDoArquivo,
                        lRiscoInfo,
                        lSaldo,
                        out  lMimeType);

                var lAnexos               = new List<OMS.Email.Lib.EmailAnexoInfo>();
                EmailAnexoInfo lEmailInfo = new EmailAnexoInfo();
                lEmailInfo.Arquivo        = lRenderedBytes;
                lEmailInfo.Nome           = string.Concat(lNomeDoArquivo, ".pdf");
                lAnexos.Add(lEmailInfo);

                Dictionary<string, string> lVariaveis = new Dictionary<string, string>();
                //base.EnviarEmail("bribeiro@gradualinvestimentos.com.br", "Saldos e Limites - Gradual Investimentos", "SaldosELimites.htm", lVariaveis, Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, lAnexos, null);
                base.EnviarEmail(base.SessaoClienteLogado.Email, "Saldos e Limites - Gradual Investimentos", "SaldosELimites.htm", lVariaveis, Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, lAnexos, null);

                base.ExibirMensagemJsOnLoad("I", "Um E-mail com o arquivo Pdf foi enviado para " + base.SessaoClienteLogado.Email);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao enviar email com PDF de Slados e Limites.");
            }
        }

        #endregion

    }
}