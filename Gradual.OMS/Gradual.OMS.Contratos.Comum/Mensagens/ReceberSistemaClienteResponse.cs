using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de receber sistema cliente
    /// </summary>
    public class ReceberSistemaClienteResponse : MensagemResponseBase
    {
        /// <summary>
        /// Sistema cliente encontrado
        /// </summary>
        public SistemaClienteInfo SistemaCliente { get; set; }
    }
}
