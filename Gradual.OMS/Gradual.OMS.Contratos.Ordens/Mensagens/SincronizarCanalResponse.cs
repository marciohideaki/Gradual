using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SincronizarCanalResponse : MensagemOrdemResponseBase
    {
        public string DescricaoStatusSolicitacao { get; set; }
    }
}
