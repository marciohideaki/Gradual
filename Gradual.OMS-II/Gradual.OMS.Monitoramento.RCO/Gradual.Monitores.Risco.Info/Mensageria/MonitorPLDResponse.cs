using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.Monitores.Risco.Lib;

namespace Gradual.Monitores.Risco.Info
{
    /// <summary>
    /// Classe de Response que PLD (Prevenção de Lavagem de dinheiro)
    /// </summary>
    [Serializable]
    [DataContract]
    public class MonitorPLDResponse
    {
        /// <summary>
        /// Lista de Operações de PLD
        /// </summary>
        [DataMember]
        public List<PLDOperacaoInfo> lstPLD = new List<PLDOperacaoInfo>();
    }
}
