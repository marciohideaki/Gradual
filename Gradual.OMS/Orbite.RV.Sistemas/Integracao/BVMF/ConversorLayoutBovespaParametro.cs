using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Sistemas.Integracao.BVMF
{
    /// <summary>
    /// Parametros do layout
    /// </summary>
    [Serializable]
    public class ConversorLayoutBovespaParametro
    {
        /// <summary>
        /// Posição inicial da string que indica o tipo da linha nos layouts bovespa.
        /// Geralmente essa posição é no início de cada linha.
        /// </summary>
        public int TipoLinhaPosicaoInicial { get; set; }

        /// <summary>
        /// Quantidade de caracteres que indica o tipo da linha nos layouts bovespa.
        /// Geralmente são 2 caracteres.
        /// </summary>
        public int TipoLinhaQuantidade { get; set; }
    }
}
