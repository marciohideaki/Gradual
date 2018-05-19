using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.ContaCorrente.Dados;

namespace Gradual.OMS.Contratos.ContaCorrente.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de salvar conta corrente
    /// </summary>
    public class SalvarContaCorrenteResponse : MensagemResponseBase
    {
        /// <summary>
        /// Conta corrente salva
        /// </summary>
        public ContaCorrenteInfo ContaCorrente { get; set; }
    }
}
