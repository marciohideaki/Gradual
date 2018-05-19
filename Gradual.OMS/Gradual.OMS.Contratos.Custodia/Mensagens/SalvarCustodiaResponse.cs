using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Custodia.Dados;

namespace Gradual.OMS.Contratos.Custodia.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de salvar custódia
    /// </summary>
    public class SalvarCustodiaResponse : MensagemResponseBase
    {
        /// <summary>
        /// Custódia salva
        /// </summary>
        public CustodiaInfo Custodia { get; set; }
    }
}
