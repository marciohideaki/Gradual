using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    // FF941811-D66B-470f-89A7-45A0C5AD9537
    [Permissao(
        CodigoPermissao = "FF941811-D66B-470f-89A7-45A0C5AD9537",
        NomePermissao = "Receber Perfil",
        DescricaoPermissao = "Indica se o usuário tem permissão para ler um perfil")]
    public class PermissaoReceberPerfil : PermissaoBase
    {
    }
}
