using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Risco.Regras.Dados
{
    /// <summary>
    /// Contem informações sobre um limite.
    /// O limite mencionado pode ser um limite financeiro ou quantitativo.
    /// Pode fazer referencia, por exemplo, a um emprestimo, a um limite superior de saldo, a um limite inferior de custodia, etc.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class EmprestimoInfo
    {
        /// <summary>
        /// Valor do emprestimo concedido
        /// </summary>
        public double ValorEmprestimo { get; set; }

        /// <summary>
        /// Data de vencimento do empréstimo
        /// </summary>
        public DateTime? DataVencimento { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public EmprestimoInfo()
        {
        }
    }
}
