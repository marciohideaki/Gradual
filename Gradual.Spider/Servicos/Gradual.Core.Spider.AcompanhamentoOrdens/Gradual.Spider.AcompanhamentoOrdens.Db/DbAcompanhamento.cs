using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Data.SqlClient;
using System.Configuration;
using Gradual.Core.Spider.AcompanhamentoOrdens.Lib.Dados;
using System.Data;

namespace Gradual.Spider.AcompanhamentoOrdens.Db
{
    public class DbAcompanhamento
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

        public DbAcompanhamento()
        {
            _sqlConn = null;
            _sqlCommand = null;
            _strConnectionStringDefault = ConfigurationManager.ConnectionStrings["GradualSpider"].ConnectionString;
        }
        ~DbAcompanhamento()
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



        public Dictionary<int, Dictionary<int, SpiderOrderInfo>> LoadOrderManager(DateTime dtReg)
        {
            try
            {
                Dictionary<int, Dictionary<int, SpiderOrderInfo>> ret = new Dictionary<int, Dictionary<int, SpiderOrderInfo>>();
                SqlDataAdapter lAdapter;
                AbrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();

                _sqlCommand = new SqlCommand("prc_spider_ac_ordens_lst", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@DataRegistro", dtReg));
                
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                int currentAccount = 0;
                int currentOrderID = 0;
                Dictionary<int, SpiderOrderInfo> currentItem = null;
                SpiderOrderInfo currentOrder = null;
                foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                {
                    int acc = lRow["Account"].DBToInt32();
                    int ordid = lRow["OrderID"].DBToInt32();
                    if (currentAccount != acc)
                    {
                        Dictionary<int, SpiderOrderInfo> item = new Dictionary<int, SpiderOrderInfo>();
                        ret.Add(acc, item);
                        currentItem = item;
                    
                    }
                    // Processar o header da ordem e o primeiro detail e inserir na lista
                    if (currentOrderID != ordid)
                    {
                        currentOrder = new SpiderOrderInfo();
                        currentOrder.OrderID 			   = lRow["OrderID"].DBToInt32();
                        currentOrder.OrigClOrdID            = lRow["OrigClOrdID"].DBToString();
                        currentOrder.ExchangeNumberID       = lRow["ExchangeNumberID"].DBToString();
                        currentOrder.ClOrdID                = lRow["ClOrdID"].DBToString();
                        currentOrder.Account                = lRow["Account"].DBToInt32();
                        currentOrder.Symbol                 = lRow["Symbol"].DBToString();
                        currentOrder.SecurityExchangeID     = lRow["SecurityExchangeID"].DBToString();
                        currentOrder.StopStartID            = lRow["StopStartID"].DBToInt32();
                        currentOrder.OrdTypeID              = lRow["OrdTypeID"].DBToString();
                        currentOrder.OrdStatusID            = lRow["OrdStatusID"].DBToInt32();
                        currentOrder.RegisterTime           = lRow["RegisterTime"].DBToDateTime();
                        currentOrder.TransactTime           = lRow["TransactTime"].DBToDateTime();
                        currentOrder.ExpireDate             = lRow["ExpireDate"].DBToDateTime();
                        currentOrder.TimeInForce            = lRow["TimeInForce"].DBToString();
                        currentOrder.ChannelID              = lRow["ChannelID"].DBToInt32();
                        currentOrder.ExecBroker             = lRow["ExecBroker"].DBToString();
                        currentOrder.Side                   = lRow["Side"].DBToInt32();
                        currentOrder.OrderQty               = lRow["OrderQty"].DBToInt32();
                        currentOrder.OrderQtyRemaining      = lRow["OrderQtyRemaining"].DBToInt32();
                        currentOrder.MinQty                 = lRow["MinQty"].DBToDecimal();
                        currentOrder.MaxFloor               = lRow["MaxFloor"].DBToDecimal();
                        currentOrder.Price                  = lRow["Price"].DBToDecimal();
                        currentOrder.StopPx                 = lRow["StopPx"].DBToDecimal();
                        currentOrder.Description            = lRow["Description"].DBToString();
                        currentOrder.CumQty                 = lRow["CumQty"].DBToInt32();
                        currentOrder.FixMsgSeqNum           = lRow["FixMsgSeqNum"].DBToInt32();
                        currentOrder.SystemID               = lRow["SystemID"].DBToString();
                        currentOrder.Memo                   = lRow["Memo"].DBToString();
                        currentOrder.SessionID              = lRow["SessionID"].DBToString();
                        currentOrder.SessionIDOrigin        = lRow["SessionIDOrigin"].DBToString();
                        currentOrder.IdSession              = lRow["IdSession"].DBToInt32();
                        currentOrder.MsgFix                 = lRow["MsgFix"].DBToString();
                        currentOrder.Msg42Base64            = lRow["Msg42Base64"].DBToString();
                        currentOrder.HandlInst              = lRow["HandlInst"].DBToString();
                        currentOrder.IntegrationName        = lRow["IntegrationName"].DBToString();
                        currentOrder.Exchange               = lRow["Bolsa"].DBToString();
                        currentItem.Add(ordid, currentOrder);
                    }

                    // Processar o detalhe da ordem
                    SpiderOrderDetailInfo detailOrder = new SpiderOrderDetailInfo();
                    detailOrder.OrderDetailID = lRow["OrderDetailID"].DBToInt32();
                    detailOrder.TransactID = lRow["TransactID"].DBToString();
                    detailOrder.OrderID = lRow["OrderID"].DBToInt32();
                    detailOrder.OrderQty = lRow["OrderQty"].DBToInt32();
                    detailOrder.OrderQtyRemaining = lRow["OrderQtyRemaining"].DBToInt32();
                    detailOrder.Price = lRow["Price"].DBToDecimal();
                    detailOrder.StopPx = lRow["StopPx"].DBToDecimal();
                    detailOrder.OrderStatusID = lRow["OrderStatusID"].DBToInt32();
                    detailOrder.TransactTime = lRow["TransactTime"].DBToDateTime();
                    detailOrder.Description = lRow["Description"].DBToString();
                    detailOrder.TradeQty = lRow["TradeQty"].DBToInt32();
                    detailOrder.CumQty = lRow["CumQty"].DBToInt32();
                    detailOrder.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32();
                    detailOrder.CxlRejResponseTo = lRow["CxlRejResponseTo"].DBToString();
                    detailOrder.CxlRejReason = lRow["CxlRejReason"].DBToInt32();
                    detailOrder.MsgFixDetail = lRow["MsgFixDetail"].DBToString();
                    
                    // Adicionar o detail no currentOrder
                    currentOrder.Details.Add(detailOrder);

                    currentAccount = acc;
                    currentOrderID = ordid;
                    
                }
                FecharConexao();
                return ret;
            }
            catch (Exception ex)
            {
                FecharConexao();
                logger.Error("LoadOrderManager(): " + ex.Message, ex);
                return null;
            }
        }

        public Dictionary<int, IntegrationInfo> LoadIntegrationInfo()
        {
            try
            {
                Dictionary<int, IntegrationInfo> ret = new Dictionary<int, IntegrationInfo>();
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
                    IntegrationInfo item = new IntegrationInfo();
                    item.IntegrationId = lRow["id_integration"].DBToInt32();
                    item.IntegrationName = lRow["IntegrationName"].DBToString();
                    item.Bolsa = lRow["Bolsa"].DBToString();
                    ret.Add(idSessaoFix, item);
                }
                FecharConexao();
                return ret;
            }
            catch (Exception ex)
            {
                FecharConexao();
                logger.Error("LoadIntegrationInfo() "  + ex.Message, ex);
                return null;
            }
        }
    }
}
