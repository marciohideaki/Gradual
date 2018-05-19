using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Consultar Representante para não Residente. PJ.
    /// </summary>
    [Permissao(
    "CA4AB31B-9241-4140-AB3A-ECCB1B29DF56"
    , "Cliente - Consultar Representante para não Residente. PJ"
    , "Permite Consultar Representante para não Residente. PJ")]
    public class PermissaoClienteRepresentanteNaoResidenteConsultar : PermissaoBase
    {
    }
}
