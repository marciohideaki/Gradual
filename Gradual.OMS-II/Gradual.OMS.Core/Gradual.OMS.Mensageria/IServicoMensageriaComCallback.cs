using System.ServiceModel;

using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Mensageria
{
    /// <summary>
    /// Interface para o serviço de mensageria com callbacks
    /// </summary>
    [ServiceContract(Namespace = "http://gradual", CallbackContract = typeof(ICallbackEvento))]
    [ServiceKnownType("RetornarTipos", typeof(LocalizadorTiposHelper))]
    public interface IServicoMensageriaComCallback : IServicoComCallback
    {
        /// <summary>
        /// Processa a mensagem solicitada.
        /// Faz o roteamento da mensagem para o devido serviço
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [OperationContract]
        MensagemResponseBase ProcessarMensagem(MensagemRequestBase parametros);

        /// <summary>
        /// Solicita a assinatura de um evento de serviço
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [OperationContract]
        AssinarEventoResponse AssinarEvento(AssinarEventoRequest parametros);
    }
}
