using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class CadastrarEntidadeFilhaRequest : BaseRequest
    {
        #region Propriedades

        public EntidadesCliente NomeEntidade { get; set; }

        public Nullable<int> IdEntidade { get; set; }

        public int IdCliente { get; set; }

        //public Dictionary<string, object> Valores { get; set; }

        public ClienteBancoInfo eClienteBanco { get; set; }

        

        #endregion
    }
}
