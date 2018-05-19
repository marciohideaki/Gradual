using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library.Fix;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;

using QuickFix;
using QuickFix42;

namespace Gradual.OMS.Sistemas.CanaisNegociacao.CanalBovespa
{
    public class AplicacaoFixClienteBovespa : AplicacaoFixBase
    {
        private IServicoOrdens _servicoOrdens = null;
        private Dictionary<string, List<InstrumentoInfo>> _securityListMessages = new Dictionary<string, List<InstrumentoInfo>>();

        public CanalNegociacaoBovespa CanalBovespa { get; set; }
        
        public AplicacaoFixClienteBovespa()
        {
            // Pega referencia ao sistema de ordens
            _servicoOrdens = Ativador.Get<IServicoOrdens>();

            // Evento de erro
            this.MessageErrorEvent += new EventHandler<HostFixMensagemErroEventArgs>(AplicacaoFixClienteBovespa_MessageErrorEvent);
        }

        /// <summary>
        /// Quando ocorre um erro no tratamento de uma mensagem, a classe AplicacaoFixBase recebe o erro e dispara o evento.
        /// Aqui é o repasse do erro para o serviço de ordens.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AplicacaoFixClienteBovespa_MessageErrorEvent(object sender, HostFixMensagemErroEventArgs e)
        {
            // TODO: AplicacaoFixClienteBovespa: completar as informações do erro que está sendo repassado para o serviço de ordens
            // Informa mensagem de erro
            _servicoOrdens.SinalizarMensagemInvalida(
                new SinalizarMensagemInvalidaRequest()
                {
                    CodigoCanal = this.CanalBovespa.Codigo,
                    Erro = e.Exception,
                    Dados = e.Message
                });
        }

        public override void onMessage(ExecutionReport message, QuickFix.SessionID session)
        {
            // Precisa avisar ao sistema de ordens sobre a chegada do execution report
            // Os execution reports sempre são relativos à ordens, assim, é necessário manter uma tabela de ordens pendentes

            // Gera mensagem de relatorio de execucao
            SinalizarExecucaoOrdemRequest execucaoOrdemRequest = new SinalizarExecucaoOrdemRequest()
            {
                CodigoBolsa = this.CanalBovespa.Codigo,
                CodigoCanal = this.CanalBovespa.Codigo,
                OrderID = message.isSetOrderID() ? message.getOrderID().getValue() : null,
                ClOrdID = message.isSetClOrdID() ? message.getClOrdID().getValue() : null,
                OrigClOrdID = message.isSetOrigClOrdID() ? message.getOrigClOrdID().getValue() : null,
                ExecID = message.isSetExecID() ? message.getExecID().getValue() : null,
                ExecRefID = message.isSetExecRefID() ? message.getExecRefID().getValue() : null,
                Symbol = message.isSetSymbol() ? message.getSymbol().getValue() : null,
                ExecTransType = message.isSetExecTransType() ? (char?)message.getExecTransType().getValue() : null,
                ExecType = message.isSetExecType() ? TradutorFix.TraduzirOrdemTipoExecucao(message.getExecType().getValue()) : OrdemTipoExecucaoEnum.NaoInformado,
                OrdStatus = message.isSetOrdStatus() ? TradutorFix.TraduzirOrdemStatus(message.getOrdStatus().getValue()) : OrdemStatusEnum.NaoInformado,
                OrdRejReason = message.isSetOrdRejReason() ? TradutorFix.TraduzirOrdemMotivoRejeicao(message.getOrdRejReason().getValue()) : OrdemMotivoRejeicaoEnum.NaoInformado,
                Side = message.isSetSide() ? TradutorFix.TraduzirOrdemDirecao(message.getSide().getValue()) : OrdemDirecaoEnum.NaoInformado,
                OrderQty = message.isSetOrderQty() ? message.getOrderQty().getValue() : 0,
                Price = message.isSetPrice() ? message.getPrice().getValue() : 0,
                StopPx = message.isSetStopPx() ? message.getStopPx().getValue() : 0,
                AvgPx = message.isSetAvgPx() ? message.getAvgPx().getValue() : 0,
                LastShares = message.isSetLastShares() ? message.getLastShares().getValue() : 0,
                LastPx = message.isSetLastPx() ? message.getLastPx().getValue() : 0,
                MinQty = message.isSetMinQty() ? message.getMinQty().getValue() : 0,
                MaxFloor = message.isSetMaxFloor() ? message.getMaxFloor().getValue() : 0,
                LeavesQty = message.isSetLeavesQty() ? message.getLeavesQty().getValue() : 0,
                CumQty = message.isSetCumQty() ? message.getCumQty().getValue() : 0,
                TimeInForce = message.isSetTimeInForce() ? TradutorFix.TraduzirOrdemValidade(message.getTimeInForce().getValue()) : OrdemValidadeEnum.NaoInformado,
                TradeDate = message.isSetTradeDate() ? TradutorFix.TraduzirData(message.getTradeDate().getValue()) : DateTime.Now.Date,
                Account = message.isSetAccount() ? message.getAccount().getValue() : null,
                TransactTime = message.isSetTransactTime() ? message.getTransactTime().getValue() : DateTime.Now,
                Text = message.isSetText() ? message.getText().getValue() : null
            };

            // Faz a chamada 
            _servicoOrdens.SinalizarExecucaoOrdem(execucaoOrdemRequest);
        }

        public override void onMessage(OrderCancelReject message, SessionID session)
        {
            // Gera mensagem de rejeição de cancelamento de ordem
            SinalizarRejeicaoCancelamentoOrdemRequest rejeicaoCancelamentoOrdemRequest = new SinalizarRejeicaoCancelamentoOrdemRequest()
            {
            };

            // Faz a chamada
            _servicoOrdens.SinalizarRejeicaoCancelamentoOrdem(rejeicaoCancelamentoOrdemRequest);
        }
    }
}
