using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Comum.Dados
{
    /// <summary>
    /// Classe de informações do usuário.
    /// </summary>
    [Serializable]
    public class UsuarioInfo : ICodigoEntidade
    {
        /// <summary>
        /// Chave primária do usuário
        /// </summary>
        [Description("Contém o código de identificação do usuário")]
        public string CodigoUsuario { get; set; }

        /// <summary>
        /// Lista de grupos do usuário.
        /// Contem apenas a lista de códigos dos grupos associados.
        /// Não faz associação direta com UsuarioGrupoInfo para facilitar o 
        /// gerenciamento com a persistencia.
        /// </summary>
        [Description("Lista de grupos a que o usuário pertence.")]
        public List<string> Grupos { get; set; }

        /// <summary>
        /// Lista de grupos do usuário.
        /// Opcionalmente pode ser preenchido com a lista de UsuarioGrupoInfo ao invés
        /// de apenas o código como na propriedade Grupos. Utilizado para facilitar o 
        /// preenchimento das telas cadastrais.
        /// </summary>
        public List<UsuarioGrupoInfo> Grupos2 { get; set; }

        /// <summary>
        /// Indica o nome do usuario
        /// </summary>
        [Description("Nome completo do usuário.")]
        public string Nome { get; set; }

        /// <summary>
        /// Indica uma abreviação do nome do usuário
        /// </summary>
        [Description("Nome abreviado do usuário")]
        public string NomeAbreviado { get; set; }

        /// <summary>
        /// Contem a senha de autenticação do usuário
        /// </summary>
        [Description("Senha do usuário")]
        public string Senha { get; set; }

        /// <summary>
        /// Assinatura eletrônica do usuário
        /// </summary>
        [Description("Assinatura eletrônica do usuário")]
        public string AssinaturaEletronica { get; set; }

        /// <summary>
        /// Indica a origem do usuário. A finalidade é informar se o usuário é interno ao 
        /// sistema, ou se é uma entidade de algum outro sistema.
        /// </summary>
        [Description("Indica a origem do usuário, por exemplo, interno, integração com sinacor, etc. Por enquanto não utilizado.")]
        public string Origem { get; set; }

        /// <summary>
        /// Permite que sejam adicionados objetos de complemento
        /// para o usuário
        /// </summary>
        [Description("Complementos de informações de usuários.")]
        [Browsable(false)]
        public ColecaoTipoInstancia Complementos { get; set; }

        /// <summary>
        /// Lista de perfis do usuário.
        /// Contem apenas a lista de códigos dos perfis associados.
        /// Não faz associação direta com PerfilInfo para facilitar o gerenciamento 
        /// com a persistencia.
        /// </summary>
        [Description("Lista de perfis a que o usuário tem direito.")]
        [Browsable(false)]
        public List<string> Perfis { get; set; }

        /// <summary>
        /// Lista de perfis do usuário.
        /// Opcionalmente pode ser preenchido com a lista de PerfilInfo ao invés
        /// de apenas o código como na propriedade Perfis. Utilizado para facilitar o 
        /// preenchimento das telas cadastrais.
        /// </summary>
        public List<PerfilInfo> Perfis2 { get; set; }

        /// <summary>
        /// Lista de permissões do usuário.
        /// </summary>
        [Description("Lista de permissões associadas ao usuário")]
        [Browsable(false)]
        public List<PermissaoAssociadaInfo> Permissoes { get; set; }

        /// <summary>
        /// Lista as relações do usuário com outros usuários
        /// </summary>
        [Description("Lista de relações entre usuários")]
        [Browsable(false)]
        public List<UsuarioRelacaoInfo> Relacoes { get; set; }
        
        /// <summary>
        /// Indica o status do usuário. Pode ser ativo, inativo, etc.
        /// </summary>
        [Description("Status do usuário")]
        public UsuarioStatusEnum Status { get; set; }

        /// <summary>
        /// Email do usuário
        /// </summary>
        [Description("Email do usuário")]
        public string Email { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public UsuarioInfo()
        {
            this.CodigoUsuario = Guid.NewGuid().ToString();
            this.Grupos = new List<string>();
            this.Perfis = new List<string>();
            this.Permissoes = new List<PermissaoAssociadaInfo>();
            this.Relacoes = new List<UsuarioRelacaoInfo>();
            this.Status = UsuarioStatusEnum.NaoInformado;
            this.Complementos = new ColecaoTipoInstancia();
        }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            return this.CodigoUsuario;
        }

        #endregion

        public override string ToString()
        {
            return Serializador.TransformarEmString(this);
        }
    }
}
