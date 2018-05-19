using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Salvar Clientes.
    /// </summary>
    [Permissao(
    "8CBBCD25-D74D-4ef6-9646-28EB37679960"
    , "Cliente - Salvar Clientes"
    , "Permite Salvar Clientes.")]
    public class PermissaoClienteSalvar : PermissaoBase
    {
    }
}
