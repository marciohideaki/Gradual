using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.OMS.Contratos.Risco.Permissoes
{
    // 5D24D255-3489-4b43-B39E-9FBD6513D008
    [Permissao(
        CodigoPermissao = "5D24D255-3489-4b43-B39E-9FBD6513D008",
        NomePermissao = "Salvar Ticket de Risco",
        DescricaoPermissao = "Indica se o usuário tem permissão para salvar ticket de risco")]
    public class PermissaoSalvarTicketRisco : PermissaoBase
    {
    }
}
