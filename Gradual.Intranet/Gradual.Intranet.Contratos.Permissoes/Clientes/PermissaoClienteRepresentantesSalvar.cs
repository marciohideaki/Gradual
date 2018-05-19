using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Salvar Representantes Legais.
    /// </summary>
    [Permissao(
    "675B2420-F136-4512-AE7F-BF5639F97CD2"
    , "Cliente - Salvar Representantes Legais"
    , "Permite Salvar Representantes Legais.")]
    public class PermissaoClienteRepresentantesSalvar : PermissaoBase
    {
    }
}
