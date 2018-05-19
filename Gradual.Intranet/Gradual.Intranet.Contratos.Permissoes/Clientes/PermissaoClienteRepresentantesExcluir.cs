using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Excluir Representantes Legais.
    /// </summary>
    [Permissao(
    "1161D23B-7038-421c-A9AE-C17248E50269"
    , "Cliente - Excluir Representantes Legais"
    , "Permite Excluir Representantes Legais.")]
    public class PermissaoClienteRepresentantesExcluir : PermissaoBase
    {
    }
}
