using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Gradual.OMS.Library.Servicos;
using System.Threading;
using log4net;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Globalization;
using Gradual.OMS.CotacaoAdm.Lib;
using Gradual.OMS.CotacaoStreamer.Dados;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Gradual.OMS.CotacaoStreamer.Lib;
using Gradual.OMS.CotacaoStreamer.Lib.Dados;

namespace Gradual.OMS.CotacaoStreamer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoCotacao : IServicoControlavel, IServicoCotacaoAdm, IServicoCotacaoStreamer
    {
        #region Globais

        private const string TIPO_REQUISICAO_NEGOCIOS_DESTAQUE = "ND";
        private const string TIPO_REQUISICAO_ACOMPANHAMENTO_LEILAO = "LE";
        private const string TIPO_REQUISICAO_RESUMO_CORRETORAS = "RC";

        private const string MDS_ACAO_ASSINAR = "1";
        private const string MDS_ACAO_CANCELAR_ASSINATURA = "2";

        private Timer gTimerDestaques;
        private Timer gTimerResumo;
        private Timer gTimerLeilao;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CultureInfo ciBR = CultureInfo.CreateSpecificCulture("pt-BR");
        private Queue<NegocioInfo> filaNegociosDestaque = new Queue<NegocioInfo>();
        private Queue<NegocioInfo> filaAcompanhamentoLeilao = new Queue<NegocioInfo>();
        private Queue<NegocioInfo> filaResumoCorretoras = new Queue<NegocioInfo>();
        private Queue<DadosFilaNegociosDestaque> filaDestaquesParaStreamer = new Queue<DadosFilaNegociosDestaque>();
        private Queue<DadosFilaAcompanhamentoLeilao> filaLeilaoParaStreamer = new Queue<DadosFilaAcompanhamentoLeilao>();
        private Queue<DadosFilaResumoCorretoras> filaResumoParaStreamer = new Queue<DadosFilaResumoCorretoras>();
        private bool bKeepRunning = true;
        private Thread thDestaquesProcessor = null;
        private Thread thLeilaoProcessor = null;
        private Thread thResumoProcessor = null;
        private Thread thMonitorConexao = null;
        private Thread thNegociosDestaque = null;
        private Thread thAcompanhamentoLeilao = null;
        private Thread thResumoCorretoras = null;
        private MDSPackageSocket lSocket = null;
        private double MaxDifHorarioBolsa;
        private double TimeoutMDS;
        private int ListenPortStreamer;
        private int FrequenciaGeracaoDestaques;
        private int FrequenciaGeracaoResumo;
        private int FrequenciaEnviaLeilao;
        private bool bEnviaAlertas = true;
        private SocketPackage socketStreamerServer = null;
        private bool montandoListaDestaques = false;
        private bool montandoListaResumo = false;
        private bool enviandoLeilao = false;

        protected ServicoStatus Status { set; get; }

        #endregion

        #region IServicoControlavel Members

        public virtual void IniciarServico()
        {
            logger.Info("*** Iniciando Cotacao MDS para Streamer ***");

            MemoriaNegociosDestaque.InicializarDicionarios();
            MemoriaResumoCorretoras.InicializarDicionarios();

            QueueManager.Instance.Start();

            QueueManager.Instance.OnFastQuoteReceived += new MDSMessageReceivedHandler(OnNegocio);

            lSocket = new MDSPackageSocket();
            lSocket.IpAddr = ConfigurationManager.AppSettings["ASConnMDSIp"].ToString();
            lSocket.Port = ConfigurationManager.AppSettings["ASConnMDSPort"].ToString();
            lSocket.OnFastQuoteReceived += new MDSMessageReceivedHandler(OnNegocio);
            lSocket.OpenConnection();

            _sendMDSLoginMSG(lSocket);

            socketStreamerServer = new SocketPackage();
            socketStreamerServer.OnClientConnected += new ClientConnectedHandler(socketStreamerServer_OnClientConnected);
            socketStreamerServer.OnRequestReceived += new MessageReceivedHandler(socketStreamerServer_OnRequestReceived);

            // Ativa thread para retirar mensagens de destaques da fila e enviar para os StreamerServers conectados
            thDestaquesProcessor = new Thread(new ThreadStart(NegociosDestaqueProcessor));
            thDestaquesProcessor.Start();

            // Ativa thread para retirar mensagens de leilao da fila e enviar para os StreamerServers conectados
            thLeilaoProcessor = new Thread(new ThreadStart(AcompanhamentoLeilaoProcessor));
            thLeilaoProcessor.Start();

            // Ativa thread para retirar mensagens de resumo da fila e enviar para os StreamerServers conectados
            thResumoProcessor = new Thread(new ThreadStart(ResumoCorretorasProcessor));
            thResumoProcessor.Start();

            // Ativa thread que monitora conexao com sinal MDS
            thMonitorConexao = new Thread(new ThreadStart(MonitorConexaoMDS));
            thMonitorConexao.Start();

            // Ativa thread que trata as mensagens para os Negocios em Destaque
            thNegociosDestaque = new Thread(new ThreadStart(TrataNegociosDestaque));
            thNegociosDestaque.Start();

            // Ativa thread que trata as mensagens para o Acompanhamento de Leilao
            thAcompanhamentoLeilao = new Thread(new ThreadStart(TrataAcompanhamentoLeilao));
            thAcompanhamentoLeilao.Start();

            // Ativa thread que trata as mensagens para o Resumo de Corretoras
            thResumoCorretoras = new Thread(new ThreadStart(TrataResumoCorretoras));
            thResumoCorretoras.Start();

            // Obtem o parametro de maxima diferenca de horario da ultima mensagem com a bolsa
            // para envio de alertas
            MaxDifHorarioBolsa = 75;
            if (ConfigurationManager.AppSettings["MaxDifHorarioBolsa"] != null)
            {
                MaxDifHorarioBolsa = Convert.ToDouble(ConfigurationManager.AppSettings["MaxDifHorarioBolsa"].ToString());
            }
            MaxDifHorarioBolsa *= 1000;

            // Obtem o timeout de mensagem com o MDS, em segundos
            TimeoutMDS = 300;
            if (ConfigurationManager.AppSettings["TimeoutMDS"] != null)
            {
                TimeoutMDS = Convert.ToDouble(ConfigurationManager.AppSettings["TimeoutMDS"].ToString());
            }
            TimeoutMDS *= 1000;

            // Obtem a frequencia de geracao da lista de destaques, em segundos
            FrequenciaGeracaoDestaques = 20;
            if (ConfigurationManager.AppSettings["FrequenciaGeracaoDestaques"] != null)
            {
                FrequenciaGeracaoDestaques = Convert.ToInt32(ConfigurationManager.AppSettings["FrequenciaGeracaoDestaques"].ToString());
            }
            FrequenciaGeracaoDestaques *= 1000;

            // Define no timer a chamada da montagem de destaques
            gTimerDestaques = new Timer(new TimerCallback(MontagemListaDestaques), null, 0, FrequenciaGeracaoDestaques);

            // Obtem a frequencia de geracao da lista de resumo de corretoras, em segundos
            FrequenciaGeracaoResumo = 25;
            if (ConfigurationManager.AppSettings["FrequenciaGeracaoResumo"] != null)
            {
                FrequenciaGeracaoResumo = Convert.ToInt32(ConfigurationManager.AppSettings["FrequenciaGeracaoResumo"].ToString());
            }
            FrequenciaGeracaoResumo *= 1000;

            // Define no timer a chamada da montagem de resumo de corretoras
            gTimerResumo = new Timer(new TimerCallback(MontagemListaResumo), null, 0, FrequenciaGeracaoResumo);

            // Obtem a frequencia de envio do acompanhamento de leilao, em milisegundos
            FrequenciaEnviaLeilao = 250;
            if (ConfigurationManager.AppSettings["FrequenciaEnviaLeilao"] != null)
            {
                FrequenciaEnviaLeilao = Convert.ToInt32(ConfigurationManager.AppSettings["FrequenciaEnviaLeilao"].ToString());
            }

            // Define no timer a chamada do envio do acompanhamento de leilao
            gTimerLeilao = new Timer(new TimerCallback(EnviaAcompanhamentoLeilao), null, 0, FrequenciaEnviaLeilao);

            if (ConfigurationManager.AppSettings["ListenPort"] != null)
            {
                ListenPortStreamer = Convert.ToInt32(ConfigurationManager.AppSettings["ListenPort"].ToString());
            }

            socketStreamerServer.StartListen(ListenPortStreamer);

            this.Status = ServicoStatus.EmExecucao;
        }

        void socketStreamerServer_OnRequestReceived(object sender, MessageEventArgs args)
        {
            try
            {
                DadosRequisicaoInfo req = JsonConvert.DeserializeObject<DadosRequisicaoInfo>(args.Message);

                if (req.tipo.Equals(TIPO_REQUISICAO_NEGOCIOS_DESTAQUE))
                {
                    logger.Info("Enviando snapshot de Negocios em Destaque para cliente[" + args.ClientNumber.ToString() + "]");
                    MemoriaNegociosDestaque.ListaRanking(filaDestaquesParaStreamer, args.ClientSocket);
                }
                else if (req.tipo.Equals(TIPO_REQUISICAO_ACOMPANHAMENTO_LEILAO))
                {
                    logger.Info("Enviando snapshot de Acompanhamento de Leilao para cliente[" + args.ClientNumber.ToString() + "]");
                    MemoriaAcompanhamentoLeilao.MontagemAcompanhamentoLeilaoStreamer(args.ClientSocket);
                    MemoriaAcompanhamentoLeilao.EnviaAcompanhamentoLeilaoStreamer(filaLeilaoParaStreamer, FrequenciaEnviaLeilao);
                }
                else if (req.tipo.Equals(TIPO_REQUISICAO_RESUMO_CORRETORAS))
                {
                    if (!((IList<string>)MemoriaResumoCorretoras.SEGMENTOS_MERCADO).Contains(req.instrumento))
                    {
                        if (req.acao.Equals(MDS_ACAO_ASSINAR))
                        {
                            MemoriaResumoCorretoras.AssinarRankingPorInstrumento(req.instrumento);
                            MemoriaResumoCorretoras.ListaRankingPorInstrumentoOuCorretora(filaResumoParaStreamer, args.ClientSocket, req.instrumento);
                        }
                        else
                        {
                            MemoriaResumoCorretoras.DesassinarRankingPorInstrumento(req.instrumento);
                        }
                    }
                    else
                    {
                        logger.Info("Enviando snapshot de Resumo de Corretoras para cliente[" + args.ClientNumber.ToString() + "]");
                        MemoriaResumoCorretoras.ListaRanking(filaResumoParaStreamer, args.ClientSocket);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("socketStreamerServer_OnRequestReceived(): " + ex.Message, ex);
            }
        }

        void socketStreamerServer_OnClientConnected(object sender, ClientConnectedEventArgs args)
        {
            logger.Info("Recebeu conexao do StreamerServer: " + args.ClientNumber);
        }

        public virtual void PararServico()
        {
            logger.Info("*** Finalizando Cotacao MDS para Streamer ***");

            bKeepRunning = false;

            if (thMonitorConexao != null)
            {
                while (thMonitorConexao.IsAlive)
                {
                    logger.Info("Aguardando finalizacao do monitor de conexao com MDS");
                    Thread.Sleep(250);
                }
            }

            QueueManager.Instance.Stop();

            logger.Info("Servico Cotacao MDS para Streamer: finalizado");
            this.Status = ServicoStatus.Parado;
        }

        public virtual ServicoStatus ReceberStatusServico()
        {
            return this.Status;
        }

        #endregion //IServicoControlavel Members

        #region Metodos de apoio

        protected void NegociosDestaqueProcessor()
        {
            logger.Info("Inicio thread NegociosDestaqueProcessor()");

            bool bAguarde = false;
            while (bKeepRunning)
            {
                try
                {
                    List<DadosFilaNegociosDestaque> filaTemporaria = new List<DadosFilaNegociosDestaque>();
                    lock (filaDestaquesParaStreamer)
                    {
                        filaTemporaria = filaDestaquesParaStreamer.ToList();
                        filaDestaquesParaStreamer.Clear();
                    }

                    foreach (DadosFilaNegociosDestaque destaque in filaTemporaria)
                    {
                        if (destaque != null && destaque.negociosDestaqueInfo != null)
                        {
                            string msgJson = JsonConvert.SerializeObject(destaque.negociosDestaqueInfo, new JsonDateTimeConverter());

                            if (destaque.socketClient == null)
                                socketStreamerServer.SendToAll(TIPO_REQUISICAO_NEGOCIOS_DESTAQUE + msgJson);
                            else
                                socketStreamerServer.SendData(TIPO_REQUISICAO_NEGOCIOS_DESTAQUE + msgJson, destaque.socketClient);
                        }
                    }

                    lock (filaDestaquesParaStreamer)
                    {
                        if (filaDestaquesParaStreamer.Count == 0)
                            bAguarde = true;
                        else
                            bAguarde = false;
                    }

                    if (bAguarde)
                        Thread.Sleep(250);
                }
                catch (Exception ex)
                {
                    logger.Error("NegociosDestaqueProcessor: " + ex.Message, ex);
                }
            }
            logger.Info("Fim thread NegociosDestaqueProcessor()");
        }

        private void AcompanhamentoLeilaoProcessor()
        {
            logger.Info("Inicio thread AcompanhamentoLeilaoProcessor()");

            bool bAguarde = false;
            while (bKeepRunning)
            {
                try
                {
                    List<DadosFilaAcompanhamentoLeilao> filaTemporaria = new List<DadosFilaAcompanhamentoLeilao>();
                    lock (filaLeilaoParaStreamer)
                    {
                        filaTemporaria = filaLeilaoParaStreamer.ToList();
                        filaLeilaoParaStreamer.Clear();
                    }

                    foreach (DadosFilaAcompanhamentoLeilao leilao in filaTemporaria)
                    {
                        if (leilao != null && leilao.acompanhamentoLeilaoInfo != null)
                        {
                            string msgJson = JsonConvert.SerializeObject(leilao.acompanhamentoLeilaoInfo, new JsonDateTimeConverter());

                            if (leilao.socketClient == null)
                                socketStreamerServer.SendToAll(TIPO_REQUISICAO_ACOMPANHAMENTO_LEILAO + msgJson);
                            else
                                socketStreamerServer.SendData(TIPO_REQUISICAO_ACOMPANHAMENTO_LEILAO + msgJson, leilao.socketClient);
                        }
                    }

                    lock (filaLeilaoParaStreamer)
                    {
                        if (filaLeilaoParaStreamer.Count == 0)
                            bAguarde = true;
                        else
                            bAguarde = false;
                    }

                    if (bAguarde)
                        Thread.Sleep(250);
                }
                catch (Exception ex)
                {
                    logger.Error("AcompanhamentoLeilaoProcessor: " + ex.Message, ex);
                }
            }
            logger.Info("Fim thread AcompanhamentoLeilaoProcessor()");
        }

        private void ResumoCorretorasProcessor()
        {
            logger.Info("Inicio thread ResumoCorretorasProcessor()");

            bool bAguarde = false;
            while (bKeepRunning)
            {
                try
                {
                    List<DadosFilaResumoCorretoras> filaTemporaria = new List<DadosFilaResumoCorretoras>();
                    lock (filaResumoParaStreamer)
                    {
                        filaTemporaria = filaResumoParaStreamer.ToList();
                        filaResumoParaStreamer.Clear();
                    }

                    foreach (DadosFilaResumoCorretoras resumo in filaTemporaria)
                    {
                        if (resumo != null && resumo.resumoCorretorasInfo != null)
                        {
                            string msgJson = JsonConvert.SerializeObject(resumo.resumoCorretorasInfo, new JsonDateTimeConverter());

                            if (resumo.socketClient == null)
                                socketStreamerServer.SendToAll(TIPO_REQUISICAO_RESUMO_CORRETORAS + msgJson);
                            else
                                socketStreamerServer.SendData(TIPO_REQUISICAO_RESUMO_CORRETORAS + msgJson, resumo.socketClient);
                        }
                    }

                    lock (filaResumoParaStreamer)
                    {
                        if (filaResumoParaStreamer.Count == 0)
                            bAguarde = true;
                        else
                            bAguarde = false;
                    }

                    if (bAguarde)
                        Thread.Sleep(250);
                }
                catch (Exception ex)
                {
                    logger.Error("ResumoCorretorasProcessor: " + ex.Message, ex);
                }
            }
            logger.Info("Fim thread ResumoCorretorasProcessor()");
        }

        protected void MontagemListaDestaques(object state)
        {
            // Evita que mais que uma instância seja executada pelo timer
            if (montandoListaDestaques)
                return;

            montandoListaDestaques = true;
            MemoriaNegociosDestaque.ListaRanking(filaDestaquesParaStreamer, null);
            montandoListaDestaques = false;
        }

        protected void MontagemListaResumo(object state)
        {
            // Evita que mais que uma instância seja executada pelo timer
            if (montandoListaResumo)
                return;

            montandoListaResumo = true;
            MemoriaResumoCorretoras.ListaRanking(filaResumoParaStreamer, null);
            MemoriaResumoCorretoras.ListaRankingPorInstrumentoOuCorretora(filaResumoParaStreamer, null, null);
            montandoListaResumo = false;
        }

        protected void EnviaAcompanhamentoLeilao(object state)
        {
            // Evita que mais que uma instância seja executada pelo timer
            if (enviandoLeilao)
                return;

            enviandoLeilao = true;
            MemoriaAcompanhamentoLeilao.EnviaAcompanhamentoLeilaoStreamer(filaLeilaoParaStreamer, FrequenciaEnviaLeilao);
            enviandoLeilao = false;
        }

        public string ReceberNegociosDestaque(string segmentoMercado, TipoDestaqueEnum tipoDestaque)
        {
            return MemoriaNegociosDestaque.ReceberNegociosDestaque(segmentoMercado, tipoDestaque);
        }

        public string ReceberResumoCorretoras(string instrumento)
        {
            return MemoriaResumoCorretoras.ReceberResumoCorretoras(instrumento);
        }

        public string ReceberAcompanhamentoLeilao()
        {
            return MemoriaAcompanhamentoLeilao.ReceberAcompanhamentoLeilao();
        }

        protected virtual void MonitorConexaoMDS()
        {
            int i = 0;
            int iTrialInterval = 600;

            logger.Info("Iniciando thread de monitoracao de conexao com MDS");
            while (bKeepRunning)
            {
                try
                {
                    // Reconecta a cada 5 min
                    if (!lSocket.IsConectado())
                    {
                        if (i > iTrialInterval)
                        {
                            _enviaAlertaDesconexao(lSocket.IpAddr, lSocket.Port);

                            logger.Info("Reabrindo conexao com MDS...");
                            // Não precisa acrescentar outro Handler no OnNegocio, quando ocorre restart do MDS
                            //lSocket.OnFastQuoteReceived += new MDSMessageReceivedHandler(OnNegocio);
                            lSocket.OpenConnection();

                            _sendMDSLoginMSG(lSocket);

                            i = 0;
                        }
                        else
                        {
                            i++;
                            // Configura intervalos de 1 minuto durante o dia ou 
                            // 5 minutos 
                            if (DateTime.Now.Hour > 7 && DateTime.Now.Hour < 21)
                                iTrialInterval = 600;
                            else
                                iTrialInterval = 3000;

                        }
                    }
                    else
                    {
                        if (i > 600)
                        {
                            logger.Info("Conexao com MDS ativa " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") +
                                " LastPkt: [" + QueueManager.Instance.LastNegocioPacket.ToString("dd/MM/yyyy HH:mm:ss.ffff") + "] [" + QueueManager.Instance.LastNegocioMsg + "]");

                            // Verifica dessincronizacao de sinal 
                            if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday &&
                                DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                            {
                                if (DateTime.Now.Hour > 7 && DateTime.Now.Hour < 20)
                                {
                                    TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - lSocket.HorarioUltimaSonda.Ticks);
                                    if (ts.TotalMilliseconds > MaxDifHorarioBolsa)
                                    {
                                        _enviaAlertaAtraso(lSocket.IpAddr, lSocket.Port);
                                    }
                                }
                            }

                            // Verifica ultima comunicacao com MDS
                            TimeSpan tslastpkt = new TimeSpan(DateTime.Now.Ticks - lSocket.LastPacket.Ticks);
                            if (tslastpkt.TotalMilliseconds > TimeoutMDS)
                            {
                                logger.Warn("Finalizando conexao com MDS por timeout!!!");
                                lSocket.CloseConnection();
                            }

                            i = 0;
                        }
                        else
                            i++;
                    }
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    logger.Fatal("MonitorCotacaoMDS(): " + ex.Message, ex);
                    Thread.Sleep(1000);
                }
            }
            logger.Info("Thread de monitoracao de conexao com MDS finalizacao");
        }

        protected virtual void TrataNegociosDestaque()
        {
            bool bWait = false;

            logger.Info("Iniciando thread para tratar mensagens de Negocios em Destaque");
            while (bKeepRunning)
            {
                try
                {
                    List<NegocioInfo> filaTemp = new List<NegocioInfo>();

                    lock (filaNegociosDestaque)
                    {
                        filaTemp = filaNegociosDestaque.ToList();
                        filaNegociosDestaque.Clear();
                    }

                    foreach (NegocioInfo negocio in filaTemp)
                    {
                        if (negocio.Status == 2)
                            MemoriaNegociosDestaque.ContabilizaNegocio(negocio);
                    }

                    lock (filaNegociosDestaque)
                    {
                        if (filaNegociosDestaque.Count == 0)
                            bWait = true;
                        else
                            bWait = false;
                    }

                    if (bWait)
                        Thread.Sleep(250);

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    logger.Fatal("TrataNegociosDestaque(): " + ex.Message, ex);
                    Thread.Sleep(1000);
                }
            }

            logger.Info("Finalizando thread para tratar mensagens de Negocios em Destaque");
        }

        protected virtual void TrataAcompanhamentoLeilao()
        {
            bool bWait = false;

            logger.Info("Iniciando thread para tratar mensagens de Acompanhamento de Leilao");
            while (bKeepRunning)
            {
                try
                {
                    List<NegocioInfo> filaTemp = new List<NegocioInfo>();

                    lock (filaAcompanhamentoLeilao)
                    {
                        filaTemp = filaAcompanhamentoLeilao.ToList();
                        filaAcompanhamentoLeilao.Clear();
                    }

                    foreach (NegocioInfo negocio in filaTemp)
                    {
                        MemoriaAcompanhamentoLeilao.AcompanhamentoLeilao(negocio, filaLeilaoParaStreamer);
                    }

                    lock (filaAcompanhamentoLeilao)
                    {
                        if (filaAcompanhamentoLeilao.Count == 0)
                            bWait = true;
                        else
                            bWait = false;
                    }

                    if (bWait)
                        Thread.Sleep(250);

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    logger.Fatal("TrataAcompanhamentoLeilao(): " + ex.Message, ex);
                    Thread.Sleep(1000);
                }
            }

            logger.Info("Finalizando thread para tratar mensagens de Acompanhamento de Leilao");
        }

        protected virtual void TrataResumoCorretoras()
        {
            bool bWait = false;

            logger.Info("Iniciando thread para tratar mensagens de Resumo de Corretoras");
            while (bKeepRunning)
            {
                try
                {
                    List<NegocioInfo> filaTemp = new List<NegocioInfo>();

                    lock (filaResumoCorretoras)
                    {
                        filaTemp = filaResumoCorretoras.ToList();
                        filaResumoCorretoras.Clear();
                    }

                    foreach (NegocioInfo negocio in filaTemp)
                    {
                        if (negocio.Status == 2)
                            MemoriaResumoCorretoras.ContabilizaNegocio(negocio);
                    }

                    lock (filaResumoCorretoras)
                    {
                        if (filaResumoCorretoras.Count == 0)
                            bWait = true;
                        else
                            bWait = false;
                    }

                    if (bWait)
                        Thread.Sleep(250);

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    logger.Fatal("TrataResumoCorretoras(): " + ex.Message, ex);
                    Thread.Sleep(1000);
                }
            }

            logger.Info("Finalizando thread para tratar mensagens de Resumo de Corretoras");
        }

        protected void OnNegocio(object sender, MDSMessageEventArgs args)
        {
            try
            {
                NegocioInfo negocio = new NegocioInfo();

                // Layout da mensagem de negocio
                // Header
                // Nome	Tipo	Tamanho	Observação
                // Tipo de Mensagem X(2)
                // Tipo de Bolsa	X(2) Espaços, ou:
                //    Bovespa = BV
                //    BM&F = BF
                // Data	N(8) - Formato AAAAMMDD
                // Hora	N(9) - Formato HHMMSSmmm (mmm = milisegundos)
                // Código do Instrumento	X(20)

                // Tipo de Mensagem = NE
                // Nome	Tipo	Tamanho	Observação
                // Data	N(8) - Formato AAAAMMDD
                // Hora	N(9) - Formato HHMMSSmmm (mmm = milisegundos)
                // Corretora Compradora	N(8)
                // Corretora Vendedora	N(8)
                // Preço	N(13)
                // Quantidade N(12)
                // Máxima do dia	N(13)
                // Mínima do dia	N(13)
                // Volume Acumulado	N(13)
                // Número de Negócios	N(8)
                // Indicador de Variação em Relação ao Fechamento do Dia Anterior	X(1)
                //     Variação positiva: “ “ (espaço em branco)
                //     Variação negativa: “-“
                // Percentual de var. em relação ao Fechamento do Dia Anterior	N(8)
                // Estado do Papel	N(1)
                //    0 – Papel não negociado
                //    1 – Papel em leilão
                //    2 – Papel em negociação
                //    3 – Papel suspenso
                //    4 – Papel congelado
                //    5 – Papel inibido

                // Exemplos de mensagens:
                //<---              header             --->data    hora     comprad vend    preco        quantidade  max dia      min dia      vol          nneg    VvFech   E
                //0 2 4       12       21                  41      49       58      66      74           87          99           112          125          138      147     155           169                   191          204          217     225          238
                //NEBV20100702162034355PETR4               201007021618470000000013100000027000000026,800000000000200000000026,860000000026,5200209283409,0000010513 00001,3220000000029.900000000026.45C000000950000000026,79000000002800V000000270000000026,80000000000200
                //NEBV20100702162238788PETR4               201007021620510000000002700000131000000026,850000000000200000000026,870000000026,5200211588300,0000010621 00001,5120000000029.900000000026.45C000000270000000026,85000000000200V000000080000000026,86000000000200

                string dataNegocio = args.Mensagem.Substring(41, 8);

                if (dataNegocio.StartsWith("0000") == false)
                {
                    negocio.Instrumento = args.Mensagem.Substring(21, 20).Trim();
                    negocio.TipoBolsa = args.Mensagem.Substring(2, 2);
                    negocio.DataHora = DateTime.ParseExact(args.Mensagem.Substring(41, 14), "yyyyMMddHHmmss", ciBR);
                    negocio.Compradora = Convert.ToInt32(args.Mensagem.Substring(58, 8), ciBR);
                    negocio.Vendedora = Convert.ToInt32(args.Mensagem.Substring(66, 8), ciBR);
                    negocio.Preco = Convert.ToDouble(args.Mensagem.Substring(74, 13), ciBR);
                    negocio.Quantidade = Convert.ToDouble(args.Mensagem.Substring(87, 12), ciBR);
                    negocio.Maxima = Convert.ToDouble(args.Mensagem.Substring(99, 13), ciBR);
                    negocio.Minima = Convert.ToDouble(args.Mensagem.Substring(112, 13), ciBR);
                    negocio.Volume = Convert.ToDouble(args.Mensagem.Substring(125, 13), ciBR);
                    negocio.NumeroNegocio = Convert.ToInt32(args.Mensagem.Substring(138, 8), ciBR);
                    negocio.Variacao = Convert.ToDouble(args.Mensagem.Substring(146, 9).Trim(), ciBR);
                    negocio.Status = Convert.ToInt32(args.Mensagem.Substring(155, 1), ciBR);
                    negocio.PrecoTeoricoAbertura = Convert.ToDouble(args.Mensagem.Substring(157, 13), ciBR);
                    negocio.VariacaoTeorica = Convert.ToDouble(args.Mensagem.Substring(170, 9).Trim(), ciBR);
                    negocio.HorarioTeorico = DateTime.ParseExact(args.Mensagem.Substring(179, 14), "yyyyMMddHHmmss", ciBR);

                    lock (filaNegociosDestaque)
                    {
                        filaNegociosDestaque.Enqueue(negocio);
                    }
                    lock (filaAcompanhamentoLeilao)
                    {
                        filaAcompanhamentoLeilao.Enqueue(negocio);
                    }
                    lock (filaResumoCorretoras)
                    {
                        filaResumoCorretoras.Enqueue(negocio);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("OnNegocio: Erro na mensagem[" + args.Mensagem + "]: " + ex.Message, ex);
            }
        }

        protected void _sendMDSLoginMSG(MDSPackageSocket mdsSocket)
        {
            string msg = "QLPP";

            try
            {
                if (ConfigurationManager.AppSettings["EfetuarLogonMDS"] == null)
                {
                    logger.Warn("Chave 'EfetuarLogonMDS' nao declarada no appsettings. Nao efetua login");
                    return;
                }

                if (!ConfigurationManager.AppSettings["EfetuarLogonMDS"].ToString().Equals("true"))
                {
                    logger.Warn("Nao efetua login no MDS, EfetuarLogonMDS=false.");
                    return;
                }

                msg += DateTime.Now.ToString("yyyyMMddHHmmssfff");
                msg += System.Environment.MachineName.PadRight(20);

                logger.Info("Efetuando login no MDS [" + msg + "]");

                if (mdsSocket != null && mdsSocket.IsConectado())
                {
                    mdsSocket.SendData(msg, true);
                }

                logger.Info("Mensagem de login enviada ao MDS");
            }
            catch (Exception ex)
            {
                logger.Info("_sendMDSLoginMSG():" + ex.Message, ex);
            }
        }

        /// <summary>
        /// Envia mensagem de alerta
        /// </summary>
        protected void _enviaAlertaAtraso(string server, string porta)
        {
            try
            {
                string body = "";

                string subject = System.Environment.MachineName + " Alerta: Atraso de sinal";
                body += System.Environment.MachineName + " Alerta: ";
                body += "\r\n";
                body += "Horario do server: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                body += "\r\n";
                body += "MDS: " + server + ":" + porta;
                body += "\r\n";
                body += "Ultimo sinal recebido: " + lSocket.HorarioUltimaSonda.ToString("yyyy/MM/dd HH:mm:ss");

                if (_enviarEmail(subject, body))
                {
                    logger.InfoFormat("Email de alerta de atraso enviado com sucesso");
                }
            }
            catch (Exception ex)
            {
                logger.Error("_enviaAlertaAtraso(): " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Envia mensagem de alerta
        /// </summary>
        protected void _enviaAlertaDesconexao(string server, string porta)
        {
            try
            {
                string body = "";

                string subject = System.Environment.MachineName + " Alerta: Desconectado do MDS";
                body += System.Environment.MachineName + " Alerta: ";
                body += "\r\n";
                body += "MDS: " + server + ":" + porta;
                body += "\r\n";
                body += "Desconectado do MDS: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                _enviarEmail(subject, body);
            }
            catch (Exception ex)
            {
                logger.Error("_enviaAlertaDesconexao(): " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Envia mensagem de alerta
        /// </summary>
        private bool _enviarEmail(string subject, string body)
        {
            try
            {
                string[] destinatarios;

                if (bEnviaAlertas == false)
                {
                    logger.Error("Envio de alertas desabilitado!!!");
                    return false;
                }

                if (ConfigurationManager.AppSettings["EmailAlertaDestinatarios"] == null)
                {
                    logger.Error("Settings 'EmailAlertaDestinatarios' nao definido. Nao eh possivel enviar alerta");
                    return false;
                }

                char[] seps = { ';' };
                destinatarios = ConfigurationManager.AppSettings["EmailAlertaDestinatarios"].ToString().Split(seps);

                var lMensagem = new MailMessage("Gradual.OMS.Cotacao@gradualinvestimentos.com.br", destinatarios[0]);

                for (int i = 1; i < destinatarios.Length; i++)
                {
                    lMensagem.To.Add(destinatarios[i]);
                }


                lMensagem.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["EmailAlertaReplyTo"].ToString()));
                lMensagem.Body = body;
                lMensagem.Subject = subject;

                new SmtpClient(ConfigurationManager.AppSettings["EmailAlertaHost"].ToString()).Send(lMensagem);

                logger.InfoFormat("Email enviado com sucesso");
            }
            catch (Exception ex)
            {
                logger.Error("_enviarEmail(): " + ex.Message, ex);
                return false;
            }

            return true;
        }


        #endregion //Metodos de apoio

        #region IServicoCotacaoAdm Members
        public void ConectarMDS()
        {
            logger.Info("Reabrindo conexao com MDS por solicitacao via ADM...");


            lSocket.OnFastQuoteReceived += new MDSMessageReceivedHandler(OnNegocio);
            lSocket.OpenConnection();

            _sendMDSLoginMSG(lSocket);
        }

        public void DesconectarMDS()
        {
            logger.Warn("Finalizando conexao com MDS por solicitacao via ADM!");

            lSocket.CloseConnection();
        }

        public void TrocarServidorMDS(string host)
        {
            logger.Warn("Trocando servidor para [" + host + "] via ADM");

            // Remove a nova senha, no arquivo de configuracao inclusive
            Configuration stmconfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            stmconfig.AppSettings.Settings.Remove("ASConnMDSIp");
            stmconfig.AppSettings.Settings.Add("ASConnMDSIp", host);

            stmconfig.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public void LigarEnvioAlertas()
        {
            bEnviaAlertas = true;
        }

        public void DesligarEnvioAlertas()
        {
            bEnviaAlertas = false;
        }

        public string ObterServidorMDS()
        {
            string ret = ConfigurationManager.AppSettings["ASConnMDSIp"].ToString();

            return ret;
        }

        public bool IsConectado()
        {
            return lSocket.IsConectado();
        }

        public DateTime LastNegocioPacket()
        {
            return lSocket.LastNegocioPacket;
        }

        public DateTime LastPacket()
        {
            return lSocket.LastPacket;
        }

        public DateTime LastLofPacket()
        {
            return DateTime.Now;
        }

        public DateTime LastDestaquePacket()
        {
            return DateTime.Now;
        }

        public DateTime LastRankingPacket()
        {
            return DateTime.Now;
        }

        public DateTime LastSonda()
        {
            return lSocket.HorarioUltimaSonda;
        }

        #endregion //IServicoCotacaoAdm Members
    }
}
