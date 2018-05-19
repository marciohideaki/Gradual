using Gradual.OMS.Library;

namespace Gradual.OMS.Persistencia
{
    /// <summary>
    /// Mensagem de solicitação de lista de mensagens.
    /// Esta função é implementada pelos serviços de persistencia. 
    /// </summary>
    public class ListarMensagensRequest : MensagemRequestBase
    {
        /// <summary>
        /// Filtro por codigo de mensagem referencia
        /// </summary>
        public string FiltroCodigoMensagemReferencia { get; set; }
    }
}
