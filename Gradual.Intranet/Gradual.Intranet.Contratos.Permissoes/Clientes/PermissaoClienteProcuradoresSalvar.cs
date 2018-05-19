using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Salvar Procuradores / Representantes PJ.
    /// </summary>
    [Permissao(
    "FDE3F8E9-F9E0-4FF8-801F-B83312BA66C6"
    , "Cliente - Salvar Procuradores / Representantes PJ."
    , "Permite Salvar Procuradores / Representantes PJ.")]
    public class PermissaoClienteProcuradoresSalvar : PermissaoBase
    {
    }
}
