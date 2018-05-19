using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RelatoriosFinanc.Lib;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;
using Newtonsoft.Json;
using EvoPdf.HtmlToPdf;

namespace Gradual.Intranet.Www.Intranet.Posicao
{
    public partial class NotaDeCorretagem : PaginaBase
    {
        #region | Atributos

        private List<DateTime> gDataUltimasNegociacao
        {
            get 
            { 
                return Session["Negociacoes_" + GetCodigoBovespa] as List<DateTime>; 
            }
            set 
            {
                Session["Negociacoes_" + GetCodigoBovespa] = value; 
            }
        }

        private CultureInfo gCultureInfo = new CultureInfo("pt-BR");

        #endregion

        #region | Propriedades

        private DateTime DataInicialNova;

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
                //if (null == gDataUltimasNegociacao)
                //{
                    var lRequest = new ConsultarEntidadeRequest<UltimasNegociacoesInfo>()
                    {
                        Objeto = new UltimasNegociacoesInfo()
                        {
                            CdCliente    = this.GetCodigoBovespa,
                            CdClienteBmf = this.GetCodigoBMF,
                            DataDe       = this.GetDataInicial,
                            DataAte      = this.GetDataFinal
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
                                if (uni.TipoBolsa == "BOL")
                                {
                                    gDataUltimasNegociacao.Add(lData);
                                }
                            }
                        });
                    }
                //}

                return gDataUltimasNegociacao;
            }
        }

        #endregion

        #region | Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.Request["Acao"] == "CarregarComoCSV")
                {    
                    this.ConvertURLToPDF(Request.Url.AbsoluteUri  , "NotaDeCorretagem", false);
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

        #region | Métodos

        private void CarregarHtml()
        {
            var lRelatorio = this.CarregarRelatorio();

            this.lblCabecalho_Provisorio.Visible = DateTime.Today.Equals(this.GetDataInicial);
            this.lblCabecalho_DataEmissao.Text = lRelatorio.CabecalhoCorretora.DataPregao.ToString("dd/MM/yyyy"); //this.GetDataInicial.ToString("dd/MM/yyyy");
            this.lblCodigoCliente.Text = this.GetCodCliente.ToString().PadLeft(10, '0');
            this.lblNomeCliente.Text = this.GetNomeCliente.ToStringFormatoNome();
            //this.lblNumeroDaNota.Text = lRelatorio.CabecalhoCorretora.NumeroNota;

            //this.lblCabecalho_Cliente_NumeroDaConta.Text = lRelatorio.CabecalhoCliente.ContaCorrente;
            //this.lblCabecalho_Cliente_NumeroDaAgencia.Text = lRelatorio.CabecalhoCliente.Agencia;
            //this.lblCabecalho_Cliente_NumeroDoBanco.Text = lRelatorio.CabecalhoCliente.NrBanco;

            this.lblCabecalho_Cliente_NumeroDaNota.Text = lRelatorio.CabecalhoCliente.NrNota.ToString();

            this.rptLinhasDoRelatorio.DataSource = new TransporteExtratoNotaDeCorretagem().TraduzirLista(lRelatorio.ListaNotaDeCorretagemExtratoInfo);
            this.rptLinhasDoRelatorio.DataBind();

            this.lblRodape_Debentures.Text = lRelatorio.Rodape.Debentures.ToString("N2", gCultureInfo);
            this.lblRodape_ValorLiquidoDasOperacoes.Text = Math.Abs(lRelatorio.Rodape.ValorLiquidoOperacoes).ToString("N2", gCultureInfo);
            this.lblRodape_VendasAVista.Text = lRelatorio.Rodape.VendaVista.ToString("N2", gCultureInfo);
            this.lblRodape_TaxaDeRegistro.Text = Math.Abs(lRelatorio.Rodape.TaxaDeRegistro).ToString("N2", gCultureInfo);
            this.lblRodape_ComprasAVista.Text = lRelatorio.Rodape.CompraVista.ToString("N2", gCultureInfo);
            this.lblRodape_TaxaDeLiquidacao.Text = Math.Abs(lRelatorio.Rodape.TaxaLiquidacao).ToString("N2", gCultureInfo);
            this.lblRodape_OpcoesCompras.Text = lRelatorio.Rodape.CompraOpcoes.ToString("N2", gCultureInfo);
            this.lblRodape_Total_123_A.Text = Math.Abs(lRelatorio.Rodape.Total_123_A).ToString("N2", gCultureInfo);

            this.lblRodape_OpcoesVendas.Text = lRelatorio.Rodape.VendaOpcoes.ToString("N2", gCultureInfo);
            this.lblRodape_TaxasTermo_Opcoes_Futuro.Text = Math.Abs(lRelatorio.Rodape.TaxaTerOpcFut).ToString("N2", gCultureInfo);
            this.lblRodape_OperacoesTermo.Text = lRelatorio.Rodape.OperacoesTermo.ToString("N2", gCultureInfo);
            this.lblRodape_TaxaANA.Text = Math.Abs(lRelatorio.Rodape.TaxaANA).ToString("N2", gCultureInfo);

            this.lblRodape_OperacoesFuturo.Text = lRelatorio.Rodape.OperacoesFuturo.ToString("N2", gCultureInfo);
            this.lblRodape_Emolumentos.Text = Math.Abs(lRelatorio.Rodape.Emolumentos).ToString("N2", gCultureInfo);
            this.lblRodape_ValorOperacoesTitPub.Text = lRelatorio.Rodape.OperacoesTitulosPublicos.ToString("N2", gCultureInfo);
            this.lblRodape_Total_Bolsa_B.Text = Math.Abs(lRelatorio.Rodape.TotalBolsaB).ToString("N2", gCultureInfo);
            this.lblRodape_ValorDasOperacoes.Text = lRelatorio.Rodape.ValorDasOperacoes.ToString("N2", gCultureInfo);
            this.lblRodape_Corretagem.Text = Math.Abs(lRelatorio.Rodape.Corretagem).ToString("N2", gCultureInfo);

            this.lblRodape_AjusteFuturo.Text                = lRelatorio.Rodape.ValorAjusteFuturo.ToString("N2", gCultureInfo);
            this.lblRodape_ISS.Text                         = Math.Abs(lRelatorio.Rodape.ISS).ToString("N2", gCultureInfo);
            this.lblRodape_IRSobreCorretagem.Text           = lRelatorio.Rodape.IRSobreCorretagem.ToString("N2", gCultureInfo);
            this.lblRodape_IRRF_BaseOperacoes.Text          = lRelatorio.Rodape.IRRFSemOperacoesBase.ToString("C2", gCultureInfo);
            this.lblRodape_IRRF_SobreOperacoes.Text         = Math.Abs(lRelatorio.Rodape.IRRFSemOperacoesValor).ToString("N2", gCultureInfo);
            this.lblRodape_IRRF_SobreDayTrade.Text          = lRelatorio.Rodape.IRRFSobreDayTrade.ToString("N2", gCultureInfo);
            this.lblRodape_IRRF_SobreOperacoesBase.Text     = lRelatorio.Rodape.IRRFSobreDayTradeBase.ToString("N2", gCultureInfo);
            this.lblRodape_IRRF_SobreOperacoesProjecao.Text = lRelatorio.Rodape.IRRFSobreDayTradeProjecao.ToString("N2", gCultureInfo);

            this.lblRodape_Outras.Text = Math.Abs(lRelatorio.Rodape.Outras).ToString("N2", gCultureInfo);
            this.lblRodape_Outras_DC.Text = lRelatorio.Rodape.Outras > 0 ? "C" : "D";

            this.lblRodape_Liquido_Para.Text = lRelatorio.Rodape.DataLiquidoPara.ToString("dd/MM/yyyy");
            this.lblRodape_Liquido_AB.Text = Math.Abs(lRelatorio.Rodape.ValorLiquidoNota).ToString("N2", gCultureInfo);
            this.lblRodape_Liquido_AB_DC.Text = lRelatorio.Rodape.ValorLiquidoNota > 0 ? "C" : "D";

            this.lblRodape_ValorLiquidoDasOperacoes_DC.Text = lRelatorio.Rodape.ValorLiquidoOperacoes > 0 ? "C" : "D";
            this.lblRodape_TaxaDeRegistro_DC.Text = lRelatorio.Rodape.TaxaDeRegistro_DC;
            this.lblRodape_TaxaDeLiquidacao_DC.Text = lRelatorio.Rodape.TaxaLiquidacao_DC;
            this.lblRodape_Total_123_A_DC.Text = lRelatorio.Rodape.Total_123_A_DC;
            this.lblRodape_TaxasTermo_Opcoes_Futuro_DC.Text = lRelatorio.Rodape.TaxaTerOpcFut_DC;
            this.lblRodape_TaxaANA_DC.Text = lRelatorio.Rodape.TaxaANA_DC;
            this.lblRodape_Emolumentos_DC.Text = "D";//lRelatorio.Rodape.Emolumentos_DC;

            this.lblRodape_Total_Bolsa_BPosNeg.Text = lRelatorio.Rodape.TotalBolsaBPosNeg.ToString("N2", gCultureInfo);
            this.lblRodape_Total_Bolsa_B_DC.Text = lRelatorio.Rodape.TotalBolsaBPosNeg > 0 ? "C" : "D";

            this.lblRodape_Corretagem_DC.Text = "D"; //lRelatorio.Rodape.Corretagem_DC;
            this.lblRodape_ISS_DC.Text = lRelatorio.Rodape.ISS_DC;
            this.lblRodape_IRRF_SobreOperacoes_DC.Text = lRelatorio.Rodape.IRRFOperacoes_DC;

            this.lblRodape_ValorLiquidoDasOperacoesPosNeg.Text = lRelatorio.Rodape.ValorLiquidoOperacoesPosNeg.ToString("N2", gCultureInfo);
            this.lblRodape_TaxaDeRegistroPosNeg.Text = lRelatorio.Rodape.TaxaDeRegistroPosNeg.ToString("N2", gCultureInfo);
            this.lblRodape_TaxaDeLiquidacaoPosNeg.Text = lRelatorio.Rodape.TaxaLiquidacaoPosNeg.ToString("N2", gCultureInfo);
            this.lblRodape_Total_123_APosNeg.Text = lRelatorio.Rodape.Total_123_APosNeg.ToString("N2", gCultureInfo);
            this.lblRodape_TaxasTermo_Opcoes_FuturoPosNeg.Text = lRelatorio.Rodape.TaxaTerOpcFutPosNeg.ToString("N2", gCultureInfo);
            this.lblRodape_TaxaANAPosNeg.Text = lRelatorio.Rodape.TaxaANAPosNeg.ToString("N2", gCultureInfo);
            this.lblRodape_EmolumentosPosNeg.Text = lRelatorio.Rodape.EmolumentosPosNeg.ToString("N2", gCultureInfo);
            this.lblRodape_CorretagemPosNeg.Text = lRelatorio.Rodape.CorretagemPosNeg.ToString("N2", gCultureInfo);
            this.lblRodape_ISSPosNeg.Text = lRelatorio.Rodape.ISSPosNeg.ToString("N2", gCultureInfo);
            this.lblRodape_IRRF_SobreOperacoesPosNeg.Text = lRelatorio.Rodape.IRRFOperacoesPosNeg.ToString("N2", gCultureInfo);
        }

        private void FormatarPaginacao()
        {
            DateTime dtFinal = this.GetDataFinal;

            string lListaDatas = string.Empty;// dtFinal.ToString("dd/MM/yyyy") + ";";

            List<DateTime> ListaDataNegociada = this.GetUltimasDatasNegociacao;

            while (dtFinal != this.GetDataInicialPaginacao)
            {
                dtFinal = dtFinal.AddDays(-1);

                if (ListaDataNegociada.Contains(Convert.ToDateTime(dtFinal)))
                {
                    lListaDatas += dtFinal.ToString("dd/MM/yyyy") + ";";
                }
            }

            if (this.GetUltimasDatasNegociacao != null && this.GetUltimasDatasNegociacao.Count > 0
                && this.GetUltimasDatasNegociacao.Contains(this.GetDataFinal))
            {
                lListaDatas = string.Concat(this.GetDataFinal.ToString("dd/MM/yyyy"),";", lListaDatas);
            }

            if (lListaDatas.Length > 10)
            {
                string[] Datas = lListaDatas.Split(new string[] {";"},StringSplitOptions.RemoveEmptyEntries);

                DateTime lDateInicial = Convert.ToDateTime(Datas[Datas.Length-1]);

                this.GetDataInicialNova = lDateInicial;
            }
            else if (lListaDatas == string.Empty)
            {
                this.GetDataInicialNova = this.GetDataInicial;
            }
            

            this.hddDatasPaginacao.Value = JsonConvert.SerializeObject(lListaDatas.TrimEnd(';'));

            //var lDatasDasNotasSelecionadas = this.GetUltimasDatasNegociacao.FindAll(udn => { return udn >= this.GetDataInicialPaginacao && udn <= this.GetDataFinal; });

            //if (null != lDatasDasNotasSelecionadas && lDatasDasNotasSelecionadas.Count > 0)
            //{
            //    var lListaDatas = string.Empty;

            //    lDatasDasNotasSelecionadas.ForEach(dns => {
            //        if (!lListaDatas.Contains(dns.ToString("dd/MM/yyyy")))
            //            lListaDatas += dns.ToString("dd/MM/yyyy") + ";"; 
            //    });

            //    this.hddDatasPaginacao.Value = JsonConvert.SerializeObject(lListaDatas.TrimEnd(';'));
            //}
        }

        /*public void ProcessRequest(HttpContext context)
        {
            StringBuilder url = new StringBuilder();
            if (context != null)
            {
                string lfileName = "{0}_{1}.pdf";

                url.Append(context.Request.Url.AbsoluteUri.Replace(this.GetType().Name + ".ashx", "Financeiro/RelExtratoMensal.aspx"));
                lfileName = string.Format(lfileName, "NotaDeCorretagem", DateTime.Now.Ticks.ToString());
                        
                this.ConvertURLToPDF(context, url.ToString(), lfileName, false);
            }
        }*/

        /// <summary>
        /// Convert the HTML code from the specified URL to a PDF document
        /// and send the document to the browser
        /// </summary>
        private void ConvertURLToPDF(string pUrl, string pfileName, bool pEnviarEmail)
        {
            string urlToConvert = pUrl;// textBoxWebPageURL.Text.Trim();

            // Create the PDF converter. Optionally the HTML viewer width can be specified as parameter
            // The default HTML viewer width is 1024 pixels.
            PdfConverter pdfConverter = new PdfConverter();

            // set the license key - required
            pdfConverter.LicenseKey = "ORIJGQoKGQkZCxcJGQoIFwgLFwAAAAA=";

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

        /*private void ResponderArquivoCSV()
        {
            var lRelatorio = this.CarregarRelatorio();

            StringBuilder lBuilder = new StringBuilder();

            CultureInfo lCultureInfo = new CultureInfo("pt-BR");

            var lListaCorretagem = new TransporteExtratoNotaDeCorretagem().TraduzirLista(lRelatorio.ListaNotaDeCorretagemExtratoInfo);

            /*
             //Exemplo de arquivo csv para esse relatorio:


                Relatório: Notas de Corretagem
                Cliente:0000031940 - RAFAEL SANCHES GARCIA
                Conta: Agência: Banco:
                Praça     C/V     Tipo de Mercado      Espec. do Título      Obs.     Quantidade     Preço     Valor (R$)     D/C

                Resumo dos Negócios
            
                Debêntures                          0,00
                Vendas à Vista                      0,00
                Compras à Vista                     0,00
                Opções - Compras                    0,00
                Opções - Vendas                     0,00
                Operações a Termo                   0,00
                Operações a Futuro                  0,00
                Valor das Oper. com Tit. Publ.      0,00
                Valor das Operações                 0,00
                Valor do Ajuste p/Futuro            0,00
                IR Sobre Corretagem                 0,00
                IRRF Sobre Day Trade                0,00

                Resumo Financeiro

                Valor Líquido das Operações(1)      0,00
                Taxa de Registro(3)                 0,00
                Taxa de Liquidação(2)               0,00
                Total (1+2+3) A                     0,00
                Taxa de Termo/Opções/Futuro         0,00
                Taxa A.N.A                          0,00
                Emolumentos                         0,00
                Total Bolsa B                       0,00
                Corretagem                          0,00
                ISS                                 0,00
                I.R.R.F. s/ operações, base R$ 0,00 0,00
                Outras                              0,00
                Liquido (A+B) para 06/04/2010       0,00
                
            */
        /*
            lBuilder.AppendLine("Relatório: Notas de Corretagem\r\n");

            lBuilder.AppendLine("GRADUAL C.C.T.V.M. S/A.");

            lBuilder.AppendLine("AV JUSCELINO KUBITSCHEK, 50 - 6 ANDAR ITAIM;SAO PAULO - SP;CEP: 04543-000");

            lBuilder.AppendLine("Tel. (55 11) 3372-8300;Fax: (55 11) 3372-8301");

            lBuilder.AppendLine("Internet: www.gradualinvestimentos.com.br;e-mail: atendimento@gradualinvestimentos.com.br");

            lBuilder.AppendLine("C.N.P.J.: 33.918.160/0001-73;Carta Patente: A68/3855");

            lBuilder.AppendLine("Ouvidoria: Tel.: 0800-655-1466;e-mail ouvidoria: ouvidoria@gradualinvestimentos.com.br\r\n");

            lBuilder.AppendFormat("Cliente:;{0}-{1}\r\n", lRelatorio.CodigoCliente.ToCodigoClienteFormatado(), lRelatorio.CabecalhoCliente.NomeCliente.ToStringFormatoNome());

            lBuilder.AppendFormat("{0};C.P.F./C.N.P.J./C.V.M./C.O.B.: {1}\r\n\r\n", 
                lRelatorio.CabecalhoCliente.EnderecoCliente, lRelatorio.CabecalhoCliente.CpfCnpj);

            lBuilder.AppendFormat("Conta: {0};Agência: {1};Banco:;{2}\r\n", lRelatorio.CabecalhoCliente.ContaCorrente, lRelatorio.CabecalhoCliente.Agencia, lRelatorio.CabecalhoCliente.NrBanco);

            lBuilder.AppendLine("Praça;C/V;Tipo de Mercado;Espec. do Título;Obs.;Quantidade;Preço;Valor (R$);D/C");

            if (lListaCorretagem.Count > 0)
            {
                foreach (TransporteExtratoNotaDeCorretagem lItem in lListaCorretagem)
                {
                    lBuilder.AppendFormat("{0};{1};{2};{3};{4};{5};{6};{7};{8}\r\n"
                                         , lItem.NomeBolsa
                                         , lItem.TipoOperacao
                                         , lItem.TipoMercado
                                         , lItem.EspecificacaoTitulo
                                         , string.IsNullOrWhiteSpace(lItem.Observacao) ? string.Empty : lItem.Observacao.Replace("N", string.Empty)
                                         , lItem.Quantidade
                                         , lItem.ValorNegocio
                                         , lItem.ValorTotal
                                         , lItem.DC);
                }
            }
            else
            {
                lBuilder.AppendLine("(0 lançamentos encontrados)");
            }
            
            lBuilder.AppendLine("");

            lBuilder.AppendLine("Resumo dos Negócios");

            lBuilder.AppendFormat("Debêntures:;{0}\r\n", lRelatorio.Rodape.Debentures.ToString("N2", gCultureInfo));
            lBuilder.AppendFormat("Vendas à Vista:;{0}\r\n", lRelatorio.Rodape.VendaVista.ToString("N2", gCultureInfo));
            lBuilder.AppendFormat("Compras à Vista:;{0}\r\n", lRelatorio.Rodape.CompraVista.ToString("N2", gCultureInfo));
            lBuilder.AppendFormat("Opções - Compras:;{0}\r\n", lRelatorio.Rodape.CompraOpcoes.ToString("N2", gCultureInfo));
            lBuilder.AppendFormat("Opções - Vendas:;{0}\r\n", lRelatorio.Rodape.VendaOpcoes.ToString("N2", gCultureInfo));
            lBuilder.AppendFormat("Operações a Termo:;{0}\r\n", lRelatorio.Rodape.OperacoesTermo.ToString("N2", gCultureInfo));
            lBuilder.AppendFormat("Operações a Futuro:;{0}\r\n", lRelatorio.Rodape.OperacoesFuturo.ToString("N2", gCultureInfo));
            lBuilder.AppendFormat("Valor das Oper. com Tit. Publ.:;{0}\r\n", lRelatorio.Rodape.OperacoesTitulosPublicos.ToString("N2", gCultureInfo));
            lBuilder.AppendFormat("Valor das Operações:;{0}\r\n", lRelatorio.Rodape.ValorDasOperacoes.ToString("N2", gCultureInfo));
            lBuilder.AppendFormat("Valor do Ajuste p/Futuro:;{0}\r\n", lRelatorio.Rodape.ValorAjusteFuturo.ToString("N2", gCultureInfo));
            lBuilder.AppendFormat("IR Sobre Corretagem:;{0}\r\n", lRelatorio.Rodape.IRSobreCorretagem.ToString("N2", gCultureInfo));
            lBuilder.AppendFormat("IRRF Sobre Day Trade:;{0}\r\n", lRelatorio.Rodape.IRRFSobreDayTrade.ToString("N2", gCultureInfo));

            lBuilder.AppendLine("");

            lBuilder.AppendLine("Resumo Financeiro;;D/C");

            lBuilder.AppendFormat("Valor Líquido das Operações(1):;{0};{1}\r\n", Math.Abs(lRelatorio.Rodape.ValorLiquidoOperacoes).ToString("N2", gCultureInfo), lRelatorio.Rodape.ValorLiquidoOperacoes > 0 ? "C" : "D");
            lBuilder.AppendFormat("Taxa de Registro(3):;{0};{1}\r\n", Math.Abs(lRelatorio.Rodape.TaxaDeRegistro).ToString("N2", gCultureInfo), lRelatorio.Rodape.TaxaDeRegistro_DC);
            lBuilder.AppendFormat("Taxa de Liquidação(2):;{0};{1}\r\n", Math.Abs(lRelatorio.Rodape.TaxaLiquidacao).ToString("N2", gCultureInfo), lRelatorio.Rodape.TaxaLiquidacao_DC);
            lBuilder.AppendFormat("Total (1+2+3) A:;{0};{1}\r\n", Math.Abs(lRelatorio.Rodape.Total_123_A).ToString("N2", gCultureInfo), lRelatorio.Rodape.Total_123_A_DC);
            lBuilder.AppendFormat("Taxa de Termo/Opções/Futuro:;{0};{1}\r\n", Math.Abs(lRelatorio.Rodape.TaxaTerOpcFut).ToString("N2", gCultureInfo), lRelatorio.Rodape.TaxaTerOpcFut_DC);
            lBuilder.AppendFormat("Taxa A.N.A:;{0};{1}\r\n", Math.Abs(lRelatorio.Rodape.TaxaANA).ToString("N2", gCultureInfo), lRelatorio.Rodape.TaxaANA_DC);
            lBuilder.AppendFormat("Emolumentos:;{0};{1}\r\n", Math.Abs(lRelatorio.Rodape.Emolumentos).ToString("N2", gCultureInfo), "D");
            lBuilder.AppendFormat("Total Bolsa B:;{0};{1}\r\n", Math.Abs(lRelatorio.Rodape.TotalBolsaB).ToString("N2", gCultureInfo), lRelatorio.Rodape.TotalBolsaBPosNeg > 0 ? "C" : "D");
            lBuilder.AppendFormat("Corretagem:;{0};{1}\r\n", Math.Abs(lRelatorio.Rodape.Corretagem).ToString("N2", gCultureInfo), "D");
            lBuilder.AppendFormat("ISS:;{0};{1}\r\n", Math.Abs(lRelatorio.Rodape.ISS).ToString("N2", gCultureInfo), lRelatorio.Rodape.ISS_DC);
            lBuilder.AppendFormat("I.R.R.F. s/ operações, base {0}:;{1};{2}\r\n", lRelatorio.Rodape.VLBaseOperacoesIRRF.ToString("C2", gCultureInfo), Math.Abs(lRelatorio.Rodape.IRRFOperacoes).ToString("N2", gCultureInfo), lRelatorio.Rodape.IRRFOperacoes_DC);
            lBuilder.AppendFormat("Outras:;{0};{1}\r\n", Math.Abs(lRelatorio.Rodape.Outras).ToString("N2", gCultureInfo), lRelatorio.Rodape.Outras > 0 ? "C" : "D");
            lBuilder.AppendFormat("Liquido (A+B) {0}:;{1};{2}", lRelatorio.Rodape.DataLiquidoPara.ToString("dd/MM/yyyy"), Math.Abs(lRelatorio.Rodape.ValorLiquidoNota).ToString("N2", gCultureInfo), lRelatorio.Rodape.ValorLiquidoNota > 0 ? "C" : "D");


            string urlToConvert = "";

            PdfConverter pdfConverter = new PdfConverter();

            // set the license key - required
            pdfConverter.LicenseKey = "ORIJGQoKGQkZCxcJGQoIFwgLFwAAAAA=";

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

            byte[] pdfBytes = pdfConverter.GetPdfBytesFromUrl(urlToConvert);

            this.Response.Clear();
            this.Response.ContentType = "text/pdf";
            this.Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
            this.Response.Charset = "iso-8859-1";
            this.Response.AddHeader("content-disposition", "attachment;filename=RelatorioDeNotasDeCorretagem.csv");
            this.Response.Write(lBuilder.ToString());
            this.Response.End();
        }*/

        private NotaDeCorretagemExtratoInfo CarregarRelatorio()
        {
            try
            {
                var lServicoAtivador = Ativador.Get<IServicoRelatoriosFinanceiros>();

                var lResponse = lServicoAtivador.ConsultarNotaDeCorretagem(
                    new NotaDeCorretagemExtratoRequest()
                    {
                        ConsultaCodigoCliente   = this.GetCodCliente,
                        ConsultaCodigoCorretora = this.GetCodCorretora, //ConfiguracoesValidadas.CodigoCorretora,
                        ConsultaDataMovimento   = Request["NumeroDaPagina"] == null ? this.GetDataInicialNova : this.GetDataInicial,
                        ConsultaTipoDeMercado   = this.GetTipoMercado,
                        ConsultaProvisorio      = false, // this.GetProvisorio,
                    });

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    base.RegistrarLogConsulta(new Contratos.Dados.Cadastro.LogIntranetInfo() { CdBovespaClienteAfetado = this.GetCodCliente, DsObservacao = string.Concat("Consulta realizada para o cliente: cd_codigo = ", this.GetCodCliente) });

                    return lResponse.Relatorio;
                }
                else
                {
                    throw new Exception(string.Format("{0}-{1}", lResponse.StatusResposta, lResponse.DescricaoResposta));
                }
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax("Houve um erro ao tentar gerar a nota de corretagem", ex);
                return new NotaDeCorretagemExtratoInfo();
            }
        }

        #endregion
    }
}
