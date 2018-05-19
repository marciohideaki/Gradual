using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.CanaisNegociacao
{
    /// <summary>
    /// Classe de eventArgs para passar mensagem recebida pelo canal
    /// </summary>
    public class MensagemRecebidaEventArgs : EventArgs
    {
        /// <summary>
        /// Mensagem recebida
        /// </summary>
        public MensagemRequestBase Mensagem { get; set; }
    }
}
