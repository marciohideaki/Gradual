using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Consultar pendencias cadastrais do cliente
    /// </summary>
    [Permissao(
        "A4C1EF79-CBFB-4a8f-8049-AFA5D2484077"
        , "Cliente - Consultar Pendencias Cadastrais"
        , "Permite Consultar pendencias cadastrais do cliente")]
    public class PermissaoClientePendenciasCadastraisConsultar : PermissaoBase
    {
    }
}
