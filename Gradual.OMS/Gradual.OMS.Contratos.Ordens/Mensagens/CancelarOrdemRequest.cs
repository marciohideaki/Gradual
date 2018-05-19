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
    /// Solicita o cancelamento de uma ordem.
    /// Mensagem equivalente à mensagem OrderCancelRequest do Fix
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Mensagem(TipoMensagemResponse = typeof(CancelarOrdemResponse))]
    public class CancelarOrdemRequest : MensagemOrdemRequestBase
    {
        #region OrderCancelRequest

        /// <summary>
        /// Código do sistema cliente.
        /// Preenchido pelo servidor através da sessão do usuário
        /// </summary>
        public string CodigoSistemaCliente { get; set; }

        /// <summary>
        /// Código da bolsa no qual a ordem deve ser enviada.
        /// </summary>
        [Category("OrderCancelRequest")]
        [Description("Código da bolsa no qual a ordem deve ser enviada.")]
        public string CodigoBolsa { get; set; }

        /// <summary>
        /// Bovespa:
        /// Conta acordada entre a corretora e a instituição. 
        /// </summary>
        [Category("OrderCancelRequest")]
        [Description("Bovespa: Conta acordada entre a corretora e a instituição.")]
        public string Account { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// ClOrdID da oferta que o cliente deseja cancelar
        /// </summary>
        [Category("OrderCancelRequest")]
        [Description("BMF / Bovespa: ClOrdID da oferta que o cliente deseja cancelar")]
        public string OrigClOrdID
        {
            get { return base.CodigoMensagemReferencia; }
            set { base.CodigoMensagemReferencia = value; }
        }
        
        /// <summary>
        /// BMF / Bovespa:
        /// OrderID atribuído pela BMFBOVESPA à oferta que o cliente deseja cancelar. Se esse campo estiver presente, o valor de OrigClOrdID será ignorado
        /// </summary>
        [Category("OrderCancelRequest")]
        [Description("BMF / Bovespa: OrderID atribuído pela BMFBOVESPA à oferta que o cliente deseja cancelar. Se esse campo estiver presente, o valor de OrigClOrdID será ignorado")]
        public string OrderID { get; set; }
        
        /// <summary>
        /// BMF / Bovespa:
        /// Identificador único da solicitação de cancelamento, conforme atribuído pela instituição
        /// </summary>
        [Category("OrderCancelRequest")]
        [Description("BMF / Bovespa: Identificador único da solicitação de cancelamento, conforme atribuído pela instituição")]
        public string ClOrdID
        {
            get { return base.CodigoMensagem; }
            set { base.CodigoMensagem = value; }
        }
        
        /// <summary>
        /// BMF:
        /// Identificador gerado pela BM&FBOVESPA para cada evento de modificação ou de reentrada de quantidade em ordem aparente
        /// </summary>
        [Category("OrderCancelRequest")]
        [Description("BMF: Identificador gerado pela BM&FBOVESPA para cada evento de modificação ou de reentrada de quantidade em ordem aparente")]
        public string SecondaryOrderID { get; set; }
        
        /// <summary>
        /// BMF / Bovespa:
        /// Ponta da oferta a ser cancelada. Valores aceitos: 1 = compra; 2 = venda
        /// </summary>
        [Category("OrderCancelRequest")]
        [Description("BMF / Bovespa: Ponta da oferta a ser cancelada. Valores aceitos: 1 = compra; 2 = venda")]
        public OrdemDirecaoEnum Side { get; set; }

        /// <summary>
        /// Bovespa:
        /// Número de ações ou contratos da oferta
        /// </summary>
        [Category("OrderCancelRequest")]
        [Description("Bovespa: Número de ações ou contratos da oferta")]
        public double OrderQty { get; set; }

        /// <summary>
        /// Bovespa:
        /// Condicionalmente obrigatório se as ordens forem roteadas pela porta 600. Contém o código da 
        /// corretora a que a oferta pertence.
        /// </summary>
        [Category("OrderCancelRequest")]
        [Description("Bovespa: Condicionalmente obrigatório se as ordens forem roteadas pela porta 600. Contém o código da corretora a que a oferta pertence.")]
        public string ExecBroker { get; set; }

        /// <summary>
        /// Bovespa:
        /// Hora de geração / liberação da solicitação de oferta pelo operador ou pelo sistema de 
        /// negociação, expresso em UTC.
        /// Faz referencia à data referencia da mensagem base.
        /// </summary>
        [Category("OrderCancelRequest")]
        [Description("Bovespa: Hora de geração / liberação da solicitação de oferta pelo operador ou pelo sistema de negociação, expresso em UTC")]
        public DateTime TransactTime 
        {
            get { return base.DataReferencia; }
            set { base.DataReferencia = value; } 
        }

        #endregion

        #region Bloco de Identificacao do Instrumento

        /// <summary>
        /// BMF:
        /// Símbolo. A BM&FBOVESPA exige que esse campo seja adequadamente preenchido. 
        /// Ele contém a forma inteligível do campo SecurityID, disponível na mensagem de 
        /// lista de instrumentos.
        /// -------------------------------------------------------------
        /// Bovespa:
        /// Símbolo da ação.
        /// </summary>
        [Category("Identificação do Instrumento")]
        [Description("BMF: Símbolo. A BMFBOVESPA exige que esse campo seja adequadamente preenchido. Ele contém a forma inteligível do campo SecurityID, disponível na mensagem de lista de instrumentos. | Bovespa: Símbolo da ação.")]
        public string Symbol { get; set; }

        /// <summary>
        /// BMF:
        /// Identificador do instrumento, conforme definido pela BM&FBOVESPA. Para a lista 
        /// de instrumentos, consulte a mensagem correspondente (Security List).
        /// </summary>
        [Category("Identificação do Instrumento")]
        [Description("Identificador do instrumento, conforme definido pela BMFBOVESPA. Para a lista de instrumentos, consulte a mensagem correspondente (Security List).")]
        public string SecurityID { get; set; }

        /// <summary>
        /// BMF:
        /// Classe ou fonte de identificação do instrumento. Campo condicionado a SecurityID 
        /// estar especificado. Valor aceito: 8 = símbolo da Bolsa (identificador BM&FBOVESPA 
        /// para instrumento)
        /// </summary>
        [Category("Identificação do Instrumento")]
        [Description("Classe ou fonte de identificação do instrumento. Campo condicionado a SecurityID estar especificado. Valor aceito: 8 = símbolo da Bolsa (identificador BMFBOVESPA para instrumento)")]
        public string SecurityIDSource { get; set; }

        /// <summary>
        /// BMF:
        /// Mercado ao qual o instrumento pertence (campo 48). Valor aceito: XBMF=BM&FBOVESPA. 
        /// Esse campo é opcional e sua ausência implica a atribuição automática do valor XBMF 
        /// (BM&FBOVESPA) para mercado (Market Center)
        /// </summary>
        [Category("Identificação do Instrumento")]
        [Description("Mercado ao qual o instrumento pertence (campo 48). Valor aceito: XBMF=BMFBOVESPA. Esse campo é opcional e sua ausência implica a atribuição automática do valor XBMF (BMFBOVESPA) para mercado (Market Center)")]
        public string SecurityExchange { get; set; }

        #endregion

        /// <summary>
        /// Construtor
        /// </summary>
        public CancelarOrdemRequest()
        {
            this.Status = MensagemStatusEnum.SolicitacaoEfetuada;
        }
    }
}
