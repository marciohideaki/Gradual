using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{
    /// <summary>
    /// Classe de Request de posição de BTC
    /// </summary>
    [DataContract]
    [Serializable]
    public class ObterPosicaoBtcRequest
    {
        /// <summary>
        /// Código do cliente para request de posição de BTC
        /// </summary>
        [DataMember]
        public int CodigoCliente { get; set; }
    }
}
