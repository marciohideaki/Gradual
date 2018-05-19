using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    public class ListarTiposResponse
    {
        public List<Type> Resultado { get; set; }

        public ListarTiposResponse()
        {
            this.Resultado = new List<Type>();
        }
    }
}
