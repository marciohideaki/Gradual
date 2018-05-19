using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Excluir Representante para não Residente. PJ.
    /// </summary>
    [Permissao(
    "12C2713C-D55B-43FE-B102-5266A3C827AB"
    , "Cliente - Excluir Representante para não Residente. PJ"
    , "Permite Excluir Representante para não Residente. PJ")]
    public class PermissaoClienteRepresentanteNaoResidenteExcluir : PermissaoBase
    {
    }
}
