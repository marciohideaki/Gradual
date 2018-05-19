using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class SalvarEntidadeCadastroRequest<T> : MensagemRequestBase
    {
        public T EntidadeCadastro { get; set; }

    }
}
