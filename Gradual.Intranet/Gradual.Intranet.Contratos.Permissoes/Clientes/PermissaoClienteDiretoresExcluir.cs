using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Excluir Diretores. PJ
    /// </summary>
    [Permissao(
    "1898E735-06A4-445F-A6FC-C8E4635832FF"
    , "Cliente - Excluir Diretores PJ"
    , "Permite Excluir Diretores. PJ")]
    public class PermissaoClienteDiretoresExcluir : PermissaoBase
    {
    }
}
