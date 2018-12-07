using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Site.DbLib.Mensagens;
using Gradual.Site.DbLib.Dados.MinhaConta;
using Microsoft.Reporting.WebForms;
using Gradual.OMS.Email.Lib;

namespace Gradual.Site.Www.MinhaConta.Financeiro.Informe
{
    public partial class Operacoes : PaginaBase
    {
        #region Propriedades

        private int AnoSelecionado
        {
            get { return int.Parse(cboAnoDeExercicio.SelectedItem.Value.ToString()); }
        }

        #endregion

        #region Eventos
        
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
                        //this.txtDataInicial.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                        //this.txtData.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                        //this.BuscarRelatorio();
                        this.CarregaCombo();
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
            this.PaginaMaster.Crumb3Text = "Informe de Rendimentos";
        }

        protected void btnOperacoes_Click(object sender, EventArgs e)
        {
            try
            {
                GerarInforme5557(int.Parse(cboAnoDeExercicio.SelectedItem.Value), Server.MapPath(@"..\..\Reports\IRPF5557.rdlc"));

            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
            }
        }

        protected void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {
                string lNomeDoArquivo = string.Format("Informe_Operacoes_{0}_{1}", base.SessaoClienteLogado.CodigoPrincipal, DateTime.Now.ToString("yyyyMMddHHmmss"));


                byte[] lRenderedBytes = GerarInforme5557Bytes(int.Parse(cboAnoDeExercicio.SelectedItem.Value), Server.MapPath(@"..\..\Reports\IRPF5557.rdlc"));

                var lAnexos = new List<OMS.Email.Lib.EmailAnexoInfo>();
                EmailAnexoInfo lEmailInfo = new EmailAnexoInfo();
                lEmailInfo.Arquivo = lRenderedBytes;
                lEmailInfo.Nome = string.Concat(lNomeDoArquivo, ".pdf");
                lAnexos.Add(lEmailInfo);

                Dictionary<string, string> lVariaveis = new Dictionary<string, string>();
                //base.EnviarEmail("bribeiro@gradualinvestimentos.com.br", "Informe de rendimentos (Operações) - Gradual Investimentos", "InformeRendimentos.htm", lVariaveis, Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, lAnexos, null);
                base.EnviarEmail(base.SessaoClienteLogado.Email, "Informe de rendimentos (Daytrade) - Gradual Investimentos", "InformeRendimentos.htm", lVariaveis, Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, lAnexos, null);

                base.ExibirMensagemJsOnLoad("I", "Um E-mail com o arquivo Pdf foi enviado para " + base.SessaoClienteLogado.Email);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad("E", "Erro ao enviar email de informe de rendimentos de operações", false, ex.StackTrace);
            }
        }
        #endregion

        #region Métodos
        private void GerarInforme5557(int pAno, string pCaminho)
        {
            int lRelatorio = 5557;

            ReceberEntidadeCadastroRequest<ClienteInfo> lRequest = new ReceberEntidadeCadastroRequest<ClienteInfo>();
            ReceberEntidadeCadastroResponse<ClienteInfo> lResponse;

            string lMensagem = "Resposta do serviço com erro em InformesComprovanets.aspx > GerarInforme5557(pAno [{0}], pCaminho [{1}]): [{2}]\r\n{3}";

            lRequest.EntidadeCadastro = new ClienteInfo();
            lRequest.EntidadeCadastro.IdCliente = base.SessaoClienteLogado.IdCliente;

            lResponse = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteInfo>(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                DateTime lDataNascimento = lResponse.EntidadeCadastro.DtNascimentoFundacao.Value;

                int lCondicaoDependente = 1;

                SinacorEnderecoRequest lRequestEndereco = new SinacorEnderecoRequest();
                SinacorEnderecoResponse lResponseEndereco;

                lRequestEndereco.SinacorEndereco = new DbLib.Dados.MinhaConta.SinacorEnderecoInfo();

                lRequestEndereco.SinacorEndereco.CPF = Convert.ToInt64(base.SessaoClienteLogado.CpfCnpj);
                lRequestEndereco.SinacorEndereco.DataNascimento = lDataNascimento;
                lRequestEndereco.SinacorEndereco.CondicaoDependente = lCondicaoDependente;

                lResponseEndereco = base.ServicoPersistenciaSite.GetEnderecoSinacorCorrespondencia(lRequestEndereco);

                if (lResponseEndereco.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    string lNomeDoArquivo = string.Format("IRPF_{0}_{1}", pAno, lRelatorio);

                    InformeRendimentosRequest lRequestRendimento = new InformeRendimentosRequest();
                    InformeRendimentosResponse lResponseRendimento;

                    lRequestRendimento.InformeRendimentos = new InformeRendimentosInfo();

                    lRequestRendimento.InformeRendimentos.CPF = Convert.ToInt64(base.SessaoClienteLogado.CpfCnpj);
                    lRequestRendimento.InformeRendimentos.DataNascimento = lDataNascimento;
                    lRequestRendimento.InformeRendimentos.CondicaoDependente = lCondicaoDependente;
                    lRequestRendimento.InformeRendimentos.DataInicio = new DateTime(pAno, 1, 1);
                    lRequestRendimento.InformeRendimentos.DataFim = new DateTime(pAno, 12, 31);
                    lRequestRendimento.InformeRendimentos.CondicaoRetencao = lRelatorio;

                    lResponseRendimento = base.ServicoPersistenciaSite.GetRendimento(lRequestRendimento);

                    if (lResponseRendimento.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                    {
                        if (lResponseRendimento.ListaInformeRendimentos.Count > 0)
                        {
                            GerarRelatorio(AnoSelecionado, pCaminho, lNomeDoArquivo, lResponseRendimento.ListaInformeRendimentos, lResponseEndereco.SinacorEndereco);
                        }
                        else
                        {
                            //base.ExibirMensagemJsOnLoad("I", string.Format("Não há Informe de Rendimentos no período de {0}.", pAno));

                            base.ExibirMensagemJsOnLoad("I", "Não há Informe de Rendimentos no período.");

                            lMensagem = null;   //pra não exibir no final
                        }
                    }
                    else
                    {
                        lMensagem = string.Format(lMensagem, pAno, pCaminho, lResponseRendimento.StatusResposta, lResponseRendimento.DescricaoResposta);
                    }
                }
                else
                {
                    lMensagem = string.Format(lMensagem, pAno, pCaminho, lResponseEndereco.StatusResposta, lResponseEndereco.DescricaoResposta);
                }
            }
            else
            {
                lMensagem = string.Format(lMensagem, pAno, pCaminho, lResponse.StatusResposta, lResponse.DescricaoResposta);
            }

            if (!string.IsNullOrEmpty(lMensagem))
            {
                gLogger.ErrorFormat(lMensagem);

                base.ExibirMensagemJsOnLoad("E", "Erro ao realizar requisição", false, lMensagem);
            }
        }

        private byte[] GerarInforme5557Bytes(int pAno, string pCaminho)
        {
            int lRelatorio = 5557;

            ReceberEntidadeCadastroRequest<ClienteInfo> lRequest = new ReceberEntidadeCadastroRequest<ClienteInfo>();
            ReceberEntidadeCadastroResponse<ClienteInfo> lResponse;

            string lMensagem = "Resposta do serviço com erro em InformesComprovanets.aspx > GerarInforme5557(pAno [{0}], pCaminho [{1}]): [{2}]\r\n{3}";

            byte[] lRetorno = null;

            lRequest.EntidadeCadastro = new ClienteInfo();
            lRequest.EntidadeCadastro.IdCliente = base.SessaoClienteLogado.IdCliente;

            lResponse = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteInfo>(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                DateTime lDataNascimento = lResponse.EntidadeCadastro.DtNascimentoFundacao.Value;

                int lCondicaoDependente = 1;

                SinacorEnderecoRequest lRequestEndereco = new SinacorEnderecoRequest();
                SinacorEnderecoResponse lResponseEndereco;

                lRequestEndereco.SinacorEndereco = new DbLib.Dados.MinhaConta.SinacorEnderecoInfo();

                lRequestEndereco.SinacorEndereco.CPF = Convert.ToInt64(base.SessaoClienteLogado.CpfCnpj);
                lRequestEndereco.SinacorEndereco.DataNascimento = lDataNascimento;
                lRequestEndereco.SinacorEndereco.CondicaoDependente = lCondicaoDependente;

                lResponseEndereco = base.ServicoPersistenciaSite.GetEnderecoSinacorCorrespondencia(lRequestEndereco);

                if (lResponseEndereco.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    string lNomeDoArquivo = string.Format("IRPF_{0}_{1}", pAno, lRelatorio);

                    InformeRendimentosRequest lRequestRendimento = new InformeRendimentosRequest();
                    InformeRendimentosResponse lResponseRendimento;

                    lRequestRendimento.InformeRendimentos = new InformeRendimentosInfo();

                    lRequestRendimento.InformeRendimentos.CPF = Convert.ToInt64(base.SessaoClienteLogado.CpfCnpj);
                    lRequestRendimento.InformeRendimentos.DataNascimento = lDataNascimento;
                    lRequestRendimento.InformeRendimentos.CondicaoDependente = lCondicaoDependente;
                    lRequestRendimento.InformeRendimentos.DataInicio = new DateTime(pAno, 1, 1);
                    lRequestRendimento.InformeRendimentos.DataFim = new DateTime(pAno, 12, 31);
                    lRequestRendimento.InformeRendimentos.CondicaoRetencao = lRelatorio;

                    lResponseRendimento = base.ServicoPersistenciaSite.GetRendimento(lRequestRendimento);

                    if (lResponseRendimento.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                    {
                        if (lResponseRendimento.ListaInformeRendimentos.Count > 0)
                        {
                            lRetorno = GerarRelatorioBytes(AnoSelecionado, pCaminho, lNomeDoArquivo, lResponseRendimento.ListaInformeRendimentos,new List<InformeRendimentosTesouroInfo>(), lResponseEndereco.SinacorEndereco);
                        }
                        else
                        {
                            //base.ExibirMensagemJsOnLoad("I", string.Format("Não há Informe de Rendimentos no período de {0}.", pAno));

                            base.ExibirMensagemJsOnLoad("I", "Não há Informe de Rendimentos no período.");

                            lMensagem = null;   //pra não exibir no final
                        }
                    }
                    else
                    {
                        lMensagem = string.Format(lMensagem, pAno, pCaminho, lResponseRendimento.StatusResposta, lResponseRendimento.DescricaoResposta);
                    }
                }
                else
                {
                    lMensagem = string.Format(lMensagem, pAno, pCaminho, lResponseEndereco.StatusResposta, lResponseEndereco.DescricaoResposta);
                }
            }
            else
            {
                lMensagem = string.Format(lMensagem, pAno, pCaminho, lResponse.StatusResposta, lResponse.DescricaoResposta);
            }

            if (!string.IsNullOrEmpty(lMensagem))
            {
                gLogger.ErrorFormat(lMensagem);

                base.ExibirMensagemJsOnLoad("E", "Erro ao realizar requisição", false, lMensagem);
            }

            return lRetorno;
        }

        private void CarregaCombo()
        {
            cboAnoDeExercicio.Items.Clear();

            ListItem lAno;

            for (int f = DateTime.Now.Year - 1; f > DateTime.Now.Year - 6; f--)
            {
                lAno = new ListItem(f.ToString(), f.ToString());

                cboAnoDeExercicio.Items.Add(lAno);
            }

            cboAnoDeExercicio.SelectedIndex = 0;
        }

        private void GerarRelatorio(int pAno, string pCaminhoRelatorio, string pCaminhoDoArquivo, List<InformeRendimentosInfo> pRendimento, Gradual.Site.DbLib.Dados.MinhaConta.SinacorEnderecoInfo pEndereco)
        {
            GerarRelatorio(pAno, pCaminhoRelatorio, pCaminhoDoArquivo, pRendimento, new List<InformeRendimentosTesouroInfo>(), pEndereco);
        }

        private void GerarRelatorio(int pAno, string pCaminhoRelatorio, string pCaminhoDoArquivo, List<InformeRendimentosInfo> pRendimento, List<InformeRendimentosTesouroInfo> pRendimentoTesouro, Gradual.Site.DbLib.Dados.MinhaConta.SinacorEnderecoInfo pEndereco)
        {
            string lNome = base.SessaoClienteLogado.Nome;

            LocalReport lReport = new LocalReport();

            //Endereço
            lReport.ReportPath = pCaminhoRelatorio;

            //Parametro
            List<ReportParameter> lParametros = new List<ReportParameter>();

            ReportParameter lParamAno = new ReportParameter("pAno", pAno.ToString());
            lParametros.Add(lParamAno);

            ReportParameter lParamNome = new ReportParameter("pNome", lNome);
            lParametros.Add(lParamNome);

            ReportParameter lParamCpf = new ReportParameter("pCpf", base.SessaoClienteLogado.CpfCnpj);
            lParametros.Add(lParamCpf);

            ReportParameter lParamCep = new ReportParameter("pCep", pEndereco.Cep);
            lParametros.Add(lParamCep);

            ReportParameter lParamEndereco = new ReportParameter("pEndereco", pEndereco.Rua + "           " + pEndereco.Numero + "           " + pEndereco.Complemento);
            lParametros.Add(lParamEndereco);

            ReportParameter lParamBairro = new ReportParameter("pBairro", pEndereco.Bairro + "           " + pEndereco.Cidade + "           " + pEndereco.UF);
            lParametros.Add(lParamBairro);

            ReportDataSource lSource = new ReportDataSource("EInformeRendimentos", pRendimento);

            lReport.DataSources.Add(lSource);

            if (pCaminhoRelatorio.IndexOf("8053") > -1)
            {
                ReportDataSource lSourceTesouro = new ReportDataSource("EInformeRendimentosPosicaoTesouroDireto", pRendimentoTesouro);
                lReport.DataSources.Add(lSourceTesouro);

                ReportParameter lParamAnoAtual = new ReportParameter("pAnoAtual", "31/12/" + lParamAno.ToString());
                lParametros.Add(lParamAnoAtual);

                ReportParameter lParamAnoAnterior = new ReportParameter("pAnoAnterior", "31/12/" + (pAno - 1).ToString());
                lParametros.Add(lParamAnoAnterior);
            }

            lReport.SetParameters(lParametros);


            string lReportType, lMimeType, lEncoding, lFileNameExtension, lFileName, lDeviceInfo;

            Warning[] lWarnings;
            string[] lStreams;
            byte[] lRenderedBytes;

            lReportType = "PDF";
            lFileName = pCaminhoDoArquivo;

            lDeviceInfo =
            "<DeviceInfo> <OutputFormat>PDF</OutputFormat> <PageWidth>8.5in</PageWidth> <PageHeight>11in</PageHeight> <MarginTop>0.5in</MarginTop> <MarginLeft>1in</MarginLeft> <MarginRight>1in</MarginRight> <MarginBottom>0.5in</MarginBottom> </DeviceInfo>";

            //Render the report
            lRenderedBytes = lReport.Render(lReportType, lDeviceInfo, out lMimeType, out lEncoding, out lFileNameExtension, out lStreams, out lWarnings);

            //Clear the response stream and write the bytes to the outputstream  //Set content-disposition to "attachment" so that user is prompted to take an action  //on the file (open or save)
            Response.Clear();
            Response.ContentType = lMimeType;
            Response.AddHeader("content-disposition", "attachment; filename=" + lFileName + "." + lFileNameExtension);
            Response.BinaryWrite(lRenderedBytes);
            Response.End();
        }

        private byte[] GerarRelatorioBytes(int pAno, string pCaminhoRelatorio, string pCaminhoDoArquivo, List<InformeRendimentosInfo> pRendimento, List<InformeRendimentosTesouroInfo> pRendimentoTesouro, Gradual.Site.DbLib.Dados.MinhaConta.SinacorEnderecoInfo pEndereco)
        {
            string lNome = base.SessaoClienteLogado.Nome;

            LocalReport lReport = new LocalReport();

            //Endereço
            lReport.ReportPath = pCaminhoRelatorio;

            //Parametro
            List<ReportParameter> lParametros = new List<ReportParameter>();

            ReportParameter lParamAno = new ReportParameter("pAno", pAno.ToString());
            lParametros.Add(lParamAno);

            ReportParameter lParamNome = new ReportParameter("pNome", lNome);
            lParametros.Add(lParamNome);

            ReportParameter lParamCpf = new ReportParameter("pCpf", base.SessaoClienteLogado.CpfCnpj);
            lParametros.Add(lParamCpf);

            ReportParameter lParamCep = new ReportParameter("pCep", pEndereco.Cep);
            lParametros.Add(lParamCep);

            ReportParameter lParamEndereco = new ReportParameter("pEndereco", pEndereco.Rua + "           " + pEndereco.Numero + "           " + pEndereco.Complemento);
            lParametros.Add(lParamEndereco);

            ReportParameter lParamBairro = new ReportParameter("pBairro", pEndereco.Bairro + "           " + pEndereco.Cidade + "           " + pEndereco.UF);
            lParametros.Add(lParamBairro);

            ReportDataSource lSource = new ReportDataSource("EInformeRendimentos", pRendimento);

            lReport.DataSources.Add(lSource);

            if (pCaminhoRelatorio.IndexOf("8053") > -1)
            {
                ReportDataSource lSourceTesouro = new ReportDataSource("EInformeRendimentosPosicaoTesouroDireto", pRendimentoTesouro);
                lReport.DataSources.Add(lSourceTesouro);

                ReportParameter lParamAnoAtual = new ReportParameter("pAnoAtual", "31/12/" + lParamAno.ToString());
                lParametros.Add(lParamAnoAtual);

                ReportParameter lParamAnoAnterior = new ReportParameter("pAnoAnterior", "31/12/" + (pAno - 1).ToString());
                lParametros.Add(lParamAnoAnterior);
            }

            lReport.SetParameters(lParametros);


            string lReportType, lMimeType, lEncoding, lFileNameExtension, lFileName, lDeviceInfo;

            Warning[] lWarnings;
            string[] lStreams;
            byte[] lRenderedBytes;

            lReportType = "PDF";
            lFileName = pCaminhoDoArquivo;

            lDeviceInfo =
            "<DeviceInfo> <OutputFormat>PDF</OutputFormat> <PageWidth>8.5in</PageWidth> <PageHeight>11in</PageHeight> <MarginTop>0.5in</MarginTop> <MarginLeft>1in</MarginLeft> <MarginRight>1in</MarginRight> <MarginBottom>0.5in</MarginBottom> </DeviceInfo>";

            //Render the report
            lRenderedBytes = lReport.Render(lReportType, lDeviceInfo, out lMimeType, out lEncoding, out lFileNameExtension, out lStreams, out lWarnings);

            return lRenderedBytes;

        }
        #endregion
    }
}