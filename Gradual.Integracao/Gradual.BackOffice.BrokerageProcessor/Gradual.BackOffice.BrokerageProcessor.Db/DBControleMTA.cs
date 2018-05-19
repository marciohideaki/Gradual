using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Gradual.BackOffice.BrokerageProcessor.Lib.MTA;
using log4net;
using System.Data.OracleClient;

namespace Gradual.BackOffice.BrokerageProcessor.Db
{
    public class DBControleMTA
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, DownloadMTAInfo> ObterControleDownloadsMTA()
        {
            Dictionary<string, DownloadMTAInfo> lRetorno = new Dictionary<string, DownloadMTAInfo>();

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualSpider"].ConnectionString);

            sqlConn.Open();

            SqlDataAdapter lAdapter;

            DataTable table = new DataTable();

            string sqlQuery = "SELECT * FROM tb_controle_download_mta";

            logger.Debug("sqlQuery = [" + sqlQuery + "]");


            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);

            sqlCmd.CommandType = System.Data.CommandType.Text;

            lAdapter = new SqlDataAdapter(sqlCmd);

            lAdapter.SelectCommand.Connection = sqlConn;

            lAdapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DownloadMTAInfo info = new DownloadMTAInfo();

                    info.HasCLCO = table.Rows[i]["flg_clco"].DBToInt32() == 1;
                    info.HasCMDF = table.Rows[i]["flg_cmdf"].DBToInt32() == 1;
                    info.HasCSGD = table.Rows[i]["flg_csgd"].DBToInt32() == 1;
                    info.HasPFEN = table.Rows[i]["flg_pfen"].DBToInt32() == 1;
                    info.HasPENR = table.Rows[i]["flg_penr"].DBToInt32() == 1;
                    
                    info.NotificadoCLCO = table.Rows[i]["ntf_clco"].DBToInt32() == 1;
                    info.NotificadoCMDF = table.Rows[i]["ntf_cmdf"].DBToInt32() == 1;
                    info.NotificadoCSGD = table.Rows[i]["ntf_csgd"].DBToInt32() == 1;
                    info.NotificadoPFEN = table.Rows[i]["ntf_pfen"].DBToInt32() == 1;
                    info.NotificadoPENR = table.Rows[i]["ntf_penr"].DBToInt32() == 1;
                    
                    info.PathCLCO = table.Rows[i]["path_clco"].DBToString();
                    info.PathCMDF = table.Rows[i]["path_cmdf"].DBToString();
                    info.PathCSGD = table.Rows[i]["path_csgd"].DBToString();
                    info.PathPFEN = table.Rows[i]["path_pfen"].DBToString();
                    info.PathPENR = table.Rows[i]["path_penr"].DBToString();
                    
                    info.MD5CLCO = table.Rows[i]["md5_clco"].DBToString();
                    info.MD5CMDF = table.Rows[i]["md5_cmdf"].DBToString();
                    info.MD5CSGD = table.Rows[i]["md5_csgd"].DBToString();
                    info.MD5PFEN = table.Rows[i]["md5_pfen"].DBToString();
                    info.MD5PENR = table.Rows[i]["md5_penr"].DBToString();

                    DateTime dtref = table.Rows[i]["dt_referencia"].DBToDateTime();

                    if ( !lRetorno.ContainsKey(dtref.ToString("yyyyMMdd")) )
                        lRetorno.Add(dtref.ToString("yyyyMMdd"), info);
                    else
                        lRetorno[dtref.ToString("yyyyMMdd")] = info;
                }       
            }

            sqlConn.Close();

            sqlConn.Dispose();

            return lRetorno;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void InserirControleDownloadMTA()
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualSpider"].ConnectionString);

            sqlConn.Open();

            string sqlQuery = "INSERT INTO tb_controle_download_mta ";
            sqlQuery += " (dt_referencia) VALUES (DATEADD(D, 0, DATEDIFF(D, 0, getdate())))";

            logger.Debug("sQuery [" + sqlQuery + "]");

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConn;
            sqlCmd.CommandText = sqlQuery;
            sqlCmd.CommandType = CommandType.Text;

            sqlCmd.ExecuteNonQuery();

            sqlConn.Close();

            sqlConn.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        public static void AtualizarControleDownloadMTA(DateTime dtRef, DownloadMTAInfo info)
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualSpider"].ConnectionString);

            sqlConn.Open();

            string sqlQuery = "UPDATE tb_controle_download_mta SET ";

            sqlQuery += String.Format(" flg_clco =  {0} , ", info.HasCLCO?1:0);
            sqlQuery += String.Format(" flg_cmdf =  {0} , ", info.HasCMDF?1:0);
            sqlQuery += String.Format(" flg_csgd =  {0} , ", info.HasCSGD?1:0);
            sqlQuery += String.Format(" flg_penr =  {0} , ", info.HasPENR?1:0);
            sqlQuery += String.Format(" flg_pfen =  {0} , ", info.HasPFEN?1:0);
            sqlQuery += String.Format(" ntf_clco =  {0} , ", info.NotificadoCLCO?1:0);
            sqlQuery += String.Format(" ntf_cmdf =  {0} , ", info.NotificadoCMDF?1:0);
            sqlQuery += String.Format(" ntf_csgd =  {0} , ", info.NotificadoCSGD?1:0);
            sqlQuery += String.Format(" ntf_penr =  {0} , ", info.NotificadoPENR?1:0);
            sqlQuery += String.Format(" ntf_pfen =  {0} , ", info.NotificadoPFEN?1:0);
            sqlQuery += String.Format(" path_clco= '{0}', ", info.PathCLCO);
            sqlQuery += String.Format(" path_cmdf= '{0}', ", info.PathCMDF);
            sqlQuery += String.Format(" path_csgd= '{0}', ", info.PathCSGD);
            sqlQuery += String.Format(" path_penr= '{0}', ", info.PathPENR);
            sqlQuery += String.Format(" path_pfen= '{0}', ", info.PathPFEN);
            sqlQuery += String.Format(" md5_clco = '{0}', ", info.MD5CLCO);
            sqlQuery += String.Format(" md5_cmdf = '{0}', ", info.MD5CMDF);
            sqlQuery += String.Format(" md5_csgd = '{0}', ", info.MD5CSGD);
            sqlQuery += String.Format(" md5_penr = '{0}', ", info.MD5PENR);
            sqlQuery += String.Format(" md5_pfen = '{0}'  ", info.MD5PFEN);

            sqlQuery += " WHERE DATEADD(D, 0, DATEDIFF(D, 0, dt_referencia)) = DATEADD(D, 0, DATEDIFF(D, 0, @dataRef))";

            logger.Debug("sQuery [" + sqlQuery + "]");

            SqlCommand sqlCmd = new SqlCommand();

            sqlCmd.Parameters.AddWithValue("@dataRef", dtRef);

            sqlCmd.Connection = sqlConn;
            sqlCmd.CommandText = sqlQuery;
            sqlCmd.CommandType = CommandType.Text;

            sqlCmd.ExecuteNonQuery();

            sqlConn.Close();

            sqlConn.Dispose();
        }

        public bool  MTALimpezaMargem()
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["SINACOR"].ConnectionString);

                StringBuilder sqlQuery = new StringBuilder();

                sqlQuery.AppendLine("BEGIN");
                sqlQuery.AppendLine("DELETE FROM TMFMARGDIN;");
                sqlQuery.AppendLine("UPDATE TMFGARMAR SET VL_DINHEIRO = 0 WHERE DT_DATMOV >= TO_DATE('25112015', 'DDMMYYYY');");
                sqlQuery.AppendLine("COMMIT;");

                sqlQuery.AppendLine("END;");

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery.ToString().Replace("\r\n", "\n");
                cmd.CommandType = CommandType.Text;

                conn.Open();

                cmd.ExecuteNonQuery();

                cmd.Dispose();

                conn.Dispose();
            }
            catch (Exception ex)
            {
                logger.Error("MTALimpezaMargem" + ex.Message, ex);
                return false;
            }

            return true;
        }

        public bool IsFeriadoSinacor()
        {
            bool retorno = false;

            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["SINACOR"].ConnectionString);

                StringBuilder sqlQuery = new StringBuilder();

                sqlQuery.AppendLine("SELECT * FROM TGEFERIA WHERE DT_FERIADO = TRUNC(SYSDATE) AND ( CD_PRACA IS NULL OR CD_PRACA = 1 )");

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery.ToString().Replace("\r\n", "\n");
                cmd.CommandType = CommandType.Text;

                conn.Open();

                OracleDataReader objDataReader = cmd.ExecuteReader();
                if (objDataReader.HasRows)
                {
                    retorno = true;
                }

                cmd.Dispose();

                conn.Dispose();
            }
            catch (Exception ex)
            {
                logger.Error("IsFeriadoSinacor" + ex.Message, ex);
            }

            return retorno;
        }

        public List<string> ObterFeriadosSinacor()
        {
            List<string> retorno = new List<string>();

            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["SINACOR"].ConnectionString);

                StringBuilder sqlQuery = new StringBuilder();

                sqlQuery.AppendLine("SELECT * FROM TGEFERIA WHERE DT_FERIADO = TRUNC(SYSDATE)");

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery.ToString().Replace("\r\n", "\n");
                cmd.CommandType = CommandType.Text;

                conn.Open();

                OracleDataReader objDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (objDataReader.HasRows)
                {
                    while (objDataReader.Read())
                    {
                        retorno.Add(objDataReader.GetDateTime(objDataReader.GetOrdinal("DT_FERIADO")).ToString("yyyy/MM/dd") );
                    }
                }

                cmd.Dispose();

                conn.Dispose();
            }
            catch (Exception ex)
            {
                logger.Error("ObterFeriadosSinacor" + ex.Message, ex);
            }

            return retorno;
        }

    }
}
