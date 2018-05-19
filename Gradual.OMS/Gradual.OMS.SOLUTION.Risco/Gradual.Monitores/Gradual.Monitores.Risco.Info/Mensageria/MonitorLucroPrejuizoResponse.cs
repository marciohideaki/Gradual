using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Monitores.Risco.Lib;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Info
{
    [DataContract]
    [Serializable]
    public class MonitorLucroPrejuizoResponse
    {
       [DataMember]
        public List<ExposicaoClienteInfo> Monitor = new List<ExposicaoClienteInfo>();
    }
}
