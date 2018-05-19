using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.OMS.Mensageria
{
    /// <summary>
    /// Contém informações da crítica relativas à validação.
    /// Basicamente, precisa informar qual regra realizou a crítica.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CriticaValidacaoInfo : CriticaInfo
    {
        /// <summary>
        /// Contem informações sobre a regra que realizou a validação
        /// </summary>
        public RegraInfo RegraInfo { get; set; }
    }
}
