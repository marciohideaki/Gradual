using System;
using System.Collections.Generic;

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class CadastrarClienteCompletoRequest : BaseRequest
    {
        public CadastrarClienteRequest Cliente { get; set; }

        public List<CadastrarTelefoneRequest> Telefones { get; set; }
    }
}
