using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.Monitores.Risco.Lib;

namespace Gradual.Monitores.Risco.Info
{
    [Serializable]
    [DataContract]
    public class MonitorPLDResponse
    {
        [DataMember]
        public List<PLDOperacaoInfo> lstPLD = new List<PLDOperacaoInfo>();
    }
}
