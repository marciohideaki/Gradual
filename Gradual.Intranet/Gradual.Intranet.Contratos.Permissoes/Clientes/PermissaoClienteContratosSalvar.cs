using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Consultar os contratos do cliente
    /// </summary>
    [Permissao(
        "35A7D558-1FC1-43b6-86D9-C2FC95A6B5DA"
        , "Cliente - Salvar Contratos"
        , "Permite Salvar os contratos do cliente")]
    public class PermissaoClienteContratosSalvar : PermissaoBase
    {
    }
}
