using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.Intranet.Contratos.Permissoes.Clientes
{
    /// <summary>
    /// Permite Efetivar a sincronização de clientes com o Sinacor
    /// </summary>
    [Permissao(
    "22EDA2C0-8036-4833-A80A-0536F36889EA"
    , "Cliente - Sincronizar com SINACOR"
    , "Permite Efetivar a sincronização de clientes com o Sinacor")]
    public class PermissaoClienteSincronizarSinacorExecutar : PermissaoBase
    {
    }
}
