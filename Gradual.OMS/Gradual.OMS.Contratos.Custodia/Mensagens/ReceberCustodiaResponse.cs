using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Custodia.Dados;

namespace Gradual.OMS.Contratos.Custodia.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de receber custódia
    /// </summary>
    public class ReceberCustodiaResponse : MensagemResponseBase
    {
        public CustodiaInfo CustodiaInfo { get; set; }
    }
}
