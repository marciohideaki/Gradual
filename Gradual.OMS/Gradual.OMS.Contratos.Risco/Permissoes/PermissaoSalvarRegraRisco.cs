using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.OMS.Contratos.Risco.Permissoes
{
    // 41653DCD-86F8-437b-878E-4E60D37921EA
    [Permissao(
        CodigoPermissao = "41653DCD-86F8-437b-878E-4E60D37921EA",
        NomePermissao = "Salvar Regra de Risco",
        DescricaoPermissao = "Indica se o usuário tem permissão para salvar regra de risco")]
    public class PermissaoSalvarRegraRisco : PermissaoBase
    {
    }
}
