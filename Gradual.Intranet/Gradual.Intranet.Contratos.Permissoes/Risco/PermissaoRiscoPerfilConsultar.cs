using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Risco
{
    /// <summary>
    /// Permite Consultar Perfil de Risco
    /// </summary>
    [Permissao(
    "097C4E1E-8EE6-4C09-B4EA-E6CFC4592202"
    , "Risco - Consultar Perfil de Risco"
    , "Permite Consultar Perfil de Risco")]
    public class PermissaoRiscoPerfilConsultar : PermissaoBase
    {
    }
}
