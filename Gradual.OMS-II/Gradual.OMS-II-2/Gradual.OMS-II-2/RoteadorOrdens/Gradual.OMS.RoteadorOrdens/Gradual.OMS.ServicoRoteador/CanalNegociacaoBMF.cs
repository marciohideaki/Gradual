using System;
using System.Collections;
using System.ServiceModel;

using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;

using QuickFix;
using QuickFix44;
using Gradual.OMS.RoteadorOrdensAdm.Lib;
using Gradual.OMS.RoteadorOrdensAdm.Lib.Mensagens;
using Gradual.OMS.RoteadorOrdensAdm.Lib.Dados;
using System.Globalization;
using System.Configuration;

namespace Gradual.OMS.ServicoRoteador
{
    /// <summary>
    /// Implementa um canal de comunicacao fix para a BM&F
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CanalNegociacaoBMF : CanalNegociacaoBase, IRoteadorOrdens, IRoteadorOrdensAdmin
    {
        private Hashtable ordemInfoPorClOrdId = new Hashtable();
        private Hashtable ordemInfoPorMsgSeqNum = new Hashtable();
        private Hashtable clOrdIdMsgSeqNum = new Hashtable();
        private Hashtable cancelInfoPorClOrdId = new Hashtable();
        private Hashtable crossInfoPorCrossID = new Hashtable();

        #region IRoteadorOrdens Members

        /// <summary>
        /// Envia um pedido de execucao de ordem para o canal correspondente
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecutarOrdemResponse ExecutarOrdem(ExecutarOrdemRequest request)
        {
            string msg = null;
            StatusRoteamentoEnum status = StatusRoteamentoEnum.Erro;
            ExecutarOrdemResponse response = new ExecutarOrdemResponse();

            // Se o logon não foi feito, rejeita imediatamente a ordem
            if (!_bConectadoBolsa)
            {
                response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaEO(
                    "Ordem rejeitada. Logon não foi realizado no CanalNegociacaoBMF",
                    StatusRoteamentoEnum.Erro);

                return response;
            }

            try
            {
                OrdemInfo ordem = request.info;

                RoteadorOrdensUtil.ConsistirOrdem(ordem);

                //Cria a mensagem FIX de NewOrderSingle
                QuickFix44.NewOrderSingle ordersingle = new NewOrderSingle();

                if ( ordem.Account > 0 ) 
                    ordersingle.set(new Account(ordem.Account.ToString()));

                // Instrument Identification Block
                ordersingle.set(new Symbol(ordem.Symbol));
                ordersingle.set(new SecurityID(ordem.SecurityID));
                ordersingle.set(new SecurityIDSource(_config.SecurityIDSource));

                ordersingle.set(new ClOrdID(ordem.ClOrdID));

                if (ordem.Side == OrdemDirecaoEnum.Venda)
                    ordersingle.set(new Side(Side.SELL));
                else
                    ordersingle.set(new Side(Side.BUY));


                TimeInForce tif = TradutorFix.deOrdemValidadeParaTimeInForce(ordem.TimeInForce);
                if (tif != null)
                    ordersingle.set(tif);

                ordersingle.set(new OrderQty(ordem.OrderQty));

                if (ordem.TimeInForce == OrdemValidadeEnum.ValidoAteDeterminadaData)
                {
                    DateTime expiredate = Convert.ToDateTime(ordem.ExpireDate);
                    ordersingle.set(new ExpireDate(expiredate.ToString("yyyyMMdd")));
                }

                OrdType ordType = TradutorFix.deOrdemTipoParaOrdType(ordem.OrdType);
                if (ordType != null)
                    ordersingle.set(ordType);

                // Verifica envio do preco
                switch (ordem.OrdType)
                {
                    case OrdemTipoEnum.Limitada:
                    case OrdemTipoEnum.MarketWithLeftOverLimit:
                        ordersingle.set(new Price(ordem.Price));
                        break;
                    case OrdemTipoEnum.StopLimitada:
                    case OrdemTipoEnum.StopStart:
                        ordersingle.set(new Price(ordem.Price));
                        ordersingle.set(new StopPx(ordem.StopPrice));
                        break;
                    case OrdemTipoEnum.Mercado:
                    case OrdemTipoEnum.OnClose:
                        break;
                    default:
                        ordersingle.set(new Price(ordem.Price));
                        break;
                }

                ordersingle.set(new TransactTime(DateTime.Now));
                ordersingle.set(new HandlInst('1'));

                if (ordem.MaxFloor > 0)
                    ordersingle.set(new MaxFloor(Convert.ToDouble(ordem.MaxFloor)));

                if (ordem.MinQty > 0)
                    ordersingle.set(new MinQty(Convert.ToDouble(ordem.MinQty)));

                // Cliente
                QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroup1 = new QuickFix44.NewOrderSingle.NoPartyIDs();
                //PartyIDGroup1.set(new PartyID(ordem.Account.ToString()));
                PartyIDGroup1.set(new PartyID(ordem.ExecBroker));
                PartyIDGroup1.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup1.set(new PartyRole(PartyRole.ENTERING_TRADER));

                // Corretora
                QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroup2 = new QuickFix44.NewOrderSingle.NoPartyIDs();
                PartyIDGroup2.set(new PartyID(_config.PartyID));
                PartyIDGroup2.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup2.set(new PartyRole(PartyRole.ENTERING_FIRM));

                // Location ID
                QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroup3 = new QuickFix44.NewOrderSingle.NoPartyIDs();
                if (ordem.SenderLocation != null && ordem.SenderLocation.Length > 0)
                    PartyIDGroup3.set(new PartyID(ordem.SenderLocation));
                else
                    PartyIDGroup3.set(new PartyID(this._config.SenderLocationID));
                PartyIDGroup3.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup3.set(new PartyRole(PartyRole.SENDER_LOCATION));

                ordersingle.addGroup(PartyIDGroup1);
                ordersingle.addGroup(PartyIDGroup2);
                ordersingle.addGroup(PartyIDGroup3);

                //BEI - 2012-Nov-13
                if (ordem.ForeignFirm != null && ordem.ForeignFirm.Length > 0)
                {
                    QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroup5 = new QuickFix44.NewOrderSingle.NoPartyIDs();
                    PartyIDGroup5.set(new PartyID(ordem.ForeignFirm));
                    PartyIDGroup5.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                    PartyIDGroup5.set(new PartyRole(PartyRole.FOREIGN_FIRM));

                    ordersingle.addGroup(PartyIDGroup5);
                }

                //SelfTradeProtection - 2012-Nov-27
                if (ordem.InvestorID != null && ordem.InvestorID.Length > 0)
                {
                    QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroup6 = new QuickFix44.NewOrderSingle.NoPartyIDs();
                    PartyIDGroup6.set(new PartyID(ordem.InvestorID));
                    PartyIDGroup6.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                    PartyIDGroup6.set(new PartyRole(PartyRole.INVESTOR_ID));

                    ordersingle.addGroup(PartyIDGroup6);
                }

                if (ordem.ExecutingTrader != null && ordem.ExecutingTrader.Length > 0)
                {
                    QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroup7 = new QuickFix44.NewOrderSingle.NoPartyIDs();
                    PartyIDGroup7.set(new PartyID(ordem.ExecutingTrader));
                    PartyIDGroup7.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                    PartyIDGroup7.set(new PartyRole(PartyRole.EXECUTING_TRADER));

                    ordersingle.addGroup(PartyIDGroup7);
                }

                if (ordem.Account > 0)
                {
                    QuickFix44.NewOrderSingle.NoAllocs noAllocsGrp = new QuickFix44.NewOrderSingle.NoAllocs();
                    noAllocsGrp.set(new AllocAccount(ordem.Account.ToString()));
                    noAllocsGrp.set(new AllocAcctIDSource(99));
                    ordersingle.addGroup(noAllocsGrp);
                }

                if (ordem.PositionEffect != null && ordem.PositionEffect.Equals("C"))
                    ordersingle.set(new PositionEffect('C'));

                // Memo Field
                if (ordem.Memo5149 != null && ordem.Memo5149.Length > 0)
                {
                    if (ordem.Memo5149.Length > 50)
                        ordem.Memo5149 = ordem.Memo5149.Substring(0, 50);

                    ordersingle.setString(5149, ordem.Memo5149);
                }

                // AccountType
                if (ordem.AcountType == ContaTipoEnum.REMOVE_ACCOUNT_INFORMATION)
                {
                    ordersingle.setInt(581,38);
                }
                else if (ordem.AcountType == ContaTipoEnum.GIVE_UP_LINK_IDENTIFIER)
                {
                    ordersingle.setInt(581, 40);
                }


                // armazena para posterior uso em caso de rejeição
                if (ordemInfoPorClOrdId.ContainsKey(ordem.ClOrdID))
                    ordemInfoPorClOrdId[ordem.ClOrdID] = ordem;
                else
                    ordemInfoPorClOrdId.Add(ordem.ClOrdID, ordem);

                OrdemInfo clone = RoteadorOrdensUtil.CloneOrder(ordem);

                if (finalizarSinalizado)
                {
                    msg = "Ordem rejeitada. CanalNegociacaoBMF em processo de finalização";
                    status = StatusRoteamentoEnum.Erro;
                }
                else
                {
                    msg = "Ordem enviada com sucesso";
                    status = StatusRoteamentoEnum.Sucesso;

                    bool bRet = Session.sendToTarget(ordersingle, _session);
                    if (bRet == false)
                    {
                        msg = "Não foi possivel enviar a ordem";
                        status = StatusRoteamentoEnum.Erro;
                    }
                }

                ordem = clone;

                // Setando informações de acompanhamento
                AcompanhamentoOrdemInfo acompanhamento = new AcompanhamentoOrdemInfo();
                acompanhamento.NumeroControleOrdem = ordem.ClOrdID;
                acompanhamento.CodigoDoCliente = ordem.Account;
                acompanhamento.CodigoResposta = ordem.ExchangeNumberID;
                acompanhamento.Instrumento = ordem.Symbol;
                acompanhamento.SecurityID = ordem.SecurityID;
                acompanhamento.CanalNegociacao = ordem.ChannelID;
                acompanhamento.Direcao = ordem.Side;
                acompanhamento.QuantidadeSolicitada = ordem.OrderQty;
                acompanhamento.Preco = new Decimal(ordem.Price);
                if (status != StatusRoteamentoEnum.Sucesso)
                {
                    ordem.OrdStatus = OrdemStatusEnum.PENDENTE;
                    acompanhamento.StatusOrdem = OrdemStatusEnum.PENDENTE;
                    acompanhamento.Descricao = "Erro no envio para o canal Bovespa";
                }
                else
                {
                    ordem.OrdStatus = OrdemStatusEnum.ENVIADAPARAABOLSA;
                    acompanhamento.StatusOrdem = OrdemStatusEnum.ENVIADAPARAABOLSA;
                    acompanhamento.Descricao = "Ordem enviada para a Bovespa";
                }
                acompanhamento.DataOrdemEnvio = ordem.TransactTime;
                acompanhamento.DataAtualizacao = DateTime.Now;

                // Adicionando informações de acompanhamento ao OrdemInfo
                ordem.Acompanhamentos.Clear();
                ordem.Acompanhamentos.Add(acompanhamento);

                // Envia o report para os assinantes
                _sendExecutionReport(ordem);

            }
            catch (Exception ex)
            {
                logger.Error("ExecutarOrdem():" + ex.Message, ex);
                msg = "Erro no envio da ordem: " + ex.Message;
                status = StatusRoteamentoEnum.Erro;
            }

            response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaEO(msg, status);

            return response;
        }

        /// <summary>
        /// Envia um pedido de cancelamento de ordem para o canal correspondente
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecutarCancelamentoOrdemResponse CancelarOrdem(ExecutarCancelamentoOrdemRequest request)
        {
            ExecutarCancelamentoOrdemResponse response = new ExecutarCancelamentoOrdemResponse();

            string msg = null;
            StatusRoteamentoEnum status = StatusRoteamentoEnum.Erro;

            try
            {
                // extrai objeto info de cancelamento do request
                OrdemCancelamentoInfo orderCancelInfo = request.info;

                RoteadorOrdensUtil.ConsistirCancelamento(orderCancelInfo);

                //Cria a mensagem FIX de OrderCancelRequest
                QuickFix44.OrderCancelRequest orderCancel = new OrderCancelRequest();

                if ( orderCancelInfo.Account > 0 )
                    orderCancel.set(new Account(orderCancelInfo.Account.ToString()));

                orderCancel.set(new OrigClOrdID(orderCancelInfo.OrigClOrdID));
                orderCancel.set(new ClOrdID(orderCancelInfo.ClOrdID));
                if (orderCancelInfo.OrderID != null && orderCancelInfo.OrderID.Length > 0)
                {
                    orderCancel.set(new OrderID(orderCancelInfo.OrderID));
                }


                // Instrument Identification Block
                orderCancel.set(new Symbol(orderCancelInfo.Symbol));
                orderCancel.set(new SecurityID(orderCancelInfo.SecurityID));
                orderCancel.set(new SecurityIDSource(_config.SecurityIDSource));

                if (orderCancelInfo.Side == OrdemDirecaoEnum.Venda)
                    orderCancel.set(new Side(Side.SELL));
                else
                    orderCancel.set(new Side(Side.BUY));
                orderCancel.set(new TransactTime(DateTime.Now));

                //ATP - 2012-10-29
                //Qtde de contratos/papeis a serem cancelados
                orderCancel.set(new OrderQty(orderCancelInfo.OrderQty));

                // Cliente
                QuickFix44.OrderCancelRequest.NoPartyIDs PartyIDGroup1 = new QuickFix44.OrderCancelRequest.NoPartyIDs();
                PartyIDGroup1.set(new PartyID(orderCancelInfo.ExecBroker));
                PartyIDGroup1.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup1.set(new PartyRole(PartyRole.ENTERING_TRADER));

                // Corretora
                QuickFix44.OrderCancelRequest.NoPartyIDs PartyIDGroup2 = new QuickFix44.OrderCancelRequest.NoPartyIDs();
                PartyIDGroup2.set(new PartyID(_config.PartyID));
                PartyIDGroup2.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup2.set(new PartyRole(PartyRole.ENTERING_FIRM));

                QuickFix44.OrderCancelRequest.NoPartyIDs PartyIDGroup3 = new QuickFix44.OrderCancelRequest.NoPartyIDs();
                if (orderCancelInfo.SenderLocation != null && orderCancelInfo.SenderLocation.Length > 0)
                    PartyIDGroup3.set(new PartyID(orderCancelInfo.SenderLocation));
                else
                    PartyIDGroup3.set(new PartyID(this._config.SenderLocationID));
                PartyIDGroup3.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup3.set(new PartyRole(PartyRole.SENDER_LOCATION));

                orderCancel.addGroup(PartyIDGroup1);
                orderCancel.addGroup(PartyIDGroup2);
                orderCancel.addGroup(PartyIDGroup3);


                //BEI - 2012-Nov-14
                if (orderCancelInfo.ForeignFirm != null && orderCancelInfo.ForeignFirm.Length > 0)
                {
                    QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroup4 = new QuickFix44.NewOrderSingle.NoPartyIDs();
                    PartyIDGroup4.set(new PartyID(orderCancelInfo.ForeignFirm));
                    PartyIDGroup4.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                    PartyIDGroup4.set(new PartyRole(PartyRole.FOREIGN_FIRM));

                    orderCancel.addGroup(PartyIDGroup4);
                }

                if (orderCancelInfo.ExecutingTrader != null && orderCancelInfo.ExecutingTrader.Length > 0)
                {
                    QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroup7 = new QuickFix44.NewOrderSingle.NoPartyIDs();
                    PartyIDGroup7.set(new PartyID(orderCancelInfo.ExecutingTrader));
                    PartyIDGroup7.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                    PartyIDGroup7.set(new PartyRole(PartyRole.EXECUTING_TRADER));

                    orderCancel.addGroup(PartyIDGroup7);
                }

                /* if (!String.IsNullOrEmpty(orderCancelInfo.CompIDBolsa))
                {
                    QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroup8 = new QuickFix44.NewOrderSingle.NoPartyIDs();
                    PartyIDGroup8.set(new PartyID(orderCancelInfo.CompIDBolsa));
                    PartyIDGroup8.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                    // 1001 - PartyRole as Order Originating Session
                    PartyIDGroup8.set(new PartyRole(1001));

                    orderCancel.addGroup(PartyIDGroup8);
                }*/

                // Memo Field
                if (orderCancelInfo.Memo5149 != null && orderCancelInfo.Memo5149.Length > 0)
                {
                    if (orderCancelInfo.Memo5149.Length > 50)
                        orderCancelInfo.Memo5149 = orderCancelInfo.Memo5149.Substring(0, 50);

                    orderCancel.setString(5149, orderCancelInfo.Memo5149);
                }

                // armazena para posterior uso em caso de rejeição
                cancelInfoPorClOrdId.Add(orderCancelInfo.ClOrdID, orderCancelInfo);

                if (finalizarSinalizado)
                {
                    msg = "Cancelamento de Ordem rejeitado. CanalNegociacaoBMF em processo de finalização";
                    status = StatusRoteamentoEnum.Erro;
                }
                else
                {
                    msg = "Cancelamento de Ordem enviado com sucesso";
                    status = StatusRoteamentoEnum.Sucesso;

                    bool bRet = Session.sendToTarget(orderCancel, _session);
                    if (bRet == false)
                    {
                        msg = "Não foi possivel enviar o cancelamento de ordem";
                        status = StatusRoteamentoEnum.Erro;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(ExecutionReport): " + ex.Message, ex);
                msg = "onMessage(ExecutionReport): " + ex.Message;
                status = StatusRoteamentoEnum.Erro;
            }

            response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaCancelamento(msg, status);

            return response;
        }

        /// <summary>
        /// Envia um pedido de cancel/replace para o canal correspondente
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecutarModificacaoOrdensResponse ModificarOrdem(ExecutarModificacaoOrdensRequest request)
        {
            string msg = null;
            StatusRoteamentoEnum status = StatusRoteamentoEnum.Erro;
            ExecutarModificacaoOrdensResponse response = new ExecutarModificacaoOrdensResponse();

            // Se o logon não foi feito, rejeita imediatamente a ordem
            if (!_bConectadoBolsa)
            {
                response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaModificacao(
                    "Modificação de Ordem rejeitada. Logon não foi realizado no CanalNegociacaoBMF",
                    StatusRoteamentoEnum.Erro);

                return response;
            }

            try
            {
                OrdemInfo ordem = request.info;

                RoteadorOrdensUtil.ConsistirOrdem(ordem,true);

                //Cria a mensagem FIX de NewOrderSingle
                QuickFix44.OrderCancelReplaceRequest orderCancelReplaceReq = new OrderCancelReplaceRequest();

                orderCancelReplaceReq.set(new OrigClOrdID(ordem.OrigClOrdID));

                if ( ordem.Account > 0 )
                    orderCancelReplaceReq.set(new Account(ordem.Account.ToString()));

                // Instrument Identification Block
                orderCancelReplaceReq.set(new Symbol(ordem.Symbol));
                orderCancelReplaceReq.set(new SecurityID(ordem.SecurityID));
                orderCancelReplaceReq.set(new SecurityIDSource(_config.SecurityIDSource));

                if (ordem.ExchangeNumberID != null && ordem.ExchangeNumberID.Length > 0)
                {
                    orderCancelReplaceReq.set(new OrderID(ordem.ExchangeNumberID));
                }

                orderCancelReplaceReq.set(new ClOrdID(ordem.ClOrdID));
                //ordersingle.set(new IDSource());
                if (ordem.Side == OrdemDirecaoEnum.Venda)
                    orderCancelReplaceReq.set(new Side(Side.SELL));
                else
                    orderCancelReplaceReq.set(new Side(Side.BUY));


                switch (ordem.OrdType)
                {
                    case OrdemTipoEnum.Limitada:
                    case OrdemTipoEnum.MarketWithLeftOverLimit:
                        orderCancelReplaceReq.set(new Price(ordem.Price));
                        break;
                    case OrdemTipoEnum.StopLimitada:
                    case OrdemTipoEnum.StopStart:
                        orderCancelReplaceReq.set(new Price(ordem.Price));
                        orderCancelReplaceReq.set(new StopPx(ordem.StopPrice));
                        break;
                    case OrdemTipoEnum.Mercado:
                    case OrdemTipoEnum.OnClose:
                        break;
                    default:
                        orderCancelReplaceReq.set(new Price(ordem.Price));
                        break;
                }

                TimeInForce tif = TradutorFix.deOrdemValidadeParaTimeInForce(ordem.TimeInForce);
                if (tif != null)
                    orderCancelReplaceReq.set(tif);

                orderCancelReplaceReq.set(new OrderQty(ordem.OrderQty));


                if (ordem.TimeInForce == OrdemValidadeEnum.ValidoAteDeterminadaData)
                {
                    DateTime expiredate = Convert.ToDateTime(ordem.ExpireDate);
                    orderCancelReplaceReq.set(new ExpireDate(expiredate.ToString("yyyyMMdd")));
                }

                OrdType ordType = TradutorFix.deOrdemTipoParaOrdType(ordem.OrdType);
                if (ordType != null)
                    orderCancelReplaceReq.set(ordType);

                //if (ordem.OrdType == OrdemTipoEnum.StopLimitada )
                //{
                //    ordersingle.set(new StopPx(ordem.StopPx));
                //}

                orderCancelReplaceReq.set(new TransactTime(DateTime.Now));
                // Cliente
                QuickFix44.OrderCancelReplaceRequest.NoPartyIDs PartyIDGroup1 = new QuickFix44.OrderCancelReplaceRequest.NoPartyIDs();
                PartyIDGroup1.set(new PartyID(ordem.ExecBroker));
                PartyIDGroup1.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup1.set(new PartyRole(PartyRole.ENTERING_TRADER));

                // Corretora
                QuickFix44.OrderCancelReplaceRequest.NoPartyIDs PartyIDGroup2 = new QuickFix44.OrderCancelReplaceRequest.NoPartyIDs();
                PartyIDGroup2.set(new PartyID(_config.PartyID));
                PartyIDGroup2.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup2.set(new PartyRole(PartyRole.ENTERING_FIRM));

                // Location ID
                QuickFix44.OrderCancelReplaceRequest.NoPartyIDs PartyIDGroup3 = new QuickFix44.OrderCancelReplaceRequest.NoPartyIDs();
                if ( ordem.SenderLocation != null && ordem.SenderLocation.Length > 0 )
                    PartyIDGroup3.set(new PartyID(ordem.SenderLocation));
                else
                    PartyIDGroup3.set(new PartyID(this._config.SenderLocationID));
                PartyIDGroup3.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup3.set(new PartyRole(54));

                orderCancelReplaceReq.addGroup(PartyIDGroup1);
                orderCancelReplaceReq.addGroup(PartyIDGroup2);
                orderCancelReplaceReq.addGroup(PartyIDGroup3);

                //BEI - 2012-Nov-14
                if (ordem.ForeignFirm != null && ordem.ForeignFirm.Length > 0)
                {
                    QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroup4 = new QuickFix44.NewOrderSingle.NoPartyIDs();
                    PartyIDGroup4.set(new PartyID(ordem.ForeignFirm));
                    PartyIDGroup4.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                    PartyIDGroup4.set(new PartyRole(PartyRole.FOREIGN_FIRM));

                    orderCancelReplaceReq.addGroup(PartyIDGroup4);
                }

                if (ordem.Account > 0)
                {
                    QuickFix44.OrderCancelReplaceRequest.NoAllocs noAllocsGrp = new QuickFix44.OrderCancelReplaceRequest.NoAllocs();
                    noAllocsGrp.set(new AllocAccount(ordem.Account.ToString()));
                    noAllocsGrp.set(new AllocAcctIDSource(99));
                    orderCancelReplaceReq.addGroup(noAllocsGrp);
                }

                //SelfTradeProtection - 2012-Nov-27
                if (ordem.InvestorID != null && ordem.InvestorID.Length > 0)
                {
                    QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroup5 = new QuickFix44.NewOrderSingle.NoPartyIDs();
                    PartyIDGroup5.set(new PartyID(ordem.InvestorID));
                    PartyIDGroup5.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                    PartyIDGroup5.set(new PartyRole(PartyRole.INVESTOR_ID));

                    orderCancelReplaceReq.addGroup(PartyIDGroup5);
                }

                if (ordem.ExecutingTrader != null && ordem.ExecutingTrader.Length > 0)
                {
                    QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroup7 = new QuickFix44.NewOrderSingle.NoPartyIDs();
                    PartyIDGroup7.set(new PartyID(ordem.ExecutingTrader));
                    PartyIDGroup7.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                    PartyIDGroup7.set(new PartyRole(PartyRole.EXECUTING_TRADER));

                    orderCancelReplaceReq.addGroup(PartyIDGroup7);
                }

                // Memo Field
                if (ordem.Memo5149 != null && ordem.Memo5149.Length > 0)
                {
                    if (ordem.Memo5149.Length > 50)
                        ordem.Memo5149 = ordem.Memo5149.Substring(0, 50);

                    orderCancelReplaceReq.setString(5149, ordem.Memo5149);
                }

                // armazena para posterior uso em caso de rejeição
                if (ordemInfoPorClOrdId.ContainsKey(ordem.ClOrdID))
                    ordemInfoPorClOrdId[ordem.ClOrdID] = ordem;
                else
                    ordemInfoPorClOrdId.Add(ordem.ClOrdID, ordem);

                OrdemInfo clone = RoteadorOrdensUtil.CloneOrder(ordem);

                if (finalizarSinalizado)
                {
                    msg = "Modificação de Ordem rejeitada. CanalNegociacaoBMF em processo de finalização";
                    status = StatusRoteamentoEnum.Erro;
                }
                else
                {
                    msg = "Modificação de Ordem enviada com sucesso";
                    status = StatusRoteamentoEnum.Sucesso;

                    bool bRet = Session.sendToTarget(orderCancelReplaceReq, _session);
                    if (bRet == false)
                    {
                        msg = "Não foi possivel enviar a Modificação de ordem";
                        status = StatusRoteamentoEnum.Erro;
                    }
                }

                ordem = clone;

                // Setando informações de acompanhamento
                AcompanhamentoOrdemInfo acompanhamento = new AcompanhamentoOrdemInfo();
                acompanhamento.NumeroControleOrdem = ordem.ClOrdID;
                acompanhamento.CodigoDoCliente = ordem.Account;
                acompanhamento.CodigoResposta = ordem.ExchangeNumberID;
                acompanhamento.Instrumento = ordem.Symbol;
                acompanhamento.SecurityID = ordem.SecurityID;
                acompanhamento.CanalNegociacao = ordem.ChannelID;
                acompanhamento.Direcao = ordem.Side;
                acompanhamento.QuantidadeSolicitada = ordem.OrderQty;
                acompanhamento.Preco = new Decimal(ordem.Price);
                if (status != StatusRoteamentoEnum.Sucesso)
                {
                    ordem.OrdStatus = OrdemStatusEnum.PENDENTE;
                    acompanhamento.StatusOrdem = OrdemStatusEnum.PENDENTE;
                    acompanhamento.Descricao = "Erro no envio para o canal Bovespa";
                }
                else
                {
                    ordem.OrdStatus = OrdemStatusEnum.ENVIADAPARAABOLSA;
                    acompanhamento.StatusOrdem = OrdemStatusEnum.ENVIADAPARAABOLSA;
                    acompanhamento.Descricao = "Ordem enviada para a Bovespa";
                }
                acompanhamento.DataOrdemEnvio = ordem.TransactTime;
                acompanhamento.DataAtualizacao = DateTime.Now;

                // Adicionando informações de acompanhamento ao OrdemInfo
                ordem.Acompanhamentos.Clear();
                ordem.Acompanhamentos.Add(acompanhamento);

                // Envia o report para os assinantes
                _sendExecutionReport(ordem);
            }
            catch (Exception ex)
            {
                logger.Error("ModificarOrdem(): " + ex.Message, ex);
                msg = "Erro ao modificar ordem: " + ex.Message;
                status = StatusRoteamentoEnum.Erro;
            }

            response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaModificacao(msg, status);

            return response;
        }


        /// <summary>
        /// Envia um pedido de execucao de ordem para o canal correspondente
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecutarOrdemCrossResponse ExecutarOrdemCross(ExecutarOrdemCrossRequest request)
        {

            ExecutarOrdemCrossResponse response = new ExecutarOrdemCrossResponse();

            // Se o logon não foi feito, rejeita imediatamente a ordem
            if (!_bConectadoBolsa)
            {
                response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaEOX(
                    "Ordem rejeitada. Logon não foi realizado no CanalNegociacaoBovespa",
                    StatusRoteamentoEnum.Erro);

                return response;
            }

            OrdemCrossInfo crossinfo = request.info;
            OrdemInfo ordemCompra = crossinfo.OrdemInfoCompra;
            OrdemInfo ordemVenda = crossinfo.OrdemInfoVenda;

            // Cria mensagem de new order cross -
            // Essa mensagem nao é padrao do 4.2 - some weird fucking things can
            // fucking happen with this fucking shit code
            QuickFix44.NewOrderCross ordercross = new QuickFix44.NewOrderCross();

            ordercross.set(new CrossID(crossinfo.CrossID));
            // Unico valor aceito 1 - Negocio executado total ou parcial
            ordercross.set(new CrossType(1));
            // Prioridade de uma das pontas. Unico valor aceito: 0 - nenhuma
            ordercross.set(new CrossPrioritization(0));


            // Ordem cross, sempre 2 pernas
            // Informacoes da perna de compra
            QuickFix44.NewOrderCross.NoSides sideGroupC = new QuickFix44.NewOrderCross.NoSides();

            sideGroupC.set(new Side(Side.BUY));
            sideGroupC.set(new ClOrdID(ordemCompra.ClOrdID));
            if ( ordemCompra.Account > 0 )
                sideGroupC.set(new Account(ordemCompra.Account.ToString()));
            sideGroupC.set(new OrderQty(ordemCompra.OrderQty));
            //sideGroupC.set(new PositionEffect('C'));
            if (ordemCompra.PositionEffect != null && ordemCompra.PositionEffect.Equals("C"))
                sideGroupC.set(new PositionEffect('C'));

            // PartIDs Compra
            // Cliente
            QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroupC1 = new QuickFix44.NewOrderSingle.NoPartyIDs();
            PartyIDGroupC1.set(new PartyID(ordemCompra.ExecBroker));
            PartyIDGroupC1.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroupC1.set(new PartyRole(PartyRole.ENTERING_TRADER));

            // Corretora
            QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroupC2 = new QuickFix44.NewOrderSingle.NoPartyIDs();
            PartyIDGroupC2.set(new PartyID(_config.PartyID));
            PartyIDGroupC2.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroupC2.set(new PartyRole(PartyRole.ENTERING_FIRM));

            // Sender Location
            QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroupC3 = new QuickFix44.NewOrderSingle.NoPartyIDs();
            PartyIDGroupC3.set(new PartyID(this._config.SenderLocationID));
            PartyIDGroupC3.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroupC3.set(new PartyRole(54));

            // Adiciono os partIDs ao Side (perna de Compra)
            sideGroupC.addGroup(PartyIDGroupC1);
            sideGroupC.addGroup(PartyIDGroupC2);
            sideGroupC.addGroup(PartyIDGroupC3);

            //BEI - 2012-Nov-14
            if (ordemCompra.ForeignFirm != null && ordemCompra.ForeignFirm.Length > 0)
            {
                QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroupC4 = new QuickFix44.NewOrderSingle.NoPartyIDs();
                PartyIDGroupC4.set(new PartyID(ordemCompra.ForeignFirm));
                PartyIDGroupC4.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroupC4.set(new PartyRole(PartyRole.FOREIGN_FIRM));

                sideGroupC.addGroup(PartyIDGroupC4);
            }

            //SelfTradeProtection - 2012-Nov-27
            if (ordemCompra.InvestorID != null && ordemCompra.InvestorID.Length > 0)
            {
                QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroup4 = new QuickFix44.NewOrderSingle.NoPartyIDs();
                PartyIDGroup4.set(new PartyID(ordemCompra.InvestorID));
                PartyIDGroup4.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup4.set(new PartyRole(PartyRole.INVESTOR_ID));

                sideGroupC.addGroup(PartyIDGroup4);
            }

            if (ordemCompra.Account > 0)
            {
                QuickFix44.NewOrderSingle.NoAllocs noAllocsGrpC = new QuickFix44.NewOrderSingle.NoAllocs();
                noAllocsGrpC.set(new AllocAccount(ordemCompra.Account.ToString()));
                noAllocsGrpC.set(new AllocAcctIDSource(99));
                sideGroupC.addGroup(noAllocsGrpC);
            }



            // Insere informacoes da perna de venda
            QuickFix44.NewOrderCross.NoSides sideGroupV = new QuickFix44.NewOrderCross.NoSides();

            sideGroupV.set(new Side(Side.SELL));
            sideGroupV.set(new ClOrdID(ordemVenda.ClOrdID));
            if ( ordemVenda.Account > 0 )
                sideGroupV.set(new Account(ordemVenda.Account.ToString()));
            sideGroupV.set(new OrderQty(ordemVenda.OrderQty)); ;
            sideGroupV.set(new PositionEffect('C'));
            if (ordemVenda.PositionEffect != null && ordemVenda.PositionEffect.Equals("C"))
                sideGroupV.set(new PositionEffect('C'));

            // Cliente
            QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroup1 = new QuickFix44.NewOrderSingle.NoPartyIDs();
            PartyIDGroup1.set(new PartyID(ordemVenda.ExecBroker));
            PartyIDGroup1.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroup1.set(new PartyRole(PartyRole.ENTERING_TRADER));

            // Corretora
            QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroup2 = new QuickFix44.NewOrderSingle.NoPartyIDs();
            PartyIDGroup2.set(new PartyID(_config.PartyID));
            PartyIDGroup2.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroup2.set(new PartyRole(PartyRole.ENTERING_FIRM));

            // Sender Location ID
            QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroup3 = new QuickFix44.NewOrderSingle.NoPartyIDs();
            PartyIDGroup3.set(new PartyID(this._config.SenderLocationID));
            PartyIDGroup3.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroup3.set(new PartyRole(54));

            // Adiciono os partIDs ao Side (perna de venda)
            sideGroupV.addGroup(PartyIDGroup1);
            sideGroupV.addGroup(PartyIDGroup2);
            sideGroupV.addGroup(PartyIDGroup3);

            //BEI - 2012-Nov-14
            if (ordemVenda.ForeignFirm != null && ordemVenda.ForeignFirm.Length > 0)
            {
                QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroupV4 = new QuickFix44.NewOrderSingle.NoPartyIDs();
                PartyIDGroupV4.set(new PartyID(ordemVenda.ForeignFirm));
                PartyIDGroupV4.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroupV4.set(new PartyRole(PartyRole.FOREIGN_FIRM));

                sideGroupV.addGroup(PartyIDGroupV4);
            }

            //SelfTradeProtection - 2012-Nov-27
            if (ordemVenda.InvestorID != null && ordemVenda.InvestorID.Length > 0)
            {
                QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroup4 = new QuickFix44.NewOrderSingle.NoPartyIDs();
                PartyIDGroup4.set(new PartyID(ordemVenda.InvestorID));
                PartyIDGroup4.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup4.set(new PartyRole(PartyRole.INVESTOR_ID));

                sideGroupV.addGroup(PartyIDGroup4);
            }

            if (ordemVenda.Account > 0)
            {
                QuickFix44.NewOrderSingle.NoAllocs noAllocsGrpV = new QuickFix44.NewOrderSingle.NoAllocs();
                noAllocsGrpV.set(new AllocAccount(ordemVenda.Account.ToString()));
                noAllocsGrpV.set(new AllocAcctIDSource(99));
                sideGroupV.addGroup(noAllocsGrpV);
            }

            // Insere os grupos das 2 pernas
            ordercross.addGroup(sideGroupC);
            ordercross.addGroup(sideGroupV);

            ordercross.set(new Symbol(crossinfo.Symbol));
            if (crossinfo.SecurityID != null &&
                crossinfo.SecurityID.Length > 0)
            {
                ordercross.set(new SecurityID(crossinfo.SecurityID));
                ordercross.set(new SecurityIDSource("8"));
            }
            OrdType ordType = TradutorFix.deOrdemTipoParaOrdType(crossinfo.OrdType);
            if (ordType != null)
                ordercross.set(ordType);

            ordercross.set(new Price(crossinfo.Price));
            ordercross.set(new TransactTime(DateTime.Now));

            // Memo Field
            if (crossinfo.Memo5149 != null && crossinfo.Memo5149.Length > 0)
            {
                if (crossinfo.Memo5149.Length > 50)
                    crossinfo.Memo5149 = crossinfo.Memo5149.Substring(0, 50);

                ordercross.setString(5149, crossinfo.Memo5149);
            }

            //ordersingle.set(new HandlInst('1'));
            //ordersingle.set(new ExecBroker(_config.PartyID));

            //if (ordem.MaxFloor > 0)
            //    ordersingle.set(new MaxFloor(Convert.ToDouble(ordem.MaxFloor)));

            //if (ordem.MinQty > 0)
            //    ordersingle.set(new MinQty(Convert.ToDouble(ordem.MinQty)));

            //QuickFix42.NewOrderSingle.NoAllocs noAllocsGrp = new QuickFix42.NewOrderSingle.NoAllocs();
            //noAllocsGrp.set(new AllocAccount("67"));
            //ordersingle.addGroup(noAllocsGrp);

            // armazena as 2 pernas  para posterior uso em caso de rejeição
            // Perna de compra
            if (ordemInfoPorClOrdId.ContainsKey(ordemCompra.ClOrdID))
                ordemInfoPorClOrdId[ordemCompra.ClOrdID] = ordemCompra;
            else
                ordemInfoPorClOrdId.Add(ordemCompra.ClOrdID, ordemCompra);

            // Perna de Venda
            if (ordemInfoPorClOrdId.ContainsKey(ordemVenda.ClOrdID))
                ordemInfoPorClOrdId[ordemVenda.ClOrdID] = ordemVenda;
            else
                ordemInfoPorClOrdId.Add(ordemVenda.ClOrdID, ordemVenda);

            OrdemInfo cloneCompra = RoteadorOrdensUtil.CloneOrder(ordemCompra);
            OrdemInfo cloneVenda = RoteadorOrdensUtil.CloneOrder(ordemVenda);

            // Envio para o canal Bovespa
            string msg = null;
            StatusRoteamentoEnum status = StatusRoteamentoEnum.Erro;
            if (finalizarSinalizado)
            {
                msg = "Ordem rejeitada. CanalNegociacaoBovespa em processo de finalização";
                status = StatusRoteamentoEnum.Erro;
            }
            else
            {
                msg = "Ordem enviada com sucesso";
                status = StatusRoteamentoEnum.Sucesso;

                bool bRet = Session.sendToTarget(ordercross, _session);
                if (bRet == false)
                {
                    msg = "Não foi possivel enviar a ordem";
                    status = StatusRoteamentoEnum.Erro;
                }
            }

            ordemCompra = cloneCompra;
            ordemVenda = cloneVenda;

            // Setando informações de acompanhamento - lado compra
            AcompanhamentoOrdemInfo acompanhamentoCompra = new AcompanhamentoOrdemInfo();
            acompanhamentoCompra.NumeroControleOrdem = ordemCompra.ClOrdID;
            acompanhamentoCompra.CodigoDoCliente = ordemCompra.Account;
            acompanhamentoCompra.CodigoResposta = ordemCompra.ExchangeNumberID;
            acompanhamentoCompra.Instrumento = ordemCompra.Symbol;
            acompanhamentoCompra.SecurityID = ordemCompra.SecurityID;
            acompanhamentoCompra.CanalNegociacao = ordemCompra.ChannelID;
            acompanhamentoCompra.Direcao = ordemCompra.Side;
            acompanhamentoCompra.QuantidadeSolicitada = ordemCompra.OrderQty;
            acompanhamentoCompra.Preco = new Decimal(ordemCompra.Price);
            if (status != StatusRoteamentoEnum.Sucesso)
            {
                ordemCompra.OrdStatus = OrdemStatusEnum.PENDENTE;
                acompanhamentoCompra.StatusOrdem = OrdemStatusEnum.PENDENTE;
                acompanhamentoCompra.Descricao = "Erro no envio para o canal Bovespa";
            }
            else
            {
                ordemCompra.OrdStatus = OrdemStatusEnum.ENVIADAPARAABOLSA;
                acompanhamentoCompra.StatusOrdem = OrdemStatusEnum.ENVIADAPARAABOLSA;
                acompanhamentoCompra.Descricao = "Ordem enviada para a Bovespa";
            }
            acompanhamentoCompra.DataOrdemEnvio = ordemCompra.TransactTime;
            acompanhamentoCompra.DataAtualizacao = DateTime.Now;

            // Adicionando informações de acompanhamento ao OrdemInfo
            ordemCompra.Acompanhamentos.Clear();
            ordemCompra.Acompanhamentos.Add(acompanhamentoCompra);

            // Envia o report para os assinantes
            _sendExecutionReport(ordemCompra);


            // Setando informações de acompanhamento - lado venda
            AcompanhamentoOrdemInfo acompanhamentoVenda = new AcompanhamentoOrdemInfo();
            acompanhamentoVenda.NumeroControleOrdem = ordemVenda.ClOrdID;
            acompanhamentoVenda.CodigoDoCliente = ordemVenda.Account;
            acompanhamentoVenda.CodigoResposta = ordemVenda.ExchangeNumberID;
            acompanhamentoVenda.Instrumento = ordemVenda.Symbol;
            acompanhamentoVenda.SecurityID = ordemVenda.SecurityID;
            acompanhamentoVenda.CanalNegociacao = ordemVenda.ChannelID;
            acompanhamentoVenda.Direcao = ordemVenda.Side;
            acompanhamentoVenda.QuantidadeSolicitada = ordemVenda.OrderQty;
            acompanhamentoVenda.Preco = new Decimal(ordemVenda.Price);
            if (status != StatusRoteamentoEnum.Sucesso)
            {
                ordemVenda.OrdStatus = OrdemStatusEnum.PENDENTE;
                acompanhamentoVenda.StatusOrdem = OrdemStatusEnum.PENDENTE;
                acompanhamentoVenda.Descricao = "Erro no envio para o canal Bovespa";
            }
            else
            {
                ordemVenda.OrdStatus = OrdemStatusEnum.ENVIADAPARAABOLSA;
                acompanhamentoVenda.StatusOrdem = OrdemStatusEnum.ENVIADAPARAABOLSA;
                acompanhamentoVenda.Descricao = "Ordem enviada para a Bovespa";
            }
            acompanhamentoVenda.DataOrdemEnvio = ordemVenda.TransactTime;
            acompanhamentoVenda.DataAtualizacao = DateTime.Now;

            // Adicionando informações de acompanhamento ao OrdemInfo
            ordemVenda.Acompanhamentos.Clear();
            ordemVenda.Acompanhamentos.Add(acompanhamentoVenda);

            // Envia o report para os assinantes
            _sendExecutionReport(ordemVenda);

            response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaEOX(msg, status);

            return response;
        }

        /// <summary>
        /// Trata as requisicoes de ping - deve ser invocado para manter o canal WCF ativo
        /// </summary>
        /// <param name="request">objeto do tipo PingRequest</param>
        /// <returns>objeto do tipo PingResponse</returns>
        public PingResponse Ping(PingRequest request)
        {
            PingResponse response = new PingResponse();

            response.Timestamp = DateTime.Now;

            return response;
        }

        #endregion

        #region // Quickfix Application Members

        /// <summary>
        /// Trata mensagem de relatorio de execucao de ordem (acatamento,negocio, modificacao)
        /// </summary>
        /// <param name="message">QuickFix44.ExecutionReport message</param>
        /// <param name="session">QuickFix.SessionID session</param>
        public override void onMessage(ExecutionReport message, QuickFix.SessionID session)
        {
            try
            {
                OrdemInfo order = null;
                string descricao = " ";

                // verifica se OrdemInfo original está no Hashtable
                string chaveClOrdId = message.isSetClOrdID() ? message.getClOrdID().getValue() : null;

                // Se houve tentativa de cancelamento, tenta obter a ordem original
                if (cancelInfoPorClOrdId.ContainsKey(chaveClOrdId))
                {
                    string cancelOrdID = chaveClOrdId;
                    OrdemCancelamentoInfo cancelInfo = (OrdemCancelamentoInfo)cancelInfoPorClOrdId[chaveClOrdId];
                    chaveClOrdId = cancelInfo.OrigClOrdID;
                    cancelInfoPorClOrdId.Remove(cancelOrdID);
                    logger.Debug("onMessage(ExecutionReport): ClOrdId = [" + cancelOrdID + "] eh cancelamento de [" + chaveClOrdId + "]");
                }

                // Em caso de execution report de cancelamento ou modificação, o ClOrdId
                // da mensagem original está no campo OrigClOrdId
                //if (message.isSetOrdStatus())
                //{
                //    char statusExecReport = message.getOrdStatus().getValue();
                //    if (statusExecReport == OrdStatus.CANCELED || statusExecReport == OrdStatus.REPLACED)
                //        chaveClOrdId = message.getOrigClOrdID().ToString();
                //}

                if (!ordemInfoPorClOrdId.ContainsKey(chaveClOrdId))
                {
                    logger.Error("onMessage(ExecutionReport): ClOrdId não encontrado em ordemInfoPorClOrdId.");
                    logger.Error("onMessage(ExecutionReport): ClOrdId = " + chaveClOrdId);
                }
                else
                {
                    order = (OrdemInfo)ordemInfoPorClOrdId[chaveClOrdId];
                    logger.Debug("onMessage(ExecutionReport): Recuperada OrdemInfo de ordemInfoPorClOrdId");
                }

                // Para OrdStatus diferente de nova ou parcialmente executada,
                // remove OrdemInfo correspondentes das hashtables
                char status = message.getOrdStatus().getValue();
                if (order != null && status != OrdStatus.NEW && status != OrdStatus.PARTIALLY_FILLED && status != OrdStatus.REPLACED)
                {
                    ordemInfoPorClOrdId.Remove(chaveClOrdId);
                    logger.Debug("onMessage(ExecutionReport): Removida OrdemInfo de ordemInfoPorClOrdId");

                    if (!clOrdIdMsgSeqNum.ContainsKey(chaveClOrdId))
                    {
                        logger.Error("onMessage(ExecutionReport): ClOrdId não encontrado em clOrdIdMsgSeqNum.");
                        logger.Error("onMessage(ExecutionReport): ClOrdId = " + chaveClOrdId);
                    }
                    else
                    {
                        int chaveMsgSeqNum = (int)clOrdIdMsgSeqNum[chaveClOrdId];
                        if (!ordemInfoPorMsgSeqNum.ContainsKey(chaveMsgSeqNum))
                        {
                            logger.Error("onMessage(ExecutionReport): MsgSeqNum não encontrado em clOrdIdMsgSeqNum.");
                            logger.Error("onMessage(ExecutionReport): MsgSeqNum = " + chaveMsgSeqNum);
                        }
                        else
                        {
                            ordemInfoPorMsgSeqNum.Remove(chaveMsgSeqNum);
                            logger.Debug("onMessage(ExecutionReport): Removida OrdemInfo de ordemInfoPorMsgSeqNum");

                            clOrdIdMsgSeqNum.Remove(chaveClOrdId);
                            logger.Debug("onMessage(ExecutionReport): Removida OrdemInfo de clOrdIdMsgSeqNum");

                        }
                    }
                }
                else
                {
                    logger.Debug("onMessage(ExecutionReport): Mantendo OrdemInfo nas Hashtables");
                }

                // se OrdemInfo não foi encontrada na Hashtable, "remonta" a partir do ExecutionReport
                if (order == null)
                {
                    order = new OrdemInfo();

                    order.Account = Convert.ToInt32(message.isSetAccount() ? message.getAccount().getValue() : "0");
                    order.Exchange = _config.Bolsa;
                    order.ChannelID = _config.Operador;
                    order.ClOrdID = chaveClOrdId;
                    order.OrigClOrdID = message.isSetOrigClOrdID() ? message.getOrigClOrdID().getValue() : null;
                    order.ExecBroker = _config.PartyID;
                    if (message.isSetExpireDate())
                    {
                        string expdate = message.getExpireDate().getValue() + "235959";
                        order.ExpireDate = DateTime.ParseExact(expdate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        order.ExpireDate = DateTime.MinValue;
                    }
                    order.MaxFloor = message.isSetMaxFloor() ? message.getMaxFloor().getValue() : 0;
                    order.MinQty = message.isSetMinQty() ? message.getMinQty().getValue() : 0;
                    order.OrderQty = Convert.ToInt32(message.isSetOrderQty() ? message.getOrderQty().getValue() : 0);
                    order.OrdType = TradutorFix.TraduzirOrdemTipo(message.getOrdType().getValue());
                    order.Price = message.isSetPrice() ? message.getPrice().getValue() : 0;
                    order.SecurityID = message.isSetSecurityID() ? message.getSecurityID().ToString() : null;
                    order.Side = message.isSetSide() ? TradutorFix.TraduzirOrdemDirecao(message.getSide().getValue()) : OrdemDirecaoEnum.NaoInformado;
                    order.Symbol = message.isSetSymbol() ? message.getSymbol().ToString() : null;
                    order.TimeInForce = message.isSetTimeInForce() ? TradutorFix.TraduzirOrdemValidade(message.getTimeInForce().getValue()) : OrdemValidadeEnum.NaoInformado;
                }

                order.ExchangeNumberID = message.isSetOrderID() ? message.getOrderID().getValue() : null;
                order.OrderQtyRemmaining = Convert.ToInt32(message.isSetLeavesQty() ? message.getLeavesQty().getValue() : 0);
                order.CumQty = Convert.ToInt32(message.isSetCumQty() ? message.getCumQty().getValue() : 0);
                order.OrdStatus = TradutorFix.TraduzirOrdemStatus(message.getOrdStatus().getValue());
                order.Side = message.isSetSide() ? TradutorFix.TraduzirOrdemDirecao(message.getSide().getValue()) : order.Side;
                order.Memo5149 = message.isSetField(5149) ? message.getString(5149) : String.Empty;
                order.PossDupFlag = message.getHeader().isSetField(PossDupFlag.FIELD) ? message.getHeader().getBoolean(PossDupFlag.FIELD) : false;
                order.PossResend = message.getHeader().isSetField(PossResend.FIELD) ? message.getHeader().getBoolean(PossResend.FIELD) : false;
                order.CompIDBolsa = message.getHeader().isSetField(SenderCompID.FIELD) ? message.getHeader().getString(SenderCompID.FIELD) : _config.TargetCompID;
                order.CompIDOMS = message.getHeader().isSetField(TargetCompID.FIELD) ? message.getHeader().getString(TargetCompID.FIELD) : _config.SenderCompID;

                switch (order.OrdStatus)
                {
                    case OrdemStatusEnum.NOVA:
                        descricao = "Ordem aberta";
                        order.RegisterTime = DateTime.Now;
                        break;
                    case OrdemStatusEnum.CANCELADA:
                        descricao = "Ordem cancelada";
                        break;
                    case OrdemStatusEnum.PARCIALMENTEEXECUTADA:
                        descricao = "Ordem com execucao parcial";
                        break;
                    case OrdemStatusEnum.SUSPENSA:
                        descricao = "Ordem suspensa";
                        break;
                    case OrdemStatusEnum.EXECUTADA:
                        descricao = "Ordem executada";
                        break;
                    case OrdemStatusEnum.SUBSTITUIDA:
                        descricao = "Ordem substituida";
                        break;
                    case OrdemStatusEnum.REJEITADA:
                        if (message.isSetText())
                            descricao = message.getText().getValue();
                        break;
                }
                order.TransactTime = DateTime.Now;
                // Try get the msg seq number
                order.FixMsgSeqNum = message.getHeader().isSetField(MsgSeqNum.FIELD)? message.getHeader().getInt(MsgSeqNum.FIELD):0;
                order.ProtectionPrice = Convert.ToDecimal(message.isSetField(35001) ? message.getString(35001) : "0");


                // Setando informações de acompanhamento
                AcompanhamentoOrdemInfo acompanhamento = new AcompanhamentoOrdemInfo();
                acompanhamento.NumeroControleOrdem = order.ClOrdID;
                acompanhamento.CodigoDoCliente = order.Account;
                acompanhamento.CodigoResposta = order.ExchangeNumberID;
                acompanhamento.CodigoTransacao = message.isSetExecID() ? message.getExecID().getValue() : null;
                acompanhamento.Instrumento = order.Symbol;
                acompanhamento.SecurityID = order.SecurityID;
                acompanhamento.CanalNegociacao = order.ChannelID;
                acompanhamento.Direcao = order.Side;
                acompanhamento.QuantidadeSolicitada = order.OrderQty;
                acompanhamento.QuantidadeExecutada = (int) message.getCumQty().getValue();
                acompanhamento.QuantidadeRemanescente = (int) message.getLeavesQty().getValue();
                acompanhamento.QuantidadeNegociada = message.isSetLastQty() ? (int)message.getLastQty().getValue():0;
                acompanhamento.Preco = new Decimal(order.Price);
                acompanhamento.StatusOrdem = TradutorFix.TraduzirOrdemStatus(message.getOrdStatus().getValue());
                acompanhamento.DataOrdemEnvio = order.TransactTime;
                acompanhamento.DataAtualizacao = DateTime.Now;
                acompanhamento.CodigoRejeicao = message.isSetOrdRejReason() ? message.getOrdRejReason().ToString() : "0";
                acompanhamento.Descricao = descricao;
                // Try get the msg seq number
                acompanhamento.FixMsgSeqNum = message.getHeader().isSetField(MsgSeqNum.FIELD) ? message.getHeader().getInt(MsgSeqNum.FIELD) : 0;
                acompanhamento.LastPx = message.isSetLastPx() ? (Decimal)message.getLastPx().getValue() : new Decimal(order.Price);
                acompanhamento.TradeDate = message.isSetTradeDate() ? message.getTradeDate().getValue() : DateTime.Now.ToString("yyyyMMdd");

                //BEI Fields
                //Added in 2012-Nov-13 by ATP
                acompanhamento.ExchangeOrderID = message.isSetField(35022) ? message.getString(35022) : String.Empty;
                acompanhamento.ExchangeExecID = message.isSetField(35023) ? message.getString(35023) : String.Empty;
                acompanhamento.LastPxInIssuedCurrency = Convert.ToDecimal(message.isSetField(35024) ? message.getString(35024) : "0");
                acompanhamento.PriceInIssuedCurrency = Convert.ToDecimal(message.isSetField(35025) ? message.getString(35025) : "0");
                acompanhamento.ExchangeSecondaryOrderID = message.isSetField(35026) ? message.getString(35026) : String.Empty;
                acompanhamento.TradeLinkID = message.isSetField(820) ? message.getString(820) : String.Empty;
                acompanhamento.OrderLinkID = message.isSetField(5975) ? message.getString(5975) : String.Empty;
                acompanhamento.ExchangeQuoteID = message.isSetField(5001) ? message.getString(5001) : String.Empty;
                acompanhamento.PossDupFlag = message.getHeader().isSetField(PossDupFlag.FIELD) ? message.getHeader().getBoolean(PossDupFlag.FIELD) : false;
                acompanhamento.PossResend = message.getHeader().isSetField(PossResend.FIELD) ? message.getHeader().getBoolean(PossResend.FIELD) : false;
                acompanhamento.CompIDBolsa = message.getHeader().isSetField(SenderCompID.FIELD) ? message.getHeader().getString(SenderCompID.FIELD) : _config.TargetCompID;
                acompanhamento.CompIDOMS = message.getHeader().isSetField(TargetCompID.FIELD) ? message.getHeader().getString(TargetCompID.FIELD) : _config.SenderCompID;

                if (message.isSetNoMiscFees())
                {
                    QuickFix.NoMiscFees noMiscFees = new QuickFix.NoMiscFees();
                    message.get(noMiscFees);

                    int ocorr = noMiscFees.getValue();

                    for(uint i=0; i < ocorr; i++)
                    {
                        EmolumentoInfo emol = new EmolumentoInfo();

                        QuickFix44.ExecutionReport.NoMiscFees feeGroup = new ExecutionReport.NoMiscFees();

                        message.getGroup(i, feeGroup);

                        emol.Valor = Convert.ToDecimal(feeGroup.isSetField(MiscFeeAmt.FIELD) ? feeGroup.getString(MiscFeeAmt.FIELD) : "0");
                        emol.BaseEmolumento = Convert.ToInt32(feeGroup.isSetField(MiscFeeBasis.FIELD) ? feeGroup.getString(MiscFeeBasis.FIELD) : "0");
                        emol.Currency = feeGroup.getString(MiscFeeCurr.FIELD);
                        emol.Tipo = TradutorFix.TraduzirEmolumentoTipo(feeGroup.getInt(MiscFeeType.FIELD));
                    }
                }



                // Adicionando informações de acompanhamento ao OrdemInfo
                order.Acompanhamentos.Clear();
                order.Acompanhamentos.Add(acompanhamento);

                // Envia o report para os assinantes
                _sendExecutionReport(order);
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(ExecutionReport): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Trata mensagem de rejeição de ordem
        /// </summary>
        /// <param name="message">QuickFix44.Reject message</param>
        /// <param name="session">QuickFix.SessionID session</param>
        public override void onMessage(Reject message, SessionID session)
        {
            try
            {
                // Recupera OrdemInfo que foi rejeitada
                OrdemInfo ordemInfoOriginal = null;
                int chaveRefSeqNum = message.getRefSeqNum().getValue();
                if (!ordemInfoPorMsgSeqNum.ContainsKey(chaveRefSeqNum))
                {
                    logger.Error("onMessage(Reject): MsgSeqNum não encontrado em ordemInfoPorClOrdId.");
                    logger.Error("onMessage(Reject): MsgSeqNum = " + chaveRefSeqNum);
                }
                else
                {
                    ordemInfoOriginal = (OrdemInfo)ordemInfoPorMsgSeqNum[chaveRefSeqNum];
                }

                // remove OrdemInfo correspondentes das hashtables
                ordemInfoPorMsgSeqNum.Remove(chaveRefSeqNum);
                logger.Debug("onMessage(Reject): Removida OrdemInfo de ordemInfoPorMsgSeqNum");

                string clOrderIdOriginal = ordemInfoOriginal.ClOrdID;
                if (!ordemInfoPorClOrdId.ContainsKey(clOrderIdOriginal))
                {
                    logger.Error("onMessage(Reject): ClOrdID não encontrado em ordemInfoPorMsgSeqNum.");
                    logger.Error("onMessage(Reject): ClOrdID = " + clOrderIdOriginal);
                }
                else
                {
                    ordemInfoPorClOrdId.Remove(clOrderIdOriginal);
                    logger.Debug("onMessage(Reject): Removida OrdemInfo de ordemInfoPorClOrdId");
                }

                if (!clOrdIdMsgSeqNum.ContainsKey(clOrderIdOriginal))
                {
                    logger.Error("onMessage(Reject): ClOrdID não encontrado em clOrdIdMsgSeqNum.");
                    logger.Error("onMessage(Reject): ClOrdID = " + clOrderIdOriginal);
                }
                else
                {
                    clOrdIdMsgSeqNum.Remove(clOrderIdOriginal);
                    logger.Debug("onMessage(Reject): Removida OrdemInfo de clOrdIdMsgSeqNum");
                }

                // Atualiza Status da Ordem no objeto OrdemInfo
                ordemInfoOriginal.OrdStatus = OrdemStatusEnum.REJEITADA;
                // Try get the msg seq number
                ordemInfoOriginal.FixMsgSeqNum = message.getHeader().isSetField(MsgSeqNum.FIELD) ? message.getHeader().getInt(MsgSeqNum.FIELD) : 0;
                ordemInfoOriginal.PossDupFlag = message.getHeader().isSetField(PossDupFlag.FIELD) ? message.getHeader().getBoolean(PossDupFlag.FIELD) : false;
                ordemInfoOriginal.PossResend = message.getHeader().isSetField(PossResend.FIELD) ? message.getHeader().getBoolean(PossResend.FIELD) : false;
                ordemInfoOriginal.CompIDBolsa = message.getHeader().isSetField(SenderCompID.FIELD) ? message.getHeader().getString(SenderCompID.FIELD) : _config.TargetCompID;
                ordemInfoOriginal.CompIDOMS = message.getHeader().isSetField(TargetCompID.FIELD) ? message.getHeader().getString(TargetCompID.FIELD) : _config.SenderCompID;

                AcompanhamentoOrdemInfo acompanhamento = new AcompanhamentoOrdemInfo();

                acompanhamento.CanalNegociacao = ordemInfoOriginal.ChannelID;
                acompanhamento.CodigoDoCliente = ordemInfoOriginal.Account;
                acompanhamento.CodigoTransacao = null;
                acompanhamento.DataAtualizacao = DateTime.Now;
                acompanhamento.FixMsgSeqNum = message.getHeader().isSetField(MsgSeqNum.FIELD) ? message.getHeader().getInt(MsgSeqNum.FIELD) : 0;
                acompanhamento.PossDupFlag = message.getHeader().isSetField(PossDupFlag.FIELD) ? message.getHeader().getBoolean(PossDupFlag.FIELD) : false;
                acompanhamento.PossResend = message.getHeader().isSetField(PossResend.FIELD) ? message.getHeader().getBoolean(PossResend.FIELD) : false;
                acompanhamento.CompIDBolsa = message.getHeader().isSetField(SenderCompID.FIELD) ? message.getHeader().getString(SenderCompID.FIELD) : _config.TargetCompID;
                acompanhamento.CompIDOMS = message.getHeader().isSetField(TargetCompID.FIELD) ? message.getHeader().getString(TargetCompID.FIELD) : _config.SenderCompID;
                acompanhamento.CodigoRejeicao = message.isSetSessionRejectReason() ? message.getSessionRejectReason().ToString() : "0";
                acompanhamento.Descricao = "RefSeqNum=[" + message.getRefSeqNum().ToString() + "] " +
                                            "getRefTagID=[" + message.getRefTagID().ToString() + "] " +
                                            "Error=[" + message.getText().ToString() + "]";

                ordemInfoOriginal.Acompanhamentos.Add(acompanhamento);

                // Envia o report para os assinantes
                _sendExecutionReport(ordemInfoOriginal);

                Text text = message.getText();
                int msgSeqNum = Convert.ToInt32(message.getRefSeqNum().ToString());

                logger.Error("onMessage(Reject): onMessage(Reject) SessionID: " + session.ToString());
                logger.Error("onMessage(Reject): RefMsgType=[" + message.getRefMsgType().ToString() + "]");
                logger.Error("onMessage(Reject): RefSeqNum=[" + message.getRefSeqNum().ToString() + "]");
                logger.Error("onMessage(Reject): getRefTagID=[" + message.getRefTagID().ToString() + "]");
                logger.Error("onMessage(Reject): Error=[" + text.ToString() + "]");

            }
            catch (Exception ex)
            {
                logger.Error("onMessage(Reject): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Trata mensagem BusinessReject
        /// </summary>
        /// <param name="message">QuickFix44.Reject message</param>
        /// <param name="session">QuickFix.SessionID session</param>
        public override void onMessage(BusinessMessageReject message, SessionID session)
        {
            try
            {

                bool bGotByClOrdID = false;
                OrdemInfo ordemInfoOriginal = null;

                if ( message.isSetBusinessRejectRefID() )
                {
                    string chaveClOrdId = message.getBusinessRejectRefID().ToString();

                    if (!ordemInfoPorClOrdId.ContainsKey(chaveClOrdId))
                    {
                        logger.Error("onMessage(ExecutionReport): ClOrdId não encontrado em ordemInfoPorClOrdId.");
                        logger.Error("onMessage(ExecutionReport): ClOrdId = " + chaveClOrdId);
                    }
                    else
                    {
                        ordemInfoOriginal = (OrdemInfo)ordemInfoPorClOrdId[chaveClOrdId];
                        logger.Debug("onMessage(ExecutionReport): Recuperada OrdemInfo de ordemInfoPorClOrdId");
                        bGotByClOrdID = true;
                    }

                }

                // remove OrdemInfo correspondentes das hashtables
                int chaveRefSeqNum = message.getRefSeqNum().getValue();
                if (!bGotByClOrdID)
                {
                    if (!ordemInfoPorMsgSeqNum.ContainsKey(chaveRefSeqNum))
                    {
                        logger.Error("onMessage(BusinessMessageReject): MsgSeqNum não encontrado em ordemInfoPorClOrdId.");
                        logger.Error("onMessage(BusinessMessageReject): MsgSeqNum = " + chaveRefSeqNum);
                    }
                    else
                    {
                        ordemInfoOriginal = (OrdemInfo)ordemInfoPorMsgSeqNum[chaveRefSeqNum];

                        string clOrderIdOriginal = ordemInfoOriginal.ClOrdID;

                        ordemInfoPorClOrdId.Remove(clOrderIdOriginal);

                        clOrdIdMsgSeqNum.Remove(clOrdIdMsgSeqNum);
                    }
                }

                AcompanhamentoOrdemInfo acompanhamento = new AcompanhamentoOrdemInfo();

                acompanhamento.FixMsgSeqNum = message.getHeader().isSetField(MsgSeqNum.FIELD) ? message.getHeader().getInt(MsgSeqNum.FIELD) : 0;
                acompanhamento.CanalNegociacao = ordemInfoOriginal.ChannelID;
                acompanhamento.CodigoDoCliente = ordemInfoOriginal.Account;
                acompanhamento.CodigoTransacao = null;
                acompanhamento.DataAtualizacao = DateTime.Now;
                acompanhamento.CodigoRejeicao = message.isSetBusinessRejectReason() ? message.getBusinessRejectReason().ToString() : "0";
                acompanhamento.FixMsgSeqNum = message.getHeader().isSetField(MsgSeqNum.FIELD) ? message.getHeader().getInt(MsgSeqNum.FIELD) : 0;
                acompanhamento.PossDupFlag = message.getHeader().isSetField(PossDupFlag.FIELD) ? message.getHeader().getBoolean(PossDupFlag.FIELD) : false;
                acompanhamento.PossResend = message.getHeader().isSetField(PossResend.FIELD) ? message.getHeader().getBoolean(PossResend.FIELD) : false;
                acompanhamento.CompIDBolsa = message.getHeader().isSetField(SenderCompID.FIELD) ? message.getHeader().getString(SenderCompID.FIELD) : _config.TargetCompID;
                acompanhamento.CompIDOMS = message.getHeader().isSetField(TargetCompID.FIELD) ? message.getHeader().getString(TargetCompID.FIELD) : _config.SenderCompID;

                acompanhamento.Descricao = "RefSeqNum=[" + message.getRefSeqNum().ToString() + "] " +
                                            "getBusinessRejectRefID=[" + message.getBusinessRejectRefID().ToString() + "] " +
                                            "Error=[" + message.getText().ToString() + "]";

                ordemInfoOriginal.Acompanhamentos.Add(acompanhamento);
                ordemInfoOriginal.FixMsgSeqNum = message.getHeader().isSetField(MsgSeqNum.FIELD) ? message.getHeader().getInt(MsgSeqNum.FIELD) : 0;
                ordemInfoOriginal.Memo5149 = message.isSetField(5149) ? message.getString(5149) : String.Empty;
                ordemInfoOriginal.PossDupFlag = message.getHeader().isSetField(PossDupFlag.FIELD) ? message.getHeader().getBoolean(PossDupFlag.FIELD) : false;
                ordemInfoOriginal.PossResend = message.getHeader().isSetField(PossResend.FIELD) ? message.getHeader().getBoolean(PossResend.FIELD) : false;
                ordemInfoOriginal.CompIDBolsa = message.getHeader().isSetField(SenderCompID.FIELD) ? message.getHeader().getString(SenderCompID.FIELD) : _config.TargetCompID;
                ordemInfoOriginal.CompIDOMS = message.getHeader().isSetField(TargetCompID.FIELD) ? message.getHeader().getString(TargetCompID.FIELD) : _config.SenderCompID;


                // Envia o report para os assinantes
                _sendExecutionReport(ordemInfoOriginal);

                Text text = message.getText();
                int msgSeqNum = Convert.ToInt32(message.getRefSeqNum().ToString());

                logger.Error("onMessage(BusinessMessageReject): onMessage(Reject) SessionID: " + session.ToString());
                logger.Error("onMessage(BusinessMessageReject): RefMsgType=[" + message.getRefMsgType().ToString() + "]");
                logger.Error("onMessage(BusinessMessageReject): RefSeqNum=[" + message.getRefSeqNum().ToString() + "]");
                logger.Error("onMessage(BusinessMessageReject): getBusinessRejectRefID=[" + message.getBusinessRejectRefID().ToString() + "]");
                logger.Error("onMessage(BusinessMessageReject): Error=[" + text.ToString() + "]");

            }
            catch (Exception ex)
            {
                logger.Error("onMessage(BusinessMessageReject): " + ex.Message, ex);
            }
        }

        public override void onMessage(NewOrderSingle order, SessionID session)
        {
            try
            {
                string chaveOrdID = order.getClOrdID().getValue();

                if (!ordemInfoPorClOrdId.ContainsKey(chaveOrdID))
                {
                    logger.Error("onMessage(NewOrderSingle): ClOrdID não encontrado em ordemInfoPorClOrdID. ClOrdID = " + chaveOrdID.ToString());
                }
                else
                {
                    OrdemInfo ordemInfo = (OrdemInfo)ordemInfoPorClOrdId[chaveOrdID];

                    int msgSeqNum = order.getHeader().getInt(MsgSeqNum.FIELD);

                    if (ordemInfoPorMsgSeqNum.ContainsKey(msgSeqNum))
                    {
                        ordemInfoPorMsgSeqNum[msgSeqNum] = ordemInfo;
                    }
                    else
                    {
                        ordemInfoPorMsgSeqNum.Add(msgSeqNum, ordemInfo);
                    }

                    if (clOrdIdMsgSeqNum.ContainsKey(chaveOrdID))
                    {
                        clOrdIdMsgSeqNum[chaveOrdID] = msgSeqNum;
                    }
                    else
                    {
                        clOrdIdMsgSeqNum.Add(chaveOrdID, msgSeqNum);
                    }

                    logger.Debug("onMessage(NewOrderSingle): Inserido OrdemInfo na hash por MsgSeqNum");
                    logger.Debug("onMessage(NewOrderSingle): MsgSeqNum = " + msgSeqNum);
                    logger.Debug("onMessage(NewOrderSingle): ClOrdID = " + chaveOrdID.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(NewOrderSingle): " + ex.Message, ex);
            }
        }

        public override void onMessage(OrderCancelRequest order, SessionID session)
        {
            try
            {
                string chaveOrdID = order.getClOrdID().getValue();

                if (!ordemInfoPorClOrdId.ContainsKey(chaveOrdID))
                {
                    logger.Error("onMessage(OrderCancelRequest): ClOrdID não encontrado em ordemInfoPorClOrdID. ClOrdID = " + chaveOrdID.ToString());
                }
                else
                {
                    OrdemCancelamentoInfo ordemInfo = (OrdemCancelamentoInfo)ordemInfoPorClOrdId[chaveOrdID];

                    int msgSeqNum = order.getHeader().getInt(MsgSeqNum.FIELD);

                    ordemInfoPorMsgSeqNum.Add(msgSeqNum, ordemInfo);

                    clOrdIdMsgSeqNum.Add(chaveOrdID, msgSeqNum);

                    logger.Debug("onMessage(OrderCancelRequest): Inserido OrdemInfo na hash por MsgSeqNum");
                    logger.Debug("onMessage(OrderCancelRequest): MsgSeqNum = " + msgSeqNum);
                    logger.Debug("onMessage(OrderCancelRequest): ClOrdID = " + chaveOrdID.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(OrderCancelRequest): " + ex.Message, ex);
            }

            return;
        }

        public override void onMessage(OrderCancelReplaceRequest order, SessionID session)
        {
            try
            {
                string chaveOrdID = order.getClOrdID().getValue();

                if (!ordemInfoPorClOrdId.ContainsKey(chaveOrdID))
                {
                    logger.Error("onMessage(OrderCancelReplaceRequest): ClOrdID não encontrado em ordemInfoPorClOrdID. ClOrdID = " + chaveOrdID.ToString());
                }
                else
                {
                    OrdemInfo ordemInfo = (OrdemInfo)ordemInfoPorClOrdId[chaveOrdID];

                    int msgSeqNum = order.getHeader().getInt(MsgSeqNum.FIELD);

                    ordemInfoPorMsgSeqNum.Add(msgSeqNum, ordemInfo);

                    clOrdIdMsgSeqNum.Add(chaveOrdID, msgSeqNum);

                    logger.Debug("onMessage(OrderCancelReplaceRequest): Inserido OrdemInfo na hash por MsgSeqNum");
                    logger.Debug("onMessage(OrderCancelReplaceRequest): MsgSeqNum = " + msgSeqNum);
                    logger.Debug("onMessage(OrderCancelReplaceRequest): ClOrdID = " + chaveOrdID.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(OrderCancelReplaceRequest): " + ex.Message, ex);
            }

            return;
        }

        //public override void onMessage(OrderCancelReject message, SessionID session)
        //{
        //    try
        //    {
        //        OrdemCancelamentoInfo cancelamento = null;

        //        //// remove OrdemInfo correspondentes das hashtables
        //        //int chaveRefSeqNum = message.getHeader().getInt(MsgSeqNum.FIELD);

        //        //if (!ordemInfoPorMsgSeqNum.ContainsKey(chaveRefSeqNum))
        //        //{
        //        //    logger.Error("onMessage(OrderCancelReject): MsgSeqNum não encontrado em ordemInfoPorClOrdId.");
        //        //    logger.Error("onMessage(OrderCancelReject): MsgSeqNum = " + chaveRefSeqNum);

        //        //    cancelamento = new OrdemCancelamentoInfo();

        //        //    cancelamento.Account = Convert.ToInt32(message.isSetAccount() ? message.getAccount().ToString() : "0");
        //        //    cancelamento.ChannelID = _config.Operador;
        //        //    cancelamento.Exchange = _config.Bolsa;
        //        //    cancelamento.OrigClOrdID = message.isSetOrigClOrdID() ? message.getOrigClOrdID().ToString() : null;
        //        //    cancelamento.ClOrdID = message.isSetClOrdID() ? message.getClOrdID().ToString() : null;
        //        //}
        //        //else
        //        //{
        //        //    cancelamento = (OrdemCancelamentoInfo)ordemInfoPorMsgSeqNum[chaveRefSeqNum];

        //        //    string clOrderIdOriginal = cancelamento.ClOrdID;

        //        //    ordemInfoPorClOrdId.Remove(clOrderIdOriginal);

        //        //    clOrdIdMsgSeqNum.Remove(clOrdIdMsgSeqNum);
        //        //}
        //        // verifica se OrdemInfo original está no Hashtable
        //        OrdemCancelamentoInfo ordemCancelamentoInfoOriginal = null;
        //        string chaveClOrdId = message.isSetClOrdID() ? message.getClOrdID().getValue() : null;
        //        if (cancelInfoPorClOrdId.ContainsKey(chaveClOrdId))
        //        {
        //            ordemCancelamentoInfoOriginal = (OrdemCancelamentoInfo)cancelInfoPorClOrdId[chaveClOrdId];
        //            logger.Debug("onMessage(OrderCancelReject): Recuperada OrdemCancelamentoInfo de cancelInfoPorClOrdId");
        //        }
        //        else
        //        {
        //            if (ordemInfoPorClOrdId.ContainsKey(chaveClOrdId))
        //            {
        //                OrdemInfo oiorig = (OrdemInfo)ordemInfoPorClOrdId[chaveClOrdId];

        //                ordemCancelamentoInfoOriginal = new OrdemCancelamentoInfo();

        //                ordemCancelamentoInfoOriginal.Account = oiorig.Account;
        //                ordemCancelamentoInfoOriginal.ChannelID = oiorig.ChannelID;
        //                ordemCancelamentoInfoOriginal.ClOrdID = chaveClOrdId;
        //                ordemCancelamentoInfoOriginal.Exchange = oiorig.Exchange;
        //                ordemCancelamentoInfoOriginal.OrderQty = oiorig.OrderQty;
        //                ordemCancelamentoInfoOriginal.OrigClOrdID = oiorig.OrigClOrdID;
        //                ordemCancelamentoInfoOriginal.SecurityID = oiorig.SecurityID;
        //                ordemCancelamentoInfoOriginal.Side = oiorig.Side;
        //                ordemCancelamentoInfoOriginal.Symbol = oiorig.Symbol;
        //            }
        //            else
        //            {
        //                logger.Error("onMessage(OrderCancelReject): ClOrdId não encontrado em ordemInfoPorClOrdId.");
        //                logger.Error("onMessage(OrderCancelReject): ClOrdId = " + chaveClOrdId);
        //            }
        //        }

        //        if (cancelInfoPorClOrdId.ContainsKey(chaveClOrdId))
        //        {
        //            cancelInfoPorClOrdId.Remove(chaveClOrdId);
        //            logger.Debug("onMessage(OrderCancelReject): Removida OrdemCancelamentoInfo de cancelInfoPorClOrdId");
        //        }

        //        if (ordemInfoPorClOrdId.ContainsKey(chaveClOrdId))
        //        {
        //            ordemInfoPorClOrdId.Remove(chaveClOrdId);
        //            logger.Debug("onMessage(OrderCancelReject): Removida OrdemInfo de ordemInfoPorClOrdId");
        //        }

        //        AcompanhamentoOrdemInfo acompanhamento = new AcompanhamentoOrdemInfo();

        //        acompanhamento.CanalNegociacao = cancelamento.ChannelID;
        //        acompanhamento.CodigoDoCliente = cancelamento.Account;
        //        acompanhamento.CodigoTransacao = null;
        //        acompanhamento.DataAtualizacao = DateTime.Now;

        //        char rejResponseTo = message.getCxlRejResponseTo().getValue();
        //        string sRespTo = null;
        //        if (rejResponseTo == '1')
        //        {
        //            sRespTo = "Order Cancel Request";
        //        }
        //        else if (rejResponseTo == '2')
        //        {
        //            sRespTo = "Order Cancel/Replace Request";
        //        }
        //        else
        //        {
        //            sRespTo = "Tipo de requisição de cancelamento desconhecido: " + rejResponseTo;
        //        }

        //        //acompanhamento.FixMsgSeqNum = Convert.ToString(message.getHeader().getInt(MsgSeqNum.FIELD));
        //        acompanhamento.CodigoRejeicao = message.isSetCxlRejReason() ? message.isSetCxlRejReason().ToString() : "CxlRejReason not set";
        //        acompanhamento.Descricao = "RejResponseTo=[" + sRespTo + "] " +
        //                                   "ClOrdId=[" + message.getClOrdID().ToString() + "] " +
        //                                   "OrigClOrdId=[" + message.getOrigClOrdID().ToString() + "] " +
        //                                   "Error=[" + message.getText().ToString() + "]";

        //        OrdemInfo ordemInfoRejeicao = new OrdemInfo();

        //        ordemInfoRejeicao.Account = cancelamento.Account;
        //        ordemInfoRejeicao.OrigClOrdID = cancelamento.OrigClOrdID;
        //        ordemInfoRejeicao.ClOrdID = cancelamento.OrigClOrdID;
        //        ordemInfoRejeicao.Symbol = cancelamento.Symbol;
        //        ordemInfoRejeicao.SecurityID = cancelamento.SecurityID;
        //        ordemInfoRejeicao.Side = cancelamento.Side;
        //        ordemInfoRejeicao.OrderQty = cancelamento.OrderQty;
        //        ordemInfoRejeicao.ChannelID = cancelamento.ChannelID;
        //        ordemInfoRejeicao.Exchange = cancelamento.Exchange;
        //        ordemInfoRejeicao.OrdType = OrdemTipoEnum.Limitada;
        //        //ordemInfoRejeicao.FixMsgSeqNum = Convert.ToString(message.getHeader().getInt(MsgSeqNum.FIELD));
        //        ordemInfoRejeicao.Acompanhamentos.Add(acompanhamento);

        //        // Envia o report para os assinantes
        //        _sendExecutionReport(ordemInfoRejeicao);

        //        Text text = message.getText();

        //        logger.Error("onMessage(OrderCancelReject): onMessage(Reject) SessionID: " + session.ToString());
        //        logger.Error("onMessage(OrderCancelReject): RejResponseTo=[" + sRespTo + "]");
        //        logger.Error("onMessage(OrderCancelReject): ClOrdId=[" + message.getClOrdID().ToString() + "]");
        //        logger.Error("onMessage(OrderCancelReject): OrigClOrdId=[" + message.getOrigClOrdID().ToString() + "]");
        //        logger.Error("onMessage(OrderCancelReject): Error=[" + text.ToString() + "]");

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("onMessage(OrderCancelReject): " + ex.Message, ex);
        //    }
        //    return;
        //}


        public override void onMessage(OrderCancelReject message, SessionID session)
        {
            try
            {
                OrdemInfo order = null;
                OrdemCancelamentoInfo cancelInfo = null;

                // verifica se OrdemInfo original está no Hashtable
                string chaveClOrdId = message.isSetClOrdID() ? message.getClOrdID().getValue() : null;

                // Se houve tentativa de cancelamento, tenta obter a ordem original
                if (cancelInfoPorClOrdId.ContainsKey(chaveClOrdId))
                {
                    string cancelOrdID = chaveClOrdId;
                    cancelInfo = (OrdemCancelamentoInfo)cancelInfoPorClOrdId[chaveClOrdId];
                    chaveClOrdId = cancelInfo.OrigClOrdID;
                    cancelInfoPorClOrdId.Remove(cancelOrdID);
                    logger.Debug("onMessage(OrderCancelReject): ClOrdId = [" + cancelOrdID + "] eh cancelamento de [" + chaveClOrdId + "]");
                }

                // Em caso de execution report de cancelamento ou modificação, o ClOrdId
                // da mensagem original está no campo OrigClOrdId
                //if (message.isSetOrdStatus())
                //{
                //    char statusExecReport = message.getOrdStatus().getValue();
                //    if (statusExecReport == OrdStatus.CANCELED || statusExecReport == OrdStatus.REPLACED)
                //        chaveClOrdId = message.getOrigClOrdID().ToString();
                //}

                if (!ordemInfoPorClOrdId.ContainsKey(chaveClOrdId))
                {
                    logger.Error("onMessage(OrderCancelReject): ClOrdId não encontrado em ordemInfoPorClOrdId.");
                    logger.Error("onMessage(OrderCancelReject): ClOrdId = " + chaveClOrdId);

                    //ATP 2012-05-24
                    // Decidimos nao criar um ordem info se nao existe mais na tabela.
                    if (ConfigurationManager.AppSettings["GerarInfoFromCancelReject"] == null ||
                        !ConfigurationManager.AppSettings["GerarInfoFromCancelReject"].ToString().ToLower().Equals("true"))
                    {
                        logger.Error("Descartando OrderCancelReject para ClOrdId = " + chaveClOrdId);

                        return;
                    }

                    order = new OrdemInfo();
                    order.Account = cancelInfo.Account;
                    order.ChannelID = cancelInfo.ChannelID;
                    order.ClOrdID = cancelInfo.OrigClOrdID;
                    //order.CompIDBolsa = cancelInfo.CompIDBolsa;
                    order.Exchange = cancelInfo.Exchange;
                    order.ExchangeNumberID = cancelInfo.OrderID;
                    order.ExecBroker = cancelInfo.ExecBroker;
                    order.ExecutingTrader = cancelInfo.ExecutingTrader;
                    order.ForeignFirm = cancelInfo.ForeignFirm;
                    order.OrderQty = cancelInfo.OrderQty;
                    order.Symbol = cancelInfo.Symbol;
                    order.SecurityID = cancelInfo.Symbol;
                    order.SenderLocation = cancelInfo.SenderLocation;
                    order.Side = cancelInfo.Side;
                    order.OrdType = OrdemTipoEnum.Limitada;
                    order.TimeInForce = OrdemValidadeEnum.ValidaParaODia;
                    order.OrdStatus = OrdemStatusEnum.CANCELAMENTOREJEITADO;
                }
                else
                {
                    order = (OrdemInfo)ordemInfoPorClOrdId[chaveClOrdId];
                    logger.Debug("onMessage(OrderCancelReject): Recuperada OrdemInfo de ordemInfoPorClOrdId");
                }


                // Setando informações de acompanhamento
                AcompanhamentoOrdemInfo acompanhamento = new AcompanhamentoOrdemInfo();
                if (order != null)
                {
                    acompanhamento.NumeroControleOrdem = order.ClOrdID;
                    acompanhamento.CodigoDoCliente = order.Account;
                    acompanhamento.CodigoTransacao = null;
                    acompanhamento.Instrumento = order.SecurityID;
                    acompanhamento.CanalNegociacao = order.ChannelID;
                    acompanhamento.Direcao = order.Side;
                    acompanhamento.QuantidadeSolicitada = order.OrderQty;
                    acompanhamento.QuantidadeExecutada = 0;
                }
                else
                {
                    order = new OrdemInfo();

                }

                acompanhamento.StatusOrdem = TradutorFix.TraduzirOrdemStatus(message.getOrdStatus().getValue());
                acompanhamento.DataAtualizacao = DateTime.Now;
                acompanhamento.CodigoRejeicao = message.isSetCxlRejReason() ? message.isSetCxlRejReason().ToString() : "CxlRejReason not set";
                acompanhamento.FixMsgSeqNum = message.getHeader().isSetField(MsgSeqNum.FIELD) ? message.getHeader().getInt(MsgSeqNum.FIELD) : 0;
                acompanhamento.PossDupFlag = message.getHeader().isSetField(PossDupFlag.FIELD) ? message.getHeader().getBoolean(PossDupFlag.FIELD) : false;
                acompanhamento.PossResend = message.getHeader().isSetField(PossResend.FIELD) ? message.getHeader().getBoolean(PossResend.FIELD) : false;
                acompanhamento.CompIDBolsa = message.getHeader().isSetField(SenderCompID.FIELD) ? message.getHeader().getString(SenderCompID.FIELD) : _config.TargetCompID;
                acompanhamento.CompIDOMS = message.getHeader().isSetField(TargetCompID.FIELD) ? message.getHeader().getString(TargetCompID.FIELD) : _config.SenderCompID;

                char rejResponseTo = message.getCxlRejResponseTo().getValue();
                string sRespTo = null;
                if (rejResponseTo == '1')
                {
                    sRespTo = "Order Cancel Request";
                }
                else if (rejResponseTo == '2')
                {
                    sRespTo = "Order Cancel/Replace Request";
                }
                else
                {
                    sRespTo = "Tipo de requisição de cancelamento desconhecido: " + rejResponseTo;
                }
                acompanhamento.Descricao = "RejResponseTo=[" + sRespTo + "] " +
                                           "ClOrdId=[" + message.getClOrdID().ToString() + "] " +
                                           "OrigClOrdId=[" + message.getOrigClOrdID().ToString() + "] " +
                                           "Error=[" + message.getText().ToString() + "]";

                OrdemInfo tmpOrdemInfo = new OrdemInfo();
                tmpOrdemInfo.Account = order.Account;
                tmpOrdemInfo.ChannelID = order.ChannelID;
                tmpOrdemInfo.ClOrdID = message.getClOrdID().ToString();
                tmpOrdemInfo.OrigClOrdID = message.getOrigClOrdID().ToString();
                tmpOrdemInfo.Exchange = order.Exchange;
                tmpOrdemInfo.OrderQty = order.OrderQty;
                tmpOrdemInfo.OrdType = order.OrdType;
                tmpOrdemInfo.Symbol = order.Symbol;
                tmpOrdemInfo.Price = order.Price;
                tmpOrdemInfo.Side = order.Side;
                tmpOrdemInfo.TransactTime = DateTime.Now;
                tmpOrdemInfo.OrdStatus = OrdemStatusEnum.CANCELAMENTOREJEITADO;
                tmpOrdemInfo.ExchangeNumberID = order.ExchangeNumberID;
                tmpOrdemInfo.FixMsgSeqNum = message.getHeader().isSetField(MsgSeqNum.FIELD) ? message.getHeader().getInt(MsgSeqNum.FIELD) : 0;
                tmpOrdemInfo.Memo5149 = message.isSetField(5149) ? message.getString(5149) : String.Empty;
                tmpOrdemInfo.PossDupFlag = message.getHeader().isSetField(PossDupFlag.FIELD) ? message.getHeader().getBoolean(PossDupFlag.FIELD) : false;
                tmpOrdemInfo.PossResend = message.getHeader().isSetField(PossResend.FIELD) ? message.getHeader().getBoolean(PossResend.FIELD) : false;
                tmpOrdemInfo.CompIDBolsa = message.getHeader().isSetField(SenderCompID.FIELD) ? message.getHeader().getString(SenderCompID.FIELD) : _config.TargetCompID;
                tmpOrdemInfo.CompIDOMS = message.getHeader().isSetField(TargetCompID.FIELD) ? message.getHeader().getString(TargetCompID.FIELD) : _config.SenderCompID;


                // Adicionando informações de acompanhamento ao OrdemInfo
                tmpOrdemInfo.Acompanhamentos.Add(acompanhamento);

                // Envia o report para os assinantes
                _sendExecutionReport(tmpOrdemInfo);

                Text text = message.getText();

                logger.Error("onMessage(OrderCancelReject): onMessage(Reject) SessionID: " + session.ToString());
                logger.Error("onMessage(OrderCancelReject): RejResponseTo=[" + sRespTo + "]");
                logger.Error("onMessage(OrderCancelReject): ClOrdId=[" + message.getClOrdID().ToString() + "]");
                logger.Error("onMessage(OrderCancelReject): OrigClOrdId=[" + message.getOrigClOrdID().ToString() + "]");
                logger.Error("onMessage(OrderCancelReject): Error=[" + text.ToString() + "]");

            }
            catch (Exception ex)
            {
                logger.Error("onMessage(OrderCancelReject): " + ex.Message, ex);
            }
            return;
        }

        public override void onMessage(Heartbeat message, SessionID session)
        {
            _sendConnectionStatus();
        }

        public override void toAdmin(QuickFix.Message message, QuickFix.SessionID session)
        {
            // Faz o processamento
            try
            {
                // Complementa a mensagem de logon com a senha 
                if (message.GetType() == typeof(Logon))
                {
                    Logon message2 = (Logon)message;
                    if (_config.LogonPassword != "")
                    {
                        message2.set(new QuickFix.RawData(_config.LogonPassword));
                        message2.set(new QuickFix.RawDataLength(_config.LogonPassword.Length));
                        if (_config.NewPassword != null && _config.NewPassword.Length > 0)
                        {
                            message2.setString(925, _config.NewPassword.Trim());
                        }
                    }


                    if (_config.CancelOnDisconnect != 0)
                    {
                        if (_config.CancelOnDisconnect >= 0 && _config.CancelOnDisconnect <= 3)
                        {
                            message2.setInt(35002, _config.CancelOnDisconnect);
                        }

                        if (_config.CODTimeout >= 0 && _config.CODTimeout <= 60)
                        {
                            message2.setInt(35003, _config.CODTimeout * 1000);
                        }
                    }
                    message2.set(new QuickFix.HeartBtInt(_config.HeartBtInt));
                    message2.set(new QuickFix.EncryptMethod(0));
                    message2.set(new QuickFix.ResetSeqNumFlag(_config.ResetSeqNum));
                }

                logger.Debug("toAdmin(). Session id : " + session.ToString() + " Msg: " + message.GetType().ToString());

                if (message.getHeader().getField(MsgType.FIELD) != QuickFix.MsgType.Heartbeat)
                    this.crack(message, session);
            }
            catch (Exception ex)
            {
                logger.Error("toAdmin() Erro: " + ex.Message, ex);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="session"></param>
        public override void onMessage(NewOrderCross order, SessionID session)
        {
            try
            {
                string crossID = order.getCrossID().getValue();

                if (!this.crossInfoPorCrossID.ContainsKey(crossID))
                {
                    logger.Error("onMessage(NewOrderCross): CrossID não encontrado em crossInfoPorCrossID. CrossID = [" + crossID + "]");
                }
                else
                {
                    OrdemCrossInfo info = (OrdemCrossInfo)crossInfoPorCrossID[crossID];

                    string ClOrdIDCompra = info.OrdemInfoCompra.ClOrdID;
                    string ClOrdIDVenda = info.OrdemInfoVenda.ClOrdID;

                    if (!ordemInfoPorClOrdId.ContainsKey(ClOrdIDCompra))
                    {
                        logger.Error("onMessage(NewOrderCross): ClOrdID não encontrado em ordemInfoPorClOrdID. ClOrdID = [" + ClOrdIDCompra + "]");
                    }
                    else
                    {
                        OrdemInfo ordemInfo = (OrdemInfo)ordemInfoPorClOrdId[ClOrdIDCompra];

                        int msgSeqNum = order.getHeader().getInt(MsgSeqNum.FIELD);

                        ordemInfoPorMsgSeqNum.Add(msgSeqNum, ordemInfo);

                        clOrdIdMsgSeqNum.Add(ClOrdIDCompra, msgSeqNum);

                        logger.Debug("onMessage(NewOrderCross): Inserido OrdemInfo na hash por MsgSeqNum");
                        logger.Debug("onMessage(NewOrderCross): MsgSeqNum = " + msgSeqNum);
                        logger.Debug("onMessage(NewOrderCross): ClOrdID = " + ClOrdIDCompra);
                    }


                    if (!ordemInfoPorClOrdId.ContainsKey(ClOrdIDVenda))
                    {
                        logger.Error("onMessage(NewOrderCross): ClOrdID não encontrado em ordemInfoPorClOrdID. ClOrdID = [" + ClOrdIDVenda + "]");
                    }
                    else
                    {
                        OrdemInfo ordemInfo = (OrdemInfo)ordemInfoPorClOrdId[ClOrdIDVenda];

                        int msgSeqNum = order.getHeader().getInt(MsgSeqNum.FIELD);

                        ordemInfoPorMsgSeqNum.Add(msgSeqNum, ordemInfo);

                        clOrdIdMsgSeqNum.Add(ClOrdIDVenda, msgSeqNum);

                        logger.Debug("onMessage(NewOrderCross): Inserido OrdemInfo na hash por MsgSeqNum");
                        logger.Debug("onMessage(NewOrderCross): MsgSeqNum = " + msgSeqNum);
                        logger.Debug("onMessage(NewOrderCross): ClOrdID = " + ClOrdIDVenda);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(NewOrderCross): " + ex.Message, ex);
            }
        }

        #endregion // Quickfix Application Members
        
        #region RoteadorOrdensAdmin

        public FixTestResponse ExecutarFixTest(RoteadorOrdensAdm.Lib.Mensagens.FixTestRequest request)
        {
            throw new NotImplementedException();
        }

        public FixResendResponse ExecutarFixResend(RoteadorOrdensAdm.Lib.Mensagens.FixResendRequest request)
        {
            FixResendResponse response = new FixResendResponse();

            StatusRoteamentoEnum status;
            string msg;

            try
            {

                ResendRequest rrequest = new ResendRequest();

                rrequest.set(new BeginSeqNo(request.BeginSeqNo));
                rrequest.set(new EndSeqNo(request.EndSeqNo));

                if (finalizarSinalizado)
                {
                    msg = "Ordem rejeitada. CanalNegociacaoBMF em processo de finalização";
                    status = StatusRoteamentoEnum.Erro;
                }
                else
                {
                    msg = "Ordem enviada com sucesso";
                    status = StatusRoteamentoEnum.Sucesso;

                    bool bRet = Session.sendToTarget(rrequest, _session);
                    if (bRet == false)
                    {
                        msg = "Não foi possivel enviar a ordem";
                        status = StatusRoteamentoEnum.Erro;
                    }
                }

                response.DadosRetorno = new DadosAdmRetornoBase();
                response.DadosRetorno.DataResposta = DateTime.Now;
                response.DadosRetorno.Erro = true;
                response.DadosRetorno.Ocorrencias.Add(msg);
            }
            catch (Exception ex)
            {
                logger.Error("Error ExecutarFixResend():" + ex.Message, ex);

                msg = "Error ExecutarFixResend(): " + ex.Message;
                status = StatusRoteamentoEnum.Erro;
                response.DadosRetorno = new DadosAdmRetornoBase();
                response.DadosRetorno.DataResposta = DateTime.Now;
                response.DadosRetorno.Erro = true;
                response.DadosRetorno.Ocorrencias.Add(msg);
            }

            return response;
        }

        public FixSequenceResetResponse ExecutarFixSequenceReset(RoteadorOrdensAdm.Lib.Mensagens.FixSequenceResetRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion // RoteadorOrdensAdmin

    }
}
