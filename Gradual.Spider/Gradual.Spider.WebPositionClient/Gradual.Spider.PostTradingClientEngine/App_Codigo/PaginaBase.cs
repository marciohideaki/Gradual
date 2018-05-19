using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using log4net;
using Gradual.OMS.Library.Servicos;
using System.ServiceModel;
using Gradual.OMS.Seguranca.Lib;

namespace Gradual.Spider.PostTradingClientEngine.App_Codigo
{
    /// <summary>
    /// Delagate de Resposta de ajax delegate
    /// </summary>
    /// <returns>Retorna a string de retorno </returns>
    public delegate string ResponderAcaoAjaxDelegate();

    /// <summary>
    /// Classe base para ser usada como herança para outras classes webforms.
    /// Contém alguns métodos uteis para ser usados nas classes filhas.
    /// </summary>
    public class PaginaBase : System.Web.UI.Page
    {
        #region Atributos
        /// <summary>
        /// Atributo para armazenar métodos javascript em string que irá ser impresso para ser 
        /// executado na onload da tela
        /// </summary>
        private string _JavascriptParaRodarOnLoad = "";

        private Dictionary<string, ResponderAcaoAjaxDelegate> gAcoes;
        public const string CONST_MENSAGEM_SEM_USUARIO_LOGADO = "SEM_USUARIO_NA_SESSAO";

        public const string RESPOSTA_JA_ENVIADA_PELA_FUNCAO = "RESPOSTA_JA_ENVIADA_PELA_FUNCAO";

        public const string RESPOSTA_SESSAO_EXPIRADA = "RESPOSTA_SESSAO_EXPIRADA";

        public const string FUNCAO_CASO_NAO_HAJA_ACTION = "FUNCAO_CASO_NAO_HAJA_ACTION";

        public const string ACAO_PARA_APENAS_CARREGAR_HTML = "CARREGARHTML";

        public const string ACAO_PARA_CARREGAR_HTML_COM_DADOS = "CARREGARHTMLCOMDADOS";

        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Propriedades
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

        /// <summary>
        /// Ação do que o javascript manda para ser roteado
        /// </summary>
        public string Acao
        {
            get
            {
                return Request["Acao"];
            }
        }

        /// <summary>
        /// Código da sessão logada pela intranet
        /// </summary>
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

        /// <summary>
        /// Dados do usuário logado na intranet
        /// </summary>
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

        /// <summary>
        /// A chave é o Código do usuário, o valor é o IP
        /// </summary>
        public Dictionary<int, TransporteUsuarioLogado> UsuariosLogados
        {
            get
            {
                if (Application["UsuariosLogados"] == null)
                    Application["UsuariosLogados"] = new Dictionary<int, TransporteUsuarioLogado>();

                return (Dictionary<int, TransporteUsuarioLogado>)Application["UsuariosLogados"];
            }
        }
        #endregion

        #region Métodos
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
        /// Registra e roteia ações enviadas pelo javascript via ajax
        /// </summary>
        /// <param name="pAcoes">Array de Ações string para serem registradas na lista de açoes </param>
        /// <param name="pFuncoes">Array de funções que irão ser chamadas para as funções</param>
        public void RegistrarRespostasAjax(string[] pAcoes, ResponderAcaoAjaxDelegate[] pFuncoes)
        {
            if (pAcoes.Length != pFuncoes.Length)
                throw new Exception("O número de ações tem que ser equivalente ao de funções");

            gAcoes = new Dictionary<string, ResponderAcaoAjaxDelegate>();

            for (int a = 0; a < pAcoes.Length; a++) if (!gAcoes.ContainsKey(pAcoes[a]))
                    gAcoes.Add(pAcoes[a].ToLower(), pFuncoes[a]);

            this.RotearRespostaAjax();
        }

        /// <summary>
        /// Método que rotea o método chamado pelo javascript
        /// </summary>
        private void RotearRespostaAjax()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Acao))
                {
                    string lAcao, lResposta;

                    lAcao = this.Acao.ToLower();

                    if (lAcao == ACAO_PARA_APENAS_CARREGAR_HTML.ToLower()) return;

                    if (lAcao == ACAO_PARA_CARREGAR_HTML_COM_DADOS.ToLower() && gAcoes.ContainsKey(lAcao))
                    {
                        gAcoes[lAcao]();

                        return;
                    }
                    else
                    {
                        lResposta = "Sem Resposta";

                        Response.Clear();

                        if (gAcoes.ContainsKey(lAcao))
                        {
                            lResposta = gAcoes[lAcao]();

                            if (lResposta != RESPOSTA_JA_ENVIADA_PELA_FUNCAO)
                                Response.Write(lResposta);
                        }
                        else
                        {
                            Response.Write(RetornarErroAjax(string.Format("Ação não registrada: [{0}]", this.Acao)));
                        }

                        if (lResposta != RESPOSTA_JA_ENVIADA_PELA_FUNCAO)
                            Response.End();
                    }
                }
                else
                {
                    if (gAcoes.ContainsKey(FUNCAO_CASO_NAO_HAJA_ACTION))
                        gAcoes[FUNCAO_CASO_NAO_HAJA_ACTION]();
                }
            }
        }

        /// <summary>
        /// Retorna erro ajax para o javascript
        /// </summary>
        /// <param name="pMensagem">Mensagem de erro a ser enviada.                  </param>
        /// <param name="pParams">Parametros de para enviar como retorno.            </param>
        /// <returns>Retorna uma string serializada em json para resposta com o erro.</returns>
        public string RetornarErroAjax(string pMensagem, params object[] pParams)
        {
            return JsonConvert.SerializeObject(new ChamadaAjaxResponse(true, pMensagem, pParams));
        }

        /// <summary>
        /// Retorna sucesso ajax para o javascript
        /// </summary>
        /// <param name="pMensagem">Mensagem de sucesso a ser enviada.                  </param>
        /// <param name="pParams">Parametros de para enviar como retorno.               </param>
        /// <returns>Retorna uma string serializada em json para resposta com o sucesso.</returns>
        public string RetornarSucessoAjax(object pObjetoDeRetorno, string pMensagem, params object[] pParams)
        {
            return JsonConvert.SerializeObject(new ChamadaAjaxResponse(pMensagem, pParams) { ObjetoDeRetorno = pObjetoDeRetorno });
        }

        /// <summary>
        /// Método para instanciar serviços 
        /// </summary>
        /// <typeparam name="T">Tipo do Serviço a instanciar</typeparam>
        /// <returns>Retorna o objeto do serviço instanciado</returns>
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
                gLogger.Error("Erro no método InstanciarServico<T> da página base", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Faz o Abort channel do serviço usado como parametro
        /// </summary>
        /// <param name="pServico">Serviço a ser abortado</param>
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
                    gLogger.ErrorFormat("Erro ao dispor serviço: [{0}] [{1}]", pServico.GetType(), ex.Message);
                }
            }
        }

        /// <summary>
        /// Verifica se o usuário está logado
        /// </summary>
        /// <param name="pCodigoDoUsuario"></param>
        /// <param name="pIP"></param>
        /// <returns></returns>
        public bool UsuarioJaLogado(int pCodigoDoUsuario, string pIP)
        {
            if (ConfiguracoesValidadas.IgnorarLoginDeOutrasMaquinas)
                return false;

            if (this.UsuariosLogados.ContainsKey(pCodigoDoUsuario))
            {
                //se já passou mais de 30 minutos do último login, assume esse:
                if (new TimeSpan(DateTime.Now.Ticks - this.UsuariosLogados[pCodigoDoUsuario].Data.Ticks).TotalMinutes > ConfiguracoesValidadas.TimeoutUsuarioJaLogadoMin)
                {
                    this.UsuariosLogados[pCodigoDoUsuario].Data = DateTime.Now;

                    this.UsuariosLogados[pCodigoDoUsuario].IP = pIP;
                }

                if (this.UsuariosLogados[pCodigoDoUsuario].IP != pIP)
                {
                    return true;
                }
            }
            else
            {
                this.UsuariosLogados.Add(pCodigoDoUsuario, new TransporteUsuarioLogado(pCodigoDoUsuario, pIP));
            }

            return false;
        }
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();

                var lUrl = Request.Url.AbsoluteUri;

                //if (this.UsuarioLogado == null)
                //{
                //    Response.Clear();

                //    Response.Write(RetornarErroAjax(CONST_MENSAGEM_SEM_USUARIO_LOGADO));

                //    Response.End();
                //}

                //if (!Request.CurrentExecutionFilePath.ToLower().Equals("/loginteste.aspx"))
                //if (this.UsuarioLogado == null)
                //{
                //    throw new Exception(MENSAGEM_SEM_USUARIO_LOGADO);
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
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}