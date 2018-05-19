using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Ativar ou desativar um cliente
    /// </summary>
    [Permissao(
    "FBCB16CD-A8AA-4eff-9EA8-844FEE2920C8"
    , "Cliente - Ativar/Desativar"
    , "Permite Ativar ou desativar um cliente")]
    public class PermissaoClienteAtivarDesativarExecutar : PermissaoBase
    {
    }
}
