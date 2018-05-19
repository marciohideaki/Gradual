using Gradual.OMS.Library;

namespace Gradual.OMS.Persistencia
{
    /// <summary>
    /// Solicitação para salvar a mensagem.
    public class SalvarMensagemRequest 
    {
        /// <summary>
        /// Informações da mensagem
        /// </summary>
        public MensagemBase Mensagem { get; set; }
    }
}
