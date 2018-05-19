using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de detalhe de Perfil de risco
    /// </summary>
    public class ReceberPerfilRiscoResponse : MensagemResponseBase
    {
        /// <summary>
        /// Perfil de risco encontrado
        /// </summary>
        public PerfilRiscoInfo PerfilRiscoInfo { get; set; }
    }
}
