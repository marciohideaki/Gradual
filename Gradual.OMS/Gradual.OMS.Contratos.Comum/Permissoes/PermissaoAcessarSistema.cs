using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    // 22FF518C-C7D3-4ff0-A0CB-96F2476068BB
    [Permissao(
        CodigoPermissao = "22FF518C-C7D3-4ff0-A0CB-96F2476068BB",
        NomePermissao = "Acessar Sistema",
        DescricaoPermissao = "Indica se o usuário tem permissão para acessar o sistema")]
    public class PermissaoAcessarSistema : PermissaoBase
    {
    }
}
