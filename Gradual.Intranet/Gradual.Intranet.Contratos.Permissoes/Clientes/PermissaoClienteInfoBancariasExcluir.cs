using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Excluir Informações Bancárias.
    /// </summary>
    [Permissao(
    "5D24F0AA-6026-4006-BE18-0CC0C8D19D47"
    , "Cliente - Excluir Informações Bancárias"
    , "Permite Excluir Informações Bancárias.")]
    public class PermissaoClienteInfoBancariasExcluir : PermissaoBase
    {
    }
}
