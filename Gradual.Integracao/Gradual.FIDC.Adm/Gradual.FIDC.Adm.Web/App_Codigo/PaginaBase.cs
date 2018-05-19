using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Seguranca.Lib;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.UI.WebControls;
using Gradual.FIDC.Adm.Web.App_Codigo;
using Gradual.FIDC.Adm.DbLib.Mensagem;
using Gradual.FIDC.Adm.DbLib.Persistencia;
using Gradual.OMS.Email.Lib;
using System.Configuration;

namespace Gradual.FIDC.Adm.Web
{
    public delegate string ResponderAcaoAjaxDelegate();

    public class PaginaBase : System.Web.UI.Page
    {
        #region Propriedades
        /// <summary>
        /// Objeto de Logger das página filhas
        /// </summary>
        public static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<string, ResponderAcaoAjaxDelegate> gAcoes;

        public const string CONST_RESPOSTA_JA_ENVIADA_PELA_FUNCAO = "RESPOSTA_JA_ENVIADA_PELA_FUNCAO";

        public const string CONST_FUNCAO_CASO_NAO_HAJA_ACTION = "funcao_caso_nao_haja_action";

        public const string CONST_MENSAGEM_SEM_USUARIO_LOGADO = "SEM_USUARIO_NA_SESSAO";

        public const string CONST_ERRO_ASSINATURA_NAO_CONFERE = "Assinatura não confere";

        public const string RESPOSTA_SESSAO_EXPIRADA = "RESPOSTA_SESSAO_EXPIRADA";

        /// <summary>
        /// Atributo para armazenar métodos javascript em string que irá ser impresso para ser 
        /// executado na onload da tela
        /// </summary>
        private string _JavascriptParaRodarOnLoad = "";

        /// <summary>
        /// Propriedade de Get/Set que irá gerenciar o javascript em string que irá ser 
        /// impresso para ser executado no evento onload da tela
        /// </summary>
        public string JavascriptParaRodarOnLoad
        {
            get { return this._JavascriptParaRodarOnLoad; }

            set
            {
                this._JavascriptParaRodarOnLoad = value;
            }
        }

        private string PastaVirtual
        {
            get { return HttpContext.Current.Request.ApplicationPath; }
        }

        public string CodigoSessao
        {
            get
            {
                if (Session["CodigoSessao"] != null)
                {
                    return Convert.ToString(Session["CodigoSessao"]);
                }
                else
                {
                    return null;
                    //throw new Exception(CONST_MENSAGEM_SEM_USUARIO_LOGADO);
                }
            }
            set
            {
                Session["CodigoSessao"] = value;
            }
        }

        public string Acao
        {
            get
            {
                return Request["Acao"];
            }
        }

        public string SkinEmUso
        {
            get
            {
                if (Session["SkinEmUso"] == null)
                {
                    Session["SkinEmUso"] = ConfiguracoesValidadas.SkinPadrao;
                }

                return (string)Session["SkinEmUso"];
            }

            set
            {
                Session["SkinEmUso"] = value;
            }
        }

        public string VersaoDoSite
        {
            get
            {
                return ConfiguracoesValidadas.VersaoDoSite;
            }
        }

        public Usuario UsuarioLogado
        {
            get
            {
                if (Session["UsuarioLogado"] != null)
                {
                    return (Usuario)Session["UsuarioLogado"];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                Session["UsuarioLogado"] = value;
            }
        }

        public Dictionary<int, string> UsuariosLogados
        {
            get
            {
                if (Application["UsuariosLogados"] == null)
                    Application["UsuariosLogados"] = new Dictionary<int, string>();

                return (Dictionary<int, string>)Application["UsuariosLogados"];
            }
        }

        private string _LinkPreSelecionado = "";

        public string LinkPreSelecionado
        {
            get
            {
                return _LinkPreSelecionado;
            }

            set
            {
                _LinkPreSelecionado = value;

                ((System.Web.UI.HtmlControls.HtmlInputHidden)((Gradual.FIDC.Adm.Web.Principal)this.Master).FindControl("hidLinkPreSelecionado")).Value = _LinkPreSelecionado;
            }
        }

        public string TituloDaPagina
        {
            get
            {
                return ((Gradual.FIDC.Adm.Web.Principal)this.Master).TituloDaPagina;
            }

            set
            {
                ((Gradual.FIDC.Adm.Web.Principal)this.Master).TituloDaPagina = value;
            }
        }


        public string HostDoSite
        {
            get
            {
                return ConfiguracoesValidadas.HostDoSite;
            }
        }

        public string RaizDoSite
        {
            get
            {
                return ConfiguracoesValidadas.RaizDoSite;
            }
        }

        public string HostERaiz
        {
            get
            {
                return string.Format("{0}{1}", this.HostDoSite, this.RaizDoSite);
            }
        }

        public string RaizURL
        {
            get
            {
                try
                {
                    return string.Format("http://{0}{1}/", HttpContext.Current.Request.ServerVariables["HTTP_HOST"],
                                        (this.PastaVirtual.Equals("/")) ? string.Empty : this.PastaVirtual);
                }
                catch
                {
                    return null;
                }
            }
        }

        public IServicoSeguranca ServicoSeguranca
        {
            get
            {
                //return this.InstanciarServico<IServicoSeguranca>();

                IServicoSeguranca lRetorno;

                lRetorno = Ativador.Get<IServicoSeguranca>();

                return lRetorno;
            }
        }
        #endregion

        #region Métodos Públicos

        

        public string HostERaizFormat(string pStringPraAdicionar)
        {
            if (pStringPraAdicionar.StartsWith("/"))
                pStringPraAdicionar = pStringPraAdicionar.TrimStart('/');

            string lRetorno = string.Format("{0}/{1}", this.HostERaiz, pStringPraAdicionar);

            if (!lRetorno.ToLower().Contains("localhost"))
            {
                if (lRetorno.ToLower().Contains("minhaconta/"))
                {
                    lRetorno = lRetorno.Replace("http:", "https:");
                }
            }

            return lRetorno;
        }

        public bool ValidarSessao()
        {
            if (this.UsuarioLogado == null)
            {
                Session["RedirecionamentoPorFaltaDeLogin"] = Request.Url.PathAndQuery;

                this.Response.Redirect(HostERaizFormat("Login.aspx"));

                return false;
            }

            return true;
        }


        public T InstanciarServico<T>()
        {
            T lRetorno = default(T);

            if (ServicoHostColecao.Default.Servicos.Count == 0 ||
                !ServicoHostColecao.Default.Servicos.ContainsKey(string.Format("{0}-", typeof(T))))
            {
                ServicoHostColecao.Default.CarregarConfig(ConfiguracoesValidadas.TipoDeObjetoAtivador);
            }


            try
            {
                lRetorno = Ativador.Get<T>();
            }
            catch (CommunicationObjectFaultedException)
            {
                Ativador.AbortChannel(lRetorno);
            }
            catch (Exception ex)
            {
                Logger.Error("Erro no método InstanciarServico<T> da página base", ex);
            }

            return lRetorno;
        }

        public void DisporServico(object pServico)
        {
            if (pServico != null)
            {
                try
                {
                    Ativador.AbortChannel(pServico);
                }
                catch (Exception ex)
                {
                    Logger.ErrorFormat("Erro ao dispor serviço: [{0}] [{1}]", pServico.GetType(), ex.Message);
                }
            }
        }

        public bool ValidarAssinaturaEletronica(string pAssinaturaEletronica)
        {
            try
            {
                IServicoSeguranca lServicoSeguranca = InstanciarServico<IServicoSeguranca>();

                MensagemResponseBase lResponse = lServicoSeguranca.ValidarAssinaturaEletronica(new ValidarAssinaturaEletronicaRequest()
                {
                    AssinaturaEletronica = Criptografia.CalculateMD5Hash(pAssinaturaEletronica),

                    CodigoSessao = UsuarioLogado.CodigoDaSessao,
                });

                DisporServico(lServicoSeguranca);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Erro [{0}] em PaginaBase.ValidarAssinaturaEletronica()\r\n    >>Stack:\r\n{1}"
                                    , ex.Message
                                    , ex.StackTrace);

                return false;
            }
        }

        public string RetornarErroAjax(string pMensagem, string[] pListaDeMensagens)
        {
            string lMensagens = "";

            foreach (string lMensagem in pListaDeMensagens)
            {
                lMensagens += Environment.NewLine + lMensagem;
            }

            return JsonConvert.SerializeObject(new RespostaAjax(true, pMensagem, lMensagens));
        }

        public string RetornarErroAjax(string pMensagem, params object[] pParams)
        {
            return JsonConvert.SerializeObject(new RespostaAjax(true, pMensagem, pParams));
        }

        public string RetornarErroAjax(string pMensagem, Exception pErro, params object[] pParams)
        {
            return JsonConvert.SerializeObject(new RespostaAjax(pMensagem, pErro, pParams));
        }

        public string RetornarErroAjax(string pMensagem, string pMensagemExtendida)
        {
            return JsonConvert.SerializeObject(new RespostaAjax(true, pMensagem) { MensagemExtendida = pMensagemExtendida });
        }

        public string RetornarErroAjax(string pMensagem, object pObjetoDeRetorno)
        {
            return JsonConvert.SerializeObject(new RespostaAjax(true, pMensagem) { ObjetoDeRetorno = pObjetoDeRetorno });
        }

        public string RetornarSucessoAjax(string pMensagem, params object[] pParams)
        {
            return JsonConvert.SerializeObject(new RespostaAjax(pMensagem, pParams));
        }

        public string RetornarSucessoAjax(object pObjetoDeRetorno, string pMensagem, params object[] pParams)
        {
            return JsonConvert.SerializeObject(new RespostaAjax(pMensagem, pParams) { ObjetoDeRetorno = pObjetoDeRetorno });
        }

        public void RegistrarRespostasAjax(string[] pAcoes, ResponderAcaoAjaxDelegate[] pFuncoes)
        {
            if (pAcoes.Length != pFuncoes.Length)
                throw new Exception("O número de ações tem que ser equivalente ao de funções");

            gAcoes = new Dictionary<string, ResponderAcaoAjaxDelegate>();

            for (int a = 0; a < pAcoes.Length; a++)
            {
                if (!gAcoes.ContainsKey(pAcoes[a]))
                    gAcoes.Add(pAcoes[a].ToLower(), pFuncoes[a]);
            }

            RotearRespostaAjax();
        }

        private void RotearRespostaAjax()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Acao))
                {
                    string lAcao, lResposta;

                    lAcao = this.Acao.ToLower();
                    lResposta = "Sem Resposta";

                    Response.Clear();

                    if (this.UsuarioLogado == null)
                    {
                        Response.Write(RetornarErroAjax(CONST_MENSAGEM_SEM_USUARIO_LOGADO));
                    }
                    else
                    {
                        if (gAcoes.ContainsKey(lAcao))
                        {
                            lResposta = gAcoes[lAcao]();

                            if (lResposta != CONST_RESPOSTA_JA_ENVIADA_PELA_FUNCAO)
                                Response.Write(lResposta);

                            if (lAcao == "registrarordem" ||
                                lAcao == "cancelarordem" ||
                                lAcao == "registrarstartstop" ||
                                lAcao == "cancelarordemstopstart" ||
                                lAcao == "confirmartermostopstart")
                            {
                                string lLog = "Response de Processos.aspx -> ";

                                lLog += string.Format(" IP: [{0}], Resposta: ", Request.UserHostAddress);

                                lLog += string.Format("[{0}]", lResposta);

                                Logger.Info(lLog);
                            }
                        }
                        else
                        {
                            Response.Write(RetornarErroAjax("Ação não registrada: [{0}]", this.Acao));
                        }
                    }


                    if (lResposta != CONST_RESPOSTA_JA_ENVIADA_PELA_FUNCAO)
                    {
                        if (Request.Url.AbsolutePath.Contains("CentralizadorDeRespostas"))
                        {
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            try { Response.End(); }
                            catch { }
                        }
                    }
                }
                else
                {
                    if (gAcoes.ContainsKey(CONST_FUNCAO_CASO_NAO_HAJA_ACTION))
                        gAcoes[CONST_FUNCAO_CASO_NAO_HAJA_ACTION]();
                }
            }
        }

        public string AbreviarNumero(object pNumero)
        {
            double lNumero;

            if (double.TryParse(pNumero.ToString(), out lNumero))
            {
                return lNumero.ToNumeroAbreviado();
            }
            else
            {
                return pNumero.ToString();
            }
        }

        public string AbreviarNumero(double pNumero)
        {
            return pNumero.ToNumeroAbreviado();
        }

        public string AbreviarNumero(int pNumero)
        {
            return pNumero.ToNumeroAbreviado();
        }

        public string RealizarLogout(string pCodigoDaSessao)
        {
            IServicoSeguranca lServicoSeguranca = this.InstanciarServico<IServicoSeguranca>();

            MensagemResponseBase lResponse = lServicoSeguranca.EfetuarLogOut(new MensagemRequestBase() { CodigoSessao = pCodigoDaSessao });

            DisporServico(lServicoSeguranca);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                this.UsuariosLogados.Remove(UsuarioLogado.IdDoUsuarioTipoInt);

                Session.Clear();

                return RetornarSucessoAjax("ok");
            }
            else
            {
                return RetornarErroAjax(lResponse.DescricaoResposta);
            }
        }

        public bool UsuarioJaLogado(int pCodigoDoUsuario, string pIP)
        {
            if (this.UsuariosLogados.ContainsKey(pCodigoDoUsuario))
            {
                if (this.UsuariosLogados[pCodigoDoUsuario] != pIP)
                {
                    return true;
                }
            }
            else
            {
                this.UsuariosLogados.Add(pCodigoDoUsuario, pIP);
            }

            return false;
        }

        #endregion

        #region Event Handlers

        protected void Page_Init(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();

            /*
            string lUrl = Request.Url.AbsoluteUri;

            if ( (lUrl.Contains("Janelas/") || lUrl.Contains("Relatorios/")) && this.UsuarioLogado == null)
            {
                Response.Clear();

                Response.Write(RetornarErroAjax(CONST_MENSAGEM_SEM_USUARIO_LOGADO));

                Response.End();
            }
            */

            //if (!Request.CurrentExecutionFilePath.ToLower().Equals("/loginteste.aspx"))
            //if (this.UsuarioLogado == null)
            //{
            //    throw new Exception(MENSAGEM_SEM_USUARIO_LOGADO);
            //}

            //if (this.UsuarioJaLogado == null)
            //{
            //    Response.Redirect("/Login")
            //}

            /*De: André Miguel
             *Data: 02/09/2010
             *
             *Não comentar e nem apagar esta chamada.
             *Esse application está sendo usado no global.asax, 
             *pois lá não concigo executar o serviço de segurança chamando o Ativador.Get direto, 
             *pois o mesmo devolve o erro "The server has rejected the client credentials.".
             */
            if (Application["ServicoSeguranca"] == null)
                Application["ServicoSeguranca"] = InstanciarServico<IServicoSeguranca>();
        }

        /// <summary>
        /// Método que é chamado para preenchimento da propriedade que 
        /// gerência os métodos javascript em string
        /// </summary>
        /// <param name="pJavascript"></param>
        public void RodarJavascriptOnLoad(string pJavascript)
        {
            this.JavascriptParaRodarOnLoad += pJavascript + "\r\n\r\n";
        }

        /// <summary>
        /// Evento de Load Complete da página
        /// </summary>
        /// <param name="sender">Não está sendo usado</param>
        /// <param name="e">Não está sendo usado</param>
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (((Principal)this.Master) != null)
            {
                Literal litJavascript = ((Principal)this.Master).FindControl("litJavascriptOnLoad") as Literal;

                //if (litJavascript == null && this.PaginaMaster != null)
                //    litJavascript = (Literal)this.PaginaMaster.FindControl("litJavascriptOnLoad");

                if (litJavascript != null)
                    litJavascript.Text = this.JavascriptParaRodarOnLoad;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.UsuarioLogado == null)
            {
                if (string.IsNullOrEmpty(this.Acao))
                {
                    this.RedirecionarPara("Default.aspx");
                    //Server.Transfer("Login.aspx");
                }
                else
                {
                    this.Response.Clear();

                    this.Response.Write(this.RetornarErroAjax(RESPOSTA_SESSAO_EXPIRADA));

                    this.Response.End();
                }
            }
            else
            {
                ReceberSessaoResponse lResSessao = ServicoSeguranca.ReceberSessao(new ReceberSessaoRequest()
                {
                    CodigoSessao = this.CodigoSessao,
                    CodigoSessaoARetornar = this.CodigoSessao
                });

                if (!lResSessao.Sessao.EhSessaoDeAdministrador)
                {
                    object[] attrs = this.GetType().GetCustomAttributes(typeof(ValidarSegurancaAttribute), true);

                    if (attrs.Length > 0)
                    {
                        List<ItemSegurancaInfo> list = new List<ItemSegurancaInfo>();

                        list.Add(((ValidarSegurancaAttribute)attrs[0]).Seguranca);
                        ValidarItemSegurancaRequest lRequestSeguranca = new ValidarItemSegurancaRequest()
                        {
                            CodigoSessao = this.CodigoSessao,
                            ItensSeguranca = list
                        };

                        try
                        {
                            ValidarItemSegurancaResponse lResponseSeguranca = this.ServicoSeguranca.ValidarItemSeguranca(lRequestSeguranca);

                            if (lResponseSeguranca.StatusResposta == MensagemResponseStatusEnum.OK)
                            {
                                if (!lResponseSeguranca.ItensSeguranca[0].Valido.Value)
                                {   //--> Acesso Negado
                                    this.Response.Clear();
                                    this.Response.End();
                                }
                            }
                            else
                            {
                                this.Response.Clear();
                                this.Response.End();
                            }
                        }
                        //catch (CommunicationObjectFaultedException)
                        //{
                        //    Ativador.AbortChannel(this.ServicoSeguranca);
                        //    this.ServicoSeguranca = Ativador.Get<IServicoSeguranca>();
                        //}
                        catch (System.Threading.ThreadAbortException)
                        {

                        }
                        catch (Exception ex)
                        {
                            this.Response.Clear();

                            this.Response.Write(this.RetornarErroAjax(ex.Message));

                            this.Response.End();
                        }
                    }
                }
            }
        }


        #endregion

        #region Métodos das páginas
        /// <summary>
        /// Método
        /// </summary>
        /// <param name="pURLRelativa"></param>
        public void RedirecionarPara(string pURLRelativa)
        {
            Response.Redirect(this.RaizURL + pURLRelativa);
        }

        /// <summary>
        /// Método de Buscar carteiras da tela de robo de downloads
        /// </summary>
        /// <param name="pRequest">Request de carteiras</param>
        /// <returns>Retorna um objeto de </returns>
        public CarteiraResponse BuscarCarteiras(CarteiraRequest pRequest)
        {
            try
            {
                var lServico = new RoboDownloadDB();

                return lServico.BuscarCarteiras(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Extrato de Cotista no banco de dados
        /// </summary>
        /// <param name="pRequest">Request de Extrato de cotista</param>
        /// <returns>Retorna um objeto de Lista de extrato de cotista</returns>
        public ExtratoCotistaResponse BuscarExtratoCotista(ExtratoCotistaRequest pRequest)
        {
            try
            {
                var lServico = new RoboDownloadDB();

                return lServico.BuscarExtratoCotista(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Buscar Mec no banco de dados
        /// </summary>
        /// <param name="pRequest">Request de Mapa de evolução de cotas</param>
        /// <returns>Retorna um objeto de Mapa de evolução de cotista</returns>
        public MecResponse BuscarMec(MecRequest pRequest)
        {
            try
            {
                var lServico = new RoboDownloadDB();

                return lServico.BuscarMec(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Buscar Títulos Liquidado no banco de dados
        /// </summary>
        /// <param name="pRequest">Request de Títulos Liquidados</param>
        /// <returns>Retorna um objeto de Títulos Liquidados</returns>
        public TitulosLiquidadosResponse BuscarTitulosLiquidados(TitulosLiquidadosRequest pRequest)
        {
            try
            {
                var lServico = new RoboDownloadDB();

                return lServico.BuscarTitulosLiquidados(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lista de Fundos 
        /// </summary>
        /// <returns>REtorna um alista de fundos de objetos ListaFundosInfo</returns>
        public ExtratoCotistaResponse BuscarListaFundos()
        {
            try
            {
                var lServico = new RoboDownloadDB();

                return lServico.BuscarListaFundos();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lista de Cotistas
        /// </summary>
        /// <returns>REtorna um objeto com uma lista de cotistas</returns>
        public ExtratoCotistaResponse BuscarListaCotistas()
        {
            try
            {
                var lServico = new RoboDownloadDB();

                return lServico.BuscarListaCotistas();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Listar titulos Liquidados de ADM
        /// </summary>
        /// <param name="pRequest">Request</param>
        /// <returns>Retorna Lista titulos Liquidados</returns>
        public TitulosLiquidadosResponse BuscarTitulosLiquidadosADM(TitulosLiquidadosRequest pRequest)
        {
            try
            {
                var lServico = new RoboDownloadDB();

                return lServico.BuscarTitulosLiquidadosADM(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Atualiza Valores de Títulos Liquidado no banco de dados ADM
        /// </summary>
        /// <param name="pRequest">Request de Títulos Liquidados</param>
        /// <returns>Retorna um objeto de Títulos Liquidados ADM</returns>
        public TitulosLiquidadosResponse AplicarValorTitulosLiquidadosADM(TitulosLiquidadosRequest pRequest)
        {
            try
            {
                var lServico = new RoboDownloadDB();

                return lServico.AplicarValorTitulosLiquidadosADM(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método de atualização e inserção de fundos
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public CadastroFundoResponse AtualizarCadastroFundo(CadastroFundoRequest pRequest)
        {
            try
            {
                #region Verificar duplicidade

                //o nome do fundo deve ser único na base de dados
                CadastroFundoDB db = new CadastroFundoDB();
                
                CadastroFundoRequest reqVerificarDuplicidade = new CadastroFundoRequest();
                reqVerificarDuplicidade.NomeFundo = pRequest.NomeFundo;

                CadastroFundoResponse respVerificarDuplicidade = db.Buscar(reqVerificarDuplicidade);

                bool duplicidade = false;

                if (respVerificarDuplicidade.ListaFundos.Count > 0 && respVerificarDuplicidade.ListaFundos[0].IdFundoCadastro != pRequest.IdFundoCadastro)
                    duplicidade = true;
                
                #endregion

                //caso não haja duplicidade, prossegue a atualização
                if (!duplicidade)
                {
                    CadastroFundoResponse response = new CadastroFundoResponse();

                    var lServico = new CadastroFundoDB();

                    //seta o tipo da transação realizada
                    string tipoTransacao = "";

                    if (pRequest.IdFundoCadastro > 0) //atualizar fundo já cadastrado
                    {                        
                        response = lServico.Atualizar(pRequest);
                        tipoTransacao = "UPDATE";
                    }
                    else //cadastrar novo fundo
                    {
                        // ao inserir um novo fundo, o mesmo deve sempre estar ativo
                        pRequest.IsAtivo = true;
                        response = lServico.Inserir(pRequest);
                        tipoTransacao = "INSERT";
                    }

                    //grava a transação na tabela de log
                    lServico.InserirLog(pRequest, tipoTransacao, pRequest.DescricaoUsuarioLogado);
                    
                    return response;
                }
                else
                {
                    //throw new Exception("Já existe um fundo com o mesmo nome cadastrado na base");
                    CadastroFundoResponse response = new CadastroFundoResponse();
                    response.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroNegocio;
                    response.DescricaoResposta = "Já existe um fundo com o mesmo nome cadastrado na base";

                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Inclui evento do calendario
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public CalendarioEventoResponse IncluirCalendarioEvento(CalendarioEventoRequest pRequest)
        {
            try
            {
                CalendarioEventoResponse response = new CalendarioEventoResponse();

                var lServico = new CalendarioEventoDB();

                response = lServico.Inserir(pRequest);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Remove evento do calendario
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public CalendarioEventoResponse RemoverCalendarioEvento(CalendarioEventoRequest pRequest)
        {
            try
            {
                CalendarioEventoResponse response = new CalendarioEventoResponse();

                var lServico = new CalendarioEventoDB();

                response = lServico.Remover(pRequest);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método de busca de fundos cadastrados
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public CadastroFundoResponse BuscarFundosCadastrados(CadastroFundoRequest pRequest)
        {
            try
            {
                var lServico = new CadastroFundoDB();

                return lServico.Buscar(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método de busca de eventos cadastrados
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public CalendarioEventoResponse BuscarCalendarioEventos(CalendarioEventoRequest pRequest)
        {
            try
            {
                var lServico = new CalendarioEventoDB();

                return lServico.Buscar(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Busca categorias de fundos cadastradas
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public FundoCategoriaResponse BuscarFundoCategorias(FundoCategoriaRequest pRequest)
        {
            try
            {
                var lServico = new FundoCategoriaDB();

                return lServico.Buscar(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FundoSubCategoriaResponse BuscarFundoSubCategorias(FundoSubCategoriaRequest pRequest)
        {
            try
            {
                var lServico = new FundoSubCategoriaDB();

                return lServico.Buscar(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FundoCategoriaSubCategoriaResponse BuscarFundosPorCategoriaSubCategoria(FundoCategoriaSubCategoriaRequest pRequest)
        {
            try
            {
                var lServico = new FundoCategoriaSubCategoriaDB();

                return lServico.BuscarFundosPorCategoriaXSubCategoria(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Busca dados para carregamento do grid do modal de fundos
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public FundoCategoriaSubCategoriaResponse BuscarFundosCarregarGridModalFundos(FundoCategoriaSubCategoriaRequest pRequest)
        {
            try
            {
                var lServico = new FundoCategoriaSubCategoriaDB();

                return lServico.BuscarFundosPorCategoriaXSubCategoriaTodos(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Adiciona um relacionamento fundo x categoria x subcategoria
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public FundoCategoriaSubCategoriaResponse AdicionarRelacionamentosFundosCategoriasSubCategorias(FundoCategoriaSubCategoriaRequest pRequest)
        {
            try
            {
                var lServico = new FundoCategoriaSubCategoriaDB();

                return lServico.Inserir(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Remove um relacionamento fundo x categoria x subcategoria
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public FundoCategoriaSubCategoriaResponse RemoverRelacionamentosFundosCategoriasSubCategorias(FundoCategoriaSubCategoriaRequest pRequest)
        {
            try
            {
                var lServico = new FundoCategoriaSubCategoriaDB();

                return lServico.Remover(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insere um log de inserção ou remoção de uma relação Fundo x Categoria x SubCategoria
        /// </summary>
        /// <param name="pRequest"></param>
        public FundoCategoriaSubCategoriaLogResponse InserirLogFundoCategoriaSubCategoria(FundoCategoriaSubCategoriaLogRequest pRequest)
        {
            try
            {
                var lServico = new FundoCategoriaSubCategoriaDB();

                return lServico.InserirLog(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Busca as etapas do fluxo de aprovação parametrizadas
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public FundoFluxoGrupoEtapaResponse BuscarEtapasFluxoAprovacaoPorGrupo(FundoFluxoGrupoEtapaRequest pRequest)
        {
            try
            {
                var lServico = new FundoFluxoGrupoEtapaDB();

                return lServico.Buscar(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Busca os status parametrizados das etapas do fluxo de aprovação
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public FundoFluxoStatusResponse BuscarStatusFluxoAprovacao(FundoFluxoStatusRequest pRequest)
        {
            try
            {
                var lServico = new FundoFluxoStatusDB();

                return lServico.Buscar(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insere uma etapa do fluxo de aprovação de um fundo específico
        /// </summary>
        /// <param name="pRequest"></param>
        public FundoFluxoAprovacaoResponse InserirEtapaFluxoAprovacaoFundo(FundoFluxoAprovacaoRequest pRequest)
        {
            try
            {
                var lServico = new FundoFluxoAprovacaoDB();

                return lServico.Inserir(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Busca os status parametrizados das etapas do fluxo de aprovação
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public FundoFluxoAprovacaoResponse BuscarEtapasAprovacaoFundo(FundoFluxoAprovacaoRequest pRequest)
        {
            try
            {
                var lServico = new FundoFluxoAprovacaoDB();

                return lServico.BuscarEtapasPorFundo(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insere um anexo de uma etapa do fluxo de aprovação de um fundo
        /// </summary>
        /// <param name="pRequest"></param>
        public FundoFluxoAprovacaoAnexoResponse InserirAnexoEtapaFluxoAprovacao(FundoFluxoAprovacaoAnexoRequest pRequest)
        {
            var lServico = new FundoFluxoAprovacaoAnexoDB();

            return lServico.Inserir(pRequest);
        }

        /// <summary>
        /// Método de busca de fundos cadastrados por categoria
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public CadastroFundoResponse BuscarFundosCadastradosPorCategoria(CadastroFundoRequest pRequest)
        {
            var lServico = new CadastroFundoDB();

            return lServico.BuscarPorCategoria(pRequest);
        }

        /// <summary>
        /// Método de busca da consulta de fundos em constituição
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public ConsultaFundosConstituicaoResponse BuscarFundosConsultaFundosConstituicao(ConsultaFundosConstituicaoRequest pRequest)
        {
            try
            {
                var lServico = new ConsultaFundosConstituicaoDB();

                return lServico.Buscar(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Busca grupos do fluxo de aprovação parametrizadas
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public FundoFluxoGrupoResponse BuscarGruposFluxoAprovacao(FundoFluxoGrupoRequest pRequest)
        {
            var lServico = new FundoFluxoGrupoDB();

            return lServico.Buscar();
        }

        /// <summary>
        /// Busca dados gerais da última etapa processada do fundo
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public ConsultaFundosConstituicaoResponse BuscarFundosConsultaFundosConstituicaoDadosGeraisUltimaEtapa(ConsultaFundosConstituicaoRequest pRequest)
        {
            var lServico = new ConsultaFundosConstituicaoDB();

            return lServico.BuscarDadosGeraisFundo(pRequest);
        }
        
        /// <summary>
        /// Envia e-mail com informações de aprovação de um fundo de investimento
        /// </summary>
        /// <param name="pDestinatarios"></param>
        /// <param name="pCorpoEmail"></param>
        /// <param name="pAnexos"></param>
        /// <returns></returns>
        public MensagemResponseStatusEnum EnviarEmailEtapasAprovacaoFundoConstituicao(List<string> pDestinatarios, string pCorpoEmail, List<EmailAnexoInfo> pAnexos = null)
        {
            var lAtivador = Ativador.Get<IServicoEmail>();

            if (lAtivador != null)
            {
                var lEmailEntrada = new EnviarEmailRequest();
                lEmailEntrada.Objeto = new EmailInfo();
                lEmailEntrada.Objeto.Assunto = ConfigurationManager.AppSettings["AssuntoEmailConsultaFundosConstituicao"].ToString();
                lEmailEntrada.Objeto.Destinatarios = pDestinatarios;
                lEmailEntrada.Objeto.Remetente = ConfigurationManager.AppSettings["RemetenteEmailConsultaFundosConstituicao"].ToString();
                lEmailEntrada.Objeto.CorpoMensagem = pCorpoEmail;

                var lEmailRetorno = lAtivador.Enviar(lEmailEntrada);

                return lEmailRetorno.StatusResposta;
            }

            throw new Exception("Ativador nulo ao enviar email. Provável erro de configuração, verificar entradas para 'TipoDeObjetoAtivador' e seção de config para 'IServicoEmail'");
        }

        public CadastroCotistasFidcResponse SelecionarListaCotistasFidc(CadastroCotistasFidcRequest pRequest)
        {
            var lServico = new CotistasFidcDb();

            return lServico.SelecionarLista(pRequest);
        }

        public CadastroCotistasFidcResponse AtualizarCadastroCotista(CadastroCotistasFidcRequest pRequest)
        {
            try
            {

                var response = new CadastroCotistasFidcResponse();

                var lServico = new CotistasFidcDb();

                if (pRequest.IdCotistaFidc > 0) //atualizar fundo já cadastrado
                {
                    response = lServico.Atualizar(pRequest);
                }
                else //cadastrar novo fundo
                {
                    response = lServico.Inserir(pRequest);
                }

                return response;

                /*else
                {
                    //throw new Exception("Já existe um fundo com o mesmo nome cadastrado na base");
                    var response = new CadastroCotistasFidcResponse();
                    response.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroNegocio;
                    response.DescricaoResposta = "Já existe um cotista com o mesmo documento cadastrado na base";

                    return response;
                }*/
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AssociacaoCotistaFidcFundoResponse InserirAssociacaoCotistaFundo(AssociacaoCotistaFidcFundoRequest pRequest)
        {
            var lServico = new CotistaFidcFundoDb();

            return lServico.Inserir(pRequest);
        }

        public AssociacaoCotistaFidcFundoResponse SelecionarGridCotistaFidcFundo(AssociacaoCotistaFidcFundoRequest pRequest)
        {
            var lServico = new CotistaFidcFundoDb();

            return lServico.SelecionarListaGrid(pRequest);
        }

        public AssociacaoCotistaFidcFundoResponse RemoverAssociacaoCotistaFundo(AssociacaoCotistaFidcFundoRequest pRequest)
        {
            var lServico = new CotistaFidcFundoDb();

            return lServico.Remover(pRequest);
        }

        public FundoCadastroAnexoResponse InserirAnexoCadastroFundo(FundoCadastroAnexoRequest pRequest)
        {
            var lServico = new FundoCadastroAnexoDb();

            return lServico.Inserir(pRequest);
        }

        public OrigemResponse BuscarOrigens()
        {
            try
            {
                var lServico = new RoboDownloadDB();

                return lServico.BuscarOrigens();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CotistaFidcProcuradorResponse InserirCotistaFidcProcurador(CotistaFidcProcuradorRequest request)
        {
            var lServico = new CotistaFidcProcuradorDb();

            return lServico.Inserir(request);
        }

        public CotistaFidcProcuradorResponse SelecionarCotistaFidcProcurador(CotistaFidcProcuradorRequest request)
        {
            var lServico = new CotistaFidcProcuradorDb();

            return lServico.SelecionarLista(request);
        }

        public CotistaFidcProcuradorResponse AtualizarCotistaFidcProcurador(CotistaFidcProcuradorRequest request)
        {
            var lServico = new CotistaFidcProcuradorDb();

            return lServico.Atualizar(request);
        }

        public CotistaFidcProcuradorResponse ExcluirCotistaFidcProcurador(CotistaFidcProcuradorRequest request)
        {
            var lServico = new CotistaFidcProcuradorDb();

            return lServico.Excluir(request);
        }

        public CotistaFidcProcuradorAnexoResponse InserirAnexoCotistaFidcProcuradorAnexo(CotistaFidcProcuradorAnexoRequest pRequest)
        {
            var lServico = new CotistaFidcProcuradorAnexoDb();

            return lServico.Inserir(pRequest);
        }

        public CotistaFidcProcuradorAnexoResponse RemoverTodosOsAnexosPorProcurador(CotistaFidcProcuradorAnexoRequest pRequest)
        {
            var lServico = new CotistaFidcProcuradorAnexoDb();

            return lServico.ExcluirPorProcurador(pRequest);
        }
        public AlteracaoRegulamentacaoConsultaFundosCarregarGridResponse ConsultarFundosGridConsultaAlteracaoRegulamentacao(AlteracaoRegulamentacaoConsultaFundosCarregarGridRequest pRequest)
        {
            var lServico = new AlteracaoRegulamentacaoFundosConsultaDb();

            return lServico.ObterLista(pRequest);
        }
        public AlteracaoRegulamentacaoConsultaFundosCarregarListaGruposResponse ObterListaDropDownGruposFluxoAlteracaoRegulamento()
        {
            var lServico = new FluxoAlteracaoRegulamentoGrupoDb();

            return lServico.ObterLista();
        }
        public AlteracaoRegulamentacaoCarregarDadosModalEnvioEmailResponse AlteracaoRegulamentacaoCarregarDadosModalEnvioEmail(int pIdFundoCadastro)
        {
            var lServico = new AlteracaoRegulamentacaoFundosConsultaDb();

            return lServico.BuscarDadosGeraisEmailFundo(pIdFundoCadastro);
        }
        public FundoFluxoAlteracaoRegulamentoResponse BuscarEtapasAlteracaoRegulamentoFundo(FundoFluxoAlteracaoRegulamentoRequest pRequest)
        {
            var lServico = new FundoFluxoAlteracaoRegulamentoDb();

            return lServico.BuscarEtapasPorFundo(pRequest);
        }
        public FluxoAlteracaoRegulamentoStatusResponse BuscarStatusFluxoAltRegulamento(int pIdFluxoAlteracaoRegulamentoStatus)
        {
            var lServico = new FluxoAlteracaoRegulamentoStatusDb();

            return lServico.Buscar(pIdFluxoAlteracaoRegulamentoStatus);
        }
        public FluxoAlteracaoRegulamentoGrupoEtapaResponse BuscarEtapasFluxoAltRegulamentoPorGrupo(FluxoAlteracaoRegulamentoGrupoEtapaRequest pRequest)
        {
            try
            {
                var lServico = new FluxoAlteracaoRegulamentoGrupoEtapaDb();

                return lServico.Buscar(pRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public FundoFluxoAlteracaoRegulamentoResponse InserirEtapaFluxoAlteracaoRegulamento(FundoFluxoAlteracaoRegulamentoRequest pRequest)
        {
            var lServico = new FundoFluxoAlteracaoRegulamentoDb();

            return lServico.Inserir(pRequest);
        }
        public FundoFluxoAlteracaoRegulamentoAnexoResponse InserirAnexoEtapaFluxoAlteracaoRegulamento(FundoFluxoAlteracaoRegulamentoAnexoRequest pRequest)
        {
            var lServico = new FundoFluxoAlteracaoRegulamentoAnexoDb();

            return lServico.Inserir(pRequest);
        }
        #endregion

    }
}