using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;
using Gradual.MDS.Core.Lib;

namespace Gradual.MDS.Core
{
    public delegate void FIXMachineGunEventHandler(object sender, FIXMachineGunEventEventArgs args);

    public class FIXMachineGunEventEventArgs : EventArgs
    {
        public QuickFix.FIX44.Message Message { get; set; }
    }

    public class MachineGunFixSplitter
    {
        protected static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ConcurrentQueue<QuickFix.FIX44.Message> mainInboundQ = new ConcurrentQueue<QuickFix.FIX44.Message>();
        private ConcurrentQueue<QuickFix.FIX44.Message> mainOutboundQ = new ConcurrentQueue<QuickFix.FIX44.Message>();

        private ConcurrentQueue<QuickFix.FIX44.Message>[] inboundQueues = new ConcurrentQueue<QuickFix.FIX44.Message>[10];
        private ConcurrentQueue<QuickFix.FIX44.Message>[] intermediateQueues = new ConcurrentQueue<QuickFix.FIX44.Message>[10];

        private object [] syncInboundQueues = new object[10];
        private object [] syncIntermediateQueues = new object[10];

        private object syncMainInboundQ = new object();
        private object syncOutboundQ = new object();
        private bool _bKeepRunning = false;

        private int currentFeed = -1;
        private int lastMsgSeqNum = 0;
        private int sentMsgSeqNum = 0;

        private Thread[] threadSplitters = new Thread[10];
        private Thread thMainInbound = null;
        private Thread thMainOutbound = null;
        private Thread thSendBullets = null;

        
        public event FIXMachineGunEventHandler UnderFIXMessageFire;



        public void EnqueueMessage(QuickFix.FIX44.Message message)
        {
            try
            {
                // - set the initial feed
                if (currentFeed < 0)
                {
                    int lastDigit = message.Header.GetInt(QuickFix.Fields.Tags.MsgSeqNum) % 10;
                    currentFeed = lastDigit;
                }

                mainInboundQ.Enqueue(message);
            }
            catch (Exception ex)
            {
                logger.Error("EnqueueMessage: " + ex.Message, ex);
            }
        }

        public void Start()
        {
            logger.Info("Inicializando MachineGunSplitter");
            
            _bKeepRunning = true;

            logger.Info("Criando filas");

            for (int i = 0; i < 10; i++)
            {
                inboundQueues[i] = new ConcurrentQueue<QuickFix.FIX44.Message>();
                intermediateQueues[i] = new ConcurrentQueue<QuickFix.FIX44.Message>();
                syncInboundQueues[i] = new object();
                syncIntermediateQueues[i] = new object();
            }

            logger.Info("Criando threads");

            thMainInbound = new Thread(new ThreadStart(mainInboundQProc));
            thMainInbound.Start();

            thMainOutbound = new Thread(new ThreadStart(mainOutboundProc));
            thMainOutbound.Start();

            thSendBullets = new Thread(new ThreadStart(sendBulletsProc));
            thSendBullets.Start();

            for (int i = 0; i < 10; i++)
            {
                threadSplitters[i] = new Thread(new ParameterizedThreadStart(messageSplitterProc));
                threadSplitters[i].Start(i);
            }

            logger.Info("MachineGunSplitter inicializado");
        }

        public void Stop()
        {
            try
            {
                logger.Info("Finalizando MachineGunSpliter");

                _bKeepRunning = false;

                if (thMainInbound != null)
                {
                    while (thMainInbound.IsAlive)
                        Thread.Sleep(100);
                }

                if (thMainOutbound != null)
                {
                    while (thMainOutbound.IsAlive)
                        Thread.Sleep(100);
                }

                if (thSendBullets != null)
                {
                    while (thSendBullets.IsAlive)
                        Thread.Sleep(100);
                }

                for (int i = 0; i < 10; i++)
                {
                    if (threadSplitters[i] != null)
                    {
                        while (threadSplitters[i].IsAlive)
                            Thread.Sleep(100);
                    }
                }

                logger.Info("MchineGunSpliter Finalizado");

            }
            catch (Exception ex)
            {
                logger.Error("Stop(): " + ex.Message, ex);
            }
        }


        public void Reset()
        {
            lastMsgSeqNum = 0;
            currentFeed = -1;
            sentMsgSeqNum = 0;

            logger.Warn("Clearing all queues");

            QuickFix.FIX44.Message message = null;

            while (mainInboundQ.TryDequeue(out message));

            for(int i=0; i < 10; i++)
            {
                while (inboundQueues[i].TryDequeue(out message));
                while (intermediateQueues[i].TryDequeue(out message)) ;
            }

            while (this.mainOutboundQ.TryDequeue(out message));


        }

        /// <summary>
        /// 
        /// </summary>
        private void mainInboundQProc()
        {
            logger.Info("Iniciando thread de processamento da fila principal de entrada");
            long lastLog = 0;

            while (_bKeepRunning)
            {
                try
                {
                    if (currentFeed < 0)
                    {
                        Thread.Sleep(250);
                        continue;
                    }

                    QuickFix.FIX44.Message message = null;
                    if (mainInboundQ.TryDequeue(out message))
                    {
                        int msgSeqNum = message.Header.GetInt(QuickFix.Fields.Tags.MsgSeqNum);
                        int lastDigit = msgSeqNum % 10;

                        // Se ha um intervalo entre as mensagens
                        // preenche com marcadores para forcar a 
                        // troca da fila
                        if (lastMsgSeqNum > 0)
                        {
                            if (msgSeqNum > (lastMsgSeqNum + 1))
                            {
                                logger.InfoFormat("Filling gap [{0}]->[{1}]", lastMsgSeqNum + 1, msgSeqNum);

                                for (int fillSeqNum = lastMsgSeqNum+1; fillSeqNum < msgSeqNum; fillSeqNum++)
                                {
                                    int markLastDigit = fillSeqNum % 10;

                                    // Envia o marcador de final da mensagem 
                                    QuickFix.FIX44.TestRequest mark = new QuickFix.FIX44.TestRequest();
                                    mark.Header.SetField(new QuickFix.Fields.MsgSeqNum(fillSeqNum));

                                    inboundQueues[markLastDigit].Enqueue(mark);
                                }
                            }
                        }

                        inboundQueues[lastDigit].Enqueue(message);
                        lastMsgSeqNum = msgSeqNum;

                        if (MDSUtils.shouldLog(lastLog))
                        {
                            logger.Info("Mensagens FIX para processar na fila (mainInboudQ): " + mainInboundQ.Count);
                            lastLog = DateTime.UtcNow.Ticks;
                        }

                        continue;
                    }

                    Thread.Sleep(25);
                }
                catch (Exception ex)
                {
                    logger.Error("mainInboundQProc: " + ex.Message, ex);
                }

            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void messageSplitterProc(object param)
        {
            int me = (int)param;

            logger.InfoFormat("Iniciando thread {0} de splitting", me);

            long lastLog = 0;

            while (_bKeepRunning)
            {
                try
                {

                    if (currentFeed < 0)
                    {
                        Thread.Sleep(250);
                        continue;
                    }

                    QuickFix.FIX44.Message message = null;
                    if (inboundQueues[me].TryDequeue(out message))
                    {
                        int msgSeqNum = message.Header.GetInt(QuickFix.Fields.Tags.MsgSeqNum);
                        string msgType = message.Header.GetString(QuickFix.Fields.Tags.MsgType);
                        int lastDigit = msgSeqNum % 10;

                        if ( lastDigit != Convert.ToInt32(msgSeqNum.ToString().AsEnumerable().Last().ToString()) )
                        {
                            logger.Error("PQP arrendondamento FDP");
                        }

                        if (!msgType.Equals(QuickFix.FIX44.TestRequest.MsgType))
                        {
                            logger.Debug("Splitting Message [" + message.Header.GetInt(QuickFix.Fields.Tags.MsgSeqNum) + "] type [" + message.Header.GetString(QuickFix.Fields.Tags.MsgType) + "]");

                            List<QuickFix.FIX44.Message> lstSplitted = FIXUtils.splitMessage(message, 0, null);

                            foreach (QuickFix.FIX44.Message splittedmsg in lstSplitted)
                            {
                                enqueueToFire(me, splittedmsg);
                            }
                        }
                        else
                        {
                            logger.DebugFormat("inboundQueue[{0}] recebeu TestRequest, skipping", me);
                        }

                        // Envia o marcador de final da mensagem 
                        QuickFix.FIX44.TestRequest mark = new QuickFix.FIX44.TestRequest();
                        mark.Header.SetField(new QuickFix.Fields.MsgSeqNum(msgSeqNum));
                        enqueueToFire(me, mark);

                        if (MDSUtils.shouldLog(lastLog))
                        {
                            logger.InfoFormat("Mensagens para splitting FIX na fila {0}: {1}", me, inboundQueues[me].Count);
                            lastLog = DateTime.UtcNow.Ticks;
                        }
                        continue;
                    }

                    Thread.Sleep(25);
                }
                catch (Exception ex)
                {
                    logger.Error("messageSplitterProc: " + ex.Message, ex);
                }

            }
        }

        /// <summary>
        /// Enfileira a mensagem para as filas intermediarias
        /// </summary>
        /// <param name="queueNumber"></param>
        /// <param name="message"></param>
        private void enqueueToFire(int queueNumber,  QuickFix.FIX44.Message message)
        {
            try
            {
                intermediateQueues[queueNumber].Enqueue(message);
            }
            catch (Exception ex)
            {
                logger.Error("enqueueToFire: " + ex.Message, ex);
            }
        }


        /// <summary>
        /// Thread de convergencia das mensagems
        /// Pega as mensagens splitadas na sequencia correta a partir
        /// da fila intermediaria
        /// </summary>
        private void sendBulletsProc()
        {
            logger.Info("Iniciando thread de convergencia");

            long [] lastLogs = {0,0,0,0,0,0,0,0,0,0};

            while (_bKeepRunning)
            {
                try
                {
                    if (currentFeed < 0)
                    {
                        Thread.Sleep(250);
                        continue;
                    }

                    QuickFix.FIX44.Message message = null;
                    if (intermediateQueues[currentFeed].TryDequeue(out message))
                    {
                        int msgSeqNum = message.Header.GetInt(QuickFix.Fields.Tags.MsgSeqNum);
                        string msgType = message.Header.GetString(QuickFix.Fields.Tags.MsgType);

                        if (sentMsgSeqNum != 0)
                        {
                            if (msgSeqNum < sentMsgSeqNum || msgSeqNum > sentMsgSeqNum + 1)
                            {
                                logger.FatalFormat("Deu merrda, capitao {0}<=>{1}", msgSeqNum, sentMsgSeqNum);
                                continue;
                            }
                        }

                        sentMsgSeqNum = msgSeqNum;

                        // Qdo receber o marcador, troca a fila de leitura
                        if (msgType.Equals(QuickFix.FIX44.TestRequest.MsgType))
                        {
                            currentFeed++;
                            if (currentFeed > 9)
                                currentFeed = 0;

                            continue;
                        }

                        logger.Debug("Converging Message [" + message.Header.GetInt(QuickFix.Fields.Tags.MsgSeqNum) + "] type [" + message.Header.GetString(QuickFix.Fields.Tags.MsgType) + "]");

                        enqueueToOutput(message);

                        if (MDSUtils.shouldLog(lastLogs[currentFeed]))
                        {
                            logger.InfoFormat("Mensagens FIX na fila intermediaria {0}: {1}", currentFeed, intermediateQueues[currentFeed].Count);
                            lastLogs[currentFeed] = DateTime.UtcNow.Ticks;
                        }

                        continue;
                    }

                    Thread.Sleep(25);
                }
                catch (Exception ex)
                {
                    logger.Error("sendBulletsProc: " + ex.Message, ex);
                }
            }

            logger.Info("Fim da thread sendBullets");
        }

        /// <summary>
        /// Enfileira a mensagem pra despacho final
        /// </summary>
        /// <param name="message"></param>
        private void enqueueToOutput(QuickFix.FIX44.Message message)
        {
            try
            {
                mainOutboundQ.Enqueue(message);
            }
            catch (Exception ex)
            {
                logger.Error("enqueueToOutput: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Thread de despacho final das mensagems
        /// </summary>
        private void mainOutboundProc()
        {
            logger.Info("Iniciando thread de despacho final");

            long lastLog = 0;

            while (_bKeepRunning)
            {
                try
                {
                    if (currentFeed < 0)
                    {
                        Thread.Sleep(250);
                        continue;
                    }

                    QuickFix.FIX44.Message message = null;
                    if (mainOutboundQ.TryDequeue(out message))
                    {
                        int msgSeqNum = message.Header.GetInt(QuickFix.Fields.Tags.MsgSeqNum);
                        string msgType = message.Header.GetString(QuickFix.Fields.Tags.MsgType);

                        logger.Debug("Dispatching Message [" + message.Header.GetInt(QuickFix.Fields.Tags.MsgSeqNum) + "] type [" + message.Header.GetString(QuickFix.Fields.Tags.MsgType) + "]");

                        if (UnderFIXMessageFire != null)
                        {
                            FIXMachineGunEventEventArgs args = new FIXMachineGunEventEventArgs();
                            args.Message = message;

                            UnderFIXMessageFire(this, args);
                        }

                        if (MDSUtils.shouldLog(lastLog))
                        {
                            logger.InfoFormat("Mensagens FIX na fila mainOutboundQ: {0}", mainOutboundQ.Count);
                            lastLog = DateTime.UtcNow.Ticks;
                        }

                        continue;
                    }

                    Thread.Sleep(25);
                }
                catch (Exception ex)
                {
                    logger.Error("mainOutboundProc: " + ex.Message, ex);
                }
            }

            logger.Info("Fim da thread mainOutboundProc");
        }

    }
}
