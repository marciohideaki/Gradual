using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Pede para o canal atualizar sincronizar a lista de mensagens.
    /// Equivalente à mensagem ResendRequest do Fix. O QuickFix, quando recebe retransmissão de mensagens, 
    /// não repassa as mensagens para a aplicação. Ele repassa apenas mensagens que ele ainda não enviou 
    /// para a aplicação, ou seja, ele faz o tratamento de retransmissão de mensagens de forma que esta 
    /// função perde efeito na aplicação.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Mensagem(TipoMensagemResponse = typeof(SinalizarMensagemInvalidaResponse))]
    public class SincronizarCanalRequest : MensagemOrdemRequestBase
    {
        public string CodigoCanal { get; set; }
    }
}
