using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Mensagem de resposta de ExecutarOrdemRequest
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ExecutarOrdemResponse : MensagemOrdemResponseBase
    {
        public ExecutarOrdemStatusSolicitacaoEnum StatusSolicitacao { get; set; }
        public string DescricaoStatusSolicitacao { get; set; }
    }
}
