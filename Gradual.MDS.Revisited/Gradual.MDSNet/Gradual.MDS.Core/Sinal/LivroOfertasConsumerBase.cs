using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.MDS.Eventos.Lib;
using log4net;
using System.Threading;
using Gradual.MDS.Core.Lib;
using OpenFAST;
using OpenFAST.Template;
using System.Configuration;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Gradual.MDS.Core.Sinal
{

    public class LivroOfertasConsumerBase : UmdfEventConsumerBase
    {
        private Dictionary<string, LivroOfertasBase> livrosOfertas = new Dictionary<string, LivroOfertasBase>();
        private ConcurrentDictionary<string, string> dicSecurityID = new ConcurrentDictionary<string, string>();
        private Dictionary<string, ChannelUDMF> dicCanais;
        private SinalLastTimestamp timestampControl = new SinalLastTimestamp();
        private long hbNotSentInterval = 30000;

        private class TopoLivroWorker
        {
            public Message Mensagem { get; set; }
            public Decimal TopBPreco { get; set; }
            public long TopBQtde { get; set; }
            public string ChannelID { get; set; }

            public TopoLivroWorker(Message fastMsg, Decimal preco, long quantidade, String channelID)
            {
                this.ChannelID = channelID;
                this.Mensagem = fastMsg;
                this.TopBPreco = preco;
                this.TopBQtde = quantidade;
            }
        }

        private class TopoLivroWorkerFix
        {
            public QuickFix.FIX44.Message Mensagem { get; set; }
            public QuickFix.Group MDEntry { get; set; }
            public Decimal TopBPreco { get; set; }
            public long TopBQtde { get; set; }
            public string ChannelID { get; set; }

            public TopoLivroWorkerFix(QuickFix.FIX44.Message fixmsg, QuickFix.Group mdentry, Decimal preco, long quantidade, String channelID)
            {
                this.ChannelID = channelID;
                this.Mensagem = fixmsg;
                this.MDEntry = mdentry;
                this.TopBPreco = preco;
                this.TopBQtde = quantidade;
            }
        }

        private int maxThreadGeraTopo = 25;
        private int currentTopBWorker = 0;
        private Thread[] thGeraTopo;
        private ConcurrentQueue<TopoLivroWorker> [] queuesGeraTopo;
        private Object[] syncQueuesGeraTopo;

        private int currentTopBWorkerFix = 0;
        private Thread[] thGeraTopoFix;
        private ConcurrentQueue<TopoLivroWorkerFix>[] queuesGeraTopoFix;
        private Object[] syncQueuesGeraTopoFix;

        private bool bGerarEventoStreamer = true;
        private bool bGerarEventoHB = true;
        private bool bGerarEventoANG = true;

        #region ctor
        public LivroOfertasConsumerBase(Dictionary<string, ChannelUDMF> dicCanais)
        {
            this.dicCanais = dicCanais;
            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            myThreadName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
            MDSUtils.AddAppender("LivroOfertasConsumerBase-", logger.Logger);
            if (ConfigurationManager.AppSettings["HBNotSentInterval"] != null)
            {
                hbNotSentInterval = Convert.ToInt64(ConfigurationManager.AppSettings["HBNotSentInterval"].ToString());
            }

            if (ConfigurationManager.AppSettings["GeraTopoLivroThreadPoolSize"] != null)
            {
                maxThreadGeraTopo = Convert.ToInt32(ConfigurationManager.AppSettings["GeraTopoLivroThreadPoolSize"].ToString());
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
        }
        #endregion ctor

        #region overrides

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evento"></param>
        protected override void trataEventoUmdf(EventoUmdf evento)
        {
            try
            {
                LivroOfertasBase livroInstrumento;

                String msgType = evento.MsgType;
                OpenFAST.Message umdfMessage = evento.MsgBody;
                String msgID = evento.MsgSeqNum.ToString();
                String channelId = evento.ChannelID;

                ChannelUDMF config = null;
                dicCanais.TryGetValue(channelId, out config);
                bool isPuma = config.channelConfig.IsPuma;
                bool isBMF = (config.channelConfig.Segment.ToUpper().Equals(ConstantesMDS.CHANNEL_UMDF_SEGMENT_BMF) ? true : false);
                bool isPuma20 = config.channelConfig.IsPuma20;

                int marketDepth = evento.MarketDepth;

                //logger.InfoFormat("trataEventoUmdf mensagem UMDF [{0}]", evento.MsgSeqNum);

                if (logger.IsDebugEnabled)
                {
                    logger.DebugFormat("channelID ....: {0} (isPuma={1})", channelId, isPuma.ToString());
                    logger.DebugFormat("msgID  .......: {0}", msgID);
                    logger.DebugFormat("Message ......: {0}", umdfMessage.ToString());
                }

                Dictionary<string, List<Dictionary<string, string>>> streamerSinalLOF = new Dictionary<string, List<Dictionary<string, string>>>();
                Dictionary<string, List<Dictionary<string, string>>> streamerSinalLOA = new Dictionary<string, List<Dictionary<string, string>>>();

                // Trata mensagens de Security List
				if ( msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_SECURITYLIST_SINGLE) )
				{
                    if (logger.IsDebugEnabled)
                    {
                        if (umdfMessage.Template.HasField("RelatedSymbols") && umdfMessage.IsDefined("RelatedSymbols"))
                            logger.Debug(UmdfUtils.writeGroup(umdfMessage.GetGroup("RelatedSymbols")));
                        else
                            logger.Debug(UmdfUtils.writeGroup(umdfMessage.GetGroup("RelatedSym")));
                    }

                    livroInstrumento = atualizaLivroOfertas(msgType, umdfMessage, evento.StreamType);
                    if (livroInstrumento == null)
                        return;

                    livroInstrumento.CasasDecimais = obtemCasasDecimais(umdfMessage);

                    if (evento.UmdfSegment.Equals(ConstantesUMDF.UMDF_SEGMENT_DERIVATIVES))
                        livroInstrumento.TipoBolsa = ConstantesMDS.DESCRICAO_DE_BOLSA_BMF;
                    else
                        livroInstrumento.TipoBolsa = ConstantesMDS.DESCRICAO_DE_BOLSA_BOVESPA;

                    logger.InfoFormat("Instrumento[{0}]: Inicializado TipoBolsa[{1}] CasasDecimais[{2}]",
                        livroInstrumento.Instrumento, livroInstrumento.TipoBolsa, livroInstrumento.CasasDecimais);

                    List<Dictionary<String, String>> ofertasLOA = new List<Dictionary<String, String>>();
                    List<Dictionary<String, String>> ofertasLOF = new List<Dictionary<String, String>>();

                    //resetLivroOfertas(LivroOfertasBase.LIVRO_COMPRA, livroInstrumento, ofertasLOA, ofertasLOF);
                    streamerSinalLOA.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_COMPRA, ofertasLOA);
                    streamerSinalLOF.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_COMPRA, ofertasLOF);

                    ofertasLOA = new List<Dictionary<String, String>>();
                    ofertasLOF = new List<Dictionary<String, String>>();
                    //resetLivroOfertas(LivroOfertasBase.LIVRO_VENDA, livroInstrumento, ofertasLOA, ofertasLOF);
                    streamerSinalLOA.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_VENDA, ofertasLOA);
                    streamerSinalLOF.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_VENDA, ofertasLOF);

                    //enviaSinalStreamer(livroInstrumento.Instrumento, streamerSinalLOF, streamerSinalLOA);

                    EventoHttpLivroOfertas eventoLOF = new EventoHttpLivroOfertas();

                    eventoLOF.cabecalho = MDSUtils.montaCabecalhoStreamer(
                        ConstantesMDS.TIPO_REQUISICAO_LIVRO_OFERTAS,
                        livrosOfertas[livroInstrumento.Instrumento].TipoBolsa,
                        ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_COMPLETO,
                        livroInstrumento.Instrumento,
                        livroInstrumento.CasasDecimais,
                        null);
                    eventoLOF.instrumento = livroInstrumento.Instrumento;

                    List<Dictionary<string, string>> livrostreamer = null;
                    if ( streamerSinalLOF.TryGetValue(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_COMPRA, out livrostreamer) )
                        eventoLOF.livroCompra = livrostreamer;

                    livrostreamer = null;
                    if (streamerSinalLOF.TryGetValue(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_VENDA, out livrostreamer))
                        eventoLOF.livroVenda = livrostreamer;

                    EventQueueManager.Instance.SendEvent(eventoLOF);

					return;
				}

                if (msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_SECURITYSTATUS))
                {
                    return;
                }

                // Trata apenas as mensagens de ofertas
                GroupValue mdEntry = umdfMessage.GetGroup("MDEntries");
                String mdEntryType = mdEntry.GetString("MDEntryType");
                if (msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_MELHOR_OFERTA) ||
                    (!mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_BID) &&
                    !mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_OFFER) &&
                    !mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_EMPTY_BOOK)))
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug("Descartando msg, nao eh msg de oferta/livro");
                    }
                    return;
                }

                // Testa se debug esta habilitado para nao invocar writegroup (+performatico)
                if (logger.IsDebugEnabled)
                {
                    logger.Debug(UmdfUtils.writeGroup(mdEntry));
                }

                livroInstrumento = atualizaLivroOfertas(msgType, umdfMessage, evento.StreamType);
                if (livroInstrumento == null)
                    return;

                string mdUpdateAction;
                if (mdEntry.IsDefined("MDUpdateAction"))
                    mdUpdateAction = mdEntry.GetString("MDUpdateAction");
                else
                    mdUpdateAction = ConstantesMDS.TIPO_ACAO_LOF_INCLUIR;

                bool sunda = false;
                //if (isPuma)
                if (sunda)
                {
                    lock (livrosOfertas[livroInstrumento.Instrumento])
                    {
                        // Trata ofertas do Canal "Ofertas por Ordem"
                        if (marketDepth == ConstantesUMDF.UMDF_MARKETDEPTH_MARKET_BY_ORDER)
                        {
                            LOFDadosOferta dadosOfertaLOF = 
                            preparaDadosOfertaLOF(livroInstrumento, mdEntryType, mdUpdateAction, mdEntry, isPuma20);

                            if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_BID))
                            {
                                contabilizaOfertasCompraOuVendaLOFPuma(
                                    LivroOfertasBase.LIVRO_COMPRA,
                                    livroInstrumento,
                                    mdEntryType,
                                    mdUpdateAction,
                                    mdEntry,
                                    dadosOfertaLOF,
                                    streamerSinalLOF);
                            }

                            else if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_OFFER))
                            {
                                contabilizaOfertasCompraOuVendaLOFPuma(
                                    LivroOfertasBase.LIVRO_VENDA,
                                    livroInstrumento,
                                    mdEntryType,
                                    mdUpdateAction,
                                    mdEntry,
                                    dadosOfertaLOF,
                                    streamerSinalLOF);
                            }

                            else if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_EMPTY_BOOK))
                            {
                                List<Dictionary<String, String>> ofertasLOF = new List<Dictionary<String, String>>();

                                resetLivroOfertasLOF(LivroOfertasBase.LIVRO_COMPRA, livroInstrumento, ofertasLOF);
                                streamerSinalLOF.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_COMPRA, ofertasLOF);

                                ofertasLOF = new List<Dictionary<String, String>>();
                                resetLivroOfertasLOF(LivroOfertasBase.LIVRO_VENDA, livroInstrumento, ofertasLOF);
                                streamerSinalLOF.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_VENDA, ofertasLOF);
                            }
                        }

                        // Trata ofertas do Canal "Ofertas por Preco"
                        else if (marketDepth == ConstantesUMDF.UMDF_MARKETDEPTH_MARKET_BY_PRICE)
                        {
                            LOAGrupoOfertas dadosOfertaLOA = new LOAGrupoOfertas();
                            preparaDadosOfertaLOA(livroInstrumento, mdEntryType, mdUpdateAction, mdEntry, dadosOfertaLOA);

                            if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_BID))
                            {
                                contabilizaOfertasCompraOuVendaLOAPuma(
                                    LivroOfertasBase.LIVRO_COMPRA,
                                    livroInstrumento,
                                    mdEntryType,
                                    mdUpdateAction,
                                    mdEntry,
                                    dadosOfertaLOA,
                                    streamerSinalLOA);
                            }

                            else if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_OFFER))
                            {
                                contabilizaOfertasCompraOuVendaLOAPuma(
                                    LivroOfertasBase.LIVRO_VENDA,
                                    livroInstrumento,
                                    mdEntryType,
                                    mdUpdateAction,
                                    mdEntry,
                                    dadosOfertaLOA,
                                    streamerSinalLOA);
                            }

                            else if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_EMPTY_BOOK))
                            {
                                List<Dictionary<String, String>> ofertasLOA = new List<Dictionary<String, String>>();

                                resetLivroOfertasLOA(LivroOfertasBase.LIVRO_COMPRA, livroInstrumento, ofertasLOA);
                                streamerSinalLOA.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_COMPRA, ofertasLOA);

                                ofertasLOA = new List<Dictionary<String, String>>();
                                resetLivroOfertasLOA(LivroOfertasBase.LIVRO_VENDA, livroInstrumento, ofertasLOA);
                                streamerSinalLOA.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_VENDA, ofertasLOA);
                            }
                        }

                        /*else if (marketDepth == ConstantesUMDF.UMDF_MARKETDEPTH_TOP_OF_BOOK)
                        {
                        }*/
                    }
                }
                else
                {
                    // Monta Livro de Ofertas Por Ordem, Ofertas Por Preco (Agregado) e Topo do Livro, a partir de um único Canal UDMF
                    lock (livrosOfertas[livroInstrumento.Instrumento])
                    {
                        LOFDadosOferta dadosOfertaLOF = preparaDadosOfertaLOF(livroInstrumento, mdEntryType, mdUpdateAction, mdEntry, isPuma20);

                        if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_BID))
                        {
                            contabilizaOfertasCompraOuVenda(
                                LivroOfertasBase.LIVRO_COMPRA,
                                livroInstrumento,
                                mdEntryType,
                                mdUpdateAction,
                                mdEntry,
                                dadosOfertaLOF,
                                umdfMessage,
                                streamerSinalLOF,
                                streamerSinalLOA,
                                channelId);
                        }
                        else if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_OFFER))
                        {
                            contabilizaOfertasCompraOuVenda(
                                LivroOfertasBase.LIVRO_VENDA,
                                livroInstrumento,
                                mdEntryType,
                                mdUpdateAction,
                                mdEntry,
                                dadosOfertaLOF,
                                umdfMessage,
                                streamerSinalLOF,
                                streamerSinalLOA,
                                channelId);
                        }

                        else if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_EMPTY_BOOK))
                        {
                            List<Dictionary<String, String>> ofertasLOA = new List<Dictionary<String, String>>();
                            List<Dictionary<String, String>> ofertasLOF = new List<Dictionary<String, String>>();

                            resetLivroOfertas(LivroOfertasBase.LIVRO_COMPRA, livroInstrumento, ofertasLOA, ofertasLOF);
                            streamerSinalLOA.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_COMPRA, ofertasLOA);
                            streamerSinalLOF.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_COMPRA, ofertasLOF);

                            ofertasLOA = new List<Dictionary<String, String>>();
                            ofertasLOF = new List<Dictionary<String, String>>();
                            resetLivroOfertas(LivroOfertasBase.LIVRO_VENDA, livroInstrumento, ofertasLOA, ofertasLOF);
                            streamerSinalLOA.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_VENDA, ofertasLOA);
                            streamerSinalLOF.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_VENDA, ofertasLOF);
                        }
                    }
                }

                if ( bGerarEventoStreamer )
                    enviaSinalStreamer(livroInstrumento.Instrumento, streamerSinalLOF, streamerSinalLOA);

                if ( bGerarEventoHB && timestampControl.ShouldSendLOF(livroInstrumento.Instrumento))
                {
                    enviaSinalHB(livroInstrumento.Instrumento);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("trataEventoUmdf(): {0} {1}", ex.Message, ex);
            }
        }

        private void enviaSinalStreamer( string instrumento, 
            Dictionary<string, List<Dictionary<string, string>>> streamerSinalLOF,
            Dictionary<string, List<Dictionary<string, string>>> streamerSinalLOA)
        {
            if (streamerSinalLOF.Count > 0)
            {
                EventoHttpLivroOfertas eventoLOF = new EventoHttpLivroOfertas();

                eventoLOF.cabecalho = MDSUtils.montaCabecalhoStreamer(
                    ConstantesMDS.TIPO_REQUISICAO_LIVRO_OFERTAS,
                    livrosOfertas[instrumento].TipoBolsa,
                    ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_ALTERAR,
                    instrumento,
                    livrosOfertas[instrumento].CasasDecimais,
                    null);
                eventoLOF.instrumento = instrumento;

                List<Dictionary<string, string>> livrostreamer = null;
                if (streamerSinalLOF.TryGetValue(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_COMPRA, out livrostreamer))
                    eventoLOF.livroCompra = livrostreamer;

                livrostreamer = null;
                if (streamerSinalLOF.TryGetValue(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_VENDA, out livrostreamer))
                    eventoLOF.livroVenda = livrostreamer;

                EventQueueManager.Instance.SendEvent(eventoLOF);
            }

            if (streamerSinalLOA.Count > 0)
            {
                EventoHttpLivroOfertasAgregado eventoLOA = new EventoHttpLivroOfertasAgregado();

                eventoLOA.cabecalho = MDSUtils.montaCabecalhoStreamer(
                    ConstantesMDS.TIPO_REQUISICAO_LIVRO_OFERTAS_AGREGADO,
                    livrosOfertas[instrumento].TipoBolsa,
                    ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_ALTERAR,
                    instrumento,
                    livrosOfertas[instrumento].CasasDecimais,
                    null);
                eventoLOA.instrumento = instrumento;

                List<Dictionary<string, string>> loastreamer = null;
                if (streamerSinalLOA.TryGetValue(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_COMPRA, out loastreamer))
                    eventoLOA.livroCompra = loastreamer;

                if (streamerSinalLOA.TryGetValue(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_VENDA, out loastreamer))
                    eventoLOA.livroVenda = loastreamer;

                EventQueueManager.Instance.SendEvent(eventoLOA);
            }

        }

        protected override void afterStart()
        {
            try
            {
                // Workers para atualizar o topo do livro - UMDF
                queuesGeraTopo = new ConcurrentQueue<TopoLivroWorker>[maxThreadGeraTopo];
                syncQueuesGeraTopo = new Object[maxThreadGeraTopo];
                thGeraTopo = new Thread[maxThreadGeraTopo];

                for (int idx = 0; idx < maxThreadGeraTopo; idx++)
                {
                    queuesGeraTopo[idx] = new ConcurrentQueue<TopoLivroWorker>();
                    syncQueuesGeraTopo[idx] = new Object();
                    thGeraTopo[idx] = new Thread(new ParameterizedThreadStart(geraTopoLivroWorker));
                    thGeraTopo[idx].Start(idx);
                }

                // Workers para atualizar o topo do livro - FIX/Conflated
                queuesGeraTopoFix = new ConcurrentQueue<TopoLivroWorkerFix>[maxThreadGeraTopo];
                syncQueuesGeraTopoFix = new Object[maxThreadGeraTopo];
                thGeraTopoFix = new Thread[maxThreadGeraTopo];

                for (int idx = 0; idx < maxThreadGeraTopo; idx++)
                {
                    queuesGeraTopoFix[idx] = new ConcurrentQueue<TopoLivroWorkerFix>();
                    syncQueuesGeraTopoFix[idx] = new Object();
                    thGeraTopoFix[idx] = new Thread(new ParameterizedThreadStart(geraTopoLivroWorkerFix));
                    thGeraTopoFix[idx].Start(idx);
                }

                // Monitor para despachar mensagens de livro para o HB
                Thread thMonitorCacheHB = new Thread(new ThreadStart(monitorCacheHBProc));
                thMonitorCacheHB.Start();
            }
            catch (Exception ex)
            {
                logger.Error("afterStart():" + ex.Message, ex);
            }

        }
        #endregion overrides

        #region privates
        private LOFDadosOferta preparaDadosOfertaLOF(
            LivroOfertasBase livroInstrumento,
            string mdEntryType,
            string mdUpdateAction,
            GroupValue mdEntry,
            bool isPuma20)
        {
            LOFDadosOferta dadosOferta = new LOFDadosOferta();

            try
            {
                if (!mdEntry.IsDefined("OrderID"))
                {
                    if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR_OFERTA_E_MELHORES))
                    {
                        dadosOferta.IDOferta = "";
                    }
                    else
                    {
                        logger.Error("Instrumento[" + livroInstrumento.Instrumento + "]: OrderID inexistente");
                        return dadosOferta;
                    }
                }
                else
                {
                    dadosOferta.IDOferta = mdEntry.GetString("OrderID");
                }

                if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_BID) ||
                    mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_OFFER))
                {
                    String posicaoSemFormato = mdEntry.GetInt("MDEntryPositionNo").ToString();
                    if (String.IsNullOrEmpty(posicaoSemFormato))
                    {
                        logger.Error("Instrumento[" + livroInstrumento.Instrumento + "]: Posicao inexistente");
                        return dadosOferta;
                    }
                    int posicao = Int32.Parse(posicaoSemFormato) - 1;

                    String data = mdEntry.GetString("MDEntryDate");
                    String formatHora = (isPuma20 ? "{0,9:d9}" : "{0:d6}");
                    String horaOriginal = String.Format(formatHora, mdEntry.GetInt("MDEntryTime"));
                    String hora = UmdfUtils.convertUTC2Local(data, horaOriginal);
                    Decimal preco = new Decimal(0);
                    long quantidade = 0;

                    if (!mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR))
                    {
                        if (mdEntry.IsDefined("MDEntryPx"))
                        {
                            String precoSemFormato = mdEntry.GetBigDecimal("MDEntryPx").ToString();
                            if (String.IsNullOrEmpty(precoSemFormato))
                            {
                                logger.Error("Instrumento[" + livroInstrumento.Instrumento + "]: Preco inexistente");
                                return dadosOferta;
                            }
                            preco = Decimal.Parse(precoSemFormato);

                            if (posicao == 0)
                            {
                                if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_BID))
                                    livroInstrumento.BestBidPx = preco;
                                else
                                    livroInstrumento.BestOfferPx = preco;
                            }

                        }
                        else
                        {
                            //Papel em leilao
                            dadosOferta.Leilao = true;
                            if (mdEntryType.Equals(ConstantesMDS.TIPO_REQUISICAO_BMF_LIVRO_OFERTAS_COMPRA))
                            {
                                Decimal auctionBidPx = livroInstrumento.BestBidPx;
                                preco = ConstantesMDS.PRECO_LIVRO_OFERTAS_MAX_VALUE;
                            }
                            else
                            {
                                Decimal auctionOfferPx = livroInstrumento.BestOfferPx;
                                preco = ConstantesMDS.PRECO_LIVRO_OFERTAS_MIN_VALUE;
                            }
                        }

                        String quantidadeSemFormato = mdEntry.GetString("MDEntrySize");
                        if (String.IsNullOrEmpty(quantidadeSemFormato))
                        {
                            if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR_OFERTA_E_MELHORES))
                            {
                                quantidade = 0;
                            }
                            else
                            {
                                logger.Error("Instrumento[" + livroInstrumento.Instrumento + "]: Quantidade inexistente");
                                return dadosOferta;
                            }
                        }
                        else
                        {
                            quantidade = Int64.Parse(quantidadeSemFormato);
                        }
                    }

                    dadosOferta.Data = data;
                    dadosOferta.Hora = hora;
                    dadosOferta.Preco = preco;
                    dadosOferta.Quantidade = quantidade;
                    dadosOferta.Posicao = posicao;
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("contabilizaLivroOfertas(): Falha no instrumento[{0}] {1} {2}",
                    livroInstrumento.Instrumento, ex.Message, ex);
            }
            return dadosOferta;
        }

        private void preparaDadosOfertaLOA(
            LivroOfertasBase livroInstrumento,
            string mdEntryType,
            string mdUpdateAction,
            GroupValue mdEntry,
            LOAGrupoOfertas dadosOferta)
        {
            try
            {
                if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_BID) ||
                    mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_OFFER))
                {
                    String posicaoSemFormato = mdEntry.GetInt("MDEntryPositionNo").ToString();
                    if (String.IsNullOrEmpty(posicaoSemFormato))
                    {
                        logger.Error("Instrumento[" + livroInstrumento.Instrumento + "]: Posicao inexistente");
                        return;
                    }
                    int posicao = Int32.Parse(posicaoSemFormato) - 1;

                    Decimal preco = new Decimal(0);
                    long quantidade = 0;
                    int qtdOrdens = 0;

                    if (!mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR) && 
                        !mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR_OFERTA_E_MELHORES) && 
                        !mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR_OFERTAS_E_PIORES))
                    {
                        String qtdOrdensSemFormato = mdEntry.GetInt("NumberOfOrders").ToString();
                        if (String.IsNullOrEmpty(qtdOrdensSemFormato))
                        {
                            logger.Error("Instrumento[" + livroInstrumento.Instrumento + "]: NumberOfOrders inexistente");
                            return;
                        }
                        qtdOrdens = Int32.Parse(qtdOrdensSemFormato);

                        if (mdEntry.IsDefined("MDEntryPx"))
                        {
                            String precoSemFormato = mdEntry.GetBigDecimal("MDEntryPx").ToString();
                            if (String.IsNullOrEmpty(precoSemFormato))
                            {
                                logger.Error("Instrumento[" + livroInstrumento.Instrumento + "]: Preco inexistente");
                                return;
                            }
                            preco = Decimal.Parse(precoSemFormato);
                        }

                        String quantidadeSemFormato = mdEntry.GetString("MDEntrySize");
                        if (String.IsNullOrEmpty(quantidadeSemFormato))
                        {
                            logger.Error("Instrumento[" + livroInstrumento.Instrumento + "]: Quantidade inexistente");
                            return;
                        }
                        quantidade = Int64.Parse(quantidadeSemFormato);
                    }

                    dadosOferta.Indice = posicao;
                    dadosOferta.Preco = preco;
                    dadosOferta.Quantidade = quantidade;
                    dadosOferta.QtdeOrdens = qtdOrdens;
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("preparaDadosOfertaLOA(): Falha no instrumento[{0}] {1} {2}",
                    livroInstrumento.Instrumento, ex.Message, ex);
                return;
            }
        }

        private void contabilizaOfertasCompraOuVenda(
            int sentido,
            LivroOfertasBase livroInstrumento, 
            string mdEntryType,
            string mdUpdateAction,
            GroupValue mdEntry,
            LOFDadosOferta dadosOferta,
            Message message,
            Dictionary<string, List<Dictionary<string, string>>> streamerSinalOfertas,
            Dictionary<string, List<Dictionary<string, string>>> streamerSinalAgregado,
            String channelID)
        {
            try
            {
                bool bGerarTopo = false; 
                List<Dictionary<String, String>> ofertasAgregado = new List<Dictionary<String, String>>();
                List<Dictionary<String, String>> ofertasLOF = new List<Dictionary<String, String>>();

                long corretora = 0;
                String mdEntryBuyerSeller = (sentido == LivroOfertasBase.LIVRO_COMPRA ? "MDEntryBuyer" : "MDEntrySeller");
                String descricaoCompraVenda = (sentido == LivroOfertasBase.LIVRO_COMPRA ? "compra" : "venda");
                List<LOFDadosOferta> listaOfertasLOF = (sentido == LivroOfertasBase.LIVRO_COMPRA ?
                    livroInstrumento.getLivroCompra() : livroInstrumento.getLivroVenda());
                List<LOAGrupoOfertas> listaOfertasLOA = (sentido == LivroOfertasBase.LIVRO_COMPRA ?
                    livroInstrumento.getAgregadoCompra() : livroInstrumento.getAgregadoVenda());

                if (mdEntry.IsDefined(mdEntryBuyerSeller))
                {
                    String corretoraPorExtenso = mdEntry.GetString(mdEntryBuyerSeller).Replace("BM", "").Trim();
                    corretora = Int64.Parse(corretoraPorExtenso);
                    dadosOferta.Corretora = corretora;
                }

                try
                {
                    if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_INCLUIR))
                    {
                        if (logger.IsDebugEnabled)
                        {
                            logger.DebugFormat("Instrumento[{0}] : Inclui oferta de {1} - posicao[{2}] preco[{3}] quantidade[{4}] corretora[{5}]",
                                livroInstrumento.Instrumento,
                                descricaoCompraVenda,
                                dadosOferta.Posicao,
                                String.Format("{0:f" + livroInstrumento.CasasDecimais + "}", Convert.ToDouble(dadosOferta.Preco)),
                                dadosOferta.Quantidade,
                                corretora);
                        }

                        ofertasLOF.Add(LOFDadosOferta.montarRegistroOferta(ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_INCLUIR, dadosOferta, livroInstrumento.CasasDecimais));

                        List<LOAItemOferta> listaOfertas = LivroOfertasBase.insereOferta(listaOfertasLOA, listaOfertasLOF, dadosOferta, sentido);

                        for (int i = 0; i < listaOfertas.Count; i++)
                        {
                            LOAItemOferta itemAgregado = listaOfertas[i];
                            ofertasAgregado.Add(LOAItemOferta.montarRegistroAgregado(itemAgregado, livroInstrumento.CasasDecimais));

                            if (itemAgregado.Indice == 0)
                                bGerarTopo = true;
                        }
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_ALTERAR))
                    {
                        if (logger.IsDebugEnabled)
                        {
                            logger.DebugFormat("Instrumento[{0}] : Altera oferta de {1} - posicao[{2}] preco[{3}] quantidade[{4}] corretora[{5}]",
                                livroInstrumento.Instrumento,
                                descricaoCompraVenda,
                                dadosOferta.Posicao,
                                String.Format("{0:f" + livroInstrumento.CasasDecimais + "}", Convert.ToDouble(dadosOferta.Preco)),
                                dadosOferta.Quantidade,
                                corretora);
                        }

                        List<LOAItemOferta> listaOfertas = LivroOfertasBase.alteraOferta(listaOfertasLOA, listaOfertasLOF, dadosOferta, sentido);

                        for (int i = 0; i < listaOfertas.Count; i++)
                        {
                            ofertasAgregado.Add(LOAItemOferta.montarRegistroAgregado(listaOfertas[i], livroInstrumento.CasasDecimais));
                            if (listaOfertas[i].Indice == 0)
                                bGerarTopo = true;
                        }

                        ofertasLOF.Add(LOFDadosOferta.montarRegistroOferta(ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_ALTERAR, dadosOferta, livroInstrumento.CasasDecimais));
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR))
                    {
                        try
                        {
                            List<LOAItemOferta> listaOfertas = LivroOfertasBase.removeOferta(listaOfertasLOA, listaOfertasLOF, dadosOferta);

                            for (int i = 0; i < listaOfertas.Count; i++)
                            {
                                LOAItemOferta itemOferta = listaOfertas[i];

                                if (logger.IsDebugEnabled)
                                {
                                    logger.DebugFormat("Instrumento[{0}] : Exclui oferta de {1} - posicao[{2}] preco[{3}] id[{4}]",
                                        livroInstrumento.Instrumento,
                                        descricaoCompraVenda,
                                        dadosOferta.Posicao,
                                        String.Format("{0:f" + livroInstrumento.CasasDecimais + "}", Convert.ToDouble(dadosOferta.Preco)),
                                        dadosOferta.IDOferta);
                                }

                                ofertasAgregado.Add(LOAItemOferta.montarRegistroAgregado(itemOferta, livroInstrumento.CasasDecimais));

                                if (itemOferta.Indice == 0)
                                    bGerarTopo = true;
                            }

                            ofertasLOF.Add(LOFDadosOferta.montarRegistroOferta(ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR, dadosOferta, livroInstrumento.CasasDecimais));
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorFormat("Instrumento[{0}]: Nao encontrado posicao[{1}] do livro de ofertas de {2}",
                                livroInstrumento.Instrumento, dadosOferta.Posicao, descricaoCompraVenda);
                        }
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR_OFERTA_E_MELHORES))
                    {
                        try
                        {
                            if (logger.IsDebugEnabled)
                            {
                                logger.DebugFormat("Instrumento[{0}] : Exclui oferta de {1} e melhores - posicao[{2}] preco[{3}] id[{4}]",
                                    livroInstrumento.Instrumento,
                                    descricaoCompraVenda,
                                    dadosOferta.Posicao,
                                    String.Format("{0:f" + livroInstrumento.CasasDecimais + "}", Convert.ToDouble(dadosOferta.Preco)),
                                    dadosOferta.IDOferta);
                            }

                            List<LOFDadosOferta> listaLOF = new List<LOFDadosOferta>();
                            List<LOAItemOferta> listaAgregado = LivroOfertasBase.removeOfertaEMelhores(
                                listaOfertasLOA, listaOfertasLOF, dadosOferta, sentido, listaLOF);

                            for (int i = 0; i < listaAgregado.Count; i++)
                            {
                                ofertasAgregado.Add(LOAItemOferta.montarRegistroAgregado(listaAgregado[i], livroInstrumento.CasasDecimais));
                            }

                            for (int i = 0; i < listaLOF.Count; i++)
                            {
                                ofertasLOF.Add(LOFDadosOferta.montarRegistroOferta(ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR, listaLOF[i], livroInstrumento.CasasDecimais));
                            }
                            bGerarTopo = true;
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorFormat("Instrumento[{0}]: Nao encontrado posicao[{1}] do livro de ofertas de {2}",
                                livroInstrumento.Instrumento, dadosOferta.Posicao, descricaoCompraVenda);
                        }
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR_OFERTAS_E_PIORES))
                    {
                        try
                        {
                            if (logger.IsDebugEnabled)
                            {
                                logger.DebugFormat("Instrumento[{0}] : Exclui oferta de {1} e piores - posicao[{2}] preco[{3}] id[{4}]",
                                    livroInstrumento.Instrumento,
                                    descricaoCompraVenda,
                                    dadosOferta.Posicao,
                                    String.Format("{0:f" + livroInstrumento.CasasDecimais + "}", Convert.ToDouble(dadosOferta.Preco)),
                                    dadosOferta.IDOferta);
                            }
                            List<LOFDadosOferta> listaLOF = new List<LOFDadosOferta>();
                            List<LOAItemOferta> listaOfertas = LivroOfertasBase.removeOfertaEPiores(
                                listaOfertasLOA, listaOfertasLOF, dadosOferta, sentido, listaLOF);

                            for (int i = 0; i < listaOfertas.Count; i++)
                            {
                                ofertasAgregado.Add(LOAItemOferta.montarRegistroAgregado(listaOfertas[i], livroInstrumento.CasasDecimais));
                                if (listaOfertas[i].Indice == 0)
                                    bGerarTopo = true;
                            }

                            for (int i = 0; i < listaLOF.Count; i++)
                            {
                                ofertasLOF.Add(LOFDadosOferta.montarRegistroOferta(ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR, listaLOF[i], livroInstrumento.CasasDecimais));
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorFormat("Instrumento[{0}]: Nao encontrado posicao[{1}] do livro de ofertas de {2}",
                                livroInstrumento.Instrumento, dadosOferta.Posicao, descricaoCompraVenda);
                        }
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_RESET))
                    {
                        resetLivroOfertas(sentido, livroInstrumento, ofertasAgregado, ofertasLOF);
                    }

                    else
                    {
                        logger.ErrorFormat("Instrumento[{0}]: Falha na manutencao do Livro Oferta de {1} - mdUpdateAction[{2}]",
                            livroInstrumento.Instrumento, descricaoCompraVenda, mdUpdateAction);
                    }

                    String descricaoHttpOfertasOcorrencias = (sentido == LivroOfertasBase.LIVRO_COMPRA ?
                        ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_COMPRA : ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_VENDA);
                    streamerSinalAgregado.Add(descricaoHttpOfertasOcorrencias, ofertasAgregado);
                    streamerSinalOfertas.Add(descricaoHttpOfertasOcorrencias, ofertasLOF);
                }

                catch (Exception ex)
                {
                    logger.ErrorFormat("Instrumento[{0}]: Falha na manutencao do Livro Oferta de {1} - mdUpdateAction[{2}]: {3} {4}",
                        livroInstrumento.Instrumento, descricaoCompraVenda, mdUpdateAction, ex.Message, ex);
                }

                if (bGerarTopo /* && listaOfertasLOA.Count > 0 && ContainerManager.Instance.NegociosConsumer.IsNegociacao(livroInstrumento.Instrumento)*/)
                {
                    Decimal topbPreco = 0;
                    long topbQuantidade = 0;

                    if (listaOfertasLOA.Count > 0)
                    {
                        topbPreco = listaOfertasLOA[0].Preco;
                        topbQuantidade = listaOfertasLOA[0].Quantidade;
                    }

                    //Stopwatch sw = new Stopwatch();
                    //sw.Start();
                    EnqueueMensagemTopoLivro(message, topbPreco, topbQuantidade, channelID);
                    //sw.Stop();

                    //logger.Info("EnqueueMensagemTopoLivro: " + sw.ElapsedTicks);

                    //sw.Start();

                    //ThreadPool.QueueUserWorkItem(
                    //    new WaitCallback(
                    //        delegate(object required)
                    //        {
                    //            try
                    //            {
                    //                geraMensagemTopoLivro(message, topbPreco, topbQuantidade, channelID);
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                logger.ErrorFormat("geraMensagemTopoLivro({0}): {1} {2}", descricaoCompraVenda, ex.Message, ex);
                    //            }
                    //        }
                    //    )
                    //);
                    //sw.Stop();

                    //logger.Info("ThreadPool(geraMensagemTopoLivro): " + sw.ElapsedTicks + "ticks");
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("contabilizaOfertasCompraOuVenda(): Falha no instrumento[{0}] {1} {2}",
                    livroInstrumento.Instrumento, ex.Message, ex);
                return;
            }
        }

        private void resetLivroOfertas(
            int sentido,
            LivroOfertasBase livroInstrumento,
            List<Dictionary<String, String>> ofertasAgregado,
            List<Dictionary<String, String>> ofertasLOF)
        {
            String mdEntryBuyerSeller = (sentido == LivroOfertasBase.LIVRO_COMPRA ? "MDEntryBuyer" : "MDEntrySeller");
            String descricaoCompraVenda = (sentido == LivroOfertasBase.LIVRO_COMPRA ? "compra" : "venda");
            List<LOFDadosOferta> listaOfertasLOF = (sentido == LivroOfertasBase.LIVRO_COMPRA ?
                livroInstrumento.getLivroCompra() : livroInstrumento.getLivroVenda());
            List<LOAGrupoOfertas> listaOfertasLOA = (sentido == LivroOfertasBase.LIVRO_COMPRA ?
                livroInstrumento.getAgregadoCompra() : livroInstrumento.getAgregadoVenda());

            try
            {
                logger.InfoFormat("Instrumento[{0}] : Reset do livro de ofertas de {1}",
                    livroInstrumento.Instrumento,
                    descricaoCompraVenda);

                List<LOFDadosOferta> listaLOF = new List<LOFDadosOferta>();
                List<LOAItemOferta> listaOfertas = LivroOfertasBase.resetLivro(
                    listaOfertasLOA, listaOfertasLOF, listaLOF);

                for (int i = 0; i < listaOfertas.Count; i++)
                {
                    ofertasAgregado.Add(LOAItemOferta.montarRegistroAgregado(listaOfertas[i], livroInstrumento.CasasDecimais));
                }

                for (int i = 0; i < listaLOF.Count; i++)
                {
                    ofertasLOF.Add(LOFDadosOferta.montarRegistroOferta(ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR, listaLOF[i], livroInstrumento.CasasDecimais));
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                logger.ErrorFormat("Instrumento[{0}]: erro ao Reiniciar LOA de {1}: {2} {3}",
                    livroInstrumento.Instrumento, descricaoCompraVenda, ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sentido"></param>
        /// <param name="livroInstrumento"></param>
        /// <param name="mdEntryType"></param>
        /// <param name="mdUpdateAction"></param>
        /// <param name="mdEntry"></param>
        /// <param name="dadosOferta"></param>
        /// <param name="streamerSinalOfertas"></param>
        private void contabilizaOfertasCompraOuVendaLOFPuma(
            int sentido,
            LivroOfertasBase livroInstrumento,
            string mdEntryType,
            string mdUpdateAction,
            GroupValue mdEntry,
            LOFDadosOferta dadosOferta,
            Dictionary<string, List<Dictionary<string, string>>> streamerSinalOfertas)
        {
            try
            {
                List<Dictionary<String, String>> ofertasLOF = new List<Dictionary<String, String>>();

                long corretora = 0;
                String mdEntryBuyerSeller = (sentido == LivroOfertasBase.LIVRO_COMPRA ? "MDEntryBuyer" : "MDEntrySeller");
                String descricaoCompraVenda = (sentido == LivroOfertasBase.LIVRO_COMPRA ? "compra" : "venda");
                List<LOFDadosOferta> listaOfertasLOF = (sentido == LivroOfertasBase.LIVRO_COMPRA ?
                    livroInstrumento.getLivroCompra() : livroInstrumento.getLivroVenda());

                if (mdEntry.IsDefined(mdEntryBuyerSeller))
                {
                    String corretoraPorExtenso = mdEntry.GetString(mdEntryBuyerSeller).Replace("BM", "").Trim();
                    corretora = Int64.Parse(corretoraPorExtenso);
                    dadosOferta.Corretora = corretora;
                }

                try
                {
                    if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_INCLUIR))
                    {
                        if (logger.IsDebugEnabled)
                        {
                            logger.DebugFormat("LOF Instrumento[{0}]: Inclui oferta de {1} - posicao[{2}] preco[{3}] quantidade[{4}] corretora[{5}]",
                                livroInstrumento.Instrumento,
                                descricaoCompraVenda,
                                dadosOferta.Posicao,
                                String.Format("{0:f" + livroInstrumento.CasasDecimais + "}", Convert.ToDouble(dadosOferta.Preco)),
                                dadosOferta.Quantidade,
                                corretora);
                        }

                        LivroOfertasBase.insereOfertaLOF(livroInstrumento.Instrumento, listaOfertasLOF, dadosOferta, sentido);

                        ofertasLOF.Add(LOFDadosOferta.montarRegistroOferta(ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_INCLUIR, dadosOferta, livroInstrumento.CasasDecimais));
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_ALTERAR))
                    {
                        if (logger.IsDebugEnabled)
                        {
                            logger.DebugFormat("LOF Instrumento[{0}]: Altera oferta de {1} - posicao[{2}] preco[{3}] quantidade[{4}] corretora[{5}]",
                                livroInstrumento.Instrumento,
                                descricaoCompraVenda,
                                dadosOferta.Posicao,
                                String.Format("{0:f" + livroInstrumento.CasasDecimais + "}", Convert.ToDouble(dadosOferta.Preco)),
                                dadosOferta.Quantidade,
                                corretora);
                        }

                        LivroOfertasBase.alteraOfertaLOF(livroInstrumento.Instrumento, listaOfertasLOF, dadosOferta, sentido);

                        ofertasLOF.Add(LOFDadosOferta.montarRegistroOferta(ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_ALTERAR, dadosOferta, livroInstrumento.CasasDecimais));
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR))
                    {
                        try
                        {
                            if (logger.IsDebugEnabled)
                            {
                                logger.DebugFormat("LOF Instrumento[{0}] : Exclui oferta de {1} - posicao[{2}]",
                                    livroInstrumento.Instrumento,
                                    descricaoCompraVenda,
                                    dadosOferta.Posicao);
                            }

                            LivroOfertasBase.removeOfertaLOF(livroInstrumento.Instrumento, listaOfertasLOF, dadosOferta, sentido);

                            ofertasLOF.Add(LOFDadosOferta.montarRegistroOferta(ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR, dadosOferta, livroInstrumento.CasasDecimais));
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorFormat("LOF Instrumento[{0}]: Nao encontrado posicao[{1}] do livro de ofertas de {2}: {3}",
                                livroInstrumento.Instrumento, dadosOferta.Posicao, descricaoCompraVenda, ex.Message);
                        }
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR_OFERTA_E_MELHORES))
                    {
                        try
                        {
                            if (logger.IsDebugEnabled)
                            {
                                logger.DebugFormat("LOF Instrumento[{0}] : Exclui oferta de {1} e melhores - posicao[{2}]",
                                    livroInstrumento.Instrumento,
                                    descricaoCompraVenda,
                                    dadosOferta.Posicao);
                            }

                            List<LOFDadosOferta> listaLOF = new List<LOFDadosOferta>();
                            LivroOfertasBase.removeOfertaEMelhoresLOF(livroInstrumento.Instrumento, listaOfertasLOF, dadosOferta, sentido, listaLOF);

                            for (int i = 0; i < listaLOF.Count; i++)
                            {
                                ofertasLOF.Add(LOFDadosOferta.montarRegistroOferta(ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR, listaLOF[i], livroInstrumento.CasasDecimais));
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorFormat("LOF Instrumento[{0}]: Nao encontrado posicao[{1}] do livro de ofertas de {2}: {3}",
                                livroInstrumento.Instrumento, dadosOferta.Posicao, descricaoCompraVenda, ex.Message);
                        }
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR_OFERTAS_E_PIORES))
                    {
                        try
                        {
                            if (logger.IsDebugEnabled)
                            {
                                logger.DebugFormat("LOF Instrumento[{0}] : Exclui oferta de {1} e piores - posicao[{2}]",
                                    livroInstrumento.Instrumento,
                                    descricaoCompraVenda,
                                    dadosOferta.Posicao);
                            }

                            List<LOFDadosOferta> listaLOF = new List<LOFDadosOferta>();
                            LivroOfertasBase.removeOfertaEPioresLOF(livroInstrumento.Instrumento, listaOfertasLOF, dadosOferta, sentido, listaLOF);

                            for (int i = 0; i < listaLOF.Count; i++)
                            {
                                ofertasLOF.Add(LOFDadosOferta.montarRegistroOferta(ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR, listaLOF[i], livroInstrumento.CasasDecimais));
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorFormat("LOF Instrumento[{0}]: Nao encontrado posicao[{1}] do livro de ofertas de {2}: {3}",
                                livroInstrumento.Instrumento, dadosOferta.Posicao, descricaoCompraVenda, ex.Message);
                        }
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_RESET))
                    {
                        resetLivroOfertasLOF(sentido, livroInstrumento, ofertasLOF);
                    }

                    else
                    {
                        logger.ErrorFormat("LOF Instrumento[{0}]: Falha na manutencao do Livro Oferta de {1} - mdUpdateAction[{2}]",
                            livroInstrumento.Instrumento, descricaoCompraVenda, mdUpdateAction);
                    }

                    String descricaoHttpOfertasOcorrencias = (sentido == LivroOfertasBase.LIVRO_COMPRA ?
                        ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_COMPRA : ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_VENDA);
                    streamerSinalOfertas.Add(descricaoHttpOfertasOcorrencias, ofertasLOF);
                }

                catch (Exception ex)
                {
                    logger.ErrorFormat("LOF Instrumento[{0}]: Falha na manutencao do Livro Oferta de {1} - mdUpdateAction[{2}]: {3} {4}",
                        livroInstrumento.Instrumento, descricaoCompraVenda, mdUpdateAction, ex.Message, ex);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("contabilizaOfertasCompraOuVendaPuma(): Falha no instrumento[{0}] {1} {2}",
                    livroInstrumento.Instrumento, ex.Message, ex);
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sentido"></param>
        /// <param name="livroInstrumento"></param>
        /// <param name="mdEntryType"></param>
        /// <param name="mdUpdateAction"></param>
        /// <param name="mdEntry"></param>
        /// <param name="dadosOferta"></param>
        /// <param name="streamerSinalOfertas"></param>
        private void contabilizaOfertasCompraOuVendaLOAPuma(
            int sentido,
            LivroOfertasBase livroInstrumento,
            string mdEntryType,
            string mdUpdateAction,
            GroupValue mdEntry,
            LOAGrupoOfertas dadosOferta,
            Dictionary<string, List<Dictionary<string, string>>> streamerSinalOfertas)
        {
            try
            {
                List<Dictionary<String, String>> ofertasLOA = new List<Dictionary<String, String>>();

                String descricaoCompraVenda = (sentido == LivroOfertasBase.LIVRO_COMPRA ? "compra" : "venda");
                List<LOAGrupoOfertas> listaOfertasLOA = (sentido == LivroOfertasBase.LIVRO_COMPRA ?
                    livroInstrumento.getAgregadoCompra() : livroInstrumento.getAgregadoVenda());

                try
                {
                    if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_INCLUIR))
                    {
                        if (logger.IsDebugEnabled)
                        {
                            logger.DebugFormat("LOA Instrumento[{0}]: Inclui oferta de {1} - posicao[{2}] preco[{3}] quantidade[{4}] qtdOrdens[{5}]",
                                livroInstrumento.Instrumento,
                                descricaoCompraVenda,
                                dadosOferta.Indice,
                                String.Format("{0:f" + livroInstrumento.CasasDecimais + "}", Convert.ToDouble(dadosOferta.Preco)),
                                dadosOferta.Quantidade,
                                dadosOferta.QtdeOrdens);
                        }

                        LivroOfertasBase.insereOfertaLOA(livroInstrumento.Instrumento, listaOfertasLOA, dadosOferta, sentido);

                        LOAItemOferta LOAItem = new LOAItemOferta();
                        LOAItem.Acao = ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_INCLUIR;
                        LOAItem.Indice = dadosOferta.Indice;
                        LOAItem.Preco = dadosOferta.Preco;
                        LOAItem.QtdeOrdens = dadosOferta.QtdeOrdens;
                        LOAItem.Quantidade = dadosOferta.Quantidade;
                        ofertasLOA.Add(LOAItemOferta.montarRegistroAgregado(LOAItem, livroInstrumento.CasasDecimais));
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_ALTERAR))
                    {
                        if (logger.IsDebugEnabled)
                        {
                            logger.DebugFormat("LOA Instrumento[{0}]: Altera oferta de {1} - posicao[{2}] preco[{3}] quantidade[{4}] qtdOrdens[{5}]",
                                livroInstrumento.Instrumento,
                                descricaoCompraVenda,
                                dadosOferta.Indice,
                                String.Format("{0:f" + livroInstrumento.CasasDecimais + "}", Convert.ToDouble(dadosOferta.Preco)),
                                dadosOferta.Quantidade,
                                dadosOferta.QtdeOrdens);
                        }

                        LivroOfertasBase.alteraOfertaLOA(livroInstrumento.Instrumento, listaOfertasLOA, dadosOferta, sentido);

                        LOAItemOferta LOAItem = new LOAItemOferta();
                        LOAItem.Acao = ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_ALTERAR;
                        LOAItem.Indice = dadosOferta.Indice;
                        LOAItem.Preco = dadosOferta.Preco;
                        LOAItem.QtdeOrdens = dadosOferta.QtdeOrdens;
                        LOAItem.Quantidade = dadosOferta.Quantidade;
                        ofertasLOA.Add(LOAItemOferta.montarRegistroAgregado(LOAItem, livroInstrumento.CasasDecimais));
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR))
                    {
                        try
                        {
                            if (logger.IsDebugEnabled)
                            {
                                logger.DebugFormat("LOA Instrumento[{0}] : Exclui oferta de {1} - posicao[{2}] preco[{3}]",
                                    livroInstrumento.Instrumento,
                                    descricaoCompraVenda,
                                    dadosOferta.Indice,
                                    String.Format("{0:f" + livroInstrumento.CasasDecimais + "}", Convert.ToDouble(dadosOferta.Preco)));
                            }

                            LivroOfertasBase.removeOfertaLOA(livroInstrumento.Instrumento, listaOfertasLOA, dadosOferta, sentido);

                            LOAItemOferta LOAItem = new LOAItemOferta();
                            LOAItem.Acao = ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR;
                            LOAItem.Indice = dadosOferta.Indice;
                            LOAItem.Preco = dadosOferta.Preco;
                            LOAItem.QtdeOrdens = dadosOferta.QtdeOrdens;
                            LOAItem.Quantidade = dadosOferta.Quantidade;
                            ofertasLOA.Add(LOAItemOferta.montarRegistroAgregado(LOAItem, livroInstrumento.CasasDecimais));
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorFormat("LOA Instrumento[{0}]: Nao encontrado posicao[{1}] do livro de ofertas de {2}: {3}",
                                livroInstrumento.Instrumento, dadosOferta.Indice, descricaoCompraVenda, ex.Message);
                        }
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR_OFERTA_E_MELHORES))
                    {
                        try
                        {
                            if (logger.IsDebugEnabled)
                            {
                                logger.DebugFormat("LOA Instrumento[{0}] : Exclui oferta de {1} e melhores - posicao[{2}] preco[{3}]",
                                    livroInstrumento.Instrumento,
                                    descricaoCompraVenda,
                                    dadosOferta.Indice,
                                    String.Format("{0:f" + livroInstrumento.CasasDecimais + "}", Convert.ToDouble(dadosOferta.Preco)));
                            }

                            List<LOAGrupoOfertas> listaLOA = new List<LOAGrupoOfertas>();
                            LivroOfertasBase.removeOfertaEMelhoresLOA(livroInstrumento.Instrumento, listaOfertasLOA, dadosOferta, sentido, listaLOA);

                            for (int i = 0; i < listaLOA.Count; i++)
                            {
                                LOAItemOferta LOAItem = new LOAItemOferta();
                                LOAItem.Acao = ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR;
                                LOAItem.Indice = listaLOA[i].Indice;
                                LOAItem.Preco = listaLOA[i].Preco;
                                LOAItem.QtdeOrdens = listaLOA[i].QtdeOrdens;
                                LOAItem.Quantidade = listaLOA[i].Quantidade;
                                ofertasLOA.Add(LOAItemOferta.montarRegistroAgregado(LOAItem, livroInstrumento.CasasDecimais));
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorFormat("LOA Instrumento[{0}]: Nao encontrado posicao[{1}] do livro de ofertas de {2}: {3}",
                                livroInstrumento.Instrumento, dadosOferta.Indice, descricaoCompraVenda, ex.Message);
                        }
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR_OFERTAS_E_PIORES))
                    {
                        try
                        {
                            if (logger.IsDebugEnabled)
                            {
                                logger.DebugFormat("LOA Instrumento[{0}] : Exclui oferta de {1} e piores - posicao[{2}] preco[{3}]",
                                    livroInstrumento.Instrumento,
                                    descricaoCompraVenda,
                                    dadosOferta.Indice,
                                    String.Format("{0:f" + livroInstrumento.CasasDecimais + "}", Convert.ToDouble(dadosOferta.Preco)));
                            }

                            List<LOAGrupoOfertas> listaLOA = new List<LOAGrupoOfertas>();
                            LivroOfertasBase.removeOfertaEPioresLOA(livroInstrumento.Instrumento, listaOfertasLOA, dadosOferta, sentido, listaLOA);

                            for (int i = 0; i < listaLOA.Count; i++)
                            {
                                LOAItemOferta LOAItem = new LOAItemOferta();
                                LOAItem.Acao = ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR;
                                LOAItem.Indice = listaLOA[i].Indice;
                                LOAItem.Preco = listaLOA[i].Preco;
                                LOAItem.QtdeOrdens = listaLOA[i].QtdeOrdens;
                                LOAItem.Quantidade = listaLOA[i].Quantidade;
                                ofertasLOA.Add(LOAItemOferta.montarRegistroAgregado(LOAItem, livroInstrumento.CasasDecimais));
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorFormat("LOA Instrumento[{0}]: Nao encontrado posicao[{1}] do livro de ofertas de {2}: {3}",
                                livroInstrumento.Instrumento, dadosOferta.Indice, descricaoCompraVenda, ex.Message);
                        }
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_RESET))
                    {
                        resetLivroOfertasLOA(sentido, livroInstrumento, ofertasLOA);
                    }

                    else
                    {
                        logger.ErrorFormat("LOA Instrumento[{0}]: Falha na manutencao do Livro Oferta de {1} - mdUpdateAction[{2}]",
                            livroInstrumento.Instrumento, descricaoCompraVenda, mdUpdateAction);
                    }

                    String descricaoHttpOfertasOcorrencias = (sentido == LivroOfertasBase.LIVRO_COMPRA ?
                        ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_COMPRA : ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_VENDA);
                    streamerSinalOfertas.Add(descricaoHttpOfertasOcorrencias, ofertasLOA);
                }

                catch (Exception ex)
                {
                    logger.ErrorFormat("LOA Instrumento[{0}]: Falha na manutencao do Livro Oferta de {1} - mdUpdateAction[{2}]: {3} {4}",
                        livroInstrumento.Instrumento, descricaoCompraVenda, mdUpdateAction, ex.Message, ex);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("contabilizaOfertasCompraOuVendaLOAPuma(): Falha no instrumento[{0}] {1} {2}",
                    livroInstrumento.Instrumento, ex.Message, ex);
                return;
            }
        }

        private void resetLivroOfertasLOF(
            int sentido,
            LivroOfertasBase livroInstrumento,
            List<Dictionary<String, String>> ofertasLOF)
        {
            String mdEntryBuyerSeller = (sentido == LivroOfertasBase.LIVRO_COMPRA ? "MDEntryBuyer" : "MDEntrySeller");
            String descricaoCompraVenda = (sentido == LivroOfertasBase.LIVRO_COMPRA ? "compra" : "venda");
            List<LOFDadosOferta> listaOfertasLOF = (sentido == LivroOfertasBase.LIVRO_COMPRA ?
                livroInstrumento.getLivroCompra() : livroInstrumento.getLivroVenda());

            try
            {
                logger.DebugFormat("Instrumento[{0}] : Reset do livro de ofertas de {1}",
                    livroInstrumento.Instrumento,
                    descricaoCompraVenda);

                List<LOFDadosOferta> listaLOF = new List<LOFDadosOferta>();
                LivroOfertasBase.resetLivroLOF(livroInstrumento.Instrumento, listaOfertasLOF, listaLOF);

                for (int i = 0; i < listaLOF.Count; i++)
                {
                    ofertasLOF.Add(LOFDadosOferta.montarRegistroOferta(ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR, listaLOF[i], livroInstrumento.CasasDecimais));
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                logger.ErrorFormat("Instrumento[{0}]: erro ao Reiniciar LOF de {1}: {2} {3}",
                    livroInstrumento.Instrumento, descricaoCompraVenda, ex.Message, ex);
            }
        }

        private void resetLivroOfertasLOA(
            int sentido,
            LivroOfertasBase livroInstrumento,
            List<Dictionary<String, String>> ofertasLOA)
        {
            String mdEntryBuyerSeller = (sentido == LivroOfertasBase.LIVRO_COMPRA ? "MDEntryBuyer" : "MDEntrySeller");
            String descricaoCompraVenda = (sentido == LivroOfertasBase.LIVRO_COMPRA ? "compra" : "venda");
            List<LOAGrupoOfertas> listaOfertasLOA = (sentido == LivroOfertasBase.LIVRO_COMPRA ?
                livroInstrumento.getAgregadoCompra() : livroInstrumento.getAgregadoVenda());

            try
            {
                logger.DebugFormat("Instrumento[{0}] : Reset do livro de ofertas de {1}",
                    livroInstrumento.Instrumento,
                    descricaoCompraVenda);

                List<LOAGrupoOfertas> listaLOA = new List<LOAGrupoOfertas>();
                LivroOfertasBase.resetLivroLOA(livroInstrumento.Instrumento, listaOfertasLOA, listaLOA);

                for (int i = 0; i < listaLOA.Count; i++)
                {
                    LOAItemOferta LOAItem = new LOAItemOferta();
                    LOAItem.Acao = ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR;
                    LOAItem.Indice = listaLOA[i].Indice;
                    LOAItem.Preco = listaLOA[i].Preco;
                    LOAItem.QtdeOrdens = listaLOA[i].QtdeOrdens;
                    LOAItem.Quantidade = listaLOA[i].Quantidade;
                    ofertasLOA.Add(LOAItemOferta.montarRegistroAgregado(LOAItem, livroInstrumento.CasasDecimais));
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                logger.ErrorFormat("Instrumento[{0}]: erro ao Reiniciar LOF de {1}: {2} {3}",
                    livroInstrumento.Instrumento, descricaoCompraVenda, ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="umdfMessage"></param>
        /// <returns></returns>
        private LivroOfertasBase atualizaLivroOfertas(String msgType, Message umdfMessage, StreamTypeEnum streamType)
        {
            try
            {
                String securityID = "";
                String instrumento;

                if (msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_SECURITYLIST_SINGLE))
                {
                    GroupValue relatedSym;
                    if (umdfMessage.Template.HasField("RelatedSymbols") && umdfMessage.IsDefined("RelatedSymbols"))
                        relatedSym = umdfMessage.GetGroup("RelatedSymbols");
                    else
                        relatedSym = umdfMessage.GetGroup("RelatedSym");

                    instrumento = relatedSym.GetString("Symbol");
                    securityID = relatedSym.GetString("SecurityID");

                    logger.InfoFormat("Instrumento[{0}]: SecurityID[{1}]", instrumento, securityID);
                    dicSecurityID.AddOrUpdate(securityID, instrumento, (key, oldValue) => instrumento);

                    // Limpa o livro de ofertas se for inicio ou reinicio do canal UMDF
                    if (!String.IsNullOrEmpty(instrumento) && streamType==StreamTypeEnum.STREAM_TYPE_SECURITY_DEFINITION)
                    {
                        logger.Info("Resetando LOF e LOA de [" + instrumento + "]");
                        if (livrosOfertas.ContainsKey(instrumento))
                            livrosOfertas.Remove(instrumento);
                    }
                }
                else
                {
                    GroupValue mdEntry = umdfMessage.GetGroup("MDEntries");

                    if (umdfMessage.IsDefined("SecurityID"))
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
                                return null;
                            }
                        }
                    }
                }

                LivroOfertasBase retorno = null;
                if (!String.IsNullOrEmpty(instrumento))
                {
                    if ( livrosOfertas.TryGetValue(instrumento, out retorno) )
                    {
                        retorno.Instrumento = instrumento;
                    }
                    else
                    {
                        retorno = new LivroOfertasBase();
                        retorno.Instrumento = instrumento;

                        livrosOfertas.Add(instrumento, retorno);
                    }
                }

                return retorno;
            }
            catch (Exception ex)
            {
                logger.Error("atualizaLivroOfertas() Erro: " + ex.Message, ex);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instrumento"></param>
        /// <param name="umdfMessage"></param>
        private int obtemCasasDecimais(Message umdfMessage)
        {
            int casasDecimais = 2;
            try
            {
                GroupValue relatedSym;
                if (umdfMessage.Template.HasField("RelatedSymbols") && umdfMessage.IsDefined("RelatedSymbols"))
                    relatedSym = umdfMessage.GetGroup("RelatedSymbols");
                else
                    relatedSym = umdfMessage.GetGroup("RelatedSym");

                if (relatedSym.IsDefined("TickSizeDenominator"))
                    casasDecimais = relatedSym.GetInt("TickSizeDenominator");
            }
            catch (Exception ex)
            {
                logger.Error("obtemCasasDecimais() Erro: " + ex.Message, ex);
            }
            return casasDecimais;
        }

        public void geraMensagemTopoLivro(Message fastMsg, Decimal preco, long quantidade, String channelID)
	    {
		    try
		    {
	            MessageTemplate srcTemplate = fastMsg.Template;
	
	            // Template da mensagem
	            ScalarValue msgType;
	            if ( fastMsg.IsDefined("MsgType") )
	        	    msgType = fastMsg.GetScalar("MsgType");
	            else
	        	    msgType = fastMsg.GetScalar("MessageType");
	        	
	            ScalarValue msgSeqNum = fastMsg.GetScalar("MsgSeqNum");
	            ScalarValue sendingTime = fastMsg.GetScalar("SendingTime");
	            ScalarValue lastMsgSeqNumProc = null;
	            if (fastMsg.IsDefined("LastMsgSeqNumProcessed"))
	                lastMsgSeqNumProc = fastMsg.GetScalar("LastMsgSeqNumProcessed");

                if (fastMsg.IsDefined("ApplVerID"))
	                fastMsg.GetScalar("ApplVerID");

	            // Mensagem
	            Message instance = new Message(srcTemplate);

	            for( int i=0; i < fastMsg.FieldCount; i++)
	            {
	        	    if ( fastMsg.IsDefined(i) )
	        	    {
                        if ( !fastMsg.Template.Fields[i].TypeName.Equals("Group") &&
                            !fastMsg.Template.Fields[i].TypeName.Equals("Sequence") )
	        		    {
		        		    String value = new String(fastMsg.GetString(i).ToCharArray());
		        		    instance.SetString(i, value);
	        		    }
	        	    }
	            }
	        
    	        instance.SetFieldValue("MsgSeqNum", msgSeqNum);
	        
	            GroupValue oldMDEntry = fastMsg.GetGroup("MDEntries");
	            GroupValue newMDEntry = new GroupValue(oldMDEntry.GetGroup());
	        
	            for( int i=0; i < oldMDEntry.FieldCount; i++)
	            {
	        	    if ( oldMDEntry.IsDefined(i) )
	        	    {
	        		    String value = new String(oldMDEntry.GetString(i).ToCharArray());
	        		    newMDEntry.SetString(i, value);
	        	    }
	            }
	        
                logger.Debug("Gerando melhor oferta pr[" + preco.ToString() + "] qtde[" + quantidade.ToString() + "]");
	        
	            newMDEntry.SetLong("MDEntryPositionNo", 0);
                newMDEntry.SetString("MDEntryPx", preco.ToString());

	            newMDEntry.SetLong("MDEntrySize", quantidade);
	        
	            instance.SetFieldValue("MDEntries", newMDEntry);

                String segment = dicCanais[channelID].channelConfig.Segment;

	            //generateEsperEvent(instance, instance.Template.Id);
                MDSUtils.GenerateEsperEvent(
                    instance, 
                    instance.Template.Id, 
                    channelID, 
                    segment, 
                    ConstantesUMDF.FAST_MSGTYPE_MELHOR_OFERTA, 
                    ConstantesUMDF.UMDF_MARKETDEPTH_MARKET_BY_ORDER,
                    StreamTypeEnum.STREAM_TYPE_LOF_PROCESSOR);
		    }
		    catch(Exception ex)
		    {
			    logger.Error("geraMensagemTopoLivro() Erro: " + ex.Message, ex );
		    }
        }

        public void geraMensagemTopoLivroFix(QuickFix.FIX44.Message fixmsg, QuickFix.Group mdEntry, Decimal preco, long quantidade, String channelID)
        {
            try
            {
                QuickFix.FIX44.MessageFactory factory = new QuickFix.FIX44.MessageFactory();

                // New Message with beginstring & msgtype
                QuickFix.FIX44.MarketDataIncrementalRefresh instance = new QuickFix.FIX44.MarketDataIncrementalRefresh();
                //QuickFix.FIX44.Message instance = factory.Create(fixmsg.Header.GetString(QuickFix.Fields.Tags.BeginString), ConstantesUMDF.FAST_MSGTYPE_MELHOR_OFERTA);

                // Fill the header - 
                instance.Header.SetField(new QuickFix.Fields.MsgType(ConstantesUMDF.FAST_MSGTYPE_MELHOR_OFERTA));
                instance.Header.SetField(new QuickFix.Fields.MsgSeqNum(fixmsg.Header.GetInt(QuickFix.Fields.Tags.MsgSeqNum)));
                instance.Header.SetField(new QuickFix.Fields.SenderCompID(fixmsg.Header.GetString(QuickFix.Fields.Tags.SenderCompID)));
                instance.Header.SetField(new QuickFix.Fields.SendingTime(fixmsg.Header.GetDateTime(QuickFix.Fields.Tags.SendingTime)));
                instance.Header.SetField(new QuickFix.Fields.TargetCompID(fixmsg.Header.GetString(QuickFix.Fields.Tags.TargetCompID)));
                if (fixmsg.Header.IsSetField(QuickFix.Fields.Tags.PossDupFlag))
                    instance.Header.SetField(new QuickFix.Fields.PossDupFlag(fixmsg.Header.GetBoolean(QuickFix.Fields.Tags.PossDupFlag)));

                string securityID = "";
                string instrumento = "";
                string securityIDSource = "";
                string securityExchange = "";
                string side = "";
                string mdEntryType = "";

                if (fixmsg.IsSetField(QuickFix.Fields.Tags.SecurityID))
                    securityID = fixmsg.GetString(QuickFix.Fields.Tags.SecurityID);
                else
                {
                    if (mdEntry.IsSetField(QuickFix.Fields.Tags.SecurityID))
                        securityID = mdEntry.GetString(QuickFix.Fields.Tags.SecurityID);
                }

                if (fixmsg.IsSetField(QuickFix.Fields.Tags.Symbol))
                    instrumento = fixmsg.GetString(QuickFix.Fields.Tags.Symbol);
                else
                {
                    if (mdEntry.IsSetField(QuickFix.Fields.Tags.Symbol))
                        instrumento = mdEntry.GetString(QuickFix.Fields.Tags.Symbol);
                }

                if ( fixmsg.IsSetField(QuickFix.Fields.Tags.SecurityIDSource))
                    securityIDSource = fixmsg.GetString(QuickFix.Fields.Tags.SecurityIDSource);
                else
                {
                    if (mdEntry.IsSetField(QuickFix.Fields.Tags.SecurityIDSource))
                        securityIDSource = mdEntry.GetString(QuickFix.Fields.Tags.SecurityIDSource);
                }

                if ( fixmsg.IsSetField(QuickFix.Fields.Tags.SecurityExchange))
                    securityExchange = fixmsg.GetString(QuickFix.Fields.Tags.SecurityExchange);
                else
                {
                    if (mdEntry.IsSetField(QuickFix.Fields.Tags.SecurityExchange))
                        securityExchange = mdEntry.GetString(QuickFix.Fields.Tags.SecurityExchange);
                }

                if (fixmsg.IsSetField(QuickFix.Fields.Tags.Side))
                    side = fixmsg.GetString(QuickFix.Fields.Tags.Side);
                else
                {
                    if (mdEntry.IsSetField(QuickFix.Fields.Tags.Side))
                        side = mdEntry.GetString(QuickFix.Fields.Tags.Side);
                }

                if (fixmsg.IsSetField(QuickFix.Fields.Tags.MDEntryType) )
                    mdEntryType = fixmsg.GetString(QuickFix.Fields.Tags.MDEntryType);
                else
                {
                    if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntryType))
                        mdEntryType = mdEntry.GetString(QuickFix.Fields.Tags.MDEntryType);
                }

                QuickFix.Group NoMEntries = new QuickFix.FIX44.MarketDataIncrementalRefresh.NoMDEntriesGroup();


                if ( !String.IsNullOrEmpty(securityID) )
                    NoMEntries.SetField(new QuickFix.Fields.SecurityID(securityID));

                if (!String.IsNullOrEmpty(instrumento))
                    NoMEntries.SetField(new QuickFix.Fields.Symbol(instrumento));

                if (!String.IsNullOrEmpty(securityIDSource) )
                    NoMEntries.SetField(new QuickFix.Fields.SecurityIDSource(securityIDSource));

                if (!String.IsNullOrEmpty(securityExchange))
                    NoMEntries.SetField(new QuickFix.Fields.SecurityExchange(securityExchange));

                if ( !String.IsNullOrEmpty(side))
                    NoMEntries.SetField(new QuickFix.Fields.Side(side[0]));

                if (!String.IsNullOrEmpty(mdEntryType))
                    NoMEntries.SetField(new QuickFix.Fields.MDEntryType(mdEntryType[0]));

                NoMEntries.SetField(new QuickFix.Fields.MDEntryPositionNo(0));
                NoMEntries.SetField(new QuickFix.Fields.MDEntryPx(preco));
                NoMEntries.SetField(new QuickFix.Fields.MDEntrySize(quantidade));

                instance.AddGroup(NoMEntries);

                EventoFIX eventoFIX = new EventoFIX();
                eventoFIX.ChannelID = channelID;
                eventoFIX.MarketDepth = ConstantesUMDF.UMDF_MARKETDEPTH_MARKET_BY_ORDER;
                eventoFIX.Message = instance;
                eventoFIX.MsgSeqNum = instance.Header.GetInt(QuickFix.Fields.Tags.MsgSeqNum);
                eventoFIX.MsgType = ConstantesUMDF.FAST_MSGTYPE_MELHOR_OFERTA;
                eventoFIX.StreamType = StreamTypeEnum.STREAM_TYPE_TCP_CONFLATED;


                ContainerManager.Instance.EnqueueFIX(eventoFIX);
            }
            catch (Exception ex)
            {
                logger.Error("geraMensagemTopoLivroFix() Erro: " + ex.Message, ex);
            }
        }

        #endregion privates

        #region public
        public EventoHttpLivroOfertas SnapshotStreamerLivroOferta(string instrumento, string sessionID)
        {
            EventoHttpLivroOfertas retorno = new EventoHttpLivroOfertas();
            retorno.instrumento = instrumento;

            if (livrosOfertas.ContainsKey(instrumento))
            {
                lock (livrosOfertas[instrumento])
                {
                    LivroOfertasBase lofBase = livrosOfertas[instrumento];
                    retorno.cabecalho = MDSUtils.montaCabecalhoStreamer(ConstantesMDS.TIPO_REQUISICAO_LIVRO_OFERTAS, lofBase.TipoBolsa, ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_COMPLETO, instrumento, lofBase.CasasDecimais, sessionID);
                    retorno.livroCompra = LivroOfertasBase.montaLivroOfertasCompleto(lofBase, LivroOfertasBase.LIVRO_COMPRA, lofBase.CasasDecimais);
                    retorno.livroVenda = LivroOfertasBase.montaLivroOfertasCompleto(lofBase, LivroOfertasBase.LIVRO_VENDA, lofBase.CasasDecimais);
                }
            }


            return retorno;
        }

        public EventoHttpLivroOfertasAgregado SnapshotStreamerLivroAgregado(string instrumento, string sessionID)
        {
            EventoHttpLivroOfertasAgregado retorno = new EventoHttpLivroOfertasAgregado();
            retorno.instrumento = instrumento;

            if (livrosOfertas.ContainsKey(instrumento))
            {
                lock (livrosOfertas[instrumento])
                {
                    LivroOfertasBase lofBase = livrosOfertas[instrumento];
                    retorno.cabecalho = MDSUtils.montaCabecalhoStreamer(ConstantesMDS.TIPO_REQUISICAO_LIVRO_OFERTAS_AGREGADO, lofBase.TipoBolsa, ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_COMPLETO, instrumento, lofBase.CasasDecimais, sessionID);
                    retorno.livroCompra = LivroOfertasBase.montaLivroAgregadoCompleto(lofBase, LivroOfertasBase.LIVRO_COMPRA, lofBase.CasasDecimais);
                    retorno.livroVenda = LivroOfertasBase.montaLivroAgregadoCompleto(lofBase, LivroOfertasBase.LIVRO_VENDA, lofBase.CasasDecimais);
                }
            }

            return retorno;
        }

        public List<string> SnapshotHomeBrokerLivroOfertas()
        {
            List<string> retorno = new List<string>();
            KeyValuePair<string, LivroOfertasBase> [] copiaLivros = null;
            int numItens = 30;

            // ATP: em lugar de um lock para obter uma copia do dicionario de LOF
            // vamos efetuar retentativas caso haja alteracao durante a copia
            // pode atrasar a obtencao da copia, mas libera as threads de processamento
            // digamos que essa é uma "solucao emergencial de campo"
            bool bSnapshot = false;
            int tentativas = 0;
            while (!bSnapshot && tentativas < 5)
            {
                try
                {
                    copiaLivros = livrosOfertas.ToArray();
                    bSnapshot = true;
                }
                catch (Exception ex)
                {
                    logger.Warn("Erro ao copiar snapshot de LOF para o HB, retentando [" + ex.Message + "]");
                    Thread.Sleep(500);
                }
                tentativas++;
            }

            StringBuilder msgLivro = new StringBuilder();
            if (copiaLivros != null)
            {
                foreach(KeyValuePair<string, LivroOfertasBase> entry in copiaLivros)
                {
                    if (entry.Key != null && entry.Value != null)
                    {
                        string instrumento = entry.Key;
                        LivroOfertasBase livro = entry.Value;

                        msgLivro.Clear();
                        msgLivro.Append(MDSUtils.montaHeaderHomeBroker(instrumento, ConstantesMDS.TIPO_REQUISICAO_HB_LIVRO, livro.TipoBolsa));
                        msgLivro.Append(LivroOfertasBase.montaMensagemHomeBrokerLOF(livro.getLivroCompra(), numItens, LivroOfertasBase.LIVRO_COMPRA));
                        msgLivro.Append(LivroOfertasBase.montaMensagemHomeBrokerLOF(livro.getLivroVenda(), numItens, LivroOfertasBase.LIVRO_VENDA));

                        retorno.Add(msgLivro.ToString());
                    }
                }
            }

            return retorno;
        }

        public List<string> SnapshotHomeBrokerLivroAgregado()
        {
            List<string> retorno = new List<string>();
            KeyValuePair<string, LivroOfertasBase> [] copiaLivros = null;
            int numItens = 30;

            // ATP: em lugar de um lock para obter uma copia do dicionario de LOF
            // vamos efetuar retentativas caso haja alteracao durante a copia
            // pode atrasar a obtencao da copia, mas libera as threads de processamento
            // digamos que essa é uma "solucao emergencial de campo"
            bool bSnapshot = false;
            int tentativas = 0;
            while (!bSnapshot && tentativas < 5)
            {
                try
                {
                    copiaLivros = livrosOfertas.ToArray(); ;
                    bSnapshot = true;
                }
                catch (Exception ex)
                {
                    logger.Warn("Erro ao copiar snapshot de LOA para o HB, retentando [" + ex.Message + "]");
                    Thread.Sleep(500);
                }
                tentativas++;
            }

            StringBuilder msgLivro = new StringBuilder();
            if (copiaLivros != null)
            {
                foreach (KeyValuePair<string, LivroOfertasBase> entry in copiaLivros)
                {
                    if (entry.Key != null && entry.Value != null)
                    {
                        string instrumento = entry.Key;
                        LivroOfertasBase livro = entry.Value;

                        msgLivro.Clear();
                        msgLivro.Append(MDSUtils.montaHeaderHomeBroker(instrumento, ConstantesMDS.TIPO_REQUISICAO_HB_LIVRO_AGREGADO, livro.TipoBolsa));
                        msgLivro.Append(LivroOfertasBase.montaMensagemHomeBrokerAgregado(livro.getAgregadoCompra(), numItens, LivroOfertasBase.LIVRO_COMPRA));
                        msgLivro.Append(LivroOfertasBase.montaMensagemHomeBrokerAgregado(livro.getAgregadoVenda(), numItens, LivroOfertasBase.LIVRO_VENDA));

                        retorno.Add(msgLivro.ToString());
                    }
                }
            }

            return retorno;
        }
        #endregion public

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instrumento"></param>
        private void enviaSinalHB(string instrumento)
        {
            try
            {
                LivroOfertasBase livro = livrosOfertas[instrumento];

                StringBuilder builderAgregado = new StringBuilder();

                builderAgregado.Append(LivroOfertasBase.montaMensagemHomeBrokerAgregado(
                    livro.getAgregadoCompra(), 30, LivroOfertasBase.LIVRO_COMPRA));

                builderAgregado.Append(LivroOfertasBase.montaMensagemHomeBrokerAgregado(
                    livro.getAgregadoVenda(), 30, LivroOfertasBase.LIVRO_VENDA));

                EventoHBLivroOfertasAgregado eventoHBLoa = new EventoHBLivroOfertasAgregado();
                builderAgregado.Insert(0, MDSUtils.montaHeaderHomeBroker(
                    instrumento, ConstantesMDS.TIPO_REQUISICAO_HB_LIVRO_AGREGADO, livro.TipoBolsa));
                eventoHBLoa.instrumento = instrumento;
                eventoHBLoa.mensagem = builderAgregado.ToString();

                EventQueueManager.Instance.SendEvent(eventoHBLoa);

                /*ThreadPool.QueueUserWorkItem(
                    new WaitCallback(
                        delegate(object required)
                        {
                            try
                            {
                                EventoHBLivroOfertasAgregado eventoHBLoa = new EventoHBLivroOfertasAgregado();
                                builderAgregado.Insert(0, MDSUtils.montaHeaderHomeBroker(
                                    instrumento, ConstantesMDS.TIPO_REQUISICAO_HB_LIVRO_AGREGADO, livro.TipoBolsa));
                                eventoHBLoa.instrumento = instrumento;
                                eventoHBLoa.mensagem = builderAgregado.ToString();

                                EventQueueManager.Instance.SendEvent(eventoHBLoa);
                            }
                            catch (Exception ex)
                            {
                                logger.ErrorFormat("SendEvent(eventoHBLoa): {0} {1}", ex.Message, ex);
                            }
                        }
                    )
                );*/

                StringBuilder builderLOF = new StringBuilder();

                builderLOF.Append(LivroOfertasBase.montaMensagemHomeBrokerLOF(
                    livro.getLivroCompra(), 30, LivroOfertasBase.LIVRO_COMPRA));

                builderLOF.Append(LivroOfertasBase.montaMensagemHomeBrokerLOF(
                    livro.getLivroVenda(), 30, LivroOfertasBase.LIVRO_VENDA));

                EventoHBLivroOfertas eventoHBLof = new EventoHBLivroOfertas();
                builderLOF.Insert(0, MDSUtils.montaHeaderHomeBroker(
                    instrumento, ConstantesMDS.TIPO_REQUISICAO_HB_LIVRO, livro.TipoBolsa));
                eventoHBLof.instrumento = instrumento;
                eventoHBLof.mensagem = builderLOF.ToString();

                EventQueueManager.Instance.SendEvent(eventoHBLof);

                /*ThreadPool.QueueUserWorkItem(
                    new WaitCallback(
                        delegate(object required)
                        {
                            try
                            {
                                EventoHBLivroOfertas eventoHBLof = new EventoHBLivroOfertas();
                                builderLOF.Insert(0, MDSUtils.montaHeaderHomeBroker(
                                    instrumento, ConstantesMDS.TIPO_REQUISICAO_HB_LIVRO, livro.TipoBolsa));
                                eventoHBLof.instrumento = instrumento;
                                eventoHBLof.mensagem = builderLOF.ToString();

                                EventQueueManager.Instance.SendEvent(eventoHBLof);
                            }
                            catch (Exception ex)
                            {
                                logger.ErrorFormat("SendEvent(eventoHBLof): {0} {1}", ex.Message, ex);
                            }
                        }
                    )
                );*/
            }
            catch (Exception ex)
            {
                logger.Error("enviaSinalHB():" + ex.Message, ex);
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
                        logger.Info("Obtendo lista dos instrumentos pendentes a enviar LOF ao HB");

                        lastRun = DateTime.UtcNow.Ticks;
                        List<string> lstNotSent = timestampControl.ObterLOFNaoEnviados();

                        if (lstNotSent.Count > 0)
                        {

                            logger.Info("Lista com " + lstNotSent.Count + " instrumentos com sinal HB pendente. Enviando...");

                            foreach (string instrumento in lstNotSent)
                            {
                                enviaSinalHB(instrumento);
                            }

                            logger.Info("Enviado LOF de " + lstNotSent.Count + " instrumentos. Done.");
                        }
                        else
                        {
                            logger.Info("Nao ha sinal de LOF pendente de envio");
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
        /// Retorna o numero de casas decimais de um instrumento
        /// </summary>
        /// <param name="instrumento"></param>
        /// <returns></returns>
        public int RetornaCasasDecimais(string instrumento)
        {
            int casasDecimais = 2;

            LivroOfertasBase lof = null;

            if (livrosOfertas.TryGetValue(instrumento, out lof))
            {
                casasDecimais = lof.CasasDecimais;
            }

            return casasDecimais;
        }


        protected override void trataMensagemFIX(EventoFIX  evento)
        {
            try
            {
                LivroOfertasBase livroInstrumento;

                QuickFix.FIX44.Message message = evento.Message;

                String msgType = message.Header.GetString(QuickFix.Fields.Tags.MsgType);
                String msgID = message.Header.GetString(QuickFix.Fields.Tags.MsgSeqNum);
                String channelId = evento.ChannelID;
                //bool isPuma = dicCanais[channelId].channelConfig.IsPuma;
                //bool isPuma20 = dicCanais[channelId].channelConfig.IsPuma20;
                int marketDepth = ConstantesUMDF.UMDF_MARKETDEPTH_MARKET_BY_ORDER;
                bool isPuma = true;
                bool isPuma20 = true;
                
                if (logger.IsDebugEnabled)
                {
                    logger.DebugFormat("channelID ....: {0} (isPuma={1})", channelId, isPuma.ToString());
                    logger.DebugFormat("msgID  .......: {0}", msgID);
                    logger.DebugFormat("Message ......: {0}", message.ToString());
                }

                Dictionary<string, List<Dictionary<string, string>>> streamerSinalLOF = new Dictionary<string, List<Dictionary<string, string>>>();
                Dictionary<string, List<Dictionary<string, string>>> streamerSinalLOA = new Dictionary<string, List<Dictionary<string, string>>>();

                // Trata mensagens de Security List
				if ( msgType.Equals(QuickFix.FIX44.SecurityList.MsgType) )
				{
                    QuickFix.Group relatedSym = message.GetGroup(1, QuickFix.Fields.Tags.NoRelatedSym);

                    if (logger.IsDebugEnabled)
                    {
                        if (message.GroupCount(QuickFix.Fields.Tags.NoRelatedSym) > 0 )
                            logger.Debug(FIXUtils.writeGroup(relatedSym));
                    }

                    // SecurityReqID so eh setado se foi o MDS que solicitou a security list
                    bool resetLivros = message.IsSetField(QuickFix.Fields.Tags.SecurityReqID);

                    livroInstrumento = atualizaLivroOfertasFIX(msgType, message, resetLivros);
                    if (livroInstrumento == null)
                        return;

                    livroInstrumento.CasasDecimais = obtemCasasDecimaisFIX(message);
                    string securityType = relatedSym.GetString(QuickFix.Fields.Tags.SecurityType);

                    switch (securityType)
                    {
                        case "FUT":
                        case "SPOT":
                        case "SOPT":
                        case "FOPT":
                        case "DTERM":
                            livroInstrumento.TipoBolsa = ConstantesMDS.DESCRICAO_DE_BOLSA_BMF;
                            break;
                        default:
                            livroInstrumento.TipoBolsa = ConstantesMDS.DESCRICAO_DE_BOLSA_BOVESPA;
                            break;
                    }

                    logger.InfoFormat("Instrumento[{0}]: Inicializado TipoBolsa[{1}] CasasDecimais[{2}]",
                        livroInstrumento.Instrumento, livroInstrumento.TipoBolsa, livroInstrumento.CasasDecimais);

                    List<Dictionary<String, String>> ofertasLOA = new List<Dictionary<String, String>>();
                    List<Dictionary<String, String>> ofertasLOF = new List<Dictionary<String, String>>();

                    //resetLivroOfertas(LivroOfertasBase.LIVRO_COMPRA, livroInstrumento, ofertasLOA, ofertasLOF);
                    streamerSinalLOA.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_COMPRA, ofertasLOA);
                    streamerSinalLOF.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_COMPRA, ofertasLOF);

                    ofertasLOA = new List<Dictionary<String, String>>();
                    ofertasLOF = new List<Dictionary<String, String>>();
                    //resetLivroOfertas(LivroOfertasBase.LIVRO_VENDA, livroInstrumento, ofertasLOA, ofertasLOF);
                    streamerSinalLOA.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_VENDA, ofertasLOA);
                    streamerSinalLOF.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_VENDA, ofertasLOF);

                    //enviaSinalStreamer(livroInstrumento.Instrumento, streamerSinalLOF, streamerSinalLOA);

                    EventoHttpLivroOfertas eventoLOF = new EventoHttpLivroOfertas();

                    eventoLOF.cabecalho = MDSUtils.montaCabecalhoStreamer(
                        ConstantesMDS.TIPO_REQUISICAO_LIVRO_OFERTAS,
                        livrosOfertas[livroInstrumento.Instrumento].TipoBolsa,
                        ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_COMPLETO,
                        livroInstrumento.Instrumento,
                        livroInstrumento.CasasDecimais,
                        null);
                    eventoLOF.instrumento = livroInstrumento.Instrumento;

                    List<Dictionary<string, string>> livrostreamer = null;
                    if (streamerSinalLOF.TryGetValue(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_COMPRA, out livrostreamer))
                        eventoLOF.livroCompra = livrostreamer;

                    livrostreamer = null;
                    if (streamerSinalLOF.TryGetValue(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_VENDA, out livrostreamer))
                        eventoLOF.livroVenda = livrostreamer;

                    EventQueueManager.Instance.SendEvent(eventoLOF);

					return;
				}

                if (msgType.Equals(QuickFix.FIX44.SecurityStatus.MsgType))
                {
                    return;
                }

                // Trata apenas as mensagens de ofertas
                QuickFix.Group mdEntry = message.GetGroup(1,QuickFix.Fields.Tags.NoMDEntries);
                String mdEntryType = mdEntry.GetString(QuickFix.Fields.Tags.MDEntryType);

                if (msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_MELHOR_OFERTA) ||
                    (!mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_BID) &&
                    !mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_OFFER) &&
                    !mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_EMPTY_BOOK)))
                {
                    return;
                }

                // Testa se debug esta habilitado para nao invocar writegroup (+performatico)
                if (logger.IsDebugEnabled)
                {
                    logger.Debug(FIXUtils.writeGroup(mdEntry));
                }


                if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_EMPTY_BOOK) )
                    livroInstrumento = atualizaLivroOfertasFIX(msgType, message, true);
                else
                    livroInstrumento = atualizaLivroOfertasFIX(msgType, message, false);

                if (livroInstrumento == null)
                    return;

                if (msgType.Equals(QuickFix.FIX44.MarketDataSnapshotFullRefresh.MsgType))
                {
                    logger.Info("Processando snapshot de [" + livroInstrumento.Instrumento + "]");
                }

                string mdUpdateAction;
                if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDUpdateAction))
                    mdUpdateAction = mdEntry.GetString(QuickFix.Fields.Tags.MDUpdateAction);
                else
                    mdUpdateAction = ConstantesMDS.TIPO_ACAO_LOF_INCLUIR;

                // Monta Livro de Ofertas Por Ordem, Ofertas Por Preco (Agregado) e Topo do Livro, a partir de um único Canal UDMF
                lock (livrosOfertas[livroInstrumento.Instrumento])
                {
                    LOFDadosOferta dadosOfertaLOF = preparaDadosOfertaLOFFIX(livroInstrumento, mdEntryType, mdUpdateAction, mdEntry, isPuma20);

                    if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_BID))
                    {
                        contabilizaOfertasCompraOuVendaFIX(
                            LivroOfertasBase.LIVRO_COMPRA,
                            livroInstrumento,
                            mdEntryType,
                            mdUpdateAction,
                            mdEntry,
                            dadosOfertaLOF,
                            message,
                            streamerSinalLOF,
                            streamerSinalLOA,
                            channelId);
                    }
                    else if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_OFFER))
                    {
                        contabilizaOfertasCompraOuVendaFIX(
                            LivroOfertasBase.LIVRO_VENDA,
                            livroInstrumento,
                            mdEntryType,
                            mdUpdateAction,
                            mdEntry,
                            dadosOfertaLOF,
                            message,
                            streamerSinalLOF,
                            streamerSinalLOA,
                            channelId);
                    }

                    else if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_EMPTY_BOOK))
                    {
                        List<Dictionary<String, String>> ofertasLOA = new List<Dictionary<String, String>>();
                        List<Dictionary<String, String>> ofertasLOF = new List<Dictionary<String, String>>();

                        resetLivroOfertas(LivroOfertasBase.LIVRO_COMPRA, livroInstrumento, ofertasLOA, ofertasLOF);
                        streamerSinalLOA.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_COMPRA, ofertasLOA);
                        streamerSinalLOF.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_COMPRA, ofertasLOF);

                        ofertasLOA = new List<Dictionary<String, String>>();
                        ofertasLOF = new List<Dictionary<String, String>>();
                        resetLivroOfertas(LivroOfertasBase.LIVRO_VENDA, livroInstrumento, ofertasLOA, ofertasLOF);
                        streamerSinalLOA.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_VENDA, ofertasLOA);
                        streamerSinalLOF.Add(ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_VENDA, ofertasLOF);
                    }
                }

                if ( bGerarEventoStreamer )
                    enviaSinalStreamer(livroInstrumento.Instrumento, streamerSinalLOF, streamerSinalLOA);

                if (bGerarEventoHB && timestampControl.ShouldSendLOF(livroInstrumento.Instrumento))
                {
                    enviaSinalHB(livroInstrumento.Instrumento);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("trataMensagemFIX(): {0} {1}", ex.Message, ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sentido"></param>
        /// <param name="livroInstrumento"></param>
        /// <param name="mdEntryType"></param>
        /// <param name="mdUpdateAction"></param>
        /// <param name="mdEntry"></param>
        /// <param name="dadosOfertaLOF"></param>
        /// <param name="message"></param>
        /// <param name="streamerSinalOfertas"></param>
        /// <param name="streamerSinalAgregado"></param>
        /// <param name="channelID"></param>
        private void contabilizaOfertasCompraOuVendaFIX(int sentido, LivroOfertasBase livroInstrumento, string mdEntryType,
            string mdUpdateAction,
            QuickFix.Group mdEntry,
            LOFDadosOferta dadosOfertaLOF,
            QuickFix.FIX44.Message message,
            Dictionary<string, List<Dictionary<string, string>>> streamerSinalOfertas,
            Dictionary<string, List<Dictionary<string, string>>> streamerSinalAgregado, string channelID)
        {
            try
            {
                bool bGerarTopo = false;
                List<Dictionary<String, String>> ofertasAgregado = new List<Dictionary<String, String>>();
                List<Dictionary<String, String>> ofertasLOF = new List<Dictionary<String, String>>();

                long corretora = 0;
                int mdEntryBuyerSellerTag = (sentido == LivroOfertasBase.LIVRO_COMPRA ? QuickFix.Fields.Tags.MDEntryBuyer : QuickFix.Fields.Tags.MDEntrySeller);
                String descricaoCompraVenda = (sentido == LivroOfertasBase.LIVRO_COMPRA ? "compra" : "venda");
                List<LOFDadosOferta> listaOfertasLOF = (sentido == LivroOfertasBase.LIVRO_COMPRA ?
                    livroInstrumento.getLivroCompra() : livroInstrumento.getLivroVenda());
                List<LOAGrupoOfertas> listaOfertasLOA = (sentido == LivroOfertasBase.LIVRO_COMPRA ?
                    livroInstrumento.getAgregadoCompra() : livroInstrumento.getAgregadoVenda());

                if (mdEntry.IsSetField(mdEntryBuyerSellerTag))
                {
                    String corretoraPorExtenso = mdEntry.GetString(mdEntryBuyerSellerTag).Replace("BM", "").Trim();
                    corretora = Int64.Parse(corretoraPorExtenso);
                    dadosOfertaLOF.Corretora = corretora;
                }

                if (mdEntryType.Equals(ConstantesMDS.TIPO_ACAO_LOF_RESET))
                {
                    resetLivroOfertas(sentido, livroInstrumento, ofertasAgregado, ofertasLOF);

                    String descricaoHttpOfertasOcorrencias = (sentido == LivroOfertasBase.LIVRO_COMPRA ?
                        ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_COMPRA : ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_VENDA);
                    streamerSinalAgregado.Add(descricaoHttpOfertasOcorrencias, ofertasAgregado);
                    streamerSinalOfertas.Add(descricaoHttpOfertasOcorrencias, ofertasLOF);

                    return;
                }

                try
                {
                    if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_INCLUIR))
                    {
                        logger.DebugFormat("Instrumento[{0}] : Inclui oferta de {1} - posicao[{2}] preco[{3}] quantidade[{4}] corretora[{5}]",
                            livroInstrumento.Instrumento,
                            descricaoCompraVenda,
                            dadosOfertaLOF.Posicao,
                            String.Format("{0:f" + livroInstrumento.CasasDecimais + "}", Convert.ToDouble(dadosOfertaLOF.Preco)),
                            dadosOfertaLOF.Quantidade,
                            corretora);

                        ofertasLOF.Add(LOFDadosOferta.montarRegistroOferta(ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_INCLUIR, dadosOfertaLOF, livroInstrumento.CasasDecimais));

                        List<LOAItemOferta> listaOfertas = LivroOfertasBase.insereOferta(listaOfertasLOA, listaOfertasLOF, dadosOfertaLOF, sentido);

                        for (int i = 0; i < listaOfertas.Count; i++)
                        {
                            LOAItemOferta itemAgregado = listaOfertas[i];
                            ofertasAgregado.Add(LOAItemOferta.montarRegistroAgregado(itemAgregado, livroInstrumento.CasasDecimais));

                            if (itemAgregado.Indice == 0)
                                bGerarTopo = true;
                        }
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_ALTERAR))
                    {
                        logger.DebugFormat("Instrumento[{0}] : Altera oferta de {1} - posicao[{2}] preco[{3}] quantidade[{4}] corretora[{5}]",
                            livroInstrumento.Instrumento,
                            descricaoCompraVenda,
                            dadosOfertaLOF.Posicao,
                            String.Format("{0:f" + livroInstrumento.CasasDecimais + "}", Convert.ToDouble(dadosOfertaLOF.Preco)),
                            dadosOfertaLOF.Quantidade,
                            corretora);

                        List<LOAItemOferta> listaOfertas = LivroOfertasBase.alteraOferta(listaOfertasLOA, listaOfertasLOF, dadosOfertaLOF, sentido);

                        for (int i = 0; i < listaOfertas.Count; i++)
                        {
                            ofertasAgregado.Add(LOAItemOferta.montarRegistroAgregado(listaOfertas[i], livroInstrumento.CasasDecimais));
                            if (listaOfertas[i].Indice == 0)
                                bGerarTopo = true;
                        }

                        ofertasLOF.Add(LOFDadosOferta.montarRegistroOferta(ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_ALTERAR, dadosOfertaLOF, livroInstrumento.CasasDecimais));
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR))
                    {
                        try
                        {
                            List<LOAItemOferta> listaOfertas = LivroOfertasBase.removeOferta(listaOfertasLOA, listaOfertasLOF, dadosOfertaLOF);

                            for (int i = 0; i < listaOfertas.Count; i++)
                            {
                                LOAItemOferta itemOferta = listaOfertas[i];

                                logger.DebugFormat("Instrumento[{0}] : Exclui oferta de {1} - posicao[{2}] preco[{3}] id[{4}]",
                                    livroInstrumento.Instrumento,
                                    descricaoCompraVenda,
                                    dadosOfertaLOF.Posicao,
                                    String.Format("{0:f" + livroInstrumento.CasasDecimais + "}", Convert.ToDouble(dadosOfertaLOF.Preco)),
                                    dadosOfertaLOF.IDOferta);

                                ofertasAgregado.Add(LOAItemOferta.montarRegistroAgregado(itemOferta, livroInstrumento.CasasDecimais));

                                if (itemOferta.Indice == 0)
                                    bGerarTopo = true;
                            }

                            ofertasLOF.Add(LOFDadosOferta.montarRegistroOferta(ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR, dadosOfertaLOF, livroInstrumento.CasasDecimais));
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorFormat("Instrumento[{0}]: Nao encontrado posicao[{1}] do livro de ofertas de {2}",
                                livroInstrumento.Instrumento, dadosOfertaLOF.Posicao, descricaoCompraVenda);
                        }
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR_OFERTA_E_MELHORES))
                    {
                        try
                        {
                            logger.DebugFormat("Instrumento[{0}] : Exclui oferta de {1} e melhores - posicao[{2}] preco[{3}] id[{4}]",
                                livroInstrumento.Instrumento,
                                descricaoCompraVenda,
                                dadosOfertaLOF.Posicao,
                                String.Format("{0:f" + livroInstrumento.CasasDecimais + "}", Convert.ToDouble(dadosOfertaLOF.Preco)),
                                dadosOfertaLOF.IDOferta);

                            List<LOFDadosOferta> listaLOF = new List<LOFDadosOferta>();
                            List<LOAItemOferta> listaAgregado = LivroOfertasBase.removeOfertaEMelhores(
                                listaOfertasLOA, listaOfertasLOF, dadosOfertaLOF, sentido, listaLOF);

                            for (int i = 0; i < listaAgregado.Count; i++)
                            {
                                ofertasAgregado.Add(LOAItemOferta.montarRegistroAgregado(listaAgregado[i], livroInstrumento.CasasDecimais));
                            }

                            for (int i = 0; i < listaLOF.Count; i++)
                            {
                                ofertasLOF.Add(LOFDadosOferta.montarRegistroOferta(ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR, listaLOF[i], livroInstrumento.CasasDecimais));
                            }
                            bGerarTopo = true;
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorFormat("Instrumento[{0}]: Nao encontrado posicao[{1}] do livro de ofertas de {2}",
                                livroInstrumento.Instrumento, dadosOfertaLOF.Posicao, descricaoCompraVenda);
                        }
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR_OFERTAS_E_PIORES))
                    {
                        try
                        {
                            logger.DebugFormat("Instrumento[{0}] : Exclui oferta de {1} e piores - posicao[{2}] preco[{3}] id[{4}]",
                                livroInstrumento.Instrumento,
                                descricaoCompraVenda,
                                dadosOfertaLOF.Posicao,
                                String.Format("{0:f" + livroInstrumento.CasasDecimais + "}", Convert.ToDouble(dadosOfertaLOF.Preco)),
                                dadosOfertaLOF.IDOferta);

                            List<LOFDadosOferta> listaLOF = new List<LOFDadosOferta>();
                            List<LOAItemOferta> listaOfertas = LivroOfertasBase.removeOfertaEPiores(
                                listaOfertasLOA, listaOfertasLOF, dadosOfertaLOF, sentido, listaLOF);

                            for (int i = 0; i < listaOfertas.Count; i++)
                            {
                                ofertasAgregado.Add(LOAItemOferta.montarRegistroAgregado(listaOfertas[i], livroInstrumento.CasasDecimais));
                                if (listaOfertas[i].Indice == 0)
                                    bGerarTopo = true;
                            }

                            for (int i = 0; i < listaLOF.Count; i++)
                            {
                                ofertasLOF.Add(LOFDadosOferta.montarRegistroOferta(ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR, listaLOF[i], livroInstrumento.CasasDecimais));
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorFormat("Instrumento[{0}]: Nao encontrado posicao[{1}] do livro de ofertas de {2}",
                                livroInstrumento.Instrumento, dadosOfertaLOF.Posicao, descricaoCompraVenda);
                        }
                    }

                    else if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_RESET))
                    {
                        resetLivroOfertas(sentido, livroInstrumento, ofertasAgregado, ofertasLOF);
                    }

                    else
                    {
                        logger.ErrorFormat("Instrumento[{0}]: Falha na manutencao do Livro Oferta de {1} - mdUpdateAction[{2}]",
                            livroInstrumento.Instrumento, descricaoCompraVenda, mdUpdateAction);
                    }

                    String descricaoHttpOfertasOcorrencias = (sentido == LivroOfertasBase.LIVRO_COMPRA ?
                        ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_COMPRA : ConstantesMDS.HTTP_OFERTAS_OCORRENCIAS_VENDA);
                    streamerSinalAgregado.Add(descricaoHttpOfertasOcorrencias, ofertasAgregado);
                    streamerSinalOfertas.Add(descricaoHttpOfertasOcorrencias, ofertasLOF);
                }

                catch (Exception ex)
                {
                    logger.ErrorFormat("Instrumento[{0}]: Falha na manutencao do Livro Oferta de {1} - mdUpdateAction[{2}]: {3} {4}",
                        livroInstrumento.Instrumento, descricaoCompraVenda, mdUpdateAction, ex.Message, ex);
                }

                if (bGerarTopo /* && listaOfertasLOA.Count > 0 && ContainerManager.Instance.NegociosConsumer.IsNegociacao(livroInstrumento.Instrumento)*/)
                {
                    Decimal topbPreco = 0;
                    long topbQuantidade = 0;

                    if (listaOfertasLOA.Count > 0)
                    {
                        topbPreco = listaOfertasLOA[0].Preco;
                        topbQuantidade = listaOfertasLOA[0].Quantidade;
                    }

                    //Stopwatch sw = new Stopwatch();
                    //sw.Start();
                    EnqueueMensagemTopoLivroFix(message, mdEntry, topbPreco, topbQuantidade, channelID);

                    //sw.Stop();

                    //logger.Info("EnqueueMensagemTopoLivro: " + sw.ElapsedTicks);

                    //sw.Start();

                    //ThreadPool.QueueUserWorkItem(
                    //    new WaitCallback(
                    //        delegate(object required)
                    //        {
                    //            try
                    //            {
                    //                geraMensagemTopoLivro(message, topbPreco, topbQuantidade, channelID);
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                logger.ErrorFormat("geraMensagemTopoLivro({0}): {1} {2}", descricaoCompraVenda, ex.Message, ex);
                    //            }
                    //        }
                    //    )
                    //);
                    //sw.Stop();

                    //logger.Info("ThreadPool(geraMensagemTopoLivro): " + sw.ElapsedTicks + "ticks");
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("contabilizaOfertasCompraOuVendaFIX(): Falha no instrumento[{0}] {1} {2}",
                    livroInstrumento.Instrumento, ex.Message, ex);
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="livroInstrumento"></param>
        /// <param name="mdEntryType"></param>
        /// <param name="mdUpdateAction"></param>
        /// <param name="mdEntry"></param>
        /// <param name="dadosOfertaLOA"></param>
        private void preparaDadosOfertaLOA(LivroOfertasBase livroInstrumento, string mdEntryType, string mdUpdateAction, QuickFix.Group mdEntry, LOAGrupoOfertas dadosOfertaLOA)
        {
            try
            {
                if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_BID) ||
                    mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_OFFER))
                {
                    String posicaoSemFormato = mdEntry.GetInt(QuickFix.Fields.Tags.MDEntryPositionNo).ToString();
                    if (String.IsNullOrEmpty(posicaoSemFormato))
                    {
                        logger.Error("Instrumento[" + livroInstrumento.Instrumento + "]: Posicao inexistente");
                        return;
                    }
                    int posicao = Int32.Parse(posicaoSemFormato) - 1;

                    Decimal preco = new Decimal(0);
                    long quantidade = 0;
                    int qtdOrdens = 0;

                    if (!mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR) &&
                        !mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR_OFERTA_E_MELHORES) &&
                        !mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR_OFERTAS_E_PIORES))
                    {
                        String qtdOrdensSemFormato = mdEntry.GetInt(QuickFix.Fields.Tags.NumberOfOrders).ToString();
                        if (String.IsNullOrEmpty(qtdOrdensSemFormato))
                        {
                            logger.Error("Instrumento[" + livroInstrumento.Instrumento + "]: NumberOfOrders inexistente");
                            return;
                        }
                        qtdOrdens = Int32.Parse(qtdOrdensSemFormato);

                        if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntryPx))
                        {
                            String precoSemFormato = mdEntry.GetDecimal(QuickFix.Fields.Tags.MDEntryPx).ToString();
                            if (String.IsNullOrEmpty(precoSemFormato))
                            {
                                logger.Error("Instrumento[" + livroInstrumento.Instrumento + "]: Preco inexistente");
                                return;
                            }
                            preco = Decimal.Parse(precoSemFormato);
                        }

                        String quantidadeSemFormato = mdEntry.GetString(QuickFix.Fields.Tags.MDEntrySize);
                        if (String.IsNullOrEmpty(quantidadeSemFormato))
                        {
                            logger.Error("Instrumento[" + livroInstrumento.Instrumento + "]: Quantidade inexistente");
                            return;
                        }
                        quantidade = Int64.Parse(quantidadeSemFormato);
                    }

                    dadosOfertaLOA.Indice = posicao;
                    dadosOfertaLOA.Preco = preco;
                    dadosOfertaLOA.Quantidade = quantidade;
                    dadosOfertaLOA.QtdeOrdens = qtdOrdens;
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("preparaDadosOfertaLOA(): Falha no instrumento[{0}] {1} {2}",
                    livroInstrumento.Instrumento, ex.Message, ex);
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="livroInstrumento"></param>
        /// <param name="mdEntryType"></param>
        /// <param name="mdUpdateAction"></param>
        /// <param name="mdEntry"></param>
        /// <param name="isPuma20"></param>
        /// <returns></returns>
        private LOFDadosOferta preparaDadosOfertaLOFFIX(LivroOfertasBase livroInstrumento, string mdEntryType, string mdUpdateAction, QuickFix.Group mdEntry, bool isPuma20)
        {
            LOFDadosOferta dadosOferta = new LOFDadosOferta();

            try
            {
                if (!mdEntry.IsSetField(QuickFix.Fields.Tags.OrderID))
                {
                    if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR_OFERTA_E_MELHORES))
                    {
                        dadosOferta.IDOferta = "";
                    }
                    else
                    {
                        logger.Error("Instrumento[" + livroInstrumento.Instrumento + "]: OrderID inexistente");
                        return dadosOferta;
                    }
                }
                else
                {
                    dadosOferta.IDOferta = mdEntry.GetString(QuickFix.Fields.Tags.OrderID);
                }

                if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_BID) ||
                    mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_OFFER))
                {
                    String posicaoSemFormato = mdEntry.GetInt(QuickFix.Fields.Tags.MDEntryPositionNo).ToString();
                    if (String.IsNullOrEmpty(posicaoSemFormato))
                    {
                        logger.Error("Instrumento[" + livroInstrumento.Instrumento + "]: Posicao inexistente");
                        return dadosOferta;
                    }
                    int posicao = Int32.Parse(posicaoSemFormato) - 1;

                    String data = mdEntry.GetString(QuickFix.Fields.Tags.MDEntryDate);
                    String formatHora = (isPuma20 ? "{0,9:d9}" : "{0:d6}");
                    String horaOriginal = mdEntry.GetString(QuickFix.Fields.Tags.MDEntryTime).Replace(":","").Replace(".","");
                    String hora = UmdfUtils.convertUTC2Local(data, horaOriginal);
                    Decimal preco = new Decimal(0);
                    long quantidade = 0;

                    if (!mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR))
                    {
                        if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntryPx))
                        {
                            String precoSemFormato = mdEntry.GetDecimal(QuickFix.Fields.Tags.MDEntryPx).ToString();
                            if (String.IsNullOrEmpty(precoSemFormato))
                            {
                                logger.Error("Instrumento[" + livroInstrumento.Instrumento + "]: Preco inexistente");
                                return dadosOferta;
                            }
                            preco = Decimal.Parse(precoSemFormato);

                            if (posicao == 0)
                            {
                                if (mdEntryType.Equals(ConstantesUMDF.UMDF_MD_ENTRY_TYPE_BID))
                                    livroInstrumento.BestBidPx = preco;
                                else
                                    livroInstrumento.BestOfferPx = preco;
                            }

                        }
                        else
                        {
                            //Papel em leilao
                            dadosOferta.Leilao = true;
                            if (mdEntryType.Equals(ConstantesMDS.TIPO_REQUISICAO_BMF_LIVRO_OFERTAS_COMPRA))
                            {
                                Decimal auctionBidPx = livroInstrumento.BestBidPx;
                                preco = ConstantesMDS.PRECO_LIVRO_OFERTAS_MAX_VALUE;
                            }
                            else
                            {
                                Decimal auctionOfferPx = livroInstrumento.BestOfferPx;
                                preco = ConstantesMDS.PRECO_LIVRO_OFERTAS_MIN_VALUE;
                            }
                        }

                        String quantidadeSemFormato = "0";
                        if (mdEntry.IsSetField(QuickFix.Fields.Tags.MDEntrySize))
                        {
                            quantidadeSemFormato = mdEntry.GetString(QuickFix.Fields.Tags.MDEntrySize);
                        }
                        else
                        {
                            if (mdUpdateAction.Equals(ConstantesMDS.TIPO_ACAO_LOF_EXCLUIR_OFERTA_E_MELHORES))
                            {
                                quantidade = 0;
                            }
                            else
                            {
                                logger.Error("Instrumento[" + livroInstrumento.Instrumento + "]: Quantidade inexistente");
                                return dadosOferta;
                            }
                        }

                        quantidade = Int64.Parse(quantidadeSemFormato);
                    }

                    dadosOferta.Data = data;
                    dadosOferta.Hora = hora;
                    dadosOferta.Preco = preco;
                    dadosOferta.Quantidade = quantidade;
                    dadosOferta.Posicao = posicao;
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("preparaDadosOfertaLOFFIX(): Falha no instrumento[{0}] {1} {2}",
                    livroInstrumento.Instrumento, ex.Message, ex);
            }
            return dadosOferta;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private int obtemCasasDecimaisFIX(QuickFix.FIX44.Message message)
        {
            int casasDecimais = 2;
            try
            {
                QuickFix.Group relatedSym = message.GetGroup(1, QuickFix.Fields.Tags.NoRelatedSym);

                if (relatedSym.IsSetField(QuickFix.Fields.Tags.TickSizeDenominator))
                    casasDecimais = relatedSym.GetInt(QuickFix.Fields.Tags.TickSizeDenominator);
            }
            catch (Exception ex)
            {
                logger.Error("obtemCasasDecimaisFIX() Erro: " + ex.Message, ex);
            }
            return casasDecimais;
        }

        private LivroOfertasBase atualizaLivroOfertasFIX(string msgType, QuickFix.FIX44.Message message, bool resetLivro)
        {
            try
            {
                String securityID = "";
                String instrumento;

				if ( msgType.Equals(QuickFix.FIX44.SecurityList.MsgType) )
				{
                    QuickFix.Group relatedSym = message.GetGroup(1,QuickFix.Fields.Tags.NoRelatedSym);

                    instrumento = relatedSym.GetString(QuickFix.Fields.Tags.Symbol);
                    securityID = relatedSym.GetString(QuickFix.Fields.Tags.SecurityID);

                    logger.InfoFormat("Instrumento[{0}]: SecurityID[{1}]", instrumento, securityID);
                    dicSecurityID.AddOrUpdate(securityID, instrumento, (key, oldValue) => instrumento);

                    // Limpa o livro de ofertas se for inicio ou reinicio do canal UMDF
                    if (!String.IsNullOrEmpty(instrumento) && resetLivro)
                    {
                        logger.Info("Resetando LOF e LOA de [" + instrumento + "]");
                        if (livrosOfertas.ContainsKey(instrumento))
                            livrosOfertas.Remove(instrumento);
                    }
                }
                else
                {
                    QuickFix.Group mdEntry = message.GetGroup(1, QuickFix.Fields.Tags.NoMDEntries);

                    if (message.IsSetField(QuickFix.Fields.Tags.SecurityID))
                        securityID = message.GetString(QuickFix.Fields.Tags.SecurityID);
                    else
                    {
                        if (mdEntry.IsSetField(QuickFix.Fields.Tags.SecurityID))
                            securityID = mdEntry.GetString(QuickFix.Fields.Tags.SecurityID);
                    }

                    if (message.IsSetField(QuickFix.Fields.Tags.Symbol))
                        instrumento = message.GetString(QuickFix.Fields.Tags.Symbol);
                    else
                    {
                        if (mdEntry.IsSetField(QuickFix.Fields.Tags.Symbol))
                            instrumento = mdEntry.GetString(QuickFix.Fields.Tags.Symbol);
                        else
                        {
                            if (String.IsNullOrEmpty(securityID) || !dicSecurityID.TryGetValue(securityID, out instrumento))
                            {
                                logger.ErrorFormat("SecurityID[{0}] Nao pode resolver instrumento/securityID", securityID);
                                return null;
                            }
                        }
                    }
                }

                LivroOfertasBase retorno = null;
                if (!String.IsNullOrEmpty(instrumento))
                {
                    //if (resetLivro)
                    //{
                    //    logger.Info("Resetando LOF e LOA de [" + instrumento + "]");
                    //    if (livrosOfertas.ContainsKey(instrumento))
                    //        livrosOfertas.Remove(instrumento);
                    //}

                    if (livrosOfertas.TryGetValue(instrumento, out retorno))
                    {
                        retorno.Instrumento = instrumento;
                    }
                    else
                    {
                        retorno = new LivroOfertasBase();
                        retorno.Instrumento = instrumento;

                        livrosOfertas.Add(instrumento, retorno);
                    }
                }

                return retorno;
            }
            catch (Exception ex)
            {
                logger.Error("atualizaLivroOfertasFIX() Erro: " + ex.Message, ex);
            }
            return null;
        }

        private void EnqueueMensagemTopoLivro(Message message, Decimal topbPreco, long topbQuantidade, string channelID)
        {
            try
            {
                TopoLivroWorker topb = new TopoLivroWorker(message, topbPreco, topbQuantidade, channelID);
                //bool bsinaliza = queuesGeraTopo[currentTopBWorker].IsEmpty;
                queuesGeraTopo[currentTopBWorker].Enqueue(topb);
                /*if (bsinaliza)
                {
                    lock (syncQueuesGeraTopo[currentTopBWorker])
                    {
                        Monitor.Pulse(syncQueuesGeraTopo[currentTopBWorker]);
                    }
                }*/
                currentTopBWorker++;
                if (currentTopBWorker >= maxThreadGeraTopo)
                    currentTopBWorker = 0;
            }
            catch (Exception ex)
            {
                logger.Error("EnqueueMensagemTopoLivro: " + ex.Message, ex);
            }
        }


        private void EnqueueMensagemTopoLivroFix(QuickFix.FIX44.Message message,  QuickFix.Group mdentry, Decimal topbPreco, long topbQuantidade, string channelID)
        {
            try
            {
                TopoLivroWorkerFix topb = new TopoLivroWorkerFix(message, mdentry, topbPreco, topbQuantidade, channelID);
                //bool bsinaliza = queuesGeraTopo[currentTopBWorker].IsEmpty;

                queuesGeraTopoFix[currentTopBWorkerFix].Enqueue(topb);
                /*if (bsinaliza)
                {
                    lock (syncQueuesGeraTopo[currentTopBWorker])
                    {
                        Monitor.Pulse(syncQueuesGeraTopo[currentTopBWorker]);
                    }
                }*/
                currentTopBWorkerFix++;
                if (currentTopBWorkerFix >= maxThreadGeraTopo)
                    currentTopBWorkerFix = 0;
            }
            catch (Exception ex)
            {
                logger.Error("EnqueueMensagemTopoLivro: " + ex.Message, ex);
            }
        }


        private void geraTopoLivroWorker(object param)
        {
            int idx = (int) param;
            long lastLog = 0;
            long lastWatchDog = DateTime.UtcNow.Ticks;

            logger.Info("Iniciando geraTopoLivroWorker(" + idx + ")");

            while (bKeepRunning)
            {
                try
                {
                    TopoLivroWorker dados = null;

                    if (MDSUtils.shouldLog(lastWatchDog, 30))
                    {
                        logger.InfoFormat("geraTopoLivroWorker({0}) ativo: {1}", idx, queuesGeraTopo[idx].Count);
                        lastWatchDog = DateTime.UtcNow.Ticks;
                    }

                    if (queuesGeraTopo[idx].TryDequeue(out dados))
                    {
                        geraMensagemTopoLivro(dados.Mensagem, dados.TopBPreco, dados.TopBQtde, dados.ChannelID);

                        if (MDSUtils.shouldLog(lastLog, 2))
                        {
                            logger.InfoFormat("Fila geraTopoLivroWorker({0}): {1}", idx, queuesGeraTopo[idx].Count);
                            lastLog = DateTime.UtcNow.Ticks;
                        }

                        continue;
                    }

                    lock (syncQueuesGeraTopo[idx])
                    {
                        Monitor.Wait(syncQueuesGeraTopo[idx], 50);
                    }

                }
                catch (Exception ex)
                {
                    logger.Error("geraTopoLivroWorker(): " + ex.Message, ex);
                }
            }
        }


        private void geraTopoLivroWorkerFix(object param)
        {
            int idx = (int)param;
            long lastLog = 0;
            long lastWatchDog = DateTime.UtcNow.Ticks;

            logger.Info("Iniciando geraTopoLivroWorkerFix(" + idx + ")");

            while (bKeepRunning)
            {
                try
                {
                    TopoLivroWorkerFix dados = null;

                    if (MDSUtils.shouldLog(lastWatchDog, 30))
                    {
                        logger.InfoFormat("geraTopoLivroWorkerFix({0}) ativo: {1}", idx, queuesGeraTopo[idx].Count);
                        lastWatchDog = DateTime.UtcNow.Ticks;
                    }

                    if (queuesGeraTopoFix[idx].TryDequeue(out dados))
                    {
                        geraMensagemTopoLivroFix(dados.Mensagem, dados.MDEntry, dados.TopBPreco, dados.TopBQtde, dados.ChannelID);

                        if (MDSUtils.shouldLog(lastLog, 2))
                        {
                            logger.InfoFormat("Fila geraTopoLivroWorkerFix({0}): {1}", idx, queuesGeraTopo[idx].Count);
                            lastLog = DateTime.UtcNow.Ticks;
                        }

                        continue;
                    }

                    lock (syncQueuesGeraTopoFix[idx])
                    {
                        Monitor.Wait(syncQueuesGeraTopoFix[idx], 50);
                    }

                }
                catch (Exception ex)
                {
                    logger.Error("geraTopoLivroWorkerFix(): " + ex.Message, ex);
                }
            }
        }

    }
}
