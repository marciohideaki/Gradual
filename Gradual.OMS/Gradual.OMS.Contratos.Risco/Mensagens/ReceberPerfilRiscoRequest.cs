using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de detalhe de um Perfil de risco
    /// </summary>
    public class ReceberPerfilRiscoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código do Perfil de risco desejado
        /// </summary>
        public string CodigoPerfilRisco { get; set; }
    }
}
