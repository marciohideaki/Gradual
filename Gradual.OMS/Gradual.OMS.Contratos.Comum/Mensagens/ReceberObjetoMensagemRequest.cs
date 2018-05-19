using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    public class ReceberObjetoMensagemRequest : ReceberObjetoRequest<MensagemBase>
    {
        public string Teste { get; set; }
    }
}
