using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class RemoverEntidadeCadastroRequest<T> : MensagemRequestBase
    {
        public T EntidadeCadastro { get; set; }

    }
}
