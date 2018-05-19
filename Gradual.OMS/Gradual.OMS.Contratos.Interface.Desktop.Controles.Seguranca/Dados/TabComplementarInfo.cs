using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Gradual.OMS.Contratos.Interface.Desktop.Controles.Seguranca.Dados
{
    /// <summary>
    /// Informações sobre tabs complementares
    /// </summary>
    [Serializable]
    public class TabComplementarInfo
    {
        /// <summary>
        /// Tipo do controle, utilizado para serialização
        /// </summary>
        [XmlElement("TipoControle")]
        public string TipoControleString { get; set; }

        /// <summary>
        /// Tipo do controle
        /// </summary>
        [XmlIgnore]
        public Type TipoControle 
        {
            get { return Type.GetType(this.TipoControleString); }
            set { this.TipoControleString = value.FullName + ", " + value.Assembly.FullName; } 
        }

        /// <summary>
        /// Título da tab
        /// </summary>
        public string Titulo { get; set; }
    }
}
