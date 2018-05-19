using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Excluir Pessoas Autorizadas.
    /// </summary>
    [Permissao(
    "94B2C130-E7E0-41f3-97FE-CEB4AB19D8A3"
    , "Cliente - Excluir Pessoas Autorizadas"
    , "Permite Excluir Pessoas Autorizadas.")]
    public class PermissaoClientePessoasAutorizadasExcluir : PermissaoBase
    {
    }
}
