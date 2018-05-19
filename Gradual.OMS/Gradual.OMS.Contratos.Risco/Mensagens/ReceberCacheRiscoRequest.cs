using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de recuperação dos caches de risco do cliente
    /// </summary>
    public class ReceberCacheRiscoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código de usuário do cliente
        /// </summary>
        public string CodigoUsuario { get; set; }
    }
}
