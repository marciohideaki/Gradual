using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens.Dados;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Utilizada pelos canais para sinalizar Rejeição de Cancelamento de Ordem.
    /// Equivalente à mensagem OrderCancelReject do FIX.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Mensagem(TipoMensagemResponse = typeof(SinalizarRejeicaoCancelamentoOrdemResponse))]
    public class SinalizarRejeicaoCancelamentoOrdemRequest : MensagemSinalizacaoBase
    {
        #region Bloco OrderCancelReject

        /// <summary>
        /// Código da bolsa no qual a ordem deve ser enviada.
        /// </summary>
        [Category("SinalizarRejeicaoCancelamentoOrdem")]
        [Description("Código da bolsa no qual a ordem deve ser enviada.")]
        public string CodigoBolsa { get; set; }

        /// <summary>
        /// Bovespa:
        /// Se CxIRejReason = ordem desconhecida, o valor desse campo será 'nenhum', caso o cancelamento seja 
        /// feito via OrigClOrdID, ou o valor do campo OrderID da solicitação original de cancelamento ou 
        /// modificação. Do contrário, será o identificador único atribuído pela BM&FBOVESPA. A unicidade 
        /// é garantida para o mesmo dia de pregão / instrumento.
        /// </summary>
        [Category("SinalizarRejeicaoCancelamentoOrdem")]
        [Description("Bovespa: Se CxIRejReason = ordem desconhecida, o valor desse campo será 'nenhum', caso o cancelamento seja feito via OrigClOrdID, ou o valor do campo OrderID da solicitação original de cancelamento ou modificação. Do contrário, será o identificador único atribuído pela BM&FBOVESPA. A unicidade é garantida para o mesmo dia de pregão / instrumento.")]
        public string OrderID { get; set; }

        /// <summary>
        /// Bovespa:
        /// Identificador único de ordem atribuído pela instituição para a solicitação de cancelamento
        /// </summary>
        [Category("SinalizarRejeicaoCancelamentoOrdem")]
        [Description("Bovespa: Identificador único de ordem atribuído pela instituição para a solicitação de cancelamento")]
        public string ClOrdID 
        {
            get { return base.CodigoMensagemReferencia; }
            set { base.CodigoMensagemReferencia = value; } 
        }

        /// <summary>
        /// Bovespa:
        /// ClOrdID que não pôde ser cancelado / substituído
        /// </summary>
        [Category("SinalizarRejeicaoCancelamentoOrdem")]
        [Description("Bovespa: ClOrdID que não pôde ser cancelado / substituído")]
        public string OrigClOrdID { get; set; }

        /// <summary>
        /// Bovespa:
        /// Valor de OrdStatus após a aplicação da rejeição de cancelamento. Se CxIRejReason = ordem desconhecida, o valor desse campo será 'rejeitada'.
        /// </summary>
        [Category("SinalizarRejeicaoCancelamentoOrdem")]
        [Description("Bovespa: Valor de OrdStatus após a aplicação da rejeição de cancelamento. Se CxIRejReason = ordem desconhecida, o valor desse campo será 'rejeitada'.")]
        public string OrdStatus { get; set; }

        /// <summary>
        /// Bovespa:
        /// Ponta da oferta a ser cancelada. Valores aceitos: 1 = compra; 2 = venda
        /// </summary>
        [Category("SinalizarRejeicaoCancelamentoOrdem")]
        [Description("Bovespa: Ponta da oferta a ser cancelada. Valores aceitos: 1 = compra; 2 = venda")]
        public OrdemDirecaoEnum Side { get; set; }

        /// <summary>
        /// Bovespa:
        /// Identifica o tipo de solicitação a que a rejeição de cancelamento se refere. Valores aceitos: 
        /// 1 = solicitação de cancelamento de ordem; 2 = solicitação de cancelamento / substituição
        /// </summary>
        [Category("SinalizarRejeicaoCancelamentoOrdem")]
        [Description("Bovespa: Identifica o tipo de solicitação a que a rejeição de cancelamento se refere. Valores aceitos: 1 = solicitação de cancelamento de ordem; 2 = solicitação de cancelamento / substituição")]
        public string CxIRejResponseTo { get; set; }

        /// <summary>
        /// Bovespa:
        /// Código que identifica o motivo da rejeição de cancelamento. Valores aceitos: 0 = tarde demais para cancelar; 
        /// 1 = oferta desconhecida; 2 = opção do corretor; 99 = outros
        /// </summary>
        [Category("SinalizarRejeicaoCancelamentoOrdem")]
        [Description("Bovespa: Código que identifica o motivo da rejeição de cancelamento. Valores aceitos: 0 = tarde demais para cancelar; 1 = oferta desconhecida; 2 = opção do corretor; 99 = outros")]
        public string CxIRejReason { get; set; }

        /// <summary>
        /// Bovespa:
        /// Descrição de erro se CxIRejReason = 99 (outros); em outros casos, poderá conter informações adicionais no campo CxIRejReason informado
        /// </summary>
        [Category("SinalizarRejeicaoCancelamentoOrdem")]
        [Description("Bovespa: Descrição de erro se CxIRejReason = 99 (outros); em outros casos, poderá conter informações adicionais no campo CxIRejReason informado")]
        public string Text { get; set; }

        #endregion
    }
}
