using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.OMS.Contratos.Ordens.Permissoes
{
    // D687A972-E854-4cbb-909F-E328B483014E
    [Permissao(
        CodigoPermissao = "D687A972-E854-4cbb-909F-E328B483014E",
        NomePermissao = "Executar Ordem",
        DescricaoPermissao = "Indica se o usuário tem permissão para enviar mensagens de solicitação de execução de ordem")]
    public class PermissaoExecutarOrdem : PermissaoBase
    {
    }
}
