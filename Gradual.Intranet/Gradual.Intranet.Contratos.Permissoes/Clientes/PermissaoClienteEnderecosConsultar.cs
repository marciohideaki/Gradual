using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Salvar Endereços.
    /// </summary>
    [Permissao(
    "906C31E7-F3E3-4978-A3EB-A56551E4EB9B"
    , "Cliente - Consultar Endereços"
    , "Permite Consultar Endereços.")]
    public class PermissaoClienteEnderecosConsultar : PermissaoBase
    {
    }
}
