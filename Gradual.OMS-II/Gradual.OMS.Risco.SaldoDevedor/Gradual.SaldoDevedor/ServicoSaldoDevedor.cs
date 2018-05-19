using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.IO;
using log4net;
using System.Drawing;
using System.Timers;
using System.ServiceModel;
using System.Configuration;
using Gradual.SaldoDevedor.lib;
using Gradual.SaldoDevedor.lib.Info;
using Gradual.SaldoDevedor.lib.Mensagens;
using Gradual.SaldoDevedor.Utils;
using Gradual.SaldoDevedor.Utils.Info;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Gradual.SaldoDevedor
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoSaldoDevedor : ISaldoDevedor
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IFormatProvider culture = new System.Globalization.CultureInfo("pt-BR", true);

        private PersistenciaDB PersistenciaDB;

        private const string PREFIXO_ARQTEXTO_TESOURARIA = "ccou";
        private const string EXTENSAO_ARQTEXTO_TESOURARIA = ".dat";
        private const string PREFIXO_ARQEXCEL_TESOURARIA = "Saldodevedor";
        private const string PREFIXO_ARQEXCEL_ATENDIMENTO = "Comunicado devedores";
        private const string PREFIXO_ARQEXCEL_MOVIMENTO_DIA = "Devedores";
        private const string EXTENSAO_ARQEXCEL = ".xls";

        private List<string> ListaHoraGravacaoHistorico = new List<string>();

        private Dictionary<int, InformacoesClienteInfo> ListaInformacoesCliente { set; get; }

        private string EmailHost
        {
            get
            {
                if (ConfigurationManager.AppSettings["EmailHost"] == null)
                    return "ironport.gradual.intra";
                return ConfigurationManager.AppSettings["EmailHost"];
            }
        }

        private string EmailFrom
        {
            get
            {
                if (ConfigurationManager.AppSettings["EmailFrom"] == null)
                    return "gradual.saldo.devedor@gradualinvestimentos.com.br";
                return ConfigurationManager.AppSettings["EmailFrom"];
            }
        }

        private string HorasGravacaoHistorico
        {
            get
            {
                if (ConfigurationManager.AppSettings["HorasGravacaoHistorico"] == null)
                    return "0800;1000;1200;1400;1600";
                return ConfigurationManager.AppSettings["HorasGravacaoHistorico"];
            }
        }

        #region Metodos publicos

        public ServicoSaldoDevedor()
        {
            logger.Info("*** Serviço Saldo Devedor ativado ***");

            PersistenciaDB = new PersistenciaDB();

            ListaHoraGravacaoHistorico = HorasGravacaoHistorico.Split(';').ToList();
            foreach (string item in ListaHoraGravacaoHistorico)
                logger.InfoFormat("Configurado timer para gravar histórico as [{0}:{1}]", item.Substring(0,2), item.Substring(2, 2));

            Timer timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Interval = 60000;
            timer.Enabled = true;
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DateTime agora = DateTime.Now;

            foreach (string item in ListaHoraGravacaoHistorico)
                if (agora.Hour == Convert.ToInt32(item.Substring(0, 2)) && agora.Minute == Convert.ToInt32(item.Substring(2, 2)))
                    GravarHistoricoClientesSaldoDevedor();  //GravarHistoricoRetroativoClientesSaldoDevedor();
        }

        public void ImportarClientes()
        {
        }

        public ParametroResponse ObterParametros()
        {
            logger.Info("Executando ObterParametros()");

            ParametroResponse response = new ParametroResponse();
            response.Retorno = ParametroResponse.RETORNO_ERRO;

            try
            {
                response.Parametro = new ParametroInfo();

                response.Parametro.LimiteSaldoMulta = PersistenciaDB.ObterParametroLimiteSaldoDisponivel();
                response.Parametro.TaxaJuros = PersistenciaDB.ObterParametroTaxaJuros();
                response.Parametro.CodigoArquivoTesouraria = Convert.ToInt32(PersistenciaDB.ObterParametroCodigoArquivoTesouraria());

                ExcecaoInfo excecao = PersistenciaDB.ObterListaExcecao();
                response.Parametro.ListaExcecaoAssessor = excecao.ListaAssessor;
                response.Parametro.ListaExcecaoCliente = excecao.ListaCliente;
                response.Retorno = ParametroResponse.RETORNO_OK;

                logger.InfoFormat("ObterParametros(): LimiteSaldoMulta[{0}] TaxaJuros[{1}] CodigoArquivoTesouraria[{2}] QtdListaExcecaoAssessor[{3}] QtdListaExcecaoCliente[{4}]",
                    response.Parametro.LimiteSaldoMulta, response.Parametro.TaxaJuros, response.Parametro.CodigoArquivoTesouraria, response.Parametro.ListaExcecaoAssessor.Count, response.Parametro.ListaExcecaoCliente.Count);
            }
            catch (Exception ex)
            {
                logger.Error("ObterParametros: " + ex.Message, ex);
            }
            return response;
        }

        public ParametroResponse ObterParametroNotificarAtendimentoDiasAlternados()
        {
            logger.Info("Executando ObterParametroNotificarAtendimentoDiasAlternados()");

            ParametroResponse response = new ParametroResponse();
            response.Retorno = ParametroResponse.RETORNO_ERRO;

            try
            {
                response.Parametro = new ParametroInfo();

                response.Parametro.NotificarAtendimentoDiasAlternados = 
                    Convert.ToInt32(PersistenciaDB.ObterParametroNotificarAtendimentoDiasAlternados());

                response.Retorno = ParametroResponse.RETORNO_OK;

                logger.InfoFormat("ObterParametroNotificarAtendimentoDiasAlternados(): [{0}]",
                    response.Parametro.NotificarAtendimentoDiasAlternados);
            }
            catch (Exception ex)
            {
                logger.Error("ObterParametroNotificarAtendimentoDiasAlternados: " + ex.Message, ex);
            }
            return response;
        }

        public ParametroResponse GravarParametros(ParametroRequest request)
        {
            logger.Info("Executando GravarParametros()");

            ParametroResponse response = new ParametroResponse();
            response.Retorno = ParametroResponse.RETORNO_ERRO;

            try
            {
                PersistenciaDB.GravarParametroLimiteSaldoDisponivel(request.Parametro.LimiteSaldoMulta);

                PersistenciaDB.GravarParametroTaxaJuros(request.Parametro.TaxaJuros);

                PersistenciaDB.RemoverListaExcecao(1);
                foreach (int item in request.Parametro.ListaExcecaoCliente)
                    PersistenciaDB.GravarItemExcecao(1, item);

                PersistenciaDB.RemoverListaExcecao(2);
                foreach (int item in request.Parametro.ListaExcecaoAssessor)
                    PersistenciaDB.GravarItemExcecao(2, item);

                response.Retorno = ParametroResponse.RETORNO_OK;

                logger.Info("GravarParametros() executado");
            }
            catch (Exception ex)
            {
                logger.Error("GravarParametros: " + ex.Message, ex);
            }
            return response;
        }

        public ParametroResponse GravarParametroNotificarAtendimentoDiasAlternados(ParametroRequest request)
        {
            logger.Info("Executando GravarParametroNotificarAtendimentoDiasAlternados()");

            ParametroResponse response = new ParametroResponse();
            response.Retorno = ParametroResponse.RETORNO_ERRO;

            try
            {
                PersistenciaDB.GravarParametroNotificarAtendimentoDiasAlternados(request.Parametro.NotificarAtendimentoDiasAlternados);

                response.Retorno = ParametroResponse.RETORNO_OK;

                logger.Info("GravarParametroNotificarAtendimentoDiasAlternados() executado");
            }
            catch (Exception ex)
            {
                logger.Error("GravarParametroNotificarAtendimentoDiasAlternados: " + ex.Message, ex);
            }
            return response;
        }

        public TextoEmailResponse GravarListaTextoEmail(TextoEmailRequest request)
        {
            logger.Info("Executando GravarListaTextoEmail()");

            TextoEmailResponse response = new TextoEmailResponse();
            try
            {
                response = PersistenciaDB.GravarListaTextoEmail(request);
            }
            catch (Exception ex)
            {
                logger.Error("GravarListaTextoEmail: " + ex.Message, ex);
            }

            logger.Info("GravarListaTextoEmail() executado");
            return response;
        }

        public TextoEmailResponse ObterListaTextoEmail()
        {
            logger.Info("Executando ObterListaTextoEmail()");

            TextoEmailResponse response = new TextoEmailResponse();

            try
            {
                response = PersistenciaDB.ObterListaTextoEmail();
            }
            catch (Exception ex)
            {
                logger.Error("ObterListaTextoEmail: " + ex.Message, ex);
            }

            logger.InfoFormat("ObterListaTextoEmail(): TotalLista[{0}]", response.Lista.Count);
            return response;
        }

        public AssessorEmailResponse GravarListaAssessorEmail(AssessorEmailRequest request)
        {
            logger.Info("Executando GravarListaAssessorEmail()");

            AssessorEmailResponse response = new AssessorEmailResponse();
            try
            {
                response = PersistenciaDB.GravarListaAssessorEmail(request);
            }
            catch (Exception ex)
            {
                logger.Error("AssessorEmailResponse: " + ex.Message, ex);
            }

            logger.Info("GravarListaAssessorEmail() executado");
            return response;
        }

        public AssessorEmailResponse RemoverListaAssessorEmail(AssessorEmailRequest request)
        {
            logger.Info("Executando RemoverListaAssessorEmail()");

            AssessorEmailResponse response = new AssessorEmailResponse();
            try
            {
                response = PersistenciaDB.RemoverListaAssessorEmail(request);
            }
            catch (Exception ex)
            {
                logger.Error("RemoverListaAssessorEmail: " + ex.Message, ex);
            }

            logger.Info("RemoverListaAssessorEmail() executado");
            return response;
        }

        public AssessorEmailResponse ObterListaAssessorEmail()
        {
            logger.Info("Executando ObterListaAssessorEmail()");

            AssessorEmailResponse response = new AssessorEmailResponse();

            try
            {
                response = PersistenciaDB.ObterListaAssessorEmail();
            }
            catch (Exception ex)
            {
                logger.Error("ObterListaAssessorEmail: " + ex.Message, ex);
            }

            logger.InfoFormat("ObterListaAssessorEmail(): TotalLista[{0}]", response.Lista.Count);
            return response;
        }

        public EmailResponse GravarListaEmail(EmailRequest request)
        {
            logger.Info("Executando GravarListaEmail()");

            EmailResponse response = new EmailResponse();
            try
            {
                response = PersistenciaDB.GravarListaEmail(request);
            }
            catch (Exception ex)
            {
                logger.Error("GravarListaEmail: " + ex.Message, ex);
            }

            logger.Info("GravarListaEmail() executado");
            return response;
        }

        public EmailResponse ObterListaEmail()
        {
            logger.Info("Executando ObterListaEmail()");

            EmailResponse response = new EmailResponse();

            try
            {
                response = PersistenciaDB.ObterListaEmail();
            }
            catch (Exception ex)
            {
                logger.Error("ObterListaEmail: " + ex.Message, ex);
            }

            logger.InfoFormat("ObterListaEmail(): TotalLista[{0}]", response.Lista.Count);
            return response;
        }

        public EmailResponse ObterDetalheEmail(int idEmail)
        {
            logger.Info("Executando ObterDetalheEmail()");

            EmailResponse response = new EmailResponse();
            response.Retorno = EmailResponse.RETORNO_ERRO;

            try
            {
                response.Lista = new Dictionary<int,EmailInfo>();
                response.Lista.Add(idEmail, PersistenciaDB.ObterDetalheEmail(idEmail));
                response.Retorno = EmailResponse.RETORNO_OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            logger.InfoFormat("ObterDetalheEmail(): Descricao[{0}]", response.Lista[idEmail].Descricao);
            return response;
        }

        public EmailResponse ObterListaDetalheEmail()
        {
            logger.Info("Executando ObterListaDetalheEmail()");

            EmailResponse response = new EmailResponse();

            try
            {
                response = PersistenciaDB.ObterListaDetalheEmail();
            }
            catch (Exception ex)
            {
                logger.Error("ObterListaDetalheEmail: " + ex.Message, ex);
            }

            logger.InfoFormat("ObterListaDetalheEmail(): TotalLista[{0}]", response.Lista.Count);
            return response;
        }

        public HistoricoDatasResponse ObterListaHistoricoDias()
        {
            logger.Info("Executando ObterListaHistoricoDias()");

            HistoricoDatasResponse response = new HistoricoDatasResponse();

            try
            {
                response = PersistenciaDB.ObterListaHistoricoDatas();
            }
            catch (Exception ex)
            {
                logger.Error("ObterListaHistoricoDias: " + ex.Message, ex);
            }

            logger.InfoFormat("ObterListaHistoricoDias(): TotalLista[{0}]", response.Lista.Count);
            return response;
        }

        public HistoricoResponse ObterListaHistorico(HistoricoRequest request)
        {
            logger.Info("Executando ObterListaHistorico()");

            HistoricoResponse response = new HistoricoResponse();

            try
            {
                response = PersistenciaDB.ObterListaHistorico(request);
            }
            catch (Exception ex)
            {
                logger.Error("ObterListaHistorico: " + ex.Message, ex);
            }

            logger.InfoFormat("ObterListaHistorico(): TotalLista[{0}]", response.Lista.Count);
            return response;
        }

        public EnviarEmailResponse EnviarEmailTesouraria(EnviarEmailRequest request)
        {
            logger.Info("Executando EnviarEmailTesouraria()");

            EnviarEmailResponse response = new EnviarEmailResponse();
            response.Retorno = EnviarEmailResponse.RETORNO_ERRO;

            try
            {
                DateTime dataMovimento = request.ListaClientes[0].DataMovimento;

                string anexos = "";

                string arqTextoTesouraria =
                    GeraArquivoTextoTesouraria(request.DadosEmail.Anexos, dataMovimento, request.ListaClientes);
                if (File.Exists(arqTextoTesouraria))
                {
                    anexos = arqTextoTesouraria;
                    logger.InfoFormat("EnviarEmailTesouraria: Arquivo gerado[{0}]", arqTextoTesouraria);
                }
                else
                    logger.ErrorFormat("EnviarEmailTesouraria: Falha na geração do arquivo [{0}]", arqTextoTesouraria);

                string arqExcelTesouraria =
                    GeraArquivoExcelTesouraria(request.DadosEmail.Anexos, dataMovimento, request.ListaClientes);
                if (File.Exists(arqExcelTesouraria))
                {
                    anexos = anexos + (anexos.Length == 0 ? "" : ";") + arqExcelTesouraria;
                    logger.InfoFormat("EnviarEmailTesouraria: Arquivo gerado[{0}]", arqExcelTesouraria);
                }
                else
                    logger.ErrorFormat("EnviarEmailTesouraria: Falha na geração do arquivo [{0}]", arqExcelTesouraria);

                request.DadosEmail.Anexos = anexos;
                request.DadosEmail.Assunto = request.DadosEmail.Assunto + " " + dataMovimento.ToString("dd-MM");

                StringWriter htmlConteudo = new StringWriter();
                HtmlTextWriter html = new HtmlTextWriter(htmlConteudo);

                // Monta HTML do corpo do e-mail
                html.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Trebuchet MS, Arial, sans-serif");
                html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "left");
                html.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "normal");
                html.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "14px");
                html.RenderBeginTag(HtmlTextWriterTag.Div);

                // Acrescenta o conteudo customizado no corpo
                foreach (string linha in request.DadosEmail.Conteudo.Split(
                        new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (linha.Length > 0)
                    {
                        html.WriteBreak();
                        html.WriteBreak();
                        html.Write(linha);
                    }
                }
                html.Write(" " + dataMovimento.ToString("dd/MM/yyyy"));

                // Acrescenta rodape no corpo
                html.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "11px");
                html.AddStyleAttribute(HtmlTextWriterStyle.Color, "gray");
                html.RenderBeginTag(HtmlTextWriterTag.Div);
                html.WriteBreak();
                html.WriteBreak();
                html.WriteBreak();
                html.WriteBreak();
                html.Write("Mensagem enviada automaticamente pelo Sistema Gradual Saldo Devedor.");
                html.RenderEndTag();

                html.RenderEndTag();

                request.DadosEmail.Conteudo = htmlConteudo.ToString();

                EnviarEmail(request);

                foreach (InformacoesClienteInfo item in request.ListaClientes)
                {
                    InformacaoClienteRequest reqMovimento = new InformacaoClienteRequest();
                    reqMovimento.DadosCliente = item;
                    reqMovimento.DadosCliente.DescricaoMovimento = 
                        String.Format("Tesouraria notificada por e-mail com os anexos ({0}) e ({1})",
                        Path.GetFileName(arqTextoTesouraria), Path.GetFileName(arqExcelTesouraria));
                    reqMovimento.DadosEmail = request.DadosEmail;
                    PersistenciaDB.GravarMovimentoCliente(reqMovimento);
                }

                response.Retorno = EnviarEmailResponse.RETORNO_OK;
            }
            catch (Exception ex)
            {
                logger.Error("EnviarEmailTesouraria: " + ex.Message, ex);
            }

            logger.Info("EnviarEmailTesouraria() executado");
            return response;
        }

        public EnviarEmailResponse EnviarEmailAtendimento(EnviarEmailRequest request)
        {
            logger.Info("Executando EnviarEmailAtendimento()");

            EnviarEmailResponse response = new EnviarEmailResponse();
            response.Retorno = EnviarEmailResponse.RETORNO_ERRO;

            try
            {
                DateTime dataMovimento = request.ListaClientes[0].DataMovimento;

                string anexos = "";

                string arqExcelAtendimento =
                    GeraArquivoExcelAtendimento(request.DadosEmail.Anexos, dataMovimento, request.ListaClientes);
                if (File.Exists(arqExcelAtendimento))
                {
                    anexos = arqExcelAtendimento;
                    logger.InfoFormat("EnviarEmailAtendimento: Arquivo gerado[{0}]", arqExcelAtendimento);
                }
                else
                    logger.ErrorFormat("EnviarEmailAtendimento: Falha na geração do arquivo [{0}]", arqExcelAtendimento);

                request.DadosEmail.Anexos = anexos;
                request.DadosEmail.Assunto = request.DadosEmail.Assunto;

                StringWriter htmlConteudo = new StringWriter();
                HtmlTextWriter html = new HtmlTextWriter(htmlConteudo);

                // Monta HTML do corpo do e-mail
                html.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Trebuchet MS, Arial, sans-serif");
                html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "left");
                html.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "normal");
                html.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "14px");
                html.RenderBeginTag(HtmlTextWriterTag.Div);

                // Acrescenta o conteudo customizado no corpo
                foreach (string linha in request.DadosEmail.Conteudo.Split(
                        new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (linha.Length > 0)
                    {
                        html.WriteBreak();
                        html.WriteBreak();
                        html.Write(linha);
                    }
                }
                html.Write(" " + dataMovimento.ToString("dd/MM/yyyy"));

                // Acrescenta rodape no corpo
                html.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "11px");
                html.AddStyleAttribute(HtmlTextWriterStyle.Color, "gray");
                html.RenderBeginTag(HtmlTextWriterTag.Div);
                html.WriteBreak();
                html.WriteBreak();
                html.WriteBreak();
                html.WriteBreak();
                html.Write("Mensagem enviada automaticamente pelo Sistema Gradual Saldo Devedor.");
                html.RenderEndTag();

                html.RenderEndTag();

                request.DadosEmail.Conteudo = htmlConteudo.ToString();

                EnviarEmail(request);

                foreach (InformacoesClienteInfo item in request.ListaClientes)
                {
                    InformacaoClienteRequest reqMovimento = new InformacaoClienteRequest();
                    reqMovimento.DadosCliente = item;
                    reqMovimento.DadosCliente.DescricaoMovimento =
                        String.Format("Atendimento notificada por e-mail com o anexo ({0})", Path.GetFileName(arqExcelAtendimento));
                    reqMovimento.DadosEmail = request.DadosEmail;
                    PersistenciaDB.GravarMovimentoCliente(reqMovimento);
                }

                response.Retorno = EnviarEmailResponse.RETORNO_OK;
            }
            catch (Exception ex)
            {
                logger.Error("EnviarEmailAtendimento: " + ex.Message, ex);
            }

            logger.Info("EnviarEmailAtendimento() executado");
            return response;
        }

        public EnviarEmailResponse EnviarEmailRisco(EnviarEmailRequest request)
        {
            logger.Info("Executando EnviarEmailRisco()");

            EnviarEmailResponse response = new EnviarEmailResponse();
            response.Retorno = EnviarEmailResponse.RETORNO_ERRO;

            try
            {
                DateTime dataMovimento = request.ListaClientes[0].DataMovimento;

                string anexos = "";

                string arqExcelRisco =
                    GeraArquivoExcelMovimentoDia(request.DadosEmail.Anexos, dataMovimento, request.ListaClientes);
                if (File.Exists(arqExcelRisco))
                {
                    anexos = arqExcelRisco;
                    logger.InfoFormat("EnviarEmailRisco: Arquivo gerado[{0}]", arqExcelRisco);
                }
                else
                    logger.ErrorFormat("EnviarEmailRisco: Falha na geração do arquivo [{0}]", arqExcelRisco);

                request.DadosEmail.Anexos = anexos;
                request.DadosEmail.Assunto = request.DadosEmail.Assunto + " " + dataMovimento.ToString("dd/MM/yyyy");

                StringWriter htmlConteudo = new StringWriter();
                HtmlTextWriter html = new HtmlTextWriter(htmlConteudo);

                // Monta HTML do corpo do e-mail
                html.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Trebuchet MS, Arial, sans-serif");
                html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "left");
                html.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "normal");
                html.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "14px");
                html.RenderBeginTag(HtmlTextWriterTag.Div);

                // Acrescenta o conteudo customizado no corpo
                foreach (string linha in request.DadosEmail.Conteudo.Split(
                        new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (linha.Length > 0)
                    {
                        html.WriteBreak();
                        html.WriteBreak();
                        html.Write(linha);
                    }
                }

                // Acrescenta rodape no corpo
                html.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "11px");
                html.AddStyleAttribute(HtmlTextWriterStyle.Color, "gray");
                html.RenderBeginTag(HtmlTextWriterTag.Div);
                html.WriteBreak();
                html.WriteBreak();
                html.WriteBreak();
                html.WriteBreak();
                html.Write("Mensagem enviada automaticamente pelo Sistema Gradual Saldo Devedor.");
                html.RenderEndTag();

                html.RenderEndTag();

                request.DadosEmail.Conteudo = htmlConteudo.ToString();

                EnviarEmail(request);

                foreach (InformacoesClienteInfo item in request.ListaClientes)
                {
                    InformacaoClienteRequest reqMovimento = new InformacaoClienteRequest();
                    reqMovimento.DadosCliente = item;
                    reqMovimento.DadosCliente.DescricaoMovimento =
                        String.Format("Histórico de Movimento enviado por e-mail com o anexo ({0})", Path.GetFileName(arqExcelRisco));
                    reqMovimento.DadosEmail = request.DadosEmail;
                    PersistenciaDB.GravarMovimentoCliente(reqMovimento);
                }

                response.Retorno = EnviarEmailResponse.RETORNO_OK;
            }
            catch (Exception ex)
            {
                logger.Error("EnviarEmailRisco: " + ex.Message, ex);
            }

            logger.Info("EnviarEmailRisco() executado");
            return response;
        }

        public EnviarEmailResponse EnviarEmailAssessores(EnviarEmailRequest request)
        {
            logger.Info("Executando EnviarEmailAssessores()");

            EnviarEmailResponse response = new EnviarEmailResponse();
            response.Retorno = EnviarEmailResponse.RETORNO_ERRO;

            try
            {
                StringWriter htmlConteudo = new StringWriter();
                HtmlTextWriter html = new HtmlTextWriter(htmlConteudo);

                // Monta HTML do corpo do e-mail
                html.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Trebuchet MS, Arial, sans-serif");
                html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "left");
                html.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "normal");
                html.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "12px");
                html.RenderBeginTag(HtmlTextWriterTag.Div);

                // Acrescenta titulo no corpo
                html.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "bold");
                html.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "16px");
                html.RenderBeginTag(HtmlTextWriterTag.Div);
                html.Write(request.DadosEmail.Titulo);
                html.WriteBreak();
                html.WriteBreak();
                html.RenderEndTag();

                // Acrescenta o conteudo customizado no corpo
                foreach (string linha in request.DadosEmail.Conteudo.Split(
                    new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (linha.Length > 0)
                    {
                        html.Write(linha);
                        html.WriteBreak();
                        html.WriteBreak();
                    }
                }

                // Acrescenta a tabela de clientes
                html.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Trebuchet MS, Arial, sans-serif");
                html.AddAttribute(HtmlTextWriterAttribute.Border, "1");
                html.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                html.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                html.RenderBeginTag(HtmlTextWriterTag.Table);

                html.AddStyleAttribute(HtmlTextWriterStyle.Height, "30px");
                html.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "12px");
                html.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "gray");
                html.AddStyleAttribute(HtmlTextWriterStyle.Color, "white");
                html.RenderBeginTag(HtmlTextWriterTag.Tr);

                html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center");
                html.AddStyleAttribute(HtmlTextWriterStyle.Width, "80px");
                html.RenderBeginTag(HtmlTextWriterTag.Td);
                html.Write("Data");
                html.RenderEndTag();
                html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center");
                html.AddStyleAttribute(HtmlTextWriterStyle.Width, "60px");
                html.RenderBeginTag(HtmlTextWriterTag.Td);
                html.Write("Assessor");
                html.RenderEndTag();
                html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "left");
                html.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "10px");
                html.AddStyleAttribute(HtmlTextWriterStyle.Width, "300px");
                html.RenderBeginTag(HtmlTextWriterTag.Td);
                html.Write("Nome Assessor");
                html.RenderEndTag();
                html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center");
                html.AddStyleAttribute(HtmlTextWriterStyle.Width, "60px");
                html.RenderBeginTag(HtmlTextWriterTag.Td);
                html.Write("Cliente");
                html.RenderEndTag();
                html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "left");
                html.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "10px");
                html.AddStyleAttribute(HtmlTextWriterStyle.Width, "300px");
                html.RenderBeginTag(HtmlTextWriterTag.Td);
                html.Write("Nome Cliente");
                html.RenderEndTag();
                html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "right");
                html.AddStyleAttribute(HtmlTextWriterStyle.Width, "80px");
                html.RenderBeginTag(HtmlTextWriterTag.Td);
                html.Write("Disponível" + HtmlTextWriter.SpaceChar);
                html.RenderEndTag();
                html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center");
                html.AddStyleAttribute(HtmlTextWriterStyle.Width, "80px");
                html.RenderBeginTag(HtmlTextWriterTag.Td);
                html.Write("Dias atraso");
                html.RenderEndTag();
                html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "right");
                html.AddStyleAttribute(HtmlTextWriterStyle.Width, "80px");
                html.RenderBeginTag(HtmlTextWriterTag.Td);
                html.Write("Projetado" + HtmlTextWriter.SpaceChar);
                html.RenderEndTag();
                html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "right");
                html.AddStyleAttribute(HtmlTextWriterStyle.Width, "90px");
                html.RenderBeginTag(HtmlTextWriterTag.Td);
                html.Write("Desenquadrado" + HtmlTextWriter.SpaceChar);
                html.RenderEndTag();
                html.RenderEndTag();

                foreach (InformacoesClienteInfo item in request.ListaClientes)
                {
                    html.AddStyleAttribute(HtmlTextWriterStyle.Height, "30px");
                    html.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "12px");
                    html.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "white");
                    html.RenderBeginTag(HtmlTextWriterTag.Tr);

                    html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center");
                    html.RenderBeginTag(HtmlTextWriterTag.Td);
                    html.Write(item.DataMovimento.ToString("dd/MM/yyyy"));
                    html.RenderEndTag();
                    html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center");
                    html.RenderBeginTag(HtmlTextWriterTag.Td);
                    html.Write(item.CodigoAssessor);
                    html.RenderEndTag();
                    html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "left");
                    html.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "10px");
                    html.RenderBeginTag(HtmlTextWriterTag.Td);
                    html.Write(item.NomeAssessor);
                    html.RenderEndTag();
                    html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center");
                    html.RenderBeginTag(HtmlTextWriterTag.Td);
                    html.Write(item.CodigoCliente);
                    html.RenderEndTag();
                    html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "left");
                    html.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "10px");
                    html.RenderBeginTag(HtmlTextWriterTag.Td);
                    html.Write(item.NomeCliente);
                    html.RenderEndTag();
                    html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "right");
                    if (item.SaldoDisponivel < 0)
                        html.AddStyleAttribute(HtmlTextWriterStyle.Color, "red");
                    else
                        html.AddStyleAttribute(HtmlTextWriterStyle.Color, "black");
                    html.RenderBeginTag(HtmlTextWriterTag.Td);
                    html.Write(item.SaldoDisponivel.ToString("N2", culture));
                    html.RenderEndTag();
                    html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center");
                    html.RenderBeginTag(HtmlTextWriterTag.Td);
                    html.Write(item.NrDiasNegativo);
                    html.RenderEndTag();
                    html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "right");
                    if (item.SaldoTotal < 0)
                        html.AddStyleAttribute(HtmlTextWriterStyle.Color, "red");
                    else
                        html.AddStyleAttribute(HtmlTextWriterStyle.Color, "black");
                    html.RenderBeginTag(HtmlTextWriterTag.Td);
                    html.Write(item.SaldoTotal.ToString("N2", culture));
                    html.RenderEndTag();
                    html.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "right");
                    if (item.Desenquadrado < 0)
                        html.AddStyleAttribute(HtmlTextWriterStyle.Color, "red");
                    else
                        html.AddStyleAttribute(HtmlTextWriterStyle.Color, "black");
                    html.RenderBeginTag(HtmlTextWriterTag.Td);
                    html.Write(item.Desenquadrado.ToString("N2", culture));
                    html.RenderEndTag();
                    html.RenderEndTag();
                }

                html.RenderEndTag();

                // Acrescenta rodape no corpo
                html.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "11px");
                html.AddStyleAttribute(HtmlTextWriterStyle.Color, "gray");
                html.RenderBeginTag(HtmlTextWriterTag.Div);
                html.WriteBreak();
                html.WriteBreak();
                html.WriteBreak();
                html.WriteBreak();
                html.Write("Mensagem enviada automaticamente pelo Sistema Gradual Saldo Devedor.");
                html.RenderEndTag();

                html.RenderEndTag();

                request.DadosEmail.Conteudo = htmlConteudo.ToString();

                EnviarEmail(request);

                foreach (InformacoesClienteInfo item in request.ListaClientes)
                {
                    InformacaoClienteRequest reqMovimento = new InformacaoClienteRequest();
                    reqMovimento.DadosCliente = item;
                    reqMovimento.DadosCliente.DescricaoMovimento = "Assessor notificado por e-mail";
                    reqMovimento.DadosEmail = request.DadosEmail;
                    PersistenciaDB.GravarMovimentoCliente(reqMovimento);
                }

                response.Retorno = EnviarEmailResponse.RETORNO_OK;
            }
            catch (Exception ex)
            {
                logger.Error("EnviarEmailAssessores: " + ex.Message, ex);
            }

            logger.Info("EnviarEmailAssessores() executado");
            return response;
        }

        public EnviarEmailResponse EnviarEmail(EnviarEmailRequest request)
        {
            logger.Info("Executando EnviarEmail()");

            EnviarEmailResponse response = new EnviarEmailResponse();
            response.Retorno = EnviarEmailResponse.RETORNO_ERRO;

            try
            {
                ConfigMailInfo cfgMail = new ConfigMailInfo();
                cfgMail.SmtpHost = EmailHost;

                MessageMailInfo msgMail = new MessageMailInfo();
                msgMail.From = EmailFrom;
                msgMail.To = request.DadosEmail.EmailPara;
                msgMail.Cc = request.DadosEmail.EmailCopia;
                msgMail.Cco = request.DadosEmail.EmailCopiaOculta;
                msgMail.Subject = request.DadosEmail.Assunto;
                msgMail.Body = request.DadosEmail.Conteudo;
                if (request.DadosEmail.Anexos != null && request.DadosEmail.Anexos.Length > 0)
                    msgMail.FileAttach = request.DadosEmail.Anexos;

                if (Mail.SendEmail(cfgMail, msgMail))
                    response.Retorno = EnviarEmailResponse.RETORNO_OK;
            }
            catch (Exception ex)
            {
                logger.Error("EnviarEmail: " + ex.Message, ex);
            }

            logger.Info("EnviarEmail() executado");
            return response;
        }

        public InformacaoClienteResponse ObterClientesSaldoDevedor(InformacaoClienteRequest request)
        {
            logger.Info("Executando ObterClientesSaldoDevedor()");

            InformacaoClienteResponse response = new InformacaoClienteResponse();

            try
            {
                response = PersistenciaDB.ObterListaClientesSaldoDevedor(request);
            }
            catch (Exception ex)
            {
                logger.Error("ObterClientesSaldoDevedor: " + ex.Message, ex);
            }

            logger.InfoFormat("ObterClientesSaldoDevedor(): TotalClientes[{0}]", response.ListaInformacoesCliente.Count);
            return response;
        }

        #endregion

        #region Metodos privados

        private string GeraArquivoTextoTesouraria(string dir, DateTime data, List<InformacoesClienteInfo> lista)
        {
            string arq = "";

            try
            {
                int codigoArqTesouraria = Convert.ToInt32(PersistenciaDB.ObterParametroCodigoArquivoTesouraria());

                arq = dir + "\\" + PREFIXO_ARQTEXTO_TESOURARIA + data.ToString("ddMM") + EXTENSAO_ARQTEXTO_TESOURARIA;

                if (File.Exists(arq))
                {
                    string arqAnt = dir + "\\" + PREFIXO_ARQTEXTO_TESOURARIA + " " +
                        data.ToString("ddMM") + "." + data.ToString("HHmmss") + EXTENSAO_ARQTEXTO_TESOURARIA;
                    File.Move(arq, arqAnt);
                }

                using (StreamWriter arqtxt = new StreamWriter(@arq))
                {
                    arqtxt.WriteLine("00OUTROS  OUT".PadRight(250, ' '));

                    foreach (InformacoesClienteInfo item in lista)
                    {
                        arqtxt.WriteLine(
                            "01" +
                            data.ToString("dd/MM/yyyy") +
                            item.CodigoCliente.ToString().PadLeft(7, '0') +
                            codigoArqTesouraria.ToString().PadLeft(4, '0') +
                            (Math.Truncate(item.JurosCalculado)).ToString("0;0").PadLeft(13, '0') +
                            item.JurosCalculado.ToString("N2", culture).Substring((item.JurosCalculado.ToString("N2", culture)).IndexOf(",") + 1) +
                            "OUT".PadLeft(192, ' ') + 
                            "N".PadRight(20, ' '));

                    }

                    arqtxt.WriteLine("99OUTROSOUT".PadRight(250, ' '));
                }
            }
            catch (Exception ex)
            {
                logger.Error("GeraArquivoTextoTesouraria: " + ex.Message, ex);
            }

            return arq;
        }

        private string GeraArquivoExcelTesouraria(string dir, DateTime data, List<InformacoesClienteInfo> lista)
        {
            string arq = "";

            try
            {
                arq = dir + "\\" + PREFIXO_ARQEXCEL_TESOURARIA + " " + data.ToString("dd-MM") + EXTENSAO_ARQEXCEL;

                if (File.Exists(arq))
                {
                    string arqAnt = dir + "\\" + PREFIXO_ARQEXCEL_TESOURARIA + " " +
                        data.ToString("dd-MM") + "." + data.ToString("HHmmss") + EXTENSAO_ARQEXCEL;
                    File.Move(arq, arqAnt);
                }

                FileInfo arquivo = new FileInfo(arq);
                ExcelPackage appExcel = new ExcelPackage(arquivo);

                ExcelWorksheet excelSheet1 = appExcel.Workbook.Worksheets.Add("Plan1");

                ExcelRange range = excelSheet1.Cells[1, 1, lista.Count + 1, 6];
                range.Style.Font.Name = "Calibri";
                range.Style.Font.Size = 10;

                ExcelRange rangeTitulos = excelSheet1.Cells[1, 1, 1, 6];
                rangeTitulos.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rangeTitulos.Style.Fill.BackgroundColor.SetColor(Color.Blue);
                rangeTitulos.Style.Font.Color.SetColor(Color.White);
                rangeTitulos.Style.Font.Bold = true;

                excelSheet1.Cells[1, 1].Value = "Código";
                excelSheet1.Cells[1, 2].Value = "Nome";
                excelSheet1.Cells[1, 3].Value = "Assessor";
                excelSheet1.Cells[1, 4].Value = "Data Movto";
                excelSheet1.Cells[1, 5].Value = "Saldo Financiado";
                excelSheet1.Cells[1, 6].Value = "Taxa";

                for (int ct = 0; ct < lista.Count; ct++)
                {
                    excelSheet1.Cells[ct + 2, 1].Value = lista[ct].CodigoCliente;
                    excelSheet1.Cells[ct + 2, 2].Value = lista[ct].NomeCliente;
                    excelSheet1.Cells[ct + 2, 3].Value = lista[ct].CodigoAssessor;
                    excelSheet1.Cells[ct + 2, 4].Value = lista[ct].DataMovimento.ToString("dd/MM/yyyy");
                    excelSheet1.Cells[ct + 2, 5].Value = Convert.ToDecimal(lista[ct].SaldoDisponivel.ToString("N2").Replace('-', ' '));
                    excelSheet1.Cells[ct + 2, 6].Value = Convert.ToDecimal(lista[ct].JurosCalculado.ToString("N2").Replace('-', ' '));
                }

                range.AutoFitColumns();

                appExcel.Save();
            }
            catch (Exception ex)
            {
                logger.Error("GeraArquivoExcelTesouraria: " + ex.Message, ex);
            }

            return arq;
        }

        private string GeraArquivoExcelAtendimento(string dir, DateTime data, List<InformacoesClienteInfo> lista)
        {
            string arq = "";

            try
            {
                arq = dir + "\\" + PREFIXO_ARQEXCEL_ATENDIMENTO + " " + data.ToString("ddMMyyyy") + EXTENSAO_ARQEXCEL;

                if (File.Exists(arq))
                {
                    string arqAnt = dir + "\\" + PREFIXO_ARQEXCEL_ATENDIMENTO + " " + 
                        data.ToString("ddMMyyyy") + "." + data.ToString("HHmmss") + EXTENSAO_ARQEXCEL;
                    File.Move(arq, arqAnt);
                }

                FileInfo arquivo = new FileInfo(arq);
                ExcelPackage appExcel = new ExcelPackage(arquivo);

                ExcelWorksheet excelSheet1 = appExcel.Workbook.Worksheets.Add("Plan1");

                ExcelRange range = excelSheet1.Cells[1, 1, lista.Count + 1, 6];
                range.Style.Font.Name = "Calibri";
                range.Style.Font.Size = 10;

                ExcelRange rangeTitulos = excelSheet1.Cells[1, 1, 1, 6];
                rangeTitulos.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rangeTitulos.Style.Fill.BackgroundColor.SetColor(Color.Blue);
                rangeTitulos.Style.Font.Color.SetColor(Color.White);
                rangeTitulos.Style.Font.Bold = true;

                excelSheet1.Cells[1, 1].Value = "cod cliente";
                excelSheet1.Cells[1, 2].Value = "cliente";
                excelSheet1.Cells[1, 3].Value = "email";
                excelSheet1.Cells[1, 4].Value = "Data Movto";
                excelSheet1.Cells[1, 5].Value = "cod assessor";
                excelSheet1.Cells[1, 6].Value = "assessor";

                for (int ct = 0; ct < lista.Count; ct++)
                {
                    excelSheet1.Cells[ct + 2, 1].Value = lista[ct].CodigoCliente;
                    excelSheet1.Cells[ct + 2, 2].Value = lista[ct].NomeCliente;
                    excelSheet1.Cells[ct + 2, 3].Value = lista[ct].EmailCliente;
                    excelSheet1.Cells[ct + 2, 4].Value = lista[ct].DataMovimento.ToString("dd/MM/yyyy");
                    excelSheet1.Cells[ct + 2, 5].Value = lista[ct].CodigoAssessor;
                    excelSheet1.Cells[ct + 2, 6].Value = lista[ct].NomeAssessor;
                }

                range.AutoFitColumns();

                appExcel.Save();
            }
            catch (Exception ex)
            {
                logger.Error("GeraArquivoExcelAtendimento: " + ex.Message, ex);
            }

            return arq;
        }

        private string GeraArquivoExcelMovimentoDia(string dir, DateTime data, List<InformacoesClienteInfo> lista)
        {
            string arq = "";

            try
            {
                arq = dir + "\\" + PREFIXO_ARQEXCEL_MOVIMENTO_DIA + " " + data.ToString("ddMMyyyy") + EXTENSAO_ARQEXCEL;

                if (File.Exists(arq))
                {
                    string arqAnt = dir + "\\" + PREFIXO_ARQEXCEL_MOVIMENTO_DIA + " " +
                        data.ToString("ddMMyyyy") + "." + data.ToString("HHmmss") + EXTENSAO_ARQEXCEL;
                    File.Move(arq, arqAnt);
                }

                FileInfo arquivo = new FileInfo(arq);
                ExcelPackage appExcel = new ExcelPackage(arquivo);

                ExcelWorksheet excelSheet1 = appExcel.Workbook.Worksheets.Add("Plan1");

                ExcelRange range = excelSheet1.Cells[1, 1, lista.Count + 1, 8];
                range.Style.Font.Name = "Calibri";
                range.Style.Font.Size = 10;

                ExcelRange rangeTitulos = excelSheet1.Cells[1, 1, 1, 8];
                rangeTitulos.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rangeTitulos.Style.Fill.BackgroundColor.SetColor(Color.Blue);
                rangeTitulos.Style.Font.Color.SetColor(Color.White);
                rangeTitulos.Style.Font.Bold = true;

                excelSheet1.Cells[1, 1].Value = "Data Movto";
                excelSheet1.Cells[1, 2].Value = "Assessor";
                excelSheet1.Cells[1, 3].Value = "Nome Assessor";
                excelSheet1.Cells[1, 4].Value = "Cliente";
                excelSheet1.Cells[1, 5].Value = "Nome Cliente";
                excelSheet1.Cells[1, 6].Value = "Saldo Abertura";
                excelSheet1.Cells[1, 7].Value = "Dias em atraso";
                excelSheet1.Cells[1, 8].Value = "Projetado Abertura";

                for (int ct = 0; ct < lista.Count; ct++)
                {
                    excelSheet1.Cells[ct + 2, 1].Value = lista[ct].DataMovimento.ToString("dd/MM/yyyy");
                    excelSheet1.Cells[ct + 2, 2].Value = lista[ct].CodigoAssessor;
                    excelSheet1.Cells[ct + 2, 3].Value = lista[ct].NomeAssessor;
                    excelSheet1.Cells[ct + 2, 4].Value = lista[ct].CodigoCliente;
                    excelSheet1.Cells[ct + 2, 5].Value = lista[ct].NomeCliente;

                    excelSheet1.Cells[ct + 2, 6].Value = Convert.ToDecimal(lista[ct].SaldoDisponivel.ToString("N2"));
                    excelSheet1.Cells[ct + 2, 6].Style.Font.Color.SetColor(Color.Red);

                    excelSheet1.Cells[ct + 2, 7].Value = lista[ct].NrDiasNegativo.ToString();

                    excelSheet1.Cells[ct + 2, 8].Value = Convert.ToDecimal(lista[ct].SaldoTotal.ToString("N2"));
                    if (lista[ct].SaldoTotal < 0)
                        excelSheet1.Cells[ct + 2, 8].Style.Font.Color.SetColor(Color.Red);
                }

                range.AutoFitColumns();

                appExcel.Save();
            }
            catch (Exception ex)
            {
                logger.Error("GeraArquivoExcelMovimentoDia: " + ex.Message, ex);
            }

            return arq;
        }

        private void GravarHistoricoClientesSaldoDevedor()
        {
            logger.Info("* Iniciando gravação de histórico de clientes com Saldo Devedor");

            InformacaoClienteResponse response = new InformacaoClienteResponse();

            try
            {
                decimal limiteSaldoDisponivel = PersistenciaDB.ObterParametroLimiteSaldoDisponivel();

                InformacaoClienteRequest request = new InformacaoClienteRequest();
                request.DadosCliente = new InformacoesClienteInfo();
                request.DadosCliente.DataMovimento = DateTime.Now;

                response = PersistenciaDB.ObterListaClientesSaldoDevedor(request);
                logger.InfoFormat("TotalClientes [{0}]", response.ListaInformacoesCliente.Count);

                List<InformacoesClienteInfo> lista = new List<InformacoesClienteInfo>();
                foreach (KeyValuePair<int, InformacoesClienteInfo> item in response.ListaInformacoesCliente)
                {
                    if (item.Value.SaldoDisponivel > limiteSaldoDisponivel)
                        continue;
                    if (item.Value.NrDiasNegativo == 0)
                        continue;
                    lista.Add(item.Value);
                }
                logger.InfoFormat("TotalClientes SaldoDevedor [{0}]", lista.Count);

                InformacaoClienteRequest req = new InformacaoClienteRequest();
                req.Lista = lista;
                PersistenciaDB.GravarHistoricoCliente(req);

                logger.Info("* Finalizando gravação de histórico de clientes com Saldo Devedor");
            }
            catch (Exception ex)
            {
                logger.Error("GravarHistoricoClientesSaldoDevedor: " + ex.Message, ex);
            }
        }

        private void GravarHistoricoRetroativoClientesSaldoDevedor()
        {
            logger.Info("* Iniciando gravação retroativa de histórico de clientes com Saldo Devedor");

            try
            {
                DateTime dataini = Convert.ToDateTime("2010/01/01");
                DateTime datafim = Convert.ToDateTime("2013/12/31");
                for (var dia = dataini.Date; dia.Date <= datafim.Date; dia = dia.AddDays(1))
                {
                    decimal limiteSaldoDisponivel = PersistenciaDB.ObterParametroLimiteSaldoDisponivel();

                    InformacaoClienteRequest request = new InformacaoClienteRequest();
                    request.DadosCliente = new InformacoesClienteInfo();
                    request.DadosCliente.DataMovimento = dia;

                    InformacaoClienteResponse response = new InformacaoClienteResponse();

                    response = PersistenciaDB.ObterListaClientesSaldoDevedor(request);
                    logger.InfoFormat("[{0}] TotalClientes [{1}]", dia.ToString("dd/MM/yyyy"), response.ListaInformacoesCliente.Count);

                    List<InformacoesClienteInfo> lista = new List<InformacoesClienteInfo>();
                    foreach (KeyValuePair<int, InformacoesClienteInfo> item in response.ListaInformacoesCliente)
                    {
                        if (item.Value.SaldoDisponivel > limiteSaldoDisponivel)
                            continue;
                        if (item.Value.NrDiasNegativo == 0)
                            continue;
                        lista.Add(item.Value);
                    }
                    logger.InfoFormat("[{0}] TotalClientes SaldoDevedor [{1}]", dia.ToString("dd/MM/yyyy"), lista.Count);

                    InformacaoClienteRequest req = new InformacaoClienteRequest();
                    req.Lista = lista;
                    PersistenciaDB.GravarHistoricoCliente(req);
                }

                logger.Info("* Finalizando gravação retroativa de histórico de clientes com Saldo Devedor");
            }
            catch (Exception ex)
            {
                logger.Error("GravarHistoricoClientesSaldoDevedor: " + ex.Message, ex);
            }
        }



        /*
        /// <summary>
        /// EFETUA O BACKUP DOS SALDOS CALCULADOS EM D-1.
        /// </summary>
        private void EfetuarBackupHistorico()
        {
            PersistenciaDB.EfetuarBackupHistorico();
        }
        */

        #endregion

    }
}
