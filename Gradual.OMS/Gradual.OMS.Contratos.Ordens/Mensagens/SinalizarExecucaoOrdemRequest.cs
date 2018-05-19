using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Contratos.Ordens.Dados;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Utilizada, basicamente, pelos canais para sinalizar alteração de ordens.
    /// Por exemplo, quando o canal receber um execution report, ele irá informar 
    /// o sistema de ordens através desta mensagem.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Mensagem(TipoMensagemResponse = typeof(SinalizarExecucaoOrdemResponse))]
    public class SinalizarExecucaoOrdemRequest : MensagemSinalizacaoBase
    {
        #region Bloco ExecutionReport

        /// <summary>
        /// Código do sistema cliente
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("Código do sistema cliente")]
        public string CodigoSistemaCliente { get; set; }
        
        /// <summary>
        /// Código da bolsa no qual a ordem deve ser enviada.
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("Código da bolsa no qual a ordem deve ser enviada.")]
        public string CodigoBolsa { get; set; }

        /// <summary>
        /// Informa o canal que gerou esta mensagem de sinalização
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("Informa o canal que gerou esta mensagem de sinalização.")]
        public string CodigoCanal { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Identificador da oferta inserida eletronicamente pela instituição
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("BMF / Bovespa: Identificador da oferta inserida eletronicamente pela instituição")]
        public string ClOrdID 
        {
            get { return base.CodigoMensagemReferencia; }
            set { base.CodigoMensagemReferencia = value; } 
        }

        /// <summary>
        /// BMF:
        /// Contém o ClOrdID da oferta de substituição. Condicionado a ExecType = 5 (Substituição)
        /// ------------------------------------------------------------------
        /// Bovespa:
        /// Contém o ClOrdID da oferta e substituição. Condicionado a ExecType = 4 (cancelamento) ou 5 (Substituição)
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("BMF: Contém o ClOrdID da oferta de substituição. Condicionado a ExecType = 5 (Substituição) | Bovespa: Contém o ClOrdID da oferta e substituição. Condicionado a ExecType = 4 (cancelamento) ou 5 (Substituição)")]
        public string OrigClOrdID { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Identificador único da ordem, conforme atribuído pela Bolsa. A unicidade é garantida para 
        /// o mesmo dia de pregão / instrumento.
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("BMF / Bovespa: Identificador único da ordem, conforme atribuído pela Bolsa. A unicidade é garantida para o mesmo dia de pregão / instrumento.")]
        public string OrderID { get; set; }

        /// <summary>
        /// BMF:
        /// Identificador secundário da ordem, atribuído pela Bolsa, que identifica a oferta do 
        /// sinal de difusão para atualizações do livro agregado por oferta, ao passo que o OrderID 
        /// não é modificado nem enviado no sinal. Se a ordem for recebida com MaxFloor (quantidade 
        /// aparente) > 0, sempre que for reentrada pelo valor da quantidade aparente (e.g. devido a 
        /// uma execução), novo SecondaryOrderID será atribuído à oferta. Esse identificador também 
        /// pode ser usado para cancelar e alterar uma ordem.
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("Identificador secundário da ordem, atribuído pela Bolsa, que identifica a oferta do sinal de difusão para atualizações do livro agregado por oferta, ao passo que o OrderID não é modificado nem enviado no sinal. Se a ordem for recebida com MaxFloor (quantidade aparente) > 0, sempre que for reentrada pelo valor da quantidade aparente (e.g. devido a uma execução), novo SecondaryOrderID será atribuído à oferta. Esse identificador também pode ser usado para cancelar e alterar uma ordem.")]
        public string SecondaryOrderID { get; set; }

        /// <summary>
        /// BMF:
        /// Identificador da oferta direta inserida eletronicamente pela instituição (no caso de resposta a uma odem direta)
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("Identificador da oferta direta inserida eletronicamente pela instituição (no caso de resposta a uma odem direta)")]
        public string CrossID { get; set; }

        /// <summary>
        /// BMF / Bovespa: 
        /// Identificador único de mensagem de execução, atribuído pela Bolsa (único por instrumento)
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("BMF / Bovespa: Identificador único de mensagem de execução, atribuído pela Bolsa (único por instrumento)")]
        public string ExecID { get; set; }

        /// <summary>
        /// BMF:
        /// Descreve a ação que gerou o relatório de execução. Consulte o campo OrdStatus(39) para o 
        /// status atual da ordem (e.g. Parcialmente executada). Valores aceitos: 0 = nova; 4 = cancelamento 
        /// de oferta; 5 = substituição; 8 = rejeição; F = negócio; H = cancelamento de negócio; C = término 
        /// de validade; D = reconfirmação
        /// -----------------------------------------------------------
        /// Bovespa:
        /// Descreve a ação que gerou o relatório de execução. Consulte o campo OrdStatus(39) para o status 
        /// atual da ordem (e.g. Parcialmente executada). Valores aceitos: 0 = nova; 1 = execução parcial; 
        /// 2 = execução; 4 = cancelamento; 5 = substituição; 8 = rejeição; 9 = suspensão; L = acionamento
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("BMF: Descreve a ação que gerou o relatório de execução. Consulte o campo OrdStatus(39) para o status atual da ordem (e.g. Parcialmente executada). Valores aceitos: 0 = nova; 4 = cancelamento de oferta; 5 = substituição; 8 = rejeição; F = negócio; H = cancelamento de negócio; C = término de validade; D = reconfirmação | Bovespa: Descreve a ação que gerou o relatório de execução. Consulte o campo OrdStatus(39) para o status atual da ordem (e.g. Parcialmente executada). Valores aceitos: 0 = nova; 1 = execução parcial; 2 = execução; 4 = cancelamento; 5 = substituição; 8 = rejeição; 9 = suspensão; L = acionamento")]
        public OrdemTipoExecucaoEnum ExecType { get; set; }

        /// <summary>
        /// BMF:
        /// Valores aceitos: 0 = nova; 1 = parcialmente executada; 2 = executada; 4 = cancelada; 5 = substituída 
        /// (removida / substituída); 8 = rejeitada
        /// -----------------------------------
        /// Bovespa:
        /// Status da oferta. Valores aceitos: 0 = nova; 1 = parcialmente executada; 2 = executada; 4 = cancelada; 
        /// 5 = substituída; 8 = rejeitada; 9 = suspensa
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("BMF: Valores aceitos: 0 = nova; 1 = parcialmente executada; 2 = executada; 4 = cancelada; 5 = substituída (removida / substituída); 8 = rejeitada | Bovespa: Status da oferta. Valores aceitos: 0 = nova; 1 = parcialmente executada; 2 = executada; 4 = cancelada; 5 = substituída; 8 = rejeitada; 9 = suspensa")]
        public OrdemStatusEnum OrdStatus { get; set; }

        /// <summary>
        /// BMF:
        /// Utilizado em cenários de varredura do livro de ofertas (quando a oferta percorre o livro e é executada 
        /// contra várias outras), indicnado o estado final da ordem. Valores aceitos: 0 = nova; 1 = parcialmente 
        /// executada; 2 = executada; 4 = cancelada; 5 = substituída; 8 = rejeitada. Esse campo é enviado na mensagem 
        /// sempre que a ordem entra no livro de ofertas e é executada imediatamente contra uma ou mais ofertas na 
        /// ponta oposta, gerando mais de uma mensagem relatório de execução para transmitir as informações de execução. 
        /// Cada relatório de execução conterá o estado final da ordem do cliente nesse campo. Portanto, se uma ordem 
        /// for executada parcialmente três vezes até ser completada, cada relatório de execução conterá os seguintes 
        /// pares: Exec Rpt #1 (OrdStatus = parcialmente executada, BTSFinalTxOrdStatus = executada); Exec Rpt #2 
        /// (OrdStatus = parcialmente executada, BTSFinalTxOrdStatus = executada); e Exec Rpt #3 (OrdStatus = executada, 
        /// BTSFinalTxOrdStatus = executada)
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("Utilizado em cenários de varredura do livro de ofertas (quando a oferta percorre o livro e é executada contra várias outras), indicnado o estado final da ordem. Valores aceitos: 0 = nova; 1 = parcialmente executada; 2 = executada; 4 = cancelada; 5 = substituída; 8 = rejeitada. Esse campo é enviado na mensagem sempre que a ordem entra no livro de ofertas e é executada imediatamente contra uma ou mais ofertas na ponta oposta, gerando mais de uma mensagem relatório de execução para transmitir as informações de execução. Cada relatório de execução conterá o estado final da ordem do cliente nesse campo. Portanto, se uma ordem for executada parcialmente três vezes até ser completada, cada relatório de execução conterá os seguintes pares: Exec Rpt #1 (OrdStatus = parcialmente executada, BTSFinalTxOrdStatus = executada); Exec Rpt #2 (OrdStatus = parcialmente executada, BTSFinalTxOrdStatus = executada); e Exec Rpt #3 (OrdStatus = executada, BTSFinalTxOrdStatus = executada)")]
        public OrdemStatusEnum BTSFinalTxOrdStatus { get; set; }

        /// <summary>
        /// BMF:
        /// Condicionado a ExecType != 8 (rejeição) ou a ExecType != H (negócio cancelado). Valores aceitos: 2 = limitada; 
        /// 4 = stop limitada; K = market with leftover as limit
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("Condicionado a ExecType != 8 (rejeição) ou a ExecType != H (negócio cancelado). Valores aceitos: 2 = limitada; 4 = stop limitada; K = market with leftover as limit")]
        public OrdemTipoEnum OrdType { get; set; }
        
        /// <summary>
        /// BMF:
        /// Para uso opcional com ExecType = 8 (rejeitada). Valores aceitos: 1 = símbolo desconhecido; 2 = fora do horário 
        /// regular de negociação; 3 = ordem excede o limite; 4 = tarde demais para inserir; 5 = ordem desconhecida; 6 = ordem 
        /// duplicada (e.g. ClOrdID duplicado); 11 = característica de ordem não suportada; 13 = quantidade incorreta; 15 = conta 
        /// desconhecida; 99 = outros (erro genérico, consulte o campo Text para mais informações)
        /// -------------------------------------------
        /// Bovespa:
        /// Código que identifica o motivo de rejeição da ordem. Para uso opcional com ExecType = 8 (rejeição). Valores válidos: 
        /// 0 = opção do corretor; 1 = símbolo desconhecido; 2 = fora do horário regular de negociação; 3 = oferta excede o limite; 
        /// 4 = tarde demais para inserir; 5 = oferta desconhecida; 6 = ordem duplicada (e.g. ClOrdID duplicado)
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("BMF: Para uso opcional com ExecType = 8 (rejeitada). Valores aceitos: 1 = símbolo desconhecido; 2 = fora do horário regular de negociação; 3 = ordem excede o limite; 4 = tarde demais para inserir; 5 = ordem desconhecida; 6 = ordem duplicada (e.g. ClOrdID duplicado); 11 = característica de ordem não suportada; 13 = quantidade incorreta; 15 = conta desconhecida; 99 = outros (erro genérico, consulte o campo Text para mais informações) | Bovespa: Código que identifica o motivo de rejeição da ordem. Para uso opcional com ExecType = 8 (rejeição). Valores válidos: 0 = opção do corretor; 1 = símbolo desconhecido; 2 = fora do horário regular de negociação; 3 = oferta excede o limite; 4 = tarde demais para inserir; 5 = oferta desconhecida; 6 = ordem duplicada (e.g. ClOrdID duplicado)")]
        public OrdemMotivoRejeicaoEnum OrdRejReason { get; set; }
        
        /// <summary>
        /// BMF / Bovespa:
        /// Ponta da oferta. Valores aceitos: 1 = compra; 2 = venda
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("BMF / Bovespa: Ponta da oferta. Valores aceitos: 1 = compra; 2 = venda")]
        public OrdemDirecaoEnum Side { get; set; }

        /// <summary>
        /// BMF:
        /// Preço médio calculado para todas as quantidades executadas da oferta. Sempre 0.
        /// --------------------------------------------
        /// Bovespa:
        /// Sempre 0
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("BMF: Preço médio calculado para todas as quantidades executadas da oferta. Sempre 0. | Bovespa: Sempre 0")]
        public double AvgPx { get; set; }

        /// <summary>
        /// BMF:
        /// Quantidade de ações ou de contratos comprada / vendida nessa execução (última). 
        /// Condicionada a ExecType = F (negócio)
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("Quantidade de ações ou de contratos comprada / vendida nessa execução (última). Condicionada a ExecType = F (negócio)")]
        public double LastQty { get; set; }

        /// <summary>
        /// BMF:
        /// Quantidade de ações ou de contratos comprada / vendida nessa execução (última). 
        /// Condicionada a ExecType = F (negócio)
        /// *** Este texto está errado... corrigir... não é quantidade, e sim preço
        /// ------------------------------------------------
        /// Bovespa:
        /// Preço dessa (última) execução. Condicionado a ExecType = 1 ou 2.
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("BMF: Quantidade de ações ou de contratos comprada / vendida nessa execução (última). Condicionada a ExecType = F (negócio) | Bovespa: Preço dessa (última) execução. Condicionado a ExecType = 1 ou 2.")]
        public double LastPx { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Saldo de ações ou contratos para execução posterior ou não executado. LeavesQty = OrderQty - CumQty.
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("BMF / Bovespa: Saldo de ações ou contratos para execução posterior ou não executado. LeavesQty = OrderQty - CumQty.")]
        public double LeavesQty { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Quantidade executada total de ações ou contratos
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("BMF / Bovespa: Quantidade executada total de ações ou contratos")]
        public double CumQty { get; set; }

        /// <summary>
        /// BMF:
        /// Quantidade original da oferta antes de ser modificada. Presente na mensagem somente se 
        /// ExecType = 5 (substituída).
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("Quantidade original da oferta antes de ser modificada. Presente na mensagem somente se ExecType = 5 (substituída).")]
        public double OrigOrdQty { get; set; }

        /// <summary>
        /// BMF:
        /// Tempo de validade da ordem. A ausência desse campo significa que a ordem é válida para o dia. 
        /// Valores aceitos: 0 = válida para o dia (ou sessão); 3 = executa integral ou parcialmente ou cancela 
        /// (IOC ou FAK); 4 = executa integralmente ou cancela (FOK)    
        /// --------------------------------------------------------------------
        /// Bovespa:
        /// Tempo de validade da ordem. A ausência desse campo significa que a ordem é valida para o dia. 
        /// Valores aceitos: 0 = válida para o dia; 1 = válida até ser cancelada (GTC); 2 = válida para a 
        /// abertura do mercado (OPG); 3 = executa integral ou parcialmente ou cancela (IOC); 6 = válida 
        /// até determinada data (GTD)
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("BMF: Tempo de validade da ordem. A ausência desse campo significa que a ordem é válida para o dia. Valores aceitos: 0 = válida para o dia (ou sessão); 3 = executa integral ou parcialmente ou cancela (IOC ou FAK); 4 = executa integralmente ou cancela (FOK) | Bovespa: Tempo de validade da ordem. A ausência desse campo significa que a ordem é valida para o dia. Valores aceitos: 0 = válida para o dia; 1 = válida até ser cancelada (GTC); 2 = válida para a abertura do mercado (OPG); 3 = executa integral ou parcialmente ou cancela (IOC); 6 = válida até determinada data (GTD)")]
        public OrdemValidadeEnum TimeInForce { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Data da operação referenciada na mensagem, no formato AAAAMMDD (expressa 
        /// o horário do local em que a operação foi realizada). A ausência desse campo 
        /// indica o dia atual.
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("BMF / Bovespa: Data da operação referenciada na mensagem, no formato AAAAMMDD (expressa o horário do local em que a operação foi realizada). A ausência desse campo indica o dia atual.")]
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// BMF:
        /// Contém o identificador único para essa operação, por instrumento + data de pregão, 
        /// atribuído pela Bolsa. Condicionado a ExecType = F (negócio)
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("Contém o identificador único para essa operação, por instrumento + data de pregão, atribuído pela Bolsa. Condicionado a ExecType = F (negócio)")]
        public string UniqueTradeId { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Mnemônico de conta
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("BMF / Bovespa: Mnemônico de conta")]
        public string Account { get; set; }

        /// <summary>
        /// BMF / Bovespa:
        /// Sequência de texto, em formato livre
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("BMF / Bovespa: Sequência de texto, em formato livre")]
        public string Text { get; set; }

        /// <summary>
        /// Bovespa:
        /// Nos avisos tanto sobre negócios realizados quanto cancelados, contém o número da operação conforme emitido pelo núcleo de negociação
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("Bovespa: Nos avisos tanto sobre negócios realizados quanto cancelados, contém o número da operação conforme emitido pelo núcleo de negociação")]
        public string ExecRefID { get; set; }

        /// <summary>
        /// Bovespa:
        /// Identifica o tipo de transação. Valores aceitos: 0 = nova; 1 = cancelamento
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("Bovespa: Identifica o tipo de transação. Valores aceitos: 0 = nova; 1 = cancelamento")]
        public char? ExecTransType { get; set; }

        /// <summary>
        /// Bovespa:
        /// Número de ações ou contratos da oferta
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("Bovespa: Número de ações ou contratos da oferta")]
        public double OrderQty { get; set; }

        /// <summary>
        /// Bovespa:
        /// Preço por ação ou contrato. Obrigatório se especificado na oferta.
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("Bovespa: Preço por ação ou contrato. Obrigatório se especificado na oferta.")]
        public double Price { get; set; }

        /// <summary>
        /// Bovespa:
        /// Preço de acionamento de ordem stop limitada (condicionado a OrdType = 4)
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("Bovespa: Preço de acionamento de ordem stop limitada (condicionado a OrdType = 4)")]
        public double StopPx { get; set; }

        /// <summary>
        /// Bovespa:
        /// Quantidade de ações ou de contratos comprada / vendida nessa execução (última). 
        /// Condicionada a ExecType = 1 ou 2.
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("Bovespa: Quantidade de ações ou de contratos comprada / vendida nessa execução (última). Condicionada a ExecType = 1 ou 2.")]
        public double LastShares { get; set; }

        /// <summary>
        /// Bovespa:
        /// Quantidade mínima para execução da oferta. Reflete o campo MinQty da oferta original.
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("Bovespa: Quantidade mínima para execução da oferta. Reflete o campo MinQty da oferta original.")]
        public double MinQty { get; set; }

        /// <summary>
        /// Bovespa:
        /// Número máximo de ações ou contratos da oferta a ser exibido no núcleo de negociação a qualquer tempo. Reflete o campo MaxFloor da oferta original.
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("Bovespa: Número máximo de ações ou contratos da oferta a ser exibido no núcleo de negociação a qualquer tempo. Reflete o campo MaxFloor da oferta original.")]
        public double MaxFloor { get; set; }

        /// <summary>
        /// Bovespa:
        /// Horário da execução / geração da ordem, expresso em UTC
        /// </summary>
        [Category("SinalizarExecucaoOrdemRequest")]
        [Description("Bovespa: Horário da execução / geração da ordem, expresso em UTC")]
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
        /// ---------------------------------------------
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

        public override string ToString()
        {
            return Serializador.TransformarEmString(this);
        }
    }
}
