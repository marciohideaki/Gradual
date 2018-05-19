using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de salvar regra de risco
    /// </summary>
    public class SalvarRegraRiscoResponse : MensagemResponseBase
    {
        /// <summary>
        /// Regra de risco salva
        /// </summary>
        public RegraRiscoInfo RegraRisco { get; set; }
    }
}
