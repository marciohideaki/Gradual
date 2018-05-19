using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;
using QuickFix.FIX44;

using Gradual.Core.OMS.DropCopyProcessor.Database;
using Gradual.Core.OMS.DropCopy.Lib.Dados;
using QuickFix;
using QuickFix.Fields;
using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;
using Gradual.Core.OMS.DropCopy.Lib.Util;
using System.Globalization;

namespace Gradual.Core.OMS.DropCopyProcessor.MessageRules
{
    public class MessageProcessor
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables
        DbDropCopy _db;
        #endregion

        public MessageProcessor()
        {
            _db = new DbDropCopy();
        }


        #region Processing Msgs
        public void ProcessNewOrderSingle(QuickFix.Message msg, string bolsa, int operador)
        {
            try
            {
                NewOrderSingle nos = (NewOrderSingle) msg;
                // OBS1: Campos nao atribuidos, nao vem na mensagem;
                // OBS2: Esta seguindo a ordem dos campos da tabela
                OrderDbInfo order = new OrderDbInfo();
                order.ClOrdID = nos.ClOrdID.getValue();
                if (bolsa.Equals(ExchangePrefixes.BOVESPA, StringComparison.InvariantCultureIgnoreCase))
                {
                    string acc = nos.Account.getValue();
                    order.Account = Convert.ToInt32(acc.Remove(acc.Length-1));
                }
                else
                    order.Account = Convert.ToInt32(nos.Account.getValue());
                order.Symbol = nos.Symbol.getValue();
                order.SecurityExchangeID = nos.IsSetField(Tags.SecurityID)? nos.SecurityID.getValue(): string.Empty; //Para BMF
                order.OrdTypeID = Conversions.Parse2FixOrderType(nos.OrdType.getValue());
                order.OrdStatus = (int)FixOrderStatus.NOVA_ORDEM_SOLICITADA; // (nova ordem) 
                order.TransactTime = nos.TransactTime.getValue();
                if (nos.IsSetField(Tags.ExpireDate))
                    order.ExpireDate = DateTime.ParseExact(nos.ExpireDate.getValue() + "235959", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                else
                    order.ExpireDate = DateTime.MinValue;
                order.TimeInForce = nos.IsSetField(Tags.TimeInForce) ? nos.TimeInForce.getValue().ToString() : "0";
                order.ChannelID = operador;
                int len = nos.NoPartyIDs.getValue();
                for (int i = 0; i < len; i++)
                {
                    Group grp = nos.GetGroup(i + 1, Tags.NoPartyIDs);
                    if (null != grp)
                    {
                        string enteringTrader = grp.GetField(Tags.PartyRole);
                        if (enteringTrader.Equals(PartyRole.ENTERING_TRADER.ToString()))
                        {
                            order.ExecBroker = grp.GetField(Tags.PartyID);
                            break;
                        }
                    }
                }
                order.Side = Convert.ToInt32(nos.Side.getValue().ToString());
                order.OrderQty = Convert.ToInt32(nos.OrderQty.getValue());
                order.OrderQtyRemaining = order.OrderQty; // (new order, entao nada foi executado)
                order.OrderQtyMin = nos.IsSetField(Tags.MinQty) ? nos.MinQty.getValue() : Decimal.Zero;
                order.OrderQtyApar = nos.IsSetField(Tags.MaxFloor) ? nos.MaxFloor.getValue() : Decimal.Zero;
                order.Price = nos.IsSetField(Tags.Price) ? nos.Price.getValue() : Decimal.Zero;
                order.SystemID = "FixServer";
                order.Memo = nos.IsSetField(Tags.Memo) ? nos.GetField(Tags.Memo): string.Empty;
                order.FixMsgSeqNum = Convert.ToInt32(nos.Header.GetField(Tags.MsgSeqNum));
                order.SessionID = nos.GetSessionID(msg).ToString();
                order.SessionIDOriginal = nos.GetString(CustomTags.ORIG_SESSION);
                order.IdFix = nos.GetInt(CustomTags.FIXID);
                order.MsgFix = nos.ToString().Replace('\x01', '|');
                // Para order_detail
                order.Description = DescMsg.NOS_OPEN;
                order.HandlInst = nos.IsSetField(Tags.HandlInst) ? nos.GetField(Tags.HandlInst) : string.Empty;
                if (!_db.InserirOrdem(order))
                {
                    logger.Info("Problemas na insercao da ordem. ClOrdId: " + order.ClOrdID);
                }
                
                DropCopyCallbackManager.Instance.EnqueueCallback(order);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem NewOrderSingle: " + ex.Message, ex);
            }
        }

        public void ProcessOrderCancelRequest(QuickFix.Message msg, string bolsa)
        {
            try
            {
                OrderCancelRequest ocr = (OrderCancelRequest) msg;
                string origclordid = ocr.OrigClOrdID.getValue();
                int account;
                if (bolsa.Equals(ExchangePrefixes.BOVESPA, StringComparison.InvariantCultureIgnoreCase))
                {
                    string acc = ocr.Account.getValue();
                    acc = (acc.Remove(acc.Length - 1));
                    account = Convert.ToInt32(acc);
                }
                else
                    account = Convert.ToInt32(ocr.Account.getValue());

                // Buscar ordem original
                OrderDbInfo orderOrig = _db.BuscarOrdem(origclordid, account, ocr.Symbol.getValue());
                // Adicionar as informacoes da ordem original na tabela de upodate
                if (orderOrig.OrderID == 0)
                {
                    orderOrig = null;
                    orderOrig = _db.BuscarOrdem(origclordid, account, ocr.Symbol.getValue(), true);
                    if (orderOrig.OrderID == 0)
                    {
                        logger.Info("OCR - Nao achou a ordem em questao!!!: " + origclordid);
                        return;
                    }
                }
                // Adicionar informacoes originais na tabela update
                OrderDbUpdateInfo updt = new OrderDbUpdateInfo();
                updt.Account = orderOrig.Account;
                updt.Instrumento = orderOrig.Symbol;
                updt.OrderID = orderOrig.OrderID;
                updt.ClOrdID = orderOrig.ClOrdID;
                updt.OrdStatusID = orderOrig.OrdStatus;
                updt.Price = orderOrig.Price;
                updt.Quantidade = orderOrig.OrderQty;
                updt.Quantidade_Exec = orderOrig.CumQty;
                updt.Quantidade_Aparente = Convert.ToInt32(orderOrig.OrderQtyApar);
                updt.Quantidade_Minima = Convert.ToInt32(orderOrig.OrderQtyMin);
                updt.Dt_Validade = orderOrig.ExpireDate;
                updt.Dt_Atualizacao = orderOrig.TransactTime;
                updt.TimeInForce = orderOrig.TimeInForce;
                if (!_db.InserirOrdermUpdate(updt))
                {
                    logger.Error("Erro ao inserir o registro na tabela tb_fix_order_update");
                    return;
                }

                // Adicionar as novas informacoes na tabela detail
                OrderDbDetalheInfo detail = new OrderDbDetalheInfo();
                detail.OrderQty = Convert.ToInt32(ocr.OrderQty.getValue());
                detail.OrderID = orderOrig.OrderID;
                detail.OrderStatusID = (int)FixOrderStatus.CANCELAMENTO_SOLICITADO;
                detail.Description = DescMsg.OCR_PENDING_CANCELLING;
                detail.TransactTime = ocr.TransactTime.getValue();
                detail.FixMsgSeqNum = Convert.ToInt32(ocr.Header.GetField(Tags.MsgSeqNum));
                if (!_db.InserirOrdemDetalhe(detail, orderOrig.ClOrdID))
                {
                    logger.Error("Erro ao inserir o registro na tabela tb_fix_order_update - msg cancelamento");
                    return;
                }

                // Atualizar a informacao original na tabela tb_fix_order
                // Atualiza os campos gravados na tabela tb_fix_order_update
                // Ordem original já se encontra em orderOrig
                orderOrig.OrigClOrdID = ocr.OrigClOrdID.getValue();
                orderOrig.ClOrdID = ocr.ClOrdID.getValue();
                orderOrig.OrdStatus = (int)FixOrderStatus.CANCELAMENTO_SOLICITADO;
                //orderOrig.OrderQty = Convert.ToInt32(ocr.OrderQty.getValue());
                orderOrig.TransactTime = ocr.TransactTime.getValue();
                orderOrig.Memo = ocr.IsSetField(Tags.Memo) ? ocr.GetField(Tags.Memo): string.Empty;
                orderOrig.FixMsgSeqNum = Convert.ToInt32(ocr.Header.GetField(Tags.MsgSeqNum));
                orderOrig.MsgFix = ocr.ToString().Replace('\x01', '|');
                if (!_db.AtualizarOrdem(orderOrig))
                {
                    logger.Info("Problemas na atualizacao da ordem cancelamento. ClOrdID: " + orderOrig.ClOrdID);
                    return;
                }

                DropCopyCallbackManager.Instance.EnqueueCallback(orderOrig);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem OrderCancelRequest: " + ex.Message, ex);
            }
        }

        public void ProcessOrderCancelReplaceRequest(QuickFix.Message msg, string bolsa)
        {
            try
            {
                OrderCancelReplaceRequest ocrr = (OrderCancelReplaceRequest) msg;
                string origclordid = ocrr.OrigClOrdID.getValue();
                int account;
                if (bolsa.Equals(ExchangePrefixes.BOVESPA, StringComparison.InvariantCultureIgnoreCase))
                {
                    string acc = ocrr.Account.getValue();
                    acc = (acc.Remove(acc.Length - 1));
                    account = Convert.ToInt32(acc);
                }
                else
                    account = Convert.ToInt32(ocrr.Account.getValue());
                // Buscar ordem original
                OrderDbInfo orderOrig = _db.BuscarOrdem(origclordid, account, ocrr.Symbol.getValue());
                // Adicionar as informacoes da ordem original na tabela de upodate
                if (orderOrig.OrderID == 0)
                {
                    logger.Info("OCRR - Nao achou a ordem em questao!!!: " + origclordid);
                    return;
                }
                // Adicionar informacoes originais na tabela update
                OrderDbUpdateInfo updt = new OrderDbUpdateInfo();
                updt.Account = orderOrig.Account;
                updt.Instrumento = orderOrig.Symbol;
                updt.OrderID = orderOrig.OrderID;
                updt.ClOrdID = orderOrig.ClOrdID;
                updt.OrdStatusID = orderOrig.OrdStatus;
                updt.Price = orderOrig.Price;
                updt.Quantidade = orderOrig.OrderQty;
                updt.Quantidade_Exec = orderOrig.CumQty;
                updt.Quantidade_Aparente = Convert.ToInt32(orderOrig.OrderQtyApar);
                updt.Quantidade_Minima = Convert.ToInt32(orderOrig.OrderQtyMin);
                updt.Dt_Validade = orderOrig.ExpireDate;
                updt.Dt_Atualizacao = orderOrig.TransactTime;
                updt.TimeInForce = orderOrig.TimeInForce;
                if (!_db.InserirOrdermUpdate(updt))
                {
                    logger.Error("Erro ao inserir o registro na tabela tb_fix_order_update");
                    return;
                }
                
                // Adicionar as novas informacoes pertinentes na tabela detail
                OrderDbDetalheInfo detail = new OrderDbDetalheInfo();
                detail.OrderID = orderOrig.OrderID;
                detail.TransactID = "";
                detail.OrderQty = Convert.ToInt32(ocrr.OrderQty.getValue());
                detail.Price = ocrr.IsSetField(Tags.Price)? ocrr.Price.getValue(): Decimal.Zero;
                detail.OrderStatusID = (int)FixOrderStatus.SUBSTITUICAO_SOLICITADA;
                detail.TransactTime = ocrr.TransactTime.getValue();
                detail.Description = DescMsg.OCRR_PENDING_MODIFICATION;
                detail.FixMsgSeqNum = Convert.ToInt32(ocrr.Header.GetField(Tags.MsgSeqNum));
                if (!_db.InserirOrdemDetalhe(detail, orderOrig.ClOrdID))
                {
                    logger.Error("Erro ao inserir o registro na tabela tb_fix_order_update - msg de modificacao");
                    return;
                }

                // Atualizar a informacao original na tabela tb_fix_order
                // Atualiza os campos gravados na tabela tb_fix_order_update
                // Ordem original já se encontra em orderOrig
                orderOrig.OrigClOrdID = ocrr.OrigClOrdID.getValue();
                orderOrig.ClOrdID = ocrr.ClOrdID.getValue();
                orderOrig.OrdStatus = (int) FixOrderStatus.SUBSTITUICAO_SOLICITADA;
                orderOrig.TransactTime = ocrr.TransactTime.getValue();
                if (ocrr.IsSetField(Tags.ExpireDate))
                    orderOrig.ExpireDate = DateTime.ParseExact(ocrr.ExpireDate.getValue() + "235959", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                else
                    orderOrig.ExpireDate = DateTime.MinValue;
                orderOrig.TimeInForce = ocrr.IsSetField(Tags.TimeInForce) ? ocrr.TimeInForce.getValue().ToString() : "0";
                orderOrig.OrderQty = Convert.ToInt32(ocrr.OrderQty.getValue());
                orderOrig.OrderQtyMin = ocrr.IsSetField(Tags.MinQty) ? ocrr.MinQty.getValue() : 0;
                orderOrig.OrderQtyApar = ocrr.IsSetField(Tags.MaxFloor) ? ocrr.MaxFloor.getValue() : 0;
                orderOrig.Price = ocrr.IsSetField(Tags.Price)? ocrr.Price.getValue(): Decimal.Zero;
                orderOrig.Memo = ocrr.IsSetField(Tags.Memo) ? ocrr.GetField(Tags.Memo): string.Empty;
                orderOrig.FixMsgSeqNum = Convert.ToInt32(ocrr.Header.GetField(Tags.MsgSeqNum));
                orderOrig.MsgFix = ocrr.ToString().Replace('\x01', '|');
                orderOrig.HandlInst = ocrr.IsSetField(Tags.HandlInst) ? ocrr.HandlInst.getValue().ToString() : string.Empty;
                if (!_db.AtualizarOrdem(orderOrig))
                {
                    logger.Info("Problemas na atualizacao da ordem (mensagem de modificacao). ClOrdID: " + orderOrig.ClOrdID);
                    return;
                }

                DropCopyCallbackManager.Instance.EnqueueCallback(orderOrig);

            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem OrderCancelReplaceRequest: " + ex.Message, ex);
            }
        }

        public void ProcessExecutionReport(QuickFix.Message msg, string bolsa)
        {
            try
            {
                ExecutionReport er = (ExecutionReport) msg;

                char execType = er.ExecType.getValue();
                char ordStatus = er.OrdStatus.getValue();
                
                switch (execType)
                {
                    // Trade Bust Working order / Non-working order (TODO [FF]: Dificil de testar esta situacao)
                    case ExecType.TRADE_CANCEL:
                        this._er_Update(er, FixOrderStatus.NEGOCIO_CANCELADO, DescMsg.ER_TRADE_BUST, bolsa);
                        break;
                    // Iceberg restatement (TODO [FF]: Dificil de testar esta situacao)
                    case ExecType.RESTATED:
                        this._er_Update(er, FixOrderStatus.REAPRESENTADA, DescMsg.ER_ORDER_RESTATED, bolsa);
                        break;
                }
                
                switch (ordStatus)
                {
                    // Order Entry Accepted    
                    case OrdStatus.NEW:
                        this._er_Update(er, FixOrderStatus.NOVA, DescMsg.ER_NOS_TO_EXCHANGE, bolsa);
                        break;
                    // Order Entry rejected
                    // Nao necessario estabelecer informacoes da tb_fix_update, pois nao se trata de alteracao / cancelamento
                    case OrdStatus.REJECTED:
                        this._er_Update(er, FixOrderStatus.REJEITADA, DescMsg.ER_NOS_REJECTED, bolsa);
                        break;
                    // Modification Accepted
                    case OrdStatus.REPLACED:
                        this._er_Update(er, FixOrderStatus.SUBSTITUIDA, DescMsg.ER_OCRR_MODIFIED, bolsa);
                        break;
                    // Cancelation Accepted
                    case OrdStatus.CANCELED:
                        this._er_Update(er, FixOrderStatus.CANCELADA, DescMsg.ER_OCR_CANCELLED, bolsa);
                        break;
                    // Full Fill
                    case OrdStatus.FILLED:
                        this._er_Update(er, FixOrderStatus.EXECUTADA, DescMsg.ER_ORDER_FILLED, bolsa);
                        break;
                    // Partial Fill
                    case OrdStatus.PARTIALLY_FILLED:
                        this._er_Update(er, FixOrderStatus.PARCIALMENTEEXECUTADA, DescMsg.ER_ORDER_PARTIALLY_FILLED, bolsa);
                        break;
                    // Order Expiration (all time in forces, except FOK and IOC)
                    case OrdStatus.EXPIRED:
                        this._er_Update(er, FixOrderStatus.EXPIRADA, DescMsg.ER_ORDER_EXPIRED, bolsa);
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem ExecutionReport: " + ex.Message, ex);
            }
        }

        public void ProcessOrderCancelReject(QuickFix.Message msg, string bolsa)
        {
            try
            {
                OrderCancelReject ocr = (OrderCancelReject) msg;
                // Buscar a ordem original a partir do OrigClOrdID
                string clordid = ocr.ClOrdID.getValue();
                int account;
                if (bolsa.Equals(ExchangePrefixes.BOVESPA, StringComparison.InvariantCultureIgnoreCase))
                {
                    string acc = ocr.IsSetAccount()? ocr.Account.getValue(): string.Empty;
                    if (!string.IsNullOrEmpty(acc))
                    {
                        acc = (acc.Remove(acc.Length - 1));
                        account = 0;
                    }
                    else
                        account = 0;
                }
                else
                    account = ocr.IsSetAccount()? Convert.ToInt32(ocr.Account.getValue()): 0;



                OrderDbInfo orderOrig = null;
                if (account != 0)
                    orderOrig = _db.BuscarOrdem(clordid, account, ocr.Symbol.getValue());
                else
                    orderOrig = _db.BuscarOrdemPorClOrdEOrigClOrd(clordid, ocr.OrigClOrdID.getValue(), ocr.Symbol.getValue());


                if (orderOrig.ClOrdID.Equals(string.Empty))
                {
                    logger.Info("Problema ao consultar informacoes da ordem original");
                    return;
                }

                // Preencher informacoes de tb_fix_order_detail (rejection)
                OrderDbDetalheInfo detail = new OrderDbDetalheInfo();
                detail.OrderID = orderOrig.OrderID;
                //detail.OrderQty = orderOrig.OrderQty;
                //detail.OrderQtyRemaining = orderOrig.OrderQtyRemaining;
                //detail.Price = orderOrig.Price;
                detail.OrderStatusID = (int) FixOrderStatus.REJEITADA;
                detail.TransactTime = ocr.IsSetField(Tags.TransactTime)? ocr.TransactTime.getValue(): DateTime.MinValue;
                detail.Description = ocr.IsSetField(Tags.Text) ? ocr.Text.getValue() : string.Empty;
                detail.CxlRejResponseTo = ocr.IsSetField(Tags.CxlRejResponseTo) ? ocr.CxlRejResponseTo.getValue().ToString() : string.Empty;
                detail.CxlRejReason = ocr.IsSetField(Tags.CxlRejReason)? ocr.CxlRejReason.getValue(): 0;
                detail.FixMsgSeqNum = Convert.ToInt32(ocr.Header.GetField(Tags.MsgSeqNum));

                if (!_db.InserirOrdemDetalhe(detail, clordid))
                {
                    logger.Info("Problemas na inserção da ordem detalhe");
                    return;
                }
                // Buscar as informacoes da tabela update, utilizando o OrigClOrdID da ordem original
                OrderDbUpdateInfo orderUpdate = _db.BuscarOrdemUpdate(orderOrig.OrigClOrdID);

                // Atribuir as informacoes do update no order original
                orderOrig.ClOrdID = orderUpdate.ClOrdID;
                orderOrig.OrdStatus = orderUpdate.OrdStatusID;
                orderOrig.Price = orderUpdate.Price;
                orderOrig.OrderQty = orderUpdate.Quantidade;
                orderOrig.OrderQtyApar = orderUpdate.Quantidade_Aparente;
                orderOrig.OrderQtyMin = orderUpdate.Quantidade_Minima;
                //orderOrig.OrderQtyRemaining = orderUpdate.Quantidade_Exec;
                orderOrig.ExpireDate = orderUpdate.Dt_Validade;
                orderOrig.OrigClOrdID = string.Empty;
                orderOrig.MsgFix = ocr.ToString().Replace('\x01', '|');
                // Atualizar as informacoes do update na tabela da ordem original
                if (!_db.AtualizarOrdem(orderOrig))
                {
                    logger.Info("Problemas na atualizacao da ordem (mensagem ordercancelreject). ClOrdID: " + orderOrig.ClOrdID);
                    return;
                }

                DropCopyCallbackManager.Instance.EnqueueCallback(orderOrig);

            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem OrderCancelReject: " + ex.Message, ex);
            }
        }

        public void ProcessBusinessMessageReject(QuickFix.Message msg)
        {
            // Efetuar o log da rejeicao
            logger.Error("********* BUSINESSMESSAGEREJECT MESSAGE");
            logger.Error("BusinessMessageReject: Message: " + msg.ToString());
            
        }

        public void ProcessReject(QuickFix.Message msg)
        {
            // Efetuar o log da rejeicao
            logger.Error("********* REJECT MESSAGE");
            logger.Error("Reject: Message: " + msg.ToString());
        }
        #endregion
        
        #region "ExecutionReport StatusManagement"
        private bool _er_Update(ExecutionReport er, FixOrderStatus ordS, string desc, string bolsa)
        {
            try
            {
                int account;
                if (bolsa.Equals(ExchangePrefixes.BOVESPA, StringComparison.InvariantCultureIgnoreCase))
                {
                    string acc = er.Account.getValue();
                    acc = (acc.Remove(acc.Length - 1));
                    account = Convert.ToInt32(acc);
                }
                else
                    account = Convert.ToInt32(er.Account.getValue());
                
                // Buscar Ordem Original, de algum jeito!!!
                // 1a. tentativa: Tentar via ExchangeNumber
                // 2a. tentativa: Tentar via ClOrdID e OrigClOrdID
                // 3a. tentativa: Tentar via ClOrdID no tb_fix_order_update e buscar a ordem pelo OrderID
                // 4a. tentativa: ahhh foda-se, nao achou mesmo

                OrderDbInfo orderOrig = null;
                
                // 1a. tentativa
                if (er.IsSetOrderID() && !er.OrderID.getValue().Equals("NONE"))
                    orderOrig = _db.BuscarOrdemPorExchangeNumber(er.OrderID.getValue());
                // 2a. Tentativa
                if (null == orderOrig || orderOrig.OrderID == 0)
                {
                    orderOrig = _db.BuscarOrdem(er.ClOrdID.getValue(), account, er.Symbol.getValue());
                    if (orderOrig.OrderID == 0)
                    {
                        // Se ordem nao encontrada, entao procurar pelo OrigClOrdID
                        if (er.IsSetOrigClOrdID())
                        {
                            orderOrig = _db.BuscarOrdem(er.OrigClOrdID.getValue(), account, er.Symbol.getValue());
                            if (orderOrig.OrderID == 0)
                            {
                                orderOrig = _db.BuscarOrdem(er.OrigClOrdID.getValue(), account, er.Symbol.getValue(), true);
                                //if (orderOrig.OrderID == 0)
                                //{
                                //    logger.Info("01 ER - Nao achou a ordem em questao!!!: " + er.OrigClOrdID.getValue() + " Status: " + ordS.ToString() + " Desc: " + desc);
                                //    return false;
                                //}
                            }
                            else
                            {
                                orderOrig = _db.BuscarOrdem(er.OrigClOrdID.getValue(), account, er.Symbol.getValue());
                            }
                        }
                        else
                        {
                            orderOrig = _db.BuscarOrdem(er.ClOrdID.getValue(), account, er.Symbol.getValue(), true);
                            //if (orderOrig.OrderID == 0)
                            //{
                            //    logger.Info("02 ER - Nao achou a ordem em questao!!!: " + er.ClOrdID.getValue() + " Status: " + ordS.ToString() + " Desc: " + desc);
                            //    return false;
                            //}
                        }
                    }
                }

                // 3a. Tentativa
                if (null == orderOrig || orderOrig.OrderID == 0)
                {
                    // Buscar a partir de tb_fix_order_update
                    OrderDbUpdateInfo orderUpdate = _db.BuscarOrdemUpdate(er.ClOrdID.getValue());
                    // Buscar ordem original a partir do order ID
                    if (orderUpdate.OrderID != 0)
                    {
                        orderOrig = _db.BuscarOrdemPorOrderID(orderUpdate.OrderID);
                    }
                    else
                    {
                        logger.Info("01 ER - Nao achou a ordem em questao!!!: " + er.ClOrdID.getValue() + " Status: " + ordS.ToString() + " Desc: " + desc);
                        return false;
                    }
                }
                if (null == orderOrig || orderOrig.OrderID == 0)
                {
                    logger.Info("02 ER - Nao achou a ordem em questao!!!: " + er.ClOrdID.getValue() + " Status: " + ordS.ToString() + " Desc: " + desc);
                    return false;
                }
                //}
                //else
                //{
                //    orderOrig = _db.BuscarOrdemPorExchangeNumber(er.OrderID.getValue());
                //    if (orderOrig.OrderID == 0)
                //    {
                //        logger.Info("ER - Nao achou a ordem em questao via exchange number (OrderID ER)!!!: " + er.OrderID.getValue() + " Status: " + ordS.ToString() + " Desc: " + desc);
                //        return false;
                //    }
                // }
                // Adicionar OrdemDetalhe
                OrderDbDetalheInfo detail = new OrderDbDetalheInfo();
                detail.OrderID = orderOrig.OrderID;
                detail.TransactID = er.ExecID.getValue();
                detail.OrderQty = Convert.ToInt32(er.OrderQty.getValue());
                detail.Price = er.IsSetField(Tags.Price) ? er.Price.getValue() : Decimal.Zero;
                detail.OrderStatusID = (int) ordS;
                detail.TransactTime = er.TransactTime.getValue();
                if (er.IsSetField(Tags.Text))
                    detail.Description = desc + " - " + er.Text.getValue();
                else
                    detail.Description = desc;

                detail.TradeQty = er.IsSetField(Tags.LastQty) ? Convert.ToInt32(er.LastQty.getValue()) : 0;
                detail.CumQty = er.IsSetField(Tags.CumQty) ? Convert.ToInt32(er.CumQty.getValue()) : 0;
                detail.FixMsgSeqNum = Convert.ToInt32(er.Header.GetField(Tags.MsgSeqNum));
                if (!_db.InserirOrdemDetalhe(detail, orderOrig.ClOrdID))
                {
                    logger.Info("Erro ao inserir o registro na tabela tb_fix_order_update");
                    return false;
                }

                // Atualizar Ordem
                orderOrig.ExchangeNumberID = er.OrderID.getValue();
                orderOrig.OrdStatus = (int) ordS;
                orderOrig.TransactTime = er.TransactTime.getValue();
                // orderOrig.ClOrdID = er.ClOrdID.getValue();
                // if (er.IsSetOrigClOrdID())
                //    orderOrig.OrigClOrdID = er.OrigClOrdID.getValue();
                if (er.IsSetField(Tags.LeavesQty))
                    orderOrig.OrderQtyRemaining = Convert.ToInt32(er.LeavesQty.getValue());
                if (er.IsSetField(Tags.CumQty))
                    orderOrig.CumQty = Convert.ToInt32(er.CumQty.getValue());
                orderOrig.Memo = er.IsSetField(Tags.Memo) ? er.GetField(Tags.Memo): string.Empty;
                if (!_db.AtualizarOrdem(orderOrig))
                {
                    logger.Info("Problemas na atualizacao da ordem. ClOrdID: " + orderOrig.ClOrdID);
                    return false;
                }

                DropCopyCallbackManager.Instance.EnqueueCallback(orderOrig);

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("_er_Update: Erro na atualizacao dos status da ordem: " + ex.Message, ex);
                return false;
            }
        }
        #endregion
    }
}
