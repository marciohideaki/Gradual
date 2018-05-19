using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class SalvarEntidadeCadastroResponse : MensagemResponseBase
    {
        public object Objeto { get; set; }
    }
}
