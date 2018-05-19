using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.OMS.Contratos.Ordens.Permissoes
{
    // 2CA39A3C-502E-4db3-8FC0-45E51F0FE6F2
    [Permissao(
        CodigoPermissao = "2CA39A3C-502E-4db3-8FC0-45E51F0FE6F2",
        NomePermissao = "Alterar Ordem",
        DescricaoPermissao = "Indica se o usuário tem permissão para enviar mensagens de solicitação de alteração de ordem")]
    public class PermissaoAlterarOrdem : PermissaoBase
    {
    }
}
