using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Comum.Dados
{
    /// <summary>
    /// Contem informações da associação da permissão com outra entidade.
    /// Esta classe é necessária para, por exemplo, indicar se a associação está 
    /// negando ou permitindo a permissão.
    /// </summary>
    [Serializable]
    public class PermissaoAssociadaInfo : ICodigoEntidade
    {
        /// <summary>
        /// Código da permissão associada
        /// </summary>
        public string CodigoPermissao { get; set; }

        /// <summary>
        /// Status da associação. Indica se a permissão está sendo permitida ou negada.
        /// </summary>
        public PermissaoAssociadaStatusEnum Status { get; set; }

        /// <summary>
        /// Detalhe da permissão.
        /// Opcionalmente pode ser preenchido com o PermissaoInfo ao invés
        /// de apenas o código como na propriedade CodigoPermissao. Utilizado para facilitar o 
        /// preenchimento das telas cadastrais.
        /// </summary>
        public PermissaoInfo PermissaoInfo { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            return this.CodigoPermissao;
        }

        #endregion
    }
}
