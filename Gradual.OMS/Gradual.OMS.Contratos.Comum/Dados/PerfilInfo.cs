using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Comum.Dados
{
    /// <summary>
    /// Contém informações sobre um perfil.
    /// Um perfil é, basicamente, um agrupamento de permissões.
    /// </summary>
    [Serializable]
    public class PerfilInfo : ICodigoEntidade
    {
        /// <summary>
        /// Código do perfil
        /// </summary>
        public string CodigoPerfil { get; set; }

        /// <summary>
        /// Nome do perfil
        /// </summary>
        public string NomePerfil { get; set; }

        /// <summary>
        /// Lista de permissões do perfil.
        /// </summary>
        [Browsable(false)]
        public List<PermissaoAssociadaInfo> Permissoes { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public PerfilInfo()
        {
            this.CodigoPerfil = Guid.NewGuid().ToString();
            this.Permissoes = new List<PermissaoAssociadaInfo>();
        }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            return this.CodigoPerfil;
        }

        #endregion

        public override string ToString()
        {
            return Serializador.TransformarEmString(this);
        }
    }
}
