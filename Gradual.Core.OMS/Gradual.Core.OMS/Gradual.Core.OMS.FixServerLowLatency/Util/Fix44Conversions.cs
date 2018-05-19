using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using log4net;

using QuickFix;
// using QuickFix.FIX44;
using QuickFix.Fields;

using Gradual.OMS.RoteadorOrdens.Lib.Dados;

using Cortex.OMS.FixUtilities.Lib;
using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;

namespace Gradual.Core.OMS.FixServerLowLatency.Util
{
    public class Fix44Conversions
    {
        private static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Fix44Conversions()
        {
               
        }

        public static OrdemInfo Fix44ExecutionReport2OrdemInfo(QuickFix.FIX44.ExecutionReport er, FixSessionItem cfg)
        {
            try
            {
                OrdemInfo order = new OrdemInfo();
                string descricao = string.Empty;

                // order = new OrdemInfo();
                order.Account = Convert.ToInt32(er.IsSetAccount() ? er.Account.getValue() : "0");
                order.Exchange = cfg.Bolsa;
                order.ChannelID = cfg.Operador;
                order.ClOrdID = er.ClOrdID.ToString();//chaveClOrdId;
                order.OrigClOrdID = er.IsSetOrigClOrdID() ? er.OrigClOrdID.getValue() : null;
                order.ExecBroker = "227";
                if (er.IsSetExpireDate())
                {
                    string expdate = er.ExpireDate.getValue() + "235959";
                    order.ExpireDate = DateTime.ParseExact(expdate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                }
                else
                {
                    order.ExpireDate = DateTime.MinValue;
                }
                order.MaxFloor = er.IsSetMaxFloor() ? Convert.ToDouble(er.MaxFloor.getValue()) : 0;
                order.MinQty = er.IsSetMinQty() ? Convert.ToDouble(er.MinQty.getValue()) : 0;
                order.OrderQty = Convert.ToInt32(er.IsSetOrderQty() ? er.OrderQty.getValue() : 0);
                order.OrdType = FixMessageUtilities.TraduzirOrdemTipo(er.OrdType.getValue());
                order.Price = er.IsSetPrice() ? Convert.ToDouble(er.Price.getValue()) : 0;
                order.SecurityID = er.IsSetSecurityID() ? er.SecurityID.ToString() : null;
                order.Side = er.IsSetSide() ? (OrdemDirecaoEnum) Convert.ToInt32(er.Side.ToString()) : OrdemDirecaoEnum.NaoInformado;
                order.Symbol = er.IsSetSymbol() ? er.Symbol.ToString() : null;
                order.TimeInForce = er.IsSetTimeInForce() ? FixMessageUtilities.deTimeInForceParaOrdemValidade(er.TimeInForce) : OrdemValidadeEnum.NaoInformado;
                //}

                order.ExchangeNumberID = er.IsSetOrderID() ? er.OrderID.getValue() : null;
                order.OrderQtyRemmaining = Convert.ToInt32(er.IsSetLeavesQty() ? er.LeavesQty.getValue() : 0);
                order.CumQty = Convert.ToInt32(er.IsSetCumQty() ? er.CumQty.getValue() : 0);
                order.OrdStatus = FixMessageUtilities.TraduzirOrdemStatus(er.OrdStatus.getValue());
                order.Memo5149 = er.IsSetField(5149) ? er.GetField(5149) : String.Empty;
                order.PossDupFlag = er.Header.IsSetField(Tags.PossDupFlag) ? er.Header.GetBoolean(Tags.PossDupFlag) : false;
                order.PossResend = er.Header.IsSetField(Tags.PossResend) ? er.Header.GetBoolean(Tags.PossResend) : false;
                order.CompIDBolsa = er.Header.IsSetField(Tags.SenderCompID) ? er.Header.GetString(Tags.SenderCompID) : cfg.SenderCompID;
                order.CompIDOMS = er.Header.IsSetField(Tags.TargetCompID) ? er.Header.GetString(Tags.TargetCompID) : cfg.TargetCompID;

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
                        if (er.IsSetText())
                            descricao = er.Text.getValue();
                        break;
                }
                order.TransactTime = DateTime.Now;
                // Try get the msg seq number
                order.FixMsgSeqNum = er.Header.IsSetField(Tags.MsgSeqNum) ? er.Header.GetInt(Tags.MsgSeqNum) : 0;
                order.ProtectionPrice = Convert.ToDecimal(er.IsSetField(35001) ? er.GetString(35001) : "0");


                // Setando informações de acompanhamento
                AcompanhamentoOrdemInfo acompanhamento = new AcompanhamentoOrdemInfo();
                acompanhamento.NumeroControleOrdem = order.ClOrdID;
                acompanhamento.CodigoDoCliente = order.Account;
                acompanhamento.CodigoResposta = order.ExchangeNumberID;
                acompanhamento.CodigoTransacao = er.IsSetExecID() ? er.ExecID.getValue() : null;
                acompanhamento.Instrumento = order.Symbol;
                acompanhamento.SecurityID = order.SecurityID;
                acompanhamento.CanalNegociacao = order.ChannelID;
                acompanhamento.Direcao = order.Side;
                acompanhamento.QuantidadeSolicitada = order.OrderQty;
                acompanhamento.QuantidadeExecutada = (int)er.CumQty.getValue();
                acompanhamento.QuantidadeRemanescente = (int)er.LeavesQty.getValue();
                acompanhamento.QuantidadeNegociada = er.IsSetLastQty() ? (int)er.LastQty.getValue() : 0;
                acompanhamento.Preco = new Decimal(order.Price);
                acompanhamento.StatusOrdem = FixMessageUtilities.TraduzirOrdemStatus(er.OrdStatus.getValue());
                acompanhamento.DataOrdemEnvio = order.TransactTime;
                acompanhamento.DataAtualizacao = DateTime.Now;
                acompanhamento.CodigoRejeicao = er.IsSetOrdRejReason() ? er.OrdRejReason.ToString() : "0";
                acompanhamento.Descricao = descricao;
                // Try get the msg seq number
                acompanhamento.FixMsgSeqNum = er.Header.IsSetField(Tags.MsgSeqNum) ? er.Header.GetInt(Tags.MsgSeqNum) : 0;
                acompanhamento.LastPx = er.IsSetLastPx() ? (Decimal)er.LastPx.getValue() : new Decimal(order.Price);
                acompanhamento.TradeDate = er.IsSetTradeDate() ? er.TradeDate.getValue() : DateTime.Now.ToString("yyyyMMdd");

                //BEI Fields
                //Added in 2012-Nov-13 by ATP
                acompanhamento.ExchangeOrderID = er.IsSetField(35022) ? er.GetString(35022) : String.Empty;
                acompanhamento.ExchangeExecID = er.IsSetField(35023) ? er.GetString(35023) : String.Empty;
                acompanhamento.LastPxInIssuedCurrency = Convert.ToDecimal(er.IsSetField(35024) ? er.GetString(35024) : "0");
                acompanhamento.PriceInIssuedCurrency = Convert.ToDecimal(er.IsSetField(35025) ? er.GetString(35025) : "0");
                acompanhamento.ExchangeSecondaryOrderID = er.IsSetField(35026) ? er.GetString(35026) : String.Empty;
                acompanhamento.TradeLinkID = er.IsSetField(820) ? er.GetString(820) : String.Empty;
                acompanhamento.OrderLinkID = er.IsSetField(5975) ? er.GetString(5975) : String.Empty;
                acompanhamento.ExchangeQuoteID = er.IsSetField(5001) ? er.GetString(5001) : String.Empty;
                acompanhamento.PossDupFlag = er.Header.IsSetField(Tags.PossDupFlag) ? er.Header.GetBoolean(Tags.PossDupFlag) : false;
                acompanhamento.PossResend = er.Header.IsSetField(Tags.PossResend) ? er.Header.GetBoolean(Tags.PossResend) : false;
                acompanhamento.CompIDBolsa = er.Header.IsSetField(Tags.SenderCompID) ? er.Header.GetString(Tags.SenderCompID) : cfg.SenderCompID;
                acompanhamento.CompIDOMS = er.Header.IsSetField(Tags.TargetCompID) ? er.Header.GetString(Tags.TargetCompID) : cfg.TargetCompID;

                if (er.IsSetNoMiscFees())
                {
                    int ocorr = er.GetInt(Tags.NoMiscFees);// noMiscFees.getValue();

                    for (uint i = 0; i < ocorr; i++)
                    {
                        EmolumentoInfo emol = new EmolumentoInfo();
                        Group feeGroup = er.GetGroup((int) i, Tags.NoMiscFees);
                        emol.Valor = Convert.ToDecimal(feeGroup.IsSetField(Tags.MiscFeeAmt) ? feeGroup.GetString(Tags.MiscFeeAmt) : "0");
                        emol.BaseEmolumento = Convert.ToInt32(feeGroup.IsSetField(Tags.MiscFeeBasis) ? feeGroup.GetString(Tags.MiscFeeBasis) : "0");
                        emol.Currency = feeGroup.GetString(Tags.MiscFeeCurr);
                        emol.Tipo = (EmolumentoTipoEnum) feeGroup.GetInt(Tags.MiscFeeType);
                    }
                }

                // Adicionando informações de acompanhamento ao OrdemInfo
                order.Acompanhamentos.Clear();
                order.Acompanhamentos.Add(acompanhamento);

                return order;
            }
            catch (Exception ex)
            {
                logger.Error("Fix44ExecutionReport2OrdemInfo(): " + ex.Message, ex);
            }
            return null;
        }

        public static QuickFix.FIX44.ExecutionReport Fix44Rejection2ExecutionReport(QuickFix.Message msgReject, QuickFix.Message msgOri)
        {
            try
            {
                QuickFix.FIX44.ExecutionReport er = new QuickFix.FIX44.ExecutionReport();
                if (msgOri.IsSetField(Tags.OrderID))
                    er.Set(new OrderID(msgOri.GetString(Tags.OrderID)));
                else
                    er.Set(new OrderID("NONE"));

                if (msgOri.IsSetField(Tags.ClOrdID)) er.Set(new ClOrdID(msgOri.GetField(Tags.ClOrdID)));
                if (msgOri.IsSetField(Tags.OrigClOrdID)) er.Set(new OrigClOrdID(msgOri.GetField(Tags.OrigClOrdID)));
                // Fazer atribuicao dos PartyIDs da mensagem original
                int len = msgOri.GetInt(Tags.NoPartyIDs);
                for (int i = 0; i < len; i++)
                {
                    Group grp = msgOri.GetGroup(i+1, Tags.NoPartyIDs);
                    er.AddGroup(grp);
                }
                er.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                er.Set(new ExecType(ExecType.REJECTED));
                er.Set(new OrdStatus(OrdStatus.REJECTED));
                if (msgOri.IsSetField(Tags.Account)) er.Set(new Account(msgOri.GetField(Tags.Account)));
                er.Set(new Symbol(msgOri.GetField(Tags.Symbol)));
                if (msgOri.IsSetField(Tags.SecurityID)) er.Set(new SecurityID(msgOri.GetField(Tags.SecurityID)));
                if (msgOri.IsSetField(Tags.SecurityIDSource)) er.Set(new SecurityIDSource(msgOri.GetField(Tags.SecurityIDSource)));
                er.Set(new Side(msgOri.GetChar(Tags.Side)));
                er.Set(new OrderQty(msgOri.GetDecimal(Tags.OrderQty)));
                if (msgOri.IsSetField(Tags.OrdType)) er.Set(new OrdType(msgOri.GetChar(Tags.OrdType)));
                if (msgOri.IsSetField(Tags.Price)) er.Set(new Price(msgOri.GetDecimal(Tags.Price)));
                if (msgOri.IsSetField(Tags.StopPx)) er.Set(new StopPx(msgOri.GetDecimal(Tags.StopPx)));
                if (msgOri.IsSetField(Tags.TimeInForce)) er.Set(new TimeInForce(msgOri.GetChar(Tags.TimeInForce)));
                if (msgOri.IsSetField(Tags.ExpireDate)) er.Set(new ExpireDate(msgOri.GetField(Tags.ExpireDate)));
                er.Set(new LeavesQty(0));
                er.Set(new CumQty(0));
                er.Set(new AvgPx(Decimal.Zero));
                DateTime transact = DateTime.UtcNow;
                er.Set(new TransactTime(transact));
                if (msgOri.IsSetField(Tags.MaxFloor)) er.Set(new MaxFloor(msgOri.GetDecimal(Tags.MaxFloor)));
                if (msgOri.IsSetField(Tags.Memo)) 
                    er.SetField(new Memo(msgOri.GetString(Tags.Memo)));
                    
                if (msgReject.IsSetField(Tags.Text))
                    er.Set(new Text(msgReject.GetField(Tags.Text)));
                if (msgReject.IsSetField(Tags.OrderID))
                    er.Set(new OrderID(msgReject.GetField(Tags.OrderID)));

                return er;
            }
            catch (Exception ex)
            {
                logger.Error("Fix44Rejection2ExecutionReport(): " + ex.Message, ex);
                return null;
            }
        }

        public static QuickFix.FIX44.OrderCancelReject Fix44Reject2OrderCancelReject(QuickFix.Message msgReject, QuickFix.Message msgOri)
        {
            try
            {
                QuickFix.FIX44.OrderCancelReject ocr = new QuickFix.FIX44.OrderCancelReject();
                if (msgOri.IsSetField(Tags.OrderID))
                    ocr.Set(new OrderID(msgOri.GetString(Tags.OrderID)));
                else
                    ocr.Set(new OrderID("NONE"));
                ocr.Set(new ClOrdID(msgOri.GetString(Tags.ClOrdID)));           // mandatory
                ocr.Set(new OrigClOrdID(msgOri.GetString(Tags.OrigClOrdID)));   // mandatory
                ocr.Set(new OrdStatus(OrdStatus.REJECTED));                     // mandatory
                if (msgOri.IsSetField(Tags.Account))
                    ocr.SetField(new Account(msgOri.GetString(Tags.Account)));
                if (msgOri.Header.GetString(Tags.MsgType).Equals(MsgType.ORDER_CANCEL_REQUEST)) // mandatory
                    ocr.Set(new CxlRejResponseTo('1'));
                else
                    ocr.Set(new CxlRejResponseTo('2'));
                if (msgReject.IsSetField(Tags.Text))
                    ocr.Set(new Text(msgReject.GetString(Tags.Text)));
                // ocr.Set(new CxlRejReason(2137));
                
                // Tratamento tag SecondaryOrderID
                // Se nao vier na mensagem busca pelo TO
                //if (msgOri.IsSetField(Tags.SecondaryOrderID))
                //{
                //    ocr.Set(new SecondaryOrderID(msgOri.GetString(Tags.SecondaryOrderID)));
                //}
                //else
                //{
                //    if (null != to)
                //    {
                //        if (!string.IsNullOrEmpty(to.SecondaryOrderID))
                //            ocr.Set(new SecondaryOrderID(to.SecondaryOrderID));
                //            
                //        if (!string.IsNullOrEmpty(to.TradeDate))
                //            ocr.Set(new TradeDate(to.TradeDate));
                //    }
                //}
                int len = msgOri.GetInt(Tags.NoPartyIDs);
                for (int i = 0; i < len; i++)
                {
                    Group grp = msgOri.GetGroup(i + 1, Tags.NoPartyIDs);
                    ocr.AddGroup(grp);
                }
                ocr.Set(new Symbol(msgOri.GetString(Tags.Symbol)));
                if (msgOri.IsSetField(Tags.SecurityID)) ocr.Set(new SecurityID(msgOri.GetField(Tags.SecurityID)));
                if (msgOri.IsSetField(Tags.SecurityIDSource)) ocr.Set(new SecurityIDSource(msgOri.GetField(Tags.SecurityIDSource)));
                ocr.Set(new Side(msgOri.GetChar(Tags.Side)));
                if (msgOri.IsSetField(Tags.Memo))
                    ocr.Set(new Memo(msgOri.GetString(Tags.Memo)));
                return ocr;
            }
            catch (Exception ex)
            {
                logger.Error("Fix44Reject2OrderCancelReject(): " + ex.Message, ex);
                return null;
            }

        }

        public static List<QuickFix.FIX44.ExecutionReport> Fix44Rejection2ExecutionReportNOC(QuickFix.Message msgReject, QuickFix.FIX44.NewOrderCross noc)
        {
            try
            {
                QuickFix.FIX44.ExecutionReport er1 = new QuickFix.FIX44.ExecutionReport();
                QuickFix.FIX44.ExecutionReport er2 = new QuickFix.FIX44.ExecutionReport();
                er1.Set(new OrderID("NONE"));
                er2.Set(new OrderID("NONE"));
                if (noc.IsSetNoSides() && noc.NoSides.getValue() == 2)
                {
                    QuickFix.Group grpNoSides = noc.GetGroup(1, Tags.NoSides);
                    if (grpNoSides.IsSetField(Tags.ClOrdID)) er1.Set(new ClOrdID(grpNoSides.GetField(Tags.ClOrdID)));
                    er1.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                    er1.Set(new ExecType(ExecType.REJECTED));
                    er1.Set(new OrdStatus(OrdStatus.REJECTED));
                    if (grpNoSides.IsSetField(Tags.Account)) er1.Set(new Account(grpNoSides.GetField(Tags.Account)));
                    er1.Set(new Symbol(noc.GetField(Tags.Symbol)));
                    if (noc.IsSetField(Tags.SecurityID)) er1.Set(new SecurityID(noc.GetField(Tags.SecurityID)));
                    if (noc.IsSetField(Tags.SecurityIDSource)) er1.Set(new SecurityIDSource(noc.GetField(Tags.SecurityIDSource)));
                    er1.Set(new Side(grpNoSides.GetChar(Tags.Side)));
                    er1.Set(new OrderQty(grpNoSides.GetDecimal(Tags.OrderQty)));
                    if (noc.IsSetField(Tags.OrdType)) er1.Set(new OrdType(noc.GetChar(Tags.OrdType)));
                    if (noc.IsSetField(Tags.Price)) er1.Set(new Price(noc.GetDecimal(Tags.Price)));
                    er1.Set(new LeavesQty(0));
                    er1.Set(new CumQty(0));
                    er1.Set(new AvgPx(Decimal.Zero));
                    DateTime transact1 = DateTime.UtcNow;
                    er1.Set(new TransactTime(transact1));
                    if (msgReject.IsSetField(Tags.Text))
                        er1.Set(new Text(msgReject.GetField(Tags.Text)));
                   
                    grpNoSides = noc.GetGroup(2, Tags.NoSides);
                    if (grpNoSides.IsSetField(Tags.ClOrdID)) er2.Set(new ClOrdID(grpNoSides.GetField(Tags.ClOrdID)));
                    er2.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                    er2.Set(new ExecType(ExecType.REJECTED));
                    er2.Set(new OrdStatus(OrdStatus.REJECTED));
                    if (grpNoSides.IsSetField(Tags.Account)) er2.Set(new Account(grpNoSides.GetField(Tags.Account)));
                    er2.Set(new Symbol(noc.GetField(Tags.Symbol)));
                    if (noc.IsSetField(Tags.SecurityID)) er2.Set(new SecurityID(noc.GetField(Tags.SecurityID)));
                    if (noc.IsSetField(Tags.SecurityIDSource)) er2.Set(new SecurityIDSource(noc.GetField(Tags.SecurityIDSource)));
                    er2.Set(new Side(grpNoSides.GetChar(Tags.Side)));
                    er2.Set(new OrderQty(grpNoSides.GetDecimal(Tags.OrderQty)));
                    if (noc.IsSetField(Tags.OrdType)) er2.Set(new OrdType(noc.GetChar(Tags.OrdType)));
                    if (noc.IsSetField(Tags.Price)) er2.Set(new Price(noc.GetDecimal(Tags.Price)));
                    er2.Set(new LeavesQty(0));
                    er2.Set(new CumQty(0));
                    er2.Set(new AvgPx(Decimal.Zero));
                    DateTime transact2 = DateTime.UtcNow;
                    er2.Set(new TransactTime(transact2));
                    if (msgReject.IsSetField(Tags.Text))
                        er2.Set(new Text(msgReject.GetField(Tags.Text)));
                }

                List<QuickFix.FIX44.ExecutionReport> ret = new List<QuickFix.FIX44.ExecutionReport>();
                ret.Add(er1);
                ret.Add(er2);
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Fix44Rejection2ExecutionReportNOC(): " + ex.Message, ex);
                return null;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////
        #region "FIX42 Conversions to Fix44"

        private static void _fix42Header_2_Fix44Header(QuickFix.Header hdr42, QuickFix.Header hdr44)
        {
            try
            {
                // if (hdr42.IsSetField(Tags.BeginString)) hdr44.SetField(new BeginString(hdr42.GetField(Tags.BeginString)));
                // if (hdr42.IsSetField(Tags.BodyLength)) hdr44.SetField(new BodyLength(hdr42.GetInt(Tags.BodyLength)));
                if (hdr42.IsSetField(Tags.MsgType)) hdr44.SetField(new MsgType(hdr42.GetField(Tags.MsgType)));
                if (hdr42.IsSetField(Tags.SenderCompID)) hdr44.SetField(new SenderCompID(hdr42.GetField(Tags.SenderCompID)));
                if (hdr42.IsSetField(Tags.TargetCompID)) hdr44.SetField(new TargetCompID(hdr42.GetField(Tags.TargetCompID)));
                if (hdr42.IsSetField(Tags.OnBehalfOfCompID)) hdr44.SetField(new OnBehalfOfCompID(hdr42.GetField(Tags.OnBehalfOfCompID)));
                if (hdr42.IsSetField(Tags.DeliverToCompID)) hdr44.SetField(new DeliverToCompID(hdr42.GetField(Tags.DeliverToCompID)));
                if (hdr42.IsSetField(Tags.SecureDataLen)) hdr44.SetField(new SecureDataLen(hdr42.GetInt(Tags.SecureDataLen)));
                if (hdr42.IsSetField(Tags.SecureData)) hdr44.SetField(new SecureData(hdr42.GetField(Tags.SecureData)));
                if (hdr42.IsSetField(Tags.MsgSeqNum)) hdr44.SetField(new MsgSeqNum(hdr42.GetInt(Tags.MsgSeqNum)));
                if (hdr42.IsSetField(Tags.SenderSubID)) hdr44.SetField(new SenderSubID(hdr42.GetField(Tags.SenderSubID)));
                if (hdr42.IsSetField(Tags.SenderLocationID)) hdr44.SetField(new SenderLocationID(hdr42.GetField(Tags.SenderLocationID)));
                if (hdr42.IsSetField(Tags.TargetSubID)) hdr44.SetField(new TargetSubID(hdr42.GetField(Tags.TargetSubID)));
                if (hdr42.IsSetField(Tags.TargetLocationID)) hdr44.SetField(new TargetLocationID(hdr42.GetField(Tags.TargetLocationID)));
                if (hdr42.IsSetField(Tags.OnBehalfOfSubID)) hdr44.SetField(new OnBehalfOfSubID(hdr42.GetField(Tags.OnBehalfOfSubID)));
                if (hdr42.IsSetField(Tags.OnBehalfOfLocationID)) hdr44.SetField(new OnBehalfOfLocationID(hdr42.GetField(Tags.OnBehalfOfLocationID)));
                if (hdr42.IsSetField(Tags.DeliverToSubID)) hdr44.SetField(new DeliverToSubID(hdr42.GetField(Tags.DeliverToSubID)));
                if (hdr42.IsSetField(Tags.DeliverToLocationID)) hdr44.SetField(new DeliverToLocationID(hdr42.GetField(Tags.DeliverToLocationID)));
                if (hdr42.IsSetField(Tags.PossDupFlag)) hdr44.SetField(new PossDupFlag(hdr42.GetBoolean(Tags.PossDupFlag)));
                if (hdr42.IsSetField(Tags.PossResend)) hdr44.SetField(new PossResend(hdr42.GetBoolean(Tags.PossResend)));
                if (hdr42.IsSetField(Tags.SendingTime)) hdr44.SetField(new SendingTime(hdr42.GetDateTime(Tags.SendingTime)));
                if (hdr42.IsSetField(Tags.OrigSendingTime)) hdr44.SetField(new OrigSendingTime(hdr42.GetDateTime(Tags.OrigSendingTime)));
                if (hdr42.IsSetField(Tags.XmlDataLen)) hdr44.SetField(new XmlDataLen(hdr42.GetInt(Tags.XmlDataLen)));
                if (hdr42.IsSetField(Tags.XmlData)) hdr44.SetField(new XmlData(hdr42.GetField(Tags.XmlData)));
                if (hdr42.IsSetField(Tags.MessageEncoding)) hdr44.SetField(new MessageEncoding(hdr42.GetField(Tags.MessageEncoding)));
                if (hdr42.IsSetField(Tags.LastMsgSeqNumProcessed)) hdr44.SetField(new LastMsgSeqNumProcessed(hdr42.GetInt(Tags.LastMsgSeqNumProcessed)));
                if (hdr42.IsSetField(Tags.OnBehalfOfSendingTime)) hdr44.SetField(new OnBehalfOfSendingTime(hdr42.GetDateTime(Tags.OnBehalfOfSendingTime)));

                /*
                if (hdr42.IsSetField(Tags.MsgType)) hdr44.SetField(new MsgType(hdr42.GetField(Tags.MsgType)));
                if (hdr42.IsSetField(Tags.BeginString)) hdr44.SetField(new BeginString(hdr42.GetField(Tags.BeginString)));
                //if (hdr42.IsSetField(Tags.BodyLength)) hdr44.SetField(new BodyLength(hdr42.GetField(Tags.BodyLength)));
                if (hdr42.IsSetField(Tags.DeliverToCompID)) hdr44.SetField(new DeliverToCompID(hdr42.GetField(Tags.DeliverToCompID)));
                if (hdr42.IsSetField(Tags.DeliverToSubID)) hdr44.SetField(new DeliverToSubID(hdr42.GetField(Tags.DeliverToSubID)));
                if (hdr42.IsSetField(Tags.MsgSeqNum)) hdr44.SetField(new MsgSeqNum(hdr42.GetInt(Tags.MsgSeqNum)));
                if (hdr42.IsSetField(Tags.OnBehalfOfCompID)) hdr44.SetField(new OnBehalfOfCompID(hdr42.GetField(Tags.OnBehalfOfCompID)));
                if (hdr42.IsSetField(Tags.OnBehalfOfSubID)) hdr44.SetField(new OnBehalfOfSubID(hdr42.GetField(Tags.OnBehalfOfSubID)));
                if (hdr42.IsSetField(Tags.OrigSendingTime)) hdr44.SetField(new OrigSendingTime(hdr42.GetDateTime(Tags.OrigSendingTime)));
                if (hdr42.IsSetField(Tags.PossDupFlag)) hdr44.SetField(new PossDupFlag(hdr42.GetBoolean(Tags.PossDupFlag)));
                if (hdr42.IsSetField(Tags.PossResend)) hdr44.SetField(new PossResend(hdr42.GetBoolean(Tags.PossResend)));
                if (hdr42.IsSetField(Tags.SenderCompID)) hdr44.SetField(new SenderCompID(hdr42.GetField(Tags.SenderCompID)));
                if (hdr42.IsSetField(Tags.SenderSubID)) hdr44.SetField(new SenderSubID(hdr42.GetField(Tags.SenderSubID)));
                if (hdr42.IsSetField(Tags.SendingTime)) hdr44.SetField(new SendingTime(hdr42.GetDateTime(Tags.SendingTime)));
                if (hdr42.IsSetField(Tags.TargetCompID)) hdr44.SetField(new TargetCompID(hdr42.GetField(Tags.TargetCompID)));
                if (hdr42.IsSetField(Tags.TargetSubID)) hdr44.SetField(new TargetSubID(hdr42.GetField(Tags.TargetSubID)));     
                 */
            }
            catch (Exception ex)
            {
                logger.Error("_fix42Header_2_Fix44Header(): " + ex.Message, ex);
            }
        }



        public static QuickFix.FIX44.NewOrderSingle Fix42NOS_2_Fix44NOS(QuickFix.FIX42.NewOrderSingle nos42)
        {
            try
            {
                QuickFix.FIX44.NewOrderSingle nos44 = new QuickFix.FIX44.NewOrderSingle();


                _fix42Header_2_Fix44Header(nos42.Header, nos44.Header);

                if (nos42.IsSetClOrdID()) nos44.Set(new ClOrdID(nos42.ClOrdID.getValue()));
                // if (nos42.IsSetClientID()) nos44.Set(new ClientID(nos42.ClientID.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetExecBroker()) nos44.Set(new ExecBroker(nos42.ExecBroker.getValue())); // Not found on 4.4EP
                if (nos42.IsSetAccount()) nos44.Set(new Account(nos42.Account.getValue()));
                //if (nos42.IsSetNoAllocs()) nos44.Set(new NoAllocs(nos42.NoAllocs.getValue())); // Not found on 4.4EP
                //if (nos42.IsSet AllocAccount()) nos44.Set(new AllocAccount(nos42.AllocAccount.getValue())); // Not found on 4.4EP
                //if (nos42.IsSetAllocShares()) nos44.Set(new AllocShares(nos42.AllocShares.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetSettlmntTyp()) nos44.SetField(new SettlType(nos42.GetField(Tags.SettlType))); // Not found on 4.4EP
                // if (nos42.IsSetFutSettDate()) nos44.Set(new FutSettDate(nos42.FutSettDate.getValue()));
                // if (nos42.IsSetHandlInst()) nos44.Set(new HandlInst(nos42.HandlInst.getValue()));
                // if (nos42.IsSetExecInst()) nos44.Set(new ExecInst(nos42.ExecInst.getValue()));
                if (nos42.IsSetMinQty()) nos44.Set(new MinQty(nos42.MinQty.getValue()));
                if (nos42.IsSetMaxFloor()) nos44.Set(new MaxFloor(nos42.MaxFloor.getValue()));
                //if (nos42.IsSetExDestination()) nos44.Set(new ExDestination(nos42.ExDestination.getValue()));
                //if (nos42.IsSetNoTradingSessions()) nos44.Set(new NoTradingSessions(nos42.NoTradingSessions.getValue()));
                //if (nos42.IsSetTradingSessionID()) nos44.Set(new TradingSessionID(nos42.TradingSessionID.getValue()));
                // if (nos42.IsSetProcessCode()) nos44.Set(new ProcessCode(nos42.ProcessCode.getValue()));
                if (nos42.IsSetSymbol()) nos44.Set(new Symbol(nos42.Symbol.getValue()));
                // if (nos42.IsSetSymbolSfx()) nos44.Set(new SymbolSfx(nos42.SymbolSfx.getValue()));
                if (nos42.IsSetSecurityID()) nos44.Set(new SecurityID(nos42.SecurityID.getValue()));
                if (nos42.IsSetIDSource()) nos44.Set(new SecurityIDSource(nos42.IDSource.getValue()));
                // if (nos42.IsSetSecurityType()) nos44.Set(new SecurityType(nos42.SecurityType.getValue()));
                // if (nos42.IsSetMaturityMonthYear()) nos44.Set(new MaturityMonthYear(nos42.MaturityMonthYear.getValue()));
                // if (nos42.IsSetMaturityDay()) nos44.Set(new MaturityDay(nos42.MaturityDay.getValue()));
                // if (nos42.IsSetPutOrCall()) nos44.Set(new PutOrCall(nos42.PutOrCall.getValue()));
                // if (nos42.IsSetStrikePrice()) nos44.Set(new StrikePrice(nos42.StrikePrice.getValue()));
                // if (nos42.IsSetOptAttribute()) nos44.Set(new OptAttribute(nos42.OptAttribute.getValue()));
                // if (nos42.IsSetContractMultiplier()) nos44.Set(new ContractMultiplier(nos42.ContractMultiplier.getValue()));
                // if (nos42.IsSetCouponRate()) nos44.Set(new CouponRate(nos42.CouponRate.getValue()));
                if (nos42.IsSetSecurityExchange()) nos44.Set(new SecurityExchange(nos42.SecurityExchange.getValue()));
                // if (nos42.IsSetIssuer()) nos44.Set(new Issuer(nos42.Issuer.getValue()));
                // if (nos42.IsSetEncodedIssuerLen()) nos44.Set(new EncodedIssuerLen(nos42.EncodedIssuerLen.getValue()));
                // if (nos42.IsSetEncodedIssuer()) nos44.Set(new EncodedIssuer(nos42.EncodedIssuer.getValue()));
                // if (nos42.IsSetSecurityDesc()) nos44.Set(new SecurityDesc(nos42.SecurityDesc.getValue()));
                // if (nos42.IsSetEncodedSecurityDescLen()) nos44.Set(new EncodedSecurityDescLen(nos42.EncodedSecurityDescLen.getValue()));
                // if (nos42.IsSetEncodedSecurityDesc()) nos44.Set(new EncodedSecurityDesc(nos42.EncodedSecurityDesc.getValue()));
                // if (nos42.IsSetPrevClosePx()) nos44.Set(new PrevClosePx(nos42.PrevClosePx.getValue()));
                if (nos42.IsSetSide()) nos44.Set(new Side(nos42.Side.getValue()));
                // if (nos42.IsSetLocateReqd()) nos44.Set(new LocateReqd(nos42.LocateReqd.getValue()));
                if (nos42.IsSetTransactTime()) nos44.Set(new TransactTime(nos42.TransactTime.getValue()));
                if (nos42.IsSetOrderQty()) nos44.Set(new OrderQty(nos42.OrderQty.getValue()));
                // if (nos42.IsSetCashOrderQty()) nos44.Set(new CashOrderQty(nos42.CashOrderQty.getValue()));
                if (nos42.IsSetOrdType()) nos44.Set(new OrdType(nos42.OrdType.getValue()));
                if (nos42.IsSetPrice()) nos44.Set(new Price(nos42.Price.getValue()));
                if (nos42.IsSetStopPx()) nos44.Set(new StopPx(nos42.StopPx.getValue()));
                if (nos42.IsSetCurrency()) nos44.Set(new Currency(nos42.Currency.getValue()));
                // if (nos42.IsSetComplianceID()) nos44.Set(new ComplianceID(nos42.ComplianceID.getValue()));
                // if (nos42.IsSetSolicitedFlag()) nos44.Set(new SolicitedFlag(nos42.SolicitedFlag.getValue()));
                // if (nos42.IsSetIOIid()) nos44.Set(new IOIid(nos42.IOIid.getValue()));
                // if (nos42.IsSetQuoteID()) nos44.Set(new QuoteID(nos42.QuoteID.getValue()));
                if (nos42.IsSetTimeInForce()) nos44.Set(new TimeInForce(nos42.TimeInForce.getValue()));
                // if (nos42.IsSetEffectiveTime()) nos44.Set(new EffectiveTime(nos42.EffectiveTime.getValue()));
                if (nos42.IsSetExpireDate()) nos44.Set(new ExpireDate(nos42.ExpireDate.getValue()));
                // if (nos42.IsSetExpireTime()) nos44.Set(new ExpireTime(nos42.ExpireTime.getValue()));
                // if (nos42.IsSetGTBookingInst()) nos44.Set(new GTBookingInst(nos42.GTBookingInst.getValue()));
                // if (nos42.IsSetCommission()) nos44.Set(new Commission(nos42.Commission.getValue()));
                // if (nos42.IsSetCommType()) nos44.Set(new CommType(nos42.CommType.getValue()));
                // if (nos42.IsSetRule80A()) nos44.Set(new Rule80A(nos42.Rule80A.getValue()));
                // if (nos42.IsSetForexReq()) nos44.Set(new ForexReq(nos42.ForexReq.getValue()));
                // if (nos42.IsSetSettlCurrency()) nos44.Set(new SettlCurrency(nos42.SettlCurrency.getValue()));
                // if (nos42.IsSetText()) nos44.Set(new Text(nos42.Text.getValue()));
                // if (nos42.IsSetEncodedTextLen()) nos44.Set(new EncodedTextLen(nos42.EncodedTextLen.getValue()));
                // if (nos42.IsSetEncodedText()) nos44.Set(new EncodedText(nos42.EncodedText.getValue()));
                // if (nos42.IsSetFutSettDate2()) nos44.Set(new FutSettDate2(nos42.FutSettDate2.getValue()));
                // if (nos42.IsSetOrderQty2()) nos44.Set(new OrderQty2(nos42.OrderQty2.getValue()));
                // if (nos42.IsSetOpenClose()) nos44.Set(new OpenClose(nos42.OpenClose.getValue()));
                // if (nos42.IsSetCoveredOrUncovered()) nos44.Set(new CoveredOrUncovered(nos42.CoveredOrUncovered.getValue()));
                // if (nos42.IsSetCustomerOrFirm()) nos44.Set(new CustomerOrFirm(nos42.CustomerOrFirm.getValue()));
                // if (nos42.IsSetMaxShow()) nos44.Set(new MaxShow(nos42.MaxShow.getValue()));
                // if (nos42.IsSetPegDifference()) nos44.Set(new PegDifference(nos42.PegDifference.getValue()));
                // if (nos42.IsSetDiscretionInst()) nos44.Set(new DiscretionInst(nos42.DiscretionInst.getValue()));
                // if (nos42.IsSetDiscretionOffset()) nos44.Set(new DiscretionOffset(nos42.DiscretionOffset.getValue()));
                // if (nos42.IsSetClearingFirm()) nos44.Set(new ClearingFirm(nos42.ClearingFirm.getValue()));
                // if (nos42.IsSetClearingAccount()) nos44.Set(new ClearingAccount(nos42.ClearingAccount.getValue()));
                if (nos42.IsSetField(Tags.Memo)) nos44.SetField(new Memo(nos42.GetField(Tags.Memo)));

                // Bloomberg Extra Fields
                if (nos42.IsSetField(Tags.TradeDate)) nos44.SetField(new TradeDate(nos42.GetField(Tags.TradeDate)));

                /*
                // Site bits to bytes
                if (nos42.IsSetClOrdID()) nos44.Set(new ClOrdID(nos42.ClOrdID.getValue()));
                //if (nos42.IsSetClientID()) nos44.Set(new ClientID(nos42.ClientID.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetExecBroker()) nos44.Set(new ExecBroker(nos42.ExecBroker.getValue())); // Not found on 4.4 (TODO[FF]: Rever com partyIDs
                if (nos42.IsSetAccount()) nos44.Set(new Account(nos42.Account.getValue()));
                // if (nos42.IsSetNoAllocs()) nos44.Set(new NoAllocs(nos42.NoAllocs.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetField(Tags.AllocAccount)) nos44.SetField(new AllocAccount(nos42.GetField(Tags.AllocAccount))); // Not found on 4.4EP
                // if (nos42.IsSetAllocShares()) nos44.Set(new AllocShares(nos42.AllocShares.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetSettlmntTyp()) nos44.SetField(new SettlType(nos42.GetField(Tags.SettlType))); // Not found on 4.4EP
                // if (nos42.IsSetFutSettDate()) nos44.Set(new SettlDate(nos42.FutSettDate.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetHandlInst()) nos44.Set(new HandlInst(nos42.HandlInst.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetExecInst()) nos44.Set(new ExecInst(nos42.ExecInst.getValue())); // Not found on 4.4EP
                if (nos42.IsSetMinQty()) nos44.Set(new MinQty(nos42.MinQty.getValue()));
                if (nos42.IsSetMaxFloor()) nos44.Set(new MaxFloor(nos42.MaxFloor.getValue()));
                // if (nos42.IsSetExDestination()) nos44.Set(new ExDestination(nos42.ExDestination.getValue())); // Not found on 4.4EP
                // TODO [FF] - Efetuar implementacao de add group
                // if (nos42.IsSetNoTradingSessions()) nos44.Set(new NoTradingSessions(nos42.NoTradingSessions.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetTradingSessionID()) nos44.Set(new TradingSessionID(nos42.TradingSessionID.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetProcessCode()) nos44.Set(new ProcessCode(nos42.ProcessCode.getValue())); // Not found on 4.4EP
                if (nos42.IsSetSymbol()) nos44.Set(new Symbol(nos42.Symbol.getValue()));
                if (nos42.IsSetSymbolSfx()) nos44.Set(new SymbolSfx(nos42.SymbolSfx.getValue()));
                if (nos42.IsSetSecurityID()) nos44.Set(new SecurityID(nos42.SecurityID.getValue()));
                if (nos42.IsSetIDSource()) nos44.Set(new SecurityIDSource(nos42.IDSource.getValue()));
                // if (nos42.IsSetSecurityType()) nos44.Set(new SecurityType(nos42.SecurityType.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetMaturityMonthYear()) nos44.Set(new MaturityMonthYear(nos42.MaturityMonthYear.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetMaturityDay()) nos44.Set(new MaturityDay(nos42.MaturityDay.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetPutOrCall()) nos44.Set(new PutOrCall(nos42.PutOrCall.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetStrikePrice()) nos44.Set(new StrikePrice(nos42.StrikePrice.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetOptAttribute()) nos44.Set(new OptAttribute(nos42.OptAttribute.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetContractMultiplier()) nos44.Set(new ContractMultiplier(nos42.ContractMultiplier.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetCouponRate()) nos44.Set(new CouponRate(nos42.CouponRate.getValue())); // Not found on 4.4EP
                if (nos42.IsSetSecurityExchange()) nos44.Set(new SecurityExchange(nos42.SecurityExchange.getValue()));
                // if (nos42.IsSetIssuer()) nos44.Set(new Issuer(nos42.Issuer.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetEncodedIssuerLen()) nos44.Set(new EncodedIssuerLen(nos42.EncodedIssuerLen.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetEncodedIssuer()) nos44.Set(new EncodedIssuer(nos42.EncodedIssuer.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetSecurityDesc()) nos44.Set(new SecurityDesc(nos42.SecurityDesc.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetEncodedSecurityDescLen()) nos44.Set(new EncodedSecurityDescLen(nos42.EncodedSecurityDescLen.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetEncodedSecurityDesc()) nos44.Set(new EncodedSecurityDesc(nos42.EncodedSecurityDesc.getValue())); // Not found on 4.4EP

                // if (nos42.IsSetPrevClosePx()) nos44.Set(new PrevClosePx(nos42.PrevClosePx.getValue())); // Not found on 4.4EP
                if (nos42.IsSetSide()) nos44.Set(new Side(nos42.Side.getValue()));
                // if (nos42.IsSetLocateReqd()) nos44.Set(new LocateReqd(nos42.LocateReqd.getValue())); // Not found on 4.4EP
                if (nos42.IsSetTransactTime()) nos44.Set(new TransactTime(nos42.TransactTime.getValue()));
                if (nos42.IsSetOrderQty()) nos44.Set(new OrderQty(nos42.OrderQty.getValue()));
                // if (nos42.IsSetCashOrderQty()) nos44.Set(new CashOrderQty(nos42.CashOrderQty.getValue())); // Not found on 4.4EP
                if (nos42.IsSetOrdType()) nos44.Set(new OrdType(nos42.OrdType.getValue()));
                if (nos42.IsSetPrice()) nos44.Set(new Price(nos42.Price.getValue()));
                if (nos42.IsSetStopPx()) nos44.Set(new StopPx(nos42.StopPx.getValue()));
                if (nos42.IsSetCurrency()) nos44.Set(new Currency(nos42.Currency.getValue()));

                // if (nos42.IsSetComplianceID()) nos44.Set(new ComplianceID(nos42.ComplianceID.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetSolicitedFlag()) nos44.Set(new SolicitedFlag(nos42.SolicitedFlag.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetIOIid()) nos44.Set(new IOIid(nos42.IOIid.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetQuoteID()) nos44.Set(new QuoteID(nos42.QuoteID.getValue())); // Not found on 4.4EP
                if (nos42.IsSetTimeInForce()) nos44.Set(new TimeInForce(nos42.TimeInForce.getValue()));
                // if (nos42.IsSetEffectiveTime()) nos44.Set(new EffectiveTime(nos42.EffectiveTime.getValue())); // Not found on 4.4EP
                if (nos42.IsSetExpireDate()) nos44.Set(new ExpireDate(nos42.ExpireDate.getValue()));
                if (nos42.IsSetExpireTime()) nos44.Set(new ExpireTime(nos42.ExpireTime.getValue()));
                // if (nos42.IsSetGTBookingInst()) nos44.Set(new GTBookingInst(nos42.GTBookingInst.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetCommission()) nos44.Set(new Commission(nos42.Commission.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetCommType()) nos44.Set(new CommType(nos42.CommType.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetRule80A()) nos44.Set(new Rule80A(nos42.Rule80A.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetForexReq()) nos44.Set(new ForexReq(nos42.ForexReq.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetSettlCurrency()) nos44.Set(new SettlCurrency(nos42.SettlCurrency.getValue())); // Not found on 4.4EP
                if (nos42.IsSetText()) nos44.Set(new Text(nos42.Text.getValue()));
                // if (nos42.IsSetEncodedTextLen()) nos44.Set(new EncodedTextLen(nos42.EncodedTextLen.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetEncodedText()) nos44.Set(new EncodedText(nos42.EncodedText.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetFutSettDate2()) nos44.Set(new FutSettDate2(nos42.FutSettDate2.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetOrderQty2()) nos44.Set(new OrderQty2(nos42.OrderQty2.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetOpenClose()) nos44.Set(new PositionEffect(nos42.OpenClose.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetCoveredOrUncovered()) nos44.Set(new CoveredOrUncovered(nos42.CoveredOrUncovered.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetCustomerOrFirm()) nos44.Set(new CustomerOrFirm(nos42.CustomerOrFirm.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetMaxShow()) nos44.Set(new MaxShow(nos42.MaxShow.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetPegDifference()) nos44.Set(new PegDifference(nos42.PegDifference.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetDiscretionInst()) nos44.Set(new DiscretionInst(nos42.DiscretionInst.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetDiscretionOffset()) nos44.Set(new DiscretionOffset(nos42.DiscretionOffset.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetClearingFirm()) nos44.Set(new ClearingFirm(nos42.ClearingFirm.getValue())); // Not found on 4.4EP
                // if (nos42.IsSetClearingAccount()) nos44.Set(new ClearingAccount(nos42.ClearingAccount.getValue())); // Not found on 4.4EP
                */
                /*
                // Bloomberg
                if (nos42.IsSetAccount()) nos44.Set(new Account(nos42.Account.getValue()));
                // if (nos42.IsSetClientID()) nos44.Set(new ClientID(nos44.ClientID.getValue())); // TODO[FF] Fix44EP does not support this tag on this msg
                if (nos42.IsSetClOrdID()) nos44.Set(new ClOrdID(nos42.ClOrdID.getValue()));
                if (nos42.IsSetCommission()) nos44.Set(new Commission(nos42.Commission.getValue())); // TODO[FF] Fix44EP does not support this tag on this msg
                if (nos42.IsSetCommType()) nos44.Set(new CommType(nos42.CommType.getValue())); // TODO[FF] Fix44EP does not support this tag on this msg
                if (nos42.IsSetCurrency()) nos44.Set(new Currency(nos42.Currency.getValue()));
                if (nos42.IsSetExDestination()) nos44.Set(new ExDestination(nos42.ExDestination.getValue())); // TODO[FF] Fix44EP does not support this tag on this msg
                if (nos42.IsSetExecInst()) nos44.Set(new ExecInst(nos42.ExecInst.getValue())); // TODO[FF] Fix44EP does not support this tag on this msg
                if (nos42.IsSetExpireTime()) nos44.Set(new ExpireTime(nos42.ExpireTime.getValue())); // TODO[FF] Fix44EP does not support this tag on this msg
                if (nos42.IsSetHandlInst()) nos44.Set(new HandlInst(nos42.HandlInst.getValue())); // TODO[FF] Fix44EP does not support this tag on this msg
                if (nos42.IsSetOrderQty()) nos44.Set(new OrderQty(nos42.OrderQty.getValue()));
                if (nos42.IsSetOrdType()) nos44.Set(new OrdType(nos42.OrdType.getValue()));
                if (nos42.IsSetPrice()) nos44.Set(new Price(nos42.Price.getValue()));
                if (nos42.IsSetSecurityID()) nos44.Set(new SecurityID(nos42.SecurityID.getValue()));
                if (nos42.IsSetField(Tags.SecurityIDSource)) nos44.Set(new SecurityIDSource(nos42.GetField(Tags.SecurityIDSource)));
                if (nos42.IsSetSecurityExchange()) nos44.Set(new SecurityExchange(nos42.SecurityExchange.getValue()));
                if (nos42.IsSetSecurityType()) nos44.Set(new SecurityType(nos42.SecurityType.getValue())); // TODO[FF] Fix44EP does not support this tag on this msg
                if (nos42.IsSetSide()) nos44.Set(new Side(nos42.Side.getValue()));
                if (nos42.IsSetStopPx()) nos44.Set(new StopPx(nos42.StopPx.getValue()));
                if (nos42.IsSetSymbol()) nos44.Set(new Symbol(nos42.Symbol.getValue()));
                if (nos42.IsSetSymbolSfx()) nos44.Set(new SymbolSfx(nos42.SymbolSfx.getValue()));
                if (nos42.IsSetTimeInForce()) nos44.Set(new TimeInForce(nos42.TimeInForce.getValue()));
                if (nos42.IsSetField(Tags.TradeDate)) nos44.Set(new TradeDate(nos42.GetField(Tags.TradeDate))); // TODO[FF] Fix44EP does not support this tag on this msg
                if (nos42.IsSetTransactTime()) nos44.Set(new TransactTime(nos42.TransactTime.getValue()));
                 */ 
                return nos44;
            }
            catch (Exception ex)
            {
                logger.Error("Fix42NOS_2_Fix44NOS(): " + ex.Message, ex);
                return null;
            }

        }

        public static QuickFix.FIX44.OrderCancelReplaceRequest Fix42OCRR_2_Fix44OCRR(QuickFix.FIX42.OrderCancelReplaceRequest ocrr42)
        {
            try
            {
                QuickFix.FIX44.OrderCancelReplaceRequest ocrr44 = new QuickFix.FIX44.OrderCancelReplaceRequest();

                _fix42Header_2_Fix44Header(ocrr42.Header, ocrr44.Header);

                if (ocrr42.IsSetOrderID()) ocrr44.Set(new OrderID(ocrr42.OrderID.getValue()));
                //if (ocrr42.IsSetClientID()) ocrr44.Set(new ClientID(ocrr42.ClientID.getValue()));
                //if (ocrr42.IsSetExecBroker()) ocrr44.Set(new ExecBroker(ocrr42.ExecBroker.getValue()));
                if (ocrr42.IsSetOrigClOrdID()) ocrr44.Set(new OrigClOrdID(ocrr42.OrigClOrdID.getValue()));
                if (ocrr42.IsSetClOrdID()) ocrr44.Set(new ClOrdID(ocrr42.ClOrdID.getValue()));
                // if (ocrr42.IsSetListID()) ocrr44.Set(new ListID(ocrr42.ListID.getValue()));
                if (ocrr42.IsSetAccount()) ocrr44.Set(new Account(ocrr42.Account.getValue()));
                // if (ocrr42.IsSetNoAllocs()) ocrr44.Set(new NoAllocs(ocrr42.NoAllocs.getValue()));
                // if (ocrr42.IsSetAllocAccount()) ocrr44.Set(new AllocAccount(ocrr42.AllocAccount.getValue()));
                // if (ocrr42.IsSetAllocShares()) ocrr44.Set(new AllocShares(ocrr42.AllocShares.getValue()));
                // if (ocrr42.IsSetSettlmntTyp()) ocrr44.Set(new SettlmntTyp(ocrr42.SettlmntTyp.getValue()));
                // if (ocrr42.IsSetFutSettDate()) ocrr44.Set(new FutSettDate(ocrr42.FutSettDate.getValue()));
                // if (ocrr42.IsSetHandlInst()) ocrr44.Set(new HandlInst(ocrr42.HandlInst.getValue()));
                // if (ocrr42.IsSetExecInst()) ocrr44.Set(new ExecInst(ocrr42.ExecInst.getValue()));
                if (ocrr42.IsSetMinQty()) ocrr44.Set(new MinQty(ocrr42.MinQty.getValue()));
                if (ocrr42.IsSetMaxFloor()) ocrr44.Set(new MaxFloor(ocrr42.MaxFloor.getValue()));
                // if (ocrr42.IsSetExDestination()) ocrr44.Set(new ExDestination(ocrr42.ExDestination.getValue()));
                // if (ocrr42.IsSetNoTradingSessions()) ocrr44.Set(new NoTradingSessions(ocrr42.NoTradingSessions.getValue()));
                // if (ocrr42.IsSetTradingSessionID()) ocrr44.Set(new TradingSessionID(ocrr42.TradingSessionID.getValue()));
                if (ocrr42.IsSetSymbol()) ocrr44.Set(new Symbol(ocrr42.Symbol.getValue()));
                // if (ocrr42.IsSetSymbolSfx()) ocrr44.Set(new SymbolSfx(ocrr42.SymbolSfx.getValue()));
                if (ocrr42.IsSetSecurityID()) ocrr44.Set(new SecurityID(ocrr42.SecurityID.getValue()));
                if (ocrr42.IsSetIDSource()) ocrr44.Set(new SecurityIDSource(ocrr42.IDSource.getValue()));
                // if (ocrr42.IsSetSecurityType()) ocrr44.Set(new SecurityType(ocrr42.SecurityType.getValue()));
                // if (ocrr42.IsSetMaturityMonthYear()) ocrr44.Set(new MaturityMonthYear(ocrr42.MaturityMonthYear.getValue()));
                // if (ocrr42.IsSetMaturityDay()) ocrr44.Set(new MaturityDay(ocrr42.MaturityDay.getValue()));
                // if (ocrr42.IsSetPutOrCall()) ocrr44.Set(new PutOrCall(ocrr42.PutOrCall.getValue()));
                // if (ocrr42.IsSetStrikePrice()) ocrr44.Set(new StrikePrice(ocrr42.StrikePrice.getValue()));
                // if (ocrr42.IsSetOptAttribute()) ocrr44.Set(new OptAttribute(ocrr42.OptAttribute.getValue()));
                // if (ocrr42.IsSetContractMultiplier()) ocrr44.Set(new ContractMultiplier(ocrr42.ContractMultiplier.getValue()));
                // if (ocrr42.IsSetCouponRate()) ocrr44.Set(new CouponRate(ocrr42.CouponRate.getValue()));
                if (ocrr42.IsSetSecurityExchange()) ocrr44.Set(new SecurityExchange(ocrr42.SecurityExchange.getValue()));
                // if (ocrr42.IsSetIssuer()) ocrr44.Set(new Issuer(ocrr42.Issuer.getValue()));
                // if (ocrr42.IsSetEncodedIssuerLen()) ocrr44.Set(new EncodedIssuerLen(ocrr42.EncodedIssuerLen.getValue()));
                // if (ocrr42.IsSetEncodedIssuer()) ocrr44.Set(new EncodedIssuer(ocrr42.EncodedIssuer.getValue()));
                // if (ocrr42.IsSetSecurityDesc()) ocrr44.Set(new SecurityDesc(ocrr42.SecurityDesc.getValue()));
                // if (ocrr42.IsSetEncodedSecurityDescLen()) ocrr44.Set(new EncodedSecurityDescLen(ocrr42.EncodedSecurityDescLen.getValue()));
                // if (ocrr42.IsSetEncodedSecurityDesc()) ocrr44.Set(new EncodedSecurityDesc(ocrr42.EncodedSecurityDesc.getValue()));
                if (ocrr42.IsSetSide()) ocrr44.Set(new Side(ocrr42.Side.getValue()));
                if (ocrr42.IsSetTransactTime()) ocrr44.Set(new TransactTime(ocrr42.TransactTime.getValue()));
                if (ocrr42.IsSetOrderQty()) ocrr44.Set(new OrderQty(ocrr42.OrderQty.getValue()));
                // if (ocrr42.IsSetCashOrderQty()) ocrr44.Set(new CashOrderQty(ocrr42.CashOrderQty.getValue()));
                if (ocrr42.IsSetOrdType()) ocrr44.Set(new OrdType(ocrr42.OrdType.getValue()));
                if (ocrr42.IsSetPrice()) ocrr44.Set(new Price(ocrr42.Price.getValue()));
                if (ocrr42.IsSetStopPx()) ocrr44.Set(new StopPx(ocrr42.StopPx.getValue()));
                // if (ocrr42.IsSetPegDifference()) ocrr44.Set(new PegDifference(ocrr42.PegDifference.getValue()));
                // if (ocrr42.IsSetDiscretionInst()) ocrr44.Set(new DiscretionInst(ocrr42.DiscretionInst.getValue()));
                // if (ocrr42.IsSetDiscretionOffset()) ocrr44.Set(new DiscretionOffset(ocrr42.DiscretionOffset.getValue()));
                // if (ocrr42.IsSetComplianceID()) ocrr44.Set(new ComplianceID(ocrr42.ComplianceID.getValue()));
                // if (ocrr42.IsSetSolicitedFlag()) ocrr44.Set(new SolicitedFlag(ocrr42.SolicitedFlag.getValue()));
                if (ocrr42.IsSetCurrency()) ocrr44.Set(new Currency(ocrr42.Currency.getValue()));
                if (ocrr42.IsSetTimeInForce()) ocrr44.Set(new TimeInForce(ocrr42.TimeInForce.getValue()));
                // if (ocrr42.IsSetEffectiveTime()) ocrr44.Set(new EffectiveTime(ocrr42.EffectiveTime.getValue()));
                if (ocrr42.IsSetExpireDate()) ocrr44.Set(new ExpireDate(ocrr42.ExpireDate.getValue()));
                // if (ocrr42.IsSetExpireTime()) ocrr44.Set(new ExpireTime(ocrr42.ExpireTime.getValue()));
                // if (ocrr42.IsSetGTBookingInst()) ocrr44.Set(new GTBookingInst(ocrr42.GTBookingInst.getValue()));
                // if (ocrr42.IsSetCommission()) ocrr44.Set(new Commission(ocrr42.Commission.getValue()));
                // if (ocrr42.IsSetCommType()) ocrr44.Set(new CommType(ocrr42.CommType.getValue()));
                // if (ocrr42.IsSetRule80A()) ocrr44.Set(new Rule80A(ocrr42.Rule80A.getValue()));
                // if (ocrr42.IsSetForexReq()) ocrr44.Set(new ForexReq(ocrr42.ForexReq.getValue()));
                // if (ocrr42.IsSetSettlCurrency()) ocrr44.Set(new SettlCurrency(ocrr42.SettlCurrency.getValue()));
                // if (ocrr42.IsSetText()) ocrr44.Set(new Text(ocrr42.Text.getValue()));
                // if (ocrr42.IsSetEncodedTextLen()) ocrr44.Set(new EncodedTextLen(ocrr42.EncodedTextLen.getValue()));
                // if (ocrr42.IsSetEncodedText()) ocrr44.Set(new EncodedText(ocrr42.EncodedText.getValue()));
                // if (ocrr42.IsSetFutSettDate2()) ocrr44.Set(new FutSettDate2(ocrr42.FutSettDate2.getValue()));
                // if (ocrr42.IsSetOrderQty2()) ocrr44.Set(new OrderQty2(ocrr42.OrderQty2.getValue()));
                // if (ocrr42.IsSetOpenClose()) ocrr44.Set(new OpenClose(ocrr42.OpenClose.getValue()));
                // if (ocrr42.IsSetCoveredOrUncovered()) ocrr44.Set(new CoveredOrUncovered(ocrr42.CoveredOrUncovered.getValue()));
                // if (ocrr42.IsSetCustomerOrFirm()) ocrr44.Set(new CustomerOrFirm(ocrr42.CustomerOrFirm.getValue()));
                // if (ocrr42.IsSetMaxShow()) ocrr44.Set(new MaxShow(ocrr42.MaxShow.getValue()));
                // if (ocrr42.IsSetLocateReqd()) ocrr44.Set(new LocateReqd(ocrr42.LocateReqd.getValue()));
                // if (ocrr42.IsSetClearingFirm()) ocrr44.Set(new ClearingFirm(ocrr42.ClearingFirm.getValue()));
                // if (ocrr42.IsSetClearingAccount()) ocrr44.Set(new ClearingAccount(ocrr42.ClearingAccount.getValue()));
                if (ocrr42.IsSetField(Tags.Memo)) ocrr44.SetField(new Memo(ocrr42.GetField(Tags.Memo)));
                
                // Bloomberg extra fields
                if (ocrr42.IsSetField(Tags.TradeDate)) ocrr44.SetField(new TradeDate(ocrr42.GetField(Tags.TradeDate)));

                /*
                if (ocrr42.IsSetOrderID()) ocrr44.Set(new OrderID(ocrr42.OrderID.getValue()));
                // if (ocrr42.IsSetClientID()) ocrr44.Set(new ClientID(ocrr42.ClientID.getValue())); // Not found on 4.4EP
                // if (ocrr42.IsSetExecBroker()) ocrr44.Set(new ExecBroker(ocrr42.ExecBroker.getValue())); // Rever partyiID
                if (ocrr42.IsSetOrigClOrdID()) ocrr44.Set(new OrigClOrdID(ocrr42.OrigClOrdID.getValue()));
                if (ocrr42.IsSetClOrdID()) ocrr44.Set(new ClOrdID(ocrr42.ClOrdID.getValue()));
                // if (ocrr42.IsSetListID()) ocrr44.Set(new ListID(ocrr42.ListID.getValue()));
                if (ocrr42.IsSetAccount()) ocrr44.Set(new Account(ocrr42.Account.getValue()));
                // if (ocrr42.IsSetNoAllocs()) ocrr44.Set(new NoAllocs(ocrr42.NoAllocs.getValue()));
                // if (ocrr42.IsSetAllocAccount()) ocrr44.Set(new AllocAccount(ocrr42.AllocAccount.getValue()));
                // if (ocrr42.IsSetAllocShares()) ocrr44.Set(new AllocShares(ocrr42.AllocShares.getValue()));
                // if (ocrr42.IsSetSettlmntTyp()) ocrr44.SetField(new SettlType(ocrr42.GetField(Tags.SettlType)));
                // if (ocrr42.IsSetFutSettDate()) ocrr44.Set(new SettlDate(ocrr42.FutSettDate.getValue()));
                // if (ocrr42.IsSetHandlInst()) ocrr44.Set(new HandlInst(ocrr42.HandlInst.getValue()));
                // if (ocrr42.IsSetExecInst()) ocrr44.Set(new ExecInst(ocrr42.ExecInst.getValue()));
                if (ocrr42.IsSetMinQty()) ocrr44.Set(new MinQty(ocrr42.MinQty.getValue()));
                if (ocrr42.IsSetMaxFloor()) ocrr44.Set(new MaxFloor(ocrr42.MaxFloor.getValue()));
                // if (ocrr42.IsSetExDestination()) ocrr44.Set(new ExDestination(ocrr42.ExDestination.getValue()));
                //if (ocrr42.IsSetNoTradingSessions()) ocrr44.Set(new NoTradingSessions(ocrr42.NoTradingSessions.getValue()));
                //if (ocrr42.IsSetTradingSessionID()) ocrr44.Set(new TradingSessionID(ocrr42.TradingSessionID.getValue()));
                if (ocrr42.IsSetSymbol()) ocrr44.Set(new Symbol(ocrr42.Symbol.getValue()));
                // if (ocrr42.IsSetSymbolSfx()) ocrr44.Set(new SymbolSfx(ocrr42.SymbolSfx.getValue()));
                if (ocrr42.IsSetSecurityID()) ocrr44.Set(new SecurityID(ocrr42.SecurityID.getValue()));
                if (ocrr42.IsSetIDSource()) ocrr44.Set(new SecurityIDSource(ocrr42.IDSource.getValue()));
                // if (ocrr42.IsSetSecurityType()) ocrr44.Set(new SecurityType(ocrr42.SecurityType.getValue()));
                // if (ocrr42.IsSetMaturityMonthYear()) ocrr44.Set(new MaturityMonthYear(ocrr42.MaturityMonthYear.getValue()));
                // if (ocrr42.IsSetMaturityDay()) ocrr44.Set(new MaturityDay(ocrr42.MaturityDay.getValue()));
                // if (ocrr42.IsSetPutOrCall()) ocrr44.Set(new PutOrCall(ocrr42.PutOrCall.getValue()));
                // if (ocrr42.IsSetStrikePrice()) ocrr44.Set(new StrikePrice(ocrr42.StrikePrice.getValue()));
                // if (ocrr42.IsSetOptAttribute()) ocrr44.Set(new OptAttribute(ocrr42.OptAttribute.getValue()));
                // if (ocrr42.IsSetContractMultiplier()) ocrr44.Set(new ContractMultiplier(ocrr42.ContractMultiplier.getValue()));
                // if (ocrr42.IsSetCouponRate()) ocrr44.Set(new CouponRate(ocrr42.CouponRate.getValue()));
                if (ocrr42.IsSetSecurityExchange()) ocrr44.Set(new SecurityExchange(ocrr42.SecurityExchange.getValue()));
                // if (ocrr42.IsSetIssuer()) ocrr44.Set(new Issuer(ocrr42.Issuer.getValue()));
                // if (ocrr42.IsSetEncodedIssuerLen()) ocrr44.Set(new EncodedIssuerLen(ocrr42.EncodedIssuerLen.getValue()));
                // if (ocrr42.IsSetEncodedIssuer()) ocrr44.Set(new EncodedIssuer(ocrr42.EncodedIssuer.getValue()));
                // if (ocrr42.IsSetSecurityDesc()) ocrr44.Set(new SecurityDesc(ocrr42.SecurityDesc.getValue()));
                // if (ocrr42.IsSetEncodedSecurityDescLen()) ocrr44.Set(new EncodedSecurityDescLen(ocrr42.EncodedSecurityDescLen.getValue()));
                // if (ocrr42.IsSetEncodedSecurityDesc()) ocrr44.Set(new EncodedSecurityDesc(ocrr42.EncodedSecurityDesc.getValue()));
                if (ocrr42.IsSetSide()) ocrr44.Set(new Side(ocrr42.Side.getValue()));
                if (ocrr42.IsSetTransactTime()) ocrr44.Set(new TransactTime(ocrr42.TransactTime.getValue()));
                if (ocrr42.IsSetOrderQty()) ocrr44.Set(new OrderQty(ocrr42.OrderQty.getValue()));
                // if (ocrr42.IsSetCashOrderQty()) ocrr44.Set(new CashOrderQty(ocrr42.CashOrderQty.getValue()));
                if (ocrr42.IsSetOrdType()) ocrr44.Set(new OrdType(ocrr42.OrdType.getValue()));
                if (ocrr42.IsSetPrice()) ocrr44.Set(new Price(ocrr42.Price.getValue()));
                if (ocrr42.IsSetStopPx()) ocrr44.Set(new StopPx(ocrr42.StopPx.getValue()));
                // if (ocrr42.IsSetPegDifference()) ocrr44.Set(new PegDifference(ocrr42.PegDifference.getValue()));
                // if (ocrr42.IsSetDiscretionInst()) ocrr44.Set(new DiscretionInst(ocrr42.DiscretionInst.getValue()));
                // if (ocrr42.IsSetDiscretionOffset()) ocrr44.Set(new DiscretionOffset(ocrr42.DiscretionOffset.getValue()));
                // if (ocrr42.IsSetComplianceID()) ocrr44.Set(new ComplianceID(ocrr42.ComplianceID.getValue()));
                // if (ocrr42.IsSetSolicitedFlag()) ocrr44.Set(new SolicitedFlag(ocrr42.SolicitedFlag.getValue()));
                if (ocrr42.IsSetCurrency()) ocrr44.Set(new Currency(ocrr42.Currency.getValue()));
                if (ocrr42.IsSetTimeInForce()) ocrr44.Set(new TimeInForce(ocrr42.TimeInForce.getValue()));
                // if (ocrr42.IsSetEffectiveTime()) ocrr44.Set(new EffectiveTime(ocrr42.EffectiveTime.getValue()));
                if (ocrr42.IsSetExpireDate()) ocrr44.Set(new ExpireDate(ocrr42.ExpireDate.getValue()));
                if (ocrr42.IsSetExpireTime()) ocrr44.Set(new ExpireTime(ocrr42.ExpireTime.getValue()));
                // if (ocrr42.IsSetGTBookingInst()) ocrr44.Set(new GTBookingInst(ocrr42.GTBookingInst.getValue()));
                // if (ocrr42.IsSetCommission()) ocrr44.Set(new Commission(ocrr42.Commission.getValue()));
                // if (ocrr42.IsSetCommType()) ocrr44.Set(new CommType(ocrr42.CommType.getValue()));
                // if (ocrr42.IsSetRule80A()) ocrr44.Set(new Rule80A(ocrr42.Rule80A.getValue()));
                // if (ocrr42.IsSetForexReq()) ocrr44.Set(new ForexReq(ocrr42.ForexReq.getValue()));
                // if (ocrr42.IsSetSettlCurrency()) ocrr44.Set(new SettlCurrency(ocrr42.SettlCurrency.getValue()));
                if (ocrr42.IsSetText()) ocrr44.Set(new Text(ocrr42.Text.getValue()));
                // if (ocrr42.IsSetEncodedTextLen()) ocrr44.Set(new EncodedTextLen(ocrr42.EncodedTextLen.getValue()));
                // if (ocrr42.IsSetEncodedText()) ocrr44.Set(new EncodedText(ocrr42.EncodedText.getValue()));
                // if (ocrr42.IsSetFutSettDate2()) ocrr44.Set(new FutSettDate2(ocrr42.FutSettDate2.getValue()));
                // if (ocrr42.IsSetOrderQty2()) ocrr44.Set(new OrderQty2(ocrr42.OrderQty2.getValue()));
                // if (ocrr42.IsSetOpenClose()) ocrr44.Set(new OpenClose(ocrr42.OpenClose.getValue()));
                // if (ocrr42.IsSetCoveredOrUncovered()) ocrr44.Set(new CoveredOrUncovered(ocrr42.CoveredOrUncovered.getValue()));
                // if (ocrr42.IsSetCustomerOrFirm()) ocrr44.Set(new CustomerOrFirm(ocrr42.CustomerOrFirm.getValue()));
                // if (ocrr42.IsSetMaxShow()) ocrr44.Set(new MaxShow(ocrr42.MaxShow.getValue()));
                // if (ocrr42.IsSetLocateReqd()) ocrr44.Set(new LocateReqd(ocrr42.LocateReqd.getValue()));
                // if (ocrr42.IsSetClearingFirm()) ocrr44.Set(new ClearingFirm(ocrr42.ClearingFirm.getValue()));
                // if (ocrr42.IsSetClearingAccount()) ocrr44.Set(new ClearingAccount(ocrr42.ClearingAccount.getValue()));
                */
                /*
                if (ocrr42.IsSetAccount()) ocrr44.Set(new Account(ocrr42.Account.getValue()));
                if (ocrr42.IsSetClOrdID()) ocrr44.Set(new ClOrdID(ocrr42.ClOrdID.getValue()));
                if (ocrr42.IsSetCurrency()) ocrr44.Set(new Currency(ocrr42.Currency.getValue()));  // TODO[FF] Fix44EP does not support this tag on this msg
                if (ocrr42.IsSetExpireTime()) ocrr44.Set(new ExpireTime(ocrr42.ExpireTime.getValue())); // TODO[FF] Fix44EP does not support this tag on this msg
                if (ocrr42.IsSetHandlInst()) ocrr44.Set(new HandlInst(ocrr42.HandlInst.getValue())); // TODO[FF] Fix44EP does not support this tag on this msg
                if (ocrr42.IsSetOrderQty()) ocrr44.Set(new OrderQty(ocrr42.OrderQty.getValue()));
                if (ocrr42.IsSetOrdType()) ocrr44.Set(new OrdType(ocrr42.OrdType.getValue()));
                if (ocrr42.IsSetOrigClOrdID()) ocrr44.Set(new OrigClOrdID(ocrr42.OrigClOrdID.getValue()));
                if (ocrr42.IsSetPrice()) ocrr44.Set(new Price(ocrr42.Price.getValue()));
                if (ocrr42.IsSetSide()) ocrr44.Set(new Side(ocrr42.Side.getValue()));
                if (ocrr42.IsSetStopPx()) ocrr44.Set(new StopPx(ocrr42.StopPx.getValue()));
                if (ocrr42.IsSetSymbol()) ocrr44.Set(new Symbol(ocrr42.Symbol.getValue()));
                if (ocrr42.IsSetTimeInForce()) ocrr44.Set(new TimeInForce(ocrr42.TimeInForce.getValue()));
                if (ocrr42.IsSetField(Tags.TradeDate)) ocrr44.Set(new TradeDate(ocrr42.TradeDate.getValue())); // TODO[FF] Fix44EP does not support this tag on this msg
                 */
                return ocrr44;
            }
            catch (Exception ex)
            {
                logger.Error("Fix42OCRR_2_Fix44OCRR(): Problemas na conversao da mensagem OCRR " + ex.Message, ex);
                return null;
            }
        }

        public static QuickFix.FIX44.OrderCancelRequest Fix42OCR_2_Fix44OCR(QuickFix.FIX42.OrderCancelRequest ocr42)
        {
            try
            {
                QuickFix.FIX44.OrderCancelRequest ocr44 = new QuickFix.FIX44.OrderCancelRequest();
                _fix42Header_2_Fix44Header(ocr42.Header, ocr44.Header);

                if (ocr42.IsSetOrigClOrdID()) ocr44.Set(new OrigClOrdID(ocr42.OrigClOrdID.getValue()));
                if (ocr42.IsSetOrderID()) ocr44.Set(new OrderID(ocr42.OrderID.getValue()));
                if (ocr42.IsSetClOrdID()) ocr44.Set(new ClOrdID(ocr42.ClOrdID.getValue()));
                // if (ocr42.IsSetListID()) ocr44.Set(new ListID(ocr42.ListID.getValue()));
                if (ocr42.IsSetAccount()) ocr44.Set(new Account(ocr42.Account.getValue()));
                // if (ocr42.IsSetClientID()) ocr44.Set(new ClientID(ocr42.ClientID.getValue()));
                // if (ocr42.IsSetExecBroker()) ocr44.Set(new ExecBroker(ocr42.ExecBroker.getValue()));
                if (ocr42.IsSetSymbol()) ocr44.Set(new Symbol(ocr42.Symbol.getValue()));
                //if (ocr42.IsSetSymbolSfx()) ocr44.Set(new SymbolSfx(ocr42.SymbolSfx.getValue()));
                if (ocr42.IsSetSecurityID()) ocr44.Set(new SecurityID(ocr42.SecurityID.getValue()));
                if (ocr42.IsSetIDSource()) ocr44.Set(new SecurityIDSource(ocr42.IDSource.getValue()));
                // if (ocr42.IsSetSecurityType()) ocr44.Set(new SecurityType(ocr42.SecurityType.getValue()));
                // if (ocr42.IsSetMaturityMonthYear()) ocr44.Set(new MaturityMonthYear(ocr42.MaturityMonthYear.getValue()));
                // if (ocr42.IsSetMaturityDay()) ocr44.Set(new MaturityDay(ocr42.MaturityDay.getValue()));
                // if (ocr42.IsSetPutOrCall()) ocr44.Set(new PutOrCall(ocr42.PutOrCall.getValue()));
                // if (ocr42.IsSetStrikePrice()) ocr44.Set(new StrikePrice(ocr42.StrikePrice.getValue()));
                // if (ocr42.IsSetOptAttribute()) ocr44.Set(new OptAttribute(ocr42.OptAttribute.getValue()));
                // if (ocr42.IsSetContractMultiplier()) ocr44.Set(new ContractMultiplier(ocr42.ContractMultiplier.getValue()));
                // if (ocr42.IsSetCouponRate()) ocr44.Set(new CouponRate(ocr42.CouponRate.getValue()));
                if (ocr42.IsSetSecurityExchange()) ocr44.Set(new SecurityExchange(ocr42.SecurityExchange.getValue()));
                // if (ocr42.IsSetIssuer()) ocr44.Set(new Issuer(ocr42.Issuer.getValue()));
                // if (ocr42.IsSetEncodedIssuerLen()) ocr44.Set(new EncodedIssuerLen(ocr42.EncodedIssuerLen.getValue()));
                // if (ocr42.IsSetEncodedIssuer()) ocr44.Set(new EncodedIssuer(ocr42.EncodedIssuer.getValue()));
                // if (ocr42.IsSetSecurityDesc()) ocr44.Set(new SecurityDesc(ocr42.SecurityDesc.getValue()));
                // if (ocr42.IsSetEncodedSecurityDescLen()) ocr44.Set(new EncodedSecurityDescLen(ocr42.EncodedSecurityDescLen.getValue()));
                // if (ocr42.IsSetEncodedSecurityDesc()) ocr44.Set(new EncodedSecurityDesc(ocr42.EncodedSecurityDesc.getValue()));
                if (ocr42.IsSetSide()) ocr44.Set(new Side(ocr42.Side.getValue()));
                if (ocr42.IsSetTransactTime()) ocr44.Set(new TransactTime(ocr42.TransactTime.getValue()));
                if (ocr42.IsSetOrderQty()) ocr44.Set(new OrderQty(ocr42.OrderQty.getValue()));
                // if (ocr42.IsSetCashOrderQty()) ocr44.Set(new CashOrderQty(ocr42.CashOrderQty.getValue()));
                // if (ocr42.IsSetComplianceID()) ocr44.Set(new ComplianceID(ocr42.ComplianceID.getValue()));
                // if (ocr42.IsSetSolicitedFlag()) ocr44.Set(new SolicitedFlag(ocr42.SolicitedFlag.getValue()));
                // if (ocr42.IsSetText()) ocr44.Set(new Text(ocr42.Text.getValue()));
                // if (ocr42.IsSetEncodedTextLen()) ocr44.Set(new EncodedTextLen(ocr42.EncodedTextLen.getValue()));
                // if (ocr42.IsSetEncodedText()) ocr44.Set(new EncodedText(ocr42.EncodedText.getValue()));
                if (ocr42.IsSetField(Tags.Memo)) ocr44.SetField(new Memo(ocr42.GetField(Tags.Memo)));

                // Bloomberg Extra fields
                if (ocr42.IsSetField(Tags.CxlType)) ocr44.SetField(new CxlType(ocr42.GetChar(Tags.CxlType)));
                if (ocr42.IsSetField(Tags.ExecInst)) ocr44.SetField(new ExecInst(ocr42.GetField(Tags.ExecInst)));
                if (ocr42.IsSetField(Tags.HandlInst)) ocr44.SetField(new HandlInst(ocr42.GetChar(Tags.HandlInst)));
                if (ocr42.IsSetField(Tags.OrdType)) ocr44.SetField(new OrdType(ocr42.GetChar(Tags.OrdType)));
                if (ocr42.IsSetField(Tags.Price)) ocr44.SetField(new Price(ocr42.GetDecimal(Tags.Price)));
                if (ocr42.IsSetField(Tags.StopPx)) ocr44.SetField(new StopPx(ocr42.GetDecimal(Tags.StopPx)));
                if (ocr42.IsSetField(Tags.TradeDate)) ocr44.SetField(new TradeDate(ocr42.GetField(Tags.TradeDate)));
                /*
                if (ocr42.IsSetOrigClOrdID()) ocr44.Set(new OrigClOrdID(ocr42.OrigClOrdID.getValue()));
                if (ocr42.IsSetOrderID()) ocr44.Set(new OrderID(ocr42.OrderID.getValue()));
                if (ocr42.IsSetClOrdID()) ocr44.Set(new ClOrdID(ocr42.ClOrdID.getValue()));
                // if (ocr42.IsSetListID()) ocr44.Set(new ListID(ocr42.ListID.getValue()));
                if (ocr42.IsSetAccount()) ocr44.Set(new Account(ocr42.Account.getValue()));
                //if (ocr42.IsSetClientID()) ocr44.Set(new ClientID(ocr42.ClientID.getValue()));
                //if (ocr42.IsSetExecBroker()) ocr44.Set(new ExecBroker(ocr42.ExecBroker.getValue()));
                if (ocr42.IsSetSymbol()) ocr44.Set(new Symbol(ocr42.Symbol.getValue()));
                // if (ocr42.IsSetSymbolSfx()) ocr44.Set(new SymbolSfx(ocr42.SymbolSfx.getValue()));
                if (ocr42.IsSetSecurityID()) ocr44.Set(new SecurityID(ocr42.SecurityID.getValue()));
                if (ocr42.IsSetIDSource()) ocr44.Set(new SecurityIDSource(ocr42.IDSource.getValue()));
                // if (ocr42.IsSetSecurityType()) ocr44.Set(new SecurityType(ocr42.SecurityType.getValue()));
                // if (ocr42.IsSetMaturityMonthYear()) ocr44.Set(new MaturityMonthYear(ocr42.MaturityMonthYear.getValue()));
                // if (ocr42.IsSetMaturityDay()) ocr44.Set(new MaturityDay(ocr42.MaturityDay.getValue()));
                // if (ocr42.IsSetPutOrCall()) ocr44.Set(new PutOrCall(ocr42.PutOrCall.getValue()));
                // if (ocr42.IsSetStrikePrice()) ocr44.Set(new StrikePrice(ocr42.StrikePrice.getValue()));
                // if (ocr42.IsSetOptAttribute()) ocr44.Set(new OptAttribute(ocr42.OptAttribute.getValue()));
                // if (ocr42.IsSetContractMultiplier()) ocr44.Set(new ContractMultiplier(ocr42.ContractMultiplier.getValue()));
                // if (ocr42.IsSetCouponRate()) ocr44.Set(new CouponRate(ocr42.CouponRate.getValue()));
                if (ocr42.IsSetSecurityExchange()) ocr44.Set(new SecurityExchange(ocr42.SecurityExchange.getValue()));
                // if (ocr42.IsSetIssuer()) ocr44.Set(new Issuer(ocr42.Issuer.getValue()));
                // if (ocr42.IsSetEncodedIssuerLen()) ocr44.Set(new EncodedIssuerLen(ocr42.EncodedIssuerLen.getValue()));
                // if (ocr42.IsSetEncodedIssuer()) ocr44.Set(new EncodedIssuer(ocr42.EncodedIssuer.getValue()));
                // if (ocr42.IsSetSecurityDesc()) ocr44.Set(new SecurityDesc(ocr42.SecurityDesc.getValue()));
                // if (ocr42.IsSetEncodedSecurityDescLen()) ocr44.Set(new EncodedSecurityDescLen(ocr42.EncodedSecurityDescLen.getValue()));
                // if (ocr42.IsSetEncodedSecurityDesc()) ocr44.Set(new EncodedSecurityDesc(ocr42.EncodedSecurityDesc.getValue()));
                if (ocr42.IsSetSide()) ocr44.Set(new Side(ocr42.Side.getValue()));
                if (ocr42.IsSetTransactTime()) ocr44.Set(new TransactTime(ocr42.TransactTime.getValue()));
                if (ocr42.IsSetOrderQty()) ocr44.Set(new OrderQty(ocr42.OrderQty.getValue()));
                // if (ocr42.IsSetCashOrderQty()) ocr44.Set(new CashOrderQty(ocr42.CashOrderQty.getValue()));
                // if (ocr42.IsSetComplianceID()) ocr44.Set(new ComplianceID(ocr42.ComplianceID.getValue()));
                // if (ocr42.IsSetSolicitedFlag()) ocr44.Set(new SolicitedFlag(ocr42.SolicitedFlag.getValue()));
                if (ocr42.IsSetText()) ocr44.Set(new Text(ocr42.Text.getValue()));
                // if (ocr42.IsSetEncodedTextLen()) ocr44.Set(new EncodedTextLen(ocr42.EncodedTextLen.getValue()));
                // if (ocr42.IsSetEncodedText()) ocr44.Set(new EncodedText(ocr42.EncodedText.getValue()));
                */
                /*
                if (ocr42.IsSetClOrdID()) ocr44.Set(new ClOrdID(ocr42.ClOrdID.getValue()));
                if (ocr42.IsSetField(Tags.CxlType)) ocr44.SetField(new CxlType(ocr42.GetChar(Tags.CxlType))); // TODO[FF] Fix44EP does not support this tag on this msg
                if (ocr42.IsSetField(Tags.ExecInst)) ocr44.SetField(new ExecInst(ocr42.GetString(Tags.ExecInst)));  // TODO[FF] Fix44EP does not support this tag on this msg
                if (ocr42.IsSetField(Tags.HandlInst)) ocr44.SetField(new HandlInst(ocr42.GetChar(Tags.HandlInst)));  // TODO[FF] Fix44EP does not support this tag on this msg
                if (ocr42.IsSetOrderQty()) ocr44.Set(new OrderQty(ocr42.OrderQty.getValue()));
                if (ocr42.IsSetField(Tags.OrdType)) ocr44.SetField(new OrdType(ocr42.GetChar(Tags.OrdType)));  // TODO[FF] Fix44EP does not support this tag on this msg
                if (ocr42.IsSetOrigClOrdID()) ocr44.Set(new OrigClOrdID(ocr42.OrigClOrdID.getValue()));
                if (ocr42.IsSetField(Tags.Price)) ocr44.SetField(new Price(ocr42.GetDecimal(Tags.Price)));  // TODO[FF] Fix44EP does not support this tag on this msg
                if (ocr42.IsSetSide()) ocr44.Set(new Side(ocr42.Side.getValue()));
                if (ocr42.IsSetField(Tags.StopPx)) ocr44.SetField(new StopPx(ocr42.GetDecimal(Tags.StopPx)));  // TODO[FF] Fix44EP does not support this tag on this msg
                if (ocr42.IsSetField(Tags.TradeDate)) ocr44.SetField(new TradeDate(ocr42.GetString(Tags.TradeDate)));  // TODO[FF] Fix44EP does not support this tag on this msg
                if (ocr42.IsSetSymbol()) ocr44.Set(new Symbol(ocr42.Symbol.getValue()));  // TODO[FF] Fix44EP does not support this tag on this msg
                 */
                return ocr44;
            }
            catch (Exception ex)
            {
                logger.Error("Fix42OCR_2_Fix44OCR(): Problemas na conversao da mensagem OCRR " + ex.Message, ex);
                return null;
            }
        }
        public static QuickFix.FIX44.SequenceReset Fix42SR_2_Fix44SR(QuickFix.FIX42.SequenceReset sr42)
        {
            try
            {
                QuickFix.FIX44.SequenceReset sr44 = new QuickFix.FIX44.SequenceReset();
                _fix42Header_2_Fix44Header(sr42.Header, sr44.Header);

                if (sr42.IsSetNewSeqNo()) sr44.Set(new NewSeqNo(sr42.NewSeqNo.getValue()));
                if (sr42.IsSetGapFillFlag()) sr44.Set(new GapFillFlag(sr42.GapFillFlag.getValue()));
                return sr44;
            }
            catch (Exception ex)
            {
                logger.Error("Fix42SR_2_Fix44SR(): Problemas na conversao da mensagem SR: " + ex.Message, ex);
                return null;
            }
        }

        //public static QuickFix.FIX44.ResendRequest Fix42RR_2_Fix44RR(QuickFix.FIX42.ResendRequest rr42)
        //{
        //    try
        //    {
        //        QuickFix.FIX44.ResendRequest rr44 = new QuickFix.FIX44.ResendRequest();
        //        _fix42Header_2_Fix44Header(rr42.Header, rr44.Header);
        //        if (rr42.IsSetBeginSeqNo()) rr44.Set(new BeginSeqNo(rr42.BeginSeqNo.getValue()));
        //        if (rr42.IsSetEndSeqNo()) rr44.Set(new EndSeqNo(rr42.EndSeqNo.getValue()));
        //        return rr44;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("Fix42RR_2_Fix44RR(): Problemas na conversao da mensagem ResendRequest: " + ex.Message, ex);
        //        return null;
        //    }
        //}

        #endregion
    }
}
