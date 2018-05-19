using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Salvar Informações Patrimoniais.
    /// </summary>
    [Permissao(
    "336585A3-167D-4cc6-8ADB-01FE9931A042"
    , "Cliente - Salvar Informações Patrimoniais"
    , "Permite Salvar Informações Patrimoniais.")]
    public class PermissaoClienteInfoPatrimoniaisSalvar : PermissaoBase
    {
    }
}
