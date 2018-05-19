using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Custodia.Dados;

namespace Gradual.OMS.Contratos.Custodia.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de salvar custódia
    /// </summary>
    public class SalvarCustodiaRequest : MensagemRequestBase
    {
        /// <summary>
        /// Custodia a ser salva
        /// </summary>
        public CustodiaInfo CustodiaInfo { get; set; }
    }
}
