using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens.Dados;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Resposta de uma solicitação de lista de ordens
    /// </summary>
    public class ListarOrdensResponse : MensagemOrdemResponseBase
    {
        public List<OrdemInfo> Ordens { get; set; }
    }
}
