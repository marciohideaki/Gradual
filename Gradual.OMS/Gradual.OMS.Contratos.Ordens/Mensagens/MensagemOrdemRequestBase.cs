using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação base para o sistema de ordens
    /// </summary>
    [Serializable]
    public class MensagemOrdemRequestBase : MensagemRequestBase
    {
        /// <summary>
        /// Indica o status da mensagem, como por exemplo, se a mensagem foi respondida,
        /// cancelada, está expirada, etc.
        /// </summary>
        [Category("MensagemRequestBase")]
        public MensagemStatusEnum Status { get; set; }

        /// <summary>
        /// Código do cliente
        /// </summary>
        [Category("MensagemClienteBase")]
        public string CodigoCliente { get; set; }

        /// <summary>
        /// Codigo do usuario
        /// </summary>
        public string CodigoUsuario { get; set; }

        /// <summary>
        /// Indica se esta mensagem é algum tipo de retorno a outra mensagem.
        /// Nesses casos, este campo indica a mensagem a que esta se refere.
        /// </summary>
        [Category("MensagemBase")]
        public string CodigoMensagemReferencia { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public MensagemOrdemRequestBase()
        {
            // Seta o status default
            this.Status = MensagemStatusEnum.NaoSeAplica;
        }
    }
}
