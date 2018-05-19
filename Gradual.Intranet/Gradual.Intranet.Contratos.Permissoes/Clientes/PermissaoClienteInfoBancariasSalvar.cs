using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Salvar Informações Bancárias.
    /// </summary>
    [Permissao(
    "A2EE4A42-FAB7-4784-B479-C7F224B5A7BB"
    , "Cliente - Salvar Informações Bancárias"
    , "Permite Salvar Informações Bancárias.")]
    public class PermissaoClienteInfoBancariasSalvar : PermissaoBase
    {
    }
}
