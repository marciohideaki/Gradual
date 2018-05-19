using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de remoção de sessao
    /// </summary>
    [Serializable]
    public class RemoverSessaoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código da sessao a ser removida
        /// </summary>
        public string CodigoSessao { get; set; }
    }
}
