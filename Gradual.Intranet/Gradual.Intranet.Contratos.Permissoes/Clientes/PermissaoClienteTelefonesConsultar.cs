using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Salvar Telefones.
    /// </summary>
    [Permissao(
    "9631B9A9-33E0-43c9-8D35-8B804D34C48D"
    , "Cliente - Consultar Telefones"
    , "Permite Consultar Telefones.")]
    public class PermissaoClienteTelefonesConsultar : PermissaoBase
    {
    }
}
