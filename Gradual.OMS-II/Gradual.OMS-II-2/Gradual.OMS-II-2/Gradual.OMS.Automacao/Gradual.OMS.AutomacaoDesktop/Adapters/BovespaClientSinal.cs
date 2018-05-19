using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using SINALCom;
using System.Threading;

namespace Gradual.OMS.AutomacaoDesktop
{
    public delegate void BovespaDataReceivedEventHandler(string LastMsgId, string SPF_Header, string DataPtr, int DataSize);
    public delegate void BovespaOnConnectEventHandler();
    public delegate void BovespaOnErrorEventHandler(int error, string msg, string description);
    public delegate void BovespaOnDisconnectEventHandler(string description);

    class MensagemBovespa
    {
        public string LastMsgId { get; set; }
        public string SPF_Header { get; set; }
        public string DataPtr { get; set; }
        public int DataSize { get; set; }

        public MensagemBovespa(string LastMsgId, string SPF_Header, string DataPtr, int DataSize)
        {
            this.LastMsgId = LastMsgId;
            this.SPF_Header = SPF_Header;
            this.DataPtr = DataPtr;
            this.DataSize = DataSize;
        }
    }

    public class BovespaClientSinal
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TSINALComClass objBovespa = new TSINALComClass();
        private bool _bConectado = false;
        private Queue<MensagemBovespa> queueBovespa = new Queue<MensagemBovespa>();
        private Thread _me = null;
        private Semaphore _semQueue = new Semaphore(0, int.MaxValue);
        private bool _bKeepRunning = false;

        /// <summary>
        /// 
        /// </summary>
        public string LastMsg { get; set; }
        public bool IsConectado
        { 
            get { return _bConectado; }
        }

        /// <summary>
        /// Liga ou desliga debug dos sinais recebidos (requer log4net.Level >= DEBUG)
        /// </summary>
        public bool Debug { get; set; }

        public event BovespaDataReceivedEventHandler OnDataReceived;
        public event BovespaOnConnectEventHandler OnConnect;
        public event BovespaOnErrorEventHandler OnError;
        public event BovespaOnDisconnectEventHandler OnDisconnect;

        public BovespaClientSinal()
        {
            objBovespa.OnConnect += new ITSINALComEvents_OnConnectEventHandler(Bovespa_OnConnect);
            objBovespa.OnError += new ITSINALComEvents_OnErrorEventHandler(Bovespa_OnError);
            objBovespa.OnDisconnect += new ITSINALComEvents_OnDisconnectEventHandler(Bovespa_OnDisconnect);
            objBovespa.OnReceiveData += new ITSINALComEvents_OnReceiveDataEventHandler(Bovespa_OnReceiveData);
        }


        /// <summary>
        /// Inicia conexao com ProxyDiff
        /// </summary>
        /// <param name="ip">endereco ip ou hostname</param>
        /// <param name="port">porta de conexao</param>
        public void Connect( string ip, string port )
        {
            logger.InfoFormat("Connect({0},{1})", ip, port);

            objBovespa.IpAddr = ip;
            objBovespa.Port = port;

            _bKeepRunning = true;
            _me = new Thread(new ThreadStart(Run));
            _me.Start();

            objBovespa.Connect();
        }

        /// <summary>
        /// Desconecta do proxydiff
        /// </summary>
        public void Disconnect()
        {
            logger.Info("Disconnect()");
            objBovespa.Disconnect();

            _bKeepRunning = false;
            while (_me!= null &&_me.IsAlive) Thread.Sleep(100);
        }


        /// <summary>
        /// Handler para o evento de conexao ao proxydiff
        /// </summary>
        private void Bovespa_OnConnect()
        {
            _bConectado = true;

            logger.Info("OnConnect(): Start requesting msg [" + LastMsg + "]");

            if (LastMsg != null && LastMsg.Length > 0)
                objBovespa.StartRequest(LastMsg);
            else
                objBovespa.StartRequest("");

            if (OnConnect != null)
                OnConnect();
        }

        /// <summary>
        /// Handler para eventos de erro
        /// </summary>
        /// <param name="error"></param>
        /// <param name="msg"></param>
        /// <param name="description"></param>
        private void Bovespa_OnError(int error, string msg, string description)
        {
            logger.ErrorFormat("OnError(): Erro {0}: {1} - {2}", error, msg, description);

            if ( OnError != null )
                OnError(error, msg, description);
        }

        /// <summary>
        /// Handler para o evento de desconexao da Bovespa
        /// </summary>
        /// <param name="description"></param>
        private void Bovespa_OnDisconnect(string description)
        {
            _bConectado = false;
            logger.Info("OnDisconnect(): " + description);

            if ( OnDisconnect != null )
                OnDisconnect(description);
        }

        private void Bovespa_OnReceiveData(string LastMsgId, string SPF_Header, string DataPtr, int DataSize)
        {
            LastMsg = LastMsgId;

            MensagemBovespa msgbov = new MensagemBovespa(LastMsg, SPF_Header, DataPtr, DataSize);

            lock (queueBovespa)
            {
                queueBovespa.Enqueue(msgbov);
            }

            _semQueue.Release(1);

            objBovespa.Ack = true;
        }


        private void Run()
        {
            List<MensagemBovespa> msgs = new List<MensagemBovespa>();

            while (_bKeepRunning)
            {
                _semQueue.WaitOne(100);

                lock(queueBovespa)
                {
                    if ( queueBovespa.Count > 0 )
                    {
                        msgs.AddRange(queueBovespa.ToList());
                        queueBovespa.Clear();
                    }
                }

                foreach (MensagemBovespa msg in msgs)
                {
                    if ( Debug && logger.IsDebugEnabled)
                        logger.Debug("msg=[" + msg.LastMsgId + "," + msg.DataPtr + "]");

                    if ( OnDataReceived != null )
                        OnDataReceived(msg.LastMsgId,msg.SPF_Header,msg.DataPtr, msg.DataSize);
                }

                msgs.Clear();
            }
        }
    }
}
