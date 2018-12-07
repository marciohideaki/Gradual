using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.ContaCorrente.Lib.Info;
using Gradual.OMS.ContaCorrente.Lib;
using System.Text;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using System.Globalization;
using Microsoft.Reporting.WebForms;
using Gradual.OMS.Email.Lib;

namespace Gradual.Site.Www.MinhaConta.Financeiro
{
    public partial class Extrato : PaginaBase
    {
        #region Propriedades

        public DateTime DataInicial
        {
            get
            {
                DateTime lData = DateTime.MinValue;

                DateTime.TryParseExact(txtDataInicial.Text, "dd/MM/yyyy", gCultureInfoBR, DateTimeStyles.None, out lData);

                return lData;
            }
        }

        public DateTime DataFinal
        {
            get
            {
                DateTime lData = DateTime.MaxValue;

                DateTime.TryParseExact(txtDataFinal.Text, "dd/MM/yyyy", gCultureInfoBR, DateTimeStyles.None, out lData);

                return lData;
            }
        }

        public string Descricao { get; set; }

        public string cssSaldoAnterior      { get; set; }
        public string cssSaldoDisponivel    { get; set; }
        public string cssSaldoTotal         { get; set; }
        #endregion

        #region Métodos Private
        
        private void BuscarRelatorio()
        {
            ContaCorrenteExtratoInfo lExtrato;

            try
            {
                lExtrato = FazerBuscaRelatorio();

                if (lExtrato != null && lExtrato.ListaContaCorrenteMovimento != null && lExtrato.ListaContaCorrenteMovimento.Count > 0)
                {
                    //this.divRelatorio.Visible = true;

                    //Carrega os Saldos
                    this.lblSaldoAnterior.Text   = lExtrato.SaldoAnterior.ToString("N");
                    this.lblSaldoDisponivel.Text = lExtrato.SaldoDisponivel.ToString("N");
                    this.lblSaldoTotal.Text      = lExtrato.SaldoTotal.ToString("N");

                    this.cssSaldoAnterior   = DefinirCorDoValor(lExtrato.SaldoAnterior);
                    this.cssSaldoDisponivel = DefinirCorDoValor(lExtrato.SaldoDisponivel);
                    this.cssSaldoTotal      = DefinirCorDoValor(lExtrato.SaldoTotal);

                    if (lExtrato.ListaContaCorrenteMovimento.Count > 0)
                    {
                        trNenhumItem.Visible = false;

                        rptExtrato.DataSource = lExtrato.ListaContaCorrenteMovimento;
                        rptExtrato.DataBind();
                    }
                    else
                    {
                        trNenhumItem.Visible = true;
                    }

                }
                else
                {
                    //this.divRelatorio.Visible = false;

                    trNenhumItem.Visible = true;

                    base.ExibirMensagemJsOnLoad("I", "Não há extratos a serem exibidos.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ContaCorrenteExtratoInfo FazerBuscaRelatorio()
        {
            ContaCorrenteExtratoInfo lRetorno = null;

            IServicoExtratos lServico = InstanciarServicoDoAtivador<IServicoExtratos>();

            ContaCorrenteExtratoRequest lRequest = new ContaCorrenteExtratoRequest();
            ContaCorrenteExtratoResponse lResponse;

            DateTime lDataInicial, lDataFinal;

            string lSufixo = "";

            this.Descricao = "Extrato de " + cboTipo.SelectedItem.Text;

            if (!string.IsNullOrEmpty(cboPeriodo.SelectedValue))
            {
                int lDias = Convert.ToInt32(cboPeriodo.SelectedValue) * -1;

                lDataFinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                lDataInicial = lDataFinal.AddDays(lDias);

                lSufixo = " (" + cboPeriodo.SelectedItem.Text + ")";
            }
            else
            {
                lDataInicial = this.DataInicial;
                lDataFinal = this.DataFinal;
            }

            this.Descricao += " de " + lDataInicial.ToString("dd/MM/yyyy") + " até " + lDataFinal.ToString("dd/MM/yyyy") + lSufixo;

            lRequest.ConsultaCodigoCliente = base.SessaoClienteLogado.CodigoPrincipal.DBToInt32();
            lRequest.ConsultaDataInicio = lDataInicial;
            lRequest.ConsultaDataFim = lDataFinal;
            lRequest.ConsultaNomeCliente = this.SessaoClienteLogado.Nome;
            lRequest.ConsultaTipoExtratoDeConta = (OMS.ContaCorrente.Lib.Info.Enum.EnumTipoExtradoDeConta)Convert.ToInt32(cboTipo.SelectedValue);

            //lRequest.ConsultaTipoExtratoDeConta = OMS.ContaCorrente.Lib.Info.Enum.EnumTipoExtradoDeConta.Liquidacao;

            lResponse = lServico.ConsultarExtratoContaCorrente(lRequest);

            if (lResponse.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
            {
                //lRetorno = new TransporteRelatorioExtratoContaCorrente( lResponse.Relatorio );

                lRetorno = lResponse.Relatorio;
            }
            else
            {
                throw new Exception(string.Format("{0}-{1}", lResponse.StatusResposta, lResponse.StackTrace));
            }

            return lRetorno;
        }

        private byte[] GerarRelatorio(
            string pCaminhoRelatorio,
            string pCaminhoDoArquivo,
            out string pMymeType)
        {
            string lNome = base.SessaoClienteLogado.Nome;

            LocalReport lReport = new LocalReport();

            lReport.ReportPath = pCaminhoRelatorio;

            ContaCorrenteExtratoInfo lExtrato =  FazerBuscaRelatorio();

            List<ReportParameter> lParametros = new List<ReportParameter>();

            ReportParameter lParamData = new ReportParameter("pData", DateTime.Now.ToString("dd/MM/yyyy"));
            lParametros.Add(lParamData);

            ReportParameter lParamCliente = new ReportParameter("pCliente", base.SessaoClienteLogado.CodigoPrincipal + "-" + base.SessaoClienteLogado.Nome);
            lParametros.Add(lParamCliente);

            ReportParameter lParamCpfCnpjCliente = new ReportParameter("pCpfCnpj", base.SessaoClienteLogado.CpfCnpj);
            lParametros.Add(lParamCpfCnpjCliente);

            ReportParameter lParamSaldoAnterior = new ReportParameter("pSaldoAnterior", lExtrato.SaldoAnterior.ToString("N2"));
            lParametros.Add(lParamSaldoAnterior);

            ReportParameter lParamSaldoDisponivel = new ReportParameter("pSaldoDisponivel", lExtrato.SaldoDisponivel.ToString("N2"));
            lParametros.Add(lParamSaldoDisponivel);

            ReportParameter lParamSaldoTotal = new ReportParameter("pSaldoTotal", lExtrato.SaldoTotal.ToString("N2"));
            lParametros.Add(lParamSaldoTotal);

            ReportDataSource lSource = new ReportDataSource("EExtrato", lExtrato.ListaContaCorrenteMovimento);

            lReport.DataSources.Add(lSource);

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

        #endregion

        #region Event Handlers

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
                        }

                        this.txtDataInicial.Text = DateTime.Now.Date.AddDays(-1).ToString("dd/MM/yyyy");
                        this.txtDataFinal.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                        //this.BuscarRelatorio();
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
            this.PaginaMaster.Crumb3Text = "Extrato";
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            StringBuilder lBuilder = new StringBuilder();

            ContaCorrenteExtratoInfo lExtrato;

            lExtrato = FazerBuscaRelatorio();

            lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t\r\n", "", "", "", "", "Saldo Anterior: R$ ", lExtrato.SaldoAnterior);

            lBuilder.AppendLine("Data Mov\tData Liqui\tHistórico\tDébito\tCrédito\tSaldo\t\r\n");

            foreach (ContaCorrenteMovimentoInfo lMovimento in lExtrato.ListaContaCorrenteMovimento)
                lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t\r\n", lMovimento.DataMovimento.ToString("dd/MM/yyyy"), lMovimento.DataLiquidacao.ToString("dd/MM/yyyy"), lMovimento.Historico, lMovimento.ValorDebito, lMovimento.ValorCredito.ToString("N2"), lMovimento.ValorSaldo.ToString("N2"));

            lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t\r\n", "", "", "", "", "Saldo Disponível: R$ ", lExtrato.SaldoDisponivel);
            lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t\r\n", "", "", "", "", "Saldo Total: R$ ", lExtrato.SaldoTotal);

            Response.Clear();

            Response.ContentType = "text/xls";

            Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

            Response.Charset = "iso-8859-1";

            Response.AddHeader("content-disposition", "attachment;filename=RelatorioPosicaoAtual.xls");

            Response.Write(lBuilder.ToString());

            Response.End();
            
        }

        protected void btnCarregarRelatorio_Click(object sender, EventArgs e)
        {
            this.BuscarRelatorio();
        }

        protected void btnImprimirPDF_Click (object sender, EventArgs e)
        {
            try
            {
                string lNomeDoArquivo = string.Format("Extrato_{0}_{1}", base.SessaoClienteLogado.CodigoPrincipal, DateTime.Now.ToString("yyyyMMddHHmmss"));

                string lMimeType;

                byte[] lRenderedBytes = this.GerarRelatorio(
                    Server.MapPath(@"..\Reports\Extrato.rdlc"),
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
                base.ExibirMensagemJsOnLoad("E", "Erro ao gerar o PDF de extrato.");
            }
        }

        public void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {
                string lNomeDoArquivo = string.Format("Extrato_{0}_{1}", base.SessaoClienteLogado.CodigoPrincipal, DateTime.Now.ToString("yyyyMMddHHmmss"));

                string lMimeType;

                byte[] lRenderedBytes = this.GerarRelatorio(
                    Server.MapPath(@"..\Reports\Extrato.rdlc"),
                    lNomeDoArquivo,
                    out lMimeType);

                var lAnexos = new List<OMS.Email.Lib.EmailAnexoInfo>();
                EmailAnexoInfo lEmailInfo = new EmailAnexoInfo();
                lEmailInfo.Arquivo = lRenderedBytes;
                lEmailInfo.Nome = string.Concat(lNomeDoArquivo, ".pdf");
                lAnexos.Add(lEmailInfo);

                Dictionary<string, string> lVariaveis = new Dictionary<string, string>();
                //base.EnviarEmail("bribeiro@gradualinvestimentos.com.br", "Extrato - Gradual Investimentos", "Extrato.htm", lVariaveis, Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, lAnexos, null);
                base.EnviarEmail(base.SessaoClienteLogado.Email, "Saldos e Limites - Gradual Investimentos", "Extrato.htm", lVariaveis, Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, lAnexos, null);

                base.ExibirMensagemJsOnLoad("I","Um E-mail com o arquivo Pdf foi enviado para " + base.SessaoClienteLogado.Email);

            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao enviar email com PDF de Extratos.");
            }
        }
        #endregion

    }
}