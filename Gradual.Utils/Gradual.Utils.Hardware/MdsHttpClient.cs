using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BayeuxClient;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using MdsBayeuxClient.Classes;
using System.ComponentModel;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace MdsBayeuxClient
{
	public class QuedaDeConexaoEventArgs
	{
		public QuedaDeConexaoEventArgs() { }

		public System.Exception Excecao { get; set; }
		public bool Conectado { get; set; }
	}

	public class MdsControleEventArgs : EventArgs
	{
		public static string ACTION_FORCE_APPCLOSE = "1";

		public string Action { get; set; }
	}

	/// <summary>
	///     Classe que encapsula todas as rotinas de conexão com o servidor de sStreamer de cotação
	/// </summary>
	public class MdsHttpClient
    {
        #region Vaiaveis Globais

        private System.Globalization.CultureInfo Cultura = new System.Globalization.CultureInfo("pt-BR");

        int DURACAO_POST = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["DURACAO_POST"]);
        int DURACAO_RESPONSE = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["DURACAO_RESPONSE"]);
        int DURACAO_READ = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["DURACAO_READ"]);
        int FILA = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["FILA"]);

		private bool conectado = false;
		private bool conectando { get; set; }
        private bool sucesso = false;
		private bool reassinar = false;
		private bool procInicializados = false;
		private bool bKeepRunning = false;

		public static bool Conectado { get { return MdsHttpClient.Instance.conectado; } }

		private const string ASSINATURA_DESTAQUES = "DESTAQUES";
		private const string ASSINATURA_LEILAO = "LEILAO";
    	private const string ASSINATURA_ACOMPANHAMENTO = "ACOMPANHAMENTO";
		private const string ASSINATURA_NOTICIAS = "NOTICIAS";

		private static BayeuxHandshake handshake;
        public static BayeuxHandshake HandShake { get { return handshake;  } }

		private static String uriBase;
        public static String UriBase { get { return uriBase; } }

        private BayeuxClient.Classes.Fila<string> filaCancelarNegocios { get; set; }

		public delegate void OnOfertasHandler(object sender, MdsOfertasEventArgs e);
		public delegate void OnRankingHandler(object sender, MdsRankingEventArgs e);
		public delegate void OnLivroNegociosHandler(object sender, MdsLivroNegociosEventArgs e);
		public delegate void OnTopoLivroOfertasHandler(object sender, DsTopoLivro e);
		public delegate void OnNegociosHandler(object sender, MdsNegociosEventArgs e);
		public delegate void OnDestaquesHandler(object sender, MdsDestaquesEventArgs e);
		public delegate void OnAcompanhamentoOrdensHandler(object sender, MdsAcompanhamentoOrdensEventArgs e);
		public delegate void OnQuedaDeConexaoHandler(object sender, QuedaDeConexaoEventArgs ex);
		public delegate void OnConexaoEfetuadaHandler(object sender);
		public delegate void OnSnapshotHandler(object sender, String instrumento);
		public delegate void OnCancelamentoHandler(object sender, String instrumento);
		public delegate void OnMdsControleHandler(object sender, MdsControleEventArgs args);
		public delegate void OnNegociosDestaqueHandler(object sender, MdsNegociosDestaqueEventArgs e);
		public delegate void OnAcompanhamentoLeilaoHandler(object sender, MdsAcompanhamentoLeilaoEventArgs e);
		public delegate void OnResumoCorretorasHandler(object sender, MdsResumoCorretorasEventArgs e);
		public delegate void OnNoticiasHandler(object sender, MdsNoticiasEventArgs e);
		public delegate void OnOfertasAgregadoHandler(object sender, MdsOfertasEventArgs e);
		public delegate void OnSnapshotAgregadoHandler(object sender, String instrumento);
		public delegate void OnCancelamentoAgregadoHandler(object sender, String instrumento);

        public event OnMdsControleHandler OnMdsControleEvent;

		public DsRankingDeCorretoras DsRankingDeCorretoras { get; set; }
		public DsAcompanhamentoOrdens DsAcompanhamentoOrdens { get; set; }

		public System.Collections.Generic.Dictionary<String, LivroDeOfertas> repositorioOfertasAgregado;
        

        private static MdsHttpClient _self = null;
        private static System.Threading.Mutex _mutex = new System.Threading.Mutex();
        private static System.Threading.Mutex _conn = new System.Threading.Mutex();
        private static Thread thrMonitorConexao = null;
        private DateTime lastSonda;

        private List<string> lstLNG2Remove = new List<string>();
        private List<string> lstLOF2Remove = new List<string>();

        private OnAcompanhamentoOrdensHandler gAcompanhamentoEventHandler = null;

        private Dictionary<string, OnAcompanhamentoOrdensHandler> gDicAcompanhamentoHandler = new Dictionary<string, OnAcompanhamentoOrdensHandler>();

        BayeuxConnect connect;

        object Sync = new Object();

        public List<OrdemInfo> gListaOrdens = new List<OrdemInfo>();

        #endregion

        private string _session = "";
		/// <summary>
		///   Sessão do cliente no servidor
		/// </summary>
		public string Session 
		{
			get { return _session; }
		}



		public static MdsHttpClient Instance
		{
			get
			{
				return GetInstance();
			}
		}

		private static MdsHttpClient GetInstance()
		{
			_mutex.WaitOne();

            if (_self == null)
            {
                _self = new MdsHttpClient();
            }

			_mutex.ReleaseMutex();

			return _self;
		}

		public MdsHttpClient()
		{
            System.Threading.Thread.CurrentThread.Name = "MdsHttpClient.Main";
            Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "***************************************************************************************************", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
            Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Debug, "Criação do MdsHttpClient", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
			DsAcompanhamentoOrdens = new DsAcompanhamentoOrdens();
			repositorioOfertasAgregado = new Dictionary<string, LivroDeOfertas>();
		}


		public BayeuxHandshake Conecta(String uriBase)
		{
            _conn.WaitOne();

            Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "Solicitação de conexão recebida pelo MdsHttpClient", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

			bKeepRunning = true;

			lastSonda = DateTime.Now;

			MdsHttpClient.uriBase = uriBase;

			if (thrMonitorConexao == null)
			{
                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "Criando thread de monitoramento de conexão", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                

				thrMonitorConexao = new Thread(new ThreadStart(_MonitorConexao));
				thrMonitorConexao.Name = "MdsHttpClient.MonitorConexao";
				thrMonitorConexao.Start();
			}

            while (!conectado)
            {
                System.Threading.Thread.Sleep(50);
            }

            _conn.ReleaseMutex();

			//TODO: Sincronizar para obter o handshake correto
			return handshake;
		}

		public void Desconecta()
		{
			bKeepRunning = false;

			while (thrMonitorConexao.IsAlive)
			{
				Thread.Sleep(100);
			}

			thrMonitorConexao = null;
            conectado = false;
            conectando = false;
            _self = null;
			//TODO: variaveis de controle do estado
		}

		public void _MonitorConexao()
		{

			while (bKeepRunning)
			{
				if (!conectado && !conectando)
				{
					_Conecta(MdsHttpClient.uriBase);

					IniciaThreadsProcessamento();
				}

				if (reassinar && conectado)
				{
					reassinar = false;
					Reassina();
				}

				//TODO: incluir monitoracao da sonda. Vai que....

				Thread.Sleep(250);
			}

            Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "***************************************************************************************************", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
            Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "Fim da monitoracao da conexao", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
            Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "***************************************************************************************************", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
		}

        void OnQuedaDeConexao(object sender, QuedaDeConexaoEventArgs ex)
        {
            Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "***************************************************************************************************", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
            Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "Perdeu a conexão com o servidor de difusão", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
            Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "***************************************************************************************************", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });


            if (!conectando)
            {
                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "Tentando reconectar", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                conectando = true;

                MdsHttpClient.Instance.Conecta(MdsHttpClient.uriBase);

                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "Reconexão solicitada", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                if (conectado)
                {
                    Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "Reconexão sucedida", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });


                    Thread reassina = new Thread(MdsHttpClient.Instance.Reassina);
                    reassina.Start();
                }

                conectando = false;
            }
        }

        void Reassina()
        {
            try
            {
                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "***************************************************************************************************", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "Iniciando as socitações de reassinatura ao se recuperar de uma queda de conexão.", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Requisição de reassinaturas de {0}", "CotacaoRapida"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                List<string> assinaturas = new List<string>(MdsAssinaturas.Instance.AssinaturasNegocios.Keys);
                foreach (string instrumento in assinaturas)
                {
                    //AssinaNegocios(instrumento);
                    GerenciadorAssinaturas.FilaAssinaturasNegocio.Enfileira(instrumento);
                    Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Requisitando reassinatura de {0} para {1}", "CotacaoRapida", instrumento), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                }

                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Requisição de reassinaturas de {0}", "LivroOfertas"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                assinaturas = new List<string>(MdsAssinaturas.Instance.AssinaturasLivroOfertas.Keys);
                foreach (string instrumento in assinaturas)
                {
                    //AssinaLivroOfertas(instrumento);
                    GerenciadorAssinaturas.FilaAssinaturasLivroOfertas.Enfileira(instrumento);
                    Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Requisitando reassinatura de {0} para {1}", "LivroOfertasAgregado", instrumento), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                }

                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Requisição de reassinaturas de {0}", "LivroOfertasAgregado"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                assinaturas = new List<string>(MdsAssinaturas.Instance.AssinaturasLivroOfertasAgregado.Keys);
                foreach (string instrumento in assinaturas)
                {
                    //AssinaLivroOfertas(instrumento);
                    GerenciadorAssinaturas.FilaAssinaturasLivroOfertasAgregado.Enfileira(instrumento);
                    Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Requisitando reassinatura de {0} para {1}", "LivroOfertasAgregado", instrumento), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                }

                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Requisição de reassinaturas de {0}", "LivroNegocios"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                assinaturas = new List<string>(MdsAssinaturas.Instance.AssinaturasLivroNegocios.Keys);
                foreach (string instrumento in assinaturas)
                {
                    //AssinaLivroNegocios(instrumento);
                    GerenciadorAssinaturas.FilaAssinaturasLivroNegocios.Enfileira(instrumento);
                    Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Requisitando reassinatura de {0} para {1}", "LivroNegocios", instrumento), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                }

                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Requisição de reassinaturas de {0}", "Acompanhamento"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                assinaturas = new List<string>(MdsAssinaturas.Instance.AssinaturasAcompanhamentoOrdens.Keys);
                System.String clientes = String.Empty;
                foreach (KeyValuePair<string, MdsAssinatura> pair in GerenciadorAssinaturas.Instance.ClientesAssinados)
                {
                    //clientes += String.Format("{0};", codigo);
                    clientes += String.Format("{0};", pair.Value.Instrumento);
                    Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Adicionando cliente {1} a lista de reasinatura de {1}", "Acompanhamento", pair.Value.Instrumento), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                }

                GerenciadorAssinaturas.Instance.AssinaAcompanhamentoOrdens(clientes);

                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Requisição de reassinaturas de {0}", "NegociosDestaque"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                assinaturas = new List<string>(MdsAssinaturas.Instance.AssinaturasNegociosDestaque.Keys);
                foreach (string instrumento in assinaturas)
                {
                    //AssinaNegociosDestaque(instrumento);
                    GerenciadorAssinaturas.FilaAssinaturasNegociosDestaque.Enfileira(instrumento);
                    Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Requisitando reassinatura de {0} para {1}", "NegociosDestaque", instrumento), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                }

                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Requisição de reassinaturas de {0}", "AcompanhamentoLeilao"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                if (MdsAssinaturas.Instance.AssinaturasAcompanhamentoLeilao.Count > 0)
                {
                    //AssinaAcompanhamentoLeilao();
                    Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Requisitando reassinatura de {0}", "AcompanhamentoLeilao"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                }

                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Requisição de reassinaturas de {0}", "ResumoCorretoras"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                assinaturas = new List<string>(MdsAssinaturas.Instance.AssinaturasResumoCorretoras.Keys);
                foreach (string instrumento in assinaturas)
                {
                    //AssinaResumoCorretoras(instrumento);
                    GerenciadorAssinaturas.FilaAssinaturasResumoCorretoras.Enfileira(instrumento);
                    Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Requisitando reassinatura de {0} para {1}", "ResumoCorretoras", instrumento), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Error, "Reassina()", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

		public void IniciaThreadsProcessamento()
		{
			if (!procInicializados)
            {
                GerenciadorAssinaturas.Instance.Initialize();

                GerenciadorCotacoes.Instance.Initialize();

                Thread trataMensagens = new Thread(ThreadTrataMensagens);
				trataMensagens.Name = "MdsHttpClient.ThreadTrataMensagens";
				trataMensagens.Start();

                GerenciadorCotacoes.filaSonda = new BayeuxClient.Classes.Fila<BayeuxResponseMessageBase>();
                Thread trataSonda = new Thread(ThreadTrataSonda);
                trataSonda.Name = "MdsHttpClient.ThreadTrataSonda";
                trataSonda.Start();
                
                GerenciadorCotacoes.filaControle = new BayeuxClient.Classes.Fila<BayeuxResponseMessageBase>();
                Thread trataControle = new Thread(ThreadTrataControle);
                trataControle.Name = "MdsHttpClient.ThreadTrataControle";
                trataControle.Start();

				procInicializados = true;
			}

            if (sucesso)
            {
                conectado = true;
                conectando = false;
                sucesso = false;
            }
		}

		/// <summary>
		///     <para>Método que efetua a conexão da instância do client http do servidor streamer de cotações.</para>
		/// </summary>
		/// <param name="uriBase">Endereço único do servidor streamer de cotação</param>
		/// <returns></returns>
		public BayeuxHandshake _Conecta(String uriBase)
		{
            Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "Conectando o MdsHttpClient", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

			if (!conectado && !conectando)
			{
                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "Iniciando série de procedimentos de subscrição", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
				try
				{
					conectando = true;

					MdsHttpClient.uriBase = uriBase;

					if (handshake == null)
					{
						handshake = new BayeuxHandshake();
						handshake.Uri = uriBase + MdsBayeuxClient.Configuracao.Default.UrlCometdHandshake;
						handshake.localhost = MdsBayeuxClient.Configuracao.Default.UriLocalhost;
						handshake.run();
					}
					else
					{
						handshake.run();
					}

                    Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("ClientID: {0}", handshake.clientId), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

					this._session = handshake.clientId;

                    EfetuarSubscricoes();

					bKeepRunning = true;

                    Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "Conexão efetuada com SUCESSSO", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                    Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "***************************************************************************************************", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                    sucesso = true;
				}
				catch (Exception ex)
				{
                    Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "***************************************************************************************************", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
					Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Error, "Conecta()", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                    Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "***************************************************************************************************", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

					conectado = false;
					conectando = false;

					return null;
				}
			}

			return handshake;
		}

        private void EfetuarSubscricoes()
        {
            BayeuxSubscribe subscribeOfertas = new BayeuxSubscribe();
            subscribeOfertas.Uri = MdsHttpClient.uriBase + MdsBayeuxClient.Configuracao.Default.UrlCometd;
            subscribeOfertas.localhost = MdsBayeuxClient.Configuracao.Default.UriLocalhost;
            subscribeOfertas.clientId = handshake.clientId;
            subscribeOfertas.subscription = MdsBayeuxClient.Configuracao.Default.UrlMdsOfertas;
            subscribeOfertas.run();

            BayeuxSubscribe subscribeNegocios = new BayeuxSubscribe();
            subscribeNegocios.Uri = MdsHttpClient.uriBase + MdsBayeuxClient.Configuracao.Default.UrlCometd;
            subscribeNegocios.localhost = MdsBayeuxClient.Configuracao.Default.UriLocalhost;
            subscribeNegocios.clientId = handshake.clientId;
            subscribeNegocios.subscription = MdsBayeuxClient.Configuracao.Default.UrlMdsNegocios;
            subscribeNegocios.run();

            BayeuxSubscribe subscribeLivroNegocios = new BayeuxSubscribe();
            subscribeLivroNegocios.Uri = MdsHttpClient.uriBase + MdsBayeuxClient.Configuracao.Default.UrlCometd;
            subscribeLivroNegocios.localhost = MdsBayeuxClient.Configuracao.Default.UriLocalhost;
            subscribeLivroNegocios.clientId = handshake.clientId;
            subscribeLivroNegocios.subscription = MdsBayeuxClient.Configuracao.Default.UrlMdsLivroNegocios;
            subscribeLivroNegocios.run();

            BayeuxSubscribe subscribeDestaques = new BayeuxSubscribe();
            subscribeDestaques.Uri = MdsHttpClient.uriBase + MdsBayeuxClient.Configuracao.Default.UrlCometd;
            subscribeDestaques.localhost = MdsBayeuxClient.Configuracao.Default.UriLocalhost;
            subscribeDestaques.clientId = handshake.clientId;
            subscribeDestaques.subscription = MdsBayeuxClient.Configuracao.Default.UrlMdsDestaques;
            subscribeDestaques.run();

            BayeuxSubscribe subscribeRanking = new BayeuxSubscribe();
            subscribeRanking.Uri = MdsHttpClient.uriBase + MdsBayeuxClient.Configuracao.Default.UrlCometd;
            subscribeRanking.localhost = MdsBayeuxClient.Configuracao.Default.UriLocalhost;
            subscribeRanking.clientId = handshake.clientId;
            subscribeRanking.subscription = MdsBayeuxClient.Configuracao.Default.UrlMdsRanking;
            subscribeRanking.run();

            BayeuxSubscribe subscribeAcompanhamentos = new BayeuxSubscribe();
            subscribeAcompanhamentos.Uri = MdsHttpClient.uriBase + MdsBayeuxClient.Configuracao.Default.UrlCometd;
            subscribeAcompanhamentos.localhost = MdsBayeuxClient.Configuracao.Default.UriLocalhost;
            subscribeAcompanhamentos.clientId = handshake.clientId;
            subscribeAcompanhamentos.subscription = MdsBayeuxClient.Configuracao.Default.UrlMdsAcompanhamentoOrdens;
            subscribeAcompanhamentos.run();

            BayeuxSubscribe subscribeNegociosDestaque = new BayeuxSubscribe();
            subscribeNegociosDestaque.Uri = MdsHttpClient.uriBase + MdsBayeuxClient.Configuracao.Default.UrlCometd;
            subscribeNegociosDestaque.localhost = MdsBayeuxClient.Configuracao.Default.UriLocalhost;
            subscribeNegociosDestaque.clientId = handshake.clientId;
            subscribeNegociosDestaque.subscription = MdsBayeuxClient.Configuracao.Default.UrlMdsNegociosDestaque;
            subscribeNegociosDestaque.run();

            BayeuxSubscribe subscribeAcompanhamentoLeilao = new BayeuxSubscribe();
            subscribeAcompanhamentoLeilao.Uri = MdsHttpClient.uriBase + MdsBayeuxClient.Configuracao.Default.UrlCometd;
            subscribeAcompanhamentoLeilao.localhost = MdsBayeuxClient.Configuracao.Default.UriLocalhost;
            subscribeAcompanhamentoLeilao.clientId = handshake.clientId;
            subscribeAcompanhamentoLeilao.subscription = MdsBayeuxClient.Configuracao.Default.UrlMdsAcompanhamentoLeilao;
            subscribeAcompanhamentoLeilao.run();

            BayeuxSubscribe subscribeNoticias = new BayeuxSubscribe();
            subscribeNoticias.Uri = MdsHttpClient.uriBase + MdsBayeuxClient.Configuracao.Default.UrlCometd;
            subscribeNoticias.localhost = MdsBayeuxClient.Configuracao.Default.UriLocalhost;
            subscribeNoticias.clientId = handshake.clientId;
            subscribeNoticias.subscription = MdsBayeuxClient.Configuracao.Default.UrlMdsNoticias;
            subscribeNoticias.run();

            BayeuxSubscribe subscribeOfertasAgregado = new BayeuxSubscribe();
            subscribeOfertasAgregado.Uri = MdsHttpClient.uriBase + MdsBayeuxClient.Configuracao.Default.UrlCometd;
            subscribeOfertasAgregado.localhost = MdsBayeuxClient.Configuracao.Default.UriLocalhost;
            subscribeOfertasAgregado.clientId = handshake.clientId;
            subscribeOfertasAgregado.subscription = MdsBayeuxClient.Configuracao.Default.UrlMdsOfertasAgregado;
            subscribeOfertasAgregado.run();

            BayeuxSubscribe subscribeSonda = new BayeuxSubscribe();
            subscribeSonda.Uri = MdsHttpClient.uriBase + MdsBayeuxClient.Configuracao.Default.UrlCometd;
            subscribeSonda.localhost = MdsBayeuxClient.Configuracao.Default.UriLocalhost;
            subscribeSonda.clientId = handshake.clientId;
            subscribeSonda.subscription = MdsBayeuxClient.Configuracao.Default.UrlMdsSonda;
            subscribeSonda.run();

            BayeuxSubscribe subscribeControle = new BayeuxSubscribe();
            subscribeSonda.Uri = MdsHttpClient.uriBase + MdsBayeuxClient.Configuracao.Default.UrlCometd;
            subscribeSonda.localhost = MdsBayeuxClient.Configuracao.Default.UriLocalhost;
            subscribeSonda.clientId = handshake.clientId;
            subscribeSonda.subscription = MdsBayeuxClient.Configuracao.Default.UrlMdsControle;
            subscribeSonda.run();

            BayeuxSubscribe subscribeResumoCorretoras = new BayeuxSubscribe();
            subscribeResumoCorretoras.Uri = MdsHttpClient.uriBase + MdsBayeuxClient.Configuracao.Default.UrlCometd;
            subscribeResumoCorretoras.localhost = MdsBayeuxClient.Configuracao.Default.UriLocalhost;
            subscribeResumoCorretoras.clientId = handshake.clientId;
            subscribeResumoCorretoras.subscription = MdsBayeuxClient.Configuracao.Default.UrlMdsResumoCorretoras;
            subscribeResumoCorretoras.run();
        }

        public object Assina(MdsAssinatura assinatura)
		{
			object result = null;

			try
			{
                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Requisição de assinatura recebida para [{0}][{1}][{2}]", handshake.clientId, assinatura.Sinal.ToString(), assinatura.Instrumento), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                
				if (assinatura != null)
				{
					switch (assinatura.Sinal)
					{
						case TipoSinal.CotacaoRapida:
							if (MdsAssinaturas.Instance.AssinaturasNegocios.ContainsKey(assinatura.Instrumento))
							{
								result = MdsAssinaturas.Instance.AssinaturasNegocios[assinatura.Instrumento].DataSet;
							}
							GerenciadorAssinaturas.Instance.AssinaNegocio(assinatura);
							break;
						case TipoSinal.LivroNegocios:
                            GerenciadorAssinaturas.Instance.AssinaLivroNegocios(assinatura);
							break;
						case TipoSinal.LivroOfertas:
                            GerenciadorAssinaturas.Instance.AssinaLivroOfertas(assinatura);
							break;
						case TipoSinal.Acompanhamento:
                            GerenciadorAssinaturas.Instance.AssinaAcompanhamento(assinatura);
							break;
						case TipoSinal.NegociosDestaque:
                            GerenciadorAssinaturas.Instance.AssinaNegociosDestaque(assinatura);
							break;
						case TipoSinal.AcompanhamentoLeilao:
                            GerenciadorAssinaturas.Instance.AssinaAcompanhamentoLeilao(assinatura);
							break;
						case TipoSinal.ResumoCorretoras:
                            GerenciadorAssinaturas.Instance.AssinaResumoCorretoras(assinatura);
							break;
						case TipoSinal.LivroOfertasAgregado:
                            GerenciadorAssinaturas.Instance.AssinaLivroOfertasAgregado(assinatura);
							break;
						default:
							{
                                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Assinatura não suportada [{0}][{1}][{3}]", handshake.clientId, assinatura.Instrumento, assinatura.Sinal), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
								break;
							}
					}
				}
				else
				{
                    Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "Assinatura nula ou vazia", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
				}
			}
			catch (Exception ex)
			{
                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Error, "Assina(MdsAssinatura)", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
			}

			return result;
		}
        
        public void CancelaAssinatura(MdsAssinatura assinatura)
		{
			try
			{
                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Requisição de cancelamento de assinatura recebida para [{0}][{1}][{2}]", handshake.clientId, assinatura.Sinal.ToString(), assinatura.Instrumento), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

				if (assinatura != null && assinatura.Instrumento != null && assinatura.Sinal != TipoSinal.Destaques)
				{
					switch (assinatura.Sinal)
					{
						case TipoSinal.CotacaoRapida:
                            GerenciadorAssinaturas.Instance.CancelaAssinaturaNegocio(assinatura);
							break;
						case TipoSinal.LivroNegocios:
                            GerenciadorAssinaturas.Instance.CancelaAssinaturaLivroNegocios(assinatura);
							break;
						case TipoSinal.LivroOfertas:
                            GerenciadorAssinaturas.Instance.CancelaAssinaturaLivroOfertas(assinatura);
							break;
						case TipoSinal.Acompanhamento:
                            GerenciadorAssinaturas.Instance.CancelaAssinaturaAcompanhamento(assinatura);
							break;
						case TipoSinal.TopoLivroOfertas:
                            GerenciadorAssinaturas.Instance.CancelaAssinaturaTopoLivroOfertas(assinatura);
							break;
						case TipoSinal.NegociosDestaque:
                            GerenciadorAssinaturas.Instance.CancelaAssinaturaNegociosDestaque(assinatura);
							break;
						case TipoSinal.AcompanhamentoLeilao:
                            GerenciadorAssinaturas.Instance.CancelaAssinaturaAcompanhamentoLeilao(assinatura);
							break;
						case TipoSinal.ResumoCorretoras:
                            GerenciadorAssinaturas.Instance.CancelaAssinaturaResumoCorretoras(assinatura);
							break;
						case TipoSinal.LivroOfertasAgregado:
                            GerenciadorAssinaturas.Instance.CancelaAssinaturaLivroOfertasAgregado(assinatura);
							break;
						default:
							{
                                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Cancelamento de assinatura não suportado [{0}][{1}][{2}]", handshake.clientId, assinatura.Instrumento, assinatura.Sinal), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
								break;

							}
					}
				}
				else
				{
                    Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, "Assinatura vazia, e, ou, nula", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
				}
			}
			catch (Exception ex)
			{
                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Error, "CancelaAssinatura()", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
#if DEBUG
				throw (ex);
#endif
			}
		}

		





        #region Tratamento de Sonda
        private bool RespondeSonda()
		{
			try
			{
				BayeuxPublish publish = new BayeuxPublish();
				publish.Uri = uriBase + MdsBayeuxClient.Configuracao.Default.UrlCometd;
				publish.localhost = MdsBayeuxClient.Configuracao.Default.UriLocalhost;
				publish.clientId = handshake.clientId;
				publish.channel = MdsBayeuxClient.Configuracao.Default.UrlRequisicaoSonda;
				publish.run();

				if (publish.respData != null)
				{
					foreach (BayeuxResponseMessageBase msg in publish.respData)
					{
						Enqueue(msg);
					}
				}

                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("Conexao ativa. Ultima sonda: {0}", lastSonda.ToString("dd/MM/yyyy HH:mm:ss.fff")), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
			}
			catch (Exception ex)
			{
                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Error, "RespondeSonda()", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
			}

			return true;
		}
		private bool CancelaSonda(String instrumento)
		{
			try
			{
				BayeuxPublish publish = new BayeuxPublish();
				publish.Uri = uriBase + MdsBayeuxClient.Configuracao.Default.UrlCometd;
				publish.localhost = MdsBayeuxClient.Configuracao.Default.UriLocalhost;
				publish.clientId = handshake.clientId;
				publish.channel = MdsBayeuxClient.Configuracao.Default.UrlCancelamentoSonda;
				publish.data = instrumento;
				publish.run();

				foreach (BayeuxResponseMessageBase msg in publish.respData)
				{
					Enqueue(msg);
				}
			}
			catch (Exception ex)
			{
                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Error, "CancelaSonda()", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
			}

			return true;
		}
        #endregion

        public void Terminate()
		{
			bKeepRunning = false;
			while (thrMonitorConexao.IsAlive)
			{
				Thread.Sleep(100);
			}

			thrMonitorConexao = null;
		}

		public static void Enqueue(BayeuxResponseMessageBase msg)
		{
			if(msg.channel.Equals(MdsBayeuxClient.Configuracao.Default.UrlMdsOfertas))
                GerenciadorCotacoes.filaOfertas.Enfileira(msg);
			else if (msg.channel.Equals(MdsBayeuxClient.Configuracao.Default.UrlMdsOfertasAgregado))
                GerenciadorCotacoes.filaOfertasAgregado.Enfileira(msg);
            else if (msg.channel.Equals(MdsBayeuxClient.Configuracao.Default.UrlMdsNegocios))
            {
                GerenciadorCotacoes.filaNegocios.Enfileira(msg);
            }
            else if (msg.channel.Equals(MdsBayeuxClient.Configuracao.Default.UrlMdsLivroNegocios))
                GerenciadorCotacoes.filaLivroNegocios.Enfileira(msg);

            else if (msg.channel.Equals(MdsBayeuxClient.Configuracao.Default.UrlMdsDestaques))
                GerenciadorCotacoes.filaDestaques.Enfileira(msg);

            else if (msg.channel.Equals(MdsBayeuxClient.Configuracao.Default.UrlMdsRanking))
                GerenciadorCotacoes.filaRanking.Enfileira(msg);

            else if (msg.channel.Equals(MdsBayeuxClient.Configuracao.Default.UrlMdsAcompanhamentoOrdens))
                GerenciadorCotacoes.filaAcompanhamentos.Enfileira(msg);

            else if (msg.channel.Equals(MdsBayeuxClient.Configuracao.Default.UrlMdsSonda))
                GerenciadorCotacoes.filaSonda.Enfileira(msg);

            else if (msg.channel.Equals(MdsBayeuxClient.Configuracao.Default.UrlMdsControle))
                GerenciadorCotacoes.filaControle.Enfileira(msg);

            else if (msg.channel.Equals(MdsBayeuxClient.Configuracao.Default.UrlMdsNegociosDestaque))
                GerenciadorCotacoes.filaNegociosDestaque.Enfileira(msg);

            else if (msg.channel.Equals(MdsBayeuxClient.Configuracao.Default.UrlMdsAcompanhamentoLeilao))
                GerenciadorCotacoes.filaAcompanhamentoLeilao.Enfileira(msg);

            else if (msg.channel.Equals(MdsBayeuxClient.Configuracao.Default.UrlMdsResumoCorretoras))
                GerenciadorCotacoes.filaResumoCorretoras.Enfileira(msg);

            else if (msg.channel.Equals(MdsBayeuxClient.Configuracao.Default.UrlMdsNoticias))
                GerenciadorCotacoes.filaNoticias.Enfileira(msg);
		}

        #region Threads de tratamento de mensagens
        private void ThreadTrataMensagens()
        {
            connect = new BayeuxConnect();
            connect.Uri = uriBase + MdsBayeuxClient.Configuracao.Default.UrlCometdConnect;
            connect.localhost = MdsBayeuxClient.Configuracao.Default.UriLocalhost;
            connect.clientId = handshake.clientId;

            try
            {
                connect.IniciaProcessamento();
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Error, "ThreadTrataMensagem()", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                Thread.Sleep(1000);
            }

            while (bKeepRunning)
            {
                try
                {
                    if (connect.filaEncaminhamento.Count > FILA)
                    {
                        Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: Mensagens na fila: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), connect.filaEncaminhamento.Count), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                    }

                    BayeuxResponseMessageBase resposta;
                    connect.filaEncaminhamento.TentaDesinfileirar(out resposta);
                    {
                        switch (resposta.channel.ToString())
                        {
                            case "/mds/ofertas":
                                GerenciadorCotacoes.filaOfertas.Enfileira(resposta);
                                break;
                            case "/mds/ofertasAgregado":
                                GerenciadorCotacoes.filaOfertasAgregado.Enfileira(resposta);
                                break;
                            case "/mds/negocios":
                                GerenciadorCotacoes.filaNegocios.Enfileira(resposta);
                                break;
                            case "/mds/livroNegocios":
                                GerenciadorCotacoes.filaLivroNegocios.Enfileira(resposta);
                                break;
                            case "/mds/destaques":
                                GerenciadorCotacoes.filaDestaques.Enfileira(resposta);
                                break;
                            case "/mds/ranking":
                                GerenciadorCotacoes.filaRanking.Enfileira(resposta);
                                break;
                            case "/mds/acompanhamentoordens":
                                GerenciadorCotacoes.filaAcompanhamentos.Enfileira(resposta);
                                break;
                            case "/mds/sonda":
                                GerenciadorCotacoes.filaSonda.Enfileira(resposta);
                                break;
                            case "/mds/controle":
                                GerenciadorCotacoes.filaControle.Enfileira(resposta);
                                break;
                            case "/mds/negociosDestaque":
                                GerenciadorCotacoes.filaNegociosDestaque.Enfileira(resposta);
                                break;
                            case "/mds/acompanhamentoLeilao":
                                GerenciadorCotacoes.filaAcompanhamentoLeilao.Enfileira(resposta);
                                break;
                            case "/mds/resumoCorretoras":
                                GerenciadorCotacoes.filaResumoCorretoras.Enfileira(resposta);
                                break;
                            case "/mds/noticias":
                                GerenciadorCotacoes.filaNoticias.Enfileira(resposta);
                                break;
                            default:
                                {
                                    Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Warning, String.Format("O canal: {0} não está sendo tratado", resposta.channel.ToString()), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                                    break;
                                }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                }
            }
        }



		private void ThreadTrataSonda()
		{
			try
			{
				int i = 0;
				while (true)
				{
					try
					{
                        if (GerenciadorCotacoes.filaSonda.Count > FILA)
                        {
                            Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: Mensagens na fila: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), GerenciadorCotacoes.filaSonda.Count), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                        }

						BayeuxResponseMessageBase msg;
                        GerenciadorCotacoes.filaSonda.TentaDesinfileirar(out msg);
						lastSonda = DateTime.Now;
						
						this.RespondeSonda();
						if (i > 5)
						{
                            Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: Conexao ativa. Ultima sonda:: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), lastSonda.ToString("dd/MM/yyyy HH:mm:ss.fff")), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
							i = 0;
						}
						i++;

					}
					catch (Exception ex)
					{
                        Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Error, "ThreadTrataSonda()", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
					}
				}
			}
			catch(Exception ex)
			{
                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Error, "ThreadTrataSonda()", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filaMensagens"></param>
        private void ThreadTrataControle()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        if (GerenciadorCotacoes.filaControle.Count > FILA)
                        {
                            Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: Mensagens na fila: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), GerenciadorCotacoes.filaControle.Count), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                        }

                        BayeuxResponseMessageBase msg;
                        GerenciadorCotacoes.filaControle.TentaDesinfileirar(out msg);

                        MdsControle msgControle =
                            JsonConvert.DeserializeObject<MdsControle>(msg.data.ToString());

                        Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Error, String.Format("Recebeu msg de controle: {0}", msg.data.ToString()), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                        MdsControleEventArgs args = new MdsControleEventArgs();
                        args.Action = msgControle.ac;

                        if (OnMdsControleEvent != null)
                        {
                            OnMdsControleEvent(this, args);
                        }
                    }
                    catch (Exception ex)
                    {
                        Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Error, "ThreadTrataControle()", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Client", Gradual.Utils.LoggingLevel.Error, "ThreadTrataControle()", new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        #endregion

		public static bool IsNumeric(string strToCheck)
		{
			return Regex.IsMatch(strToCheck, "^\\d+(\\,\\d+)?$");

		}

		public void AtualizarAcompanhamentoOrdens(OrdemInfo ordem )
		{
			DsAcompanhamentoOrdens lAcompanhamento = ((DsAcompanhamentoOrdens)MdsAssinaturas.Instance.AssinaturasAcompanhamentoOrdens[ASSINATURA_ACOMPANHAMENTO].DataSet);

			DataRowCollection lista = lAcompanhamento.Ordens.Rows; 
			//this.DsAcompanhamentoOrdens.Ordens.Rows;
			DsAcompanhamentoOrdens.OrdensRow registro = null;

			if (lista.Contains(ordem.ClOrdID))
			{
				registro = lista.Find(ordem.ClOrdID) as DsAcompanhamentoOrdens.OrdensRow;
				lista.Remove(registro);
			}
			else
			{
				registro = lAcompanhamento.Ordens.NewOrdensRow();
			}

			if (registro != null)
			{
				registro.Account            = ordem.Account;
				registro.ChannelID          = ordem.ChannelID;
				registro.ClOrdID            = ordem.ClOrdID;
				registro.CumQty             = ordem.CumQty;
				registro.Exchange           = ordem.Exchange;
				registro.ExchangeNumberID   = ordem.ExchangeNumberID;
				registro.ExecBroker         = ordem.ExecBroker;
				registro.ExpireDate         = ordem.ExpireDate.HasValue?ordem.ExpireDate.Value:DateTime.MinValue;
				registro.FixMsgSeqNum       = ordem.FixMsgSeqNum.ToString();
				registro.IdOrdem            = ordem.IdOrdem;
				registro.MaxFloor           = ordem.MaxFloor.HasValue?ordem.MaxFloor.Value:0;
				registro.MinQty             = ordem.MinQty.HasValue? ordem.MinQty.Value:0;
				registro.OrderQty           = ordem.OrderQty;
				registro.OrderQtyRemaining  = ordem.OrderQtyRemmaining;
				registro.OrdStatus          = ordem.OrdStatus.ToString();
				registro.OrdType            = ordem.OrdType.ToString();
				registro.OrigClOrdID        = ordem.OrigClOrdID;
				registro.Price              = ordem.Price;
				registro.RegisterTime       = ordem.RegisterTime;
				registro.SecurityExchangeID = ordem.SecurityExchangeID;
				registro.SecurityID         = ordem.SecurityID;
				registro.SecurityIDSource   = ordem.SecurityIDSource;
				registro.Side               = ordem.Side.ToString();
				registro.StopPrice          = ordem.StopPrice;
				registro.StopStartID        = ordem.StopStartID.HasValue?ordem.StopStartID.Value.ToString():"";
				registro.Symbol             = ordem.Symbol;
				registro.TimeInForce        = ordem.TimeInForce.ToString();
				registro.TransactTime       = ordem.TransactTime;

				lista.Add(registro);
			}


			lista = lAcompanhamento.Acompanhamentos.Rows;

			DataRow[] rows = lAcompanhamento.Acompanhamentos.Select("NumeroControleOrdem='" + ordem.ClOrdID + "'");

			foreach( DataRow row in rows)
			{
				lista.Remove(row);
			}

			foreach (AcompanhamentoOrdemInfo acompanhamento in ordem.Acompanhamentos)
			{
				DsAcompanhamentoOrdens.AcompanhamentosRow row = lAcompanhamento.Acompanhamentos.NewAcompanhamentosRow();

				row.CanalNegociacao        = acompanhamento.CanalNegociacao;
				row.CodigoDoCliente        = acompanhamento.CodigoDoCliente;
				row.CodigoRejeicao         = acompanhamento.CodigoRejeicao;
				row.CodigoResposta         = acompanhamento.CodigoResposta;
				row.CodigoTransacao        = acompanhamento.CodigoTransacao;
				row.DataAtualizacao        = acompanhamento.DataAtualizacao;
				row.DataOrdemEnvio         = acompanhamento.DataOrdemEnvio;
				row.DataValidade           = acompanhamento.DataValidade;
				row.Descricao              = acompanhamento.Descricao;
				row.Direcao                = acompanhamento.Direcao.ToString();
				row.FixMsgSeqNum           = acompanhamento.FixMsgSeqNum.ToString();
				row.Instrumento            = acompanhamento.Instrumento;
				row.NumeroControleOrdem    = acompanhamento.NumeroControleOrdem;
				row.Preco                  = acompanhamento.Preco;
				row.QuantidadeExecutada    = acompanhamento.QuantidadeExecutada;
				row.QuantidadeNegociada    = acompanhamento.QuantidadeNegociada;
				row.QuantidadeRemanescente = acompanhamento.QuantidadeRemanescente;
				row.QuantidadeSolicitada   = acompanhamento.QuantidadeSolicitada;
				row.SecurityID             = acompanhamento.SecurityID;
				row.StatusOrdem            = acompanhamento.StatusOrdem.ToString();

				lista.Add(row);
			}
		}

		void AtualizarNegocios(MdsNegociosEventArgs e)
		{
			lock (MdsAssinaturas.Instance.AssinaturasNegocios)
			{
				if (MdsAssinaturas.Instance.AssinaturasNegocios.ContainsKey(e.negocios.cb.i))
				{
					MdsAssinaturas.Instance.AssinaturasNegocios[e.negocios.cb.i].DataSet = e;
				}
			}
		}
    }
} 
