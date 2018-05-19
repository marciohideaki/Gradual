using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.Monitores.Risco.Lib;
using Gradual.Monitores.Risco.Enum;

namespace Gradual.Monitores.Risco.Info
{
    [Serializable]
    [DataContract]
    public class MonitorLucroPrejuizoRequest
    {        
        [DataMember]
        public int Assessor { set; get; }
        [DataMember]
        public int Cliente { set; get; }
        [DataMember]
        public EnumSemaforo Semaforo { set; get; }
        [DataMember]
        public EnumProporcaoPrejuiso ProporcaoPrejuiso { set; get; }
    }
}
