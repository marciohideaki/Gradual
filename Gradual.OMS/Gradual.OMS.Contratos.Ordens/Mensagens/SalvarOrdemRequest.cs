using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Solicitação para salvar o detalhe, ou o evento, de uma ordem.
    /// Implementado pelos serviços de persistencia.
    /// Pode ser repassado pelo serviço de ordens.
    [Mensagem(TipoMensagemResponse = typeof(SalvarOrdemResponse))]
    public class SalvarOrdemRequest : MensagemOrdemRequestBase
    {
        /// <summary>
        /// Informações da ordem
        /// </summary>
        public OrdemInfo OrdemInfo { get; set; }

        /// <summary>
        /// Mensagem de sinalização indicando o evento que ocorreu com a ordem
        /// </summary>
        public SinalizarExecucaoOrdemRequest MensagemSinalizacao { get; set; }
    }
}
