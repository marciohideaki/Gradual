using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Consultar Pessoas Autorizadas.
    /// </summary>
    [Permissao(
    "A1617EC7-2292-45e4-B32E-48CC35A255E6"
    , "Cliente - Consultar Pessoas Autorizadas"
    , "Permite Consultar Pessoas Autorizadas.")]
    public class PermissaoClientePessoasAutorizadasConsultar : PermissaoBase
    {
    }
}
