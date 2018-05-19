using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockMarket.Excel2007.Classes
{
    [Serializable()]
    public class PosicaoNetInfo
    {
        /// <summary>
        /// Código do cliente logado
        /// </summary>
        //public string cliente { get; set; }

        /// <summary>
        /// Ativo negociado
        /// </summary>
        //public string ativo { get; set; }

        /// <summary>
        /// Quantidade Executada de Compra
        /// </summary>
        public double qtdeExecCompra { get; set; }

        /// <summary>
        /// Quantidade Executada de Venda
        /// </summary>
        public double qtdeExecVenda { get; set; }

        /// <summary>
        /// Net da quantidade executada (Compra - Venda)
        /// </summary>
        public double NetExec { get; set; }

        /// <summary>
        /// Preco medio Executado de Compra
        /// </summary>
        public double precoMedioExecCompra { get; set; }

        /// <summary>
        /// Preco medio Executado de Venda
        /// </summary>
        public double precoMedioExecVenda { get; set; }

        /// <summary>
        /// Quantidade Aberta de Compra
        /// </summary>
        public double qtdeAbertCompra { get; set; }

        /// <summary>
        /// Quantidade Aberta de Venda
        /// </summary>
        public double qtdeAbertVenda { get; set; }

        /// <summary>
        /// Net da quantidade aberta (Compra - Venda)
        /// </summary>
        public double NetAbert { get; set; }

        /// <summary>
        /// Preco medio Aberta de Compra
        /// </summary>
        public double precoMedioAbertCompra { get; set; }

        /// <summary>
        /// Preco medio Aberta de Venda
        /// </summary>
        public double precoMedioAbertVenda { get; set; }

        /// <summary>
        /// Volume financeiro Net Executada
        /// </summary>
        public double volumeFinNetExec { get; set; }

        /// <summary>
        /// Volume financeiro Net Aberta
        /// </summary>
        public double volumeFinNetAbert { get; set; }
    }
}
