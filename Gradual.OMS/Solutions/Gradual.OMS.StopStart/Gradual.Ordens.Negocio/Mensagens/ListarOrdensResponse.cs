using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Automacao.Ordens.Dados;

namespace Gradual.OMS.Automacao.Ordens.Mensagens
{
    [Serializable]
    public class ListarOrdensResponse : MensagemResponseClienteBase
    {
        public List<AutomacaoOrdensInfo> ListaDeAutomacaoOrdens { get; set; }
    }   
}
