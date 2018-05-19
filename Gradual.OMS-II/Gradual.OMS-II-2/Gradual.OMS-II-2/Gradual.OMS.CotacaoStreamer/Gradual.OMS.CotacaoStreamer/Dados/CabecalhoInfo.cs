using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.OMS.CotacaoStreamer.Dados
{
    public class CabecalhoInfo
    {
        /// <summary>
        /// Tipo de Mensagem
        /// </summary>
        [DataMember]
        public string tp { get; set; }

        /// <summary>
        /// Data (yyyyMMdd)
        /// </summary>
        [DataMember]
        public string d { get; set; }

        /// <summary>
        /// Hora (HHmmssSSS)
        /// </summary>
        [DataMember]
        public string h { get; set; }
    }
}
