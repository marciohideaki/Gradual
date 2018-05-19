using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    // F3C957F0-1C4B-4ad5-8D39-C307F7237314
    [Permissao(
        CodigoPermissao = "F3C957F0-1C4B-4ad5-8D39-C307F7237314",
        NomePermissao = "Associar Permissão",
        DescricaoPermissao = "Indica se o usuário tem permissão para associar permissões a outras entidades (Usuário, Grupo, Perfil)")]
    public class PermissaoAssociarPermissao : PermissaoBase
    {
    }
}
