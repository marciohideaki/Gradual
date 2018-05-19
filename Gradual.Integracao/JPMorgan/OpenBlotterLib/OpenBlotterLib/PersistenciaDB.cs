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

namespace OpenBlotterLib
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

        public Dictionary<string, Trade> BuscarTrades(DateTime lastSnapshot, string clientIdList)
        {
            Dictionary<string, Trade> gtrades = new Dictionary<string,Trade>();

            string query = "";
            query += "SELECT ";
            query += "      TO_CHAR(C.NR_SEQCOMI) as NR_SEQCOMI,";
            query += "      N.DT_STAMP,";
            query += "      C.NR_NEGOCIO,";
            query += "      C.TP_REGISTRO,";
            query += "      C.DT_NEGOCIO,";
            query += "      C.CD_COMMOD,";
            query += "      C.CD_SERIE,";
            query += "      C.CD_MERCAD,";
            query += "      C.QT_QTDESP,";
            query += "      C.CD_CLIENTE,";
            query += "      C.CD_NATOPE,";
            query += "      TO_CHAR(N.DT_STAMP,'YYYY/MM/DD HH24:MI:SS') as DT_STAMP_N,";
            query += "      TO_CHAR(N.HR_NEGOCIO) as HR_NEGOCIO,";
            query += "      N.CD_CANAL,";
            query += "      N.IN_AFTER,";
            query += "      N.PR_NEGOCIO,";
            query += "      N.VL_DELTA,";
            query += "      N.CD_CLIENTE as CD_CONTRAPARTE,";
            query += "      N.CD_CONTRAPAR as CD_CORRET,";
            query += "      N.VL_FUT_CURTO,";
            query += "      N.VL_FUT_LONGO,";
            query += "      N.DT_STAMP ,";
            query += "      G.NM_CLIENTE,";
            query += "      L.NM_CORRET AS NM_CONTRAPARTE ,";
            query += "      P.DT_VENC,";
            query += "      D.NM_EMIT_ORDEM,";
            query += "      TO_CHAR(E.CD_CPFCGC_EMIT,'FM99999999999999999999') CD_CPFCGC_EMIT";
            query += "  FROM ";
            query += "      tmfcomi C";
            query += "      JOIN tmfnegos N";
            query += "      ON (C.NR_NEGOCIO = N.NR_NEGOCIO";
            query += "      AND C.TP_REGISTRO = N.TP_REGISTRO";
            query += "      AND C.DT_NEGOCIO = N.DT_PREGAO";
            query += "      AND C.CD_NATOPE = N.CD_NATOPE";
            query += "      AND C.CD_NEGOCIO = N.CD_NEGOCIO)";
            query += "";
            query += "      join tscclibmf B";
            query += "      on (C.CD_CLIENTE = B.CODCLI)";
            query += "";
            query += "      join tsccliger G";
            query += "      on (b.CD_CPFCGC = g.cd_cpfcgc)";
            query += "";
            query += "      join tmfserie P";
            query += "      on(P.CD_SERIE = C.CD_SERIE)";
            query += "      and P.CD_COMMOD = C.CD_COMMOD";
            query += "      and p.CD_MERCAD = C.CD_MERCAD";
            query += "      and P.DT_VENC >= C.DT_NEGOCIO";
            query += "";
            query += "      join tmfmovd D";
            query += "      on ( D.NR_SEQORD = C.NR_SEQORD";
            query += "      and D.CD_NEGOCIO = C.CD_NEGOCIO";
            query += "      and D.DT_DATORD = C.DT_NEGOCIO";
            query += "      and D.CD_CLIENTE = C.CD_CLIENTE";
            query += "      and D.CD_NATOPE = C.CD_NATOPE)";
            query += "";
            query += "      join  TSCEMITORDEM  E";
            query += "      on(  E.CD_CPFCGC     = b.CD_CPFCGC";
            query += "      and  E.DT_NASC_FUND  = b.DT_NASC_FUND";
            query += "      and  E.CD_CON_DEP    = b.CD_CON_DEP";
            query += "      and  E.CD_SISTEMA    = 'BMF'";
            query += "      AND  E.NR_SEQ_EMIT   = D.NR_SEQ_EMIT )";
            query += "      left join TMFCORRET  L";
            query += "      on(L.cd_corret = N.cd_contrapar)    ";
            query += "";
            query += "      WHERE";
            query += "      N.DT_STAMP >= TO_DATE('" + lastSnapshot.ToString("yyyy/MM/dd HH:mm:ss") + "','YYYY/MM/DD HH24:MI:SS')";
            query += "      and C.CD_COMMOD IN ('D11','D12','D13','D14','D15','D16','D17','DAP','DDI','DI1','DOL','DR1','FRC','FRP','IAP','IDI','IND','IR1','SCC','VF2','VF3', 'VUD', 'VTC')";
            query += "      and C.CD_CLIENTE_BRO IN (" + clientIdList + ")";  //10080,10871,10126,10313,10314,140211)";
            query += "      and C.TP_REGISTRO IN (1,2,3)";
            query += "      ORDER BY 1 DESC";

            logger.Debug("BuscarTrades(): [" + query + "]");

            try
            {
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
                trade.Orientation = OracleConvert.GetString("CD_NATOPE", odr).Equals("C") ? "B" : "S";
                trade.ProductID = OracleConvert.GetString("CD_COMMOD", odr);
                trade.MarketID = OracleConvert.GetString("CD_MERCAD", odr);
                trade.Serie = OracleConvert.GetString("CD_SERIE", odr);
                trade.NegotiationChannel = OracleConvert.GetString("CD_CANAL", odr);
                trade.AfterHours = OracleConvert.GetString("IN_AFTER", odr).Equals("S") ? true : false;
                trade.NumberOfContracts = OracleConvert.GetInt("QT_QTDESP", odr);
                trade.Price = Convert.ToDecimal( OracleConvert.GetDouble("PR_NEGOCIO", odr));
                trade.SequentialNumber = OracleConvert.GetInt("NR_SEQORD", odr).ToString();
                trade.Papel = OracleConvert.GetString("CD_PAPEL", odr);
                trade.MaturityDate = OracleConvert.GetDateTime("DT_VENC", odr);
                trade.HedgeLongMaturity = trade.MaturityDate;

                if ( trade.Orientation.Equals("B") )
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

            while (datareader.Read())
            {
                Trade trade = _Sinacor2Trade(datareader);
                if (trade != null)
                {
                    if (!ret.ContainsKey(trade.TradeID))
                    {
                        logger.Info("Inserindo trade [" + trade.TradeID + "]");
                        ret.Add(trade.TradeID, trade);
                    }
                    else
                    {
                        logger.Info("Atualizando trade [" + trade.TradeID + "]");
                        ret[trade.TradeID] = trade;
                    }
                }

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


        //public List<Trade> BuscarTrades(DateTime lastSnapshot)
        //{
        //    List<Trade> gtrades = null;

        //    string query = "select C.TP_REGISTRO, C.DT_NEGOCIO, C.CD_NATOPE, C.CD_COMMOD, C.CD_SERIE, C.CD_MERCAD, CONCAT(C.CD_COMMOD, C.CD_SERIE) as CD_PAPEL, C.NR_NEGOCIO, TO_CHAR(N.DT_STAMP,'YYYY/MM/DD') as DT_STAMP, TO_CHAR(N.HR_NEGOCIO) as HR_NEGOCIO,";
        //    query += " N.CD_CANAL, N.IN_AFTER, C.QT_QTDESP, N.PR_NEGOCIO, C.CD_CLIENTE, G.NM_CLIENTE, C.CD_NATOPE, ";
        //    query += " N.VL_DELTA, N.CD_CLIENTE as CD_CONTRAPARTE, N.CD_CONTRAPAR as CD_CORRET, S.NM_CLIENTE AS NM_CONTRAPARTE, N.VL_FUT_CURTO, N.VL_FUT_LONGO, P.DT_VENC, D.NM_EMIT_ORDEM, E.CD_CPFCGC_EMIT";
        //    query += " from tmfcomi C, tmfnegos N, tsccliger G, tscclibmf B, tscclibmf R, tsccliger S, tmfserie P,  tmfmovd D, tscemitordem E";
        //    query += " where N.NR_NEGOCIO=C.NR_NEGOCIO";
        //    query += " and C.CD_CLIENTE_BRO IN (10080,10871,10126,10313,10314,140211)";
        //    query += " and C.CD_CLIENTE = B.CODCLI ";
        //    query += " and B.CD_CPFCGC = G.CD_CPFCGC";
        //    query += " and N.CD_CLIENTE = R.CODCLI ";
        //    query += " and S.CD_CPFCGC = R.CD_CPFCGC";
        //    query += " and C.TP_REGISTRO IN (1,2,3)";
        //    query += " and N.DT_STAMP >= TO_DATE('" + lastSnapshot.ToString("yyyy/MM/dd HH:mm:ss") + "','YYYY/MM/DD HH24:MI:SS')";
        //    query += " and C.CD_COMMOD IN ('D11','D12','D13','D14','DAP','DDI','DI1','DOL','DR1','FRC','FRP','IAP','IDI','IND','IR1','SCC','VF2','VF3', 'VUD', 'VTC')";
        //    query += " and P.CD_COMMOD = C.CD_COMMOD";
        //    query += " and D.NR_SEQORD = C.NR_SEQORD";
        //    query += " and P.CD_SERIE = C.CD_SERIE";
        //    query += " and E.NM_EMIT_ORDEM = D.NM_EMIT_ORDEM";

        //    query += " order by DT_STAMP desc, HR_NEGOCIO desc";  // Mudar ordenacao

        //    logger.Debug("BuscarTrades(): [" + query + "]");

        //    try
        //    {
        //        OracleDataReader objDataReader = _ExecutaSELECT(query);
        //        if (objDataReader.HasRows)
        //            gtrades = _Sinacor2TradeList(objDataReader);
        //        objDataReader.Close();
        //        objDataReader.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("BuscarTrades(): (" + ex.StackTrace + ") [" + ex.Message + "]");
        //    }

        //    return gtrades;
        //}

    }
}
