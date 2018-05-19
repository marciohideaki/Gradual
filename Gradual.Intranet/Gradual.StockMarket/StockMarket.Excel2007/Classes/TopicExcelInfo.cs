using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockMarket.Excel2007.Classes
{
    [Serializable()]
    public class TopicExcelInfo
    {
        /// <summary>
        /// funcao informada na celula do Excel
        /// </summary>
        public string funcao { get; set; }

        /// <summary>
        /// Cliente informado para obter do acompanhamento de ordens
        /// </summary>
        public string cliente { get; set; }

        /// <summary>
        /// Ativo informado para assinatura de cotacao ou oferta
        /// </summary>
        public string ativo { get; set; }

        /// <summary>
        /// Propriedade associada ao ativo
        /// </summary>
        public string propriedade { get; set; }

        /// <summary>
        /// Sentido (compra ou venda) da propriedade (usado no Livro de Ofertas)
        /// </summary>
        public string sentido { get; set; }

        /// <summary>
        /// Posicao no livro de ofertas
        /// </summary>
        public int ocorrencia { get; set; }

        /// <summary>
        /// Ultimo valor atualizado (apenas atualiza o Excel se o valor novo for diferente do valor anterior)
        /// </summary>
        public object valorAnterior { get; set; }
    }
}
