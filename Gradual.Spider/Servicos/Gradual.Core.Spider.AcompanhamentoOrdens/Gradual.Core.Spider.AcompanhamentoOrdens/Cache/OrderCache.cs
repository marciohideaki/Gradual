using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.Spider.AcompanhamentoOrdens.Db;

using Gradual.Core.Spider.AcompanhamentoOrdens.Lib;
using Gradual.Core.Spider.AcompanhamentoOrdens.Lib.Dados;
using System.Threading;
using Gradual.Core.Spider.AcompanhamentoOrdens.Lib.Mensageria;
using Gradual.Core.Spider.AcompanhamentoOrdens.Util;
using System.Linq.Expressions;
using Gradual.Core.OMS.DropCopy.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.Core.Spider.AcompanhamentoOrdens.Callback;
using Gradual.Core.OMS.DropCopy.Lib.Mensagens;
using System.Collections.Concurrent;
using System.Configuration;


namespace Gradual.Core.Spider.AcompanhamentoOrdens.Cache
{
    public class OrderCache : IAcompanhamentoOrdensAdm
    {

        #region log4net declaration
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region private variables
        private static OrderCache _me = null;
        private static object _objLock = new object();

        DbAcompanhamento _db;
        Dictionary<int, Dictionary <int, SpiderOrderInfo>> _dicAcOrdens;
        Dictionary<int, IntegrationInfo> _dicIntegration;

        Thread _thLoad;
        Thread _thSignSpider;
        Thread _thProcessOrders;
        bool _isRunning; 
        bool _isConnected;
        bool _startProcess;
        SpiderDropCopyCallback _spiderDropCopy;
        // ISpiderSignDropCopyCallback _itfSpiderDropCopy;

        ConcurrentQueue<TOSpiderOrder> _cqOrder;
        object _ordLock = new object();

        Dictionary<string, InstanciaInfo> _dicInstancia;
        #endregion


        public static OrderCache GetInstance()
        {
            lock (_objLock)
            {
                if (_me == null)
                {
                    _me = new OrderCache();
                }
            }
            return _me;
        }

        public OrderCache()
        {   
            _db = new DbAcompanhamento();
            _isRunning = false;
            _isConnected = false;
            _startProcess = false;
            _dicInstancia = new Dictionary<string, InstanciaInfo>();

            
        }

        ~OrderCache()
        {
            _db = null;
        }

        public void Start()
        {
            try
            {
                
                logger.Info("Iniciando OrderCache.... ");
                logger.Info("Carregando IntegrationInfo...");
                
                // Carregar endereco das instancias
                for (int i = 1; i <= 10; i++)
                {
                    string chave = string.Format("InstanciaDropCopy{0}", i);
                    if (ConfigurationManager.AppSettings.AllKeys.Contains(chave))
                    {
                        if (!_dicInstancia.ContainsKey(chave))
                        {
                            InstanciaInfo item = new InstanciaInfo();
                            item.Addr = ConfigurationManager.AppSettings[chave].ToString();
                            _dicInstancia.Add(chave, item);
                        }
                    }
                    else
                        break;
                }

                _isRunning = true;
                _dicAcOrdens = new Dictionary<int, Dictionary<int, SpiderOrderInfo>>();
                if (false == this._loadIntegrationName())
                {
                    logger.Error("Problemas na carga das informacoes da integracao...");
                    throw new Exception("Problemas na carga das informacoes da integracao...");
                }
                
                logger.Info("Iniciando thread de carga das ordens...");
                _thLoad = new Thread(new ThreadStart(this._loadOrders));
                _thLoad.Start();

                logger.Info("Iniciando thread de monintoramento de assinatura SpiderDropCopy...");
                _thSignSpider = new Thread(new ThreadStart(this._assinarSpiderDropCopyCallbackMonitor));
                _thSignSpider.Start();

                _cqOrder = new ConcurrentQueue<TOSpiderOrder>();
                logger.Info("Iniciando thread de processamento de order / order detail");
                _thProcessOrders = new Thread(new ThreadStart(this._processOrders));
                _thProcessOrders.Start();
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no start do OrderCache: " + ex.Message, ex);
                throw ex;
            }
        }

        public void Stop()
        {
            try
            {
                logger.Info("Parando OrderCache...");
                _isRunning = false;
                if (null != _thLoad)
                {
                    if (_thLoad.IsAlive)
                    {
                        _thLoad.Join(1000);
                        _thLoad.Abort();
                    }
                }

                logger.Info("Parando thread de Monitoramneto de assinaturas Spider...");
                if (null != _thSignSpider)
                {
                    if (_thSignSpider.IsAlive)
                    {
                        _thSignSpider.Join(1000);
                        _thSignSpider.Abort();
                    }
                    _thSignSpider = null;
                }
                logger.Info("Parando thread de processmaento das mensagens de ordens...");
                if (null != _thProcessOrders)
                {
                    if (_thProcessOrders.IsAlive)
                    {
                        _thProcessOrders.Join(1000);
                        _thProcessOrders.Abort();
                    }
                    _thProcessOrders = null;
                }
                
                // Desativar a conexao
                //if (null != _itfSpiderDropCopy)
                //{
                //    try
                //    {
                //        Ativador.AbortChannel(_itfSpiderDropCopy);
                //    }
                //    catch
                //    {
                //        logger.Error("Erro ao desconectar o ativador...");
                //    }
                //}
                // Limpar o dicionario
                this._clearAcOrder();

                _startProcess = false;
                _isConnected = false;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no stop do order cache: " + ex.Message, ex);
            }
        }


        #region DatabaseLoad
        private void _loadOrders()
        {
            try
            {
                int i = 1;
                
                // TODO[FF] - Efetuar o controle
                while (_isRunning)
                {
                    try
                    {
                        if (i >= 40)
                        {
                            i = 1;
                            if (true == _isConnected && false == _startProcess)
                            {
                                //DateTime date = new DateTime(2014, 09, 01); // TODO [FF]: Mudar para datetime now
                                DateTime date = DateTime.Now;
                                Dictionary<int, Dictionary<int, SpiderOrderInfo>> dic = new Dictionary<int, Dictionary<int, SpiderOrderInfo>>();

                                dic = _db.LoadOrderManager(date);
                                if (null != dic)
                                {
                                    if (_dicAcOrdens != null && _dicAcOrdens.Count != 0)
                                    {
                                        this._clearAcOrder();
                                    }
                                    
                                    foreach (KeyValuePair<int, Dictionary<int, SpiderOrderInfo>> item in dic)
                                    {
                                        logger.InfoFormat("Carregando Acompanhamento de Ordens... Account: [{0}] Count: [{1}]", item.Key, item.Value.Count);
                                        if (null == _dicAcOrdens)
                                            _dicAcOrdens = new Dictionary<int, Dictionary<int, SpiderOrderInfo>>();

                                        lock(_dicAcOrdens)
                                            _dicAcOrdens.Add(item.Key, item.Value);

                                        
                                    }
                                    logger.Info("=========> Carga Efetuada... Registros _dicAcOrdens: " + _dicAcOrdens.Count);
                                    _startProcess = true;
                                    logger.Info("=========> Iniciando processamento da fila de ordens...");

                                }
                                else
                                    logger.Info("Registros não foram encontrados...");
                            }
                        }
                        else
                            i++;

                    }
                    catch (Exception ex)
                    {
                        logger.Error("Problemas ao efetuar a carga do Acompanhamento de Ordens...: "  +ex.Message, ex);
                    }
                    Thread.Sleep(250);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no carregamento da lista de ordens: " + ex.Message, ex);
                throw ex;
            }
        }

        private bool _loadIntegrationName()
        {
            try
            {
                if (null == _db)
                {
                    _db = new DbAcompanhamento();
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

        #region Thread Functions
        private void _assinarSpiderDropCopyCallbackMonitor()
        {
            int i = 1;
            logger.Info("Assinando Spider DropCopyCallbackMonitor...");

            try
            {
                _spiderDropCopy = new SpiderDropCopyCallback(this);

                foreach (KeyValuePair<string, InstanciaInfo> item in _dicInstancia)
                {
                    item.Value.InterfaceDC = Ativador.GetByAddr<ISpiderSignDropCopyCallback>(item.Value.Addr, false, _spiderDropCopy);
                    item.Value.InterfaceDC.AssinarAcompanhamentoOrdensSpider(new AssinarSpiderDropCopyRequest());
                }
            }
            catch 
            {
                logger.Error("Nao foi possivel assinar o callback");
                _spiderDropCopy = null;
                
                _isConnected = false;
                _startProcess = false;
            }

            while (_isRunning)
            {
                if (i >= 120)
                {
                    if (null == _spiderDropCopy)
                        _spiderDropCopy = new SpiderDropCopyCallback(this);
                    foreach (KeyValuePair<string, InstanciaInfo> item in _dicInstancia)
                    {
                        try
                        {
                            if (null == item.Value.InterfaceDC)
                            {
                                item.Value.InterfaceDC = Ativador.GetByAddr<ISpiderSignDropCopyCallback>(item.Value.Addr, false, _spiderDropCopy);
                                item.Value.InterfaceDC.AssinarAcompanhamentoOrdensSpider(new AssinarSpiderDropCopyRequest());
                            }
                            item.Value.InterfaceDC.Pong(new PongDropCopyRequest());
                            logger.Info("PingResponse: " + DateTime.Now);
                            _isConnected = true;
                            i = 1;
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Erro no ping do ativador. Teoricamente conexao perdida com o callback dropcopy: " + item.Key);
                            item.Value.InterfaceDC = null;
                            _isConnected = false;
                            _startProcess = false;
                            i = 1;
                        }
                    }
                }
                else
                    i++;
                Thread.Sleep(250);
            }
        }

        private void _processOrders()
        {
            try
            {
                while (_isRunning)
                {
                    if (_startProcess)
                    {
                        TOSpiderOrder toSpider = null;
                        if (!_cqOrder.TryDequeue(out toSpider))
                        {
                            lock(_ordLock)
                                Monitor.Wait(_ordLock, 100);
                        }
                        if (null != toSpider)
                            this._processToSpider(toSpider);
                    }
                    else
                    {
                        lock(_ordLock)
                            Monitor.Wait(_ordLock, 500);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem de ordem: " + ex.Message, ex);
            }
        }

        public void AddSpiderOrder(TOSpiderOrder to)
        {
            try
            {
                _cqOrder.Enqueue(to);
                lock (_ordLock)
                    Monitor.Pulse(_ordLock);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas ao adicionar o elemento na fila de ordens... " + ex.Message, ex);
            }
        }

        private void _processToSpider(TOSpiderOrder to)
        {
            try
            {
                // Validar se existe registro (nao devia cair nesta situacao)
                if (null == to.Order)
                {
                    logger.Error("SpiderOrder NULL!!");
                    return;
                }
                lock(_dicAcOrdens)
                {
                    // Verifica se existe o account no dicionario
                    Dictionary<int, SpiderOrderInfo> item = null;
                    if (!_dicAcOrdens.TryGetValue(to.Order.Account, out item))
                    {
                        item = new Dictionary<int, SpiderOrderInfo>();
                        _dicAcOrdens.Add(to.Order.Account, item);
                    }
                    // Verificar se existe o orderID
                    SpiderOrderInfo itemOrder = null;
                    if (!item.TryGetValue(to.Order.OrderID, out itemOrder))
                    {
                        itemOrder = new SpiderOrderInfo();
                        item.Add(to.Order.OrderID, itemOrder);
                    }
                    // Efetuar atribuicao do SpiderOrder, propriedade a propriedade
                    // para evitar de sobrescrever a lista de SpiderOrderDetails;
                    if (string.IsNullOrEmpty(to.Order.HandlInst))
                        logger.Error("HandlInst do caralho veio em branco: " + to.Order.OrderID);

                    this._spiderOrderUpdate(itemOrder, to.Order);

                    // Validar se o OrderDetailID existe na lista de Details
                    if (null != to.OrderDetail)
                    {
                        SpiderOrderDetailInfo aux = itemOrder.Details.FirstOrDefault(x=>x.OrderDetailID == to.OrderDetail.OrderDetailID);
                        if (null == aux)
                            itemOrder.Details.Add(to.OrderDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro no processamento da mensagem TO: " + ex.Message, ex);

            }
        }


        private void _clearAcOrder()
        {
            try
            {
                // Limpar o dicionario de AcOrdens
                if (null != _dicAcOrdens)
                {
                    lock (_dicAcOrdens)
                    {
                        int len = _dicAcOrdens.Count;
                        foreach (KeyValuePair<int, Dictionary<int, SpiderOrderInfo>> x in _dicAcOrdens)
                        {
                            x.Value.Clear();
                        }
                        _dicAcOrdens.Clear();
                    }
                    _dicAcOrdens = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao limpar o dicionario de acompanhamento de ordens: " + ex.Message, ex);
            }

        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Interface functions
        /// </summary>
        #region "IAcompanhamentoOrdensAdm Members"
        public void DummyFunction()
        {
            logger.Info("DummyFunction called...");
        }

        public OrderManagerMsgResponse GetOrders(OrderManagerMsgRequest req)
        {
            OrderManagerMsgResponse ret = new OrderManagerMsgResponse();
            try
            {
                if (req.FilterOptions.Conta.Count != 0)
                {
                    var pred = _applyFilter(req.FilterOptions);
                    Dictionary<int, SpiderOrderInfo> dicItem = null;
                    int len = req.FilterOptions.Conta.Count;
                    for (int i = 0; i < req.FilterOptions.Conta.Count; i++)
                    {
                        if (_me._dicAcOrdens.TryGetValue(req.FilterOptions.Conta[i], out dicItem))
                        {
                            // IQueryable<SpiderOrderInfo> aux = lstItem.All;
                            var query = from orders in dicItem.Values.AsQueryable().Where(pred)
                                        select orders;
                            ret.ListOrders.AddRange(query.ToList());
                        }
                    }
                }
                // Nao veio filtro de account, percorrer todos e adicionar a lista
                else
                {
                    var pred = _applyFilter(req.FilterOptions);
                    foreach (KeyValuePair<int, Dictionary<int, SpiderOrderInfo>> item in _me._dicAcOrdens)
                    {
                        var query = from orders in item.Value.Values.AsQueryable().Where(pred)
                                    select orders;
                        ret.ListOrders.AddRange(query.ToList());
                    }
                }
                ret.ListOrders = ret.ListOrders.OrderByDescending(x => x.OrderID).ToList();
                ret.StatusResponse = Response.OK;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na consulta das ofertas... Msg: " + ex.Message, ex);
                ret.StackTrace = ex.StackTrace;
                ret.StatusResponse = Response.ERRO;
                ret.DescricaoErro = ex.Message;
            }
            return ret;
        }

        private static Expression<Func<SpiderOrderInfo, bool>> _applyFilter(OrderFilterInfo filter)
        {
            
            try
            {
                var predicate = PredicateBuilder.True<SpiderOrderInfo>();

                // Ativo
                List<string> lstAtivo = filter.Ativo.Distinct().ToList();
                int len = lstAtivo.Count;
                var predAux = PredicateBuilder.False<SpiderOrderInfo>();
                for (int i = 0; i < len; i++)
                {
                    string ativo = lstAtivo[i];
                    predAux = predAux.Or(x => x.Symbol.Equals(ativo, StringComparison.CurrentCultureIgnoreCase));
                }
                if (len >0)
                    predicate = predicate.And(predAux);
                
                // DataInicial
                if (filter.DataInicial!= DateTime.MinValue)
                    predicate = predicate.And(x => x.RegisterTime.Date >= filter.DataInicial.Date);
                
                // DataFinal
                if (filter.DataFinal != DateTime.MinValue)
                    predicate = predicate.And(x => x.RegisterTime.Date <= filter.DataFinal.Date);

                // Sessao
                predAux = PredicateBuilder.False<SpiderOrderInfo>();
                List<string> lstSessao = filter.Sessao.Distinct().ToList();
                len = lstSessao.Count;
                for (int i = 0; i < len; i++)
                {
                    string sessao = lstSessao[i];
                    predAux = predAux.Or(x => x.SessionIDOrigin.Equals(sessao, StringComparison.CurrentCultureIgnoreCase));
                }
                if (len > 0)
                    predicate = predicate.And(predAux);

                // Sentido
                predAux = PredicateBuilder.False<SpiderOrderInfo>();
                List<string> lstSentido = filter.Sentido.Distinct().ToList();
                len = lstSentido.Count;
                for (int i = 0; i < len; i++)
                {
                    string sentido = lstSentido[i];
                    predAux = predAux.Or(x => x.Side == Convert.ToInt32(sentido));
                }
                if (len > 0)
                    predicate = predicate.And(predAux);

                // Bolsa
                predAux = PredicateBuilder.False<SpiderOrderInfo>();
                List<string> lstBolsa = filter.Bolsa.Distinct().ToList();
                len = lstBolsa.Count;
                for (int i = 0; i < len; i++)
                {
                    string bolsa = lstBolsa[i];
                    predAux = predAux.Or(x => x.Exchange.Equals(bolsa, StringComparison.CurrentCultureIgnoreCase));
                }
                if (len > 0)
                    predicate = predicate.And(predAux);

                // OrderStatusID
                predAux = PredicateBuilder.False<SpiderOrderInfo>();
                List<int> lstOrderStatus = filter.OrderStatusID.Distinct().ToList();
                len = lstOrderStatus.Count;
                for (int i = 0; i < len; i++)
                {
                    int ordStatus = lstOrderStatus[i];
                    predAux = predAux.Or(x => x.OrdStatusID == ordStatus);
                }
                if (len > 0)
                    predicate = predicate.And(predAux);

                // HandlInst
                predAux = PredicateBuilder.False<SpiderOrderInfo>();
                List<string> lstHandlInst = filter.HandlInst.Distinct().ToList();
                len = lstHandlInst.Count;
                for (int i = 0; i < len; i++)
                {
                    string handlInst = lstHandlInst[i];
                    predAux = predAux.Or(x => x.HandlInst.Equals(handlInst, StringComparison.CurrentCultureIgnoreCase));
                }
                if (len > 0)
                    predicate = predicate.And(predAux);
                return predicate;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas em efetuar aplicacao do filtro: " + ex.Message, ex);
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
            origOrd.IdSession = newOrd.IdSession; 
            origOrd.IntegrationName = newOrd.IntegrationName;
            origOrd.MaxFloor = newOrd.MaxFloor;
            origOrd.Memo = newOrd.Memo;
            origOrd.MinQty = newOrd.MinQty;
            origOrd.Msg42Base64 = newOrd.Msg42Base64;
            origOrd.MsgFix = newOrd.MsgFix;
            origOrd.OrderID = newOrd.OrderID;
            origOrd.OrderQty = newOrd.OrderQty;
            origOrd.OrderQtyRemaining = newOrd.OrderQtyRemaining;
            origOrd.OrdStatusID = newOrd.OrdStatusID; 
            origOrd.OrdTypeID = newOrd.OrdTypeID;
            origOrd.OrigClOrdID = newOrd.OrigClOrdID;
            origOrd.Price = newOrd.Price;
            origOrd.RegisterTime = newOrd.RegisterTime;
            origOrd.SecurityExchangeID = newOrd.SecurityExchangeID;
            origOrd.SessionID = newOrd.SessionID; 
            origOrd.SessionIDOrigin = newOrd.SessionIDOrigin;
            origOrd.Side = newOrd.Side;
            origOrd.StopPx = newOrd.StopPx;
            origOrd.StopStartID = newOrd.StopStartID;
            origOrd.Symbol = newOrd.Symbol;
            origOrd.SystemID = newOrd.SystemID;
            origOrd.TimeInForce = newOrd.TimeInForce;
            origOrd.TransactTime = newOrd.TransactTime;

            // Buscar o IntegrationName e Bolsa a partir do IdSessaoFix
            IntegrationInfo item = null;
            if (_dicIntegration.TryGetValue(origOrd.IdSession, out item))
            {
                origOrd.IntegrationName = item.IntegrationName;
                origOrd.Exchange = item.Bolsa;
            }

        }
        #endregion
    }
}
