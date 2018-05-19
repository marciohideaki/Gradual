using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    // 23BBC31B-DD05-4c41-9BC6-5A814FB2D31C
    [Permissao(
        CodigoPermissao = "23BBC31B-DD05-4c41-9BC6-5A814FB2D31C",
        NomePermissao = "Remover Grupo de Usuário",
        DescricaoPermissao = "Indica se o usuário tem permissão para remover um grupo de usuário")]
    public class PermissaoRemoverUsuarioGrupo : PermissaoBase
    {
    }
}
