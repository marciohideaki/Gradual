using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using log4net;
using System.Configuration;

namespace WSZarathustra
{
    public class TradeProcessor
    {
        private string clientIDList;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Retorna um XML com as informacoes das Transacoes JPMorgan.
        /// </summary>
        /// <param name="lastSnapshot">data e hora inicial dos registros a serem retornados</param>
        /// <returns></returns>
        public List<traderBean> GetTrades(DateTime lastSnapshot)
        {
            List<traderBean> beans = new List<traderBean>();

            if (ConfigurationManager.AppSettings["PathSqlFile"] == null)
            {
                logger.Fatal("Path do arquivo com a query nao foi definida");
                return null;
            }

            string pathSqlFile = ConfigurationManager.AppSettings["PathSqlFile"].ToString();

            string xmlRet = "<list/>";

            TradeSerializer cache = new TradeSerializer();

            Dictionary<string, Trade> trades = new Dictionary<string, Trade>();

            Dictionary<string, Trade> cachedTrades = cache.LoadTrades();
            
            PersistenciaDB db = new PersistenciaDB(ConfigurationManager.ConnectionStrings["SINACOR"].ConnectionString);

            trades = db.BuscarTrades(lastSnapshot, pathSqlFile);

            cachedTrades.Clear();
            foreach( KeyValuePair<string, Trade> item in trades )
            {
                cachedTrades.Add(item.Key, item.Value);
            }

            // Grava
            cache.SaveTrades(cachedTrades.Values.ToList<Trade>());

            // Gera o XML
            if (cachedTrades != null && cachedTrades.Count > 0)
            {
                foreach (Trade trade in cachedTrades.Values)
                {
                    beans.Add(trade.ToBean());
                }
            }

            return beans;
        }
    }
}
