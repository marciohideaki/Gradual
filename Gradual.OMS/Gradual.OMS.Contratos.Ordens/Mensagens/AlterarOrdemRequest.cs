using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens.Dados;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de alteração de ordem.
    /// Equivalente à mensagem OrderCancelReplaceRequest do Fix
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Mensagem(TipoMensagemResponse = typeof(AlterarOrdemResponse))]
    public class AlterarOrdemRequest : MensagemOrdemRequestBase
    {
        #region Bloco de Identificacao do Instrumento

        /// <summary>
        /// Código do sistema cliente.
        /// Preenchido pelo servidor através da sessão do usuário
        /// </summary>
        public string CodigoSistemaCliente { get; set; }

        /// <summary>
        /// BMF:
        /// Símbolo. A BM&FBOVESPA exige que esse campo seja adequadamente preenchido. 
        /// Ele contém a forma inteligível do campo SecurityID, disponível na mensagem de 
        /// lista de instrumentos.
        /// -----------------------------------------------
        /// Bovespa:
        /// Símbolo da ação
        /// </summary>
        [Category("Identificação do Instrumento")]
        [Description("BMF: Símbolo. A BM&FBOVESPA exige que esse campo seja adequadamente preenchido. Ele contém a forma inteligível do campo SecurityID, disponível na mensagem de lista de instrumentos. | Bovespa: Símbolo da ação")]
        public string Symbol { get; set; }

        /// <summary>
        /// BMF:
        /// Identificador do instrumento, conforme definido pela BM&FBOVESPA. Para a lista 
        /// de instrumentos, consulte a mensagem correspondente (Security List).
        /// </summary>
        [Category("Identificação do Instrumento")]
        [Description("BMF: Identificador do instrumento, conforme definido pela BM&FBOVESPA. Para a lista de instrumentos, consulte a mensagem correspondente (Security List).")]
        public string SecurityID { get; set; }

        /// <summary>
        /// BMF:
        /// Classe ou fonte de identificação do instrumento. Campo condicionado a SecurityID 
        /// estar especificado. Valor aceito: 8 = símbolo da Bolsa (identificador BM&FBOVESPA 
        /// para instrumento)
        /// </summary>
        [Category("Identificação do Instrumento")]
        [Description("BMF: Classe ou fonte de identificação do instrumento. Campo condicionado a SecurityID estar especificado. Valor aceito: 8 = símbolo da Bolsa (identificador BM&FBOVESPA para instrumento)")]
        public string SecurityIDSource { get; set; }

        /// <summary>
        /// BMF:
        /// Mercado ao qual o instrumento pertence (campo 48). Valor aceito: XBMF=BM&FBOVESPA. 
        /// Esse campo é opcional e sua ausência implica a atribuição automática do valor XBMF 
        /// (BM&FBOVESPA) para mercado (Market Center)
        /// </summary>
        [Category("Identificação do Instrumento")]
        [Description("BMF: Mercado ao qual o instrumento pertence (campo 48). Valor aceito: XBMF=BMFBOVESPA. Esse campo é opcional e sua ausência implica a atribuição automática do valor XBMF (BMFBOVESPA) para mercado (Market Center)")]
        public string SecurityExchange { get; set; }

        #endregion

        #region Bloco OrderCancelReplaceRequest

        /// <summary>
        /// Código da bolsa no qual a ordem deve ser enviada.
        /// </summary>
        [Category("New Order Single")]
        [Description("Código da bolsa no qual a ordem deve ser enviada.")]
        public string CodigoBolsa { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// ClOrdID da oferta que o cliente deseja cancelar
        /// </summary>
        [Category("OrderCancelReplaceRequest")]
        [Description("BMF / Bovespa: ClOrdID da oferta que o cliente deseja cancelar")]
        public string OrigClOrdID 
        {
            get { return base.CodigoMensagemReferencia; }
            set { base.CodigoMensagemReferencia = value; } 
        }

        /// <summary>
        /// BMF / Bovespa:
        /// OrderID atribuída pela BM&FBOVESPA à oferta que o cliente deseja cancelar. Se esse campo estiver 
        /// presente, o valor de OrigClOrdID será ignorado.
        /// </summary>
        [Category("OrderCancelReplaceRequest")]
        [Description("BMF / Bovespa: OrderID atribuída pela BM&FBOVESPA à oferta que o cliente deseja cancelar. Se esse campo estiver presente, o valor de OrigClOrdID será ignorado.")]
        public string OrderID { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Identificador único da solicitação de substituição de cancelamento, conforme atribuído pela instituição
        /// </summary>
        [Category("OrderCancelReplaceRequest")]
        [Description("BMF / Bovespa: Identificador único da solicitação de substituição de cancelamento, conforme atribuído pela instituição")]
        public string ClOrdID 
        {
            get { return base.CodigoMensagem; }
            set { base.CodigoMensagem = value; }  
        }

        /// <summary>
        /// BMF:
        /// Identificador gerado pela Bolsa para cada evento de modificação ou de reentrada de quantidade em ordem aparente
        /// </summary>
        [Category("OrderCancelReplaceRequest")]
        [Description("BMF: Identificador gerado pela Bolsa para cada evento de modificação ou de reentrada de quantidade em ordem aparente")]
        public string SecondaryOrderID { get; set; }

        /// <summary>
        /// BMF:
        /// Valor do campo SecondaryOrderID original para oferta modificada
        /// </summary>
        [Category("OrderCancelReplaceRequest")]
        [Description("BMF: Valor do campo SecondaryOrderID original para oferta modificada")]
        public string OrigSecondaryOrderID { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Ponta da oferta a ser cancelada. Valores aceitos: 1 = compra; 2 = venda
        /// </summary>
        [Category("OrderCancelReplaceRequest")]
        [Description("BMF / Bovespa: Ponta da oferta a ser cancelada. Valores aceitos: 1 = compra; 2 = venda")]
        public OrdemDirecaoEnum Side { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Número de ações ou contratos da oferta
        /// </summary>
        [Category("OrderCancelReplaceRequest")]
        [Description("BMF / Bovespa: Número de ações ou contratos da oferta")]
        public double? OrderQty { get; set; }

        /// <summary>
        /// BMF:
        /// Quantidade mínima para execução da oferta
        /// </summary>
        [Category("OrderCancelReplaceRequest")]
        [Description("BMF: Quantidade mínima para execução da oferta")]
        public double? MinQty { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Número máximo de ações ou contratos da oferta a ser exibido no núcleo de 
        /// negociação a qualquer tempo
        /// </summary>
        [Category("OrderCancelReplaceRequest")]
        [Description("BMF / Bovespa: Número máximo de ações ou contratos da oferta a ser exibido no núcleo de negociação a qualquer tempo")]
        public double? MaxFloor { get; set; }

        /// <summary>
        /// BMF:
        /// Tipo de ordem. Valores aceitos: 2 = limitada; 4 = stop limitada; K = market 
        /// with leftover as limit
        /// ------------------------------------------------------------------------------------
        /// Bovespa:
        /// Tipo de ordem. Valores aceitos: 1 = mercado; 2 = limitada; 4 = stop limitada; 
        /// A = on close; K = market with leftover as limit
        /// </summary>
        [Category("OrderCancelReplaceRequest")]
        [Description("BMF: Tipo de ordem. Valores aceitos: 2 = limitada; 4 = stop limitada; K = market with leftover as limit | Bovespa: Tipo de ordem. Valores aceitos: 1 = mercado; 2 = limitada; 4 = stop limitada; A = on close; K = market with leftover as limit")]
        public OrdemTipoEnum OrdType { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Preço por ação ou contrato. Condicionado ao tipo de ordem definir preço-limite 
        /// (exceto ordem a mercado)
        /// </summary>
        [Category("OrderCancelReplaceRequest")]
        [Description("BMF / Bovespa: Preço por ação ou contrato. Condicionado ao tipo de ordem definir preço-limite (exceto ordem a mercado)")]
        public double? Price { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Horário de execução / geração da ordem, expresso em UTC.
        /// Aponta para a data referência da mensagem base.
        /// </summary>
        [Category("OrderCancelReplaceRequest")]
        [Description("BMF / Bovespa: Horário de execução / geração da ordem, expresso em UTC")]
        public DateTime TransactTime 
        {
            get { return base.DataReferencia; }
            set { base.DataReferencia = value; } 
        }

        /// <summary>
        /// Bovespa:
        /// Identifica a corretora executora
        /// </summary>
        [Category("OrderCancelReplaceRequest")]
        [Description("Bovespa: Identifica a corretora executora")]
        public string ExecBroker { get; set; }

        /// <summary>
        /// Bovespa:
        /// Tempo de validade da ordem. A ausência desse campo significa que a ordem é valida para o dia. 
        /// Valores aceitos: 0 = válida para o dia; 1 = válida até ser cancelada (GTC); 2 = válida para 
        /// a abertura do mercado (OPG); 3 = executa integral ou parcialmente ou cancela (IOC); 6 = válida 
        /// até determinada data (GTD)
        /// </summary>
        [Category("OrderCancelReplaceRequest")]
        [Description("Bovespa: Tempo de validade da ordem. A ausência desse campo significa que a ordem é valida para o dia. Valores aceitos: 0 = válida para o dia; 1 = válida até ser cancelada (GTC); 2 = válida para a abertura do mercado (OPG); 3 = executa integral ou parcialmente ou cancela (IOC); 6 = válida até determinada data (GTD)")]
        public OrdemValidadeEnum TimeInForce { get; set; }

        #endregion

        #region Construtores

        public AlterarOrdemRequest()
        {
            this.Status = MensagemStatusEnum.SolicitacaoEfetuada;
        }

        #endregion
    }
}
