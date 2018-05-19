using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Automacao.Ordens.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Automacao.Ordens.Mensagens
{
    public class ExecutarAutomacaoOrdemResponse : MensagemResponseBase
    {

        /// <summary>
        /// Código do cliente
        /// </summary>
        public string CodigoCliente { get; set; }

        /// <summary>
        /// Código do Sistema Cliente, por exemplo: HB (HomeBroker), Plataforma, Robo, etc.
        /// </summary>
        public string CodigoSistemaCliente { get; set; }

        public string CodigoItemAutomacaoOrdem { get; set; }

        public ExecutarAutomacaoOrdemResponse()
        {
        }
    }
}
