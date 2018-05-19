using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Contratos.CanaisNegociacao
{
    /// <summary>
    /// Interface para o serviço de canais.
    /// Implementa funções controle de vida de serviços (iniciar, parar) e a função 
    /// para envio de mensagens para o canal.
    /// Quando este serviço inicia, lê arquivo de configurações para saber quais 
    /// canais irão ser iniciados.
    /// O retorno das mensagens do canal é feito diretamente do canal para o serviço 
    /// de ordens, não passando pelo serviço de canais.
    /// </summary>
    [ServiceContract(Namespace = "http://gradual")]
    public interface IServicoCanaisNegociacao : IServicoControlavel
    {
        /// <summary>
        /// Envia a mensagem para o canal solicitado.
        /// </summary>
        /// <param name="codigoCanal">Código do canal para o qual a mensagem deve ser enviada</param>
        /// <param name="mensagem">Mensagem a ser enviada</param>
        [OperationContract]
        void EnviarMensagem(string codigoCanal, MensagemRequestBase mensagem);

        // TODO - Criar evento para retornar as mensagens recebidas pelo canal
    }
}
