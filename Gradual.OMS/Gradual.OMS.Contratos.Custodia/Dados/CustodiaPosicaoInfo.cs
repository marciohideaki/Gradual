using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Custodia.Dados
{
    /// <summary>
    /// Representa a posição de custódia de um ativo em uma carteira
    /// </summary>
    [Serializable]
    public class CustodiaPosicaoInfo
    {
        /// <summary>
        /// Código do ativo que está com sua posição representada.
        /// Este campo junto com a carteira, seriam a chave primária neste contexto.
        /// </summary>
        public string CodigoAtivo { get; set; }

        /// <summary>
        /// Código da bolsa do ativo. Propriedade auxiliar para evitar
        /// consulta a um cadastro de papeis
        /// </summary>
        public string CodigoBolsa { get; set; }

        /// <summary>
        /// Código da carteira da posição.
        /// </summary>
        public string Carteira { get; set; }

        /// <summary>
        /// Quantidade da posição no momento de abertura
        /// </summary>
        public double QuantidadeAbertura { get; set; }

        /// <summary>
        /// Indica a quantidade comprada no período após a abertura
        /// </summary>
        public double QuantidadeCompra { get; set; }

        /// <summary>
        /// Indica a quantidade vendida no período após a abertura.
        /// </summary>
        public double QuantidadeVenda { get; set; }

        /// <summary>
        /// Contém a quantidade atual, que é o resultado da abertura mais as movimentações 
        /// de compra e venda realizadas no período.
        /// </summary>
        public double QuantidadeAtual { get; set; }

        /// <summary>
        /// Contem a cotação de mercado do ativo da posição.
        /// </summary>
        public double ValorCotacao { get; set; }

        /// <summary>
        /// Indica a data em que a cotação do ativo foi obtida.
        /// </summary>
        public DateTime DataCotacao { get; set; }

        /// <summary>
        /// Indica o valor da posição, que é o produto do ValorCotacao pelo QuantidadeAtual.
        /// </summary>
        public double ValorPosicao 
        {
            get { return this.QuantidadeAtual * this.ValorCotacao; }
        }
    }
}
