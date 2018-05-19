using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Configuration;
using log4net;

namespace WSBNPParibas
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
            bformatter.Serialize(stream, trade.AfterHour);
            bformatter.Serialize(stream, trade.Brokeragem);
            bformatter.Serialize(stream, trade.NumOrdem);
            bformatter.Serialize(stream, trade.CnpjCliente);
            bformatter.Serialize(stream, trade.CodCorretoraBMFDestino);
            bformatter.Serialize(stream, trade.CodCorretoraBMFOrigem);
            bformatter.Serialize(stream, trade.DataOperacao);
            bformatter.Serialize(stream, trade.DataVencimento);
            bformatter.Serialize(stream, trade.Delta);
            bformatter.Serialize(stream, trade.ExercicioOpcaoBMF);
            bformatter.Serialize(stream, trade.Externa);
            bformatter.Serialize(stream, trade.LeilaoTermo);
            bformatter.Serialize(stream, trade.NomeAtivo);
            bformatter.Serialize(stream, trade.NomeCliente);
            bformatter.Serialize(stream, trade.NomeCorretoraBMFDestino);
            bformatter.Serialize(stream, trade.NomeCorretoraBMFOrigem);
            bformatter.Serialize(stream, trade.NomeOperador);
            bformatter.Serialize(stream, trade.NumContaCliente);
            bformatter.Serialize(stream, trade.Observacao);
            bformatter.Serialize(stream, trade.Preco);
            bformatter.Serialize(stream, trade.Quantidade);
            bformatter.Serialize(stream, trade.Sequencial);
            bformatter.Serialize(stream, trade.ValorFuturoCurto);
            bformatter.Serialize(stream, trade.ValorFuturoLongo);
            bformatter.Serialize(stream, trade.RecordType);
            bformatter.Serialize(stream, trade.TipoOperacao);
            bformatter.Serialize(stream, trade.TraderID);
            bformatter.Serialize(stream, trade.TraderName);

            //bformatter.Serialize(stream, trade.TradeDetails.Count);

            //if (trade.TradeDetails.Count > 0)
            //{
            //    foreach (TradeDetail detail in trade.TradeDetails)
            //    {
            //        bformatter.Serialize(stream, detail.MarketID);
            //        bformatter.Serialize(stream, detail.NumberOfContracts);
            //        bformatter.Serialize(stream, detail.Price);
            //        bformatter.Serialize(stream, detail.Rate);
            //        bformatter.Serialize(stream, detail.Type);
            //    }
            //}
        }


        public Dictionary<long, Trade> LoadTrades()
        {
            Stream stream = null;
            string path = ConfigurationManager.AppSettings["TradeCacheFile"].ToString();
            Dictionary<long, Trade> trades = new Dictionary<long, Trade>();

            try
            {

                stream = File.Open(path, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();

                int tradeCount = (int)bformatter.Deserialize(stream);

                logger.Info("Carregando " + tradeCount + " trades");

                for (int i = 0; i < tradeCount; i++)
                {
                    Trade trade = _loadTrade(bformatter, stream);

                    if (trade.DataOperacao.Date.Equals(DateTime.Now.Date))
                    {
                        trades.Add(trade.Sequencial, trade);
                    }
                    else
                    {
                        logger.WarnFormat("Descartando trade [{0}] Prd [{1}] Ser [{2}] Qtde [{3}] Prc[{4}] marcado para remocao",
                            trade.Sequencial,
                            trade.NomeAtivo,
                            trade.Externa,
                            trade.Quantidade,
                            trade.Preco);
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

            trade.AfterHour = (string)bformatter.Deserialize(stream);
            trade.Brokeragem = (string)bformatter.Deserialize(stream);
            trade.NumOrdem = (long)bformatter.Deserialize(stream);
            trade.CnpjCliente = (string)bformatter.Deserialize(stream);
            trade.CodCorretoraBMFDestino = (long)bformatter.Deserialize(stream);
            trade.CodCorretoraBMFOrigem = (long)bformatter.Deserialize(stream);
            trade.DataOperacao = (DateTime)bformatter.Deserialize(stream);
            trade.DataVencimento = (DateTime)bformatter.Deserialize(stream);
            trade.Delta = (string)bformatter.Deserialize(stream);
            trade.ExercicioOpcaoBMF = (string)bformatter.Deserialize(stream);
            trade.Externa = (string)bformatter.Deserialize(stream);
            trade.LeilaoTermo = (string)bformatter.Deserialize(stream);
            trade.NomeAtivo = (string)bformatter.Deserialize(stream);
            trade.NomeCliente = (string)bformatter.Deserialize(stream);
            trade.NomeCorretoraBMFDestino = (string)bformatter.Deserialize(stream);
            trade.NomeCorretoraBMFOrigem = (string)bformatter.Deserialize(stream);
            trade.NomeOperador = (string)bformatter.Deserialize(stream);
            trade.NumContaCliente = (string)bformatter.Deserialize(stream);
            trade.Observacao = (string)bformatter.Deserialize(stream);
            trade.Preco = (Decimal)bformatter.Deserialize(stream);
            trade.Quantidade = (long)bformatter.Deserialize(stream);
            trade.Sequencial = (long)bformatter.Deserialize(stream);
            trade.ValorFuturoCurto = (string)bformatter.Deserialize(stream);
            trade.ValorFuturoLongo = (string)bformatter.Deserialize(stream);
            trade.RecordType = (string)bformatter.Deserialize(stream);
            trade.TipoOperacao = (string)bformatter.Deserialize(stream);
            trade.TraderID = (string)bformatter.Deserialize(stream);
            trade.TraderName = (string)bformatter.Deserialize(stream);
            //int numtradedetails = (int)bformatter.Deserialize(stream);

            //if (numtradedetails > 0)
            //{
            //    for (int i = 0; i < numtradedetails; i++)
            //    {
            //        TradeDetail detail = new TradeDetail();

            //        detail.MarketID = (string)bformatter.Deserialize(stream);
            //        detail.NumberOfContracts = (int)bformatter.Deserialize(stream);
            //        detail.Price = (Decimal)bformatter.Deserialize(stream);
            //        detail.Rate = (Decimal)bformatter.Deserialize(stream);
            //        detail.Type = (string)bformatter.Deserialize(stream);

            //        trade.TradeDetails.Add(detail);
            //    }
            //}

            return trade;
        }

    }
}
