using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    // 3803F285-F6BA-4250-9315-D3F869FF2F5F
    [Permissao(
        CodigoPermissao = "3803F285-F6BA-4250-9315-D3F869FF2F5F",
        NomePermissao = "Listar Perfis",
        DescricaoPermissao = "Indica se o usuário tem permissão para listar perfis")]
    public class PermissaoListarPerfis : PermissaoBase
    {
    }
}
