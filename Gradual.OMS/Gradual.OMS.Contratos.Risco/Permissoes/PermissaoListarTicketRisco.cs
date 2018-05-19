using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.OMS.Contratos.Risco.Permissoes
{
    // FE0A1B85-E457-4a5c-BC24-D26ADCAE3650
    [Permissao(
        CodigoPermissao = "FE0A1B85-E457-4a5c-BC24-D26ADCAE3650",
        NomePermissao = "Listar Tickets de Risco",
        DescricaoPermissao = "Indica se o usuário tem permissão para listar tickets de risco")]
    public class PermissaoListarTicketRisco : PermissaoBase
    {
    }
}
