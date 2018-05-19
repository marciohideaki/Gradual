using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.Comum.Contratos.Mensagens
{
    /// <summary>
    /// Classe base para mensagens.
    /// </summary>
    public abstract class MensagemBase
    {
        /// <summary>
        /// Código da mensagem.
        /// </summary>
        public string CodigoMensagem { get; set; }

        /// <summary>
        /// Data de referência ou de criação da mensagem.
        /// </summary>
        public DateTime DataReferencia { get; set; }
    }
}
