using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{
    [Serializable]
    public class CustodiaAberturaInfo
    {
        [DataMember]
        public string Instrumento { set; get; }

        [DataMember]
        public string TipoMercado { set; get; }

        [DataMember]
        public int Quantidade { set; get; }

        [DataMember]
        public int CodigoCliente { set; get; }

        [DataMember]
        public int LoteNegociacao { set; get; }

        [DataMember]
        public int CodigoCarteira { set; get; }

        [DataMember]
        public string CodigoSerie { set; get; }
    }
}
