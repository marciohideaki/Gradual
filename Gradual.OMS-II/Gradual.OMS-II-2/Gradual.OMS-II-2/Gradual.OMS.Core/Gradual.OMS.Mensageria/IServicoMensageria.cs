using System.ServiceModel;

using Gradual.OMS.Library;

namespace Gradual.OMS.Mensageria
{
    /// <summary>
    /// Interface para o serviço de mensageria
    /// </summary>
    [ServiceContract(Namespace = "http://gradual")]
    [ServiceKnownType("RetornarTipos", typeof(LocalizadorTiposHelper))]
    public interface IServicoMensageria
    {
        /// <summary>
        /// Processa a mensagem solicitada.
        /// Faz o roteamento da mensagem para o devido serviço
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [OperationContract]
        MensagemResponseBase ProcessarMensagem(MensagemRequestBase parametros);
    }
}
