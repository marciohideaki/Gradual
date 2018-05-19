using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// ServicoHostConfig - Configuracao do Hoster de servicos
    /// </summary>
    public class ServicoHostConfig 
    {

        /// <summary>
        /// Lista de servicos configurados
        /// </summary>
        public List<ServicoInfo> Servicos { get; set; }

        /// <summary>
        /// Endereco base para endpoints WCF
        /// </summary>
        ///
        [XmlElement("BaseAddress")]
        public List<string> BaseAddress { get; set; }

        /// <summary>
        /// Endereco base para exposicao de Metadados (MEX)
        /// </summary>
        public string MexBaseAddress { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public ServicoHostConfig()
        {
            this.Servicos = new List<ServicoInfo>();
            this.BaseAddress = new List<string>();
        }
    }
}
