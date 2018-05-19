using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    // A5488A98-0EB4-49df-8100-2815CC2D4DA2
    [Permissao(
        CodigoPermissao = "A5488A98-0EB4-49df-8100-2815CC2D4DA2",
        NomePermissao = "Listar Usuários",
        DescricaoPermissao = "Indica se o usuário tem permissão para listar usuários")]
    public class PermissaoListarUsuarios : PermissaoBase
    {
    }
}
