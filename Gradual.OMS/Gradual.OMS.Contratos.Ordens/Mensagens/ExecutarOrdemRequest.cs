using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de execução de ordem.
    /// Equivalente à mensagem NewOrderSingle do Fix
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Mensagem(TipoMensagemResponse = typeof(ExecutarOrdemResponse))]
    public class ExecutarOrdemRequest : MensagemOrdemRequestBase
    {
        #region Bloco de Identificacao do Instrumento

        /// <summary>
        /// BMF:
        /// Símbolo. A BMFBOVESPA exige que esse campo seja adequadamente preenchido. 
        /// Ele contém a forma inteligível do campo SecurityID, disponível na mensagem de 
        /// lista de instrumentos.
        /// ------------------------------------------------------------------------------
        /// Bovespa:
        /// Símbolo da ação
        /// </summary>
        [Category("Identificação do Instrumento")]
        [Description("BMF: Símbolo. A BMFBOVESPA exige que esse campo seja adequadamente preenchido. Ele contém a forma inteligível do campo SecurityID, disponível na mensagem de lista de instrumentos. | Bovespa: Símbolo da ação")]
        public string Symbol { get; set; }

        /// <summary>
        /// BMF:
        /// Identificador do instrumento, conforme definido pela BMFBOVESPA. Para a lista 
        /// de instrumentos, consulte a mensagem correspondente (Security List).
        /// </summary>
        [Category("Identificação do Instrumento")]
        [Description("BMF: Identificador do instrumento, conforme definido pela BMFBOVESPA. Para a lista de instrumentos, consulte a mensagem correspondente (Security List).")]
        public string SecurityID { get; set; }

        /// <summary>
        /// BMF:
        /// Classe ou fonte de identificação do instrumento. Campo condicionado a SecurityID 
        /// estar especificado. Valor aceito: 8 = símbolo da Bolsa (identificador BMFBOVESPA 
        /// para instrumento)
        /// </summary>
        [Category("Identificação do Instrumento")]
        [Description("BMF: Classe ou fonte de identificação do instrumento. Campo condicionado a SecurityID estar especificado. Valor aceito: 8 = símbolo da Bolsa (identificador BMFBOVESPA para instrumento)")]
        public string SecurityIDSource { get; set; }

        /// <summary>
        /// BMF:
        /// Mercado ao qual o instrumento pertence (campo 48). Valor aceito: XBMF=BMFBOVESPA. 
        /// Esse campo é opcional e sua ausência implica a atribuição automática do valor XBMF 
        /// (BMFBOVESPA) para mercado (Market Center)
        /// </summary>
        [Category("Identificação do Instrumento")]
        [Description("BMF: Mercado ao qual o instrumento pertence (campo 48). Valor aceito: XBMF=BMFBOVESPA. Esse campo é opcional e sua ausência implica a atribuição automática do valor XBMF (BMFBOVESPA) para mercado (Market Center)")]
        public string SecurityExchange { get; set; }

        #endregion

        #region Bloco NewOrderSingle

        /// <summary>
        /// Indica o sistema cliente.
        /// Preenchido no servidor através da sessão do usuário
        /// </summary>
        public string CodigoSistemaCliente { get; set; }

        /// <summary>
        /// Identifica o usuário a qual a ordem irá pertencer
        /// </summary>
        [Category("New Order Single")]
        [Description("Identifica o usuário a qual a ordem irá pertencer")]
        public string CodigoUsuarioDestino { get; set; }
        
        /// <summary>
        /// Bovespa:
        /// Conta acordada entre a corretora e a instituição. Observação: o campo de código do usuário 
        /// não deve ultrapassar 8 digitos, incluindo o dígito de verificação, sem o hífen.
        /// </summary>
        [Category("New Order Single")]
        [Description("Bovespa: Conta acordada entre a corretora e a instituição. Observação: o campo de código do usuário não deve ultrapassar 8 digitos, incluindo o dígito de verificação, sem o hífen.")]
        public string Account { get; set; }

        /// <summary>
        /// Bovespa:
        /// Instruções para manuseio de ordens pelo corretor. Valor aceito: 1 = ordem de execução automatizada, 
        /// sem intervenção do corretor.
        /// -----------------------------------------------------
        /// Responsabilidade:
        /// Preenchido pela aplicação
        /// </summary>
        [Category("New Order Single")]
        [Description("Bovespa: Instruções para manuseio de ordens pelo corretor. Valor aceito: 1 = ordem de execução automatizada, sem intervenção do corretor | Responsabilidade: Preenchido pela aplicação")]
        public char HandInst { get; set; }

        /// <summary>
        /// Bovespa:
        /// Condicionalmente obrigatório se as ordens forem roteadas pela porta 600. Contém o código da corretora 
        /// a que a oferta pertence.
        /// -----------------------------------------------------
        /// Responsabilidade:
        /// Preenchido pela aplicação
        /// </summary>
        [Category("New Order Single")]
        [Description("Bovespa: Condicionalmente obrigatório se as ordens forem roteadas pela porta 600. Contém o código da corretora a que a oferta pertence. | Responsabilidade: Preenchido pela aplicação")]
        public string ExecBroker { get; set; }

        /// <summary>
        /// Código da bolsa no qual a ordem deve ser enviada.
        /// </summary>
        [Category("New Order Single")]
        [Description("Código da bolsa no qual a ordem deve ser enviada.")]
        public string CodigoBolsa { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Identificador único da ordem, conforme atribuído pela instituição
        /// </summary>
        [Category("New Order Single")]
        [Description("BMF / Bovespa: Identificador único da ordem, conforme atribuído pela instituição")]
        public string ClOrdID
        {
            get { return base.CodigoMensagem; }
            set { base.CodigoMensagem = value; }
        }

        /// <summary>
        /// BMF / Bovespa:
        /// Ponta da ordem. Valores aceitos: 1 = compra; 2 = venda
        /// </summary>
        [Category("New Order Single")]
        [Description("BMF / Bovespa: Ponta da ordem. Valores aceitos: 1 = compra; 2 = venda")]
        public OrdemDirecaoEnum Side { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Número de ações ou contratos da oferta
        /// </summary>
        [Category("New Order Single")]
        [Description("BMF / Bovespa: Número de ações ou contratos da oferta")]
        public double OrderQty { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Quantidade mínima para execução da oferta
        /// </summary>
        [Category("New Order Single")]
        [Description("BMF / Bovespa: Quantidade mínima para execução da oferta")]
        public double? MinQty { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Número máximo de ações ou contratos da oferta a ser exibido no núcleo de negociação a qualquer tempo
        /// </summary>
        [Category("New Order Single")]
        [Description("BMF / Bovespa: Número máximo de ações ou contratos da oferta a ser exibido no núcleo de negociação a qualquer tempo")]
        public double? MaxFloor { get; set; }

        /// <summary>
        /// BMF:
        /// Tipo de ordem. Valores aceitos: 2 = limitada; 4 = stop limitada; K = market with leftover as limit
        /// -------------------------------------------------------------------------------------------------------
        /// Bovespa:
        /// Tipo de ordem. Valores aceitos: 1 = mercado; 2 = limitada; 4 = stop limitada (ordem que se transforma em 
        /// ordem limitada assim que o preço de acionamento é atingido); A = on close (ordem a mercado a ser executada 
        /// pelo preço de fechamento do leilão de pré-abertura); K = market with leftover as limit (ordem a mercado em 
        /// que qualquer quantidade não executada se torna ordem limitada)
        /// </summary>
        [Category("New Order Single")]
        [Description("BMF: Tipo de ordem. Valores aceitos: 2 = limitada; 4 = stop limitada; K = market with leftover as limit | Bovespa: Tipo de ordem. Valores aceitos: 1 = mercado; 2 = limitada; 4 = stop limitada (ordem que se transforma em ordem limitada assim que o preço de acionamento é atingido); A = on close (ordem a mercado a ser executada pelo preço de fechamento do leilão de pré-abertura); K = market with leftover as limit (ordem a mercado em que qualquer quantidade não executada se torna ordem limitada)")]
        public OrdemTipoEnum OrdType { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Preço por ação ou contrato. Condicionado ao tipo de ordem definir preço-limite (exceto ordem a mercado)
        /// </summary>
        [Category("New Order Single")]
        [Description("BMF / Bovespa: Preço por ação ou contrato. Condicionado ao tipo de ordem definir preço-limite (exceto ordem a mercado)")]
        public double Price { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Preço de acionamento de ordem stop limitada (condicionado a OrdType = 4)
        /// </summary>
        [Category("New Order Single")]
        [Description("Preço de acionamento de ordem stop limitada (condicionado a OrdType = 4)")]
        public double StopPx { get; set; }

        /// <summary>
        /// BMF:
        /// Tempo de validade da ordem. A ausência desse campo significa que a ordem 
        /// é válida para o dia. Valores aceitos: 0 = válida para o dia (ou sessão); 
        /// 3 = executa integral ou parcialmente ou cancela (IOC ou FAK); 
        /// 4 = executa integralmente ou cancela (FOK)
        /// ------------------------------------------------------------------------
        /// Bovespa:
        /// Tempo de validade da ordem. A ausência desse campo significa que a ordem é 
        /// valida para o dia. Valores aceitos: 0 = válida para o dia; 1 = válida até 
        /// ser cancelada (GTC); 2 = válida para a abertura do mercado (OPG); 3 = executa 
        /// integral ou parcialmente ou cancela (IOC); 6 = válida até determinada data (GTD)
        /// </summary>
        [Category("New Order Single")]
        [Description("BMF: Tempo de validade da ordem. A ausência desse campo significa que a ordem é válida para o dia. Valores aceitos: 0 = válida para o dia (ou sessão); 3 = executa integral ou parcialmente ou cancela (IOC ou FAK); 4 = executa integralmente ou cancela (FOK) | Bovespa: Tempo de validade da ordem. A ausência desse campo significa que a ordem é valida para o dia. Valores aceitos: 0 = válida para o dia; 1 = válida até ser cancelada (GTC); 2 = válida para a abertura do mercado (OPG); 3 = executa integral ou parcialmente ou cancela (IOC); 6 = válida até determinada data (GTD)")]
        public OrdemValidadeEnum TimeInForce { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Horário de execução / geração da ordem, expresso em UTC.
        /// Faz referencia à data referencia da mensagem base.
        /// </summary>
        [Category("New Order Single")]
        [Description("BMF / Bovespa: Horário de execução / geração da ordem, expresso em UTC")]
        public DateTime TransactTime
        {
            get { return base.DataReferencia; }
            set { base.DataReferencia = value; }
        }

        /// <summary>
        /// Bovespa:
        /// Data de vencimento da ordem. Condicionalmente obrigatório se 59 = GTD.
        /// </summary>
        [Category("New Order Single")]
        [Description("Bovespa: Data de vencimento da ordem. Condicionalmente obrigatório se 59 = GTD.")]
        public DateTime? ExpireDate { get; set; }

        #endregion

        #region Outros Atributos

        /// <summary>
        /// Permite associar um código externo à ordem. Posteriormente a ordem poderá ser recuperada por este
        /// código externo.
        /// </summary>
        [Category("Outros")]
        [Description("Permite associar um código externo à ordem. Posteriormente a ordem poderá ser recuperada por este código externo.")]
        public string CodigoExterno { get; set; }

        /// <summary>
        /// Código do canal em que a ordem foi executada
        /// </summary>
        [Category("Outros")]
        [Description("Código do canal em que a ordem foi executada.")]
        public string CodigoCanal { get; set; }

        /// <summary>
        /// Permite que uma ordem seja executada em carater de exceção, através da emissão de um ticket de risco.
        /// Este ticket é emitido pela área de risco com algumas características possíveis da operação.
        /// </summary>
        [Category("Outros")]
        [Description("Permite que uma ordem seja executada em carater de exceção, através da emissão de um ticket de risco. Este ticket é emitido pela área de risco com algumas características possíveis da operação.")]
        public string CodigoTicketRisco { get; set; }

        #endregion

        #region Contrutores

        /// <summary>
        /// Construtor
        /// </summary>
        public ExecutarOrdemRequest()
        {
            this.Status = MensagemStatusEnum.SolicitacaoEfetuada;
        }

        #endregion

        public override string ToString()
        {
            return Serializador.TransformarEmString(this);
        }
    }
}
