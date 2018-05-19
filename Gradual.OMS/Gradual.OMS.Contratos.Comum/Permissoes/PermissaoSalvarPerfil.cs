using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    // 7ED6A969-8C59-4e62-8BE7-CEB83134FF62
    [Permissao(
        CodigoPermissao = "7ED6A969-8C59-4e62-8BE7-CEB83134FF62",
        NomePermissao = "Salvar Perfil",
        DescricaoPermissao = "Indica se o usuário tem permissão para salvar pefis, tanto novos quanto existentes")]
    public class PermissaoSalvarPerfil : PermissaoBase
    {
    }
}
