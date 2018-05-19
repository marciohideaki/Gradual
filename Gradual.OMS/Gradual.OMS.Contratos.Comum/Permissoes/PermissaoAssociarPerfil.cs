using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    // D31AD6D1-FCA6-4529-ACBE-B5B9D60E5755
    [Permissao(
        CodigoPermissao = "D31AD6D1-FCA6-4529-ACBE-B5B9D60E5755",
        NomePermissao = "Associar Perfil",
        DescricaoPermissao = "Indica se o usuário tem permissão para associar perfis a outras entidades (Usuário, Grupo)")]
    public class PermissaoAssociarPerfil : PermissaoBase
    {
    }
}
