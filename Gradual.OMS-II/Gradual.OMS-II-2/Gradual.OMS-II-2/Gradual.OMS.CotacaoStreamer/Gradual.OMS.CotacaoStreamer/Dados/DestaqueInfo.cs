using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.OMS.CotacaoStreamer.Dados
{
    public class DestaqueInfo
    {
        /// <summary>
        /// Data e Hora do ultimo negocio
        /// </summary>
        [DataMember]
        public string dt { get; set; }

        /// <summary>
        /// Codigo do Instrumento
        /// </summary>
        [DataMember]
        public string i { get; set; }

        /// <summary>
        /// Variação do preco do Instrumento
        /// </summary>
        [DataMember]
        public string v { get; set; }

        /// <summary>
        /// Quantidade de papeis negociados do Instrumento
        /// </summary>
        [DataMember]
        public string qt { get; set; }

        /// <summary>
        /// Ultimo preco do Instrumento
        /// </summary>
        [DataMember]
        public string p { get; set; }

        /// <summary>
        /// Volume financeiro do Instrumento
        /// </summary>
        [DataMember]
        public string vl { get; set; }

        /// <summary>
        /// Numero de negocios do Instrumento
        /// </summary>
        [DataMember]
        public string n { get; set; }

        /// <summary>
        /// Melhor corretora compradora do Instrumento
        /// </summary>
        [DataMember]
        public string cp { get; set; }

        /// <summary>
        /// Melhor corretora vendedora do Instrumento
        /// </summary>
        [DataMember]
        public string vd { get; set; }
    }
}
