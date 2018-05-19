using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    
    /// <summary>
    /// Permite Consultar os contratos do cliente
    /// </summary>
    [Permissao(
        "4FBAB4B9-E003-4a9e-A476-7B1BFEE3B9C4"
        , "Cliente - Consultar Contratos"
        , "Permite Consultar os contratos do cliente")]
    public class PermissaoClienteContratosConsultar : PermissaoBase
    {
    }
}
