using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Gradual.Site.DbLib.Mensagens;
using Gradual.OMS.PosicaoBTC.Lib;
using System.Globalization;

using Gradual.Monitores.Risco.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.Monitores.Risco.Info;
using Microsoft.Reporting.WebForms;
using Gradual.OMS.Email.Lib;
using Gradual.Site.Www.Transporte;
using Gradual.OMS.Monitor.Custodia.Lib.Mensageria;
using Gradual.OMS.Monitor.Custodia.Lib;

namespace Gradual.Site.Www.MinhaConta.Posicao
{
    public partial class RendaVariavel : PaginaBase
    {
        #region Atributos
        private const string DI            = "DI1";
        private const string DOLAR         = "DOL";
        private const string INDICE        = "IND";
        private const string MINIBOLSA     = "WIN";
        private const string MINIDOLAR     = "WDL";
        private const string MINIDOLARFUT  = "WDO";
        private const string CHEIOBOI      = "BGI";
        private const string MINIBOI       = "WBG";
        private const string EURO          = "EUR";
        private const string MINIEURO      = "WEU";
        private const string CAFE          = "ICF";
        private const string MINICAFE      = "WCF";
        private const string FUTUROACUCAR  = "ISU";
        private const string ETANOL        = "ETH";
        private const string ETANOLFISICO  = "ETN";
        private const string MILHO         = "CCM";
        private const string SOJA          = "SFI";
        private const string OURO          = "OZ1";
        private const string ROLAGEMDOLAR  = "DR1";
        private const string ROLAGEMINDICE = "IR1";
        private const string ROLAGEMBOI    = "BR1";
        private const string ROLAGEMCAFE   = "CR1";
        private const string ROLAGEMMILHO  = "MR1";
        private const string ROLAGEMSOJA   = "SR1";
        private const string ISP           = "ISP";
        #endregion

        #region Propriedades

        private string TipoRelatorio
        {
            get { return ViewState["TipoRelatorio"].ToString(); }

            set { ViewState["TipoRelatorio"] = value; }
        }

        public string TipoDeBolsa
        {
            get
            {
                if (string.Format("{0}", Request["TipoRelatorio"]).ToLower() == "bovespa")
                {
                    return "Bovespa";
                }

                return "BM&F";
            }
        }

        #endregion

        #region Events
        
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
                            return;
                        }

                        CarregarGrids();
                        RodarJavascriptOnLoad("MinhaConta_GerarGrafico_RendaVariavel(); GradSite_VerificarPositivoseNegativos('#tblSaldo td.ValorNumerico');");
                    }
                }

            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
            }
        }
        #endregion

        #region Métodos
        public struct PreencheGraficoPosicaoVariavelAux
        {
            public string   Key { get; set; }
            public string   Color { get; set; }
            public decimal  Value { get; set; }
        }

        public struct TransportePapelAux
        {
            public string   Papel { get; set; }
            public decimal  Quant { get; set; }
            public decimal  Preco { get; set; }
            public string   Bolsa { get; set; }
        }

        private Dictionary<string, decimal> PreencheListaPosicaoGrafico(List<TransportePosicaoAtual> lListaBov , List<TransportePosicaoAtual> lListaBmf,List<Transporte_BTC> lListaBtc, List<Transporte_Termo> lListaTermo   )
        {
            CultureInfo lBR = new CultureInfo("pt-BR");

            var lPosicao = new List<TransportePapelAux>();

            var lAgrupadoPorPapel = new Dictionary<string, decimal>();

            var lPrecosPorPapel = new Dictionary<string, decimal>();

            var lRetorno = new Dictionary<string, decimal>();

            decimal lValorTotal = 0.0M;
            decimal lPrecoPapel;

            lListaBmf.ForEach(bmf => 
            {
                var lTrans = new TransportePapelAux();
                
                lTrans.Papel = bmf.CodigoInstrumento;
                lTrans.Preco = Convert.ToDecimal( bmf.Resultado,lBR);
                lTrans.Quant = Convert.ToDecimal( bmf.QtdeAtual, lBR);
                lTrans.Bolsa = "bmf";
                lPosicao.Add(lTrans); 
            });

            lListaBov.ForEach(bov => 
            {
                var lTrans = new TransportePapelAux();

                lTrans.Papel = bov.CodigoAtivo;
                lTrans.Preco = Convert.ToDecimal(bov.ValorCotacao, lBR);
                lTrans.Quant = int.Parse(bov.QuantidadeTotal.Replace(".",string.Empty), lBR);
                lTrans.Bolsa = "bov";
                lPosicao.Add(lTrans); 
            });

            lListaBtc.ForEach(btc => 
            {
                var lTrans = new TransportePapelAux();

                lTrans.Papel = btc.Instrumento;
                lTrans.Preco = Convert.ToDecimal(btc.PrecoMedio, lBR);
                lTrans.Quant = int.Parse(btc.Quantidade.Replace(".",string.Empty), lBR);
                lTrans.Bolsa = "bov";
                lPosicao.Add(lTrans); 
            });

            lListaTermo.ForEach(termo =>
            {

                var lTrans = new TransportePapelAux();

                lTrans.Papel = termo.Instrumento;
                lTrans.Preco = Convert.ToDecimal(termo.PrecoMedio);
                lTrans.Quant = int.Parse(termo.Quantidade.Replace(".",string.Empty), lBR);

                lPosicao.Add(lTrans);

            });

            foreach (var lPos in lPosicao )
            {
                if (!lPrecosPorPapel.ContainsKey(lPos.Papel)) lPrecosPorPapel.Add( lPos.Papel, 0);

                lPrecosPorPapel[lPos.Papel] = lPos.Preco;
            }

            foreach (var lPos in lPosicao)
            {
                if (lPos.Quant != 0)
                {
                    string lPapel = lPos.Papel;

                    if (lPrecosPorPapel[lPapel] > 0)
                    {
                        if (!lAgrupadoPorPapel.ContainsKey(lPapel)) lAgrupadoPorPapel.Add(lPapel, 0);

                        if (lPos.Bolsa == "bmf")
                        {
                            lPrecoPapel = lPrecosPorPapel[lPapel] ;
                        }
                        else
                        {
                            lPrecoPapel = lPrecosPorPapel[lPapel] * lPos.Quant;
                        }

                        lAgrupadoPorPapel[lPapel] += lPrecoPapel;

                        lValorTotal += lPrecoPapel;
                    }
                }
            }

            var items = from pair in lAgrupadoPorPapel
                        orderby pair.Value descending
                        select pair;

            foreach (KeyValuePair<string, decimal> pair in items)
            {
                lRetorno.Add(string.Format("{0} {1:P} (R$ {2:n})"
                                           , pair.Key
                                           , (pair.Value / lValorTotal)
                                           , pair.Value)
                                           , pair.Value);
            }

            lblComposicaoRendaVariavel.Text = lValorTotal.ToString("N2");

            return lRetorno;
        }

        private void CarregarGrids()
        {
            List<TransportePosicaoAtual> lListaBov = BuscarRelatorioBovespa();

            if (lListaBov != null)
            {
                rptPosicaoAtualBovespa.DataSource = lListaBov;
                rptPosicaoAtualBovespa.DataBind();

                trNenhumBovespa.Visible = (lListaBov.Count == 0);
            }

            List<TransportePosicaoAtual> lListaBmf = BuscarRelatorioBmf();

            if (lListaBmf != null)
            {
                rptPosicaoAtualBmf.DataSource = lListaBmf;
                rptPosicaoAtualBmf.DataBind();

                trNenhumBmf.Visible = (lListaBmf.Count == 0);
            }

            List<Transporte_BTC> lListaBtc = BuscarRelatorioBtc();

            if (lListaBtc != null)
            {
                rptPosicaoBtc.DataSource = lListaBtc;
                rptPosicaoBtc.DataBind();

                trNenhumPosicaoBtc.Visible = lListaBtc.Count == 0;
            }

            List<Transporte_Termo> lListaTermo = BuscarRelatorioTermo();

            if (lListaTermo != null)
            {
                rptPosicaoTermo.DataSource = lListaTermo;
                rptPosicaoTermo.DataBind();

                trNenhumPosicaoTermo.Visible = lListaTermo.Count == 0;
            }

            List<PreencheGraficoPosicaoVariavelAux> lListaPosicao = new List<PreencheGraficoPosicaoVariavelAux>();

            string[] CoresPreferencias = { "#524646", "#B4837A", "#D1D2D3", "#EE9620", "#F6BC15" };

            int lCount = 0;

            var lRandom = new Random();

            var lPosicaoGrafico = PreencheListaPosicaoGrafico(lListaBov, lListaBmf, lListaBtc, lListaTermo);

            foreach (KeyValuePair<string, decimal> kvp in lPosicaoGrafico)
            {
                var lCust = new PreencheGraficoPosicaoVariavelAux();

                lCust.Color = (lCount <= 4) ? CoresPreferencias[lCount] : String.Format("#{0:X6}", lRandom.Next(0x1000000));
                lCust.Value = kvp.Value;
                lCust.Key   = kvp.Key;

                lListaPosicao.Add(lCust);
                lCount++;
            }

            this.rptGrafico_RendaVariavel.DataSource = lListaPosicao;
            this.rptGrafico_RendaVariavel.DataBind();
        }

        private List<TransportePosicaoAtual> BuscarRelatorioBovespa()
        {
            List<TransportePosicaoAtual> lListaCustodia = new List<TransportePosicaoAtual>();

            CustodiaResponse lResposta;

            lResposta = base.ServicoPersistenciaSite.ObterPosicaoAtual(base.SessaoClienteLogado.CodigoPrincipal.DBToInt32()); //Busca o relatório para a posição em BOVESPA

            lListaCustodia = TransportePosicaoAtual.TraduzirLista(lResposta.ListaCustodia);

            return lListaCustodia;
        }

        private List<TransportePosicaoAtual> BuscarRelatorioBmf()
        {
            var lServicoCustodia = Ativador.Get<IServicoMonitorCustodia>();

            var lRequest = new MonitorCustodiaRequest();

            lRequest.CodigoClienteBmf =  base.SessaoClienteLogado.CodigoBMF.DBToInt32();
            lRequest.CodigoCliente =     base.SessaoClienteLogado.CodigoPrincipal.DBToInt32();

            var lResponse = lServicoCustodia.ObterMonitorCustodiaMemoria(lRequest);

            var lListaCustodia = new List<TransportePosicaoAtual>();

            //CustodiaResponse lResposta;

            //lResposta = base.ServicoPersistenciaSite.ObterPosicaoAtualBMF(base.SessaoClienteLogado.CodigoBMF.DBToInt32()); //Busca o Relatório para posição em BMF

            //lListaCustodia = TransportePosicaoAtual.TraduzirLista(lResposta.ListaCustodia);

            lListaCustodia = TransportePosicaoAtual.TraduzirListaBmf(lResponse.MonitorCustodia.ListaCustodia);

            return lListaCustodia;

        }

        private List<Transporte_BTC> BuscarRelatorioBtc()
        {
            var lRetorno = new List<Transporte_BTC>();

            try
            {
                IServicoMonitorRisco lServico = Ativador.Get<IServicoMonitorRisco>();

                MonitorLucroPrejuizoRequest lRequest = new MonitorLucroPrejuizoRequest();

                lRequest.Cliente = int.Parse( SessaoClienteLogado.CodigoPrincipal);

                //lRequest.Cliente = 3895;

                var lRetornoConsulta = lServico.ObterMonitorLucroPrejuizo(lRequest);

                if (lRetornoConsulta != null && lRetornoConsulta.Monitor != null && lRetornoConsulta.Monitor.Count > 0)
                {
                    lRetorno = new Transporte_BTC().TraduzirLista(lRetornoConsulta.Monitor[0].OrdensBTC);

                   
                }
                
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
            }

            return lRetorno;
        }
        
        private List<Transporte_Termo> BuscarRelatorioTermo()
        {
            var lRetorno = new List<Transporte_Termo>();

            try
            {
                IServicoMonitorRisco lServico = Ativador.Get<IServicoMonitorRisco>();

                MonitorLucroPrejuizoRequest lRequest = new MonitorLucroPrejuizoRequest();

                lRequest.Cliente = int.Parse(SessaoClienteLogado.CodigoPrincipal);

                //lRequest.Cliente = 35037;

                var lRetornoConsulta = lServico.ObterMonitorLucroPrejuizo(lRequest);

                if (lRetornoConsulta != null && lRetornoConsulta.Monitor != null && lRetornoConsulta.Monitor.Count > 0)
                {
                    lRetorno = new Transporte_Termo().TraduzirLista(lRetornoConsulta.Monitor[0].OrdensTermo);


                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
            }
            return lRetorno;
        }

        private void ExportarExcel()
        {
            StringBuilder lBuilder = new StringBuilder();

            List<TransportePosicaoAtual> lCustodia = BuscarRelatorioBovespa();

            lBuilder.AppendLine("CodigoAtivo\tNomeAtivo\tMercado\tTipoCarteira\tQuantidadeAexecutarVenda\tQuantidadeAexecutarCompra\tQuantidadeTotal\tValorCotacao\tValorFinanceiro\tSaldoD1\tSaldoD2\tSaldoD3\tDataVencimento\t\r\n");

            foreach (TransportePosicaoAtual ECustodiaitem in lCustodia)
            {
                lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t\r\n", ECustodiaitem.CodigoAtivo, ECustodiaitem.NomeAtivo, ECustodiaitem.Mercado, ECustodiaitem.TipoCarteira, ECustodiaitem.QuantidadeAexecutarCompra, ECustodiaitem.QuantidadeAexecutarVenda, ECustodiaitem.QuantidadeTotal, ECustodiaitem.ValorCotacao, ECustodiaitem.ValorFinanceiro, ECustodiaitem.SaldoD1, ECustodiaitem.SaldoD2, ECustodiaitem.SaldoD3, ECustodiaitem.DataVencimento);
            }

            lBuilder.AppendLine("\t\r\n");

            List<TransportePosicaoAtual> lCustodiaBmf = BuscarRelatorioBmf();

            lBuilder.AppendLine("CodigoAtivo\tNomeAtivo\tMercado\tTipoCarteira\tQuantidadeAexecutarVenda\tQuantidadeAexecutarCompra\tQuantidadeTotal\tValorCotacao\tValorFinanceiro\tSaldoD1\tSaldoD2\tSaldoD3\tDataVencimento\t\r\n");

            foreach (TransportePosicaoAtual ECustodiaitem in lCustodiaBmf)
            {
                lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t\r\n", ECustodiaitem.CodigoAtivo, ECustodiaitem.NomeAtivo, ECustodiaitem.Mercado, ECustodiaitem.TipoCarteira, ECustodiaitem.QuantidadeAexecutarCompra, ECustodiaitem.QuantidadeAexecutarVenda, ECustodiaitem.QuantidadeTotal, ECustodiaitem.ValorCotacao, ECustodiaitem.ValorFinanceiro, ECustodiaitem.SaldoD1, ECustodiaitem.SaldoD2, ECustodiaitem.SaldoD3, ECustodiaitem.DataVencimento);
            }

            List<Transporte_BTC> lPosicaoBtc = this.BuscarRelatorioBtc();

            lBuilder.AppendLine("Carteira\tCodigoCliente\tDataAbertura\tDataVencimento\tInstrumento\tPrecoMedio\tPrecoMercado\tQuantidade\tRemuneracao\tTaxa\tTipoContrato\tSubtotalQuantidade\tSubtotalValor\t\r\n");

            foreach (Transporte_BTC EPosicaoBtc in lPosicaoBtc)
            {
                lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t\r\n", 
                    EPosicaoBtc.Carteira, 
                    EPosicaoBtc.CodigoCliente,
                    EPosicaoBtc.DataAbertura,
                    EPosicaoBtc.DataVencimento,
                    EPosicaoBtc.Instrumento,
                    EPosicaoBtc.PrecoMedio,
                    EPosicaoBtc.PrecoMercado, EPosicaoBtc.Quantidade,
                    EPosicaoBtc.Remuneracao,
                    EPosicaoBtc.Taxa,
                    EPosicaoBtc.TipoContrato, 
                    EPosicaoBtc.SubtotalQuantidade,
                    EPosicaoBtc.SubtotalValor);
            }

            List<Transporte_Termo> lPosicaoTermo = this.BuscarRelatorioTermo();

            lBuilder.AppendLine("CodigoCliente\tInstrumento\tSubtotalQuant.\tPreçoMedio\tSubtotalContrato\tSubtotalLP\t\r\n");

            foreach (Transporte_Termo EPosicaoTermo in lPosicaoTermo)
            {
                lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t\r\n", EPosicaoTermo.CodigoCliente, EPosicaoTermo.Instrumento, EPosicaoTermo.SubtotalQuantidade, EPosicaoTermo.PrecoMedio, EPosicaoTermo.SubtotalContrato, EPosicaoTermo.SubtotalLucroPrejuizo);
            }

            Response.Clear();

            Response.ContentType = "text/xls";

            Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

            Response.Charset = "iso-8859-1";

            Response.AddHeader("content-disposition", "attachment;filename=RendaVariavel.xls");

            Response.Write(lBuilder.ToString());

            Response.End();
        }

        private byte[] GerarRelatorio(
            string pCaminhoRelatorio,
            string pCaminhoDoArquivo,
            out string pMymeType)
        {
            string lNome = base.SessaoClienteLogado.Nome;

            LocalReport lReport = new LocalReport();

            lReport.ReportPath = pCaminhoRelatorio;

            List<TransportePosicaoAtual> lBovespa = this.BuscarRelatorioBovespa();

            List<TransportePosicaoAtual> lBmf = this.BuscarRelatorioBmf();

            List<Transporte_BTC> lBtc = this.BuscarRelatorioBtc();

            List<Transporte_Termo> lTermo = this.BuscarRelatorioTermo();

            List<ReportParameter> lParametros = new List<ReportParameter>();

            ReportParameter lParamData = new ReportParameter("pData", DateTime.Now.ToString("dd/MM/yyyy"));
            lParametros.Add(lParamData);

            ReportParameter lParamCliente = new ReportParameter("pCliente", base.SessaoClienteLogado.CodigoPrincipal + "-" + base.SessaoClienteLogado.Nome);
            lParametros.Add(lParamCliente);

            ReportParameter lParamCpfCnpjCliente = new ReportParameter("pCpfCnpj", base.SessaoClienteLogado.CpfCnpj);
            lParametros.Add(lParamCpfCnpjCliente);

            ReportDataSource lSourceBov = new ReportDataSource("ECustodiaBov", lBovespa);

            ReportDataSource lSourceBmf = new ReportDataSource("ECustodiaBmf", lBmf);

            ReportDataSource lSourceBtc = new ReportDataSource("ECustodiaBtc", lBtc);

            ReportDataSource lSourceTermo = new ReportDataSource("ECustodiaTermo", lTermo);

            lReport.DataSources.Add(lSourceBov);

            lReport.DataSources.Add(lSourceBmf);

            lReport.DataSources.Add(lSourceBtc);

            lReport.DataSources.Add(lSourceTermo);

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

        protected void btnImprimirPDF_Click(object sender, EventArgs e)
        {
            try
            {
                string lNomeDoArquivo = string.Format("RendaVariavel_{0}_{1}", base.SessaoClienteLogado.CodigoPrincipal, DateTime.Now.ToString("yyyyMMddHHmmss"));

                string lMimeType;

                byte[] lRenderedBytes = this.GerarRelatorio(
                    Server.MapPath(@"..\Reports\RendaVariavel.rdlc"),
                    lNomeDoArquivo,
                    out lMimeType);

                //Clear the response stream and write the bytes to the outputstream  //Set content-disposition to "attachment" so that user is prompted to take an action  //on the file (open or save)
                Response.Clear();
                Response.ContentType = lMimeType;
                Response.AddHeader("content-disposition", "attachment; filename=" + lNomeDoArquivo + ".pdf");
                Response.BinaryWrite(lRenderedBytes);
                Response.End();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao gerar o PDF de Renda Variável.");
            }
        }

        protected void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {
                string lNomeDoArquivo = string.Format("RendaVariavel_{0}_{1}", base.SessaoClienteLogado.CodigoPrincipal, DateTime.Now.ToString("yyyyMMddHHmmss"));

                string lMimeType;

                byte[] lRenderedBytes = this.GerarRelatorio(
                    Server.MapPath(@"..\Reports\RendaVariavel.rdlc"),
                    lNomeDoArquivo,
                    out lMimeType);

                var lAnexos = new List<OMS.Email.Lib.EmailAnexoInfo>();
                EmailAnexoInfo lEmailInfo = new EmailAnexoInfo();
                lEmailInfo.Arquivo = lRenderedBytes;
                lEmailInfo.Nome = string.Concat(lNomeDoArquivo, ".pdf");
                lAnexos.Add(lEmailInfo);

                Dictionary<string, string> lVariaveis = new Dictionary<string, string>();
                //base.EnviarEmail("bribeiro@gradualinvestimentos.com.br", "Extrato - Gradual Investimentos", "Extrato.htm", lVariaveis, Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, lAnexos, null);
                base.EnviarEmail(base.SessaoClienteLogado.Email, "Renda Variavel - Gradual Investimentos", "RendaVariavel.htm", lVariaveis, Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, lAnexos, null);

                base.ExibirMensagemJsOnLoad("I", "Um E-mail com o arquivo Pdf foi enviado para " + base.SessaoClienteLogado.Email);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao Enviar email com pdf de Renda Variável.");
            }
        }

        protected void btnImprimirExcel_Click(object sender, EventArgs e)
        {
            try
            {
                this.ExportarExcel();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao Enviar email com xls de Renda Variável.");
            }
        }

        
        #endregion

        
    }
}
