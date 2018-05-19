using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.OMS.Contratos.Risco.Permissoes
{
    // C3AFB8E6-873F-43fb-83F5-CD7A0933CB2E
    [Permissao(
        CodigoPermissao = "C3AFB8E6-873F-43fb-83F5-CD7A0933CB2E",
        NomePermissao = "Listar Perfis de Risco",
        DescricaoPermissao = "Indica se o usuário tem permissão para listar perfis de risco")]
    public class PermissaoListarPerfilRisco : PermissaoBase
    {
    }
}
