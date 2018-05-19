using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Consultar pendencias cadastrais do cliente
    /// </summary>
    [Permissao(
        "C33D260E-0050-45a2-BBD3-2EFBF96E7C4F"
        , "Cliente - Salvar Pendencias Cadastrais"
        , "Permite Salvar pendencias cadastrais do cliente")]
    public class PermissaoClientePendenciasCadastraisSalvar : PermissaoBase
    {
    }
}
