using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickFix44;
using com.espertech.esper.compat.collections;
using log4net;
using QuickFix;
using System.Threading;

namespace Gradual.OMS.AutomacaoDesktop.Adapters
{
    public class SessaoFIXInstantaneo
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private LinkedBlockingQueue<MarketDataSnapshotFullRefresh> filaMensagensFIX;
        private LinkedBlockingQueue<string> filaMensagensRetransmissorBMF;
        private SortedDictionary<string, MarketDataSnapshotFullRefresh.NoMDEntries> listaInstrumentosFIX;

        private bool _bKeepRunning = false;
        private Thread _me = null;

        public SessaoFIXInstantaneo(
                LinkedBlockingQueue<MarketDataSnapshotFullRefresh> filaMensagensFIX,
                LinkedBlockingQueue<string> filaMensagensRetransmissorBMF)
        {
            this.filaMensagensFIX = filaMensagensFIX;
            this.filaMensagensRetransmissorBMF = filaMensagensRetransmissorBMF;
        }

        public void Start()
        {
            logger.Info("Iniciando thread SessaoFIXInstantaneo");
            _bKeepRunning = true;
            _me = new Thread(new ThreadStart(Run));
            _me.Start();
            logger.Info("Thread SessaoFIXInstantaneo iniciada");
        }

        public void Stop()
        {
            logger.Info("Finalizando SessaoFIXInstantaneo");
            _bKeepRunning = false;
            if (_me != null)
            {
                while (_me.IsAlive)
                    Thread.Sleep(250);
            }
            logger.Info("SessaoFIXInstantaneo finalizado");
        }

        private  void Run()
        {
            long antes;
            long depois;

            while (_bKeepRunning)
            {
                MarketDataSnapshotFullRefresh mensagem;
                try
                {
                    mensagem = filaMensagensFIX.Pop();
                }
                catch (Exception e)
                {
                    logger.Error("Falha na leitura da fila: " + e.Message);
                    break;
                }

                antes = DateTime.Now.Ticks;

                trataMensagemFIX(mensagem);

                depois = DateTime.Now.Ticks;
                TimeSpan elapsed = new TimeSpan(depois - antes);

                logger.Debug("Duracao: " + elapsed.TotalMilliseconds +
                        " ms (Mensagens na fila: " +
                        filaMensagensFIX.Count + ")");
            }
        }

        private void trataMensagemFIX(MarketDataSnapshotFullRefresh mensagem)
        {
            listaInstrumentosFIX = new SortedDictionary<string, MarketDataSnapshotFullRefresh.NoMDEntries>();

            try
            {
                int ocorrencias =
                    mensagem.getInt(NoMDEntries.FIELD);

                // Apenas trata a mensagem FIX se houver ocorrências no NoMDEntries
                if (ocorrencias > 0)
                {
                    uint ocorrencia;
                    for (ocorrencia = 1;
                        ocorrencia <= ocorrencias; ocorrencia++)
                    {
                        MarketDataSnapshotFullRefresh.NoMDEntries grupo = new MarketDataSnapshotFullRefresh.NoMDEntries();
                        mensagem.getGroup(ocorrencia, grupo);

                        // Mensagens de Negocio são enviados depois de todas as outras mensagens
                        if (grupo.getChar(MDEntryType.FIELD) == MDEntryType.TRADE)
                        {
                            if (!listaInstrumentosFIX.ContainsKey(
                                    grupo.getString(6032)))
                            {
                                listaInstrumentosFIX.Add(grupo.getString(6032), grupo);
                            }
                        }

                        enviaInstantaneo(
                                mensagem.getHeader().getInt(MsgSeqNum.FIELD),
                                ocorrencia,
                                mensagem,
                                grupo);
                    }

                    if (listaInstrumentosFIX.Count > 0 )
                    {
                        IEnumerator<KeyValuePair<string, MarketDataSnapshotFullRefresh.NoMDEntries>>
                            itens = listaInstrumentosFIX.GetEnumerator();
                        while (itens.MoveNext())
                        {
                            KeyValuePair<string, MarketDataSnapshotFullRefresh.NoMDEntries>
                                item = itens.Current;
                            enviaInstantaneo(
                                    Convert.ToInt32(item.Key),
                                    ocorrencia++,
                                    mensagem,
                                    item.Value);
                        }
                    }
                }
                else
                {
                    logger.Debug("SNAPSHOT - sem elementos");
                }
            }

            catch (FieldNotFound e)
            {
                logger.Error("Campo nao encontrado na mensagem: " + e.Message);
            }
        }

        /**
         * Monta e envia mensagem Market Data de Snapshot (instantaneo) para o ESPER.
         * Layout da mensagem:
         * SeqNum									9(15)
         * type										X(1)
         * symbol									X(20)
         * securityID								X(20)
         * body:
         * 		MDUpdateAction (279)				X(1)
         * 		MDEntryID (278)					X(10)
         * 		MDEntryDate (272)					X(8)
         * 		MDEntryTime (273)					X(8)
         *
         *		MDEntryPositionNo (290)			X(6)
         *		MDEntryPx (270)					X(15)
         *		MDEntrySize (271)					X(15)
         *		NumberOfOrders (346)				X(15)
         *		OrderID (37)						X(50)
         *		MDEntryBuyer (288)					X(10)
         *		MDEntrySeller (289)				X(10)
         *		TickDirection (274)				X(1)
         *		NetChgPrevDay (451)				X(11)
         *		UniqueTradeID (6032)				X(20)
         *		OpenCloseSettlFlag (286)			9(1)
         *		TradingSessionSubID (625)		X(2)
         *		SecurityTradingStatus (326)		9(3)
         *		NoReferentialPrices (6932)		9(3)
         *			ReferentialPriceType (6934)	9(1)
         *			ReferentialPx (6933)			X(15)
         *			TransactTime (60)				X(20)
         */
        private void enviaInstantaneo(
                int seqNum,
                uint ocorrencia,
                MarketDataSnapshotFullRefresh mensagem,
                MarketDataSnapshotFullRefresh.NoMDEntries grupo)
        {
            String mensagemMDS;
            String instrumento = null;

            try
            {
                // SeqNum - 9(15)
                mensagemMDS = seqNum.ToString("D10");
                mensagemMDS = mensagemMDS + ocorrencia.ToString("D5");

                // type - X(1)
                mensagemMDS = mensagemMDS + grupo.getChar(MDEntryType.FIELD);

                // symbol - X(20)
                instrumento = mensagem.getString(Symbol.FIELD);
                mensagemMDS = mensagemMDS + instrumento.PadLeft(20);

                // securityID - X(20)
                mensagemMDS = mensagemMDS + mensagem.getString(SecurityID.FIELD).PadLeft(20);

                // body

                // Se OFERTA_COMPRA ou OFERTA_VENDA, envia NEW em MDUpdateAction
                if (grupo.getChar(MDEntryType.FIELD) == MDEntryType.BID ||
                        grupo.getChar(MDEntryType.FIELD) == MDEntryType.OFFER)
                    mensagemMDS = mensagemMDS + MDUpdateAction.NEW;
                else
                    mensagemMDS = mensagemMDS + " ";

                mensagemMDS = mensagemMDS + " ".PadLeft(10);
                mensagemMDS = mensagemMDS + (grupo.isSet(new MDEntryDate()) ? grupo.getString(MDEntryDate.FIELD) : "").PadLeft(8);
                mensagemMDS = mensagemMDS + (grupo.isSet(new MDEntryTime()) ? grupo.getString(MDEntryTime.FIELD) : "").PadLeft(8).Trim();
                mensagemMDS = mensagemMDS + (grupo.isSet(new MDEntryPositionNo()) ? grupo.getString(MDEntryPositionNo.FIELD) : " ").PadLeft(6);
                mensagemMDS = mensagemMDS + (grupo.isSet(new MDEntryPx()) ? grupo.getString(MDEntryPx.FIELD) : " ").PadLeft(15);
                mensagemMDS = mensagemMDS + (grupo.isSet(new MDEntrySize()) ? grupo.getString(MDEntrySize.FIELD) : " ").PadLeft(15);
                mensagemMDS = mensagemMDS + (grupo.isSet(new NumberOfOrders()) ? grupo.getString(NumberOfOrders.FIELD) : " ").PadLeft(15);
                mensagemMDS = mensagemMDS + (grupo.isSet(new OrderID()) ? grupo.getString(OrderID.FIELD) : " ").PadLeft(50);
                mensagemMDS = mensagemMDS + (grupo.isSet(new MDEntryBuyer()) ? grupo.getString(MDEntryBuyer.FIELD) : " ").PadLeft(10);
                mensagemMDS = mensagemMDS + (grupo.isSet(new MDEntrySeller()) ? grupo.getString(MDEntrySeller.FIELD) : " ").PadLeft(10);
                mensagemMDS = mensagemMDS + (grupo.isSet(new TickDirection()) ? grupo.getString(TickDirection.FIELD) : " ").PadLeft(1);
                mensagemMDS = mensagemMDS + (mensagem.isSetField(NetChgPrevDay.FIELD) ? mensagem.getString(NetChgPrevDay.FIELD) : " ").PadLeft(11);
                mensagemMDS = mensagemMDS + (grupo.isSetField(6032) ? grupo.getString(6032) : " ").PadLeft(20);
                mensagemMDS = mensagemMDS + (grupo.isSet(new OpenCloseSettlFlag()) ? grupo.getInt(OpenCloseSettlFlag.FIELD) : 0).ToString("D1");
                mensagemMDS = mensagemMDS + (grupo.isSet(new TradingSessionSubID()) ? grupo.getString(TradingSessionSubID.FIELD) : " ").PadLeft(2);
                mensagemMDS = mensagemMDS + (grupo.isSetField(326) ? grupo.getInt(326) : 0).ToString("D3");

                if (grupo.isSetField(6932))
                {
                    int ocorrencias = grupo.getInt(6932);
                    mensagemMDS = mensagemMDS + ocorrencias.ToString("D3");

                    for (uint ocorrenciaPrecos = 1;
                        ocorrenciaPrecos <= ocorrencias; ocorrenciaPrecos++)
                    {
                        //ATP: Sinistraço!!!!
                        Group grupoPrecos = new Group(6932, 6934, new int[] { 6934, 6933, 60, 0 });
                        grupo.getGroup(ocorrenciaPrecos, grupoPrecos);

                        mensagemMDS = mensagemMDS + (grupoPrecos.isSetField(6934) ? grupoPrecos.getInt(6934).ToString("D1") : "0");
                        mensagemMDS = mensagemMDS + (grupoPrecos.isSetField(6933) ? grupoPrecos.getString(6933) : " ").PadLeft(15);
                        mensagemMDS = mensagemMDS + (grupoPrecos.isSetField(TransactTime.FIELD) ? grupoPrecos.getString(TransactTime.FIELD) : "").PadLeft(20);
                    }
                }
                else
                {
                    mensagemMDS = mensagemMDS + "000";
                }

                try
                {
                    filaMensagensRetransmissorBMF.Push(mensagemMDS);
                }
                catch (Exception e)
                {
                    logger.Error("Falha na leitura da fila filaMensagensRetransmissorBMF: " +
                            e.Message,e);
                }

                logger.Debug("SNAPHOT - Instrumento[" +
                        mensagem.getString(Symbol.FIELD) + "] tipo[" +
                        grupo.getChar(MDEntryType.FIELD) + "] Mensagem[" +
                        mensagemMDS + "]");
            }
            catch (FieldNotFound e)
            {
                logger.Error("Instrumento[" + instrumento +
                        "]: Campo nao encontrado na mensagem: " + e.Message);
                return;
            }
            catch (Exception e)
            {
                logger.Error("enviaInstantaneo: " + e.Message, e);
            }
        }
    }
}
