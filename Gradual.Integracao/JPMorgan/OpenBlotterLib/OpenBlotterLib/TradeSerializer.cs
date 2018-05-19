using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using log4net;
using System.Runtime.Serialization.Formatters.Binary;

namespace OpenBlotterLib
{
    public class TradeSerializer
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void SaveTrades(List<Trade> trades)
        {
            Stream stream = null;
            string path = ConfigurationManager.AppSettings["TradeCacheFile"].ToString();

            try
            {
                logger.Info("Salvando " + trades.Count + " trades.");

                stream = File.Open(path, FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();

                bformatter.Serialize(stream, trades.Count);

                foreach (Trade trade in trades)
                {
                    _serializeTrade(bformatter, stream, trade);
                }

                stream.Close();
                stream = null;
            }
            catch (Exception ex)
            {
                logger.Error("SaveTrades(): " + ex.Message, ex);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        private void _serializeTrade(BinaryFormatter bformatter, Stream stream, Trade trade)
        {
            bformatter.Serialize(stream, trade.AfterHours);
            bformatter.Serialize(stream, trade.BrokerCode);
            bformatter.Serialize(stream, trade.BrokerName);
            bformatter.Serialize(stream, trade.BrokerType);
            bformatter.Serialize(stream, trade.BuyerCode);
            bformatter.Serialize(stream, trade.BuyerName);
            bformatter.Serialize(stream, trade.DMASource);
            bformatter.Serialize(stream, trade.DMATrade);
            bformatter.Serialize(stream, trade.DMATradeID);
            bformatter.Serialize(stream, trade.HedgeLongMaturity);
            bformatter.Serialize(stream, trade.MarketID);
            bformatter.Serialize(stream, trade.MaturityDate);
            bformatter.Serialize(stream, trade.NegotiationChannel);
            bformatter.Serialize(stream, trade.NumberOfContracts);
            bformatter.Serialize(stream, trade.OptionType);
            bformatter.Serialize(stream, trade.Orientation);
            bformatter.Serialize(stream, trade.Papel);
            bformatter.Serialize(stream, trade.Price);
            bformatter.Serialize(stream, trade.ProductID);
            bformatter.Serialize(stream, trade.RecordType);
            bformatter.Serialize(stream, trade.SellerCode);
            bformatter.Serialize(stream, trade.SellerName);
            bformatter.Serialize(stream, trade.SequentialNumber);
            bformatter.Serialize(stream, trade.Serie);
            bformatter.Serialize(stream, trade.TradeDate);
            bformatter.Serialize(stream, trade.TradeID);
            bformatter.Serialize(stream, trade.TraderCode);
            bformatter.Serialize(stream, trade.TraderName);
            bformatter.Serialize(stream, trade.TradeTimestamp);
            bformatter.Serialize(stream, trade.ValDelta);
            bformatter.Serialize(stream, trade.ValReference1);
            bformatter.Serialize(stream, trade.ValReference2);

            bformatter.Serialize(stream, trade.TradeDetails.Count);

            if (trade.TradeDetails.Count > 0)
            {
                foreach (TradeDetail detail in trade.TradeDetails)
                {
                    bformatter.Serialize(stream, detail.MarketID);
                    bformatter.Serialize(stream, detail.NumberOfContracts);
                    bformatter.Serialize(stream, detail.Price);
                    bformatter.Serialize(stream, detail.Rate);
                    bformatter.Serialize(stream, detail.Type);
                }
            }
        }


        public  Dictionary<string, Trade> LoadTrades(DateTime lastSnapshot)
        {
            Stream stream = null;
            string path = ConfigurationManager.AppSettings["TradeCacheFile"].ToString();
            Dictionary<string, Trade> trades = new Dictionary<string, Trade>();

            try
            {

                stream = File.Open(path, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();

                int tradeCount = (int) bformatter.Deserialize(stream);

                logger.Info("Carregando " + tradeCount + " trades");

                for (int i = 0; i < tradeCount; i++)
                {
                    Trade trade = _loadTrade(bformatter, stream);

                    if (trade.TradeTimestamp.CompareTo(lastSnapshot) >=0 )
                    {
                        trades.Add(trade.TradeID, trade);
                    }
                    else
                    {
                        logger.WarnFormat("Descartando trade [{0}] Prd [{1}] Ser [{2}] Qtde [{3}] Prc[{4}] marcado para remocao do cache",
                            trade.TradeID,
                            trade.ProductID,
                            trade.Serie,
                            trade.NumberOfContracts,
                            trade.Price);
                    }
                }

                logger.Info("Trades carregados");

                stream.Close();
                stream = null;
            }
            catch (Exception ex)
            {
                logger.Error("LoadTrades(): " + ex.Message, ex);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return trades;
        }

        private Trade _loadTrade(BinaryFormatter bformatter, Stream stream)
        {
            Trade trade = new Trade();

            trade.AfterHours = (bool) bformatter.Deserialize(stream);
            trade.BrokerCode = (string) bformatter.Deserialize(stream);
            trade.BrokerName = (string) bformatter.Deserialize(stream);
            trade.BrokerType = (string) bformatter.Deserialize(stream);
            trade.BuyerCode = (string) bformatter.Deserialize(stream);
            trade.BuyerName = (string) bformatter.Deserialize(stream);
            trade.DMASource = (string) bformatter.Deserialize(stream);
            trade.DMATrade = (string) bformatter.Deserialize(stream);
            trade.DMATradeID = (string) bformatter.Deserialize(stream);
            trade.HedgeLongMaturity = (DateTime) bformatter.Deserialize(stream);
            trade.MarketID = (string) bformatter.Deserialize(stream);
            trade.MaturityDate = (DateTime) bformatter.Deserialize(stream);
            trade.NegotiationChannel = (string) bformatter.Deserialize(stream);
            trade.NumberOfContracts = (int) bformatter.Deserialize(stream);
            trade.OptionType = (string) bformatter.Deserialize(stream);
            trade.Orientation = (string) bformatter.Deserialize(stream);
            trade.Papel = (string) bformatter.Deserialize(stream);
            trade.Price = (Decimal) bformatter.Deserialize(stream);
            trade.ProductID = (string) bformatter.Deserialize(stream);
            trade.RecordType = (string) bformatter.Deserialize(stream);
            trade.SellerCode = (string) bformatter.Deserialize(stream);
            trade.SellerName = (string) bformatter.Deserialize(stream);
            trade.SequentialNumber = (string) bformatter.Deserialize(stream);
            trade.Serie = (string) bformatter.Deserialize(stream);
            trade.TradeDate = (DateTime) bformatter.Deserialize(stream);
            trade.TradeID = (string) bformatter.Deserialize(stream);
            trade.TraderCode = (string) bformatter.Deserialize(stream);
            trade.TraderName = (string) bformatter.Deserialize(stream);
            trade.TradeTimestamp = (DateTime) bformatter.Deserialize(stream);
            trade.ValDelta = (Decimal) bformatter.Deserialize(stream);
            trade.ValReference1 = (Decimal) bformatter.Deserialize(stream);
            trade.ValReference2 = (Decimal) bformatter.Deserialize(stream);

            int numtradedetails = (int)bformatter.Deserialize(stream);

            if (numtradedetails > 0)
            {
                for( int i=0; i < numtradedetails; i++)
                {
                    TradeDetail detail = new TradeDetail();

                    detail.MarketID = (string) bformatter.Deserialize(stream);
                    detail.NumberOfContracts = (int) bformatter.Deserialize(stream);
                    detail.Price = (Decimal)bformatter.Deserialize(stream);
                    detail.Rate = (Decimal)bformatter.Deserialize(stream);
                    detail.Type = (string)bformatter.Deserialize(stream);

                    trade.TradeDetails.Add(detail);
                }
            }

            return trade;
        }

        public void GravarXMLRemovidos(List<Trade> trades)
        {
            string path = ConfigurationManager.AppSettings["RemovedTradeDir"].ToString();

            string arquivo = String.Format(@"{0}\DLTrades-{1}.xml", path, DateTime.Now.ToString("yyyyMMdd"));

            try
            {
                logger.Info("Salvando " + trades.Count + " trades removidos [" + arquivo + "]");

                string xml = "";

                foreach(Trade trade in trades)
                {
                    xml += trade.ToXML();
                }

                File.WriteAllText( arquivo, xml);
            }
            catch (Exception ex)
            {
                logger.Error("GravarXMLRemovidos(): " + ex.Message, ex);
            }
        }

    }
}
