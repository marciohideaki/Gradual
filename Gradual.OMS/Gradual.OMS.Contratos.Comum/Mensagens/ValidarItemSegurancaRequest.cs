using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de validação de um item de segurança
    /// </summary>
    public class ValidarItemSegurancaRequest : MensagemRequestBase
    {
        /// <summary>
        /// Item de segurança a ser validado
        /// </summary>
        public List<ItemSegurancaInfo> ItensSeguranca { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ValidarItemSegurancaRequest()
        {
            this.ItensSeguranca = new List<ItemSegurancaInfo>();
        }
    }
}
