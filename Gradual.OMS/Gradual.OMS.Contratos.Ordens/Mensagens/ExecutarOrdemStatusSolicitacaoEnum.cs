using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Enumerador que indica os possíveis status de solicitação de ordens. 
    /// Sinaliza, por exemplo, que o risco invalidou a ordem e ela nem 
    /// chegou a ir para a bolsa.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public enum ExecutarOrdemStatusSolicitacaoEnum
    {
        EmExecucao,
        Invalido,
        InvalidoRisco
    }
}
