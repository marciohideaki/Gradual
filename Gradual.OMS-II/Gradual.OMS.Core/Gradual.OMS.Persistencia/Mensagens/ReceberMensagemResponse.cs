using Gradual.OMS.Library;

namespace Gradual.OMS.Persistencia
{
    /// <summary>
    /// Resposta a uma solicitação de recebimento de mensagem
    /// </summary>
    public class ReceberMensagemResponse : MensagemResponseBase
    {
        /// <summary>
        /// Mensagem recuperada
        /// </summary>
        public MensagemBase Mensagem { get; set; }
    }
}
