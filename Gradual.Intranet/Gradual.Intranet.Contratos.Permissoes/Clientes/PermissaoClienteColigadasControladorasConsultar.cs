using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Consultar Empresas Coligadas ou Controladoras. PJ
    /// </summary>
    [Permissao(
    "ABA0DCB9-CB77-4D55-BC11-8B8895D1A243"
    , "Cliente - Consultar Empresas Coligadas/Controladoras PJ"
    , "Permite Consultar Empresas Coligadas ou Controladoras. PJ")]
    public class PermissaoClienteColigadasControladorasConsultar : PermissaoBase
    {
    }
}
