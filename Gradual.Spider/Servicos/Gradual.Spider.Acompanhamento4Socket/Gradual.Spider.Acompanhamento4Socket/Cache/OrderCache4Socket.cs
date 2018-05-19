using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;

using log4net;
using Gradual.Spider.Acompanhamento4Socket.Db;
using Gradual.Spider.Acompanhamento4Socket.Lib.Dados;
using Gradual.Core.Spider.OrderFixProcessing.Lib.Dados;
using Gradual.Spider.Acompanhamento4Socket.Rede;
using Gradual.Spider.Acompanhamento4Socket.Lib.Util;
using Gradual.Spider.Acompanhamento4Socket;
using Gradual.Core.Spider.OrderFixProcessing.Lib.Util;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using System.Configuration;
using Gradual.Spider.CommSocket;

namespace Gradual.Spider.Acompanhamento4Socket.Cache
{
    public class OrderCache4Socket
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables
        private static OrderCache4Socket _me = null;
        private static object _objLock = new object();

        private DbAc4Socket _db = null;
        bool _isRunning;
        bool _isConnected;
        bool _isProcessingSnap;
        bool _isSnapshotLoaded;
        // Thread _thReconnect = null;
        Thread _thLoadSnapShot = null;
        Thread _thProcessQueue = null;
        ConcurrentQueue<TOOrderFixInfo> _cqMsg = null;
        object sync_ = new object();
        ConcurrentDictionary<int, ConcurrentDictionary<int, SpiderOrderInfo>> _orders;
        // ConcurrentDictionary<int, List<SpiderOrderDetailInfo>> _orderdetails;
        ConcurrentDictionary<int, ConcurrentDictionary<int, SpiderOrderDetailInfo>> _orderdetails;
        Dictionary<int, SpiderIntegration> _dicIntegration;
        int _maxDetailCount = 0;
        #endregion


        public static OrderCache4Socket GetInstance()
        {
            lock (_objLock)
            {
                if (_me == null)
                {
                    _me = new OrderCache4Socket();
                }
            }
            return _me;
        }

        public OrderCache4Socket()
        {
            _db = new DbAc4Socket();
            _cqMsg = new ConcurrentQueue<TOOrderFixInfo>();
            _isRunning = false;
            _isConnected = false;
            _isSnapshotLoaded = false;
            _isProcessingSnap = false;
            _orders = new ConcurrentDictionary<int, ConcurrentDictionary<int, SpiderOrderInfo>>();
            _orderdetails = new ConcurrentDictionary<int, ConcurrentDictionary<int, SpiderOrderDetailInfo>>();
            _dicIntegration = new Dictionary<int, SpiderIntegration>();

            if (ConfigurationManager.AppSettings.AllKeys.Contains("MaxDetailCount"))
            {
                _maxDetailCount = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDetailCount"]);
            }
            else
                _maxDetailCount = 0;
        }

        public void Start()
        {
            try
            {
                logger.Info("Iniciando OrderCache 4 Socket ...");

                logger.Info("Carregando SpiderIntegration Info");
                this._loadIntegrationName();

                _isRunning = true;
                logger.Info("Iniciando Thread LoadSnapShot...");
                _thLoadSnapShot = new Thread(new ThreadStart(_loadSnapshotProcess));
                _thLoadSnapShot.Start();

                logger.Info("Iniciando Thread ProcessQueue...");
                _thProcessQueue = new Thread(new ThreadStart(_dequeueProcess));
                _thProcessQueue.Start();
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no start do Order Cache 4 Socket: " + ex.Message, ex);
                throw;
            }
        }

        public void Stop()
        {
            try
            {
                logger.Info("Parando OrderCache 4 Socket... ");
                _isRunning = false;
                logger.Info("Parando Thread LoadSnapShot...");
                if (null != _thLoadSnapShot)
                {
                    if (_thLoadSnapShot.IsAlive) _thLoadSnapShot.Join(500);
                    if (_thLoadSnapShot.IsAlive) _thLoadSnapShot.Abort();
                    _thLoadSnapShot = null;
                }
                logger.Info("Parando Thread ProcessQueue...");
                if (null != _thProcessQueue)
                {
                    if (_thProcessQueue.IsAlive) _thProcessQueue.Join(500);
                    if (_thProcessQueue.IsAlive) _thProcessQueue.Abort();
                    _thProcessQueue = null;
                }

            }
            catch (Exception ex)
            {
                logger.Error("Problemas no stop do Order Cache 4 Socket: " + ex.Message, ex);
                throw;
            }
        }

        public void Connected(bool valor)
        {
            _isConnected = valor;
            if (false == valor)
            {
                _isProcessingSnap = false;
                _isSnapshotLoaded = false;
            }
        }

        #region Thread Controls
        private void _loadSnapshotProcess()
        {
            try
            {
                int i = 0;
                while (_isRunning)
                {
                    i++;
                    if (i >= 150)
                    {
                        if (_isConnected && !_isProcessingSnap)
                        {
                            _isProcessingSnap = true;
                            logger.Info("Iniciando montagem do snapshot...");
                            int maxSnapshot = 10;
                            int x = 0;
                            if (ConfigurationManager.AppSettings.AllKeys.Contains("MaxSnapshotTries"))
                                maxSnapshot = Convert.ToInt32(ConfigurationManager.AppSettings["MaxSnapshotTries"]);

                            while (x < maxSnapshot)
                            {
                                x++;
                                logger.Info("Tentativa de montagem de snapshot: " + x);
                                bool ret = _db.LoadOrderSnapshot(DateTime.Now, out _orders, out _orderdetails);
                                // Efetuar sincronizacao com informacoes existentes na fila
                                logger.Info("Order Count: " + _orders.Count);
                                logger.Info("OrderDetail Count: " + _orderdetails.Count);
                                _isSnapshotLoaded = _syncSnapshotOrders(_orders);

                                if (ret && _isSnapshotLoaded)
                                    break;
                            }
                            if (x >= maxSnapshot)
                                throw new Exception("Problemas na geracao do snapshot");

                            logger.Info("Fim montagem do snapshot");
                            if (_orders == null)
                            {
                                logger.ErrorFormat("Erro na carga do Snapshot!!!");
                                _isProcessingSnap = false;
                            }
                        }
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da thread de carga de snapshot: " + ex.Message, ex);
                throw;
            }
        }

        private bool _syncSnapshotOrders(   ConcurrentDictionary<int, ConcurrentDictionary<int, SpiderOrderInfo>> orders) 
                                            // ConcurrentDictionary<int, ConcurrentDictionary<int, SpiderOrderDetailInfo>> orderdetails) 
        {
            bool ret = false;
            try
            {
                logger.Info("Sincronizando o acompanhamento de ordens...");
                
                // Buscar o maior MsgSeqNum
                long maxSeqNum = 0;
                foreach (KeyValuePair<int, ConcurrentDictionary<int, SpiderOrderInfo>> item in orders)
                {
                    foreach (KeyValuePair<int, SpiderOrderInfo> item2 in item.Value)
                    {
                        if (item2.Value.MsgSeqNum >= maxSeqNum)
                            maxSeqNum = item2.Value.MsgSeqNum;
                    }
                }

                logger.Info("MaxMsgSeqNum encontrado nas ordens: " + maxSeqNum);

                // Descartar os itens que estiverem na fila
                long currentSeqNum = 0;
                while(true)
                {
                    // Se fila vazia, cai fora do laco
                    if (_cqMsg.Count == 0)
                    {
                        logger.Info("Fila zerada. Sincronizado!!!!!");
                        ret = true;
                        break;
                    }
                    TOOrderFixInfo to = null;
                    _cqMsg.TryPeek(out to);
                    if (to!=null)
                    {
                        currentSeqNum = to.OrderFixInfo.MsgSeqNum;
                        if (currentSeqNum - maxSeqNum > 1)
                        {
                            // TODO[FF]: Verificar como fazer o resnapshot
                            logger.ErrorFormat("Perda de mensagem. Necessario refazer o snapshot: Max[{0}] Current[{1}]", maxSeqNum, currentSeqNum);
                            ret = false;
                            break;
                        }
                        // Verificar perda de msg
                        if (currentSeqNum > maxSeqNum)
                        {
                            logger.InfoFormat("Nao precisa descartar mais nada... Sincronizado...Current:[{0}] MaxSnapshot[{1}]",
                                currentSeqNum, maxSeqNum);
                            break;
                        }

                        if (currentSeqNum  == maxSeqNum)
                        {
                            logger.InfoFormat("Ok... sincronizado: Current[{0}] MaxSnapshot[{1}]", currentSeqNum, maxSeqNum);
                            _cqMsg.TryDequeue(out to);
                            ret = true;
                            break;
                        }
                        if (currentSeqNum <= maxSeqNum)
                        {
                            logger.InfoFormat("Descartando OrderID[{0}] SeqNum[{1}]", to.OrderFixInfo.OrderID, to.OrderFixInfo.MsgSeqNum);
                            _cqMsg.TryDequeue(out to);
                            continue;
                        }
                    }
                }

                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na sincronizacao do snapshot com o canal incremental: " + ex.Message, ex);
                return false;
            }
        }

        public void EnqueueOrderInfo(TOOrderFixInfo to)
        {
            try
            {
                _cqMsg.Enqueue(to);
                lock (sync_)
                    Monitor.Pulse(sync_);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no enfileiramneto da mensagem: " + ex.Message,ex);
            }
        }

        private void _dequeueProcess()
        {
            try
            {
                while (_isRunning)
                {
                    TOOrderFixInfo item = null;
                    if (!_isSnapshotLoaded)
                    {
                        lock (sync_)
                            Monitor.Wait(sync_, 100);
                        continue;
                    }
                    
                    if (_cqMsg.TryDequeue(out item))
                    {
                        if (item != null)
                            this._processMsg(item);
                    }
                    
                    else
                    {
                        lock (sync_)
                            Monitor.Wait(sync_, 100);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da thread de carga de snapshot: " + ex.Message, ex);
            }
        }

        private void _processMsg(TOOrderFixInfo to)
        {
            try
            {
                int ordId = 0;
                bool notSend = false;
                // Validar se existe registro (nao devia cair nesta situacao)
                if (null == to.OrderFixInfo)
                {
                    logger.Error("SpiderOrd NULL!! Nao deveria cair nesta situacao!!!!!!");
                    return;
                }
                
                // Verifica se existe o account no dicionario
                ConcurrentDictionary<int, SpiderOrderInfo> item = null;
                if (!_orders.TryGetValue(to.OrderFixInfo.Account, out item))
                {
                    item = new ConcurrentDictionary<int, SpiderOrderInfo>();
                    _orders.AddOrUpdate(to.OrderFixInfo.Account, item, (key, oldValue) => item);
                }
                // Verificar se existe o orderID
                SpiderOrderInfo itemOrder = null;
                SpiderOrderInfo aux = null;
                if (!item.TryGetValue(to.OrderFixInfo.OrderID, out itemOrder))
                {
                    itemOrder = new SpiderOrderInfo();
                    item.AddOrUpdate(to.OrderFixInfo.OrderID, itemOrder, (key, oldValue) => itemOrder);
                }

                itemOrder.MsgSeqNum = to.OrderFixInfo.MsgSeqNum;
                // Efetuar atribuicao do SpiderOrder, propriedade a propriedade
                // para evitar de sobrescrever a lista de SpiderOrderDetails;
                if (string.IsNullOrEmpty(to.OrderFixInfo.HandlInst))
                    logger.Error("HandlInst em branco: " + to.OrderFixInfo.OrderID);

                this._spiderOrderUpdate(itemOrder, to.OrderFixInfo);
                // itemOrder = CloneSpiderOrderInfo.CopyOrderInfo(to.OrderFixInfo);
                //if (itemOrder.OrdStatus != to.OrderFixInfo.OrdStatus)
                //{
                //    logger.Info("Problema aqui!!!!");
                //}
                string ax = string.Empty;
                ordId = to.OrderFixInfo.OrderID;
                ConcurrentDictionary<int, SpiderOrderDetailInfo> cdAux  = null;
                if (!_orderdetails.TryGetValue(ordId, out cdAux))
                {
                    cdAux = new ConcurrentDictionary<int, SpiderOrderDetailInfo>();
                    ax = "Order id nao existe, mas OrderfixDetail Null";
                    if (null != to.OrderFixDetail)
                        cdAux.AddOrUpdate(to.OrderFixDetail.OrderDetailID, to.OrderFixDetail, (key, old) => to.OrderFixDetail);
                    _orderdetails.AddOrUpdate(ordId, cdAux, (key, oldValue) => cdAux);
                }
                else
                {
                    if (null != to.OrderFixDetail)
                    {
                        SpiderOrderDetailInfo xpto;
                        if (!cdAux.TryGetValue(to.OrderFixDetail.OrderDetailID, out xpto))
                            cdAux.AddOrUpdate(to.OrderFixDetail.OrderDetailID, to.OrderFixDetail, (key, old) => to.OrderFixDetail);
                    }
                    ax = "Order id existe, mas orderfixdetail null ou ja existe na lista";                
                }

                if (cdAux!=null && cdAux.Count == 0)
                {
                    if (to.OrderFixInfo.OrdStatus == 1 || to.OrderFixInfo.OrdStatus == 2)
                        logger.InfoFormat("LST AUX NULL ou ZERADO com STATUS PARC.EXEC ou EXEC: {0} OrderID[{1}]", ax, to.OrderFixInfo.OrderID);
                    else
                        logger.InfoFormat("LST AUX NULL ou ZERADO com STATUS PARC.EXEC ou EXEC: {0}", ax);
                }
                
                lock (itemOrder.Details)
                {
                    itemOrder.Details.Clear();
                    itemOrder.Details.AddRange(cdAux.Values);
                }
                if (null != to.OrderFixDetail) // calcular somente se for nulo e se o status for partial fill ou fill
                {
                    if (to.OrderFixDetail.OrderStatusID == (int)SpiderOrderStatus.PARCIALMENTEEXECUTADA ||
                        to.OrderFixDetail.OrderStatusID == (int)SpiderOrderStatus.EXECUTADA)
                    {
                        itemOrder.AvgPxW = Calculator.CalculateWeightedAvgPx(cdAux.Values.ToList());
                        itemOrder.AvgPx = Calculator.CalculateAvgPx(cdAux.Values.ToList());
                    }
                    if (_maxDetailCount > 0)
                    {
                        if (to.OrderFixDetail.OrderStatusID == (int)SpiderOrderStatus.ENVIADA_PARA_O_ROTEADOR ||
                            to.OrderFixDetail.OrderStatusID == (int)SpiderOrderStatus.ENVIADA_PARA_A_BOLSA ||
                            to.OrderFixDetail.OrderStatusID == (int)SpiderOrderStatus.ENVIADA_PARA_O_CANAL)
                            notSend = true;
                    }
                }
                aux = CloneSpiderOrderInfo.CopyOrderInfo(itemOrder);
                if (logger.IsDebugEnabled)
                {
                    bool abc = to.OrderFixDetail == null ? true : false;
                    logger.DebugFormat("Processando OrderID==>AUX[{0}] MsgSeqNum[{1}] Detail: [{2}] Exchange [{3}] Account[{4}] Symbol[{5}] DateTime[{6}] Status[{7}] Side [{8}] OrderQty[{9}] OrderQtyRemaining [{10}] CumQty[{11}] IsDetailNull[{12}]",  
                        aux.OrderID, aux.MsgSeqNum, aux.Details.Count, aux.Exchange, aux.Account, aux.Symbol, aux.TransactTime, aux.OrdStatus, aux.Side, aux.OrderQty, aux.OrderQtyRemaining, aux.CumQty, abc);
                }
                
                if (!notSend)
                {
                    A4SocketSrv.GetInstance().SendAcOrdem(aux);
                    A4SocketSrv.GetInstance().SendStreamerOrder(aux);
                }
                

            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem: " + ex.Message, ex);
            }
            
        }

        public void ProcessCancelRejection(OrdemInfo ord)
        {
            try
            {
                // Validar se existe registro (nao devia cair nesta situacao)
                if (null == ord)
                {
                    logger.Error("OrdemInfo NULL!! Nao deveria cair nesta situacao!!!!!!");
                    return;
                }

                // Buscar a ordem a partir do ExchangeNumberID
                List<SpiderOrderInfo> aux = new List<SpiderOrderInfo>();
                foreach (KeyValuePair<int, ConcurrentDictionary<int, SpiderOrderInfo>> item in _orders)
                {
                    aux.AddRange(item.Value.Values.ToList());
                }
                SpiderOrderInfo spiderOrd = aux.FirstOrDefault(x => x.ExchangeNumberID == ord.ExchangeNumberID);
                if (null == spiderOrd)
                {
                    // Se nao encontrado tentar pelo ClOrdID que na verdade eh o OrigClOrdID do ord
                    spiderOrd = aux.FirstOrDefault(x => x.ClOrdID == ord.OrigClOrdID);
                }
                if (null == spiderOrd)
                {
                    logger.ErrorFormat("Order nao encontrada na memoria do Ac4Socket: ExchangeNumber[{0}] ClOrdID[{1}]", 
                        ord.ExchangeNumberID, ord.OrigClOrdID);
                    return;
                }
                logger.InfoFormat("Requisicao de cancelamento da ordem rejeitada. OrderID: [{0}] ExchangeNumberID: [{1}] ", 
                    spiderOrd.OrderID, spiderOrd.ExchangeNumberID);
                
                // Criar o order detail de rejeicao de cancelamento e atualizar a memoria
                // List<SpiderOrderDetailInfo> lstDetail = null;
                ConcurrentDictionary<int, SpiderOrderDetailInfo> cdDetail = null;
                if (!_orderdetails.TryGetValue(spiderOrd.OrderID, out cdDetail))
                {
                    logger.ErrorFormat("SpiderOrderDetails nao encontrado: [{0}] ExchangeNumberID: [{1}] ", 
                        spiderOrd.OrderID, spiderOrd.ExchangeNumberID);
                    return;
                }
                // Criar o SpiderOrderDetail
                SpiderOrderDetailInfo rejDet = new SpiderOrderDetailInfo();
                rejDet.OrderID = spiderOrd.OrderID;
                rejDet.OrderQty = spiderOrd.OrderQty;
                rejDet.OrderQtyRemaining = spiderOrd.OrderQtyRemaining;
                rejDet.Price = spiderOrd.Price;
                rejDet.StopPx = spiderOrd.StopPx;
                rejDet.OrderStatusID = 8; // Rejected
                rejDet.TransactTime = DateTime.Now;
                rejDet.Description = "Requisicao de cancelamento rejeitada";
                rejDet.TradeQty = 0;
                rejDet.CumQty = spiderOrd.CumQty;

                // Efetuar a gravacao no banco de dados
                bool ret = _db.InserirOrdemDetalhe(rejDet, string.Empty);
                if (!ret)
                {
                    logger.ErrorFormat("Nao inseriu o SpiderOrderDetail no banco. Mesmo assim nao retornara erro e tentara compor a memoria. " +
                                        "OrderID: [{0}] ClOrdID: [{1}] ExchangeNumberID: [{2}]",
                                        spiderOrd.OrderID, spiderOrd.ClOrdID, spiderOrd.ExchangeNumberID);
                }
                cdDetail.AddOrUpdate(rejDet.OrderDetailID, rejDet, (key, old) => rejDet);
                

                SpiderOrderInfo obj = null;
                lock (spiderOrd)
                {
                    spiderOrd.Details = null;
                    spiderOrd.Details = new List<SpiderOrderDetailInfo>(cdDetail.Values);
                }
                obj = CloneSpiderOrderInfo.CopyOrderInfo(spiderOrd);
                
                // Sinalizar o envio para os clients
                A4SocketSrv.GetInstance().SendAcOrdem(obj);
                A4SocketSrv.GetInstance().SendStreamerOrder(obj);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem: " + ex.Message, ex);
            }

        }

        #endregion


        #region "Filter Methods"
        public List<SpiderOrderInfo> GetOrders(FilterSpiderOrder flt, int maxLength, bool getDetails, out ExecResp ret)
        {
            ret = null;
            List<SpiderOrderInfo> lstRet = new List<SpiderOrderInfo>();
            try
            {
                // TODO[FF]: efetuar a montagem dos filtros
                
                // Varrer todas as contas e ir adicionando à lista
                
                if (flt.Account.Compare == TypeCompare.UNDEFINED)
                {
                    var pred = FilterAssemble.AssembleOrderFilter(flt);

                    List<SpiderOrderInfo> abcd = new List<SpiderOrderInfo>();
                    // Efetuar a carga dos valores em uma lista auxiliar
                    foreach (KeyValuePair<int, ConcurrentDictionary<int, SpiderOrderInfo>> item in _orders)
                    {
                        abcd.AddRange(item.Value.Values.ToList());
                    }
                    var query = from orders in abcd.AsQueryable().Where(pred)
                                select orders;
                    lstRet.AddRange(query.ToList().OrderByDescending(x => x.TransactTime));
                    ret = new ExecResp(MsgErrors.OK, MsgErrors.MSG_OK);
                    abcd.Clear();
                    abcd = null;
                    return lstRet;
                }
                else
                {
                    // Buscar registros a partir de um account
                    int account = flt.Account.Value;
                    ConcurrentDictionary<int, SpiderOrderInfo> ordPerAccount = null;
                    if (_orders.TryGetValue(account, out ordPerAccount))
                    {
                        var pred = FilterAssemble.AssembleOrderFilter(flt);
                        var query = from orders in ordPerAccount.Values.AsQueryable().Where(pred)
                                    select orders;
                        lstRet.AddRange(query.ToList().OrderByDescending(x=>x.TransactTime));
                        query = null;
                        if (lstRet.Count > maxLength)
                        {
                            lstRet.Clear();
                            ret = new ExecResp(MsgErrors.ERR_COUNT_MAX_EXCEEDED, MsgErrors.MSG_ERR_COUNT_MAX_EXCEEDED);
                            return lstRet;
                        }

                        // Montagem dos Details, caso flag true


                        // OK
                        ret = new ExecResp(MsgErrors.OK, MsgErrors.MSG_OK);
                        return lstRet;
                    }
                    else
                    {
                        ret = new ExecResp(MsgErrors.ERR_ACCOUNT_NOT_FOUND, MsgErrors.MSG_ERR_ACCOUNT_NOT_FOUND);
                        return lstRet;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na montagem da listagem de ordens: " + ex.Message, ex);
                ret = new ExecResp(MsgErrors.ERROR, ex.Message);
                return null;
            }
        }

        public List<SpiderOrderDetailInfo> GetOrderDetails(int orderID, out ExecResp ret, bool restrictDetails = true)
        {
            ret = new ExecResp(MsgErrors.OK, MsgErrors.MSG_OK);
            List<SpiderOrderDetailInfo> lstRet = new List<SpiderOrderDetailInfo>();
            try
            {
                ConcurrentDictionary<int, SpiderOrderDetailInfo> cdAux = null;
                if (_orderdetails.TryGetValue(orderID, out cdAux))
                {
                    // Se _maxDetailCount!=0, filtra os details
                    if (restrictDetails)
                    {
                        if (_maxDetailCount != 0)
                        {
                            List<SpiderOrderDetailInfo> aux = new List<SpiderOrderDetailInfo>(cdAux.Values.Where(x => x.OrderStatusID != 100 && x.OrderStatusID != 101 && x.OrderStatusID != 102).OrderBy(x => x.TransactTime));
                            int count = aux.Count;
                            lstRet.AddRange(aux.Skip(count - _maxDetailCount));
                        }
                        else
                            lstRet.AddRange(cdAux.Values);
                    }
                    else
                        lstRet.AddRange(cdAux.Values);
                    return lstRet;
                }
                else
                {
                    ret = new ExecResp(MsgErrors.ERR_ACCOUNT_NOT_FOUND, MsgErrors.MSG_ERR_ACCOUNT_NOT_FOUND);
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na montagem da listagem de ordens: " + ex.Message, ex);
                return null;
            }
        }

        

        #endregion


        #region Util Functions
        private void _spiderOrderUpdate(SpiderOrderInfo origOrd, SpiderOrderInfo newOrd)
        {
            origOrd.Account = newOrd.Account;
            origOrd.ChannelID = newOrd.ChannelID;
            origOrd.ClOrdID = newOrd.ClOrdID;
            origOrd.CumQty = newOrd.CumQty;
            origOrd.Description = newOrd.Description;
            origOrd.Exchange = newOrd.Exchange;
            origOrd.ExchangeNumberID = newOrd.ExchangeNumberID;
            origOrd.ExecBroker = newOrd.ExecBroker;
            origOrd.ExpireDate = newOrd.ExpireDate;
            origOrd.FixMsgSeqNum = newOrd.FixMsgSeqNum;
            origOrd.HandlInst = newOrd.HandlInst;
            origOrd.IdFix = newOrd.IdFix;
            origOrd.IntegrationName = newOrd.IntegrationName;
            origOrd.OrderQtyApar = newOrd.OrderQtyApar;
            origOrd.Memo = newOrd.Memo;
            origOrd.OrderQtyMin = newOrd.OrderQtyMin;
            origOrd.Msg42Base64 = newOrd.Msg42Base64;
            origOrd.MsgFix = newOrd.MsgFix;
            origOrd.OrderID = newOrd.OrderID;
            origOrd.OrderQty = newOrd.OrderQty;
            origOrd.OrderQtyRemaining = newOrd.OrderQtyRemaining;
            origOrd.OrdStatus = newOrd.OrdStatus;
            origOrd.OrdTypeID = newOrd.OrdTypeID;
            origOrd.OrigClOrdID = newOrd.OrigClOrdID;
            origOrd.Price = newOrd.Price;
            origOrd.RegisterTime = newOrd.RegisterTime;
            origOrd.SecurityExchangeID = newOrd.SecurityExchangeID;
            origOrd.SessionID = newOrd.SessionID;
            origOrd.SessionIDOriginal = newOrd.SessionIDOriginal;
            origOrd.Side = newOrd.Side;
            origOrd.StopPx = newOrd.StopPx;
            origOrd.StopStartID = newOrd.StopStartID;
            origOrd.Symbol = newOrd.Symbol;
            origOrd.SystemID = newOrd.SystemID;
            origOrd.TimeInForce = newOrd.TimeInForce;
            origOrd.TransactTime = newOrd.TransactTime;
            origOrd.AccountDv = newOrd.AccountDv;
            origOrd.Exchange = newOrd.Exchange;
            origOrd.IntegrationName = newOrd.IntegrationName;
            // Buscar o IntegrationName e Bolsa a partir do IdSessaoFix

            if (string.IsNullOrEmpty(origOrd.Exchange) || string.IsNullOrEmpty(origOrd.IntegrationName))
            {
                SpiderIntegration item = null;
                if (_dicIntegration.TryGetValue(origOrd.IdFix, out item))
                {
                    origOrd.IntegrationName = item.IntegrationName;
                    origOrd.Exchange = item.Exchange;
                }
            }
        }

        private bool _loadIntegrationName()
        {
            try
            {
                if (null == _db)
                {
                    _db = new DbAc4Socket();
                }
                _dicIntegration = _db.LoadIntegrationInfo();
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

        #endregion

        #region Snapshot Methods
        public List<SpiderOrderInfo> GetMemorySnapshot()
        {
            try
            {
                List<SpiderOrderInfo> ret = new List<SpiderOrderInfo>();
                foreach ( KeyValuePair<int, ConcurrentDictionary<int, SpiderOrderInfo>> item in _orders)
                {
                    foreach (KeyValuePair<int, SpiderOrderInfo> item2 in item.Value)
                    {
                        // Buscar details do _orderdetails
                        SpiderOrderInfo aux = item2.Value;
                        ConcurrentDictionary<int, SpiderOrderDetailInfo> cdAux;
                        _orderdetails.TryGetValue(item2.Key, out cdAux);
                        // _orderdetails.TryGetValue(item2.Key, out aux.Details);
                        lock (aux.Details)
                            aux.Details.AddRange(cdAux.Values);
                        ret.Add(aux);
                    }
                }
                return ret.OrderBy(x=>x.MsgSeqNum).ToList();
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na montagem do snapshot: " + ex.Message, ex);
                return null;
            }
        }
        #endregion
    }
}
