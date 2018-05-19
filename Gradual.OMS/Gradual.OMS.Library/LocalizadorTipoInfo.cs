using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Contem informações para localizar os tipos a serem adicionados
    /// </summary>
    public class LocalizadorTipoInfo
    {
        /// <summary>
        /// Indica que todos os tipos do namespace devem ser incluídos.
        /// O namespace é representado no formato namespace, assembly
        /// </summary>
        [XmlAttribute]
        public string IncluirNamespace { get; set; }

        /// <summary>
        /// Indica que o tipo deve ser incluído.
        /// O formato desta propriedade é tipo, assembly
        /// </summary>
        [XmlAttribute]
        public string IncluirTipo { get; set; }

        /// <summary>
        /// Indica que todos os tipos do assembly devem ser incluidos.
        /// O formato é apenas o nome do assembly
        /// </summary>
        [XmlAttribute]
        public string IncluirAssembly { get; set; }

        /// <summary>
        /// Indica se deve considerar o namespace informado e os filhos
        /// </summary>
        [XmlAttribute]
        public bool AprofundarNamespace { get; set; }
    }
}
