using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de validação de um item de
    /// segurança.
    /// </summary>
    public class ValidarItemSegurancaResponse : MensagemResponseBase
    {
        /// <summary>
        /// Contem a lista de itens validados
        /// </summary>
        public List<ItemSegurancaInfo> ItensSeguranca { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ValidarItemSegurancaResponse()
        {
            this.ItensSeguranca = new List<ItemSegurancaInfo>();
        }
    }
}
