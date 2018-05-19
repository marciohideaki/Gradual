using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.MDS.Core.Lib;
using OpenFAST;
using Gradual.MDS.Eventos.Lib;
using System.Threading;
using log4net;
using System.Net.Sockets;

namespace Gradual.MDS.Core
{
    public class MarketIncrementalProcessor : AsyncUdpClient
    {
        private int lastMsgSeqNum;
        private int seqContador;
        private ChannelUMDFState channelState;
        private ChannelUMDFConfig channelConfig;
        private String lastSendingTime = "";
        private MonitorConfig monitorConfig;
        //private bool primeiraMsgProcessada = false;
        
        private bool ProcessamentoMensagensEnabled;
        private object ProcessamentoMensagensLockObject;
        private object ReplayLockObject;
        private Queue<UdpPacket> qUdpPktOut = new Queue<UdpPacket>();
        private Queue<UdpPacket> qUdpPktReplay = new Queue<UdpPacket>();
        private string[] listaSecurityListTemplateID;
        private string[] listaMDIncrementalTemplateID;
        private string[] listaSecurityStatusTemplateID;
        private string[] listaNewsTemplateID;

        public MarketIncrementalProcessor(ChannelUMDFState state, ChannelUMDFConfig config, MonitorConfig monitorConfig) :
            base(config.MDIncrementalHost, config.MDIncrementalPorta, config.TemplateFile, config.ChannelID, config.LocalInterfaceAddress)
        {
            this.channelConfig = config;
            this.channelState = state;
            this.processorType = ProcessorType.MarketIncremental;
            this.monitorConfig = monitorConfig;

            ProcessamentoMensagensEnabled = false;
            ProcessamentoMensagensLockObject = new object();
            ReplayLockObject = new object();

            logger = LogManager.GetLogger("MarketIncrementalProcessor-" + config.ChannelID);

            MDSUtils.AddAppender("MarketIncrementalProcessor-" + config.ChannelID, logger.Logger);

            //if (int.Parse(channelConfig.ChannelID) <= 10)
            //    tcpReplayBMF = TCPReplayBMF.GetInstance(this, channelConfig, config.TemplateFile, qUdpPkt, ReplayLockObject);
            //else
            //    tcpReplayBovespa = new TCPReplayBovespa(this, channelConfig, config.TemplateFile, qUdpPkt, ReplayLockObject);

            //if (channelConfig.IsPuma)
            //    tcpReplayBMF = TCPReplayBMF.GetInstance(this, channelConfig, config.TemplateFile, qUdpPktReplay, ReplayLockObject);
            //else
            //    tcpReplayBovespa = new TCPReplayBovespa(this, channelConfig, config.TemplateFile, qUdpPktReplay, ReplayLockObject);

            fixInitiator = new FixInitiator(this, channelConfig, config.TemplateFile, qUdpPktReplay, ReplayLockObject);
        }

        public bool IsAlive()
        {
            if (bKeepRunning &&
                thUmdfMessageProcessor.IsAlive)
            {
                return true;
            }

            return false;
        }

        public void SolicitacaoReplay(int applBegSeqNum, int applEndSeqNum)
        {
            fixInitiator.SendMessageRequest(applBegSeqNum, applEndSeqNum);
        }

        /*public bool HabilitarProcessamentoMensagens
        {
            set
            {
                ProcessamentoMensagensEnabled = value;
            }
            get
            {
                return ProcessamentoMensagensEnabled;
            }
        }*/

        public void DiscardPackets(int lastSeqNumRecovery)
        {
            logger.Info("*** Inicio do Snapshot - descarta pacotes enfileirados antes do SeqNum[" + lastSeqNumRecovery + "]");
            if ( lastpkt < lastSeqNumRecovery )
                lastpkt = lastSeqNumRecovery;

            Message message;
            while( queueToProcessor.TryPeek(out message) )
            {
                if (message.IsDefined("MsgSeqNum"))
                {
                    int seqNum = message.GetInt("MsgSeqNum");

                    if (seqNum < lastSeqNumRecovery)
                    {
                        logger.Debug("Descartando mensagem [" + seqNum + "]");
                        queueToProcessor.TryDequeue(out message);
                        continue;
                    }

                    break;
                }
            }
            //lastpkt = 0;
        }

        public void StartPacketProcessing(int lastSeqNumRecovery)
        {
            if ( lastMsgSeqNum < lastSeqNumRecovery )
                lastMsgSeqNum = lastSeqNumRecovery;
            
            if (lastpkt < lastSeqNumRecovery)
                lastpkt = lastSeqNumRecovery;

            logger.Info("*** Fim do Snapshot - Trata pacotes enfileirados apos o SeqNum[" + lastSeqNumRecovery + "]");

            ProcessamentoMensagensEnabled = true;
            lock (ProcessamentoMensagensLockObject)
            {
                Monitor.Pulse(ProcessamentoMensagensLockObject);
            }
        }


        public int PauseIncremental()
        {
            try
            {
                if (clientSocketPRI != null)
                {
                    logger.Info("*** Fechando Conexao UDP");
                    clientSocketPRI.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DropMembership, mcoPRI);
                    clientSocketPRI.Close();
                    clientSocketPRI.Dispose();
                }

                if (clientSocketSEC != null)
                {
                    logger.Info("*** Fechando Conexao UDP Secundaria");
                    clientSocketSEC.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DropMembership, mcoSEC);
                    clientSocketSEC.Close();
                    clientSocketSEC.Dispose();
                }
            }
            catch (ObjectDisposedException)
            { }
            catch (Exception ex)
            {
                logger.Error("PauseIncremental(): " + ex.Message, ex);
            }

            return lastMsgSeqNum;
        }

        public void ResumeIncremental()
        {
            try
            {
                CreateSocket(multicastServerPRI, multicastPortPRI, multicastServerSEC, multicastPortSEC);
            }
            catch (ObjectDisposedException)
            { }
            catch (Exception ex)
            {
                logger.Error("ResumeIncremental(): " + ex.Message, ex);
            }
        }


        protected override void umdfPacketProcessor() { }

        protected override void umdfMessageProcessor()
        {
		    try
		    {
			    logger.Info("Carregando lista de Templates:");

                String securityListTemplateID = this.channelConfig.SecurityListTemplateID;
                listaSecurityListTemplateID = securityListTemplateID.Split(",".ToCharArray());

                String mdIncrementalTemplateID = this.channelConfig.MDIncrementalTemplateID;
                listaMDIncrementalTemplateID = mdIncrementalTemplateID.Split(",".ToCharArray());

                String securityStatusTemplateID = this.channelConfig.SecurityStatusTemplateID;
                listaSecurityStatusTemplateID = securityStatusTemplateID.Split(",".ToCharArray());

                String newsTemplateID = this.channelConfig.NewTemplateID;
                listaNewsTemplateID = newsTemplateID.Split(",".ToCharArray());

                foreach (string templateID in listaSecurityListTemplateID)
                {
                    logger.Info("SecurityList template [" + templateID + "]");
                }
                foreach (string templateID in listaMDIncrementalTemplateID)
                {
                    logger.Info("MDIncremental template [" + templateID + "]");
                }
                foreach (string templateID in listaSecurityStatusTemplateID)
                {
                    logger.Info("SecurityStatus template [" + templateID + "]");
                }
                foreach (string templateID in listaNewsTemplateID)
                {
                    logger.Info("News template [" + templateID + "]");
                }

                long lastLogTicks = 0;
	
			    while (bKeepRunning)
			    {
				    int seqNum = 0;
				    int tcpSeqNum = 0;

                    if (ProcessamentoMensagensEnabled == false)
                    {
                        lock (ProcessamentoMensagensLockObject)
                        {
                            Monitor.Wait(ProcessamentoMensagensLockObject, 100);
                            continue;
                        }
                    }

                    Message message;
                    if (!queueToProcessor.TryDequeue(out message))
                    {
                        lock (syncQueueToProcessor)
                        {
                            Monitor.Wait(syncQueueToProcessor, 50);
                        }
                        continue;
                    }

                    //TODO: revisar o algoritmo de conciliacao das filas

                    /*
                    if ( message == null )
                    {
                        logger.Debug("Timeout aguardando pacote FAST");
                        continue;
                    }*/

                    // Tratamento de Sequence Reset

                    //if (ConstantesMDSUMDF.FAST_MSGTYPE_SEQUENCERESET.equals(message.GetString("MsgType")))

                    string msgType;
                    if (message.Template.HasField("MsgType"))
                        msgType = message.GetString("MsgType");
                    else
                        msgType = message.GetString("MessageType");

                    if (msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_SEQUENCERESET))
                    {
                        lastMsgSeqNum = 0;
                        if (message.Template.HasField("NewSeqNo"))
                            lastMsgSeqNum = message.GetInt("NewSeqNo");
				        logger.InfoFormat("Recebido SequenceReset - lastMsgSeqNum[{0}]", lastMsgSeqNum);
				        
                        seqContador = lastMsgSeqNum;
                        continue;
                    }

                    if (msgType.Equals(ConstantesUMDF.FAST_MSGTYPE_HEARTBEAT))
                    {
                        logger.Info("Recebeu Hearbeat");
                        continue;
                    }

                    String sendingTime = message.GetString("SendingTime").Substring(0, 8);

                    // Descarta as mensagens ja recebidas no snapshot
                    if (message.IsDefined("MsgSeqNum"))
                    {
                        seqNum = message.GetInt("MsgSeqNum");

                        if (seqNum <= lastMsgSeqNum)
                        {
                            if (lastSendingTime.Equals(""))
                                lastSendingTime = sendingTime;

                            if (sendingTime.CompareTo(lastSendingTime) > 0)
                            {
                                logger.Info("Virou o dia! [" + sendingTime + "]");
                                lastMsgSeqNum = seqNum;
                                seqContador = lastMsgSeqNum;
                            }
                            else
                            {
                                logger.Info("Descartando msg [" + seqNum + "]");
                                continue;
                            }
                        }
                    }

                    /*
                    while ( this.filaTCPReplay.size() > 0 )
                    {
                        logger.Debug("Mensagens na fila do TCP-Replay: " + this.filaTCPReplay.size());
					
                        messageTcp = this.filaTCPReplay.take();
                        tcpSeqNum = messageTcp.getInt("MsgSeqNum");

                        if ( tcpSeqNum <= this.lastMsgSeqNum )
                        {
                            logger.Error("Descartando msg replay [" + tcpSeqNum + "]");
                            continue;
                        }
					
                        if (tcpSeqNum >= seqNum )
                        {
                            logger.Debug("tcpSeqNum[" + tcpSeqNum + "] >= seqNum[" + seqNum + "] - saindo do loop da fila TCP-Replay");
                            break;
                        }

                        logger.Debug("Processando msg replay [" + tcpSeqNum + "]");
					
                        processMD(messageTcp);
                        this.lastMsgSeqNum = tcpSeqNum;
                    }
                    */
                    if ( logger.IsDebugEnabled )
                        logger.Debug("Processando msg [" + message.ToString() + "]");

                    if (MDSUtils.shouldLog(lastLogTicks))
                    {
                        string convertedmsg = message.ToString();

                        lastLogTicks = DateTime.UtcNow.Ticks;
                        logger.Info("Tamanho da fila: " + this.queueToProcessor.Count);

                        string msgTruncado = (convertedmsg.Length < 200 ? convertedmsg : convertedmsg.Substring(0, 200));
                        monitorConfig.channels[channelConfig.ChannelID].dateTimeStartedStopped =
                            "Incr - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + msgTruncado;
                        monitorConfig.channels[channelConfig.ChannelID].AddDetails(
                            "3) Incremental", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), msgTruncado);
                    }

                    if (listaMDIncrementalTemplateID.Any(message.Template.Id.Contains) ||
                        listaSecurityListTemplateID.Any(message.Template.Id.Contains))
                    {
                        List<Message> mensagens = UmdfUtils.splitMessage(message, channelConfig.MarketDepth, ConstantesUMDF.FAST_MSGTYPE_INCREMENTAL_SINGLE);

                        string templateID = ConstantesUMDF.FAST_MSGTYPE_INCREMENTAL_SINGLE;
                        if (listaSecurityListTemplateID.Any(message.Template.Id.Contains))
                            templateID = ConstantesUMDF.FAST_MSGTYPE_SECURITYLIST_SINGLE;

                        foreach (Message newMessage in mensagens)
                        {
                            MDSUtils.EnqueueEventoUmdf(newMessage,
                                newMessage.Template.Id,
                                channelConfig.ChannelID,
                                channelConfig.Segment,
                                templateID,
                                channelConfig.MarketDepth,
                                StreamTypeEnum.STREAM_TYPE_MARKET_INCREMENTAL);
                        }
                    }
                    else if (listaSecurityStatusTemplateID.Any(message.Template.Id.Contains))
                    {
                        MDSUtils.EnqueueEventoUmdf(message,
                            message.Template.Id,
                            channelConfig.ChannelID,
                            channelConfig.Segment,
                            channelConfig.MarketDepth,
                            StreamTypeEnum.STREAM_TYPE_MARKET_INCREMENTAL);
                    }
                    else if (listaNewsTemplateID.Any(message.Template.Id.Contains))
                    {
                        MDSUtils.EnqueueEventoUmdfNews(message,
                            message.Template.Id,
                            channelConfig.ChannelID,
                            channelConfig.Segment,
                            ConstantesUMDF.FAST_MSGTYPE_NEWS,
                            StreamTypeEnum.STREAM_TYPE_MARKET_INCREMENTAL);
                    }
                    else
                    {
                        logger.Error("Template ID nao reconhecido [" + message.Template.Id + "]");
                    }
                    lastMsgSeqNum = seqNum;
                    lastSendingTime = sendingTime;
				
			    } // main loop
		
			    logger.Info("Fim");
		    }
		    catch(Exception ex)
		    {
			    logger.Error("Damn: " + ex.Message, ex);
			    bKeepRunning = false;
                monitorConfig.channels[channelConfig.ChannelID].RemoveDetails("3) Incremental");
            }
        }

        protected bool packetTimeWindow(Queue<UdpPacket> seqOut, int beginSeqNum, int endSeqNum)
        {
            logger.Info("Solicitando Replay  - Intervalo [" + beginSeqNum + "] -> [" + endSeqNum + "]");

            bool bResult = false;

            lock (ReplayLockObject)
            {
                SolicitacaoReplay(beginSeqNum, endSeqNum);

                bResult = Monitor.Wait(ReplayLockObject, 5000);
            }

            return bResult;
        }

        protected override void udpPacketProcessor()
        {
            int lastUdpReceived = 0;
            long lastWatchDog = 0;
            long seqNumInicialAnterior = 0;
            long seqNumFinalAnterior = 0;
            long lastLogUDP = 0;
            long lastForaOrdem = 0;
            int queueRead = 0;

            CreateSocket(multicastServerPRI, multicastPortPRI);

            while (bKeepRunning)
            {
                try
                {
                    TimeSpan tsWatchDog = new TimeSpan(DateTime.UtcNow.Ticks - lastWatchDog);

                    if (tsWatchDog.TotalMilliseconds > 30000)
                    {
                        logger.Info("Aguardando recepcao pacotes UDP");
                        lastWatchDog = DateTime.UtcNow.Ticks;
                    }

                    UdpPacket udppacket = null;
                    if ( qUdpPkt.TryDequeue( out udppacket)  && queueRead < 1000)
                    {
                        queueRead++;

                        UmdfPacket umdfpacket = new UmdfPacket(udppacket.byteData, 0, udppacket.pktLength);

                        if (logger.IsDebugEnabled)
                        {
                            logger.DebugFormat("Recebeu pacote: {0} {1}/{2} {3}",
                                umdfpacket.seqNum,
                                umdfpacket.noChunks,
                                umdfpacket.currChunk,
                                umdfpacket.msgLength);
                        }

                        // ATP: O chunk pode vir fora de sequencia.... o teste abaixo eh furado
                        //if (umdfpacket.seqNum == 1 && umdfpacket.currChunk == 1)
                        //{
                        //    logger.Info("Limpa toda a fila de mensagens de fora de ordem!");
                        //    qUdpPktOut.Clear();
                        //    lastpkt = 0;
                        //}

                        // Se o pacote recebido for anterior ao ultimo processado, despreza
                        if (umdfpacket.seqNum < (lastpkt + 1) && umdfpacket.seqNum!=1 && 
                            umdfpacket.currChunk < (lastChunk + 1) )
                        {
                            logger.Info("Despreza SeqNum[" + umdfpacket.seqNum + "] ja processado");
                            continue;
                        }

                        // Se o pacote recebido maior, mas nao consecutivo ao ultimo recebido,
                        // enfileira e solicita o intervalo
                        if (umdfpacket.seqNum > (lastUdpReceived + 1))
                        {
                            //if (lastpkt == 0 && umdfpacket.seqNum != 1)
                            //{
                            //    logger.Debug("Despreza pacote[" + umdfpacket.seqNum + "] ate o reinicio da lista");
                            //    continue;
                            //}

                            qUdpPktOut.Enqueue(udppacket);

                            //if (lastUdpReceived != 0 && channelConfig.IsPuma==true)
                            //{
                            //    logger.Debug("Perdeu pacote[" + (lastUdpReceived + 1) + "], reinfilera pacote[" + umdfpacket.seqNum + "] (Tam seqOut = " + qUdpPktOut.Count + ")");

                            //    packetTimeWindow(qUdpPktOut, lastUdpReceived + 1, umdfpacket.seqNum - 1);
                            //}

                            lastUdpReceived = umdfpacket.seqNum;
                            continue;
                        }

                        // Se for apenas maior que o ultimo processado, enfilera
                        // para reordenacao
                        if (umdfpacket.seqNum > (this.lastpkt + 1))
                        {
                            if (logger.IsDebugEnabled)
                            {
                                logger.Debug("Reenfileira pacote[" + (lastUdpReceived + 1) + "] para fila out (Tam seqOut = " + qUdpPktOut.Count + ")");
                            }
                            qUdpPktOut.Enqueue(udppacket);
                            lastUdpReceived = umdfpacket.seqNum;

                            continue;
                        }

                        if ( logger.IsDebugEnabled )
                            logger.Debug("Submetendo pacote [" + umdfpacket.seqNum + "] para fila de processamento");

                        //bool bsinaliza = qUmdfPacket.IsEmpty;
                        qUmdfPacket.Enqueue(umdfpacket);
                        //if (bsinaliza)
                        //{
                        //    lock (syncQueueUmdfPacket)
                        //    {
                        //        Monitor.Pulse(syncQueueUmdfPacket);
                        //    }
                        //}

                        lastUdpReceived = lastpkt = umdfpacket.seqNum;
                        if (umdfpacket.currChunk == umdfpacket.noChunks)
                            lastChunk = 0;
                        else
                            lastChunk = umdfpacket.currChunk;

                        if (MDSUtils.shouldLog(lastLogUDP))
                        {
                            logger.Info("Fila de pacotes UDP recebidos: " + qUdpPkt.Count);
                            lastLogUDP = DateTime.UtcNow.Ticks;
                        }

                        continue;
                    }

                    queueRead = 0;

                    if (MDSUtils.shouldLog(lastForaOrdem, 5))
                    {
                        logger.Info("Processando fila de pacotes fora de ordem (out) tamout=" + qUdpPktOut.Count);
                        lastForaOrdem = DateTime.UtcNow.Ticks;

                        if (qUdpPktOut.Count > 25000)
                        {
                            logger.Fatal("Holy shit, deve reiniciar o canal por excesso de pacotes aguardando processamento");

                            string msg = "Canal [" + this.channelConfig.ChannelID + "] precisa ser derrubado, pois nao fila esta acumulando.\r\n";
                            msg += "Ultima processada: " + lastMsgSeqNum;
                            string titulo = string.Format("Derrubando canal [{0}] por fila crescente", this.channelConfig.ChannelID);

                            bKeepRunning = false;
                        }
                    }

                    // Tenta processar pacotes recebidos via replay
                    int tentativa = 0;
                    while (qUdpPktReplay.Count > 0 && bKeepRunning && tentativa < qUdpPktReplay.Count)
                    {
                        tentativa++;
                        UdpPacket udppacket1 = null;
                        lock (this.qUdpPktReplay)
                        {
                            udppacket1 = qUdpPktReplay.Dequeue();
                        }

                        UmdfPacket umdfpacket = new UmdfPacket(udppacket1.byteData, 0, udppacket1.pktLength);

                        if (logger.IsDebugEnabled)
                        {
                            logger.DebugFormat("Recebeu pacote Replay : {0} {1}/{2} {3}",
                                umdfpacket.seqNum,
                                umdfpacket.noChunks,
                                umdfpacket.currChunk,
                                umdfpacket.msgLength);
                        }

                        if (umdfpacket.seqNum < (lastpkt + 1) && umdfpacket.seqNum!=1)
                        {
                            logger.Info("Despreza pacote replay SeqNum[" + umdfpacket.seqNum + "] ja processado");
                            continue;
                        }

                        if (umdfpacket.seqNum > (lastpkt + 1))
                        {
                            if (logger.IsDebugEnabled)
                            {
                                logger.Debug("Pacote replay ainda nao eh o esperado, reinfilera pacote[" + umdfpacket.seqNum + "]");
                            }

                            lock (qUdpPktReplay)
                            {
                                qUdpPktReplay.Enqueue(udppacket1);
                            }

                            continue;
                        }

                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug("Submetendo pacote replay [" + umdfpacket.seqNum + "] para fila de processamento");
                        }

                        //bool bsinaliza = qUmdfPacket.IsEmpty;
                        qUmdfPacket.Enqueue(umdfpacket);
                        //if (bsinaliza)
                        //{
                        //    lock (syncQueueUmdfPacket)
                        //    {
                        //        Monitor.Pulse(syncQueueUmdfPacket);
                        //    }
                        //}

                        lastpkt = umdfpacket.seqNum;

                        continue;
                    }

                    // Tenta processar pacotes fora de ordem....
                    //int tentativa = 0;
                    //tentativa = 0;
                    if (tentativa == 0)  // Se ja processou tudo via replay
                    {
                        while (qUdpPktOut.Count > 0 && bKeepRunning && tentativa < qUdpPktOut.Count)
                        {
                            tentativa++;
                            UdpPacket udppacket1 = null;
                            lock (qUdpPktOut)
                            {
                                udppacket1 = qUdpPktOut.Peek();
                            }

                            UmdfPacket umdfpacket = new UmdfPacket(udppacket1.byteData, 0, udppacket1.pktLength);

                            if (logger.IsDebugEnabled)
                            {
                                logger.DebugFormat("Recebeu pacote out: {0} {1}/{2} {3}",
                                    umdfpacket.seqNum,
                                    umdfpacket.noChunks,
                                    umdfpacket.currChunk,
                                    umdfpacket.msgLength);
                            }

                            if (umdfpacket.seqNum < (lastpkt + 1) && umdfpacket.seqNum != 1 && umdfpacket.noChunks==1)
                            {
                                logger.Info("Despreza out SeqNum[" + umdfpacket.seqNum + "] ja processado");
                                lock (qUdpPktOut)
                                {
                                    udppacket1 = qUdpPktOut.Dequeue();
                                }
                                continue;
                            }

                            if (!this.channelConfig.IsNewsChannel)
                            {
                                if (!this.channelConfig.IsPuma || (DateTime.Now.Hour >= 9 && DateTime.Now.Hour <= 18))
                                {
                                    if (umdfpacket.seqNum > (lastpkt + 1) && ProcessamentoMensagensEnabled)
                                    {
                                        if (logger.IsDebugEnabled)
                                        {
                                            logger.Debug("Out: Perdeu pacote [" + (lastpkt + 1) + "], reinfilera pacote[" + umdfpacket.seqNum + "]");
                                        }

                                        //lock (qUdpPktOut)
                                        //{
                                        //    qUdpPktOut.Enqueue(udppacket);
                                        //}

                                        //Sanity check....
                                        int seqNumInicial = lastpkt + 1;
                                        int seqNumFinal = umdfpacket.seqNum - 1;

                                        if (seqNumInicial <= seqNumFinal)
                                        {
                                            // Se expirou o TTL do pacote, descarta
                                            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - udppacket1.pktTimestamp);
                                            if (ts.TotalMilliseconds > 20 && lastpkt != 0 && 
                                                (seqNumInicialAnterior == 0 || seqNumFinalAnterior == 0 || seqNumInicial < seqNumInicialAnterior || seqNumInicial > seqNumFinalAnterior))
                                            {
                                                if (!packetTimeWindow(qUdpPktOut, seqNumInicial, seqNumFinal))
                                                {
                                                    logger.Fatal("Holy shit, nao recebeu replay, deve reiniciar o canal");

                                                    string msg = "Canal [" + this.channelConfig.ChannelID + "] precisa ser derrubado, pois nao recebeu resposta do replay.\r\n";
                                                    msg += "Timeout aguardando tcpreplay, ultima processada: " + lastMsgSeqNum;
                                                    string titulo = string.Format("Derrubando canal [{0}] por timeout replay", this.channelConfig.ChannelID);
                                                    
                                                    if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday && DateTime.Now.DayOfWeek != DayOfWeek.Saturday)
                                                    {
                                                        if (DateTime.Now.Hour >= ConstantesMDS.HORARIO_NOTIFICACAO_EMAIL_INICIO &&
                                                            DateTime.Now.Hour < ConstantesMDS.HORARIO_NOTIFICACAO_EMAIL_FIM)
                                                        {

                                                            MDSUtils.EnviarEmail(titulo, msg);
                                                        }
                                                    }

                                                    bKeepRunning = false;
                                                }
                                                seqNumInicialAnterior = seqNumInicial;
                                                seqNumFinalAnterior = seqNumFinal;
                                                break;
                                            }
                                        }

                                        break;
                                    }
                                }
                            }

                            if (logger.IsDebugEnabled)
                            {
                                logger.Debug("Submetendo pacote out[" + umdfpacket.seqNum + "] para fila de processamento");
                            }

                            lock (qUdpPktOut)
                            {
                                udppacket1 = qUdpPktOut.Dequeue();
                            }

                            //bool bsinaliza = qUmdfPacket.IsEmpty;
                            qUmdfPacket.Enqueue(umdfpacket);
                            //if (bsinaliza)
                            //{
                            //    lock (syncQueueUmdfPacket)
                            //    {
                            //        Monitor.Pulse(syncQueueUmdfPacket);
                            //    }
                            //}

                            lastpkt = umdfpacket.seqNum;
                        }
                    }

                    lock (syncQueueUdpPkt)
                    {
                        Monitor.Wait(syncQueueUdpPkt, 100);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("udpPacketProcessor: " + ex.Message, ex);
                }
            }
        }

    }
}
