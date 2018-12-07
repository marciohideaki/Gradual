using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Servicos.BancoDeDados.Negocio;
using Gradual.OMS.Email.Lib;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Persistencia;
using Gradual.Site.DbLib.Dados;
using Gradual.Site.Www.Resc.UserControls;
using log4net;
using Newtonsoft.Json;
using System.Text;
using Gradual.OMS.Seguranca.Lib;
using Gradual.Site.DbLib;
using Gradual.Site.DbLib.Widgets;

namespace Gradual.Site.Www
{
    public class UserControlBase : System.Web.UI.UserControl
    {
        #region Globais

        public const string CONST_RESPOSTA_JA_ENVIADA_PELA_FUNCAO = "RESPOSTA_JA_ENVIADA_PELA_FUNCAO";

        public const string CONST_FUNCAO_CASO_NAO_HAJA_ACTION = "FUNCAO_CASO_NAO_HAJA_ACTION";

        public const string CONST_MENSAGEM_SEM_USUARIO_LOGADO = "SEM_USUARIO_NA_SESSAO";

        public const string CONST_ERRO_ASSINATURA_NAO_CONFERE = "Assinatura não confere";

        public const int CONST_ID_ASSESSOR_DELA = 305;

        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _JavascriptParaRodarOnLoad = "";
        #endregion

        #region Propriedades
        //public string JavascriptParaRodarOnLoad
        //{
        //    get { return this._JavascriptParaRodarOnLoad; }

        //    set
        //    {
        //        this._JavascriptParaRodarOnLoad += value;
        //    }
        //}
        public string Acao
        {
            get
            {
                return Request["Acao"];
            }
        }

        public PaginaBase PaginaBase
        {
            get
            {
                return (PaginaBase)this.Page;
            }
        }

        public PaginaInternaMaster PaginaMaster
        {
            get { return (PaginaInternaMaster)this.Page.Master; }
        }

        public bool PassoDoCadastro { get; set; }

        public TransporteSessaoClienteLogado SessaoClienteLogado
        {
            get { return (TransporteSessaoClienteLogado)this.Session["ClienteLogado"]; }
            set { this.Session["ClienteLogado"] = value; }
        }

        #endregion

        #region Métodos Private

        public T InstanciarServicoDoAtivador<T>()
        {
            if (Application["ServicosCarregados"] == null)
            {
                if (!ServicoHostColecao.Default.Servicos.ContainsKey(string.Format("{0}-", typeof(T))))
                    ServicoHostColecao.Default.CarregarConfig("Desenvolvimento");

                Application["ServicosCarregados"] = true;
            }

            return Ativador.Get<T>();
        }

        protected WidgetBase InstanciarWidget(WidgetInfo pWidgetInfo)
        {
            Regex lRegex = new Regex("\"Tipo\".*?.*\"");

            Match lMatch = lRegex.Match(pWidgetInfo.WidgetJson);

            if (!string.IsNullOrEmpty(lMatch.Value))
            {
                string lTipo = lMatch.Value;

                lTipo = lTipo.Substring(lTipo.IndexOf(":"));
                lTipo = lTipo.Replace("\"", "").Replace(",", "").Replace(":", "");

                WidgetBase.TipoWidget tipo = (WidgetBase.TipoWidget)Enum.Parse(typeof(WidgetBase.TipoWidget), lTipo);

                switch (tipo)
                {
                    case WidgetBase.TipoWidget.Imagem:

                        widImagem lWidgetImagem = JsonConvert.DeserializeObject<widImagem>(pWidgetInfo.WidgetJson);

                        lWidgetImagem.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetImagem.IdDoWidget = pWidgetInfo.CodigoWidget;

                        return lWidgetImagem;

                    case WidgetBase.TipoWidget.Titulo:

                        widTitulo lWidgetTitulo = JsonConvert.DeserializeObject<widTitulo>(pWidgetInfo.WidgetJson);

                        lWidgetTitulo.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetTitulo.IdDoWidget = pWidgetInfo.CodigoWidget;

                        return lWidgetTitulo;

                    case WidgetBase.TipoWidget.Lista:

                        widLista lWidgetLista = JsonConvert.DeserializeObject<widLista>(pWidgetInfo.WidgetJson);

                        lWidgetLista.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetLista.IdDoWidget = pWidgetInfo.CodigoWidget;
                        lWidgetLista.IdDaLista = pWidgetInfo.CodigoListaConteudo;

                        return lWidgetLista;

                    case WidgetBase.TipoWidget.Texto:

                        widTexto lWidgetTexto = JsonConvert.DeserializeObject<widTexto>(pWidgetInfo.WidgetJson);

                        lWidgetTexto.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetTexto.IdDoWidget = pWidgetInfo.CodigoWidget;

                        return lWidgetTexto;

                    case WidgetBase.TipoWidget.Tabela:

                        widTabela lWidgetTabela = JsonConvert.DeserializeObject<widTabela>(pWidgetInfo.WidgetJson);

                        lWidgetTabela.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetTabela.IdDoWidget = pWidgetInfo.CodigoWidget;
                        lWidgetTabela.IdDaLista = pWidgetInfo.CodigoListaConteudo;

                        return lWidgetTabela;

                    case WidgetBase.TipoWidget.Repetidor:

                        widRepetidor lWidgetRepetidor = JsonConvert.DeserializeObject<widRepetidor>(pWidgetInfo.WidgetJson);

                        lWidgetRepetidor.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetRepetidor.IdDoWidget = pWidgetInfo.CodigoWidget;
                        lWidgetRepetidor.IdDaLista = pWidgetInfo.CodigoListaConteudo;

                        return lWidgetRepetidor;

                    case WidgetBase.TipoWidget.ListaDeDefinicao:

                        widListaDeDefinicao lWidgetListaDeDefinicao = JsonConvert.DeserializeObject<widListaDeDefinicao>(pWidgetInfo.WidgetJson);

                        lWidgetListaDeDefinicao.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetListaDeDefinicao.IdDoWidget = pWidgetInfo.CodigoWidget;
                        lWidgetListaDeDefinicao.IdDaLista = pWidgetInfo.CodigoListaConteudo;

                        return lWidgetListaDeDefinicao;

                    case WidgetBase.TipoWidget.TextoHTML:

                        widTextoHTML lWidgetTextoHTML = JsonConvert.DeserializeObject<widTextoHTML>(pWidgetInfo.WidgetJson);

                        lWidgetTextoHTML.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetTextoHTML.IdDoWidget    = pWidgetInfo.CodigoWidget;
                        lWidgetTextoHTML.IdDaLista     = pWidgetInfo.CodigoListaConteudo;

                        return lWidgetTextoHTML;

                    default:
                        break;
                }
            }
            return null;
        }

        #endregion

        #region Métodos Públicos

        protected string ToCodigoClienteFormatado(object pObject)
        {
            int lDigito = 0;

            int lCodigoCorretora = 227;

            lDigito = (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(1 - 1, 1)) * 5)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(2 - 1, 1)) * 4)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(1 - 1, 1)) * 3)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(2 - 1, 1)) * 2)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(3 - 1, 1)) * 9)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(4 - 1, 1)) * 8)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(5 - 1, 1)) * 7)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(3 - 1, 1)) * 6)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(4 - 1, 1)) * 5)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(5 - 1, 1)) * 4)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(6 - 1, 1)) * 3)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(7 - 1, 1)) * 2);

            lDigito = lDigito % 11;

            if (lDigito == 0 || lDigito == 1)
            {
                lDigito = 0;
            }
            else
            {
                lDigito = 11 - lDigito;
            }

            return lDigito.ToString();
        }

        public void PopularControleComListaDoSinacor(eInformacao pLista, params Repeater[] pControles)
        {
            SinacorListaInfo lInfo = new SinacorListaInfo(pLista);

            List<SinacorListaInfo> lLista = this.BuscarListaDoSinacor(lInfo);

            foreach (Repeater lControle in pControles)
            {
                lControle.DataSource = lLista;
                lControle.DataBind();
            }
        }

        public void PopularControleComListaDoSinacor(eInformacao pLista, params DropDownList[] pControles)
        {
            SinacorListaInfo lInfo = new SinacorListaInfo(pLista);

            List<SinacorListaInfo> lLista = this.BuscarListaDoSinacor(lInfo);

            foreach (DropDownList lControle in pControles)
            {
                lControle.DataSource = lLista;
                lControle.DataBind();
            }
        }

        public void PopularControleComListaDoSinacor(eInformacao pLista, params HtmlSelect[] pControles)
        {
            SinacorListaInfo lInfo = new SinacorListaInfo(pLista);

            List<SinacorListaInfo> lLista = this.BuscarListaDoSinacor(lInfo);

            foreach (HtmlSelect lControle in pControles)
            {
                lLista.ForEach(lDadoSinacor => { lControle.Items.Add(new ListItem() { Text = lDadoSinacor.Value, Value = lDadoSinacor.Id }); });
            }
        }

        public List<SinacorListaInfo> BuscarListaDoSinacor(SinacorListaInfo pConfiguracaoInfo)
        {
            string lKey = string.Concat(pConfiguracaoInfo.Informacao.ToString(), this.GetType().ToString());

            List<SinacorListaInfo> lRetorno = new List<SinacorListaInfo>();

            if (Cache[lKey] == null)
            {
                ConsultarEntidadeCadastroRequest<SinacorListaInfo> lRequest;
                ConsultarEntidadeCadastroResponse<SinacorListaInfo> lResponse;

                lRequest = new ConsultarEntidadeCadastroRequest<SinacorListaInfo>() { EntidadeCadastro = pConfiguracaoInfo };

                lResponse = this.PaginaBase.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<SinacorListaInfo>(lRequest);

                lRetorno = lResponse.Resultado;

                Cache[lKey] = lRetorno;
            }
            else
            {
                lRetorno = Cache[lKey] as List<SinacorListaInfo>;
            }

            return lRetorno;
        }

        public void RetirarClienteDaSessao()
        {
            this.PaginaBase.SessaoClienteLogado = null;

            Session["RedirecionamentoPorFaltaDeLogin"] = null;

            Session["TipoDeClientePreview"] = null;
        }

        public void ValidarSessao()
        {
            if (this.PaginaBase.SessaoClienteLogado == null)
            {
                Session["RedirecionamentoPorFaltaDeLogin"] = Request.Url.PathAndQuery;

                this.Response.Redirect( this.PaginaBase.HostERaizFormat("MinhaConta/Login.aspx"));
            }
            else if (this.PassoDoCadastro)
            {
                switch (this.PaginaBase.SessaoClienteLogado.Passo)
                {
                    case 0:

                        if (this.Request.Url.OriginalString.Contains("Cadastro_PF_Passo1.aspx")) break;
                        
                        Response.Redirect(this.PaginaBase.HostERaizFormat("MinhaConta/Cadastro/Cadastro_PF_Passo1.aspx"));

                        break;

                    case 1:

                        if (this.Request.Url.OriginalString.Contains("Cadastro_PF_Passo2.aspx")) break;
                        
                        Response.Redirect(this.PaginaBase.HostERaizFormat("MinhaConta/Cadastro/Cadastro_PF_Passo2.aspx"));

                        break;

                    case 2:

                        if (this.Request.Url.OriginalString.Contains("Cadastro_PF_Passo3.aspx")) break;
                        
                        Response.Redirect(this.PaginaBase.HostERaizFormat("MinhaConta/Cadastro/Cadastro_PF_Passo3.aspx"));

                        break;

                    case 3:

                        if (!VerificarClienteComSuitability())
                        {
                            if (this.Request.Url.OriginalString.Contains("Cadastro_PF_Passo4.aspx")) break;
                            Response.Redirect(this.PaginaBase.HostERaizFormat("MinhaConta/Cadastro/Cadastro_PF_Passo4.aspx"));
                        }
                        else
                        {
                            if (this.Request.Url.OriginalString.Contains("Cadastro_PF_Passo5.aspx")) break;
                            Response.Redirect(this.PaginaBase.HostERaizFormat("MinhaConta/Cadastro/Cadastro_PF_Passo5.aspx"));
                        }

                        break;
                }
            }
        }

        private bool VerificarClienteComSuitability()
        {
            ConsultarEntidadeCadastroRequest<ClienteSuitabilityInfo> lRequest = new ConsultarEntidadeCadastroRequest<ClienteSuitabilityInfo>();
            ConsultarEntidadeCadastroResponse<ClienteSuitabilityInfo> lResponse;

            lRequest.DescricaoUsuarioLogado = string.Concat("Tela de Login do Portal - IP: ", Request.UserHostAddress);
            lRequest.EntidadeCadastro       = new ClienteSuitabilityInfo() { IdCliente = this.PaginaBase.SessaoClienteLogado.IdCliente.Value };

            lResponse = this.PaginaBase.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteSuitabilityInfo>(lRequest);

            return (lResponse.Resultado.Count > 0);
        }

        protected void CarregarItemUnico(RenderizadorDeWidgets pRenderizador, string pIdConteudo, string pPropriedadeParaRenderizar = "ConteudoHTML")
        {

        }

        public void ExibirMensagemJsOnLoad(string pTipo_IAE, string pMensagem, bool pRetornarAoEstadoNormalAposSegundos = false, string pMensagemAdicional = "")
        {
            string lMensagem, lMensagemExtendida;

            lMensagem = SanitarStringPraJavascript(pMensagem);

            lMensagemExtendida = SanitarStringPraJavascript(pMensagemAdicional);

            /*
            if (lMensagem.Length > 180)
            {
                lMensagemExtendida = string.Format("{0}\\n\\n{1}", lMensagem, lMensagemExtendida);

                lMensagem = lMensagem.Substring(0, 180) + "(...)";
            }
            else
            {
            }
            */

            lMensagemExtendida = string.Format("{0}\\n\\n{1}", lMensagem, lMensagemExtendida);

            this.RodarJavascriptOnLoad(string.Format("GradSite_ExibirMensagem(\"{0}\", \"{1}\", {2}{3});"
                                                    , pTipo_IAE
                                                    , lMensagem
                                                    , pRetornarAoEstadoNormalAposSegundos ? "true" : "false"
                                                    , string.IsNullOrEmpty(lMensagemExtendida) ? "" : ", \"" + lMensagemExtendida + "\""));
        }

        private string SanitarStringPraJavascript(string pString)
        {
            if (string.IsNullOrEmpty(pString)) return "";

            return pString.Replace('"', '\'').Replace("\n", "\\n").Replace("\r", "");
        }

        /// <summary>
        /// Roda javascript depois do load da página; o código fica dentro de uma função, então não definir outras funções por aqui, só executar código.
        /// </summary>
        /// <param name="pJavascript">Javascript pra rodar dentro da função Page_Load_Codebehind()</param>
        public void RodarJavascriptOnLoad(string pJavascript)
        {
            ((PaginaBase)this.Page).JavascriptParaRodarOnLoad+= pJavascript + "\r\n\r\n";
        }

        public bool ValidarAssinaturaEletronica(string pAssinaturaEletronica)
        {
            try
            {
                ReceberEntidadeCadastroRequest<LoginAssinaturaEletronicaInfo> lRequest = new ReceberEntidadeCadastroRequest<LoginAssinaturaEletronicaInfo>();
                ReceberEntidadeCadastroResponse<LoginAssinaturaEletronicaInfo> lResponse;

                lRequest.EntidadeCadastro = new LoginAssinaturaEletronicaInfo();

                lRequest.EntidadeCadastro.CdAssinaturaEletronica = pAssinaturaEletronica;
                lRequest.EntidadeCadastro.DsEmail = SessaoClienteLogado.Email;

                lResponse = InstanciarServicoDoAtivador<IServicoPersistenciaCadastro>().ReceberEntidadeCadastro<LoginAssinaturaEletronicaInfo>(lRequest);

                if (lResponse.EntidadeCadastro.IdLogin != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro [{0}] em ValidarAssinaturaEletronica()\r\n {1}"
                                    , ex.Message
                                    , ex.StackTrace);

                return false;
            }
        }
        #endregion

        #region Event Handlers

        protected void Page_Init(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #endregion
    }
}