using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.OMS.Persistencia
{
    public class ConsultarObjetosResponse<T> : MensagemResponseBase
    {
        public List<T> Resultado { get; set; }

        public ConsultarObjetosResponse()
        {
            this.Resultado = new List<T>();
        }
    }
}
