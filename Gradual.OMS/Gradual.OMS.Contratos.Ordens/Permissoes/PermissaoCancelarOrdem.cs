using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.OMS.Contratos.Ordens.Permissoes
{
    // 57AF2CF9-23E7-4197-A691-001AC563567F
    [Permissao(
        CodigoPermissao = "57AF2CF9-23E7-4197-A691-001AC563567F",
        NomePermissao = "Cancelar Ordem",
        DescricaoPermissao = "Indica se o usuário tem permissão para enviar mensagens de solicitação de cancelamento de ordem")]
    public class PermissaoCancelarOrdem : PermissaoBase
    {
    }
}
