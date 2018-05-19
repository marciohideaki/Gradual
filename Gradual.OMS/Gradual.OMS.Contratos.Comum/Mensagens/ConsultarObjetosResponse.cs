using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    public class ConsultarObjetosResponse<T>
    {
        public List<T> Resultado { get; set; }

        public ConsultarObjetosResponse()
        {
            this.Resultado = new List<T>();
        }
    }
}
