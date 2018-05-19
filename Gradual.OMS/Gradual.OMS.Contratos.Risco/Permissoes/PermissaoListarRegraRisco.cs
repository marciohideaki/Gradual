using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.OMS.Contratos.Risco.Permissoes
{
    // 7F7A239F-AC92-4752-B16F-3BA384254565
    [Permissao(
        CodigoPermissao = "7F7A239F-AC92-4752-B16F-3BA384254565",
        NomePermissao = "Listar Regras de Risco",
        DescricaoPermissao = "Indica se o usuário tem permissão para listar regras de risco")]
    public class PermissaoListarRegraRisco : PermissaoBase
    {
    }
}
