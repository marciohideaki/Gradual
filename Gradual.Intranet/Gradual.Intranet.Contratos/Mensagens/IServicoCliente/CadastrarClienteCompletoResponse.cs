using System;
using System.Collections.Generic;

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class CadastrarClienteCompletoResponse : BaseResponse
    {
        #region Propriedades

        public CadastrarClienteResponse ResultadoCadastroCliente { get; set; }

        public List<CadastrarTelefoneResponse> ResultadoCadastroTelefones { get; set; }

        #endregion
    }
}
