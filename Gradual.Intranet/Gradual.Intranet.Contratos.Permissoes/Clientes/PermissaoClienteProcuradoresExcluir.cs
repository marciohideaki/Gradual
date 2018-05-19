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
    "52A14EA8-3982-43A1-95F3-5EAC62F7018B"
    , "Cliente - Excluir Procuradores / Representantes PJ."
    , "Permite Excluir Procuradores / Representantes PJ.")]
    public class PermissaoClienteProcuradoresExcluir : PermissaoBase
    {
    }
}
