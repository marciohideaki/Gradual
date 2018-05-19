using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Risco
{
    /// <summary>
    /// Permite Consultar Regras do Perfil de Risco
    /// </summary>
    [Permissao(
    "F69D1BF3-F240-46B4-B084-8F8AF86181DA"
    , "Risco - Consultar Regras do Perfil de Risco"
    , "Permite Consultar Regras do Perfil de Risco")]
    public class PermissaoRiscoPerfilRegrasConsultar : PermissaoBase
    {
    }
}
