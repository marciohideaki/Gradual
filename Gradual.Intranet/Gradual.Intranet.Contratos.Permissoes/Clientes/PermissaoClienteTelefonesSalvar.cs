using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Salvar Telefones.
    /// </summary>
    [Permissao(
    "45231EA2-CA95-48a7-8006-8A55BCFCA3CD"
    , "Cliente - Salvar Telefones"
    , "Permite Salvar Telefones.")]
    public class PermissaoClienteTelefonesSalvar : PermissaoBase
    {
    }
}
