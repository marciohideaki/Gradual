using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Visualizar a aba de clientes da intranet e fazer a consulta.
    /// </summary>
    [Permissao(
        "F31790A9-CBAD-43B2-8357-BAB02F47ED0D"
        , "Cliente - Consultar Clientes"
        , "Permite Visualizar a aba de clientes da intranet e fazer a consulta.")]
    public class PermissaoClienteConsultar : PermissaoBase
    {

    }
}
