using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de responsta a uma solicitação de detalhe de sessao
    /// </summary>
    [Serializable]
    public class ReceberSessaoResponse : MensagemResponseBase
    {
        /// <summary>
        /// Sessao encontrada
        /// </summary>
        public SessaoInfo Sessao { get; set; }

        /// <summary>
        /// Opcionalmente pode retornar o usuário
        /// </summary>
        public UsuarioInfo Usuario { get; set; }
    }
}
