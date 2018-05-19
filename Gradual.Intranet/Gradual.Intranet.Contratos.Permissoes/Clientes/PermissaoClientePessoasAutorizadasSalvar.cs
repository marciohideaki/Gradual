using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Salvar Pessoas Autorizadas.
    /// </summary>
    [Permissao(
    "821C9455-C3D8-468f-85D2-E7B6E71F4128"
    , "Cliente - Salvar Pessoas Autorizadas"
    , "Permite Salvar Pessoas Autorizadas.")]
    public class PermissaoClientePessoasAutorizadasSalvar : PermissaoBase
    {
    }
}
