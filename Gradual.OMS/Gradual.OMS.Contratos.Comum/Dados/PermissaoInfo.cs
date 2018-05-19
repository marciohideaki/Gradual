using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Comum.Dados
{
    /// <summary>
    /// Contém informações de uma permissão.
    /// A classe de metadados e o atributo de permissão utilizam esta classe.
    /// </summary>
    [Serializable]
    public class PermissaoInfo : ICodigoEntidade
    {
        /// <summary>
        /// Codigo da permissao
        /// </summary>
        public string CodigoPermissao { get; set; }

        /// <summary>
        /// Nome da permissão
        /// </summary>
        public string NomePermissao { get; set; }

        /// <summary>
        /// Descricao da permissao
        /// </summary>
        public string DescricaoPermissao { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            return this.CodigoPermissao;
        }

        #endregion
    }
}
