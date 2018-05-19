using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{
    [Serializable]
    public class LimitesInfo
    {
        [DataMember]
        public decimal LimiteAVista { set; get; }
        [DataMember]
        public decimal LimiteOpcoes { set; get; }
        [DataMember]
        public decimal LimiteTotal { set; get; }
        [DataMember]
        public decimal LimiteDisponivel { set; get; }
    }
}
