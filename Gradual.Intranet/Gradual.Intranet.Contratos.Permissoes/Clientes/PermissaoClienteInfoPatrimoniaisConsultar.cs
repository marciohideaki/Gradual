using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Consultar Informações Patrimoniais.
    /// </summary>
    [Permissao(
    "268EAF54-320E-4959-8262-77D283A1A817"
    , "Cliente - Consultar Informações Patrimoniais"
    , "Permite Consultar Informações Patrimoniais.")]
    public class PermissaoClienteInfoPatrimoniaisConsultar : PermissaoBase
    {
    }
}
