using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Orbite.RV.Contratos.MarketData.Dados
{
    /// <summary>
    /// Contém informações sobre um canal de market data.
    /// </summary>
    public class CanalInfo
    {
        /// <summary>
        /// Nome que identifica o canal.
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Tipo do canal, no formato Tipo, Assembly
        /// </summary>
        [XmlIgnore]
        public Type TipoCanal 
        {
            get { return Type.GetType(this.TipoCanalString); }
            set { this.TipoCanalString = value.FullName + ", " + value.Assembly.FullName; } 
        }

        /// <summary>
        /// Propriedade de auxilio para serializacao
        /// </summary>
        [XmlElement("TipoCanal")]
        public string TipoCanalString { get; set; }
    }
}
