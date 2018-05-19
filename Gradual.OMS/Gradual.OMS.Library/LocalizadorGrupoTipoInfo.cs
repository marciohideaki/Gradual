using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Representa um tipo chamador que contem uma 
    /// lista de tipos que podem ser referenciados
    /// </summary>
    public class LocalizadorGrupoTipoInfo
    {
        /// <summary>
        /// Indica o tipo do chamador
        /// </summary>
        [XmlIgnore]
        public Type TipoChamador 
        {
            get { return Type.GetType(this.TipoChamadorString); }
            set { this.TipoChamadorString = value.FullName + ", " + value.Assembly.FullName; } 
        }

        /// <summary>
        /// Propriedade utilizada na serialização
        /// </summary>
        [XmlAttribute("TipoChamador")]
        public string TipoChamadorString { get; set; }

        /// <summary>
        /// Tipos a serem incluidos para esse chamador
        /// </summary>
        public List<LocalizadorTipoInfo> Tipos { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public LocalizadorGrupoTipoInfo()
        {
            this.Tipos = new List<LocalizadorTipoInfo>();
        }
    }
}
