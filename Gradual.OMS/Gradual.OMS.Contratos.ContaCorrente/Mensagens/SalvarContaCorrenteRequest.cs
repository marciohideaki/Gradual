using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.ContaCorrente.Dados;

namespace Gradual.OMS.Contratos.ContaCorrente.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação para salvar conta corrente
    /// </summary>
    public class SalvarContaCorrenteRequest : MensagemRequestBase
    {
        /// <summary>
        /// Conta corrente a ser salva
        /// </summary>
        public ContaCorrenteInfo ContaCorrenteInfo { get; set; }
    }
}
