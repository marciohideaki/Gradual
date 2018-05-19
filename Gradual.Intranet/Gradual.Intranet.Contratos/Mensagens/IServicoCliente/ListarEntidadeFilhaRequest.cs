using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class ListarEntidadeFilhaRequest : BaseRequest
    {
        #region Propriedades

        public EntidadesCliente NomeEntidade { get; set; }

        public int IdCliente { get; set; }

        #endregion
    }
}
