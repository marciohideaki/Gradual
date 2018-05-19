using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Interface.Desktop.Mensagens
{
    /// <summary>
    /// Resposta a uma solicitação de envio de mensagem para controle
    /// </summary>
    public class EnviarMensagemParaControleResponse : MensagemInterfaceResponseBase 
    {
        /// <summary>
        /// Mensagem de resposta retornado pelo controle
        /// </summary>
        public MensagemInterfaceResponseBase MensagemResposta { get; set; }
    }
}
