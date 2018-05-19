using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.CadastroPapeis.Dados
{
    /// <summary>
    /// Papeis negociados na bovespa
    /// </summary>
    [Serializable]
    public class PapelNegociadoBovespaInfo
    {
        #region Properties
        /// <summary>
        /// Código do Papel no sistema ISIN
        /// </summary>
        public string CodISIN { get; set; }

        /// <summary>
        /// Código de Negociação do papel da bovespa
        /// </summary>
        public string CodNegociacao { get; set; }

        /// <summary>
        /// Nome, no pregão, da empresa emitente do papel
        /// </summary>
        public string NomeEmpresa { get; set; }

        /// <summary>
        /// Código do mercado em que o papel está cadastrado
        /// </summary>
        public int TipoMercado { get; set; }

        /// <summary>
        /// Descrição do mercado
        /// </summary>
        public string DescMercado { get; set; }

        /// <summary>
        /// Número de distribuição ex correspondente ao próximo estado direito do papel
        /// </summary>
        public Nullable<int> Dismex { get; set; }

        /// <summary>
        /// Código do setor de atividade da empresa emitente do papel
        /// </summary>
        public string CodSetorAtividade { get; set; }

        /// <summary>
        /// Descrição do setor de atividade da empresa emitente do papel
        /// </summary>
        public string DescSetorAtividade { get; set; }

        /// <summary>
        /// Preço de exercício ou valor de contrato p/ os mercados de opções ou termo secundário, respectivamente
        /// </summary>
        public Nullable<double> PrecoExercicio { get; set; }

        /// <summary>
        /// Data de vencimento para os mercados de opções, termo secundário ou futuro
        /// </summary>
        public Nullable<DateTime> DataVencimento { get; set; }

        /// <summary>
        /// Preço de fechamento do papel no ultimo lançamento em que houve negociação
        /// </summary>
        public double PrecoFechamento { get; set; }

        /// <summary>
        /// Data de fechamento ou data do pregão em que houve a última negociação
        /// </summary>
        public DateTime DataFechamento { get; set; }

        /// <summary>
        /// Preço médio do papel no pregão em que houve negociação
        /// </summary>
        public double PrecoMedio { get; set; }

        /// <summary>
        /// Fator de cotação vigente dos preços
        /// </summary>
        public int FatorCotacao { get; set; }

        /// <summary>
        /// Lote padrão de negociação
        /// </summary>
        public int LoteNegociacao { get; set; }

        /// <summary>
        /// Tipo de Sistema
        /// </summary>
        public string Sistema { get { return "BOVESPA"; } }
        #endregion

        #region Construtors
        public PapelNegociadoBovespaInfo()
        {

        }
        #endregion
    }
}
