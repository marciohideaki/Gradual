using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.Servicos.Contratos.TesteWCF.Dados;

namespace Gradual.Servicos.Contratos.TesteWCF.Mensagens
{
    public class ReceberMensagemDeTextoResponse : MensagemResponseBase
    {
        public MensagemTextoInfo MensagemTexto { get; set; }
    }
}
