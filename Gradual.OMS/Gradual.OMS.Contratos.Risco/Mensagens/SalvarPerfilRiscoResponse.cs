using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de salvar perfil de risco
    /// </summary>
    public class SalvarPerfilRiscoResponse : MensagemResponseBase
    {
        /// <summary>
        /// Perfil de risco salvo
        /// </summary>
        public PerfilRiscoInfo PerfilRisco { get; set; }
    }
}
