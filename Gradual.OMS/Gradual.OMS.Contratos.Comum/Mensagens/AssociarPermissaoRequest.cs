using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de adicionar permissao
    /// </summary>
    public class AssociarPermissaoRequest : MensagemRequestBase 
    {
        /// <summary>
        /// Codigo da permissao a ser associada.
        /// Caso esta propriedade seja informada, ignora o TipoPermissao
        /// </summary>
        public string CodigoPermissao { get; set; }

        /// <summary>
        /// Tipo da permissao a ser associada.
        /// </summary>
        [XmlIgnore]
        public Type TipoPermissao 
        {
            get { return Type.GetType(this.TipoPermissaoString); }
            set { this.TipoPermissaoString = value.FullName + ", " + value.Assembly.FullName; }
        }

        /// <summary>
        /// String do tipo da permissao a ser associada.
        /// Propriedade necessário para serialização
        /// </summary>
        [XmlElement("TipoPermissao")]
        public string TipoPermissaoString { get; set; }

        /// <summary>
        /// Indica o status da permissão a associar. Permitido, não permitido, outros.
        /// </summary>
        public PermissaoAssociadaStatusEnum StatusPermissao { get; set; }

        /// <summary>
        /// Código do usuário a ter a permissão associada
        /// </summary>
        public string CodigoUsuario { get; set; }

        /// <summary>
        /// Código do usuário grupo a ter a permissão associada
        /// </summary>
        public string CodigoUsuarioGrupo { get; set; }

        /// <summary>
        /// Código do perfil a ter a permissão associada
        /// </summary>
        public string CodigoPerfil { get; set; }
    }
}
