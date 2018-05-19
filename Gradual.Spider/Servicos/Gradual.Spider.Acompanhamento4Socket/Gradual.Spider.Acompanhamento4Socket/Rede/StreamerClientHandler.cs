using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Collections.Concurrent;
using Gradual.Spider.Acompanhamento4Socket.Lib.Mensagem;
using System.Threading;
using Gradual.Spider.Acompanhamento4Socket.Lib.Dados;
using Gradual.Spider.Acompanhamento4Socket.Cache;
using Gradual.Spider.Acompanhamento4Socket.Lib.Util;
using System.Linq.Expressions;
using Gradual.Core.Spider.OrderFixProcessing.Lib.Dados;
using System.Net.Sockets;
using Gradual.Spider.CommSocket;

namespace Gradual.Spider.Acompanhamento4Socket.Rede
{
    public class StreamerClientHandler
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region private variables
        Queue<StreamerOrderInfo> _qOrd;
        Thread _thQueue;
        Thread _thSnapshot;
        bool _isRunning;
        object _sync = new object();
        bool _isSnapshotLoaded;
        FilterStreamerRequest _filter;
        Expression<Func<SpiderOrderInfo, bool>> _pred;
        ConnInfo _connection;

        bool _cancelTransfer;
        #endregion



        public StreamerClientHandler()
        {
            _qOrd = new Queue<StreamerOrderInfo>();
            _isSnapshotLoaded = false;
            _cancelTransfer = false;
            _filter = null;
            _connection = null;
        }

        ~StreamerClientHandler()
        {
            if (null != _qOrd)
            {
                lock (_qOrd)

                    _qOrd.Clear();
                _qOrd = null;
            }
        }

        public void Start()
        {
            try
            {
                logger.Info("Iniciando StreamerClientHandler...");
                _isRunning = true;

                logger.Info("Iniciando Thread de Processamento das mensagens...");
                _thQueue = new Thread(new ThreadStart(this._dequeueOrder));
                _thQueue.Start();

            }
            catch (Exception ex)
            {
                logger.Error("Problemas no start do StreamerClientHandler: " + ex.Message, ex);
                _isRunning = false;
            }
        }

        public void Stop()
        {
            try
            {
                logger.Info("Parando StreamerClientHandler... ");
                _isRunning = false;

                logger.Info("Parando Thread ProcessQueue...");
                if (null != _thQueue)
                {
                    if (_thQueue.IsAlive) _thQueue.Join(500);
                    if (_thQueue.IsAlive) _thQueue.Abort();
                    _thQueue = null;
                }
                _connection = null;
                _isSnapshotLoaded = false;
                _filter = null;

            }
            catch (Exception ex)
            {
                logger.Error("Problemas no stop do StreamerClientHandler: " + ex.Message, ex);
            }
        }


        public void EnqueueStreamerOrder(StreamerOrderInfo item)
        {
            try
            {
                lock (_qOrd)
                {
                    item.Id = _filter.Id;
                    _qOrd.Enqueue(item);
                }
                //lock(_sync)
                //    Monitor.Pulse(_sync);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na insercao do objeto na fila: " + ex.Message, ex);
            }

        }

        private void _dequeueOrder()
        {
            try
            {
                while (_isRunning)
                {
                    if (_isSnapshotLoaded)
                    {
                        StreamerOrderInfo item = null;
                        lock (_qOrd)
                        {
                            if (_qOrd.Count >0)
                                item = _qOrd.Dequeue();
                            else
                                Monitor.Wait(_qOrd, 50);
                        }
                        if (item != null)
                            this._processMsg(item);
                        item = null;
                    }
                    else
                    {
                        // Verificar um limite maximo para processamento da fila
                        lock (_qOrd)
                        {
                            if (_qOrd.Count > 50000)
                            {
                                logger.Error("ATINGIU O LIMITE MAXIMO PARA PROCESSAMENTO DA FILA. PARANDO O StreamerClientHandler...");
                                this.Stop();
                            }
                        }
                        Thread.Sleep(100);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro no processamento do StreamerOrderInfo: " + ex.Message, ex);
            }
        }

        private void _processMsg(StreamerOrderInfo info)
        {
            try
            {
                if (!_cancelTransfer)
                    this._sendToClient(info.MsgType, info.Order, info.Id);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no envio do StreamerOrderInfo para o cliente: " + ex.Message, ex);
            }
        }

        #region Connection Controls
        public void SetConnection (int id, Socket clientSocket, bool restrictDetails = true)
        {
            if (null == _connection)
                _connection = new ConnInfo();
            _connection.IdClient = id;
            _connection.ClientSocket = clientSocket;
            _connection.RestrictDetails = restrictDetails;
        }

        public ConnInfo GetConnection()
        {
            return _connection;
        }
        private void _sendToClient(int msgType, SpiderOrderInfo ord, string id)
        {
            try
            {
                if (_connection != null && _connection.ClientSocket != null && _connection.ClientSocket.Connected)
                {
                    StreamerOrderInfo xx = new StreamerOrderInfo();
                    xx.MsgType = msgType;
                    xx.Order = ord;
                    xx.Id = id;
                    SpiderSocket.SendObject(xx, _connection.ClientSocket);
                }
                else
                {
                    logger.Error("Socket não conectado ou conexao indisponivel");
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no envio do objeto SpiderOrderInfo para o cliente: " + ex.Message, ex);
            }
        }
        #endregion



        #region Snapshot Controls
        public void CancelTransfer(bool valor)
        {
            _cancelTransfer = valor;
            logger.Info("Limpando fila de envio...");
            lock (_qOrd)
                _qOrd.Clear();
        }

        public void SetFilter(FilterStreamerRequest flt)
        {
            _filter = flt;
        }

        public bool VerifyFilter(SpiderOrderInfo order)
        {
            try
            {
                List<SpiderOrderInfo> aux = new List<SpiderOrderInfo>();
                aux.Add(order);
                if (_pred != null)
                {
                    var query = from orders in aux.AsQueryable().Where(_pred)
                            select orders;
                    if (query.ToList().Count() == 0)
                    {
                        return false;
                    }
                }
                else
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Problema na comparacao do filtro: " + ex.Message, ex);
                return false;
            }
        }

        public bool ComposeSnapShot(FilterStreamerResponse respStr)
        {
            try
            {
                
                if (_filter == null)
                {
                    logger.Error("Filtro setado esta nulo");
                    return false;
                }
                _isSnapshotLoaded = false;
                
                lock (_qOrd)
                    _qOrd.Clear();
                // Montar o filter info request padrao
                FilterInfoRequest req = new FilterInfoRequest();
                FilterInfoResponse resp = new FilterInfoResponse();
                ExecResp ret;
                req.Filter = new FilterSpiderOrder();
                req.Filter.Account = _filter.Account;
                req.Filter.SessionIDOriginal = _filter.SessionID;
                req.Filter.Symbol = _filter.Symbol;
                req.RecordLimit = 10000000;

                // Guardar o PredicateBuilder
                _pred = FilterAssemble.AssembleOrderFilter(req.Filter);
                logger.Info("Inicio geracao do snapshot streamer...");
                resp.Orders = OrderCache4Socket.GetInstance().GetOrders(req.Filter, req.RecordLimit, req.ReturnDetails, out ret);
                logger.Info("Fim geracao do snapshot streamer...");
                if (ret.Code != MsgErrors.OK)
                {
                    respStr.ErrCode = ret.Code;
                    respStr.ErrMsg = ret.Msg;
                    logger.Error("Impossivel gerar o snapshot: " + ret.Msg);
                    logger.Error("Enviando um objeto vazio para indicar erro na geracao...");
                    SpiderOrderInfo orderErro = new SpiderOrderInfo();
                    this._sendToClient(MsgTypeConst.SNAPSHOT, orderErro, _filter.Id);

                    // Habilitar a flag de snapshot para ficar recebendo os incrementais de qualquer jeito
                    _isSnapshotLoaded = true;

                    return false;
                }
                // Verificar se a thread está rodando com outra requisicao
                if (_thSnapshot != null && _thSnapshot.IsAlive)
                {
                    try
                    {
                        _thSnapshot.Join(50);
                        if (_thSnapshot.IsAlive)
                        {
                            _thSnapshot.Abort();
                            _thSnapshot = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Debug("Thread snapshot aborted: " + ex.Message);
                    }
                }
                _thSnapshot = new Thread(new ParameterizedThreadStart(sendSnapshot));
                _thSnapshot.Start(resp.Orders);

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na composicao do snapshot do streamer: " + ex.Message, ex);
                logger.Error("Gerando o objeto vazio para indicar erro");
                respStr.ErrCode = MsgErrors.ERROR;
                respStr.ErrMsg = ex.Message;
                SpiderOrderInfo orderErro = new SpiderOrderInfo();
                this._sendToClient(MsgTypeConst.SNAPSHOT, orderErro, _filter.Id);
                return false;
            }
        }

        public void sendSnapshot(object aa)
        {
            try
            {
                List<SpiderOrderInfo> lst = (List<SpiderOrderInfo>) aa;
                int len = lst.Count;
                logger.InfoFormat("Enviando registros de snapshot: ID[{0}] Length[{1}]", _filter.Id, len);
                for (int i = 0; i < len; i++)
                {
                    // Verificacao de stop de envio
                    if (_cancelTransfer)
                    {
                        logger.InfoFormat("Cancelou envio de snapshot: [{0}] de [{1}]", i, len);
                        break;
                    }
                    // Montagem dos details para envio
                    ExecResp retDetail = null;
                    SpiderOrderInfo aux = lst[i];

                    aux.Details = OrderCache4Socket.GetInstance().GetOrderDetails(aux.OrderID, out retDetail, _connection.RestrictDetails);
                    if (retDetail.Code != MsgErrors.OK)
                        logger.Error("Problemas na composicao das informacoes do SpiderOrderDetail: " + retDetail.Msg);
                    this._sendToClient(MsgTypeConst.SNAPSHOT, aux, _filter.Id);
                }
                _isSnapshotLoaded = true;
                lst.Clear();
                lst = null;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no envio do snapshot: " + ex.Message, ex);
            }
        }

        #endregion
    }
}
