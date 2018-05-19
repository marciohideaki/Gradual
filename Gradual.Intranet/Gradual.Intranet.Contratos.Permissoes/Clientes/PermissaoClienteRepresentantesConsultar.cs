using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Consultar Representantes Legais.
    /// </summary>
    [Permissao(
    "09DFBFF1-D916-4267-B9E9-923B78BF01AB"
    , "Cliente - Consultar Representantes Legais"
    , "Permite Consultar Representantes Legais.")]
    public class PermissaoClienteRepresentantesConsultar : PermissaoBase
    {

    }
}
