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
using QuickFix44;

namespace Gradual.OMS.Sistemas.CanaisNegociacao.CanalBMF
{
    public class AplicacaoFixClienteBMF : AplicacaoFixBase
    {
        private IServicoOrdens _servicoOrdens = null;
        private Dictionary<string, List<InstrumentoInfo>> _securityListMessages = new Dictionary<string, List<InstrumentoInfo>>();

        public CanalNegociacaoBMF CanalBMF { get; set; }
        
        public AplicacaoFixClienteBMF()
        {
            // Pega referencia ao sistema de ordens
            _servicoOrdens = Ativador.Get<IServicoOrdens>();

            // Evento de erro
            this.MessageErrorEvent += new EventHandler<HostFixMensagemErroEventArgs>(AplicacaoFixClienteBMF_MessageErrorEvent);
        }

        /// <summary>
        /// Quando ocorre um erro no tratamento de uma mensagem, a classe AplicacaoFixBase recebe o erro e dispara o evento.
        /// Aqui é o repasse do erro para o serviço de ordens.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AplicacaoFixClienteBMF_MessageErrorEvent(object sender, HostFixMensagemErroEventArgs e)
        {
            // TODO: AplicacaoFixClienteBMF: completar as informações do erro que está sendo repassado para o serviço de ordens
            // Informa mensagem de erro
            _servicoOrdens.SinalizarMensagemInvalida(
                new SinalizarMensagemInvalidaRequest() 
                { 
                    CodigoCanal = this.CanalBMF.Codigo,
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
                CodigoBolsa = this.CanalBMF.Codigo,
                CodigoCanal = this.CanalBMF.Codigo,
                ExecID = message.isSetExecID() ? message.getExecID().getValue() : null,
                UniqueTradeId = message.isSetField(6032) ? message.getString(6032) : null,
                ClOrdID = message.isSetClOrdID() ? message.getClOrdID().getValue() : null,
                OrderID = message.isSetOrderID() ? message.getOrderID().getValue() : null,
                SecondaryOrderID = message.isSetSecondaryOrderID() ? message.getSecondaryOrderID().getValue() : null,
                CrossID = message.isSetCrossID() ? message.getCrossID().getValue() : null,
                OrigClOrdID = message.isSetOrigClOrdID() ? message.getOrigClOrdID().getValue() : null,
                Account = message.isSetAccount() ? message.getAccount().getValue() : null,
                TradeDate = message.isSetTradeDate() ? TradutorFix.TraduzirData(message.getTradeDate().getValue()) : DateTime.Now.Date,
                OrdRejReason = message.isSetOrdRejReason() ? TradutorFix.TraduzirOrdemMotivoRejeicao(message.getOrdRejReason().getValue()) : OrdemMotivoRejeicaoEnum.NaoInformado,
                OrdStatus = message.isSetOrdStatus() ? TradutorFix.TraduzirOrdemStatus(message.getOrdStatus().getValue()) : OrdemStatusEnum.NaoInformado,
                BTSFinalTxOrdStatus = message.isSetField(10029) ? TradutorFix.TraduzirOrdemStatus(message.getString(10029)[0]) : OrdemStatusEnum.NaoInformado,
                OrdType = message.isSetOrdType() ? TradutorFix.TraduzirOrdemTipo(message.getOrdType().getValue()) : OrdemTipoEnum.NaoInformado,
                TimeInForce = message.isSetTimeInForce() ? TradutorFix.TraduzirOrdemValidade(message.getTimeInForce().getValue()) : OrdemValidadeEnum.NaoInformado,
                AvgPx = message.isSetAvgPx() ? message.getAvgPx().getValue() : 0,
                CumQty = message.isSetCumQty() ? message.getCumQty().getValue() : 0,
                OrigOrdQty = message.isSetField(9757) ? message.getDouble(9757) : 0,
                LeavesQty = message.isSetLeavesQty() ? message.getLeavesQty().getValue() : 0,
                ExecType = message.isSetExecType() ? TradutorFix.TraduzirOrdemTipoExecucao(message.getExecType().getValue()) : OrdemTipoExecucaoEnum.NaoInformado,
                LastQty = message.isSetLastQty() ? message.getLastQty().getValue() : 0,
                LastPx = message.isSetLastPx() ? message.getLastPx().getValue() : 0,
                Text = message.isSetText() ? message.getText().getValue() : null,
                Side = message.isSetSide() ? TradutorFix.TraduzirOrdemDirecao(message.getSide().getValue()) : OrdemDirecaoEnum.NaoInformado,
                Symbol = message.isSetSymbol() ? message.getSymbol().getValue() : null,
                SecurityExchange = message.isSetSecurityExchange() ? message.getSecurityExchange().getValue() : null,
                SecurityID = message.isSetSecurityID() ? message.getSecurityID().getValue() : null,
                SecurityIDSource = message.isSetSecurityIDSource() ? message.getSecurityIDSource().getValue() : null
            };

            // Faz a chamada 
            _servicoOrdens.SinalizarExecucaoOrdem(execucaoOrdemRequest);
        }

        /// <summary>
        /// Trata mensagem SecurityList. Acumula todos os retornos de security list. 
        /// Na última mensagem envia para o sistema de ordens a lista de instrumentos.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="session"></param>
        public override void onMessage(SecurityList message, QuickFix.SessionID session)
        {
            // Pega id da requisicao
            SecurityReqID securityReqId = message.getSecurityReqID();

            // Verifica se esta requisicao já tem uma entrada na lista, e pega a entrada
            if (!_securityListMessages.ContainsKey(securityReqId.getValue()))
                _securityListMessages.Add(securityReqId.getValue(), new List<InstrumentoInfo>());
            List<InstrumentoInfo> securityList = _securityListMessages[securityReqId.getValue()];

            // Acumula lista de mensagens
            // ** Por enquanto código da bolsa é o código do canal
            for (uint i = 1; i <= message.getNoRelatedSym().getValue(); i++)
            {
                // Pega o grupo
                SecurityList.NoRelatedSym noRelatedSym = new SecurityList.NoRelatedSym();
                message.getGroup(i, noRelatedSym);

                // Adiciona informacoes do instrumento
                securityList.Add(
                    new InstrumentoInfo()
                    {
                        CodigoBolsa = this.CanalBMF.Codigo,
                        SecurityExchange = noRelatedSym.getSecurityExchange().getValue(),
                        SecurityID = noRelatedSym.getSecurityID().getValue(),
                        SecurityIDSource = noRelatedSym.getSecurityIDSource().getValue(),
                        Symbol = noRelatedSym.getSymbol().getValue()
                    });
            }

            // Se for a ultima mensagem, informa sistema de ordens
            if (message.isSetLastFragment())
            {
                LastFragment lastFragment = new LastFragment();
                message.get(lastFragment);
                if (lastFragment.getValue())
                    _servicoOrdens.SinalizarListaInstrumentos(
                        new SinalizarListaInstrumentosRequest()
                        {
                            CodigoCanal = this.CanalBMF.Codigo,
                            SecurityReqID = securityReqId.getValue(),
                            SecurityListMessages = securityList
                        });
            }
        }

        public override void onMessage(OrderCancelReject message, SessionID session)
        {
            // Sinalizar mensagem de rejeicao para a aplicacao
            _servicoOrdens.SinalizarRejeicaoCancelamentoOrdem (
                new SinalizarRejeicaoCancelamentoOrdemRequest()
                {
                    CodigoBolsa = this.CanalBMF.Codigo,
                    OrderID = message.isSetOrderID() ? message.getOrderID().getValue() : null,
                    ClOrdID = message.isSetClOrdID() ? message.getClOrdID().getValue() : null,
                    OrigClOrdID = message.isSetOrigClOrdID() ? message.getOrigClOrdID().getValue() : null,
                    OrdStatus = message.isSetOrdStatus() ? message.getOrdStatus().getValue().ToString() : null,
                    Side = message.isSetField(54) ? TradutorFix.TraduzirOrdemDirecao((Char)message.getChar(54)) : OrdemDirecaoEnum.NaoInformado,
                    CxIRejResponseTo = message.isSetCxlRejResponseTo() ? message.getCxlRejResponseTo().getValue().ToString() : null,
                    CxIRejReason = message.isSetCxlRejReason() ? message.getCxlRejReason().getValue().ToString() : null,
                    Text = message.isSetText() ? message.getText().getValue() : null
                });
        }

        public override void onMessage(Reject message, SessionID session)
        {
            // Sinalizar mensagem de rejeicao para a aplicacao
            _servicoOrdens.SinalizarMensagemInvalida(
                new SinalizarMensagemInvalidaRequest() 
                { 
                    CodigoCanal = this.CanalBMF.Codigo,
                    RefSeqNum = message.isSetRefSeqNum() ? message.getRefSeqNum().getValue() : 0,
                    RefTagID = message.isSetRefTagID() ? message.getRefTagID().getValue() : 0,
                    RefMsgType = message.isSetRefMsgType() ? message.getRefMsgType().getValue() : null,
                    SessionRejectReason = message.isSetSessionRejectReason() ? message.getSessionRejectReason().getValue() : 0,
                    Text = message.isSetText() ? message.getText().getValue() : null
                });
        }

        protected override void OnToAdmin(QuickFix.Message message, QuickFix.SessionID session)
        {
            // Complementa a mensagem de logon com a senha 
            if (message.GetType() == typeof(Logon))
            {
                Logon message2 = (Logon)message;
                if (this.CanalBMF.Config.LogonPassword != "")
                {
                    message2.set(new QuickFix.RawData(this.CanalBMF.Config.LogonPassword));
                    message2.set(new QuickFix.RawDataLength(this.CanalBMF.Config.LogonPassword.Length));
                }
                message2.set(new QuickFix.HeartBtInt(this.CanalBMF.Config.HeartBtInt));
                message2.set(new QuickFix.EncryptMethod(0));
                message2.set(new QuickFix.ResetSeqNumFlag(this.CanalBMF.Config.RessetarSeqNum));
            }

            // Repassa chamada
            base.OnToAdmin(message, session);
        }

        protected override void OnToApp(QuickFix.Message message, SessionID session)
        {
            base.OnToApp(message, session);
        }

        protected override void OnFromApp(QuickFix.Message message, SessionID session)
        {
            base.OnFromApp(message, session);
        }
    }
}
