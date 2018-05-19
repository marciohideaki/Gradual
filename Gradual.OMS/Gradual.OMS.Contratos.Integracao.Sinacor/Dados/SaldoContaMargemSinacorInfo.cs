using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.Dados
{
    /// <summary>
    /// Contem informações de conta margem de um cliente no sinacor
    /// </summary>
    [Serializable]
    public class SaldoContaMargemSinacorInfo
    {
        /// <summary>
        /// Código cblc do cliente
        /// </summary>
        public string CodigoClienteCBLC { get; set; }

        /// <summary>
        /// Data referencia do saldo, ou a data do pregão considerado
        /// </summary>
        public DateTime DataReferencia { get; set; }

        /// <summary>
        /// Valor do limite
        /// </summary>
        public double ValorLimite { get; set; }

        /// <summary>
        /// Valor de depósito em conta corrente
        /// </summary>
        public double ValorDepositoContaCorrente { get; set; }

        /// <summary>
        /// Valor Financiado
        /// </summary>
        public double ValorFinanciado { get; set; }

        /// <summary>
        /// Valor do IOF
        /// </summary>
        public double ValorIOF { get; set; }
    }
}
