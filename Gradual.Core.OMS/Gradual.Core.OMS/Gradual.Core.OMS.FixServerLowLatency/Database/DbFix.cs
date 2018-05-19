using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using log4net;

using Cortex.OMS.ServidorFIX;


using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;
using Gradual.Core.OMS.DropCopy.Lib.Dados;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Gradual.Core.OMS.FixServerLowLatency.Database
{
    public class DbFix
    {

        #region log4net declaration
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables
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

        /// <summary>
        /// Constructor
        /// </summary>
        public DbFix()
        {
            _sqlConn = null;
            _sqlCommand = null;
            _strConnectionStringDefault = ConfigurationManager.ConnectionStrings["GradualSpider"].ConnectionString;
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
            catch { }
        }
        
        ~DbFix()
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
                session.SenderSubID = lRow["SenderSubID"].DBToString();
                session.SenderLocationID = lRow["SenderLocationID"].DBToString();
                session.TargetCompID = lRow["TargetCompID"].DBToString();
                session.TargetSubID = lRow["TargetSubID"].DBToString();
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
                session.IntegrationID = lRow["IntegrationID"].DBToInt32();
                session.IntegrationName = lRow["IntegrationName"].DBToString();
                aux = lRow["FinancialLimit"].DBToString();
                if (aux.Equals("0") || aux.ToLower().Equals("n") || string.IsNullOrEmpty(aux))
                    session.FinancialLimit = false;
                else
                    session.FinancialLimit = true;

                aux = lRow["ParseAccount"].DBToString();
                if (aux.Equals("0") || aux.ToLower().Equals("n") || string.IsNullOrEmpty(aux))
                    session.ParseAccount = false;
                else
                    session.ParseAccount = true;

                // PID = PartyID
                // INI -- Composicao do PartyIDs da Sessao
                SqlCommand commPID= new SqlCommand("prc_fixsrv_sessaofix_server_party_id", _sqlConn);
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
                    ret.MsgFix = lRow["MsgFix"].DBToString();
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


        public Dictionary<int, ClientMnemonicInfo> BuscarMnemonicoCliente()
        {
            try
            {
                Dictionary<int, ClientMnemonicInfo> ret = new Dictionary<int,ClientMnemonicInfo>();
                SqlDataAdapter lAdapter;
                AbrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_fix_client_mnemonic_lst", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                {
                    ClientMnemonicInfo item = new ClientMnemonicInfo();
                    item.IdCliente = lRow["id_cliente"].DBToInt32();
                    item.Mnemonic = lRow["Mnemonic"].DBToString();
                    item.Exchange = lRow["Exchange"].DBToString();
                    ret.Add(item.IdCliente, item);
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


        public OrderSessionInfo ProcesssarOrderSession(string fileName)
        {
            try
            {
                OrderSessionInfo ret = new OrderSessionInfo();
                SqlDataAdapter lAdapter;
                AbrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_fix_ordersession_consultar", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@FileName", fileName));
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                {
                    ret.Id= lRow["Id_session"].DBToInt32();
                    ret.FileName = lRow["FileName"].DBToString();
                }
                FecharConexao();
                return ret;
            }
            catch (Exception ex)
            {
                FecharConexao();
                logger.Error("ProcesssarOrderSession(): " + ex.Message, ex);
                return null;

            }
        }
        
        public bool InserirOrderSessionItem(int idSessaoFix, TOMessageBackup to)
        {
            try
            {
                AbrirConexao(_strConnectionStringDefault);
                _sqlCommand = new SqlCommand("prc_fix_ordersession_mngr_inserir", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@id_session", idSessaoFix));
                _sqlCommand.Parameters.Add(new SqlParameter("@Chave", to.Key));
                _sqlCommand.Parameters.Add(new SqlParameter("@BeginString", to.BeginString));
                _sqlCommand.Parameters.Add(new SqlParameter("@TargetCompID", to.TargetCompID));
                _sqlCommand.Parameters.Add(new SqlParameter("@TargetSubID", to.TargetSubID));
                _sqlCommand.Parameters.Add(new SqlParameter("@SenderCompID", to.SenderCompID));
                _sqlCommand.Parameters.Add(new SqlParameter("@SenderSubID", to.SenderSubID));
                _sqlCommand.Parameters.Add(new SqlParameter("@TipoExpiracao", to.TipoExpiracao));
                _sqlCommand.Parameters.Add(new SqlParameter("@DataExpiracao", to.DataExpiracao));
                _sqlCommand.Parameters.Add(new SqlParameter("@DataEnvio", to.DataEnvio));
                _sqlCommand.Parameters.Add(new SqlParameter("@MsgSeqNum", to.MsgSeqNum));
                _sqlCommand.Parameters.Add(new SqlParameter("@OrigClOrdID", to.OrigClOrdID));
                _sqlCommand.Parameters.Add(new SqlParameter("@ClOrdID", to.ClOrdID));
                _sqlCommand.Parameters.Add(new SqlParameter("@Account", to.Account));
                
                // Serializar PartyIDs
                string partyids = string.Empty;
                if (to.PartyIDs.Count > 0)
                {
                    BinaryFormatter bs = new BinaryFormatter();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        bs.Serialize(stream, to.PartyIDs);
                        partyids = Convert.ToBase64String(stream.ToArray());
                    }
                    bs = null;
                }
                _sqlCommand.Parameters.Add(new SqlParameter("@PartyIDs", partyids));
                // Serializar OrdemInfo
                string orderinfo = string.Empty;
                if (to.Order != null)
                {
                    BinaryFormatter bs = new BinaryFormatter();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        bs.Serialize(stream, to.Order);
                        orderinfo = Convert.ToBase64String(stream.ToArray());
                    }
                    bs = null;
                }
                _sqlCommand.Parameters.Add(new SqlParameter("@OrderInfo", orderinfo));
                _sqlCommand.Parameters.Add(new SqlParameter("@TipoLimite", to.TipoLimite));
                _sqlCommand.Parameters.Add(new SqlParameter("@MensagemQF", to.MensagemQF));
                _sqlCommand.Parameters.Add(new SqlParameter("@ExchangeNumberID", to.ExchangeNumberID));
                _sqlCommand.Parameters.Add(new SqlParameter("@ExchangeSeqNum", to.ExchangeSeqNum));
                _sqlCommand.Parameters.Add(new SqlParameter("@ChaveDicionario", to.ChaveDicionario));

                int rows = _sqlCommand.ExecuteNonQuery();
                FecharConexao();
                return true;
            }
            catch (Exception ex)
            {
                FecharConexao();
                logger.Error("InserirOrderSessionItem(): " + ex.Message, ex);
                return false;
            }
        }

        public List<TOMessageBackup> BuscarOrderSessionIDs(int idSessao)
        {
            try
            {
                List<TOMessageBackup> ret = new List<TOMessageBackup>();
                SqlDataAdapter lAdapter;
                AbrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_fix_ordersession_mngr_consultar", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@IdSessao", idSessao));
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                {
                    TOMessageBackup to = new TOMessageBackup();

                    to.Key = lRow["Chave"].DBToString();
                    to.BeginString = lRow["BeginString"].DBToString();
                    to.SenderCompID = lRow["SenderCompID"].DBToString();
                    to.SenderSubID = lRow["SenderSubID"].DBToString();
                    to.TargetCompID = lRow["TargetCompID"].DBToString();
                    to.TargetSubID = lRow["TargetSubID"].DBToString();
                    to.TipoExpiracao = lRow["TipoExpiracao"].DBToString();
                    to.DataExpiracao = lRow["DataExpiracao"].DBToString();
                    to.DataEnvio = lRow["DataEnvio"].DBToString();
                    to.MsgSeqNum = lRow["MsgSeqNum"].DBToString();
                    to.ClOrdID = lRow["ClOrdID"].DBToString();
                    to.OrigClOrdID = lRow["OrigClOrdID"].DBToString();
                    to.Account = lRow["Account"].DBToString();
                    
                    // TODO[FF]: Efetuar a desconversao do base64, de-serealizar e montar o list
                    string partyIds = lRow["PartyIDs"].DBToString();
                    if (!string.IsNullOrEmpty(partyIds))
                    {
                        byte[] bPartyId = Convert.FromBase64String(partyIds);
                        BinaryFormatter bs = new BinaryFormatter();
                        MemoryStream s = new MemoryStream(bPartyId);
                        to.PartyIDs = bs.Deserialize(s) as List<PartyIDBackup>;
                        s.Dispose();
                        bs = null;
                    }
                    string orderInfo = lRow["OrderInfo"].DBToString();
                    if (!string.IsNullOrEmpty(orderInfo))
                    {
                        byte[] bOrderInfo = Convert.FromBase64String(orderInfo);
                        BinaryFormatter bs = new BinaryFormatter();
                        MemoryStream s = new MemoryStream(bOrderInfo);
                        to.Order = bs.Deserialize(s) as Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemInfo;
                        s.Dispose();
                        bs = null;
                        
                    }
                    
                    to.TipoLimite = lRow["TipoLimite"].DBToInt32();
                    
                    //to.Order = item.Value.Order; // TODO [FF] - desconverter do base64, de-serealizar e montar o list
                    to.MensagemQF = lRow["MensagemQF"].DBToString();
                    to.ExchangeNumberID = lRow["ExchangeNumberID"].DBToString();
                    to.ExchangeSeqNum = lRow["ExchangeSeqNum"].DBToInt32();
                    ret.Add(to);
                    to = null;
                }
                FecharConexao();
                return ret;
            }
            catch (Exception ex)
            {
                FecharConexao();
                logger.Error("BuscarOrderSessionIDs(): " + ex.Message, ex);
                return null;
            }
        }

        public bool LimparOrderSessionIDs(int idOrderSession)
        {
            try
            {
                AbrirConexao(_strConnectionStringDefault);
                _sqlCommand = new SqlCommand("prc_fix_ordersession_mngr_limpar", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@IdSessao", idOrderSession));
                int rows = _sqlCommand.ExecuteNonQuery();
                FecharConexao();
                return true;
            }
            catch (Exception ex)
            {
                FecharConexao();
                logger.Error("InserirOrderSessionItem(): " + ex.Message, ex);
                return false;
            }
        }


    }
}
