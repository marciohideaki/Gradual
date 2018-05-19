using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using log4net;
using System.Threading;
using OpenFAST.Template.Loader;
using OpenFAST.Template;
using OpenFAST;
using OpenFAST.Codec;
using Gradual.MDS.Core.Lib;
using System.Collections.Concurrent;
using System.IO;

namespace Gradual.MDS.Core
{
    public abstract class AsyncUdpClient
    {
        protected ILog logger;

        protected enum ProcessorType
        {
            SecurityList = 1,
            MarketRecovery,
            MarketIncremental
        }

        protected ProcessorType processorType;

        protected Socket clientSocketPRI; //The main client socket
        protected EndPoint epServerPRI;   //The EndPoint of the server
        protected Socket clientSocketSEC; //The main client socket
        protected EndPoint epServerSEC;   //The EndPoint of the server

        public const int BUFFER_SIZE = 1500;
        public const long PACKET_TIME_WINDOW = 100;


        private TemplateRegistry registry = null;
        protected Thread thUdpPacketProcessor;
        protected Thread thUmdfPacketAssembler;
        protected Thread thUmdfPacketProcessor;
        protected Thread thUmdfMessageProcessor;
        protected IPAddress ipAddressPRI;
        protected MulticastOption mcoPRI;
        protected IPAddress ipAddressSEC;
        protected MulticastOption mcoSEC;
        private byte[] byteDataPRI = new byte[BUFFER_SIZE];
        private byte[] byteDataSEC = new byte[BUFFER_SIZE];

        protected string multicastServerPRI;
        protected int multicastPortPRI;
        protected string multicastServerSEC;
        protected int multicastPortSEC;
        protected string localInterfaceAddress;
        private string templateFile;
        private string channelID;
        protected FixInitiator fixInitiator = null;

        protected ConcurrentQueue<UmdfPacket> qUmdfPacket = new ConcurrentQueue<UmdfPacket>();
        protected ConcurrentQueue<UdpPacket> qUdpPkt = new ConcurrentQueue<UdpPacket>();
        protected ConcurrentQueue<Message> queueToProcessor = new ConcurrentQueue<Message>();
        protected Object syncQueueToProcessor = new Object();
        protected Object syncQueueUmdfPacket = new Object();
        protected Object syncQueueUdpPkt = new Object();
        protected bool bKeepRunning;
        protected int lastpkt = 0;
        protected int lastChunk = 0;
        protected long lastLogPRI = 0;
        protected long lastLogSEC = 0;



        public AsyncUdpClient(string multicastserverPRI, int multicastportPRI, string templatefile, string chanID, string localBindAddress)
        {
            multicastPortPRI = multicastportPRI;
            multicastServerPRI = multicastserverPRI;
            templateFile = templatefile;
            channelID = chanID;
            localInterfaceAddress = localBindAddress;
        }

        public AsyncUdpClient(string multicastserverPRI, int multicastportPRI, string multicastserverSEC, int multicastportSEC, string templatefile, string chanID, string localBindAddress)
        {
            multicastPortPRI = multicastportPRI;
            multicastServerPRI = multicastserverPRI;
            multicastPortSEC = multicastportSEC;
            multicastServerSEC = multicastserverSEC;
            templateFile = templatefile;
            channelID = chanID;
            localInterfaceAddress = localBindAddress;
        }

        public virtual void Start()
        {
            bKeepRunning = true;
            XMLMessageTemplateLoader loader = new XMLMessageTemplateLoader();
            registry = UmdfUtils.loadTemplates(templateFile);

            thUdpPacketProcessor = new Thread(new ThreadStart(udpPacketProcessor));
            thUdpPacketProcessor.Name = "threadUdpPacketProcessor" + channelID;
            thUdpPacketProcessor.Start();

            thUmdfPacketAssembler = new Thread(new ThreadStart(umdfPacketAssembler));
            thUmdfPacketAssembler.Name = "threadUmdfPacketAssembler" + channelID;
            thUmdfPacketAssembler.Start();

            thUmdfPacketProcessor = new Thread(new ThreadStart(umdfPacketProcessor));
            thUmdfPacketProcessor.Name = "threadUmdfPacketProcessor-" + channelID;
            thUmdfPacketProcessor.Start();

            thUmdfMessageProcessor = new Thread(new ThreadStart(umdfMessageProcessor));
            thUmdfMessageProcessor.Name = "threadUmdfMessageProcessor-" + channelID;
            thUmdfMessageProcessor.Start();

        }


        public virtual void Stop()
        {
            bKeepRunning = false;
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

                if (fixInitiator != null)
                {
                    fixInitiator.Stop();
                    fixInitiator = null;
                    logger.Info("*** Fechando Conexao FixInitiator");
                }
            }
            catch (ObjectDisposedException)
            { }
            catch (Exception ex)
            {
                logger.Error("Stop(): " + ex.Message, ex);
            }
        }

        public void WaitStop()
        {
            try
            {
                while (thUdpPacketProcessor != null && thUdpPacketProcessor.IsAlive)
                {
                    logger.Info("Aguardando finalizacao da thread UDP PacketProcessor");
                    Thread.Sleep(1000);
                }

                while (thUmdfMessageProcessor != null && thUmdfMessageProcessor.IsAlive)
                {
                    logger.Info("Aguardando finalizacao da thread UMDF MessageProcessor");
                    Thread.Sleep(1000);
                }

                while (thUmdfPacketProcessor != null && thUmdfPacketProcessor.IsAlive)
                {
                    logger.Info("Aguardando finalizacao da thread UMDF PacketProcessor ");
                    Thread.Sleep(1000);
                }
                while (thUmdfPacketAssembler != null && thUmdfPacketAssembler.IsAlive)
                {
                    logger.Info("Aguardando finalizacao da thread UMDF PacketAssembler");
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Stop(): " + ex.Message, ex);
            }
        }


        protected virtual void CreateSocket(string mcastserverPRI, int mcastportPRI)
        {
            try
            {
                bKeepRunning = true;

                logger.Info("Criando socket para host:porta [" + mcastserverPRI + ":" + mcastportPRI + "]");

                //Using UDP sockets
                clientSocketPRI = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                clientSocketPRI.ReceiveBufferSize = 1024 * 1024 * 16;
                clientSocketPRI.UseOnlyOverlappedIO = true;

                //IP address of the server machine
                ipAddressPRI = IPAddress.Parse(mcastserverPRI);
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, mcastportPRI);
                clientSocketPRI.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                clientSocketPRI.ExclusiveAddressUse = false;
                clientSocketPRI.Bind(localEndPoint);

                if (!String.IsNullOrEmpty(localInterfaceAddress) && !localInterfaceAddress.ToLowerInvariant().Equals("any"))
                {
                    logger.Info("Binding local interface: [" + localInterfaceAddress + "]");
                    mcoPRI = new MulticastOption(ipAddressPRI, IPAddress.Parse(localInterfaceAddress));
                }
                else
                {
                    logger.Info("Binding PRI all interfaces"); 
                    mcoPRI = new MulticastOption(ipAddressPRI, IPAddress.Any);
                }
                clientSocketPRI.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcoPRI);
                clientSocketPRI.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 200);
                IPEndPoint serverEndPoint = new IPEndPoint(ipAddressPRI, mcastportPRI);

                epServerPRI = (EndPoint)serverEndPoint;

                logger.Info("Joining primary group...");

                byteDataPRI = new byte[BUFFER_SIZE];

                logger.Info("Start primary receiving");

                clientSocketPRI.BeginReceiveFrom(byteDataPRI, 0, byteDataPRI.Length, SocketFlags.None, ref epServerPRI,
                                           new AsyncCallback(this.OnReceivePRI), null);
            }
            catch (Exception ex)
            {
                logger.Error("CreateSocket(): " + ex.Message, ex);
                bKeepRunning = false;
            }
        }

        protected virtual void CreateSocket(string mcastserverPRI, int mcastportPRI, string mcastserverSEC, int mcastportSEC)
        {
            try
            {
                bKeepRunning = true;

                logger.Info("Criando socket para host:porta [" + mcastserverPRI + ":" + mcastportPRI + "]");

                //Using UDP sockets
                clientSocketPRI = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                clientSocketPRI.ReceiveBufferSize = 1024 * 1024 * 16;
                clientSocketPRI.UseOnlyOverlappedIO = true;

                //IP address of the server machine
                ipAddressPRI = IPAddress.Parse(mcastserverPRI);
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, mcastportPRI);
                clientSocketPRI.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                clientSocketPRI.ExclusiveAddressUse = false; 
                clientSocketPRI.Bind(localEndPoint);

                if (!String.IsNullOrEmpty(localInterfaceAddress) && !localInterfaceAddress.ToLowerInvariant().Equals("any"))
                {
                    logger.Info("Binding PRI local interface: [" + localInterfaceAddress + "]");
                    mcoPRI = new MulticastOption(ipAddressPRI, IPAddress.Parse(localInterfaceAddress));
                }
                else
                {
                    logger.Info("Binding PRI all interfaces");
                    mcoPRI = new MulticastOption(ipAddressPRI, IPAddress.Any);
                }
                clientSocketPRI.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcoPRI);
                clientSocketPRI.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 200);
                IPEndPoint serverEndPoint = new IPEndPoint(ipAddressPRI, mcastportPRI);

                epServerPRI = (EndPoint)serverEndPoint;

                logger.Debug("Joining primary group...");

                byteDataPRI = new byte[BUFFER_SIZE];

                logger.Info("Start primary receiving");

                clientSocketPRI.BeginReceiveFrom(byteDataPRI, 0, byteDataPRI.Length, SocketFlags.None, ref epServerPRI,
                                           new AsyncCallback(this.OnReceivePRI), null);

                if (!String.IsNullOrEmpty(mcastserverSEC))
                {
                    logger.Info("Criando socket para host:porta [" + mcastserverSEC + ":" + mcastportSEC + "]");

                    //Using UDP sockets
                    clientSocketSEC = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    clientSocketSEC.ReceiveBufferSize = 1024 * 1024 * 16;
                    clientSocketSEC.UseOnlyOverlappedIO = true;

                    //IP address of the server machine
                    ipAddressSEC = IPAddress.Parse(mcastserverSEC);
                    localEndPoint = new IPEndPoint(IPAddress.Any, mcastportSEC);
                    clientSocketSEC.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    clientSocketSEC.ExclusiveAddressUse = false;
                    clientSocketSEC.Bind(localEndPoint);

                    if (!String.IsNullOrEmpty(localInterfaceAddress) && !localInterfaceAddress.ToLowerInvariant().Equals("any"))
                    {
                        logger.Info("Binding SEC local interface: [" + localInterfaceAddress + "]");
                        mcoSEC = new MulticastOption(ipAddressSEC, IPAddress.Parse(localInterfaceAddress));
                    }
                    else
                    {
                        logger.Info("Binding SEC all interfaces");
                        mcoSEC = new MulticastOption(ipAddressSEC, IPAddress.Any);
                    }
                    clientSocketSEC.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcoSEC);
                    clientSocketSEC.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 200);
                    serverEndPoint = new IPEndPoint(ipAddressSEC, mcastportSEC);

                    epServerSEC = (EndPoint)serverEndPoint;

                    logger.Debug("Joining secondary group...");

                    byteDataSEC = new byte[BUFFER_SIZE];

                    logger.Debug("Start secondary receiving");

                    clientSocketSEC.BeginReceiveFrom(byteDataSEC, 0, byteDataSEC.Length, SocketFlags.None, ref epServerSEC,
                                               new AsyncCallback(this.OnReceiveSEC), null);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CreateSocket(): " + ex.Message, ex);
                bKeepRunning = false;
            }
        }

        public virtual void Close()
        {
            try
            {
                //clientSocket.DropMulticastGroup(ipAddress);
                clientSocketPRI.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DropMembership, mcoPRI);
                clientSocketPRI.Close();
            }
            catch (Exception ex)
            {
                logger.Error("Close(): " + ex.Message, ex);
            }
        }

        //public void ReceiveCallback(IAsyncResult ar)
        //{
        //    //UdpClient u = (UdpClient)ar.AsyncState;
        //    IPEndPoint e = (IPEndPoint)ar.AsyncState;
        //    byte[] byteData = clientSocket.EndReceive(ar, ref e);

        //    clientSocket.BeginReceive(new AsyncCallback(ReceiveCallback), e);

        //    UdpPacket packet = new UdpPacket();
        //    packet.byteData = byteData;
        //    packet.pktTimestamp = DateTime.Now.Ticks;


        //    lock (qpkt)
        //    {
        //        qpkt.Enqueue(packet);
        //        Monitor.Pulse(qpkt);
        //    }
        //    //string receiveString = Encoding.ASCII.GetString(receiveBytes);

        //}

        protected virtual void udpPacketProcessor()
        {
            //long ultimoTimeStamp = 0;
            Queue<UdpPacket> qUdpPktOut = new Queue<UdpPacket>();

            CreateSocket(multicastServerPRI, multicastPortPRI, multicastServerSEC, multicastPortSEC);

            while (bKeepRunning)
            {
                UdpPacket udppacket = null;

                if ( qUdpPkt.TryDequeue(out udppacket) )
                {
                    List<UmdfPacket> lstPacotes = crackUDPPacket(udppacket.byteData, udppacket.pktLength);

                    foreach (UmdfPacket umdfpacket in lstPacotes)
                    {
                        logger.DebugFormat("Recebeu pacote: {0} {1}/{2} {3} feeder{4}",
                            umdfpacket.seqNum,
                            umdfpacket.currChunk,
                            umdfpacket.noChunks,
                            umdfpacket.msgLength,
                            udppacket.feeder);


                        // ATP: O chunk pode vir fora de sequencia.... o teste abaixo eh furado
                        //if (umdfpacket.seqNum == 1 && umdfpacket.currChunk == 1)
                        //{
                        //    logger.Info("Limpa toda a fila de mensagens de fora de ordem!");
                        //    qUdpPktOut.Clear();
                        //    lastpkt = 0;
                        //}

                        if (umdfpacket.seqNum < lastpkt && umdfpacket.seqNum != 1)
                        {
                            logger.Info("Despreza SeqNum[" + umdfpacket.seqNum + "] ja processado");
                            continue;
                        }

                        if (umdfpacket.seqNum < (lastpkt + 1) && umdfpacket.seqNum == 1 && udppacket.feeder == UdpPacket.FEEDER_SECUNDARIO)
                        {
                            logger.Info("Despreza reinicio do SeqNum[" + umdfpacket.seqNum + "] feeder secundario");
                            continue;
                        }

                        if (umdfpacket.seqNum > (lastpkt + 1))
                        {
                            if (lastpkt == 0 && umdfpacket.seqNum != 1)
                            {
                                logger.Debug("Despreza pacote[" + umdfpacket.seqNum + "] ate o reinicio da lista");
                                continue;
                            }

                            lock (qUdpPktOut)
                            {
                                qUdpPktOut.Enqueue(udppacket);
                            }

                            logger.Debug("Perdeu pacote[" + (lastpkt + 1) + "], reinfilera pacote[" + umdfpacket.seqNum + "] (Tam seqOut = " + qUdpPktOut.Count + ")");
                            continue;
                        }

                        /*if (umdfpacket.seqNum == lastpkt && umdfpacket.currChunk > (lastChunk + 1))
                        {
                            logger.DebugFormat("Enfileira chunck fora de sequencia: {0} {1}/{2} {3}",
                                umdfpacket.seqNum,
                                umdfpacket.noChunks,
                                umdfpacket.currChunk,
                                umdfpacket.msgLength);

                            qUdpPktOut.Enqueue(udppacket);
                            continue;
                        }*/


                        bool bsinaliza = qUmdfPacket.IsEmpty;
                        qUmdfPacket.Enqueue(umdfpacket);
                        if ( bsinaliza )
                        {
                            lock (syncQueueUmdfPacket)
                            {
                                Monitor.Pulse(syncQueueUmdfPacket);
                            }
                        }

                        lastpkt = umdfpacket.seqNum;
                        if (umdfpacket.currChunk == umdfpacket.noChunks)
                            lastChunk = 0;
                        else
                            lastChunk = umdfpacket.currChunk;
                    }

                    continue;
                }

                // Tenta processar pacotes fora de ordem....
                int tentativa = 0;
                while (qUdpPktOut.Count > 0 && bKeepRunning && tentativa < qUdpPktOut.Count)
                {
                    tentativa++;
                    UdpPacket udppacket1 = null;
                    lock (qUdpPktOut)
                    {
                        udppacket1 = qUdpPktOut.Dequeue();
                    }

                    List<UmdfPacket> lstPacotes = crackUDPPacket(udppacket1.byteData, udppacket1.pktLength);

                    foreach (UmdfPacket umdfpacket in lstPacotes)
                    {

                        logger.DebugFormat("Recebeu pacote out: {0} {1}/{2} {3} feeder {4}",
                            umdfpacket.seqNum,
                            umdfpacket.currChunk,
                            umdfpacket.noChunks,
                            umdfpacket.msgLength,
                            udppacket1.feeder);


                        if (umdfpacket.seqNum > (lastpkt + 1))
                        {
                            // Se expirou o TTL do pacote, descarta
                            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - udppacket1.pktTimestamp);
                            if (ts.TotalMilliseconds > PACKET_TIME_WINDOW)
                            {
                                logger.Debug("Perdeu pacote[" + (lastpkt + 1) + "], descarta pacote[" + umdfpacket.seqNum + "] feeder [" + udppacket1.feeder + "] expirado");
                                continue;
                            }

                            logger.Debug("Perdeu pacote[" + (lastpkt + 1) + "], reinfilera pacote[" + umdfpacket.seqNum + "] feeder [" + udppacket1.feeder + "]");
                            lock (qUdpPktOut)
                            {
                                qUdpPktOut.Enqueue(udppacket1);
                            }
                            continue;
                        }

                        if (umdfpacket.seqNum < (lastpkt + 1) && umdfpacket.seqNum != 1)
                        {
                            logger.Info("Despreza SeqNum[" + umdfpacket.seqNum + "] Feeder ["+ udppacket1.feeder + "] ja processado");
                            continue;
                        }
                        if (umdfpacket.seqNum < (lastpkt + 1) && umdfpacket.seqNum == 1 && udppacket1.feeder==UdpPacket.FEEDER_SECUNDARIO)
                        {
                            logger.Info("Despreza reinicio do SeqNum[" + umdfpacket.seqNum + "] feeder secundario");
                            continue;
                        }

                        bool bsinaliza = qUmdfPacket.IsEmpty;
                        qUmdfPacket.Enqueue(umdfpacket);
                        if (bsinaliza)
                        {
                            lock (syncQueueUmdfPacket)
                            {
                                Monitor.Pulse(syncQueueUmdfPacket);
                            }
                        }

                        lastpkt = umdfpacket.seqNum;
                    }
                }

                lock (syncQueueUdpPkt)
                {
                    Monitor.Wait(syncQueueUdpPkt, 50);
                }

            }
        }

        protected virtual void umdfPacketAssembler()
        {
            //UmdfPacket packet = null;
            int lastSeqNum = 0;
            UmdfPacket [] chunks = null;
            int receivedChunks = 0;
            Dictionary<int, long> dctSeqNum = new Dictionary<int, long>();

            //StreamWriter tracewriter = new StreamWriter("d:\\turingoms\\servicos\\mdsnetpuma2\\logs\\Trace-" + this.channelID + "-" +  this.multicastPortPRI + ".txt", true);

            while (bKeepRunning)
            {

                Context context = null;
                UmdfPacket packet = null;

                if ( qUmdfPacket.TryDequeue(out packet) )
                {
                    int totalBytes = packet.data.Length;
                    try
                    {

                        //if (context != null)
                        //{
                        //    ((OpenFAST.Debug.BasicDecodeTrace)context.DecodeTrace).Writer.Close();
                        //    ((OpenFAST.Debug.BasicDecodeTrace)context.DecodeTrace).Writer.Dispose();
                        //    context = null;
                        //}

                        context = new Context();
                        context.TemplateRegistry = registry;
                        //context.TraceEnabled = true;
                        //context.StartTrace();
                        //((OpenFAST.Debug.BasicDecodeTrace)context.DecodeTrace).Writer = tracewriter;

                        if (packet.noChunks > 1)
                        {
                            logger.Debug("pktAss() noChunks: " + packet.noChunks + " currChunk: " + packet.currChunk + " seqNum: " + packet.seqNum + " lastSeqNum:" + lastSeqNum);
                            if (chunks == null || packet.seqNum != lastSeqNum)
                            {
                                logger.Debug("pktAss() Alocando array para [" + packet.noChunks + "] chunks: null ou seqNum!=");
                                chunks = new UmdfPacket[packet.noChunks];
                                receivedChunks = 0;
                            }

                            // Verifica se ainda esta no mesmo chunk, e se ainda dentro da mesma
                            // rodada do snapshot
                            if (dctSeqNum.ContainsKey(packet.seqNum))
                            {
                                TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - dctSeqNum[packet.seqNum]);
                                if (ts.TotalMilliseconds > PACKET_TIME_WINDOW && processorType.Equals(ProcessorType.MarketIncremental))
                                {
                                    logger.Debug("pktAss() REALOCANDO array para [" + packet.noChunks + "] chunks");
                                    chunks = new UmdfPacket[packet.noChunks];
                                    receivedChunks = 0;
                                }
                                dctSeqNum[packet.seqNum] = DateTime.Now.Ticks;
                            }
                            else
                            {
                                logger.Debug("pktAss() Alocando array para [" + packet.noChunks + "] chunks");
                                dctSeqNum.Add(packet.seqNum, DateTime.Now.Ticks);
                                chunks = new UmdfPacket[packet.noChunks];
                                receivedChunks = 0;
                            }

                            
                            if ((packet.currChunk - 1) < chunks.Length)
                            {
                                if (chunks[packet.currChunk - 1] == null)
                                    receivedChunks++;
                                chunks[packet.currChunk - 1] = packet;
                            }
                            else
                            {
                                logger.Error("pktAss() Puts...[" + (packet.currChunk - 1) + "] <=> [" + chunks.Length + "] fdp furou!!!!");
                                continue;
                            }


                            logger.Debug("pktAss() noChunks: " + packet.noChunks + " currChunk: " + packet.currChunk);
                            lastSeqNum = packet.seqNum;

                            if (receivedChunks < packet.noChunks)
                            {
                                continue;
                            }

                            dctSeqNum.Remove(packet.seqNum);
                            logger.Debug("pktAss() Remontando pacote noChunks=" + packet.noChunks + " received=" + receivedChunks + " length=" + chunks.Length);

                            byte[] reassembled = UmdfUtils.reassemble(chunks);
                            totalBytes = reassembled.Length;
                            chunks = null;
                            receivedChunks = 0;
                            packet.data = reassembled;
                            packet.msgLength = totalBytes;

                            System.IO.MemoryStream byteIn = new System.IO.MemoryStream(packet.data, 0, packet.msgLength);

                            FastDecoder decoder = new FastDecoder(context, byteIn);

                            Message message = decoder.ReadMessage();

                            bool bsinaliza = queueToProcessor.IsEmpty;
                            queueToProcessor.Enqueue(message);
                            if (bsinaliza)
                            {
                                lock (syncQueueToProcessor)
                                {
                                    Monitor.Pulse(syncQueueToProcessor);
                                }
                            }

                            // testa se debug esta habilitado por conta do writegroup
                            // mais performatico
                            if (logger.IsDebugEnabled)
                            {
                                logger.Debug(UmdfUtils.writeGroup(message));
                            }
                        }
                        else
                        {
                            lastSeqNum = packet.seqNum;
                            System.IO.MemoryStream byteIn = new System.IO.MemoryStream(packet.data, 0, packet.msgLength);

                            FastDecoder decoder = new FastDecoder(context, byteIn);

                            Message message = decoder.ReadMessage();

                            bool bsinaliza = queueToProcessor.IsEmpty;
                            queueToProcessor.Enqueue(message);
                            if (bsinaliza)
                            {
                                lock (syncQueueToProcessor)
                                {
                                    Monitor.Pulse(syncQueueToProcessor);
                                }
                            }

                            if (logger.IsDebugEnabled)
                            {
                                logger.Debug(UmdfUtils.writeGroup(message));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("pktAss() Erro: " + ex.Message, ex);

                        // Bom, com uma exception dessas, acho melhor iniciar o canal, pode ser falha no decoder
                        string msg = string.Format("Canal [{0}] precisa ser derrubado, pois deu falha grave no PacketAssembler().\r\n\r\n{1}\r\n\r\n{2}", channelID, ex.Message, ex);
                        string titulo = string.Format("Derrubando canal [{0}] por falha em pktAss()", channelID);
                        MDSUtils.EnviarEmail(titulo, msg);
                        bKeepRunning = false;

                        //lastpkt = 0;
                    }

                    continue;
                }

                lock (syncQueueUmdfPacket)
                {
                    Monitor.Wait(syncQueueUmdfPacket, 100);
                }
            }
        }


        protected abstract void umdfPacketProcessor();
        //{
        //}

        protected abstract void umdfMessageProcessor();
        //{
        //}

        protected virtual void OnReceivePRI(IAsyncResult ar)
        {
            try
            {
                if (!bKeepRunning)
                    return;

                int blidos = clientSocketPRI.EndReceive(ar);

                if (blidos <= 0)
                {
                    logger.Error("Blidos <= 0 ");
                    return;
                }

                UdpPacket packet = new UdpPacket();
                packet.byteData = byteDataPRI;
                packet.pktLength = blidos;
                packet.pktTimestamp = DateTime.Now.Ticks;
                packet.feeder = UdpPacket.FEEDER_PRIMARIO;

                //UmdfPacket umdfPacket = new UmdfPacket(byteData, 0, blidos);
                //logger.DebugFormat("Recebeu pacote1[{0}] [{1}/{2}] [{3}]", umdfPacket.seqNum, umdfPacket.currChunk, umdfPacket.noChunks, umdfPacket.msgLength);

                byteDataPRI = new byte[BUFFER_SIZE];

                bool bsinaliza = qUdpPkt.IsEmpty;
                qUdpPkt.Enqueue(packet);
                if (bsinaliza)
                {
                    lock (syncQueueUdpPkt)
                    {
                        Monitor.PulseAll(syncQueueUdpPkt);
                    }
                }

                if (DateTime.UtcNow.Ticks - lastLogPRI > TimeSpan.TicksPerSecond)
                {
                    lastLogPRI = DateTime.UtcNow.Ticks;
                    logger.Info("OnReceivePRI: pacotes na fila: " + qUdpPkt.Count);
                }

                //Start listening to receive more data from the user
                clientSocketPRI.BeginReceiveFrom(byteDataPRI, 0, byteDataPRI.Length, SocketFlags.None, ref epServerPRI,
                                           new AsyncCallback(OnReceivePRI), null);
            }
            catch (ObjectDisposedException)
            {
                logger.Error("OnReceivePRI(): conexao finalizada!");
            }
            catch (Exception ex)
            {
                logger.Error("OnReceivePRI(): " + ex.Message, ex);
            }
        }

        protected virtual void OnReceiveSEC(IAsyncResult ar)
        {
            try
            {
                if (!bKeepRunning)
                    return;

                int blidos = clientSocketSEC.EndReceive(ar);

                if (blidos <= 0)
                {
                    logger.Error("Blidos <= 0 ");
                    return;
                }

                UdpPacket packet = new UdpPacket();
                packet.byteData = byteDataSEC;
                packet.pktLength = blidos;
                packet.pktTimestamp = DateTime.Now.Ticks;
                packet.feeder = UdpPacket.FEEDER_SECUNDARIO;

                //UmdfPacket umdfPacket = new UmdfPacket(byteData2, 0, blidos);
                //logger.DebugFormat("Recebeu pacote2[{0}] [{1}/{2}] [{3}]", umdfPacket.seqNum, umdfPacket.currChunk, umdfPacket.noChunks, umdfPacket.msgLength);

                byteDataSEC = new byte[BUFFER_SIZE];

                bool bsinaliza = qUdpPkt.IsEmpty;
                qUdpPkt.Enqueue(packet);
                if (bsinaliza)
                {
                    lock (syncQueueUdpPkt)
                    {
                        Monitor.PulseAll(syncQueueUdpPkt);
                    }
                }

                if (DateTime.UtcNow.Ticks - lastLogSEC > TimeSpan.TicksPerSecond)
                {
                    lastLogSEC = DateTime.UtcNow.Ticks;
                    logger.Info("OnReceiveSEC: pacotes na fila: " + qUdpPkt.Count);
                }

                //Start listening to receive more data from the user
                clientSocketSEC.BeginReceiveFrom(byteDataSEC, 0, byteDataSEC.Length, SocketFlags.None, ref epServerSEC,
                                           new AsyncCallback(OnReceiveSEC), null);
            }
            catch (ObjectDisposedException)
            {
                logger.Error("OnReceiveSEC(): conexao finalizada!");
            }
            catch (Exception ex)
            {
                logger.Error("OnReceiveSEC(): " + ex.Message, ex);
            }
        }


        public List<UmdfPacket> crackUDPPacket(byte[] buff, int length)
        {
            List<UmdfPacket> retorno = new List<UmdfPacket>();

            int pos = 0;

            int seqNum = 0;
            int noChunks = 0;
            int currChunk = 0;
            int msgLength = 0;

            while (pos < length)
            {
                try
                {
                    seqNum = 0;
                    noChunks = 0;
                    currChunk = 0;
                    msgLength = 0;

                    seqNum += (((int)buff[pos++]) & 0xFF) << 24;
                    seqNum += (((int)buff[pos++]) & 0xFF) << 16;
                    seqNum += (((int)buff[pos++]) & 0xFF) << 8;
                    seqNum += (((int)buff[pos++]) & 0xFF);

                    noChunks += (((int)buff[pos++]) & 0xFF) << 8;
                    noChunks += (((int)buff[pos++]) & 0xFF);

                    currChunk += (((int)buff[pos++]) & 0xFF) << 8;
                    currChunk += (((int)buff[pos++]) & 0xFF);

                    msgLength += (((int)buff[pos++]) & 0xFF) << 8;
                    msgLength += (((int)buff[pos++]) & 0xFF);

                    //logger.DebugFormat("crackUDP: pos={0} seqNum={1} chunk={2}/{3} msgLength={4} bufferLen={5}",
                    //    pos, seqNum, currChunk, noChunks, msgLength, length);

                    byte[] buffer = new byte[msgLength];

                    System.Buffer.BlockCopy(buff, pos, buffer, 0, msgLength);

                    pos += msgLength;

                    UmdfPacket umdfPacket = new UmdfPacket();

                    umdfPacket.currChunk = currChunk;
                    umdfPacket.data = buffer;
                    umdfPacket.msgLength = msgLength;
                    umdfPacket.noChunks = noChunks;
                    umdfPacket.seqNum = seqNum;

                    retorno.Add(umdfPacket);
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("Erro crackUDP: pos={0} seqNum={1} chunk={2}/{3} msgLength={4} bufferLen={5}",
                        pos, seqNum, currChunk, noChunks, msgLength, length);
                    logger.Error("crackUDPMessage: " + ex.Message, ex);
                }

            }
            return retorno;
        }
    }
}
