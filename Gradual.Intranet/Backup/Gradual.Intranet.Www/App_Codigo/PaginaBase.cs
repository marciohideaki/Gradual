using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.OMS.AcompanhamentoOrdens.Lib;
using Gradual.OMS.Email.Lib;
using Gradual.OMS.Interface;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Persistencia;
using Gradual.OMS.Risco.Regra.Lib;
using Gradual.OMS.Seguranca.Lib;
using log4net;
using Newtonsoft.Json;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using System.Linq;
using Gradual.Intranet.Www.App_Codigo;
using System.Collections;


namespace Gradual.Intranet
{
    public delegate string ResponderAcaoAjaxDelegate();

    public class PaginaBase : System.Web.UI.Page
    {
        #region | Atributos

        public static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<string, ResponderAcaoAjaxDelegate> gAcoes;

        public const string RESPOSTA_JA_ENVIADA_PELA_FUNCAO = "RESPOSTA_JA_ENVIADA_PELA_FUNCAO";

        public const string RESPOSTA_SESSAO_EXPIRADA = "RESPOSTA_SESSAO_EXPIRADA";

        public const string FUNCAO_CASO_NAO_HAJA_ACTION = "FUNCAO_CASO_NAO_HAJA_ACTION";

        public const string ACAO_PARA_APENAS_CARREGAR_HTML = "CARREGARHTML";

        public const string ACAO_PARA_CARREGAR_HTML_COM_DADOS = "CARREGARHTMLCOMDADOS";

        #endregion

        #region | Propriedades

        public string Acao
        {
            get
            {
                return Request["Acao"];
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

        public string SkinEmUso
        {
            get
            {
                return ConfiguracoesValidadas.SkinEmUso;
            }
        }

        public string VersaoDoSite
        {
            get
            {
                return ConfiguracoesValidadas.VersaoDoSite;
            }
        }

        public IServicoPersistenciaCadastro ServicoPersistenciaCadastro
        {
            get
            {
                if (Application["ServicoPersistenciaCadastro"] == null)
                    Application["ServicoPersistenciaCadastro"] = BuscarServicoDoAtivador<IServicoPersistenciaCadastro>();

                return (IServicoPersistenciaCadastro)Application["ServicoPersistenciaCadastro"];
            }
        }

        public IServicoPersistencia ServicoPersistencia
        {
            get
            {
                if (Application["ServicoPersistencia"] == null)
                    Application["ServicoPersistencia"] = BuscarServicoDoAtivador<IServicoPersistencia>();

                return (IServicoPersistencia)Application["ServicoPersistencia"];
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

        public IServicoInterface ServicoInterface
        {
            get
            {
                if (Application["ServicoInterface"] == null)
                    Application["ServicoInterface"] = BuscarServicoDoAtivador<IServicoInterface>();

                return (IServicoInterface)Application["ServicoInterface"];
            }
        }

        public IServicoRegrasRisco ServicoRegrasRisco
        {
            get
            {
                if (Application["ServicoRegrasRisco"] == null)
                    Application["ServicoRegrasRisco"] = BuscarServicoDoAtivador<IServicoRegrasRisco>();

                return (IServicoRegrasRisco)Application["ServicoRegrasRisco"];
            }
        }

        public IServicoAcompanhamentoOrdens ServicoAcompanhamentoOrdens
        {
            get
            {
                if (Application["ServicoAcompanhamentoOrdens"] == null)
                    Application["ServicoAcompanhamentoOrdens"] = BuscarServicoDoAtivador<IServicoAcompanhamentoOrdens>();

                return (IServicoAcompanhamentoOrdens)Application["ServicoAcompanhamentoOrdens"];
            }

            set
            {
                Application["ServicoAcompanhamentoOrdens"] = value;
            }
        }

        public string CodigoSessao
        {
            get
            {
                return Convert.ToString(Session["CodigoSessao"]);
            }
            set
            {
                Session["CodigoSessao"] = value;
            }
        }
        /// <summary>
        /// Código de conta corrente do cliente.
        /// </summary>
        public Usuario UsuarioLogado
        {
            get
            {
                return (Usuario)Session["Usuario"];
            }
        }

        public int? GetIdAssessorFilialLogado
        {
            get
            {
                int? lRetorno = null;

                if (this.CodigoAssessor != null)
                {
                    var lServico = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<AssessorFilialInfo>(new ReceberEntidadeCadastroRequest<AssessorFilialInfo>()
                    {
                        EntidadeCadastro = new AssessorFilialInfo()
                        {
                            ConsultaCdAssessor = this.CodigoAssessor
                        }
                    });

                    if (lServico.StatusResposta == MensagemResponseStatusEnum.OK)
                        lRetorno = lServico.EntidadeCadastro.CdFilial;
                }

                return lRetorno;
            }
        }

        public int ObterDiasUteis(DateTime dataInicial, DateTime dataFinal)
        {
            int dias = 0;
            int ContadorDias = 0;

            dias = dataInicial.Date.Subtract(dataFinal).Days;

            DateTime lDataAtual = dataInicial.Date;

            if (dias < 0)
                dias = dias * -1;

            var lContFeriados = from feriado in this.ListaFeriados where feriado >= lDataAtual && feriado <= dataFinal select feriado;

            for (int i = 1; i <= dias; i++)
            {
                dataInicial = dataInicial.AddDays(1).Date;

                if ((dataInicial.DayOfWeek == DayOfWeek.Sunday || dataInicial.DayOfWeek == DayOfWeek.Saturday) && lContFeriados.Contains(dataInicial))
                {
                    ContadorDias++;
                }

                if (dataInicial.DayOfWeek != DayOfWeek.Sunday && dataInicial.DayOfWeek != DayOfWeek.Saturday)
                    ContadorDias++;
            }

            if (lContFeriados != null)
            {
                ContadorDias = ContadorDias - lContFeriados.Count();
            }

            return ContadorDias;
        }

        public double CalcularTaxaDI(string Instrumento, double taxaOperada,  double lTaxaMercado)
        {
            double Ajuste = 0;
            double Numerador = 100000;
            double NumeroDiasBase = 252;

            DateTime dtVencimento = DateTime.Parse(ListaVencimentosDI[Instrumento].ToString());

            double NumeroDiasCalculados = this.ObterDiasUteis(DateTime.Now, dtVencimento);
            double Exponencial = (NumeroDiasCalculados / NumeroDiasBase);

            double taxaMercado = lTaxaMercado;

            if (taxaOperada == taxaMercado)
            {
                return 0;
            }

            #region CALCULO PU

            double PUInicial = (Numerador / Math.Pow(((1 + (taxaOperada / 100))), Exponencial));
            double PUFinal = (Numerador / Math.Pow(((1 + (taxaMercado / 100))), Exponencial));

            Ajuste = (Math.Round(PUFinal, 2) - Math.Round(PUInicial, 2));

            #endregion

            return Math.Round(Ajuste, 2);
        }

        public Hashtable ListaVencimentosDI
        {
            get
            {
                Hashtable lVencimentos = null;

                if (Session["ListaVencimentosDI"] == null)
                {
                    ReceberEntidadeRequest<MonitoramentoRiscoLucroVencimentosDI> lRequest = new ReceberEntidadeRequest<MonitoramentoRiscoLucroVencimentosDI>();

                    ReceberObjetoResponse<MonitoramentoRiscoLucroVencimentosDI> lRetornoVencimentos = new PersistenciaDbIntranet().ReceberObjeto<MonitoramentoRiscoLucroVencimentosDI>(lRequest);

                    if (lRetornoVencimentos.Objeto != null)
                    {
                        lVencimentos = lRetornoVencimentos.Objeto.VencimentosDI;
                    }

                    if (lVencimentos.Count > 0)
                    {
                        Session.Add("ListaVencimentosDI", lVencimentos);
                    }

                }

                return Session["ListaVencimentosDI"] as Hashtable;
            }
        }


        public List<DateTime> ListaFeriados
        {
            get
            {
                List<DateTime> lFeriados = null;

                if (Session["ListaFeriados"] == null)
                {
                    ReceberEntidadeRequest<MonitorRiscoFeriadosInfo> lRequest = new ReceberEntidadeRequest<MonitorRiscoFeriadosInfo>();

                    ReceberObjetoResponse<MonitorRiscoFeriadosInfo> lRetornoFeriados = new PersistenciaDbIntranet().ReceberObjeto<MonitorRiscoFeriadosInfo>(lRequest);

                    if (lRetornoFeriados.Objeto != null)
                        lFeriados = lRetornoFeriados.Objeto.ListaFeriados;

                    if (lFeriados.Count > 0)
                    {
                        Session.Add("ListaFeriados", lFeriados);
                    }
                }

                return Session["ListaFeriados"] as List<DateTime>;
            }
        }
        /// <summary>
        /// Codigo do assessor 
        /// </summary>
        public int? CodigoAssessor
        {
            get
            {
                string lCodigoAssessor = string.Empty;

                if (Session["CodigoAssessor"] == null)
                {
                    ReceberEntidadeCadastroRequest<LoginInfo> lEntradaLogin = new ReceberEntidadeCadastroRequest<LoginInfo>();

                    lEntradaLogin.EntidadeCadastro = new LoginInfo() { IdLogin = UsuarioLogado.Id };

                    ReceberEntidadeCadastroResponse<LoginInfo> lRetornoLogin = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<LoginInfo>(lEntradaLogin);

                    if (lRetornoLogin.EntidadeCadastro != null)
                        lCodigoAssessor = lRetornoLogin.EntidadeCadastro.CdAssessor.ToString();

                    if (!string.IsNullOrEmpty(lCodigoAssessor))
                        Session.Add("CodigoAssessor", lCodigoAssessor);
                }

                if (Session["CodigoAssessor"] == null)
                    return null;
                else
                    return Convert.ToInt32(Session["CodigoAssessor"]);
            }
        }

        public bool EhAdministrador
        {
            get
            {
                Usuario lUser = Session["Usuario"] as Usuario;

                return lUser.EhAdministrador;
            }
        }

        /// <summary>
        /// Verifica se o usuário logado contêm o perfil de cadastro
        /// </summary>
        /// <returns>Retorna um boleano que identificando se o usuário contem o perfil ou não</returns>
        public bool EhPerfilCadastro
        {
            get
            {
                bool lRetorno = false;

                Usuario lUsuario = Session["Usuario"] as Usuario;

                if (lUsuario.Perfis != null && lUsuario.Perfis.Count != 0)
                {
                    if (lUsuario.Perfis.Contains("4"))
                    {
                        lRetorno = true;
                    }
                }

                return lRetorno;
            }
        }

        public bool ExibirApenasAssessorAtual
        {
            get
            {
                var lRetorno = default(bool);


                var lPerfilUsuario = this.ServicoSeguranca.ListarUsuarios(new ListarUsuariosRequest() { FiltroCodigoUsuario = this.UsuarioLogado.Id.ToString() });

                lRetorno = (lPerfilUsuario != null
                        && (lPerfilUsuario.Usuarios != null)
                        && (lPerfilUsuario.Usuarios.Count > 0)
                        && (lPerfilUsuario.Usuarios[0].CodigoTipoAcesso == (int)eTipoAcesso.Assessor));

                return lRetorno;
            }
        }

        #endregion

        #region | Eventos

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //De: André Miguel
            //Data: 02/09/2010
            //
            //Não comentar e nem apagar esta chamada.
            //Esse application está sendo usado no global.asax, 
            //pois lá não concigo executar o serviço de segurança chamando o Ativador.Get direto, 
            //pois o mesmo devolve o erro "The server has rejected the client credentials.".
            if (Application["ServicoSeguranca"] == null)
                Application["ServicoSeguranca"] = BuscarServicoDoAtivador<IServicoSeguranca>();
        }

        #endregion

        #region | Métodos Private

        private T BuscarServicoDoAtivador<T>()
        {
            String lTipo;

            if (Application["ServicosCarregados"] == null)
            {
                if (!ServicoHostColecao.Default.Servicos.ContainsKey(string.Format("{0}-", typeof(T))))
                {
                    lTipo = String.Format("{0}",typeof(T));
                    ServicoHostColecao.Default.CarregarConfig(ConfiguracoesValidadas.TipoDeObjetoAtivador);
                }

                Application["ServicosCarregados"] = true;
            }

            return Ativador.Get<T>();
        }

        private string BuscarRecursoLocal(string pChaveDaMensagem)
        {
            object lMensagem = null;

            try
            {
                lMensagem = HttpContext.GetLocalResourceObject(HttpContext.Current.Request.Path, pChaveDaMensagem);
            }
            catch (Exception)
            {
                //falta o arquivo de resources

                HttpContext.Current.Trace.Write("MissingResourceError" + HttpContext.Current.Request.FilePath);
            }

            if (lMensagem == null) lMensagem = string.Format("[{0}][{1}]", HttpContext.Current.Request.Path, pChaveDaMensagem);

            return lMensagem.ToString();
        }

        private static void embaralhar(ref char[] array, int qtd)
        {
            Random random = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < qtd; i++)
            {
                for (int j = 0; j <= array.Length; j++)
                {
                    swap(ref array[random.Next(0, array.Length)], ref array[random.Next(0, array.Length)]);
                }
            }
        }

        private static void swap(ref char arg1, ref char arg2)
        {
            char temp = arg1; arg1 = arg2; arg2 = temp;
        }

        #endregion

        #region | Métodos Públicos

        public T InstanciarServico<T>()
        {
            T lRetorno = default(T);

            try
            {
                lRetorno = Ativador.Get<T>();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return lRetorno;
        }

        public List<SinacorListaInfo> BuscarListaDoSinacor(SinacorListaInfo pConfiguracaoInfo)
        {
            string lKey = string.Concat(pConfiguracaoInfo.Informacao.ToString(), this.GetType().ToString());

            List<SinacorListaInfo> lRetorno = new List<SinacorListaInfo>();

            if (Cache[lKey] == null)
            {
                ConsultarEntidadeCadastroRequest<SinacorListaInfo> lRequest;
                ConsultarEntidadeCadastroResponse<SinacorListaInfo> lConsultaResponse;

                lRequest = new ConsultarEntidadeCadastroRequest<SinacorListaInfo>() { EntidadeCadastro = pConfiguracaoInfo };

                lConsultaResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<SinacorListaInfo>(lRequest);

                lRetorno = lConsultaResponse.Resultado;

                Cache[lKey] = lRetorno;
            }
            else
            {
                lRetorno = Cache[lKey] as List<SinacorListaInfo>;
            }

            return lRetorno;
        }

        public List<T> BuscarListaDoCadastro<T>(T pEntidade) where T : ICodigoEntidade
        {
            List<T> lRetorno = new List<T>();

            ConsultarEntidadeCadastroRequest<T> lRequest;
            ConsultarEntidadeCadastroResponse<T> lResponse;

            lRequest = new ConsultarEntidadeCadastroRequest<T>(pEntidade);

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<T>(lRequest);

            lRetorno = lResponse.Resultado;

            return lRetorno;
        }

        private List<SinacorListaInfo> PermissaoTipo(eTipoPessoa pTipoPessoa, List<SinacorListaInfo> lLista)
        {
            List<SinacorListaInfo> lRetorno = new List<SinacorListaInfo>();
            string[] lTipos;

            if (pTipoPessoa == eTipoPessoa.Fisica)
                lTipos = System.Configuration.ConfigurationManager.AppSettings["TipoClientePF"].ToString().Split(',');
            else if (pTipoPessoa == eTipoPessoa.Juridica)
                lTipos = System.Configuration.ConfigurationManager.AppSettings["TipoClientePJ"].ToString().Split(',');
            else
                lTipos = (System.Configuration.ConfigurationManager.AppSettings["TipoClientePF"].ToString() + "," + System.Configuration.ConfigurationManager.AppSettings["TipoClientePJ"].ToString()).Split(',');

            foreach (SinacorListaInfo Lista in lLista)
                foreach (string Config in lTipos)
                    if (Lista.Id == Config)
                        lRetorno.Add(Lista);

            return lRetorno;
        }

        public void PopularControleComTipoPessoa(eTipoPessoa pTipoPessoa, params Repeater[] pControles)
        {
            SinacorListaInfo lInfo = new SinacorListaInfo(eInformacao.TipoCliente);

            List<SinacorListaInfo> lLista = this.BuscarListaDoSinacor(lInfo);

            lLista = PermissaoTipo(pTipoPessoa, lLista);

            foreach (Repeater lControle in pControles)
            {
                lControle.DataSource = lLista;
                lControle.DataBind();
            }
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

        public void PopularControleComListaDoCadastro<T>(T pEntidade, params Repeater[] pControles) where T : ICodigoEntidade
        {
            List<T> lLista = this.BuscarListaDoCadastro<T>(pEntidade);

            foreach (Repeater lControle in pControles)
            {
                lControle.DataSource = lLista;
                lControle.DataBind();
            }
        }

        public void PopularComboComListaGenerica<T>(List<T> pLista, params Repeater[] pControles) where T : ICodigoEntidade
        {
            if (null != pLista) foreach (Repeater lControle in pControles)
                {
                    lControle.DataSource = pLista;
                    lControle.DataBind();
                }
        }

        public void RedirecionarPara(string pURLRelativa)
        {
            Response.Redirect(this.RaizURL + pURLRelativa);
        }

        public string RetornarErroAjax(string pMensagem, string[] pListaDeMensagens)
        {
            System.Text.StringBuilder lMensagens = new System.Text.StringBuilder();

            foreach (string lMensagem in pListaDeMensagens)
                lMensagens.AppendFormat("{0}{1}", Environment.NewLine, lMensagem);

            return JsonConvert.SerializeObject(new ChamadaAjaxResponse(true, pMensagem, lMensagens.ToString()));
        }

        public string RetornarErroAjax(string pMensagem, params object[] pParams)
        {
            return JsonConvert.SerializeObject(new ChamadaAjaxResponse(true, pMensagem, pParams));
        }

        public string RetornarErroAjax(string pMensagem, Exception pErro, params object[] pParams)
        {
            return JsonConvert.SerializeObject(new ChamadaAjaxResponse(pMensagem, pErro, pParams));
        }

        public string RetornarErroAjax(string pMensagem, string pMensagemExtendida)
        {
            return JsonConvert.SerializeObject(new ChamadaAjaxResponse(true, pMensagem) { MensagemExtendida = pMensagemExtendida });
        }

        public string RetornarSucessoAjax(string pMensagem, params object[] pParams)
        {
            return JsonConvert.SerializeObject(new ChamadaAjaxResponse(pMensagem, pParams));
        }

        public string RetornarSucessoAjax(object pObjetoDeRetorno, string pMensagem, params object[] pParams)
        {
            return JsonConvert.SerializeObject(new ChamadaAjaxResponse(pMensagem, pParams) { ObjetoDeRetorno = pObjetoDeRetorno });
        }

        public void RegistrarRespostasAjax(string[] pAcoes, ResponderAcaoAjaxDelegate[] pFuncoes)
        {
            if (pAcoes.Length != pFuncoes.Length)
                throw new Exception("O número de ações tem que ser equivalente ao de funções");

            gAcoes = new Dictionary<string, ResponderAcaoAjaxDelegate>();

            for (int a = 0; a < pAcoes.Length; a++) if (!gAcoes.ContainsKey(pAcoes[a]))
                    gAcoes.Add(pAcoes[a].ToLower(), pFuncoes[a]);

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



        public MensagemResponseStatusEnum EnviarEmailSuitability(Nullable<int> pIdCliente, String pPerfil, List<String> pDestinatarios, string pAssunto, string pNomeArquivo, Dictionary<string, string> pVariaveisEmail, eTipoEmailDisparo pTipoEmailDisparo, List<Gradual.OMS.Email.Lib.EmailAnexoInfo> pAnexos = null)
        {
            using (var lStreamReader = File.OpenText(this.Server.MapPath(string.Concat("~/Extras/Emails/", pNomeArquivo)))) //--> Carregando o arquivo num StreamReader
            {
                var lStringBuilder = new System.Text.StringBuilder(lStreamReader.ReadToEnd()); //--> Convertendo o arquivo html em texto.

                foreach (KeyValuePair<string, string> item in pVariaveisEmail)
                    lStringBuilder.Replace(item.Key, item.Value); //--> Substituindo as variáveis do conteúdo do e-mail.

                var lAtivador = Ativador.Get<IServicoEmail>();

                if (lAtivador != null)
                {
                    var lEmailEntrada = new EnviarEmailRequest();
                    lEmailEntrada.Objeto = new EmailInfo();
                    lEmailEntrada.Objeto.Assunto = pAssunto;
                    lEmailEntrada.Objeto.Destinatarios = pDestinatarios;
                    lEmailEntrada.Objeto.Remetente = ConfigurationManager.AppSettings["EmailRemetenteGradual"].ToString();
                    lEmailEntrada.Objeto.CorpoMensagem = lStringBuilder.ToString();

                    if (pTipoEmailDisparo.Equals(eTipoEmailDisparo.Compliance))
                    {
                        lEmailEntrada.Objeto.DestinatariosCO = new List<string>() { ConfiguracoesValidadas.EmailComCopiaOculta };
                    }

                    if (null != pAnexos) foreach (var item in pAnexos)
                        {
                            lEmailEntrada.Objeto.Anexos.Add(item);
                        }

                    EnviarEmailResponse lEmailRetorno = lAtivador.Enviar(lEmailEntrada);


                    if (MensagemResponseStatusEnum.OK.Equals(lEmailRetorno.StatusResposta))
                        ServicoPersistencia.SalvarObjeto<LogEmailInfo>(new SalvarObjetoRequest<LogEmailInfo>() { Objeto = new LogEmailInfo(lEmailEntrada.Objeto, pTipoEmailDisparo, pIdCliente, pPerfil) });

                    return lEmailRetorno.StatusResposta;
                }
                else
                {
                    throw new Exception("Ativador nulo ao enviar email. Provável erro de configuração, verificar entradas para 'TipoDeObjetoAtivador' e seção de config para 'IServicoEmail'");
                }
            }
        }

        public MensagemResponseStatusEnum EnviarEmail(List<String> pDestinatarios, string pAssunto, string pNomeArquivo, Dictionary<string, string> pVariaveisEmail, eTipoEmailDisparo pTipoEmailDisparo, List<Gradual.OMS.Email.Lib.EmailAnexoInfo> pAnexos = null)
        {
            using (var lStreamReader = File.OpenText(this.Server.MapPath(string.Concat("~/Extras/Emails/", pNomeArquivo)))) //--> Carregando o arquivo num StreamReader
            {
                var lStringBuilder = new System.Text.StringBuilder(lStreamReader.ReadToEnd()); //--> Convertendo o arquivo html em texto.

                foreach (KeyValuePair<string, string> item in pVariaveisEmail)
                    lStringBuilder.Replace(item.Key, item.Value); //--> Substituindo as variáveis do conteúdo do e-mail.

                return this.EnviarEmail(pDestinatarios, pAssunto, lStringBuilder.ToString(), pTipoEmailDisparo, pAnexos);
            }
        }

        public MensagemResponseStatusEnum EnviarEmail(string pDestinatario, string pAssunto, string pNomeArquivo, Dictionary<string, string> pVariaveisEmail, eTipoEmailDisparo pTipoEmailDisparo, List<Gradual.OMS.Email.Lib.EmailAnexoInfo> pAnexos = null)
        {
            using (var lStreamReader = File.OpenText(this.Server.MapPath(string.Concat("~/Extras/Emails/", pNomeArquivo)))) //--> Carregando o arquivo num StreamReader
            {
                var lStringBuilder = new System.Text.StringBuilder(lStreamReader.ReadToEnd()); //--> Convertendo o arquivo html em texto.

                foreach (KeyValuePair<string, string> item in pVariaveisEmail)
                    lStringBuilder.Replace(item.Key, item.Value); //--> Substituindo as variáveis do conteúdo do e-mail.

                return this.EnviarEmail(pDestinatario, pAssunto, lStringBuilder.ToString(), pTipoEmailDisparo, pAnexos);
            }
        }

        private MensagemResponseStatusEnum EnviarEmail(string pDestinatario, string pAssunto, string pCorpoEmail, eTipoEmailDisparo pTipoEmailDisparo, List<Gradual.OMS.Email.Lib.EmailAnexoInfo> pAnexos = null)
        {
            var lDestinatarios = new List<string>();

            if (pDestinatario.Contains(';'))
            {
                string[] lItems = pDestinatario.Split(';');

                lDestinatarios.AddRange(lItems);
            }
            else
            {
                lDestinatarios.Add(pDestinatario);
            }

            return this.EnviarEmail(lDestinatarios, pAssunto, pCorpoEmail, pTipoEmailDisparo, pAnexos);
        }

        private MensagemResponseStatusEnum EnviarEmail(List<string> pDestinatarios, string pAssunto, string pCorpoEmail, eTipoEmailDisparo pTipoEmailDisparo, List<Gradual.OMS.Email.Lib.EmailAnexoInfo> pAnexos = null)
        {
            var lAtivador = Ativador.Get<IServicoEmail>();

            if (lAtivador != null)
            {
                var lEmailEntrada = new EnviarEmailRequest();
                lEmailEntrada.Objeto = new EmailInfo();
                lEmailEntrada.Objeto.Assunto = pAssunto;
                lEmailEntrada.Objeto.Destinatarios = pDestinatarios;
                lEmailEntrada.Objeto.Remetente = ConfigurationManager.AppSettings["EmailRemetenteGradual"].ToString();
                lEmailEntrada.Objeto.CorpoMensagem = pCorpoEmail;

                if (pTipoEmailDisparo.Equals(eTipoEmailDisparo.Compliance))
                {
                    //if (pTipoEmailDisparo.Equals(eTipoEmailDisparo.Compliance))
                    //{
                        lEmailEntrada.Objeto.DestinatariosCO = new List<string>() { ConfiguracoesValidadas.EmailComCopiaOculta };
                    //}
                }

                if (null != pAnexos) foreach (var item in pAnexos)
                {
                    lEmailEntrada.Objeto.Anexos.Add(item);
                }
                
                EnviarEmailResponse lEmailRetorno = lAtivador.Enviar(lEmailEntrada);
                

                if (MensagemResponseStatusEnum.OK.Equals(lEmailRetorno.StatusResposta))
                    ServicoPersistencia.SalvarObjeto<LogEmailInfo>(new SalvarObjetoRequest<LogEmailInfo>() { Objeto = new LogEmailInfo(lEmailEntrada.Objeto, pTipoEmailDisparo) });

                return lEmailRetorno.StatusResposta;
            }
            else
            {
                throw new Exception("Ativador nulo ao enviar email. Provável erro de configuração, verificar entradas para 'TipoDeObjetoAtivador' e seção de config para 'IServicoEmail'");
            }
        }

        public static String GerarSenha()
        {
            string carac = "abcdefhijkmnopqrstuvxwyz123456789_*&%$#";
            char[] caracter = carac.ToCharArray();
            embaralhar(ref caracter, 3);

            string novaSenha = string.Empty;

            for (int i = 0; i < 8; i++)
                novaSenha += caracter[i];

            return novaSenha;
        }

        public string ConsultarCodigoAssessoresVinculadosString(int? pCdAssessor)
        {
            if (null == pCdAssessor)
                return null;

            var lRetorno = string.Empty;

            var lListaAssessores = this.ConsultarCodigoAssessoresVinculadosLista(pCdAssessor);
            if (lListaAssessores == null && pCdAssessor.HasValue)
            {
                lRetorno = pCdAssessor.Value.ToString();
            }
            else if (null != lListaAssessores && null != lListaAssessores.ListaCodigoAssessoresVinculados && lListaAssessores.ListaCodigoAssessoresVinculados.Count > 0)
                lListaAssessores.ListaCodigoAssessoresVinculados.ForEach(ass =>
                {
                    lRetorno = string.Concat(lRetorno, ",", ass);
                });

            return lRetorno.Trim(',');
        }

        public List<int> ConsultarCodigoAssessoresVinculadosListInt(int? pCdAssessor)
        {
            if (null == pCdAssessor)
                return null;

            List<int> lRetorno = new List<int>(); ;

            var lListaAssessores = this.ConsultarCodigoAssessoresVinculadosLista(pCdAssessor);
            if (lListaAssessores == null && pCdAssessor.HasValue)
            {
                lRetorno.Add(pCdAssessor.Value);
            }
            else if (null != lListaAssessores && null != lListaAssessores.ListaCodigoAssessoresVinculados && lListaAssessores.ListaCodigoAssessoresVinculados.Count > 0)
                lListaAssessores.ListaCodigoAssessoresVinculados.ForEach(ass =>
                {
                    lRetorno.Add(ass);// = string.Concat(lRetorno, ",", ass);
                });

            return lRetorno;
        }

        public ListaEmailAssessorInfo ConsultarListaEmailAssessor(int pCdAssessor)
        {
            var lRetorno = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ListaEmailAssessorInfo>(
                new ReceberEntidadeCadastroRequest<ListaEmailAssessorInfo>()
                {
                    EntidadeCadastro = new ListaEmailAssessorInfo()
                    {
                        IdAssessor = pCdAssessor
                    }
                });

            return lRetorno.EntidadeCadastro;
        }

        public ListaAssessoresVinculadosInfo ConsultarCodigoAssessoresVinculadosLista(int? pCdAssessor)
        {
            if (null == pCdAssessor)
                return null;

            ReceberEntidadeCadastroRequest<ListaAssessoresVinculadosInfo> lRequest = new ReceberEntidadeCadastroRequest<ListaAssessoresVinculadosInfo>();

            lRequest.EntidadeCadastro = new ListaAssessoresVinculadosInfo();

            lRequest.EntidadeCadastro.ConsultaCodigoAssessor = pCdAssessor;

            if (this.CodigoAssessor.HasValue)
            {
                lRequest.EntidadeCadastro.CodigoLogin = this.UsuarioLogado.Id;
            }

            var lRetorno = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ListaAssessoresVinculadosInfo>(lRequest);

            return lRetorno.EntidadeCadastro;
        }

        #endregion

        #region Monitoramento Risco
        public decimal CalcularTaxaPtax(decimal taxaOperada)
        {
            decimal lRetorno = 0.0M;

            ReceberEntidadeCadastroRequest<MonitoramentoRiscoLucroTaxaPTAXInfo> lRequest = 
                new ReceberEntidadeCadastroRequest<MonitoramentoRiscoLucroTaxaPTAXInfo>();

            ReceberEntidadeCadastroResponse<MonitoramentoRiscoLucroTaxaPTAXInfo> lResponse=
                new ReceberEntidadeCadastroResponse<MonitoramentoRiscoLucroTaxaPTAXInfo>();

            lResponse = ServicoPersistenciaCadastro.ReceberEntidadeCadastro<MonitoramentoRiscoLucroTaxaPTAXInfo>(lRequest);

            decimal taxaMercado = lResponse.EntidadeCadastro.ValorPtax;

            lRetorno = taxaOperada * taxaMercado;

            return lRetorno;
        }

        
        #endregion

        #region | Registro de Logs

        public void RegistrarLogAlteracao(string pDsObservacao = null)
        {
            this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<LogIntranetInfo>(
                new SalvarEntidadeCadastroRequest<LogIntranetInfo>()
                {
                    EntidadeCadastro = new LogIntranetInfo()
                    {
                        DsIp = this.Request.UserHostAddress,
                        DsObservacao = pDsObservacao,
                        DsTela = this.Request.FilePath,
                        DtEvento = DateTime.Now,
                        IdAcao = Contratos.Dados.Enumeradores.TipoAcaoUsuario.Edicao,
                        IdLogin = this.UsuarioLogado.Id,
                    }
                });
        }

        public void RegistrarLogAlteracao(LogIntranetInfo pLogIntranetInfo)
        {
            if (null == pLogIntranetInfo)
                pLogIntranetInfo = new LogIntranetInfo();

            pLogIntranetInfo.IdAcao = Contratos.Dados.Enumeradores.TipoAcaoUsuario.Edicao;
            pLogIntranetInfo.DsIp = this.Request.UserHostAddress;
            pLogIntranetInfo.DsTela = this.Request.FilePath;
            pLogIntranetInfo.DtEvento = DateTime.Now;
            pLogIntranetInfo.IdLogin = this.UsuarioLogado.Id;

            this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<LogIntranetInfo>(
                new SalvarEntidadeCadastroRequest<LogIntranetInfo>()
                {
                    EntidadeCadastro = pLogIntranetInfo
                });
        }

        public void RegistrarLogConsulta(string pDsObservacao = null)
        {
            this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<LogIntranetInfo>(
                new SalvarEntidadeCadastroRequest<LogIntranetInfo>()
                {
                    EntidadeCadastro = new LogIntranetInfo()
                    {
                        DsIp = this.Request.UserHostAddress,
                        DsObservacao = pDsObservacao,
                        DsTela = this.Request.FilePath,
                        DtEvento = DateTime.Now,
                        IdAcao = Contratos.Dados.Enumeradores.TipoAcaoUsuario.Consulta,
                        IdLogin = this.UsuarioLogado.Id,
                    }
                });
        }

        public void RegistrarLogConsulta(LogIntranetInfo pLogIntranetInfo)
        {
            if (null == pLogIntranetInfo)
                pLogIntranetInfo = new LogIntranetInfo();

            pLogIntranetInfo.IdAcao = Contratos.Dados.Enumeradores.TipoAcaoUsuario.Consulta;
            pLogIntranetInfo.DsIp = this.Request.UserHostAddress;
            pLogIntranetInfo.DsTela = this.Request.FilePath;
            pLogIntranetInfo.DtEvento = DateTime.Now;
            if (this.UsuarioLogado != null)
                pLogIntranetInfo.IdLogin = this.UsuarioLogado.Id;

            this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<LogIntranetInfo>(
                new SalvarEntidadeCadastroRequest<LogIntranetInfo>()
                {
                    EntidadeCadastro = pLogIntranetInfo
                });
        }

        public void RegistrarLogExclusao(string pDsObservacao = null)
        {
            this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<LogIntranetInfo>(
                new SalvarEntidadeCadastroRequest<LogIntranetInfo>()
                {
                    EntidadeCadastro = new LogIntranetInfo()
                    {
                        DsIp = this.Request.UserHostAddress,
                        DsObservacao = pDsObservacao,
                        DsTela = this.Request.FilePath,
                        DtEvento = DateTime.Now,
                        IdAcao = Contratos.Dados.Enumeradores.TipoAcaoUsuario.Exclusao,
                        IdLogin = this.UsuarioLogado.Id,
                    }
                });
        }

        public void RegistrarLogExclusao(LogIntranetInfo pLogIntranetInfo)
        {
            if (null == pLogIntranetInfo)
                pLogIntranetInfo = new LogIntranetInfo();

            pLogIntranetInfo.IdAcao = Contratos.Dados.Enumeradores.TipoAcaoUsuario.Exclusao;
            pLogIntranetInfo.DsIp = this.Request.UserHostAddress;
            pLogIntranetInfo.DsTela = this.Request.FilePath;
            pLogIntranetInfo.DtEvento = DateTime.Now;
            pLogIntranetInfo.IdLogin = this.UsuarioLogado.Id;

            this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<LogIntranetInfo>(
                new SalvarEntidadeCadastroRequest<LogIntranetInfo>()
                {
                    EntidadeCadastro = pLogIntranetInfo
                });
        }

        public void RegistrarLogInclusao(string pDsObservacao = null)
        {
            this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<LogIntranetInfo>(
                new SalvarEntidadeCadastroRequest<LogIntranetInfo>()
                {
                    EntidadeCadastro = new LogIntranetInfo()
                    {
                        DsIp = this.Request.UserHostAddress,
                        DsObservacao = pDsObservacao,
                        DsTela = this.Request.FilePath,
                        DtEvento = DateTime.Now,
                        IdAcao = Contratos.Dados.Enumeradores.TipoAcaoUsuario.Inclusao,
                        IdLogin = this.UsuarioLogado.Id,
                    }
                });
        }

        public void RegistrarLogInclusao(LogIntranetInfo pLogIntranetInfo)
        {
            if (null == pLogIntranetInfo)
                pLogIntranetInfo = new LogIntranetInfo();

            pLogIntranetInfo.IdAcao = Contratos.Dados.Enumeradores.TipoAcaoUsuario.Inclusao;
            pLogIntranetInfo.DsIp = this.Request.UserHostAddress;
            pLogIntranetInfo.DsTela = this.Request.FilePath;
            pLogIntranetInfo.DtEvento = DateTime.Now;
            pLogIntranetInfo.IdLogin = this.UsuarioLogado.Id;

            this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<LogIntranetInfo>(
                new SalvarEntidadeCadastroRequest<LogIntranetInfo>()
                {
                    EntidadeCadastro = pLogIntranetInfo
                });
        }

        public List<Transporte_PosicaoCotista> PosicaoFundos(int IdCotista, string CpfCnpj)
        {
            var lRetorno = new List<Transporte_PosicaoCotista>();


            FundoRequest lRequest = new FundoRequest();
            FundoResponse lResponse;

            lRequest.CpfDoCliente = CpfCnpj;

            lResponse = ClienteDbLib.SelecionarFundoItau(lRequest);

            lRetorno.AddRange(new Transporte_PosicaoCotista().TrauzirListaItau(lResponse.ListaFundo));

            Gradual.Intranet.Www.PosicaoCotista.PosicaoCotistaWSGradualSoapClient lServicoFinancial = new Gradual.Intranet.Www.PosicaoCotista.PosicaoCotistaWSGradualSoapClient();
            Gradual.Intranet.Www.PosicaoCotista.ValidateLogin lLogin = new Gradual.Intranet.Www.PosicaoCotista.ValidateLogin();

            lLogin.Username = ConfiguracoesValidadas.UsuarioFinancial;
            lLogin.Password = ConfiguracoesValidadas.SenhaFinancial;

            Gradual.Intranet.Www.PosicaoCotista.PosicaoCotistaViewModel[] lPosicao = lServicoFinancial.Exporta(lLogin, null, IdCotista, null);

            lRetorno.AddRange(new Transporte_PosicaoCotista().TraduzirLista(lPosicao));

            return lRetorno;
        }

        public List<Transporte_PosicaoCotista> PosicaoFundosSumarizado(int IdCotista, string CpfCnpj)
        {
            //var lRetorno = new List<Transporte_PosicaoCotista>();
            //FundoRequest lRequest = new FundoRequest();
            //FundoResponse lResponse;
            //lRequest.CpfDoCliente = CpfCnpj;
            //lResponse = ClienteDbLib.SelecionarFundoItau(lRequest);
            //lRetorno.AddRange(new Transporte_PosicaoCotista().TrauzirListaItau(lResponse.ListaFundo));
            //Gradual.Intranet.Www.PosicaoCotista.PosicaoCotistaWSGradualSoapClient lServicoFinancial = new Gradual.Intranet.Www.PosicaoCotista.PosicaoCotistaWSGradualSoapClient();
            //Gradual.Intranet.Www.PosicaoCotista.ValidateLogin lLogin = new Gradual.Intranet.Www.PosicaoCotista.ValidateLogin();
            //lLogin.Username = ConfiguracoesValidadas.UsuarioFinancial;
            //lLogin.Password = ConfiguracoesValidadas.SenhaFinancial;
            //Gradual.Intranet.Www.PosicaoCotista.PosicaoCotistaViewModel[] lPosicao = lServicoFinancial.Exporta(lLogin, null, IdCotista, null);
            //lRetorno.AddRange(new Transporte_PosicaoCotista().TraduzirListaSumarizada(lPosicao));
            //return lRetorno;


            var lRetorno = new List<Transporte_PosicaoCotista>();
            
            System.Data.DataTable lTable = ClienteDbLib.ConsultaFundosBritech(IdCotista);

            lRetorno.AddRange(new Transporte_PosicaoCotista().TraduzirListaSumarizada(lTable));

            return lRetorno;
        }

        public List<ClienteResumidoInfo> ReceberListaClientesAssessoresVinculados(int CodigoAssessor)
        {
            return ClienteDbLib.ReceberListaClientesAssessoresVinculadosComCPF(CodigoAssessor);
        }

        public List<ClienteResumidoInfo> ReceberListaClientesDoAssessor(int CodigoAssessor)
        {
            return ClienteDbLib.ReceberListaClientesDoAssessor(CodigoAssessor, null);
        }

        public List<ClienteResumidoInfo> ConsultarClienteResumido_DadosBasicos(string cpf)
        {
            var lRequest = new ConsultarEntidadeRequest<ClienteResumidoInfo>();
            //pParametros.Objeto.OpcaoBuscarPor
            lRequest.Objeto = new ClienteResumidoInfo();
            lRequest.Objeto.OpcaoBuscarPor = OpcoesBuscarPor.CpfCnpj;
            lRequest.Objeto.TermoDeBusca = cpf;

            var lCliente = ClienteDbLib.ConsultarClienteResumido_DadosBasicos(lRequest);

            return lCliente.Resultado;
        }

        public List<ClienteResumidoInfo> ConsultarClienteResumido_DadosBasicos(int CodigoConta)
        {
            var lRequest = new ConsultarEntidadeRequest<ClienteResumidoInfo>();
            //pParametros.Objeto.OpcaoBuscarPor
            lRequest.Objeto = new ClienteResumidoInfo();
            lRequest.Objeto.OpcaoBuscarPor = OpcoesBuscarPor.CodBovespa;
            lRequest.Objeto.TermoDeBusca = CodigoConta.ToString();

            var lCliente = ClienteDbLib.ConsultarClienteResumido_DadosBasicos(lRequest);

            return lCliente.Resultado;
        }
        #endregion
    }
}
