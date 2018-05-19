using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Consultar Informações Bancárias.
    /// </summary>
    [Permissao(
    "21BF79E2-534B-4afe-9F69-7A539E0B4201"
    , "Cliente - Consultar Informações Bancárias"
    , "Permite Consultar Informações Bancárias.")]
    public class PermissaoClienteInfoBancariasConsultar : PermissaoBase
    {
    }
}
