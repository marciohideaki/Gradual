using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Automacao.Ordens.Dados;

namespace Gradual.OMS.Contratos.Automacao.Ordens.Mensagens
{
    [Serializable]
    public class SelecionarOrdemResponse : MensagemResponseClienteBase
    {
        public AutomacaoOrdensInfo AutomacaoOrdem { get; set; }
    }
}
