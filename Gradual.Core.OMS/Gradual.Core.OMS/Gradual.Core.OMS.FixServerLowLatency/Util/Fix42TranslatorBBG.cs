using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;
using QuickFix.Fields;
using QuickFix;
using Gradual.Core.OMS.FixServerLowLatency.Rede;

namespace Gradual.Core.OMS.FixServerLowLatency.Util
{
    public class Fix42TranslatorBBG
    {
        #region log4net
        private static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion


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
                int len = msgOri.IsSetField(Tags.NoPartyIDs) ? msgOri.GetInt(Tags.NoPartyIDs) : 0;
                
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
                bool retTxt = false;
                if (msgOri.IsSetField(Tags.Text))
                {
                    ocr.Set(new Text(msgOri.GetString(Tags.Text)));
                    retTxt = true;
                }
                if (!retTxt)
                {
                    if (msgReject.IsSetField(Tags.Text))
                        ocr.Set(new Text(msgReject.GetString(Tags.Text)));
                }
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
                //if (er44.IsSetExecRestatementReason()) er42.Set(new ExecRestatementReason(er44.ExecRestatementReason.getValue()));
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
                    case OrdStatus.REPLACED:
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
                // Validacao do campo ExecType, em caso de cancelamento de trade
                // TODO [FF] - Verificar necessidade desta implementacao (validar os status de ExecType e OrdStatus)
                // parece que não é necessario. A validacao acima ja suportaria.
                switch (er44.ExecType.getValue())
                {
                    case ExecType.TRADE_CANCEL:
                        {
                            // Partial Fill - returns the partial fill status
                            if (er44.OrdStatus.getValue() == OrdStatus.PARTIALLY_FILLED)
                            {
                                er42.SetField(new ExecTransType(ExecTransType.STATUS), true);
                                er42.SetField(new ExecType(er44.OrdStatus.getValue()), true); // 4.2 nao aceita ExecType=F
                            }
                            // Full Fill - returns a reject
                            else
                            {
                                er42.SetField(new ExecTransType(ExecTransType.STATUS), true);
                                er42.SetField(new OrdStatus(OrdStatus.REJECTED), true);
                                er42.SetField(new ExecType(ExecType.REJECTED), true);
                                er42.SetField(new Text("Trade Cancel sent from BMF/Bovespa"));
                            }
                        }
                        break;
                }

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

        public static void Generate42RejectMessage(QuickFix.Message msg, SessionID s, string msgType, Exception ex, SessionAcceptor ssAcceptor, string msgText)
        {
            try
            {
                if (null != ex)
                {
                    string aux = string.Format("QuickFix42 MsgType: [{0}], Message: [{1}]", msgType, ex.Message);
                    logger.Error(aux, ex);
                }
                QuickFix.FIX42.Reject rej = new QuickFix.FIX42.Reject();
                rej.Set(new RefMsgType(msgType));
                rej.Set(new RefSeqNum(msg.Header.GetInt(Tags.MsgSeqNum)));
                if (string.IsNullOrEmpty(msgText))
                    rej.Set(new Text("System unavaliable"));
                else
                    rej.Set(new Text(msgText));

                if (msgType.Equals(MsgType.ORDER_CANCEL_REQUEST) || msgType.Equals(MsgType.ORDER_CANCEL_REPLACE_REQUEST))
                {
                    QuickFix.FIX42.OrderCancelReject ocr = Fix42TranslatorBBG.Fix42Reject2OrderCancelReject(rej, msg);
                    Session.SendToTarget(ocr, s);
                    if (null != ssAcceptor) ssAcceptor.Send2DropCopy(ocr);
                }
                else
                {
                    QuickFix.FIX42.ExecutionReport er = Fix42TranslatorBBG.Fix42Rejection2ExecutionReport(rej, msg);
                    Session.SendToTarget(er, s);
                    if (null != ssAcceptor) ssAcceptor.Send2DropCopy(er);
                }
            }
            catch (Exception exC)
            {
                logger.Error("Problemas na geracao de mensagem de reject (tratamento de excecoes) de mensagem fix 4.2: " + exC.Message, exC);
            }
        }


        #region Conversions to Fix44 Messages


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
                if (nos42.IsSetHandlInst()) nos44.Set(new HandlInst(nos42.HandlInst.getValue()));
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
                if (ocrr42.IsSetHandlInst()) ocrr44.Set(new HandlInst(ocrr42.HandlInst.getValue()));
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



        

        #endregion



    }

    

}
