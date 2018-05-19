using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Contratos.Comum.Dados
{
    /// <summary>
    /// Classe de dados para identificar um gerador de regra
    /// </summary>
    public class GeradorRegraInfo
    {
        /// <summary>
        /// Tipo do gerador de regra
        /// </summary>
        [XmlIgnore]
        public Type TipoGeradorRegra 
        {
            get { return Type.GetType(this.TipoGeradorRegraString); }
            set { this.TipoGeradorRegraString = value.FullName + ", " + value.Assembly.FullName; }
        }

        /// <summary>
        /// String do tipo do gerador de regra. Para ser utilizado na serialização.
        /// </summary>
        [XmlElement("TipoGeradorRegra")]
        public string TipoGeradorRegraString { get; set; }

        /// <summary>
        /// Indica os tipos associados a esse gerador.
        /// Caso seja nulo, indica todos os tipos.
        /// Contem um ou mais tipos separados por ';'. Os tipos devem estar no formato
        /// tipos, assembly.
        /// </summary>
        public string TiposAssociados { get; set; }

        /// <summary>
        /// Indica namespaces associados a esse gerador.
        /// Caso seja nulo, indica todos os namespaces.
        /// Contem um ou mais namespaces separados por ';'. Os namespaces devem estar no
        /// formato namespace, assembly.
        /// </summary>
        public string NamespacesAssociados { get; set; }

        /// <summary>
        /// Indica se deve considerar os namespaces informados e os filhos
        /// </summary>
        public bool AprofundarNamespaces { get; set; }
    }
}
