using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Comum
{
    /// <summary>
    /// Interface do serviço responsável por realizar a autenticação do usuário.
    /// Está como um serviço separado pois irá subir num canal separado com 
    /// características próprias, como comunicação segura, etc.
    /// </summary>
    [ServiceContract(Namespace = "http://gradual")]
    [ServiceKnownType("RetornarTipos", typeof(LocalizadorTiposHelper))]
    public interface IServicoAutenticador
    {
        /// <summary>
        /// Processa a mensagem solicitada
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [OperationContract]
        MensagemResponseBase ProcessarMensagem(MensagemRequestBase parametros);
    }
}
