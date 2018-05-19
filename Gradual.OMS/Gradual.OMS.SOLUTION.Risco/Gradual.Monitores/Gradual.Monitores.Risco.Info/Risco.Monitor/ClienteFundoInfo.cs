using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{

    [Serializable]
    public class ClienteFundoInfo
    {
        [DataMember]
        public int IdCliente { set; get; }
        [DataMember]
        public string NomeFundo { set; get; }
        [DataMember]
        public decimal Saldo { set; get; }
    }
}
