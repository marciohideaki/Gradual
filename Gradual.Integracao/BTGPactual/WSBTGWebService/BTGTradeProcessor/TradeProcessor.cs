using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Configuration;
using System.Data;

namespace BTGTradeProcessor
{
    public class TradeProcessor
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string GetTrades(long lastSequence)
        {
            string xmlRet = "<TRADES/>";

            TradeSerializer cache = new TradeSerializer();

            Dictionary<long, Trade> trades = new Dictionary<long, Trade>();

            Dictionary<long, Trade> cachedTrades = cache.LoadTrades();

            string datanegos = DateTime.Now.ToString("yyyy/MM/dd");
            datanegos += " 00:00:00";

            if (ConfigurationManager.AppSettings["DataNegosTeste"] != null)
            {
                datanegos = ConfigurationManager.AppSettings["DataNegosTeste"].ToString();
            }

            PersistenciaDB db = new PersistenciaDB(ConfigurationManager.ConnectionStrings["SINACOR"].ToString());

            trades = db.BuscarTrades(datanegos,lastSequence, ConfigurationManager.AppSettings["ClientList"].ToString());

            if (trades != null)
            {
                // Acrescenta os novos trades na lista a ser cacheada
                foreach (KeyValuePair<long, Trade> item in trades)
                {
                    if (cachedTrades.ContainsKey(item.Key))
                    {
                        Trade cachedTrade = cachedTrades[item.Key];
                        Trade trade = item.Value;

                        // Marca trade se houve alteracao de preco ou 
                        // numero de contratos
                        if (trade.Preco != cachedTrade.Preco ||
                            trade.Quantidade != cachedTrade.Quantidade)
                        {
                            cachedTrade.TipoBoletagem = "AM";
                            cachedTrades[item.Key] = cachedTrade;
                        }
                    }
                    else
                    {
                        cachedTrades.Add(item.Key, item.Value);
                    }
                }
            }

            lock (cachedTrades)
            {
                // Marca trades removidos / reespecificados para deleção
                foreach (KeyValuePair<long, Trade> item in cachedTrades)
                {
                    if (trades == null || trades.ContainsKey(item.Key) == false)
                    {
                        logger.WarnFormat("Trade [{0}] Prd [{1}] Ser [{2}] Qtde [{3}] Prc[{4}] marcado para remocao",
                            item.Value.Sequencial,
                            item.Value.NomeAtivo,
                            item.Value.Brokeragem,
                            item.Value.Quantidade,
                            item.Value.Preco);
                        //item.Value.RecordType = "DL"; // Marca para delecao

                        cachedTrades[item.Key] = item.Value;
                    }
                }
            }

            // Grava
            cache.SaveTrades(cachedTrades.Values.ToList<Trade>());

            // Gera o XML
            //if (cachedTrades != null && cachedTrades.Count > 0)
            //{
            //    xmlRet = "<TRADES>";

            //    foreach (Trade trade in cachedTrades.Values)
            //    {
            //        xmlRet += trade.ToXML();
            //    }

            //    xmlRet += "</TRADES>";
            //}

            NewDataSet ds = new NewDataSet(); //Auto-Generated Class is named as NewDataSet

            foreach(Trade trade in cachedTrades.Values)
            {
                DataRow dr = ds.Tables["Table"].NewRow(); //DataRow to be added to DataSet

                dr["SEQUENCIAL"] = trade.Sequencial;
                dr["TIPO_BOLETAGEM"] = trade.TipoBoletagem;
                dr["CHAVE_ESTRANGEIRA"] = trade.ChaveEstrangeira;
                dr["NOME_OPERADOR"] = trade.NomeOperador;
                dr["DATA_OPERACAO"] = trade.DataOperacao;
                dr["NOME_ATIVO"] = trade.NomeAtivo;
                dr["TIPO_DE_OPERACAO"] = trade.TipoOperacao;
                dr["QUANTIDADE"] = trade.Quantidade;
                dr["PRECO"] = trade.Preco;
                dr["AFTER_HOUR"] = trade.AfterHour;
                dr["COD_CORRETORA_BMF_ORIGEM"] = trade.CodCorretoraBMFOrigem;
                dr["NOME_CORRETORA_ORIGEM"] = trade.NomeCorretoraBMFOrigem;
                dr["EXERCICIO_OPCAO_BMF"] = trade.ExercicioOpcaoBMF;
                dr["CNPJ_CLIENTE"] = trade.CnpjCliente;
                dr["NOME_CLIENTE"] = trade.NomeCliente;
                dr["EXTERNA"] = trade.Externa;
                dr["BROKERAGEM"] = trade.Brokeragem;
                dr["COD_CORRETORA_BMF_DESTINO"] = trade.CodCorretoraBMFDestino;
                dr["NOME_CORRETORA_DESTTINO"] = trade.NomeCorretoraBMFDestino;
                dr["LEILAO_TERMO"] = trade.LeilaoTermo;
                dr["DATA_TERMO"] = trade.DataTermo;
                dr["TRADERID"] = trade.TraderID;
                dr["TRADERNAME"] = trade.TraderName;
                dr["NUM_CONTA_CLIENTE"] = trade.NumContaCliente;
                dr["OBSERVACAO"] = "";
                dr["DELTA"] = trade.Delta;
                dr["TX_x0020_JUROS_x0020_LONGO"] = trade.TaxaJurosLongo;
                dr["TX_x0020_JUROS_x0020_CURTO"] = trade.TaxaJurosCurto;

                ds.Tables["Table"].Rows.Add(dr);
            }

            xmlRet = ds.GetXml();
            return xmlRet;
        }

        public NewDataSet GetTradesDataset(long lastSequence)
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


            NewDataSet ds = new NewDataSet(); //Auto-Generated Class is named as NewDataSet

            foreach (Trade trade in trades.Values)
            {
                DataRow dr = ds.Tables["Table"].NewRow(); //DataRow to be added to DataSet

                dr["SEQUENCIAL"] = trade.Sequencial;
                dr["TIPO_BOLETAGEM"] = trade.TipoBoletagem;
                dr["CHAVE_ESTRANGEIRA"] = trade.ChaveEstrangeira;
                dr["NOME_OPERADOR"] = trade.NomeOperador;
                dr["DATA_OPERACAO"] = trade.DataOperacao;
                dr["NOME_ATIVO"] = trade.NomeAtivo;
                dr["TIPO_DE_OPERACAO"] = trade.TipoOperacao;
                dr["QUANTIDADE"] = trade.Quantidade;
                dr["PRECO"] = trade.Preco;
                dr["AFTER_HOUR"] = trade.AfterHour;
                dr["COD_CORRETORA_BMF_ORIGEM"] = trade.CodCorretoraBMFOrigem;
                dr["NOME_CORRETORA_ORIGEM"] = trade.NomeCorretoraBMFOrigem;
                dr["EXERCICIO_OPCAO_BMF"] = trade.ExercicioOpcaoBMF;
                dr["CNPJ_CLIENTE"] = trade.CnpjCliente;
                dr["NOME_CLIENTE"] = trade.NomeCliente;
                dr["EXTERNA"] = trade.Externa;
                dr["BROKERAGEM"] = trade.Brokeragem;
                dr["COD_CORRETORA_BMF_DESTINO"] = trade.CodCorretoraBMFDestino;
                dr["NOME_CORRETORA_DESTTINO"] = trade.NomeCorretoraBMFDestino;
                dr["LEILAO_TERMO"] = trade.LeilaoTermo;
                dr["DATA_TERMO"] = trade.DataTermo;
                dr["TRADERID"] = trade.TraderID;
                dr["TRADERNAME"] = trade.TraderName;
                dr["NUM_CONTA_CLIENTE"] = trade.NumContaCliente;
                dr["OBSERVACAO"] = "";
                dr["DELTA"] = trade.Delta;
                dr["TX_x0020_JUROS_x0020_LONGO"] = trade.TaxaJurosLongo;
                dr["TX_x0020_JUROS_x0020_CURTO"] = trade.TaxaJurosCurto;

                ds.Tables["Table"].Rows.Add(dr);
            }

            xmlRet = ds.GetXml();
            return ds;
        }
    }
}
