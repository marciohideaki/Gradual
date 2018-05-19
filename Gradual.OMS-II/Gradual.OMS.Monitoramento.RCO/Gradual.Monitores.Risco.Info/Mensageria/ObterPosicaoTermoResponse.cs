using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{
    [DataContract]
    [Serializable]
    public class ObterPosicaoTermoResponse
    {
        [DataMember]
        public List<PosicaoTermoInfo> PosicaoTermo { get; set; }
    }
}
