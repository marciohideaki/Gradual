using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{
    [Serializable]
    [DataContract]
    public class OperacoesInfo
    {
        /// <summary>
        /// Codigo Bovespa
        /// </summary>
        [DataMember]
        public string  Cliente      { set; get; }

        /// <summary>
        /// Instrumento
        /// </summary>
        [DataMember]
        public string  Instrumento  { set; get; }

        /// <summary>
        /// Quantidade 
        /// </summary>
        [DataMember]
        public int     Quantidade   { set; get; }

        /// <summary>
        /// Preço Negocio
        /// </summary>
        [DataMember]
        public decimal PrecoNegocio { set; get; }

        /// <summary>
        /// Preço Mercado
        /// </summary>
        [DataMember]
        public decimal PrecoMercado { set; get; }

        /// <summary>
        /// Total do negocio
        /// </summary>
        [DataMember]
        public decimal TotalNegocio { set; get; }

        /// <summary>
        /// Total Mercado
        /// </summary>
        [DataMember]
        public decimal TotalMercado { set; get; }

        /// <summary>
        /// Lucro Prejuízo da operação
        /// </summary>
        [DataMember]
        public decimal LucroPrejuiso{ set; get; }

        /// <summary>
        /// Sentido da operação
        /// </summary>
        [DataMember]
        public string  Sentido      { set; get; }

        /// <summary>
        /// Porta de operação
        /// </summary>
        [DataMember]
        public string Porta { get; set; }


        [DataMember]
        public decimal LucroPrejuizo { get; set; }
       
    }
}
