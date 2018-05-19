using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

using log4net;
using System.Data;

using Gradual.Core.Spider.Monitoring.Lib;
using Gradual.Core.Spider.Monitoring.Lib.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Gradual.Core.Spider.Monitor.Db
{
    public class DbMonitor
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
        public DbMonitor()
        {
            _sqlConn = null;
            _sqlCommand = null;
            _strConnectionStringDefault = ConfigurationManager.ConnectionStrings["GradualSpider"].ConnectionString;
        }


        protected void _abrirConexao(string strConnectionString)
        {
            if (!this.ConexaoAberta)
            {
                _sqlConn = new SqlConnection(strConnectionString);
                _sqlConn.Open();
            }
        }

        private void _fecharConexao()
        {
            try
            {
                _sqlConn.Close();
                _sqlConn.Dispose();
            }
            catch { }
        }

        ~DbMonitor()
        {
            if (null != _sqlCommand)
            {
                _sqlCommand.Dispose();
                _sqlCommand = null;
            }
            if (null != _sqlConn)
            {
                _fecharConexao();
            }
        }

        #region Consultas
        /// <summary>
        /// Configuration Load
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ConfigInfo> LoadMonitorConfig()
        {
            try
            {
                Dictionary<string, ConfigInfo> ret = null;
                SqlDataAdapter lAdapter;
                _abrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_spider_monitor_config_lst", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                if (lDataSet.Tables.Count > 0)
                {
                    if (lDataSet.Tables[0].Rows.Count != 0)
                        ret = new Dictionary<string, ConfigInfo>();
                    foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                    {
                        ConfigInfo item = new ConfigInfo();
                        item.IdConfig  = lRow["id_config"].DBToInt32();
                        item.Chave = lRow["chave"].DBToString();
                        item.Valor = lRow["valor"].DBToString();
                        item.Description = lRow["description"].DBToString();
                        item.DateReg = lRow["date_reg"].DBToDateTime();
                        ConfigInfo itemAux = null;
                        if (!ret.TryGetValue(item.Chave, out itemAux))
                            ret.Add(item.Chave, item);
                        else
                            itemAux = item;
                    }
                }
                _fecharConexao();
                lAdapter.Dispose();
                lAdapter = null;
                lDataSet.Dispose();
                lDataSet = null;
                _sqlCommand = null;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro no carregamento das configuracoes de monitoramento: " + ex.Message, ex);
                return null;
            }
        }

        public Dictionary<int, FixSessionInfo> LoadMonitorSessionFix(int idType)
        {
            try
            {
                Dictionary<int, FixSessionInfo> ret = null;
                SqlDataAdapter lAdapter;
                _abrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_spider_monitor_session_fix_lst", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@id_type", idType));
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                if (lDataSet.Tables.Count > 0)
                {
                    if (lDataSet.Tables[0].Rows.Count != 0)
                        ret = new Dictionary<int, FixSessionInfo>();
                    foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                    {
                        FixSessionInfo item = new FixSessionInfo();
                        item.MainInfo.IdMonitor = lRow["id_monitor"].DBToInt32();
                        //item.MainInfo.IdFix = lRow["IdFix"].DBToInt32();
                        item.MainInfo.Description = lRow["description"].DBToString();
                        item.MainInfo.UpdateTime = lRow["update_time"].DBToDateTime();
                        //item.MainInfo.IdType = lRow["id_type"].DBToInt32();
                        if (lRow["id_type"].DBToString().Equals("n", StringComparison.CurrentCultureIgnoreCase) ||
                            lRow["id_type"].DBToString().Equals("0", StringComparison.CurrentCultureIgnoreCase) ||
                            string.IsNullOrEmpty(lRow["id_type"].DBToString()))
                            item.MainInfo.Active = false;
                        else
                            item.MainInfo.Active = true;

                        item.IdMonitor = item.MainInfo.IdMonitor;
                        // item.IdFix = item.MainInfo.IdFix;
                        item.ActiveUsers = lRow["active_users"].DBToInt32();
                        item.OrderCount = lRow["order_count"].DBToInt32();
                        item.TransactionCount = lRow["transaction_count"].DBToInt32();
                        item.LastMessage = lRow["last_message"].DBToDateTime();
                        item.BeginString = lRow["beginstring"].DBToString();
                        item.TargetCompID = lRow["targetcompid"].DBToString();
                        item.TargetSubID = lRow["targetsubid"].DBToString();
                        item.SenderCompID = lRow["sendercompid"].DBToString();
                        item.SenderSubID = lRow["sendersubid"].DBToString();
                        ret.Add(item.IdMonitor, item);
                    }
                }
                _fecharConexao();
                lAdapter.Dispose();
                lAdapter = null;
                lDataSet.Dispose();
                lDataSet = null;
                _sqlCommand = null;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro no carregamento das configuracoes de monitoramento: " + ex.Message, ex);
                return null;
            }
        }

        public Dictionary<int, MonitorInfo> LoadMonitors()
        {
            try
            {
                Dictionary<int, MonitorInfo> ret = null;
                SqlDataAdapter lAdapter;
                _abrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_spider_monitor_sel", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                if (lDataSet.Tables.Count > 0)
                {
                    if (lDataSet.Tables[0].Rows.Count != 0)
                        ret = new Dictionary<int, MonitorInfo>();
                    foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                    {
                        MonitorInfo item = new MonitorInfo();
                        item.IdMonitor = lRow["id_monitor"].DBToInt32();
                        item.KeyValue = lRow["key_value"].DBToString();
                        item.Description = lRow["description"].DBToString();
                        item.UpdateTime = lRow["update_time"].DBToDateTime();
                        if (lRow["active"].DBToString().Equals("n", StringComparison.CurrentCultureIgnoreCase) ||
                            lRow["active"].DBToString().Equals("0", StringComparison.CurrentCultureIgnoreCase))
                            item.Active = false;
                        else
                            item.Active = true;

                        string  content = lRow["content"].DBToString();
                        if (!string.IsNullOrEmpty(content))
                        {
                            IsoDateTimeConverter dtConvert = new IsoDateTimeConverter();
                            dtConvert.DateTimeFormat = "dd/MM/yyyy HH:mm:ss.fff";
                            item.Content = JsonConvert.DeserializeObject<Dictionary<string, string>>(content, dtConvert);
                        }
                        ret.Add(item.IdMonitor, item);

                    }
                }
                _fecharConexao();
                lAdapter.Dispose();
                lAdapter = null;
                lDataSet.Dispose();
                lDataSet = null;
                _sqlCommand = null;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro no carregamento das informacoes de monitoramento: " + ex.Message, ex);
                return null;
            }
        }

        #endregion

        #region Cadastros / Atualizacoes
        public bool InsertMonitorFix(FixSessionInfo fix)
        {
            try
            {
                _abrirConexao(_strConnectionStringDefault);
                _sqlCommand = new SqlCommand("prc_spider_monitor_fix_insert", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@description", fix.MainInfo.Description));
                //_sqlCommand.Parameters.Add(new SqlParameter("@id_type", fix.MainInfo.IdType));
                if (fix.MainInfo.Active)
                    _sqlCommand.Parameters.Add(new SqlParameter("@active", 'Y'));
                else
                    _sqlCommand.Parameters.Add(new SqlParameter("@active", 'N'));

                _sqlCommand.Parameters.Add(new SqlParameter("@active", fix.MainInfo.Active));
                _sqlCommand.Parameters.Add(new SqlParameter("@active_user", fix.ActiveUsers));
                _sqlCommand.Parameters.Add(new SqlParameter("@order_count", fix.OrderCount));
                _sqlCommand.Parameters.Add(new SqlParameter("@transaction_count", fix.TransactionCount));
                _sqlCommand.Parameters.Add(new SqlParameter("@last_msg", fix.LastMessage));
                _sqlCommand.Parameters.Add(new SqlParameter("@begin_str", fix.BeginString));
                _sqlCommand.Parameters.Add(new SqlParameter("@targetcomp_id", fix.TargetCompID));
                _sqlCommand.Parameters.Add(new SqlParameter("@targetsub_id", fix.TargetSubID));
                _sqlCommand.Parameters.Add(new SqlParameter("@sendercomp_id", fix.SenderCompID));
                _sqlCommand.Parameters.Add(new SqlParameter("@sendersub_id", fix.SenderSubID));

                int rows = _sqlCommand.ExecuteNonQuery();
                _fecharConexao();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na insercao do monitoramento fix: " + ex.Message, ex);
                return false;
            }
        }

        public bool UpdateMonitorInfo(MonitorInfo item)
        {
            try
            {
                _abrirConexao(_strConnectionStringDefault);
                _sqlCommand = new SqlCommand("prc_spider_monitor_updt", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;


                _sqlCommand.Parameters.Add(new SqlParameter("@id_monitor", item.IdMonitor));
                _sqlCommand.Parameters.Add(new SqlParameter("@key_value", item.KeyValue));
                _sqlCommand.Parameters.Add(new SqlParameter("@description", item.Description));
                _sqlCommand.Parameters.Add(new SqlParameter("@update_time", item.UpdateTime));
                if (item.Active)
                    _sqlCommand.Parameters.Add(new SqlParameter("@active", "S"));
                else
                    _sqlCommand.Parameters.Add(new SqlParameter("@active", "N"));

                IsoDateTimeConverter dtConvert = new IsoDateTimeConverter();
                dtConvert.DateTimeFormat = "dd/MM/yyyy HH:mm:ss.fff";
                string content = JsonConvert.SerializeObject(item.Content, dtConvert);

                _sqlCommand.Parameters.Add(new SqlParameter("@content", content));
                int rows = _sqlCommand.ExecuteNonQuery();
                _fecharConexao();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na atualizacao do monitoramento fix: " + ex.Message, ex);
                return false;
            }
        }
        #endregion
    }
}