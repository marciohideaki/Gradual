using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Custodia.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de receber detalhe de custódia
    /// </summary>
    public class ReceberCustodiaRequest : MensagemRequestBase
    {
        /// <summary>
        /// Codigo da custodia a ser retornada
        /// </summary>
        public string CodigoCustodia { get; set; }

        /// <summary>
        /// Indica se o sistema de custodia deve carregar as cotações atuais
        /// efetuar a marcação a mercado
        /// </summary>
        public bool CarregarCotacoes { get; set; }
    }
}
