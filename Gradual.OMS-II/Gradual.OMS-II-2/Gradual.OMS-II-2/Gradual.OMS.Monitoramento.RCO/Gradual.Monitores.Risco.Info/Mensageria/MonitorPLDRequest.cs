using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using  Gradual.Monitores.Risco.Lib;
using Gradual.Monitores.Risco.Enum;

namespace Gradual.Monitores.Risco.Info
{
    /// <summary>
    /// Classe de Monitor de PLD de Request
    /// </summary>
    [Serializable]
    [DataContract]
    public class MonitorPLDRequest
    {
        /// <summary>
        /// Enumerado com status PLD
        /// </summary>
        [DataMember]
        public EnumStatusPLD EnumStatusPLD { set; get; }

        /// <summary>
        /// Instrumento para efetuar filtro
        /// </summary>
        [DataMember]
        public string Instrumento { set; get; }

        /// <summary>
        /// Número do negócio para efetuar o filtro
        /// </summary>
        [DataMember]
        public int NumeroNegocio { set; get; }

    }
}
