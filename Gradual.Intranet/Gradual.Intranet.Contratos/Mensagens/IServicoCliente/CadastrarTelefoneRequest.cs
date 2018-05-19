using System;

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class CadastrarTelefoneRequest : BaseRequest
    {
        #region Propriedades

        public Nullable<int> IdTelefone { get; set; }

        public int IdCliente { get; set; }

        public int IdTipoTelefone { get; set; }

        public bool Principal { get; set; }

        public int DDD { get; set; }

        public int Ramal { get; set; }

        public int Numero { get; set; }

        #endregion
    }
}
