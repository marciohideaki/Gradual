using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;


namespace Gradual.Intranet.Contratos.Mensagens
{
    public class ConsultarEntidadeCadastroResponse<T> : MensagemResponseBase
    {
        public List<T> Resultado { get; set; }

        public ConsultarEntidadeCadastroResponse()
        {
            this.Resultado = new List<T>();
        }
    }
}
