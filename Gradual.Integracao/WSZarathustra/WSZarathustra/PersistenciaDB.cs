using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Data.OracleClient;
using System.Data;
using System.Globalization;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace WSZarathustra
{
    public class PersistenciaDB
    {
        private CultureInfo ciPtBR = CultureInfo.CreateSpecificCulture("pt-BR");
        private CultureInfo ciEn = CultureInfo.CreateSpecificCulture("en-US");
        private string _ConnectionString;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="connstring"></param>
        public PersistenciaDB(string connstring)
        {
            _ConnectionString = connstring;
        }

        /// <summary>
        /// DBConnect() - function to connect to an oracle database
        /// </summary>
        /// <returns>OracleConnection object</returns>
        private OracleConnection DBConnect()
        {
            OracleConnection objORAConnection = new OracleConnection();

            objORAConnection.ConnectionString = _ConnectionString;

            objORAConnection.Open();

            return objORAConnection;
        }

        public Dictionary<string, Trade> BuscarTrades(DateTime lastSnapshot, string pathSQLFile)
        {
            Dictionary<string, Trade> gtrades = new Dictionary<string,Trade>();
            try
            {
                string sql = File.ReadAllText(pathSQLFile);

                string query = String.Format(sql, lastSnapshot.ToString("yyyy/MM/dd"), lastSnapshot.ToString("yyyy/MM/dd 00:00:00"));

                logger.Debug("BuscarTrades(): [" + query + "]");

                OracleDataReader objDataReader = _ExecutaSELECT(query);
                if (objDataReader.HasRows)
                    gtrades = _Sinacor2TradeList(objDataReader);

                objDataReader.Close();
                objDataReader.Dispose();
            }
            catch (Exception ex)
            {
                logger.Error("BuscarTrades(): (" + ex.StackTrace + ") [" + ex.Message + "]");
            }

            return gtrades;
        }


        private OracleDataReader _ExecutaSELECT(string query)
        {
            OracleDataReader objRetorno;

            OracleConnection objORAConnection = DBConnect();

            using (OracleCommand objORACommand = objORAConnection.CreateCommand())
            {
                objORACommand.CommandText = query;
                objRetorno = objORACommand.ExecuteReader(CommandBehavior.CloseConnection);
            }

            return objRetorno;
        }


        /// <summary>
        /// Popula um objeto Trade a partir dos dados lidos do Oracle
        /// </summary>
        /// <param name="odr"></param>
        /// <returns></returns>
        private Trade _Sinacor2Trade(OracleDataReader odr)
        {
            Trade trade = new Trade();

            try
            {

                trade.TradeID = OracleConvert.GetString("NR_SEQCOMI", odr);

                string dtStamp = OracleConvert.GetString("DT_STAMP_N", odr);

                trade.TradeTimestamp = DateTime.ParseExact(dtStamp, "yyyy/MM/dd HH:mm:ss", ciEn);

                trade.TradeDate = OracleConvert.GetDateTime("DT_NEGOCIO", odr);
                trade.HoraNegocio = OracleConvert.GetString("HR_NEGOCIO", odr);
                trade.Orientation = OracleConvert.GetString("CD_NATOPE", odr);
                trade.ProductID = OracleConvert.GetString("CD_COMMOD", odr);
                trade.MarketID = OracleConvert.GetString("CD_MERCAD", odr);
                trade.Serie = OracleConvert.GetString("SERPAP", odr);
                trade.NegotiationChannel = OracleConvert.GetString("CD_CANAL", odr);
                trade.AfterHours = OracleConvert.GetString("IN_AFTER", odr).Equals("S") ? true : false;
                trade.NumberOfContracts = OracleConvert.GetInt("QT_QTDESP", odr);
                trade.Price = Convert.ToDecimal( OracleConvert.GetDouble("PR_NEGOCIO", odr));
                trade.SequentialNumber = OracleConvert.GetInt("NR_SEQORD", odr).ToString();
                trade.Papel = OracleConvert.GetString("CODNEG", odr);
                trade.MaturityDate = OracleConvert.GetDateTime("DT_VENC", odr);
                trade.HedgeLongMaturity = trade.MaturityDate;

                if ( trade.Orientation.Equals("C") )
                {
                    trade.BuyerCode = OracleConvert.GetInt("CD_CLIENTE", odr).ToString();
                    trade.BuyerName = OracleConvert.GetString("NM_CLIENTE", odr);
                    trade.SellerCode = OracleConvert.GetInt("CD_CORRET", odr).ToString();
                    trade.SellerName = OracleConvert.GetString("NM_CONTRAPARTE", odr);
                }
                else
                {
                    trade.SellerCode = OracleConvert.GetInt("CD_CLIENTE", odr).ToString();
                    trade.SellerName = OracleConvert.GetString("NM_CLIENTE", odr);
                    trade.BuyerCode = OracleConvert.GetInt("CD_CORRET", odr).ToString();
                    trade.BuyerName = OracleConvert.GetString("NM_CONTRAPARTE", odr);
                }

                trade.BrokerCode = "227";
                trade.BrokerName = "GRADUAL INVESTIMENTOS";
                trade.BrokerType = "";
                trade.TraderCode = OracleConvert.GetString("CD_CPFCGC_EMIT", odr);
                trade.TraderName = OracleConvert.GetString("NM_EMIT_ORDEM", odr);
                trade.ValReference1 = Convert.ToDecimal(OracleConvert.GetDouble("VL_FUT_CURTO", odr));
                trade.ValReference2 = Convert.ToDecimal(OracleConvert.GetDouble("VL_FUT_LONGO", odr));

                if (trade.ProductID.Equals("VID") ||
                    trade.ProductID.Equals("VTF") ||
                    trade.ProductID.Equals("VTC"))
                {
                    TradeDetail detail = new TradeDetail();

                    detail.NumberOfContracts = trade.NumberOfContracts;
                    detail.Price = trade.Price;
                    detail.MarketID = trade.MarketID;

                    if (trade.ValReference1 > 0)
                        detail.Type = "Short";

                    if (trade.ValReference2 > 0)
                        detail.Type = "Long";

                    if ( trade.Orientation.Equals("B") )
                        trade.OptionType = "CALL";
                    else
                        trade.OptionType = "PUT";

                    trade.TradeDetails.Add(detail);
                }

                int tpregistro = OracleConvert.GetInt("TP_REGISTRO", odr);

                switch (tpregistro)
                {
                    case 1: trade.RecordType = "NW"; break;
                    case 2: trade.RecordType = "DL"; break;
                    case 3: trade.RecordType = "AM"; break;
                    default: trade.RecordType = "IV"; break;
                }

                trade.SegmentoBolsa =  OracleConvert.GetString("MERCADO", odr);

                return trade;
            }
            catch (Exception ex)
            {
                logger.Error("_Sinacor2Trade(): (" + ex.StackTrace + ") [" + ex.Message + "]");
            }

            return null;
        }

        /// <summary>
        /// Retorna uma lista de objetos Trade, a partir do data reader oracle
        /// </summary>
        /// <param name="datareader"></param>
        /// <returns>List[Trade] object</returns>
        private Dictionary<string, Trade> _Sinacor2TradeList(OracleDataReader datareader)
        {
            Dictionary<string, Trade> ret = new Dictionary<string, Trade>();

            int i = 1;
            while (datareader.Read())
            {
                Trade trade = _Sinacor2Trade(datareader);
                trade.RecordNumber = i;
                if ( trade != null )
                    ret.Add(trade.TradeID, trade);

                i++;
            }

            return ret;
        }

        #region Trades_cache
        public bool LimparCacheTrades()
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TradesCache"].ConnectionString);

                conn.Open();

                SqlCommand sqlCmd = new SqlCommand("DELETE FROM tradeCache", conn);
                sqlCmd.CommandType = System.Data.CommandType.Text;

                sqlCmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                logger.Error("LimparCacheTrades():" + ex.Message, ex);
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
                return false;
            }

            return true;
        }

        #endregion
    }
}
