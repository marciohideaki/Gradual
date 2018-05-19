using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.ContaCorrente.Dados;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitacao de sincronizacao de conta
    /// corrente com o sinacor
    /// </summary>
    public class SincronizarContaCorrenteSinacorResponse : MensagemResponseBase
    {
        /// <summary>
        /// Conta corrente atualizada
        /// </summary>
        public ContaCorrenteInfo ContaCorrente { get; set; }
    }
}
