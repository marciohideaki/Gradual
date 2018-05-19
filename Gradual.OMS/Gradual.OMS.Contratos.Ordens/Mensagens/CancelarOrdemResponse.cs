using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Resposta de uma mensagem CancelarOrdemRequest.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CancelarOrdemResponse : MensagemOrdemResponseBase
    {
        public string DescricaoStatusSolicitacao { get; set; }
    }
}
