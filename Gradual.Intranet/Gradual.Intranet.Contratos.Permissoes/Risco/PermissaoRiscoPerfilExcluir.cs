using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Risco
{
    /// <summary>
    /// Permite Excluir Perfil de Risco
    /// </summary>
    [Permissao(
    "F9BCDD0E-9290-4004-9D11-9CDA6CE54F0F"
    , "Risco - Excluir Perfil de Risco"
    , "Permite Excluir Perfil de Risco")]
    public class PermissaoRiscoPerfilExcluir : PermissaoBase
    {
    }
}
