using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    // 0D08C6D1-3CD1-490d-9868-E2567ED25F7A
    [Permissao(
        CodigoPermissao = "0D08C6D1-3CD1-490d-9868-E2567ED25F7A",
        NomePermissao = "Salvar Grupo de Usuário",
        DescricaoPermissao = "Indica se o usuário tem permissão para salvar grupos de usuários, tanto novos quanto existentes")]
    public class PermissaoSalvarUsuarioGrupo : PermissaoBase
    {
    }
}
