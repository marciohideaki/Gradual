using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Risco.Enum
{
    /// <summary>
    /// Enumerador de direção da ordem: compra ou venda.
    /// </summary>
    [Serializable]
    public enum CriticaRiscoEnum
    {
        PermissaoInsuficiente,
        ParametroLimiteExcedido,
        DescricaoCritica,
        Exception,
        Sucesso
    }
}
