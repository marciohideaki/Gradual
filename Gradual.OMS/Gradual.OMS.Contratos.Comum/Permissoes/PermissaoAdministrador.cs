using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    // 9C5DA26B-8C30-4c1d-AA7A-B7A22CF2CA8F
    [Permissao(
        CodigoPermissao = "9C5DA26B-8C30-4c1d-AA7A-B7A22CF2CA8F",
        NomePermissao = "Administrador",
        DescricaoPermissao = "Indica que o usuário tem acesso a todas as funções do sistema")]
    public class PermissaoAdministrador : PermissaoBase
    {
    }
}
