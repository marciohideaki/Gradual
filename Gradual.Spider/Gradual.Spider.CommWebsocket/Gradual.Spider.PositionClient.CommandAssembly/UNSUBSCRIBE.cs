using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperWebSocket.SubProtocol;

namespace Gradual.Spider.PositionClient.CommandAssembly
{
    /// <summary>
    /// Classe responsável pela cancelamento da assniatura da aplicação cliente para consumir via websocket 
    /// os dados de Position Client
    /// Para cancelar a assinatura é necessário o código do cliente para tirar da WebSocketSession
    /// </summary>
    public class UNSUBSCRIBE : SubCommandBase
    {
        /// <summary>
        /// Método que sobrescreve o método já pronto em SubcommandBase para efetuar 
        /// o cancelamento da assinatura e remover o cliente da lista de webSocketSession
        /// </summary>
        /// <param name="pSession">Sessão com a lista de clientes que irá verificada e o cliente solicitado será removido.</param>
        /// <param name="pRequestInfo">Não Está sendo usado</param>
        public override void ExecuteCommand(SuperWebSocket.WebSocketSession pSession, SubRequestInfo pRequestInfo)
        {
            var lParamArray = pRequestInfo.Body.Split(' ');

            if (lParamArray.Length > 0)
            {
                for (int i = 0; i < lParamArray.Length; i++)
                {
                    int lCodigoCliente = Convert.ToInt32( lParamArray[i]);

                    if (pSession.ListClientSubscriptions.Contains(lCodigoCliente))
                    {
                        pSession.ListClientSubscriptions.Remove(lCodigoCliente);
                    }
                }
            }
        }
    }
}
