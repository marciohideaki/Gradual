using System;
using System.Web;
using System.Collections.Generic;
using System.Configuration;

using Newtonsoft.Json;


using log4net;
//using System.ServiceModel;
using System.Web.UI.WebControls;
using Gradual.Spider;
using Gradual.OMS.Seguranca.Lib;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.Spider.Lib.Dados;
using Gradual.Spider.Lib;
using Gradual.Spider.DbLib;
using System.Security.Cryptography;
using System.Text;
using Gradual.Spider.Lib.Mensagens;
using Gradual.IntranetCorp.Lib.Mensagens;
using Gradual.Spider.Www.App_Codigo;


namespace Gradual.Spider
{
    public delegate string ResponderAcaoAjaxDelegate();

    public class PaginaBase : System.Web.UI.Page
    {

        #region Propriedades
        private string _JavascriptParaRodarOnLoad = "";

        private Dictionary<string, ResponderAcaoAjaxDelegate> gAcoes;

        public const string RESPOSTA_JA_ENVIADA_PELA_FUNCAO = "RESPOSTA_JA_ENVIADA_PELA_FUNCAO";

        public const string CONST_RESPOSTA_JA_ENVIADA_PELA_FUNCAO = "RESPOSTA_JA_ENVIADA_PELA_FUNCAO";

        public const string CONST_FUNCAO_CASO_NAO_HAJA_ACTION = "funcao_caso_nao_haja_action";

        public const string CONST_MENSAGEM_SEM_USUARIO_LOGADO = "SEM_USUARIO_NA_SESSAO";

        public const string RESPOSTA_SESSAO_EXPIRADA = "RESPOSTA_SESSAO_EXPIRADA";

        //public const string CONST_ERRO_ASSINATURA_NAO_CONFERE = "Assinatura não confere";
        public Gradual.Spider.Www.App_Codigo.Usuario UsuarioLogado
        {
            get
            {
                return (Gradual.Spider.Www.App_Codigo.Usuario)Session["Usuario"];
            }
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


        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
        #endregion

        #region Métodos Públicos

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
            return JsonConvert.SerializeObject(new RespostaAjax(true, pMensagem) { ObjetoDeRetorno = pObjetoDeRetorno});
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

                    lResposta = gAcoes[lAcao]();

                    if (lResposta != CONST_RESPOSTA_JA_ENVIADA_PELA_FUNCAO)
                    {
                        Response.Write(lResposta);

                        if (Request.Url.AbsolutePath.Contains("CentralizadorDeRespostas"))
                        {
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            try {   Response.End();  }  catch { }
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

            if(double.TryParse(pNumero.ToString(), out lNumero))
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

        public void RegistrarLogInclusao(string pDsObservacao = null)
        {
            var lDb = new LogSpiderDbLib();

            lDb.RegistrarLog(
                   new LogSpiderInfo()
                {
                    DsIp         = this.Request.UserHostAddress,
                    DsObservacao = pDsObservacao,
                    DsTela       = this.Request.FilePath,
                    DtEvento     = DateTime.Now,
                    IdAcao       = TipoAcaoUsuario.Inclusao,
                    IdLogin      = this.UsuarioLogado.Id,
                });
        }

        public void RegistrarLogAlteracao(string pDsObservacao = null)
        {
            var lDb = new LogSpiderDbLib();

            lDb.RegistrarLog(
                new LogSpiderInfo() 
                {
                    DsIp         = this.Request.UserHostAddress,
                    DsObservacao = pDsObservacao,
                    DsTela       = this.Request.FilePath,
                    DtEvento     = DateTime.Now,
                    IdAcao       = TipoAcaoUsuario.Edicao,
                    IdLogin      = this.UsuarioLogado.Id,
                });
        }

        public void RegistrarLogConsulta(string pDsObservacao)
        {
            var lDb = new LogSpiderDbLib();

            lDb.RegistrarLog(
                new LogSpiderInfo()
                {
                    DsIp         = this.Request.UserHostAddress,
                    DsObservacao = pDsObservacao,
                    DsTela       = this.Request.FilePath,
                    DtEvento     = DateTime.Now,
                    IdAcao       = TipoAcaoUsuario.Consulta,
                    IdLogin      = this.UsuarioLogado.Id,
                });
        }

        public void RegistrarLogExclusao(string pDsObservacao)
        {
            var lDb = new LogSpiderDbLib();

            lDb.RegistrarLog(
                new LogSpiderInfo()
                {
                    DsIp         = this.Request.UserHostAddress,
                    DsObservacao = pDsObservacao,
                    DsTela       = this.Request.FilePath,
                    DtEvento     = DateTime.Now,
                    IdAcao       = TipoAcaoUsuario.Exclusao,
                    IdLogin      = this.UsuarioLogado.Id,
                });
        }

        public List<SinacorListaInfo> ListarSinacor(SinacorListaInfo pParametro)
        {
            var lDb = new AssessoresDbLib();

            return lDb.ConsultarListaSinacor(pParametro);
        }

        public List<AssessorInfo> ListarAssessorComplemento(AssessorInfo pParamentros)
        {
            var lDb = new AssessoresDbLib();

            return lDb.ListarAssessorComplemento(pParamentros);
        }

        public EsqueciSenhaInfo ReceberEsqueciSenha(EsqueciSenhaInfo pParametros)
        {
            var lDb = new EsqueciSenhaDbLib();

            return lDb.ReceberEsqueciSenha(pParametros);
        }

        public void RodarJavascriptOnLoad(string pJavascript)
        {
            this.JavascriptParaRodarOnLoad += pJavascript + "\r\n\r\n";
        }

        public void RedirecionarPara(string pURLRelativa)
        {
            Response.Redirect(this.RaizURL + pURLRelativa);
        }

        public Dictionary<int, string> ListarSessaoFix()
        {
            var lDb = new AssessoresDbLib();

            return lDb.ListarSessaoFix();
        }

        public Dictionary<int, string> ListarLocalidadeAssessor()
        {
            var lDb = new AssessoresDbLib();

            return lDb.ListarLocalidadeAssessor();
        }
        

        public AssessorInfo InserirAtualizarAssessorComplemento(AssessorInfo pParametros)
        {
            var lDb = new AssessoresDbLib();

            return lDb.InserirAtualizarAssessorComplemento(pParametros);
        }

        public ClienteInfo BuscarCliente(ClienteInfo pParametros)
        {
            var lDb = new ClienteDbLib();

            return lDb.BuscarCliente(pParametros);
        }

        public ClienteInfo InserirCliente(ClienteInfo pParametros)
        {
            var lDb = new ClienteDbLib();

            return lDb.InserirCliente(pParametros);
        }

        public string CompilarCriticas(List<string> pCriticas)
        {
            string lRetorno = string.Empty;

            pCriticas.ForEach(critica => 
            {
                lRetorno += critica + "</ br>";
            });

            return lRetorno;
        }

        public RiscoListarPermissoesResponse ListarPermissoesRisco(RiscoListarPermissoesRequest pParametro)
        {
            var lRetorno = new RiscoListarPermissoesResponse();

            var lDb = new RiscoLimiteDbLib();

            lRetorno = lDb.ListarPermissoesRisco(pParametro);

            return lRetorno;

        }
        public RiscoListarPermissoesClienteResponse ListarPermissoesRiscoClienteSpider(RiscoListarPermissoesClienteRequest pParametro)
        {
            var lRetorno = new RiscoListarPermissoesClienteResponse();

            var lDb = new RiscoLimiteDbLib();

            lRetorno = lDb.ListarPermissoesRiscoClienteSpider(pParametro);

            return lRetorno;
        }

        public RiscoListarParametrosClienteResponse ListarLimitePorClienteSpider(RiscoListarParametrosClienteRequest pParametro)
        {
            var lRetorno = new RiscoListarParametrosClienteResponse();

            var lDb = new RiscoLimiteDbLib();

            lRetorno = lDb.ListarLimitePorClienteSpider(pParametro);

            return lRetorno;
        }

        public List<PlataformaSessaoInfo> ListarPlataforma(PlataformaSessaoInfo pParametros)
        {
            var lRetorno = new List<PlataformaSessaoInfo>();

            var lDb = new PlataformaSessaoDbLib();

            lRetorno = lDb.ListarPlataforma(pParametros);

            return lRetorno;
        }

        public List<PlataformaSessaoInfo> ListarSessao(PlataformaSessaoInfo pParametros)
        {
            var lRetorno = new List<PlataformaSessaoInfo>();

            var lDb = new PlataformaSessaoDbLib();

            lRetorno = lDb.ListarSessao(pParametros);

            return lRetorno;
        }

        public PlataformaSalvarResponse InserirPlataformaSessao(List<PlataformaSessaoInfo> pParametros)
        {
            var lRetorno = new PlataformaSalvarResponse();

            var lDb = new PlataformaSessaoDbLib();

            lRetorno = lDb.InserirPlataformaSessao(pParametros);

            return lRetorno;
        }

        public GerenciadorPlataformaSalvarResponse InserirGerenciadorPlataforma(List<GerenciadorPlataformaInfo> pParametros)
        {
            var lRetorno = new GerenciadorPlataformaSalvarResponse();

            var lDb = new GerenciadorPlataformaDbLib();

            lRetorno = lDb.InserirGerenciadorPlataforma(pParametros);

            return lRetorno;
        }

        public List<PlataformaSessaoInfo> ListarPlataformaSessao(PlataformaSessaoInfo pParametros)
        {
            var lRetorno = new List<PlataformaSessaoInfo>();

            var lDb = new PlataformaSessaoDbLib();

            lRetorno = lDb.ListarPlataformaSessao(pParametros);

            return lRetorno;
        }

        public List<PlataformaSessaoInfo> SelecionarPlataformaSessao(PlataformaSessaoInfo pParametros)
        {
            var lRetorno = new List<PlataformaSessaoInfo>();

            var lDb = new PlataformaSessaoDbLib();

            lRetorno = lDb.SelecionarPlataformaSessao(pParametros);

            return lRetorno;
        }

        public List<GerenciadorPlataformaInfo> SelecionarGerenciadorPlataforma(GerenciadorPlataformaInfo pParametros)
        {
            var lRetorno = new List<GerenciadorPlataformaInfo>();

            var lDb = new GerenciadorPlataformaDbLib();

            lRetorno = lDb.SelecionarGerenciadorPlataforma(pParametros);

            return lRetorno;
        }

        public List<GerenciadorPlataformaInfo> ListarGerenciadorPlataforma(GerenciadorPlataformaInfo pParametros)
        {
            var lRetorno = new List<GerenciadorPlataformaInfo>();

            var lDb = new GerenciadorPlataformaDbLib();

            lRetorno = lDb.ListarGerenciadorPlataforma(pParametros);

            return lRetorno;
        }

        public ListarUsuariosResponse ListarUsuarioOperadores()
        {
            var lRetorno = new ListarUsuariosResponse();

            var lRequest = new ListarUsuariosRequest();
            
            lRequest.CodigoSessao = this.CodigoSessao;

            ListarUsuariosResponse lResponse = ServicoSeguranca.ListarUsuarios(lRequest);

            lRetorno = lResponse;

            return lRetorno;
        }

         

        public string CalculateMD5Hash(string input)
        {
            try
            {
                //Primeiro passo, validar o input
                if (string.IsNullOrEmpty(input))
                    input = "gradual123*";

                // Segundo passo, calcular o MD5 hash a partir da string
                MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

                byte[] hash = md5.ComputeHash(inputBytes);

                // Terceiro passo, converter o array de bytes em uma string haxadecimal
                StringBuilder _HashBuilder = new StringBuilder();

                for (int intX = 0; intX < hash.Length; intX++)
                {
                    _HashBuilder.Append(hash[intX].ToString("X2"));
                }
                return _HashBuilder.ToString();

            }

            catch
            {
                throw new Exception("Ocorreu um erro ao Criptografar a senha.");

            }
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

            
            /*De: André Miguel
             *Data: 02/09/2010
             *
             *Não comentar e nem apagar esta chamada.
             *Esse application está sendo usado no global.asax, 
             *pois lá não concigo executar o serviço de segurança chamando o Ativador.Get direto, 
             *pois o mesmo devolve o erro "The server has rejected the client credentials.".
             */
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (this.CodigoSessao == null)
            //{
            //    string lScript = "GradSpider_TratarRespostaComErro({ TemErro: true, Mensagem: \"RESPOSTA_SESSAO_EXPIRADA\" });";

            //    this.RodarJavascriptOnLoad(lScript);
            //}

            if (this.UsuarioLogado == null)
            {
                if (string.IsNullOrEmpty(this.Acao))
                {
                    this.RedirecionarPara("Login.aspx");
                    //Server.Transfer("Login.aspx");
                }
                else
                {
                    this.Response.Clear();

                    this.Response.Write(this.RetornarErroAjax(RESPOSTA_SESSAO_EXPIRADA));

                    //string lScript = "GradSpider_TratarRespostaComErro({ TemErro: true, Mensagem: \"RESPOSTA_SESSAO_EXPIRADA\" });";

                    //this.RodarJavascriptOnLoad(lScript);

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

        public Gradual.Spider.Www.Spider PaginaMaster
        {
            get { return (Gradual.Spider.Www.Spider)this.Master; }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Literal litJavascript = (Literal)this.FindControl("litJavascriptOnLoad");

            if (litJavascript == null && this.PaginaMaster != null)
                litJavascript = (Literal)this.PaginaMaster.FindControl("litJavascriptOnLoad");

            if (litJavascript != null)
                litJavascript.Text = this.JavascriptParaRodarOnLoad;
        }

        #endregion
    }
}
