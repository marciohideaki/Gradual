using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.Servicos.Contratos.TesteWCF.Dados;

namespace Gradual.Servicos.Contratos.TesteWCF.Mensagens
{
    [Serializable]
    public class SalvarMensagemDeTextoRequest : MensagemRequestBase
    {
        public MensagemTextoInfo MensagemDeTexto { get; set; }
    }
}
