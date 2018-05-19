using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Efetivar a renovação do cliente
    /// </summary>
    [Permissao(
        "D671BE57-3B5A-48f0-9FC2-5412EEE0C850"
        , "Cliente - Efetivar Renovação"
        , "Permite Efetivar a renovação do cliente")]
    public class PermissaoClienteEfetuarRenovacaoExecutar : PermissaoBase
    {
    }
}
