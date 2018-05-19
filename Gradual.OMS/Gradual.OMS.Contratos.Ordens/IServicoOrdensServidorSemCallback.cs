using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Contratos.Ordens
{
    /// <summary>
    /// Serviço para realizar o gerenciamento de diversos clientes no serviço de ordens.
    /// Não utiliza callback, ou seja, não dispara eventos para o cliente.
    /// </summary>
    [ServiceContract(Namespace = "http://gradual")]
    [ServiceKnownType("RetornarTipos", typeof(OrdensTiposHelper))]
    public interface IServicoOrdensServidorSemCallback 
    {
        /// <summary>
        /// Pede para o serviço de ordens o processamento de uma mensagem
        /// </summary>
        /// <param name="mensagem"></param>
        [OperationContract]
        MensagemResponseBase ProcessarMensagem(MensagemRequestBase mensagem);
    }
}
