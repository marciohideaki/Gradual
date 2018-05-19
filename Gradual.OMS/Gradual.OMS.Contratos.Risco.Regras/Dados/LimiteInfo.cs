using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Risco.Regras
{
    /// <summary>
    /// Contem informações sobre um limite.
    /// O limite mencionado pode ser um limite financeiro ou quantitativo.
    /// Pode fazer referencia, por exemplo, a um emprestimo, a um limite superior de saldo, a um limite inferior de custodia, etc.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class LimiteInfo
    {
        /// <summary>
        /// Indica se no momento de consultar as posições de custódia, deve considerar o ativo,
        /// independente de onde o elemento foi pego.
        /// </summary>
        public bool IncluirOAtivo { get; set; }

        /// <summary>
        /// Limite por operacao de quantidade superior
        /// </summary>
        [Category("Informações de Operação")]
        public double? LimiteQuantidadeOperacaoSuperior { get; set; }

        /// <summary>
        /// Limite por operacao de quantidade inferior
        /// </summary>
        [Category("Informações de Operação")]
        public double? LimiteQuantidadeOperacaoInferior { get; set; }

        /// <summary>
        /// Limite por operacao de valor superior
        /// </summary>
        [Category("Informações de Operação")]
        public double? LimiteValorOperacaoSuperior { get; set; }

        /// <summary>
        /// Limite por operacao de valor inferior
        /// </summary>
        [Category("Informações de Operação")]
        public double? LimiteValorOperacaoInferior { get; set; }

        /// <summary>
        /// Limite superior de valor em custódia
        /// </summary>
        [Category("Informações de Custódia")]
        public double? LimiteValorCustodiaSuperior { get; set; }

        /// <summary>
        /// Limite inferior de valor em custódia
        /// </summary>
        [Category("Informações de Custódia")]
        public double? LimiteValorCustodiaInferior { get; set; }

        /// <summary>
        /// Limite superior de quantidade em custódia
        /// </summary>
        [Category("Informações de Custódia")]
        public double? LimiteQuantidadeCustodiaSuperior { get; set; }

        /// <summary>
        /// Limite inferior de quantidade em custódia
        /// </summary>
        [Category("Informações de Custódia")]
        public double? LimiteQuantidadeCustodiaInferior { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public LimiteInfo()
        {
        }
    }
}
