using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Excluir pendencias cadastrais do cliente
    /// </summary>
    [Permissao(
        "E390C1DD-7CB1-4a9f-8E92-24F8E1F6F4A1"
        , "Cliente - Excluir Pendencias Cadastrais"
        , "Permite Excluir pendencias cadastrais do cliente")]
    public class PermissaoClientePendenciasCadastraisExcluir : PermissaoBase
    {
    }
}
