using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.Spider.CommSocket;
using System.Collections.Concurrent;
using Gradual.Core.Spider.OrderFixProcessing.Lib.Dados;
using System.Threading;
using Gradual.Spider.SupervisorRisco.DB.Lib;
using Gradual.Spider.Acompanhamento4Socket.Lib.Dados;
using System.Configuration;
using Gradual.Core.Spider.OrderFixProcessing.Lib.Mensagens;
using System.Net.Sockets;
using Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem;
using Gradual.Spider.SupervisorRisco.Lib.Dados;

namespace Gradual.Spider.ServicoSupervisor.Memory
{
    public class AcSpiderCache
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        

        #region private variables
        private string APP_NAME = "ServicoSupervisorRisco";
        
        SpiderSocket _client;
        ConcurrentDictionary<int, ConcurrentDictionary<int, SpiderOrderInfo>> _orders;
        bool _isRunning;
        bool _isConnected;
        bool _isProcessingSnap;
        bool _isSnapshotLoaded;
        bool _canDequeue;
        DbAc4Socket _dbAc;
        ConcurrentDictionary<int, List<SpiderOrderDetailInfo>> _orderdetails;
        ConcurrentQueue<TOSpOrder> _cqOpLimit;
        ConcurrentQueue<TOSpOrder> _cqMaxLoss;
        ConcurrentQueue<TOSpOrder> _cqRestSymbol;
        ConcurrentQueue<TOSpOrder> _cqRestGroup;
        ConcurrentQueue<TOSpOrder> _cqRestGlobal;
        ConcurrentQueue<TOSpOrder> _cqPosClient;
        object _sync1 = new object();
        object _sync2 = new object();
        object _sync3 = new object();
        object _sync4 = new object();
        object _sync5 = new object();
        object _sync6 = new object();
        Dictionary<int, SpiderIntegration> _dicIntegration;

        Thread _thLoadSnapShot = null;
        Thread _thOpLimit = null;
        Thread _thMaxLoss = null;
        Thread _thRestSymbol = null;
        Thread _thRestGroup = null;
        Thread _thRestGlobal = null;
        Thread _thPosClient = null;
        Thread _thConnect = null;
        bool _memorySnapshot;

        #endregion

        #region Static Objects
        private static AcSpiderCache _me = null;
        public static AcSpiderCache Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new AcSpiderCache();
                }
                return _me;
            }
        }
        #endregion

        // Constructor / Destructor
        public AcSpiderCache()
        {
            _isRunning = false;

            this._setConnectionFlag(false);
            //_isConnected = false;
            //_isProcessingSnap = false;
            //_isSnapshotLoaded = false;
            
            _dbAc = new DbAc4Socket();
            _orders = new ConcurrentDictionary<int, ConcurrentDictionary<int, SpiderOrderInfo>>();
            _orderdetails = new ConcurrentDictionary<int, List<SpiderOrderDetailInfo>>();

            _cqOpLimit = new ConcurrentQueue<TOSpOrder>();
            _cqMaxLoss = new ConcurrentQueue<TOSpOrder>();
            _cqRestGlobal = new ConcurrentQueue<TOSpOrder>();
            _cqRestGroup = new ConcurrentQueue<TOSpOrder>();
            _cqRestSymbol = new ConcurrentQueue<TOSpOrder>();
            _cqPosClient = new ConcurrentQueue<TOSpOrder>();
        }

        ~AcSpiderCache()
        {

        }

        public void Start()
        {
            try
            {
                logger.Info("Iniciando AcSpiderCache...");
                logger.Info("Carregando SpiderIntegration Info");
                this._loadIntegrationName();

                _isRunning = true;
                _canDequeue = true;
                _memorySnapshot = false;

                // COMENTADO PARA EVITAR PROCESSAMENTO DESNECESSARIO
                //logger.Info("Iniciando Thread OperationalLimit...");
                //_thOpLimit = new Thread(new ThreadStart(_dequeueOpLimit));
                //_thOpLimit.Start();
                //logger.Info("Iniciando Thread MaxLoss...");
                //_thMaxLoss = new Thread(new ThreadStart(_dequeueMaxLoss));
                //_thMaxLoss.Start();
                //logger.Info("Iniciando Thread RestricaoSymbol...");
                //_thRestSymbol = new Thread(new ThreadStart(_dequeueRestSymbol));
                //_thRestSymbol.Start();
                //logger.Info("Iniciando Thread RestricaoGrupo...");
                //_thRestGroup = new Thread(new ThreadStart(_dequeueRestGroup));
                //_thRestGroup.Start();
                //logger.Info("Iniciando Thread RestricaoGlobal...");
                //_thRestGlobal = new Thread(new ThreadStart(_dequeueRestGlobal));
                //_thRestGlobal.Start();
                logger.Info("Iniciando Thread PositionClient...");
                _thPosClient = new Thread(new ThreadStart(_dequeuePosClient));
                _thPosClient.Start();

                logger.Info("Tentando conectar ao Acompanhamento de Ordens...");
                string [] arrParam;
                if (ConfigurationManager.AppSettings.AllKeys.Contains("Ac4SocketSrv"))
                {
                    arrParam = ConfigurationManager.AppSettings["Ac4SocketSrv"].ToString().Split(new char [] {':'});
                }
                else
                    throw new Exception("Parametro Ac4SocketSrv é obrigatorio");
                logger.InfoFormat("Efetuando conexao com o servidor Acompanhamento de Ordens 4 Socket: [{0}:{1}]", arrParam[0], arrParam[1]);
                _client = new SpiderSocket();
                _client.IpAddr = arrParam[0].ToString();
                _client.Port = Convert.ToInt32(arrParam[1]);
                _client.OnConnectionOpened += new ConnectionOpenedHandler(_client_OnConnectionOpened);

                _client.AddHandler<LogonSrvResponse>(new ProtoObjectReceivedHandler<LogonSrvResponse>(_client_OnLogonResponse));
                _client.AddHandler<SpiderOrderInfo>(new ProtoObjectReceivedHandler<SpiderOrderInfo>(_client_OnSpiderOrderInfoReceived));
                _client.AddHandler<SondaSrvInfo>(new ProtoObjectReceivedHandler<SondaSrvInfo>(_client_OnSondaSrvInfoReceived));
                
                _thConnect = new Thread(new ThreadStart(_tryConnect));
                _thConnect.Start();
                Thread.Sleep(1000); // Esperar para conseguir a primeira conexao
                //logger.Info("Iniciando Thread LoadSnapShot...");
                //_isRunning = true;
                //_thLoadSnapShot = new Thread(new ThreadStart(_loadOrderSnapshot));
                //_thLoadSnapShot.Start();

                

            }
            catch (Exception ex)
            {
                logger.Error("Problemas no start do cache de Acompanhamento de Ordens: " + ex.Message, ex);
            }

        }

       

        public void Stop()
        {
            try
            {
                logger.Info("Parando AcSpiderCache... ");
                _isRunning = false;
                logger.Info("Parando Thread LoadSnapShot...");
                if (null != _thLoadSnapShot)
                {
                    if (_thLoadSnapShot.IsAlive) _thLoadSnapShot.Join(500);
                    if (_thLoadSnapShot.IsAlive) _thLoadSnapShot.Abort();
                    _thLoadSnapShot = null;
                }
                //logger.Info("Parando Thread OperationalLimit...");
                //if (null != _thOpLimit)
                //{
                //    if (_thOpLimit.IsAlive) _thOpLimit.Join(500);
                //    if (_thOpLimit.IsAlive) _thOpLimit.Abort();
                //    _thOpLimit = null;
                //}
                //logger.Info("Parando Thread PerdaMaxima...");
                //if (null != _thMaxLoss)
                //{
                //    if (_thMaxLoss.IsAlive) _thMaxLoss.Join(500);
                //    if (_thMaxLoss.IsAlive) _thMaxLoss.Abort();
                //    _thMaxLoss = null;
                //}
                //logger.Info("Parando Thread RestricoesGlobais...");
                //if (null != _thRestGlobal)
                //{
                //    if (_thRestGlobal.IsAlive) _thRestGlobal.Join(500);
                //    if (_thRestGlobal.IsAlive) _thRestGlobal.Abort();
                //    _thRestGlobal = null;
                //}
                //logger.Info("Parando Thread RestricoesGrupo...");
                //if (null != _thRestGroup)
                //{
                //    if (_thRestGroup.IsAlive) _thRestGroup.Join(500);
                //    if (_thRestGroup.IsAlive) _thRestGroup.Abort();
                //    _thRestGroup = null;
                //}
                //logger.Info("Parando Thread RestricoesSimbolo...");
                //if (null != _thRestSymbol)
                //{
                //    if (_thRestSymbol.IsAlive) _thRestSymbol.Join(500);
                //    if (_thRestSymbol.IsAlive) _thRestSymbol.Abort();
                //    _thRestSymbol = null;
                //}

                logger.Info("Parando Thread PositionClient...");
                if (null != _thPosClient)
                {
                    if (_thPosClient.IsAlive) _thPosClient.Join(500);
                    if (_thPosClient.IsAlive) _thPosClient.Abort();
                    _thPosClient = null;
                }
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
                
                if (null != _client)
                {
                    if (_client.IsConectado())
                        _client.CloseSocket();

                    _client.OnConnectionOpened -= _client_OnConnectionOpened;

                    _client.Dispose();
                    _client = null;
                }

                
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no stop do cache de Acompanhamento de Ordens: " + ex.Message, ex);
            }
        }


        private bool _loadIntegrationName()
        {
            try
            {
                if (null == _dbAc)
                {
                    _dbAc = new DbAc4Socket();
                }
                _dicIntegration = _dbAc.LoadIntegrationInfo();
                if (_dicIntegration == null)
                    return false;

                logger.Info("IntegrationInfo Count: " + _dicIntegration.Count);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na carga dos IntegrationNames: " + ex.Message, ex);
                return false;
            }
        }

        //private void _loadOrderSnapshot()
        //{
        //    try
        //    {
        //        int i = 150;
        //        while (_isRunning)
        //        {
        //            i++;
        //            if (i >= 150)
        //            {
        //                if (_isConnected && !_isProcessingSnap)
        //                {
                            
        //                    _isProcessingSnap = true;
        //                    logger.Info("Iniciando montagem do snapshot (Database) ...");
        //                    bool ret = _dbAc.LoadOrderSnapshot(DateTime.Now, out _orders, out _orderdetails);
        //                    _canDequeue = false;
        //                    logger.Info("Fim montagem do snapshot (Database)");
        //                    if (_orders != null)
        //                    {
        //                        logger.Info(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> INI PROCESSAMENTO SNAPSHOT");
        //                        foreach (KeyValuePair<int, ConcurrentDictionary<int, SpiderOrderInfo>> item in _orders)
        //                        {
        //                            // logger.InfoFormat("Account: [{0}] Orders: [{1}]", item.Key, item.Value.Count);
        //                            for (int j = 0; j < item.Value.Count; j++)
        //                            {
        //                                SpiderOrderInfo aux = item.Value.Values.ElementAt(j);
        //                                TOSpOrder to = new TOSpOrder();
        //                                to.MsgType = MsgTOType.SNAPSHOT;
        //                                to.Order = aux;
        //                                this._processSnapshotOrders(to);
        //                                aux = null;
        //                            }
        //                        }
        //                        _orders.Clear();
        //                        _orders = null;
        //                        _orderdetails.Clear();
        //                        _orderdetails = null;
        //                        logger.Info(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> FIM PROCESSAMENTO SNAPSHOT");
        //                        _canDequeue = true;
        //                        _isSnapshotLoaded = true;
        //                    }
        //                    else
        //                    {
        //                        logger.ErrorFormat("Erro na carga do Snapshot!!!");
        //                        _isProcessingSnap = false;
        //                    }
        //                }
        //            }
        //            Thread.Sleep(200);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("Problemas no processamento da thread de carga de snapshot: " + ex.Message, ex);
        //    }
        //}


        #region Socket Events
        void _client_OnConnectionOpened(object sender, ConnectionOpenedEventArgs args)
        {
            try
            {
                logger.Info("Cliente CONECTADO!!");
                LogonSrvRequest req = new LogonSrvRequest();
                req.AppDescription = AppType.A4SOCKET_FULL_DETAIL;
                if (null != _client)
                    _client.SendObject(req);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na abertura da conexao: " + ex.Message, ex);
            }
        }

        void _client_OnLogonResponse(object sender, int clientNumber, Socket clientSocket, LogonSrvResponse args)
        {

            logger.Info("Logon Received: " + args.SessionID);
            this._setConnectionFlag(true);

            //if (null!=_client && _client.IsConectado())
            //{
            //    A4SocketRequest req = new A4SocketRequest();
            //    req.AppDescription = APP_NAME;
            //    _client.SendObject(req);
            //}
        }

        void _client_OnSondaSrvInfoReceived(object sender, int clientNumber, Socket clientSocket, SondaSrvInfo args)
        {
            logger.Info("Sonda received: " + args.TimeStamp);
            if (_client.IsConectado())
            {
                SondaSrvInfo sonda = new SondaSrvInfo();
                sonda.TimeStamp = DateTime.Now.Ticks;
                _client.SendObject(sonda);
            }
        }

        void _client_OnSpiderOrderInfoReceived(object sender, int clientNumber, Socket clientSocket, SpiderOrderInfo args)
        {
            try
            {

                if (logger.IsDebugEnabled)
                {
                    logger.DebugFormat("OrderID[{0}] Account[{1}] Symbol[{2}] Side[{3}] Status[{4}] Detail[{5}] OrderQty[{6}] OrderQtyRemaining[{7}] CumQty[{8}]",
                        args.OrderID, args.Account, args.Symbol, args.Side, args.OrdStatus, args.Details.Count, args.OrderQty, args.OrderQtyRemaining, args.CumQty);
                }

                TOSpOrder to = new TOSpOrder();
                to.MsgType = MsgTOType.INCREMENTAL;
                to.Order = args;
                this._enqueueOrderInfo(to);

            }
            catch (Exception ex)
            {
                logger.Error("Problemas no recebimento do SpiderOrderInfo: " + ex.Message, ex);
            }
        }

        #endregion

        #region Thread Control

        private void _setConnectionFlag(bool valor)
        {
            _isConnected = valor;
            if (false == valor)
            {
                _isProcessingSnap = false;
                _isSnapshotLoaded = false;
            }
        }

        public bool GetSnapshotFlag()
        {
            return _isSnapshotLoaded;
        }
        private void _tryConnect()
        {
            try
            {
                int i = 0;
                try
                {
                    if (null != _client)
                    {
                        if (!_client.IsConectado())
                        {
                            logger.Info("Tentando efetuar conexao ao servidor...");
                            // _isConnected = false;
                            this._setConnectionFlag(false);
                            _client.OpenConnection();
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
                        if (null != _client)
                        {
                            if (!_client.IsConectado())
                            {
                                logger.Info("Tentando efetuar conexao ao servidor...");
                                _isConnected = false;
                                this._setConnectionFlag(false);
                                _client.OpenConnection();
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
                logger.Error("Problema na tentativa de conexao com o server Ac4Socket: " + ex.Message, ex);
                this._setConnectionFlag(false);
            }
        }

        private void _enqueueOrderInfo(TOSpOrder to)
        {
            try
            {
                //_cqOpLimit.Enqueue(to);
                //_cqMaxLoss.Enqueue(to);
                //_cqRestGlobal.Enqueue(to);
                //_cqRestGroup.Enqueue(to);
                //_cqRestSymbol.Enqueue(to);
                _cqPosClient.Enqueue(to);
                //lock (_sync1) Monitor.Pulse(_sync1);
                //lock (_sync2) Monitor.Pulse(_sync2);
                //lock (_sync3) Monitor.Pulse(_sync3);
                //lock (_sync4) Monitor.Pulse(_sync4);
                //lock (_sync5) Monitor.Pulse(_sync5);
                lock (_sync6) Monitor.Pulse(_sync6);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no enfileiramneto da mensagem: " + ex.Message, ex);
            }
        }

        private void _dequeueOpLimit()
        {
            try
            {
                while (_isRunning)
                {
                    if (!_canDequeue)
                    {
                        lock (_sync1)
                            Monitor.Wait(_sync1, 100);

                    }
                    TOSpOrder item = null;
                    if (_cqOpLimit.TryDequeue(out item))
                    {
                        if (_isSnapshotLoaded)
                        {
                            if (item != null)
                                this._processOpLimit(item);
                        }
                    }
                    else
                    {
                        lock (_sync1)
                            Monitor.Wait(_sync1, 100);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da thread de carga de limite operacional: " + ex.Message, ex);
            }
        }

        private void _dequeueMaxLoss()
        {
            try
            {
                while (_isRunning)
                {
                    if (!_canDequeue)
                    {
                        lock (_sync2)
                            Monitor.Wait(_sync2, 100);
                    }
                    TOSpOrder item = null;
                    if (_cqMaxLoss.TryDequeue(out item))
                    {
                        if (_isSnapshotLoaded )
                        {
                            if (item != null)
                                this._processMaxLoss(item);
                        }
                    }
                    else
                    {
                        lock (_sync2)
                            Monitor.Wait(_sync2, 100);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da thread de perda maxima: " + ex.Message, ex);
            }
        }

        private void _dequeueRestSymbol()
        {
            try
            {
                while (_isRunning)
                {
                    if (!_canDequeue)
                    {
                        lock (_sync3)
                            Monitor.Wait(_sync3, 100);
                    }
                    TOSpOrder item = null;
                    if (_cqRestSymbol.TryDequeue(out item))
                    {
                        if (_isSnapshotLoaded)
                        {
                            if (item != null)
                                this._processRestSymbol(item);
                        }
                    }
                    else
                    {
                        lock (_sync3)
                            Monitor.Wait(_sync3, 100);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da thread de restricao de simbolo: " + ex.Message, ex);
            }
        }

        private void _dequeueRestGroup()
        {
            try
            {
                while (_isRunning)
                {
                    if (!_canDequeue)
                    {
                        lock (_sync4)
                            Monitor.Wait(_sync4, 100);
                    }
                    TOSpOrder item = null;
                    if (_cqRestGroup.TryDequeue(out item))
                    {
                        if (_isSnapshotLoaded)
                        {
                            if (item != null)
                                this._processRestGroup(item);
                        }
                    }
                    else
                    {
                        lock (_sync4)
                            Monitor.Wait(_sync4, 100);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da thread de restricao de grupo de simbolo: " + ex.Message, ex);
            }
        }

        private void _dequeueRestGlobal()
        {
            try
            {
                while (_isRunning)
                {
                    if (!_canDequeue)
                    {
                        lock (_sync4)
                            Monitor.Wait(_sync4, 100);
                    }

                    TOSpOrder item = null;
                    if (_cqRestGlobal.TryDequeue(out item))
                    {
                        if (_isSnapshotLoaded )
                        {
                            if (item != null)
                                this._processRestGlobal(item);
                        }
                    }
                    else
                    {
                        lock (_sync5)
                            Monitor.Wait(_sync5, 100);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da thread de restricao de symbol global: " + ex.Message, ex);
            }
        }

        private void _dequeuePosClient()
        {
            try
            {
                while (_isRunning)
                {
                    if (!_canDequeue)
                    {
                        lock (_sync6)
                            Monitor.Wait(_sync6, 100);
                    }
                    TOSpOrder item = null;
                    // Parando processamento para compor snapshot de memoria
                    //if (!_memorySnapshot)
                    //{
                        if (_cqPosClient.TryDequeue(out item))
                        {
                            if (item != null)
                                this._processPositionClient(item);
                        }
                        else
                        {
                            lock (_sync6)
                                Monitor.Wait(_sync6, 100);
                        }
                    //}
                    //else
                    //{
                    //    lock (_sync6)
                    //        Monitor.Wait(_sync6, 100);
                    //}
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da thread de position client: " + ex.Message, ex);
            }
        }


        private void _processOpLimit(TOSpOrder item)
        {
            try
            {
                // logger.InfoFormat("OrderID [{0}] Details Count: [{1}]", item.OrderID, item.Details.Count);
                // TODO[FF]
                // - definir quais tipos de limites deverao ser calculados aqui
                // - definir necessidade de novo enfileiramento para nao travar alguma fila
                

                // Calculo de limite "operacional" para bmf ou bovespa
                RiskCache.Instance.CalculateOperationalLimit(item);

            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem SpiderOrderInfo - OpLimit: " + ex.Message, ex);
            }
        }

        private void _processMaxLoss(TOSpOrder item)
        {
            try
            {
                RiskCache.Instance.CalculateMaxLoss(item);

            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem SpiderOrderInfo - MaxLoss: " + ex.Message, ex);
            }
        }

        private void _processRestSymbol(TOSpOrder item)
        {
            try
            {
                RiskCache.Instance.CalculateRestSymbol(item);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem SpiderOrderInfo- RestSymbol: " + ex.Message, ex);
            }
        }

        private void _processRestGroup(TOSpOrder item)
        {
            try
            {
                RiskCache.Instance.CalculateRestGroup(item);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem SpiderOrderInfo - RestGroup: " + ex.Message, ex);
            }
        }

        private void _processRestGlobal(TOSpOrder item)
        {
            try
            {
                RiskCache.Instance.CalculateRestGlobal(item);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem SpiderOrderInfo - RestGlobal: " + ex.Message, ex);
            }
        }

        private void _processPositionClient(TOSpOrder item)
        {
            try
            {
                PositionClientManager.Instance.CalculatePositionClient(item);
                
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem SpiderOrderInfo - PositionClient: " + ex.Message, ex);
            }
        }

        private void _processSnapshotOrders(TOSpOrder item)
        {
            //_processRestGlobal(item);
            //_processRestGroup(item);
            //_processRestSymbol(item);
            //_processOpLimit(item);
            //_processMaxLoss(item);
            _processPositionClient(item);
            item = null;
        }

        public void SetMemorySnapshot(bool val)
        {
            _memorySnapshot = val;
        }
        #endregion

    }
}
