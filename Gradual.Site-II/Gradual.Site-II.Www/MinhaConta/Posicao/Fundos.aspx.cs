using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Gradual.Site.DbLib.Dados.MinhaConta;
using System.Text;
using Microsoft.Reporting.WebForms;
using Gradual.OMS.Email.Lib;

using System.Globalization;
using Gradual.Site.Www.Transporte;

namespace Gradual.Site.Www.MinhaConta.Posicao
{
    public partial class Fundos : PaginaFundos
    {
        protected void Page_Load(object sender, EventArgs e)
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

                    //this.CarregarRelatorioFundos();

                }
                else
                {
                    if (this.Posicao_Aba_Simular_Selecionada.Value == "AbaRelatorios")
                    {
                        base.RodarJavascriptOnLoad("MinhaConta_Operacoes_Fundos_Relatorios_LoadAccordeon_Click('" + this.Posicao_Aba_Simular_Selecionada.Value + "');");
                        //base.RodarJavascriptOnLoad("MinhaConta_GerarGrafico_Fundos_ExtratoMensal();");
                    }
                }
                string lScript = "MinhaConta_GerarGrafico_Fundos();MinhaConta_GerarGrafico_Fundos_ExtratoMensal()";

                RodarJavascriptOnLoad(lScript);
            }
        }

        private List<FundoInfo> CarregarRelatorioFundos()
        {
            var lRetorno = new List<FundoInfo>();

            try
            {
                var lBaseFundos = new PaginaFundos();

                var lListaFundos = lBaseFundos.PosicaoFundosSumarizada();

                this.rptPosicaoFundo.DataSource = lListaFundos;
                this.rptPosicaoFundo.DataBind();

                this.trNehumFundos.Visible = lListaFundos.Count() == 0;

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
                gLogger.ErrorFormat("Erro em PosicaoFundos.aspx > CarregarRelatorio(pCBLC: [{0}]) [{1}]\r\n{2}", SessaoClienteLogado.CpfCnpj, ex.Message, ex.StackTrace);

                throw ex;
            }

            return lRetorno;
        }

        #region Eventos
        protected void btnImprimirPDF_Click(object sender, EventArgs e)
        {
            try
            {
                string lNomeDoArquivo = string.Format("Fundos_{0}_{1}", base.SessaoClienteLogado.CodigoPrincipal, DateTime.Now.ToString("yyyyMMddHHmmss"));

                string lMimeType;

                byte[] lRenderedBytes = this.GerarRelatorio(
                    Server.MapPath(@"..\Reports\Fundos.rdlc"),
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
                base.ExibirMensagemJsOnLoad("E", "Erro ao criar pdf de Fundos.");
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
                base.ExibirMensagemJsOnLoad("E", "Erro ao criar excell de Fundos.");
            }
        }

        protected void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {
                string lNomeDoArquivo = string.Format("Fundos_{0}_{1}", base.SessaoClienteLogado.CodigoPrincipal, DateTime.Now.ToString("yyyyMMddHHmmss"));

                string lMimeType;

                byte[] lRenderedBytes = this.GerarRelatorio(
                    Server.MapPath(@"..\Reports\Fundos.rdlc"),
                    lNomeDoArquivo,
                    out lMimeType);

                var lAnexos = new List<OMS.Email.Lib.EmailAnexoInfo>();
                EmailAnexoInfo lEmailInfo = new EmailAnexoInfo();
                lEmailInfo.Arquivo = lRenderedBytes;
                lEmailInfo.Nome = string.Concat(lNomeDoArquivo, ".pdf");
                lAnexos.Add(lEmailInfo);

                Dictionary<string, string> lVariaveis = new Dictionary<string, string>();
                //base.EnviarEmail("bribeiro@gradualinvestimentos.com.br", "Extrato - Gradual Investimentos", "Extrato.htm", lVariaveis, Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, lAnexos, null);
                base.EnviarEmail(base.SessaoClienteLogado.Email, "Fundos - Gradual Investimentos", "RendaFixa.htm", lVariaveis, Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, lAnexos, null);

                base.ExibirMensagemJsOnLoad("I", "Um E-mail com o arquivo Pdf foi enviado para " + base.SessaoClienteLogado.Email);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao Enviar email com pdf de Renda Fixa.");
            }
        }

        protected void rptPosicaoFundo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ((Button)(((Control)(e.Item)).Controls[1])).CommandArgument = ((Transporte_PosicaoCotista)(e.Item.DataItem)).IdFundo;
        }

        protected void btnResgatar_Click(object sender, EventArgs e)
        {
            try
            {
                var lBase = (PaginaBase)this.Page;

                string idFundo = ((Button)(sender)).CommandArgument;

                Response.Redirect(lBase.HostERaizFormat("MinhaConta/Produtos/Fundos/resgate.aspx?idFundo=" + idFundo));
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Metodos
        public void CarregarExcel()
        {
            try
            {
                StringBuilder lBuilder = new StringBuilder();

                var lFundos = CarregarRelatorioFundos();

                if (lFundos.Count > 0) lBuilder.AppendLine("Nome do Fundo;Cota;Quantidade;Valor Bruto (R$);IR%;IOF%;Valor Líquido (R$)");

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

                Response.Clear();

                Response.ContentType = "text/csv";

                Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

                Response.Charset = "iso-8859-1";

                Response.AddHeader("content-disposition", "attachment;filename=PosicaoFundos_"+ DateTime.Now.ToString("yyyyMMdd") +".csv");

                Response.Write(lBuilder.ToString());

                Response.End();
            }
            catch (Exception ex)
            {
                gLogger.Error("[Erro relatório o Excel para o relatório de Posição de Fundos]", ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao gerar relatório de Fundos.");

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

            //List<TituloMercadoInfo> lTesouro = CarregarTesouroDireto();

            //List<Transporte_TesouroDireto_Extrato> lListaTD = new Transporte_TesouroDireto_Extrato().TraduzirLista(lTesouro);

            List<FundoInfo> lFundos = CarregarRelatorioFundos();

            //List<ClubeInfo> lClubes = CarregarRelatorioClubes();

            List<ReportParameter> lParametros = new List<ReportParameter>();

            ReportParameter lParamData = new ReportParameter("pData", DateTime.Now.ToString("dd/MM/yyyy"));
            lParametros.Add(lParamData);

            ReportParameter lParamCliente = new ReportParameter("pCliente", base.SessaoClienteLogado.CodigoPrincipal + "-" + base.SessaoClienteLogado.Nome);
            lParametros.Add(lParamCliente);

            ReportParameter lParamCpfCnpjCliente = new ReportParameter("pCpfCnpj", base.SessaoClienteLogado.CpfCnpj);
            lParametros.Add(lParamCpfCnpjCliente);

            ReportDataSource lSourceFundos = new ReportDataSource("EFundos", lFundos);

            //ReportDataSource lSourceTesouro = new ReportDataSource("ETesouro", lListaTD);

            //ReportDataSource lSourceClubes = new ReportDataSource("EClubes", lClubes);

            lReport.DataSources.Add(lSourceFundos);

            //lReport.DataSources.Add(lSourceTesouro);

            //lReport.DataSources.Add(lSourceClubes);

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
        public struct TransportePapelAux
        {
            public string Papel { get; set; }
            public decimal Quant { get; set; }
            public decimal Preco { get; set; }
        }
        public struct PreencheGraficoPosicaoVariavelAux
        {
            public string Key { get; set; }
            public string Color { get; set; }
            public decimal Value { get; set; }
        }
        private Dictionary<string, decimal> PreencheListaPosicaoGrafico(List<FundoInfo> lListaFundo)
        {
            CultureInfo lBR = new CultureInfo("pt-BR");

            var lPosicao = new List<TransportePapelAux>();

            var lAgrupadoPorPapel = new Dictionary<string, decimal>();

            var lPrecosPorPapel = new Dictionary<string, decimal>();

            var lRetorno = new Dictionary<string, decimal>();

            decimal lValorTotal = 0.0M;
            decimal lPrecoPapel;

            lListaFundo.ForEach(fundo =>
            {
                var lTrans = new TransportePapelAux();

                lTrans.Papel = fundo.NomeFundo;
                lTrans.Preco += Convert.ToDecimal(fundo.Cota, lBR);
                lTrans.Quant = fundo.Quantidade.Value;

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

            this.lblComposicaoFundos.Text = lValorTotal.ToString("N2");

            return lRetorno;
        }

        private void CarregarGrids()
        {
            List<FundoInfo> ListaFundo = this.CarregarRelatorioFundos();

            List<PreencheGraficoPosicaoVariavelAux> lListaPosicao = new List<PreencheGraficoPosicaoVariavelAux>();

            string[] CoresPreferencias = { "#524646", "#B4837A", "#D1D2D3", "#EE9620", "#F6BC15" };

            int lCount = 0;

            var lPosicaoGrafico = PreencheListaPosicaoGrafico(ListaFundo);

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

            this.rptGrafico_Fundos.DataSource = lListaPosicao;
            this.rptGrafico_Fundos.DataBind();

            //this.trNehumFundos.Visible = ListaFundo.Count == 0;
            //this.trNenhumClube.Visible = ListaClube.Count == 0;

        }
        #endregion
    }
}
