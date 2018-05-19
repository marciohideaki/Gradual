using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Solicitação para salvar a mensagem.
    public class SalvarMensagemRequest 
    {
        /// <summary>
        /// Informações da mensagem
        /// </summary>
        public MensagemBase Mensagem { get; set; }
    }
}
