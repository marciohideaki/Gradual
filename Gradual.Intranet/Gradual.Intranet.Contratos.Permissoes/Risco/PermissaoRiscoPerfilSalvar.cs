using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Risco
{
    /// <summary>
    /// Permite Salvar Perfil de Risco
    /// </summary>
    [Permissao(
    "2E470C76-7085-4BE2-91FB-525E0F63C9D9"
    , "Risco - Salvar Perfil de Risco"
    , "Permite Salvar Perfil de Risco")]
    public class PermissaoRiscoPerfilSalvar : PermissaoBase
    {
    }
}
