using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    // FCB9C6BB-B54F-4d5f-9749-85BA6806C6A3
    [Permissao(
        CodigoPermissao = "FCB9C6BB-B54F-4d5f-9749-85BA6806C6A3",
        NomePermissao = "Listar Grupos de Usuários",
        DescricaoPermissao = "Indica se o usuário tem permissão para listar grupos de usuários")]
    public class PermissaoListarUsuarioGrupos : PermissaoBase
    {
    }
}
