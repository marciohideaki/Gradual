using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{
    [Serializable]
    [DataContract]
    public class ClienteRiscoResumo
    {
        [DataMember]
        public int     Cliente             { set; get; }
        [DataMember]
        public string  Instrumento         { set; get; }
        [DataMember]
        public string  TipoMercado         { set; get; }
        [DataMember]
        public decimal LucroPrejuizo       { set; get; }
        [DataMember]
        public decimal VLNegocioCompra          { set; get; }
        [DataMember]
        public decimal VLNegocioVenda { set; get; }
        [DataMember]
        public decimal VLMercadoCompra { set; get; }
        [DataMember]
        public decimal VLMercadoVenda  { set; get; }
        [DataMember]
        public decimal Cotacao              { set; get; }
        [DataMember]
        public int     QtdeAber { set; get; }
        [DataMember]
        public int     QtdeComprada         { set; get; }
        [DataMember]
        public int     QtdeVendida          { set; get; }
        [DataMember]
        public decimal QtdeAtual { set; get; }
        [DataMember]
        public decimal FinanceiroAbertura { set; get; }
        [DataMember]
        public decimal FinanceiroComprado        { set; get; }
        [DataMember]
        public decimal FinanceiroVendido         { set; get; }
        [DataMember]        
        public decimal QtReversao              { set; get; }
        [DataMember]
        public decimal PrecoReversao           { set; get; }
        [DataMember]
        public decimal NetOperacao              { set; get; }
    }
}
