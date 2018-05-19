using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Lista de status de mensagens
    /// </summary>
    public enum MensagemStatusEnum
    {
        /// <summary>
        /// Indica que a solicitação foi feita. Pode indicar uma espécie de pendência.
        /// </summary>
        SolicitacaoEfetuada,

        /// <summary>
        /// Indica que esta mensagem foi respondida por outra.
        /// </summary>
        Respondida,

        /// <summary>
        /// Indica que esta mensagem foi respondida por mais de uma mensagem.
        /// </summary>
        RespondidaMaisDeUmaVez,

        /// <summary>
        /// Indica que está mensagem expirou.
        /// </summary>
        Expirada,

        /// <summary>
        /// Indica que esta mensagem foi cancelada por outra mensagem
        /// </summary>
        CanceladaPorOutraMensagem,

        /// <summary>
        /// Indica que o status não se aplica a este tipo de mensagem.
        /// Mensagens de sinalização irão carregar este status.
        /// </summary>
        NaoSeAplica
    }
}
