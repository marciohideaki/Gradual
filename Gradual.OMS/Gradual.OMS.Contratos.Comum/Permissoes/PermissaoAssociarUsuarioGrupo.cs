using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    // 626EC9D9-D36E-4378-9761-BCA51F5D76B6
    [Permissao(
        CodigoPermissao = "626EC9D9-D36E-4378-9761-BCA51F5D76B6",
        NomePermissao = "Associar Grupo de Usuário",
        DescricaoPermissao = "Indica se o usuário tem permissão para associar grupos de usuários a Usuários")]
    public class PermissaoAssociarUsuarioGrupo : PermissaoBase
    {
    }
}
