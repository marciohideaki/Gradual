using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{
    [Serializable]
    public class TradesInfo
    {
        [DataMember]
        public int Account { set; get; }
        [DataMember]
        public string Side { set; get; }
        [DataMember]
        public int Quantity { set; get; }
        [DataMember]
        public decimal Price { set; get; }
        [DataMember]
        public string Instrument { set; get; }
    }
}
