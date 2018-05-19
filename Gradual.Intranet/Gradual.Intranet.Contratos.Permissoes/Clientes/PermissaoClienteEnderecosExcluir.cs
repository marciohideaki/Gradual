using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Excluir Endereços.
    /// </summary>
    [Permissao(
    "8BEF04C4-7F63-49ec-9518-2DE25DDA667F"
    , "Cliente - Excluir Endereços"
    , "Permite Excluir Endereços.")]
    public class PermissaoClienteEnderecosExcluir : PermissaoBase
    {
    }
}
