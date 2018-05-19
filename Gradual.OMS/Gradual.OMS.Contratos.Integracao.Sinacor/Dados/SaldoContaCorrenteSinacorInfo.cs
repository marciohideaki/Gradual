using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.Dados
{
    /// <summary>
    /// Representa o saldo de conta corrente no sinacor.
    /// Um cliente pode ter diversos códigos cblc. Por exemplo,
    /// se o cliente possuir conta corrente e conta investimento,
    /// ele terá um código cblc para a conta corrente e outro
    /// para a conta investimento. Geralmente o código cblc da
    /// conta investimento é 9 + cblc da conta corrente.
    /// </summary>
    [Serializable]
    public class SaldoContaCorrenteSinacorInfo
    {
        /// <summary>
        /// Código cblc do cliente
        /// </summary>
        public string CodigoClienteCBLC { get; set; }

        /// <summary>
        /// Data referencia do saldo, ou a data do pregão considerado
        /// ** Está coluna não é retornada pela procedure, mas deveria
        /// ** Por isso está aqui apenas como sugestão
        /// </summary>
        public DateTime DataReferencia { get; set; }

        /// <summary>
        /// Saldo em D0
        /// </summary>
        public double SaldoD0 { get; set; }

        /// <summary>
        /// Saldo em D1
        /// </summary>
        public double SaldoD1 { get; set; }

        /// <summary>
        /// Saldo em D2
        /// </summary>
        public double SaldoD2 { get; set; }

        /// <summary>
        /// Saldo em D3
        /// </summary>
        public double SaldoD3 { get; set; }
    }
}
