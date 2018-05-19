using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.OMS.Persistencia
{
    /// <summary>
    /// Resposta de uma solicitação de lista de mensagens
    /// </summary>
    public class ListarMensagensResponse : MensagemResponseBase
    {
        public List<MensagemBase> Mensagens { get; set; }
    }
}
