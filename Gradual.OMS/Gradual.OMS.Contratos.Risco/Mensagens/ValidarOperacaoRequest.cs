using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de validação da operação.
    /// Executa o pipeline de validação do risco sem executar a operação
    /// </summary>
    public class ValidarOperacaoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Mensagem de execução a ser validada
        /// </summary>
        public MensagemBase Mensagem { get; set; }
    }
}
