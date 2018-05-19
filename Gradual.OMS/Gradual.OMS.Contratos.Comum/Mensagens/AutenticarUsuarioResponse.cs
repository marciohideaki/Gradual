using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de autenticação de usuário
    /// </summary>
    public class AutenticarUsuarioResponse : MensagemResponseBase
    {
        /// <summary>
        /// Contem informações da sessão criada
        /// </summary>
        public SessaoInfo Sessao { get; set; }
    }
}
