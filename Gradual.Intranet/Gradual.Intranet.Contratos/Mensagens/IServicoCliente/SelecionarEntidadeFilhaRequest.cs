using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class SelecionarEntidadeFilhaRequest : BaseRequest
    {
        #region Propriedades

        public EntidadesCliente NomeEntidade { get; set; }

        public int IdEntidade { get; set; }

        #endregion
    }
}
