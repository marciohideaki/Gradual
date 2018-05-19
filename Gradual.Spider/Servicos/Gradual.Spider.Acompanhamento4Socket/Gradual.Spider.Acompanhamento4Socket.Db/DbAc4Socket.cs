using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Data.SqlClient;
using System.Configuration;
using Gradual.Spider.Acompanhamento4Socket.Lib.Dados;
using System.Data;
using System.Collections.Concurrent;
using Gradual.Core.Spider.OrderFixProcessing.Lib.Dados;
using Gradual.Spider.Acompanhamento4Socket.Lib.Util;

namespace Gradual.Spider.Acompanhamento4Socket.Db
{
    public class DbAc4Socket
    {

        #region log4net declaration
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region private variables
        protected SqlConnection _sqlConn;
        protected SqlCommand _sqlCommand;
        string _strConnectionStringDefault;
        #endregion

        #region Properties
        public bool ConexaoIniciada
        {
            get
            {
                return !(_sqlConn == null);
            }
        }
        public bool ConexaoAberta
        {
            get
            {
                return (_sqlConn != null && _sqlConn.State == System.Data.ConnectionState.Open);
            }
        }
        #endregion 



        public DbAc4Socket()
        {
            _sqlConn = null;
            _sqlCommand = null;
            _strConnectionStringDefault = ConfigurationManager.ConnectionStrings["GradualSpider"].ConnectionString;
        }

        ~DbAc4Socket()
        {
            if (null != _sqlCommand)
            {
                _sqlCommand.Dispose();
                _sqlCommand = null;
            }
            if (null != _sqlConn)
            {
                FecharConexao();
            }
        }

        protected void AbrirConexao(string strConnectionString)
        {
            if (!this.ConexaoAberta)
            {
                _sqlConn = new SqlConnection(strConnectionString);
                _sqlConn.Open();
                
            }
        }

        protected void FecharConexao()
        {
            try
            {
                _sqlConn.Close();
                _sqlConn.Dispose();
            }
            catch
            {
                logger.Error("Problemas no fechamento da conexao");
            }
        }


        // public ConcurrentDictionary<int, ConcurrentDictionary<int, SpiderOrderInfo>> LoadOrderSnapshot(DateTime dtReg)
        public bool LoadOrderSnapshot(DateTime dtReg, out ConcurrentDictionary<int, ConcurrentDictionary<int, SpiderOrderInfo>> orders, 
                                      out ConcurrentDictionary<int, ConcurrentDictionary<int, SpiderOrderDetailInfo>> orderdetails)  
        {
            try
            {
                // Carregar as informacoes de Integracao para compor os registros
                Dictionary<int, SpiderIntegration> dic = new Dictionary<int, SpiderIntegration>();
                dic = this.LoadIntegrationInfo();

                Dictionary<int, int> dicAux = new Dictionary<int, int>();

                orders = new ConcurrentDictionary<int, ConcurrentDictionary<int, SpiderOrderInfo>>();
                orderdetails = new ConcurrentDictionary<int, ConcurrentDictionary<int,SpiderOrderDetailInfo>>();
                SqlDataAdapter lAdapter;
                AbrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();

                _sqlCommand = new SqlCommand("prc_spider_ac_ordens4socket", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@DataRegistro", dtReg));

                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.SelectCommand.CommandTimeout = 0;
                lAdapter.Fill(lDataSet);
                int currentAccount = -1;
                int currentOrderID = 0;
                ConcurrentDictionary<int, SpiderOrderInfo> currentItem = null;
                SpiderOrderInfo currentOrder = null;

                if (lDataSet.Tables.Count < 2)
                    throw new Exception("Numero invalido de tables na query!!");
                int len = lDataSet.Tables[0].Rows == null ? 0 : lDataSet.Tables[0].Rows.Count;

                for (int i = 0; i < len; i++)
                {
                    DataRow lRow = lDataSet.Tables[0].Rows[i];
                    int acc = lRow["Account"].DBToInt32();
                    int ordid = lRow["OrderID"].DBToInt32();

                    if (currentAccount != acc)
                    {
                        ConcurrentDictionary<int, SpiderOrderInfo> item = new ConcurrentDictionary<int, SpiderOrderInfo>();
                        orders.AddOrUpdate(acc, item, (key, oldValue) => item);
                        currentItem = item;
                    }
                    if (currentOrderID != ordid)
                    {
                        currentOrder = new SpiderOrderInfo();
                        currentOrder.OrderID = lRow["OrderID"].DBToInt32();
                        currentOrder.OrigClOrdID = lRow["OrigClOrdID"].DBToString();
                        currentOrder.ExchangeNumberID = lRow["ExchangeNumberID"].DBToString();
                        currentOrder.ClOrdID = lRow["ClOrdID"].DBToString();
                        currentOrder.Account = lRow["Account"].DBToInt32();
                        currentOrder.Symbol = lRow["Symbol"].DBToString();
                        currentOrder.SecurityExchangeID = lRow["SecurityExchangeID"].DBToString();
                        currentOrder.StopStartID = lRow["StopStartID"].DBToInt32();
                        currentOrder.OrdTypeID = lRow["OrdTypeID"].DBToString();
                        currentOrder.OrdStatus = lRow["OrdStatusID"].DBToInt32();
                        currentOrder.RegisterTime = lRow["RegisterTime"].DBToDateTime();
                        currentOrder.TransactTime = lRow["TransactTime"].DBToDateTime();
                        currentOrder.ExpireDate = lRow["ExpireDate"].DBToDateTime();
                        currentOrder.TimeInForce = lRow["TimeInForce"].DBToString();
                        currentOrder.ChannelID = lRow["ChannelID"].DBToInt32();
                        currentOrder.ExecBroker = lRow["ExecBroker"].DBToString();
                        currentOrder.Side = lRow["Side"].DBToInt32();
                        currentOrder.OrderQty = lRow["OrderQty"].DBToInt32();
                        currentOrder.OrderQtyRemaining = lRow["OrderQtyRemaining"].DBToInt32();
                        currentOrder.OrderQtyMin = lRow["MinQty"].DBToDecimal();
                        currentOrder.OrderQtyApar = lRow["MaxFloor"].DBToDecimal();
                        currentOrder.Price = lRow["Price"].DBToDecimal();
                        currentOrder.StopPx = lRow["StopPx"].DBToDecimal();
                        currentOrder.CumQty = lRow["CumQty"].DBToInt32();
                        currentOrder.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32();
                        currentOrder.SystemID = lRow["SystemID"].DBToString();
                        currentOrder.Memo = lRow["Memo"].DBToString();
                        currentOrder.SessionID = lRow["SessionID"].DBToString();
                        currentOrder.SessionIDOriginal = lRow["SessionIDOrigin"].DBToString();
                        currentOrder.IdFix = lRow["IdSession"].DBToInt32();
                        currentOrder.MsgFix = lRow["MsgFix"].DBToString();
                        currentOrder.Msg42Base64 = lRow["Msg42Base64"].DBToString();
                        currentOrder.HandlInst = lRow["HandlInst"].DBToString();
                        currentOrder.AccountDv = lRow["AccountDv"].DBToInt32();
                        currentOrder.Exchange = lRow["Exchange"].DBToString();
                        currentOrder.MsgSeqNum = lRow["MsgSeqNum"].DBToInt32();

                        if (string.IsNullOrEmpty(currentOrder.Exchange))
                        {
                            SpiderIntegration integ = null;
                            if (dic.TryGetValue(currentOrder.IdFix, out integ))
                            {
                                currentOrder.IntegrationName = integ.IntegrationName;
                                currentOrder.Exchange = integ.Exchange;
                            }
                        }
                        currentItem.AddOrUpdate(ordid, currentOrder, (key, oldValue) => currentOrder);
                        dicAux.Add(ordid, acc);
                    }
                    currentAccount = acc;
                    currentOrderID = ordid;
                }

                // Processar os details e adicionar no order id correto
                int currentOrderID_Det = -1;

                ConcurrentDictionary<int, ConcurrentDictionary<int, SpiderOrderDetailInfo>> currentDetail = new ConcurrentDictionary<int, ConcurrentDictionary<int, SpiderOrderDetailInfo>>();
                ConcurrentDictionary<int, SpiderOrderDetailInfo> cdDetail = new ConcurrentDictionary<int, SpiderOrderDetailInfo>();
                //SpiderOrderInfo currentSpiderOrder = null;
                bool firstReg = true;
                len = lDataSet.Tables[1].Rows == null ? 0 : lDataSet.Tables[1].Rows.Count;
                for (int i = 0; i < len; i++)
                {
                    DataRow lRow2 = lDataSet.Tables[1].Rows[i];
                    int ordid_det = lRow2["OrderID"].DBToInt32();
                    // Se diferente, mudou de registro e deve buscar o novo account, dicionario de ordens e afins
                    if (currentOrderID_Det != ordid_det && currentOrderID_Det!=-1)
                    {
                        if (ordid_det == 0)
                        {
                            logger.Error("Order ID ZERADO!!!");
                            continue;
                        }
                        int accountAux =-1;
                        if (!dicAux.TryGetValue(currentOrderID_Det, out accountAux))
                        {
                            logger.Error("Account NOT FOUND!!! OrderID: " + currentOrderID_Det);
                            continue;
                        }

                        // Adicionar o elemento anterior e zerar a lista
                        orderdetails.AddOrUpdate(currentOrderID_Det, cdDetail, (key, oldValue) => cdDetail);

                        // Calcular o preco medio 
                        decimal avgPxW = Calculator.CalculateWeightedAvgPx(cdDetail.Values.ToList());
                        decimal avgPx = Calculator.CalculateAvgPx(cdDetail.Values.ToList());
                        // Buscar o order ID para atualizar o order info "header"
                        ConcurrentDictionary<int, SpiderOrderInfo> xx = null;
                        if (orders.TryGetValue(accountAux, out xx))
                        {
                            SpiderOrderInfo xx2 = null;
                            if (xx.TryGetValue(currentOrderID_Det, out xx2))
                            {
                                xx2.AvgPxW = avgPxW;
                                xx2.AvgPx = avgPx;
                            }
                        }

                        cdDetail = null;
                        cdDetail = new ConcurrentDictionary<int, SpiderOrderDetailInfo>();
                    }

                    // Processar o detalhe da ordem

                    SpiderOrderDetailInfo detailOrder = new SpiderOrderDetailInfo();
                    detailOrder.OrderDetailID = lRow2["OrderDetailID"].DBToInt32();
                    detailOrder.TransactID = lRow2["TransactID"].DBToString();
                    detailOrder.OrderID = lRow2["OrderID"].DBToInt32();
                    detailOrder.OrderQty = lRow2["OrderQty"].DBToInt32();
                    detailOrder.OrderQtyRemaining = lRow2["OrdQtyRemaining"].DBToInt32();
                    detailOrder.Price = lRow2["Price"].DBToDecimal();
                    detailOrder.StopPx = lRow2["StopPx"].DBToDecimal();
                    detailOrder.OrderStatusID = lRow2["OrderStatusID"].DBToInt32();
                    detailOrder.TransactTime = lRow2["TransactTime"].DBToDateTime();
                    detailOrder.Description = lRow2["Description"].DBToString();
                    detailOrder.TradeQty = lRow2["TradeQty"].DBToInt32();
                    detailOrder.CumQty = lRow2["CumQty"].DBToInt32();
                    detailOrder.FixMsgSeqNum = lRow2["FixMsgSeqNum"].DBToInt32();
                    detailOrder.CxlRejResponseTo = lRow2["CxlRejResponseTo"].DBToString();
                    detailOrder.CxlRejReason = lRow2["CxlRejReason"].DBToInt32();
                    detailOrder.MsgFixDetail = lRow2["MsgFixDetail"].DBToString();
                    
                    // Adicionar o detail no currentOrder
                    cdDetail.AddOrUpdate(detailOrder.OrderDetailID, detailOrder, (key, oldlvalue)=>detailOrder);
                    currentOrderID_Det = ordid_det;
                }

                // Fazer o ultimo registro do detail
                int abc = -1;
                if (!dicAux.TryGetValue(currentOrderID_Det, out abc))
                {
                    logger.Error("Account NOT FOUND!!! OrderID: " + currentOrderID_Det);
                }

                // Adicionar o elemento anterior e zerar a lista
                orderdetails.AddOrUpdate(currentOrderID_Det, cdDetail, (key, oldValue) => cdDetail);

                // Calcular o preco medio 
                decimal avgPxW2 = Calculator.CalculateWeightedAvgPx(cdDetail.Values.ToList());
                decimal avgPx2 = Calculator.CalculateAvgPx(cdDetail.Values.ToList());
                // Buscar o order ID para atualizar o order info "header"
                ConcurrentDictionary<int, SpiderOrderInfo> xxx = null;
                if (orders.TryGetValue(abc, out xxx))
                {
                    SpiderOrderInfo xx3 = null;
                    if (xxx.TryGetValue(currentOrderID_Det, out xx3))
                    {
                        xx3.AvgPxW = avgPxW2;
                        xx3.AvgPx = avgPx2;
                    }
                }

                
                
                lDataSet.Clear();
                lDataSet.Dispose();
                lAdapter.Dispose();
                FecharConexao();
                return true;
            }
            catch (Exception ex)
            {
                FecharConexao();
                logger.Error("LoadOrderManager(): " + ex.Message, ex);
                orders = null;
                orderdetails = null;
                return false;
            }
        }

        public Dictionary<int, SpiderIntegration> LoadIntegrationInfo()
        {
            try
            {
                Dictionary<int, SpiderIntegration> ret = new Dictionary<int, SpiderIntegration>();
                SqlDataAdapter lAdapter;
                AbrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();

                _sqlCommand = new SqlCommand("prc_spider_fix_integration_name_lst", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);

                foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                {
                    int idSessaoFix = lRow["IdSessaoFix"].DBToInt32();
                    SpiderIntegration item = new SpiderIntegration();
                    item.IntegrationId = lRow["id_integration"].DBToInt32();
                    item.IntegrationName = lRow["IntegrationName"].DBToString();
                    item.Exchange = lRow["Bolsa"].DBToString();
                    ret.Add(idSessaoFix, item);
                }
                FecharConexao();
                return ret;
            }
            catch (Exception ex)
            {
                FecharConexao();
                logger.Error("LoadIntegrationInfo() " + ex.Message, ex);
                return null;
            }
        }

        public bool InserirOrdemDetalhe(SpiderOrderDetailInfo orderDt, string clOrdID)
        {
            try
            {
                AbrirConexao(_strConnectionStringDefault);
                _sqlCommand = new SqlCommand("prc_spider_inserir_ordem_detalhe", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter orderDetailID = new SqlParameter("@OrderDetailID", SqlDbType.Int);
                orderDetailID.Direction = ParameterDirection.Output;
                _sqlCommand.Parameters.Add(orderDetailID);
                _sqlCommand.Parameters.Add(new SqlParameter("@TransactID", orderDt.TransactID));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrderID", orderDt.OrderID));
                _sqlCommand.Parameters.Add(new SqlParameter("@ClOrdID", clOrdID));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrderQty", orderDt.OrderQty));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrdQtyRemaining", orderDt.OrderQtyRemaining));
                _sqlCommand.Parameters.Add(new SqlParameter("@CumQty", orderDt.CumQty));
                _sqlCommand.Parameters.Add(new SqlParameter("@TradeQty", orderDt.TradeQty));
                _sqlCommand.Parameters.Add(new SqlParameter("@Price", orderDt.Price));
                _sqlCommand.Parameters.Add(new SqlParameter("@StopPx", orderDt.StopPx));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrderStatusID", orderDt.OrderStatusID));
                _sqlCommand.Parameters.Add(new SqlParameter("@Description", orderDt.Description));
                if (orderDt.TransactTime == DateTime.MinValue)
                    _sqlCommand.Parameters.Add(new SqlParameter("@EventTime", DateTime.Now));
                else
                    _sqlCommand.Parameters.Add(new SqlParameter("@EventTime", orderDt.TransactTime));
                _sqlCommand.Parameters.Add(new SqlParameter("@CxlRejResponseTo", orderDt.CxlRejResponseTo));
                _sqlCommand.Parameters.Add(new SqlParameter("@CxlRejReason", orderDt.CxlRejReason));
                _sqlCommand.Parameters.Add(new SqlParameter("@FixMsgSeqNum", orderDt.FixMsgSeqNum));

                if (!string.IsNullOrEmpty(orderDt.MsgFixDetail))
                    _sqlCommand.Parameters.Add(new SqlParameter("@MsgFixDetail", orderDt.MsgFixDetail));

                if (!string.IsNullOrEmpty(orderDt.ContraBroker))
                    _sqlCommand.Parameters.Add(new SqlParameter("@ContraBroker", orderDt.ContraBroker));
                int rows = _sqlCommand.ExecuteNonQuery();
                orderDt.OrderDetailID = orderDetailID.Value.DBToInt32();
                FecharConexao();
                return true;
            }
            catch (Exception ex)
            {
                // _dbArq.InserirOrdemDetalhe(orderDt, clOrdID);
                FecharConexao();
                logger.Error("InserirOrdemDetalhe(): " + ex.Message, ex);
                return false;
            }
        }
    }
}
