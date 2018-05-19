using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickFix.Fields;
using QuickFix;

using log4net;

namespace Gradual.Core.OMS.FixServerLowLatency.Util
{
    public class Fix42Conversions
    {

        private static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static QuickFix.FIX42.ExecutionReport Fix42Rejection2ExecutionReport(QuickFix.Message msgReject, QuickFix.Message msgOri)
        {
            try
            {
                QuickFix.FIX42.ExecutionReport er = new QuickFix.FIX42.ExecutionReport();
                if (msgOri.IsSetField(Tags.OrderID))
                    er.SetField(new OrderID(msgOri.GetString(Tags.OrderID)));
                else
                    er.Set(new OrderID("NONE"));
                if (msgOri.IsSetField(Tags.ClOrdID)) er.Set(new ClOrdID(msgOri.GetField(Tags.ClOrdID)));
                if (msgOri.IsSetField(Tags.OrigClOrdID)) er.Set(new OrigClOrdID(msgOri.GetField(Tags.OrigClOrdID)));
                // Fazer atribuicao dos PartyIDs da mensagem original
                int len = msgOri.IsSetField(Tags.NoPartyIDs)? msgOri.GetInt(Tags.NoPartyIDs): 0;
                for (int i = 0; i < len; i++)
                {
                    Group grp = msgOri.GetGroup(i + 1, Tags.NoPartyIDs);
                    er.AddGroup(grp);
                }
                er.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                er.Set(new ExecType(ExecType.REJECTED));
                er.Set(new OrdStatus(OrdStatus.REJECTED));
                if (msgOri.IsSetField(Tags.Account)) er.Set(new Account(msgOri.GetField(Tags.Account)));
                er.Set(new Symbol(msgOri.GetField(Tags.Symbol)));
                if (msgOri.IsSetField(Tags.SecurityID)) er.Set(new SecurityID(msgOri.GetField(Tags.SecurityID)));
                //if (msgOri.IsSetField(Tags.SecurityIDSource)) er.Set(new SecurityIDSource(msgOri.GetField(Tags.SecurityIDSource)));
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
                if (msgReject.IsSetField(Tags.Text))
                    er.Set(new Text(msgReject.GetField(Tags.Text)));
                if (msgOri.IsSetField(Tags.Memo))
                    er.SetField(new Memo(msgOri.GetString(Tags.Memo)));

                // Validacao da tag ExecTransType. Para rejeicao, atribuindo default STATUS
                er.Set(new ExecTransType(ExecTransType.STATUS));

                return er;
            }
            catch (Exception ex)
            {
                logger.Error("Fix44Rejection2ExecutionReport(): " + ex.Message, ex);
                return null;
            }
        }

        public static QuickFix.FIX42.OrderCancelReject Fix42Reject2OrderCancelReject(QuickFix.Message msgReject, QuickFix.Message msgOri)
        {
            try
            {
                QuickFix.FIX42.OrderCancelReject ocr = new QuickFix.FIX42.OrderCancelReject();
                
                if (msgOri.IsSetField(Tags.OrderID)) 
                    ocr.Set(new OrderID(msgOri.GetString(Tags.OrderID)));
                else
                    ocr.Set(new OrderID("NONE"));
                if (msgOri.IsSetField(Tags.SecondaryOrderID)) ocr.Set(new SecondaryOrderID(msgOri.GetString(Tags.SecondaryOrderID)));
                if (msgOri.IsSetField(Tags.ClOrdID)) ocr.Set(new ClOrdID(msgOri.GetString(Tags.ClOrdID)));
                if (msgOri.IsSetField(Tags.OrigClOrdID)) ocr.Set(new OrigClOrdID(msgOri.GetString(Tags.OrigClOrdID)));
                ocr.Set(new OrdStatus(OrdStatus.REJECTED));
                // if (msgOri.IsSetField(Tags.ClientID)) ocr.Set(new ClientID(msgOri.GetString(Tags.ClientID))); // not found on 4.4EP
                // TODO[FF] - Verificar a montagem do execbroker a partir de party ids
                // if (msgOri.IsSetField(Tags.ExecBroker)) ocr.Set(new ExecBroker(msgOri.GetString(Tags.ExecBroker))); // not found on 4.4EP
                if (msgOri.IsSetField(Tags.ListID)) ocr.Set(new ListID(msgOri.GetString(Tags.ListID)));
                if (msgOri.IsSetField(Tags.Account)) ocr.Set(new Account(msgOri.GetString(Tags.Account)));
                if (msgOri.IsSetField(Tags.TransactTime)) ocr.Set(new TransactTime(msgOri.GetDateTime(Tags.TransactTime)));
                if (msgOri.Header.GetString(Tags.MsgType).Equals(MsgType.ORDER_CANCEL_REQUEST)) // mandatory
                    ocr.Set(new CxlRejResponseTo('1'));
                else
                    ocr.Set(new CxlRejResponseTo('2'));
                //if (msgOri.IsSetField(Tags.CxlRejReason)) ocr.Set(new CxlRejReason(msgOri.GetInt(Tags.CxlRejReason)));
                if (msgOri.IsSetField(Tags.CxlRejReason)) ocr.Set(new CxlRejReason(CxlRejReason.BROKER_OPTION));
                if (msgOri.IsSetField(Tags.Text)) ocr.Set(new Text(msgOri.GetString(Tags.Text)));
                if (msgOri.IsSetField(Tags.EncodedTextLen)) ocr.Set(new EncodedTextLen(msgOri.GetInt(Tags.EncodedTextLen)));
                if (msgOri.IsSetField(Tags.EncodedText)) ocr.Set(new EncodedText(msgOri.GetString(Tags.EncodedText)));
                if (msgOri.IsSetField(Tags.Memo)) ocr.SetField(new Memo(msgOri.GetString(Tags.Memo)));
                /*
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
                int len = msgOri.GetInt(Tags.NoPartyIDs);
                for (int i = 0; i < len; i++)
                {
                    Group grp = msgOri.GetGroup(i + 1, Tags.NoPartyIDs);
                    ocr.AddGroup(grp);
                }
                ocr.SetField(new Symbol(msgOri.GetString(Tags.Symbol)));
                if (msgOri.IsSetField(Tags.SecurityID)) ocr.SetField(new SecurityID(msgOri.GetField(Tags.SecurityID)));
                if (msgOri.IsSetField(Tags.SecurityIDSource)) ocr.SetField(new SecurityIDSource(msgOri.GetField(Tags.SecurityIDSource)));
                ocr.SetField(new Side(msgOri.GetChar(Tags.Side)));
                if (msgOri.IsSetField(Tags.Memo))
                    ocr.SetField(new Memo(msgOri.GetString(Tags.Memo)));
                 */
                return ocr;
            }
            catch (Exception ex)
            {
                logger.Error("Fix42Reject2OrderCancelReject(): " + ex.Message, ex);
                return null;
            }
        }


        public static QuickFix.FIX42.ExecutionReport Fix44ER_2_Fix42ER(QuickFix.FIX44.ExecutionReport er44)
        {
            try
            {
                QuickFix.FIX42.ExecutionReport er42 = new QuickFix.FIX42.ExecutionReport();
                // if (er44.IsSetApplID()) er42.Set(new ApplID(er44.ApplID.getValue()));
                // if (er44.IsSetApplSeqNum()) er42.Set(new ApplSeqNum(er44.ApplSeqNum.getValue()));
                if (er44.IsSetOrderID()) er42.Set(new OrderID(er44.OrderID.getValue()));
                if (er44.IsSetSecondaryOrderID()) er42.Set(new SecondaryOrderID(er44.SecondaryOrderID.getValue()));
                // if (er44.IsSetSecondaryExecID()) er42.Set(new SecondaryExecID(er44.SecondaryExecID.getValue()));
                if (er44.IsSetClOrdID()) er42.Set(new ClOrdID(er44.ClOrdID.getValue()));
                if (er44.IsSetOrigClOrdID()) er42.Set(new OrigClOrdID(er44.OrigClOrdID.getValue()));
                //if (er44.IsSetNoPartyIDs()) er42.Set(new NoPartyIDs(er44.NoPartyIDs.getValue()));
                //if (er44.IsSetPartyID()) er42.Set(new PartyID(er44.PartyID.getValue()));
                //if (er44.IsSetPartyIDSource()) er42.Set(new PartyIDSource(er44.PartyIDSource.getValue()));
                //if (er44.IsSetPartyRole()) er42.Set(new PartyRole(er44.PartyRole.getValue()));
                
                // TODO[FF] - Montagem dos grupos
                if (er44.IsSetNoContraBrokers())
                {
                    er42.Set(new NoContraBrokers(er44.NoContraBrokers.getValue()));
                    int len = er44.NoContraBrokers.getValue();
                    for (int i = 1; i <= len; i++)
                    {
                        Group grp = er44.GetGroup(i, Tags.NoContraBrokers);
                        er42.AddGroup(grp);
                    }
                }

                //if (er44.IsSetContra ContraBroker()) er42.Set(new ContraBroker(er44.ContraBroker.getValue()));
                // if (er44.IsSetCrossID()) er42.Set(new CrossID(er44.CrossID.getValue()));
                if (er44.IsSetExecID()) er42.Set(new ExecID(er44.ExecID.getValue()));
                if (er44.IsSetExecRefID()) er42.Set(new ExecRefID(er44.ExecRefID.getValue()));
                if (er44.IsSetExecType()) er42.Set(new ExecType(er44.ExecType.getValue()));
                if (er44.IsSetOrdStatus()) er42.Set(new OrdStatus(er44.OrdStatus.getValue()));
                // if (er44.IsSetWorkingIndicator()) er42.Set(new WorkingIndicator(er44.WorkingIndicator.getValue()));
                //if (er44.IsSetOrdRejReason()) er42.Set(new OrdRejReason(er44.OrdRejReason.getValue()));
                if (er44.IsSetOrdRejReason()) er42.Set(new OrdRejReason(99)); // Will always set "99" - other
                if (er44.IsSetExecRestatementReason()) er42.Set(new ExecRestatementReason(er44.ExecRestatementReason.getValue()));
                if (er44.IsSetAccount()) er42.Set(new Account(er44.Account.getValue()));
                // if (er44.IsSetAccountType()) er42.Set(new AccountType(er44.AccountType.getValue()));
                if (er44.IsSetSettlType()) er42.Set(new SettlmntTyp(er44.SettlType.getValue()[0]));
                // if (er44.IsSetDaysToSettlement()) er42.Set(new DaysToSettlement(er44.DaysToSettlement.getValue()));
                // if (er44.IsSetFixedRate()) er42.Set(new FixedRate(er44.FixedRate.getValue()));
                if (er44.IsSetSymbol()) er42.Set(new Symbol(er44.Symbol.getValue()));
                if (er44.IsSetSecurityID()) er42.Set(new SecurityID(er44.SecurityID.getValue()));
                // if (er44.IsSetSecurityIDSource()) er42.Set(new SecurityIDSource(er44.SecurityIDSource.getValue()));
                if (er44.IsSetSecurityExchange()) er42.Set(new SecurityExchange(er44.SecurityExchange.getValue()));
                if (er44.IsSetSide()) er42.Set(new Side(er44.Side.getValue()));
                if (er44.IsSetOrderQty()) er42.Set(new OrderQty(er44.OrderQty.getValue()));
                if (er44.IsSetOrdType()) er42.Set(new OrdType(er44.OrdType.getValue()));
                if (er44.IsSetPrice()) er42.Set(new Price(er44.Price.getValue()));
                if (er44.IsSetStopPx()) er42.Set(new StopPx(er44.StopPx.getValue()));
                // if (er44.IsSetProtectionPrice()) er42.Set(new ProtectionPrice(er44.ProtectionPrice.getValue()));
                if (er44.IsSetCurrency()) er42.Set(new Currency(er44.Currency.getValue()));
                if (er44.IsSetTimeInForce()) er42.Set(new TimeInForce(er44.TimeInForce.getValue()));
                if (er44.IsSetExpireDate()) er42.Set(new ExpireDate(er44.ExpireDate.getValue()));
                if (er44.IsSetLastQty()) er42.Set(new LastShares(er44.LastQty.getValue()));
                if (er44.IsSetLastPx()) er42.Set(new LastPx(er44.LastPx.getValue()));
                if (er44.IsSetLeavesQty()) er42.Set(new LeavesQty(er44.LeavesQty.getValue()));
                if (er44.IsSetCumQty()) er42.Set(new CumQty(er44.CumQty.getValue()));
                if (er44.IsSetAvgPx()) er42.Set(new AvgPx(er44.AvgPx.getValue()));
                if (er44.IsSetTradeDate()) er42.Set(new TradeDate(er44.TradeDate.getValue()));
                if (er44.IsSetTransactTime()) er42.Set(new TransactTime(er44.TransactTime.getValue()));
                // if (er44.IsSetAggressorIndicator()) er42.Set(new AggressorIndicator(er44.AggressorIndicator.getValue()));
                if (er44.IsSetMinQty()) er42.Set(new MinQty(er44.MinQty.getValue()));
                if (er44.IsSetMaxFloor()) er42.Set(new MaxFloor(er44.MaxFloor.getValue()));
                if (er44.IsSetText()) er42.Set(new Text(er44.Text.getValue()));
                if (er44.IsSetMultiLegReportingType()) er42.Set(new MultiLegReportingType(er44.MultiLegReportingType.getValue()));
                // if (er44.IsSetTotNoRelatedSym()) er42.Set(new TotNoRelatedSym(er44.TotNoRelatedSym.getValue()));
                // if (er44.IsSetNoMiscFees()) er42.Set(new NoMiscFees(er44.NoMiscFees.getValue()));
                // if (er44.IsSetMiscFeeAmt()) er42.Set(new MiscFeeAmt(er44.MiscFeeAmt.getValue()));
                // if (er44.IsSetMiscFeeCurr()) er42.Set(new MiscFeeCurr(er44.MiscFeeCurr.getValue()));
                // if (er44.IsSetMiscFeeType()) er42.Set(new MiscFeeType(er44.MiscFeeType.getValue()));
                // if (er44.IsSetMiscFeeBasis()) er42.Set(new MiscFeeBasis(er44.MiscFeeBasis.getValue()));
                // if (er44.IsSetUniqueTradeID()) er42.Set(new UniqueTradeID(er44.UniqueTradeID.getValue()));
                // if (er44.IsSetTradeLinkID()) er42.Set(new TradeLinkID(er44.TradeLinkID.getValue()));
                // if (er44.IsSetOrderLinkID()) er42.Set(new OrderLinkID(er44.OrderLinkID.getValue()));
                // if (er44.IsSetExchangeQuoteID()) er42.Set(new ExchangeQuoteID(er44.ExchangeQuoteID.getValue()));
                // if (er44.IsSetExchangeOrderID()) er42.Set(new ExchangeOrderID(er44.ExchangeOrderID.getValue()));
                // if (er44.IsSetExchangeExecID()) er42.Set(new ExchangeExecID(er44.ExchangeExecID.getValue()));
                // if (er44.IsSetLastPxInIssuedCurrency()) er42.Set(new LastPxInIssuedCurrency(er44.LastPxInIssuedCurrency.getValue()));
                // if (er44.IsSetPriceInIssuedCurrency()) er42.Set(new PriceInIssuedCurrency(er44.PriceInIssuedCurrency.getValue()));
                // if (er44.IsSetExchangeSecondaryOrderID()) er42.Set(new ExchangeSecondaryOrderID(er44.ExchangeSecondaryOrderID.getValue()));
                // if (er44.IsSetOrderCategory()) er42.Set(new OrderCategory(er44.OrderCategory.getValue()));
                if (er44.IsSetField(Tags.Memo)) er42.SetField(new Memo(er44.GetField(Tags.Memo)));

                // Validacao campo ExecTransType
                switch (er44.OrdStatus.getValue())
                {
                    case OrdStatus.NEW:
                        er42.Set(new ExecTransType(ExecTransType.NEW));
                        break;
                    case OrdStatus.CANCELED:
                    case OrdStatus.REPLACED: // TODO [FF] - Rever se o tratamento esta correto
                        er42.Set(new ExecTransType(ExecTransType.STATUS));
                        // er42.Set(new ExecRefID(er44.ExecRefID.getValue()));
                        break;
                    case OrdStatus.REJECTED:
                    case OrdStatus.FILLED:
                    case OrdStatus.PARTIALLY_FILLED:
                        er42.Set(new ExecTransType(ExecTransType.STATUS));
                        er42.Set(new ExecType(er44.OrdStatus.getValue())); // 4.2 nao aceita ExecType=F
                        break;
                    case OrdStatus.EXPIRED:
                        er42.Set(new ExecTransType(ExecTransType.STATUS));
                        er42.Set(new OrdStatus(OrdStatus.DONE_FOR_DAY));
                        er42.Set(new ExecType(ExecType.DONE_FOR_DAY));
                        break;

                }
                // Validacao campo 
                

                /*
                if (er44.IsSetAvgPx()) er42.Set(new AvgPx(er44.AvgPx.getValue()));
                if (er44.IsSetClOrdID()) er42.Set(new ClOrdID(er44.ClOrdID.getValue()));
                if (er44.IsSetCommission()) er42.Set(new Commission(er44.Commission.getValue()));
                if (er44.IsSetCommType()) er42.Set(new CommType(er44.CommType.getValue()));
                if (er44.IsSetCumQty()) er42.Set(new CumQty(er44.CumQty.getValue()));
                if (er44.IsSetCurrency()) er42.Set(new Currency(er44.Currency.getValue()));
                if (er44.IsSetExecID()) er42.Set(new ExecID(er44.ExecID.getValue()));
                if (er44.IsSetExecRefID()) er42.Set(new ExecRefID(er44.ExecRefID.getValue()));

                //if (er44.IsSetExecTransType()) er42.Set(new ExecTransType(er44.ExecTransType.getValue())); // TODO[FF] - Verificar o gerenciamento deste campo
                if (er44.IsSetExecType()) er42.Set(new ExecType(er44.ExecType.getValue()));
                // if (er44.IsSetIDSource()) er42.Set(new IDSource(er44.IDSource.getValue())); // TODO[FF] - Verificar o gerenciamento deste campo
                if (er44.IsSetLastPx()) er42.Set(new LastPx(er44.LastPx.getValue()));
                // if (er44.IsSetLastShares()) er42.Set(new LastShares(er44.LastShares.getValue())); // TODO[FF] - Verificar o gerenciamento deste campo
                if (er44.IsSetLeavesQty()) er42.Set(new LeavesQty(er44.LeavesQty.getValue()));
                //if (er44.IsSetNoMiscFees()) er42.Set(new NoMiscFees(er44.NoMiscFees.getValue())); // TODO[FF] - Verificar o gerenciamento deste campo por ser group
                //if (er44.IsSetMiscFeeAmt()) er42.Set(new MiscFeeAmt(er44.MiscFeeAmt.getValue())); // TODO[FF] - Verificar o gerenciamento deste campo por ser elemento do group
                if (er44.IsSetOrderID()) er42.Set(new OrderID(er44.OrderID.getValue()));
                if (er44.IsSetOrderQty()) er42.Set(new OrderQty(er44.OrderQty.getValue()));
                if (er44.IsSetOrdStatus()) er42.Set(new OrdStatus(er44.OrdStatus.getValue()));
                if (er44.IsSetOrdType()) er42.Set(new OrdType(er44.OrdType.getValue()));
                if (er44.IsSetOrigClOrdID()) er42.Set(new OrigClOrdID(er44.OrigClOrdID.getValue()));
                if (er44.IsSetSecurityID()) er42.Set(new SecurityID(er44.SecurityID.getValue()));
                if (er44.IsSetSide()) er42.Set(new Side(er44.Side.getValue()));
                if (er44.IsSetSymbol()) er42.Set(new Symbol(er44.Symbol.getValue()));
                if (er44.IsSetTransactTime()) er42.Set(new TransactTime(er44.TransactTime.getValue()));
                if (er44.IsSetSecurityType()) er42.Set(new SecurityType(er44.SecurityType.getValue()));
                if (er44.IsSetSecurityExchange()) er42.Set(new SecurityExchange(er44.SecurityExchange.getValue()));
                */
                return er42;
            }
            catch (Exception ex)
            {
                logger.Error("Fix44ER_2_Fix42ER(): " + ex.Message, ex);
                return null;
            }
        }


        public static QuickFix.FIX42.OrderCancelReject Fix44OCR_2_Fix42OCR(QuickFix.FIX44.OrderCancelReject ocrj44)
        {
            try
            {
                // TODO [FF] - Efetuar a conversao do Fix44 para Fix42 de OrderCancelReject
                QuickFix.FIX42.OrderCancelReject ocrj42 = new QuickFix.FIX42.OrderCancelReject();
                
                if (ocrj44.IsSetOrderID()) ocrj42.Set(new OrderID(ocrj44.OrderID.getValue()));
                if (ocrj44.IsSetSecondaryOrderID()) ocrj42.Set(new SecondaryOrderID(ocrj44.SecondaryOrderID.getValue()));
                // if (ocrj44.IsSetSecondaryClOrdID()) ocrj42.Set(new SecondaryClOrdID(ocrj44.SecondaryClOrdID.getValue())); // Not Found on 4.2
                if (ocrj44.IsSetClOrdID()) ocrj42.Set(new ClOrdID(ocrj44.ClOrdID.getValue()));
                // if (ocrj44.IsSetClOrdLinkID()) ocrj42.Set(new ClOrdLinkID(ocrj44.ClOrdLinkID.getValue())); // Not Found on 4.2
                if (ocrj44.IsSetOrigClOrdID()) ocrj42.Set(new OrigClOrdID(ocrj44.OrigClOrdID.getValue()));
                if (ocrj44.IsSetOrdStatus()) ocrj42.Set(new OrdStatus(ocrj44.OrdStatus.getValue()));
                // if (ocrj44.IsSetWorkingIndicator()) ocrj42.Set(new WorkingIndicator(ocrj44.WorkingIndicator.getValue())); // Not found on 4.2
                // if (ocrj44.IsSetOrigOrdModTime()) ocrj42.Set(new OrigOrdModTime(ocrj44.OrigOrdModTime.getValue())); // Not found on 4.2
                if (ocrj44.IsSetListID()) ocrj42.Set(new ListID(ocrj44.ListID.getValue()));
                if (ocrj44.IsSetAccount()) ocrj42.Set(new Account(ocrj44.Account.getValue()));
                // if (ocrj44.IsSetAcctIDSource()) ocrj42.Set(new AcctIDSource(ocrj44.AcctIDSource.getValue())); // Not found on 4.2
                // if (ocrj44.IsSetAccountType()) ocrj42.Set(new AccountType(ocrj44.AccountType.getValue())); // Not found on 4.2
                // if (ocrj44.IsSetTradeOriginationDate()) ocrj42.Set(new TradeOriginationDate(ocrj44.TradeOriginationDate.getValue())); // Not found on 4.2
                // if (ocrj44.IsSetTradeDate()) ocrj42.Set(new TradeDate(ocrj44.TradeDate.getValue())); // Not found on 4.2
                if (ocrj44.IsSetTransactTime()) ocrj42.Set(new TransactTime(ocrj44.TransactTime.getValue()));
                if (ocrj44.IsSetCxlRejResponseTo()) ocrj42.Set(new CxlRejResponseTo(ocrj44.CxlRejResponseTo.getValue()));
                // if (ocrj44.IsSetCxlRejReason()) ocrj42.Set(new CxlRejReason(ocrj44.CxlRejReason.getValue()));
                // TODO [FF]: Verify value pattern: Set to "2": BROKER OPTION and describe rejection in text field
                if (ocrj44.IsSetCxlRejReason()) ocrj42.Set(new CxlRejReason(CxlRejReason.BROKER_OPTION)); 
                if (ocrj44.IsSetText()) ocrj42.Set(new Text(ocrj44.Text.getValue()));
                if (ocrj44.IsSetEncodedTextLen()) ocrj42.Set(new EncodedTextLen(ocrj44.EncodedTextLen.getValue()));
                if (ocrj44.IsSetEncodedText()) ocrj42.Set(new EncodedText(ocrj44.EncodedText.getValue()));
                if (ocrj44.IsSetMemo()) ocrj42.SetField(new Memo(ocrj44.Memo.getValue()));			
                
                return ocrj42;
            }
            catch (Exception ex)
            {
                logger.Error("Fix44OCR_2_Fix42OCR(): " + ex.Message, ex);
                return null;
            }
        }
    }
}
