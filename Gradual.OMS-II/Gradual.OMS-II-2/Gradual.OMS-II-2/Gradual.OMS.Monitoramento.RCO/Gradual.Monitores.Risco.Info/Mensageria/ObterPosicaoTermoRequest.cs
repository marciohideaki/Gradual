using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{
    /// <summary>
    /// Classe de Request de posição de Termo
    /// </summary>
    [DataContract]
    [Serializable]
    public class ObterPosicaoTermoRequest
    {
        /// <summary>
        /// Código de Cliente
        /// </summary>
        [DataMember]
        public int CodigoCliente { get; set; }
    }
}
