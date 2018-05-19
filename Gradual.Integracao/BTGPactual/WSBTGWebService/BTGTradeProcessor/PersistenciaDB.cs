using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using log4net;
using System.Globalization;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace BTGTradeProcessor
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

        public Dictionary<string, TraderInfo> CarregarTraderInfo()
        {

            Dictionary<string, TraderInfo> ret = new Dictionary<string, TraderInfo>();
            
            // Feio pra kct, mas sem especificacao decente, vai assim....

            TraderConfig traderConfig = Gradual.OMS.Library.GerenciadorConfig.ReceberConfig<TraderConfig>();

            if (traderConfig != null)
            {
                foreach (TraderInfo info in traderConfig.TraderList)
                {
                    ret.Add(info.CpfCnpj, info);
                }
            }

            //TraderInfo info = new TraderInfo();
            //info.CdCorretoraBmfDestino = 1182;
            //info.ContaCliente = "000009300";
            //info.CpfCnpj = "21269094840";
            //info.NomeCorretoraBmfDestino = "PLD - PACTUAL";
            //info.NomeEmitente = "ROBERTO SOARES MORENO";

            //ret.Add(info.CpfCnpj, info);


            //info = new TraderInfo();
            //info.CdCorretoraBmfDestino = 1183;
            //info.ContaCliente = "000003380";
            //info.CpfCnpj = "99999999999";
            //info.NomeCorretoraBmfDestino = "PLD - ASSET";
            //info.NomeEmitente = "BRUNO COUTINHO";

            //ret.Add(info.CpfCnpj, info);

            return ret;
        }

        public Dictionary<long, Trade> BuscarTrades(string datanegos, long lastSequence, string clientIdList)
        {
            Dictionary<long, Trade> gtrades = new Dictionary<long,Trade>();


            string query = "";
            query += "SELECT ";
            query += "      C.NR_SEQCOMI as NR_SEQCOMI,";
            query += "      N.DT_STAMP,";
            query += "      C.NR_NEGOCIO,";
            query += "      C.TP_REGISTRO,";
            query += "      C.DT_NEGOCIO,";
            query += "      C.CD_COMMOD,";
            query += "      C.CD_SERIE,";
            query += "      C.CD_MERCAD,";
            query += "      CONCAT(C.CD_COMMOD, C.CD_SERIE) as CD_PAPEL,";
            query += "      C.QT_QTDESP,";
            query += "      C.CD_CLIENTE,";
            query += "      C.CD_NATOPE,";
            query += "      TO_CHAR(N.DT_STAMP,'YYYY/MM/DD HH24:MI:SS') as DT_STAMP_N,";
            query += "      TO_CHAR(N.HR_NEGOCIO) as HR_NEGOCIO,";
            query += "      N.CD_CANAL,";
            query += "      N.IN_AFTER,";
            query += "      N.PR_NEGOCIO,";
            query += "      N.VL_DELTA,";
            //query += "      N.CD_CLIENTE as CD_CONTRAPARTE,";
            //query += "      N.CD_CONTRAPAR as CD_CORRET,";
            query += "      N.CD_CONTRAPAR as CD_CONTRAPARTE,";
            query += "      N.VL_FUT_CURTO,";
            query += "      N.VL_FUT_LONGO,";
            query += "      N.DT_STAMP ,";
            query += "      TO_CHAR(G.CD_CPFCGC,'FM99999999999999999999') CNPJ_CLIENTE, ";
            query += "      G.NM_CLIENTE,";
            query += "      L.NM_CORRET AS NM_CONTRAPARTE ,";
            query += "      P.DT_VENC,";
            query += "      D.NM_EMIT_ORDEM,";
            query += "      TO_CHAR(E.CD_CPFCGC_EMIT,'FM99999999999999999999') CD_CPFCGC_EMIT,";
            query += "      N.CD_OPERADOR,";
            query += "      U.NM_USUARIO AS NM_OPERADOR ";
            query += "  FROM ";
            query += "      tmfcomi C";
            query += "      JOIN tmfnegos N";
            query += "      ON (C.NR_NEGOCIO = N.NR_NEGOCIO";
            query += "      AND C.TP_REGISTRO = N.TP_REGISTRO";
            query += "      AND C.DT_NEGOCIO = N.DT_PREGAO";
            query += "      AND C.CD_NATOPE = N.CD_NATOPE";
            query += "      AND C.CD_NEGOCIO = N.CD_NEGOCIO";
            query += "      AND N.CD_OPERADOR = '###')";
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
            query += "      left join TMFCORRET  L  on(L.cd_corret = N.cd_contrapar)    ";
            query += "";
            query += "      left join TMFOPERA J on (J.CD_OPERA_BMF = N.CD_OPERADOR) ";
            query += "      left join TGEUSUARIO U on (U.CD_USUARIO = J.CD_OPERADOR) ";
            query += "      WHERE";
            query += "       C.NR_SEQCOMI > " + lastSequence + "  ";
            query += "      and C.CD_CLIENTE_BRO IN (" + clientIdList + ")";  //10289)";
            query += "      and C.TP_REGISTRO IN (1,2,3)";

            if (lastSequence == 0)
            {
                query += "      and N.DT_STAMP >= TO_DATE('" + datanegos + "','YYYY/MM/DD HH24:MI:SS')";
            }

            query += "      ORDER BY C.NR_SEQCOMI";

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

            Dictionary<string, TraderInfo> traders = this.CarregarTraderInfo();

            try
            {
                string dtStamp = OracleConvert.GetString("DT_STAMP_N", odr);

                trade.TradeTimestamp = DateTime.ParseExact(dtStamp, "yyyy/MM/dd HH:mm:ss", ciEn);

                trade.Sequencial = trade.ChaveEstrangeira = OracleConvert.GetLong("NR_SEQCOMI", odr);
                trade.DataOperacao = OracleConvert.GetDateTime("DT_NEGOCIO", odr);
                trade.TipoOperacao = OracleConvert.GetString("CD_NATOPE", odr);
                trade.AfterHour = OracleConvert.GetString("IN_AFTER", odr);
                trade.Quantidade = OracleConvert.GetInt("QT_QTDESP", odr);
                trade.Preco = Convert.ToDecimal(OracleConvert.GetDouble("PR_NEGOCIO", odr));

                string papel = OracleConvert.GetString("CD_PAPEL", odr);
                string commod = OracleConvert.GetString("CD_COMMOD", odr).Trim();
                string serie = OracleConvert.GetString("CD_SERIE", odr).Trim();
                string mercado = OracleConvert.GetString("CD_MERCAD", odr).Trim();
                trade.NomeAtivo = commod + mercado.Substring(0, 1) + serie;

                trade.NomeCliente = OracleConvert.GetString("NM_CLIENTE", odr);
                trade.CnpjCliente = OracleConvert.GetString("CNPJ_CLIENTE", odr);
                trade.NumContaCliente = OracleConvert.GetInt("CD_CLIENTE", odr).ToString();

                string traderid = OracleConvert.GetString("CD_CPFCGC_EMIT", odr);

                logger.Debug("Buscando traderID [" + traderid + "]");
                if (!traders.ContainsKey(traderid))
                {
                    logger.Error("Trader ID [" + traderid + "] nao encontrado");
                }

                TraderInfo traderInfo = traders[traderid];

                //if (trade.NumContaCliente.Equals("10289"))
                //{
                //    trade.NumContaCliente = "000009300";
                //}
                trade.NumContaCliente = traderInfo.ContaCliente;

                trade.CodCorretoraBMFOrigem = 227;
                trade.NomeCorretoraBMFOrigem = "GRADUAL C.C.T.V.M  S/A";
                //trade.CodCorretoraBMFDestino = OracleConvert.GetInt("CD_CONTRAPARTE", odr);
                //trade.NomeCorretoraBMFDestino = OracleConvert.GetString("NM_CONTRAPARTE", odr);
                trade.CodCorretoraBMFDestino = traderInfo.CdCorretoraBmfDestino;
                trade.NomeCorretoraBMFDestino = traderInfo.NomeCorretoraBmfDestino;

                trade.DataTermo = OracleConvert.GetDateTime("DT_VENC", odr);
                trade.TraderID = OracleConvert.GetString("CD_CPFCGC_EMIT", odr);
                trade.TraderName = OracleConvert.GetString("NM_EMIT_ORDEM", odr);
                trade.Delta = Convert.ToDecimal(OracleConvert.GetDouble("VL_DELTA", odr)).ToString();
                trade.TaxaJurosCurto = Convert.ToDecimal(OracleConvert.GetDouble("VL_FUT_CURTO", odr)).ToString();
                trade.TaxaJurosLongo = Convert.ToDecimal(OracleConvert.GetDouble("VL_FUT_LONGO", odr)).ToString();
                trade.LeilaoTermo = "N";

                //trade.TipoBoletagem = "I";
                trade.ExercicioOpcaoBMF = "N";
                trade.Externa = "N";
                trade.Brokeragem = "R";

                int tpregistro = OracleConvert.GetInt("TP_REGISTRO", odr);

                switch (tpregistro)
                {
                    case 1: trade.TipoBoletagem = "I"; break;
                    case 2: trade.TipoBoletagem = "E"; break;
                    case 3: trade.TipoBoletagem = "A"; break;
                    default: trade.TipoBoletagem = "I"; break;
                }

                string cdOperador = OracleConvert.GetString("CD_OPERADOR", odr);

                if (cdOperador != null && cdOperador.Length > 0 && !cdOperador.Equals("###"))
                    trade.NomeOperador = OracleConvert.GetString("NM_OPERADOR", odr);
                else
                    trade.NomeOperador = "DMA";
                    


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
        private Dictionary<long, Trade> _Sinacor2TradeList(OracleDataReader datareader)
        {
            Dictionary<long, Trade> ret = new Dictionary<long, Trade>();

            while (datareader.Read())
            {
                Trade trade = _Sinacor2Trade(datareader);
                if (trade != null)
                    ret.Add(trade.Sequencial, trade);
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
