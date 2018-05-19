using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Solicitação para receber o detalhe de uma ordem.
    /// Implementado pelos serviços de persistencia.
    /// Pode ser repassado pelo serviço de ordens.
    /// </summary>
    [Mensagem(TipoMensagemResponse = typeof(ReceberOrdemResponse))]
    public class ReceberOrdemRequest : MensagemOrdemRequestBase
    {
        /// <summary>
        /// ClOrdID que se deseja recuperar
        /// </summary>
        public string ClOrdID { get; set; }

        /// <summary>
        /// Permite recuperar a ordem pelo código externo
        /// </summary>
        public string CodigoExterno { get; set; }
    }
}
