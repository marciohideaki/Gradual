using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSZarathustra
{
    public class TradeDetail
    {
        public const string TRADE_DETAIL_TYPE_LONG = "Long";
        public const string TRADE_DETAIL_TYPE_SHORT = "Short";

        public const string TRADE_DETAIL_MKT_FUT = "FUT";

        /// <summary>
        /// Numero de contratos
        /// </summary>
        public int NumberOfContracts { get; set; }

        /// <summary>
        /// PU para produtos negociados em preco
        /// </summary>
        public Decimal Price { get; set; }
        
        /// <summary>
        /// PU para produtos negociados em taxa
        /// </summary>
        public Decimal Rate { get; set; }

        /// <summary>
        /// Long ou Short - obrigatorio para VTF
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Mercado de negociacao do produto
        /// </summary>
        public string MarketID { get; set; }


        /// <summary>
        /// Retorna um XML que representa o detalhe do contrato
        /// </summary>
        /// <returns></returns>
        public string ToXML()
        {
            string xml = "<additional-detail>";

            xml += "<number-of-contracts>{0}</number-of-contracts>";
            xml += "<price>{1}</price>";
            xml += "<rate>{2}</rate>";
            xml += "<type>{3}</type>";
            xml += "<market-id>{4}</market-id>";
            xml += "</additional-detail>";

            string ret = string.Format(xml,
                NumberOfContracts,
                Price,
                Rate,
                Type,
                MarketID);

            return ret;
        }

    }
}
