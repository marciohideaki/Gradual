using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;
using Gradual.OMS.RelatoriosFinanc.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Library;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;
using System.Text;
using EvoPdf.HtmlToPdf;
using Newtonsoft.Json;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using System.Configuration;

namespace Gradual.Intranet.Www.Intranet.Posicao
{
    public partial class NotaDeCorretagemBmf :PaginaBase
    {
        #region Atributes
        private List<DateTime> gDataUltimasNegociacao 
        {
            get { return Session["Negociacoes_" + GetCodigoBMF] as List<DateTime> ;}
            set { Session["Negociacoes_" + GetCodigoBMF] = value; }
        }

        private CultureInfo gCultureInfo = new CultureInfo("pt-BR");

        private DateTime DataInicialNova;
        #endregion

        #region Propriedades
        private DateTime GetDataInicial
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request["DataInicial"], out lRetorno);

                return lRetorno;
            }
        }

        private DateTime GetDataInicialPaginacao
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request["DataInicialPaginacao"], out lRetorno);

                return lRetorno;
            }
        }

        private DateTime GetDataFinal
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request["DataFinal"], gCultureInfo, DateTimeStyles.AdjustToUniversal, out lRetorno);

                return lRetorno;
            }
            set
            {
                GetDataFinal = value;
            }
        }

        private DateTime GetDataInicialNova
        {
            get
            {
                return DataInicialNova;
            }
            set
            {
                DataInicialNova = value;
            }
        }

        private string GetBolsa
        {
            get
            {
                var lRetorno = this.Request["Bolsa"];

                if (string.IsNullOrEmpty(lRetorno))
                    return string.Empty;

                return Server.HtmlEncode(lRetorno);
            }
        }

        private int GetCodCliente
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request["CdBovespaCliente"], out lRetorno);

                if (lRetorno == default(int))
                    int.TryParse(this.Request["CdBmfCliente"], out lRetorno);

                return lRetorno;
            }
        }

        private int GetCodigoBovespa
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request["CdBovespaCliente"], out lRetorno);

                return lRetorno;
            }
        }

        private int GetCodigoBMF
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request["CdBmfCliente"], out lRetorno);

                return lRetorno;
            }
        }

        private string GetNomeCliente
        {
            get
            {
                return this.Request["NomeCliente"];
            }
        }

        private int GetCodCorretora
        {
            get
            {
                return 2271;
            }
        }

        private string GetTipoMercado
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["TipoMercado"]))
                    return string.Empty;

                return this.Request["TipoMercado"];
            }
        }
        private List<DateTime> GetUltimasDatasNegociacao
        {
            get
            {
                if (null == gDataUltimasNegociacao)
                {
                    var lRequest = new ConsultarEntidadeRequest<UltimasNegociacoesInfo>()
                    {
                        Objeto = new UltimasNegociacoesInfo()
                        {
                            CdCliente = this.GetCodigoBovespa,
                            CdClienteBmf = this.GetCodigoBMF,
                            DataDe = this.GetDataInicial,
                            DataAte = this.GetDataFinal
                        }
                    };

                    var lResponse = new PersistenciaDbIntranet().ConsultarObjetos<UltimasNegociacoesInfo>(lRequest);

                    if ((null != lResponse)
                    && ((null != lResponse.Resultado))
                    && ((lResponse.Resultado.Count > 0)))
                    {
                        gDataUltimasNegociacao = new List<DateTime>();

                        lResponse.Resultado.ForEach(uni =>
                        {
                            DateTime lData;

                            DateTime.TryParseExact(uni.DtUltimasNegociacoes.ToString("dd/MM/yyyy"), "dd/MM/yyyy", gCultureInfo, DateTimeStyles.None, out lData);
                            
                            if (!gDataUltimasNegociacao.Contains(lData))
                            {
                                if (uni.TipoBolsa == "BMF")
                                {
                                    gDataUltimasNegociacao.Add(lData);
                                }
                            }
                        });
                    }
                }

                return gDataUltimasNegociacao;
            }
        }
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.Request["Acao"] == "CarregarComoCSV")
                {
                    this.ConvertURLToPDF(Request.Url.AbsoluteUri, "NotaDeCorretagem", false);
                }
                else if (this.Request["Acao"] == "CarregarComPaginacao")
                {
                    this.FormatarPaginacao();
                    this.CarregarHtml();
                }
                else
                {
                    this.FormatarPaginacao();
                    this.CarregarHtml();
                }
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax("Houve um erro ao processar a Nota de Corretagem", ex);
            }
        }
        #endregion

        #region Metodos
        private void ConvertURLToPDF(string pUrl, string pfileName, bool pEnviarEmail)
        {
            string urlToConvert = pUrl;// textBoxWebPageURL.Text.Trim();

            // Create the PDF converter. Optionally the HTML viewer width can be specified as parameter
            // The default HTML viewer width is 1024 pixels.
            PdfConverter pdfConverter = new PdfConverter();

            // set the license key - required
            pdfConverter.LicenseKey = "B4mYiJubiJiInIaYiJuZhpmahpGRkZE=";

            pdfConverter.EvoInternalFileName = ConfigurationManager.AppSettings["pathEvoInternal"].ToString();

            // set the converter options - optional
            pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
            pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.NoCompression;
            pdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Landscape;

            // set if header and footer are shown in the PDF - optional - default is false 
            pdfConverter.PdfDocumentOptions.ShowHeader = false;////cbAddHeader.Checked;
            pdfConverter.PdfDocumentOptions.ShowFooter = false;// cbAddFooter.Checked;
            // set if the HTML content is resized if necessary to fit the PDF page width - default is true
            //pdfConverter.PdfDocumentOptions.FitWidth = true;// cbFitWidth.Checked;
            //pdfConverter.PdfDocumentOptions.StretchToFit = true;
            pdfConverter.PdfDocumentOptions.AutoSizePdfPage = false;
            //pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.

            pdfConverter.PdfDocumentOptions.RightMargin = 5;

            // set the embedded fonts option - optional - default is false
            pdfConverter.PdfDocumentOptions.EmbedFonts = false;// cbEmbedFonts.Checked;
            // set the live HTTP links option - optional - default is true
            pdfConverter.PdfDocumentOptions.LiveUrlsEnabled = false;// cbLiveLinks.Checked;

            // set if the JavaScript is enabled during conversion to a PDF - default is true
            pdfConverter.JavaScriptEnabled = true;// cbClientScripts.Checked;

            // set if the images in PDF are compressed with JPEG to reduce the PDF document size - default is true
            pdfConverter.PdfDocumentOptions.JpegCompressionEnabled = true;// cbJpegCompression.Checked;

            pdfConverter.ConversionDelay = 5;

            // be saved to a file or sent as a browser response
            byte[] pdfBytes = pdfConverter.GetPdfBytesFromUrl(urlToConvert);

            this.Response.Clear();
            this.Response.ContentType = "text/pdf";
            this.Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
            this.Response.Charset = "iso-8859-1";
            this.Response.AddHeader("content-disposition", "attachment;filename=RelatorioDeNotasDeCorretagem.csv");
            //this.Response.Write(lBuilder.ToString());
            this.Response.End();

        }

        private void FormatarPaginacao()
        {
            DateTime dtFinal = this.GetDataFinal;

            string lListaDatas = string.Empty;//dtFinal.ToString("dd/MM/yyyy") + ";";

            List<DateTime> ListaDataNegociada = this.GetUltimasDatasNegociacao;

            while(dtFinal != this.GetDataInicialPaginacao)
            {
                dtFinal = dtFinal.AddDays(-1);
                
                if (ListaDataNegociada.Contains(Convert.ToDateTime(dtFinal)))
                {
                    lListaDatas += dtFinal.ToString("dd/MM/yyyy") + ";";
                }
            }

            this.hddDatasPaginacao.Value = JsonConvert.SerializeObject(lListaDatas.TrimEnd(';'));

            if (lListaDatas.Length > 10)
            {
                string[] Datas = lListaDatas.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                DateTime lDateInicial = Convert.ToDateTime(Datas[Datas.Length - 1]);

                this.GetDataInicialNova = lDateInicial;
            }
            else if (lListaDatas == string.Empty)
            {
                this.GetDataInicialNova = this.GetDataInicial;
            }

        }

        private void CarregarHtml()
        {
            var lRelatorio = this.CarregarRelatorio();

            this.rptLinhasDoRelatorio.DataSource = new TransporteNotaDeCorretagemExtratoBmf().TraduzirLista(lRelatorio.ListaNotaDeCorretagemExtratoBmfInfo);
            this.rptLinhasDoRelatorio.DataBind();

            this.lblCabecalho_Provisorio.Visible = DateTime.Today.Equals(this.GetDataInicial);
            this.lblCabecalho_DataEmissao.Text   = this.GetDataInicial.ToString("dd/MM/yyyy");
            this.lblCodigoCliente.Text           = this.GetCodigoBMF.ToString().PadLeft(10, '0');
            this.lblNomeCliente.Text             = this.GetNomeCliente;//.ToStringFormatoNome();
            this.lblNumeroDaNota.Text            = lRelatorio.Rodape.NumeroDaNota;
            this.lblCpfCnpjCliente.Text          = lRelatorio.Rodape.DsCpfCnpj;
            this.lblCodigoDoCliente.Text         = this.GetCodigoBMF.ToString().PadLeft(10, '0');

            //this.lblCabecalho_Cliente_NumeroDaConta.Text   = lRelatorio.CabecalhoCliente.ContaCorrente;
            //this.lblCabecalho_Cliente_NumeroDaAgencia.Text = lRelatorio.CabecalhoCliente.Agencia;
            //this.lblCabecalho_Cliente_NumeroDoBanco.Text   = lRelatorio.CabecalhoCliente.NrBanco;

            this.lblRodape_VendaDisponivel.Text  = Math.Abs(lRelatorio.Rodape.VendaDisponivel).ToString("N2");
            this.lblRodape_CompraDisponivel.Text = Math.Abs(lRelatorio.Rodape.CompraDisponivel).ToString("N2");
            this.lblRodape_VendaOpcoes.Text      = Math.Abs(lRelatorio.Rodape.VendaOpcoes).ToString("N2");
            this.lblRodape_CompraOpcoes.Text     = Math.Abs(lRelatorio.Rodape.CompraOpcoes).ToString("N2");
            this.lblRodape_ValorNegocio.Text     = Math.Abs(lRelatorio.Rodape.ValorNegocios).ToString("N2");
            this.lblRodape_IRRF.Text             = Math.Abs(lRelatorio.Rodape.IRRF).ToString("N2");
            this.lblRodape_IRRFDayTrade.Text     = Math.Abs(lRelatorio.Rodape.IRRFDayTrade).ToString("N2");
            this.lblRodape_TaxaOperacional.Text  = Math.Abs(lRelatorio.Rodape.TaxaOperacional).ToString("N2");
            this.lblRodape_TaxaRegistroBmf.Text  = Math.Abs(lRelatorio.Rodape.TaxaRegistroBmf).ToString("N2");
            this.lblRodape_TaxasBmf.Text         = Math.Abs(lRelatorio.Rodape.TaxaBmf).ToString("N2");
            this.lblRodape_ISS.Text              = Math.Abs(lRelatorio.Rodape.ISS).ToString("N2");
            this.lblRodape_AjustePosicao.Text    = Math.Abs(lRelatorio.Rodape.AjustePosicao).ToString("N2");
            this.lblRodape_AjusteDayTrade.Text   = Math.Abs(lRelatorio.Rodape.AjusteDayTrade).ToString("N2");
            this.lblRodape_TotalDespesas.Text    = Math.Abs(lRelatorio.Rodape.TotalDespesas).ToString("N2");
            this.lblRodape_IRRFCorretagem.Text   = Math.Abs(lRelatorio.Rodape.IrrfCorretagem).ToString("N2");
            this.lblRodape_TotalContaNormal.Text = Math.Abs(lRelatorio.Rodape.TotalContaNormal).ToString("N2");
            this.lblRodape_TotalLiquido.Text     = Math.Abs(lRelatorio.Rodape.TotalLiquido).ToString("N2");
            this.lblRodape_TotalLiquidoNota.Text = Math.Abs(lRelatorio.Rodape.TotalLiquidoNota).ToString("N2");
        }

        private NotaDeCorretagemExtratoBmfInfo CarregarRelatorio()
        {
            try
            {
                var lServicoAtivador = Ativador.Get<IServicoRelatoriosFinanceiros>();

                var lResponse = lServicoAtivador.ConsultarNotaDeCorretagemBmf(
                    new NotaDeCorretagemExtratoRequest()
                    {
                        ConsultaCodigoCliente   = this.GetCodigoBovespa,
                        ConsultaCodigoCorretora = this.GetCodCorretora, //ConfiguracoesValidadas.CodigoCorretora,
                        ConsultaDataMovimento   = Request["NumeroDaPagina"] == null ? this.GetDataInicialNova : this.GetDataInicial,
                        ConsultaTipoDeMercado   = this.GetTipoMercado,
                        ConsultaProvisorio      = false, // this.GetProvisorio,
                        ConsultaCodigoClienteBmf = this.GetCodigoBMF
                    });

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    base.RegistrarLogConsulta(new Contratos.Dados.Cadastro.LogIntranetInfo() { CdBovespaClienteAfetado = this.GetCodCliente, DsObservacao = string.Concat("Consulta realizada para o cliente: cd_codigo = ", this.GetCodCliente) });

                    lResponse.RelatorioBmf.CabecalhoCliente   = lResponse.Relatorio.CabecalhoCliente;
                    lResponse.RelatorioBmf.CabecalhoCorretora = lResponse.Relatorio.CabecalhoCorretora;

                    return lResponse.RelatorioBmf;
                }
                else
                {
                    throw new Exception(string.Format("{0}-{1}", lResponse.StatusResposta, lResponse.DescricaoResposta));
                }
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax("Houve um erro ao tentar gerar a nota de corretagem", ex);
                return new NotaDeCorretagemExtratoBmfInfo();
            }
        }
        #endregion
    }
}
