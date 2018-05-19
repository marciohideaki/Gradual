using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{
    [Serializable]
    [DataContract]
    public class LucroPrejuisoPLDInfo
    {
        [DataMember]
        public string  Instrumento    { set; get; }
        [DataMember]
        public string  Sentido        { set; get; }
        [DataMember]
        public int     Seq            { set; get; }
        [DataMember]
        public decimal PrecoNegocio   { set; get; }
        [DataMember]
        public decimal QtdeNegocio    { set; get; }
        [DataMember]
        public decimal PrecoMercado   { set; get; }
        [DataMember]
        public decimal LucroPrejuiso  { set; get; }
        [DataMember]
        public decimal FatorMultiplicador { set; get; }
    }
}
