using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.ContaCorrente.Dados;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de sincronização de conta corrente
    /// </summary>
    public class SincronizarContaCorrenteResponse : MensagemResponseBase
    {
        /// <summary>
        /// Retorna a conta corrente atualizada
        /// </summary>
        public ContaCorrenteInfo ContaCorrente { get; set; }
    }
}
