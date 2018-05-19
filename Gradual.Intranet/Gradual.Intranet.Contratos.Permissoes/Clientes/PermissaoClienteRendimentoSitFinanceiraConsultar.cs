using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Consultar Situação Financeira e Rendimentos - PJ.
    /// </summary>
    [Permissao(
    "200F9C7F-2C71-431E-A838-32F83C2B72D2"
    , "Cliente - Consultar Situação Financeira e Rendimentos - PJ."
    , "Permite Consultar Situação Financeira e Rendimentos - PJ")]
    public class PermissaoClienteRendimentoSitFinanceiraConsultar : PermissaoBase
    {
    }
}
