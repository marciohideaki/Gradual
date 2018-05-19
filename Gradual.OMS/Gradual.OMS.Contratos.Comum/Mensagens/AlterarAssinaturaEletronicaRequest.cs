using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de alteração de assinatura eletrônica
    /// </summary>
    public class AlterarAssinaturaEletronicaRequest : MensagemRequestBase
    {
        /// <summary>
        /// Nova assinatura eletronica
        /// </summary>
        public string NovaAssinaturaEletronica { get; set; }
    }
}
