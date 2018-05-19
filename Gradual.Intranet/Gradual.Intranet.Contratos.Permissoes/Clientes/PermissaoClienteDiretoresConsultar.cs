using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Consultar Diretores. PJ
    /// </summary>
    [Permissao(
    "4A091610-9DCE-4FE5-A0DE-568FA9430FD0"
    , "Cliente - Consultar Diretores PJ"
    , "Permite Consultar Diretores. PJ")]
    public class PermissaoClienteDiretoresConsultar : PermissaoBase
    {
    }
}
