using System;
using System.Collections;
using System.ServiceModel;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using QuickFix;
using QuickFix42;
using System.Globalization;

namespace Gradual.OMS.ServicoRoteador
{
    /// <summary>
    /// Implementa um canal de comunicacao fix para a Bovespa
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CanalNegociacaoBovespa : CanalNegociacaoBase, IRoteadorOrdens
    {
        private Hashtable ordemInfoPorClOrdId = new Hashtable();
        private Hashtable ordemInfoPorMsgSeqNum = new Hashtable();
        private Hashtable clOrdIdMsgSeqNum = new Hashtable();
        private Hashtable crossInfoPorCrossID = new Hashtable();
        private Hashtable cancelInfoPorClOrdId = new Hashtable();

        #region IRoteadorOrdens Members

        /// <summary>
        /// Envia um pedido de execucao de ordem para o canal correspondente
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecutarOrdemResponse ExecutarOrdem(ExecutarOrdemRequest request)
        {

            ExecutarOrdemResponse response = new ExecutarOrdemResponse();

            // Se o logon não foi feito, rejeita imediatamente a ordem
            if (!_bConectadoBolsa)
            {
                response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaEO(
                    "Ordem rejeitada. Logon não foi realizado no CanalNegociacaoBovespa",
                    StatusRoteamentoEnum.Erro);

                return response;
            }

            OrdemInfo ordem = request.info;

            //Cria a mensagem FIX de NewOrderSingle
            QuickFix42.NewOrderSingle ordersingle = new NewOrderSingle();

            ordersingle.set(new Account(ordem.Account.ToString()));
            ordersingle.set(new Symbol(ordem.Symbol));
            ordersingle.set(new ClOrdID(ordem.ClOrdID));

            // Armazena ClOrdID em Memo (5149) para posterior referência nos tratamentos de retornos
            ordersingle.setField(5149, ordem.ClOrdID);

            //ordersingle.set(new IDSource());
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
                case OrdemTipoEnum.StopLimitada:
                    ordersingle.set(new Price(ordem.Price));
                    break;
                case OrdemTipoEnum.StopStart:
                    ordersingle.set(new Price(ordem.Price));
                    ordersingle.set(new StopPx(ordem.StopPrice));
                    break;
                case OrdemTipoEnum.Mercado:
                case OrdemTipoEnum.OnClose:
                    ordersingle.set(new Price(ordem.Price));
                    break;
                default:
                    ordersingle.set(new Price(ordem.Price));
                    break;
            }

            ordersingle.set(new TransactTime(DateTime.Now));
            ordersingle.set(new HandlInst('1'));
            ordersingle.set(new ExecBroker(_config.PartyID));

            if (ordem.MaxFloor > 0)
                ordersingle.set(new MaxFloor(Convert.ToDouble(ordem.MaxFloor)));

            if (ordem.MinQty > 0)
                ordersingle.set(new MinQty(Convert.ToDouble(ordem.MinQty)));

            QuickFix42.NewOrderSingle.NoAllocs noAllocsGrp = new QuickFix42.NewOrderSingle.NoAllocs();
            noAllocsGrp.set(new AllocAccount("67"));
            ordersingle.addGroup(noAllocsGrp);

            // armazena para posterior uso em caso de rejeição
            if (ordemInfoPorClOrdId.ContainsKey(ordem.ClOrdID))
                ordemInfoPorClOrdId[ordem.ClOrdID] = ordem;
            else
                ordemInfoPorClOrdId.Add(ordem.ClOrdID, ordem);

            OrdemInfo clone = RoteadorOrdensUtil.CloneOrder(ordem);

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

            // extrai objeto info de cancelamento do request
            OrdemCancelamentoInfo orderCancelInfo = request.info;

            //Cria a mensagem FIX de OrderCancelRequest
            QuickFix42.OrderCancelRequest orderCancel = new OrderCancelRequest();

            orderCancel.set(new Account(orderCancelInfo.Account.ToString()));
            orderCancel.set(new OrigClOrdID(orderCancelInfo.OrigClOrdID));
            orderCancel.set(new ClOrdID(orderCancelInfo.ClOrdID));
            orderCancel.set(new Symbol(orderCancelInfo.Symbol));
            if (orderCancelInfo.Side == OrdemDirecaoEnum.Venda)
                orderCancel.set(new Side(Side.SELL));
            else
                orderCancel.set(new Side(Side.BUY));
            orderCancel.set(new OrderQty(orderCancelInfo.OrderQty));
            orderCancel.set(new TransactTime(DateTime.Now));

            // armazena para posterior uso em caso de rejeição
            cancelInfoPorClOrdId.Add(orderCancelInfo.ClOrdID, orderCancelInfo);

            string msg = null;
            StatusRoteamentoEnum status = StatusRoteamentoEnum.Erro;
            if (finalizarSinalizado)
            {
                msg = "Cancelamento de Ordem rejeitado. CanalNegociacaoBovespa em processo de finalização";
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
            ExecutarModificacaoOrdensResponse response = new ExecutarModificacaoOrdensResponse();

            // Se o logon não foi feito, rejeita imediatamente a ordem
            if (!_bConectadoBolsa)
            {
                response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaModificacao(
                    "Modificação de Ordem rejeitada. Logon não foi realizado no CanalNegociacaoBovespa",
                    StatusRoteamentoEnum.Erro);

                return response;
            }

            OrdemInfo ordem = request.info;

            //Cria a mensagem FIX de NewOrderSingle
            QuickFix42.OrderCancelReplaceRequest orderCancelReplaceReq = new OrderCancelReplaceRequest();

            orderCancelReplaceReq.set(new OrigClOrdID(ordem.OrigClOrdID));

            orderCancelReplaceReq.set(new Account(ordem.Account.ToString()));
            orderCancelReplaceReq.set(new Symbol(ordem.Symbol));
            orderCancelReplaceReq.set(new ClOrdID(ordem.ClOrdID));
            //ordersingle.set(new IDSource());
            if (ordem.Side == OrdemDirecaoEnum.Venda)
                orderCancelReplaceReq.set(new Side(Side.SELL));
            else
                orderCancelReplaceReq.set(new Side(Side.BUY));

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

            // Verifica envio do preco
            switch (ordem.OrdType)
            {

                case OrdemTipoEnum.Limitada:
                case OrdemTipoEnum.MarketWithLeftOverLimit:
                case OrdemTipoEnum.StopLimitada:
                    orderCancelReplaceReq.set(new Price(ordem.Price));
                    break;
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

            orderCancelReplaceReq.set(new TransactTime(DateTime.Now));
            orderCancelReplaceReq.set(new HandlInst('1'));
            orderCancelReplaceReq.set(new ExecBroker(_config.PartyID));

            if (ordem.MaxFloor > 0)
                orderCancelReplaceReq.set(new MaxFloor(Convert.ToDouble(ordem.MaxFloor)));

            if (ordem.MinQty > 0)
                orderCancelReplaceReq.set(new MinQty(Convert.ToDouble(ordem.MinQty)));


            QuickFix42.NewOrderSingle.NoAllocs noAllocsGrp = new QuickFix42.NewOrderSingle.NoAllocs();
            noAllocsGrp.set(new AllocAccount("67"));

            orderCancelReplaceReq.addGroup(noAllocsGrp);

            // armazena para posterior uso em caso de rejeição
            if (ordemInfoPorClOrdId.ContainsKey(ordem.ClOrdID))
                ordemInfoPorClOrdId[ordem.ClOrdID] = ordem;
            else
                ordemInfoPorClOrdId.Add(ordem.ClOrdID, ordem);

            OrdemInfo clone = RoteadorOrdensUtil.CloneOrder(ordem);

            string msg = null;
            StatusRoteamentoEnum status = StatusRoteamentoEnum.Erro;
            if (finalizarSinalizado)
            {
                msg = "Modificação de Ordem rejeitada. CanalNegociacaoBovespa em processo de finalização";
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
            QuickFix42.NewOrderCross ordercross = new QuickFix42.NewOrderCross();

            ordercross.set(new CrossID(crossinfo.CrossID));
            // Unico valor aceito 1 - Negocio executado total ou parcial
            ordercross.set(new CrossType(1));
            // Prioridade de uma das pontas. Unico valor aceito: 0 - nenhuma
            ordercross.set(new CrossPrioritization(0));

            // Numero de pernas. Sempre 2
            QuickFix42.NewOrderCross.NoSides sideGroupC = new QuickFix42.NewOrderCross.NoSides();

            // Insere informacoes da perna de compra
            sideGroupC.set(new Side(Side.BUY));
            sideGroupC.set(new ClOrdID(ordemCompra.ClOrdID));
            sideGroupC.set(new Account(ordemCompra.Account.ToString()));
            sideGroupC.set(new OrderQty(ordemCompra.OrderQty));

            QuickFix42.NewOrderCross.NoSides sideGroupV = new QuickFix42.NewOrderCross.NoSides();

            // Insere informacoes da perna de venda
            sideGroupV.set(new Side(Side.SELL));
            sideGroupV.set(new ClOrdID(ordemVenda.ClOrdID));
            sideGroupV.set(new Account(ordemVenda.Account.ToString()));
            sideGroupV.set(new OrderQty(ordemVenda.OrderQty));

            ordercross.addGroup(sideGroupC);
            ordercross.addGroup(sideGroupV);

            ordercross.set(new Symbol(crossinfo.Symbol));
            OrdType ordType = TradutorFix.deOrdemTipoParaOrdType(crossinfo.OrdType);
            if (ordType != null)
                ordercross.set(ordType);

            ordercross.set(new Price(crossinfo.Price));
            ordercross.set(new TransactTime(DateTime.Now));
            
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

            // Idem, mas guarda a ordem cross
            if (this.crossInfoPorCrossID.ContainsKey(crossinfo.CrossID))
                crossInfoPorCrossID[crossinfo.CrossID] = crossinfo;
            else
                crossInfoPorCrossID.Add(crossinfo.CrossID, crossinfo);

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
                    }

                    if (_config.CancelOnDisconnect != 0 )
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
        /// Trata mensagem de relatorio de execucao de ordem (acatamento,negocio, modificacao)
        /// </summary>
        /// <param name="message">QuickFix42.ExecutionReport message</param>
        /// <param name="session">QuickFix.SessionID session</param>
        public override void onMessage(ExecutionReport message, QuickFix.SessionID session)
        {
            try
            {
                OrdemInfo order = null;
                string descricao=" ";

                // verifica se OrdemInfo original está no Hashtable
                string chaveClOrdId = message.isSetClOrdID() ? message.getClOrdID().getValue() : null;

                // Se houve tentativa de cancelamento, tenta obter a ordem original
                if (cancelInfoPorClOrdId.ContainsKey(chaveClOrdId))
                {
                    string cancelOrdID = chaveClOrdId;
                    OrdemCancelamentoInfo cancelInfo = (OrdemCancelamentoInfo) cancelInfoPorClOrdId[chaveClOrdId];
                    chaveClOrdId = cancelInfo.OrigClOrdID;
                    cancelInfoPorClOrdId.Remove(cancelOrdID);
                    logger.Debug("onMessage(ExecutionReport): ClOrdId = [" + cancelOrdID + "] eh cancelamento de [" + chaveClOrdId + "]");
                }

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
                if (order != null && status != OrdStatus.NEW && status != OrdStatus.PARTIALLY_FILLED)
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
                    logger.Debug("onMessage(ExecutionReport): Remontando OrdemInfo a partir do ExecutionReport");

                    order = new OrdemInfo();

                    order.Account = Convert.ToInt32(message.isSetAccount() ? message.getAccount().getValue() : "0");
                    order.Exchange = _config.Bolsa;
                    order.ChannelID = _config.Operador;
                    
            
                    // Rafael Sanches Garcia - 28/04/2011
                    // Alteração na obtenção do CLOrdID para ober sempre o código enviado pelo roteador de ordens.
                    order.ClOrdID = chaveClOrdId;
                    order.OrigClOrdID = message.isSetOrigClOrdID() ? message.getOrigClOrdID().getValue() : null;
                    order.ExecBroker = _config.PartyID;
                    order.ExpireDate = message.isSetExpireDate() ? DateTime.ParseExact(message.getExpireDate().getValue(), "yyyyMMdd", new CultureInfo("pt-BR")) : DateTime.MinValue;
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
                order.Memo5149 = message.isSetField(5149) ? message.getString(5149) : String.Empty;
                switch (order.OrdStatus )
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
                    case OrdemStatusEnum.EXECUTADA:
                        descricao = "Ordem executada";
                        break;
                    case OrdemStatusEnum.SUSPENSA:
                        descricao = "Ordem suspensa";
                        break;
                    case OrdemStatusEnum.SUBSTITUIDA:
                        descricao = "Ordem modificada";
                        break;
                }
                order.TransactTime = DateTime.Now;
                //order.FixMsgSeqNum = Convert.ToString(message.getHeader().getInt(MsgSeqNum.FIELD));

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
                acompanhamento.QuantidadeExecutada = (int)message.getCumQty().getValue();
                acompanhamento.QuantidadeRemanescente = (int)message.getLeavesQty().getValue();
                acompanhamento.QuantidadeNegociada = message.isSetLastShares() ? (int)message.getLastShares().getValue() : 0;
                acompanhamento.Preco = new Decimal(order.Price);
                acompanhamento.StatusOrdem = TradutorFix.TraduzirOrdemStatus(message.getOrdStatus().getValue());
                acompanhamento.DataOrdemEnvio = order.TransactTime;
                acompanhamento.DataAtualizacao = DateTime.Now;
                acompanhamento.CodigoRejeicao = message.isSetOrdRejReason() ? message.getOrdRejReason().ToString() : "0";
                acompanhamento.Descricao = message.isSetText() ? message.getText().ToString() : descricao;
                //acompanhamento.FixMsgSeqNum = Convert.ToString(message.getHeader().getInt(MsgSeqNum.FIELD));
                acompanhamento.LastPx = message.isSetLastPx() ? (Decimal)message.getLastPx().getValue() : new Decimal(order.Price);
                acompanhamento.TradeDate = message.isSetTradeDate() ? message.getTradeDate().getValue() : DateTime.Now.ToString("yyyyMMdd");
                if (message.isSetField(6032))
                {
                    acompanhamento.UniqueTradeID = message.getField(6032);
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
        /// <param name="message">QuickFix42.Reject message</param>
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
                AcompanhamentoOrdemInfo acompanhamento = new AcompanhamentoOrdemInfo();

                if (ordemInfoOriginal != null)
                {
                    ordemInfoOriginal.OrdStatus = OrdemStatusEnum.REJEITADA;

                    // Setando informações de acompanhamento
                    acompanhamento.NumeroControleOrdem = ordemInfoOriginal.ClOrdID;
                    acompanhamento.CodigoDoCliente = ordemInfoOriginal.Account;
                    acompanhamento.CodigoResposta = ordemInfoOriginal.ExchangeNumberID;
                    acompanhamento.CodigoTransacao = null;
                    acompanhamento.Instrumento = ordemInfoOriginal.SecurityID;
                    acompanhamento.CanalNegociacao = ordemInfoOriginal.ChannelID;
                    acompanhamento.Direcao = ordemInfoOriginal.Side;
                    acompanhamento.QuantidadeSolicitada = ordemInfoOriginal.OrderQty;
                    acompanhamento.QuantidadeExecutada = 0;
                    acompanhamento.Preco = new Decimal(ordemInfoOriginal.Price);
                    acompanhamento.StatusOrdem = OrdemStatusEnum.REJEITADA;
                    acompanhamento.DataOrdemEnvio = ordemInfoOriginal.TransactTime;
                    acompanhamento.DataAtualizacao = DateTime.Now;
                }
                else
                    ordemInfoOriginal = new OrdemInfo();

                string sessionRejectReason = message.isSetSessionRejectReason() ? message.getSessionRejectReason().ToString() : "0";
                string refSeqNum = message.isSetRefSeqNum() ? message.getRefSeqNum().ToString() : "no-ref-seq-num";
                string refTagID = message.isSetRefTagID() ? message.getRefTagID().ToString() : "no-tag-id";
                string refMsgType = message.isSetRefMsgType() ? message.getRefMsgType().ToString() : "no-ref-msg-type";
                string text = message.isSetText() ? message.getText().ToString() : "no-text";

                //acompanhamento.FixMsgSeqNum = Convert.ToString(message.getHeader().getInt(MsgSeqNum.FIELD));
                acompanhamento.CodigoRejeicao = sessionRejectReason;
                acompanhamento.Descricao = "RefSeqNum=[" + refSeqNum + "] " +
                                            "RefTagID=[" + refTagID + "] " +
                                            "RefMsgType=[" + refMsgType + "]" +
                                            "Error=[" + text + "]";

                // Adicionando informações de acompanhamento ao OrdemInfo
                ordemInfoOriginal.Acompanhamentos.Clear();
                ordemInfoOriginal.Acompanhamentos.Add(acompanhamento);
                ordemInfoOriginal.TransactTime = DateTime.Now;
                //ordemInfoOriginal.FixMsgSeqNum = Convert.ToString(message.getHeader().getInt(MsgSeqNum.FIELD));

                // Envia o report para os assinantes
                _sendExecutionReport(ordemInfoOriginal);

                logger.Error("onMessage(Reject): onMessage(Reject) SessionID: " + session.ToString());
                logger.Error("onMessage(Reject): Reason =[" + sessionRejectReason + "]");
                logger.Error("onMessage(Reject): RefSeqNum=[" + refSeqNum + "]");
                logger.Error("onMessage(Reject): RefTagID=[" + refTagID + "]");
                logger.Error("onMessage(Reject): RefMsgType=[" + refMsgType + "]");
                logger.Error("onMessage(Reject): Error=[" + text + "]");
            }
            catch (QuickFix.UnsupportedMessageType uex)
            {
                logger.Error("onMessage(Reject): " + uex.Message + "\r\n Data: " + uex.Data, uex);
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(Reject): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Trata mensagem BusinessReject
        /// </summary>
        /// <param name="message">QuickFix42.BusinessMessageReject message</param>
        /// <param name="session">QuickFix.SessionID session</param>
        public override void onMessage(BusinessMessageReject message, SessionID session)
        {
            try
            {
                // Recupera OrdemInfo que foi rejeitada
                OrdemInfo ordemInfoOriginal = null;
                int chaveRefSeqNum = message.getRefSeqNum().getValue();
                if (!ordemInfoPorMsgSeqNum.ContainsKey(chaveRefSeqNum))
                {
                    logger.Error("onMessage(BusinessMessageReject): MsgSeqNum não encontrado em ordemInfoPorClOrdId.");
                    logger.Error("onMessage(BusinessMessageReject): MsgSeqNum = " + chaveRefSeqNum);
                }
                else
                {
                    ordemInfoOriginal = (OrdemInfo)ordemInfoPorMsgSeqNum[chaveRefSeqNum];
                }

                // remove OrdemInfo correspondentes das hashtables
                ordemInfoPorMsgSeqNum.Remove(chaveRefSeqNum);
                logger.Debug("onMessage(BusinessMessageReject): Removida OrdemInfo de ordemInfoPorMsgSeqNum");

                string clOrderIdOriginal = ordemInfoOriginal.ClOrdID;
                if (!ordemInfoPorClOrdId.ContainsKey(clOrderIdOriginal))
                {
                    logger.Error("onMessage(BusinessMessageReject): ClOrdID não encontrado em ordemInfoPorMsgSeqNum.");
                    logger.Error("onMessage(BusinessMessageReject): ClOrdID = " + clOrderIdOriginal);
                }
                else
                {
                    ordemInfoPorClOrdId.Remove(clOrderIdOriginal);
                    logger.Debug("onMessage(BusinessMessageReject): Removida OrdemInfo de ordemInfoPorClOrdId");
                }

                if (!clOrdIdMsgSeqNum.ContainsKey(clOrderIdOriginal))
                {
                    logger.Error("onMessage(BusinessMessageReject): ClOrdID não encontrado em clOrdIdMsgSeqNum.");
                    logger.Error("onMessage(BusinessMessageReject): ClOrdID = " + clOrderIdOriginal);
                }
                else
                {
                    clOrdIdMsgSeqNum.Remove(clOrderIdOriginal);
                    logger.Debug("onMessage(BusinessMessageReject): Removida OrdemInfo de clOrdIdMsgSeqNum");
                }

                // Setando informações de acompanhamento
                AcompanhamentoOrdemInfo acompanhamento = new AcompanhamentoOrdemInfo();

                if (ordemInfoOriginal != null)
                {
                    acompanhamento.NumeroControleOrdem = ordemInfoOriginal.ClOrdID;
                    acompanhamento.CodigoDoCliente = ordemInfoOriginal.Account;
                    acompanhamento.CodigoResposta = ordemInfoOriginal.ExchangeNumberID;
                    acompanhamento.CodigoTransacao = null;
                    acompanhamento.Instrumento = ordemInfoOriginal.SecurityID;
                    acompanhamento.CanalNegociacao = ordemInfoOriginal.ChannelID;
                    acompanhamento.Direcao = ordemInfoOriginal.Side;
                    acompanhamento.QuantidadeSolicitada = ordemInfoOriginal.OrderQty;
                    acompanhamento.QuantidadeExecutada = 0;
                    acompanhamento.Preco = new Decimal(ordemInfoOriginal.Price);
                    acompanhamento.StatusOrdem = OrdemStatusEnum.REJEITADA;
                    acompanhamento.DataOrdemEnvio = ordemInfoOriginal.TransactTime;
                }
                else
                    ordemInfoOriginal = new OrdemInfo();

                acompanhamento.DataAtualizacao = DateTime.Now;
                //acompanhamento.FixMsgSeqNum = Convert.ToString(message.getHeader().getInt(MsgSeqNum.FIELD));
                acompanhamento.CodigoRejeicao = message.isSetBusinessRejectReason() ? message.getBusinessRejectReason().ToString() : "0";
                acompanhamento.Descricao =  "RefSeqNum=[" + message.getRefSeqNum().ToString() + "] " +
                                            "RefMsgType=[" + message.getRefMsgType().ToString() + "] " +
                                            "BusinessRejectRefID=[" + message.getBusinessRejectRefID().ToString() + "] " +
                                            "Error=[" + message.getText().ToString() + "]";

                // Adicionando informações de acompanhamento ao OrdemInfo
                //ordemInfoOriginal.FixMsgSeqNum = Convert.ToString(message.getHeader().getInt(MsgSeqNum.FIELD));
                ordemInfoOriginal.Acompanhamentos.Add(acompanhamento);

                // Envia o report para os assinantes
                _sendExecutionReport(ordemInfoOriginal);

                Text text = message.getText();
                int msgSeqNum = Convert.ToInt32(message.getRefSeqNum().ToString());

                logger.Error("onMessage(BusinessMessageReject): onMessage(Reject) SessionID: " + session.ToString());
                logger.Error("onMessage(BusinessMessageReject): RefSeqNum=[" + message.getRefSeqNum().ToString() + "]");
                logger.Error("onMessage(BusinessMessageReject): RefMsgType=[" + message.getRefMsgType().ToString() + "]");
                logger.Error("onMessage(BusinessMessageReject): BusinessRejectRefID=[" + message.getBusinessRejectRefID().ToString() + "]");
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
                        ordemInfoPorMsgSeqNum[msgSeqNum]=ordemInfo;
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

        public override void onMessage(OrderCancelReject message, SessionID session)
        {
            try
            {
                double origPrice = 0.0;
                int origRemainingQty = 0;
                int origCumQty = 0;
                int origQty = 0;
                DateTime origExpir = DateTime.MinValue;

                // verifica se OrdemInfo original está no Hashtable
                OrdemCancelamentoInfo ordemCancelamentoInfoOriginal = null;
                string chaveClOrdId = message.isSetClOrdID() ? message.getClOrdID().getValue() : null;
                if ( cancelInfoPorClOrdId.ContainsKey(chaveClOrdId))
                {
                    ordemCancelamentoInfoOriginal = (OrdemCancelamentoInfo) cancelInfoPorClOrdId[chaveClOrdId];
                    logger.Debug("onMessage(OrderCancelReject): Recuperada OrdemCancelamentoInfo de cancelInfoPorClOrdId");
                }
                else
                {
                    if ( ordemInfoPorClOrdId.ContainsKey(chaveClOrdId))
                    {
                        OrdemInfo oiorig = (OrdemInfo) ordemInfoPorClOrdId[chaveClOrdId];

                        ordemCancelamentoInfoOriginal = new OrdemCancelamentoInfo();

                        ordemCancelamentoInfoOriginal.Account = oiorig.Account;
                        ordemCancelamentoInfoOriginal.ChannelID = oiorig.ChannelID;
                        ordemCancelamentoInfoOriginal.ClOrdID = chaveClOrdId;
                        ordemCancelamentoInfoOriginal.Exchange = oiorig.Exchange;
                        ordemCancelamentoInfoOriginal.OrderQty = oiorig.OrderQty;
                        ordemCancelamentoInfoOriginal.OrigClOrdID = oiorig.OrigClOrdID;
                        ordemCancelamentoInfoOriginal.SecurityID = oiorig.SecurityID;
                        ordemCancelamentoInfoOriginal.Side = oiorig.Side;
                        ordemCancelamentoInfoOriginal.Symbol = oiorig.Symbol;
                        origPrice = oiorig.Price;
                        origCumQty = oiorig.CumQty;
                        origQty = oiorig.OrderQty;
                        origRemainingQty = oiorig.OrderQtyRemmaining;
                        if (oiorig.ExpireDate.HasValue )
                            origExpir = oiorig.ExpireDate.Value;
                    }
                    else
                    {
                        logger.Error("onMessage(OrderCancelReject): ClOrdId não encontrado em ordemInfoPorClOrdId.");
                        logger.Error("onMessage(OrderCancelReject): ClOrdId = " + chaveClOrdId);
                    }
                }

                if (cancelInfoPorClOrdId.ContainsKey(chaveClOrdId))
                {
                    cancelInfoPorClOrdId.Remove(chaveClOrdId);
                    logger.Debug("onMessage(OrderCancelReject): Removida OrdemCancelamentoInfo de cancelInfoPorClOrdId");
                }

                if (ordemInfoPorClOrdId.ContainsKey(chaveClOrdId))
                {
                    ordemInfoPorClOrdId.Remove(chaveClOrdId);
                    logger.Debug("onMessage(OrderCancelReject): Removida OrdemInfo de ordemInfoPorClOrdId");
                }

                if (!clOrdIdMsgSeqNum.ContainsKey(chaveClOrdId))
                {
                    logger.Error("onMessage(OrderCancelReject): ClOrdId não encontrado em clOrdIdMsgSeqNum.");
                    logger.Error("onMessage(OrderCancelReject): ClOrdId = " + chaveClOrdId);
                }
                else
                {
                    int chaveMsgSeqNum = (int)clOrdIdMsgSeqNum[chaveClOrdId];
                    if (!ordemInfoPorMsgSeqNum.ContainsKey(chaveMsgSeqNum))
                    {
                        logger.Error("onMessage(OrderCancelReject): MsgSeqNum não encontrado em clOrdIdMsgSeqNum.");
                        logger.Error("onMessage(OrderCancelReject): MsgSeqNum = " + chaveMsgSeqNum);
                    }
                    else
                    {
                        ordemInfoPorMsgSeqNum.Remove(chaveMsgSeqNum);
                        logger.Debug("onMessage(OrderCancelReject): Removida OrdemInfo de ordemInfoPorMsgSeqNum");

                        clOrdIdMsgSeqNum.Remove(chaveClOrdId);
                        logger.Debug("onMessage(OrderCancelReject): Removida OrdemInfo de clOrdIdMsgSeqNum");

                    }
                }

                // Setando informações de acompanhamento
                AcompanhamentoOrdemInfo acompanhamento = new AcompanhamentoOrdemInfo();
                if (ordemCancelamentoInfoOriginal != null)
                {
                    acompanhamento.NumeroControleOrdem = ordemCancelamentoInfoOriginal.ClOrdID;
                    acompanhamento.CodigoDoCliente = ordemCancelamentoInfoOriginal.Account;
                    acompanhamento.CodigoTransacao = null;
                    acompanhamento.Instrumento = ordemCancelamentoInfoOriginal.SecurityID;
                    acompanhamento.CanalNegociacao = ordemCancelamentoInfoOriginal.ChannelID;
                    acompanhamento.Direcao = ordemCancelamentoInfoOriginal.Side;
                    acompanhamento.QuantidadeSolicitada = origQty;
                    acompanhamento.QuantidadeExecutada = origCumQty;
                    acompanhamento.QuantidadeRemanescente = origRemainingQty;
                    acompanhamento.QuantidadeNegociada = 0;

                }
                else
                {
                    ordemCancelamentoInfoOriginal = new OrdemCancelamentoInfo();
                }

                acompanhamento.StatusOrdem = TradutorFix.TraduzirOrdemStatus(message.getOrdStatus().getValue());
                acompanhamento.DataAtualizacao = DateTime.Now;
                acompanhamento.CodigoRejeicao = message.isSetCxlRejReason() ? message.isSetCxlRejReason().ToString() : "CxlRejReason not set";

                //acompanhamento.FixMsgSeqNum = Convert.ToString(message.getHeader().getInt(MsgSeqNum.FIELD));

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
                tmpOrdemInfo.Account = ordemCancelamentoInfoOriginal.Account;
                tmpOrdemInfo.ChannelID = ordemCancelamentoInfoOriginal.ChannelID;
                tmpOrdemInfo.ClOrdID = message.getOrigClOrdID().ToString();
                tmpOrdemInfo.Exchange = ordemCancelamentoInfoOriginal.Exchange;
                tmpOrdemInfo.OrderQty = ordemCancelamentoInfoOriginal.OrderQty;
                tmpOrdemInfo.Symbol = ordemCancelamentoInfoOriginal.Symbol;
                tmpOrdemInfo.OrdType = OrdemTipoEnum.Limitada;
                tmpOrdemInfo.TransactTime = DateTime.Now;
                tmpOrdemInfo.OrdStatus = TradutorFix.TraduzirOrdemStatus(message.getOrdStatus().getValue());
                tmpOrdemInfo.OrderQtyRemmaining = origRemainingQty;
                tmpOrdemInfo.Price = origPrice;
                tmpOrdemInfo.CumQty = origCumQty;
                //tmpOrdemInfo.FixMsgSeqNum = Convert.ToString(message.getHeader().getInt(MsgSeqNum.FIELD));
                tmpOrdemInfo.Memo5149 = message.isSetField(5149) ? message.getString(5149) : String.Empty;


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
                    OrdemCrossInfo info = (OrdemCrossInfo) crossInfoPorCrossID[crossID];

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
    }
}
