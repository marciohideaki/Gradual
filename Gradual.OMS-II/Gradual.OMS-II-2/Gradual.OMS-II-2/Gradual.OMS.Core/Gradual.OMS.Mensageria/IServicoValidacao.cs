
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Mensageria
{
    /// <summary>
    /// Interface para o serviço de validação.
    /// </summary>
    public interface IServicoValidacao : IServicoControlavel
    {
        /// <summary>
        /// Solicita que uma mensagem passe pelo pipeline de validação.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ValidarMensagemResponse ValidarMensagem(ValidarMensagemRequest parametros);
    }
}
