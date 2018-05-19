using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    // 024958B9-0377-4d8c-B69A-A6C9C4410EE3
    [Permissao(
        CodigoPermissao = "024958B9-0377-4d8c-B69A-A6C9C4410EE3", 
        NomePermissao = "Salvar Usuário", 
        DescricaoPermissao = "Indica se o usuário tem permissão para salvar usuários, tanto novos quanto existentes")]
    public class PermissaoSalvarUsuario : PermissaoBase
    {
    }
}
