using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.Spider.CommSocket;
using System.Configuration;
using Gradual.Core.Spider.OrderFixProcessing.Lib.Mensagens;
using System.Threading;
using System.Net.Sockets;
using Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem;
using Gradual.Core.Spider.OrderFixProcessing.Lib.Dados;
using Gradual.Spider.Acompanhamento4Socket.Cache;
using Gradual.Spider.Acompanhamento4Socket.Lib.Dados;

namespace Gradual.Spider.Acompanhamento4Socket.Rede
{
    public class A4SocketSrv
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion


        #region private variables
        static A4SocketSrv _me = null;
        SpiderSocket _sckServer = null;

        int _port;
        //Dictionary<int, AcConnectionInfo> _dicConn;
        Dictionary<int, A4SocketClientConnection> _dicConn;
        Dictionary<int, A4SocketClientConnection> _dicAcToSend;
        Dictionary<int, StreamerClientHandler> _dicStreamerOrder;
        bool _isRunning;
        Thread _thMonitor;

        int _maxDetailCount = 0;
        #endregion

        public A4SocketSrv()
        {
            _dicConn = new Dictionary<int, A4SocketClientConnection>();
            _dicAcToSend = new Dictionary<int, A4SocketClientConnection>();
            _dicStreamerOrder = new Dictionary<int, StreamerClientHandler>();
            if (ConfigurationManager.AppSettings.AllKeys.Contains("MaxDetailCount"))
                _maxDetailCount = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDetailCount"]);
            else
                _maxDetailCount = 0;
        }

        ~A4SocketSrv()
        {
            _dicConn = null;
            _dicAcToSend = null;
            _dicStreamerOrder = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static A4SocketSrv GetInstance()
        {
            if (_me == null)
            {
                _me = new A4SocketSrv();
            }
            return _me;
        }

        public void Start()
        {
            try
            {
                logger.Info("Iniciando o Ac4Socket Server...");
                
                if (ConfigurationManager.AppSettings.AllKeys.Contains("PortServerListener"))
                {
                    _port = Convert.ToInt32(ConfigurationManager.AppSettings["PortServerListener"].ToString());
                }
                else
                    throw new Exception("Parametro Port Server Obrigatorio!!!");

                logger.Info("Iniciando Listener...:" + _port);
                _sckServer = new SpiderSocket();
                
                _sckServer.OnClientConnected += new ClientConnectedHandler(_sckServer_OnClientConnected);
                _sckServer.OnClientDisconnected += new ClientDisconnectedHandler(_sckServer_OnClientDisconnected);
                _sckServer.OnUnmappedObjectReceived += new UnmappedObjectReceivedHandler(_sckServer_OnUnmappedObjectReceived);

                // Message Requests
                _sckServer.AddHandler<SondaSrvInfo>(new ProtoObjectReceivedHandler<SondaSrvInfo>(_sckServer_OnSondaSrv));
                _sckServer.AddHandler<LogonSrvRequest>(new ProtoObjectReceivedHandler<LogonSrvRequest>(_sckServer_OnLogonSrvReq));
                _sckServer.AddHandler<A4SocketRequest>(new ProtoObjectReceivedHandler<A4SocketRequest>(_sckServer_OnA4SocketReq));
                _sckServer.AddHandler<FilterStreamerRequest>(new ProtoObjectReceivedHandler<FilterStreamerRequest>(_sckServer_OnFilterStreamRequest));
                _sckServer.AddHandler<FilterInfoRequest>(new ProtoObjectReceivedHandler<FilterInfoRequest>(_sckServer_OnFilterInfoReq));
                _sckServer.AddHandler<FilterDetailInfoRequest>(new ProtoObjectReceivedHandler<FilterDetailInfoRequest>(_sckServer_OnFilterDetailInfoReq));

                _sckServer.StartListen(_port);
                _isRunning = true;

                logger.Info("Iniciando Monitor de Clientes...");
                _thMonitor = new Thread(new ThreadStart(this._monitorClientes));
                _thMonitor.Start();

                logger.Info("A4SocketServer iniciado!!!!");
                
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na inicializacao A4Socket Server " + ex.Message, ex);
            }
        }


        public void Stop()
        {
            try
            {
                logger.Info("Parando o Ac4Socket Server...");

                logger.Info("Fechando socket...");
                if (null != _sckServer)
                {
                    _sckServer.StopListen();
                    _sckServer.OnClientConnected -= _sckServer_OnClientConnected;
                    _sckServer.OnClientDisconnected -= _sckServer_OnClientDisconnected;
                    _sckServer.OnUnmappedObjectReceived -= _sckServer_OnUnmappedObjectReceived;
                    _sckServer.Dispose();
                    _sckServer = null;
                }

                lock (_dicConn)
                    _dicConn.Clear();
                _dicConn = null;

                logger.Info("Parando thread de monitoramento de clientes...");
                if (null != _thMonitor)
                {
                    if (_thMonitor.IsAlive)
                    {
                        _thMonitor.Join(1000);
                        if (_thMonitor.IsAlive) _thMonitor.Abort();
                        _thMonitor = null;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na parada do Ac4Socket Server: " + ex.Message, ex);
            }
        }


        #region Message / Socket Events
        void _sckServer_OnClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            try
            {

                lock (_dicAcToSend)
                {
                    if (_dicAcToSend.ContainsKey(args.ClientNumber))
                    {
                        _dicAcToSend.Remove(args.ClientNumber);
                        logger.InfoFormat("Cliente AcOrdem Broadcast [{0}]:[{1}] removido", args.ClientNumber, args.ClientSocket);
                    }
                }
                lock (_dicStreamerOrder)
                {
                    StreamerClientHandler item = null;
                    if (_dicStreamerOrder.TryGetValue ( args.ClientNumber, out item))
                    {
                        item.Stop();
                        item = null;
                        _dicStreamerOrder.Remove(args.ClientNumber);
                        logger.InfoFormat("Cliente StreamerOrder [{0}]:[{1}] removido", args.ClientNumber, args.ClientSocket);
                    }
                }

                lock (_dicConn)
                {
                    if (_dicConn.ContainsKey(args.ClientNumber))
                    {
                        logger.Info("Parando cliente " + args.ClientNumber);
                        _dicConn[args.ClientNumber].Stop();
                        _dicConn[args.ClientNumber] = null;
                        _dicConn.Remove(args.ClientNumber);
                        logger.InfoFormat("Cliente [{0}]:[{1}] removido", args.ClientNumber, args.ClientSocket);
                    }
                }
                
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("ClientNumber: {0} ClientSocket: {1}", args.ClientNumber, args.ClientSocket);
                logger.Error("Problemas na desconexao do cliente: " + ex.Message, ex);
            }
        }

        void _sckServer_OnClientConnected(object sender, ClientConnectedEventArgs args)
        {
            try
            {
                lock (_dicConn)
                {
                    A4SocketClientConnection conn = new A4SocketClientConnection();
                    AcConnectionInfo aux = new AcConnectionInfo();
                    logger.InfoFormat("Conexao recebida: ClientID: [{0}] ClientSocket: [{1}]", args.ClientNumber, args.ClientSocket.RemoteEndPoint.ToString());
                    aux.ClientNumber = args.ClientNumber;
                    aux.ClientSocket = args.ClientSocket;
                    // Ja atribuio logon para comecar a enfileirar a mensagem
                    aux.Logged = true;
                    conn.ConnectionInfo = aux;
                    _dicConn.Add(args.ClientNumber, conn);
                    conn.Start();
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("ClientNumber: {0} ClientSocket: {1}", args.ClientNumber, args.ClientSocket);
                logger.Error("Problemas na conexao do cliente: " + ex.Message, ex);
            }
        }

        void _sckServer_OnSondaSrv(object sender, int clientNumber, Socket clientSocket, SondaSrvInfo args)
        {
            logger.Debug("Recebeu sonda do cliente [" + clientNumber + "]");
            A4SocketClientConnection item = null;
            if (_dicConn.TryGetValue(clientNumber, out item))
            {
                item.ConnectionInfo.LastSonda = args.TimeStamp;
            }
        }

        void _sckServer_OnLogonSrvReq(object sender, int clientNumber, Socket clientSocket, LogonSrvRequest args)
        {
            LogonSrvResponse resp = new LogonSrvResponse();
            resp.SessionID = Guid.NewGuid().ToString();
            logger.InfoFormat("Recebeu logon do cliente SessionID: [{0}]", resp.SessionID);
            //if (_dicConn.ContainsKey(clientNumber))
            //    _dicConn[clientNumber].ConnectionInfo.Logged = true;
            if (args.AppDescription == AppType.A4SOCKET_FULL_DETAIL)
                _dicConn[clientNumber].ConnectionInfo.RestrictDetails = false;
            else
                _dicConn[clientNumber].ConnectionInfo.RestrictDetails = true;
            if (null != _sckServer)
                SpiderSocket.SendObject(resp, clientSocket);

            // TODO [FF] - Montar o snapshot e mandar os objetos...
            logger.Info("Inicio de montagem de snapshot...");
            List<SpiderOrderInfo> lst = OrderCache4Socket.GetInstance().GetMemorySnapshot();
            logger.Info("Fim de montagem de snapshot...");
            lock (lst)
            {
                if (lst != null)
                {
                    logger.InfoFormat("Processando o snapshot: " + lst.Count);
                    bool ret = _dicConn[clientNumber].ProcessSnapshot(lst);

                }
                else
                {
                    logger.ErrorFormat("Problemas na geracao/envio do snapshot. ClientNumber[{0}]", clientNumber);
                }
            }
        }

        void _sckServer_OnUnmappedObjectReceived(object sender, int clientNumber, Socket clientSock, Type objectType, object objeto)
        {
            logger.Debug("Recebeu objeto nao mapeado... " + objeto.GetType());
        }


        void _sckServer_OnA4SocketReq(object sender, int clientNumber, Socket clientSocket, A4SocketRequest args)
        {
            try
            {
                logger.InfoFormat("Recebido A4SocketRequest: ClientNumber [{0}] AppDesc:[{1}] ", clientNumber, args.AppDescription);
                A4SocketClientConnection item = null;
                if (_dicConn.TryGetValue(clientNumber, out item))
                {
                    lock(_dicAcToSend)
                        _dicAcToSend.Add(clientNumber, item);
                }
                
                A4SocketResponse resp = new A4SocketResponse();
                SpiderSocket.SendObject(resp, clientSocket);

            }
            catch (Exception ex)
            {
                logger.Error("Problemas no registro de AC4SocketRequest: " + ex.Message, ex);
            }
        }

        void _sckServer_OnFilterInfoReq(object sender, int clientNumber, Socket clientSocket, FilterInfoRequest args)
        {
            try
            {
                ExecResp ret;
                FilterInfoResponse resp = new FilterInfoResponse();
                if (logger.IsDebugEnabled)
                    logger.DebugFormat("ClientNumber: [{0}] Socket[{1}] RecordLimit[{2}] Filter[{3}]", clientNumber, clientSocket.RemoteEndPoint.ToString(), args.RecordLimit, args.Filter.ToString());
                resp.Orders = OrderCache4Socket.GetInstance().GetOrders(args.Filter, args.RecordLimit, args.ReturnDetails, out ret);
                resp.ErrCode = ret.Code;
                resp.ErrMsg = ret.Msg;
                resp.Id = args.Id;
                SpiderSocket.SendObject(resp, clientSocket);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no request do FilterInfoRequest: " + ex.Message, ex);
            }
        }

        void _sckServer_OnFilterDetailInfoReq(object sender, int clientNumber, Socket clientSocket, FilterDetailInfoRequest args)
        {
            try
            {
                
                ExecResp ret;
                FilterDetailInfoResponse resp = new FilterDetailInfoResponse();
                
                if (logger.IsDebugEnabled)
                    logger.DebugFormat("ClientNumber: [{0}] Socket[{1}] OrderID[{2}]", clientNumber, clientSocket.RemoteEndPoint.ToString(), args.OrderID);
                bool restrictDetails = _dicConn[clientNumber].ConnectionInfo.RestrictDetails;
                resp.Details = OrderCache4Socket.GetInstance().GetOrderDetails(args.OrderID, out ret, restrictDetails);
                resp.ErrCode = ret.Code;
                resp.ErrMsg = ret.Msg;
                resp.Id = args.Id;
                SpiderSocket.SendObject(resp, clientSocket);
                 
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no request do FilterDetailInfoRequest: " + ex.Message, ex);
            }
        }

        void _sckServer_OnFilterStreamRequest(object sender, int clientNumber, Socket clientSocket, FilterStreamerRequest args)
        {
            try
            {   
                logger.InfoFormat("FilterStreamRequest: Account[{0}] Session: [{1}] Symbol: [{2}]", args.Account.Value, args.SessionID.Value, args.Symbol.Value);
                StreamerClientHandler st = null;
                FilterStreamerResponse resp = new FilterStreamerResponse();
                resp.Id = args.Id;
                bool found = false;
                lock(_dicStreamerOrder)
                {
                    if (!_dicStreamerOrder.TryGetValue(clientNumber, out st))
                    {

                        logger.Info("Nova conexao: " + clientNumber);
                        st = new StreamerClientHandler();
                        _dicStreamerOrder.Add(clientNumber, st);
                        bool restrictDet = _dicConn[clientNumber].ConnectionInfo.RestrictDetails;
                        st.SetFilter(args);
                        st.SetConnection(clientNumber, clientSocket, restrictDet);
                        st.Start();
                        if (!st.ComposeSnapShot(resp))
                        {
                            logger.Error("Erro na geracao do snapshot: " + resp.ErrMsg);
                        }
                        SpiderSocket.SendObject(resp, clientSocket);
                    }
                    else
                        found = true;
                }
                
                if (found)
                {
                    st.CancelTransfer(true);
                    Thread.Sleep(100);
                    st.SetFilter(args);
                    st.CancelTransfer(false);
                    if (!st.ComposeSnapShot(resp))
                    {
                        logger.Error("Erro na geracao do snapshot: " + resp.ErrMsg);
                    }
                    SpiderSocket.SendObject(resp, clientSocket);
                }


            }
            catch (Exception ex)
            {
                logger.Error("Problemas no request do FilterStreamRequest: " + ex.Message, ex);
            }
        }


        public void SendAcOrdem(SpiderOrderInfo order)
        {
            try
            {
                int len = 0;
                List<A4SocketClientConnection> lst = null;
                if (_dicConn.Count > 0)
                {
                    lst = _dicConn.Select(x => x.Value).Where(x => x.ConnectionInfo.Logged == true).ToList();
                    len = lst == null ? 0 : lst.Count;
                    for (int i = 0; i < len; i++)
                    {
                        // Adiciona para filas indedendentes de cada conexao
                        lst[i].AddMsg(order);
                        // SpiderSocket.SendObject(order, lst[i].ConnectionInfo.ClientSocket);
                    }
                    lst.Clear();
                    lst = null;
                }
                order = null;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no envio do AcOrdem: " + order.OrderID + " --- " + ex.Message, ex);
            }
        }

        public void SendStreamerOrder(SpiderOrderInfo xx)
        {
            try
            {
                if (_dicStreamerOrder.Count > 0)
                {
                    for (int i = 0; i < _dicStreamerOrder.Values.Count; i++)
                    {
                        StreamerClientHandler item = _dicStreamerOrder.Values.ElementAt(i);
                        if (item.VerifyFilter(xx))
                        {
                            StreamerOrderInfo ord = new StreamerOrderInfo();
                            ord.MsgType = MsgTypeConst.INCREMENTAL;
                            ord.Order = xx;
                            // Fazer filtragem dos details em caso de _maxCount!=0
                            if (_maxDetailCount != 0)
                            {
                                List<SpiderOrderDetailInfo> aux = new List<SpiderOrderDetailInfo>(ord.Order.Details.Where(x => x.OrderStatusID != 100 && x.OrderStatusID != 101 && x.OrderStatusID != 102).OrderBy(x => x.TransactTime));
                                int count = aux.Count;
                                ord.Order.Details.Clear();
                                ord.Order.Details.AddRange(aux.Skip(count-_maxDetailCount));
                            }
                            // SpiderSocket.SendObject(ord, item.GetConnection().ClientSocket);
                            item.EnqueueStreamerOrder(ord);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no envio do Streamer Order: " + ex.Message, ex);
            }
        }


        #endregion

        #region Thread Controls
        private void _monitorClientes()
        {
            logger.Info("Iniciando a monitoracao dos clientes conectados");
            long lastSonda = 0;
            List<int> toDelete = new List<int>();
            try
            {
                while (_isRunning)
                {
                    TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - lastSonda);
                    toDelete.Clear();

                    if (ts.TotalMilliseconds > 30000 && _dicConn.Count > 0)
                    {
                        lastSonda = DateTime.Now.Ticks;
                        logger.Info("Enviando sonda para " + _dicConn.Count + " clientes");

                        foreach (A4SocketClientConnection strut in _dicConn.Values)
                        {
                            try
                            {
                                SondaSrvInfo sonda = new SondaSrvInfo();
                                logger.Debug("Enviando sonda para " + strut.ConnectionInfo.ClientSocket.RemoteEndPoint.ToString() + " Connection: " + strut.ConnectionInfo.ClientSocket.Connected);
                                SpiderSocket.SendObject(sonda, strut.ConnectionInfo.ClientSocket);
                            }
                            catch
                            {
                                logger.Error("Erro ao enviar sonda para cliente [" + strut.ConnectionInfo.ClientNumber + "], removendo");
                                toDelete.Add(strut.ConnectionInfo.ClientNumber);
                            }
                        }

                        foreach (int clientNumber in toDelete)
                        {
                            try
                            {
                                _dicConn[clientNumber].ConnectionInfo.ClientSocket.Close();
                                _dicConn[clientNumber].Stop();
                            }
                            catch
                            {
                                logger.Error("Erro ao fechar socket");
                            }

                            lock (_dicConn)
                                _dicConn.Remove(clientNumber);
                            lock (_dicAcToSend)
                            {
                                if (_dicAcToSend.ContainsKey(clientNumber))
                                    _dicAcToSend.Remove(clientNumber);
                            }
                            lock (_dicStreamerOrder)
                            {
                                StreamerClientHandler item = null;
                                if (_dicStreamerOrder.TryGetValue(clientNumber, out item))
                                {
                                    item.Stop();
                                    item = null;
                                    _dicStreamerOrder.Remove(clientNumber);
                                }
                            }
                        }
                    }
                    Thread.Sleep(100);
                }

                logger.Info("Finalizando a monitoracao dos clientes conectados");
            }
            catch (Exception ex)
            {
                logger.Error("Erro no Monitor Cliente: " + ex.Message, ex);
            }
        }
        #endregion



    }
}
