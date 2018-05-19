

using Gradual.OMS.Library;
namespace Gradual.OMS.Mensageria
{
    /// <summary>
    /// Mensagem de solicitação de validação de mensagem
    /// </summary>
    public class ValidarMensagemRequest : MensagemRequestBase
    {
        /// <summary>
        /// Mensagem a ser validada
        /// </summary>
        public MensagemBase Mensagem { get; set; }

        /// <summary>
        /// Informações da sessão que está executando a mensagem
        /// </summary>
        //public SessaoInfo SessaoInfo { get; set; }
    }
}
