using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Helper para representar um elemento externo ao sistema
    /// fazer a conversão de valores externos para internos e vice-versa
    /// </summary>
    public class RelacaoValorValorInfo<T, U>
    {
        /// <summary>
        /// Representa o valor 1
        /// </summary>
        public T Valor1 { get; set; }

        /// <summary>
        /// Representa o valor 2
        /// </summary>
        public U Valor2 { get; set; }
    }
}
