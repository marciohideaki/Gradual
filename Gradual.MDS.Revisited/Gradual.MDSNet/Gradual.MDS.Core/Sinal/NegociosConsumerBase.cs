using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Gradual.MDS.Core.Lib;
using Gradual.MDS.Eventos.Lib;
using log4net;
using OpenFAST;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Gradual.MDS.Core.Sinal
{
    public class NegociosConsumerBase : UmdfEventConsumerBase
    {
        private Dictionary<string, NegocioBase> dctNegocios = new Dictionary<string, NegocioBase>();
        private ConcurrentDictionary<string, string> dicSecurityID = new ConcurrentDictionary<string, string>();
        private Dictionary<string, GrupoCotacaoInfo> dctGrupoCotacao = new Dictionary<string, GrupoCotacaoInfo>();
        private Dictionary<string, List<string>> dctComposicaoIndice = new Dictionary<string, List<string>>();
        private Dictionary<string, string> dctUltimoNeg = new Dictionary<string, string>();
        private int numItensLNG = ConstantesMDS.NUMERO_ITENS_LIVRO_NEGOCIOS;
        private Dictionary<string, ChannelUDMF> dicCanais;
        private DbLib dbLib;
        private SinalLastTimestamp timestampControl = new SinalLastTimestamp();
        private ConcurrentQueue<string> queueNegocio = new ConcurrentQueue<string>();
        private ConcurrentQueue<string> queueSerieHistorica = new ConcurrentQueue<string>();
        private ConcurrentQueue<string> queueObterDataHoraUltNeg = new ConcurrentQueue<string>();
        private object syncQueueNegocio = new object();
        private object syncQueueSerieHistorica = new object();
        private ConcurrentDictionary<string, long> dctLastAtraso = new ConcurrentDictionary<string, long>();

        private int maxThreads = 25;
        private Thread[] arrThreadEventosStreamer;
        private ConcurrentQueue<TOEventosStreamer> [] arrCQueue;
        private object [] arrLock;
        private int _currentIdx = 0;
        private bool bGerarEventoStreamer = true;
        private bool bGerarEventoHB = true;
        private bool bGerarEventoANG = true;
        private string horarioSuspensaoAnaliseGraficaVista = ConstantesMDS.HORARIO_SUSPENSAO_ANALISE_GRAFICA_VISTA;
        private string horarioSuspensaoAnaliseGraficaTermo = ConstantesMDS.HORARIO_SUSPENSAO_ANALISE_GRAFICA_TERMO;
        private string horarioSuspensaoAnaliseGraficaOpcoes = ConstantesMDS.HORARIO_SUSPENSAO_ANALISE_GRAFICA_OPCOES;
        private string horarioSuspensaoAnaliseGraficaExerOpcoes = ConstantesMDS.HORARIO_SUSPENSAO_ANALISE_GRAFICA_EXER_OPCOES;
        private long lastAvisoAtraso = 0;

        private ConnectionMultiplexer redis = null;
        private IDatabase redisDB = null;

        private class AtualizaIndiceInfo
        {
            public Decimal ValorNegocio { get; set; }
            public long Quantidade { get; set; }
            public long NumeroNegocio { get; set; }
            public string Action { get; set; }
        }

        private class GrupoCotacaoInfo
        {
            public int estado { get; set; }
            public List<string> listaGrupoCotacao { get; set; }
        }

        public NegociosConsumerBase(Dictionary<string, ChannelUDMF> dicCanais)
        {
            this.dicCanais = dicCanais;
            try
            {
                if (ConfigurationManager.AppSettings["MaxItensLivroNegocios"] != null)
                    numItensLNG = Convert.ToInt32(ConfigurationManager.AppSettings["MaxItensLivroNegocios"].ToString());
            }
            catch (Exception e)
            {
                numItensLNG = ConstantesMDS.NUMERO_ITENS_LIVRO_NEGOCIOS;
            }

            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            myThreadName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
            MDSUtils.AddAppender("NegociosConsumerBase-", logger.Logger);
            dbLib = new DbLib();

            dctUltimoNeg = dbLib.obterDataUltimoNegocioTodosAtivos();

            logger.Info("Carregou data/hora do ultimo negocio para [" + dctUltimoNeg.Count + "] ativos");

            ConfigurationOptions redisOpts = new ConfigurationOptions();
            if (ConfigurationManager.AppSettings["RedisServerPort"] != null)
            {
                redis = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["RedisServerPort"].ToString());
                redisDB = redis.GetDatabase();
            }


            if (ConfigurationManager.AppSettings["GeraEventoNegociosThreadPoolSize"] != null)
            {
                maxThreads = Convert.ToInt32(ConfigurationManager.AppSettings["GeraEventoNegociosThreadPoolSize"].ToString());
            }

            if (ConfigurationManager.AppSettings["GerarEventoStreamer"] != null)
            {
                if (ConfigurationManager.AppSettings["GerarEventoStreamer"].ToString().ToLowerInvariant().Equals("false"))
                {
                    bGerarEventoStreamer = false;
                }
            }

            if (ConfigurationManager.AppSettings["GerarEventoHB"] != null)
            {
                if (ConfigurationManager.AppSettings["GerarEventoHB"].ToString().ToLowerInvariant().Equals("false"))
                {
                    bGerarEventoHB = false;
                }
            }

            if (ConfigurationManager.AppSettings["GerarEventoANG"] != null)
            {
                if (ConfigurationManager.AppSettings["GerarEventoANG"].ToString().ToLowerInvariant().Equals("false"))
                {
                    bGerarEventoANG = false;
                }
            }

            if (ConfigurationManager.AppSettings["HorarioSuspensaoAnaliseGraficaVista"] != null)
            {
                horarioSuspensaoAnaliseGraficaVista = ConfigurationManager.AppSettings["HorarioSuspensaoAnaliseGraficaVista"];
            }

            if (ConfigurationManager.AppSettings["HorarioSuspensaoAnaliseGraficaTermo"] != null)
            {
                horarioSuspensaoAnaliseGraficaTermo = ConfigurationManager.AppSettings["HorarioSuspensaoAnaliseGraficaTermo"];
            }

            if (ConfigurationManager.AppSettings["HorarioSuspensaoAnaliseGraficaOpcoes"] != null)
            {
                horarioSuspensaoAnaliseGraficaOpcoes = ConfigurationManager.AppSettings["HorarioSuspensaoAnaliseGraficaOpcoes"];
            }

            if (ConfigurationManager.AppSettings["HorarioSuspensaoAnaliseGraficaExerOpcoes"] != null)
            {
                horarioSuspensaoAnaliseGraficaExerOpcoes = ConfigurationManager.AppSettings["HorarioSuspensaoAnaliseGraficaExerOpcoes"];
            }
        }

        protected override void beforeStart()
        {
        }

        protected override void afterStart()
        {
            try
            {
                Thread thMonitorCacheHB = new Thread(new ThreadStart(monitorCacheHBProc));
                thMonitorCacheHB.Start();

                Thread thMonitorCacheStreamer = new Thread(new ThreadStart(monitorCacheStreamerProc));
                thMonitorCacheStreamer.Start();

                Thread thMonitorCacheAnaliseGrafica = new Thread(new ThreadStart(monitorCacheAnaliseGraficaProc));
                thMonitorCacheAnaliseGrafica.Start();

                Thread thMonitorCacheSerieHistorica = new Thread(new ThreadStart(monitorCacheSerieHistoricaProc));
                thMonitorCacheSerieHistorica.Start();

                Thread thBuscarDataHoraUltNeg = new Thread(new ThreadStart(buscarDataHoraUltNegProc));
                thBuscarDataHoraUltNeg.Start();


                arrThreadEventosStreamer = new Thread[maxThreads];
                arrCQueue = new ConcurrentQueue<TOEventosStreamer>[maxThreads];
                arrLock = new object[maxThreads];
                for (int i = 0; i < maxThreads; i++)
                {
                    arrLock[i] = new object();
                    arrCQueue[i] = new ConcurrentQueue<TOEventosStreamer>();
                    arrThreadEventosStreamer[i] = new Thread(new ParameterizedThreadStart(_monitorEventoStreamer));
                    arrThreadEventosStreamer[i].Start(i);
                }

            }
            catch (Exception ex)
            {
                logger.Error("afterStart():" + ex.Message, ex);
            }

        }

        #region UMDF Message Handling
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="evento"></param>
        protected override void trataEventoUmdf(EventoUmdf evento)
        {
            try
            {
                String msgType = evento.MsgType;
                OpenFAST.Message umdfMessage = evento.MsgBody;
                String instrumento = "";
                String msgID = evento.MsgSeqNum.ToString();
                String channelId = evento.ChannelID;
                
                ChannelUDMF config = null;
                dicCanais.TryGetValue(channelId, out config);
                bool isPuma = config.channelConfig.IsPuma;
                bool isBMF = (config.channelConfig.Segment.ToUpper().Equals(ConstantesMDS.CHANNEL_UMDF_SEGMENT_BMF) ? true : false);
                bool isPuma20 = config.channelConfig.IsPuma20;

                //bool isPuma = dicCanais[channelId].channelConfig.IsPuma;
                //bool isBMF = (dicCanais[channelId].channelConfig.Segment.ToUpper().Equals(ConstantesMDS.CHANNEL_UMDF_SEGMENT_BMF) ? true : false);
                //bool isPuma20 = dicCanais[channelId].channelConfig.IsPuma20;
                
                int marketDepth = evento.MarketDepth;
                bool sendToStreamer = true;

                //logger.InfoFormat("trataEventoUmdf mensagem UMDF [{0}]", evento.MsgSeqNum);

                if (logger.IsDebugEnabled)
                {
                    logger.DebugFormat("channelID ....: {0} (isPuma={1})", channelId, isPuma.ToString());
                    logger.DebugFormat("msgID  .......: {0}", msgID);
                    logger.DebugFormat("Message ......: {0}", umdfMessage.ToString());
                }

                List<Dictionary<string, string>> streamerLNG = new List<Dictionary<string, string>>();

                // Tratar mensagem de melhor oferta
                if ( msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_MELHOR_OFERTA) ||
                     msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_INCREMENTAL_SINGLE) ||
                     msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_SNAPSHOT_SINGLE) )
                {
                    if (umdfMessage.IsDefined("MDEntries"))
                    {
                        GroupValue mdEntry = umdfMessage.GetGroup("MDEntries");
                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug(UmdfUtils.writeGroup(mdEntry));
                        }

                        String securityID="";

                        if ( umdfMessage.IsDefined("SecurityID") )
                            securityID = umdfMessage.GetString("SecurityID");
                        else
                        {
                            if (mdEntry.IsDefined("SecurityID"))
                                securityID = mdEntry.GetString("SecurityID");
                        }

                        if (mdEntry.IsDefined("Symbol"))
                            instrumento = mdEntry.GetString("Symbol");
                        else
                        {
                            if (umdfMessage.IsDefined("Symbol"))
                                instrumento = umdfMessage.GetString("Symbol");
                            else
                            {
                                if (String.IsNullOrEmpty(securityID) || !dicSecurityID.TryGetValue(securityID, out instrumento))
                                {
                                    logger.ErrorFormat("SecurityID[{0}] Nao pode resolver instrumento/securityID", securityID);
                                    return;
                                }
                            }
                        }

                        if (mdEntry.IsDefined("MDEntryPositionNo") && marketDepth != ConstantesUMDF.UMDF_MARKETDEPTH_TOP_OF_BOOK)
                        {
                            if (mdEntry.GetInt("MDEntryPositionNo") != 0)
                                return;
                        }

                        if (!dctNegocios.ContainsKey(instrumento))
                        {
                            dctNegocios.Add(instrumento, new NegocioBase(instrumento));
                            if (instrumento.Equals(ConstantesMDS.INDICE_IBOVESPA))
                            {
                                if (!dctNegocios.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_TOTAL))
                                    dctNegocios.Add(ConstantesMDS.INDICE_IBOVESPA_TOTAL, new NegocioBase(instrumento));
                                if (!dctNegocios.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_VISTA))
                                    dctNegocios.Add(ConstantesMDS.INDICE_IBOVESPA_VISTA, new NegocioBase(instrumento));
                                if (!dctNegocios.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_OPCOES))
                                    dctNegocios.Add(ConstantesMDS.INDICE_IBOVESPA_OPCOES, new NegocioBase(instrumento));
                                if (!dctNegocios.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_TERMO))
                                    dctNegocios.Add(ConstantesMDS.INDICE_IBOVESPA_TERMO, new NegocioBase(instrumento));
                            }
                        }

                        switch(msgType)
                        {
                            case ConstantesUMDF.FAST_MSGTYPE_SNAPSHOT_SINGLE:
                            case ConstantesUMDF.FAST_MSGTYPE_INCREMENTAL_SINGLE:
                                instrumento = processaMensagemMarketData(instrumento, msgID, umdfMessage, streamerLNG, msgType, marketDepth, isPuma, isBMF, ref sendToStreamer);
                                break;
                            case ConstantesUMDF.FAST_MSGTYPE_MELHOR_OFERTA:
                                instrumento = tratarMensagemMelhorOferta(instrumento, msgID, mdEntry);
                                break;
                            default:
                                logger.Error("MessageType nao esperado [" + msgType + "]");
                                return;
                        }
                    }
                }

                else if (msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_SECURITYLIST_SINGLE))
                {
                    instrumento = processaMensagemSecurityList(msgID, umdfMessage);

                    if (!dctNegocios.ContainsKey(instrumento))
                    {
                        dctNegocios.Add(instrumento, new NegocioBase(instrumento));
                        if (instrumento.Equals(ConstantesMDS.INDICE_IBOVESPA))
                        {
                            if (!dctNegocios.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_TOTAL))
                                dctNegocios.Add(ConstantesMDS.INDICE_IBOVESPA_TOTAL, new NegocioBase(instrumento));
                            if (!dctNegocios.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_VISTA))
                                dctNegocios.Add(ConstantesMDS.INDICE_IBOVESPA_VISTA, new NegocioBase(instrumento));
                            if (!dctNegocios.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_OPCOES))
                                dctNegocios.Add(ConstantesMDS.INDICE_IBOVESPA_OPCOES, new NegocioBase(instrumento));
                            if (!dctNegocios.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_TERMO))
                                dctNegocios.Add(ConstantesMDS.INDICE_IBOVESPA_TERMO, new NegocioBase(instrumento));
                        }
                    }

                    atualizaCadastroBasico(instrumento, umdfMessage, isPuma20, evento.UmdfSegment);

                    atualizaGrupoCotacao(instrumento);

                    if (evento.UmdfSegment.Equals(ConstantesUMDF.UMDF_SEGMENT_DERIVATIVES))
                    {
                        dctNegocios[instrumento].TipoBolsa = ConstantesMDS.DESCRICAO_DE_BOLSA_BMF;
                    }
                    else
                    {
                        dctNegocios[instrumento].TipoBolsa = ConstantesMDS.DESCRICAO_DE_BOLSA_BOVESPA;
                    }
                }

                // Tratar mensagem de alteracao do estado do grupo de cotacao
                else if (msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_SECURITYSTATUS) && umdfMessage.IsDefined("SecurityGroup"))
                {
                    logger.InfoFormat("GrupoCotacao[{0}]: Trata estado dos papeis", umdfMessage.GetString("SecurityGroup"));
                    tratarMensagemEstadoGrupoCotacao(instrumento, msgID, umdfMessage, null, isPuma);
                }

                else if (msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_SECURITYSTATUS) && umdfMessage.IsDefined("SecurityTradingStatus"))
                {
                    String securityID = umdfMessage.GetString("SecurityID");
                    if (String.IsNullOrEmpty(securityID) || !dicSecurityID.TryGetValue(securityID, out instrumento))
                    {
                        logger.ErrorFormat("SecurityID[{0}] Nao pode resolver instrumento/securityID", securityID);
                        return;
                    }

                    tratarMensagemEstadoInstrumento(instrumento, msgID, umdfMessage, null, msgType, isPuma);
                    /*
                    bmfNegociosContabilizacao.tratarMensagemFaseNegociacao(
                            instrumento, msgID, umdfMessage);
		
                    bmfNegociosContabilizacao.tratarMensagemEstadoInstrumento(
                            instrumento, msgID, umdfMessage);
                    */
                }

            
                if (!String.IsNullOrEmpty(instrumento))
                {
                    NegocioBase registroNegocio;
                    registroNegocio = dctNegocios[instrumento];
                    bool shouldSendNegStreamer = timestampControl.ShouldSendNEGStreamer(instrumento) && bGerarEventoStreamer;
                    bool shouldSendNegHB = timestampControl.ShouldSendNEGHB(instrumento) && bGerarEventoHB;
                    bool shouldSendLivroNegocioHB = timestampControl.ShouldSendLivroNegociosHB(instrumento) && bGerarEventoHB;

                    TOEventosStreamer to = new TOEventosStreamer();
                    to.Instrumento = instrumento;
                    to.RegistroNegocio = registroNegocio;
                    to.SendToStreamer = sendToStreamer;
                    to.ShouldSendNegHB = shouldSendNegHB;
                    to.ShouldSendNegStreamer = shouldSendNegStreamer;
                    to.ShouldSendLivroNegocioHB = shouldSendLivroNegocioHB;
                    to.StreamerLNG = streamerLNG;
                    this._addToEvento(_currentIdx, to);
                    _currentIdx++;
                    if (_currentIdx >= maxThreads)
                        _currentIdx = 0;
                    
                    int maxWorkerThreads=0;
                    int maxCompletionPortThreads =0;
                    int availWorkerThreads=0;
                    int availCompletionPortThreads = 0;

                    ThreadPool.GetAvailableThreads(out availWorkerThreads, out availCompletionPortThreads);

                    if (availWorkerThreads <= 50)
                    {
                        ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxCompletionPortThreads);
                        logger.Warn("MaxPoolThreads exausting...holy crap!!! Avail=" + availWorkerThreads + "/" + maxWorkerThreads);
                    }

                    /* ThreadPool.QueueUserWorkItem(
                        new WaitCallback(
                            delegate(object required)
                            {
                                try
                                {
                                    if (sendToStreamer && shouldSendNegStreamer)
                                    {
                                        enviaMensagemStreamer(registroNegocio, instrumento);

                                        if (instrumento.Equals(ConstantesMDS.INDICE_IBOVESPA))
                                        {
                                            enviaMensagemStreamer(dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TOTAL], ConstantesMDS.INDICE_IBOVESPA_TOTAL);
                                            enviaMensagemStreamer(dctNegocios[ConstantesMDS.INDICE_IBOVESPA_VISTA], ConstantesMDS.INDICE_IBOVESPA_VISTA);
                                            enviaMensagemStreamer(dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TERMO], ConstantesMDS.INDICE_IBOVESPA_TERMO);
                                            enviaMensagemStreamer(dctNegocios[ConstantesMDS.INDICE_IBOVESPA_OPCOES], ConstantesMDS.INDICE_IBOVESPA_OPCOES);
                                        }
                                    }

                                    // Envia sinal de Livro de Negocios
                                    if (streamerLNG.Count > 0)
                                    {
                                        EventoHttpLivroNegocios eventoLNG = new EventoHttpLivroNegocios();
                                        eventoLNG.instrumento = instrumento;
                                        eventoLNG.cabecalho = MDSUtils.montaCabecalhoStreamer(ConstantesMDS.TIPO_REQUISICAO_LIVRO_NEGOCIOS, registroNegocio.TipoBolsa, ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_ALTERAR, instrumento, registroNegocio.CasasDecimais, null);
                                        eventoLNG.negocio = streamerLNG;

                                        EventQueueManager.Instance.SendEvent(eventoLNG);
                                    }

                                    if (shouldSendNegHB)
                                    {
                                        enviaMensagemHomeBrokerOuCotacaoStreamer(registroNegocio, instrumento);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.Error("delegate(): " + ex.Message, ex);
                                }
                            }
                        )
                    );*/
                }
            }
            catch (Exception ex)
            {
                logger.Error("processaEventoUmdf(): " + ex.Message, ex);
            }
        }

        private void _processaEventoStreamer(TOEventosStreamer to)
        {
            try
            {
                if (to.SendToStreamer && to.ShouldSendNegStreamer)
                {
                    enviaMensagemStreamer(to.RegistroNegocio, to.Instrumento);

                    if (to.Instrumento.Equals(ConstantesMDS.INDICE_IBOVESPA))
                    {
                        enviaMensagemStreamer(dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TOTAL], ConstantesMDS.INDICE_IBOVESPA_TOTAL);
                        enviaMensagemStreamer(dctNegocios[ConstantesMDS.INDICE_IBOVESPA_VISTA], ConstantesMDS.INDICE_IBOVESPA_VISTA);
                        enviaMensagemStreamer(dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TERMO], ConstantesMDS.INDICE_IBOVESPA_TERMO);
                        enviaMensagemStreamer(dctNegocios[ConstantesMDS.INDICE_IBOVESPA_OPCOES], ConstantesMDS.INDICE_IBOVESPA_OPCOES);
                    }
                }

                // Envia sinal de Livro de Negocios
                if (to.StreamerLNG.Count > 0)
                {
                    EventoHttpLivroNegocios eventoLNG = new EventoHttpLivroNegocios();
                    eventoLNG.instrumento = to.Instrumento;
                    eventoLNG.cabecalho = MDSUtils.montaCabecalhoStreamer(ConstantesMDS.TIPO_REQUISICAO_LIVRO_NEGOCIOS, to.RegistroNegocio.TipoBolsa, ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_ALTERAR, to.Instrumento, to.RegistroNegocio.CasasDecimais, null);
                    eventoLNG.negocio = to.StreamerLNG;

                    EventQueueManager.Instance.SendEvent(eventoLNG);
                }

                if (to.ShouldSendNegHB)
                {
                    enviaMensagemHomeBrokerOuCotacaoStreamer(to.RegistroNegocio, to.Instrumento);
                }

                if (to.ShouldSendLivroNegocioHB)
                {
                    EventoHBNegocio eventoHBLNG = new EventoHBNegocio();
                    eventoHBLNG.instrumento = to.Instrumento;
                    eventoHBLNG.mensagem = NegocioBase.montarLivroNegociosHomeBroker(to.RegistroNegocio);
                    EventQueueManager.Instance.SendEvent(eventoHBLNG);
                }
            }
            catch (Exception ex)
            {
                logger.Error("_processaEventoStreamer(): " + ex.Message, ex);
            }
        }

        private void _monitorEventoStreamer(object param)
        {
            int idx = (int) param;
            long lastLog = 0;
            long lastWatchDog = DateTime.UtcNow.Ticks;

            while (bKeepRunning)
            {
                try
                {
                    if (MDSUtils.shouldLog(lastWatchDog, 30))
                    {
                        logger.InfoFormat("monitorEventoStreamer({0}) ativo: {1}msgs", idx, arrCQueue[idx].Count);
                        lastWatchDog = DateTime.UtcNow.Ticks;
                    }

                    TOEventosStreamer to = null;
                    if (arrCQueue[idx].TryDequeue(out to))
                    {
                        _processaEventoStreamer(to);

                        if (MDSUtils.shouldLog(lastLog, 2))
                        {
                            lastLog = DateTime.UtcNow.Ticks;
                            logger.InfoFormat("monitorEventoStreamer({0}) fila: {1}", idx, arrCQueue[idx].Count);
                        }

                        continue;
                    }
                    lock (arrLock[idx])
                        Monitor.Wait(arrLock[idx], 100);
                }
                catch (Exception ex)
                {
                    logger.Error("Problemas no processamento do evento: " + ex.Message, ex);
                }

            }
        }

        private void _addToEvento(int idx, TOEventosStreamer to)
        {
            try
            {
                //bool bsinaliza = arrCQueue[idx].IsEmpty;
                arrCQueue[idx].Enqueue(to);
                //if (bsinaliza)
                //{
                //    lock (arrLock[idx])
                //        Monitor.Pulse(arrLock[idx]);
                //}
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no enfileiramento do evento streamer: " + ex.Message, ex);
            }

        }


        private void enviaMensagemStreamer(NegocioBase registroNegocio, string instrumento)
        {
            EventoHttpNegocio eventoNEG = new EventoHttpNegocio();
            eventoNEG.instrumento = instrumento;
            eventoNEG.cabecalho = MDSUtils.montaCabecalhoStreamer(ConstantesMDS.TIPO_REQUISICAO_NEGOCIOS, registroNegocio.TipoBolsa, 0, instrumento, registroNegocio.CasasDecimais, null);
            eventoNEG.negocio = NegocioBase.montarNegocioStreamer(registroNegocio.Negocio, registroNegocio.CasasDecimais);
            EventQueueManager.Instance.SendEvent(eventoNEG);
        }

        private void enviaMensagemHomeBrokerOuCotacaoStreamer(NegocioBase registroNegocio, string instrumento)
        {
            // Envia evento para o HomeBroker/CotacaoStreamer
            EventoHBNegocio eventoHBNeg = new EventoHBNegocio();
            eventoHBNeg.instrumento = instrumento;
            eventoHBNeg.mensagem = NegocioBase.montarNegocioHomeBroker(registroNegocio);
            EventQueueManager.Instance.SendEvent(eventoHBNeg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="umdfMessage"></param>
        /// <returns></returns>
        private String processaMensagemSecurityList(String msgID, Message umdfMessage)
        {
            GroupValue relatedSym;
            if (umdfMessage.Template.HasField("RelatedSymbols") && umdfMessage.IsDefined("RelatedSymbols"))
                relatedSym = umdfMessage.GetGroup("RelatedSymbols");
            else
                relatedSym = umdfMessage.GetGroup("RelatedSym");

            String instrumento = relatedSym.GetString("Symbol");
            String securityID = relatedSym.GetString("SecurityID");

            logger.InfoFormat("Instrumento[{0}]: SecurityID[{1}]", instrumento, securityID);
            dicSecurityID.AddOrUpdate(securityID, instrumento, (key, oldValue) => instrumento);

            return instrumento;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instrumento"></param>
        /// <param name="umdfMessage"></param>
        private void atualizaCadastroBasico(String instrumento, Message umdfMessage, bool isPuma20, string tipoBolsa)
        {
            // Se ocorreu atualizacao do cadastro basico de um instrumento no mesmo segundo, ignorar
            if (dctNegocios[instrumento] != null && 
                dctNegocios[instrumento].DataAtualizacao.ToString("yyyyMMddHHmmss").Equals(DateTime.Now.ToString("yyyyMMddHHmmss")))
                return;

            GroupValue relatedSym;
            if (umdfMessage.Template.HasField("RelatedSymbols") && umdfMessage.IsDefined("RelatedSymbols"))
                relatedSym = umdfMessage.GetGroup("RelatedSymbols");
            else
                relatedSym = umdfMessage.GetGroup("RelatedSym");

            string grupoCotacao = (relatedSym.IsDefined("SecurityGroup") ? relatedSym.GetString("SecurityGroup") : "");
            logger.InfoFormat("Instrumento[{0}]: GrupoCotacao[{1}]", instrumento, grupoCotacao);

            string empresa = (relatedSym.IsDefined("SecurityDesc") ? relatedSym.GetString("SecurityDesc") : "");
            logger.InfoFormat("Instrumento[{0}]: Empresa[{1}]", instrumento, empresa);

            string segmentoMercado = (relatedSym.IsDefined("SecurityType") ? relatedSym.GetString("SecurityType") : "");
            if (isPuma20)
            {
                if (segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_VISTA) || 
                    segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_ACOES_ORDINARIAS) || 
                    segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_ACOES_PREFERENCIAS) || 
                    segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_CORPORATE_FIXED_INCOME))
                    segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_VISTA;
                else if (segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_TERMO))
                    segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_TERMO;
                else if (segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_OPCOES) || 
                         segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_OPCOES_INDICE))
                    segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_OPCOES;
                else if (segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_EXERCICIO_OPCOES))
                    segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_EXERCICIO_DE_OPCOES;
                else if (segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_INDICE))
                    segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_INDICES;
                else if (segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_ETF))
                    segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_ETF;
                else if (segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_MULTILEG) ||
                         segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_EMPRESTIMO_OU_BTC))
                    segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_RESERVADO;
            }
            else
            {
                if (segmentoMercado.Equals(ConstantesUMDF.UMDF_SECURITY_TYPE_BOVESPA_VISTA))
                    segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_VISTA;
                else if (segmentoMercado.Equals(ConstantesUMDF.UMDF_SECURITY_TYPE_BOVESPA_TERMO))
                    segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_TERMO;
                else if (segmentoMercado.Equals(ConstantesUMDF.UMDF_SECURITY_TYPE_BOVESPA_OPCOES))
                    segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_OPCOES;
                else if (segmentoMercado.Equals(ConstantesUMDF.UMDF_SECURITY_TYPE_BOVESPA_EXERCICIO_OPCOES))
                    segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_EXERCICIO_DE_OPCOES;
                else if (segmentoMercado.Equals(ConstantesUMDF.UMDF_SECURITY_TYPE_BOVESPA_INDICE))
                    segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_INDICES;
                else if (segmentoMercado.Equals(ConstantesUMDF.UMDF_SECURITY_TYPE_BOVESPA_ETF))
                    segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_ETF;
            }
            logger.InfoFormat("Instrumento[{0}]: SegmentoMercado[{1}]", instrumento, segmentoMercado);

            int formaCotacao = 1;
            if (!segmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_INDICES))
            {
                if (relatedSym.IsDefined("PriceDivisor"))
                {
                    int divisor = relatedSym.GetInt("PriceDivisor");
                    if (divisor == 100)
                        formaCotacao = 3;
                    else if (divisor == 1000)
                        formaCotacao = 4;
                    else if (divisor == 10000)
                        formaCotacao = 5;
                }
            }
            logger.InfoFormat("Instrumento[{0}]: FormaCotacao[{1}]", instrumento, formaCotacao);


            Decimal coeficienteMultiplicacao = (relatedSym.IsDefined("ContractMultiplier") ? relatedSym.GetBigDecimal("ContractMultiplier") : 1);
            logger.InfoFormat("Instrumento[{0}]: CoeficienteMultiplicacao[{1}]", instrumento, coeficienteMultiplicacao);

            string securityIDSource = "";
            string codigoPapelObjeto = instrumento;
            if (relatedSym.IsDefined("Underlyings"))
            {
                SequenceValue underlyings = relatedSym.GetSequence("Underlyings");
                if (underlyings != null && underlyings.Values.Length > 0)
                {
                    GroupValue underlying = underlyings.Values[0];
                    securityIDSource = (underlying.IsDefined("UnderlyingSecurityIDSource") ? underlying.GetString("UnderlyingSecurityIDSource") : "");

                    if (tipoBolsa.Equals(ConstantesUMDF.UMDF_SEGMENT_DERIVATIVES))
                    {
                        // BM&F
                        if (relatedSym.IsDefined("Asset"))
                            codigoPapelObjeto = relatedSym.GetString("Asset");
                    }
                    else
                    {
                        // Bovespa
                        if (underlying.IsDefined("UnderlyingSymbol"))
                            codigoPapelObjeto = underlying.GetString("UnderlyingSymbol");
                    }
                }
            }
            logger.InfoFormat("Instrumento[{0}]: CodigoPapelObjeto[{1}]", instrumento, codigoPapelObjeto);
            logger.InfoFormat("Instrumento[{0}]: SecurityIDSource[{1}]", instrumento, securityIDSource);

            string codigoISIN = "";
            if (relatedSym.IsDefined("SecurityAltID"))
            {
                // Bovespa
                SequenceValue securityAltIDs = relatedSym.GetSequence("SecurityAltID");
                if (securityAltIDs != null && securityAltIDs.Values.Length > 0)
                {
                    GroupValue securityAltID = securityAltIDs.Values[0];
                    if (securityAltID.IsDefined("SecurityAltID"))
                        codigoISIN = securityAltID.GetString("SecurityAltID");
                    else
                        codigoISIN = (dctNegocios.ContainsKey(codigoPapelObjeto) ? dctNegocios[codigoPapelObjeto].CodigoISIN : "");
                }
            }
            else if (relatedSym.IsDefined("SecurityAltIDs"))
            {
                // BM&F ou Bovespa
                SequenceValue securityAltIDs = relatedSym.GetSequence("SecurityAltIDs");
                if (securityAltIDs != null && securityAltIDs.Values.Length > 0)
                {
                    Dictionary<string, string> securityAltIDValues = new Dictionary<string,string>();
                    for (int j=0; j < securityAltIDs.Values.Length; j++)
                    {
                        GroupValue securityAltID = securityAltIDs.Values[j];
                        if (securityAltID.IsDefined("SecurityAltID") && securityAltID.IsDefined("SecurityAltIDSource"))
                            securityAltIDValues.Add(securityAltID.GetString("SecurityAltIDSource"), securityAltID.GetString("SecurityAltID"));
                    }
                    if (tipoBolsa.Equals(ConstantesUMDF.UMDF_SEGMENT_DERIVATIVES))
                    {
                        if (securityAltIDValues.ContainsKey("8"))
                            codigoISIN = securityAltIDValues["8"];
                        else
                            codigoISIN = (dctNegocios.ContainsKey(codigoPapelObjeto) ? dctNegocios[codigoPapelObjeto].CodigoISIN : "");
                    }
                    else
                    {
                        if (securityAltIDValues.ContainsKey("4"))
                            codigoISIN = securityAltIDValues["4"];
                        else
                            codigoISIN = (dctNegocios.ContainsKey(codigoPapelObjeto) ? dctNegocios[codigoPapelObjeto].CodigoISIN : "");
                    }
                }
            }
            logger.InfoFormat("Instrumento[{0}]: CodigoISIN[{1}]", instrumento, codigoISIN);

            int lotePadrao = 0;
            if (relatedSym.IsDefined("RoundLot"))
                lotePadrao = relatedSym.GetInt("RoundLot");
            else if (relatedSym.IsDefined("Lots"))
            {
                SequenceValue lots = relatedSym.GetSequence("Lots");
                if (lots != null && lots.Values.Length > 0)
                {
                    Dictionary<string, int> lotValues = new Dictionary<string, int>();
                    for (int j = 0; j < lots.Values.Length; j++)
                    {
                        GroupValue lot = lots.Values[j];
                        if (lot.IsDefined("MinLotSize") && lot.IsDefined("LotType"))
                            lotValues.Add(lot.GetString("LotType"), lot.GetInt("MinLotSize"));
                    }
                    if (lotValues.ContainsKey("1"))
                        lotePadrao = lotValues["1"];
                    else if (lotValues.ContainsKey("2"))
                        lotePadrao = lotValues["2"];
                }
            }            
            logger.InfoFormat("Instrumento[{0}]: LotePadrao[{1}]", instrumento, lotePadrao);

            int loteMinimo = (relatedSym.IsDefined("MinOrderQty") ? relatedSym.GetInt("MinOrderQty") : 0);
            logger.InfoFormat("Instrumento[{0}]: LoteMinimo[{1}]", instrumento, loteMinimo);

            string dataVencimento = (relatedSym.IsDefined("MaturityDate") ? relatedSym.GetString("MaturityDate") : "00010101");
            if (dataVencimento.Equals("99991231"))
                dataVencimento = "00010101";
            logger.InfoFormat("Instrumento[{0}]: DataVencimento[{1}]", instrumento, dataVencimento);

            Decimal precoExercicio = (relatedSym.IsDefined("StrikePrice") ? relatedSym.GetBigDecimal("StrikePrice") : Decimal.Zero);
            logger.InfoFormat("Instrumento[{0}]: precoExercicio[{1}]", instrumento, precoExercicio);

            string indicadorOpcao = (relatedSym.IsDefined("PutOrCall") ? relatedSym.GetString("PutOrCall") : " ");
            if (indicadorOpcao.Equals("1"))
                indicadorOpcao = ConstantesMDS.INDICADOR_OPCAO_BOVESPA_OPCAO_COMPRA;
            else if (indicadorOpcao.Equals("0"))
                indicadorOpcao = ConstantesMDS.INDICADOR_OPCAO_BOVESPA_OPCAO_VENDA;
            logger.InfoFormat("Instrumento[{0}]: IndicadorOpcao[{1}]", instrumento, indicadorOpcao);

            int casasDecimais = (relatedSym.IsDefined("TickSizeDenominator") ? relatedSym.GetInt("TickSizeDenominator") : 2);
            logger.InfoFormat("Instrumento[{0}]: CasasDecimais[{1}]", instrumento, casasDecimais);

            NegocioBase registroNegocio = dctNegocios[instrumento];
            if (registroNegocio != null)
            {
                string dataHoraUltimoNegocio = "00000000000000";

                if (dctUltimoNeg.ContainsKey(instrumento))
                    dataHoraUltimoNegocio = dctUltimoNeg[instrumento];

                logger.InfoFormat("Instrumento[{0}]: dataHoraUltimoNegocio[{1}]", instrumento, dataHoraUltimoNegocio);

                registroNegocio.DataAtualizacao = DateTime.Now;
                registroNegocio.Negocio.Data = dataHoraUltimoNegocio.Substring(0, 8);
                registroNegocio.Negocio.Hora = dataHoraUltimoNegocio.Substring(8, 6);
                registroNegocio.Negocio.HorarioTeorico = DateTime.MinValue;
                registroNegocio.FormaCotacao = formaCotacao;
                registroNegocio.GrupoCotacao = grupoCotacao;
                registroNegocio.Especificacao = empresa;
                registroNegocio.SegmentoMercado = segmentoMercado;
                registroNegocio.CoeficienteMultiplicacao = coeficienteMultiplicacao;
                registroNegocio.CodigoPapelObjeto = codigoPapelObjeto;
                registroNegocio.CodigoISIN = codigoISIN;
                registroNegocio.LotePadrao = lotePadrao;
                registroNegocio.DataVencimento = dataVencimento;
                registroNegocio.PrecoExercicio = precoExercicio;
                registroNegocio.IndicadorOpcao = indicadorOpcao;
                registroNegocio.CasasDecimais = casasDecimais;
                registroNegocio.SecurityIDSource = securityIDSource;
                registroNegocio.FaseNegociacao = ConstantesMDS.FASE_NEGOCIACAO_ATUALIZAR_ANALISE_GRAFICA;

                // Se for Instrumento Bovespa a Vista, Termo ou Opcao, acrescenta no IBOV_TOTAL na lista de indices para contabilizacao
                if (registroNegocio.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_VISTA) ||
                    registroNegocio.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_TERMO) ||
                    registroNegocio.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_OPCOES) ||
                    registroNegocio.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_EXERCICIO_DE_OPCOES))
                {
                    if (!dctComposicaoIndice.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_TOTAL))
                        dctComposicaoIndice.Add(ConstantesMDS.INDICE_IBOVESPA_TOTAL, new List<string>());

                    if (!dctComposicaoIndice[ConstantesMDS.INDICE_IBOVESPA_TOTAL].Contains(instrumento))
                        dctComposicaoIndice[ConstantesMDS.INDICE_IBOVESPA_TOTAL].Add(instrumento);
                }

                // Se for Instrumento Bovespa a Vista, acrescenta no IBOV_VISTA na lista de indices para contabilizacao
                if (registroNegocio.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_VISTA))
                {
                    if (!dctComposicaoIndice.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_VISTA))
                        dctComposicaoIndice.Add(ConstantesMDS.INDICE_IBOVESPA_VISTA, new List<string>());

                    if (!dctComposicaoIndice[ConstantesMDS.INDICE_IBOVESPA_VISTA].Contains(instrumento))
                        dctComposicaoIndice[ConstantesMDS.INDICE_IBOVESPA_VISTA].Add(instrumento);
                }

                // Se for Instrumento Bovespa Termo, acrescenta no IBOV_TERMO na lista de indices para contabilizacao
                if (registroNegocio.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_TERMO))
                {
                    if (!dctComposicaoIndice.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_TERMO))
                        dctComposicaoIndice.Add(ConstantesMDS.INDICE_IBOVESPA_TERMO, new List<string>());

                    if (!dctComposicaoIndice[ConstantesMDS.INDICE_IBOVESPA_TERMO].Contains(instrumento))
                        dctComposicaoIndice[ConstantesMDS.INDICE_IBOVESPA_TERMO].Add(instrumento);
                }

                // Se for Instrumento Bovespa Opcao, acrescenta no IBOV_OPCAO na lista de indices para contabilizacao
                if (registroNegocio.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_OPCOES) ||
                    registroNegocio.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_EXERCICIO_DE_OPCOES))
                {
                    if (!dctComposicaoIndice.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_OPCOES))
                        dctComposicaoIndice.Add(ConstantesMDS.INDICE_IBOVESPA_OPCOES, new List<string>());

                    if (!dctComposicaoIndice[ConstantesMDS.INDICE_IBOVESPA_OPCOES].Contains(instrumento))
                        dctComposicaoIndice[ConstantesMDS.INDICE_IBOVESPA_OPCOES].Add(instrumento);
                }

                if (bGerarEventoANG)
                {
                    EventoNegocioANG eventoANG = new EventoNegocioANG();
                    eventoANG.instrumento = instrumento;
                    eventoANG.mensagem = NegocioBase.montarCadastroBasico(registroNegocio);
                    if (!eventoANG.mensagem.Equals(""))
                        EventQueueManager.Instance.SendEvent(eventoANG);
                }
            }

            if (segmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_INDICES) && relatedSym.IsDefined("Underlyings"))
                atualizaComposicaoIndice(instrumento, relatedSym);

            return;
        }

                /// <summary>
        /// 
        /// </summary>
        /// <param name="indice"></param>
        /// <param name="umdfMessage"></param>
        private void atualizaComposicaoIndice(String indice, GroupValue relatedSym)
        {
            logger.InfoFormat("Indice[{0}]: Obtendo a lista de todos os papeis atrelados", indice);

            if (!dctComposicaoIndice.ContainsKey(indice))
            {
                dctComposicaoIndice.Add(indice, new List<string>());
            }

            SequenceValue sequenceValue = relatedSym.GetSequence("Underlyings");
            if (sequenceValue != null)
            {
                GroupValue[] seqValues = sequenceValue.Values;
                for (int j=0; j<seqValues.Length; j++) 
                {
                    if ( seqValues[j].IsDefined("UnderlyingSymbol") )
                    {
                        string item = seqValues[j].GetString("UnderlyingSymbol");

                        logger.InfoFormat("Indice[{0}]: Composicao [{1}]", indice, item);
                        if (!dctComposicaoIndice[indice].Contains(item))
                            dctComposicaoIndice[indice].Add(item);
                    }
                }

                if (bGerarEventoANG)
                {
                    EventoNegocioANG eventoANG = new EventoNegocioANG();
                    eventoANG.instrumento = indice;
                    eventoANG.mensagem = NegocioBase.montarComposicaoIndice(indice, dctComposicaoIndice[indice]);
                    EventQueueManager.Instance.SendEvent(eventoANG);
                }
            }
        }

        private void atualizaGrupoCotacao(String instrumento)
        {
            string grupoCotacao = dctNegocios[instrumento].GrupoCotacao;

            if (!dctGrupoCotacao.ContainsKey(grupoCotacao))
            {
                logger.InfoFormat("GrupoCotacao[{0}]: Criado!", grupoCotacao);

                GrupoCotacaoInfo grupoCotacaoInfo = new GrupoCotacaoInfo();
                grupoCotacaoInfo.estado = ConstantesMDS.ESTADO_PAPEL_NAO_NEGOCIADO;
                grupoCotacaoInfo.listaGrupoCotacao = new List<string>();
                dctGrupoCotacao.Add(grupoCotacao, grupoCotacaoInfo);
            }

            if (!dctGrupoCotacao[grupoCotacao].listaGrupoCotacao.Contains(instrumento))
            {
                logger.InfoFormat("GrupoCotacao[{0}]: Inserido Instrumento[{1}]", grupoCotacao, instrumento);
                dctGrupoCotacao[grupoCotacao].listaGrupoCotacao.Add(instrumento);
            }
        }

        private String processaMensagemMarketData(String instrumento, String msgID, Message umdfMessage, List<Dictionary<string, string>> streamerLNG, string msgType, int marketDepth, bool isPuma, bool isBMF, ref bool sendToStreamer)
        {
            GroupValue mdEntry = umdfMessage.GetGroup("MDEntries");
            String mdEntryType = mdEntry.GetString("MDEntryType");

            switch(mdEntryType)
            {
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_PRICE_BAND:  
                    // Tratar mensagem de Precos Referenciais
                    tratarMensagemBandaPrecos(instrumento, msgID, mdEntry);
                    sendToStreamer = false;
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_OPENING_PRICE:
                    // Tratar mensagem de Preco de Abertura
                    tratarMensagemPrecoAbertura(instrumento, msgID, mdEntry, msgType, isPuma);
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_CLOSING_PRICE:
                    // Tratar mensagem de Preco de Fechamento
                    tratarMensagemPrecoFechamento(instrumento, msgID, mdEntry);
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_SESSION_VWAP_PRICE:
                    // Tratar mensagem de Preco Medio
                    tratarMensagemPrecoMedio( instrumento, msgID, mdEntry);
                    sendToStreamer = false;
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_SESSION_HIGH_PRICE:
                    // Tratar mensagem de Maxima do dia
                    tratarMensagemMaximoDia( instrumento, msgID, mdEntry);
                    sendToStreamer = false;
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_SESSION_LOW_PRICE:
                    // Tratar mensagem de Minima do dia
                    tratarMensagemMinimoDia( instrumento, msgID, mdEntry);
                    sendToStreamer = false;
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_TRADE:
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_INDEX_VALUE:
                    // Tratar mensagem de negocio
                    tratarMensagemNegocio(instrumento, msgID, mdEntry, streamerLNG, msgType, isPuma, isBMF);
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_TRADE_VOLUME:
                    // Tratar mensagem de Volume de Negocios
                    tratarMensagemVolumeNegocios( instrumento, msgID, mdEntry);
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_SETTLEMENT_PRICE:
                    tratarMensagemPrecoAjuste( instrumento, msgID, mdEntry);
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_SECURITY_TRADING_STATE:
                    // Tratar mensagem de Estado do Instrumento
                    tratarMensagemEstadoInstrumento(instrumento, msgID, umdfMessage, mdEntry, msgType, isPuma);
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_BID:
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_OFFER:
                    tratarMensagemMelhorOfertaPuma(instrumento, msgID, mdEntry);
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_QUANTITY_BAND:
                    logger.WarnFormat("Instrumento[{0}]: MDEntryType QuantityBand(h) sem tratamento", instrumento);
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_OPEN_INTEREST:
                    logger.WarnFormat("Instrumento[{0}]: MDEntryType OpenInterest(C) sem tratamento", instrumento);
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_IMBALANCE:
                    tratarMensagemImbalance(instrumento, msgID, umdfMessage, mdEntry, isPuma);
                    break;
                default:
                    logger.Error("MDEntryType invalido [" + mdEntryType + "]");
                    break;
            }

            return instrumento;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instrumento"></param>
        /// <param name="msgID"></param>
        /// <param name="umdfMessage"></param>
        /// <param name="mdEntry"></param>
        /// <param name="isPuma"></param>
        private void tratarMensagemImbalance(string instrumento, string msgID, Message umdfMessage, GroupValue mdEntry, bool isPuma)
        {
            long qtderemanescente = 0;
            NegocioBase registroNegocios = dctNegocios[instrumento];

            bool moreBuyers = true;
            if (mdEntry.IsDefined("TradeCondition"))
            {
                if (mdEntry.GetString("TradeCondition").Equals("Q"))
                {
                    moreBuyers = false;
                }
            }

            if (mdEntry.IsDefined("MDEntrySize"))
            {
                qtderemanescente = mdEntry.GetLong("MDEntrySize");
                long qtdeLeilao = registroNegocios.Negocio.Quantidade;

                if (moreBuyers)
                {
                    registroNegocios.Negocio.MelhorQuantidadeVenda = qtdeLeilao;
                    registroNegocios.Negocio.MelhorQuantidadeCompra = qtdeLeilao + qtderemanescente;
                }
                else
                {
                    registroNegocios.Negocio.MelhorQuantidadeCompra = qtdeLeilao;
                    registroNegocios.Negocio.MelhorQuantidadeVenda = qtdeLeilao + qtderemanescente;
                }

                registroNegocios.Negocio.MelhorPrecoVenda = registroNegocios.Negocio.PrecoTeoricoAbertura;
                registroNegocios.Negocio.MelhorPrecoCompra = registroNegocios.Negocio.PrecoTeoricoAbertura;
            }

            lock (dctNegocios[instrumento])
            {
                dctNegocios[instrumento] = registroNegocios;
            }
        }

        // Tratar mensagem de Precos Referenciais
        public void tratarMensagemBandaPrecos( string instrumento, string numSeq, GroupValue mdEntry)
        {
            Decimal precoAjuste = Decimal.Zero;
            NegocioBase registroNegocios = dctNegocios[instrumento];

            if (mdEntry.IsDefined("TradingReferencePrice"))
            {
                precoAjuste = mdEntry.GetBigDecimal("TradingReferencePrice");
            }
            else if (mdEntry.IsDefined("TradingRefPrice"))
            {
                precoAjuste = mdEntry.GetBigDecimal("TradingRefPrice");
            }

            // Preferi tirar esse calculo, tava gerando um precoAjuste muito estranho
            /*else if (mdEntry.IsDefined("HighLimitPrice") && mdEntry.IsDefined("LowLimitPrice"))
            {
                Decimal precoLimiteMax = mdEntry.GetBigDecimal("HighLimitPrice");
                Decimal precoLimiteMin = mdEntry.GetBigDecimal("LowLimitPrice");
                precoAjuste = (precoLimiteMax + precoLimiteMin) / 2;

                //precoAjuste = String.format("%013.2f",
                //        precoAjusteCalculado.doubleValue()).replace('.', ',');
            }*/

            if (precoAjuste != Decimal.Zero)
            {
                registroNegocios.Negocio.PrecoAjuste = precoAjuste;
                registroNegocios.Negocio.MsgIdAnterior = numSeq;

                //lock (dctNegocios[instrumento])
                //{
                //    dctNegocios[instrumento] = registroNegocios;
                //}

                logger.Info("Instrumento[" + instrumento +
                        "] sequencial[" + numSeq +
                        "] precoAjuste[" + precoAjuste + "]");
            }

            return;
        }

        // Tratar mensagem de Preco de Abertura
        public void tratarMensagemPrecoAbertura(string instrumento, string numSeq, GroupValue mdEntry, string msgType, bool isPuma)
        {
            NegocioBase registroNegocios = dctNegocios[instrumento];

            Decimal ultimoPrecoAbertura = registroNegocios.Negocio.PrecoAbertura;

            if (mdEntry.IsDefined("MDEntryPx"))
            {
                Decimal precoAbertura = mdEntry.GetBigDecimal("MDEntryPx");
                long quantidade = 0;
                if (mdEntry.IsDefined("MDEntrySize"))
		            quantidade = mdEntry.GetInt("MDEntrySize");

                if (precoAbertura > 0)
                {
                    if (mdEntry.IsDefined("MDEntryDate") && !msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_SNAPSHOT_SINGLE))
                    {
                        try
                        {
                            String data = mdEntry.GetString("MDEntryDate");
                            String formatHora = (isPuma ? "{0,9:d9}" : "{0:d6}");
                            String hora = UmdfUtils.convertUTC2Local(data, String.Format(formatHora, mdEntry.GetInt("MDEntryTime")));
                            registroNegocios.Negocio.Data = data;
                            registroNegocios.Negocio.Hora = hora;
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Falha na formatacao da data[" + mdEntry.GetInt("MDEntryDate") +
                                    "] e hora[" + mdEntry.GetInt("MDEntryTime") + "]: " + ex.Message);
                        }
                    }

                    registroNegocios.Negocio.MsgIdAnterior = numSeq;

                    string OpenCloseSettlFlag = ConstantesUMDF.UMDF_OCSF_UNDOCUMENTED_OPENING_PRICE;

                    if (mdEntry.IsDefined("OpenCloseSettlFlag"))
                        OpenCloseSettlFlag = mdEntry.GetString("OpenCloseSettlFlag");
                    else if (mdEntry.IsDefined("OpenCloseSettleFlag"))
                        OpenCloseSettlFlag = mdEntry.GetString("OpenCloseSettleFlag");

                    switch (OpenCloseSettlFlag)
                    {
                        case ConstantesUMDF.UMDF_OCSF_THEORETICAL_PRICE:

                            // Forçar leilão aqui é um recurso que tem no MDS versão java, mas talvez esteja errada essa implementação, requer melhor avaliação
                            //registroNegocios.Negocio.EstadoInstrumento = ConstantesMDS.ESTADO_PAPEL_EM_LEILAO;
                            //logger.InfoFormat("Instrumento[{0}] estadoInstrumento[{1}]", instrumento, registroNegocios.Negocio.EstadoInstrumento);

                            registroNegocios.Negocio.PrecoTeoricoAbertura = precoAbertura;
                            registroNegocios.Negocio.Quantidade = quantidade;
                            registroNegocios.Negocio.VariacaoTeorica = MDSUtils.calcularVariacao(precoAbertura, registroNegocios.Negocio.PrecoFechamento);

                            logger.InfoFormat("Instrumento[{0}] sequencial[{1}] precoTeoricoAbertura[{2}] VariacaoTeorica[{3}]",
                                instrumento, numSeq, registroNegocios.Negocio.PrecoTeoricoAbertura, registroNegocios.Negocio.VariacaoTeorica);

                            if (bGerarEventoANG)
                            {
                                verificaSuspensaoAnaliseGrafica(registroNegocios);

                                EventoNegocioANG eventoANG = new EventoNegocioANG();
                                eventoANG.instrumento = instrumento;
                                eventoANG.mensagem = NegocioBase.montarNegocioAnaliseGrafica(registroNegocios);
                                EventQueueManager.Instance.SendEvent(eventoANG);
                            }
                            break;

                        case ConstantesUMDF.UMDF_OCSF_SESSION_SETTLEMENT_ENTRY:
                        case ConstantesUMDF.UMDF_OCSF_DAILY_SETTLEMENT_ENTRY:
                        case ConstantesUMDF.UMDF_OCSF_UNDOCUMENTED_OPENING_PRICE:
                            registroNegocios.Negocio.PrecoAbertura = precoAbertura;
                            registroNegocios.Negocio.Quantidade = quantidade;

                            logger.InfoFormat("Instrumento[{0}] sequencial[{1}] precoAbertura[{2}]",
                                instrumento, numSeq, registroNegocios.Negocio.PrecoAbertura);

                            if (bGerarEventoANG)
                            {
                                ThreadPool.QueueUserWorkItem(
                                    new WaitCallback(
                                        delegate(object required)
                                        {
                                            EventoNegocioANG eventoANG = new EventoNegocioANG();
                                            eventoANG.instrumento = instrumento;
                                            eventoANG.mensagem = NegocioBase.montarPrecoAbertura(registroNegocios);
                                            EventQueueManager.Instance.SendEvent(eventoANG);
                                        }
                                    )
                                );
                            }

                            /*
                            if (registroNegocios.Negocio.QtdeNegocios > 0)
                            {
                                logger.InfoFormat("Instrumento[{0}]: SerieHistorica QtdNegocios[{1}]",
                                    instrumento, registroNegocios.Negocio.QtdeNegocios);
                                enviaMensagemFila(queueSerieHistorica, JsonConvert.SerializeObject(registroNegocios));
                            }
                            */

                            if (dctComposicaoIndice.ContainsKey(instrumento) &&
                                ultimoPrecoAbertura != precoAbertura  &&
                                ultimoPrecoAbertura != 0 &&
                                true==false)
                            {
                                registroNegocios.Negocio.Preco = 0;
                                registroNegocios.Negocio.PrecoMedio = 0;
                                registroNegocios.Negocio.PrecoMinimo = 0;
                                registroNegocios.Negocio.PrecoMaximo = 0;
                                registroNegocios.Negocio.Quantidade = 0;
                                registroNegocios.Negocio.Variacao = 0;
                                registroNegocios.Negocio.QtdeNegociadaDia = 0;
                                registroNegocios.Negocio.Compradora = "0";
                                registroNegocios.Negocio.Vendedora = "0";
                                registroNegocios.Negocio.NumeroNegocio = "0";
                                registroNegocios.Negocio.QtdeNegocios = 0;
                                registroNegocios.Negocio.VolumeTotal = 0;
                                registroNegocios.FaseNegociacao = ConstantesMDS.FASE_NEGOCIACAO_ATUALIZAR_ANALISE_GRAFICA;
                                registroNegocios.LivroNegocios = new List<LNGDadosNegocio>();

                                logger.InfoFormat("Instrumento[{0}]: Inicializando contadores do indice!", instrumento);

                                if (instrumento.Equals(ConstantesMDS.INDICE_IBOVESPA))
                                {
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TOTAL].Negocio.QtdeNegociadaDia = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TOTAL].Negocio.NumeroNegocio = "0";
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TOTAL].Negocio.QtdeNegocios = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TOTAL].Negocio.VolumeTotal = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_VISTA].Negocio.QtdeNegociadaDia = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_VISTA].Negocio.NumeroNegocio = "0";
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_VISTA].Negocio.QtdeNegocios = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_VISTA].Negocio.VolumeTotal = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TERMO].Negocio.QtdeNegociadaDia = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TERMO].Negocio.NumeroNegocio = "0";
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TERMO].Negocio.QtdeNegocios = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TERMO].Negocio.VolumeTotal = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_OPCOES].Negocio.QtdeNegociadaDia = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_OPCOES].Negocio.NumeroNegocio = "0";
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_OPCOES].Negocio.QtdeNegocios = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_OPCOES].Negocio.VolumeTotal = 0;
                                }
                            }
                            break;

                        case ConstantesUMDF.UMDF_OCSF_ENTRY_FROM_PREVIOUS_BUSINESS_DAY:
                            //registroNegocios.Negocio.PrecoFechamento = precoAbertura;
                            //registroNegocios.Negocio.Quantidade = quantidade;
                            break;
                        default:
                            logger.Error("Invalid OpenCloseSettlFlag[" + OpenCloseSettlFlag + "]");
                            break;
                    }

                    lock (dctNegocios[instrumento])
                    {
                        dctNegocios[instrumento] = registroNegocios;
                    }
                }
            }
            return;
        }

        // Tratar mensagem de Preco de Fechamento
        public void tratarMensagemPrecoFechamento(string instrumento, string numSeq, GroupValue mdEntry)
        {
            NegocioBase registroNegocios = dctNegocios[instrumento];

            if (mdEntry.IsDefined("MDEntryPx"))
            {
                Decimal precoFechamento = mdEntry.GetBigDecimal("MDEntryPx");
                string mdEntryDate = mdEntry.GetString("MDEntryDate");

                string openCloseSettlFlag = null;
                if (mdEntry.IsDefined("OpenCloseSettlFlag"))
                    openCloseSettlFlag = mdEntry.GetString("OpenCloseSettlFlag");
                else if (mdEntry.IsDefined("OpenCloseSettleFlag"))
                    openCloseSettlFlag = mdEntry.GetString("OpenCloseSettleFlag");

                if (openCloseSettlFlag != null)
                {
                    if (openCloseSettlFlag.Equals(ConstantesUMDF.UMDF_OCSF_ENTRY_FROM_PREVIOUS_BUSINESS_DAY) ||
                       (openCloseSettlFlag.Equals(ConstantesUMDF.UMDF_OCSF_DAILY_SETTLEMENT_ENTRY) && registroNegocios.Negocio.PrecoFechamento == 0) ||
                       (openCloseSettlFlag.Equals(ConstantesUMDF.UMDF_OCSF_DAILY_SETTLEMENT_ENTRY) && registroNegocios.TipoBolsa.Equals(ConstantesMDS.DESCRICAO_DE_BOLSA_BMF)))
                    {
                        registroNegocios.Negocio.MsgIdAnterior = numSeq;
                        //if (registroNegocios.Negocio.PrecoAjuste == 0)
                        registroNegocios.Negocio.PrecoFechamento = precoFechamento;

                        if (registroNegocios.Negocio.PrecoTeoricoAbertura != 0)
                        {
                            registroNegocios.Negocio.VariacaoTeorica = MDSUtils.calcularVariacao(
                                registroNegocios.Negocio.PrecoTeoricoAbertura,
                                registroNegocios.Negocio.PrecoFechamento);

                            logger.InfoFormat("Instrumento[{0}] sequencial[{1}] VariacaoTeorica[{2}]",
                            instrumento, numSeq, registroNegocios.Negocio.VariacaoTeorica);
                        }


                        logger.InfoFormat("Instrumento[{0}] sequencial[{1}] precoFechamento[{2}] OpenCloseSettlFlag[{3}] MDEntryDate[{4}]",
                            instrumento, numSeq, precoFechamento, openCloseSettlFlag, mdEntryDate);

                        if (openCloseSettlFlag.Equals(ConstantesUMDF.UMDF_OCSF_DAILY_SETTLEMENT_ENTRY) &&
                            registroNegocios.Negocio.QtdeNegocios > 0 &&
                            registroNegocios.Negocio.Data.Equals(DateTime.Now.ToString("yyyyMMdd")) &&
                            registroNegocios.FaseNegociacao != ConstantesMDS.FASE_NEGOCIACAO_ENVIADO_SERIE_HISTORICA)
                        {
                            registroNegocios.FaseNegociacao = ConstantesMDS.FASE_NEGOCIACAO_ENVIADO_SERIE_HISTORICA;

                            logger.InfoFormat("Instrumento[{0}]: SerieHistorica QtdNegocios[{1}]",
                                instrumento, registroNegocios.Negocio.QtdeNegocios);

                            if (bGerarEventoANG)
                            {
                                EventoNegocioANG eventoANG = new EventoNegocioANG();
                                eventoANG.instrumento = registroNegocios.Instrumento;
                                eventoANG.mensagem = NegocioBase.montarSerieHistorica(registroNegocios);
                                EventQueueManager.Instance.SendEvent(eventoANG);
                            }
                        }

                        lock (dctNegocios[instrumento])
                        {
                            dctNegocios[instrumento] = registroNegocios;
                        }
                    }
                    else
                        logger.InfoFormat("Desconsiderando Instrumento[{0}] sequencial[{1}] precoFechamento[{2}] OpenCloseSett[{3}]",
                            instrumento, numSeq, precoFechamento, openCloseSettlFlag);

                    if (bGerarEventoANG)
                    {
                        ThreadPool.QueueUserWorkItem(
                            new WaitCallback(
                                delegate(object required)
                                {
                                    EventoNegocioANG eventoANG = new EventoNegocioANG();
                                    eventoANG.instrumento = instrumento;
                                    eventoANG.mensagem = NegocioBase.montarPrecoFechamento(registroNegocios);
                                    EventQueueManager.Instance.SendEvent(eventoANG);
                                }
                            )
                        );
                    }
                }
                else
                {
                    logger.InfoFormat("Fechamento sem  OpenCloseSett Instrumento[{0}] sequencial[{1}] precoFechamento[{2}]",
                        instrumento, numSeq, precoFechamento);
                    if (registroNegocios.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_INDICES))
                    {
                        registroNegocios.Negocio.PrecoFechamento = precoFechamento;
                        
                        lock (dctNegocios[instrumento])
                        {
                            dctNegocios[instrumento] = registroNegocios;
                        }
                    }
                }
            }
            return;
        }

        // Tratar mensagem de Preco Medio
        public void tratarMensagemPrecoMedio( string instrumento, string numSeq, GroupValue mdEntry)
        {
            NegocioBase registroNegocios = dctNegocios[instrumento];

            if (mdEntry.IsDefined("MDEntryPx"))
            {
                Decimal precoMedio = mdEntry.GetBigDecimal("MDEntryPx");

                List<HashEntry> lstHashes = new List<HashEntry>();

                lstHashes.Add(new HashEntry("PrecoMedio", precoMedio.ToString()));

                if ( redisDB != null && redis.IsConnected)
                    redisDB.HashSet(instrumento, lstHashes.ToArray());

                //String precoAbertura = String.format("%013.2f",
                //        Double.parseDouble(precoAberturaSemFormato)).replace('.', ',');
                registroNegocios.Negocio.MsgIdAnterior = numSeq;
                registroNegocios.Negocio.PrecoMedio = precoMedio;

                //lock (dctNegocios[instrumento])
                //{
                //    dctNegocios[instrumento] = registroNegocios;
                //}

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Instrumento[" + instrumento +
                            "] sequencial[" + numSeq +
                            "] precoMedio[" + precoMedio + "]");
                }
            }
            return;
        }

        // Tratar mensagem de Maxima do dia
        public void tratarMensagemMaximoDia( string instrumento, string numSeq, GroupValue mdEntry)
        {
            NegocioBase registroNegocios = dctNegocios[instrumento];

            if (mdEntry.IsDefined("MDEntryPx"))
            {
                Decimal precoMaximo = mdEntry.GetBigDecimal("MDEntryPx");

                //String precoAbertura = String.format("%013.2f",
                //        Double.parseDouble(precoAberturaSemFormato)).replace('.', ',');
                registroNegocios.Negocio.MsgIdAnterior = numSeq;
                registroNegocios.Negocio.PrecoMaximo = precoMaximo;

                //lock (dctNegocios[instrumento])
                //{
                //    dctNegocios[instrumento] = registroNegocios;
                //}
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Instrumento[" + instrumento +
                            "] sequencial[" + numSeq +
                            "] precoMaximo[" + precoMaximo + "]");
                }
            }
            return;
        }

        // Tratar mensagem de Minima do dia
        public void tratarMensagemMinimoDia( string instrumento, string numSeq, GroupValue mdEntry)
        {
            NegocioBase registroNegocios = dctNegocios[instrumento];

            if (mdEntry.IsDefined("MDEntryPx"))
            {
                Decimal precoMinimo = mdEntry.GetBigDecimal("MDEntryPx");

                //String precoAbertura = String.format("%013.2f",
                //        Double.parseDouble(precoAberturaSemFormato)).replace('.', ',');
                registroNegocios.Negocio.MsgIdAnterior = numSeq;
                registroNegocios.Negocio.PrecoMinimo = precoMinimo;

                //lock (dctNegocios[instrumento])
                //{
                //    dctNegocios[instrumento] = registroNegocios;
                //}

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Instrumento[" + instrumento +
                            "] sequencial[" + numSeq +
                            "] precoMinimo[" + precoMinimo + "]");
                }
            }
            return;
        }

        // Tratar mensagem de Volume de Negocios
        public void tratarMensagemVolumeNegocios(string instrumento, string numSeq, GroupValue mdEntry)
        {
            NegocioBase registroNegocios = dctNegocios[instrumento];

            if (mdEntry.IsDefined("MDEntryPx"))
            {
                Decimal volumeTotal = mdEntry.GetBigDecimal("MDEntryPx");

                //String precoAbertura = String.format("%013.2f",
                //        Double.parseDouble(precoAberturaSemFormato)).replace('.', ',');
                registroNegocios.Negocio.MsgIdAnterior = numSeq;
                registroNegocios.Negocio.VolumeTotal = volumeTotal;

                //lock (dctNegocios[instrumento])
                //{
                //    dctNegocios[instrumento] = registroNegocios;
                //}

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Instrumento[" + instrumento +
                            "] sequencial[" + numSeq +
                            "] VolumeTotal[" + volumeTotal + "]");
                }
            }
            return;
        }


        public void tratarMensagemPrecoAjuste( string instrumento, string numSeq, GroupValue mdEntry)
        {
            NegocioBase registroNegocios = dctNegocios[instrumento];

            // Despreza precoAjuste quando OpenCloseSettlFlag <> 0 (Daily settlement entry)
            string openCloseSettlFlag = null;
            if (mdEntry.IsDefined("OpenCloseSettlFlag"))
                openCloseSettlFlag = mdEntry.GetString("OpenCloseSettlFlag");
            else if (mdEntry.IsDefined("OpenCloseSettleFlag"))
                openCloseSettlFlag = mdEntry.GetString("OpenCloseSettleFlag");

            string settlPriceType = null;
            if (mdEntry.IsDefined("SettlPriceType"))
                settlPriceType = mdEntry.GetString("SettlPriceType");
            else if (mdEntry.IsDefined("SettlePriceType"))
                settlPriceType = mdEntry.GetString("SettlePriceType");

            // PriceType = 1 - 
            string priceType = "2";
            if (mdEntry.IsDefined("PriceType"))
                priceType = mdEntry.GetString("PriceType");

            // Before session OpenCloseSettlFlag=4 SettlPriceType=1 => Previous day final settle
            // During session OpenCloseSettlFlag=4 SettlPriceType=3 => Previous day updated settle
            // During session OpenCloseSettlFlag=1 SettlPriceType=2 => Current day preview settle
            // After  session OpenCloseSettlFlag=1 SettlPriceType=1 => Current day final settle

            if (mdEntry.IsDefined("MDEntryPx"))
            {
                Decimal precoAjuste = mdEntry.GetBigDecimal("MDEntryPx");


                // Estaremos considerando que os valores altos, quando forem maiores que o valor de fechamento + 1000, sejam os
                // precos unitarios (P.U.) de alguns contratos da BMF (como DI1..., DDI...).
                // Como não tem nenhuma flag que identifique essa situação, foi necessário improvisar esse tratamento
                // ATP 2015-05-05:
                // Na verdade, ha uma flag, a PriceType
                // PriceType=1 - MDEntryPx em porcentagem 
                // PriceType=2 - MDEntryPx em preco unitario
                // O problema eh separar os ativos que sao negociados em taxa em lugar de preco.
                // Esta regra esta correta, mas nao eh elegante
                if (precoAjuste < registroNegocios.Negocio.PrecoFechamento + 1000) // || registroNegocios.Negocio.PrecoFechamento == 0)
                {
                    registroNegocios.Negocio.PrecoAjuste = precoAjuste;
                    registroNegocios.Negocio.MsgIdAnterior = numSeq;

                    if (!String.IsNullOrEmpty(settlPriceType))
                    {
                        if (settlPriceType.Equals("1"))
                        {
                            registroNegocios.Negocio.PrecoFechamento = precoAjuste;
                            logger.InfoFormat("Instrumento[{0}] sequencial[{1}] precoAjuste[{2}] openCloseSettlFlag[{3}] settlPriceType[{4}] PriceType[{5}] setando para fechamento",
                            instrumento, numSeq, precoAjuste, openCloseSettlFlag, settlPriceType, priceType);
                        }
                    }

                    if (bGerarEventoANG)
                    {
                        ThreadPool.QueueUserWorkItem(
                            new WaitCallback(
                                delegate(object required)
                                {
                                    EventoNegocioANG eventoANG = new EventoNegocioANG();
                                    eventoANG.instrumento = instrumento;
                                    eventoANG.mensagem = NegocioBase.montarPrecoAjuste(registroNegocios);
                                    EventQueueManager.Instance.SendEvent(eventoANG);
                                }
                            )
                        );
                    }

                    logger.InfoFormat("Instrumento[{0}] sequencial[{1}] precoAjuste[{2}] openCloseSettlFlag[{3}]",
                        instrumento, numSeq, precoAjuste, openCloseSettlFlag);

                    lock (dctNegocios[instrumento])
                    {
                        dctNegocios[instrumento] = registroNegocios;
                    }
                }
                else //if (precoAjuste >= registroNegocios.Negocio.PrecoFechamento + 1000)
                {
                    registroNegocios.Negocio.PrecoUnitario = precoAjuste;
                    registroNegocios.Negocio.MsgIdAnterior = numSeq;

                    if (bGerarEventoANG)
                    {
                        ThreadPool.QueueUserWorkItem(
                            new WaitCallback(
                                delegate(object required)
                                {
                                    EventoNegocioANG eventoANG = new EventoNegocioANG();
                                    eventoANG.instrumento = instrumento;
                                    eventoANG.mensagem = NegocioBase.montarPrecoUnitario(registroNegocios);
                                    EventQueueManager.Instance.SendEvent(eventoANG);
                                }
                            )
                        );
                    }

                    logger.InfoFormat("Instrumento[{0}] sequencial[{1}] precoUnitario[{2}] openCloseSettlFlag[{3}]",
                        instrumento, numSeq, precoAjuste, openCloseSettlFlag);

                    lock (dctNegocios[instrumento])
                    {
                        dctNegocios[instrumento] = registroNegocios;
                    }
                }
                /*else
                {
                    logger.InfoFormat("Descartando Instrumento[{0}] sequencial[{1}] precoAjuste[{2}] openCloseSettlFlag[{3}]",
                        instrumento, numSeq, precoAjuste, openCloseSettlFlag);
                }*/
            }

            return;
        }

        // Tratar mensagem de negocio
        public void tratarMensagemNegocio(string instrumento, string numSeq, GroupValue mdEntry, List<Dictionary<string, string>> streamerLNG, string msgType, bool isPuma, bool isBMF)
        {
            NegocioBase registroNegocios = dctNegocios[instrumento];
		
		    string mdUpdateAction = "0";
		    if ( mdEntry.IsDefined("MDUpdateAction"))
			    mdUpdateAction = mdEntry.GetString("MDUpdateAction");

            String mdEntryType = mdEntry.GetString("MDEntryType");

		    String data = "";
		    String hora = "";
		    try
		    {
	            data = mdEntry.GetString("MDEntryDate");
                String formatHora = (isPuma ? "{0,9:d9}" : "{0:d6}");
                hora = UmdfUtils.convertUTC2Local(data, String.Format(formatHora, mdEntry.GetInt("MDEntryTime")));
		    }
		    catch (Exception ex)
		    {
			    logger.Error("Falha na formatacao da data[" + mdEntry.GetInt("MDEntryDate") + 
					    "] e hora[" + mdEntry.GetInt("MDEntryTime") + "]: " + ex.Message);
		    }
			
		    String compradora = "0";
		    try
		    {
                if (mdEntry.IsDefined("MDEntryBuyer"))
                {
                    String compradoraFormatada = mdEntry.GetString("MDEntryBuyer").Replace("BM", "").Trim();
                    if (String.IsNullOrEmpty(compradoraFormatada))
                    {
                        compradoraFormatada = "0";
                    }
                    compradora = String.Format("{0:d8}", Int64.Parse(compradoraFormatada));
                }
		    }
		    catch (Exception ex)
		    {
			    logger.Error("Falha na formatacao da compradora[" + mdEntry.GetString("MDEntryBuyer") + "]: " + ex.Message, ex);
		    }

		    String vendedora = "0";
		    try
		    {
                if (mdEntry.IsDefined("MDEntrySeller"))
                {
                    String vendedoraFormatada = mdEntry.GetString("MDEntrySeller").Replace("BM", "").Trim();
                    if (String.IsNullOrEmpty(vendedoraFormatada))
                    {
                        vendedoraFormatada = "0";
                    }
                    vendedora = String.Format("{0:d8}", Int64.Parse(vendedoraFormatada));
                }
		    }
		    catch (Exception eex)
		    {
			    logger.Error("Falha na formatacao da vendedora[" + mdEntry.GetString("MDEntrySeller") + "]: " + eex.Message, eex);
		    }

		    Decimal precoTrade = Decimal.Zero;
		    try
		    {
			    precoTrade = mdEntry.GetBigDecimal("MDEntryPx");
		    }
		    catch (Exception ex)
		    {
			    logger.Error("Falha na formatacao do preco[" + mdEntry.GetBigDecimal("MDEntryPx") + "]: " + ex.Message, ex);
		    }

		    long quantidade = 0;
            if ( mdEntry.IsDefined("MDEntrySize") )
            {
                quantidade = mdEntry.GetInt("MDEntrySize");
            }

		    Decimal valorNegocio = Decimal.Zero;
            valorNegocio = precoTrade * quantidade;

            long qtdeNegociadaDia = 0;
            if (mdEntry.IsDefined("TradeVolume"))
            {
                qtdeNegociadaDia = mdEntry.GetLong("TradeVolume");
            }

            // Calcula preco medio e volume financeiro apenas para MEGA, pois no UMDF 1.6 e 2.0 já vem os campos SessionVWAPPrice(9) e TradeVolume(B)
            Decimal volume = Decimal.Zero;
            Decimal precoMedio = Decimal.Zero;

            if (msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_SNAPSHOT_SINGLE))
            {
                // Quando recebe o snapshot do umdf, e for retomada durante o dia, faz um calculo aproximado do precoMedio e volume
                precoMedio = (registroNegocios.Negocio.PrecoMinimo + registroNegocios.Negocio.PrecoMaximo) / 2;
                volume = qtdeNegociadaDia * precoMedio;
            }
            else
            {
                volume = registroNegocios.Negocio.VolumeTotal + valorNegocio;
                if (mdUpdateAction.Equals(ConstantesUMDF.UMDF_MD_UPDT_ACTION_NEW))
                    volume = registroNegocios.Negocio.VolumeTotal + valorNegocio;
                else
                    volume = registroNegocios.Negocio.VolumeTotal - valorNegocio;

                if (qtdeNegociadaDia != 0)
                    precoMedio = (volume * registroNegocios.CoeficienteMultiplicacao) / qtdeNegociadaDia;
            }

		    long numeroNegocio = 0;
		    String numeroNegocioFormatado = "0";
            if (mdEntry.IsDefined("TradeID"))
		    {
			    try
			    {
				    numeroNegocioFormatado = mdEntry.GetString("TradeID");
				    //numeroNegocioFormatado = numeroNegocioFormatado.Substring(0, numeroNegocioFormatado.Length - 1);
                    numeroNegocioFormatado = numeroNegocioFormatado.Remove(numeroNegocioFormatado.Length - 1);
				    numeroNegocio = Int64.Parse(numeroNegocioFormatado);
			    }
			    catch (Exception ex)
			    {
				    logger.Error("Falha na formatacao do numeroNegocio[" + 
						    mdEntry.GetString("TradeID") + "]: " + ex.Message, ex);
			    }
		    }
		
            Decimal variacao = Decimal.Zero;
            if (mdUpdateAction.Equals(ConstantesUMDF.UMDF_MD_UPDT_ACTION_NEW) &&
                (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_TRADE) || mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_INDEX_VALUE)))
            {
                try
                {
                    Decimal precoFechamento = Decimal.Zero;
                    if (mdEntry.IsDefined("NetChgPrevDay") && !isBMF)
                    {
                        Decimal diferenca = mdEntry.GetBigDecimal("NetChgPrevDay");
                        precoFechamento = precoTrade - diferenca;
                    }
                    else
                    {
                        precoFechamento = registroNegocios.Negocio.PrecoFechamento;
                    }

                    if (precoFechamento == Decimal.Zero)
                        precoFechamento = precoTrade;
                    variacao = precoTrade / precoFechamento;
                    variacao -= Decimal.One;
                    variacao *= 100;
                }
                catch (Exception ex)
                {
                    logger.Error("Falha na formatacao da variacao: " + ex.Message, ex);
                }
            }

            /* DEPRECATED
            if (mdUpdateAction.Equals(ConstantesUMDF.UMDF_MD_UPDT_ACTION_NEW) &&
                mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_INDEX_VALUE))
            {
                try
                {
                    variacao = mdEntry.GetBigDecimal("PercentageVar");
                }
                catch (Exception ex)
                {
                    logger.Error("Falha na formatacao da variacao: " + ex.Message, ex);
                }
            }
            */

            //if (!msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_SNAPSHOT_SINGLE))
		    //    registroNegocios.Negocio.EstadoInstrumento = ConstantesMDS.ESTADO_PAPEL_EM_NEGOCIACAO;

            //if (registroNegocios.FaseNegociacao != ConstantesMDS.FASE_NEGOCIACAO_CONGELAR_HORARIO_ANALISE_GRAFICA)
            //{
                registroNegocios.Negocio.Data = data;
                registroNegocios.Negocio.Hora = hora;
            //}

            registroNegocios.Negocio.Variacao = variacao;
            registroNegocios.Negocio.Compradora = compradora;
            registroNegocios.Negocio.Vendedora = vendedora;
            
            registroNegocios.Negocio.Preco = precoTrade;
            if (registroNegocios.Negocio.PrecoAbertura == 0)
                registroNegocios.Negocio.PrecoAbertura = precoTrade;
            if (registroNegocios.Negocio.PrecoFechamento == 0)
                registroNegocios.Negocio.PrecoFechamento = precoTrade;
            if (registroNegocios.Negocio.PrecoMinimo == 0)
                registroNegocios.Negocio.PrecoMinimo = precoTrade;
            if (registroNegocios.Negocio.PrecoMaximo == 0)
                registroNegocios.Negocio.PrecoMaximo = precoTrade;
            if (registroNegocios.Negocio.PrecoMedio == 0)
                registroNegocios.Negocio.PrecoMedio = precoTrade;

            if (!mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_INDEX_VALUE))
            {
                registroNegocios.Negocio.NumeroNegocio = numeroNegocioFormatado;
                registroNegocios.Negocio.QtdeNegocios = numeroNegocio;
                registroNegocios.Negocio.VolumeTotal = volume;
                registroNegocios.Negocio.Quantidade = quantidade;
                registroNegocios.Negocio.QtdeNegociadaDia = qtdeNegociadaDia;

                if (!isPuma)
                {
                    registroNegocios.Negocio.PrecoMedio = precoMedio;
                }
            }
            else
            {
                ContabilizaIndices(registroNegocios, instrumento);
                if (instrumento.Equals(ConstantesMDS.INDICE_IBOVESPA))
                {
                    ContabilizaIndices(dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TOTAL], ConstantesMDS.INDICE_IBOVESPA_TOTAL);
                    ContabilizaIndices(dctNegocios[ConstantesMDS.INDICE_IBOVESPA_VISTA], ConstantesMDS.INDICE_IBOVESPA_VISTA);
                    ContabilizaIndices(dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TERMO], ConstantesMDS.INDICE_IBOVESPA_TERMO);
                    ContabilizaIndices(dctNegocios[ConstantesMDS.INDICE_IBOVESPA_OPCOES], ConstantesMDS.INDICE_IBOVESPA_OPCOES);
                }
            }

            if (mdUpdateAction.Equals(ConstantesUMDF.UMDF_MD_UPDT_ACTION_NEW))
            {
                if (!msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_SNAPSHOT_SINGLE))
                {
                    NegocioBase.AdicionarLivroNegocios(registroNegocios, numItensLNG);

                    streamerLNG.Add(NegocioBase.montarItemLivroNegociosStreamer(ConstantesMDS.HTTP_LIVRO_NEGOCIOS_TIPO_ACAO_INCLUIR, 0,
                                                                            registroNegocios.LivroNegocios[0], registroNegocios.CasasDecimais));
                }
            }
            else
            {
                int posicao = NegocioBase.BuscarPosicaoNegocioCancelado(numeroNegocioFormatado, registroNegocios);

                if (posicao >= 0 && posicao < registroNegocios.LivroNegocios.Count)
                {
                    streamerLNG.Add(NegocioBase.montarItemLivroNegociosStreamer(ConstantesMDS.HTTP_LIVRO_NEGOCIOS_TIPO_ACAO_EXCLUIR, posicao,
                                                                        registroNegocios.LivroNegocios[posicao], registroNegocios.CasasDecimais));

                    NegocioBase.RemoverNegocioCancelado(posicao, registroNegocios);
                }

            }

            //lock (dctNegocios[instrumento])
            //{
            //    dctNegocios[instrumento] = registroNegocios;
            //}
            //enviaMensagemFila(queueNegocio, JsonConvert.SerializeObject(registroNegocios), syncQueueNegocio);
            if (bGerarEventoANG)
            {
                EventoNegocioANG eventoANG = new EventoNegocioANG();
                eventoANG.instrumento = instrumento;
                eventoANG.mensagem = NegocioBase.montarNegocioAnaliseGrafica(registroNegocios);
                EventQueueManager.Instance.SendEvent(eventoANG);
            }

            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks-lastAvisoAtraso);
            if ( ts.TotalMilliseconds > 30000)
            {
                lastAvisoAtraso = DateTime.Now.Ticks;
                DateTime dthr = DateTime.ParseExact(data + hora, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                ts = new TimeSpan(DateTime.Now.Ticks - dthr.Ticks);
                if (ts.TotalMilliseconds > 30000)
                {
                    string msg = "Atraso no sinal de negocio de [" + instrumento + "] horario da cotacao [" + hora + "] processado as [" + DateTime.Now.ToString("HH:mm:ss.fff") + "]";
                    logger.Warn(msg);

                    ThreadPool.QueueUserWorkItem(
                        new WaitCallback(
                            delegate(object required)
                            {
                                MDSUtils.EnviarEmail("Atraso na cotacao", msg);
                            }
                        )
                    );
                }
            }


            return;
        }

        private void enviaMensagemFila(ConcurrentQueue<string> fila, string mensagem, object syncObj)
        {
            bool sinaliza = fila.IsEmpty;

            fila.Enqueue(mensagem);

            if (sinaliza)
            {
                lock (syncObj)
                {
                    Monitor.Pulse(syncObj);
                }
            }
        }

        private void ContabilizaIndices(NegocioBase registroNegocios, string instrumento)
        {
            long qtdeNegociadaIndice = 0;
            long numNegociosIndice = 0;
            decimal volumeIndice = 0;
            int totalPapeis = 0;

            try
            {
                if (dctComposicaoIndice.ContainsKey(instrumento))
                {
                    foreach (string item in dctComposicaoIndice[instrumento])
                    {
                        if (item == null)
                            continue;
                        totalPapeis++;
                        if (!dctNegocios.ContainsKey(item))
                            continue;
                        if (dctNegocios[item].Negocio == null)
                            continue;
                        qtdeNegociadaIndice += dctNegocios[item].Negocio.QtdeNegociadaDia;
                        numNegociosIndice +=
                            (dctNegocios[item].Negocio.NumeroNegocio == null || dctNegocios[item].Negocio.NumeroNegocio.Equals("") ?
                            0 : Convert.ToInt64(dctNegocios[item].Negocio.NumeroNegocio));
                        volumeIndice += dctNegocios[item].Negocio.VolumeTotal;
                    }
                    registroNegocios.Negocio.QtdeNegociadaDia = qtdeNegociadaIndice;
                    registroNegocios.Negocio.NumeroNegocio = numNegociosIndice.ToString();
                    registroNegocios.Negocio.QtdeNegocios = numNegociosIndice;
                    registroNegocios.Negocio.VolumeTotal = volumeIndice;

                    logger.InfoFormat("Indice[{0}]: Somados {1} papeis totalizando QtdePapeis[{2}] NumNegocios[{3}] Volume[{4}]",
                        instrumento, totalPapeis, registroNegocios.Negocio.QtdeNegociadaDia, registroNegocios.Negocio.NumeroNegocio, registroNegocios.Negocio.VolumeTotal);
                }
            }
            catch (InvalidOperationException exInvOper)
            {
                logger.InfoFormat("Indice[{0}]: Falha em ContabilizaIndices(), aguardando receber todo o Cadastro Basico: {1}",
                    instrumento, exInvOper.Message);
            }
            catch (Exception ex)
            {
				logger.Error("Indice [" + instrumento + "]: Falha em ContabilizaIndices() " + ex.Message, ex);
            }
        }

        public string tratarMensagemMelhorOferta(string instrumento, string numSeq, GroupValue mdEntry)
        {
            NegocioBase registroNegocios = dctNegocios[instrumento];

	        string mdEntryType = mdEntry.GetString("MDEntryType");
		    Decimal melhorPreco =mdEntry.GetBigDecimal("MDEntryPx");
		    long quantidadeSemFormato = mdEntry.GetInt("MDEntrySize");
		
			if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_BID))
			{
				string compradoraPorExtenso = "0";
				if ( mdEntry.IsDefined("MDEntryBuyer") )
				{
					compradoraPorExtenso = mdEntry.GetString("MDEntryBuyer").Replace("BM", "").Trim();
				}

                registroNegocios.Negocio.MelhorPrecoCompra = melhorPreco;
                registroNegocios.Negocio.MelhorQuantidadeCompra = quantidadeSemFormato;
                registroNegocios.Negocio.Compradora = compradoraPorExtenso;

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Instrumento[" + instrumento +
                            "]: Melhor Compra Preco[" + melhorPreco +
                            "] Quantidade[" + quantidadeSemFormato +
                            "] Corretora[" + compradoraPorExtenso + "]");
                }
			}
			else
			{
				string vendedoraPorExtenso = "0";
				if ( mdEntry.IsDefined("MDEntrySeller") )
				{
					vendedoraPorExtenso = mdEntry.GetString("MDEntrySeller").Replace("BM", "").Trim();
				}

                registroNegocios.Negocio.MelhorPrecoVenda = melhorPreco;
                registroNegocios.Negocio.MelhorQuantidadeVenda = quantidadeSemFormato;
                registroNegocios.Negocio.Vendedora = vendedoraPorExtenso;

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Instrumento[" + instrumento +
                            "]: Melhor Venda Preco[" + melhorPreco +
                            "] Quantidade[" + quantidadeSemFormato +
                            "] Corretora[" + vendedoraPorExtenso + "]");
                }
            }

            lock (dctNegocios[instrumento])
            {
                dctNegocios[instrumento] = registroNegocios;
            }

		    return instrumento;
        }

        public void tratarMensagemMelhorOfertaPuma(string instrumento, string numSeq, GroupValue mdEntry)
        {
            // Despreza mensagens de MDUpdateAction = delete
            if (mdEntry.IsDefined("MDUpdateAction"))
            {
                if (mdEntry.GetString("MDUpdateAction").Equals(ConstantesUMDF.UMDF_MD_UPDT_ACTION_DELETE))
                    return;
            }

            NegocioBase registroNegocios = dctNegocios[instrumento];

            string mdEntryType = mdEntry.GetString("MDEntryType");

            Decimal melhorPreco = Decimal.Zero;
            if (mdEntry.IsDefined("MDEntryPx"))
            {
                melhorPreco = mdEntry.GetBigDecimal("MDEntryPx");
            }

            long quantidadeSemFormato = 0;
            if (mdEntry.IsDefined("MDEntrySize"))
                quantidadeSemFormato = mdEntry.GetInt("MDEntrySize");

            if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_BID))
            {
                registroNegocios.Negocio.MelhorPrecoCompra = melhorPreco;
                registroNegocios.Negocio.MelhorQuantidadeCompra = quantidadeSemFormato;

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Instrumento[" + instrumento +
                            "]: Melhor Compra Preco[" + melhorPreco +
                            "] Quantidade[" + quantidadeSemFormato + "]");
                }
            }
            else
            {
                registroNegocios.Negocio.MelhorPrecoVenda = melhorPreco;
                registroNegocios.Negocio.MelhorQuantidadeVenda = quantidadeSemFormato;

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Instrumento[" + instrumento +
                            "]: Melhor Venda Preco[" + melhorPreco +
                            "] Quantidade[" + quantidadeSemFormato + "]");
                }
            }

            //lock (dctNegocios[instrumento])
            //{
            //    dctNegocios[instrumento] = registroNegocios;
            //}

            return;
        }

        // Tratar mensagem de Estado de Grupo de Cotacao
        public void tratarMensagemEstadoGrupoCotacao(string instrumento, string numSeq, Message umdfMsg, GroupValue mdEntry, bool isPuma)
        {
            string grupoCotacao = umdfMsg.GetString("SecurityGroup");
            if (dctGrupoCotacao.ContainsKey(grupoCotacao))
            {
                if (dctGrupoCotacao[grupoCotacao].listaGrupoCotacao.Count == 0)
                    return;

                DateTime dataHoraMensagem = DateTime.MinValue;
                string dataMensagem = null;
                string horaMensagem = null;
                if (umdfMsg.IsDefined("SendingTime"))
                {
                    dataMensagem = umdfMsg.GetString("SendingTime").Substring(0, 8);
                    string horaUTC = umdfMsg.GetString("SendingTime").Substring(8, 6);
                    horaMensagem = UmdfUtils.convertUTC2Local(dataMensagem, horaUTC);
                    dataHoraMensagem = DateTime.ParseExact(dataMensagem + horaMensagem, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                }

                string condicao = umdfMsg.GetString("TradingSessionSubID");
                int estado;
                string faseNegociacao = null;

                if (condicao.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_RESERVED) ||
                    condicao.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_FINAL_CLOSING_CALL))
                    estado = ConstantesMDS.ESTADO_PAPEL_EM_LEILAO;

                else if (condicao.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_OPEN))
                    estado = ConstantesMDS.ESTADO_PAPEL_EM_NEGOCIACAO;

                else if (condicao.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_UNKNOWN))
                    estado = ConstantesMDS.ESTADO_PAPEL_INIBIDO;

                else if (condicao.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_FORBIDDEN))
                    estado = ConstantesMDS.ESTADO_PAPEL_NAO_NEGOCIADO;

                else if (condicao.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_HALT))
                    estado = ConstantesMDS.ESTADO_PAPEL_INIBIDO;
                else
                    estado = ConstantesMDS.ESTADO_PAPEL_NAO_NEGOCIADO;

                if (condicao.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_FINAL_CLOSING_CALL))
                    faseNegociacao = ConstantesMDS.FASE_NEGOCIACAO_CONGELAR_HORARIO_ANALISE_GRAFICA;

                // Se nao houve mudanca no estado do Grupo de Cotacao, ignora
                if (dctGrupoCotacao[grupoCotacao].estado == estado)
                    return;

                dctGrupoCotacao[grupoCotacao].estado = estado;

                logger.InfoFormat("GrupoCotacao[{0}]: Alteracao de estado - TradingSessionSubID[{1}] estado[{2}] Data/Hora[{3}]",
                    grupoCotacao, condicao, estado, dataHoraMensagem.ToString("dd/MM/yyyy HH:mm:ss"));

                foreach (string item in dctGrupoCotacao[grupoCotacao].listaGrupoCotacao)
                {
                    if (dctNegocios.ContainsKey(item))
                    {
                        lock (dctNegocios[item])
                        {
                            NegocioBase registroNegocio = dctNegocios[item];

                            if (dataHoraMensagem >= registroNegocio.Negocio.HorarioTeorico)
                            {
                                registroNegocio.Negocio.EstadoInstrumento = estado;

                                if (bGerarEventoANG)
                                {
                                    EventoNegocioANG eventoANG = new EventoNegocioANG();
                                    eventoANG.instrumento = instrumento;
                                    eventoANG.mensagem = NegocioBase.montarNegocioAnaliseGrafica(registroNegocio);
                                    EventQueueManager.Instance.SendEvent(eventoANG);
                                }

                                if (faseNegociacao != null)
                                    registroNegocio.FaseNegociacao = faseNegociacao;

                                if (estado == ConstantesMDS.ESTADO_PAPEL_EM_LEILAO &&
                                    (String.IsNullOrEmpty(registroNegocio.Negocio.Data) || !registroNegocio.Negocio.Data.Equals(dataMensagem)))
                                {
                                    registroNegocio.Negocio.Preco = 0;
                                    registroNegocio.Negocio.PrecoAbertura = 0;
                                    registroNegocio.Negocio.PrecoMedio = 0;
                                    registroNegocio.Negocio.PrecoMinimo = 0;
                                    registroNegocio.Negocio.PrecoMaximo = 0;
                                    registroNegocio.Negocio.Quantidade = 0;
                                    registroNegocio.Negocio.Variacao = 0;
                                    registroNegocio.Negocio.QtdeNegociadaDia = 0;
                                    registroNegocio.Negocio.Compradora = "0";
                                    registroNegocio.Negocio.Vendedora = "0";
                                    registroNegocio.Negocio.NumeroNegocio = "0";
                                    registroNegocio.Negocio.QtdeNegocios = 0;
                                    registroNegocio.Negocio.VolumeTotal = 0;
                                    registroNegocio.Negocio.PrecoLeilao = 0;
                                    registroNegocio.FaseNegociacao = ConstantesMDS.FASE_NEGOCIACAO_ATUALIZAR_ANALISE_GRAFICA;
                                    registroNegocio.LivroNegocios = new List<LNGDadosNegocio>();

                                    logger.InfoFormat("Instrumento[{0}]: Inicializando contadores!", item);
                                }

                                EventoHttpNegocio eventoNEG = new EventoHttpNegocio();
                                eventoNEG.instrumento = item;
                                eventoNEG.cabecalho = MDSUtils.montaCabecalhoStreamer(ConstantesMDS.TIPO_REQUISICAO_NEGOCIOS, registroNegocio.TipoBolsa, 0, item, registroNegocio.CasasDecimais, null);
                                eventoNEG.negocio = NegocioBase.montarNegocioStreamer(registroNegocio.Negocio, registroNegocio.CasasDecimais);
                                EventQueueManager.Instance.SendEvent(eventoNEG);

                                // Envia evento para o HomeBroker/CotacaoStreamer
                                EventoHBNegocio eventoHBNeg = new EventoHBNegocio();
                                eventoHBNeg.instrumento = item;
                                eventoHBNeg.mensagem = NegocioBase.montarNegocioHomeBroker(registroNegocio);
                                EventQueueManager.Instance.SendEvent(eventoHBNeg);

                                /*ThreadPool.QueueUserWorkItem(
                                    new WaitCallback(
                                        delegate(object required)
                                        {
                                            try
                                            {
                                                EventoHttpNegocio eventoNEG = new EventoHttpNegocio();
                                                eventoNEG.instrumento = item;
                                                eventoNEG.cabecalho = MDSUtils.montaCabecalhoStreamer(ConstantesMDS.TIPO_REQUISICAO_NEGOCIOS, registroNegocio.TipoBolsa, 0, item, registroNegocio.CasasDecimais, null);
                                                eventoNEG.negocio = NegocioBase.montarNegocioStreamer(registroNegocio.Negocio, registroNegocio.CasasDecimais);
                                                EventQueueManager.Instance.SendEvent(eventoNEG);

                                                // Envia evento para o HomeBroker/CotacaoStreamer
                                                EventoHBNegocio eventoHBNeg = new EventoHBNegocio();
                                                eventoHBNeg.instrumento = item;
                                                eventoHBNeg.mensagem = NegocioBase.montarNegocioHomeBroker(registroNegocio);
                                                EventQueueManager.Instance.SendEvent(eventoHBNeg);
                                            }
                                            catch (Exception ex)
                                            {
                                                logger.Error("delegate(): " + ex.Message, ex);
                                            }
                                        }
                                    )
                                ); */

                                logger.InfoFormat("Instrumento[{0}] GrupoCotacao[{1}]: Alteracao para EstadoInstrumento[{2}]",
                                    item, grupoCotacao, estado);
                            }

                            /*if (estado == ConstantesMDS.ESTADO_PAPEL_EM_NEGOCIACAO && 
                                registroNegocio.Negocio.QtdeNegocios > 0 &&
                                registroNegocio.Negocio.Data.Equals(DateTime.Now.ToString("yyyyMMdd")))
                            {
                                logger.InfoFormat("Instrumento[{0}]: Suspender Analise Grafica QtdeNegocios[{1}]", item, registroNegocio.Negocio.QtdeNegocios);
                                registroNegocio.FaseNegociacao = ConstantesMDS.FASE_NEGOCIACAO_SUSPENDER_ANALISE_GRAFICA;
                            }*/

                            // Gera e envia mensagem de Serie Historica
                            if (condicao.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_HALT) &&
                                registroNegocio.Negocio.QtdeNegocios > 0 &&
                                registroNegocio.Negocio.Data.Equals(DateTime.Now.ToString("yyyyMMdd")) &&
                                registroNegocio.FaseNegociacao != ConstantesMDS.FASE_NEGOCIACAO_ENVIADO_SERIE_HISTORICA) // &&
                                //registroNegocio.FaseNegociacao != ConstantesMDS.FASE_NEGOCIACAO_SUSPENDER_ANALISE_GRAFICA)
                            {
                                registroNegocio.FaseNegociacao = ConstantesMDS.FASE_NEGOCIACAO_ENVIADO_SERIE_HISTORICA;
                                registroNegocio.Negocio.PrecoFechamento = registroNegocio.Negocio.Preco;

                                logger.InfoFormat("Instrumento[{0}]: Define precoAtual como precoFechamento[{1}]",
                                    item, registroNegocio.Negocio.PrecoFechamento);

                                logger.InfoFormat("Instrumento[{0}]: SerieHistorica QtdNegocios[{1}]",
                                    item, registroNegocio.Negocio.QtdeNegocios);

                                if (bGerarEventoANG)
                                {
                                    EventoNegocioANG eventoANG = new EventoNegocioANG();
                                    eventoANG.instrumento = registroNegocio.Instrumento;
                                    eventoANG.mensagem = NegocioBase.montarSerieHistorica(registroNegocio);
                                    EventQueueManager.Instance.SendEvent(eventoANG);
                                }
                            }
                            verificaSuspensaoAnaliseGrafica(registroNegocio);
                        }
                    }
                }
            }
        }

        // Tratar mensagem de Estado do Instrumento
        public void tratarMensagemEstadoInstrumento(string instrumento, string numSeq, Message umdfMsg, GroupValue mdEntry, string msgType, bool isPuma)
        {
            string tradingStatus = ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_UNKNOWN;
            int estado = ConstantesMDS.ESTADO_PAPEL_INIBIDO;

            NegocioBase registroNegocios = dctNegocios[instrumento];

            if ((mdEntry != null && mdEntry.IsDefined("TradingSessionSubID")) &&
                (!mdEntry.IsDefined("TradSesOpenTime") || msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_SNAPSHOT_SINGLE)))
            {
                tradingStatus = mdEntry.GetString("TradingSessionSubID");
            }
            else if (mdEntry != null && mdEntry.IsDefined("SecurityTradingStatus"))
            {
                tradingStatus = mdEntry.GetString("SecurityTradingStatus");
            }
            else if (umdfMsg.IsDefined("SecurityTradingStatus"))
            {
                tradingStatus = umdfMsg.GetString("SecurityTradingStatus");
            }

            if (tradingStatus.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_RESERVED) ||
                tradingStatus.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_FINAL_CLOSING_CALL))
            {
                estado = ConstantesMDS.ESTADO_PAPEL_EM_LEILAO;

                if (mdEntry != null && mdEntry.IsDefined("TradSesOpenTime"))
                {
                    string data = mdEntry.GetString("TradSesOpenTime").Substring(0, 8);
                    string horaProrrogacaoUTC = String.Format("{0,9:d9}", mdEntry.GetString("TradSesOpenTime").Substring(8, 9));
                    string horaProrrogacao = UmdfUtils.convertUTC2Local(data, horaProrrogacaoUTC);

                    registroNegocios.Negocio.HorarioTeorico = DateTime.ParseExact(data + horaProrrogacao, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    //logger.InfoFormat("Instrumento[{0}]: HorarioProrrogacaoLeilao[{1}]", instrumento, registroNegocios.Negocio.HorarioTeorico);
                }
                else
                {
                    if (umdfMsg.IsDefined("TradSesOpenTime"))
                    {
                        string data = umdfMsg.GetString("TradSesOpenTime").Substring(0, 8);
                        string horaProrrogacaoUTC = String.Format("{0,9:d9}", umdfMsg.GetString("TradSesOpenTime").Substring(8, 9));
                        string horaProrrogacao = UmdfUtils.convertUTC2Local(data, horaProrrogacaoUTC);

                        registroNegocios.Negocio.HorarioTeorico = DateTime.ParseExact(data + horaProrrogacao, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                        //logger.InfoFormat("Instrumento[{0}]: HorarioProrrogacaoLeilao[{1}]", instrumento, registroNegocios.Negocio.HorarioTeorico);
                    }
                }
            }

            else if (tradingStatus.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_OPEN))
            {
                estado = ConstantesMDS.ESTADO_PAPEL_EM_NEGOCIACAO;
                registroNegocios.Negocio.HorarioTeorico = DateTime.MinValue;
            }

            else if (tradingStatus.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_UNKNOWN))
                estado = ConstantesMDS.ESTADO_PAPEL_INIBIDO;

            else if (tradingStatus.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_FORBIDDEN))
                estado = ConstantesMDS.ESTADO_PAPEL_NAO_NEGOCIADO;

            else if (tradingStatus.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_HALT))
                estado = ConstantesMDS.ESTADO_PAPEL_INIBIDO;
            else
                estado = ConstantesMDS.ESTADO_PAPEL_NAO_NEGOCIADO;

            /*
       		if (mdEntry != null && mdEntry.IsDefined("MDEntryDate"))
		    {
			    string data = mdEntry.GetString("MDEntryDate");
                String formatHora = (isPuma ? "{0,9:d9}" : "{0:d6}");
                string horaUTC = String.Format(formatHora, mdEntry.GetInt("MDEntryTime")).Substring(0, 6);
			    string hora = UmdfUtils.convertUTC2Local(data, horaUTC);

                if (String.IsNullOrEmpty(registroNegocios.Negocio.Data) || 
                    registroNegocios.Negocio.Data.Equals("00000000") ||
				    estado == ConstantesMDS.ESTADO_PAPEL_EM_LEILAO ||
				    estado == ConstantesMDS.ESTADO_PAPEL_EM_NEGOCIACAO)
			    {
                    registroNegocios.Negocio.Data = data;
                    registroNegocios.Negocio.Hora = hora;
			    }
		    }
            */

            if (registroNegocios.Negocio.HorarioTeorico == DateTime.MinValue)
                logger.InfoFormat("Instrumento[{0}]: Mudanca para EstadoInstrumento[{1}] tradingStatus[{2}]",
                    instrumento, estado, tradingStatus);
            else
                logger.InfoFormat("Instrumento[{0}]: Mudanca para EstadoInstrumento[{1}] tradingStatus[{2}] HorarioProrrogacaoLeilao[{3}]",
                    instrumento, estado, tradingStatus, registroNegocios.Negocio.HorarioTeorico.ToString("dd/MM/yyyy HH:mm:ss"));

            registroNegocios.Negocio.EstadoInstrumento = estado;
            registroNegocios.Negocio.EstadoPapel2 = tradingStatus;

            if (bGerarEventoANG)
            {
                verificaSuspensaoAnaliseGrafica(registroNegocios);

                EventoNegocioANG eventoANG = new EventoNegocioANG();
                eventoANG.instrumento = instrumento;
                eventoANG.mensagem = NegocioBase.montarNegocioAnaliseGrafica(registroNegocios);
                EventQueueManager.Instance.SendEvent(eventoANG);
            }

            if (tradingStatus.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_FINAL_CLOSING_CALL))
                registroNegocios.FaseNegociacao = ConstantesMDS.FASE_NEGOCIACAO_CONGELAR_HORARIO_ANALISE_GRAFICA;

		    // Se leilao, zera quantidade teorica, pois a mudanca de estado nao deve ter quantidade teorica preenchida
		    //if (estado == ConstantesMDS.ESTADO_PAPEL_EM_LEILAO)
            //    registroNegocios.Negocio.Quantidade = 0;

            lock (dctNegocios[instrumento])
            {
                dctNegocios[instrumento] = registroNegocios;
            }
        }

        public EventoHttpNegocio SnapshotStreamerNegocio(string instrumento)
        {
            NegocioBase registroNegocio = null;

            if (dctNegocios.ContainsKey(instrumento))
                registroNegocio = dctNegocios[instrumento];
            else
                return null;

            EventoHttpNegocio eventoNEG = new EventoHttpNegocio();
            eventoNEG.instrumento = instrumento;
            eventoNEG.negocio = NegocioBase.montarNegocioStreamer(registroNegocio.Negocio, registroNegocio.CasasDecimais);

            return eventoNEG;
        }

        public EventoHttpLivroNegocios SnapshotStreamerLivroNegocios(string instrumento)
        {
            EventoHttpLivroNegocios retorno = new EventoHttpLivroNegocios();
            retorno.instrumento = instrumento;

            lock (dctNegocios[instrumento])
            {
                retorno.negocio = NegocioBase.montaLivroNegociosCompleto(dctNegocios[instrumento].LivroNegocios, dctNegocios[instrumento].CasasDecimais);
            }

            return retorno;
        }

        public List<string> SnapshotHB()
        {
            List<string> retorno = new List<string>();
            NegocioBase[] arrNegocios = null;


            // ATP: em lugar de um lock para obter uma copia do dicionario de negocios
            // vamos efetuar retentativas caso haja alteracao durante a copia
            // pode atrasar a obtencao da copia, mas libera as threads de processamento
            // digamos que essa é uma "solucao emergencial de campo"
            bool bSnapshot = false;
            while( !bSnapshot )
            {
                try
                {
                    arrNegocios = dctNegocios.Values.ToArray<NegocioBase>();
                    bSnapshot = true;
                }
                catch (Exception ex)
                {
                    logger.Error("Erro ao obter snapshot do HB, retentando [" + ex.Message + "]");
                    Thread.Sleep(500);
                }
            }


            if (arrNegocios != null && arrNegocios.Length > 0)
            {
                foreach (NegocioBase registroNegocio in arrNegocios)
                {
                    if (registroNegocio == null)
                    {
                        logger.Error("Xiii");
                        continue;
                    }

                    if (registroNegocio.Negocio == null)
                    {
                        logger.Error("Vixe [" + registroNegocio.Instrumento + "]");
                        continue;
                    }

                    retorno.Add(NegocioBase.montarNegocioHomeBroker(registroNegocio));
                    string lng = NegocioBase.montarLivroNegociosHomeBroker(registroNegocio);
                    if ( !String.IsNullOrEmpty(lng) )
                        retorno.Add(lng);
                }
            }
            return retorno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instrumento"></param>
        /// <returns></returns>
        public bool IsNegociacao(string instrumento)
        {
            if (dctNegocios.ContainsKey(instrumento))
            {
                if (dctNegocios[instrumento].Negocio.EstadoInstrumento == ConstantesMDS.ESTADO_PAPEL_EM_NEGOCIACAO)
                    return true;
            }
            return false;
        }

        private void verificaSuspensaoAnaliseGrafica(NegocioBase registroNegocios)
        {
            if (!registroNegocios.FaseNegociacao.Equals(ConstantesMDS.FASE_NEGOCIACAO_SUSPENDER_ANALISE_GRAFICA))
            {
                bool suspenderAnaliseGrafica = false;
                if (registroNegocios.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_VISTA))
                {
                    if (registroNegocios.Negocio.Data.Equals(DateTime.Now.ToString("yyyyMMdd")) &&
                        Convert.ToInt32(registroNegocios.Negocio.Hora) >= Convert.ToInt32(horarioSuspensaoAnaliseGraficaVista))
                        suspenderAnaliseGrafica = true;

                }
                else if (registroNegocios.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_TERMO))
                {
                    if (registroNegocios.Negocio.Data.Equals(DateTime.Now.ToString("yyyyMMdd")) &&
                        Convert.ToInt32(registroNegocios.Negocio.Hora) >= Convert.ToInt32(horarioSuspensaoAnaliseGraficaTermo))
                        suspenderAnaliseGrafica = true;

                }
                else if (registroNegocios.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_OPCOES))
                {
                    if (registroNegocios.Negocio.Data.Equals(DateTime.Now.ToString("yyyyMMdd")) &&
                        Convert.ToInt32(registroNegocios.Negocio.Hora) >= Convert.ToInt32(horarioSuspensaoAnaliseGraficaOpcoes))
                        suspenderAnaliseGrafica = true;

                }
                else if (registroNegocios.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_EXERCICIO_DE_OPCOES))
                {
                    if (registroNegocios.Negocio.Data.Equals(DateTime.Now.ToString("yyyyMMdd")) &&
                        Convert.ToInt32(registroNegocios.Negocio.Hora) >= Convert.ToInt32(horarioSuspensaoAnaliseGraficaExerOpcoes))
                        suspenderAnaliseGrafica = true;
                }

                if (suspenderAnaliseGrafica)
                {
                    logger.InfoFormat("Instrumento[{0}]: Suspender Analise Grafica QtdeNegocios[{1}]", 
                        registroNegocios.Instrumento, registroNegocios.Negocio.QtdeNegocios);
                    registroNegocios.FaseNegociacao = ConstantesMDS.FASE_NEGOCIACAO_SUSPENDER_ANALISE_GRAFICA;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void monitorCacheHBProc()
        {
            long lastRun = 0;
            long intervalo = 2000;

            if (ConfigurationManager.AppSettings["IntervaloVerificacaoNaoEnviados"] != null)
            {
                intervalo = Convert.ToInt64(ConfigurationManager.AppSettings["IntervaloVerificacaoNaoEnviados"].ToString().Trim());
                if (intervalo < 2000)
                    intervalo = 2000;
            }

            while (bKeepRunning)
            {
                try
                {
                    TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - lastRun);
                    if (ts.TotalMilliseconds > intervalo)
                    {
                        logger.Info("Obtendo lista dos instrumentos pendentes a enviar NEG ao HB");

                        lastRun = DateTime.UtcNow.Ticks;
                        List<string> lstNotSent = timestampControl.ObterNEGNaoEnviadosHB(intervalo);

                        if (lstNotSent.Count > 0)
                        {

                            logger.Info("Lista com " + lstNotSent.Count + " instrumentos com sinal HB pendente. Enviando...");

                            foreach (string instrumento in lstNotSent)
                            {
                                enviaSinalHB(instrumento);
                            }

                            logger.Info("Enviado NEG de " + lstNotSent.Count + " instrumentos ao HB. Done.");
                        }
                        else
                        {
                            logger.Info("Nao ha sinal de NEG pendente de envio ao HB");
                        }


                        logger.Info("Obtendo lista dos instrumentos pendentes a enviar Livro de Negocios ao HB");

                        lstNotSent = timestampControl.ObterLNGNaoEnviadosHB(intervalo);

                        if (lstNotSent.Count > 0)
                        {

                            logger.Info("Lista com " + lstNotSent.Count + " instrumentos com Livro de Negocios HB pendente. Enviando...");

                            foreach (string instrumento in lstNotSent)
                            {
                                enviaSinalLivroNegociosHB(instrumento);
                            }

                            logger.Info("Enviado Livro de Negocios de " + lstNotSent.Count + " instrumentos ao HB. Done.");
                        }
                        else
                        {
                            logger.Info("Nao ha sinal de Livro de Negocios pendente de envio ao HB");
                        }

                    }
                }
                catch (Exception ex)
                {
                    logger.Error("monitorCacheHBProc():" + ex.Message, ex);
                }

                Thread.Sleep(250);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instrumento"></param>
        private void enviaSinalHB(string instrumento)
        {
            try
            {
                NegocioBase registroNegocio = dctNegocios[instrumento];


                // Envia evento para o HomeBroker/CotacaoStreamer
                EventoHBNegocio eventoHBNeg = new EventoHBNegocio();
                eventoHBNeg.instrumento = instrumento;
                eventoHBNeg.mensagem = NegocioBase.montarNegocioHomeBroker(registroNegocio);
                EventQueueManager.Instance.SendEvent(eventoHBNeg);
            }
            catch (Exception ex)
            {
                logger.Info("enviaSinalHB(): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instrumento"></param>
        private void enviaSinalLivroNegociosHB(string instrumento)
        {
            try
            {
                NegocioBase registroNegocio = dctNegocios[instrumento];


                // Envia evento para o HomeBroker/CotacaoStreamer
                EventoHBNegocio eventoHBNeg = new EventoHBNegocio();
                eventoHBNeg.instrumento = instrumento;
                eventoHBNeg.mensagem = NegocioBase.montarLivroNegociosHomeBroker(registroNegocio);
                EventQueueManager.Instance.SendEvent(eventoHBNeg);
            }
            catch (Exception ex)
            {
                logger.Info("enviaSinalHB(): " + ex.Message, ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void monitorCacheStreamerProc()
        {
            long lastRun = 0;
            long intervalo = 2000;
            if (ConfigurationManager.AppSettings["IntervaloVerificacaoNaoEnviados"] != null)
            {
                intervalo = Convert.ToInt64(ConfigurationManager.AppSettings["IntervaloVerificacaoNaoEnviados"].ToString().Trim());
                if (intervalo < 2000)
                    intervalo = 2000;
            }

            while (bKeepRunning)
            {
                try
                {
                    TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - lastRun);
                    if (ts.TotalMilliseconds > intervalo)
                    {
                        logger.Info("Obtendo lista dos instrumentos pendentes a enviar NEG ao Streamer");

                        lastRun = DateTime.UtcNow.Ticks;
                        List<string> lstNotSent = timestampControl.ObterNEGNaoEnviadosStreamer(intervalo);

                        if (lstNotSent.Count > 0)
                        {
                            logger.Info("Lista com " + lstNotSent.Count + " instrumentos com sinal Streamer pendente. Enviando...");

                            foreach (string instrumento in lstNotSent)
                            {
                                enviaSinalStreamer(instrumento);
                            }

                            logger.Info("Enviado NEG de " + lstNotSent.Count + " instrumentos ao Streamer. Done.");
                        }
                        else
                        {
                            logger.Info("Nao ha sinal de NEG pendente de envio ao streamer");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("monitorCacheStreamerProc():" + ex.Message, ex);
                }

                Thread.Sleep(250);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instrumento"></param>
        private void enviaSinalStreamer(string instrumento)
        {
            try
            {
                NegocioBase registroNegocio = dctNegocios[instrumento];

                EventoHttpNegocio eventoNEG = new EventoHttpNegocio();
                eventoNEG.instrumento = instrumento;
                eventoNEG.cabecalho = MDSUtils.montaCabecalhoStreamer(ConstantesMDS.TIPO_REQUISICAO_NEGOCIOS, registroNegocio.TipoBolsa, 0, instrumento, registroNegocio.CasasDecimais, null);
                eventoNEG.negocio = NegocioBase.montarNegocioStreamer(registroNegocio.Negocio, registroNegocio.CasasDecimais);

                EventQueueManager.Instance.SendEvent(eventoNEG);
            }
            catch (Exception ex)
            {
                logger.Info("enviaSinalStreamer(): " + ex.Message, ex);
            }
        }

        private void monitorCacheAnaliseGraficaProc()
        {
            long lastLogTicks = 0;
            while (bKeepRunning)
            {
                try
                {
                    string mensagem;
                    if (!queueNegocio.TryDequeue(out mensagem))
                    {
                        lock (syncQueueNegocio)
                        {
                            Monitor.Wait(syncQueueNegocio, 50);
                        }
                        continue;
                    }

                    if (MDSUtils.shouldLog(lastLogTicks))
                    {
                        lastLogTicks = DateTime.UtcNow.Ticks;
                        logger.Info("Tamanho da fila AnaliseGrafica: " + this.queueNegocio.Count);
                    }

                    NegocioBase negocio = JsonConvert.DeserializeObject<NegocioBase>(mensagem);

                    EventoNegocioANG eventoANG = new EventoNegocioANG();
                    eventoANG.instrumento = negocio.Instrumento;
                    eventoANG.mensagem = NegocioBase.montarNegocioAnaliseGrafica(negocio);
                    EventQueueManager.Instance.SendEvent(eventoANG);

                }
                catch (Exception ex)
                {
                    logger.Error("monitorCacheAnaliseGraficaProc():" + ex.Message, ex);
                }
            }
        }

        private void monitorCacheSerieHistoricaProc()
        {
            long lastLogTicks = 0;
            while (bKeepRunning)
            {
                try
                {
                    string mensagem;
                    if (!queueSerieHistorica.TryDequeue(out mensagem))
                    {
                        lock (syncQueueSerieHistorica)
                        {
                            Monitor.Wait(syncQueueSerieHistorica, 50);
                        }
                        continue;
                    }

                    if (MDSUtils.shouldLog(lastLogTicks))
                    {
                        lastLogTicks = DateTime.UtcNow.Ticks;
                        logger.Info("Tamanho da fila SerieHistorica: " + this.queueSerieHistorica.Count);
                    }

                    NegocioBase negocio = JsonConvert.DeserializeObject<NegocioBase>(mensagem);

                    EventoNegocioANG eventoANG = new EventoNegocioANG();
                    eventoANG.instrumento = negocio.Instrumento;
                    eventoANG.mensagem = NegocioBase.montarSerieHistorica(negocio);
                    EventQueueManager.Instance.SendEvent(eventoANG);
                }
                catch (Exception ex)
                {
                    logger.Error("monitorCacheSerieHistoricaProc():" + ex.Message, ex);
                }
            }
        }
        #endregion //UMDF Message Handling

        /// <summary>
        /// Retorna o numero de casas decimais de um instrumento
        /// </summary>
        /// <param name="instrumento"></param>
        /// <returns></returns>
        public int RetornaCasasDecimais(string instrumento)
        {
            int casasDecimais = 2;

            if (dctNegocios.ContainsKey(instrumento))
            {
                casasDecimais = dctNegocios[instrumento].CasasDecimais;
            }

            return casasDecimais;
        }

        #region FIXConflated

        protected override void trataMensagemFIX(EventoFIX evento )
        {
            try
            {
                QuickFix.FIX44.Message message = evento.Message;
                String msgType = message.Header.GetString(QuickFix.Fields.Tags.MsgType);
                String instrumento = "";
                String msgID = message.Header.GetInt(QuickFix.Fields.Tags.MsgSeqNum).ToString();
                String channelId = evento.ChannelID;
                //bool isPuma = dicCanais[channelId].channelConfig.IsPuma;
                //bool isBMF = (dicCanais[channelId].channelConfig.Segment.ToUpper().Equals(ConstantesMDS.CHANNEL_UMDF_SEGMENT_BMF) ? true : false);
                //bool isPuma20 = dicCanais[channelId].channelConfig.IsPuma20;
                bool isPuma = true;
                bool isBMF = false;

                int marketDepth = ConstantesUMDF.UMDF_MARKETDEPTH_MARKET_BY_ORDER;
                bool sendToStreamer = true;

                if (logger.IsDebugEnabled)
                {
                    logger.DebugFormat("channelID ....: {0} (isPuma={1})", channelId, isPuma.ToString());
                    logger.DebugFormat("msgID  .......: {0}", msgID);
                    logger.DebugFormat("Message ......: {0}", message.ToString());
                }

                List<Dictionary<string, string>> streamerLNG = new List<Dictionary<string, string>>();

                // Tratar mensagem de melhor oferta
                if (msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_MELHOR_OFERTA) ||
                     msgType.Equals(QuickFix.FIX44.MarketDataIncrementalRefresh.MsgType) ||
                     msgType.Equals(QuickFix.FIX44.MarketDataSnapshotFullRefresh.MsgType))
                {
                    if (message.GroupCount(QuickFix.Fields.Tags.NoMDEntries) > 0)
                    {
                        QuickFix.Group  mdEntry = message.GetGroup(1,QuickFix.Fields.Tags.NoMDEntries);
                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug(FIXUtils.writeGroup(mdEntry));
                        }

                        String securityID = "";

                        if (message.IsSetField(QuickFix.Fields.Tags.SecurityID))
                            securityID = message.GetString(QuickFix.Fields.Tags.SecurityID);
                        else
                        {
                            if (mdEntry.IsSetField(QuickFix.Fields.Tags.SecurityID))
                                securityID = mdEntry.GetString(QuickFix.Fields.Tags.SecurityID);
                        }

                        if (mdEntry.IsSetField(QuickFix.Fields.Tags.Symbol))
                            instrumento = mdEntry.GetString(QuickFix.Fields.Tags.Symbol);
                        else
                        {
                            if (message.IsSetField(QuickFix.Fields.Tags.Symbol))
                                instrumento = message.GetString(QuickFix.Fields.Tags.Symbol);
                            else
                            {
                                if (String.IsNullOrEmpty(securityID) || !dicSecurityID.TryGetValue(securityID, out instrumento))
                                {
                                    logger.ErrorFormat("SecurityID[{0}] Nao pode resolver instrumento/securityID", securityID);
                                    return;
                                }
                            }
                        }


                        if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntryPositionNo) && marketDepth != ConstantesUMDF.UMDF_MARKETDEPTH_TOP_OF_BOOK)
                        {
                            if (mdEntry.GetInt(QuickFix.Fields.Tags.MDEntryPositionNo) != 0)
                                return;
                        }

                        if (!dctNegocios.ContainsKey(instrumento))
                        {
                            dctNegocios.Add(instrumento, new NegocioBase(instrumento));
                            if (instrumento.Equals(ConstantesMDS.INDICE_IBOVESPA))
                            {
                                if (!dctNegocios.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_TOTAL))
                                    dctNegocios.Add(ConstantesMDS.INDICE_IBOVESPA_TOTAL, new NegocioBase(instrumento));
                                if (!dctNegocios.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_VISTA))
                                    dctNegocios.Add(ConstantesMDS.INDICE_IBOVESPA_VISTA, new NegocioBase(instrumento));
                                if (!dctNegocios.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_OPCOES))
                                    dctNegocios.Add(ConstantesMDS.INDICE_IBOVESPA_OPCOES, new NegocioBase(instrumento));
                                if (!dctNegocios.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_TERMO))
                                    dctNegocios.Add(ConstantesMDS.INDICE_IBOVESPA_TERMO, new NegocioBase(instrumento));
                            }
                        }

                        switch (msgType)
                        {
                            case QuickFix.FIX44.MarketDataSnapshotFullRefresh.MsgType:
                            case QuickFix.FIX44.MarketDataIncrementalRefresh.MsgType:
                                instrumento = processaMensagemMarketDataFIX(instrumento, msgID, message, streamerLNG, msgType, marketDepth, isPuma, isBMF, ref sendToStreamer);
                                break;
                            case ConstantesUMDF.FAST_MSGTYPE_MELHOR_OFERTA:
                                tratarMensagemMelhorOfertaFix(instrumento, msgID, mdEntry);
                                break;
                            default:
                                logger.Error("MessageType nao esperado [" + msgType + "]");
                                return;
                        }
                    }
                }

                else if (msgType.Equals(QuickFix.FIX44.SecurityList.MsgType))
                {
                    QuickFix.Group relatedSym = message.GetGroup(1, QuickFix.Fields.Tags.NoRelatedSym);

                    instrumento = processaMensagemSecurityListFIX(msgID, message);

                    lock (dctNegocios)
                    {
                        if (!dctNegocios.ContainsKey(instrumento))
                        {
                            dctNegocios.Add(instrumento, new NegocioBase(instrumento));
                            if (instrumento.Equals(ConstantesMDS.INDICE_IBOVESPA))
                            {
                                if (!dctNegocios.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_TOTAL))
                                    dctNegocios.Add(ConstantesMDS.INDICE_IBOVESPA_TOTAL, new NegocioBase(instrumento));
                                if (!dctNegocios.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_VISTA))
                                    dctNegocios.Add(ConstantesMDS.INDICE_IBOVESPA_VISTA, new NegocioBase(instrumento));
                                if (!dctNegocios.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_OPCOES))
                                    dctNegocios.Add(ConstantesMDS.INDICE_IBOVESPA_OPCOES, new NegocioBase(instrumento));
                                if (!dctNegocios.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_TERMO))
                                    dctNegocios.Add(ConstantesMDS.INDICE_IBOVESPA_TERMO, new NegocioBase(instrumento));
                            }

                        }
                    }

                    string tipoBolsa = ConstantesMDS.DESCRICAO_DE_BOLSA_BOVESPA;
                    string securityType = relatedSym.GetString(QuickFix.Fields.Tags.SecurityType);

                    switch (securityType)
                    {
                        case "FUT":
                        case "SPOT":
                        case "SOPT":
                        case "FOPT":
                        case "DTERM":
                            tipoBolsa = ConstantesMDS.DESCRICAO_DE_BOLSA_BMF;
                            break;
                    }

                    atualizaCadastroBasicoFIX(instrumento, message, tipoBolsa);

                    atualizaGrupoCotacao(instrumento);

                    dctNegocios[instrumento].TipoBolsa = tipoBolsa;
                }

                // Tratar mensagem de alteracao do estado do grupo de cotacao
                else if (msgType.Equals(QuickFix.FIX44.SecurityStatus.MsgType) && message.IsSetField(QuickFix.Fields.Tags.SecurityGroup))
                {
                    logger.InfoFormat("GrupoCotacao[{0}]: Trata estado dos papeis", message.GetString(QuickFix.Fields.Tags.SecurityGroup));
                    tratarMensagemEstadoGrupoCotacaoFIX(instrumento, msgID, message, null, isPuma);
                }

                else if (msgType.Equals(QuickFix.FIX44.SecurityStatus.MsgType) && message.IsSetField(QuickFix.Fields.Tags.SecurityID))
                {
                    String securityID = message.GetString(QuickFix.Fields.Tags.SecurityID);

                    if (String.IsNullOrEmpty(securityID) || !dicSecurityID.TryGetValue(securityID, out instrumento))
                    {
                        logger.ErrorFormat("SecurityID[{0}] Nao pode resolver instrumento/securityID", securityID);
                        return;
                    }

                    tratarMensagemEstadoInstrumentoFIX(instrumento, msgID, message, null, msgType, isPuma);
                    /*
                    bmfNegociosContabilizacao.tratarMensagemFaseNegociacao(
                            instrumento, msgID, umdfMessage);
		
                    bmfNegociosContabilizacao.tratarMensagemEstadoInstrumento(
                            instrumento, msgID, umdfMessage);
                    */
                }


                if (!String.IsNullOrEmpty(instrumento))
                {
                    NegocioBase registroNegocio;
                    registroNegocio = dctNegocios[instrumento];

                    ThreadPool.QueueUserWorkItem(
                        new WaitCallback(
                            delegate(object required)
                            {
                                try
                                {
                                    if (sendToStreamer && timestampControl.ShouldSendNEGStreamer(instrumento))
                                    {
                                        EventoHttpNegocio eventoNEG = new EventoHttpNegocio();
                                        eventoNEG.instrumento = instrumento;
                                        eventoNEG.cabecalho = MDSUtils.montaCabecalhoStreamer(ConstantesMDS.TIPO_REQUISICAO_NEGOCIOS, registroNegocio.TipoBolsa, 0, instrumento, registroNegocio.CasasDecimais, null);
                                        eventoNEG.negocio = NegocioBase.montarNegocioStreamer(registroNegocio.Negocio, registroNegocio.CasasDecimais);

                                        EventQueueManager.Instance.SendEvent(eventoNEG);
                                    }

                                    // Envia sinal de Livro de Negocios
                                    if (streamerLNG.Count > 0)
                                    {
                                        EventoHttpLivroNegocios eventoLNG = new EventoHttpLivroNegocios();
                                        eventoLNG.instrumento = instrumento;
                                        eventoLNG.cabecalho = MDSUtils.montaCabecalhoStreamer(ConstantesMDS.TIPO_REQUISICAO_LIVRO_NEGOCIOS, registroNegocio.TipoBolsa, ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_ALTERAR, instrumento, registroNegocio.CasasDecimais, null);
                                        eventoLNG.negocio = streamerLNG;

                                        EventQueueManager.Instance.SendEvent(eventoLNG);
                                    }

                                    if (timestampControl.ShouldSendNEGHB(instrumento))
                                    {
                                        // Envia evento para o HomeBroker/CotacaoStreamer
                                        EventoHBNegocio eventoHBNeg = new EventoHBNegocio();
                                        eventoHBNeg.instrumento = instrumento;
                                        eventoHBNeg.mensagem = NegocioBase.montarNegocioHomeBroker(registroNegocio);
                                        EventQueueManager.Instance.SendEvent(eventoHBNeg);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.Error("trataMensagemFix:delegate(): " + ex.Message, ex);
                                }
                            }
                        )
                    );

                }
            }
            catch (Exception ex)
            {
                logger.Error("trataMensagemFix(): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instrumento"></param>
        /// <param name="numSeq"></param>
        /// <param name="message"></param>
        /// <param name="mdEntry"></param>
        /// <param name="msgType"></param>
        /// <param name="isPuma"></param>
        private void tratarMensagemEstadoInstrumentoFIX(string instrumento, string numSeq, QuickFix.FIX44.Message message, QuickFix.Group mdEntry, string msgType, bool isPuma)
        {
            string tradingStatus = ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_UNKNOWN;
            string securityStatus = ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_UNKNOWN;
            string sessOpenTime = "";
            string secTradEvent = "";
            int estado = ConstantesMDS.ESTADO_PAPEL_INIBIDO;

            NegocioBase registroNegocios = dctNegocios[instrumento];

            if (mdEntry != null)
            {
                tradingStatus = mdEntry.IsSetField(QuickFix.Fields.Tags.TradingSessionSubID) ? mdEntry.GetString(QuickFix.Fields.Tags.TradingSessionSubID) : "0";
                securityStatus = mdEntry.IsSetField(QuickFix.Fields.Tags.SecurityTradingStatus) ? mdEntry.GetString(QuickFix.Fields.Tags.SecurityTradingStatus) : "0";
                sessOpenTime = mdEntry.IsSetField(QuickFix.Fields.Tags.TradSesOpenTime) ? mdEntry.GetString(QuickFix.Fields.Tags.TradSesOpenTime) : "0";
                secTradEvent = mdEntry.IsSetField(QuickFix.Fields.Tags.SecurityTradingEvent) ? mdEntry.GetString(QuickFix.Fields.Tags.SecurityTradingEvent) : "0";
            }
            else
            {
                tradingStatus = message.IsSetField(QuickFix.Fields.Tags.TradingSessionSubID) ? message.GetString(QuickFix.Fields.Tags.TradingSessionSubID) : "0";
                securityStatus = message.IsSetField(QuickFix.Fields.Tags.SecurityTradingStatus) ? message.GetString(QuickFix.Fields.Tags.SecurityTradingStatus) : "0";
                sessOpenTime = message.IsSetField(QuickFix.Fields.Tags.TradSesOpenTime) ? message.GetString(QuickFix.Fields.Tags.TradSesOpenTime) : "0";
                secTradEvent = message.IsSetField(QuickFix.Fields.Tags.SecurityTradingEvent) ? message.GetString(QuickFix.Fields.Tags.SecurityTradingEvent) : "0";
            }

            logger.InfoFormat("Instrumento [{0}] tssi [{1}] sts[{2}] opnTm [{3}] trdEvt [{4}]",
                instrumento,
                tradingStatus,
                securityStatus,
                sessOpenTime,
                secTradEvent);


            if ((mdEntry != null && mdEntry.IsSetField(QuickFix.Fields.Tags.TradingSessionSubID)) &&
                (!mdEntry.IsSetField(QuickFix.Fields.Tags.TradSesOpenTime) || msgType.Equals(QuickFix.FIX44.MarketDataSnapshotFullRefresh.MsgType)))
            {
                tradingStatus = mdEntry.GetInt(QuickFix.Fields.Tags.TradingSessionSubID).ToString();
            }
            else if (mdEntry != null && mdEntry.IsSetField(QuickFix.Fields.Tags.SecurityTradingStatus))
            {
                tradingStatus = mdEntry.GetInt(QuickFix.Fields.Tags.SecurityTradingStatus).ToString();
            }
            else if (message.IsSetField(QuickFix.Fields.Tags.SecurityTradingStatus))
            {
                tradingStatus = message.GetInt(QuickFix.Fields.Tags.SecurityTradingStatus).ToString();
            }

            if (tradingStatus.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_RESERVED) ||
                tradingStatus.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_FINAL_CLOSING_CALL))
            {
                estado = ConstantesMDS.ESTADO_PAPEL_EM_LEILAO;

                if (mdEntry != null && mdEntry.IsSetField(QuickFix.Fields.Tags.TradSesOpenTime))
                {
                    string data = mdEntry.GetString(QuickFix.Fields.Tags.TradSesOpenTime);

                    //string horaProrrogacaoUTC = String.Format("{0,9:d9}", mdEntry.GetString(QuickFix.Fields.Tags.TradSesOpenTime).Substring(8, 9));
                    //string horaProrrogacao = UmdfUtils.convertUTC2Local(data, horaProrrogacaoUTC);

                    DateTime horTeorico = DateTime.ParseExact(data, "yyyyMMdd-HH:mm:ss.fff", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);
                    registroNegocios.Negocio.HorarioTeorico = horTeorico.ToLocalTime();
                    //logger.InfoFormat("Instrumento[{0}]: HorarioProrrogacaoLeilao[{1}]", instrumento, registroNegocios.Negocio.HorarioTeorico);
                }
                else
                {
                    if (message.IsSetField(QuickFix.Fields.Tags.TradSesOpenTime))
                    {
                        string data = message.GetString(QuickFix.Fields.Tags.TradSesOpenTime);
                        //string horaProrrogacaoUTC = String.Format("{0,9:d9}", message.GetString(QuickFix.Fields.Tags.TradSesOpenTime).Substring(8, 9));
                        //string horaProrrogacao = UmdfUtils.convertUTC2Local(data, horaProrrogacaoUTC);

                        DateTime horTeorico = DateTime.ParseExact(data, "yyyyMMdd-HH:mm:ss.fff", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);
                        registroNegocios.Negocio.HorarioTeorico = horTeorico.ToLocalTime();
                        //logger.InfoFormat("Instrumento[{0}]: HorarioProrrogacaoLeilao[{1}]", instrumento, registroNegocios.Negocio.HorarioTeorico);
                    }
                }
            }

            else if (tradingStatus.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_OPEN))
            {
                estado = ConstantesMDS.ESTADO_PAPEL_EM_NEGOCIACAO;
                registroNegocios.Negocio.HorarioTeorico = DateTime.MinValue;
            }

            else if (tradingStatus.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_UNKNOWN))
                estado = ConstantesMDS.ESTADO_PAPEL_INIBIDO;

            else if (tradingStatus.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_FORBIDDEN))
                estado = ConstantesMDS.ESTADO_PAPEL_NAO_NEGOCIADO;

            else if (tradingStatus.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_HALT))
                estado = ConstantesMDS.ESTADO_PAPEL_INIBIDO;
            else
                estado = ConstantesMDS.ESTADO_PAPEL_NAO_NEGOCIADO;

            if (registroNegocios.Negocio.HorarioTeorico == DateTime.MinValue)
                logger.InfoFormat("Instrumento[{0}]: Mudanca para EstadoInstrumento[{1}] tradingStatus[{2}]",
                    instrumento, estado, tradingStatus);
            else
                logger.InfoFormat("Instrumento[{0}]: Mudanca para EstadoInstrumento[{1}] tradingStatus[{2}] HorarioProrrogacaoLeilao[{3}]",
                    instrumento, estado, tradingStatus, registroNegocios.Negocio.HorarioTeorico.ToString("dd/MM/yyyy HH:mm:ss"));

            registroNegocios.Negocio.EstadoInstrumento = estado;

            registroNegocios.Negocio.EstadoPapel2 = tradingStatus;

            if (bGerarEventoANG)
            {
                EventoNegocioANG eventoANG = new EventoNegocioANG();
                eventoANG.instrumento = instrumento;
                eventoANG.mensagem = NegocioBase.montarNegocioAnaliseGrafica(registroNegocios);
                EventQueueManager.Instance.SendEvent(eventoANG);
            }

            if (tradingStatus.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_FINAL_CLOSING_CALL))
                registroNegocios.FaseNegociacao = ConstantesMDS.FASE_NEGOCIACAO_CONGELAR_HORARIO_ANALISE_GRAFICA;

            // Se leilao, zera quantidade teorica, pois a mudanca de estado nao deve ter quantidade teorica preenchida
            //if (estado == ConstantesMDS.ESTADO_PAPEL_EM_LEILAO)
            //    registroNegocios.Negocio.Quantidade = 0;

            lock (dctNegocios[instrumento])
            {
                dctNegocios[instrumento] = registroNegocios;
            }
        }

        private void tratarMensagemEstadoGrupoCotacaoFIX(string instrumento, string numSeq, QuickFix.FIX44.Message message, QuickFix.Group mdEntry, bool isPuma)
        {
            string grupoCotacao = message.GetString(QuickFix.Fields.Tags.SecurityGroup);
            if (dctGrupoCotacao.ContainsKey(grupoCotacao))
            {
                if (dctGrupoCotacao[grupoCotacao].listaGrupoCotacao.Count == 0)
                    return;

                DateTime dataHoraMensagem = DateTime.MinValue;
                string dataMensagem = null;
                string horaMensagem = null;
                if (message.IsSetField(QuickFix.Fields.Tags.SendingTime))
                {
                    dataMensagem = message.GetString(QuickFix.Fields.Tags.SendingTime).Substring(0, 8);
                    dataHoraMensagem = DateTime.ParseExact(dataMensagem, "yyyyMMdd-HH:mm:ss.fff", CultureInfo.InvariantCulture);
                }

                string condicao = message.GetString(QuickFix.Fields.Tags.TradingSessionSubID);
                string estado2 = condicao;
                if (message.IsSetField(QuickFix.Fields.Tags.SecurityTradingStatus))
                {
                    estado2 = message.GetString(QuickFix.Fields.Tags.TradingSessionSubID);
                }

                int estado;
                string faseNegociacao = null;

                if (condicao.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_RESERVED) ||
                    condicao.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_FINAL_CLOSING_CALL))
                    estado = ConstantesMDS.ESTADO_PAPEL_EM_LEILAO;

                else if (condicao.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_OPEN))
                    estado = ConstantesMDS.ESTADO_PAPEL_EM_NEGOCIACAO;

                else if (condicao.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_UNKNOWN))
                    estado = ConstantesMDS.ESTADO_PAPEL_INIBIDO;

                else if (condicao.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_FORBIDDEN))
                    estado = ConstantesMDS.ESTADO_PAPEL_NAO_NEGOCIADO;

                else if (condicao.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_HALT))
                    estado = ConstantesMDS.ESTADO_PAPEL_INIBIDO;
                else
                    estado = ConstantesMDS.ESTADO_PAPEL_NAO_NEGOCIADO;

                if (condicao.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_FINAL_CLOSING_CALL))
                    faseNegociacao = ConstantesMDS.FASE_NEGOCIACAO_CONGELAR_HORARIO_ANALISE_GRAFICA;

                // Se nao houve mudanca no estado do Grupo de Cotacao, ignora
                if (dctGrupoCotacao[grupoCotacao].estado == estado)
                    return;

                dctGrupoCotacao[grupoCotacao].estado = estado;

                logger.InfoFormat("GrupoCotacao[{0}]: Alteracao de estado - TradingSessionSubID[{1}] estado[{2}] Data/Hora[{3}]",
                    grupoCotacao, condicao, estado, dataHoraMensagem.ToString("dd/MM/yyyy HH:mm:ss"));

                foreach (string item in dctGrupoCotacao[grupoCotacao].listaGrupoCotacao)
                {
                    if (dctNegocios.ContainsKey(item))
                    {
                        lock (dctNegocios[item])
                        {
                            NegocioBase registroNegocio = dctNegocios[item];

                            if (dataHoraMensagem >= registroNegocio.Negocio.HorarioTeorico)
                            {
                                registroNegocio.Negocio.EstadoInstrumento = estado;
                                registroNegocio.Negocio.EstadoGrupo = condicao;
                                registroNegocio.Negocio.EstadoPapel2 = estado2;

                                if (bGerarEventoANG)
                                {
                                    EventoNegocioANG eventoANG = new EventoNegocioANG();
                                    eventoANG.instrumento = instrumento;
                                    eventoANG.mensagem = NegocioBase.montarNegocioAnaliseGrafica(registroNegocio);
                                    EventQueueManager.Instance.SendEvent(eventoANG);
                                }

                                if (estado == ConstantesMDS.ESTADO_PAPEL_EM_LEILAO &&
                                    (String.IsNullOrEmpty(registroNegocio.Negocio.Data) || !registroNegocio.Negocio.Data.Equals(dataMensagem)))
                                {
                                    registroNegocio.Negocio.Preco = 0;
                                    registroNegocio.Negocio.PrecoAbertura = 0;
                                    registroNegocio.Negocio.PrecoMedio = 0;
                                    registroNegocio.Negocio.PrecoMinimo = 0;
                                    registroNegocio.Negocio.PrecoMaximo = 0;
                                    registroNegocio.Negocio.Quantidade = 0;
                                    registroNegocio.Negocio.Variacao = 0;
                                    registroNegocio.Negocio.QtdeNegociadaDia = 0;
                                    registroNegocio.Negocio.Compradora = "0";
                                    registroNegocio.Negocio.Vendedora = "0";
                                    registroNegocio.Negocio.NumeroNegocio = "0";
                                    registroNegocio.Negocio.QtdeNegocios = 0;
                                    registroNegocio.Negocio.VolumeTotal = 0;
                                    registroNegocio.Negocio.PrecoLeilao = 0;
                                    registroNegocio.FaseNegociacao = ConstantesMDS.FASE_NEGOCIACAO_ATUALIZAR_ANALISE_GRAFICA;
                                    registroNegocio.LivroNegocios = new List<LNGDadosNegocio>();

                                    logger.InfoFormat("Instrumento[{0}]: Inicializando contadores!", item);
                                }

                                EventoHttpNegocio eventoNEG = new EventoHttpNegocio();
                                eventoNEG.instrumento = item;
                                eventoNEG.cabecalho = MDSUtils.montaCabecalhoStreamer(ConstantesMDS.TIPO_REQUISICAO_NEGOCIOS, registroNegocio.TipoBolsa, 0, item, registroNegocio.CasasDecimais, null);
                                eventoNEG.negocio = NegocioBase.montarNegocioStreamer(registroNegocio.Negocio, registroNegocio.CasasDecimais);
                                EventQueueManager.Instance.SendEvent(eventoNEG);

                                // Envia evento para o HomeBroker/CotacaoStreamer
                                EventoHBNegocio eventoHBNeg = new EventoHBNegocio();
                                eventoHBNeg.instrumento = item;
                                eventoHBNeg.mensagem = NegocioBase.montarNegocioHomeBroker(registroNegocio);
                                EventQueueManager.Instance.SendEvent(eventoHBNeg);

                                logger.InfoFormat("Instrumento[{0}] GrupoCotacao[{1}]: Alteracao para EstadoInstrumento[{2}]",
                                    item, grupoCotacao, estado);
                            }

                            /*if (estado == ConstantesMDS.ESTADO_PAPEL_EM_NEGOCIACAO &&
                                registroNegocio.Negocio.QtdeNegocios > 0 &&
                                registroNegocio.Negocio.Data.Equals(DateTime.Now.ToString("yyyyMMdd")))
                            {
                                logger.InfoFormat("Instrumento[{0}]: Suspender Analise Grafica QtdeNegocios[{1}]", item, registroNegocio.Negocio.QtdeNegocios);
                                registroNegocio.FaseNegociacao = ConstantesMDS.FASE_NEGOCIACAO_SUSPENDER_ANALISE_GRAFICA;
                            }*/

                            // Gera e envia mensagem de Serie Historica
                            if (condicao.Equals(ConstantesUMDF.UMDF_SECURITY_TRADING_STATUS_HALT) &&
                                registroNegocio.Negocio.QtdeNegocios > 0 &&
                                registroNegocio.Negocio.Data.Equals(DateTime.Now.ToString("yyyyMMdd")) &&
                                registroNegocio.FaseNegociacao != ConstantesMDS.FASE_NEGOCIACAO_ENVIADO_SERIE_HISTORICA)
                            {
                                registroNegocio.FaseNegociacao = ConstantesMDS.FASE_NEGOCIACAO_ENVIADO_SERIE_HISTORICA;
                                registroNegocio.Negocio.PrecoFechamento = registroNegocio.Negocio.Preco;

                                logger.InfoFormat("Instrumento[{0}]: Define precoAtual como precoFechamento[{1}]",
                                    item, registroNegocio.Negocio.PrecoFechamento);

                                logger.InfoFormat("Instrumento[{0}]: SerieHistorica QtdNegocios[{1}]",
                                    item, registroNegocio.Negocio.QtdeNegocios);
                                enviaMensagemFila(queueSerieHistorica, JsonConvert.SerializeObject(registroNegocio), syncQueueSerieHistorica);
                            }
                        }
                    }
                }
            }
        }

        private string processaMensagemMarketDataFIX(string instrumento, string msgID, QuickFix.FIX44.Message message, List<Dictionary<string, string>> streamerLNG, string msgType, int marketDepth, bool isPuma, bool isBMF, ref bool sendToStreamer)
        {
            QuickFix.Group mdEntry = message.GetGroup(1, QuickFix.Fields.Tags.NoMDEntries);
            String mdEntryType = mdEntry.GetString(QuickFix.Fields.Tags.MDEntryType);

            switch (mdEntryType)
            {
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_PRICE_BAND:
                    // Tratar mensagem de Precos Referenciais
                    tratarMensagemBandaPrecosFIX(instrumento, msgID, mdEntry);
                    sendToStreamer = false;
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_OPENING_PRICE:
                    // Tratar mensagem de Preco de Abertura
                    tratarMensagemPrecoAberturaFIX(instrumento, msgID, mdEntry, msgType, isPuma);
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_CLOSING_PRICE:
                    // Tratar mensagem de Preco de Fechamento
                    tratarMensagemPrecoFechamentoFIX(instrumento, msgID, mdEntry);
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_SESSION_VWAP_PRICE:
                    // Tratar mensagem de Preco Medio
                    tratarMensagemPrecoMedioFIX(instrumento, msgID, mdEntry);
                    sendToStreamer = false;
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_SESSION_HIGH_PRICE:
                    // Tratar mensagem de Maxima do dia
                    tratarMensagemMaximoDiaFIX(instrumento, msgID, mdEntry);
                    sendToStreamer = false;
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_SESSION_LOW_PRICE:
                    // Tratar mensagem de Minima do dia
                    tratarMensagemMinimoDiaFIX(instrumento, msgID, mdEntry);
                    sendToStreamer = false;
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_TRADE:
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_INDEX_VALUE:
                    // Tratar mensagem de negocio
                    tratarMensagemNegocioFIX(instrumento, msgID, mdEntry, streamerLNG, msgType, isPuma, isBMF);
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_TRADE_VOLUME:
                    // Tratar mensagem de Volume de Negocios
                    tratarMensagemVolumeNegociosFIX(instrumento, msgID, mdEntry);
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_SETTLEMENT_PRICE:
                    tratarMensagemPrecoAjusteFIX(instrumento, msgID, mdEntry);
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_SECURITY_TRADING_STATE:
                    // Tratar mensagem de Estado do Instrumento
                    tratarMensagemEstadoInstrumentoFIX(instrumento, msgID, message, mdEntry, msgType, isPuma);
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_BID:
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_OFFER:
                    tratarMensagemMelhorOfertaFix(instrumento, msgID, mdEntry);
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_QUANTITY_BAND:
                    logger.WarnFormat("Instrumento[{0}]: MDEntryType QuantityBand(h) sem tratamento", instrumento);
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_OPEN_INTEREST:
                    logger.WarnFormat("Instrumento[{0}]: MDEntryType OpenInterest(C) sem tratamento", instrumento);
                    break;
                case ConstantesUMDF.UMDF_MD_ENTRY_TYPE_IMBALANCE:
                    tratarMensagemImbalance(instrumento, msgID, message, mdEntry, isPuma);
                    break;
                default:
                    logger.Error("MDEntryType invalido [" + mdEntryType + "]");
                    break;
            }

            return instrumento;
        }

        private void tratarMensagemBandaPrecosFIX(string instrumento, string numSeq, QuickFix.Group mdEntry)
        {
            Decimal precoAjuste = Decimal.Zero;
            NegocioBase registroNegocios = dctNegocios[instrumento];

            if (mdEntry.IsSetField(QuickFix.Fields.Tags.TradingReferencePrice))
            {
                precoAjuste = mdEntry.GetDecimal(QuickFix.Fields.Tags.TradingReferencePrice);
            }

            // Preferi tirar esse calculo, tava gerando um precoAjuste muito estranho
            /*else if (mdEntry.IsDefined("HighLimitPrice") && mdEntry.IsDefined("LowLimitPrice"))
            {
                Decimal precoLimiteMax = mdEntry.GetBigDecimal("HighLimitPrice");
                Decimal precoLimiteMin = mdEntry.GetBigDecimal("LowLimitPrice");
                precoAjuste = (precoLimiteMax + precoLimiteMin) / 2;

                //precoAjuste = String.format("%013.2f",
                //        precoAjusteCalculado.doubleValue()).replace('.', ',');
            }*/

            if (precoAjuste != Decimal.Zero)
            {
                registroNegocios.Negocio.PrecoAjuste = precoAjuste;
                registroNegocios.Negocio.MsgIdAnterior = numSeq;

                //lock (dctNegocios[instrumento])
                //{
                //    dctNegocios[instrumento] = registroNegocios;
                //}

                logger.Info("Instrumento[" + instrumento +
                        "] sequencial[" + numSeq +
                        "] precoAjuste[" + precoAjuste + "]");
            }

            return;
        }

        private void tratarMensagemPrecoAberturaFIX(string instrumento, string numSeq, QuickFix.Group mdEntry, string msgType, bool isPuma)
        {
            NegocioBase registroNegocios = dctNegocios[instrumento];

            Decimal ultimoPrecoAbertura = registroNegocios.Negocio.PrecoAbertura;

            if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntryPx))
            {
                Decimal precoAbertura = mdEntry.GetDecimal(QuickFix.Fields.Tags.MDEntryPx);
                long quantidade = 0;
                if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntrySize))
                    quantidade = mdEntry.GetInt(QuickFix.Fields.Tags.MDEntrySize);

                if (precoAbertura > 0)
                {
                    String data = DateTime.Now.ToString("yyyyMMdd");
                    String hora = DateTime.Now.ToString("HH:mm:ss.fff");

                    if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntryDate) && !msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_SNAPSHOT_SINGLE))
                    {
                        try
                        {
                            data = mdEntry.GetString(QuickFix.Fields.Tags.MDEntryDate);
                            String horaOriginal = mdEntry.GetString(QuickFix.Fields.Tags.MDEntryTime).Replace(":", "").Replace(".", "");
                            hora = UmdfUtils.convertUTC2Local(data, horaOriginal);
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Falha na formatacao da data[" + mdEntry.GetString(QuickFix.Fields.Tags.MDEntryDate) +
                                    "] e hora[" + mdEntry.GetString(QuickFix.Fields.Tags.MDEntryTime) + "]: " + ex.Message);
                        }
                    }

                    registroNegocios.Negocio.MsgIdAnterior = numSeq;

                    string OpenCloseSettlFlag = ConstantesUMDF.UMDF_OCSF_UNDOCUMENTED_OPENING_PRICE;

                    if (mdEntry.IsSetField(QuickFix.Fields.Tags.OpenCloseSettlFlag))
                        OpenCloseSettlFlag = mdEntry.GetString(QuickFix.Fields.Tags.OpenCloseSettlFlag);
                    else if (mdEntry.IsSetField(QuickFix.Fields.Tags.OpenCloseSettleFlag))
                        OpenCloseSettlFlag = mdEntry.GetString(QuickFix.Fields.Tags.OpenCloseSettleFlag);

                    switch (OpenCloseSettlFlag)
                    {
                        case ConstantesUMDF.UMDF_OCSF_THEORETICAL_PRICE:

                            // Forçar leilão aqui é um recurso que tem no MDS versão java, mas talvez esteja errada essa implementação, requer melhor avaliação
                            //registroNegocios.Negocio.EstadoInstrumento = ConstantesMDS.ESTADO_PAPEL_EM_LEILAO;
                            //logger.InfoFormat("Instrumento[{0}] estadoInstrumento[{1}]", instrumento, registroNegocios.Negocio.EstadoInstrumento);

                            registroNegocios.Negocio.PrecoTeoricoAbertura = precoAbertura;
                            registroNegocios.Negocio.Quantidade = quantidade;
                            registroNegocios.Negocio.VariacaoTeorica = MDSUtils.calcularVariacao(precoAbertura, registroNegocios.Negocio.PrecoFechamento);

                            logger.InfoFormat("Instrumento[{0}] sequencial[{1}] precoTeoricoAbertura[{2}] VariacaoTeorica[{3}]",
                                instrumento, numSeq, registroNegocios.Negocio.PrecoTeoricoAbertura, registroNegocios.Negocio.VariacaoTeorica);

                            if (bGerarEventoANG)
                            {
                                verificaSuspensaoAnaliseGrafica(registroNegocios);

                                EventoNegocioANG eventoANG = new EventoNegocioANG();
                                eventoANG.instrumento = instrumento;
                                eventoANG.mensagem = NegocioBase.montarNegocioAnaliseGrafica(registroNegocios);
                                EventQueueManager.Instance.SendEvent(eventoANG);
                            }
                            break;

                        case ConstantesUMDF.UMDF_OCSF_SESSION_SETTLEMENT_ENTRY:
                        case ConstantesUMDF.UMDF_OCSF_DAILY_SETTLEMENT_ENTRY:
                        case ConstantesUMDF.UMDF_OCSF_UNDOCUMENTED_OPENING_PRICE:
                            registroNegocios.Negocio.PrecoAbertura = precoAbertura;
                            registroNegocios.Negocio.Quantidade = quantidade;

                            if (DateTime.Now.ToString("yyyyMMdd").Equals(data))
                            {
                                registroNegocios.Negocio.Data = data;
                                registroNegocios.Negocio.Hora = hora;
                            }

                            logger.InfoFormat("Instrumento[{0}] sequencial[{1}] precoAbertura[{2}]",
                                instrumento, numSeq, registroNegocios.Negocio.PrecoAbertura);

                            if (bGerarEventoANG)
                            {
                                ThreadPool.QueueUserWorkItem(
                                    new WaitCallback(
                                        delegate(object required)
                                        {
                                            EventoNegocioANG eventoANG = new EventoNegocioANG();
                                            eventoANG.instrumento = instrumento;
                                            eventoANG.mensagem = NegocioBase.montarPrecoAbertura(registroNegocios);
                                            EventQueueManager.Instance.SendEvent(eventoANG);
                                        }
                                    )
                                );
                            }

                            /*
                            if (registroNegocios.Negocio.QtdeNegocios > 0)
                            {
                                logger.InfoFormat("Instrumento[{0}]: SerieHistorica QtdNegocios[{1}]",
                                    instrumento, registroNegocios.Negocio.QtdeNegocios);
                                queueSerieHistorica.Enqueue(JsonConvert.SerializeObject(registroNegocios));
                            }
                            */

                            if (dctComposicaoIndice.ContainsKey(instrumento) &&
                                ultimoPrecoAbertura != precoAbertura &&
                                ultimoPrecoAbertura != 0 &&
                                true == false)
                            {
                                registroNegocios.Negocio.Preco = 0;
                                registroNegocios.Negocio.PrecoMedio = 0;
                                registroNegocios.Negocio.PrecoMinimo = 0;
                                registroNegocios.Negocio.PrecoMaximo = 0;
                                registroNegocios.Negocio.Quantidade = 0;
                                registroNegocios.Negocio.Variacao = 0;
                                registroNegocios.Negocio.QtdeNegociadaDia = 0;
                                registroNegocios.Negocio.Compradora = "0";
                                registroNegocios.Negocio.Vendedora = "0";
                                registroNegocios.Negocio.NumeroNegocio = "0";
                                registroNegocios.Negocio.QtdeNegocios = 0;
                                registroNegocios.Negocio.VolumeTotal = 0;
                                registroNegocios.FaseNegociacao = ConstantesMDS.FASE_NEGOCIACAO_ATUALIZAR_ANALISE_GRAFICA;
                                registroNegocios.LivroNegocios = new List<LNGDadosNegocio>();

                                logger.InfoFormat("Instrumento[{0}]: Inicializando contadores do indice!", instrumento);

                                if (instrumento.Equals(ConstantesMDS.INDICE_IBOVESPA))
                                {
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TOTAL].Negocio.QtdeNegociadaDia = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TOTAL].Negocio.NumeroNegocio = "0";
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TOTAL].Negocio.QtdeNegocios = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TOTAL].Negocio.VolumeTotal = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_VISTA].Negocio.QtdeNegociadaDia = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_VISTA].Negocio.NumeroNegocio = "0";
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_VISTA].Negocio.QtdeNegocios = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_VISTA].Negocio.VolumeTotal = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TERMO].Negocio.QtdeNegociadaDia = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TERMO].Negocio.NumeroNegocio = "0";
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TERMO].Negocio.QtdeNegocios = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TERMO].Negocio.VolumeTotal = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_OPCOES].Negocio.QtdeNegociadaDia = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_OPCOES].Negocio.NumeroNegocio = "0";
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_OPCOES].Negocio.QtdeNegocios = 0;
                                    dctNegocios[ConstantesMDS.INDICE_IBOVESPA_OPCOES].Negocio.VolumeTotal = 0;
                                }
                            }
                            break;

                        case ConstantesUMDF.UMDF_OCSF_ENTRY_FROM_PREVIOUS_BUSINESS_DAY:
                            //registroNegocios.Negocio.PrecoFechamento = precoAbertura;
                            //registroNegocios.Negocio.Quantidade = quantidade;
                            break;
                        default:
                            logger.Error("Invalid OpenCloseSettlFlag[" + OpenCloseSettlFlag + "]");
                            break;
                    }

                    lock (dctNegocios[instrumento])
                    {
                        dctNegocios[instrumento] = registroNegocios;
                    }
                }
            }
            return;
        }


        private void tratarMensagemPrecoFechamentoFIX(string instrumento, string numSeq, QuickFix.Group mdEntry)
        {
            NegocioBase registroNegocios = dctNegocios[instrumento];

            if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntryPx))
            {
                Decimal precoFechamento = mdEntry.GetDecimal(QuickFix.Fields.Tags.MDEntryPx);
                string mdEntryDate = mdEntry.GetString(QuickFix.Fields.Tags.MDEntryDate);

                string openCloseSettlFlag = null;
                if (mdEntry.IsSetField(QuickFix.Fields.Tags.OpenCloseSettlFlag))
                    openCloseSettlFlag = mdEntry.GetString(QuickFix.Fields.Tags.OpenCloseSettlFlag);
                else if (mdEntry.IsSetField(QuickFix.Fields.Tags.OpenCloseSettleFlag))
                    openCloseSettlFlag = mdEntry.GetString(QuickFix.Fields.Tags.OpenCloseSettleFlag);

                if (openCloseSettlFlag != null)
                {
                    if (openCloseSettlFlag.Equals(ConstantesUMDF.UMDF_OCSF_ENTRY_FROM_PREVIOUS_BUSINESS_DAY) ||
                       (openCloseSettlFlag.Equals(ConstantesUMDF.UMDF_OCSF_DAILY_SETTLEMENT_ENTRY) && registroNegocios.Negocio.PrecoFechamento == 0) ||
                       (openCloseSettlFlag.Equals(ConstantesUMDF.UMDF_OCSF_DAILY_SETTLEMENT_ENTRY) && registroNegocios.TipoBolsa.Equals(ConstantesMDS.DESCRICAO_DE_BOLSA_BMF)))
                    {
                        String data = DateTime.Now.ToString("yyyyMMdd");
                        String hora = DateTime.Now.ToString("HH:mm:ss.fff");

                        if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntryDate) )
                        {
                            try
                            {
                                data = mdEntry.GetString(QuickFix.Fields.Tags.MDEntryDate);
                                String horaOriginal = mdEntry.GetString(QuickFix.Fields.Tags.MDEntryTime).Replace(":", "").Replace(".", "");
                                hora = UmdfUtils.convertUTC2Local(data, horaOriginal);
                            }
                            catch (Exception ex)
                            {
                                logger.Error("Falha na formatacao da data[" + mdEntry.GetString(QuickFix.Fields.Tags.MDEntryDate) +
                                        "] e hora[" + mdEntry.GetString(QuickFix.Fields.Tags.MDEntryTime) + "]: " + ex.Message);
                            }

                            registroNegocios.Negocio.Data = data;
                            registroNegocios.Negocio.Hora = hora;
                        }

                        registroNegocios.Negocio.MsgIdAnterior = numSeq;
                        //if (registroNegocios.Negocio.PrecoAjuste == 0)
                        registroNegocios.Negocio.PrecoFechamento = precoFechamento;

                        if (registroNegocios.Negocio.PrecoTeoricoAbertura != 0)
                        {
                            registroNegocios.Negocio.VariacaoTeorica = MDSUtils.calcularVariacao(
                                registroNegocios.Negocio.PrecoTeoricoAbertura,
                                registroNegocios.Negocio.PrecoFechamento);

                            logger.InfoFormat("Instrumento[{0}] sequencial[{1}] VariacaoTeorica[{2}]",
                            instrumento, numSeq, registroNegocios.Negocio.VariacaoTeorica);
                        }


                        logger.InfoFormat("Instrumento[{0}] sequencial[{1}] precoFechamento[{2}] OpenCloseSettlFlag[{3}] MDEntryDate[{4}]",
                            instrumento, numSeq, precoFechamento, openCloseSettlFlag, mdEntryDate);

                        if (openCloseSettlFlag.Equals(ConstantesUMDF.UMDF_OCSF_DAILY_SETTLEMENT_ENTRY) &&
                            registroNegocios.Negocio.QtdeNegocios > 0 &&
                            registroNegocios.Negocio.Data.Equals(DateTime.Now.ToString("yyyyMMdd")) &&
                            registroNegocios.FaseNegociacao != ConstantesMDS.FASE_NEGOCIACAO_ENVIADO_SERIE_HISTORICA)
                        {
                            registroNegocios.FaseNegociacao = ConstantesMDS.FASE_NEGOCIACAO_ENVIADO_SERIE_HISTORICA;

                            logger.InfoFormat("Instrumento[{0}]: SerieHistorica QtdNegocios[{1}]",
                                instrumento, registroNegocios.Negocio.QtdeNegocios);

                            if (bGerarEventoANG)
                            {
                                EventoNegocioANG eventoANG = new EventoNegocioANG();
                                eventoANG.instrumento = registroNegocios.Instrumento;
                                eventoANG.mensagem = NegocioBase.montarSerieHistorica(registroNegocios);
                                EventQueueManager.Instance.SendEvent(eventoANG);
                            }
                        }

                        lock (dctNegocios[instrumento])
                        {
                            dctNegocios[instrumento] = registroNegocios;
                        }
                    }
                    else
                        logger.InfoFormat("Desconsiderando Instrumento[{0}] sequencial[{1}] precoFechamento[{2}] OpenCloseSett[{3}]",
                            instrumento, numSeq, precoFechamento, openCloseSettlFlag);

                    if (bGerarEventoANG)
                    {
                        ThreadPool.QueueUserWorkItem(
                            new WaitCallback(
                                delegate(object required)
                                {
                                    EventoNegocioANG eventoANG = new EventoNegocioANG();
                                    eventoANG.instrumento = instrumento;
                                    eventoANG.mensagem = NegocioBase.montarPrecoFechamento(registroNegocios);
                                    EventQueueManager.Instance.SendEvent(eventoANG);
                                }
                            )
                        );
                    }
                }
                else
                {
                    logger.InfoFormat("Fechamento sem  OpenCloseSett Instrumento[{0}] sequencial[{1}] precoFechamento[{2}]",
                        instrumento, numSeq, precoFechamento);
                    if (registroNegocios.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_INDICES))
                    {
                        registroNegocios.Negocio.PrecoFechamento = precoFechamento;

                        lock (dctNegocios[instrumento])
                        {
                            dctNegocios[instrumento] = registroNegocios;
                        }
                    }
                }
            }
            return;
        }


        private void tratarMensagemPrecoMedioFIX(string instrumento, string numSeq, QuickFix.Group mdEntry)
        {
            NegocioBase registroNegocios = dctNegocios[instrumento];

            if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntryPx))
            {
                Decimal precoMedio = mdEntry.GetDecimal(QuickFix.Fields.Tags.MDEntryPx);

                List<HashEntry> lstHashes = new List<HashEntry>();

                lstHashes.Add(new HashEntry("PrecoMedio", precoMedio.ToString()));

                if (redisDB != null && redis.IsConnected)
                    redisDB.HashSet(instrumento, lstHashes.ToArray());

                //String precoAbertura = String.format("%013.2f",
                //        Double.parseDouble(precoAberturaSemFormato)).replace('.', ',');
                registroNegocios.Negocio.MsgIdAnterior = numSeq;
                registroNegocios.Negocio.PrecoMedio = precoMedio;

                //lock (dctNegocios[instrumento])
                //{
                //    dctNegocios[instrumento] = registroNegocios;
                //}

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Instrumento[" + instrumento +
                            "] sequencial[" + numSeq +
                            "] precoMedio[" + precoMedio + "]");
                }
            }
            return;
        }

        private void tratarMensagemMaximoDiaFIX(string instrumento, string numSeq, QuickFix.Group mdEntry)
        {
            NegocioBase registroNegocios = dctNegocios[instrumento];

            if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntryPx))
            {
                Decimal precoMaximo = mdEntry.GetDecimal(QuickFix.Fields.Tags.MDEntryPx);

                List<HashEntry> lstHashes = new List<HashEntry>();

                lstHashes.Add(new HashEntry("PrecoMaximo", precoMaximo.ToString()));

                if (redisDB != null && redis.IsConnected)
                    redisDB.HashSet(instrumento, lstHashes.ToArray());

                //String precoAbertura = String.format("%013.2f",
                //        Double.parseDouble(precoAberturaSemFormato)).replace('.', ',');
                registroNegocios.Negocio.MsgIdAnterior = numSeq;
                registroNegocios.Negocio.PrecoMaximo = precoMaximo;

                //lock (dctNegocios[instrumento])
                //{
                //    dctNegocios[instrumento] = registroNegocios;
                //}
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Instrumento[" + instrumento +
                            "] sequencial[" + numSeq +
                            "] precoMaximo[" + precoMaximo + "]");
                }
            }
            return;
        }

        private void tratarMensagemMinimoDiaFIX(string instrumento, string numSeq, QuickFix.Group mdEntry)
        {
            NegocioBase registroNegocios = dctNegocios[instrumento];

            if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntryPx))
            {
                Decimal precoMinimo = mdEntry.GetDecimal(QuickFix.Fields.Tags.MDEntryPx);

                List<HashEntry> lstHashes = new List<HashEntry>();

                lstHashes.Add(new HashEntry("PrecoMinimo", precoMinimo.ToString()));

                if (redisDB != null && redis.IsConnected)
                    redisDB.HashSet(instrumento, lstHashes.ToArray());

                //String precoAbertura = String.format("%013.2f",
                //        Double.parseDouble(precoAberturaSemFormato)).replace('.', ',');
                registroNegocios.Negocio.MsgIdAnterior = numSeq;
                registroNegocios.Negocio.PrecoMinimo = precoMinimo;

                //lock (dctNegocios[instrumento])
                //{
                //    dctNegocios[instrumento] = registroNegocios;
                //}

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Instrumento[" + instrumento +
                            "] sequencial[" + numSeq +
                            "] precoMinimo[" + precoMinimo + "]");
                }
            }
            return;
        }

        private void tratarMensagemNegocioFIX(string instrumento, string msgID, QuickFix.Group mdEntry, List<Dictionary<string, string>> streamerLNG, string msgType, bool isPuma, bool isBMF)
        {
            NegocioBase registroNegocios = dctNegocios[instrumento];

            string mdUpdateAction = "0";
            if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDUpdateAction))
                mdUpdateAction = mdEntry.GetString(QuickFix.Fields.Tags.MDUpdateAction);

            String mdEntryType = mdEntry.GetString(QuickFix.Fields.Tags.MDEntryType);

            String data = "";
            String hora = "";
            try
            {
                data = mdEntry.GetString(QuickFix.Fields.Tags.MDEntryDate);
                String formatHora = (isPuma ? "{0,9:d9}" : "{0:d6}");
                String horaOriginal = mdEntry.GetString(QuickFix.Fields.Tags.MDEntryTime).Replace(":", "").Replace(".", "");
                hora = UmdfUtils.convertUTC2Local(data, horaOriginal);
            }
            catch (Exception ex)
            {
                logger.Error("Falha na formatacao da data[" + mdEntry.GetString(QuickFix.Fields.Tags.MDEntryDate) +
                        "] e hora[" + mdEntry.GetString(QuickFix.Fields.Tags.MDEntryTime) + "]: " + ex.Message);
            }

            String compradora = "0";
            try
            {
                if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntryBuyer))
                {
                    String compradoraFormatada = mdEntry.GetString(QuickFix.Fields.Tags.MDEntryBuyer).Replace("BM", "").Trim();
                    if (String.IsNullOrEmpty(compradoraFormatada))
                    {
                        compradoraFormatada = "0";
                    }
                    compradora = String.Format("{0:d8}", Int64.Parse(compradoraFormatada));
                }
            }
            catch (Exception ex)
            {
                logger.Error("Falha na formatacao da compradora[" + mdEntry.GetString(QuickFix.Fields.Tags.MDEntryBuyer) + "]: " + ex.Message, ex);
            }

            String vendedora = "0";
            try
            {
                if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntrySeller))
                {
                    String vendedoraFormatada = mdEntry.GetString(QuickFix.Fields.Tags.MDEntrySeller).Replace("BM", "").Trim();
                    if (String.IsNullOrEmpty(vendedoraFormatada))
                    {
                        vendedoraFormatada = "0";
                    }
                    vendedora = String.Format("{0:d8}", Int64.Parse(vendedoraFormatada));
                }
            }
            catch (Exception eex)
            {
                logger.Error("Falha na formatacao da vendedora[" + mdEntry.GetString(QuickFix.Fields.Tags.MDEntrySeller) + "]: " + eex.Message, eex);
            }

            Decimal precoTrade = Decimal.Zero;
            try
            {
                precoTrade = mdEntry.GetDecimal(QuickFix.Fields.Tags.MDEntryPx);
            }
            catch (Exception ex)
            {
                logger.Error("Falha na formatacao do preco[" + mdEntry.GetDecimal(QuickFix.Fields.Tags.MDEntryPx) + "]: " + ex.Message, ex);
            }

            long quantidade = 0;
            if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntrySize))
            {
                quantidade = mdEntry.GetInt(QuickFix.Fields.Tags.MDEntrySize);
            }

            Decimal valorNegocio = Decimal.Zero;
            valorNegocio = precoTrade * quantidade;

            long qtdeNegociadaDia = 0;
            if (mdEntry.IsSetField(QuickFix.Fields.Tags.TradeVolume))
            {
                qtdeNegociadaDia = mdEntry.GetInt(QuickFix.Fields.Tags.TradeVolume);
            }

            // Calcula preco medio e volume financeiro apenas para MEGA, pois no UMDF 1.6 e 2.0 já vem os campos SessionVWAPPrice(9) e TradeVolume(B)
            Decimal volume = Decimal.Zero;
            Decimal precoMedio = Decimal.Zero;

            if (msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_SNAPSHOT_SINGLE))
            {
                // Quando recebe o snapshot do umdf, e for retomada durante o dia, faz um calculo aproximado do precoMedio e volume
                precoMedio = (registroNegocios.Negocio.PrecoMinimo + registroNegocios.Negocio.PrecoMaximo) / 2;
                volume = qtdeNegociadaDia * precoMedio;
            }
            else
            {
                volume = registroNegocios.Negocio.VolumeTotal + valorNegocio;
                if (mdUpdateAction.Equals(ConstantesUMDF.UMDF_MD_UPDT_ACTION_NEW))
                    volume = registroNegocios.Negocio.VolumeTotal + valorNegocio;
                else
                    volume = registroNegocios.Negocio.VolumeTotal - valorNegocio;

                if (qtdeNegociadaDia != 0)
                    precoMedio = (volume * registroNegocios.CoeficienteMultiplicacao) / qtdeNegociadaDia;
            }

            long numeroNegocio = 0;
            String numeroNegocioFormatado = "0";
            if (mdEntry.IsSetField(QuickFix.Fields.Tags.TradeID))
            {
                try
                {
                    numeroNegocioFormatado = mdEntry.GetString(QuickFix.Fields.Tags.TradeID);
                    numeroNegocioFormatado = numeroNegocioFormatado.Substring(0, numeroNegocioFormatado.Length - 1);
                    numeroNegocio = Int64.Parse(numeroNegocioFormatado);
                }
                catch (Exception ex)
                {
                    logger.Error("Falha na formatacao do numeroNegocio[" +
                            mdEntry.GetString(QuickFix.Fields.Tags.TradeID) + "]: " + ex.Message, ex);
                }
            }

            Decimal variacao = Decimal.Zero;
            if (mdUpdateAction.Equals(ConstantesUMDF.UMDF_MD_UPDT_ACTION_NEW) &&
                (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_TRADE) || mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_INDEX_VALUE)))
            {
                try
                {
                    Decimal precoFechamento = Decimal.Zero;
                    if (mdEntry.IsSetField(QuickFix.Fields.Tags.NetChgPrevDay) && !isBMF)
                    {
                        Decimal diferenca = mdEntry.GetDecimal(QuickFix.Fields.Tags.NetChgPrevDay);
                        precoFechamento = precoTrade - diferenca;
                    }
                    else
                    {
                        precoFechamento = registroNegocios.Negocio.PrecoFechamento;
                    }

                    if (precoFechamento == Decimal.Zero)
                        precoFechamento = precoTrade;
                    variacao = precoTrade / precoFechamento;
                    variacao -= Decimal.One;
                    variacao *= 100;
                }
                catch (Exception ex)
                {
                    logger.Error("Falha na formatacao da variacao: " + ex.Message, ex);
                }
            }

            List<HashEntry> lstHashes = new List<HashEntry>();

            lstHashes.Add(new HashEntry("Data", data));
            lstHashes.Add(new HashEntry("Hora", hora));
            lstHashes.Add(new HashEntry("Variacao", variacao.ToString()));
            lstHashes.Add(new HashEntry("Compradora", compradora));
            lstHashes.Add(new HashEntry("Vendedora", vendedora));
            lstHashes.Add(new HashEntry("Preco", precoTrade.ToString()));


            if (redisDB != null && redis.IsConnected)
                redisDB.HashSet(instrumento, lstHashes.ToArray());

            //if (!msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_SNAPSHOT_SINGLE))
            //    registroNegocios.Negocio.EstadoInstrumento = ConstantesMDS.ESTADO_PAPEL_EM_NEGOCIACAO;

            registroNegocios.Negocio.Data = data;
            registroNegocios.Negocio.Hora = hora;
            registroNegocios.Negocio.Variacao = variacao;
            registroNegocios.Negocio.Compradora = compradora;
            registroNegocios.Negocio.Vendedora = vendedora;

            registroNegocios.Negocio.Preco = precoTrade;
            if (registroNegocios.Negocio.PrecoAbertura == 0)
                registroNegocios.Negocio.PrecoAbertura = precoTrade;
            if (registroNegocios.Negocio.PrecoFechamento == 0)
                registroNegocios.Negocio.PrecoFechamento = precoTrade;
            if (registroNegocios.Negocio.PrecoMinimo == 0)
                registroNegocios.Negocio.PrecoMinimo = precoTrade;
            if (registroNegocios.Negocio.PrecoMaximo == 0)
                registroNegocios.Negocio.PrecoMaximo = precoTrade;
            if (registroNegocios.Negocio.PrecoMedio == 0)
                registroNegocios.Negocio.PrecoMedio = precoTrade;

            if (!mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_INDEX_VALUE))
            {
                registroNegocios.Negocio.NumeroNegocio = numeroNegocioFormatado;
                registroNegocios.Negocio.QtdeNegocios = numeroNegocio;
                registroNegocios.Negocio.VolumeTotal = volume;
                registroNegocios.Negocio.Quantidade = quantidade;
                registroNegocios.Negocio.QtdeNegociadaDia = qtdeNegociadaDia;

                if (!isPuma)
                {
                    registroNegocios.Negocio.PrecoMedio = precoMedio;
                }
            }
            else
            {
                ContabilizaIndices(registroNegocios, instrumento);
                if (instrumento.Equals(ConstantesMDS.INDICE_IBOVESPA))
                {
                    ContabilizaIndices(dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TOTAL], ConstantesMDS.INDICE_IBOVESPA_TOTAL);
                    ContabilizaIndices(dctNegocios[ConstantesMDS.INDICE_IBOVESPA_VISTA], ConstantesMDS.INDICE_IBOVESPA_VISTA);
                    ContabilizaIndices(dctNegocios[ConstantesMDS.INDICE_IBOVESPA_TERMO], ConstantesMDS.INDICE_IBOVESPA_TERMO);
                    ContabilizaIndices(dctNegocios[ConstantesMDS.INDICE_IBOVESPA_OPCOES], ConstantesMDS.INDICE_IBOVESPA_OPCOES);
                }
            }

            if (mdUpdateAction.Equals(ConstantesUMDF.UMDF_MD_UPDT_ACTION_NEW))
            {
                if (!msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_SNAPSHOT_SINGLE))
                {
                    NegocioBase.AdicionarLivroNegocios(registroNegocios, numItensLNG);

                    streamerLNG.Add(NegocioBase.montarItemLivroNegociosStreamer(ConstantesMDS.HTTP_LIVRO_NEGOCIOS_TIPO_ACAO_INCLUIR, 0,
                                                                            registroNegocios.LivroNegocios[0], registroNegocios.CasasDecimais));
                }
            }
            else
            {
                int posicao = NegocioBase.BuscarPosicaoNegocioCancelado(numeroNegocioFormatado, registroNegocios);

                if (posicao >= 0 && posicao < registroNegocios.LivroNegocios.Count)
                {
                    streamerLNG.Add(NegocioBase.montarItemLivroNegociosStreamer(ConstantesMDS.HTTP_LIVRO_NEGOCIOS_TIPO_ACAO_EXCLUIR, posicao,
                                                                        registroNegocios.LivroNegocios[posicao], registroNegocios.CasasDecimais));

                    NegocioBase.RemoverNegocioCancelado(posicao, registroNegocios);
                }

            }

            //lock (dctNegocios[instrumento])
            //{
            //    dctNegocios[instrumento] = registroNegocios;
            //}
            //enviaMensagemFila(queueNegocio, JsonConvert.SerializeObject(registroNegocios), syncQueueNegocio);
            if (bGerarEventoANG)
            {
                EventoNegocioANG eventoANG = new EventoNegocioANG();
                eventoANG.instrumento = instrumento;
                eventoANG.mensagem = NegocioBase.montarNegocioAnaliseGrafica(registroNegocios);
                EventQueueManager.Instance.SendEvent(eventoANG);
            }

            return;
        }

        private void tratarMensagemVolumeNegociosFIX(string instrumento, string numSeq, QuickFix.Group mdEntry)
        {
            NegocioBase registroNegocios = dctNegocios[instrumento];

            if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntryPx))
            {
                Decimal volumeTotal = mdEntry.GetDecimal(QuickFix.Fields.Tags.MDEntryPx);

                //String precoAbertura = String.format("%013.2f",
                //        Double.parseDouble(precoAberturaSemFormato)).replace('.', ',');
                registroNegocios.Negocio.MsgIdAnterior = numSeq;
                registroNegocios.Negocio.VolumeTotal = volumeTotal;

                //lock (dctNegocios[instrumento])
                //{
                //    dctNegocios[instrumento] = registroNegocios;
                //}

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Instrumento[" + instrumento +
                            "] sequencial[" + numSeq +
                            "] VolumeTotal[" + volumeTotal + "]");
                }
            }
            return;
        }

        private void tratarMensagemPrecoAjusteFIX(string instrumento, string numSeq, QuickFix.Group mdEntry)
        {
            NegocioBase registroNegocios = dctNegocios[instrumento];

            // Despreza precoAjuste quando OpenCloseSettlFlag <> 0 (Daily settlement entry)
            string openCloseSettlFlag = "0";
            if (mdEntry.IsSetField(QuickFix.Fields.Tags.OpenCloseSettlFlag))
                openCloseSettlFlag = mdEntry.GetString(QuickFix.Fields.Tags.OpenCloseSettlFlag);

            string settlPriceType = "0";
            if (mdEntry.IsSetField(QuickFix.Fields.Tags.SettlPriceType))
                settlPriceType = mdEntry.GetString(QuickFix.Fields.Tags.SettlPriceType);

            // PriceType = 1 - 
            string priceType = "2";
            if (mdEntry.IsSetField(QuickFix.Fields.Tags.PriceType))
                priceType = mdEntry.GetString(QuickFix.Fields.Tags.PriceType);

            // Before session OpenCloseSettlFlag=4 SettlPriceType=1 => Previous day final settle
            // During session OpenCloseSettlFlag=4 SettlPriceType=3 => Previous day updated settle
            // During session OpenCloseSettlFlag=1 SettlPriceType=2 => Current day preview settle
            // After  session OpenCloseSettlFlag=1 SettlPriceType=1 => Current day final settle

            if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntryPx))
            {
                Decimal precoAjuste = mdEntry.GetDecimal(QuickFix.Fields.Tags.MDEntryPx);


                // Estaremos considerando que os valores altos, quando forem maiores que o valor de fechamento + 1000, sejam os
                // precos unitarios (P.U.) de alguns contratos da BMF (como DI1..., DDI...).
                // Como não tem nenhuma flag que identifique essa situação, foi necessário improvisar esse tratamento
                // ATP 2015-05-05:
                // Na verdade, ha uma flag, a PriceType
                // PriceType=1 - MDEntryPx em porcentagem 
                // PriceType=2 - MDEntryPx em preco unitario
                // O problema eh separar os ativos que sao negociados em taxa em lugar de preco.
                // Esta regra esta correta, mas nao eh elegante
                if (precoAjuste < registroNegocios.Negocio.PrecoFechamento + 1000) // || registroNegocios.Negocio.PrecoFechamento == 0)
                {
                    registroNegocios.Negocio.PrecoAjuste = precoAjuste;
                    registroNegocios.Negocio.MsgIdAnterior = numSeq;

                    if (!String.IsNullOrEmpty(settlPriceType))
                    {
                        if (settlPriceType.Equals("1"))
                        {
                            registroNegocios.Negocio.PrecoFechamento = precoAjuste;
                            logger.InfoFormat("Instrumento[{0}] sequencial[{1}] precoAjuste[{2}] openCloseSettlFlag[{3}] settlPriceType[{4}] PriceType[{5}] setando para fechamento",
                            instrumento, numSeq, precoAjuste, openCloseSettlFlag, settlPriceType, priceType);
                        }
                    }

                    if (bGerarEventoANG)
                    {
                        ThreadPool.QueueUserWorkItem(
                            new WaitCallback(
                                delegate(object required)
                                {
                                    EventoNegocioANG eventoANG = new EventoNegocioANG();
                                    eventoANG.instrumento = instrumento;
                                    eventoANG.mensagem = NegocioBase.montarPrecoAjuste(registroNegocios);
                                    EventQueueManager.Instance.SendEvent(eventoANG);
                                }
                            )
                        );
                    }

                    logger.InfoFormat("Instrumento[{0}] sequencial[{1}] precoAjuste[{2}] openCloseSettlFlag[{3}]",
                        instrumento, numSeq, precoAjuste, openCloseSettlFlag);

                    lock (dctNegocios[instrumento])
                    {
                        dctNegocios[instrumento] = registroNegocios;
                    }
                }
                else //if (precoAjuste >= registroNegocios.Negocio.PrecoFechamento + 1000)
                {
                    registroNegocios.Negocio.PrecoUnitario = precoAjuste;
                    registroNegocios.Negocio.MsgIdAnterior = numSeq;

                    if (bGerarEventoANG)
                    {
                        ThreadPool.QueueUserWorkItem(
                            new WaitCallback(
                                delegate(object required)
                                {
                                    EventoNegocioANG eventoANG = new EventoNegocioANG();
                                    eventoANG.instrumento = instrumento;
                                    eventoANG.mensagem = NegocioBase.montarPrecoUnitario(registroNegocios);
                                    EventQueueManager.Instance.SendEvent(eventoANG);
                                }
                            )
                        );
                    }

                    logger.InfoFormat("Instrumento[{0}] sequencial[{1}] precoUnitario[{2}] openCloseSettlFlag[{3}]",
                        instrumento, numSeq, precoAjuste, openCloseSettlFlag);

                    lock (dctNegocios[instrumento])
                    {
                        dctNegocios[instrumento] = registroNegocios;
                    }
                }
            }
            return;
        }

        private void tratarMensagemMelhorOfertaFix(string instrumento, string msgID, QuickFix.Group mdEntry)
        {
            // Despreza mensagens de MDUpdateAction = delete
            if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDUpdateAction))
            {
                if (mdEntry.GetString(QuickFix.Fields.Tags.MDUpdateAction).Equals(ConstantesUMDF.UMDF_MD_UPDT_ACTION_DELETE))
                    return;
            }

            NegocioBase registroNegocios = dctNegocios[instrumento];

            string mdEntryType = mdEntry.GetString(QuickFix.Fields.Tags.MDEntryType);

            Decimal melhorPreco = Decimal.Zero;
            if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntryPx))
            {
                melhorPreco = mdEntry.GetDecimal(QuickFix.Fields.Tags.MDEntryPx);
            }

            long quantidadeSemFormato = 0;
            if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntrySize))
                quantidadeSemFormato = Convert.ToInt64(mdEntry.GetString(QuickFix.Fields.Tags.MDEntrySize));

            List<HashEntry> lstHashes = new List<HashEntry>();

            if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_BID))
            {
                registroNegocios.Negocio.MelhorPrecoCompra = melhorPreco;
                registroNegocios.Negocio.MelhorQuantidadeCompra = quantidadeSemFormato;

                lstHashes.Add(new HashEntry("MelhorPrecoCompra", melhorPreco.ToString()));
                lstHashes.Add(new HashEntry("MelhorQuantidadeCompra", quantidadeSemFormato.ToString()));

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Instrumento[" + instrumento +
                            "]: Melhor Compra Preco[" + melhorPreco +
                            "] Quantidade[" + quantidadeSemFormato + "]");
                }
            }
            else
            {
                registroNegocios.Negocio.MelhorPrecoVenda = melhorPreco;
                registroNegocios.Negocio.MelhorQuantidadeVenda = quantidadeSemFormato;

                lstHashes.Add(new HashEntry("MelhorPrecoVenda", melhorPreco.ToString()));
                lstHashes.Add(new HashEntry("MelhorQuantidadeVenda", quantidadeSemFormato.ToString()));

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Instrumento[" + instrumento +
                            "]: Melhor Venda Preco[" + melhorPreco +
                            "] Quantidade[" + quantidadeSemFormato + "]");
                }
            }

            if (redisDB != null && redis.IsConnected)
                redisDB.HashSet(instrumento, lstHashes.ToArray());

            lock (dctNegocios[instrumento])
            {
                dctNegocios[instrumento] = registroNegocios;
            }

            return;
        }

        private void tratarMensagemImbalance(string instrumento, string msgID, QuickFix.FIX44.Message message, QuickFix.Group mdEntry, bool isPuma)
        {
            long qtderemanescente = 0;
            NegocioBase registroNegocios = dctNegocios[instrumento];

            bool moreBuyers = true;
            if (mdEntry.IsSetField(QuickFix.Fields.Tags.TradeCondition))
            {
                if (mdEntry.GetString(QuickFix.Fields.Tags.TradeCondition).Equals("Q"))
                {
                    moreBuyers = false;
                }
            }

            if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntrySize))
            {
                qtderemanescente = mdEntry.GetInt(QuickFix.Fields.Tags.MDEntrySize);
                long qtdeLeilao = registroNegocios.Negocio.Quantidade;

                if (moreBuyers)
                {
                    registroNegocios.Negocio.MelhorQuantidadeVenda = qtdeLeilao;
                    registroNegocios.Negocio.MelhorQuantidadeCompra = qtdeLeilao + qtderemanescente;
                }
                else
                {
                    registroNegocios.Negocio.MelhorQuantidadeCompra = qtdeLeilao;
                    registroNegocios.Negocio.MelhorQuantidadeVenda = qtdeLeilao + qtderemanescente;
                }

                registroNegocios.Negocio.MelhorPrecoVenda = registroNegocios.Negocio.PrecoTeoricoAbertura;
                registroNegocios.Negocio.MelhorPrecoCompra = registroNegocios.Negocio.PrecoTeoricoAbertura;
            }

            lock (dctNegocios[instrumento])
            {
                dctNegocios[instrumento] = registroNegocios;
            }
        }

        private void atualizaCadastroBasicoFIX(string instrumento, QuickFix.FIX44.Message message, string tipoBolsa)
        {
            // Se ocorreu atualizacao do cadastro basico de um instrumento no mesmo segundo, ignorar
            if (dctNegocios[instrumento] != null &&
                dctNegocios[instrumento].DataAtualizacao.ToString("yyyyMMddHHmmss").Equals(DateTime.Now.ToString("yyyyMMddHHmmss")))
                return;

            QuickFix.Group relatedSym = message.GetGroup(1, QuickFix.Fields.Tags.NoRelatedSym);

            string grupoCotacao = (relatedSym.IsSetField(QuickFix.Fields.Tags.SecurityGroup) ? relatedSym.GetString(QuickFix.Fields.Tags.SecurityGroup) : "");
            logger.InfoFormat("Instrumento[{0}]: GrupoCotacao[{1}]", instrumento, grupoCotacao);

            string empresa = (relatedSym.IsSetField(QuickFix.Fields.Tags.SecurityDesc) ? relatedSym.GetString(QuickFix.Fields.Tags.SecurityDesc) : "");
            logger.InfoFormat("Instrumento[{0}]: Empresa[{1}]", instrumento, empresa);

            string securityType = (relatedSym.IsSetField(QuickFix.Fields.Tags.SecurityType) ? relatedSym.GetString(QuickFix.Fields.Tags.SecurityType) : "");
            string product = (relatedSym.IsSetField(QuickFix.Fields.Tags.Product) ? relatedSym.GetString(QuickFix.Fields.Tags.Product) : "");
            string cfiCode = (relatedSym.IsSetField(QuickFix.Fields.Tags.CFICode) ? relatedSym.GetString(QuickFix.Fields.Tags.CFICode) : "");
            string securitySubType = (relatedSym.IsSetField(QuickFix.Fields.Tags.SecuritySubType) ? relatedSym.GetString(QuickFix.Fields.Tags.SecuritySubType) : "");

            logger.InfoFormat("CONFLATED Instrumento[{0}] Product [{1}] SecurityType[{2}] SecuritySubType[{3}] SecurityGroup[{4}]",
                instrumento,
                product,
                securityType,
                securitySubType,
                grupoCotacao);

            string segmentoMercado = (relatedSym.IsSetField(QuickFix.Fields.Tags.SecurityType) ? relatedSym.GetString(QuickFix.Fields.Tags.SecurityType) : "");
            if (segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_VISTA) ||
                segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_ACOES_ORDINARIAS) ||
                segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_ACOES_PREFERENCIAS) ||
                segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_CORPORATE_FIXED_INCOME))
                segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_VISTA;
            else if (segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_TERMO))
                segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_TERMO;
            else if (segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_OPCOES) ||
                     segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_OPCOES_INDICE))
                segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_OPCOES;
            else if (segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_EXERCICIO_OPCOES))
                segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_EXERCICIO_DE_OPCOES;
            else if (segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_INDICE))
                segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_INDICES;
            else if (segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_ETF))
                segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_ETF;
            else if (segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_MULTILEG) ||
                     segmentoMercado.Equals(ConstantesUMDF.UMDF20_SECURITY_TYPE_BOVESPA_EMPRESTIMO_OU_BTC))
                segmentoMercado = ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_RESERVADO;
            logger.InfoFormat("Instrumento[{0}]: SegmentoMercado[{1}]", instrumento, segmentoMercado);

            int formaCotacao = 1;
            if (!segmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_INDICES))
            {
                if (relatedSym.IsSetField(QuickFix.Fields.Tags.PriceDivisor))
                {
                    int divisor = relatedSym.GetInt(QuickFix.Fields.Tags.PriceDivisor);
                    if (divisor == 100)
                        formaCotacao = 3;
                    else if (divisor == 1000)
                        formaCotacao = 4;
                    else if (divisor == 10000)
                        formaCotacao = 5;
                }
            }
            logger.InfoFormat("Instrumento[{0}]: FormaCotacao[{1}]", instrumento, formaCotacao);

            Decimal coeficienteMultiplicacao = (relatedSym.IsSetField(QuickFix.Fields.Tags.ContractMultiplier) ? relatedSym.GetDecimal(QuickFix.Fields.Tags.ContractMultiplier) : 1);
            logger.InfoFormat("Instrumento[{0}]: CoeficienteMultiplicacao[{1}]", instrumento, coeficienteMultiplicacao);

            string securityIDSource = "";
            string codigoPapelObjeto = instrumento;
            if (relatedSym.GroupCount(QuickFix.Fields.Tags.NoUnderlyings) > 0)
            {
                // TODO: talvez tenha que ter um loop aqui...
                QuickFix.Group underlyings = relatedSym.GetGroup(1, QuickFix.Fields.Tags.NoUnderlyings);
                securityIDSource = (underlyings.IsSetField(QuickFix.Fields.Tags.UnderlyingSecurityIDSource) ? underlyings.GetString(QuickFix.Fields.Tags.UnderlyingSecurityIDSource) : "");

                if (tipoBolsa.Equals(ConstantesMDS.DESCRICAO_DE_BOLSA_BMF))
                {
                    // BM&F
                    if (relatedSym.IsSetField(QuickFix.Fields.Tags.Asset))
                        codigoPapelObjeto = relatedSym.GetString(QuickFix.Fields.Tags.Asset);
                }
                else
                {
                    // Bovespa
                    if (underlyings.IsSetField(QuickFix.Fields.Tags.UnderlyingSymbol))
                        codigoPapelObjeto = underlyings.GetString(QuickFix.Fields.Tags.UnderlyingSymbol);
                }
            }
            logger.InfoFormat("Instrumento[{0}]: CodigoPapelObjeto[{1}]", instrumento, codigoPapelObjeto);
            logger.InfoFormat("Instrumento[{0}]: SecurityIDSource[{1}]", instrumento, securityIDSource);

            string codigoISIN = "";
            if (relatedSym.GroupCount(QuickFix.Fields.Tags.NoSecurityAltID) > 0)
            {
                Dictionary<string, string> securityAltIDValues = new Dictionary<string, string>();

                for (int j = 1; j <= relatedSym.GroupCount(QuickFix.Fields.Tags.NoSecurityAltID); j++)
                {
                    QuickFix.Group securityAltID = relatedSym.GetGroup(j, QuickFix.Fields.Tags.NoSecurityAltID);
                    if (securityAltID.IsSetField(QuickFix.Fields.Tags.SecurityAltID) && securityAltID.IsSetField(QuickFix.Fields.Tags.SecurityAltIDSource))
                        securityAltIDValues.Add(securityAltID.GetString(QuickFix.Fields.Tags.SecurityAltIDSource), securityAltID.GetString(QuickFix.Fields.Tags.SecurityAltID));
                }

                if (tipoBolsa.Equals(ConstantesMDS.DESCRICAO_DE_BOLSA_BMF))
                {
                    if (securityAltIDValues.ContainsKey("8"))
                        codigoISIN = securityAltIDValues["8"];
                    else
                        codigoISIN = (dctNegocios.ContainsKey(codigoPapelObjeto) ? dctNegocios[codigoPapelObjeto].CodigoISIN : "");
                }
                else
                {
                    if (securityAltIDValues.ContainsKey("4"))
                        codigoISIN = securityAltIDValues["4"];
                    else
                        codigoISIN = (dctNegocios.ContainsKey(codigoPapelObjeto) ? dctNegocios[codigoPapelObjeto].CodigoISIN : "");
                }
            }
            logger.InfoFormat("Instrumento[{0}]: CodigoISIN[{1}]", instrumento, codigoISIN);

            // TODO: Mudou o tratamento de LotePadrao para UMDF 2.0, pois tem o grupo 'Lots', precisa verificar como ficou no conflated...
            //int lotePadrao = (relatedSym.IsSetField(QuickFix.Fields.Tags.RoundLot) ? relatedSym.GetInt(QuickFix.Fields.Tags.RoundLot) : 0);
            //logger.InfoFormat("Instrumento[{0}]: LotePadrao[{1}]", instrumento, lotePadrao);

            //ATP 2015-12-02
            int lotePadrao = 0;
            if (relatedSym.IsSetField(QuickFix.Fields.Tags.RoundLot))
                lotePadrao = relatedSym.GetInt(QuickFix.Fields.Tags.RoundLot);
            else if (relatedSym.GroupCount(QuickFix.Fields.Tags.NoLotTypeRules) > 0 )
            {
                Dictionary<string, int> lotValues = new Dictionary<string, int>();
                for (int j = 1; j <= relatedSym.GroupCount(QuickFix.Fields.Tags.NoLotTypeRules); j++)
                {
                    QuickFix.Group lot = relatedSym.GetGroup(j, QuickFix.Fields.Tags.NoLotTypeRules);

                    if (lot.IsSetField(QuickFix.Fields.Tags.LotType) && 
                        lot.IsSetField(QuickFix.Fields.Tags.MinLotSize)  )
                        lotValues.Add(lot.GetString(QuickFix.Fields.Tags.LotType), lot.GetInt(QuickFix.Fields.Tags.MinLotSize));
                }
                if (lotValues.ContainsKey("1"))
                    lotePadrao = lotValues["1"];
                else if (lotValues.ContainsKey("2"))
                    lotePadrao = lotValues["2"];
            }
            logger.InfoFormat("Instrumento[{0}]: LotePadrao[{1}]", instrumento, lotePadrao);

            int loteMinimo = (relatedSym.IsSetField(QuickFix.Fields.Tags.MinOrderQty) ? relatedSym.GetInt(QuickFix.Fields.Tags.MinOrderQty) : 0);
            logger.InfoFormat("Instrumento[{0}]: LoteMinimo[{1}]", instrumento, loteMinimo);

            string dataVencimento = (relatedSym.IsSetField(QuickFix.Fields.Tags.MaturityDate) ? relatedSym.GetString(QuickFix.Fields.Tags.MaturityDate) : "00010101");
            if (dataVencimento.Equals("99991231"))
                dataVencimento = "00010101";
            logger.InfoFormat("Instrumento[{0}]: DataVencimento[{1}]", instrumento, dataVencimento);

            Decimal precoExercicio = (relatedSym.IsSetField(QuickFix.Fields.Tags.StrikePrice) ? relatedSym.GetDecimal(QuickFix.Fields.Tags.StrikePrice) : Decimal.Zero);
            logger.InfoFormat("Instrumento[{0}]: precoExercicio[{1}]", instrumento, precoExercicio);

            string indicadorOpcao = (relatedSym.IsSetField(QuickFix.Fields.Tags.PutOrCall) ? relatedSym.GetString(QuickFix.Fields.Tags.PutOrCall) : " ");
            if (indicadorOpcao.Equals("1"))
                indicadorOpcao = ConstantesMDS.INDICADOR_OPCAO_BOVESPA_OPCAO_COMPRA;
            else if (indicadorOpcao.Equals("0"))
                indicadorOpcao = ConstantesMDS.INDICADOR_OPCAO_BOVESPA_OPCAO_VENDA;
            logger.InfoFormat("Instrumento[{0}]: IndicadorOpcao[{1}]", instrumento, indicadorOpcao);

            int casasDecimais = (relatedSym.IsSetField(QuickFix.Fields.Tags.TickSizeDenominator) ? relatedSym.GetInt(QuickFix.Fields.Tags.TickSizeDenominator) : 2);
            logger.InfoFormat("Instrumento[{0}]: CasasDecimais[{1}]", instrumento, casasDecimais);

            NegocioBase registroNegocio = dctNegocios[instrumento];
            if (registroNegocio != null)
            {
                string dataHoraUltimoNegocio = "00000000000000";

                if (dctUltimoNeg.ContainsKey(instrumento))
                    dataHoraUltimoNegocio = dctUltimoNeg[instrumento];

                logger.InfoFormat("Instrumento[{0}]: dataHoraUltimoNegocio[{1}]", instrumento, dataHoraUltimoNegocio);

                registroNegocio.DataAtualizacao = DateTime.Now;
                registroNegocio.Negocio.Data = dataHoraUltimoNegocio.Substring(0, 8);
                registroNegocio.Negocio.Hora = dataHoraUltimoNegocio.Substring(8, 6);
                registroNegocio.Negocio.HorarioTeorico = DateTime.MinValue;
                registroNegocio.FormaCotacao = formaCotacao;
                registroNegocio.GrupoCotacao = grupoCotacao;
                registroNegocio.Especificacao = empresa;
                registroNegocio.SegmentoMercado = segmentoMercado;
                registroNegocio.CoeficienteMultiplicacao = coeficienteMultiplicacao;
                registroNegocio.CodigoPapelObjeto = codigoPapelObjeto;
                registroNegocio.CodigoISIN = codigoISIN;
                registroNegocio.LotePadrao = lotePadrao;
                registroNegocio.DataVencimento = dataVencimento;
                registroNegocio.PrecoExercicio = precoExercicio;
                registroNegocio.IndicadorOpcao = indicadorOpcao;
                registroNegocio.CasasDecimais = casasDecimais;
                registroNegocio.SecurityIDSource = securityIDSource;
                registroNegocio.FaseNegociacao = ConstantesMDS.FASE_NEGOCIACAO_ATUALIZAR_ANALISE_GRAFICA;

                // Se for Instrumento Bovespa a Vista, Termo ou Opcao, acrescenta no IBOV_TOTAL na lista de indices para contabilizacao
                if (registroNegocio.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_VISTA) ||
                    registroNegocio.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_TERMO) ||
                    registroNegocio.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_OPCOES) ||
                    registroNegocio.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_EXERCICIO_DE_OPCOES))
                {
                    if (!dctComposicaoIndice.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_TOTAL))
                        dctComposicaoIndice.Add(ConstantesMDS.INDICE_IBOVESPA_TOTAL, new List<string>());

                    if (!dctComposicaoIndice[ConstantesMDS.INDICE_IBOVESPA_TOTAL].Contains(instrumento))
                        dctComposicaoIndice[ConstantesMDS.INDICE_IBOVESPA_TOTAL].Add(instrumento);
                }

                // Se for Instrumento Bovespa a Vista, acrescenta no IBOV_VISTA na lista de indices para contabilizacao
                if (registroNegocio.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_VISTA))
                {
                    if (!dctComposicaoIndice.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_VISTA))
                        dctComposicaoIndice.Add(ConstantesMDS.INDICE_IBOVESPA_VISTA, new List<string>());

                    if (!dctComposicaoIndice[ConstantesMDS.INDICE_IBOVESPA_VISTA].Contains(instrumento))
                        dctComposicaoIndice[ConstantesMDS.INDICE_IBOVESPA_VISTA].Add(instrumento);
                }

                // Se for Instrumento Bovespa Termo, acrescenta no IBOV_TERMO na lista de indices para contabilizacao
                if (registroNegocio.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_TERMO))
                {
                    if (!dctComposicaoIndice.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_TERMO))
                        dctComposicaoIndice.Add(ConstantesMDS.INDICE_IBOVESPA_TERMO, new List<string>());

                    if (!dctComposicaoIndice[ConstantesMDS.INDICE_IBOVESPA_TERMO].Contains(instrumento))
                        dctComposicaoIndice[ConstantesMDS.INDICE_IBOVESPA_TERMO].Add(instrumento);
                }

                // Se for Instrumento Bovespa Opcao, acrescenta no IBOV_OPCAO na lista de indices para contabilizacao
                if (registroNegocio.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_OPCOES) ||
                    registroNegocio.SegmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_EXERCICIO_DE_OPCOES))
                {
                    if (!dctComposicaoIndice.ContainsKey(ConstantesMDS.INDICE_IBOVESPA_OPCOES))
                        dctComposicaoIndice.Add(ConstantesMDS.INDICE_IBOVESPA_OPCOES, new List<string>());

                    if (!dctComposicaoIndice[ConstantesMDS.INDICE_IBOVESPA_OPCOES].Contains(instrumento))
                        dctComposicaoIndice[ConstantesMDS.INDICE_IBOVESPA_OPCOES].Add(instrumento);
                }

                if (bGerarEventoANG)
                {
                    EventoNegocioANG eventoANG = new EventoNegocioANG();
                    eventoANG.instrumento = instrumento;
                    eventoANG.mensagem = NegocioBase.montarCadastroBasico(registroNegocio);
                    if (!eventoANG.mensagem.Equals(""))
                        EventQueueManager.Instance.SendEvent(eventoANG);
                }
            }

            if (segmentoMercado.Equals(ConstantesMDS.SEGMENTO_MERCADO_BOVESPA_INDICES) && relatedSym.GroupCount(QuickFix.Fields.Tags.NoUnderlyings) > 0)
                atualizaComposicaoIndiceFIX(instrumento, relatedSym);

            return;
        }

        private void atualizaComposicaoIndiceFIX(string indice, QuickFix.Group relatedSym)
        {
            logger.InfoFormat("Indice[{0}]: Obtendo a lista de todos os papeis atrelados", indice);

            if (!dctComposicaoIndice.ContainsKey(indice))
            {
                dctComposicaoIndice.Add(indice, new List<string>());
            }

            int noUnderlyings = relatedSym.GroupCount(QuickFix.Fields.Tags.NoUnderlyings);
            if (noUnderlyings > 0)
            {
                for (int j = 1; j <= noUnderlyings; j++)
                {
                    QuickFix.Group underlying = relatedSym.GetGroup(j, QuickFix.Fields.Tags.NoUnderlyings);
                    if (underlying.IsSetField(QuickFix.Fields.Tags.UnderlyingSymbol))
                    {
                        string item = underlying.GetString(QuickFix.Fields.Tags.UnderlyingSymbol);

                        logger.InfoFormat("Indice[{0}]: Composicao [{1}]", indice, item);
                        if (!dctComposicaoIndice[indice].Contains(item))
                            dctComposicaoIndice[indice].Add(item);
                    }
                }

                if ( bGerarEventoANG )
                {
                    ThreadPool.QueueUserWorkItem(
                        new WaitCallback(
                            delegate(object required)
                            {
                                EventoNegocioANG eventoANG = new EventoNegocioANG();
                                eventoANG.instrumento = indice;
                                eventoANG.mensagem = NegocioBase.montarComposicaoIndice(indice, dctComposicaoIndice[indice]);
                                EventQueueManager.Instance.SendEvent(eventoANG);
                            }
                        )
                    );
                }
            }
        }


        private string processaMensagemSecurityListFIX(string msgID, QuickFix.FIX44.Message message)
        {
            QuickFix.Group relatedSym = message.GetGroup( 1, QuickFix.Fields.Tags.NoRelatedSym);

            String instrumento = relatedSym.GetString(QuickFix.Fields.Tags.Symbol);
            String securityID = relatedSym.GetString(QuickFix.Fields.Tags.SecurityID);

            logger.InfoFormat("FIX: Instrumento[{0}]: SecurityID[{1}]", instrumento, securityID);
            dicSecurityID.AddOrUpdate(securityID, instrumento, (key, oldValue) => instrumento);

            return instrumento;
        }

        #endregion //FIXConflated

        private void buscarDataHoraUltNegProc()
        {
            logger.Info("Iniciando thread de busca da data e hora do ultimo negocio");

            Dictionary<string, string> dctUltimoNeg = dbLib.obterDataUltimoNegocioTodosAtivos();

            logger.Info("Carregou data/hora do ultimo negocio para [" + dctUltimoNeg.Count + "] ativos");

            string datazerada = DateTime.MinValue.ToString("yyyyMMdd");
            string horazerada = DateTime.MinValue.ToString("HHmmss");

            while (bKeepRunning)
            {
                try
                {
                    string instrumento = null;
                    if (queueObterDataHoraUltNeg.TryDequeue(out instrumento))
                    {
                        NegocioBase registroNegocio = null;
                        if (dctNegocios.TryGetValue(instrumento, out registroNegocio))
                        {
                            if (registroNegocio.Negocio.Data.Equals(datazerada) &&
                                registroNegocio.Negocio.Hora.Equals(horazerada)
                                && dctUltimoNeg.ContainsKey(instrumento) )
                            {
                                string dataHoraUltimoNegocio = dctUltimoNeg[instrumento];
                                logger.InfoFormat("Instrumento[{0}]: dataHoraUltimoNegocio[{1}]", instrumento, dataHoraUltimoNegocio);

                                registroNegocio.DataAtualizacao = DateTime.Now;
                                registroNegocio.Negocio.Data = dataHoraUltimoNegocio.Substring(0, 8);
                                registroNegocio.Negocio.Hora = dataHoraUltimoNegocio.Substring(8, 6);
                            }
                        }

                        continue;
                    }

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    logger.Error("buscarDataHoraUltNegProc()" + ex.Message, ex);
                }
            }

            logger.Info("Finalizando thread de busca da data e hora do ultimo negocio");

        }

    }
}
