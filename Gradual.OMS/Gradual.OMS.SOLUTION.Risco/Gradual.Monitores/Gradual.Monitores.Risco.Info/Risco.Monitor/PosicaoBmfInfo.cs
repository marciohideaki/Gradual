using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{
    [Serializable]
    public class PosicaoBmfInfo
    {
        [DataMember]
        public int Cliente { set; get; }
        [DataMember]
        public string Sentido { set; get; }
        [DataMember]
        public string Contrato { set; get; }
        [DataMember]
        public int QuantidadeContato { set; get; }
        [DataMember]
        public decimal FatorMultiplicador { set; get; }
        [DataMember]
        public decimal PrecoAquisicaoContrato { set; get; }
        [DataMember]
        public decimal PrecoContatoMercado    { set; get; }
        [DataMember]
        public decimal DiferencialPontos      { set; get; }
        [DataMember]
        public decimal LucroPrejuizoContrato  { set; get; }
        [DataMember]
        public DateTime DataOperacao { set; get; }

    }
}
