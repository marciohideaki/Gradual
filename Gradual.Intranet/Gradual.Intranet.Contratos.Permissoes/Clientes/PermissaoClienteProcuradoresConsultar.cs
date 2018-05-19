using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Consultar Procuradores / Representantes PJ.
    /// </summary>
    [Permissao(
    "AE18C291-039B-4377-ADB7-A89EEC22632C"
    , "Cliente - Consultar Procuradores / Representantes PJ."
    , "Permite Consultar Procuradores / Representantes PJ.")]
    public class PermissaoClienteProcuradoresConsultar : PermissaoBase
    {

    }
}
