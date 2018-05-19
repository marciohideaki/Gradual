using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação para salvar uma regra de risco
    /// </summary>
    public class SalvarRegraRiscoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Regra de risco a ser salva
        /// </summary>
        public RegraRiscoInfo RegraRiscoInfo { get; set; }
    }
}
