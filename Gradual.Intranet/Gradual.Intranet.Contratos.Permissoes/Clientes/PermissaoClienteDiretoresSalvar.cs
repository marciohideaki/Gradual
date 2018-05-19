using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Salvar Diretores. PJ
    /// </summary>
    [Permissao(
    "923DF791-18B0-4761-8276-10D99FBA71CE"
    , "Cliente - Salvar Diretores PJ"
    , "Permite Salvar Diretores. PJ")]
    public class PermissaoClienteDiretoresSalvar : PermissaoBase
    {
    }
}
