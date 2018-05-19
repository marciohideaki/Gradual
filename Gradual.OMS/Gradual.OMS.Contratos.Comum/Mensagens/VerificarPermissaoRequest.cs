using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de verificação de permissão por sessão.
    /// </summary>
    public class VerificarPermissaoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código da permissao que se deseja testar.
        /// Se o código for informado, ignora o tipo.
        /// </summary>
        public string CodigoPermissao { get; set; }

        /// <summary>
        /// Tipo da permissao que se deseja testar.
        /// </summary>
        [XmlIgnore]
        public Type TipoPermissao 
        {
            get { return Type.GetType(this.TipoPermissaoString); }
            set { this.TipoPermissaoString = value.FullName + ", " + value.Assembly.FullName; }
        }

        /// <summary>
        /// Tipo da permissao que se deseja testar.
        /// Propriedade utilizada para serialização
        /// </summary>
        [XmlElement("TipoPermissao")]
        public string TipoPermissaoString { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public VerificarPermissaoRequest()
        {
        }
    }
}
