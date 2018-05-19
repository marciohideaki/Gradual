using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Salvar Endereços.
    /// </summary>
    [Permissao(
    "10FEAC2B-7E38-4922-A8E5-8E1EF331E92C"
    , "Cliente - Salvar Endereços"
    , "Permite Salvar Endereços.")]
    public class PermissaoClienteEnderecosSalvar : PermissaoBase
    {
    }
}
 