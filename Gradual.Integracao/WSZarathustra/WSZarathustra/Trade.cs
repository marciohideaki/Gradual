using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace WSZarathustra
{
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

        public string HoraNegocio { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        public int RecordNumber { get; set; }

        public string SegmentoBolsa { get; set; }
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
            string xml = "<traderBean>";

            xml += "<ID>" + RecordNumber + "</ID>";
            xml += "<DATA_NEGOCIO>" + TradeDate.ToString("yyyy-MM-dd") + "</DATA_NEGOCIO>";
            xml += "<HORA_NEGOCIO>" + HoraNegocio + "</HORA_NEGOCIO>";
            xml += "<NR_NEGOCIO>" + TradeID + "</NR_NEGOCIO>";
            xml += "<CODNEG>" + Papel + "</CODNEG>";
            xml += "<SERPAP>" + Serie + "</SERPAP>";
            xml += "<OPERACAO>" + Orientation + "</OPERACAO>";
            xml += "<QTDADE>" + NumberOfContracts + "</QTDADE>";
            xml += "<PDENEG>" + Price.ToString(ciEn) + "</PDENEG>";
            xml += "<FC_NOME>" + BuyerName + "</FC_NOME>";
            xml += "<NM_EMIT_ORDEM>" + TraderName  + "/<NM_EMIT_ORDEM>";
            xml += "<MERCADO>" +  SegmentoBolsa + "</MERCADO>";


            response += xml;

            response += "</traderBean>";

            return response;
        }

        public traderBean ToBean()
        {
            traderBean bean = new traderBean();
            
            bean.ID = RecordNumber.ToString();
            bean.DATA_NEGOCIO=TradeDate.ToString("yyyy-MM-dd");
            bean.HORA_NEGOCIO=HoraNegocio;
            bean.NR_NEGOCIO=TradeID;
            bean.CODNEG=Papel;
            bean.SERPAP=Serie;
            bean.OPERACAO=Orientation;
            bean.QTDADE = NumberOfContracts.ToString();
            bean.PDENEG=Price.ToString(ciEn);
            bean.FC_NOME=BuyerName;
            bean.NM_EMIT_ORDEM=TraderName;
            bean.MERCADO = SegmentoBolsa;

            return bean;
        }
    }
}
