using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Excluir Telefones.
    /// </summary>
    [Permissao(
    "384A906D-09D6-4732-8A2B-E1C7683A966B"
    , "Cliente - Excluir Telefones"
    , "Permite Excluir Telefones.")]
    public class PermissaoClienteTelefonesExcluir : PermissaoBase
    {
    }
}
