using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Spider.PositionClient.Monitor.Transporte;

namespace Gradual.Spider.PositionClient.Monitor.Message
{
    /// <summary>
    /// Classe de arguments de position client
    /// Usada para passar a classe como parametro para a mensagem ser enviadas 
    /// aos aplcativos conectados no websocket
    /// </summary>
    public class MessagePositionClientArgs : EventArgs
    {
        public int CodigoCliente                { get; set; }

        public TransportePositionClient Message { get; set; }
    }
}
