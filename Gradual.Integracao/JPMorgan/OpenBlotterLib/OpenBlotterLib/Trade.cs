using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace OpenBlotterLib
{
    [Serializable]
    public class Trade
    {
        private CultureInfo ciPtBR = CultureInfo.CreateSpecificCulture("pt-BR");
        private CultureInfo ciEn = CultureInfo.CreateSpecificCulture("en-US");

        #region Elementos Obrigatorios
        /// <summary>
        /// Tipo da operacao 
        /// NW - New/Nova
        /// DL - Deletion/Cancelamento
        /// AM - Amendment/Alteracao
        /// </summary>
        public string RecordType { get; set; }

        /// <summary>
        /// Data da operacao
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// Indicador de operacao de compra ou venda
        /// B - Buy/Compra
        /// S - Sell/Venda
        /// </summary>
        public string Orientation { get; set; }

        /// <summary>
        /// ID do produto
        /// </summary>
        public string ProductID { get; set; }

        /// <summary>
        /// Identificacao do mercado de negociacao 
        /// FUT - Futuro
        /// OPD - Opção sem disponivel
        /// OPF - Opção sem futuro
        /// TMO - Termo
        /// </summary>
        public string MarketID { get; set; }

        /// <summary>
        /// Identificacao da transação na corretora
        /// </summary>
        public string TradeID { get; set; }

        /// <summary>
        /// Numero sequencial da Ordem
        /// </summary>
        public string SequentialNumber { get; set; }

        /// <summary>
        /// Codigo da série na BM&F
        /// </summary>
        public string Serie { get; set; }

        /// <summary>
        /// Codigo do papel (CD_COMMOD+CD_SERIE)
        /// </summary>
        public string Papel { get; set; }

        /// <summary>
        /// Data de vencimento
        /// </summary>
        public DateTime MaturityDate { get; set; }

        /// <summary>
        /// Data e hora de captura do trade
        /// </summary>
        public DateTime TradeTimestamp { get; set; }

        /// <summary>
        /// Canal de negociacao
        /// </summary>
        public string NegotiationChannel { get; set; }

        /// <summary>
        /// Flag se a operação foi efetuada em aftehour
        /// </summary>
        public bool AfterHours { get; set; }

        /// <summary>
        /// Quantidade de contratos na operacao
        /// </summary>
        public int NumberOfContracts { get; set; }

        /// <summary>
        /// Preco para produtos negociados por preco ou
        /// Taxa para produtos negociados em taxa
        /// </summary>
        public Decimal Price { get; set; }
        #endregion //Elementos Obrigatorios

        #region Contrapartes

        /// <summary>
        /// Codigo comprador na corretora
        /// </summary>
        public string BuyerCode { get; set; }

        /// <summary>
        /// Nome do comprador
        /// </summary>
        public string BuyerName { get; set; }

        /// <summary>
        /// Codigo da corretora
        /// </summary>
        public string BrokerCode { get; set; }

        /// <summary>
        /// Nome da corretora
        /// </summary>
        public string BrokerName { get; set; }

        /// <summary>
        /// Tipo da corretora
        /// </summary>
        public string BrokerType { get; set; }

        /// <summary>
        /// Codigo do vendedor na corretora
        /// </summary>
        public string SellerCode { get; set; }

        /// <summary>
        /// Nome do vendedor
        /// </summary>
        public string SellerName { get; set; }

        /// <summary>
        /// Codigo do trader na corretora
        /// </summary>
        public string TraderCode { get; set; }

        /// <summary>
        /// Nome do trader
        /// </summary>
        public string TraderName { get; set; }
        

        #endregion //Contrapartes

        #region Elementos Opcionais

        /// <summary>
        /// Data de vencimento da Long/Leg em uma ordem com VTF ou 
        /// opção de DI1
        /// </summary>
        public DateTime HedgeLongMaturity { get; set; }

        /// <summary>
        /// Indicador de opção de compra ou venda para os produtos  VID, VTF, VTC
        /// CALL 
        /// PUT
        /// </summary>
        public string OptionType { get; set; }

        /// <summary>
        /// Origem da operacao DMA
        /// </summary>
        public string DMASource { get; set; }

        /// <summary>
        /// Trader responsavel pela operacao DMA
        /// </summary>
        public string DMATrade { get; set; }

        /// <summary>
        /// Identificador da operação na Bloomberg
        /// </summary>
        public string DMATradeID { get; set; }

        /// <summary>
        /// Primeiro valor de referencia para operacoes estruturadas
        /// </summary>
        public Decimal ValReference1 { get; set; }

        /// <summary>
        ///  Segundo valor de referencia para operacoes estruturadas
        /// </summary>
        public Decimal ValReference2 { get; set; }

        /// <summary>
        /// Valor do delta para as operações com delta envolvido
        /// </summary>
        public Decimal ValDelta { get; set; }

        #endregion //Elementos Opcionais

        #region Operacoes Estruturadas
        public List<TradeDetail> TradeDetails { get; set; }
        #endregion //Operacoes Estruturadas

        public Trade()
        {
            AfterHours = false;
            BrokerCode = "";
            BrokerName = "";
            BrokerType = "";
            BuyerCode = "";
            BuyerName = "";
            DMASource = "";
            DMATrade = "";
            DMATradeID = "";
            HedgeLongMaturity = DateTime.MinValue;
            MarketID = "";
            MaturityDate = DateTime.MinValue;
            NegotiationChannel = "";
            NumberOfContracts = 0;
            OptionType = "";
            Orientation = "";
            Papel = "";
            Price = new Decimal(0.0);
            ProductID = "";
            RecordType = "";
            SellerCode = "";
            SellerName = "";
            SequentialNumber = "";
            Serie = "";
            TradeDate = DateTime.MinValue;
            TradeID = "";
            TraderCode = "";
            TraderName = "";
            TradeTimestamp = DateTime.MinValue;
            ValDelta = new Decimal(0.0);
            ValReference1 = new Decimal(0.0);
            ValReference2 = new Decimal(0.0);

            TradeDetails = new List<TradeDetail>();
        }


        /// <summary>
        /// Gera um subset XML completo com as informacoes deste 
        /// </summary>
        /// <returns></returns>
        public string ToXML()
        {
            string response = "";
            string xml = "<trade>";

            xml += "<trade-date>" + TradeDate.ToString("yyyy-MM-dd") + "</trade-date>";
            xml += "<trade-id>" + TradeID + "</trade-id>";

            if ((DMASource!=null && DMASource.Length > 0) && 
                (DMATrade!=null && DMATrade.Length > 0) &&
                (DMATradeID!=null && DMATradeID.Length > 0) ) 
            {
                xml += "<dma>";
                xml += "<dma-source>"+DMASource+"</dma-source>";
                xml += "<dma-trader>"+DMATrade+"</dma-trader>";
                xml += "<dma-trade-id>"+DMATradeID+"</dma-trade-id>";
                xml += "</dma>";
            }
            //else
            //    xml += "<dma/>";

            xml += "<record-type>" + RecordType + "</record-type>";
            xml += "<product-id>" + ProductID + "</product-id>";
            xml += "<market-id>" + MarketID + "</market-id>";
            xml += "<serie>" + Serie + "</serie>";
            xml += "<maturity-date>" + MaturityDate.ToString("yyyy-MM-dd") + "</maturity-date>";
            xml += "<hedge-long-maturity>"+HedgeLongMaturity.ToString("yyyy-MM-dd")+"</hedge-long-maturity>";
            xml += "<trade-timestamp>"+TradeTimestamp.ToString("yyyy-MM-ddTHH:mm:ss")+"</trade-timestamp>";
            //xml += "<trade-timestamp>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "</trade-timestamp>";
            xml += "<negotiation-channel>" + NegotiationChannel + "</negotiation-channel>";
            xml += "<after-hours>" + AfterHours.ToString().ToLowerInvariant() + "</after-hours>";
            xml += "<orientation>" + Orientation + "</orientation>";
            xml += "<number-of-contracts>" + NumberOfContracts + "</number-of-contracts>";
            xml += "<price>" + Price.ToString(ciEn) + "</price>";

            if ( OptionType != null && OptionType.Length > 0 )
                xml += "<option-type>" + OptionType + "</option-type>";

            if ( ValReference1 != 0 )
                xml += "<val-reference-1>" + ValReference1 + "</val-reference-1>";

            if (ValReference2 != 0)
                xml += "<val-reference-2>" + ValReference2 + "</val-reference-2>";

            if (ValDelta != 0)
                xml += "<val-delta>" + ValDelta + "</val-delta>";

            response += xml;

            if (TradeDetails.Count > 0)
            {
                xml = "<additional-details>";
                foreach (TradeDetail detail in TradeDetails)
                {
                    xml += detail.ToXML();
                }
                xml += "</additional-details>";
            }
            else
                xml = "<additional-details/>";

            response += xml;

            xml = "<buyer>";
            xml += "<buyer-code>{0}</buyer-code>";
            xml += "<buyer-name>{1}</buyer-name>";        
            xml += "</buyer>";

            xml += "<broker>";
            xml += "<broker-code>{2}</broker-code>";
            xml += "<broker-name>{3}</broker-name>";
            xml += "</broker>";

            xml += "<seller>";
            xml += "<seller-code>{4}</seller-code>";
            xml += "<seller-name>{5}</seller-name>";
            xml += "</seller>";

            xml += "<trader>";
            xml += "<trader-code>{6}</trader-code>";
            xml += "<trader-name>{7}</trader-name>";
            xml += "</trader>";

            response += string.Format(xml,
                BuyerCode,
                BuyerName,
                BrokerCode,
                BrokerName,
                SellerCode,
                SellerName,
                TraderCode,
                TraderName);

            response += "</trade>";

            return response;
        }
    }
}
