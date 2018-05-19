using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using  Gradual.Monitores.Risco.Lib;
using Gradual.Monitores.Risco.Enum;

namespace Gradual.Monitores.Risco.Info
{
    [Serializable]
    [DataContract]
    public class MonitorPLDRequest
    {
        [DataMember]
        public EnumStatusPLD EnumStatusPLD { set; get; }

        [DataMember]
        public string Instrumento { set; get; }

        [DataMember]
        public int NumeroNegocio { set; get; }

    }
}
