using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.Core.Spider.OrderFixProcessing.Lib.Dados;
using System.Collections.Concurrent;
using System.Threading;
using Gradual.Spider.Acompanhamento4Socket.Lib.Dados;
using Gradual.Spider.CommSocket;

namespace Gradual.Spider.Acompanhamento4Socket.Rede
{
    public class A4SocketClientConnection
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables
        ConcurrentQueue<SpiderOrderInfo> _cqTO;
        Thread _thSend;
        bool _isRunning;
        object _sync = new object();
        bool _canDequeue;
        #endregion

        #region Properties
        public AcConnectionInfo ConnectionInfo{get;set;}
        public SpiderSocket A4SClient { get; set; }
        #endregion

        public A4SocketClientConnection()
        {
            _isRunning = false;
            this.ConnectionInfo = null;
            this.A4SClient = null;
            _canDequeue = false;
        }

        public void Start()
        {
            try
            {
                logger.Info("Iniciando conexao client A4SocketClientConnection");
                _canDequeue = false;
                _cqTO = new ConcurrentQueue<SpiderOrderInfo>();

                _isRunning = true;
                logger.Info("Iniciando thread de processamento...");
                _thSend = new Thread(new ThreadStart(_dequeueMsg));
                _thSend.Start();

            }
            catch (Exception ex)
            {
                logger.Error("Problemas no start da conexao AcSocketConnection: " + ex.Message, ex);
            }
        }

        public void Stop()
        {
            try
            {
                logger.Info("Parando A4SocketClientConnection");

                _isRunning = false;
                _canDequeue = false;
                // logger.Infno(
                if (_thSend != null && _thSend.IsAlive)
                {
                    _thSend.Join(500);
                    try
                    {
                        if (_thSend.IsAlive)
                            _thSend.Abort();
                    }
                    catch { }
                    _thSend = null;
                }
                _cqTO = null;



            }
            catch (Exception ex)
            {
                logger.Error("Problemas no stop da conexao AcSocketConnection: " + ex.Message, ex);
            }
        }

        public void AddMsg(SpiderOrderInfo o)
        {
            _cqTO.Enqueue(o);
            lock (_sync)
                Monitor.Pulse(_sync);

        }

        
        

        private void _dequeueMsg()
        {
            try
            {
                while (_isRunning)
                {
                    if (!_canDequeue)
                    {
                        lock (_sync)
                            Monitor.Wait(_sync, 100);
                    }

                    try
                    {
                        SpiderOrderInfo to = null;
                        if (_cqTO.TryDequeue(out to))
                        {
                            SpiderSocket.SendObject(to, this.ConnectionInfo.ClientSocket);
                        }
                        else
                        {
                            lock(_sync)
                                Monitor.Wait(_sync, 100);
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas ao desenfileirar o objeto da fila: " + ex.Message, ex);
            }
        }

        public bool ProcessSnapshot(List<SpiderOrderInfo> lst)
        {
            try
            {
                long maxSeq = 0;
                foreach (SpiderOrderInfo item in lst)
                {
                    SpiderSocket.SendObject(item, this.ConnectionInfo.ClientSocket);
                    maxSeq = item.MsgSeqNum;
                }

                logger.Info("ProcessSnapshot(): MaxSeqNum: " + maxSeq);
                // Sincronizar a fila de envio e habilitar o envio
                while (true)
                {
                    if (_cqTO.Count == 0)
                    {
                        logger.InfoFormat("ProcessSnapshot(): Fila Zerada: sincronizado");
                        break;
                    }
                       
                    SpiderOrderInfo ord = null;
                    _cqTO.TryDequeue(out ord);
                    if (ord.MsgSeqNum == maxSeq)
                    {
                        logger.InfoFormat("ProcessSnapshot(): Sincronizou: Current [{0}] MaxSeq [{1}]", ord.MsgSeqNum, maxSeq);
                        break;
                    }
                    if (ord.MsgSeqNum > maxSeq)
                    {
                        logger.ErrorFormat("ProcessSnapshot(): Deu problema... Current[{0}] MaxSeq[{1}]", ord.MsgSeqNum, maxSeq);
                        break;
                    }
                    if (ord.MsgSeqNum < maxSeq)
                    {
                        logger.InfoFormat("ProcessSnapshot(): Ignorando Mensagem: Account[{0}] OrdID[{1}] Symbol[{2}] MsgSeqNum[{3}]", 
                            ord.Account, ord.OrderID, ord.Symbol, ord.MsgSeqNum);
                    }
                }
                
                // Liberando o envio
                _canDequeue = true;
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no envio do snapshot / sincronizacao das mensagens: " + ex.Message, ex);
                return false;
            }
        }

    }
}
