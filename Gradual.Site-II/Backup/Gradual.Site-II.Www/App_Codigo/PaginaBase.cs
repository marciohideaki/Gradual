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
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Servicos.BancoDeDados.Negocio;
using Gradual.OMS.Email.Lib;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Persistencia;
using Gradual.Site.DbLib;
using Gradual.Site.DbLib.Dados;
using Gradual.Site.DbLib.Mensagens;
using Gradual.Site.Www.Resc.UserControls;
using log4net;
using Newtonsoft.Json;
using Gradual.Intranet.Contratos.Dados.Cadastro;

using Gradual.OMS.PlanoCliente.Lib;
using Gradual.OMS.Cotacao.Lib;
using Gradual.OMS.Seguranca.Lib;
using System.Text;
using System.Security.Cryptography;
using Gradual.Site.DbLib.Widgets;
using System.Globalization;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.Site.DbLib.Persistencias.MinhaConta.Ordens;
using System.Collections;
using System.Net;
using System.Linq;

namespace Gradual.Site.Www
{
    public delegate string ResponderAcaoAjaxDelegate();

    public class PaginaBase : System.Web.UI.Page
    {
        #region Globais

        public const string CONST_RESPOSTA_JA_ENVIADA_PELA_FUNCAO = "RESPOSTA_JA_ENVIADA_PELA_FUNCAO";

        public const string CONST_FUNCAO_CASO_NAO_HAJA_ACTION = "FUNCAO_CASO_NAO_HAJA_ACTION";

        public const string CONST_MENSAGEM_SEM_USUARIO_LOGADO = "SEM_USUARIO_NA_SESSAO";

        public const string CONST_ERRO_ASSINATURA_NAO_CONFERE = "Assinatura não confere";

        public const string CONST_ID_ASSESSOR_DELA = "305";

        private Dictionary<string, ResponderAcaoAjaxDelegate> gAcoes;

        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _JavascriptParaRodarOnLoad = "";

        public CultureInfo gCultureInfoBR = new CultureInfo("pt-BR");

        #endregion

        #region Propriedades

        public string Acao
        {
            get
            {
                return Request["Acao"];
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

        public string HostERaizHttps
        {
            get
            {
                string lRetorno = string.Format("{0}{1}", this.HostDoSite, this.RaizDoSite);

                if (!lRetorno.ToLower().Contains("localhost"))
                {
                    lRetorno = lRetorno.Replace("http:", "https:");
                }

                return lRetorno;
            }
        }

        public TransporteSessaoClienteLogado SessaoClienteLogado
        {
            get { return (TransporteSessaoClienteLogado)this.Session["ClienteLogado"]; }
            set { this.Session["ClienteLogado"] = value; }
        }

        public bool EstruturaDaPaginaParaTodosTipos
        {
            get
            {
                if (ViewState["EstruturaDaPaginaParaTodosTipos"] == null)
                    return true;

                return (bool)ViewState["EstruturaDaPaginaParaTodosTipos"];
            }

            set
            {
                ViewState["EstruturaDaPaginaParaTodosTipos"] = value;
            }
        }

        public int TipoDeUsuarioLogado
        {
            get
            {
                int lRetorno = this.EstruturaDaPaginaParaTodosTipos ? 1 : 2;   //1 é "Todos (ambos)", 2 é "Visitante", 3 é "Cliente"

                if (Session["TipoDeClientePreview"] != null)
                {
                    lRetorno = Convert.ToInt32(Session["TipoDeClientePreview"]);
                }
                else
                {
                    if (this.SessaoClienteLogado != null && !this.EstruturaDaPaginaParaTodosTipos)
                    {
                        lRetorno = 3;   //"Cliente"
                    }
                }

                return lRetorno;
            }
        }

        public string AliasDoTipoDeUsuario
        {
            get
            {
                string lRetorno = "";

                int lTipo = this.TipoDeUsuarioLogado;

                if (lTipo == 1)
                {
                    lRetorno = "Todos";
                }
                else if (lTipo == 2)
                {
                    lRetorno = "Visitante";
                }
                else if (lTipo == 3)
                {
                    lRetorno = "Cliente";
                }

                return lRetorno;
            }
        }

        public List<int> TiposDeUsuarioLogadoDisponiveis
        {
            get
            {
                if (Session["TiposDeUsuarioLogadoDisponiveis"] == null)
                {
                    List<int> lLista = new List<int>();

                    lLista.Add(1); //"Todos"

                    Session["TiposDeUsuarioLogadoDisponiveis"] = lLista;
                }

                return (List<int>)Session["TiposDeUsuarioLogadoDisponiveis"];
            }

            set
            {
                Session["TiposDeUsuarioLogadoDisponiveis"] = value;
            }
        }

        public int IDEstruturaCorrente
        {
            get
            {
                if (Session["IDEstruturaCorrente"] == null)
                    return 0;
                else
                    return Session["IDEstruturaCorrente"].DBToInt32();
            }
            set
            {
                Session["IDEstruturaCorrente"] = value;
            }
        }

        public bool QuantidadeWidgetMaiorQueZero
        {
            get
            {
                if (Session["QuantidadeWidgetMaiorQueZero"] == null)
                    return false;
                else
                    return Session["QuantidadeWidgetMaiorQueZero"].DBToBoolean();
            }
            set
            {
                Session["QuantidadeWidgetMaiorQueZero"] = value;
            }
        }

        public PaginaInternaMaster PaginaMaster
        {
            get { return (PaginaInternaMaster)this.Master; }
        }

        public IServicoPersistenciaCadastro ServicoPersistenciaCadastro
        {
            get
            {
                return InstanciarServicoDoAtivador<IServicoPersistenciaCadastro>();
            }
        }

        public IServicoPersistencia ServicoPersistencia
        {
            get
            {
                return InstanciarServicoDoAtivador<IServicoPersistencia>();
            }
        }

        public IServicoPersistenciaSite ServicoPersistenciaSite
        {
            get
            {
                return InstanciarServicoDoAtivador<IServicoPersistenciaSite>();
            }
        }

        public IServicoCotacao ServicoCotacao
        {
            get
            {
                return InstanciarServicoDoAtivador<IServicoCotacao>();
            }
        }

        public IServicoSeguranca ServicoSeguranca
        {
            get
            {
                return InstanciarServicoDoAtivador<IServicoSeguranca>();
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

        public bool PassoDoCadastro { get; set; }

        public bool EPaginaDoMinhaConta
        {
            get
            {
                string lURL = Request.Url.AbsoluteUri.ToLower();

                return (lURL.Contains("/minhaconta"));
            }
        }

        public Dictionary<int, TransporteBannerLateral> BannersLateraisDisponiveis
        {
            get
            {
                if (Application["BannersLateraisDisponiveis"] == null)
                {
                    CarregarBanners();
                }

                return (Dictionary<int, TransporteBannerLateral>)Application["BannersLateraisDisponiveis"];
            }

            set
            {
                Application["BannersLateraisDisponiveis"] = value;
            }
        }

        public Dictionary<string, List<TransporteBannerLateral>> BannersLateraisPorPagina
        {
            get
            {
                if (Application["BannersLateraisPorPagina"] == null)
                {
                    CarregarBanners();
                }

                return (Dictionary<string, List<TransporteBannerLateral>>)Application["BannersLateraisPorPagina"];
            }

            set
            {
                Application["BannersLateraisPorPagina"] = value;
            }
        }

        public string PerfMonChave { get; set; }

        public bool PerfMonHabilitado
        {
            get
            {
                return !(string.IsNullOrEmpty(this.PerfMonChave));
            }
        }

        public List<PaginaInfo> ListaDePaginas
        {
            get
            {
                if (Application["ListaDePaginas"] == null)
                {
                    CarregarListaDePaginas();
                }

                return (List<PaginaInfo>)Application["ListaDePaginas"];
            }

            set
            {
                Application["ListaDePaginas"] = value;
            }
        }

        public List<string> ListaDePaginasComDiretorios
        {
            get
            {
                if (Application["ListaDePaginasComDiretorios"] == null)
                {
                    CarregarListaDePaginas();
                }

                return (List<string>)Application["ListaDePaginasComDiretorios"];
            }

            set
            {
                Application["ListaDePaginasComDiretorios"] = value;
            }
        }

        public Dictionary<string, string> CacheDePaginas
        {
            get
            {
                if(Application["CacheDePaginas"] == null)
                    Application["CacheDePaginas"] = new Dictionary<string, string>();

                return (Dictionary<string, string>)Application["CacheDePaginas"];
            }

            set
            {
                Application["CacheDePaginas"] = value;
            }
        }

        public List<string> ListaDeVersoes
        {
            get
            {
                if (Application["ListaDeVersoes"] == null)
                {
                    CarregarListaDeVersoes();
                }

                return (List<string>)Application["ListaDeVersoes"];
            }

            set
            {
                Application["ListaDeVersoes"] = value;
            }
        }

        public List<string> ListaDeVersoesDaPagina
        {
            get
            {
                List<string> lVersoes = new List<string>();

                foreach (PaginaInfo lPagina in this.ListaDePaginas)
                {
                    if (lPagina.CodigoPagina == PaginaMaster.IdDaPagina)
                    {
                        foreach (VersaoInfo lVersao in lPagina.Versoes)
                        {
                            lVersoes.Add(lVersao.CodigoDeIdentificacao);
                        }

                        return lVersoes;
                    }
                }

                return null;
            }
        }

        public string Versao { get; set; }

        public string VersaoPublicada { get; set; }

        private string _NomeDaPagina = "nome_nao_carregado";

        public string NomeDaPagina
        {
            get
            {
                return _NomeDaPagina;
            }

            set
            {
                _NomeDaPagina = value;
            }
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

                WidgetBase.TipoWidget tipo = (WidgetBase.TipoWidget)System.Enum.Parse(typeof(WidgetBase.TipoWidget), lTipo);

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

                    case WidgetBase.TipoWidget.Embed:

                        widEmbed lWidgetEmbed = JsonConvert.DeserializeObject<widEmbed>(pWidgetInfo.WidgetJson);

                        lWidgetEmbed.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetEmbed.IdDoWidget = pWidgetInfo.CodigoWidget;

                        return lWidgetEmbed;

                    case WidgetBase.TipoWidget.Abas:

                        widAbas lWidgetAbas = JsonConvert.DeserializeObject<widAbas>(pWidgetInfo.WidgetJson);

                        lWidgetAbas.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetAbas.IdDoWidget = pWidgetInfo.CodigoWidget;
                        lWidgetAbas.HostERaiz = this.HostERaiz;

                        return lWidgetAbas;

                    case WidgetBase.TipoWidget.Acordeon:

                        widAcordeon lWidgetAcordeon = JsonConvert.DeserializeObject<widAcordeon>(pWidgetInfo.WidgetJson);

                        lWidgetAcordeon.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetAcordeon.IdDoWidget = pWidgetInfo.CodigoWidget;
                        lWidgetAcordeon.HostERaiz = this.HostERaiz;

                        return lWidgetAcordeon;

                    case WidgetBase.TipoWidget.TextoHTML:

                        widTextoHTML lWidgetHTML = JsonConvert.DeserializeObject<widTextoHTML>(pWidgetInfo.WidgetJson);

                        lWidgetHTML.IdDaEstrutura = pWidgetInfo.CodigoEstrutura;
                        lWidgetHTML.IdDoWidget = pWidgetInfo.CodigoWidget;
                        lWidgetHTML.HostERaiz = this.HostERaiz;

                        return lWidgetHTML;
                    default:
                        break;
                }
            }
            return null;
        }

        private MensagemResponseStatusEnum EnviarEmail(string pDestinatario, string pAssunto, string pCorpoEmail, eTipoEmailDisparo pTipoEmailDisparo, List<Gradual.OMS.Email.Lib.EmailAnexoInfo> pAnexos = null, List<string> pDestinatariosOcultos = null)
        {
            List<string> lDestinatarios = new List<string>();

            if (pDestinatario.Contains(';'))
            {
                string[] lItems = pDestinatario.Split(';');

                lDestinatarios.AddRange(lItems);
            }
            else
            {
                lDestinatarios.Add(pDestinatario);
            }

            return this.EnviarEmail(lDestinatarios, pAssunto, pCorpoEmail, pTipoEmailDisparo, pAnexos, pDestinatariosOcultos);
        }

        private MensagemResponseStatusEnum EnviarEmail(List<string> pDestinatarios, string pAssunto, string pCorpoEmail, eTipoEmailDisparo pTipoEmailDisparo, List<Gradual.OMS.Email.Lib.EmailAnexoInfo> pAnexos = null, List<string> pDestinatariosOcultos = null)
        {
            try
            {
                IServicoEmail lServico = InstanciarServicoDoAtivador<IServicoEmail>();

                if (lServico != null)
                {
                    EnviarEmailRequest lRequest = new EnviarEmailRequest();
                    EnviarEmailResponse lResponse;

                    string lEmails = "";

                    foreach (string lEmail in pDestinatarios)
                    {
                        lEmails += lEmail + ",";
                    }

                    lEmails.TrimEnd(",".ToCharArray());

                    if (pDestinatariosOcultos == null)
                        pDestinatariosOcultos = new List<string>(); //o serviço "não suporta" nulos

                    lRequest.Objeto                 = new EmailInfo();
                    lRequest.Objeto.Assunto         = pAssunto;
                    lRequest.Objeto.Destinatarios   = pDestinatarios;
                    lRequest.Objeto.DestinatariosCO = pDestinatariosOcultos;
                    lRequest.Objeto.Remetente     = ConfiguracoesValidadas.Email_RemetenteGradual;
                    lRequest.Objeto.CorpoMensagem = pCorpoEmail;

                    if (null != pAnexos) foreach (var item in pAnexos)
                            lRequest.Objeto.Anexos.Add(item);

                    lResponse = lServico.Enviar(lRequest);

                    if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        gLogger.InfoFormat("Email enviado com sucesso para [{0}] assunto [{1}]", lEmails, pAssunto);

                        ServicoPersistencia.SalvarObjeto<LogEmailInfo>(new SalvarObjetoRequest<LogEmailInfo>() { Objeto = new LogEmailInfo(lRequest.Objeto, pTipoEmailDisparo) });
                    }
                    else
                    {
                        gLogger.InfoFormat("Mensagem com erro do serivço ao enviar email para [{0}] assunto [{1}]: [{2}], [{3}]", lEmails, pAssunto, lResponse.StatusResposta, lResponse.DescricaoResposta);
                    }

                    return lResponse.StatusResposta;
                }
                else
                {
                    throw new Exception("Ativador nulo ao enviar email. Provável erro de configuração, verificar entradas para 'TipoDeObjetoAtivador' e seção de config para 'IServicoEmail'");
                }
            }
            catch (Exception ex) { throw ex; }
        }

        private string SanitarStringPraJavascript(string pString)
        {
            if (string.IsNullOrEmpty(pString)) return "";

            return pString.Replace('"', '\'').Replace("\n", "\\n").Replace("\r", "");
        }

        private void CarregarBanners()
        {
            BuscarItensDaListaResponse lResponse;
            BuscarItensDaListaRequest  lRequest = new BuscarItensDaListaRequest();

            TransporteBannerLateral lTransporte;

            Dictionary<int, TransporteBannerLateral> lListaBannersDisponiveis = new Dictionary<int, TransporteBannerLateral>();
            Dictionary<string, List<TransporteBannerLateral>> lListaBannersDaPagina = new Dictionary<string, List<TransporteBannerLateral>>();

            gLogger.Info("Buscando banners...");

            lResponse = this.ServicoPersistenciaSite.BuscarBannersLaterais(lRequest);

            gLogger.InfoFormat("Objetos de banner encontrados: [{0}]", lResponse.Itens.Count);

            foreach (ConteudoInfo lInfo in lResponse.Itens)
            {
                lTransporte = new TransporteBannerLateral(lInfo);

                if (lTransporte.IdTipo == ConfiguracoesValidadas.IdDoTipo_BannerLateral)
                {
                    if (!lListaBannersDisponiveis.ContainsKey(lTransporte.IdBanner))
                    {
                        lListaBannersDisponiveis.Add(lTransporte.IdBanner, lTransporte);
                    }
                }
                else
                {
                    string lChave = lTransporte.UrlDaPagina.ToLower();

                    if (!lListaBannersDaPagina.ContainsKey(lChave))
                    {
                        lListaBannersDaPagina.Add(lChave, new List<TransporteBannerLateral>());
                    }

                    lListaBannersDaPagina[lChave].Add(lTransporte);
                }
            }

            //complementando a informação dos "filhos" com os "pais":
            foreach (string lChave in lListaBannersDaPagina.Keys)
            {
                foreach (TransporteBannerLateral lBannerFilho in lListaBannersDaPagina[lChave])
                {
                    lBannerFilho.LinkParaArquivo = lListaBannersDisponiveis[lBannerFilho.IdBanner].LinkParaArquivo;
                    lBannerFilho.Link = lListaBannersDisponiveis[lBannerFilho.IdBanner].Link;
                }
            }

            this.BannersLateraisDisponiveis = lListaBannersDisponiveis;
            this.BannersLateraisPorPagina = lListaBannersDaPagina;
        }

        private void CarregarListaDePaginas()
        {
            PaginaResponse lResponse = ServicoPersistenciaSite.BuscarPaginasEVersoes(new PaginaRequest());

            List<string> lListaDePastas = new List<string>();

            bool lPastaInclusa = false;
            string lPasta;

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                lResponse.ListaPagina.Sort(delegate(PaginaInfo x, PaginaInfo y) { return x.Galho.CompareTo(y.Galho); });

                Application["ListaDePaginas"] = lResponse.ListaPagina;

                foreach (PaginaInfo lPagina in lResponse.ListaPagina)
                {
                    lPasta = lPagina.Galho.TrimEnd("/".ToCharArray());

                    if (!lListaDePastas.Contains(lPasta))
                        lListaDePastas.Add(lPasta);

                    while (lPasta.LastIndexOf('/') != -1)
                    {
                        lPasta = lPasta.Substring(0, lPasta.LastIndexOf('/'));

                        if (!lListaDePastas.Contains(lPasta))
                            lListaDePastas.Add(lPasta);
                    }
                }

                lListaDePastas.Sort(delegate(string x, string y) { return x.CompareTo(y); });

                Application["ListaDePaginasComDiretorios"] = lListaDePastas;
            }
            else
            {
                throw new Exception(string.Format("Erro ao buscar lista de páginas: [{0}] [{1}]", lResponse.StatusResposta, lResponse.DescricaoResposta));
            }
        }

        private void CarregarListaDeVersoes()
        {
            BuscarVersoesResponse lResponse = ServicoPersistenciaSite.BuscarVersoes(new BuscarVersoesRequest());

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                Application["ListaDeVersoes"] = lResponse.Versoes;
            }
            else
            {
                throw new Exception(string.Format("Erro ao buscar lista de versões: [{0}] [{1}]", lResponse.StatusResposta, lResponse.DescricaoResposta));
            }
        }

        #endregion

        #region Métodos Públicos

        public string BuscarPaginaPai(string pUrlFilho)
        {
            string lFilho = pUrlFilho.ToLower();

            string lPai = lFilho;

            if (lPai.Contains("/"))
            {
                lPai = lPai.Substring(0, lPai.LastIndexOf('/'));
            }

            while (lPai.Contains("/abas"))
            {
                lPai = lPai.Substring(0, lPai.IndexOf("/abas"));
            }

            return lPai;
        }

        public void MarcarPerformanceMonitor(string pDescricao)
        {
            if (this.PerfMonHabilitado)
            {
                PerfMon.Marcar(this.PerfMonChave, pDescricao);
            }
        }

        public void MarcarPerformanceMonitorFormat(string pDescricao, params object[] pParams)
        {
            MarcarPerformanceMonitor(string.Format(pDescricao, pParams));
        }

        public string HostERaizFormat(string pStringPraAdicionar)
        {
            if (pStringPraAdicionar.StartsWith("/"))
                pStringPraAdicionar = pStringPraAdicionar.TrimStart('/');

            string lRetorno = string.Format("{0}/{1}", this.HostERaiz, pStringPraAdicionar);

            if (!lRetorno.ToLower().Contains("localhost"))
            {
                if (lRetorno.ToLower().Contains("minhaconta/"))
                    lRetorno = lRetorno.Replace("http:", "https:");
            }

            return lRetorno;
        }

        public string RetornarErroAjax(string pMensagem, string[] pListaDeMensagens)
        {
            string lMensagens = "";

            foreach (string lMensagem in pListaDeMensagens)
                lMensagens += Environment.NewLine + lMensagem;

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
                if (!gAcoes.ContainsKey(pAcoes[a]))
                    gAcoes.Add(pAcoes[a].ToUpper(), pFuncoes[a]);

            RotearRespostaAjax();
        }

        private void RotearRespostaAjax()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Acao))
                {
                    string lAcao, lResposta;

                    lAcao = this.Acao.ToUpper();
                    lResposta = "Sem Resposta";

                    Response.Clear();

                    //if (this.UsuarioLogado == null)
                    //{
                    //    Response.Write(RetornarErroAjax(CONST_MENSAGEM_SEM_USUARIO_LOGADO));
                    //}
                    //else
                    //{
                    if (gAcoes.ContainsKey(lAcao))
                    {
                        lResposta = gAcoes[lAcao]();

                        if (lResposta != CONST_RESPOSTA_JA_ENVIADA_PELA_FUNCAO)
                            Response.Write(lResposta);
                    }
                    else
                    {
                        Response.Write(RetornarErroAjax(string.Format("Ação não registrada: [{0}]", this.Acao)));
                    }
                    //}

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

        /// <summary>
        /// Roda javascript depois do load da página; o código fica dentro de uma função, então não definir outras funções por aqui, só executar código.
        /// </summary>
        /// <param name="pJavascript">Javascript pra rodar dentro da função Page_Load_Codebehind()</param>
        public void RodarJavascriptOnLoad(string pJavascript)
        {
            this.JavascriptParaRodarOnLoad += pJavascript + "\r\n\r\n";
        }

        /// <summary>
        /// Exibe um "alert" no Page_Load via javascript
        /// </summary>
        /// <param name="pTipo_IAE">String de estilo da mensagem: 'A' para alerta, 'I' para informação e 'E' para erro.</param>
        /// <param name="pMensagem">Mensagem que será exibida</param>
        /// <param name="pRetornarAoEstadoNormalAposSegundos">Flag que indica se o painel deve voltar ao estado 'normal' depois de alguns segundos de exibição da mensagem.</param>
        /// <param name="pMensagemAdicional">(opcional) Mensagem longa, que aparece na caixa de texto no painel de erro.</param>
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
            }*/

            this.RodarJavascriptOnLoad(string.Format("GradSite_ExibirMensagem(\"{0}\", \"{1}\", {2}{3});"
                                                    , pTipo_IAE
                                                    , lMensagem
                                                    , pRetornarAoEstadoNormalAposSegundos ? "true" : "false"
                                                    , string.IsNullOrEmpty(lMensagemExtendida) ? "" : ", \"" + lMensagemExtendida + "\""));
        }

        /// <summary>
        /// Exibe um "alert" no Page_Load via javascript
        /// </summary>
        /// <param name="pTipo_IAE">String de estilo da mensagem: 'A' para alerta, 'I' para informação e 'E' para erro.</param>
        /// <param name="pExcecao">Exceção disparada</param>
        /// <param name="pRetornarAoEstadoNormalAposSegundos">Flag que indica se o painel deve voltar ao estado 'normal' depois de alguns segundos de exibição da mensagem.</param>
        /// <param name="pMensagemAdicional">(opcional) Mensagem longa, que aparece na caixa de texto no painel de erro.</param>
        public void ExibirMensagemJsOnLoad(Exception pExcecao)
        {
            ExibirMensagemJsOnLoad("E", pExcecao.Message, false, pExcecao.StackTrace);
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
                lLista.ForEach(lDadoSinacor => { lControle.Items.Add(new ListItem() { Text = lDadoSinacor.Value, Value = lDadoSinacor.Id }); });
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

                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<SinacorListaInfo>(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = lResponse.Resultado;

                    Cache[lKey] = lRetorno;
                }
                else
                {
                    string lMensagem = string.Format("Erro em BuscarListaDoSinacor({0}): [{1}]\r\n{2}", pConfiguracaoInfo.Informacao, lResponse.StatusResposta, lResponse.DescricaoResposta);

                    gLogger.Error(lMensagem);

                    throw new Exception(lMensagem);
                }
            }
            else
            {
                lRetorno = Cache[lKey] as List<SinacorListaInfo>;
            }

            return lRetorno;
        }

        public MensagemResponseStatusEnum EnviarEmail(string pDestinatario, string pAssunto, string pNomeArquivo, Dictionary<string, string> pVariaveisEmail, eTipoEmailDisparo pTipoEmailDisparo, List<Gradual.OMS.Email.Lib.EmailAnexoInfo> pAnexos = null, List<string> pDestinatariosOcultos = null)
        {
            /*
            using (var lStreamReader = File.OpenText(this.Server.MapPath(string.Concat("~/Resc/Emails/", pNomeArquivo)))) //--> Carregando o arquivo num StreamReader
            {
                var lStringBuilder = new System.Text.StringBuilder(lStreamReader.ReadToEnd()); //--> Convertendo o arquivo html em texto.

                foreach (KeyValuePair<string, string> item in pVariaveisEmail)
                    lStringBuilder.Replace(item.Key, item.Value); //--> Substituindo as variáveis do conteúdo do e-mail.

                return this.EnviarEmail(pDestinatario, pAssunto, lStringBuilder.ToString(), pTipoEmailDisparo, pAnexos);
            }*/

            string lCaminho;

            string lArquivoOriginal, lArquivoConcatenado;

            gLogger.InfoFormat("PaginaBase.EnviarEmail > [{0}]", pDestinatario);

            lCaminho = Server.MapPath(Path.Combine("~/Resc/Emails/", pNomeArquivo));

            lArquivoOriginal = File.ReadAllText(lCaminho);
            
            gLogger.InfoFormat("PaginaBase.EnviarEmail > Variaveis: [{0}]", JsonConvert.SerializeObject(pVariaveisEmail));

            lArquivoConcatenado = lArquivoOriginal;

            foreach (KeyValuePair<string, string> lItem in pVariaveisEmail)
            {
                gLogger.InfoFormat("PaginaBase.EnviarEmail: Arquivo [{0}], Substituindo [{1}] por [{2}]...", lCaminho, lItem.Key, lItem.Value);

                lArquivoConcatenado = lArquivoConcatenado.Replace(lItem.Key, lItem.Value);
            }

            foreach (KeyValuePair<string, string> lItem in pVariaveisEmail)
            {
                if (!string.IsNullOrEmpty(lItem.Value))
                {
                    if (!lArquivoConcatenado.Contains(lItem.Value))
                    {
                        gLogger.InfoFormat("Atenção! Valor [{0}] não está presente no arquivo final (variável [{1}]), provável erro no arquivo."
                                            , lItem.Value
                                            , lItem.Key);
                    }
                }
            }

            return this.EnviarEmail(pDestinatario, pAssunto, lArquivoConcatenado, pTipoEmailDisparo, pAnexos, pDestinatariosOcultos);
            try
            {
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em PaginaBase.EnviarEmail: [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                throw ex;
            }
        }

        private void CarregarPerfil(TransporteSessaoClienteLogado pClienteBase)
        {
            IServicoPersistenciaCadastro lServico = Ativador.Get<IServicoPersistenciaCadastro>();

            ConsultarEntidadeCadastroRequest<ClienteSuitabilityInfo> lRequest = new ConsultarEntidadeCadastroRequest<ClienteSuitabilityInfo>();
            ConsultarEntidadeCadastroResponse<ClienteSuitabilityInfo> lResponse;

            lRequest.EntidadeCadastro = new ClienteSuitabilityInfo() { IdCliente = pClienteBase.IdCliente.Value };

            lResponse = lServico.ConsultarEntidadeCadastro<ClienteSuitabilityInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado != null && lResponse.Resultado.Count > 0)
                {
                    pClienteBase.PerfilSuitability = lResponse.Resultado[0].ds_perfil;
                    
                    
                }
                else
                {
                    pClienteBase.PerfilSuitability = "n/d";
                }
            }
            else
            {
                string lMensagem = string.Format("Resposta do serviço com erro em PerfilCliente.aspx > BuscarPerfilDoCliente(IdCliente [{0}]) : [{1}]\r\n{2}"
                                                , lRequest.EntidadeCadastro.IdCliente
                                                , lResponse.StatusResposta
                                                , lResponse.DescricaoResposta
                                                );

                throw new Exception(lMensagem);
            }
        }

        public void CarregarClienteEmSessao(string pIdCliente)
        {
            ReceberEntidadeCadastroRequest<SessaoPortalInfo> lRequest = new ReceberEntidadeCadastroRequest<SessaoPortalInfo>();
            ReceberEntidadeCadastroResponse<SessaoPortalInfo> lRetornoSessaoPortal;

            //lRequest.DescricaoUsuarioLogado = string.Format("Portal - IP: [{0}]", this.Request.UserHostAddress);


            if (string.IsNullOrEmpty(pIdCliente))
            {
                if (SessaoClienteLogado != null)
                {
                    //veio do login normal, é um IdLogin
                    lRequest.EntidadeCadastro = new SessaoPortalInfo() { IdLogin = SessaoClienteLogado.IdLogin };

                    gLogger.InfoFormat("PaginaBase.cs - CarregarClienteEmSessao(pIdLogin: [{0}])", lRequest.EntidadeCadastro.IdLogin);

                }
            }
            else
            {
                //veio do cadastro, veio um IdCliente:

                lRequest.EntidadeCadastro = new SessaoPortalInfo() { IdCliente = pIdCliente.DBToInt32() };

                gLogger.InfoFormat("PaginaBase.cs - CarregarClienteEmSessao(pIdCliente: [{0}])", lRequest.EntidadeCadastro.IdCliente);
            }

            lRetornoSessaoPortal = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<SessaoPortalInfo>(lRequest);

            if (lRetornoSessaoPortal.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lRetornoSessaoPortal.EntidadeCadastro.IdCliente != 0 && lRetornoSessaoPortal.EntidadeCadastro.IdLogin != 0)
                {
                    gLogger.InfoFormat("PaginaBase.cs - CarregarClienteEmSessao(pIdCliente: [{0}]/[{1}] | pIdLogin: [{2}]/[{3}]) OK! CpfCnpj: [{4}], Email: [{5}], CodigoPrincipal: [{6}]"
                                        , lRequest.EntidadeCadastro.IdCliente
                                        , lRetornoSessaoPortal.EntidadeCadastro.IdCliente
                                        , lRequest.EntidadeCadastro.IdLogin
                                        , lRetornoSessaoPortal.EntidadeCadastro.IdLogin
                                        , lRetornoSessaoPortal.EntidadeCadastro.DsCpfCnpj
                                        , lRetornoSessaoPortal.EntidadeCadastro.DsEmailRetorno
                                        , lRetornoSessaoPortal.EntidadeCadastro.CdCodigoPrincipal);

                    SessaoClienteLogado.CpfCnpj             = lRetornoSessaoPortal.EntidadeCadastro.DsCpfCnpj;
                    SessaoClienteLogado.Email               = lRetornoSessaoPortal.EntidadeCadastro.DsEmailRetorno;
                    SessaoClienteLogado.IdCliente           = lRetornoSessaoPortal.EntidadeCadastro.IdCliente;
                    //SessaoClienteLogado.IdLogin             = pIdCliente.DBToInt32();   //lRetornoSessaoPortal.EntidadeCadastro.IdLogin; //é pra ser o mesmo.
                    SessaoClienteLogado.CodigoPrincipal     = lRetornoSessaoPortal.EntidadeCadastro.CdCodigoPrincipal.DBToString();

                    SessaoClienteLogado.AssessorPrincipal   = lRetornoSessaoPortal.EntidadeCadastro.CdAssessorPrincipal.DBToString();
                    SessaoClienteLogado.Passo               = lRetornoSessaoPortal.EntidadeCadastro.StPasso;
                    SessaoClienteLogado.NascimentoFundacao  = lRetornoSessaoPortal.EntidadeCadastro.DtNascimentoFundacao;
                    SessaoClienteLogado.TipoPessoa          = lRetornoSessaoPortal.EntidadeCadastro.TpPessoa == "J" ? TransporteSessaoClienteLogado.EnumTipoPessoa.Juridica : TransporteSessaoClienteLogado.EnumTipoPessoa.Fisica;
                    //SessaoClienteLogado.Senha               = Crypto.CalculateMD5Hash(lRetornoSessaoPortal.EntidadeCadastro.CdSenha);

                    SessaoClienteLogado.NumeroDiasAcesso    = (DateTime.Now - lRetornoSessaoPortal.EntidadeCadastro.DtPasso1).Days;
                    SessaoClienteLogado.Nome                = lRetornoSessaoPortal.EntidadeCadastro.DsNome.ToStringFormatoNome();

                    SessaoClienteLogado.DataDeUltimoLogin = lRetornoSessaoPortal.EntidadeCadastro.DtUltimoLogin;

                    this.CarregarPerfil(SessaoClienteLogado);

                    switch (lRetornoSessaoPortal.EntidadeCadastro.StPasso)
                    {
                        case 1:
                        case 2:

                            if (this.SessaoClienteLogado.NumeroDiasAcesso.Value <= 30)
                            {
                                this.SessaoClienteLogado.TipoAcesso = TransporteSessaoClienteLogado.EnumTipoCliente.VisitanteAte30Dias;
                            }
                            else
                            {
                                this.SessaoClienteLogado.TipoAcesso = TransporteSessaoClienteLogado.EnumTipoCliente.VisitanteExpirado;
                            }

                            break;

                        case 3:

                            if (this.SessaoClienteLogado.NumeroDiasAcesso.Value <= 30)
                            {
                                this.SessaoClienteLogado.TipoAcesso = TransporteSessaoClienteLogado.EnumTipoCliente.Cadastrado;
                            }
                            else
                            {
                                this.SessaoClienteLogado.TipoAcesso = TransporteSessaoClienteLogado.EnumTipoCliente.VisitanteExpirado;
                            }

                            break;

                        case 4:

                            this.SessaoClienteLogado.TipoAcesso = TransporteSessaoClienteLogado.EnumTipoCliente.CadastradoEExportado;

                            break;
                    }
                }
                else
                {
                    gLogger.InfoFormat("PaginaBase.cs - CarregarClienteEmSessao: Aparentemente, não existe cadastro para esse 'cliente' na tabela tb_cliente, somente na tb_login; tratando como um login administrativo...");
                }
            }
            else
            {
                string lMensagem = string.Format("Resposta com erro do serviço ServicoPersistenciaCadastro.ReceberEntidadeCadastro<SessaoPortalInfo>(pIdCliente: [{0}]) - [{1}]\r\n{2}"
                                                , lRequest.EntidadeCadastro.IdLogin
                                                , lRetornoSessaoPortal.StatusResposta
                                                , lRetornoSessaoPortal.DescricaoResposta);

                gLogger.Error(lMensagem);

                throw new Exception(lMensagem);
            }
        }

        public void BuscarCodigoDeSessaoParaUsuarioLogado()
        {
            string lMensagem;

            AutenticarUsuarioRequest  lRequestAutenticacao;
            AutenticarUsuarioResponse lResponseAutenticacao;

            lRequestAutenticacao = new AutenticarUsuarioRequest();

            lRequestAutenticacao.Email = SessaoClienteLogado.Email;
            lRequestAutenticacao.Senha = SessaoClienteLogado.Senha;

            lRequestAutenticacao.IP = Request.ServerVariables["REMOTE_ADDR"];

            try
            {
                gLogger.InfoFormat("Buscando novo código de sessão para [{0}]", lRequestAutenticacao.Email);

                lResponseAutenticacao = this.ServicoSeguranca.AutenticarUsuario(lRequestAutenticacao);
            }
            catch (System.ServiceModel.CommunicationObjectFaultedException)
            {
                Application["ServicoSeguranca"] = null;

                lResponseAutenticacao = ServicoSeguranca.AutenticarUsuario(lRequestAutenticacao);
            }

            if (lResponseAutenticacao.StatusResposta == Gradual.OMS.Library.MensagemResponseStatusEnum.OK)
            {
                gLogger.InfoFormat("Código de sessão para [{0}] ok: [{1}]", lRequestAutenticacao.Email, lResponseAutenticacao.Sessao.CodigoSessao);

                this.SessaoClienteLogado.CodigoDaSessao = lResponseAutenticacao.Sessao.CodigoSessao;
            }
            else
            {
                this.SessaoClienteLogado.CodigoDaSessao = "SEM_ACESSO";

                lMensagem = string.Format("Retorno do serviço de segurança com erro em BuscarCodigoDeSessaoParaUsuarioLogado({0}) [{1}] [{2}]"
                                            , lRequestAutenticacao.Email
                                            , lResponseAutenticacao.StatusResposta
                                            , lResponseAutenticacao.DescricaoResposta);

                gLogger.Error(lMensagem);
            }
        }

        public bool ValidarSessao()
        {
            if (this.SessaoClienteLogado == null)
            {
                Session["RedirecionamentoPorFaltaDeLogin"] = Request.Url.PathAndQuery;

                this.Response.Redirect(HostERaizFormat("MinhaConta/Login.aspx"));

                return false;
            }

            return true;
        }

        private bool VerificarClienteComSuitability()
        {
            ConsultarEntidadeCadastroRequest<ClienteSuitabilityInfo> lRequest = new ConsultarEntidadeCadastroRequest<ClienteSuitabilityInfo>();
            ConsultarEntidadeCadastroResponse<ClienteSuitabilityInfo> lResponse;

            lRequest.DescricaoUsuarioLogado = string.Format("Tela de Login do Portal - IP: {0}", Request.UserHostAddress);

            lRequest.EntidadeCadastro = new ClienteSuitabilityInfo();

            lRequest.EntidadeCadastro.IdCliente = this.SessaoClienteLogado.IdCliente.Value;

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteSuitabilityInfo>(lRequest);

            if(lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                return (lResponse.Resultado.Count > 0);
            }
            else
            {
                string lMensagem = string.Format("Erro em PaginaBase > VerificarClienteComSuitability(IdCliente [{0}]): [{1}]\r\n{2}"
                                                 , lRequest.EntidadeCadastro.IdCliente
                                                 , lResponse.StatusResposta
                                                 , lResponse.DescricaoResposta);

                gLogger.ErrorFormat(lMensagem);

                throw new Exception(lMensagem);
            }
        }

        private EstruturaInfo BuscarEstruturaRequisitada(string pURL, string pVersao = "")
        {
            EstruturaInfo lEstruturaDaPagina = null;

            foreach (PaginaInfo lPagina in this.ListaDePaginas)
            {
                if (lPagina.DescURL.ToLower() == pURL.ToLower())
                {
                    this.NomeDaPagina = lPagina.NomePagina;

                    List<EstruturaInfo> lEstruturas = null;

                    if (string.IsNullOrEmpty(pVersao))
                    {
                        lEstruturas = lPagina.VersaoPublicada.ListaEstrutura;
                    }
                    else
                    {
                        foreach (VersaoInfo lInfo in lPagina.Versoes)
                        {
                            if (lInfo.CodigoDeIdentificacao == pVersao)
                            {
                                lEstruturas = lInfo.ListaEstrutura;

                                break;
                            }
                        }
                    }

                    if (lEstruturas == null)
                    {
                        throw new Exception("SEM_ESTRUTURA");
                    }

                    this.TiposDeUsuarioLogadoDisponiveis.Clear();
                    this.EstruturaDaPaginaParaTodosTipos = false;

                    for (int i = 0; i < lEstruturas.Count; i++)
                    {
                        this.TiposDeUsuarioLogadoDisponiveis.Add(lEstruturas[i].TipoUsuario);

                        if (lEstruturas[i].TipoUsuario == 1)
                        {
                            this.EstruturaDaPaginaParaTodosTipos = true;

                            lEstruturaDaPagina = lEstruturas[i];

                            break;
                        }
                        else if (lEstruturas[i].TipoUsuario == this.TipoDeUsuarioLogado)
                        {
                            lEstruturaDaPagina = lEstruturas[i];
                        }
                    }
                    
                    if (lEstruturaDaPagina == null)
                    {
                        throw new Exception("SEM_ESTRUTURA_USUARIO");
                    }

                    if (lEstruturaDaPagina != null && (lEstruturaDaPagina.ListaWidget == null || lEstruturaDaPagina.ListaWidget.Count == 0))
                    {
                        WidgetResponse lRespWid;
                        WidgetRequest lReqWid = new WidgetRequest();

                        lReqWid.Widget = new WidgetInfo();
                        lReqWid.Widget.CodigoEstrutura = lEstruturaDaPagina.CodigoEstrutura;

                        lRespWid = this.ServicoPersistenciaSite.SelecionarWdiget(lReqWid);

                        if (lRespWid.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lEstruturaDaPagina.ListaWidget = lRespWid.ListaWidget;
                        }
                        else
                        {
                            gLogger.ErrorFormat("Erro ao carregar Widgets da Estrutura [{0}] > {1}\r\n{2}"
                                    , lEstruturaDaPagina.CodigoEstrutura
                                    , lRespWid.StatusResposta
                                    , lRespWid.DescricaoResposta);
                        }
                    }

                    this.VersaoPublicada = lPagina.VersaoPublicada.CodigoDeIdentificacao;

                    if (lEstruturaDaPagina != null)
                    {
                        this.Versao = lEstruturaDaPagina.IdentificadorVersao;
                    }

                    break;
                }
            }

            return lEstruturaDaPagina;
        }

        protected void CarregarPagina(RenderizadorDeWidgets pRenderizador, string pURL, string pVersao = "", string pIdRequisitada = "")
        {
            List<WidgetBase> lLista = new List<WidgetBase>();

            if (string.IsNullOrEmpty(pIdRequisitada))
            {
                string lMensagem = "";

                EstruturaInfo lEstruturaDaPagina = null;

                try
                {
                    lEstruturaDaPagina = BuscarEstruturaRequisitada(pURL, pVersao);
                }
                catch (Exception ex)
                {
                    if (ex.Message == "SEM_ESTRUTURA")
                    {
                        lMensagem = string.Format("Página sem estrutura para versão [{0}].", pVersao);
                    }
                    else if (ex.Message == "SEM_ESTRUTURA_USUARIO")
                    {
                        lMensagem = string.Format("Usuário logado como tipo [{0}], porém a página não tem Estrutura para esse tipo.", this.TipoDeUsuarioLogado);
                    }
                    else
                    {
                        throw ex;
                    }
                }

                if (lEstruturaDaPagina != null)
                {
                    this.PaginaMaster.IdDaPagina = lEstruturaDaPagina.CodigoPagina;

                    this.PaginaMaster.IdDaEstrutura = lEstruturaDaPagina.CodigoEstrutura;   //utilizado somente na clonagem de estruturas

                    //this.NomeDaPagina = lEstruturaDaPagina.

                    string lIdCache = string.Format("{0}-{1}-{2}", this.PaginaMaster.IdDaPagina, this.PaginaMaster.IdDaEstrutura, this.ModuloCMSSeraExibido());

                    //MarcarPerformanceMonitorFormat("CarregarPaginas() > Preparando-se para renderizar estrutura [{0}]...", this.PaginaMaster.IdDaEstrutura);

                    this.IDEstruturaCorrente = lEstruturaDaPagina.CodigoEstrutura; //Guarda em sessão o ID da Estrutura

                    this.QuantidadeWidgetMaiorQueZero = lEstruturaDaPagina.ListaWidget.Count > 0;

                    if (!this.CacheDePaginas.ContainsKey(lIdCache))
                    {
                        foreach (var item in lEstruturaDaPagina.ListaWidget)
                        {
                            lLista.Add((WidgetBase)this.InstanciarWidget(item));
                        }

                        pRenderizador.RenderizarHabilitandoCMS = this.ModuloCMSSeraExibido();

                        pRenderizador.ExisteUsuarioLogado = (this.SessaoClienteLogado != null);

                        pRenderizador.Widgets = lLista;

                        pRenderizador.HostERaiz = this.HostERaiz;

                        pRenderizador.VersaoDaEstrutura = lEstruturaDaPagina.IdentificadorVersao;

                        MarcarPerformanceMonitorFormat("CarregarPaginas() > [{0}] widgets para renderizar; RenderizarHabilitandoCMS:[{1}], ExisteUsuarioLogado:[{2}]"
                                                        , lLista.Count
                                                        , pRenderizador.RenderizarHabilitandoCMS
                                                        , pRenderizador.ExisteUsuarioLogado);

                        pRenderizador.DataBind();

                        this.CacheDePaginas.Add(lIdCache, pRenderizador.ConteudoHTML);
                    }
                    else
                    {
                        //MarcarPerformanceMonitorFormat("CarregarPaginas() > HTML pego do cache de páginas [{0}]...", lIdCache);

                        pRenderizador.Widgets = null;   //força ele a pegar o ConteudoHTML

                        pRenderizador.ConteudoHTML = this.CacheDePaginas[lIdCache];

                        pRenderizador.DataBind();
                    }

                    //MarcarPerformanceMonitor("CarregarPaginas() > Renderização finalizada.");
                }
                else
                {
                    gLogger.Info(lMensagem);

                    pRenderizador.ConteudoHTML = string.Format("<span style='display:none'>{0}</span>", lMensagem);

                    pRenderizador.DataBind();
                }
            }
            else
            {
                CarregarPaginaConteudo(pRenderizador, pURL, pIdRequisitada);
            }
        }

        protected void CarregarPaginaConteudo(RenderizadorDeWidgets pRenderizador, string pURL, string pIdRequisitada)
        {
            ConteudoRequest lRequestConteudo = new ConteudoRequest();
            ConteudoResponse lResponseConteudo;

            int lId;

            if (int.TryParse(pIdRequisitada, out lId))
            {
                lRequestConteudo.Conteudo = new ConteudoInfo() { CodigoConteudo = lId };

                lResponseConteudo = this.ServicoPersistenciaSite.SelecionarConteudo(lRequestConteudo);

                if (lResponseConteudo.StatusResposta == MensagemResponseStatusEnum.OK && lResponseConteudo.ListaConteudo.Count > 0)
                {
                    string lValor;

                    lValor = lResponseConteudo.ListaConteudo[0].ConteudoHtml;

                    lValor = lValor.Replace("\\n", Environment.NewLine);

                    pRenderizador.ConteudoHTML = lValor;

                    pRenderizador.DataBind();
                }
                else
                {
                    gLogger.ErrorFormat("Resposta com erro do ServicoPersistenciaSite.CarregarPaginaConteudo(pCodigoConteudo: [{0}]) em PaginaBase.CarregarPaginas > [{1}]\r\n{2}"
                                        , pIdRequisitada
                                        , lResponseConteudo.StatusResposta
                                        , lResponseConteudo.DescricaoResposta);
                }
            }
            else
            {
                gLogger.ErrorFormat("Erro em ServicoPersistenciaSite.CarregarPaginaConteudo() : Request['id'] não é um int32 válido: [{0}]", Request["id"]);
            }
        }

        protected void CarregarItemUnico(RenderizadorDeWidgets pRenderizador, string pIdConteudo, string pPropriedadeParaRenderizar = "ConteudoHTML")
        {

        }

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

        public void CarregarProdutosAdquiridosDoCliente(bool pRecarregar)
        {
            if (SessaoClienteLogado != null)
            {
                if (SessaoClienteLogado.ProdutosAdquiridos == null || pRecarregar)
                {
                    IServicoPersistenciaSite lServico;

                    lServico = InstanciarServicoDoAtivador<IServicoPersistenciaSite>();

                    BuscarProdutosDoClienteRequest lRequest = new BuscarProdutosDoClienteRequest();
                    BuscarProdutosDoClienteResponse lResponse;

                    lRequest.CpfCnpj = SessaoClienteLogado.CpfCnpj;

                    lResponse = lServico.BuscarProdutosDoCliente(lRequest);

                    if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        SessaoClienteLogado.ProdutosAdquiridos = lResponse.ListaDeProdutos;
                    }
                    else
                    {
                        SessaoClienteLogado.ProdutosAdquiridos = new List<Gradual.Site.DbLib.Dados.MinhaConta.Comercial.ProdutoCompradoInfo>();

                        gLogger.ErrorFormat("Resposta com erro do IServicoPersistenciaSite.BuscarProdutosDoCliente(pCpfCnpj: [{0}]) em PaginaBase.CarregarProdutosAdquiridosDoCliente > [{1}]\r\n{2}"
                                            , lRequest.CpfCnpj
                                            , lResponse.StatusResposta
                                            , lResponse.DescricaoResposta);
                    }
                }
            }
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

        public void AcessarCalculadoraIR()
        {
            if (SessaoClienteLogado != null)
            {
                try
                {
                    string lChave0, lChave1, lChave2, lChave3;

                    lChave0 = ""; //ID da corretora definido pelo mycapital
                    lChave1 = ""; //Código Bovespa do cliente + digitito verificador.
                    lChave2 = ""; //Email do cliente com criptografia em MD5, após a geração do md5 remover o caracter de acordo com a hora da geração.
                    lChave3 = ""; //Junção dos campos chave+chave1+Data e hora da autenticação no formato (ddMMyyyyHH) com criptografia em MD5, após a geração do md5 remover o caracter de acordo com a hora da geração

                    lChave0 = ConfiguracoesValidadas.CalculadoraIR_IDCorretora;
                    lChave1 = string.Format("{0}{1}", SessaoClienteLogado.CodigoPrincipal, ToCodigoClienteFormatado(SessaoClienteLogado.CodigoPrincipal));

                    lChave2 = ConfiguracoesValidadas.CalculadoraIR_EmailIR;
                    lChave2 = Gradual.Intranet.Servicos.BancoDeDados.Negocio.Crypto.CalculateMD5Hash(lChave2).ToUpper();
                    lChave2 = lChave2.Remove((DateTime.Now.Hour - 1), 1);

                    lChave3 = lChave0 + lChave1 + DateTime.Now.ToString("ddMMyyyyHH");
                    lChave3 = Gradual.Intranet.Servicos.BancoDeDados.Negocio.Crypto.CalculateMD5Hash(lChave3).ToUpper();
                    lChave3 = lChave3.Remove((DateTime.Now.Hour - 1), 1);

                    string lURL = string.Format("{0}?chave={1}&chave1={2}&chave2={3}&chave3={4}"
                                                , ConfiguracoesValidadas.CalculadoraIR_SiteMyCapital
                                                , lChave0
                                                , lChave1
                                                , lChave2
                                                , lChave3);

                    string lJs = string.Format("window.open('{0}' , 'mycapital', 'width=800,height=600, toolbar=yes, menubar=yes,status=yes,scrollbars=yes,resizable=yes');", lURL);

                    RodarJavascriptOnLoad(lJs);
                }
                catch (Exception ex)
                {
                    string lMensagem;

                    lMensagem = string.Format("Erro em PaginaBase.AcessarCalculadoraIR(): [{0}]\r\n{1}"
                                                , ex.Message
                                                , ex.StackTrace);

                    gLogger.Error(lMensagem);

                    ExibirMensagemJsOnLoad("E", "Erro ao gerar código de acesso para o sistema Calculadora IR", false, lMensagem);
                }
            }
        }

        public bool ModuloCMSSeraExibido(bool pExcluirPaginaDoMinhaConta = true)
        {
            bool lRetorno = false;

            bool lPassou = ((pExcluirPaginaDoMinhaConta && !this.EPaginaDoMinhaConta) || (!pExcluirPaginaDoMinhaConta && this.EPaginaDoMinhaConta));

            if (SessaoClienteLogado != null && lPassou)
            {
                if (SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarCMS))
                {
                    lRetorno = true;
                }
                else
                {
                    string lURL = Request.Url.ToString().ToLower();

                    if(lURL.Contains("analiseseconomicas.aspx") && SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarAnaliseEconomica))
                        lRetorno = true;

                    if(lURL.Contains("analisesfundamentalistas.aspx") && SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarAnaliseFundamentalista))
                        lRetorno = true;

                    if(lURL.Contains("analisesgraficas.aspx") && SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarAnaliseGrafica))
                        lRetorno = true;

                    if(lURL.Contains("carteirasrecomendadas.aspx") && SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarCarteirasRecomendadas))
                        lRetorno = true;

                    if(lURL.Contains("nikkei") && SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarNikkei))
                        lRetorno = true;

                    if(lURL.Contains("gradiusgestao.aspx") && SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarGradiusGestao))
                        lRetorno = true;
                }
            }

            return lRetorno;
        }

        public string GerarSHA1(string pSenhaOriginal)
        {
            //criando nova instância de um hasher sha1
            SHA1 lSha1Hasher = SHA1.Create();

            //criando um gerador de strings
            StringBuilder lGerarString = new StringBuilder();

            //criando um vetor de bytes que recebera
            //em bytes o valor da senha original
            byte[] lVetor = Encoding.Default.GetBytes(pSenhaOriginal);

            //calculando o hash dos bytes e inserindo no próprio vetor
            lVetor = lSha1Hasher.ComputeHash(lVetor);

            //repita para cada elemento do vetor gerar uma
            //string convertida para hexadecimal
            foreach (byte lItem in lVetor)
            {
                lGerarString.Append(lItem.ToString("x2"));
            }

            //retornar a string em forma de string e em maiúscula
            return lGerarString.ToString();
        }

        public void LimparCacheDeBanners()
        {
            this.BannersLateraisDisponiveis = null; //pra recarregar depois
            this.BannersLateraisPorPagina = null; //pra recarregar depois
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

        protected List<OrdemInfo> GetOrdem (string ClOrdId)
        {
            var lDb = new PersistenciaOrdens();

            return lDb.BuscarOrdem(ClOrdId);
        }

        public void ZerarCacheDePaginas()
        {
            this.ListaDePaginas = null;
            this.ListaDePaginasComDiretorios = null;
        }

        public void LimparCacheParaPagina(int pIdPagina)
        {
            List<string> lRemover = new List<string>();

            foreach (string lChave in CacheDePaginas.Keys)
            {
                if (lChave.StartsWith(string.Format("{0}-", pIdPagina)))
                {
                    lRemover.Add(lChave);
                }
            }

            foreach (string lChave in lRemover)
            {
                this.CacheDePaginas.Remove(lChave);
            }
        }

        /// <summary>
        /// Lista do sinacor com a opção "SELECIONE" adicionada
        /// </summary>
        /// <param name="pLista">Lista para adicionar o item</param>
        /// <param name="pRemoverRepeticoes">Opcional, remove instâncias repetidas de um item</param>
        /// <returns></returns>
        public object ListaComSelecione(List<SinacorListaInfo> pLista, string pRemoverRepeticoes = null)
        {
            if (pLista.Count > 0)
            {
                if (pLista[0].Id != "")
                {
                    pLista.Insert(0, new SinacorListaInfo() { Id="", Value ="[SELECIONE]" });
                }

                if (!string.IsNullOrEmpty(pRemoverRepeticoes))
                {
                    int lPrimeiroItem = -1;
                    List<int> lRemover = new List<int>();

                    for (int a = 0; a < pLista.Count; a++)
                    {
                        if (pLista[a].Value == pRemoverRepeticoes)
                        {
                            if (lPrimeiroItem == -1)
                            {
                                //achou o primeiro
                                lPrimeiroItem = a;
                            }
                            else
                            {
                                lRemover.Add(a);
                            }
                        }
                    }

                    for (int a = lRemover.Count - 1; a >= 0; a--)
                    {
                        pLista.RemoveAt(lRemover[a]);
                    }
                }
            }

            return pLista;
        }

        public void RetirarClienteDaSessao()
        {
            gLogger.InfoFormat("Deslogando cliente ID [{0}] Código [{1}] Sessão [{2}]", SessaoClienteLogado.IdCliente, SessaoClienteLogado.CodigoPrincipal, SessaoClienteLogado.CodigoDaSessao);

            try
            {
                string lCodigoSessao = SessaoClienteLogado.CodigoDaSessao;
                string lRequestStr = "";

                HttpWebRequest lRequest;
                HttpWebResponse lResponse;

                MensagemResponseBase lRespostaLogout;

                lRespostaLogout = ServicoSeguranca.EfetuarLogOut(new MensagemRequestBase() { CodigoSessao = lCodigoSessao });

                gLogger.InfoFormat("Resposta do serviço: [{0}] [{1}]", lRespostaLogout.StatusResposta, lRespostaLogout.DescricaoResposta);

                try
                {
                    foreach (string lIpHb in ConfiguracoesValidadas.IPsDeLogoutHB)
                    {
                        lRequestStr = string.Format("http://{0}/backend/asyncgeral.aspx?acao=r&s={1}", lIpHb, lCodigoSessao);

                        lRequest = (HttpWebRequest)WebRequest.Create(lRequestStr);

                        lRequest.Timeout = 2000;

                        lResponse = (HttpWebResponse)lRequest.GetResponse();

                        StreamReader lReader = new StreamReader(lResponse.GetResponseStream());

                        string lResponseContent = lReader.ReadToEnd();

                        if (lResponse.StatusCode == HttpStatusCode.OK)
                        {
                            //nevermind I guess...
                            gLogger.InfoFormat("Request para deslogar retornada de [{0}] OK: [{1}]", lRequestStr, lResponseContent);
                        }
                        else
                        {
                            gLogger.ErrorFormat("Erro em RetirarClienteDaSessao > WebResponse({0}): [{1}] [{2}]", lRequestStr, lResponse.StatusCode, lResponseContent);
                        }
                    }
                }
                catch (Exception exweb)
                {
                    gLogger.ErrorFormat("Erro em RetirarClienteDaSessao > WebRequest({0}): [{1}]\r\n{2}", lRequestStr, exweb.Message, exweb.StackTrace);
                }

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em RetirarClienteDaSessao(): [{0}]\r\n{1}", ex.Message, ex.StackTrace);
            }

            SessaoClienteLogado = null;

            Session["RedirecionamentoPorFaltaDeLogin"] = null;

            Session["TipoDeClientePreview"] = null;
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

        public double CalcularTaxaDI(string Instrumento, double taxaOperada, double lTaxaMercado)
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

        #endregion

        #region Event Handlers

        protected void Page_Init(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();

            //string lAllow = "https://";

            if (Request.ServerVariables["HTTPS"] == "off" && ConfiguracoesValidadas.HostDoSite.StartsWith("https://"))
            {
                Response.Redirect(Request.Url.ToString().Replace("http://", "https://"));
            }

            //Response.AppendHeader("Access-Control-Allow-Origin", string.Format("{0}www.gradualinvestimentos.com.br", lAllow));
            //Response.AppendHeader("Access-Control-Allow-Credentials", "true");

            string lUrl = string.Format("{0}", Request.Url.Query);

            if (lUrl.Contains("?"))
            {
                lUrl = lUrl.Substring(lUrl.LastIndexOf('?') + 1);

                string[] lParametros = lUrl.Split('&');

                string[] lValores;

                for (int a = 0; a < lParametros.Length; a++)
                {
                    lValores = lParametros[a].Split('=');

                    if (lValores.Length == 2)
                    {
                        if (lValores[0].ToLower() == "perfmon" && !string.IsNullOrEmpty(lValores[1]))
                        {
                            this.PerfMonChave = lValores[1];
                        }
                        
                        if (lValores[0].ToLower() == "cache" && lValores[1] == "0")
                        {
                            ServicoPersistenciaSite.LimparCache(null);

                            this.CacheDePaginas = null;
                        }
                    }
                }

            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Literal litJavascript = (Literal)this.FindControl("litJavascriptOnLoad");

            if(litJavascript == null && this.PaginaMaster != null)
                litJavascript = (Literal)this.PaginaMaster.FindControl("litJavascriptOnLoad");

            if(litJavascript != null)
                litJavascript .Text = this.JavascriptParaRodarOnLoad;

            if (this.PerfMonHabilitado && this.PaginaMaster != null)
            {
                this.PaginaMaster.RelatorioDePerformance = PerfMon.RenderizarRelatorio(this.PerfMonChave, true);
            }
        }

        #endregion
    }
}
