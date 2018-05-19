using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de recuperar os caches de risco 
    /// de um cliente
    /// </summary>
    public class ReceberCacheRiscoResponse : MensagemResponseBase
    {
        /// <summary>
        /// Cache de risco do usuário solicitado
        /// </summary>
        public CacheRiscoInfo CacheRisco { get; set; }
    }
}
