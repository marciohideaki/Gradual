using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Salvar Empresas Coligadas ou Controladoras. PJ
    /// </summary>
    [Permissao(
    "3593CB02-1772-4A6C-9893-BC90A7137B0E"
    , "Cliente - Salvar Empresas Coligadas/Controladoras PJ"
    , "Permite Salvar Empresas Coligadas ou Controladoras. PJ")]
    public class PermissaoClienteColigadasControladorasSalvar : PermissaoBase
    {
    }
}
