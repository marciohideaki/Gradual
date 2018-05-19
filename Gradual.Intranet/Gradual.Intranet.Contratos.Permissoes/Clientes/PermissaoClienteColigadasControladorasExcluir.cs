using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Excluir Empresas Coligadas ou Controladoras. PJ
    /// </summary>
    [Permissao(
    "EF3F0430-C176-4B8F-8E2C-BE6B43178B92"
    , "Cliente - Excluir Empresas Coligadas/Controladoras PJ"
    , "Permite Excluir Empresas Coligadas ou Controladoras. PJ")]
    public class PermissaoClienteColigadasControladorasExcluir : PermissaoBase
    {
    }
}
