using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Sistemas.Comum
{
    /// <summary>
    /// Classe de eventargs para permitir que o hook de arquivo interfira
    /// no processamento das mensagens
    /// </summary>
    public class PersistenciaArquivoEventoEventArgs : EventArgs
    {
        /// <summary>
        /// Mensagem de request.
        /// Esta foi a mensagem que chegou para a persistencia de arquivo e que ela
        /// está dando a oportunidade do hook processar
        /// </summary>
        public object MensagemRequest { get; set; }

        /// <summary>
        /// Mensagem de response.
        /// Caso o hook queira processar a mensagem, ele deverá responder aqui.
        /// </summary>
        public object MensagemResponse { get; set; }
    }
}
