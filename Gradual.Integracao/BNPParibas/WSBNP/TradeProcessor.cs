using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Configuration;
using System.Data;

namespace WSBNPParibas
{
    public class TradeProcessor
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public DataSet GetTradesDataset(long lastSequence)
        {
            string xmlRet = "<TRADES/>";

            TradeSerializer cache = new TradeSerializer();

            Dictionary<long, Trade> trades = new Dictionary<long, Trade>();

            PersistenciaDB db = new PersistenciaDB(ConfigurationManager.ConnectionStrings["SINACOR"].ToString());

            string datanegos = DateTime.Now.ToString("yyyy/MM/dd");
            datanegos += " 00:00:00";

            if (ConfigurationManager.AppSettings["DataNegosTeste"] != null)
            {
                datanegos = ConfigurationManager.AppSettings["DataNegosTeste"].ToString();
            }

            trades = db.BuscarTrades(datanegos, lastSequence, ConfigurationManager.AppSettings["ClientList"].ToString());


            // Gera o dataset com as tabelas e colunas
            DataSet ds = new DataSet("NewDataSet"); 
            DataTable table = new DataTable("Table");
            List<DataColumn> columnList = new List<DataColumn>();
            columnList.Add(new DataColumn("COMMOD"));
            columnList.Add(new DataColumn("VL_ROSPREAD"));
            columnList.Add(new DataColumn("MERCADO"));
            columnList.Add(new DataColumn("SERIE"));
            columnList.Add(new DataColumn("ISIN"));
            columnList.Add(new DataColumn("FUT_REF_AMNT"));
            columnList.Add(new DataColumn("SIDE"));
            columnList.Add(new DataColumn("PRICE"));
            columnList.Add(new DataColumn("NUMBER_OF_CONTRACTS"));
            columnList.Add(new DataColumn("GROSSAMOUNT"));
            columnList.Add(new DataColumn("TRADEDATE"));
            columnList.Add(new DataColumn("SETTLEMENTDATE"));
            columnList.Add(new DataColumn("NM_CLIENTE"));
            columnList.Add(new DataColumn("ACCOUNT"));
            columnList.Add(new DataColumn("TRADE_REF"));
            columnList.Add(new DataColumn("TRANSACTIONTYPE"));
            columnList.Add(new DataColumn("DEAL_DT"));
            columnList.Add(new DataColumn("TRADERID"));
            columnList.Add(new DataColumn("TRADERNAME"));
            columnList.Add(new DataColumn("PORTFOLIO"));
            columnList.Add(new DataColumn("BROKER_COD"));
            columnList.Add(new DataColumn("EVENTID"));
            columnList.Add(new DataColumn("TIPORE"));
            columnList.Add(new DataColumn("TRADEID"));
            columnList.Add(new DataColumn("DS_LOGIN"));
            columnList.Add(new DataColumn("AFTER"));


            table.Columns.AddRange(columnList.ToArray());

            ds.Tables.Add(table);

            foreach (Trade trade in trades.Values)
            {
                DataRow dr = ds.Tables["Table"].NewRow(); //DataRow to be added to DataSet

                dr["COMMOD"] = trade.Commod;
                dr["VL_ROSPREAD"] = trade.Delta;
                dr["MERCADO"] = trade.Mercado;
                dr["SERIE"] = trade.Serie;
                dr["ISIN"] = trade.Isin;
                dr["FUT_REF_AMNT"] = trade.ValorFuturoCurto; 
                dr["SIDE"] = trade.TipoOperacao;
                dr["PRICE"] = trade.Preco;
                dr["NUMBER_OF_CONTRACTS"] = trade.Quantidade;
                dr["GROSSAMOUNT"] = trade.Preco * trade.Quantidade;
                dr["TRADEDATE"] = trade.DataOperacao.ToString("dd/MM/yyyy");
                dr["NM_CLIENTE"] = trade.NomeCliente;
                dr["ACCOUNT"] = trade.NumContaCliente;
                dr["TRADE_REF"] = trade.Sequencial;
                dr["TRANSACTIONTYPE"] = trade.RecordType;
                dr["SETTLEMENTDATE"] = trade.DataVencimento.ToString("dd/MM/yyyy");
                dr["DEAL_DT"] = trade.TradeTimestamp.ToString("yyyyMMddHHmmss");
                dr["TRADERID"] = trade.TraderID;
                dr["TRADERNAME"] = trade.TraderName;
                dr["PORTFOLIO"] = "-1";
                dr["BROKER_COD"] = "GRADSAO";
                dr["EVENTID"] = "VOZ" + trade.NumOrdem + trade.NumNegocio;
                dr["TIPORE"] = "0";
                dr["TRADEID"] = "0";
                dr["DS_LOGIN"] = "0";
                dr["AFTER"] = "0";

                ds.Tables["Table"].Rows.Add(dr);
            }

            xmlRet = ds.GetXml();

            logger.Debug("XML: [" + xmlRet + "]");

            return ds;
        }
    }
}
