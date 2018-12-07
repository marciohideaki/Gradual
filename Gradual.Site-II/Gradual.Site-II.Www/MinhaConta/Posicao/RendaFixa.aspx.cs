using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Site.DbLib;
using Gradual.Site.DbLib.Mensagens;
using Gradual.OMS.Library.Servicos;
using System.Text;
using Gradual.Site.DbLib.Dados.MinhaConta;
using Gradual.OMS.TesouroDireto.Lib.Dados;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Compra;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Consultas;
using System.Globalization;
using Gradual.OMS.PosicaoBTC.Lib;
using Gradual.OMS.TesouroDireto.Lib;
using Gradual.OMS.Library;
using Gradual.OMS.Email.Lib;
using Microsoft.Reporting.WebForms;



namespace Gradual.Site.Www.MinhaConta.Posicao
{
    public partial class RendaFixa : PaginaTesouroDireto
    {
        #region Propriedades
        
        private string gMensagemInstabilidade = "Prezado Cliente,\r\n\r\nEstamos em fase de migração do sistema que gerencia as suas posições. Para consultar sua poisção com segurança ligue:\r\n\r\n4007-1873 | Região Metropolitana\r\n0800 655 1873 | Demais Regiões";
        
        private IServicoTesouroDireto ServicoTesouro
        {
            get
            {
                return InstanciarServicoDoAtivador<IServicoTesouroDireto>();
            }
        }

        public String CPFdoCliente
        {
            get
            {
                return base.SessaoClienteLogado.CpfCnpj;
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

                        this.CarregarGrids();

                        string lScript = "MinhaConta_GerarGrafico_RendaFixa(); GradSite_VerificarPositivoseNegativos('#tblSaldo td.ValorNumerico');";

                        if (this.Request.UrlReferrer.AbsolutePath.ToLower().Contains("produtos.aspx"))
                        {
                            lScript += "MinhaConta_RendaFixa_TD_LoadAccordeon_Click('Compra');";
                        }

                        RodarJavascriptOnLoad(lScript);
                    }
                    else
                    {
                        this.CarregarGrids();
                        if (this.Posicao_Acordeon_Selecionado.Value == "s")
                        {
                            RodarJavascriptOnLoad("MinhaConta_RendaFixa_TD_LoadAccordeon_Click('" + this.Posicao_Aba_Selecionada.Value + "'); MinhaConta_GerarGrafico_RendaFixa();");
                            //RodarJavascriptOnLoad("MinhaConta_GerarGrafico_RendaFixa(); GradSite_VerificarPositivoseNegativos('#tblSaldo td.ValorNumerico');");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
            }
        }

        protected void btnImprimirPDF_Click(object sender, EventArgs e)
        {
            try
            {
                string lNomeDoArquivo = string.Format("RendaFixa_{0}_{1}", base.SessaoClienteLogado.CodigoPrincipal, DateTime.Now.ToString("yyyyMMddHHmmss"));

                string lMimeType;

                byte[] lRenderedBytes = this.GerarRelatorio(
                    Server.MapPath(@"..\Reports\RendaFixa.rdlc"),
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
                base.ExibirMensagemJsOnLoad("E", "Erro ao criar pdf de Renda Fixa.");
            }
        }

        protected void btnImprimirExcel_Click(object sender, EventArgs e)
        {
            try
            {
                this.CarregarExcel();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao criar excell de Renda Fixa.");
            }
        }

        protected void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {
                string lNomeDoArquivo = string.Format("RendaVariavel_{0}_{1}", base.SessaoClienteLogado.CodigoPrincipal, DateTime.Now.ToString("yyyyMMddHHmmss"));

                string lMimeType;

                byte[] lRenderedBytes = this.GerarRelatorio(
                    Server.MapPath(@"..\Reports\RendaFixa.rdlc"),
                    lNomeDoArquivo,
                    out lMimeType);

                var lAnexos = new List<OMS.Email.Lib.EmailAnexoInfo>();
                EmailAnexoInfo lEmailInfo = new EmailAnexoInfo();
                lEmailInfo.Arquivo = lRenderedBytes;
                lEmailInfo.Nome = string.Concat(lNomeDoArquivo, ".pdf");
                lAnexos.Add(lEmailInfo);

                Dictionary<string, string> lVariaveis = new Dictionary<string, string>();
                //base.EnviarEmail("bribeiro@gradualinvestimentos.com.br", "Extrato - Gradual Investimentos", "Extrato.htm", lVariaveis, Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, lAnexos, null);
                base.EnviarEmail(base.SessaoClienteLogado.Email, "Renda Fixa - Gradual Investimentos", "RendaFixa.htm", lVariaveis, Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, lAnexos, null);

                base.ExibirMensagemJsOnLoad("I", "Um E-mail com o arquivo Pdf foi enviado para " + base.SessaoClienteLogado.Email);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao Enviar email com pdf de Renda Fixa.");
            }
        }
        #endregion

        #region Métodos Fundos e Clubes e tesouro direto

        private void CarregarGrids()
        {
            List<RendaFixaInfo> ListaRendaFixa = this.CarregarRelatorioRendaFixa();

            List<TituloMercadoInfo> ListaTD = this.CarregarTesouroDireto();

            List<FundoInfo> ListaFundo = this.CarregarRelatorioFundos();

            List<ClubeInfo> ListaClube = this.CarregarRelatorioClubes();

            List<PreencheGraficoPosicaoVariavelAux> lListaPosicao = new List<PreencheGraficoPosicaoVariavelAux>();

            string[] CoresPreferencias = { "#524646", "#B4837A", "#D1D2D3", "#EE9620", "#F6BC15" };

            int lCount = 0;

            var lPosicaoGrafico = PreencheListaPosicaoGrafico(ListaFundo, ListaClube, ListaTD, ListaRendaFixa);

            var lPosicaoGraficoOrdenado = lPosicaoGrafico.OrderByDescending(posicao => posicao.Value).ToList();

            var lRandom = new Random();

            foreach (KeyValuePair<string, decimal> kvp in lPosicaoGraficoOrdenado)
            {
                var lCust = new PreencheGraficoPosicaoVariavelAux();

                lCust.Color = (lCount <= 4) ? CoresPreferencias[lCount] : String.Format("#{0:X6}", lRandom.Next(0x1000000));
                lCust.Value = kvp.Value;
                lCust.Key = kvp.Key;

                lListaPosicao.Add(lCust);
                lCount++;
            }

            this.rptGrafico_RendaFixa.DataSource = lListaPosicao;
            this.rptGrafico_RendaFixa.DataBind();

            this.trNenhumItemRenda.Visible = ListaRendaFixa.Count == 0;
            this.trTotalRendaFixa.Visible = ListaRendaFixa.Count != 0;

            //this.trNehumFundos.Visible = ListaFundo.Count == 0;
            //this.trNenhumClube.Visible = ListaClube.Count == 0;

        }
        
        private List<TituloMercadoInfo> CarregarTesouroDireto()
        {
            var lRetorno = new List<TituloMercadoInfo>();
            try
            {
                ConsultasConsultaExtratoMensalRequest lRequest = new ConsultasConsultaExtratoMensalRequest();
                ConsultasConsultaExtratoMensalResponse lServicoTesouro;

                lRequest.CPFNegociador = this.CPFdoCliente;

                lServicoTesouro = this.ServicoTesouro.ConsultarExtratoMensal(lRequest);

                if (!MostraWSErro(lServicoTesouro))
                    return new List<TituloMercadoInfo>();

                List<TituloMercadoInfo> lParcial = lServicoTesouro.Titulos;

                if (lParcial != null && lParcial.Count > 0)
                {
                    lRetorno = lParcial;
                }
            }
            catch (System.Exception ex)
            {
                base.ExibirMensagemJsOnLoad("E", "Erro ao tentar consultas extrato.");

                gLogger.Error("[Erro ao tentar consultas extrato.]", ex);
            }

            return lRetorno;
        }

        private List<RendaFixaInfo> CarregarRelatorioRendaFixa()
        {
            var lRetorno = new List<RendaFixaInfo>();

            decimal lTotalQuantidade     = 0;
            decimal lTotalValorOriginal  = 0;
            decimal lTotalSaldoBruto     = 0;
            decimal lTotalIRRF           = 0;
            decimal lTotalIOF            = 0;
            decimal lTotalSaldoLiquido   = 0;

            try
            {
                var lRequest = new RendaFixaInfo();

                lRequest.CodigoCliente = this.SessaoClienteLogado.CodigoPrincipal.DBToInt32();

                var lListaRenda = base.ConsultarRendaFixa(lRequest);

                lListaRenda.ForEach(renda => 
                {
                    lTotalQuantidade   += renda.Quantidade;
                    lTotalValorOriginal+= renda.ValorOriginal;
                    lTotalSaldoBruto   += renda.SaldoBruto;
                    lTotalIRRF         += renda.IRRF;
                    lTotalIOF          += renda.IOF;
                    lTotalSaldoLiquido += renda.SaldoLiquido;
                    
                });

                lRetorno.AddRange(lListaRenda);

                this.rptRendaFixa.DataSource = lRetorno;
                this.rptRendaFixa.DataBind();

                if (lRetorno.Count > 0)
                {
                    this.lblTotalQuantidade.Text    = lTotalQuantidade    .ToString("N2") ;
                    this.lblTotalValorOriginal.Text = lTotalValorOriginal .ToString("N2") ;
                    this.lblTotalSaldoBruto.Text    = lTotalSaldoBruto    .ToString("N2") ;
                    this.lblTotalIRRF.Text          = lTotalIRRF          .ToString("N2") ;
                    this.lblTotalIOF.Text           = lTotalIOF           .ToString("N2") ;
                    this.lblTotalSaldoLiquido.Text  = lTotalSaldoLiquido  .ToString("N2") ;
                }

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em RendaFixa.aspx > CarregarRelatorioRendaFixa(pCBLC: [{0}]) [{1}]\r\n{2}", SessaoClienteLogado.CpfCnpj, ex.Message, ex.StackTrace);

                throw ex;
            }

            return lRetorno;
        }

        private List<FundoInfo> CarregarRelatorioFundos()
        {
            var lRetorno = new List<FundoInfo>();

            try
            {
                var lBaseFundos = new PaginaFundos();

                var lListaFundos = lBaseFundos.PosicaoFundosSumarizada();

                //this.rptPosicaoFundo.DataSource = lListaFundos;
                //this.rptPosicaoFundo.DataBind();

                //this.trNehumFundos.Visible = lListaFundos.Count() == 0;

                foreach (var lFundo in lListaFundos)
                {
                    FundoInfo lInfo = new FundoInfo();

                    lInfo.CodigoFundo     = lFundo.IdFundo.DBToInt32();
                    lInfo.Cota            = lFundo.ValorCota.DBToDecimal();
                    lInfo.DataAtualizacao = lFundo.DataAtualizacao.DBToDateTime();
                    lInfo.IOF             = lFundo.IOF.DBToDecimal();
                    lInfo.IR              = lFundo.IR.DBToDecimal();
                    lInfo.NomeFundo       = lFundo.NomeFundo;
                    lInfo.ValorBruto      = lFundo.ValorBruto.DBToDecimal();
                    lInfo.ValorLiquido    = lFundo.ValorLiquido.DBToDecimal();
                    lInfo.Quantidade      = lFundo.QtdCotas.DBToDecimal();

                    lRetorno.Add(lInfo);
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em RendaFixa.aspx > CarregarRelatorio(pCBLC: [{0}]) [{1}]\r\n{2}", SessaoClienteLogado.CpfCnpj, ex.Message, ex.StackTrace);

                throw ex;
            }

            return lRetorno;
        }

        private List<ClubeInfo> CarregarRelatorioClubes()
        {
            var lRetorno = new List<ClubeInfo>();
            try
            {
                DateTime lDiaAnterior = base.ServicoPersistenciaSite.SelecionaUltimoDiaUtil();

                ClubeRequest lRequest = new ClubeRequest();
                ClubeResponse lResponse;

                lRequest.Clube = new DbLib.Dados.MinhaConta.ClubeInfo();

                lRequest.Clube.IdCliente = base.SessaoClienteLogado.CodigoPrincipal.DBToInt32();

                //lRequest.Clube.IdClube = Convert.ToInt32(this.cboClube.SelectedValue);

                lRequest.Clube.DataPosicao = lDiaAnterior;

                lResponse = base.ServicoPersistenciaSite.SelecionarPosicaoClube(lRequest);

                if (lResponse.ListaClube.Count > 0)
                {
                    lRetorno = lResponse.ListaClube;
                    rptPosicaoClubes.DataSource = lResponse.ListaClube;
                    rptPosicaoClubes.DataBind();
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em RendaFixa.aspx > CarregarRelatorio(pCBLC: [{0}]) [{1}]\r\n{2}", base.SessaoClienteLogado.CodigoPrincipal, ex.Message, ex.StackTrace);

                base.ExibirMensagemJsOnLoad("I", gMensagemInstabilidade);
            }

            return lRetorno;
        }

        private void CarregarExcelClubes()
        {
            try
            {
                StringBuilder lBuilder = new StringBuilder();

                DateTime lDiaAnterior = base.ServicoPersistenciaSite.SelecionaUltimoDiaUtil();

                lBuilder.AppendLine("Nome do Clube;Cota;Quantidade;Valor Bruto (R$);IR%;IOF%;Valor Líquido (R$)");

                ClubeRequest lRequest = new ClubeRequest();
                ClubeResponse lResponse;

                lRequest.Clube = new DbLib.Dados.MinhaConta.ClubeInfo();

                lRequest.Clube.IdCliente = base.SessaoClienteLogado.CodigoPrincipal.DBToInt32();

                //lRequest.Clube.IdClube = Convert.ToInt32(this.cboClube.SelectedValue);

                lRequest.Clube.DataPosicao = lDiaAnterior;

                lResponse = base.ServicoPersistenciaSite.SelecionarPosicaoClube(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    foreach (ClubeInfo item in lResponse.ListaClube)
                    {
                        lBuilder.AppendFormat("{0};{1};{2};{3};{4};{5};{6}\r\n"
                                                    , item.NomeClube
                                                    , item.Cota
                                                    , item.Quantidade
                                                    , item.ValorBruto
                                                    , item.IR
                                                    , item.IOF
                                                    , item.ValorLiquido
                                                    );
                    }
                }
                else
                {
                    gLogger.ErrorFormat("Resposta com erro do ServicoPersistenciaSite.SelecionarPosicaoClube(IdCliente: [{0}], IdClube: [{1}], DataPosicao: [{2}]) em PosicaoClubes.aspx > CarregarExcel > [{3}]\r\n{4}"
                                        , lRequest.Clube.IdCliente
                                        , lRequest.Clube.IdClube
                                        , lRequest.Clube.DataPosicao
                                        , lResponse.StatusResposta
                                        , lResponse.DescricaoResposta);
                }

                Response.Clear();

                Response.ContentType = "text/csv";

                Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

                Response.Charset = "iso-8859-1";

                Response.AddHeader("content-disposition", "attachment;filename=RelatorioDeNotasDeCorretagem.csv");

                Response.Write(lBuilder.ToString());

                Response.End();
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em PosicaoClubes.aspx > CarregarExcel > [{0}]\r\n{1}"
                                    , ex.Message
                                    , ex.StackTrace);

                base.ExibirMensagemJsOnLoad("E", "Erro ao gerar o relatório Posição Clube.");
            }
        }

        public void CarregarExcel()
        {
            try
            {
                StringBuilder lBuilder = new StringBuilder();

                var lFundos = CarregarRelatorioFundos();

                if (lFundos.Count > 0 )  lBuilder.AppendLine("Nome do Fundo;Cota;Quantidade;Valor Bruto (R$);IR%;IOF%;Valor Líquido (R$)");

                foreach (FundoInfo item in lFundos)
                {
                    lBuilder.AppendFormat("{0};{1};{2};{3};{4};{5};{6}\r\n"
                                                , item.NomeFundo
                                                , item.Cota
                                                , item.Quantidade
                                                , item.ValorBruto
                                                , item.IR
                                                , item.IOF
                                                , item.ValorLiquido
                                                );
                }

                var lClubes = CarregarRelatorioClubes();

                if (lClubes.Count > 0) lBuilder.AppendLine("Nome do Clube;Cota;Quantidade;Valor Bruto (R$);IR%;IOF%;Valor Líquido (R$)");

                foreach (ClubeInfo item in lClubes)
                {
                    lBuilder.AppendFormat("{0};{1};{2};{3};{4};{5};{6}\r\n"
                                                , item.NomeClube
                                                , item.Cota
                                                , item.Quantidade
                                                , item.ValorBruto
                                                , item.IR
                                                , item.IOF
                                                , item.ValorLiquido
                                                );
                }

                var lTesouro = CarregarTesouroDireto();

                List<Transporte_TesouroDireto_Extrato> lTesouros = new Transporte_TesouroDireto_Extrato().TraduzirLista(lTesouro);

                if (lTesouros.Count > 0) lBuilder.AppendLine("Titulo;Vencimento;Crédito;Débito;Qtd. Bloqueada; Saldo Anterior; Saldo atual; Título Valor; Valor Atual;Tx Dev. até a Data");

                foreach (Transporte_TesouroDireto_Extrato item in lTesouros)
                {
                    lBuilder.AppendFormat("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}\r\n"
                                                , item.TituloNome
                                                , item.DataVencimento
                                                , item.QuantidadeCredito
                                                , item.QuantidadeDebito
                                                , item.QuantidadeBloqueada
                                                , item.SaldoAnterior
                                                , item.SaldoAtual
                                                , item.TituloValor
                                                , item.ValorAtual
                                                , item.ValorTaxaDevida
                                                );
                }

                var lRendaFixa = CarregarRelatorioRendaFixa();

                if (lRendaFixa.Count > 0) lBuilder.AppendLine("Titulo;Aplicação;Vencimento;Taxa;QUantidade;Valor Original;Saldo Bruto; IRRF; IOF;Saldo Líquido");

                foreach (RendaFixaInfo item in lRendaFixa)
                {
                    lBuilder.AppendFormat("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}\r\n"
                                                , item.Titulo
                                                , item.Aplicacao  .ToString("dd/MM/yyyy")
                                                , item.Vencimento .ToString("dd/MM/yyyy")
                                                , item.Taxa       .ToString("N4") + "%"
                                                , item.Quantidade .ToString("N8")
                                                , item.ValorOriginal.ToString("N2")
                                                , item.SaldoBruto   .ToString("N2")
                                                , item.IRRF         .ToString("N2")
                                                , item.IOF          .ToString("N2")
                                                , item.SaldoLiquido .ToString("N2"));
                }

                Response.Clear();

                Response.ContentType = "text/csv";

                Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

                Response.Charset = "iso-8859-1";

                Response.AddHeader("content-disposition", "attachment;filename=RendaFixa.csv");

                Response.Write(lBuilder.ToString());

                Response.End();
            }
            catch (Exception ex)
            {
                gLogger.Error("[Erro relatório o Excel para o relatório Renda Fixa]", ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao gerar relatório de Renda Fixa.");

            }
        }

        private byte[] GerarRelatorio(
            string pCaminhoRelatorio,
            string pCaminhoDoArquivo,
            out string pMymeType)
        {
            string lNome = base.SessaoClienteLogado.Nome;

            LocalReport lReport = new LocalReport();

            lReport.ReportPath = pCaminhoRelatorio;

            List<TituloMercadoInfo> lTesouro = CarregarTesouroDireto();

            List<Transporte_TesouroDireto_Extrato> lListaTD = new Transporte_TesouroDireto_Extrato().TraduzirLista(lTesouro);

            List<FundoInfo> lFundos = CarregarRelatorioFundos();

            List<ClubeInfo> lClubes = CarregarRelatorioClubes();

            List<RendaFixaInfo> lRendaFixa = CarregarRelatorioRendaFixa();

            List<ReportParameter> lParametros = new List<ReportParameter>();

            ReportParameter lParamData = new ReportParameter("pData", DateTime.Now.ToString("dd/MM/yyyy"));
            lParametros.Add(lParamData);

            ReportParameter lParamCliente = new ReportParameter("pCliente", base.SessaoClienteLogado.CodigoPrincipal + "-" + base.SessaoClienteLogado.Nome);
            lParametros.Add(lParamCliente);

            ReportParameter lParamCpfCnpjCliente = new ReportParameter("pCpfCnpj", base.SessaoClienteLogado.CpfCnpj);
            lParametros.Add(lParamCpfCnpjCliente);

            ReportDataSource lSourceFundos = new ReportDataSource("EFundos", lFundos);

            ReportDataSource lSourceTesouro = new ReportDataSource("ETesouro", lListaTD);

            ReportDataSource lSourceClubes = new ReportDataSource("EClubes", lClubes);

            ReportDataSource lSourceRendaFixa = new ReportDataSource("ERendaFixa", lRendaFixa);

            lReport.DataSources.Add(lSourceFundos);

            lReport.DataSources.Add(lSourceTesouro);

            lReport.DataSources.Add(lSourceClubes);

            lReport.DataSources.Add(lSourceRendaFixa);

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
        #endregion

        #region Métodos
        private Boolean MostraWSErro(MensagemResponseBase pResposta)
        {
            if (pResposta.StatusResposta != MensagemResponseStatusEnum.OK)
            {
                string lMensagemErro = pResposta.DescricaoResposta;

                if (pResposta.DescricaoResposta.Contains("-2147220910")) { lMensagemErro = "Dados informados inválidos."; }
                else if (pResposta.DescricaoResposta.Contains("-2147217900")) { lMensagemErro = "Erro ao acessar o Banco de Dados."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220898")) { lMensagemErro = "Erro ao indentificar o cliente no sistema."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220187")) { lMensagemErro = "Mercado inválido."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220888")) { lMensagemErro = "Título inexistente no mercado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220865")) { lMensagemErro = "A(s) quantidade(s) deve(m) ser múltipla(s) de 0,2."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220912")) { lMensagemErro = "Erro ao validar os dados do cliente."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220902")) { lMensagemErro = "Valor inferior ao limite mínimo de compra."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220170")) { lMensagemErro = "Existem titulos que não fazem parte da cesta."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220188")) { lMensagemErro = "Cesta Inválida."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220166")) { lMensagemErro = "Item inexistente na cesta."; }
                else if (pResposta.DescricaoResposta.Contains("-2147217873")) { lMensagemErro = "Você não pode excluir o(s) registro(s) porque o(s) mesmo(s) faz(em) parte de outra(s) tabela(s)"; }
                else if (pResposta.DescricaoResposta.Contains("-2147220173")) { lMensagemErro = "Já existe uma cesta para este negociador."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220174")) { lMensagemErro = "A Cesta não pode ser alterada pois já foi fechada."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220175")) { lMensagemErro = "O CPF é diferente do CPF da cesta."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220851")) { lMensagemErro = "Saldo Bloqueado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220852")) { lMensagemErro = "Título Bloqueado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220889")) { lMensagemErro = "Saldo Insuficiente para venda."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220890")) { lMensagemErro = "A Quantidade de venda é maior que a Quantidade Disponivel no Mercado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220891")) { lMensagemErro = "O Limite mensal de vendas foi ultrapassado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220896")) { lMensagemErro = "Mercado Fechado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220897")) { lMensagemErro = "Mercado Suspenso."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220900")) { lMensagemErro = "A Quantidade a comprar e maior que a Quantidade Disponivel."; }
                else if (pResposta.DescricaoResposta.Contains("-2147217833")) { lMensagemErro = "Um erro ocorreu. Contate o administrador."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220899")) { lMensagemErro = "Investidor Suspenso."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220901")) { lMensagemErro = "O Limite mensal foi ultrapassado."; }
                else { lMensagemErro = "Mercado Suspenso."; }

                base.ExibirMensagemJsOnLoad("E", lMensagemErro);

                return false;
            }

            return true;
        }

        public struct PreencheGraficoPosicaoVariavelAux
        {
            public string Key { get; set; }
            public string Color { get; set; }
            public decimal Value { get; set; }
        }

        public struct TransportePapelAux
        {
            public string Papel { get; set; }
            public decimal Quant { get; set; }
            public decimal Preco { get; set; }
        }

        private Dictionary<string, decimal> PreencheListaPosicaoGrafico(List<FundoInfo> lListaFundo, List<ClubeInfo> lListaClube, List<TituloMercadoInfo> lListaTD, List<RendaFixaInfo> lListaRendaFixa)
        {
            CultureInfo lBR = new CultureInfo("pt-BR");

            var lPosicao = new List<TransportePapelAux>();

            var lAgrupadoPorPapel = new Dictionary<string, decimal>();

            var lPrecosPorPapel = new Dictionary<string, decimal>();

            var lRetorno = new Dictionary<string, decimal>();

            decimal lValorTotal = 0.0M;
            decimal lPrecoPapel;

            //lListaFundo.ForEach(fundo =>
            //{
            //    var lTrans = new TransportePapelAux();
                
            //    lTrans.Papel = "Fundos";
            //    lTrans.Preco += Convert.ToDecimal(fundo.Cota , lBR);
            //    lTrans.Quant = fundo.Quantidade.Value;

            //    lPosicao.Add(lTrans);
            //});

            lListaClube.ForEach(clube =>
            {
                var lTrans = new TransportePapelAux();

                lTrans.Papel ="Clubes";
                lTrans.Preco += Convert.ToDecimal(clube.Cota, lBR);
                lTrans.Quant = int.Parse(clube.Quantidade.ToString());

                lPosicao.Add(lTrans);
            });

            lListaTD.ForEach(TD =>
            {
                var lTrans = new TransportePapelAux();

                decimal lSaldoAtual = decimal.Parse( (TD.SaldoAnterior.DBToDouble() - TD.QuantidadeDebito.DBToDouble() + TD.QuantidadeCredito).ToString("N2"));
                decimal lValorAtual = TD.ValorBase;
                if (lSaldoAtual != 0)
                {
                    lTrans.Papel = "Tesouro";
                    lTrans.Preco += Convert.ToDecimal(lValorAtual , lBR);
                    lTrans.Quant = lSaldoAtual;

                    lPosicao.Add(lTrans);
                }
            });

            lListaRendaFixa.ForEach(renda => 
            {
                var lTrans = new TransportePapelAux();

                lTrans.Papel = "Renda";
                lTrans.Preco += Convert.ToDecimal(renda.SaldoLiquido, lBR);
                lTrans.Quant = 1;

                lPosicao.Add(lTrans);

            });

            foreach (var lPos in lPosicao)
            {
                if (!lPrecosPorPapel.ContainsKey(lPos.Papel)) lPrecosPorPapel.Add(lPos.Papel, 0);

                lPrecosPorPapel[lPos.Papel] = lPos.Preco;
            }

            foreach (var lPos in lPosicao)
            {
                if (lPos.Quant > 0)
                {
                    string lPapel = lPos.Papel;

                    if (lPrecosPorPapel[lPapel] > 0)
                    {
                        if (!lAgrupadoPorPapel.ContainsKey(lPapel)) lAgrupadoPorPapel.Add(lPapel, 0);

                        lPrecoPapel = lPos.Preco * lPos.Quant; //lPrecosPorPapel[lPapel];// *lPos.Quant;

                        lAgrupadoPorPapel[lPapel] += lPrecoPapel;

                        lValorTotal += lPrecoPapel;
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

            this.lblComposicaoRendaFixa.Text = lValorTotal.ToString("N2");

            return lRetorno;
        }
        #endregion

    }
}