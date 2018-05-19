using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

using Gradual.OMS.Library;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Comum
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
