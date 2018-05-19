using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    // A0FE5EA1-86FF-4efb-A7DE-7A24996E4080
    [Permissao(
        CodigoPermissao = "A0FE5EA1-86FF-4efb-A7DE-7A24996E4080",
        NomePermissao = "Receber Usuário",
        DescricaoPermissao = "Indica se o usuário tem permissão para ler um usuário")]
    public class PermissaoReceberUsuario : PermissaoBase
    {
    }
}
