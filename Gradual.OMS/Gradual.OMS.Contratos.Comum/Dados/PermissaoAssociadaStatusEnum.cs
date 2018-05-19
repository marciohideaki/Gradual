using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Dados
{
    /// <summary>
    /// Contem os status da associação de permissão. Por exemplo, permitida ou negada.
    /// </summary>
    public enum PermissaoAssociadaStatusEnum
    {
        /// <summary>
        /// Indica que a permissão vale para o contexto
        /// </summary>
        Permitido,

        /// <summary>
        /// Indica uma negação da permissão no contexto
        /// </summary>
        Negado
    }
}
