using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.Spider.CommSocket;
using System.Configuration;
using System.Threading;
using Gradual.Spider.Acompanhamento4Socket.Cache;
using Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem;
using Gradual.Core.Spider.OrderFixProcessing.Lib.Dados;
using Gradual.Core.Spider.OrderFixProcessing.Lib.Mensagens;
using System.Net.Sockets;

namespace Gradual.Spider.Acompanhamento4Socket.Rede
{
    public class A4SocketClient
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion


        #region private variables
        SpiderSocket _scktClient = null;
        string[] _arrOrderFixSrv;

        bool _isRunning = false;
        Thread _thConnect = null;

        bool _isLogged = false;

        Thread _thHeartBeat = null;
        #endregion

        #region "Properties"
        //public LogonSrvInfo LoginInfo;


        #endregion


        #region Constructor/Destructor
        public A4SocketClient()
        {
        //    this.LoginInfo = new LogonSrvInfo();
        }

        ~A4SocketClient()
        {
        }

        #endregion

        #region Properties
        public bool IsConnected()
        {
            if (null != _scktClient)
                return _scktClient.IsConectado();
            else
                return false;
        }
        public bool IsLoggedOn()
        {
            return _isLogged;
        }
        #endregion
        
        #region "IServico Members"
        public void Start()
        {
            try
            {
                logger.Info("Iniciando A4Socket Client...");
                if (ConfigurationManager.AppSettings.AllKeys.Contains("OrderFixMsgSrv"))
                {
                    _arrOrderFixSrv = ConfigurationManager.AppSettings["OrderFixMsgSrv"].ToString().Split(new char [] {':'});
                }
                else
                    throw new Exception("Parametro OrderFixMsgSrv e obrigatorio");

                if (_arrOrderFixSrv.Length!=2)
                    throw new Exception("Parametro OrderFixMsgSrv invalido");

                logger.InfoFormat("Efetuando conexao com o servidor SpiderOrderFixMsg: [{0}:{1}]", _arrOrderFixSrv[0], _arrOrderFixSrv[1]);
                _scktClient = new SpiderSocket();
                _scktClient.IpAddr = _arrOrderFixSrv[0].ToString();
                _scktClient.Port = Convert.ToInt32(_arrOrderFixSrv[1]);
                
                _scktClient.OnConnectionOpened += new ConnectionOpenedHandler(_scktClient_OnConnectionOpened);

                // Receives / Responses
                _scktClient.AddHandler<TOOrderFixInfo>(new ProtoObjectReceivedHandler<TOOrderFixInfo>(_scktClient_OnOrderReceived));
                _scktClient.AddHandler<LogonSrvResponse>(new ProtoObjectReceivedHandler<LogonSrvResponse>(_scktClient_OnLogonResponse));
                _scktClient.AddHandler<SondaSrvInfo>(new ProtoObjectReceivedHandler<SondaSrvInfo>(_scktClient_OnSondaSrvIInfoReceived));
                
                _isRunning = true;
                _thConnect = new Thread(new ThreadStart(_tryConnect));
                _thConnect.Start();
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no start do A4SocketClient:" + ex.Message, ex);
            }
        }

        

        public void Stop()
        {
            try
            {
                logger.Info("Parando A4Socket Client...");
                _isRunning = false;
                if (null != _thConnect)
                {
                    try
                    {
                        if (_thConnect.IsAlive)
                        {
                            _thConnect.Join(1000);
                            if (_thConnect.IsAlive) _thConnect.Abort();
                            _thConnect = null;
                        }
                    }
                    catch { }
                }

                
                if (null != _scktClient)
                {
                    if (_scktClient.IsConectado())
                        _scktClient.CloseSocket();
                    
                    _scktClient.OnConnectionOpened -= _scktClient_OnConnectionOpened;
                    
                    _scktClient.Dispose();
                    _scktClient = null;
                }

            }
            catch (Exception ex)
            {
                logger.Error("Problemas ao parar o A4SocketClient: " + ex.Message, ex);
            }
        }
        #endregion

        


        #region "SocketEvents"
        
        /// <summary>
        ///  Evento para envio de "LogonRequest"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void _scktClient_OnConnectionOpened(object sender, ConnectionOpenedEventArgs args)
        {
            try
            {
                logger.Info("Cliente CONECTADO!!");
                LogonSrvRequest req = new LogonSrvRequest();
                req.AppDescription = "Acompanhamento4Socket";
                if (null != _scktClient)
                    _scktClient.SendObject(req);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na abertura da conexao: " + ex.Message, ex);
            }
        }
        

        void _scktClient_OnOrderReceived(object sender, int clientNumber, Socket clientSocket, TOOrderFixInfo args)
        {
            try
            {
                OrderCache4Socket.GetInstance().EnqueueOrderInfo(args);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no enfileiramento do TOOrderFixInfo: " + ex.Message, ex);
            }
        }

        void _scktClient_OnLogonResponse(object sender, int clientNumber, Socket clientSocket, LogonSrvResponse args)
        {

            logger.Info("Logon Received: " + args.SessionID);
            OrderCache4Socket.GetInstance().Connected(true);
        }

        void _scktClient_OnSondaSrvIInfoReceived(object sender, int clientNumber, Socket clientSocket, SondaSrvInfo args)
        {
            logger.Info("Sonda received: " + args.TimeStamp);
            if (_scktClient.IsConectado())
            {
                SondaSrvInfo sonda = new SondaSrvInfo();
                sonda.TimeStamp = DateTime.Now.Ticks;
                _scktClient.SendObject(sonda);
            }
        }

        #endregion

        #region Thread Control
        private void _tryConnect()
        {
            try
            {
                int i = 0;
                try
                {
                    if (null != _scktClient)
                    {
                        if (!_scktClient.IsConectado())
                        {
                            logger.Info("Tentando efetuar conexao ao servidor...");
                            OrderCache4Socket.GetInstance().Connected(false);
                            _scktClient.OpenConnection();
                            // OrderCache4Socket.GetInstance().Connected(true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Nao foi possivel conectar...: " + ex.Message, ex);
                }
                while (_isRunning)
                {

                    if (i >= 300)
                    {
                        if (null != _scktClient)
                        {
                            if (!_scktClient.IsConectado())
                            {
                                logger.Info("Tentando efetuar conexao ao servidor...");
                                OrderCache4Socket.GetInstance().Connected(false);
                                _scktClient.OpenConnection();
                                // OrderCache4Socket.GetInstance().Connected(true);
                            }
                        }
                        i = 0;
                    }
                    Thread.Sleep(100);
                    i++;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problema na tentativa de conexao com o server OrderFixMsg: " + ex.Message, ex);
                OrderCache4Socket.GetInstance().Connected(false);
            }
        }
        #endregion


        


    }
}
