using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using log4net;
using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;
using Cortex.OMS.ServidorFIX;
using Gradual.Core.OMS.DropCopy.Lib.Dados;

//using Gradual.Core.OMS.FixServerLowLatency.Dados;

namespace Gradual.Core.OMS.DropCopyProcessor.Database
{
    public class DbDropCopy
    {

        #region log4net declaration
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables
        private SqlConnection _sqlConn;
        private SqlCommand _sqlCommand;
        string _strConnectionStringDefault;
        DbArquivo _dbArq;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public DbDropCopy()
        {
            _sqlConn = null;
            _sqlCommand = null;
            _strConnectionStringDefault = ConfigurationManager.ConnectionStrings["GradualSpider"].ConnectionString;
            _dbArq = new DbArquivo();
        }

        
        private void AbrirConexao(string strConnectionString)
        {
            if (!this.ConexaoAberta)
            {
                _sqlConn = new SqlConnection(strConnectionString);
                _sqlConn.Open();
            }
        }

        private void FecharConexao()
        {
            try
            {
                _sqlConn.Close();
                _sqlConn.Dispose();
            }
            catch { }
        }
        
        ~DbDropCopy()
        {
            if (null != _sqlCommand)
            {
                _sqlCommand.Dispose();
                _sqlCommand = null;
            }
            if (null!=_sqlConn)
            {
                FecharConexao();
            }
            _dbArq = null;
        }

        /// <summary>
        /// BuscarSessoesFIX - recupera toda lista de sessoes configuradas
        /// </summary>
        /// <param name="ordemalterada">OrdemInfo da ordem alterada</param>
        /// <returns>Lista de registors</returns>
        public List<FixSessionItem> BuscarSessoesFIXServer(string strExecFile, string active = "Y")
        {
            List<FixSessionItem> ret = new List<FixSessionItem>();

            SqlDataAdapter lAdapter;
            AbrirConexao(_strConnectionStringDefault);
            DataSet lDataSet = new DataSet();
            _sqlCommand = new SqlCommand("prc_fixsrv_sessaofix_server", _sqlConn);
            _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

            _sqlCommand.Parameters.Add(new SqlParameter("@ExecArq", strExecFile));
            _sqlCommand.Parameters.Add(new SqlParameter("@ActiveFlag", active[0]));
            lAdapter = new SqlDataAdapter(_sqlCommand);
            lAdapter.Fill(lDataSet);
            foreach (DataRow lRow in lDataSet.Tables[0].Rows)
            {
                FixSessionItem session = new FixSessionItem();

                session.IdSessaoFix = lRow["IdSessaoFix"].DBToInt32();
                session.IdCliente = lRow["id_cliente"].DBToInt32();
                session.Mnemonico = lRow["Mnemonico"].DBToString();
                session.Bolsa = lRow["Bolsa"].DBToString();
                session.Operador = lRow["Operador"].DBToInt32();
                session.BeginString = lRow["BeginString"].DBToString();
                session.SenderCompID = lRow["SenderCompID"].DBToString();
                session.SenderLocationID = lRow["SenderLocationID"].DBToString();
                session.TargetCompID = lRow["TargetCompID"].DBToString();
                //session.PartyID = lRow["PartyID"].DBToInt32().ToString();
                //session.PartyIDSource = lRow["PartyIDSource"].DBToString();
                //session.PartyRole = lRow["PartyRole"].DBToInt32();
                session.SecurityIDSource = lRow["SecurityIDSource"].DBToInt32().ToString();
                session.LogonPassword = lRow["LogonPassword"].DBToString();
                session.HeartBtInt = lRow["HeartBeatInt"].DBToInt32();
                string aux = lRow["ResetSeqNum"].DBToString();
                if (aux.ToLower().Equals("0") || aux.ToLower().Equals("n") || string.IsNullOrEmpty(aux))
                    session.ResetSeqNum = false;
                else
                    session.ResetSeqNum = true;
                aux = lRow["PersistMessages"].DBToString();
                if (aux.Equals("0") || aux.ToLower().Equals("n") || string.IsNullOrEmpty(aux))
                    session.PersistMessages = false;
                else
                    session.PersistMessages = true;

                session.SocketPort = lRow["SocketPort"].DBToInt32();
                session.ReconnectInterval = lRow["ReconnectInterval"].DBToInt32();
                string cancelOnDisconn = lRow["CancelOnDisconnect"].DBToString();
                try
                {
                    session.CancelOnDisconnect = Convert.ToInt32(cancelOnDisconn);
                }
                catch
                {
                    session.CancelOnDisconnect = -1;
                }
                //session.CancelOnDisconnect = lRow["CancelOnDisconnect"].DBToString();
                session.CODTimeout = lRow["CODTimeout"].DBToInt32();
                session.Host = lRow["Host"].DBToString();
                session.FileStorePath = lRow["FileStorePath"].DBToString();
                session.FileLogPath = lRow["FileLogPath"].DBToString();
                session.DebugFileLogPath = lRow["DebugFileLogPath"].DBToString();
                session.StartTime = lRow["StartTime"].DBToString();
                session.EndTime = lRow["EndTime"].DBToString();
                aux = lRow["UseDataDictionary"].DBToString();
                if (aux.Equals("0") || aux.ToLower().Equals("n") || string.IsNullOrEmpty(aux))
                    session.UseDataDictionary = false;
                else
                    session.UseDataDictionary = true;

                session.DataDictionary = lRow["DataDictionary"].DBToString();
                session.ConnectionType = lRow["ConnectionType"].DBToString();
                aux = lRow["FinancialLimit"].DBToString();
                if (aux.Equals("0") || aux.ToLower().Equals("n") || string.IsNullOrEmpty(aux))
                    session.FinancialLimit = false;
                else
                    session.FinancialLimit = true;
                // PID = PartyID
                // INI -- Composicao do PartyIDs da Sessao
                SqlCommand commPID = new SqlCommand("prc_fixsrv_sessaofix_server_party_id", _sqlConn);
                commPID.CommandType = System.Data.CommandType.StoredProcedure;
                commPID.Parameters.Add(new SqlParameter("@IdSessaoFix", session.IdSessaoFix));

                SqlDataAdapter lAdapterPID = new SqlDataAdapter(commPID);
                DataSet lDataSetPID = new DataSet();
                lAdapterPID.Fill(lDataSetPID);
                foreach (DataRow lRowPID in lDataSetPID.Tables[0].Rows)
                {
                    PartyIDItem pid = new PartyIDItem();
                    pid.IdPartyID = lRowPID["IdPartyID"].DBToInt32();
                    pid.IdSessaoFIx = lRowPID["IdSessaoFix"].DBToInt32();
                    pid.PartyID = lRowPID["PartyID"].DBToString();
                    pid.PartyIDSource = lRowPID["PartyIDSource"].DBToString()[0];
                    pid.PartyRole = lRowPID["PartyRole"].DBToInt32();
                    session.PartyIDs.Add(pid);
                }
                // FIM -- Composicao do PartyIDs da Sessao
                ret.Add(session);
            }
            FecharConexao();
            return ret;
        }

        public bool AtualizarServerName(int idSessaoFix, string serverName)
        {
            try
            {
                AbrirConexao(_strConnectionStringDefault);
                _sqlCommand = new SqlCommand("prc_fixsrv_atualizar_nome_servidor", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@IdSessaoFix", idSessaoFix));

                if (string.IsNullOrEmpty(serverName))
                    _sqlCommand.Parameters.Add(new SqlParameter("@ServerName", DBNull.Value));
                else
                    _sqlCommand.Parameters.Add(new SqlParameter("@ServerName", serverName));
                int rows = _sqlCommand.ExecuteNonQuery();
                FecharConexao();
                return true;
            }
            catch (Exception ex)
            {
                FecharConexao();
                logger.Error("AtualizarServerName(): " + ex.Message, ex);
                return false;
            }
        }

        public OrderDbInfo BuscarOrdem(string clordid, int account, string symbol, bool origcl = false)
        {
            try
            {
                OrderDbInfo ret = new OrderDbInfo();
                SqlDataAdapter lAdapter;
                AbrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                if (!origcl)
                    _sqlCommand = new SqlCommand("prc_fix_buscar_ordem", _sqlConn);
                else
                    _sqlCommand = new SqlCommand("prc_fix_buscar_ordem_origcl", _sqlConn);

                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@ClOrdId", clordid));
                _sqlCommand.Parameters.Add(new SqlParameter("@Account", account));
                _sqlCommand.Parameters.Add(new SqlParameter("@Symbol", symbol));
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                {
                    ret.OrderID = lRow["OrderID"].DBToInt32();
                    ret.OrigClOrdID = lRow["OrigClOrdID"].DBToString();
                    ret.ExchangeNumberID = lRow["ExchangeNumberID"].DBToString();
                    ret.ClOrdID = lRow["ClOrdID"].DBToString();
                    ret.Account = lRow["Account"].DBToInt32();
                    ret.Symbol = lRow["Symbol"].DBToString();
                    ret.SecurityExchangeID = lRow["SecurityExchangeID"].DBToString();
                    ret.StopStartID = lRow["StopStartID"].DBToInt32();
                    ret.OrdTypeID = lRow["OrdTypeID"].DBToString();
                    ret.OrdStatus = lRow["OrdStatusID"].DBToInt32();
                    ret.RegisterTime = lRow["RegisterTime"].DBToDateTime();
                    ret.TransactTime = lRow["TransactTime"].DBToDateTime();
                    
                    ret.ExpireDate =  lRow["ExpireDate"].DBToDateTime();
                    ret.TimeInForce = lRow["TimeInForce"].DBToString();
                    ret.ChannelID = lRow["ChannelID"].DBToInt32();
                    ret.ExecBroker = lRow["ExecBroker"].DBToString();
                    ret.Side = lRow["Side"].DBToInt32();
                    ret.OrderQty = lRow["OrderQty"].DBToInt32();
                    ret.OrderQtyRemaining = lRow["OrderQtyRemaining"].DBToInt32();
                    ret.OrderQtyMin = lRow["MinQty"].DBToDecimal();
                    ret.OrderQtyApar = lRow["MaxFloor"].DBToDecimal();
                    ret.Price = lRow["Price"].DBToDecimal();
                    ret.CumQty = lRow["CumQty"].DBToInt32();
                    ret.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32();
                    ret.SystemID = lRow["SystemID"].DBToString();
                    ret.Memo = lRow["Memo"].DBToString();
                    ret.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32();
                    ret.SessionID = lRow["SessionID"].DBToString();
                    ret.SessionIDOriginal = lRow["SessionIDOrigin"].DBToString();
                    ret.IdFix = lRow["IDSession"].DBToInt32();
                    ret.MsgFix = lRow["MsgFix"].ToString();
                }
                FecharConexao();
                return ret;
            }
            catch (Exception ex)
            {
                FecharConexao();
                logger.Error("BuscarOrdem(): " + ex.Message, ex);
                return null;
            }
        }

        public OrderDbInfo BuscarOrdemPorClOrdEOrigClOrd(string clordid, string origclord, string symbol)
        {
            try
            {
                OrderDbInfo ret = new OrderDbInfo();
                SqlDataAdapter lAdapter;
                AbrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_fix_buscar_ordem_clordid_origcld", _sqlConn);

                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@ClOrdId", clordid));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrigClOrdId", origclord));
                _sqlCommand.Parameters.Add(new SqlParameter("@Symbol", symbol));
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                {
                    ret.OrderID = lRow["OrderID"].DBToInt32();
                    ret.OrigClOrdID = lRow["OrigClOrdID"].DBToString();
                    ret.ExchangeNumberID = lRow["ExchangeNumberID"].DBToString();
                    ret.ClOrdID = lRow["ClOrdID"].DBToString();
                    ret.Account = lRow["Account"].DBToInt32();
                    ret.Symbol = lRow["Symbol"].DBToString();
                    ret.SecurityExchangeID = lRow["SecurityExchangeID"].DBToString();
                    ret.StopStartID = lRow["StopStartID"].DBToInt32();
                    ret.OrdTypeID = lRow["OrdTypeID"].DBToString();
                    ret.OrdStatus = lRow["OrdStatusID"].DBToInt32();
                    ret.RegisterTime = lRow["RegisterTime"].DBToDateTime();
                    ret.TransactTime = lRow["TransactTime"].DBToDateTime();

                    ret.ExpireDate = lRow["ExpireDate"].DBToDateTime();
                    ret.TimeInForce = lRow["TimeInForce"].DBToString();
                    ret.ChannelID = lRow["ChannelID"].DBToInt32();
                    ret.ExecBroker = lRow["ExecBroker"].DBToString();
                    ret.Side = lRow["Side"].DBToInt32();
                    ret.OrderQty = lRow["OrderQty"].DBToInt32();
                    ret.OrderQtyRemaining = lRow["OrderQtyRemaining"].DBToInt32();
                    ret.OrderQtyMin = lRow["MinQty"].DBToDecimal();
                    ret.OrderQtyApar = lRow["MaxFloor"].DBToDecimal();
                    ret.Price = lRow["Price"].DBToDecimal();
                    ret.CumQty = lRow["CumQty"].DBToInt32();
                    ret.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32();
                    ret.SystemID = lRow["SystemID"].DBToString();
                    ret.Memo = lRow["Memo"].DBToString();
                    ret.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32();
                    ret.SessionID = lRow["SessionID"].DBToString();
                    ret.SessionIDOriginal = lRow["SessionIDOrigin"].DBToString();
                    ret.IdFix = lRow["IDSession"].DBToInt32();
                }
                FecharConexao();
                return ret;
            }
            catch (Exception ex)
            {
                FecharConexao();
                logger.Error("BuscarOrdem(): " + ex.Message, ex);
                return null;
            }
        }


        public OrderDbInfo BuscarOrdemPorExchangeNumber(string exchangenumberid)
        {
            try
            {
                OrderDbInfo ret = new OrderDbInfo();
                SqlDataAdapter lAdapter;
                AbrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_fix_buscar_ordem_exchange_number", _sqlConn);

                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@ExchangeNumber", exchangenumberid));
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                {
                    ret.OrderID = lRow["OrderID"].DBToInt32();
                    ret.OrigClOrdID = lRow["OrigClOrdID"].DBToString();
                    ret.ExchangeNumberID = lRow["ExchangeNumberID"].DBToString();
                    ret.ClOrdID = lRow["ClOrdID"].DBToString();
                    ret.Account = lRow["Account"].DBToInt32();
                    ret.Symbol = lRow["Symbol"].DBToString();
                    ret.SecurityExchangeID = lRow["SecurityExchangeID"].DBToString();
                    ret.StopStartID = lRow["StopStartID"].DBToInt32();
                    ret.OrdTypeID = lRow["OrdTypeID"].DBToString();
                    ret.OrdStatus = lRow["OrdStatusID"].DBToInt32();
                    ret.RegisterTime = lRow["RegisterTime"].DBToDateTime();
                    ret.TransactTime = lRow["TransactTime"].DBToDateTime();

                    ret.ExpireDate = lRow["ExpireDate"].DBToDateTime();
                    ret.TimeInForce = lRow["TimeInForce"].DBToString();
                    ret.ChannelID = lRow["ChannelID"].DBToInt32();
                    ret.ExecBroker = lRow["ExecBroker"].DBToString();
                    ret.Side = lRow["Side"].DBToInt32();
                    ret.OrderQty = lRow["OrderQty"].DBToInt32();
                    ret.OrderQtyRemaining = lRow["OrderQtyRemaining"].DBToInt32();
                    ret.OrderQtyMin = lRow["MinQty"].DBToDecimal();
                    ret.OrderQtyApar = lRow["MaxFloor"].DBToDecimal();
                    ret.Price = lRow["Price"].DBToDecimal();
                    ret.CumQty = lRow["CumQty"].DBToInt32();
                    ret.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32();
                    ret.SystemID = lRow["SystemID"].DBToString();
                    ret.Memo = lRow["Memo"].DBToString();
                    ret.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32();
                    ret.SessionID = lRow["SessionID"].DBToString();
                    ret.SessionIDOriginal = lRow["SessionIDOrigin"].DBToString();
                    ret.IdFix = lRow["IDSession"].DBToInt32();
                }
                FecharConexao();
                return ret;
            }
            catch (Exception ex)
            {
                FecharConexao();
                logger.Error("BuscarOrdem(): " + ex.Message, ex);
                return null;
            }
        }

        public OrderDbInfo BuscarOrdemPorOrderID(int orderID)
        {
            try
            {
                OrderDbInfo ret = new OrderDbInfo();
                SqlDataAdapter lAdapter;
                AbrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_fix_buscar_ordem_order_id", _sqlConn);

                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@OrderID", orderID));
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                {
                    ret.OrderID = lRow["OrderID"].DBToInt32();
                    ret.OrigClOrdID = lRow["OrigClOrdID"].DBToString();
                    ret.ExchangeNumberID = lRow["ExchangeNumberID"].DBToString();
                    ret.ClOrdID = lRow["ClOrdID"].DBToString();
                    ret.Account = lRow["Account"].DBToInt32();
                    ret.Symbol = lRow["Symbol"].DBToString();
                    ret.SecurityExchangeID = lRow["SecurityExchangeID"].DBToString();
                    ret.StopStartID = lRow["StopStartID"].DBToInt32();
                    ret.OrdTypeID = lRow["OrdTypeID"].DBToString();
                    ret.OrdStatus = lRow["OrdStatusID"].DBToInt32();
                    ret.RegisterTime = lRow["RegisterTime"].DBToDateTime();
                    ret.TransactTime = lRow["TransactTime"].DBToDateTime();

                    ret.ExpireDate = lRow["ExpireDate"].DBToDateTime();
                    ret.TimeInForce = lRow["TimeInForce"].DBToString();
                    ret.ChannelID = lRow["ChannelID"].DBToInt32();
                    ret.ExecBroker = lRow["ExecBroker"].DBToString();
                    ret.Side = lRow["Side"].DBToInt32();
                    ret.OrderQty = lRow["OrderQty"].DBToInt32();
                    ret.OrderQtyRemaining = lRow["OrderQtyRemaining"].DBToInt32();
                    ret.OrderQtyMin = lRow["MinQty"].DBToDecimal();
                    ret.OrderQtyApar = lRow["MaxFloor"].DBToDecimal();
                    ret.Price = lRow["Price"].DBToDecimal();
                    ret.CumQty = lRow["CumQty"].DBToInt32();
                    ret.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32();
                    ret.SystemID = lRow["SystemID"].DBToString();
                    ret.Memo = lRow["Memo"].DBToString();
                    ret.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32();
                    ret.SessionID = lRow["SessionID"].DBToString();
                    ret.SessionIDOriginal = lRow["SessionIDOrigin"].DBToString();
                    ret.IdFix = lRow["IDSession"].DBToInt32();
                    
                }
                FecharConexao();
                return ret;
            }
            catch (Exception ex)
            {
                FecharConexao();
                logger.Error("BuscarOrdem(): " + ex.Message, ex);
                return null;
            }
        }


        public OrderDbUpdateInfo BuscarOrdemUpdate(string clordid)
        {
            try
            {
                OrderDbUpdateInfo ret = new OrderDbUpdateInfo();
                SqlDataAdapter lAdapter;
                AbrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_fix_buscar_ordem_original", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@ClOrdIdOriginal", clordid));
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                {

                    ret.Account = lRow["Account"].DBToInt32();
                    ret.Instrumento = lRow["Instrumento"].DBToString();
                    ret.OrderID = lRow["OrderID"].DBToInt32();
                    ret.ClOrdID = lRow["ClOrdID"].DBToString();
                    ret.OrdStatusID = lRow["OrdStatusID"].DBToInt32();
                    ret.Price = lRow["Price"].DBToDecimal();
                    ret.Quantidade = lRow["Quantidade"].DBToInt32();
                    ret.Quantidade_Exec = lRow["Quantidade_Exec"].DBToInt32();
                    ret.Quantidade_Aparente = lRow["Quantidade_Aparente"].DBToInt32();
                    ret.Quantidade_Minima = lRow["Quantidade_Minima"].DBToInt32();
                    ret.Dt_Validade = lRow["DT_Validade"].DBToDateTime();
                    ret.Dt_Atualizacao = lRow["DT_Atualizacao"].DBToDateTime();
                }
                FecharConexao();
                return ret;
            }
            catch (Exception ex)
            {
                FecharConexao();
                logger.Error("BuscarOrdemUpdate(): " + ex.Message, ex);
                return null;
            }
        }

        

        public bool InserirOrdem(OrderDbInfo order)
        {
            try
            {
                AbrirConexao(_strConnectionStringDefault);
                _sqlCommand = new SqlCommand("prc_fix_inserir_ordem", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@OrderID", order.OrderID));
                _sqlCommand.Parameters.Add(new SqlParameter("@ClOrdID", order.ClOrdID));
                _sqlCommand.Parameters.Add(new SqlParameter("@StopStartID", order.StopStartID));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrigClOrdID", order.OrigClOrdID));
                _sqlCommand.Parameters.Add(new SqlParameter("@ExchangeNumberID", order.ExchangeNumberID));
                _sqlCommand.Parameters.Add(new SqlParameter("@Account", order.Account));
                _sqlCommand.Parameters.Add(new SqlParameter("@Symbol", order.Symbol));
                _sqlCommand.Parameters.Add(new SqlParameter("@SecurityExchangeID", order.SecurityExchangeID));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrdTypeID", order.OrdTypeID));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrdStatusID", order.OrdStatus));
                _sqlCommand.Parameters.Add(new SqlParameter("@TransactTime", order.TransactTime));
                if (DateTime.MinValue != order.ExpireDate)
                    _sqlCommand.Parameters.Add(new SqlParameter("@ExpireDate", order.ExpireDate));
                else
                    _sqlCommand.Parameters.Add(new SqlParameter("@ExpireDate", DBNull.Value));
                _sqlCommand.Parameters.Add(new SqlParameter("@TimeInForce", order.TimeInForce));
                _sqlCommand.Parameters.Add(new SqlParameter("@ChannelID", order.ChannelID));
                _sqlCommand.Parameters.Add(new SqlParameter("@ExecBroker", order.ExecBroker));
                _sqlCommand.Parameters.Add(new SqlParameter("@Side", order.Side));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrderQty", order.OrderQty));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrderQtyMin", order.OrderQtyMin));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrderQtyApar", order.OrderQtyApar));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrderQtyRemaining", order.OrderQtyRemaining));
                _sqlCommand.Parameters.Add(new SqlParameter("@Price", order.Price));
                _sqlCommand.Parameters.Add(new SqlParameter("@CumQty", order.CumQty));
                _sqlCommand.Parameters.Add(new SqlParameter("@description", order.Description));
                _sqlCommand.Parameters.Add(new SqlParameter("@FixMsgSeqNum", order.FixMsgSeqNum));
                _sqlCommand.Parameters.Add(new SqlParameter("@systemID", order.SystemID));
                _sqlCommand.Parameters.Add(new SqlParameter("@Memo", order.Memo));
                _sqlCommand.Parameters.Add(new SqlParameter("@SessionID", order.SessionID));
                _sqlCommand.Parameters.Add(new SqlParameter("@OriginalSessionID", order.SessionIDOriginal));
                _sqlCommand.Parameters.Add(new SqlParameter("@IdFix", order.IdFix));
                _sqlCommand.Parameters.Add(new SqlParameter("@MsgFix", order.MsgFix));
                
                if (!string.IsNullOrEmpty(order.HandlInst))
                    _sqlCommand.Parameters.Add(new SqlParameter("@HandlInst", order.HandlInst));
                
                int rows = _sqlCommand.ExecuteNonQuery();
                FecharConexao();
                return true;
            }
            catch (Exception ex)
            {
                _dbArq.InserirOrdem(order);
                FecharConexao();
                logger.Error("InserirOrdem(): " + ex.Message, ex);
                return false;
            }
        }

        public bool InserirOrdemDetalhe(OrderDbDetalheInfo orderDt, string clOrdID)
        {
            try
            {
                AbrirConexao(_strConnectionStringDefault);
                _sqlCommand = new SqlCommand("prc_fix_inserir_ordem_detalhe", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@TransactID", orderDt.TransactID));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrderID", orderDt.OrderID));
                _sqlCommand.Parameters.Add(new SqlParameter("@ClOrdID", clOrdID));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrderQty", orderDt.OrderQty));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrdQtyRemaining", orderDt.OrderQtyRemaining));
                _sqlCommand.Parameters.Add(new SqlParameter("@CumQty", orderDt.CumQty));
                _sqlCommand.Parameters.Add(new SqlParameter("@TradeQty", orderDt.TradeQty));
                _sqlCommand.Parameters.Add(new SqlParameter("@Price", orderDt.Price));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrderStatusID", orderDt.OrderStatusID));
                _sqlCommand.Parameters.Add(new SqlParameter("@Description", orderDt.Description));
                if (orderDt.TransactTime == DateTime.MinValue)
                    _sqlCommand.Parameters.Add(new SqlParameter("@EventTime", DateTime.Now));
                else
                    _sqlCommand.Parameters.Add(new SqlParameter("@EventTime", orderDt.TransactTime));
                _sqlCommand.Parameters.Add(new SqlParameter("@CxlRejResponseTo", orderDt.CxlRejResponseTo));
                _sqlCommand.Parameters.Add(new SqlParameter("@CxlRejReason", orderDt.CxlRejReason));
                _sqlCommand.Parameters.Add(new SqlParameter("@FixMsgSeqNum", orderDt.FixMsgSeqNum));
                int rows = _sqlCommand.ExecuteNonQuery();
                FecharConexao();
                return true;
            }
            catch (Exception ex)
            {
                _dbArq.InserirOrdemDetalhe(orderDt, clOrdID);
                FecharConexao();
                logger.Error("InserirOrdemDetalhe(): " + ex.Message, ex);
                return false;
            }
        }

        public bool InserirOrdermUpdate(OrderDbUpdateInfo updt)
        {
            try
            {
                AbrirConexao(_strConnectionStringDefault);
                _sqlCommand = new SqlCommand("prc_fix_inserir_ordem_update", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@OrderID", updt.OrderID));
                _sqlCommand.Parameters.Add(new SqlParameter("@account", updt.Account));
                _sqlCommand.Parameters.Add(new SqlParameter("@instrumento", updt.Instrumento));
                _sqlCommand.Parameters.Add(new SqlParameter("@CLOrdID", updt.ClOrdID));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrdStatusID", updt.OrdStatusID));
                _sqlCommand.Parameters.Add(new SqlParameter("@price", updt.Price));
                _sqlCommand.Parameters.Add(new SqlParameter("@quantidade", updt.Quantidade));
                _sqlCommand.Parameters.Add(new SqlParameter("@quantidade_exec", updt.Quantidade_Exec));
                _sqlCommand.Parameters.Add(new SqlParameter("@quantidade_aparente", updt.Quantidade_Aparente));
                _sqlCommand.Parameters.Add(new SqlParameter("@quantidade_minima", updt.Quantidade_Minima));
                _sqlCommand.Parameters.Add(new SqlParameter("@TimeInForce", updt.TimeInForce));
                if (DateTime.MinValue == updt.Dt_Validade)
                    _sqlCommand.Parameters.Add(new SqlParameter("@dt_validade", DBNull.Value));
                else
                    _sqlCommand.Parameters.Add(new SqlParameter("@dt_validade", updt.Dt_Validade));
                int rows = _sqlCommand.ExecuteNonQuery();
                FecharConexao();
                return true;
            }
            catch (Exception ex)
            {
                _dbArq.InserirOrdemUpdate(updt);
                FecharConexao();
                logger.Error("InserirOrdermUpdate(): " + ex.Message, ex);
                return false;
            }
        }

        public bool AtualizarOrdem(OrderDbInfo order)
        {
            try
            {
                AbrirConexao(_strConnectionStringDefault);
                _sqlCommand = new SqlCommand("prc_fix_atualizar_ordem", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@OrderID", order.OrderID));
                _sqlCommand.Parameters.Add(new SqlParameter("@ClOrdID", order.ClOrdID));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrigClOrdID", order.OrigClOrdID));
                _sqlCommand.Parameters.Add(new SqlParameter("@ExchangeNumberID", order.ExchangeNumberID));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrdStatusID", order.OrdStatus));
                _sqlCommand.Parameters.Add(new SqlParameter("@TransactTime", order.TransactTime));
                if (DateTime.MinValue != order.ExpireDate)
                    _sqlCommand.Parameters.Add(new SqlParameter("@ExpireDate", order.ExpireDate));
                else
                    _sqlCommand.Parameters.Add(new SqlParameter("@ExpireDate", DBNull.Value));
                _sqlCommand.Parameters.Add(new SqlParameter("@TimeInForce", order.TimeInForce));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrderQty", order.OrderQty));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrderQtyMin", order.OrderQtyMin));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrderQtyApar", order.OrderQtyApar));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrderQtyRemaining", order.OrderQtyRemaining));
                _sqlCommand.Parameters.Add(new SqlParameter("@CumQty", order.CumQty));
                _sqlCommand.Parameters.Add(new SqlParameter("@Price", order.Price));
                _sqlCommand.Parameters.Add(new SqlParameter("@FixMsgSeqNum", order.FixMsgSeqNum));
                _sqlCommand.Parameters.Add(new SqlParameter("@Memo", order.Memo));
                if(string.IsNullOrEmpty(order.HandlInst))
                    _sqlCommand.Parameters.Add(new SqlParameter("@HandlInst", DBNull.Value));
                else
                    _sqlCommand.Parameters.Add(new SqlParameter("@HandlInst", order.HandlInst));

                if (string.IsNullOrEmpty(order.MsgFix))
                    _sqlCommand.Parameters.Add(new SqlParameter("@MsgFix", DBNull.Value));
                else
                    _sqlCommand.Parameters.Add(new SqlParameter("@MsgFix", order.MsgFix));
                int rows = _sqlCommand.ExecuteNonQuery();
                FecharConexao();
                return true;
            }
            catch (Exception ex)
            {
                _dbArq.AtualizarOrdem(order);
                FecharConexao();
                logger.Error("AtualizarOrdem(): " + ex.Message, ex);
                return false;
            }
        }

    }
}
