using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.OMS.Contratos.Risco.Permissoes
{
    // D63E4B10-186F-4cf1-A77E-64053B4DEC6F
    [Permissao(
        CodigoPermissao = "D63E4B10-186F-4cf1-A77E-64053B4DEC6F",
        NomePermissao = "Salvar Perfil de Risco",
        DescricaoPermissao = "Indica se o usuário tem permissão para salvar perfil de risco")]
    public class PermissaoSalvarPerfilRisco : PermissaoBase
    {
    }
}
