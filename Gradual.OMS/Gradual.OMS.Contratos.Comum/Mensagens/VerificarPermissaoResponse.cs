using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma consulta de permissão por sessão.
    /// </summary>
    public class VerificarPermissaoResponse : MensagemResponseBase
    {
        /// <summary>
        /// Indica se a permissão é ou não permitida para a sessão
        /// </summary>
        public bool Permitido { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public VerificarPermissaoResponse()
        {
        }
    }
}
