using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    // A4A850D1-B933-464c-92FA-A07203A39BF0
    [Permissao(
        CodigoPermissao = "A4A850D1-B933-464c-92FA-A07203A39BF0",
        NomePermissao = "Receber Grupo de Usuário",
        DescricaoPermissao = "Indica se o usuário tem permissão para ler um grupo de usuário")]
    public class PermissaoReceberUsuarioGrupo : PermissaoBase
    {
    }
}
