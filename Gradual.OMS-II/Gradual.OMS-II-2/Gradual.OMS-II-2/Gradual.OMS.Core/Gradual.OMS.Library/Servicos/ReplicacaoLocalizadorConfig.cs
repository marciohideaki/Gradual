using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Library.Servicos
{
    public class ReplicacaoLocalizadorConfig
    {
        /// <summary>
        /// Lista de localizadores a serem replicados
        /// </summary>
        [XmlElement("Replicador")]
        public List<string> Replicadores { get; set; }
    }
}
