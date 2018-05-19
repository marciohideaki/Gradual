using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    // 54DDD6FF-871B-49cd-B3F6-745BAE281AEB
    [Permissao(
        CodigoPermissao = "54DDD6FF-871B-49cd-B3F6-745BAE281AEB",
        NomePermissao = "Remover Perfil",
        DescricaoPermissao = "Indica se o usuário tem permissão para remover um perfil")]
    public class PermissaoRemoverPerfil : PermissaoBase
    {
    }
}
