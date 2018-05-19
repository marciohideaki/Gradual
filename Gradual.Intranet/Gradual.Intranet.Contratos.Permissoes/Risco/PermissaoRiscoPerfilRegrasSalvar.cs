using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Risco
{
    /// <summary>
    /// Permite Salvar Regras do Perfil de Risco
    /// </summary>
    [Permissao(
    "{ED7CD91C-3153-4A84-AE3E-5C3F6B795A01}"
    , "Risco - Salvar Regras do Perfil de Risco"
    , "Permite Salvar Regras do Perfil de Risco")]
    public class PermissaoRiscoPerfilRegrasSalvar : PermissaoBase
    {
    }
}
