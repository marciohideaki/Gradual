using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    // B4F0748A-C5F5-4b39-9EC1-FBB15D1C1EA3
    [Permissao(
        CodigoPermissao = "B4F0748A-C5F5-4b39-9EC1-FBB15D1C1EA3",
        NomePermissao = "Remover Usuário",
        DescricaoPermissao = "Indica se o usuário tem permissão para remover um usuário")]
    public class PermissaoRemoverUsuario : PermissaoBase
    {
    }
}
