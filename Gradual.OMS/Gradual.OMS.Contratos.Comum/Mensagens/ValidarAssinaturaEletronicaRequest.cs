using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Solicitação de validação da assinatura eletronica para a
    /// sessão informada
    /// </summary>
    public class ValidarAssinaturaEletronicaRequest : MensagemRequestBase
    {
        /// <summary>
        /// Assinatura eletronica a validar
        /// </summary>
        public string AssinaturaEletronica { get; set; }
    }
}
