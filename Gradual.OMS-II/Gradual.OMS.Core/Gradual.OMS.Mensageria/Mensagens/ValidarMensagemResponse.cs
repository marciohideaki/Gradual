

using Gradual.OMS.Library;
namespace Gradual.OMS.Mensageria
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de validação de mensagem
    /// </summary>
    public class ValidarMensagemResponse : MensagemResponseBase
    {
        /// <summary>
        /// Informa o contexto utilizado na validação da mensagem.
        /// </summary>
        public ContextoValidacaoInfo ContextoValidacao { get; set; }
    }
}
