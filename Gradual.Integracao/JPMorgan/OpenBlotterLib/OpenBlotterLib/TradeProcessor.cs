using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using log4net;

namespace OpenBlotterLib
{
    public class TradeProcessor
    {
        private OpenBlotterConfig _config;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public TradeProcessor()
        {
            _config = GerenciadorConfig.ReceberConfig<OpenBlotterConfig>();
        }

        /// <summary>
        /// Retorna um XML com as informacoes das Transacoes JPMorgan.
        /// </summary>
        /// <param name="lastSnapshot">data e hora inicial dos registros a serem retornados</param>
        /// <returns></returns>
        public string GetTrades(DateTime lastSnapshot)
        {
            string xmlRet = "<trades/>";

            TradeSerializer cache = new TradeSerializer();

            Dictionary<string, Trade> trades = new Dictionary<string, Trade>();

            Dictionary<string, Trade> cachedTrades = cache.LoadTrades(lastSnapshot);
            
            PersistenciaDB db = new PersistenciaDB(_config.ConnectionString);

            trades = db.BuscarTrades(lastSnapshot, _config.ClientIDList);

            // Acrescenta os novos trades na lista a ser cacheada
            foreach (KeyValuePair<string, Trade> item in trades)
            {
                if (cachedTrades.ContainsKey(item.Key))
                {
                    Trade cachedTrade = cachedTrades[item.Key];
                    Trade trade = item.Value;

                    // Marca trade se houve alteracao de preco ou 
                    // numero de contratos
                    if (trade.Price != cachedTrade.Price ||
                        trade.NumberOfContracts != cachedTrade.NumberOfContracts)
                    {
                        cachedTrade.RecordType = "AM";
                        cachedTrade.Price = trade.Price;
                        cachedTrade.NumberOfContracts = trade.NumberOfContracts;
                        cachedTrades[item.Key] = cachedTrade;

                    }
                    else
                        cachedTrades[item.Key] = trade;
                }
                else
                {
                    cachedTrades.Add(item.Key, item.Value);
                }
            }

            // ATP: 2015-06-01
            // Grava os trades removidos em um XML a parte para envio se solicitado. 
            // Nao da pra deixar automatico, erros no Sinacor acabam corrompendo o status
            // enviado ao JP

            // Marca trades removidos / reespecificados para deleção
            List<string> tradesKey = cachedTrades.Keys.ToList();
            List<Trade> deletedTrades = new List<Trade>();
            foreach (string key in tradesKey)
            {
                if (trades.ContainsKey(key) == false)
                {
                    logger.WarnFormat("Trade [{0}] Prd [{1}] Ser [{2}] Qtde [{3}] Prc[{4}] exportado remocao",
                        cachedTrades[key].TradeID,
                        cachedTrades[key].ProductID,
                        cachedTrades[key].Serie,
                        cachedTrades[key].NumberOfContracts,
                        cachedTrades[key].Price);

                    Trade xxx = cachedTrades[key].ClonarObjeto();
                    xxx.RecordType = "DL";

                    deletedTrades.Add(xxx);
                }
            }

            // Grava cache
            cache.SaveTrades(cachedTrades.Values.ToList<Trade>());

            // Grava um XML dos removidos para uso emergencial
            if ( deletedTrades.Count > 0 )
                cache.GravarXMLRemovidos(deletedTrades);


            //] ATP: 2015/05/26
            // Le um XML do arquivo e acrescenta no final para emergencia
            string gambi = null;

            try
            {
                string complementFile = String.Format(@"C:\Temp\ComplementoJP-" + DateTime.Now.ToString("yyyyMMdd") +".xml");

                logger.Info("Procurando arquivo [" + complementFile  + "]");

                if ( System.IO.File.Exists(complementFile) )
                    gambi = System.IO.File.ReadAllText(complementFile);
            }
            catch (Exception ex)
            {
                logger.Error("Erro: " + ex.Message, ex);
            }

            // Gera o XML
            if (cachedTrades != null && cachedTrades.Count > 0 || !String.IsNullOrEmpty(gambi))
            {
                xmlRet = "<trades>";

                if (cachedTrades != null && cachedTrades.Count > 0)
                {
                    foreach (Trade trade in cachedTrades.Values)
                    {
                        xmlRet += trade.ToXML();
                    }
                }

                if (!String.IsNullOrEmpty(gambi))
                {
                    xmlRet += gambi;
                }

                xmlRet += "</trades>";
            }

            return xmlRet;
        }
    }
}
