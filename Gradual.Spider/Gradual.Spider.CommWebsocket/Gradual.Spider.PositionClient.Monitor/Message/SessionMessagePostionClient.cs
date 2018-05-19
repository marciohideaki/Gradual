using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperWebSocket;

namespace Gradual.Spider.PositionClient.Monitor.Message
{
    /// <summary>
    /// Classe de controle de sessão de cada mensagem que chega de position client
    /// </summary>
    public class SessionMessagePostionClient
    {
        /// <summary>
        /// Propriedade de controle de sessão para envio das mensagens de position client
        /// </summary>
        public WebSocketSession Session { get; set; }

        /// <summary>
        /// Propriedade de envio de mensagem de position client no tipo de array de bytes
        /// </summary>
        public byte[] MessageByte       { get; set; }

        /// <summary>
        /// Priedade de enviode mensagem de position client em string
        /// </summary>
        public string MessageString     { get; set; }

    }
}
