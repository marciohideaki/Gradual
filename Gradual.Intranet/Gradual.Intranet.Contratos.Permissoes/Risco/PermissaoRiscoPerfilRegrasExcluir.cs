using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Risco
{
    /// <summary>
    /// Permite Excluir Regras do Perfil de Risco
    /// </summary>
    [Permissao(
    "E4C38E83-07AE-4E8E-80E0-E1DD991B84BC"
    , "Risco - Excluir Regras do Perfil de Risco"
    , "Permite Excluir Regras do Perfil de Risco")]
    public class PermissaoRiscoPerfilRegrasExcluir : PermissaoBase
    {
    }
}
