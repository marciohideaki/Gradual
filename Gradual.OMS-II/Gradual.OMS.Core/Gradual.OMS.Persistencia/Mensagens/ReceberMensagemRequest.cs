using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Persistencia
{
    /// <summary>
    /// Solicitação para receber o detalhe de uma mensagem.
    /// Implementado pelos serviços de persistencia.
    /// Pode ser repassado pelo serviço de ordens.
    /// </summary>
    public class ReceberMensagemRequest 
    {
        /// <summary>
        /// Código da mensagem que se deseja recuperar
        /// </summary>
        public string CodigoMensagem { get; set; }
    }
}
