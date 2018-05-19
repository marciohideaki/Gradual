using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Comum.Dados
{
    /// <summary>
    /// Representa um grupo de usuários.
    /// </summary>
    [Serializable]
    public class UsuarioGrupoInfo : ICodigoEntidade
    {
        /// <summary>
        /// Código do grupo de usuários
        /// </summary>
        [XmlAttribute]
        public string CodigoUsuarioGrupo { get; set; }

        /// <summary>
        /// Nome do grupo de usuários.
        /// </summary>
        [XmlAttribute]
        public string NomeUsuarioGrupo { get; set; }

        /// <summary>
        /// Lista de perfis associados ao grupo de usuários.
        /// Esta propriedade é uma lista de strings com os códigos dos perfis associados.
        /// Não faz a associação direta com PerfilInfo para facilitar o gerenciamento junto 
        /// ao serviço de persistencia.
        /// </summary>
        public List<string> Perfis { get; set; }

        /// <summary>
        /// Lista de perfis do grupo.
        /// Opcionalmente pode ser preenchido com a lista de PerfilInfo ao invés
        /// de apenas o código como na propriedade Perfis. Utilizado para facilitar o 
        /// preenchimento das telas cadastrais.
        /// </summary>
        public List<PerfilInfo> Perfis2 { get; set; }

        /// <summary>
        /// Lista de permissões do grupo.
        /// </summary>
        public List<PermissaoAssociadaInfo> Permissoes { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public UsuarioGrupoInfo()
        {
            this.CodigoUsuarioGrupo = Guid.NewGuid().ToString();
            this.Perfis = new List<string>();
            this.Permissoes = new List<PermissaoAssociadaInfo>();
        }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            return this.CodigoUsuarioGrupo;
        }

        #endregion

        public override string ToString()
        {
            return Serializador.TransformarEmString(this);
        }
    }
}
