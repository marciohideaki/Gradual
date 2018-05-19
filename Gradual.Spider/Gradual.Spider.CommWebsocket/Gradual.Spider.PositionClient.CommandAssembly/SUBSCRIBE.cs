using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperWebSocket.SubProtocol;
using SuperWebSocket;
using Gradual.Spider.PositionClient.Monitor;
using Gradual.Spider.PositionClient.Monitor.Lib.Message;

namespace Gradual.Spider.PositionClient.CommandAssembly
{
    /// <summary>
    /// Classe responsável pela assniatura da aplicação cliente para consumir via websocket 
    /// os dados de Position Client
    /// A assinatura é é o código do cliente
    /// </summary>
    public class SUBSCRIBE : SubCommandBase
    {
        /// <summary>
        /// Atributo de envio de Mensagem de Position Cliente para o 
        /// cliente que acabou de conectar.
        /// </summary>
        public event EventHandler<MessagePositionClientArgs> SendMessageClientConnected;

        /// <summary>
        /// Classe estática com a instância do monitor de Position Client que armazena
        /// todas os snapshots de position client.
        /// As chamadas para dessa instancia devem estar em 
        /// </summary>
        private static PositionClientMonitor _ClientMonitor = new PositionClientMonitor();

        /// <summary>
        /// Método que sobrescreve o método já pronto em SubcommandBase para efetuar 
        /// a assinatura e gravar do cliente no WebSocketSession para ser gerenciado pelo serviço
        /// </summary>
        /// <param name="pSession">Sessão que ira ser armazenado o código do cliente querendo assinatura de WebSocket</param>
        /// <param name="pRequestInfo">(Não está sendo usado)</param>
        public override void ExecuteCommand(SuperWebSocket.WebSocketSession pSession, SubRequestInfo pRequestInfo)
        {
            var lParamArray = pRequestInfo.Body.Split(' ');

            if (lParamArray.Length > 0)
            {
                for (int i = 0; i < lParamArray.Length; i++)
                {
                    int lCodigoCliente = Convert.ToInt32(lParamArray[i]);

                    if (!pSession.ListClientSubscriptions.Contains(lCodigoCliente))
                    {
                        pSession.ListClientSubscriptions.Add(lCodigoCliente);

                        SendMessageClientConnected += _ClientMonitor.SendMessageMemoryPositionClientToQueue;

                        EventHandler<MessagePositionClientArgs> lHandler = SendMessageClientConnected;

                        if (lHandler != null)
                        {
                            var e = new MessagePositionClientArgs();

                            e.Session = pSession;

                            e.CodigoCliente = lCodigoCliente;

                            lHandler(this, e);
                        }

                        SendMessageClientConnected -= _ClientMonitor.SendMessageMemoryPositionClientToQueue;
                    }
                }
            }
        }
    }
}
