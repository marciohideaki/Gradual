using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Salvar Situação Financeira e Rendimentos - PJ.
    /// </summary>
    [Permissao(
    "ED62D854-7E79-4D01-B2B8-43521B9C4370"
    , "Cliente - Salvar Situação Financeira e Rendimentos - PJ."
    , "Permite Salvar Situação Financeira e Rendimentos - PJ")]
    public class PermissaoClienteRendimentoSitFinanceiraSalvar : PermissaoBase
    {
    }
}
