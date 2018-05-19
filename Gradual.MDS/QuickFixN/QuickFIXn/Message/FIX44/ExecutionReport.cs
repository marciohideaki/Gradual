// This is a generated file.  Don't edit it directly!

using QuickFix.Fields;
namespace QuickFix
{
    namespace FIX44 
    {
        public class ExecutionReport : Message
        {
            public const string MsgType = "8";

            public ExecutionReport() : base()
            {
                this.Header.SetField(new QuickFix.Fields.MsgType("8"));
            }

            public ExecutionReport(
                    QuickFix.Fields.OrderID aOrderID,
                    QuickFix.Fields.ExecID aExecID,
                    QuickFix.Fields.ExecType aExecType,
                    QuickFix.Fields.OrdStatus aOrdStatus,
                    QuickFix.Fields.Symbol aSymbol,
                    QuickFix.Fields.SecurityID aSecurityID,
                    QuickFix.Fields.SecurityIDSource aSecurityIDSource,
                    QuickFix.Fields.Side aSide,
                    QuickFix.Fields.LeavesQty aLeavesQty,
                    QuickFix.Fields.CumQty aCumQty,
                    QuickFix.Fields.AvgPx aAvgPx
                ) : this()
            {
                this.OrderID = aOrderID;
                this.ExecID = aExecID;
                this.ExecType = aExecType;
                this.OrdStatus = aOrdStatus;
                this.Symbol = aSymbol;
                this.SecurityID = aSecurityID;
                this.SecurityIDSource = aSecurityIDSource;
                this.Side = aSide;
                this.LeavesQty = aLeavesQty;
                this.CumQty = aCumQty;
                this.AvgPx = aAvgPx;
            }

            public QuickFix.Fields.ApplID ApplID
            { 
                get 
                {
                    QuickFix.Fields.ApplID val = new QuickFix.Fields.ApplID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ApplID val) 
            { 
                this.ApplID = val;
            }
            
            public QuickFix.Fields.ApplID Get(QuickFix.Fields.ApplID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ApplID val) 
            { 
                return IsSetApplID();
            }
            
            public bool IsSetApplID() 
            { 
                return IsSetField(Tags.ApplID);
            }
            public QuickFix.Fields.ApplSeqNum ApplSeqNum
            { 
                get 
                {
                    QuickFix.Fields.ApplSeqNum val = new QuickFix.Fields.ApplSeqNum();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ApplSeqNum val) 
            { 
                this.ApplSeqNum = val;
            }
            
            public QuickFix.Fields.ApplSeqNum Get(QuickFix.Fields.ApplSeqNum val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ApplSeqNum val) 
            { 
                return IsSetApplSeqNum();
            }
            
            public bool IsSetApplSeqNum() 
            { 
                return IsSetField(Tags.ApplSeqNum);
            }
            public QuickFix.Fields.ApplLastSeqNum ApplLastSeqNum
            { 
                get 
                {
                    QuickFix.Fields.ApplLastSeqNum val = new QuickFix.Fields.ApplLastSeqNum();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ApplLastSeqNum val) 
            { 
                this.ApplLastSeqNum = val;
            }
            
            public QuickFix.Fields.ApplLastSeqNum Get(QuickFix.Fields.ApplLastSeqNum val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ApplLastSeqNum val) 
            { 
                return IsSetApplLastSeqNum();
            }
            
            public bool IsSetApplLastSeqNum() 
            { 
                return IsSetField(Tags.ApplLastSeqNum);
            }
            public QuickFix.Fields.ApplResendFlag ApplResendFlag
            { 
                get 
                {
                    QuickFix.Fields.ApplResendFlag val = new QuickFix.Fields.ApplResendFlag();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ApplResendFlag val) 
            { 
                this.ApplResendFlag = val;
            }
            
            public QuickFix.Fields.ApplResendFlag Get(QuickFix.Fields.ApplResendFlag val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ApplResendFlag val) 
            { 
                return IsSetApplResendFlag();
            }
            
            public bool IsSetApplResendFlag() 
            { 
                return IsSetField(Tags.ApplResendFlag);
            }
            public QuickFix.Fields.OrderID OrderID
            { 
                get 
                {
                    QuickFix.Fields.OrderID val = new QuickFix.Fields.OrderID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.OrderID val) 
            { 
                this.OrderID = val;
            }
            
            public QuickFix.Fields.OrderID Get(QuickFix.Fields.OrderID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.OrderID val) 
            { 
                return IsSetOrderID();
            }
            
            public bool IsSetOrderID() 
            { 
                return IsSetField(Tags.OrderID);
            }
            public QuickFix.Fields.SecondaryOrderID SecondaryOrderID
            { 
                get 
                {
                    QuickFix.Fields.SecondaryOrderID val = new QuickFix.Fields.SecondaryOrderID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.SecondaryOrderID val) 
            { 
                this.SecondaryOrderID = val;
            }
            
            public QuickFix.Fields.SecondaryOrderID Get(QuickFix.Fields.SecondaryOrderID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.SecondaryOrderID val) 
            { 
                return IsSetSecondaryOrderID();
            }
            
            public bool IsSetSecondaryOrderID() 
            { 
                return IsSetField(Tags.SecondaryOrderID);
            }
            public QuickFix.Fields.SecondaryClOrdID SecondaryClOrdID
            { 
                get 
                {
                    QuickFix.Fields.SecondaryClOrdID val = new QuickFix.Fields.SecondaryClOrdID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.SecondaryClOrdID val) 
            { 
                this.SecondaryClOrdID = val;
            }
            
            public QuickFix.Fields.SecondaryClOrdID Get(QuickFix.Fields.SecondaryClOrdID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.SecondaryClOrdID val) 
            { 
                return IsSetSecondaryClOrdID();
            }
            
            public bool IsSetSecondaryClOrdID() 
            { 
                return IsSetField(Tags.SecondaryClOrdID);
            }
            public QuickFix.Fields.SecondaryExecID SecondaryExecID
            { 
                get 
                {
                    QuickFix.Fields.SecondaryExecID val = new QuickFix.Fields.SecondaryExecID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.SecondaryExecID val) 
            { 
                this.SecondaryExecID = val;
            }
            
            public QuickFix.Fields.SecondaryExecID Get(QuickFix.Fields.SecondaryExecID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.SecondaryExecID val) 
            { 
                return IsSetSecondaryExecID();
            }
            
            public bool IsSetSecondaryExecID() 
            { 
                return IsSetField(Tags.SecondaryExecID);
            }
            public QuickFix.Fields.OrigSecondaryOrderID OrigSecondaryOrderID
            { 
                get 
                {
                    QuickFix.Fields.OrigSecondaryOrderID val = new QuickFix.Fields.OrigSecondaryOrderID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.OrigSecondaryOrderID val) 
            { 
                this.OrigSecondaryOrderID = val;
            }
            
            public QuickFix.Fields.OrigSecondaryOrderID Get(QuickFix.Fields.OrigSecondaryOrderID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.OrigSecondaryOrderID val) 
            { 
                return IsSetOrigSecondaryOrderID();
            }
            
            public bool IsSetOrigSecondaryOrderID() 
            { 
                return IsSetField(Tags.OrigSecondaryOrderID);
            }
            public QuickFix.Fields.ClOrdID ClOrdID
            { 
                get 
                {
                    QuickFix.Fields.ClOrdID val = new QuickFix.Fields.ClOrdID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ClOrdID val) 
            { 
                this.ClOrdID = val;
            }
            
            public QuickFix.Fields.ClOrdID Get(QuickFix.Fields.ClOrdID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ClOrdID val) 
            { 
                return IsSetClOrdID();
            }
            
            public bool IsSetClOrdID() 
            { 
                return IsSetField(Tags.ClOrdID);
            }
            public QuickFix.Fields.OrigClOrdID OrigClOrdID
            { 
                get 
                {
                    QuickFix.Fields.OrigClOrdID val = new QuickFix.Fields.OrigClOrdID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.OrigClOrdID val) 
            { 
                this.OrigClOrdID = val;
            }
            
            public QuickFix.Fields.OrigClOrdID Get(QuickFix.Fields.OrigClOrdID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.OrigClOrdID val) 
            { 
                return IsSetOrigClOrdID();
            }
            
            public bool IsSetOrigClOrdID() 
            { 
                return IsSetField(Tags.OrigClOrdID);
            }
            public QuickFix.Fields.ClOrdLinkID ClOrdLinkID
            { 
                get 
                {
                    QuickFix.Fields.ClOrdLinkID val = new QuickFix.Fields.ClOrdLinkID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ClOrdLinkID val) 
            { 
                this.ClOrdLinkID = val;
            }
            
            public QuickFix.Fields.ClOrdLinkID Get(QuickFix.Fields.ClOrdLinkID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ClOrdLinkID val) 
            { 
                return IsSetClOrdLinkID();
            }
            
            public bool IsSetClOrdLinkID() 
            { 
                return IsSetField(Tags.ClOrdLinkID);
            }
            public QuickFix.Fields.QuoteRespID QuoteRespID
            { 
                get 
                {
                    QuickFix.Fields.QuoteRespID val = new QuickFix.Fields.QuoteRespID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.QuoteRespID val) 
            { 
                this.QuoteRespID = val;
            }
            
            public QuickFix.Fields.QuoteRespID Get(QuickFix.Fields.QuoteRespID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.QuoteRespID val) 
            { 
                return IsSetQuoteRespID();
            }
            
            public bool IsSetQuoteRespID() 
            { 
                return IsSetField(Tags.QuoteRespID);
            }
            public QuickFix.Fields.OrdStatusReqID OrdStatusReqID
            { 
                get 
                {
                    QuickFix.Fields.OrdStatusReqID val = new QuickFix.Fields.OrdStatusReqID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.OrdStatusReqID val) 
            { 
                this.OrdStatusReqID = val;
            }
            
            public QuickFix.Fields.OrdStatusReqID Get(QuickFix.Fields.OrdStatusReqID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.OrdStatusReqID val) 
            { 
                return IsSetOrdStatusReqID();
            }
            
            public bool IsSetOrdStatusReqID() 
            { 
                return IsSetField(Tags.OrdStatusReqID);
            }
            public QuickFix.Fields.MassStatusReqID MassStatusReqID
            { 
                get 
                {
                    QuickFix.Fields.MassStatusReqID val = new QuickFix.Fields.MassStatusReqID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.MassStatusReqID val) 
            { 
                this.MassStatusReqID = val;
            }
            
            public QuickFix.Fields.MassStatusReqID Get(QuickFix.Fields.MassStatusReqID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.MassStatusReqID val) 
            { 
                return IsSetMassStatusReqID();
            }
            
            public bool IsSetMassStatusReqID() 
            { 
                return IsSetField(Tags.MassStatusReqID);
            }
            public QuickFix.Fields.TotNumReports TotNumReports
            { 
                get 
                {
                    QuickFix.Fields.TotNumReports val = new QuickFix.Fields.TotNumReports();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TotNumReports val) 
            { 
                this.TotNumReports = val;
            }
            
            public QuickFix.Fields.TotNumReports Get(QuickFix.Fields.TotNumReports val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TotNumReports val) 
            { 
                return IsSetTotNumReports();
            }
            
            public bool IsSetTotNumReports() 
            { 
                return IsSetField(Tags.TotNumReports);
            }
            public QuickFix.Fields.LastRptRequested LastRptRequested
            { 
                get 
                {
                    QuickFix.Fields.LastRptRequested val = new QuickFix.Fields.LastRptRequested();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.LastRptRequested val) 
            { 
                this.LastRptRequested = val;
            }
            
            public QuickFix.Fields.LastRptRequested Get(QuickFix.Fields.LastRptRequested val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.LastRptRequested val) 
            { 
                return IsSetLastRptRequested();
            }
            
            public bool IsSetLastRptRequested() 
            { 
                return IsSetField(Tags.LastRptRequested);
            }
            public QuickFix.Fields.NoPartyIDs NoPartyIDs
            { 
                get 
                {
                    QuickFix.Fields.NoPartyIDs val = new QuickFix.Fields.NoPartyIDs();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoPartyIDs val) 
            { 
                this.NoPartyIDs = val;
            }
            
            public QuickFix.Fields.NoPartyIDs Get(QuickFix.Fields.NoPartyIDs val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoPartyIDs val) 
            { 
                return IsSetNoPartyIDs();
            }
            
            public bool IsSetNoPartyIDs() 
            { 
                return IsSetField(Tags.NoPartyIDs);
            }
            public QuickFix.Fields.TradeOriginationDate TradeOriginationDate
            { 
                get 
                {
                    QuickFix.Fields.TradeOriginationDate val = new QuickFix.Fields.TradeOriginationDate();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TradeOriginationDate val) 
            { 
                this.TradeOriginationDate = val;
            }
            
            public QuickFix.Fields.TradeOriginationDate Get(QuickFix.Fields.TradeOriginationDate val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TradeOriginationDate val) 
            { 
                return IsSetTradeOriginationDate();
            }
            
            public bool IsSetTradeOriginationDate() 
            { 
                return IsSetField(Tags.TradeOriginationDate);
            }
            public QuickFix.Fields.NoContraBrokers NoContraBrokers
            { 
                get 
                {
                    QuickFix.Fields.NoContraBrokers val = new QuickFix.Fields.NoContraBrokers();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoContraBrokers val) 
            { 
                this.NoContraBrokers = val;
            }
            
            public QuickFix.Fields.NoContraBrokers Get(QuickFix.Fields.NoContraBrokers val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoContraBrokers val) 
            { 
                return IsSetNoContraBrokers();
            }
            
            public bool IsSetNoContraBrokers() 
            { 
                return IsSetField(Tags.NoContraBrokers);
            }
            public QuickFix.Fields.ListID ListID
            { 
                get 
                {
                    QuickFix.Fields.ListID val = new QuickFix.Fields.ListID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ListID val) 
            { 
                this.ListID = val;
            }
            
            public QuickFix.Fields.ListID Get(QuickFix.Fields.ListID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ListID val) 
            { 
                return IsSetListID();
            }
            
            public bool IsSetListID() 
            { 
                return IsSetField(Tags.ListID);
            }
            public QuickFix.Fields.CrossID CrossID
            { 
                get 
                {
                    QuickFix.Fields.CrossID val = new QuickFix.Fields.CrossID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.CrossID val) 
            { 
                this.CrossID = val;
            }
            
            public QuickFix.Fields.CrossID Get(QuickFix.Fields.CrossID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.CrossID val) 
            { 
                return IsSetCrossID();
            }
            
            public bool IsSetCrossID() 
            { 
                return IsSetField(Tags.CrossID);
            }
            public QuickFix.Fields.OrigCrossID OrigCrossID
            { 
                get 
                {
                    QuickFix.Fields.OrigCrossID val = new QuickFix.Fields.OrigCrossID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.OrigCrossID val) 
            { 
                this.OrigCrossID = val;
            }
            
            public QuickFix.Fields.OrigCrossID Get(QuickFix.Fields.OrigCrossID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.OrigCrossID val) 
            { 
                return IsSetOrigCrossID();
            }
            
            public bool IsSetOrigCrossID() 
            { 
                return IsSetField(Tags.OrigCrossID);
            }
            public QuickFix.Fields.CrossType CrossType
            { 
                get 
                {
                    QuickFix.Fields.CrossType val = new QuickFix.Fields.CrossType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.CrossType val) 
            { 
                this.CrossType = val;
            }
            
            public QuickFix.Fields.CrossType Get(QuickFix.Fields.CrossType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.CrossType val) 
            { 
                return IsSetCrossType();
            }
            
            public bool IsSetCrossType() 
            { 
                return IsSetField(Tags.CrossType);
            }
            public QuickFix.Fields.ExecID ExecID
            { 
                get 
                {
                    QuickFix.Fields.ExecID val = new QuickFix.Fields.ExecID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ExecID val) 
            { 
                this.ExecID = val;
            }
            
            public QuickFix.Fields.ExecID Get(QuickFix.Fields.ExecID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ExecID val) 
            { 
                return IsSetExecID();
            }
            
            public bool IsSetExecID() 
            { 
                return IsSetField(Tags.ExecID);
            }
            public QuickFix.Fields.ExecRefID ExecRefID
            { 
                get 
                {
                    QuickFix.Fields.ExecRefID val = new QuickFix.Fields.ExecRefID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ExecRefID val) 
            { 
                this.ExecRefID = val;
            }
            
            public QuickFix.Fields.ExecRefID Get(QuickFix.Fields.ExecRefID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ExecRefID val) 
            { 
                return IsSetExecRefID();
            }
            
            public bool IsSetExecRefID() 
            { 
                return IsSetField(Tags.ExecRefID);
            }
            public QuickFix.Fields.ExecType ExecType
            { 
                get 
                {
                    QuickFix.Fields.ExecType val = new QuickFix.Fields.ExecType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ExecType val) 
            { 
                this.ExecType = val;
            }
            
            public QuickFix.Fields.ExecType Get(QuickFix.Fields.ExecType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ExecType val) 
            { 
                return IsSetExecType();
            }
            
            public bool IsSetExecType() 
            { 
                return IsSetField(Tags.ExecType);
            }
            public QuickFix.Fields.OrdStatus OrdStatus
            { 
                get 
                {
                    QuickFix.Fields.OrdStatus val = new QuickFix.Fields.OrdStatus();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.OrdStatus val) 
            { 
                this.OrdStatus = val;
            }
            
            public QuickFix.Fields.OrdStatus Get(QuickFix.Fields.OrdStatus val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.OrdStatus val) 
            { 
                return IsSetOrdStatus();
            }
            
            public bool IsSetOrdStatus() 
            { 
                return IsSetField(Tags.OrdStatus);
            }
            public QuickFix.Fields.BTSFinalTxOrdStatus BTSFinalTxOrdStatus
            { 
                get 
                {
                    QuickFix.Fields.BTSFinalTxOrdStatus val = new QuickFix.Fields.BTSFinalTxOrdStatus();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.BTSFinalTxOrdStatus val) 
            { 
                this.BTSFinalTxOrdStatus = val;
            }
            
            public QuickFix.Fields.BTSFinalTxOrdStatus Get(QuickFix.Fields.BTSFinalTxOrdStatus val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.BTSFinalTxOrdStatus val) 
            { 
                return IsSetBTSFinalTxOrdStatus();
            }
            
            public bool IsSetBTSFinalTxOrdStatus() 
            { 
                return IsSetField(Tags.BTSFinalTxOrdStatus);
            }
            public QuickFix.Fields.WorkingIndicator WorkingIndicator
            { 
                get 
                {
                    QuickFix.Fields.WorkingIndicator val = new QuickFix.Fields.WorkingIndicator();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.WorkingIndicator val) 
            { 
                this.WorkingIndicator = val;
            }
            
            public QuickFix.Fields.WorkingIndicator Get(QuickFix.Fields.WorkingIndicator val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.WorkingIndicator val) 
            { 
                return IsSetWorkingIndicator();
            }
            
            public bool IsSetWorkingIndicator() 
            { 
                return IsSetField(Tags.WorkingIndicator);
            }
            public QuickFix.Fields.OrdRejReason OrdRejReason
            { 
                get 
                {
                    QuickFix.Fields.OrdRejReason val = new QuickFix.Fields.OrdRejReason();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.OrdRejReason val) 
            { 
                this.OrdRejReason = val;
            }
            
            public QuickFix.Fields.OrdRejReason Get(QuickFix.Fields.OrdRejReason val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.OrdRejReason val) 
            { 
                return IsSetOrdRejReason();
            }
            
            public bool IsSetOrdRejReason() 
            { 
                return IsSetField(Tags.OrdRejReason);
            }
            public QuickFix.Fields.ExecRestatementReason ExecRestatementReason
            { 
                get 
                {
                    QuickFix.Fields.ExecRestatementReason val = new QuickFix.Fields.ExecRestatementReason();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ExecRestatementReason val) 
            { 
                this.ExecRestatementReason = val;
            }
            
            public QuickFix.Fields.ExecRestatementReason Get(QuickFix.Fields.ExecRestatementReason val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ExecRestatementReason val) 
            { 
                return IsSetExecRestatementReason();
            }
            
            public bool IsSetExecRestatementReason() 
            { 
                return IsSetField(Tags.ExecRestatementReason);
            }
            public QuickFix.Fields.Account Account
            { 
                get 
                {
                    QuickFix.Fields.Account val = new QuickFix.Fields.Account();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.Account val) 
            { 
                this.Account = val;
            }
            
            public QuickFix.Fields.Account Get(QuickFix.Fields.Account val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.Account val) 
            { 
                return IsSetAccount();
            }
            
            public bool IsSetAccount() 
            { 
                return IsSetField(Tags.Account);
            }
            public QuickFix.Fields.AcctIDSource AcctIDSource
            { 
                get 
                {
                    QuickFix.Fields.AcctIDSource val = new QuickFix.Fields.AcctIDSource();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AcctIDSource val) 
            { 
                this.AcctIDSource = val;
            }
            
            public QuickFix.Fields.AcctIDSource Get(QuickFix.Fields.AcctIDSource val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AcctIDSource val) 
            { 
                return IsSetAcctIDSource();
            }
            
            public bool IsSetAcctIDSource() 
            { 
                return IsSetField(Tags.AcctIDSource);
            }
            public QuickFix.Fields.AccountType AccountType
            { 
                get 
                {
                    QuickFix.Fields.AccountType val = new QuickFix.Fields.AccountType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AccountType val) 
            { 
                this.AccountType = val;
            }
            
            public QuickFix.Fields.AccountType Get(QuickFix.Fields.AccountType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AccountType val) 
            { 
                return IsSetAccountType();
            }
            
            public bool IsSetAccountType() 
            { 
                return IsSetField(Tags.AccountType);
            }
            public QuickFix.Fields.DayBookingInst DayBookingInst
            { 
                get 
                {
                    QuickFix.Fields.DayBookingInst val = new QuickFix.Fields.DayBookingInst();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.DayBookingInst val) 
            { 
                this.DayBookingInst = val;
            }
            
            public QuickFix.Fields.DayBookingInst Get(QuickFix.Fields.DayBookingInst val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.DayBookingInst val) 
            { 
                return IsSetDayBookingInst();
            }
            
            public bool IsSetDayBookingInst() 
            { 
                return IsSetField(Tags.DayBookingInst);
            }
            public QuickFix.Fields.BookingUnit BookingUnit
            { 
                get 
                {
                    QuickFix.Fields.BookingUnit val = new QuickFix.Fields.BookingUnit();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.BookingUnit val) 
            { 
                this.BookingUnit = val;
            }
            
            public QuickFix.Fields.BookingUnit Get(QuickFix.Fields.BookingUnit val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.BookingUnit val) 
            { 
                return IsSetBookingUnit();
            }
            
            public bool IsSetBookingUnit() 
            { 
                return IsSetField(Tags.BookingUnit);
            }
            public QuickFix.Fields.TradeAllocIndicator TradeAllocIndicator
            { 
                get 
                {
                    QuickFix.Fields.TradeAllocIndicator val = new QuickFix.Fields.TradeAllocIndicator();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TradeAllocIndicator val) 
            { 
                this.TradeAllocIndicator = val;
            }
            
            public QuickFix.Fields.TradeAllocIndicator Get(QuickFix.Fields.TradeAllocIndicator val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TradeAllocIndicator val) 
            { 
                return IsSetTradeAllocIndicator();
            }
            
            public bool IsSetTradeAllocIndicator() 
            { 
                return IsSetField(Tags.TradeAllocIndicator);
            }
            public QuickFix.Fields.PreallocMethod PreallocMethod
            { 
                get 
                {
                    QuickFix.Fields.PreallocMethod val = new QuickFix.Fields.PreallocMethod();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.PreallocMethod val) 
            { 
                this.PreallocMethod = val;
            }
            
            public QuickFix.Fields.PreallocMethod Get(QuickFix.Fields.PreallocMethod val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.PreallocMethod val) 
            { 
                return IsSetPreallocMethod();
            }
            
            public bool IsSetPreallocMethod() 
            { 
                return IsSetField(Tags.PreallocMethod);
            }
            public QuickFix.Fields.SettlType SettlType
            { 
                get 
                {
                    QuickFix.Fields.SettlType val = new QuickFix.Fields.SettlType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.SettlType val) 
            { 
                this.SettlType = val;
            }
            
            public QuickFix.Fields.SettlType Get(QuickFix.Fields.SettlType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.SettlType val) 
            { 
                return IsSetSettlType();
            }
            
            public bool IsSetSettlType() 
            { 
                return IsSetField(Tags.SettlType);
            }
            public QuickFix.Fields.DaysToSettlement DaysToSettlement
            { 
                get 
                {
                    QuickFix.Fields.DaysToSettlement val = new QuickFix.Fields.DaysToSettlement();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.DaysToSettlement val) 
            { 
                this.DaysToSettlement = val;
            }
            
            public QuickFix.Fields.DaysToSettlement Get(QuickFix.Fields.DaysToSettlement val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.DaysToSettlement val) 
            { 
                return IsSetDaysToSettlement();
            }
            
            public bool IsSetDaysToSettlement() 
            { 
                return IsSetField(Tags.DaysToSettlement);
            }
            public QuickFix.Fields.FixedRate FixedRate
            { 
                get 
                {
                    QuickFix.Fields.FixedRate val = new QuickFix.Fields.FixedRate();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.FixedRate val) 
            { 
                this.FixedRate = val;
            }
            
            public QuickFix.Fields.FixedRate Get(QuickFix.Fields.FixedRate val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.FixedRate val) 
            { 
                return IsSetFixedRate();
            }
            
            public bool IsSetFixedRate() 
            { 
                return IsSetField(Tags.FixedRate);
            }
            public QuickFix.Fields.SettlDate SettlDate
            { 
                get 
                {
                    QuickFix.Fields.SettlDate val = new QuickFix.Fields.SettlDate();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.SettlDate val) 
            { 
                this.SettlDate = val;
            }
            
            public QuickFix.Fields.SettlDate Get(QuickFix.Fields.SettlDate val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.SettlDate val) 
            { 
                return IsSetSettlDate();
            }
            
            public bool IsSetSettlDate() 
            { 
                return IsSetField(Tags.SettlDate);
            }
            public QuickFix.Fields.CashMargin CashMargin
            { 
                get 
                {
                    QuickFix.Fields.CashMargin val = new QuickFix.Fields.CashMargin();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.CashMargin val) 
            { 
                this.CashMargin = val;
            }
            
            public QuickFix.Fields.CashMargin Get(QuickFix.Fields.CashMargin val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.CashMargin val) 
            { 
                return IsSetCashMargin();
            }
            
            public bool IsSetCashMargin() 
            { 
                return IsSetField(Tags.CashMargin);
            }
            public QuickFix.Fields.ClearingFeeIndicator ClearingFeeIndicator
            { 
                get 
                {
                    QuickFix.Fields.ClearingFeeIndicator val = new QuickFix.Fields.ClearingFeeIndicator();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ClearingFeeIndicator val) 
            { 
                this.ClearingFeeIndicator = val;
            }
            
            public QuickFix.Fields.ClearingFeeIndicator Get(QuickFix.Fields.ClearingFeeIndicator val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ClearingFeeIndicator val) 
            { 
                return IsSetClearingFeeIndicator();
            }
            
            public bool IsSetClearingFeeIndicator() 
            { 
                return IsSetField(Tags.ClearingFeeIndicator);
            }
            public QuickFix.Fields.Symbol Symbol
            { 
                get 
                {
                    QuickFix.Fields.Symbol val = new QuickFix.Fields.Symbol();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.Symbol val) 
            { 
                this.Symbol = val;
            }
            
            public QuickFix.Fields.Symbol Get(QuickFix.Fields.Symbol val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.Symbol val) 
            { 
                return IsSetSymbol();
            }
            
            public bool IsSetSymbol() 
            { 
                return IsSetField(Tags.Symbol);
            }
            public QuickFix.Fields.SecurityID SecurityID
            { 
                get 
                {
                    QuickFix.Fields.SecurityID val = new QuickFix.Fields.SecurityID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.SecurityID val) 
            { 
                this.SecurityID = val;
            }
            
            public QuickFix.Fields.SecurityID Get(QuickFix.Fields.SecurityID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.SecurityID val) 
            { 
                return IsSetSecurityID();
            }
            
            public bool IsSetSecurityID() 
            { 
                return IsSetField(Tags.SecurityID);
            }
            public QuickFix.Fields.SecurityIDSource SecurityIDSource
            { 
                get 
                {
                    QuickFix.Fields.SecurityIDSource val = new QuickFix.Fields.SecurityIDSource();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.SecurityIDSource val) 
            { 
                this.SecurityIDSource = val;
            }
            
            public QuickFix.Fields.SecurityIDSource Get(QuickFix.Fields.SecurityIDSource val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.SecurityIDSource val) 
            { 
                return IsSetSecurityIDSource();
            }
            
            public bool IsSetSecurityIDSource() 
            { 
                return IsSetField(Tags.SecurityIDSource);
            }
            public QuickFix.Fields.SecurityExchange SecurityExchange
            { 
                get 
                {
                    QuickFix.Fields.SecurityExchange val = new QuickFix.Fields.SecurityExchange();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.SecurityExchange val) 
            { 
                this.SecurityExchange = val;
            }
            
            public QuickFix.Fields.SecurityExchange Get(QuickFix.Fields.SecurityExchange val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.SecurityExchange val) 
            { 
                return IsSetSecurityExchange();
            }
            
            public bool IsSetSecurityExchange() 
            { 
                return IsSetField(Tags.SecurityExchange);
            }
            public QuickFix.Fields.AgreementDesc AgreementDesc
            { 
                get 
                {
                    QuickFix.Fields.AgreementDesc val = new QuickFix.Fields.AgreementDesc();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AgreementDesc val) 
            { 
                this.AgreementDesc = val;
            }
            
            public QuickFix.Fields.AgreementDesc Get(QuickFix.Fields.AgreementDesc val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AgreementDesc val) 
            { 
                return IsSetAgreementDesc();
            }
            
            public bool IsSetAgreementDesc() 
            { 
                return IsSetField(Tags.AgreementDesc);
            }
            public QuickFix.Fields.AgreementID AgreementID
            { 
                get 
                {
                    QuickFix.Fields.AgreementID val = new QuickFix.Fields.AgreementID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AgreementID val) 
            { 
                this.AgreementID = val;
            }
            
            public QuickFix.Fields.AgreementID Get(QuickFix.Fields.AgreementID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AgreementID val) 
            { 
                return IsSetAgreementID();
            }
            
            public bool IsSetAgreementID() 
            { 
                return IsSetField(Tags.AgreementID);
            }
            public QuickFix.Fields.AgreementDate AgreementDate
            { 
                get 
                {
                    QuickFix.Fields.AgreementDate val = new QuickFix.Fields.AgreementDate();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AgreementDate val) 
            { 
                this.AgreementDate = val;
            }
            
            public QuickFix.Fields.AgreementDate Get(QuickFix.Fields.AgreementDate val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AgreementDate val) 
            { 
                return IsSetAgreementDate();
            }
            
            public bool IsSetAgreementDate() 
            { 
                return IsSetField(Tags.AgreementDate);
            }
            public QuickFix.Fields.AgreementCurrency AgreementCurrency
            { 
                get 
                {
                    QuickFix.Fields.AgreementCurrency val = new QuickFix.Fields.AgreementCurrency();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AgreementCurrency val) 
            { 
                this.AgreementCurrency = val;
            }
            
            public QuickFix.Fields.AgreementCurrency Get(QuickFix.Fields.AgreementCurrency val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AgreementCurrency val) 
            { 
                return IsSetAgreementCurrency();
            }
            
            public bool IsSetAgreementCurrency() 
            { 
                return IsSetField(Tags.AgreementCurrency);
            }
            public QuickFix.Fields.TerminationType TerminationType
            { 
                get 
                {
                    QuickFix.Fields.TerminationType val = new QuickFix.Fields.TerminationType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TerminationType val) 
            { 
                this.TerminationType = val;
            }
            
            public QuickFix.Fields.TerminationType Get(QuickFix.Fields.TerminationType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TerminationType val) 
            { 
                return IsSetTerminationType();
            }
            
            public bool IsSetTerminationType() 
            { 
                return IsSetField(Tags.TerminationType);
            }
            public QuickFix.Fields.StartDate StartDate
            { 
                get 
                {
                    QuickFix.Fields.StartDate val = new QuickFix.Fields.StartDate();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.StartDate val) 
            { 
                this.StartDate = val;
            }
            
            public QuickFix.Fields.StartDate Get(QuickFix.Fields.StartDate val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.StartDate val) 
            { 
                return IsSetStartDate();
            }
            
            public bool IsSetStartDate() 
            { 
                return IsSetField(Tags.StartDate);
            }
            public QuickFix.Fields.EndDate EndDate
            { 
                get 
                {
                    QuickFix.Fields.EndDate val = new QuickFix.Fields.EndDate();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.EndDate val) 
            { 
                this.EndDate = val;
            }
            
            public QuickFix.Fields.EndDate Get(QuickFix.Fields.EndDate val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.EndDate val) 
            { 
                return IsSetEndDate();
            }
            
            public bool IsSetEndDate() 
            { 
                return IsSetField(Tags.EndDate);
            }
            public QuickFix.Fields.DeliveryType DeliveryType
            { 
                get 
                {
                    QuickFix.Fields.DeliveryType val = new QuickFix.Fields.DeliveryType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.DeliveryType val) 
            { 
                this.DeliveryType = val;
            }
            
            public QuickFix.Fields.DeliveryType Get(QuickFix.Fields.DeliveryType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.DeliveryType val) 
            { 
                return IsSetDeliveryType();
            }
            
            public bool IsSetDeliveryType() 
            { 
                return IsSetField(Tags.DeliveryType);
            }
            public QuickFix.Fields.MarginRatio MarginRatio
            { 
                get 
                {
                    QuickFix.Fields.MarginRatio val = new QuickFix.Fields.MarginRatio();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.MarginRatio val) 
            { 
                this.MarginRatio = val;
            }
            
            public QuickFix.Fields.MarginRatio Get(QuickFix.Fields.MarginRatio val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.MarginRatio val) 
            { 
                return IsSetMarginRatio();
            }
            
            public bool IsSetMarginRatio() 
            { 
                return IsSetField(Tags.MarginRatio);
            }
            public QuickFix.Fields.NoUnderlyings NoUnderlyings
            { 
                get 
                {
                    QuickFix.Fields.NoUnderlyings val = new QuickFix.Fields.NoUnderlyings();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoUnderlyings val) 
            { 
                this.NoUnderlyings = val;
            }
            
            public QuickFix.Fields.NoUnderlyings Get(QuickFix.Fields.NoUnderlyings val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoUnderlyings val) 
            { 
                return IsSetNoUnderlyings();
            }
            
            public bool IsSetNoUnderlyings() 
            { 
                return IsSetField(Tags.NoUnderlyings);
            }
            public QuickFix.Fields.Side Side
            { 
                get 
                {
                    QuickFix.Fields.Side val = new QuickFix.Fields.Side();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.Side val) 
            { 
                this.Side = val;
            }
            
            public QuickFix.Fields.Side Get(QuickFix.Fields.Side val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.Side val) 
            { 
                return IsSetSide();
            }
            
            public bool IsSetSide() 
            { 
                return IsSetField(Tags.Side);
            }
            public QuickFix.Fields.NoStipulations NoStipulations
            { 
                get 
                {
                    QuickFix.Fields.NoStipulations val = new QuickFix.Fields.NoStipulations();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoStipulations val) 
            { 
                this.NoStipulations = val;
            }
            
            public QuickFix.Fields.NoStipulations Get(QuickFix.Fields.NoStipulations val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoStipulations val) 
            { 
                return IsSetNoStipulations();
            }
            
            public bool IsSetNoStipulations() 
            { 
                return IsSetField(Tags.NoStipulations);
            }
            public QuickFix.Fields.QtyType QtyType
            { 
                get 
                {
                    QuickFix.Fields.QtyType val = new QuickFix.Fields.QtyType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.QtyType val) 
            { 
                this.QtyType = val;
            }
            
            public QuickFix.Fields.QtyType Get(QuickFix.Fields.QtyType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.QtyType val) 
            { 
                return IsSetQtyType();
            }
            
            public bool IsSetQtyType() 
            { 
                return IsSetField(Tags.QtyType);
            }
            public QuickFix.Fields.OrderQty OrderQty
            { 
                get 
                {
                    QuickFix.Fields.OrderQty val = new QuickFix.Fields.OrderQty();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.OrderQty val) 
            { 
                this.OrderQty = val;
            }
            
            public QuickFix.Fields.OrderQty Get(QuickFix.Fields.OrderQty val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.OrderQty val) 
            { 
                return IsSetOrderQty();
            }
            
            public bool IsSetOrderQty() 
            { 
                return IsSetField(Tags.OrderQty);
            }
            public QuickFix.Fields.CashOrderQty CashOrderQty
            { 
                get 
                {
                    QuickFix.Fields.CashOrderQty val = new QuickFix.Fields.CashOrderQty();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.CashOrderQty val) 
            { 
                this.CashOrderQty = val;
            }
            
            public QuickFix.Fields.CashOrderQty Get(QuickFix.Fields.CashOrderQty val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.CashOrderQty val) 
            { 
                return IsSetCashOrderQty();
            }
            
            public bool IsSetCashOrderQty() 
            { 
                return IsSetField(Tags.CashOrderQty);
            }
            public QuickFix.Fields.OrderPercent OrderPercent
            { 
                get 
                {
                    QuickFix.Fields.OrderPercent val = new QuickFix.Fields.OrderPercent();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.OrderPercent val) 
            { 
                this.OrderPercent = val;
            }
            
            public QuickFix.Fields.OrderPercent Get(QuickFix.Fields.OrderPercent val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.OrderPercent val) 
            { 
                return IsSetOrderPercent();
            }
            
            public bool IsSetOrderPercent() 
            { 
                return IsSetField(Tags.OrderPercent);
            }
            public QuickFix.Fields.RoundingDirection RoundingDirection
            { 
                get 
                {
                    QuickFix.Fields.RoundingDirection val = new QuickFix.Fields.RoundingDirection();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.RoundingDirection val) 
            { 
                this.RoundingDirection = val;
            }
            
            public QuickFix.Fields.RoundingDirection Get(QuickFix.Fields.RoundingDirection val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.RoundingDirection val) 
            { 
                return IsSetRoundingDirection();
            }
            
            public bool IsSetRoundingDirection() 
            { 
                return IsSetField(Tags.RoundingDirection);
            }
            public QuickFix.Fields.RoundingModulus RoundingModulus
            { 
                get 
                {
                    QuickFix.Fields.RoundingModulus val = new QuickFix.Fields.RoundingModulus();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.RoundingModulus val) 
            { 
                this.RoundingModulus = val;
            }
            
            public QuickFix.Fields.RoundingModulus Get(QuickFix.Fields.RoundingModulus val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.RoundingModulus val) 
            { 
                return IsSetRoundingModulus();
            }
            
            public bool IsSetRoundingModulus() 
            { 
                return IsSetField(Tags.RoundingModulus);
            }
            public QuickFix.Fields.OrdType OrdType
            { 
                get 
                {
                    QuickFix.Fields.OrdType val = new QuickFix.Fields.OrdType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.OrdType val) 
            { 
                this.OrdType = val;
            }
            
            public QuickFix.Fields.OrdType Get(QuickFix.Fields.OrdType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.OrdType val) 
            { 
                return IsSetOrdType();
            }
            
            public bool IsSetOrdType() 
            { 
                return IsSetField(Tags.OrdType);
            }
            public QuickFix.Fields.PriceType PriceType
            { 
                get 
                {
                    QuickFix.Fields.PriceType val = new QuickFix.Fields.PriceType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.PriceType val) 
            { 
                this.PriceType = val;
            }
            
            public QuickFix.Fields.PriceType Get(QuickFix.Fields.PriceType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.PriceType val) 
            { 
                return IsSetPriceType();
            }
            
            public bool IsSetPriceType() 
            { 
                return IsSetField(Tags.PriceType);
            }
            public QuickFix.Fields.Price Price
            { 
                get 
                {
                    QuickFix.Fields.Price val = new QuickFix.Fields.Price();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.Price val) 
            { 
                this.Price = val;
            }
            
            public QuickFix.Fields.Price Get(QuickFix.Fields.Price val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.Price val) 
            { 
                return IsSetPrice();
            }
            
            public bool IsSetPrice() 
            { 
                return IsSetField(Tags.Price);
            }
            public QuickFix.Fields.StopPx StopPx
            { 
                get 
                {
                    QuickFix.Fields.StopPx val = new QuickFix.Fields.StopPx();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.StopPx val) 
            { 
                this.StopPx = val;
            }
            
            public QuickFix.Fields.StopPx Get(QuickFix.Fields.StopPx val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.StopPx val) 
            { 
                return IsSetStopPx();
            }
            
            public bool IsSetStopPx() 
            { 
                return IsSetField(Tags.StopPx);
            }
            public QuickFix.Fields.ProtectionPrice ProtectionPrice
            { 
                get 
                {
                    QuickFix.Fields.ProtectionPrice val = new QuickFix.Fields.ProtectionPrice();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ProtectionPrice val) 
            { 
                this.ProtectionPrice = val;
            }
            
            public QuickFix.Fields.ProtectionPrice Get(QuickFix.Fields.ProtectionPrice val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ProtectionPrice val) 
            { 
                return IsSetProtectionPrice();
            }
            
            public bool IsSetProtectionPrice() 
            { 
                return IsSetField(Tags.ProtectionPrice);
            }
            public QuickFix.Fields.PegOffsetValue PegOffsetValue
            { 
                get 
                {
                    QuickFix.Fields.PegOffsetValue val = new QuickFix.Fields.PegOffsetValue();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.PegOffsetValue val) 
            { 
                this.PegOffsetValue = val;
            }
            
            public QuickFix.Fields.PegOffsetValue Get(QuickFix.Fields.PegOffsetValue val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.PegOffsetValue val) 
            { 
                return IsSetPegOffsetValue();
            }
            
            public bool IsSetPegOffsetValue() 
            { 
                return IsSetField(Tags.PegOffsetValue);
            }
            public QuickFix.Fields.PegMoveType PegMoveType
            { 
                get 
                {
                    QuickFix.Fields.PegMoveType val = new QuickFix.Fields.PegMoveType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.PegMoveType val) 
            { 
                this.PegMoveType = val;
            }
            
            public QuickFix.Fields.PegMoveType Get(QuickFix.Fields.PegMoveType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.PegMoveType val) 
            { 
                return IsSetPegMoveType();
            }
            
            public bool IsSetPegMoveType() 
            { 
                return IsSetField(Tags.PegMoveType);
            }
            public QuickFix.Fields.PegOffsetType PegOffsetType
            { 
                get 
                {
                    QuickFix.Fields.PegOffsetType val = new QuickFix.Fields.PegOffsetType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.PegOffsetType val) 
            { 
                this.PegOffsetType = val;
            }
            
            public QuickFix.Fields.PegOffsetType Get(QuickFix.Fields.PegOffsetType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.PegOffsetType val) 
            { 
                return IsSetPegOffsetType();
            }
            
            public bool IsSetPegOffsetType() 
            { 
                return IsSetField(Tags.PegOffsetType);
            }
            public QuickFix.Fields.PegLimitType PegLimitType
            { 
                get 
                {
                    QuickFix.Fields.PegLimitType val = new QuickFix.Fields.PegLimitType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.PegLimitType val) 
            { 
                this.PegLimitType = val;
            }
            
            public QuickFix.Fields.PegLimitType Get(QuickFix.Fields.PegLimitType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.PegLimitType val) 
            { 
                return IsSetPegLimitType();
            }
            
            public bool IsSetPegLimitType() 
            { 
                return IsSetField(Tags.PegLimitType);
            }
            public QuickFix.Fields.PegRoundDirection PegRoundDirection
            { 
                get 
                {
                    QuickFix.Fields.PegRoundDirection val = new QuickFix.Fields.PegRoundDirection();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.PegRoundDirection val) 
            { 
                this.PegRoundDirection = val;
            }
            
            public QuickFix.Fields.PegRoundDirection Get(QuickFix.Fields.PegRoundDirection val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.PegRoundDirection val) 
            { 
                return IsSetPegRoundDirection();
            }
            
            public bool IsSetPegRoundDirection() 
            { 
                return IsSetField(Tags.PegRoundDirection);
            }
            public QuickFix.Fields.PegScope PegScope
            { 
                get 
                {
                    QuickFix.Fields.PegScope val = new QuickFix.Fields.PegScope();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.PegScope val) 
            { 
                this.PegScope = val;
            }
            
            public QuickFix.Fields.PegScope Get(QuickFix.Fields.PegScope val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.PegScope val) 
            { 
                return IsSetPegScope();
            }
            
            public bool IsSetPegScope() 
            { 
                return IsSetField(Tags.PegScope);
            }
            public QuickFix.Fields.DiscretionInst DiscretionInst
            { 
                get 
                {
                    QuickFix.Fields.DiscretionInst val = new QuickFix.Fields.DiscretionInst();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.DiscretionInst val) 
            { 
                this.DiscretionInst = val;
            }
            
            public QuickFix.Fields.DiscretionInst Get(QuickFix.Fields.DiscretionInst val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.DiscretionInst val) 
            { 
                return IsSetDiscretionInst();
            }
            
            public bool IsSetDiscretionInst() 
            { 
                return IsSetField(Tags.DiscretionInst);
            }
            public QuickFix.Fields.DiscretionOffsetValue DiscretionOffsetValue
            { 
                get 
                {
                    QuickFix.Fields.DiscretionOffsetValue val = new QuickFix.Fields.DiscretionOffsetValue();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.DiscretionOffsetValue val) 
            { 
                this.DiscretionOffsetValue = val;
            }
            
            public QuickFix.Fields.DiscretionOffsetValue Get(QuickFix.Fields.DiscretionOffsetValue val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.DiscretionOffsetValue val) 
            { 
                return IsSetDiscretionOffsetValue();
            }
            
            public bool IsSetDiscretionOffsetValue() 
            { 
                return IsSetField(Tags.DiscretionOffsetValue);
            }
            public QuickFix.Fields.DiscretionMoveType DiscretionMoveType
            { 
                get 
                {
                    QuickFix.Fields.DiscretionMoveType val = new QuickFix.Fields.DiscretionMoveType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.DiscretionMoveType val) 
            { 
                this.DiscretionMoveType = val;
            }
            
            public QuickFix.Fields.DiscretionMoveType Get(QuickFix.Fields.DiscretionMoveType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.DiscretionMoveType val) 
            { 
                return IsSetDiscretionMoveType();
            }
            
            public bool IsSetDiscretionMoveType() 
            { 
                return IsSetField(Tags.DiscretionMoveType);
            }
            public QuickFix.Fields.DiscretionOffsetType DiscretionOffsetType
            { 
                get 
                {
                    QuickFix.Fields.DiscretionOffsetType val = new QuickFix.Fields.DiscretionOffsetType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.DiscretionOffsetType val) 
            { 
                this.DiscretionOffsetType = val;
            }
            
            public QuickFix.Fields.DiscretionOffsetType Get(QuickFix.Fields.DiscretionOffsetType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.DiscretionOffsetType val) 
            { 
                return IsSetDiscretionOffsetType();
            }
            
            public bool IsSetDiscretionOffsetType() 
            { 
                return IsSetField(Tags.DiscretionOffsetType);
            }
            public QuickFix.Fields.DiscretionLimitType DiscretionLimitType
            { 
                get 
                {
                    QuickFix.Fields.DiscretionLimitType val = new QuickFix.Fields.DiscretionLimitType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.DiscretionLimitType val) 
            { 
                this.DiscretionLimitType = val;
            }
            
            public QuickFix.Fields.DiscretionLimitType Get(QuickFix.Fields.DiscretionLimitType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.DiscretionLimitType val) 
            { 
                return IsSetDiscretionLimitType();
            }
            
            public bool IsSetDiscretionLimitType() 
            { 
                return IsSetField(Tags.DiscretionLimitType);
            }
            public QuickFix.Fields.DiscretionRoundDirection DiscretionRoundDirection
            { 
                get 
                {
                    QuickFix.Fields.DiscretionRoundDirection val = new QuickFix.Fields.DiscretionRoundDirection();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.DiscretionRoundDirection val) 
            { 
                this.DiscretionRoundDirection = val;
            }
            
            public QuickFix.Fields.DiscretionRoundDirection Get(QuickFix.Fields.DiscretionRoundDirection val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.DiscretionRoundDirection val) 
            { 
                return IsSetDiscretionRoundDirection();
            }
            
            public bool IsSetDiscretionRoundDirection() 
            { 
                return IsSetField(Tags.DiscretionRoundDirection);
            }
            public QuickFix.Fields.DiscretionScope DiscretionScope
            { 
                get 
                {
                    QuickFix.Fields.DiscretionScope val = new QuickFix.Fields.DiscretionScope();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.DiscretionScope val) 
            { 
                this.DiscretionScope = val;
            }
            
            public QuickFix.Fields.DiscretionScope Get(QuickFix.Fields.DiscretionScope val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.DiscretionScope val) 
            { 
                return IsSetDiscretionScope();
            }
            
            public bool IsSetDiscretionScope() 
            { 
                return IsSetField(Tags.DiscretionScope);
            }
            public QuickFix.Fields.PeggedPrice PeggedPrice
            { 
                get 
                {
                    QuickFix.Fields.PeggedPrice val = new QuickFix.Fields.PeggedPrice();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.PeggedPrice val) 
            { 
                this.PeggedPrice = val;
            }
            
            public QuickFix.Fields.PeggedPrice Get(QuickFix.Fields.PeggedPrice val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.PeggedPrice val) 
            { 
                return IsSetPeggedPrice();
            }
            
            public bool IsSetPeggedPrice() 
            { 
                return IsSetField(Tags.PeggedPrice);
            }
            public QuickFix.Fields.DiscretionPrice DiscretionPrice
            { 
                get 
                {
                    QuickFix.Fields.DiscretionPrice val = new QuickFix.Fields.DiscretionPrice();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.DiscretionPrice val) 
            { 
                this.DiscretionPrice = val;
            }
            
            public QuickFix.Fields.DiscretionPrice Get(QuickFix.Fields.DiscretionPrice val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.DiscretionPrice val) 
            { 
                return IsSetDiscretionPrice();
            }
            
            public bool IsSetDiscretionPrice() 
            { 
                return IsSetField(Tags.DiscretionPrice);
            }
            public QuickFix.Fields.TargetStrategy TargetStrategy
            { 
                get 
                {
                    QuickFix.Fields.TargetStrategy val = new QuickFix.Fields.TargetStrategy();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TargetStrategy val) 
            { 
                this.TargetStrategy = val;
            }
            
            public QuickFix.Fields.TargetStrategy Get(QuickFix.Fields.TargetStrategy val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TargetStrategy val) 
            { 
                return IsSetTargetStrategy();
            }
            
            public bool IsSetTargetStrategy() 
            { 
                return IsSetField(Tags.TargetStrategy);
            }
            public QuickFix.Fields.TargetStrategyParameters TargetStrategyParameters
            { 
                get 
                {
                    QuickFix.Fields.TargetStrategyParameters val = new QuickFix.Fields.TargetStrategyParameters();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TargetStrategyParameters val) 
            { 
                this.TargetStrategyParameters = val;
            }
            
            public QuickFix.Fields.TargetStrategyParameters Get(QuickFix.Fields.TargetStrategyParameters val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TargetStrategyParameters val) 
            { 
                return IsSetTargetStrategyParameters();
            }
            
            public bool IsSetTargetStrategyParameters() 
            { 
                return IsSetField(Tags.TargetStrategyParameters);
            }
            public QuickFix.Fields.ParticipationRate ParticipationRate
            { 
                get 
                {
                    QuickFix.Fields.ParticipationRate val = new QuickFix.Fields.ParticipationRate();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ParticipationRate val) 
            { 
                this.ParticipationRate = val;
            }
            
            public QuickFix.Fields.ParticipationRate Get(QuickFix.Fields.ParticipationRate val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ParticipationRate val) 
            { 
                return IsSetParticipationRate();
            }
            
            public bool IsSetParticipationRate() 
            { 
                return IsSetField(Tags.ParticipationRate);
            }
            public QuickFix.Fields.TargetStrategyPerformance TargetStrategyPerformance
            { 
                get 
                {
                    QuickFix.Fields.TargetStrategyPerformance val = new QuickFix.Fields.TargetStrategyPerformance();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TargetStrategyPerformance val) 
            { 
                this.TargetStrategyPerformance = val;
            }
            
            public QuickFix.Fields.TargetStrategyPerformance Get(QuickFix.Fields.TargetStrategyPerformance val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TargetStrategyPerformance val) 
            { 
                return IsSetTargetStrategyPerformance();
            }
            
            public bool IsSetTargetStrategyPerformance() 
            { 
                return IsSetField(Tags.TargetStrategyPerformance);
            }
            public QuickFix.Fields.Currency Currency
            { 
                get 
                {
                    QuickFix.Fields.Currency val = new QuickFix.Fields.Currency();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.Currency val) 
            { 
                this.Currency = val;
            }
            
            public QuickFix.Fields.Currency Get(QuickFix.Fields.Currency val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.Currency val) 
            { 
                return IsSetCurrency();
            }
            
            public bool IsSetCurrency() 
            { 
                return IsSetField(Tags.Currency);
            }
            public QuickFix.Fields.ComplianceID ComplianceID
            { 
                get 
                {
                    QuickFix.Fields.ComplianceID val = new QuickFix.Fields.ComplianceID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ComplianceID val) 
            { 
                this.ComplianceID = val;
            }
            
            public QuickFix.Fields.ComplianceID Get(QuickFix.Fields.ComplianceID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ComplianceID val) 
            { 
                return IsSetComplianceID();
            }
            
            public bool IsSetComplianceID() 
            { 
                return IsSetField(Tags.ComplianceID);
            }
            public QuickFix.Fields.SolicitedFlag SolicitedFlag
            { 
                get 
                {
                    QuickFix.Fields.SolicitedFlag val = new QuickFix.Fields.SolicitedFlag();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.SolicitedFlag val) 
            { 
                this.SolicitedFlag = val;
            }
            
            public QuickFix.Fields.SolicitedFlag Get(QuickFix.Fields.SolicitedFlag val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.SolicitedFlag val) 
            { 
                return IsSetSolicitedFlag();
            }
            
            public bool IsSetSolicitedFlag() 
            { 
                return IsSetField(Tags.SolicitedFlag);
            }
            public QuickFix.Fields.TimeInForce TimeInForce
            { 
                get 
                {
                    QuickFix.Fields.TimeInForce val = new QuickFix.Fields.TimeInForce();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TimeInForce val) 
            { 
                this.TimeInForce = val;
            }
            
            public QuickFix.Fields.TimeInForce Get(QuickFix.Fields.TimeInForce val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TimeInForce val) 
            { 
                return IsSetTimeInForce();
            }
            
            public bool IsSetTimeInForce() 
            { 
                return IsSetField(Tags.TimeInForce);
            }
            public QuickFix.Fields.EffectiveTime EffectiveTime
            { 
                get 
                {
                    QuickFix.Fields.EffectiveTime val = new QuickFix.Fields.EffectiveTime();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.EffectiveTime val) 
            { 
                this.EffectiveTime = val;
            }
            
            public QuickFix.Fields.EffectiveTime Get(QuickFix.Fields.EffectiveTime val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.EffectiveTime val) 
            { 
                return IsSetEffectiveTime();
            }
            
            public bool IsSetEffectiveTime() 
            { 
                return IsSetField(Tags.EffectiveTime);
            }
            public QuickFix.Fields.ExpireDate ExpireDate
            { 
                get 
                {
                    QuickFix.Fields.ExpireDate val = new QuickFix.Fields.ExpireDate();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ExpireDate val) 
            { 
                this.ExpireDate = val;
            }
            
            public QuickFix.Fields.ExpireDate Get(QuickFix.Fields.ExpireDate val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ExpireDate val) 
            { 
                return IsSetExpireDate();
            }
            
            public bool IsSetExpireDate() 
            { 
                return IsSetField(Tags.ExpireDate);
            }
            public QuickFix.Fields.ExpireTime ExpireTime
            { 
                get 
                {
                    QuickFix.Fields.ExpireTime val = new QuickFix.Fields.ExpireTime();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ExpireTime val) 
            { 
                this.ExpireTime = val;
            }
            
            public QuickFix.Fields.ExpireTime Get(QuickFix.Fields.ExpireTime val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ExpireTime val) 
            { 
                return IsSetExpireTime();
            }
            
            public bool IsSetExpireTime() 
            { 
                return IsSetField(Tags.ExpireTime);
            }
            public QuickFix.Fields.ExecInst ExecInst
            { 
                get 
                {
                    QuickFix.Fields.ExecInst val = new QuickFix.Fields.ExecInst();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ExecInst val) 
            { 
                this.ExecInst = val;
            }
            
            public QuickFix.Fields.ExecInst Get(QuickFix.Fields.ExecInst val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ExecInst val) 
            { 
                return IsSetExecInst();
            }
            
            public bool IsSetExecInst() 
            { 
                return IsSetField(Tags.ExecInst);
            }
            public QuickFix.Fields.OrderCapacity OrderCapacity
            { 
                get 
                {
                    QuickFix.Fields.OrderCapacity val = new QuickFix.Fields.OrderCapacity();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.OrderCapacity val) 
            { 
                this.OrderCapacity = val;
            }
            
            public QuickFix.Fields.OrderCapacity Get(QuickFix.Fields.OrderCapacity val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.OrderCapacity val) 
            { 
                return IsSetOrderCapacity();
            }
            
            public bool IsSetOrderCapacity() 
            { 
                return IsSetField(Tags.OrderCapacity);
            }
            public QuickFix.Fields.OrderRestrictions OrderRestrictions
            { 
                get 
                {
                    QuickFix.Fields.OrderRestrictions val = new QuickFix.Fields.OrderRestrictions();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.OrderRestrictions val) 
            { 
                this.OrderRestrictions = val;
            }
            
            public QuickFix.Fields.OrderRestrictions Get(QuickFix.Fields.OrderRestrictions val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.OrderRestrictions val) 
            { 
                return IsSetOrderRestrictions();
            }
            
            public bool IsSetOrderRestrictions() 
            { 
                return IsSetField(Tags.OrderRestrictions);
            }
            public QuickFix.Fields.CustOrderCapacity CustOrderCapacity
            { 
                get 
                {
                    QuickFix.Fields.CustOrderCapacity val = new QuickFix.Fields.CustOrderCapacity();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.CustOrderCapacity val) 
            { 
                this.CustOrderCapacity = val;
            }
            
            public QuickFix.Fields.CustOrderCapacity Get(QuickFix.Fields.CustOrderCapacity val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.CustOrderCapacity val) 
            { 
                return IsSetCustOrderCapacity();
            }
            
            public bool IsSetCustOrderCapacity() 
            { 
                return IsSetField(Tags.CustOrderCapacity);
            }
            public QuickFix.Fields.LastQty LastQty
            { 
                get 
                {
                    QuickFix.Fields.LastQty val = new QuickFix.Fields.LastQty();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.LastQty val) 
            { 
                this.LastQty = val;
            }
            
            public QuickFix.Fields.LastQty Get(QuickFix.Fields.LastQty val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.LastQty val) 
            { 
                return IsSetLastQty();
            }
            
            public bool IsSetLastQty() 
            { 
                return IsSetField(Tags.LastQty);
            }
            public QuickFix.Fields.UnderlyingLastQty UnderlyingLastQty
            { 
                get 
                {
                    QuickFix.Fields.UnderlyingLastQty val = new QuickFix.Fields.UnderlyingLastQty();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.UnderlyingLastQty val) 
            { 
                this.UnderlyingLastQty = val;
            }
            
            public QuickFix.Fields.UnderlyingLastQty Get(QuickFix.Fields.UnderlyingLastQty val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.UnderlyingLastQty val) 
            { 
                return IsSetUnderlyingLastQty();
            }
            
            public bool IsSetUnderlyingLastQty() 
            { 
                return IsSetField(Tags.UnderlyingLastQty);
            }
            public QuickFix.Fields.LastPx LastPx
            { 
                get 
                {
                    QuickFix.Fields.LastPx val = new QuickFix.Fields.LastPx();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.LastPx val) 
            { 
                this.LastPx = val;
            }
            
            public QuickFix.Fields.LastPx Get(QuickFix.Fields.LastPx val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.LastPx val) 
            { 
                return IsSetLastPx();
            }
            
            public bool IsSetLastPx() 
            { 
                return IsSetField(Tags.LastPx);
            }
            public QuickFix.Fields.UnderlyingLastPx UnderlyingLastPx
            { 
                get 
                {
                    QuickFix.Fields.UnderlyingLastPx val = new QuickFix.Fields.UnderlyingLastPx();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.UnderlyingLastPx val) 
            { 
                this.UnderlyingLastPx = val;
            }
            
            public QuickFix.Fields.UnderlyingLastPx Get(QuickFix.Fields.UnderlyingLastPx val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.UnderlyingLastPx val) 
            { 
                return IsSetUnderlyingLastPx();
            }
            
            public bool IsSetUnderlyingLastPx() 
            { 
                return IsSetField(Tags.UnderlyingLastPx);
            }
            public QuickFix.Fields.LastParPx LastParPx
            { 
                get 
                {
                    QuickFix.Fields.LastParPx val = new QuickFix.Fields.LastParPx();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.LastParPx val) 
            { 
                this.LastParPx = val;
            }
            
            public QuickFix.Fields.LastParPx Get(QuickFix.Fields.LastParPx val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.LastParPx val) 
            { 
                return IsSetLastParPx();
            }
            
            public bool IsSetLastParPx() 
            { 
                return IsSetField(Tags.LastParPx);
            }
            public QuickFix.Fields.LastSpotRate LastSpotRate
            { 
                get 
                {
                    QuickFix.Fields.LastSpotRate val = new QuickFix.Fields.LastSpotRate();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.LastSpotRate val) 
            { 
                this.LastSpotRate = val;
            }
            
            public QuickFix.Fields.LastSpotRate Get(QuickFix.Fields.LastSpotRate val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.LastSpotRate val) 
            { 
                return IsSetLastSpotRate();
            }
            
            public bool IsSetLastSpotRate() 
            { 
                return IsSetField(Tags.LastSpotRate);
            }
            public QuickFix.Fields.LastForwardPoints LastForwardPoints
            { 
                get 
                {
                    QuickFix.Fields.LastForwardPoints val = new QuickFix.Fields.LastForwardPoints();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.LastForwardPoints val) 
            { 
                this.LastForwardPoints = val;
            }
            
            public QuickFix.Fields.LastForwardPoints Get(QuickFix.Fields.LastForwardPoints val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.LastForwardPoints val) 
            { 
                return IsSetLastForwardPoints();
            }
            
            public bool IsSetLastForwardPoints() 
            { 
                return IsSetField(Tags.LastForwardPoints);
            }
            public QuickFix.Fields.LastMkt LastMkt
            { 
                get 
                {
                    QuickFix.Fields.LastMkt val = new QuickFix.Fields.LastMkt();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.LastMkt val) 
            { 
                this.LastMkt = val;
            }
            
            public QuickFix.Fields.LastMkt Get(QuickFix.Fields.LastMkt val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.LastMkt val) 
            { 
                return IsSetLastMkt();
            }
            
            public bool IsSetLastMkt() 
            { 
                return IsSetField(Tags.LastMkt);
            }
            public QuickFix.Fields.TradingSessionID TradingSessionID
            { 
                get 
                {
                    QuickFix.Fields.TradingSessionID val = new QuickFix.Fields.TradingSessionID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TradingSessionID val) 
            { 
                this.TradingSessionID = val;
            }
            
            public QuickFix.Fields.TradingSessionID Get(QuickFix.Fields.TradingSessionID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TradingSessionID val) 
            { 
                return IsSetTradingSessionID();
            }
            
            public bool IsSetTradingSessionID() 
            { 
                return IsSetField(Tags.TradingSessionID);
            }
            public QuickFix.Fields.TradingSessionSubID TradingSessionSubID
            { 
                get 
                {
                    QuickFix.Fields.TradingSessionSubID val = new QuickFix.Fields.TradingSessionSubID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TradingSessionSubID val) 
            { 
                this.TradingSessionSubID = val;
            }
            
            public QuickFix.Fields.TradingSessionSubID Get(QuickFix.Fields.TradingSessionSubID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TradingSessionSubID val) 
            { 
                return IsSetTradingSessionSubID();
            }
            
            public bool IsSetTradingSessionSubID() 
            { 
                return IsSetField(Tags.TradingSessionSubID);
            }
            public QuickFix.Fields.TimeBracket TimeBracket
            { 
                get 
                {
                    QuickFix.Fields.TimeBracket val = new QuickFix.Fields.TimeBracket();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TimeBracket val) 
            { 
                this.TimeBracket = val;
            }
            
            public QuickFix.Fields.TimeBracket Get(QuickFix.Fields.TimeBracket val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TimeBracket val) 
            { 
                return IsSetTimeBracket();
            }
            
            public bool IsSetTimeBracket() 
            { 
                return IsSetField(Tags.TimeBracket);
            }
            public QuickFix.Fields.LastCapacity LastCapacity
            { 
                get 
                {
                    QuickFix.Fields.LastCapacity val = new QuickFix.Fields.LastCapacity();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.LastCapacity val) 
            { 
                this.LastCapacity = val;
            }
            
            public QuickFix.Fields.LastCapacity Get(QuickFix.Fields.LastCapacity val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.LastCapacity val) 
            { 
                return IsSetLastCapacity();
            }
            
            public bool IsSetLastCapacity() 
            { 
                return IsSetField(Tags.LastCapacity);
            }
            public QuickFix.Fields.LeavesQty LeavesQty
            { 
                get 
                {
                    QuickFix.Fields.LeavesQty val = new QuickFix.Fields.LeavesQty();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.LeavesQty val) 
            { 
                this.LeavesQty = val;
            }
            
            public QuickFix.Fields.LeavesQty Get(QuickFix.Fields.LeavesQty val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.LeavesQty val) 
            { 
                return IsSetLeavesQty();
            }
            
            public bool IsSetLeavesQty() 
            { 
                return IsSetField(Tags.LeavesQty);
            }
            public QuickFix.Fields.CumQty CumQty
            { 
                get 
                {
                    QuickFix.Fields.CumQty val = new QuickFix.Fields.CumQty();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.CumQty val) 
            { 
                this.CumQty = val;
            }
            
            public QuickFix.Fields.CumQty Get(QuickFix.Fields.CumQty val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.CumQty val) 
            { 
                return IsSetCumQty();
            }
            
            public bool IsSetCumQty() 
            { 
                return IsSetField(Tags.CumQty);
            }
            public QuickFix.Fields.AvgPx AvgPx
            { 
                get 
                {
                    QuickFix.Fields.AvgPx val = new QuickFix.Fields.AvgPx();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AvgPx val) 
            { 
                this.AvgPx = val;
            }
            
            public QuickFix.Fields.AvgPx Get(QuickFix.Fields.AvgPx val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AvgPx val) 
            { 
                return IsSetAvgPx();
            }
            
            public bool IsSetAvgPx() 
            { 
                return IsSetField(Tags.AvgPx);
            }
            public QuickFix.Fields.DayOrderQty DayOrderQty
            { 
                get 
                {
                    QuickFix.Fields.DayOrderQty val = new QuickFix.Fields.DayOrderQty();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.DayOrderQty val) 
            { 
                this.DayOrderQty = val;
            }
            
            public QuickFix.Fields.DayOrderQty Get(QuickFix.Fields.DayOrderQty val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.DayOrderQty val) 
            { 
                return IsSetDayOrderQty();
            }
            
            public bool IsSetDayOrderQty() 
            { 
                return IsSetField(Tags.DayOrderQty);
            }
            public QuickFix.Fields.DayCumQty DayCumQty
            { 
                get 
                {
                    QuickFix.Fields.DayCumQty val = new QuickFix.Fields.DayCumQty();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.DayCumQty val) 
            { 
                this.DayCumQty = val;
            }
            
            public QuickFix.Fields.DayCumQty Get(QuickFix.Fields.DayCumQty val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.DayCumQty val) 
            { 
                return IsSetDayCumQty();
            }
            
            public bool IsSetDayCumQty() 
            { 
                return IsSetField(Tags.DayCumQty);
            }
            public QuickFix.Fields.DayAvgPx DayAvgPx
            { 
                get 
                {
                    QuickFix.Fields.DayAvgPx val = new QuickFix.Fields.DayAvgPx();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.DayAvgPx val) 
            { 
                this.DayAvgPx = val;
            }
            
            public QuickFix.Fields.DayAvgPx Get(QuickFix.Fields.DayAvgPx val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.DayAvgPx val) 
            { 
                return IsSetDayAvgPx();
            }
            
            public bool IsSetDayAvgPx() 
            { 
                return IsSetField(Tags.DayAvgPx);
            }
            public QuickFix.Fields.GTBookingInst GTBookingInst
            { 
                get 
                {
                    QuickFix.Fields.GTBookingInst val = new QuickFix.Fields.GTBookingInst();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.GTBookingInst val) 
            { 
                this.GTBookingInst = val;
            }
            
            public QuickFix.Fields.GTBookingInst Get(QuickFix.Fields.GTBookingInst val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.GTBookingInst val) 
            { 
                return IsSetGTBookingInst();
            }
            
            public bool IsSetGTBookingInst() 
            { 
                return IsSetField(Tags.GTBookingInst);
            }
            public QuickFix.Fields.TradeDate TradeDate
            { 
                get 
                {
                    QuickFix.Fields.TradeDate val = new QuickFix.Fields.TradeDate();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TradeDate val) 
            { 
                this.TradeDate = val;
            }
            
            public QuickFix.Fields.TradeDate Get(QuickFix.Fields.TradeDate val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TradeDate val) 
            { 
                return IsSetTradeDate();
            }
            
            public bool IsSetTradeDate() 
            { 
                return IsSetField(Tags.TradeDate);
            }
            public QuickFix.Fields.TransactTime TransactTime
            { 
                get 
                {
                    QuickFix.Fields.TransactTime val = new QuickFix.Fields.TransactTime();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TransactTime val) 
            { 
                this.TransactTime = val;
            }
            
            public QuickFix.Fields.TransactTime Get(QuickFix.Fields.TransactTime val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TransactTime val) 
            { 
                return IsSetTransactTime();
            }
            
            public bool IsSetTransactTime() 
            { 
                return IsSetField(Tags.TransactTime);
            }
            public QuickFix.Fields.AggressorIndicator AggressorIndicator
            { 
                get 
                {
                    QuickFix.Fields.AggressorIndicator val = new QuickFix.Fields.AggressorIndicator();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AggressorIndicator val) 
            { 
                this.AggressorIndicator = val;
            }
            
            public QuickFix.Fields.AggressorIndicator Get(QuickFix.Fields.AggressorIndicator val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AggressorIndicator val) 
            { 
                return IsSetAggressorIndicator();
            }
            
            public bool IsSetAggressorIndicator() 
            { 
                return IsSetField(Tags.AggressorIndicator);
            }
            public QuickFix.Fields.ReportToExch ReportToExch
            { 
                get 
                {
                    QuickFix.Fields.ReportToExch val = new QuickFix.Fields.ReportToExch();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ReportToExch val) 
            { 
                this.ReportToExch = val;
            }
            
            public QuickFix.Fields.ReportToExch Get(QuickFix.Fields.ReportToExch val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ReportToExch val) 
            { 
                return IsSetReportToExch();
            }
            
            public bool IsSetReportToExch() 
            { 
                return IsSetField(Tags.ReportToExch);
            }
            public QuickFix.Fields.Commission Commission
            { 
                get 
                {
                    QuickFix.Fields.Commission val = new QuickFix.Fields.Commission();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.Commission val) 
            { 
                this.Commission = val;
            }
            
            public QuickFix.Fields.Commission Get(QuickFix.Fields.Commission val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.Commission val) 
            { 
                return IsSetCommission();
            }
            
            public bool IsSetCommission() 
            { 
                return IsSetField(Tags.Commission);
            }
            public QuickFix.Fields.CommType CommType
            { 
                get 
                {
                    QuickFix.Fields.CommType val = new QuickFix.Fields.CommType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.CommType val) 
            { 
                this.CommType = val;
            }
            
            public QuickFix.Fields.CommType Get(QuickFix.Fields.CommType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.CommType val) 
            { 
                return IsSetCommType();
            }
            
            public bool IsSetCommType() 
            { 
                return IsSetField(Tags.CommType);
            }
            public QuickFix.Fields.CommCurrency CommCurrency
            { 
                get 
                {
                    QuickFix.Fields.CommCurrency val = new QuickFix.Fields.CommCurrency();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.CommCurrency val) 
            { 
                this.CommCurrency = val;
            }
            
            public QuickFix.Fields.CommCurrency Get(QuickFix.Fields.CommCurrency val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.CommCurrency val) 
            { 
                return IsSetCommCurrency();
            }
            
            public bool IsSetCommCurrency() 
            { 
                return IsSetField(Tags.CommCurrency);
            }
            public QuickFix.Fields.FundRenewWaiv FundRenewWaiv
            { 
                get 
                {
                    QuickFix.Fields.FundRenewWaiv val = new QuickFix.Fields.FundRenewWaiv();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.FundRenewWaiv val) 
            { 
                this.FundRenewWaiv = val;
            }
            
            public QuickFix.Fields.FundRenewWaiv Get(QuickFix.Fields.FundRenewWaiv val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.FundRenewWaiv val) 
            { 
                return IsSetFundRenewWaiv();
            }
            
            public bool IsSetFundRenewWaiv() 
            { 
                return IsSetField(Tags.FundRenewWaiv);
            }
            public QuickFix.Fields.Spread Spread
            { 
                get 
                {
                    QuickFix.Fields.Spread val = new QuickFix.Fields.Spread();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.Spread val) 
            { 
                this.Spread = val;
            }
            
            public QuickFix.Fields.Spread Get(QuickFix.Fields.Spread val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.Spread val) 
            { 
                return IsSetSpread();
            }
            
            public bool IsSetSpread() 
            { 
                return IsSetField(Tags.Spread);
            }
            public QuickFix.Fields.BenchmarkCurveCurrency BenchmarkCurveCurrency
            { 
                get 
                {
                    QuickFix.Fields.BenchmarkCurveCurrency val = new QuickFix.Fields.BenchmarkCurveCurrency();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.BenchmarkCurveCurrency val) 
            { 
                this.BenchmarkCurveCurrency = val;
            }
            
            public QuickFix.Fields.BenchmarkCurveCurrency Get(QuickFix.Fields.BenchmarkCurveCurrency val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.BenchmarkCurveCurrency val) 
            { 
                return IsSetBenchmarkCurveCurrency();
            }
            
            public bool IsSetBenchmarkCurveCurrency() 
            { 
                return IsSetField(Tags.BenchmarkCurveCurrency);
            }
            public QuickFix.Fields.BenchmarkCurveName BenchmarkCurveName
            { 
                get 
                {
                    QuickFix.Fields.BenchmarkCurveName val = new QuickFix.Fields.BenchmarkCurveName();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.BenchmarkCurveName val) 
            { 
                this.BenchmarkCurveName = val;
            }
            
            public QuickFix.Fields.BenchmarkCurveName Get(QuickFix.Fields.BenchmarkCurveName val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.BenchmarkCurveName val) 
            { 
                return IsSetBenchmarkCurveName();
            }
            
            public bool IsSetBenchmarkCurveName() 
            { 
                return IsSetField(Tags.BenchmarkCurveName);
            }
            public QuickFix.Fields.BenchmarkCurvePoint BenchmarkCurvePoint
            { 
                get 
                {
                    QuickFix.Fields.BenchmarkCurvePoint val = new QuickFix.Fields.BenchmarkCurvePoint();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.BenchmarkCurvePoint val) 
            { 
                this.BenchmarkCurvePoint = val;
            }
            
            public QuickFix.Fields.BenchmarkCurvePoint Get(QuickFix.Fields.BenchmarkCurvePoint val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.BenchmarkCurvePoint val) 
            { 
                return IsSetBenchmarkCurvePoint();
            }
            
            public bool IsSetBenchmarkCurvePoint() 
            { 
                return IsSetField(Tags.BenchmarkCurvePoint);
            }
            public QuickFix.Fields.BenchmarkPrice BenchmarkPrice
            { 
                get 
                {
                    QuickFix.Fields.BenchmarkPrice val = new QuickFix.Fields.BenchmarkPrice();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.BenchmarkPrice val) 
            { 
                this.BenchmarkPrice = val;
            }
            
            public QuickFix.Fields.BenchmarkPrice Get(QuickFix.Fields.BenchmarkPrice val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.BenchmarkPrice val) 
            { 
                return IsSetBenchmarkPrice();
            }
            
            public bool IsSetBenchmarkPrice() 
            { 
                return IsSetField(Tags.BenchmarkPrice);
            }
            public QuickFix.Fields.BenchmarkPriceType BenchmarkPriceType
            { 
                get 
                {
                    QuickFix.Fields.BenchmarkPriceType val = new QuickFix.Fields.BenchmarkPriceType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.BenchmarkPriceType val) 
            { 
                this.BenchmarkPriceType = val;
            }
            
            public QuickFix.Fields.BenchmarkPriceType Get(QuickFix.Fields.BenchmarkPriceType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.BenchmarkPriceType val) 
            { 
                return IsSetBenchmarkPriceType();
            }
            
            public bool IsSetBenchmarkPriceType() 
            { 
                return IsSetField(Tags.BenchmarkPriceType);
            }
            public QuickFix.Fields.BenchmarkSecurityID BenchmarkSecurityID
            { 
                get 
                {
                    QuickFix.Fields.BenchmarkSecurityID val = new QuickFix.Fields.BenchmarkSecurityID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.BenchmarkSecurityID val) 
            { 
                this.BenchmarkSecurityID = val;
            }
            
            public QuickFix.Fields.BenchmarkSecurityID Get(QuickFix.Fields.BenchmarkSecurityID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.BenchmarkSecurityID val) 
            { 
                return IsSetBenchmarkSecurityID();
            }
            
            public bool IsSetBenchmarkSecurityID() 
            { 
                return IsSetField(Tags.BenchmarkSecurityID);
            }
            public QuickFix.Fields.BenchmarkSecurityIDSource BenchmarkSecurityIDSource
            { 
                get 
                {
                    QuickFix.Fields.BenchmarkSecurityIDSource val = new QuickFix.Fields.BenchmarkSecurityIDSource();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.BenchmarkSecurityIDSource val) 
            { 
                this.BenchmarkSecurityIDSource = val;
            }
            
            public QuickFix.Fields.BenchmarkSecurityIDSource Get(QuickFix.Fields.BenchmarkSecurityIDSource val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.BenchmarkSecurityIDSource val) 
            { 
                return IsSetBenchmarkSecurityIDSource();
            }
            
            public bool IsSetBenchmarkSecurityIDSource() 
            { 
                return IsSetField(Tags.BenchmarkSecurityIDSource);
            }
            public QuickFix.Fields.YieldType YieldType
            { 
                get 
                {
                    QuickFix.Fields.YieldType val = new QuickFix.Fields.YieldType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.YieldType val) 
            { 
                this.YieldType = val;
            }
            
            public QuickFix.Fields.YieldType Get(QuickFix.Fields.YieldType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.YieldType val) 
            { 
                return IsSetYieldType();
            }
            
            public bool IsSetYieldType() 
            { 
                return IsSetField(Tags.YieldType);
            }
            public QuickFix.Fields.Yield Yield
            { 
                get 
                {
                    QuickFix.Fields.Yield val = new QuickFix.Fields.Yield();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.Yield val) 
            { 
                this.Yield = val;
            }
            
            public QuickFix.Fields.Yield Get(QuickFix.Fields.Yield val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.Yield val) 
            { 
                return IsSetYield();
            }
            
            public bool IsSetYield() 
            { 
                return IsSetField(Tags.Yield);
            }
            public QuickFix.Fields.YieldCalcDate YieldCalcDate
            { 
                get 
                {
                    QuickFix.Fields.YieldCalcDate val = new QuickFix.Fields.YieldCalcDate();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.YieldCalcDate val) 
            { 
                this.YieldCalcDate = val;
            }
            
            public QuickFix.Fields.YieldCalcDate Get(QuickFix.Fields.YieldCalcDate val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.YieldCalcDate val) 
            { 
                return IsSetYieldCalcDate();
            }
            
            public bool IsSetYieldCalcDate() 
            { 
                return IsSetField(Tags.YieldCalcDate);
            }
            public QuickFix.Fields.YieldRedemptionDate YieldRedemptionDate
            { 
                get 
                {
                    QuickFix.Fields.YieldRedemptionDate val = new QuickFix.Fields.YieldRedemptionDate();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.YieldRedemptionDate val) 
            { 
                this.YieldRedemptionDate = val;
            }
            
            public QuickFix.Fields.YieldRedemptionDate Get(QuickFix.Fields.YieldRedemptionDate val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.YieldRedemptionDate val) 
            { 
                return IsSetYieldRedemptionDate();
            }
            
            public bool IsSetYieldRedemptionDate() 
            { 
                return IsSetField(Tags.YieldRedemptionDate);
            }
            public QuickFix.Fields.YieldRedemptionPrice YieldRedemptionPrice
            { 
                get 
                {
                    QuickFix.Fields.YieldRedemptionPrice val = new QuickFix.Fields.YieldRedemptionPrice();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.YieldRedemptionPrice val) 
            { 
                this.YieldRedemptionPrice = val;
            }
            
            public QuickFix.Fields.YieldRedemptionPrice Get(QuickFix.Fields.YieldRedemptionPrice val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.YieldRedemptionPrice val) 
            { 
                return IsSetYieldRedemptionPrice();
            }
            
            public bool IsSetYieldRedemptionPrice() 
            { 
                return IsSetField(Tags.YieldRedemptionPrice);
            }
            public QuickFix.Fields.YieldRedemptionPriceType YieldRedemptionPriceType
            { 
                get 
                {
                    QuickFix.Fields.YieldRedemptionPriceType val = new QuickFix.Fields.YieldRedemptionPriceType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.YieldRedemptionPriceType val) 
            { 
                this.YieldRedemptionPriceType = val;
            }
            
            public QuickFix.Fields.YieldRedemptionPriceType Get(QuickFix.Fields.YieldRedemptionPriceType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.YieldRedemptionPriceType val) 
            { 
                return IsSetYieldRedemptionPriceType();
            }
            
            public bool IsSetYieldRedemptionPriceType() 
            { 
                return IsSetField(Tags.YieldRedemptionPriceType);
            }
            public QuickFix.Fields.GrossTradeAmt GrossTradeAmt
            { 
                get 
                {
                    QuickFix.Fields.GrossTradeAmt val = new QuickFix.Fields.GrossTradeAmt();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.GrossTradeAmt val) 
            { 
                this.GrossTradeAmt = val;
            }
            
            public QuickFix.Fields.GrossTradeAmt Get(QuickFix.Fields.GrossTradeAmt val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.GrossTradeAmt val) 
            { 
                return IsSetGrossTradeAmt();
            }
            
            public bool IsSetGrossTradeAmt() 
            { 
                return IsSetField(Tags.GrossTradeAmt);
            }
            public QuickFix.Fields.NumDaysInterest NumDaysInterest
            { 
                get 
                {
                    QuickFix.Fields.NumDaysInterest val = new QuickFix.Fields.NumDaysInterest();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NumDaysInterest val) 
            { 
                this.NumDaysInterest = val;
            }
            
            public QuickFix.Fields.NumDaysInterest Get(QuickFix.Fields.NumDaysInterest val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NumDaysInterest val) 
            { 
                return IsSetNumDaysInterest();
            }
            
            public bool IsSetNumDaysInterest() 
            { 
                return IsSetField(Tags.NumDaysInterest);
            }
            public QuickFix.Fields.ExDate ExDate
            { 
                get 
                {
                    QuickFix.Fields.ExDate val = new QuickFix.Fields.ExDate();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ExDate val) 
            { 
                this.ExDate = val;
            }
            
            public QuickFix.Fields.ExDate Get(QuickFix.Fields.ExDate val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ExDate val) 
            { 
                return IsSetExDate();
            }
            
            public bool IsSetExDate() 
            { 
                return IsSetField(Tags.ExDate);
            }
            public QuickFix.Fields.AccruedInterestRate AccruedInterestRate
            { 
                get 
                {
                    QuickFix.Fields.AccruedInterestRate val = new QuickFix.Fields.AccruedInterestRate();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AccruedInterestRate val) 
            { 
                this.AccruedInterestRate = val;
            }
            
            public QuickFix.Fields.AccruedInterestRate Get(QuickFix.Fields.AccruedInterestRate val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AccruedInterestRate val) 
            { 
                return IsSetAccruedInterestRate();
            }
            
            public bool IsSetAccruedInterestRate() 
            { 
                return IsSetField(Tags.AccruedInterestRate);
            }
            public QuickFix.Fields.AccruedInterestAmt AccruedInterestAmt
            { 
                get 
                {
                    QuickFix.Fields.AccruedInterestAmt val = new QuickFix.Fields.AccruedInterestAmt();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AccruedInterestAmt val) 
            { 
                this.AccruedInterestAmt = val;
            }
            
            public QuickFix.Fields.AccruedInterestAmt Get(QuickFix.Fields.AccruedInterestAmt val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AccruedInterestAmt val) 
            { 
                return IsSetAccruedInterestAmt();
            }
            
            public bool IsSetAccruedInterestAmt() 
            { 
                return IsSetField(Tags.AccruedInterestAmt);
            }
            public QuickFix.Fields.InterestAtMaturity InterestAtMaturity
            { 
                get 
                {
                    QuickFix.Fields.InterestAtMaturity val = new QuickFix.Fields.InterestAtMaturity();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.InterestAtMaturity val) 
            { 
                this.InterestAtMaturity = val;
            }
            
            public QuickFix.Fields.InterestAtMaturity Get(QuickFix.Fields.InterestAtMaturity val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.InterestAtMaturity val) 
            { 
                return IsSetInterestAtMaturity();
            }
            
            public bool IsSetInterestAtMaturity() 
            { 
                return IsSetField(Tags.InterestAtMaturity);
            }
            public QuickFix.Fields.EndAccruedInterestAmt EndAccruedInterestAmt
            { 
                get 
                {
                    QuickFix.Fields.EndAccruedInterestAmt val = new QuickFix.Fields.EndAccruedInterestAmt();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.EndAccruedInterestAmt val) 
            { 
                this.EndAccruedInterestAmt = val;
            }
            
            public QuickFix.Fields.EndAccruedInterestAmt Get(QuickFix.Fields.EndAccruedInterestAmt val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.EndAccruedInterestAmt val) 
            { 
                return IsSetEndAccruedInterestAmt();
            }
            
            public bool IsSetEndAccruedInterestAmt() 
            { 
                return IsSetField(Tags.EndAccruedInterestAmt);
            }
            public QuickFix.Fields.StartCash StartCash
            { 
                get 
                {
                    QuickFix.Fields.StartCash val = new QuickFix.Fields.StartCash();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.StartCash val) 
            { 
                this.StartCash = val;
            }
            
            public QuickFix.Fields.StartCash Get(QuickFix.Fields.StartCash val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.StartCash val) 
            { 
                return IsSetStartCash();
            }
            
            public bool IsSetStartCash() 
            { 
                return IsSetField(Tags.StartCash);
            }
            public QuickFix.Fields.EndCash EndCash
            { 
                get 
                {
                    QuickFix.Fields.EndCash val = new QuickFix.Fields.EndCash();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.EndCash val) 
            { 
                this.EndCash = val;
            }
            
            public QuickFix.Fields.EndCash Get(QuickFix.Fields.EndCash val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.EndCash val) 
            { 
                return IsSetEndCash();
            }
            
            public bool IsSetEndCash() 
            { 
                return IsSetField(Tags.EndCash);
            }
            public QuickFix.Fields.TradedFlatSwitch TradedFlatSwitch
            { 
                get 
                {
                    QuickFix.Fields.TradedFlatSwitch val = new QuickFix.Fields.TradedFlatSwitch();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TradedFlatSwitch val) 
            { 
                this.TradedFlatSwitch = val;
            }
            
            public QuickFix.Fields.TradedFlatSwitch Get(QuickFix.Fields.TradedFlatSwitch val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TradedFlatSwitch val) 
            { 
                return IsSetTradedFlatSwitch();
            }
            
            public bool IsSetTradedFlatSwitch() 
            { 
                return IsSetField(Tags.TradedFlatSwitch);
            }
            public QuickFix.Fields.BasisFeatureDate BasisFeatureDate
            { 
                get 
                {
                    QuickFix.Fields.BasisFeatureDate val = new QuickFix.Fields.BasisFeatureDate();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.BasisFeatureDate val) 
            { 
                this.BasisFeatureDate = val;
            }
            
            public QuickFix.Fields.BasisFeatureDate Get(QuickFix.Fields.BasisFeatureDate val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.BasisFeatureDate val) 
            { 
                return IsSetBasisFeatureDate();
            }
            
            public bool IsSetBasisFeatureDate() 
            { 
                return IsSetField(Tags.BasisFeatureDate);
            }
            public QuickFix.Fields.BasisFeaturePrice BasisFeaturePrice
            { 
                get 
                {
                    QuickFix.Fields.BasisFeaturePrice val = new QuickFix.Fields.BasisFeaturePrice();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.BasisFeaturePrice val) 
            { 
                this.BasisFeaturePrice = val;
            }
            
            public QuickFix.Fields.BasisFeaturePrice Get(QuickFix.Fields.BasisFeaturePrice val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.BasisFeaturePrice val) 
            { 
                return IsSetBasisFeaturePrice();
            }
            
            public bool IsSetBasisFeaturePrice() 
            { 
                return IsSetField(Tags.BasisFeaturePrice);
            }
            public QuickFix.Fields.Concession Concession
            { 
                get 
                {
                    QuickFix.Fields.Concession val = new QuickFix.Fields.Concession();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.Concession val) 
            { 
                this.Concession = val;
            }
            
            public QuickFix.Fields.Concession Get(QuickFix.Fields.Concession val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.Concession val) 
            { 
                return IsSetConcession();
            }
            
            public bool IsSetConcession() 
            { 
                return IsSetField(Tags.Concession);
            }
            public QuickFix.Fields.TotalTakedown TotalTakedown
            { 
                get 
                {
                    QuickFix.Fields.TotalTakedown val = new QuickFix.Fields.TotalTakedown();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TotalTakedown val) 
            { 
                this.TotalTakedown = val;
            }
            
            public QuickFix.Fields.TotalTakedown Get(QuickFix.Fields.TotalTakedown val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TotalTakedown val) 
            { 
                return IsSetTotalTakedown();
            }
            
            public bool IsSetTotalTakedown() 
            { 
                return IsSetField(Tags.TotalTakedown);
            }
            public QuickFix.Fields.NetMoney NetMoney
            { 
                get 
                {
                    QuickFix.Fields.NetMoney val = new QuickFix.Fields.NetMoney();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NetMoney val) 
            { 
                this.NetMoney = val;
            }
            
            public QuickFix.Fields.NetMoney Get(QuickFix.Fields.NetMoney val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NetMoney val) 
            { 
                return IsSetNetMoney();
            }
            
            public bool IsSetNetMoney() 
            { 
                return IsSetField(Tags.NetMoney);
            }
            public QuickFix.Fields.SettlCurrAmt SettlCurrAmt
            { 
                get 
                {
                    QuickFix.Fields.SettlCurrAmt val = new QuickFix.Fields.SettlCurrAmt();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.SettlCurrAmt val) 
            { 
                this.SettlCurrAmt = val;
            }
            
            public QuickFix.Fields.SettlCurrAmt Get(QuickFix.Fields.SettlCurrAmt val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.SettlCurrAmt val) 
            { 
                return IsSetSettlCurrAmt();
            }
            
            public bool IsSetSettlCurrAmt() 
            { 
                return IsSetField(Tags.SettlCurrAmt);
            }
            public QuickFix.Fields.SettlCurrency SettlCurrency
            { 
                get 
                {
                    QuickFix.Fields.SettlCurrency val = new QuickFix.Fields.SettlCurrency();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.SettlCurrency val) 
            { 
                this.SettlCurrency = val;
            }
            
            public QuickFix.Fields.SettlCurrency Get(QuickFix.Fields.SettlCurrency val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.SettlCurrency val) 
            { 
                return IsSetSettlCurrency();
            }
            
            public bool IsSetSettlCurrency() 
            { 
                return IsSetField(Tags.SettlCurrency);
            }
            public QuickFix.Fields.SettlCurrFxRate SettlCurrFxRate
            { 
                get 
                {
                    QuickFix.Fields.SettlCurrFxRate val = new QuickFix.Fields.SettlCurrFxRate();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.SettlCurrFxRate val) 
            { 
                this.SettlCurrFxRate = val;
            }
            
            public QuickFix.Fields.SettlCurrFxRate Get(QuickFix.Fields.SettlCurrFxRate val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.SettlCurrFxRate val) 
            { 
                return IsSetSettlCurrFxRate();
            }
            
            public bool IsSetSettlCurrFxRate() 
            { 
                return IsSetField(Tags.SettlCurrFxRate);
            }
            public QuickFix.Fields.SettlCurrFxRateCalc SettlCurrFxRateCalc
            { 
                get 
                {
                    QuickFix.Fields.SettlCurrFxRateCalc val = new QuickFix.Fields.SettlCurrFxRateCalc();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.SettlCurrFxRateCalc val) 
            { 
                this.SettlCurrFxRateCalc = val;
            }
            
            public QuickFix.Fields.SettlCurrFxRateCalc Get(QuickFix.Fields.SettlCurrFxRateCalc val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.SettlCurrFxRateCalc val) 
            { 
                return IsSetSettlCurrFxRateCalc();
            }
            
            public bool IsSetSettlCurrFxRateCalc() 
            { 
                return IsSetField(Tags.SettlCurrFxRateCalc);
            }
            public QuickFix.Fields.HandlInst HandlInst
            { 
                get 
                {
                    QuickFix.Fields.HandlInst val = new QuickFix.Fields.HandlInst();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.HandlInst val) 
            { 
                this.HandlInst = val;
            }
            
            public QuickFix.Fields.HandlInst Get(QuickFix.Fields.HandlInst val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.HandlInst val) 
            { 
                return IsSetHandlInst();
            }
            
            public bool IsSetHandlInst() 
            { 
                return IsSetField(Tags.HandlInst);
            }
            public QuickFix.Fields.MinQty MinQty
            { 
                get 
                {
                    QuickFix.Fields.MinQty val = new QuickFix.Fields.MinQty();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.MinQty val) 
            { 
                this.MinQty = val;
            }
            
            public QuickFix.Fields.MinQty Get(QuickFix.Fields.MinQty val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.MinQty val) 
            { 
                return IsSetMinQty();
            }
            
            public bool IsSetMinQty() 
            { 
                return IsSetField(Tags.MinQty);
            }
            public QuickFix.Fields.MaxFloor MaxFloor
            { 
                get 
                {
                    QuickFix.Fields.MaxFloor val = new QuickFix.Fields.MaxFloor();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.MaxFloor val) 
            { 
                this.MaxFloor = val;
            }
            
            public QuickFix.Fields.MaxFloor Get(QuickFix.Fields.MaxFloor val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.MaxFloor val) 
            { 
                return IsSetMaxFloor();
            }
            
            public bool IsSetMaxFloor() 
            { 
                return IsSetField(Tags.MaxFloor);
            }
            public QuickFix.Fields.PositionEffect PositionEffect
            { 
                get 
                {
                    QuickFix.Fields.PositionEffect val = new QuickFix.Fields.PositionEffect();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.PositionEffect val) 
            { 
                this.PositionEffect = val;
            }
            
            public QuickFix.Fields.PositionEffect Get(QuickFix.Fields.PositionEffect val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.PositionEffect val) 
            { 
                return IsSetPositionEffect();
            }
            
            public bool IsSetPositionEffect() 
            { 
                return IsSetField(Tags.PositionEffect);
            }
            public QuickFix.Fields.MaxShow MaxShow
            { 
                get 
                {
                    QuickFix.Fields.MaxShow val = new QuickFix.Fields.MaxShow();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.MaxShow val) 
            { 
                this.MaxShow = val;
            }
            
            public QuickFix.Fields.MaxShow Get(QuickFix.Fields.MaxShow val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.MaxShow val) 
            { 
                return IsSetMaxShow();
            }
            
            public bool IsSetMaxShow() 
            { 
                return IsSetField(Tags.MaxShow);
            }
            public QuickFix.Fields.BookingType BookingType
            { 
                get 
                {
                    QuickFix.Fields.BookingType val = new QuickFix.Fields.BookingType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.BookingType val) 
            { 
                this.BookingType = val;
            }
            
            public QuickFix.Fields.BookingType Get(QuickFix.Fields.BookingType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.BookingType val) 
            { 
                return IsSetBookingType();
            }
            
            public bool IsSetBookingType() 
            { 
                return IsSetField(Tags.BookingType);
            }
            public QuickFix.Fields.Text Text
            { 
                get 
                {
                    QuickFix.Fields.Text val = new QuickFix.Fields.Text();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.Text val) 
            { 
                this.Text = val;
            }
            
            public QuickFix.Fields.Text Get(QuickFix.Fields.Text val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.Text val) 
            { 
                return IsSetText();
            }
            
            public bool IsSetText() 
            { 
                return IsSetField(Tags.Text);
            }
            public QuickFix.Fields.EncodedTextLen EncodedTextLen
            { 
                get 
                {
                    QuickFix.Fields.EncodedTextLen val = new QuickFix.Fields.EncodedTextLen();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.EncodedTextLen val) 
            { 
                this.EncodedTextLen = val;
            }
            
            public QuickFix.Fields.EncodedTextLen Get(QuickFix.Fields.EncodedTextLen val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.EncodedTextLen val) 
            { 
                return IsSetEncodedTextLen();
            }
            
            public bool IsSetEncodedTextLen() 
            { 
                return IsSetField(Tags.EncodedTextLen);
            }
            public QuickFix.Fields.EncodedText EncodedText
            { 
                get 
                {
                    QuickFix.Fields.EncodedText val = new QuickFix.Fields.EncodedText();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.EncodedText val) 
            { 
                this.EncodedText = val;
            }
            
            public QuickFix.Fields.EncodedText Get(QuickFix.Fields.EncodedText val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.EncodedText val) 
            { 
                return IsSetEncodedText();
            }
            
            public bool IsSetEncodedText() 
            { 
                return IsSetField(Tags.EncodedText);
            }
            public QuickFix.Fields.SettlDate2 SettlDate2
            { 
                get 
                {
                    QuickFix.Fields.SettlDate2 val = new QuickFix.Fields.SettlDate2();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.SettlDate2 val) 
            { 
                this.SettlDate2 = val;
            }
            
            public QuickFix.Fields.SettlDate2 Get(QuickFix.Fields.SettlDate2 val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.SettlDate2 val) 
            { 
                return IsSetSettlDate2();
            }
            
            public bool IsSetSettlDate2() 
            { 
                return IsSetField(Tags.SettlDate2);
            }
            public QuickFix.Fields.OrderQty2 OrderQty2
            { 
                get 
                {
                    QuickFix.Fields.OrderQty2 val = new QuickFix.Fields.OrderQty2();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.OrderQty2 val) 
            { 
                this.OrderQty2 = val;
            }
            
            public QuickFix.Fields.OrderQty2 Get(QuickFix.Fields.OrderQty2 val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.OrderQty2 val) 
            { 
                return IsSetOrderQty2();
            }
            
            public bool IsSetOrderQty2() 
            { 
                return IsSetField(Tags.OrderQty2);
            }
            public QuickFix.Fields.LastForwardPoints2 LastForwardPoints2
            { 
                get 
                {
                    QuickFix.Fields.LastForwardPoints2 val = new QuickFix.Fields.LastForwardPoints2();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.LastForwardPoints2 val) 
            { 
                this.LastForwardPoints2 = val;
            }
            
            public QuickFix.Fields.LastForwardPoints2 Get(QuickFix.Fields.LastForwardPoints2 val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.LastForwardPoints2 val) 
            { 
                return IsSetLastForwardPoints2();
            }
            
            public bool IsSetLastForwardPoints2() 
            { 
                return IsSetField(Tags.LastForwardPoints2);
            }
            public QuickFix.Fields.MultiLegReportingType MultiLegReportingType
            { 
                get 
                {
                    QuickFix.Fields.MultiLegReportingType val = new QuickFix.Fields.MultiLegReportingType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.MultiLegReportingType val) 
            { 
                this.MultiLegReportingType = val;
            }
            
            public QuickFix.Fields.MultiLegReportingType Get(QuickFix.Fields.MultiLegReportingType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.MultiLegReportingType val) 
            { 
                return IsSetMultiLegReportingType();
            }
            
            public bool IsSetMultiLegReportingType() 
            { 
                return IsSetField(Tags.MultiLegReportingType);
            }
            public QuickFix.Fields.TotNoRelatedSym TotNoRelatedSym
            { 
                get 
                {
                    QuickFix.Fields.TotNoRelatedSym val = new QuickFix.Fields.TotNoRelatedSym();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TotNoRelatedSym val) 
            { 
                this.TotNoRelatedSym = val;
            }
            
            public QuickFix.Fields.TotNoRelatedSym Get(QuickFix.Fields.TotNoRelatedSym val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TotNoRelatedSym val) 
            { 
                return IsSetTotNoRelatedSym();
            }
            
            public bool IsSetTotNoRelatedSym() 
            { 
                return IsSetField(Tags.TotNoRelatedSym);
            }
            public QuickFix.Fields.CancellationRights CancellationRights
            { 
                get 
                {
                    QuickFix.Fields.CancellationRights val = new QuickFix.Fields.CancellationRights();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.CancellationRights val) 
            { 
                this.CancellationRights = val;
            }
            
            public QuickFix.Fields.CancellationRights Get(QuickFix.Fields.CancellationRights val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.CancellationRights val) 
            { 
                return IsSetCancellationRights();
            }
            
            public bool IsSetCancellationRights() 
            { 
                return IsSetField(Tags.CancellationRights);
            }
            public QuickFix.Fields.MoneyLaunderingStatus MoneyLaunderingStatus
            { 
                get 
                {
                    QuickFix.Fields.MoneyLaunderingStatus val = new QuickFix.Fields.MoneyLaunderingStatus();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.MoneyLaunderingStatus val) 
            { 
                this.MoneyLaunderingStatus = val;
            }
            
            public QuickFix.Fields.MoneyLaunderingStatus Get(QuickFix.Fields.MoneyLaunderingStatus val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.MoneyLaunderingStatus val) 
            { 
                return IsSetMoneyLaunderingStatus();
            }
            
            public bool IsSetMoneyLaunderingStatus() 
            { 
                return IsSetField(Tags.MoneyLaunderingStatus);
            }
            public QuickFix.Fields.RegistID RegistID
            { 
                get 
                {
                    QuickFix.Fields.RegistID val = new QuickFix.Fields.RegistID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.RegistID val) 
            { 
                this.RegistID = val;
            }
            
            public QuickFix.Fields.RegistID Get(QuickFix.Fields.RegistID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.RegistID val) 
            { 
                return IsSetRegistID();
            }
            
            public bool IsSetRegistID() 
            { 
                return IsSetField(Tags.RegistID);
            }
            public QuickFix.Fields.Designation Designation
            { 
                get 
                {
                    QuickFix.Fields.Designation val = new QuickFix.Fields.Designation();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.Designation val) 
            { 
                this.Designation = val;
            }
            
            public QuickFix.Fields.Designation Get(QuickFix.Fields.Designation val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.Designation val) 
            { 
                return IsSetDesignation();
            }
            
            public bool IsSetDesignation() 
            { 
                return IsSetField(Tags.Designation);
            }
            public QuickFix.Fields.TransBkdTime TransBkdTime
            { 
                get 
                {
                    QuickFix.Fields.TransBkdTime val = new QuickFix.Fields.TransBkdTime();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TransBkdTime val) 
            { 
                this.TransBkdTime = val;
            }
            
            public QuickFix.Fields.TransBkdTime Get(QuickFix.Fields.TransBkdTime val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TransBkdTime val) 
            { 
                return IsSetTransBkdTime();
            }
            
            public bool IsSetTransBkdTime() 
            { 
                return IsSetField(Tags.TransBkdTime);
            }
            public QuickFix.Fields.ExecValuationPoint ExecValuationPoint
            { 
                get 
                {
                    QuickFix.Fields.ExecValuationPoint val = new QuickFix.Fields.ExecValuationPoint();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ExecValuationPoint val) 
            { 
                this.ExecValuationPoint = val;
            }
            
            public QuickFix.Fields.ExecValuationPoint Get(QuickFix.Fields.ExecValuationPoint val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ExecValuationPoint val) 
            { 
                return IsSetExecValuationPoint();
            }
            
            public bool IsSetExecValuationPoint() 
            { 
                return IsSetField(Tags.ExecValuationPoint);
            }
            public QuickFix.Fields.ExecPriceType ExecPriceType
            { 
                get 
                {
                    QuickFix.Fields.ExecPriceType val = new QuickFix.Fields.ExecPriceType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ExecPriceType val) 
            { 
                this.ExecPriceType = val;
            }
            
            public QuickFix.Fields.ExecPriceType Get(QuickFix.Fields.ExecPriceType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ExecPriceType val) 
            { 
                return IsSetExecPriceType();
            }
            
            public bool IsSetExecPriceType() 
            { 
                return IsSetField(Tags.ExecPriceType);
            }
            public QuickFix.Fields.ExecPriceAdjustment ExecPriceAdjustment
            { 
                get 
                {
                    QuickFix.Fields.ExecPriceAdjustment val = new QuickFix.Fields.ExecPriceAdjustment();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ExecPriceAdjustment val) 
            { 
                this.ExecPriceAdjustment = val;
            }
            
            public QuickFix.Fields.ExecPriceAdjustment Get(QuickFix.Fields.ExecPriceAdjustment val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ExecPriceAdjustment val) 
            { 
                return IsSetExecPriceAdjustment();
            }
            
            public bool IsSetExecPriceAdjustment() 
            { 
                return IsSetField(Tags.ExecPriceAdjustment);
            }
            public QuickFix.Fields.PriorityIndicator PriorityIndicator
            { 
                get 
                {
                    QuickFix.Fields.PriorityIndicator val = new QuickFix.Fields.PriorityIndicator();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.PriorityIndicator val) 
            { 
                this.PriorityIndicator = val;
            }
            
            public QuickFix.Fields.PriorityIndicator Get(QuickFix.Fields.PriorityIndicator val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.PriorityIndicator val) 
            { 
                return IsSetPriorityIndicator();
            }
            
            public bool IsSetPriorityIndicator() 
            { 
                return IsSetField(Tags.PriorityIndicator);
            }
            public QuickFix.Fields.PriceImprovement PriceImprovement
            { 
                get 
                {
                    QuickFix.Fields.PriceImprovement val = new QuickFix.Fields.PriceImprovement();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.PriceImprovement val) 
            { 
                this.PriceImprovement = val;
            }
            
            public QuickFix.Fields.PriceImprovement Get(QuickFix.Fields.PriceImprovement val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.PriceImprovement val) 
            { 
                return IsSetPriceImprovement();
            }
            
            public bool IsSetPriceImprovement() 
            { 
                return IsSetField(Tags.PriceImprovement);
            }
            public QuickFix.Fields.LastLiquidityInd LastLiquidityInd
            { 
                get 
                {
                    QuickFix.Fields.LastLiquidityInd val = new QuickFix.Fields.LastLiquidityInd();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.LastLiquidityInd val) 
            { 
                this.LastLiquidityInd = val;
            }
            
            public QuickFix.Fields.LastLiquidityInd Get(QuickFix.Fields.LastLiquidityInd val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.LastLiquidityInd val) 
            { 
                return IsSetLastLiquidityInd();
            }
            
            public bool IsSetLastLiquidityInd() 
            { 
                return IsSetField(Tags.LastLiquidityInd);
            }
            public QuickFix.Fields.NoContAmts NoContAmts
            { 
                get 
                {
                    QuickFix.Fields.NoContAmts val = new QuickFix.Fields.NoContAmts();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoContAmts val) 
            { 
                this.NoContAmts = val;
            }
            
            public QuickFix.Fields.NoContAmts Get(QuickFix.Fields.NoContAmts val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoContAmts val) 
            { 
                return IsSetNoContAmts();
            }
            
            public bool IsSetNoContAmts() 
            { 
                return IsSetField(Tags.NoContAmts);
            }
            public QuickFix.Fields.CopyMsgIndicator CopyMsgIndicator
            { 
                get 
                {
                    QuickFix.Fields.CopyMsgIndicator val = new QuickFix.Fields.CopyMsgIndicator();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.CopyMsgIndicator val) 
            { 
                this.CopyMsgIndicator = val;
            }
            
            public QuickFix.Fields.CopyMsgIndicator Get(QuickFix.Fields.CopyMsgIndicator val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.CopyMsgIndicator val) 
            { 
                return IsSetCopyMsgIndicator();
            }
            
            public bool IsSetCopyMsgIndicator() 
            { 
                return IsSetField(Tags.CopyMsgIndicator);
            }
            public QuickFix.Fields.NoMiscFees NoMiscFees
            { 
                get 
                {
                    QuickFix.Fields.NoMiscFees val = new QuickFix.Fields.NoMiscFees();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoMiscFees val) 
            { 
                this.NoMiscFees = val;
            }
            
            public QuickFix.Fields.NoMiscFees Get(QuickFix.Fields.NoMiscFees val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoMiscFees val) 
            { 
                return IsSetNoMiscFees();
            }
            
            public bool IsSetNoMiscFees() 
            { 
                return IsSetField(Tags.NoMiscFees);
            }
            public QuickFix.Fields.UniqueTradeID UniqueTradeID
            { 
                get 
                {
                    QuickFix.Fields.UniqueTradeID val = new QuickFix.Fields.UniqueTradeID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.UniqueTradeID val) 
            { 
                this.UniqueTradeID = val;
            }
            
            public QuickFix.Fields.UniqueTradeID Get(QuickFix.Fields.UniqueTradeID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.UniqueTradeID val) 
            { 
                return IsSetUniqueTradeID();
            }
            
            public bool IsSetUniqueTradeID() 
            { 
                return IsSetField(Tags.UniqueTradeID);
            }
            public QuickFix.Fields.TradeLinkID TradeLinkID
            { 
                get 
                {
                    QuickFix.Fields.TradeLinkID val = new QuickFix.Fields.TradeLinkID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TradeLinkID val) 
            { 
                this.TradeLinkID = val;
            }
            
            public QuickFix.Fields.TradeLinkID Get(QuickFix.Fields.TradeLinkID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TradeLinkID val) 
            { 
                return IsSetTradeLinkID();
            }
            
            public bool IsSetTradeLinkID() 
            { 
                return IsSetField(Tags.TradeLinkID);
            }
            public QuickFix.Fields.OrderLinkID OrderLinkID
            { 
                get 
                {
                    QuickFix.Fields.OrderLinkID val = new QuickFix.Fields.OrderLinkID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.OrderLinkID val) 
            { 
                this.OrderLinkID = val;
            }
            
            public QuickFix.Fields.OrderLinkID Get(QuickFix.Fields.OrderLinkID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.OrderLinkID val) 
            { 
                return IsSetOrderLinkID();
            }
            
            public bool IsSetOrderLinkID() 
            { 
                return IsSetField(Tags.OrderLinkID);
            }
            public QuickFix.Fields.ExchangeQuoteID ExchangeQuoteID
            { 
                get 
                {
                    QuickFix.Fields.ExchangeQuoteID val = new QuickFix.Fields.ExchangeQuoteID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ExchangeQuoteID val) 
            { 
                this.ExchangeQuoteID = val;
            }
            
            public QuickFix.Fields.ExchangeQuoteID Get(QuickFix.Fields.ExchangeQuoteID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ExchangeQuoteID val) 
            { 
                return IsSetExchangeQuoteID();
            }
            
            public bool IsSetExchangeQuoteID() 
            { 
                return IsSetField(Tags.ExchangeQuoteID);
            }
            public QuickFix.Fields.ExchangeOrderID ExchangeOrderID
            { 
                get 
                {
                    QuickFix.Fields.ExchangeOrderID val = new QuickFix.Fields.ExchangeOrderID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ExchangeOrderID val) 
            { 
                this.ExchangeOrderID = val;
            }
            
            public QuickFix.Fields.ExchangeOrderID Get(QuickFix.Fields.ExchangeOrderID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ExchangeOrderID val) 
            { 
                return IsSetExchangeOrderID();
            }
            
            public bool IsSetExchangeOrderID() 
            { 
                return IsSetField(Tags.ExchangeOrderID);
            }
            public QuickFix.Fields.ExchangeExecID ExchangeExecID
            { 
                get 
                {
                    QuickFix.Fields.ExchangeExecID val = new QuickFix.Fields.ExchangeExecID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ExchangeExecID val) 
            { 
                this.ExchangeExecID = val;
            }
            
            public QuickFix.Fields.ExchangeExecID Get(QuickFix.Fields.ExchangeExecID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ExchangeExecID val) 
            { 
                return IsSetExchangeExecID();
            }
            
            public bool IsSetExchangeExecID() 
            { 
                return IsSetField(Tags.ExchangeExecID);
            }
            public QuickFix.Fields.LastPxInIssuedCurrency LastPxInIssuedCurrency
            { 
                get 
                {
                    QuickFix.Fields.LastPxInIssuedCurrency val = new QuickFix.Fields.LastPxInIssuedCurrency();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.LastPxInIssuedCurrency val) 
            { 
                this.LastPxInIssuedCurrency = val;
            }
            
            public QuickFix.Fields.LastPxInIssuedCurrency Get(QuickFix.Fields.LastPxInIssuedCurrency val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.LastPxInIssuedCurrency val) 
            { 
                return IsSetLastPxInIssuedCurrency();
            }
            
            public bool IsSetLastPxInIssuedCurrency() 
            { 
                return IsSetField(Tags.LastPxInIssuedCurrency);
            }
            public QuickFix.Fields.PriceInIssuedCurrency PriceInIssuedCurrency
            { 
                get 
                {
                    QuickFix.Fields.PriceInIssuedCurrency val = new QuickFix.Fields.PriceInIssuedCurrency();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.PriceInIssuedCurrency val) 
            { 
                this.PriceInIssuedCurrency = val;
            }
            
            public QuickFix.Fields.PriceInIssuedCurrency Get(QuickFix.Fields.PriceInIssuedCurrency val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.PriceInIssuedCurrency val) 
            { 
                return IsSetPriceInIssuedCurrency();
            }
            
            public bool IsSetPriceInIssuedCurrency() 
            { 
                return IsSetField(Tags.PriceInIssuedCurrency);
            }
            public QuickFix.Fields.ExchangeSecondaryOrderID ExchangeSecondaryOrderID
            { 
                get 
                {
                    QuickFix.Fields.ExchangeSecondaryOrderID val = new QuickFix.Fields.ExchangeSecondaryOrderID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ExchangeSecondaryOrderID val) 
            { 
                this.ExchangeSecondaryOrderID = val;
            }
            
            public QuickFix.Fields.ExchangeSecondaryOrderID Get(QuickFix.Fields.ExchangeSecondaryOrderID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ExchangeSecondaryOrderID val) 
            { 
                return IsSetExchangeSecondaryOrderID();
            }
            
            public bool IsSetExchangeSecondaryOrderID() 
            { 
                return IsSetField(Tags.ExchangeSecondaryOrderID);
            }
            public QuickFix.Fields.OrigOrdQty OrigOrdQty
            { 
                get 
                {
                    QuickFix.Fields.OrigOrdQty val = new QuickFix.Fields.OrigOrdQty();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.OrigOrdQty val) 
            { 
                this.OrigOrdQty = val;
            }
            
            public QuickFix.Fields.OrigOrdQty Get(QuickFix.Fields.OrigOrdQty val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.OrigOrdQty val) 
            { 
                return IsSetOrigOrdQty();
            }
            
            public bool IsSetOrigOrdQty() 
            { 
                return IsSetField(Tags.OrigOrdQty);
            }
            public QuickFix.Fields.OrderCategory OrderCategory
            { 
                get 
                {
                    QuickFix.Fields.OrderCategory val = new QuickFix.Fields.OrderCategory();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.OrderCategory val) 
            { 
                this.OrderCategory = val;
            }
            
            public QuickFix.Fields.OrderCategory Get(QuickFix.Fields.OrderCategory val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.OrderCategory val) 
            { 
                return IsSetOrderCategory();
            }
            
            public bool IsSetOrderCategory() 
            { 
                return IsSetField(Tags.OrderCategory);
            }
            public QuickFix.Fields.Memo Memo
            { 
                get 
                {
                    QuickFix.Fields.Memo val = new QuickFix.Fields.Memo();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.Memo val) 
            { 
                this.Memo = val;
            }
            
            public QuickFix.Fields.Memo Get(QuickFix.Fields.Memo val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.Memo val) 
            { 
                return IsSetMemo();
            }
            
            public bool IsSetMemo() 
            { 
                return IsSetField(Tags.Memo);
            }
            public class NoPartyIDsGroup : Group
            {
                public static int[] fieldOrder = {Tags.PartyID, Tags.PartyIDSource, Tags.PartyRole, Tags.NoPartySubIDs, 0};
            
                public NoPartyIDsGroup() 
                  :base( Tags.NoPartyIDs, Tags.PartyID, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoPartyIDsGroup();
                    clone.CopyStateFrom(this);
                    return clone;
                }
            
                            public QuickFix.Fields.PartyID PartyID
                { 
                    get 
                    {
                        QuickFix.Fields.PartyID val = new QuickFix.Fields.PartyID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.PartyID val) 
                { 
                    this.PartyID = val;
                }
                
                public QuickFix.Fields.PartyID Get(QuickFix.Fields.PartyID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.PartyID val) 
                { 
                    return IsSetPartyID();
                }
                
                public bool IsSetPartyID() 
                { 
                    return IsSetField(Tags.PartyID);
                }
                public QuickFix.Fields.PartyIDSource PartyIDSource
                { 
                    get 
                    {
                        QuickFix.Fields.PartyIDSource val = new QuickFix.Fields.PartyIDSource();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.PartyIDSource val) 
                { 
                    this.PartyIDSource = val;
                }
                
                public QuickFix.Fields.PartyIDSource Get(QuickFix.Fields.PartyIDSource val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.PartyIDSource val) 
                { 
                    return IsSetPartyIDSource();
                }
                
                public bool IsSetPartyIDSource() 
                { 
                    return IsSetField(Tags.PartyIDSource);
                }
                public QuickFix.Fields.PartyRole PartyRole
                { 
                    get 
                    {
                        QuickFix.Fields.PartyRole val = new QuickFix.Fields.PartyRole();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.PartyRole val) 
                { 
                    this.PartyRole = val;
                }
                
                public QuickFix.Fields.PartyRole Get(QuickFix.Fields.PartyRole val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.PartyRole val) 
                { 
                    return IsSetPartyRole();
                }
                
                public bool IsSetPartyRole() 
                { 
                    return IsSetField(Tags.PartyRole);
                }
                public QuickFix.Fields.NoPartySubIDs NoPartySubIDs
                { 
                    get 
                    {
                        QuickFix.Fields.NoPartySubIDs val = new QuickFix.Fields.NoPartySubIDs();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.NoPartySubIDs val) 
                { 
                    this.NoPartySubIDs = val;
                }
                
                public QuickFix.Fields.NoPartySubIDs Get(QuickFix.Fields.NoPartySubIDs val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.NoPartySubIDs val) 
                { 
                    return IsSetNoPartySubIDs();
                }
                
                public bool IsSetNoPartySubIDs() 
                { 
                    return IsSetField(Tags.NoPartySubIDs);
                }
                            public class NoPartySubIDsGroup : Group
                {
                    public static int[] fieldOrder = {Tags.PartySubID, Tags.PartySubIDType, 0};
                
                    public NoPartySubIDsGroup() 
                      :base( Tags.NoPartySubIDs, Tags.PartySubID, fieldOrder)
                    {
                    }
                
                    public override Group Clone()
                    {
                        var clone = new NoPartySubIDsGroup();
                        clone.CopyStateFrom(this);
                        return clone;
                    }
                
                                    public QuickFix.Fields.PartySubID PartySubID
                    { 
                        get 
                        {
                            QuickFix.Fields.PartySubID val = new QuickFix.Fields.PartySubID();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.PartySubID val) 
                    { 
                        this.PartySubID = val;
                    }
                    
                    public QuickFix.Fields.PartySubID Get(QuickFix.Fields.PartySubID val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.PartySubID val) 
                    { 
                        return IsSetPartySubID();
                    }
                    
                    public bool IsSetPartySubID() 
                    { 
                        return IsSetField(Tags.PartySubID);
                    }
                    public QuickFix.Fields.PartySubIDType PartySubIDType
                    { 
                        get 
                        {
                            QuickFix.Fields.PartySubIDType val = new QuickFix.Fields.PartySubIDType();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.PartySubIDType val) 
                    { 
                        this.PartySubIDType = val;
                    }
                    
                    public QuickFix.Fields.PartySubIDType Get(QuickFix.Fields.PartySubIDType val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.PartySubIDType val) 
                    { 
                        return IsSetPartySubIDType();
                    }
                    
                    public bool IsSetPartySubIDType() 
                    { 
                        return IsSetField(Tags.PartySubIDType);
                    }
                
                }
            }
            public class NoContraBrokersGroup : Group
            {
                public static int[] fieldOrder = {Tags.ContraBroker, Tags.ContraTrader, Tags.ContraTradeQty, Tags.ContraTradeTime, Tags.ContraLegRefID, 0};
            
                public NoContraBrokersGroup() 
                  :base( Tags.NoContraBrokers, Tags.ContraBroker, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoContraBrokersGroup();
                    clone.CopyStateFrom(this);
                    return clone;
                }
            
                            public QuickFix.Fields.ContraBroker ContraBroker
                { 
                    get 
                    {
                        QuickFix.Fields.ContraBroker val = new QuickFix.Fields.ContraBroker();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.ContraBroker val) 
                { 
                    this.ContraBroker = val;
                }
                
                public QuickFix.Fields.ContraBroker Get(QuickFix.Fields.ContraBroker val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.ContraBroker val) 
                { 
                    return IsSetContraBroker();
                }
                
                public bool IsSetContraBroker() 
                { 
                    return IsSetField(Tags.ContraBroker);
                }
                public QuickFix.Fields.ContraTrader ContraTrader
                { 
                    get 
                    {
                        QuickFix.Fields.ContraTrader val = new QuickFix.Fields.ContraTrader();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.ContraTrader val) 
                { 
                    this.ContraTrader = val;
                }
                
                public QuickFix.Fields.ContraTrader Get(QuickFix.Fields.ContraTrader val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.ContraTrader val) 
                { 
                    return IsSetContraTrader();
                }
                
                public bool IsSetContraTrader() 
                { 
                    return IsSetField(Tags.ContraTrader);
                }
                public QuickFix.Fields.ContraTradeQty ContraTradeQty
                { 
                    get 
                    {
                        QuickFix.Fields.ContraTradeQty val = new QuickFix.Fields.ContraTradeQty();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.ContraTradeQty val) 
                { 
                    this.ContraTradeQty = val;
                }
                
                public QuickFix.Fields.ContraTradeQty Get(QuickFix.Fields.ContraTradeQty val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.ContraTradeQty val) 
                { 
                    return IsSetContraTradeQty();
                }
                
                public bool IsSetContraTradeQty() 
                { 
                    return IsSetField(Tags.ContraTradeQty);
                }
                public QuickFix.Fields.ContraTradeTime ContraTradeTime
                { 
                    get 
                    {
                        QuickFix.Fields.ContraTradeTime val = new QuickFix.Fields.ContraTradeTime();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.ContraTradeTime val) 
                { 
                    this.ContraTradeTime = val;
                }
                
                public QuickFix.Fields.ContraTradeTime Get(QuickFix.Fields.ContraTradeTime val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.ContraTradeTime val) 
                { 
                    return IsSetContraTradeTime();
                }
                
                public bool IsSetContraTradeTime() 
                { 
                    return IsSetField(Tags.ContraTradeTime);
                }
                public QuickFix.Fields.ContraLegRefID ContraLegRefID
                { 
                    get 
                    {
                        QuickFix.Fields.ContraLegRefID val = new QuickFix.Fields.ContraLegRefID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.ContraLegRefID val) 
                { 
                    this.ContraLegRefID = val;
                }
                
                public QuickFix.Fields.ContraLegRefID Get(QuickFix.Fields.ContraLegRefID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.ContraLegRefID val) 
                { 
                    return IsSetContraLegRefID();
                }
                
                public bool IsSetContraLegRefID() 
                { 
                    return IsSetField(Tags.ContraLegRefID);
                }
            
            }
            public class NoUnderlyingsGroup : Group
            {
                public static int[] fieldOrder = {Tags.UnderlyingSymbol, Tags.UnderlyingSecurityID, Tags.UnderlyingSecurityIDSource, Tags.UnderlyingSecurityExchange, 0};
            
                public NoUnderlyingsGroup() 
                  :base( Tags.NoUnderlyings, Tags.UnderlyingSymbol, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoUnderlyingsGroup();
                    clone.CopyStateFrom(this);
                    return clone;
                }
            
                            public QuickFix.Fields.UnderlyingSymbol UnderlyingSymbol
                { 
                    get 
                    {
                        QuickFix.Fields.UnderlyingSymbol val = new QuickFix.Fields.UnderlyingSymbol();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.UnderlyingSymbol val) 
                { 
                    this.UnderlyingSymbol = val;
                }
                
                public QuickFix.Fields.UnderlyingSymbol Get(QuickFix.Fields.UnderlyingSymbol val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.UnderlyingSymbol val) 
                { 
                    return IsSetUnderlyingSymbol();
                }
                
                public bool IsSetUnderlyingSymbol() 
                { 
                    return IsSetField(Tags.UnderlyingSymbol);
                }
                public QuickFix.Fields.UnderlyingSecurityID UnderlyingSecurityID
                { 
                    get 
                    {
                        QuickFix.Fields.UnderlyingSecurityID val = new QuickFix.Fields.UnderlyingSecurityID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.UnderlyingSecurityID val) 
                { 
                    this.UnderlyingSecurityID = val;
                }
                
                public QuickFix.Fields.UnderlyingSecurityID Get(QuickFix.Fields.UnderlyingSecurityID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.UnderlyingSecurityID val) 
                { 
                    return IsSetUnderlyingSecurityID();
                }
                
                public bool IsSetUnderlyingSecurityID() 
                { 
                    return IsSetField(Tags.UnderlyingSecurityID);
                }
                public QuickFix.Fields.UnderlyingSecurityIDSource UnderlyingSecurityIDSource
                { 
                    get 
                    {
                        QuickFix.Fields.UnderlyingSecurityIDSource val = new QuickFix.Fields.UnderlyingSecurityIDSource();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.UnderlyingSecurityIDSource val) 
                { 
                    this.UnderlyingSecurityIDSource = val;
                }
                
                public QuickFix.Fields.UnderlyingSecurityIDSource Get(QuickFix.Fields.UnderlyingSecurityIDSource val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.UnderlyingSecurityIDSource val) 
                { 
                    return IsSetUnderlyingSecurityIDSource();
                }
                
                public bool IsSetUnderlyingSecurityIDSource() 
                { 
                    return IsSetField(Tags.UnderlyingSecurityIDSource);
                }
                public QuickFix.Fields.UnderlyingSecurityExchange UnderlyingSecurityExchange
                { 
                    get 
                    {
                        QuickFix.Fields.UnderlyingSecurityExchange val = new QuickFix.Fields.UnderlyingSecurityExchange();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.UnderlyingSecurityExchange val) 
                { 
                    this.UnderlyingSecurityExchange = val;
                }
                
                public QuickFix.Fields.UnderlyingSecurityExchange Get(QuickFix.Fields.UnderlyingSecurityExchange val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.UnderlyingSecurityExchange val) 
                { 
                    return IsSetUnderlyingSecurityExchange();
                }
                
                public bool IsSetUnderlyingSecurityExchange() 
                { 
                    return IsSetField(Tags.UnderlyingSecurityExchange);
                }
            
            }
            public class NoStipulationsGroup : Group
            {
                public static int[] fieldOrder = {Tags.StipulationType, Tags.StipulationValue, 0};
            
                public NoStipulationsGroup() 
                  :base( Tags.NoStipulations, Tags.StipulationType, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoStipulationsGroup();
                    clone.CopyStateFrom(this);
                    return clone;
                }
            
                            public QuickFix.Fields.StipulationType StipulationType
                { 
                    get 
                    {
                        QuickFix.Fields.StipulationType val = new QuickFix.Fields.StipulationType();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.StipulationType val) 
                { 
                    this.StipulationType = val;
                }
                
                public QuickFix.Fields.StipulationType Get(QuickFix.Fields.StipulationType val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.StipulationType val) 
                { 
                    return IsSetStipulationType();
                }
                
                public bool IsSetStipulationType() 
                { 
                    return IsSetField(Tags.StipulationType);
                }
                public QuickFix.Fields.StipulationValue StipulationValue
                { 
                    get 
                    {
                        QuickFix.Fields.StipulationValue val = new QuickFix.Fields.StipulationValue();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.StipulationValue val) 
                { 
                    this.StipulationValue = val;
                }
                
                public QuickFix.Fields.StipulationValue Get(QuickFix.Fields.StipulationValue val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.StipulationValue val) 
                { 
                    return IsSetStipulationValue();
                }
                
                public bool IsSetStipulationValue() 
                { 
                    return IsSetField(Tags.StipulationValue);
                }
            
            }
            public class NoContAmtsGroup : Group
            {
                public static int[] fieldOrder = {Tags.ContAmtType, Tags.ContAmtValue, Tags.ContAmtCurr, 0};
            
                public NoContAmtsGroup() 
                  :base( Tags.NoContAmts, Tags.ContAmtType, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoContAmtsGroup();
                    clone.CopyStateFrom(this);
                    return clone;
                }
            
                            public QuickFix.Fields.ContAmtType ContAmtType
                { 
                    get 
                    {
                        QuickFix.Fields.ContAmtType val = new QuickFix.Fields.ContAmtType();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.ContAmtType val) 
                { 
                    this.ContAmtType = val;
                }
                
                public QuickFix.Fields.ContAmtType Get(QuickFix.Fields.ContAmtType val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.ContAmtType val) 
                { 
                    return IsSetContAmtType();
                }
                
                public bool IsSetContAmtType() 
                { 
                    return IsSetField(Tags.ContAmtType);
                }
                public QuickFix.Fields.ContAmtValue ContAmtValue
                { 
                    get 
                    {
                        QuickFix.Fields.ContAmtValue val = new QuickFix.Fields.ContAmtValue();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.ContAmtValue val) 
                { 
                    this.ContAmtValue = val;
                }
                
                public QuickFix.Fields.ContAmtValue Get(QuickFix.Fields.ContAmtValue val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.ContAmtValue val) 
                { 
                    return IsSetContAmtValue();
                }
                
                public bool IsSetContAmtValue() 
                { 
                    return IsSetField(Tags.ContAmtValue);
                }
                public QuickFix.Fields.ContAmtCurr ContAmtCurr
                { 
                    get 
                    {
                        QuickFix.Fields.ContAmtCurr val = new QuickFix.Fields.ContAmtCurr();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.ContAmtCurr val) 
                { 
                    this.ContAmtCurr = val;
                }
                
                public QuickFix.Fields.ContAmtCurr Get(QuickFix.Fields.ContAmtCurr val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.ContAmtCurr val) 
                { 
                    return IsSetContAmtCurr();
                }
                
                public bool IsSetContAmtCurr() 
                { 
                    return IsSetField(Tags.ContAmtCurr);
                }
            
            }
            public class NoMiscFeesGroup : Group
            {
                public static int[] fieldOrder = {Tags.MiscFeeAmt, Tags.MiscFeeCurr, Tags.MiscFeeType, Tags.MiscFeeBasis, 0};
            
                public NoMiscFeesGroup() 
                  :base( Tags.NoMiscFees, Tags.MiscFeeAmt, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoMiscFeesGroup();
                    clone.CopyStateFrom(this);
                    return clone;
                }
            
                            public QuickFix.Fields.MiscFeeAmt MiscFeeAmt
                { 
                    get 
                    {
                        QuickFix.Fields.MiscFeeAmt val = new QuickFix.Fields.MiscFeeAmt();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MiscFeeAmt val) 
                { 
                    this.MiscFeeAmt = val;
                }
                
                public QuickFix.Fields.MiscFeeAmt Get(QuickFix.Fields.MiscFeeAmt val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MiscFeeAmt val) 
                { 
                    return IsSetMiscFeeAmt();
                }
                
                public bool IsSetMiscFeeAmt() 
                { 
                    return IsSetField(Tags.MiscFeeAmt);
                }
                public QuickFix.Fields.MiscFeeCurr MiscFeeCurr
                { 
                    get 
                    {
                        QuickFix.Fields.MiscFeeCurr val = new QuickFix.Fields.MiscFeeCurr();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MiscFeeCurr val) 
                { 
                    this.MiscFeeCurr = val;
                }
                
                public QuickFix.Fields.MiscFeeCurr Get(QuickFix.Fields.MiscFeeCurr val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MiscFeeCurr val) 
                { 
                    return IsSetMiscFeeCurr();
                }
                
                public bool IsSetMiscFeeCurr() 
                { 
                    return IsSetField(Tags.MiscFeeCurr);
                }
                public QuickFix.Fields.MiscFeeType MiscFeeType
                { 
                    get 
                    {
                        QuickFix.Fields.MiscFeeType val = new QuickFix.Fields.MiscFeeType();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MiscFeeType val) 
                { 
                    this.MiscFeeType = val;
                }
                
                public QuickFix.Fields.MiscFeeType Get(QuickFix.Fields.MiscFeeType val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MiscFeeType val) 
                { 
                    return IsSetMiscFeeType();
                }
                
                public bool IsSetMiscFeeType() 
                { 
                    return IsSetField(Tags.MiscFeeType);
                }
                public QuickFix.Fields.MiscFeeBasis MiscFeeBasis
                { 
                    get 
                    {
                        QuickFix.Fields.MiscFeeBasis val = new QuickFix.Fields.MiscFeeBasis();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MiscFeeBasis val) 
                { 
                    this.MiscFeeBasis = val;
                }
                
                public QuickFix.Fields.MiscFeeBasis Get(QuickFix.Fields.MiscFeeBasis val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MiscFeeBasis val) 
                { 
                    return IsSetMiscFeeBasis();
                }
                
                public bool IsSetMiscFeeBasis() 
                { 
                    return IsSetField(Tags.MiscFeeBasis);
                }
            
            }
        }
    }
}
