// This is a generated file.  Don't edit it directly!

using QuickFix.Fields;
namespace QuickFix
{
    namespace FIX44 
    {
        public class MarketDataIncrementalRefresh : Message
        {
            public const string MsgType = "X";

            public MarketDataIncrementalRefresh() : base()
            {
                this.Header.SetField(new QuickFix.Fields.MsgType("X"));
            }


            public QuickFix.Fields.MDReqID MDReqID
            { 
                get 
                {
                    QuickFix.Fields.MDReqID val = new QuickFix.Fields.MDReqID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.MDReqID val) 
            { 
                this.MDReqID = val;
            }
            
            public QuickFix.Fields.MDReqID Get(QuickFix.Fields.MDReqID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.MDReqID val) 
            { 
                return IsSetMDReqID();
            }
            
            public bool IsSetMDReqID() 
            { 
                return IsSetField(Tags.MDReqID);
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
            public QuickFix.Fields.MDBookType MDBookType
            { 
                get 
                {
                    QuickFix.Fields.MDBookType val = new QuickFix.Fields.MDBookType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.MDBookType val) 
            { 
                this.MDBookType = val;
            }
            
            public QuickFix.Fields.MDBookType Get(QuickFix.Fields.MDBookType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.MDBookType val) 
            { 
                return IsSetMDBookType();
            }
            
            public bool IsSetMDBookType() 
            { 
                return IsSetField(Tags.MDBookType);
            }
            public QuickFix.Fields.MarketDepth MarketDepth
            { 
                get 
                {
                    QuickFix.Fields.MarketDepth val = new QuickFix.Fields.MarketDepth();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.MarketDepth val) 
            { 
                this.MarketDepth = val;
            }
            
            public QuickFix.Fields.MarketDepth Get(QuickFix.Fields.MarketDepth val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.MarketDepth val) 
            { 
                return IsSetMarketDepth();
            }
            
            public bool IsSetMarketDepth() 
            { 
                return IsSetField(Tags.MarketDepth);
            }
            public QuickFix.Fields.NoMDEntries NoMDEntries
            { 
                get 
                {
                    QuickFix.Fields.NoMDEntries val = new QuickFix.Fields.NoMDEntries();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoMDEntries val) 
            { 
                this.NoMDEntries = val;
            }
            
            public QuickFix.Fields.NoMDEntries Get(QuickFix.Fields.NoMDEntries val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoMDEntries val) 
            { 
                return IsSetNoMDEntries();
            }
            
            public bool IsSetNoMDEntries() 
            { 
                return IsSetField(Tags.NoMDEntries);
            }
            public QuickFix.Fields.ApplQueueDepth ApplQueueDepth
            { 
                get 
                {
                    QuickFix.Fields.ApplQueueDepth val = new QuickFix.Fields.ApplQueueDepth();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ApplQueueDepth val) 
            { 
                this.ApplQueueDepth = val;
            }
            
            public QuickFix.Fields.ApplQueueDepth Get(QuickFix.Fields.ApplQueueDepth val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ApplQueueDepth val) 
            { 
                return IsSetApplQueueDepth();
            }
            
            public bool IsSetApplQueueDepth() 
            { 
                return IsSetField(Tags.ApplQueueDepth);
            }
            public QuickFix.Fields.ApplQueueResolution ApplQueueResolution
            { 
                get 
                {
                    QuickFix.Fields.ApplQueueResolution val = new QuickFix.Fields.ApplQueueResolution();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ApplQueueResolution val) 
            { 
                this.ApplQueueResolution = val;
            }
            
            public QuickFix.Fields.ApplQueueResolution Get(QuickFix.Fields.ApplQueueResolution val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ApplQueueResolution val) 
            { 
                return IsSetApplQueueResolution();
            }
            
            public bool IsSetApplQueueResolution() 
            { 
                return IsSetField(Tags.ApplQueueResolution);
            }
            public class NoMDEntriesGroup : Group
            {
                public static int[] fieldOrder = {Tags.MDUpdateAction, Tags.DeleteReason, Tags.MDEntryType, Tags.MDEntryID, Tags.MDEntryRefID, Tags.Symbol, Tags.SymbolSfx, Tags.SecurityID, Tags.SecurityIDSource, Tags.SecurityExchange, Tags.InstrumentID, Tags.PutOrCall, Tags.NoSecurityAltID, Tags.Product, Tags.CFICode, Tags.SecurityGroupTMR, Tags.SecurityType, Tags.SecuritySubType, Tags.MaturityMonthYear, Tags.MaturityDate, Tags.CouponPaymentDate, Tags.IssueDate, Tags.RepoCollateralSecurityType, Tags.RepurchaseTerm, Tags.RepurchaseRate, Tags.Factor, Tags.CreditRating, Tags.InstrRegistry, Tags.CountryOfIssue, Tags.StateOrProvinceOfIssue, Tags.LocaleOfIssue, Tags.RedemptionDate, Tags.StrikePrice, Tags.StrikeCurrency, Tags.ExerciseStyle, Tags.OptAttribute, Tags.ContractMultiplier, Tags.CouponRate, Tags.Issuer, Tags.EncodedIssuerLen, Tags.EncodedIssuer, Tags.SecurityDesc, Tags.EncodedSecurityDescLen, Tags.EncodedSecurityDesc, Tags.Pool, Tags.ContractSettlMonth, Tags.CPProgram, Tags.CPRegType, Tags.NoEvents, Tags.DatedDate, Tags.InterestAccrualDate, Tags.SettlType, Tags.SettlDate, Tags.MDStreamID, Tags.NoUnderlyings, Tags.NoLegs, Tags.FinancialStatus, Tags.CorporateAction, Tags.MDEntryPx, Tags.MDEntryInterestRate, Tags.LastTradeDate, Tags.PriceAdjustmentMethod, Tags.PriceBandType, Tags.PriceLimitType, Tags.LowLimitPrice, Tags.HighLimitPrice, Tags.TradingReferencePrice, Tags.PriceBandMidpointPriceType, Tags.AvgDailyTradedQty, Tags.StopPx, Tags.Currency, Tags.MDEntrySize, Tags.MDEntryDate, Tags.MDEntryTime, Tags.MDInsertDate, Tags.MDInsertTime, Tags.TickDirection, Tags.MDMkt, Tags.TradingSessionID, Tags.TradingSessionSubID, Tags.QuoteCondition, Tags.TradeCondition, Tags.MDEntryOriginator, Tags.LocationID, Tags.DeskID, Tags.OpenCloseSettlFlag, Tags.TimeInForce, Tags.ExpireDate, Tags.EarlyTermination, Tags.MaxTradeVol, Tags.ExpireTime, Tags.MinQty, Tags.ExecInst, Tags.SellerDays, Tags.SettlPriceType, Tags.OrderID, Tags.QuoteEntryID, Tags.MDEntryBuyer, Tags.MDEntrySeller, Tags.NumberOfOrders, Tags.MDEntryPositionNo, Tags.PriceType, Tags.Scope, Tags.PriceDelta, Tags.NetChgPrevDay, Tags.TradeID, Tags.TradeVolume, Tags.Text, Tags.EncodedTextLen, Tags.EncodedText, Tags.UniqueTradeID, Tags.SecurityTradingStatus, Tags.Duration, Tags.NoReferentialPrices, Tags.TradSesOpenTime, Tags.RLCMsgID, Tags.MicrosecondOrderTimestamp, Tags.IndexSeq, 0};
            
                public NoMDEntriesGroup() 
                  :base( Tags.NoMDEntries, Tags.MDUpdateAction, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoMDEntriesGroup();
                    clone.CopyStateFrom(this);
                    return clone;
                }
            
                            public QuickFix.Fields.MDUpdateAction MDUpdateAction
                { 
                    get 
                    {
                        QuickFix.Fields.MDUpdateAction val = new QuickFix.Fields.MDUpdateAction();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MDUpdateAction val) 
                { 
                    this.MDUpdateAction = val;
                }
                
                public QuickFix.Fields.MDUpdateAction Get(QuickFix.Fields.MDUpdateAction val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MDUpdateAction val) 
                { 
                    return IsSetMDUpdateAction();
                }
                
                public bool IsSetMDUpdateAction() 
                { 
                    return IsSetField(Tags.MDUpdateAction);
                }
                public QuickFix.Fields.DeleteReason DeleteReason
                { 
                    get 
                    {
                        QuickFix.Fields.DeleteReason val = new QuickFix.Fields.DeleteReason();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.DeleteReason val) 
                { 
                    this.DeleteReason = val;
                }
                
                public QuickFix.Fields.DeleteReason Get(QuickFix.Fields.DeleteReason val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.DeleteReason val) 
                { 
                    return IsSetDeleteReason();
                }
                
                public bool IsSetDeleteReason() 
                { 
                    return IsSetField(Tags.DeleteReason);
                }
                public QuickFix.Fields.MDEntryType MDEntryType
                { 
                    get 
                    {
                        QuickFix.Fields.MDEntryType val = new QuickFix.Fields.MDEntryType();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MDEntryType val) 
                { 
                    this.MDEntryType = val;
                }
                
                public QuickFix.Fields.MDEntryType Get(QuickFix.Fields.MDEntryType val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MDEntryType val) 
                { 
                    return IsSetMDEntryType();
                }
                
                public bool IsSetMDEntryType() 
                { 
                    return IsSetField(Tags.MDEntryType);
                }
                public QuickFix.Fields.MDEntryID MDEntryID
                { 
                    get 
                    {
                        QuickFix.Fields.MDEntryID val = new QuickFix.Fields.MDEntryID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MDEntryID val) 
                { 
                    this.MDEntryID = val;
                }
                
                public QuickFix.Fields.MDEntryID Get(QuickFix.Fields.MDEntryID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MDEntryID val) 
                { 
                    return IsSetMDEntryID();
                }
                
                public bool IsSetMDEntryID() 
                { 
                    return IsSetField(Tags.MDEntryID);
                }
                public QuickFix.Fields.MDEntryRefID MDEntryRefID
                { 
                    get 
                    {
                        QuickFix.Fields.MDEntryRefID val = new QuickFix.Fields.MDEntryRefID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MDEntryRefID val) 
                { 
                    this.MDEntryRefID = val;
                }
                
                public QuickFix.Fields.MDEntryRefID Get(QuickFix.Fields.MDEntryRefID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MDEntryRefID val) 
                { 
                    return IsSetMDEntryRefID();
                }
                
                public bool IsSetMDEntryRefID() 
                { 
                    return IsSetField(Tags.MDEntryRefID);
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
                public QuickFix.Fields.SymbolSfx SymbolSfx
                { 
                    get 
                    {
                        QuickFix.Fields.SymbolSfx val = new QuickFix.Fields.SymbolSfx();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.SymbolSfx val) 
                { 
                    this.SymbolSfx = val;
                }
                
                public QuickFix.Fields.SymbolSfx Get(QuickFix.Fields.SymbolSfx val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.SymbolSfx val) 
                { 
                    return IsSetSymbolSfx();
                }
                
                public bool IsSetSymbolSfx() 
                { 
                    return IsSetField(Tags.SymbolSfx);
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
                public QuickFix.Fields.InstrumentID InstrumentID
                { 
                    get 
                    {
                        QuickFix.Fields.InstrumentID val = new QuickFix.Fields.InstrumentID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.InstrumentID val) 
                { 
                    this.InstrumentID = val;
                }
                
                public QuickFix.Fields.InstrumentID Get(QuickFix.Fields.InstrumentID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.InstrumentID val) 
                { 
                    return IsSetInstrumentID();
                }
                
                public bool IsSetInstrumentID() 
                { 
                    return IsSetField(Tags.InstrumentID);
                }
                public QuickFix.Fields.PutOrCall PutOrCall
                { 
                    get 
                    {
                        QuickFix.Fields.PutOrCall val = new QuickFix.Fields.PutOrCall();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.PutOrCall val) 
                { 
                    this.PutOrCall = val;
                }
                
                public QuickFix.Fields.PutOrCall Get(QuickFix.Fields.PutOrCall val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.PutOrCall val) 
                { 
                    return IsSetPutOrCall();
                }
                
                public bool IsSetPutOrCall() 
                { 
                    return IsSetField(Tags.PutOrCall);
                }
                public QuickFix.Fields.NoSecurityAltID NoSecurityAltID
                { 
                    get 
                    {
                        QuickFix.Fields.NoSecurityAltID val = new QuickFix.Fields.NoSecurityAltID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.NoSecurityAltID val) 
                { 
                    this.NoSecurityAltID = val;
                }
                
                public QuickFix.Fields.NoSecurityAltID Get(QuickFix.Fields.NoSecurityAltID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.NoSecurityAltID val) 
                { 
                    return IsSetNoSecurityAltID();
                }
                
                public bool IsSetNoSecurityAltID() 
                { 
                    return IsSetField(Tags.NoSecurityAltID);
                }
                public QuickFix.Fields.Product Product
                { 
                    get 
                    {
                        QuickFix.Fields.Product val = new QuickFix.Fields.Product();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.Product val) 
                { 
                    this.Product = val;
                }
                
                public QuickFix.Fields.Product Get(QuickFix.Fields.Product val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.Product val) 
                { 
                    return IsSetProduct();
                }
                
                public bool IsSetProduct() 
                { 
                    return IsSetField(Tags.Product);
                }
                public QuickFix.Fields.CFICode CFICode
                { 
                    get 
                    {
                        QuickFix.Fields.CFICode val = new QuickFix.Fields.CFICode();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.CFICode val) 
                { 
                    this.CFICode = val;
                }
                
                public QuickFix.Fields.CFICode Get(QuickFix.Fields.CFICode val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.CFICode val) 
                { 
                    return IsSetCFICode();
                }
                
                public bool IsSetCFICode() 
                { 
                    return IsSetField(Tags.CFICode);
                }
                public QuickFix.Fields.SecurityGroupTMR SecurityGroupTMR
                { 
                    get 
                    {
                        QuickFix.Fields.SecurityGroupTMR val = new QuickFix.Fields.SecurityGroupTMR();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.SecurityGroupTMR val) 
                { 
                    this.SecurityGroupTMR = val;
                }
                
                public QuickFix.Fields.SecurityGroupTMR Get(QuickFix.Fields.SecurityGroupTMR val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.SecurityGroupTMR val) 
                { 
                    return IsSetSecurityGroupTMR();
                }
                
                public bool IsSetSecurityGroupTMR() 
                { 
                    return IsSetField(Tags.SecurityGroupTMR);
                }
                public QuickFix.Fields.SecurityType SecurityType
                { 
                    get 
                    {
                        QuickFix.Fields.SecurityType val = new QuickFix.Fields.SecurityType();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.SecurityType val) 
                { 
                    this.SecurityType = val;
                }
                
                public QuickFix.Fields.SecurityType Get(QuickFix.Fields.SecurityType val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.SecurityType val) 
                { 
                    return IsSetSecurityType();
                }
                
                public bool IsSetSecurityType() 
                { 
                    return IsSetField(Tags.SecurityType);
                }
                public QuickFix.Fields.SecuritySubType SecuritySubType
                { 
                    get 
                    {
                        QuickFix.Fields.SecuritySubType val = new QuickFix.Fields.SecuritySubType();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.SecuritySubType val) 
                { 
                    this.SecuritySubType = val;
                }
                
                public QuickFix.Fields.SecuritySubType Get(QuickFix.Fields.SecuritySubType val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.SecuritySubType val) 
                { 
                    return IsSetSecuritySubType();
                }
                
                public bool IsSetSecuritySubType() 
                { 
                    return IsSetField(Tags.SecuritySubType);
                }
                public QuickFix.Fields.MaturityMonthYear MaturityMonthYear
                { 
                    get 
                    {
                        QuickFix.Fields.MaturityMonthYear val = new QuickFix.Fields.MaturityMonthYear();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MaturityMonthYear val) 
                { 
                    this.MaturityMonthYear = val;
                }
                
                public QuickFix.Fields.MaturityMonthYear Get(QuickFix.Fields.MaturityMonthYear val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MaturityMonthYear val) 
                { 
                    return IsSetMaturityMonthYear();
                }
                
                public bool IsSetMaturityMonthYear() 
                { 
                    return IsSetField(Tags.MaturityMonthYear);
                }
                public QuickFix.Fields.MaturityDate MaturityDate
                { 
                    get 
                    {
                        QuickFix.Fields.MaturityDate val = new QuickFix.Fields.MaturityDate();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MaturityDate val) 
                { 
                    this.MaturityDate = val;
                }
                
                public QuickFix.Fields.MaturityDate Get(QuickFix.Fields.MaturityDate val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MaturityDate val) 
                { 
                    return IsSetMaturityDate();
                }
                
                public bool IsSetMaturityDate() 
                { 
                    return IsSetField(Tags.MaturityDate);
                }
                public QuickFix.Fields.CouponPaymentDate CouponPaymentDate
                { 
                    get 
                    {
                        QuickFix.Fields.CouponPaymentDate val = new QuickFix.Fields.CouponPaymentDate();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.CouponPaymentDate val) 
                { 
                    this.CouponPaymentDate = val;
                }
                
                public QuickFix.Fields.CouponPaymentDate Get(QuickFix.Fields.CouponPaymentDate val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.CouponPaymentDate val) 
                { 
                    return IsSetCouponPaymentDate();
                }
                
                public bool IsSetCouponPaymentDate() 
                { 
                    return IsSetField(Tags.CouponPaymentDate);
                }
                public QuickFix.Fields.IssueDate IssueDate
                { 
                    get 
                    {
                        QuickFix.Fields.IssueDate val = new QuickFix.Fields.IssueDate();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.IssueDate val) 
                { 
                    this.IssueDate = val;
                }
                
                public QuickFix.Fields.IssueDate Get(QuickFix.Fields.IssueDate val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.IssueDate val) 
                { 
                    return IsSetIssueDate();
                }
                
                public bool IsSetIssueDate() 
                { 
                    return IsSetField(Tags.IssueDate);
                }
                public QuickFix.Fields.RepoCollateralSecurityType RepoCollateralSecurityType
                { 
                    get 
                    {
                        QuickFix.Fields.RepoCollateralSecurityType val = new QuickFix.Fields.RepoCollateralSecurityType();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.RepoCollateralSecurityType val) 
                { 
                    this.RepoCollateralSecurityType = val;
                }
                
                public QuickFix.Fields.RepoCollateralSecurityType Get(QuickFix.Fields.RepoCollateralSecurityType val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.RepoCollateralSecurityType val) 
                { 
                    return IsSetRepoCollateralSecurityType();
                }
                
                public bool IsSetRepoCollateralSecurityType() 
                { 
                    return IsSetField(Tags.RepoCollateralSecurityType);
                }
                public QuickFix.Fields.RepurchaseTerm RepurchaseTerm
                { 
                    get 
                    {
                        QuickFix.Fields.RepurchaseTerm val = new QuickFix.Fields.RepurchaseTerm();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.RepurchaseTerm val) 
                { 
                    this.RepurchaseTerm = val;
                }
                
                public QuickFix.Fields.RepurchaseTerm Get(QuickFix.Fields.RepurchaseTerm val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.RepurchaseTerm val) 
                { 
                    return IsSetRepurchaseTerm();
                }
                
                public bool IsSetRepurchaseTerm() 
                { 
                    return IsSetField(Tags.RepurchaseTerm);
                }
                public QuickFix.Fields.RepurchaseRate RepurchaseRate
                { 
                    get 
                    {
                        QuickFix.Fields.RepurchaseRate val = new QuickFix.Fields.RepurchaseRate();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.RepurchaseRate val) 
                { 
                    this.RepurchaseRate = val;
                }
                
                public QuickFix.Fields.RepurchaseRate Get(QuickFix.Fields.RepurchaseRate val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.RepurchaseRate val) 
                { 
                    return IsSetRepurchaseRate();
                }
                
                public bool IsSetRepurchaseRate() 
                { 
                    return IsSetField(Tags.RepurchaseRate);
                }
                public QuickFix.Fields.Factor Factor
                { 
                    get 
                    {
                        QuickFix.Fields.Factor val = new QuickFix.Fields.Factor();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.Factor val) 
                { 
                    this.Factor = val;
                }
                
                public QuickFix.Fields.Factor Get(QuickFix.Fields.Factor val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.Factor val) 
                { 
                    return IsSetFactor();
                }
                
                public bool IsSetFactor() 
                { 
                    return IsSetField(Tags.Factor);
                }
                public QuickFix.Fields.CreditRating CreditRating
                { 
                    get 
                    {
                        QuickFix.Fields.CreditRating val = new QuickFix.Fields.CreditRating();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.CreditRating val) 
                { 
                    this.CreditRating = val;
                }
                
                public QuickFix.Fields.CreditRating Get(QuickFix.Fields.CreditRating val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.CreditRating val) 
                { 
                    return IsSetCreditRating();
                }
                
                public bool IsSetCreditRating() 
                { 
                    return IsSetField(Tags.CreditRating);
                }
                public QuickFix.Fields.InstrRegistry InstrRegistry
                { 
                    get 
                    {
                        QuickFix.Fields.InstrRegistry val = new QuickFix.Fields.InstrRegistry();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.InstrRegistry val) 
                { 
                    this.InstrRegistry = val;
                }
                
                public QuickFix.Fields.InstrRegistry Get(QuickFix.Fields.InstrRegistry val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.InstrRegistry val) 
                { 
                    return IsSetInstrRegistry();
                }
                
                public bool IsSetInstrRegistry() 
                { 
                    return IsSetField(Tags.InstrRegistry);
                }
                public QuickFix.Fields.CountryOfIssue CountryOfIssue
                { 
                    get 
                    {
                        QuickFix.Fields.CountryOfIssue val = new QuickFix.Fields.CountryOfIssue();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.CountryOfIssue val) 
                { 
                    this.CountryOfIssue = val;
                }
                
                public QuickFix.Fields.CountryOfIssue Get(QuickFix.Fields.CountryOfIssue val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.CountryOfIssue val) 
                { 
                    return IsSetCountryOfIssue();
                }
                
                public bool IsSetCountryOfIssue() 
                { 
                    return IsSetField(Tags.CountryOfIssue);
                }
                public QuickFix.Fields.StateOrProvinceOfIssue StateOrProvinceOfIssue
                { 
                    get 
                    {
                        QuickFix.Fields.StateOrProvinceOfIssue val = new QuickFix.Fields.StateOrProvinceOfIssue();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.StateOrProvinceOfIssue val) 
                { 
                    this.StateOrProvinceOfIssue = val;
                }
                
                public QuickFix.Fields.StateOrProvinceOfIssue Get(QuickFix.Fields.StateOrProvinceOfIssue val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.StateOrProvinceOfIssue val) 
                { 
                    return IsSetStateOrProvinceOfIssue();
                }
                
                public bool IsSetStateOrProvinceOfIssue() 
                { 
                    return IsSetField(Tags.StateOrProvinceOfIssue);
                }
                public QuickFix.Fields.LocaleOfIssue LocaleOfIssue
                { 
                    get 
                    {
                        QuickFix.Fields.LocaleOfIssue val = new QuickFix.Fields.LocaleOfIssue();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.LocaleOfIssue val) 
                { 
                    this.LocaleOfIssue = val;
                }
                
                public QuickFix.Fields.LocaleOfIssue Get(QuickFix.Fields.LocaleOfIssue val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.LocaleOfIssue val) 
                { 
                    return IsSetLocaleOfIssue();
                }
                
                public bool IsSetLocaleOfIssue() 
                { 
                    return IsSetField(Tags.LocaleOfIssue);
                }
                public QuickFix.Fields.RedemptionDate RedemptionDate
                { 
                    get 
                    {
                        QuickFix.Fields.RedemptionDate val = new QuickFix.Fields.RedemptionDate();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.RedemptionDate val) 
                { 
                    this.RedemptionDate = val;
                }
                
                public QuickFix.Fields.RedemptionDate Get(QuickFix.Fields.RedemptionDate val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.RedemptionDate val) 
                { 
                    return IsSetRedemptionDate();
                }
                
                public bool IsSetRedemptionDate() 
                { 
                    return IsSetField(Tags.RedemptionDate);
                }
                public QuickFix.Fields.StrikePrice StrikePrice
                { 
                    get 
                    {
                        QuickFix.Fields.StrikePrice val = new QuickFix.Fields.StrikePrice();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.StrikePrice val) 
                { 
                    this.StrikePrice = val;
                }
                
                public QuickFix.Fields.StrikePrice Get(QuickFix.Fields.StrikePrice val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.StrikePrice val) 
                { 
                    return IsSetStrikePrice();
                }
                
                public bool IsSetStrikePrice() 
                { 
                    return IsSetField(Tags.StrikePrice);
                }
                public QuickFix.Fields.StrikeCurrency StrikeCurrency
                { 
                    get 
                    {
                        QuickFix.Fields.StrikeCurrency val = new QuickFix.Fields.StrikeCurrency();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.StrikeCurrency val) 
                { 
                    this.StrikeCurrency = val;
                }
                
                public QuickFix.Fields.StrikeCurrency Get(QuickFix.Fields.StrikeCurrency val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.StrikeCurrency val) 
                { 
                    return IsSetStrikeCurrency();
                }
                
                public bool IsSetStrikeCurrency() 
                { 
                    return IsSetField(Tags.StrikeCurrency);
                }
                public QuickFix.Fields.ExerciseStyle ExerciseStyle
                { 
                    get 
                    {
                        QuickFix.Fields.ExerciseStyle val = new QuickFix.Fields.ExerciseStyle();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.ExerciseStyle val) 
                { 
                    this.ExerciseStyle = val;
                }
                
                public QuickFix.Fields.ExerciseStyle Get(QuickFix.Fields.ExerciseStyle val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.ExerciseStyle val) 
                { 
                    return IsSetExerciseStyle();
                }
                
                public bool IsSetExerciseStyle() 
                { 
                    return IsSetField(Tags.ExerciseStyle);
                }
                public QuickFix.Fields.OptAttribute OptAttribute
                { 
                    get 
                    {
                        QuickFix.Fields.OptAttribute val = new QuickFix.Fields.OptAttribute();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.OptAttribute val) 
                { 
                    this.OptAttribute = val;
                }
                
                public QuickFix.Fields.OptAttribute Get(QuickFix.Fields.OptAttribute val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.OptAttribute val) 
                { 
                    return IsSetOptAttribute();
                }
                
                public bool IsSetOptAttribute() 
                { 
                    return IsSetField(Tags.OptAttribute);
                }
                public QuickFix.Fields.ContractMultiplier ContractMultiplier
                { 
                    get 
                    {
                        QuickFix.Fields.ContractMultiplier val = new QuickFix.Fields.ContractMultiplier();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.ContractMultiplier val) 
                { 
                    this.ContractMultiplier = val;
                }
                
                public QuickFix.Fields.ContractMultiplier Get(QuickFix.Fields.ContractMultiplier val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.ContractMultiplier val) 
                { 
                    return IsSetContractMultiplier();
                }
                
                public bool IsSetContractMultiplier() 
                { 
                    return IsSetField(Tags.ContractMultiplier);
                }
                public QuickFix.Fields.CouponRate CouponRate
                { 
                    get 
                    {
                        QuickFix.Fields.CouponRate val = new QuickFix.Fields.CouponRate();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.CouponRate val) 
                { 
                    this.CouponRate = val;
                }
                
                public QuickFix.Fields.CouponRate Get(QuickFix.Fields.CouponRate val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.CouponRate val) 
                { 
                    return IsSetCouponRate();
                }
                
                public bool IsSetCouponRate() 
                { 
                    return IsSetField(Tags.CouponRate);
                }
                public QuickFix.Fields.Issuer Issuer
                { 
                    get 
                    {
                        QuickFix.Fields.Issuer val = new QuickFix.Fields.Issuer();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.Issuer val) 
                { 
                    this.Issuer = val;
                }
                
                public QuickFix.Fields.Issuer Get(QuickFix.Fields.Issuer val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.Issuer val) 
                { 
                    return IsSetIssuer();
                }
                
                public bool IsSetIssuer() 
                { 
                    return IsSetField(Tags.Issuer);
                }
                public QuickFix.Fields.EncodedIssuerLen EncodedIssuerLen
                { 
                    get 
                    {
                        QuickFix.Fields.EncodedIssuerLen val = new QuickFix.Fields.EncodedIssuerLen();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.EncodedIssuerLen val) 
                { 
                    this.EncodedIssuerLen = val;
                }
                
                public QuickFix.Fields.EncodedIssuerLen Get(QuickFix.Fields.EncodedIssuerLen val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.EncodedIssuerLen val) 
                { 
                    return IsSetEncodedIssuerLen();
                }
                
                public bool IsSetEncodedIssuerLen() 
                { 
                    return IsSetField(Tags.EncodedIssuerLen);
                }
                public QuickFix.Fields.EncodedIssuer EncodedIssuer
                { 
                    get 
                    {
                        QuickFix.Fields.EncodedIssuer val = new QuickFix.Fields.EncodedIssuer();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.EncodedIssuer val) 
                { 
                    this.EncodedIssuer = val;
                }
                
                public QuickFix.Fields.EncodedIssuer Get(QuickFix.Fields.EncodedIssuer val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.EncodedIssuer val) 
                { 
                    return IsSetEncodedIssuer();
                }
                
                public bool IsSetEncodedIssuer() 
                { 
                    return IsSetField(Tags.EncodedIssuer);
                }
                public QuickFix.Fields.SecurityDesc SecurityDesc
                { 
                    get 
                    {
                        QuickFix.Fields.SecurityDesc val = new QuickFix.Fields.SecurityDesc();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.SecurityDesc val) 
                { 
                    this.SecurityDesc = val;
                }
                
                public QuickFix.Fields.SecurityDesc Get(QuickFix.Fields.SecurityDesc val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.SecurityDesc val) 
                { 
                    return IsSetSecurityDesc();
                }
                
                public bool IsSetSecurityDesc() 
                { 
                    return IsSetField(Tags.SecurityDesc);
                }
                public QuickFix.Fields.EncodedSecurityDescLen EncodedSecurityDescLen
                { 
                    get 
                    {
                        QuickFix.Fields.EncodedSecurityDescLen val = new QuickFix.Fields.EncodedSecurityDescLen();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.EncodedSecurityDescLen val) 
                { 
                    this.EncodedSecurityDescLen = val;
                }
                
                public QuickFix.Fields.EncodedSecurityDescLen Get(QuickFix.Fields.EncodedSecurityDescLen val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.EncodedSecurityDescLen val) 
                { 
                    return IsSetEncodedSecurityDescLen();
                }
                
                public bool IsSetEncodedSecurityDescLen() 
                { 
                    return IsSetField(Tags.EncodedSecurityDescLen);
                }
                public QuickFix.Fields.EncodedSecurityDesc EncodedSecurityDesc
                { 
                    get 
                    {
                        QuickFix.Fields.EncodedSecurityDesc val = new QuickFix.Fields.EncodedSecurityDesc();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.EncodedSecurityDesc val) 
                { 
                    this.EncodedSecurityDesc = val;
                }
                
                public QuickFix.Fields.EncodedSecurityDesc Get(QuickFix.Fields.EncodedSecurityDesc val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.EncodedSecurityDesc val) 
                { 
                    return IsSetEncodedSecurityDesc();
                }
                
                public bool IsSetEncodedSecurityDesc() 
                { 
                    return IsSetField(Tags.EncodedSecurityDesc);
                }
                public QuickFix.Fields.Pool Pool
                { 
                    get 
                    {
                        QuickFix.Fields.Pool val = new QuickFix.Fields.Pool();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.Pool val) 
                { 
                    this.Pool = val;
                }
                
                public QuickFix.Fields.Pool Get(QuickFix.Fields.Pool val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.Pool val) 
                { 
                    return IsSetPool();
                }
                
                public bool IsSetPool() 
                { 
                    return IsSetField(Tags.Pool);
                }
                public QuickFix.Fields.ContractSettlMonth ContractSettlMonth
                { 
                    get 
                    {
                        QuickFix.Fields.ContractSettlMonth val = new QuickFix.Fields.ContractSettlMonth();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.ContractSettlMonth val) 
                { 
                    this.ContractSettlMonth = val;
                }
                
                public QuickFix.Fields.ContractSettlMonth Get(QuickFix.Fields.ContractSettlMonth val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.ContractSettlMonth val) 
                { 
                    return IsSetContractSettlMonth();
                }
                
                public bool IsSetContractSettlMonth() 
                { 
                    return IsSetField(Tags.ContractSettlMonth);
                }
                public QuickFix.Fields.CPProgram CPProgram
                { 
                    get 
                    {
                        QuickFix.Fields.CPProgram val = new QuickFix.Fields.CPProgram();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.CPProgram val) 
                { 
                    this.CPProgram = val;
                }
                
                public QuickFix.Fields.CPProgram Get(QuickFix.Fields.CPProgram val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.CPProgram val) 
                { 
                    return IsSetCPProgram();
                }
                
                public bool IsSetCPProgram() 
                { 
                    return IsSetField(Tags.CPProgram);
                }
                public QuickFix.Fields.CPRegType CPRegType
                { 
                    get 
                    {
                        QuickFix.Fields.CPRegType val = new QuickFix.Fields.CPRegType();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.CPRegType val) 
                { 
                    this.CPRegType = val;
                }
                
                public QuickFix.Fields.CPRegType Get(QuickFix.Fields.CPRegType val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.CPRegType val) 
                { 
                    return IsSetCPRegType();
                }
                
                public bool IsSetCPRegType() 
                { 
                    return IsSetField(Tags.CPRegType);
                }
                public QuickFix.Fields.NoEvents NoEvents
                { 
                    get 
                    {
                        QuickFix.Fields.NoEvents val = new QuickFix.Fields.NoEvents();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.NoEvents val) 
                { 
                    this.NoEvents = val;
                }
                
                public QuickFix.Fields.NoEvents Get(QuickFix.Fields.NoEvents val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.NoEvents val) 
                { 
                    return IsSetNoEvents();
                }
                
                public bool IsSetNoEvents() 
                { 
                    return IsSetField(Tags.NoEvents);
                }
                public QuickFix.Fields.DatedDate DatedDate
                { 
                    get 
                    {
                        QuickFix.Fields.DatedDate val = new QuickFix.Fields.DatedDate();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.DatedDate val) 
                { 
                    this.DatedDate = val;
                }
                
                public QuickFix.Fields.DatedDate Get(QuickFix.Fields.DatedDate val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.DatedDate val) 
                { 
                    return IsSetDatedDate();
                }
                
                public bool IsSetDatedDate() 
                { 
                    return IsSetField(Tags.DatedDate);
                }
                public QuickFix.Fields.InterestAccrualDate InterestAccrualDate
                { 
                    get 
                    {
                        QuickFix.Fields.InterestAccrualDate val = new QuickFix.Fields.InterestAccrualDate();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.InterestAccrualDate val) 
                { 
                    this.InterestAccrualDate = val;
                }
                
                public QuickFix.Fields.InterestAccrualDate Get(QuickFix.Fields.InterestAccrualDate val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.InterestAccrualDate val) 
                { 
                    return IsSetInterestAccrualDate();
                }
                
                public bool IsSetInterestAccrualDate() 
                { 
                    return IsSetField(Tags.InterestAccrualDate);
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
                public QuickFix.Fields.MDStreamID MDStreamID
                { 
                    get 
                    {
                        QuickFix.Fields.MDStreamID val = new QuickFix.Fields.MDStreamID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MDStreamID val) 
                { 
                    this.MDStreamID = val;
                }
                
                public QuickFix.Fields.MDStreamID Get(QuickFix.Fields.MDStreamID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MDStreamID val) 
                { 
                    return IsSetMDStreamID();
                }
                
                public bool IsSetMDStreamID() 
                { 
                    return IsSetField(Tags.MDStreamID);
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
                public QuickFix.Fields.NoLegs NoLegs
                { 
                    get 
                    {
                        QuickFix.Fields.NoLegs val = new QuickFix.Fields.NoLegs();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.NoLegs val) 
                { 
                    this.NoLegs = val;
                }
                
                public QuickFix.Fields.NoLegs Get(QuickFix.Fields.NoLegs val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.NoLegs val) 
                { 
                    return IsSetNoLegs();
                }
                
                public bool IsSetNoLegs() 
                { 
                    return IsSetField(Tags.NoLegs);
                }
                public QuickFix.Fields.FinancialStatus FinancialStatus
                { 
                    get 
                    {
                        QuickFix.Fields.FinancialStatus val = new QuickFix.Fields.FinancialStatus();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.FinancialStatus val) 
                { 
                    this.FinancialStatus = val;
                }
                
                public QuickFix.Fields.FinancialStatus Get(QuickFix.Fields.FinancialStatus val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.FinancialStatus val) 
                { 
                    return IsSetFinancialStatus();
                }
                
                public bool IsSetFinancialStatus() 
                { 
                    return IsSetField(Tags.FinancialStatus);
                }
                public QuickFix.Fields.CorporateAction CorporateAction
                { 
                    get 
                    {
                        QuickFix.Fields.CorporateAction val = new QuickFix.Fields.CorporateAction();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.CorporateAction val) 
                { 
                    this.CorporateAction = val;
                }
                
                public QuickFix.Fields.CorporateAction Get(QuickFix.Fields.CorporateAction val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.CorporateAction val) 
                { 
                    return IsSetCorporateAction();
                }
                
                public bool IsSetCorporateAction() 
                { 
                    return IsSetField(Tags.CorporateAction);
                }
                public QuickFix.Fields.MDEntryPx MDEntryPx
                { 
                    get 
                    {
                        QuickFix.Fields.MDEntryPx val = new QuickFix.Fields.MDEntryPx();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MDEntryPx val) 
                { 
                    this.MDEntryPx = val;
                }
                
                public QuickFix.Fields.MDEntryPx Get(QuickFix.Fields.MDEntryPx val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MDEntryPx val) 
                { 
                    return IsSetMDEntryPx();
                }
                
                public bool IsSetMDEntryPx() 
                { 
                    return IsSetField(Tags.MDEntryPx);
                }
                public QuickFix.Fields.MDEntryInterestRate MDEntryInterestRate
                { 
                    get 
                    {
                        QuickFix.Fields.MDEntryInterestRate val = new QuickFix.Fields.MDEntryInterestRate();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MDEntryInterestRate val) 
                { 
                    this.MDEntryInterestRate = val;
                }
                
                public QuickFix.Fields.MDEntryInterestRate Get(QuickFix.Fields.MDEntryInterestRate val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MDEntryInterestRate val) 
                { 
                    return IsSetMDEntryInterestRate();
                }
                
                public bool IsSetMDEntryInterestRate() 
                { 
                    return IsSetField(Tags.MDEntryInterestRate);
                }
                public QuickFix.Fields.LastTradeDate LastTradeDate
                { 
                    get 
                    {
                        QuickFix.Fields.LastTradeDate val = new QuickFix.Fields.LastTradeDate();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.LastTradeDate val) 
                { 
                    this.LastTradeDate = val;
                }
                
                public QuickFix.Fields.LastTradeDate Get(QuickFix.Fields.LastTradeDate val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.LastTradeDate val) 
                { 
                    return IsSetLastTradeDate();
                }
                
                public bool IsSetLastTradeDate() 
                { 
                    return IsSetField(Tags.LastTradeDate);
                }
                public QuickFix.Fields.PriceAdjustmentMethod PriceAdjustmentMethod
                { 
                    get 
                    {
                        QuickFix.Fields.PriceAdjustmentMethod val = new QuickFix.Fields.PriceAdjustmentMethod();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.PriceAdjustmentMethod val) 
                { 
                    this.PriceAdjustmentMethod = val;
                }
                
                public QuickFix.Fields.PriceAdjustmentMethod Get(QuickFix.Fields.PriceAdjustmentMethod val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.PriceAdjustmentMethod val) 
                { 
                    return IsSetPriceAdjustmentMethod();
                }
                
                public bool IsSetPriceAdjustmentMethod() 
                { 
                    return IsSetField(Tags.PriceAdjustmentMethod);
                }
                public QuickFix.Fields.PriceBandType PriceBandType
                { 
                    get 
                    {
                        QuickFix.Fields.PriceBandType val = new QuickFix.Fields.PriceBandType();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.PriceBandType val) 
                { 
                    this.PriceBandType = val;
                }
                
                public QuickFix.Fields.PriceBandType Get(QuickFix.Fields.PriceBandType val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.PriceBandType val) 
                { 
                    return IsSetPriceBandType();
                }
                
                public bool IsSetPriceBandType() 
                { 
                    return IsSetField(Tags.PriceBandType);
                }
                public QuickFix.Fields.PriceLimitType PriceLimitType
                { 
                    get 
                    {
                        QuickFix.Fields.PriceLimitType val = new QuickFix.Fields.PriceLimitType();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.PriceLimitType val) 
                { 
                    this.PriceLimitType = val;
                }
                
                public QuickFix.Fields.PriceLimitType Get(QuickFix.Fields.PriceLimitType val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.PriceLimitType val) 
                { 
                    return IsSetPriceLimitType();
                }
                
                public bool IsSetPriceLimitType() 
                { 
                    return IsSetField(Tags.PriceLimitType);
                }
                public QuickFix.Fields.LowLimitPrice LowLimitPrice
                { 
                    get 
                    {
                        QuickFix.Fields.LowLimitPrice val = new QuickFix.Fields.LowLimitPrice();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.LowLimitPrice val) 
                { 
                    this.LowLimitPrice = val;
                }
                
                public QuickFix.Fields.LowLimitPrice Get(QuickFix.Fields.LowLimitPrice val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.LowLimitPrice val) 
                { 
                    return IsSetLowLimitPrice();
                }
                
                public bool IsSetLowLimitPrice() 
                { 
                    return IsSetField(Tags.LowLimitPrice);
                }
                public QuickFix.Fields.HighLimitPrice HighLimitPrice
                { 
                    get 
                    {
                        QuickFix.Fields.HighLimitPrice val = new QuickFix.Fields.HighLimitPrice();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.HighLimitPrice val) 
                { 
                    this.HighLimitPrice = val;
                }
                
                public QuickFix.Fields.HighLimitPrice Get(QuickFix.Fields.HighLimitPrice val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.HighLimitPrice val) 
                { 
                    return IsSetHighLimitPrice();
                }
                
                public bool IsSetHighLimitPrice() 
                { 
                    return IsSetField(Tags.HighLimitPrice);
                }
                public QuickFix.Fields.TradingReferencePrice TradingReferencePrice
                { 
                    get 
                    {
                        QuickFix.Fields.TradingReferencePrice val = new QuickFix.Fields.TradingReferencePrice();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.TradingReferencePrice val) 
                { 
                    this.TradingReferencePrice = val;
                }
                
                public QuickFix.Fields.TradingReferencePrice Get(QuickFix.Fields.TradingReferencePrice val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.TradingReferencePrice val) 
                { 
                    return IsSetTradingReferencePrice();
                }
                
                public bool IsSetTradingReferencePrice() 
                { 
                    return IsSetField(Tags.TradingReferencePrice);
                }
                public QuickFix.Fields.PriceBandMidpointPriceType PriceBandMidpointPriceType
                { 
                    get 
                    {
                        QuickFix.Fields.PriceBandMidpointPriceType val = new QuickFix.Fields.PriceBandMidpointPriceType();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.PriceBandMidpointPriceType val) 
                { 
                    this.PriceBandMidpointPriceType = val;
                }
                
                public QuickFix.Fields.PriceBandMidpointPriceType Get(QuickFix.Fields.PriceBandMidpointPriceType val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.PriceBandMidpointPriceType val) 
                { 
                    return IsSetPriceBandMidpointPriceType();
                }
                
                public bool IsSetPriceBandMidpointPriceType() 
                { 
                    return IsSetField(Tags.PriceBandMidpointPriceType);
                }
                public QuickFix.Fields.AvgDailyTradedQty AvgDailyTradedQty
                { 
                    get 
                    {
                        QuickFix.Fields.AvgDailyTradedQty val = new QuickFix.Fields.AvgDailyTradedQty();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.AvgDailyTradedQty val) 
                { 
                    this.AvgDailyTradedQty = val;
                }
                
                public QuickFix.Fields.AvgDailyTradedQty Get(QuickFix.Fields.AvgDailyTradedQty val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.AvgDailyTradedQty val) 
                { 
                    return IsSetAvgDailyTradedQty();
                }
                
                public bool IsSetAvgDailyTradedQty() 
                { 
                    return IsSetField(Tags.AvgDailyTradedQty);
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
                public QuickFix.Fields.MDEntrySize MDEntrySize
                { 
                    get 
                    {
                        QuickFix.Fields.MDEntrySize val = new QuickFix.Fields.MDEntrySize();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MDEntrySize val) 
                { 
                    this.MDEntrySize = val;
                }
                
                public QuickFix.Fields.MDEntrySize Get(QuickFix.Fields.MDEntrySize val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MDEntrySize val) 
                { 
                    return IsSetMDEntrySize();
                }
                
                public bool IsSetMDEntrySize() 
                { 
                    return IsSetField(Tags.MDEntrySize);
                }
                public QuickFix.Fields.MDEntryDate MDEntryDate
                { 
                    get 
                    {
                        QuickFix.Fields.MDEntryDate val = new QuickFix.Fields.MDEntryDate();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MDEntryDate val) 
                { 
                    this.MDEntryDate = val;
                }
                
                public QuickFix.Fields.MDEntryDate Get(QuickFix.Fields.MDEntryDate val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MDEntryDate val) 
                { 
                    return IsSetMDEntryDate();
                }
                
                public bool IsSetMDEntryDate() 
                { 
                    return IsSetField(Tags.MDEntryDate);
                }
                public QuickFix.Fields.MDEntryTime MDEntryTime
                { 
                    get 
                    {
                        QuickFix.Fields.MDEntryTime val = new QuickFix.Fields.MDEntryTime();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MDEntryTime val) 
                { 
                    this.MDEntryTime = val;
                }
                
                public QuickFix.Fields.MDEntryTime Get(QuickFix.Fields.MDEntryTime val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MDEntryTime val) 
                { 
                    return IsSetMDEntryTime();
                }
                
                public bool IsSetMDEntryTime() 
                { 
                    return IsSetField(Tags.MDEntryTime);
                }
                public QuickFix.Fields.MDInsertDate MDInsertDate
                { 
                    get 
                    {
                        QuickFix.Fields.MDInsertDate val = new QuickFix.Fields.MDInsertDate();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MDInsertDate val) 
                { 
                    this.MDInsertDate = val;
                }
                
                public QuickFix.Fields.MDInsertDate Get(QuickFix.Fields.MDInsertDate val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MDInsertDate val) 
                { 
                    return IsSetMDInsertDate();
                }
                
                public bool IsSetMDInsertDate() 
                { 
                    return IsSetField(Tags.MDInsertDate);
                }
                public QuickFix.Fields.MDInsertTime MDInsertTime
                { 
                    get 
                    {
                        QuickFix.Fields.MDInsertTime val = new QuickFix.Fields.MDInsertTime();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MDInsertTime val) 
                { 
                    this.MDInsertTime = val;
                }
                
                public QuickFix.Fields.MDInsertTime Get(QuickFix.Fields.MDInsertTime val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MDInsertTime val) 
                { 
                    return IsSetMDInsertTime();
                }
                
                public bool IsSetMDInsertTime() 
                { 
                    return IsSetField(Tags.MDInsertTime);
                }
                public QuickFix.Fields.TickDirection TickDirection
                { 
                    get 
                    {
                        QuickFix.Fields.TickDirection val = new QuickFix.Fields.TickDirection();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.TickDirection val) 
                { 
                    this.TickDirection = val;
                }
                
                public QuickFix.Fields.TickDirection Get(QuickFix.Fields.TickDirection val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.TickDirection val) 
                { 
                    return IsSetTickDirection();
                }
                
                public bool IsSetTickDirection() 
                { 
                    return IsSetField(Tags.TickDirection);
                }
                public QuickFix.Fields.MDMkt MDMkt
                { 
                    get 
                    {
                        QuickFix.Fields.MDMkt val = new QuickFix.Fields.MDMkt();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MDMkt val) 
                { 
                    this.MDMkt = val;
                }
                
                public QuickFix.Fields.MDMkt Get(QuickFix.Fields.MDMkt val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MDMkt val) 
                { 
                    return IsSetMDMkt();
                }
                
                public bool IsSetMDMkt() 
                { 
                    return IsSetField(Tags.MDMkt);
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
                public QuickFix.Fields.QuoteCondition QuoteCondition
                { 
                    get 
                    {
                        QuickFix.Fields.QuoteCondition val = new QuickFix.Fields.QuoteCondition();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.QuoteCondition val) 
                { 
                    this.QuoteCondition = val;
                }
                
                public QuickFix.Fields.QuoteCondition Get(QuickFix.Fields.QuoteCondition val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.QuoteCondition val) 
                { 
                    return IsSetQuoteCondition();
                }
                
                public bool IsSetQuoteCondition() 
                { 
                    return IsSetField(Tags.QuoteCondition);
                }
                public QuickFix.Fields.TradeCondition TradeCondition
                { 
                    get 
                    {
                        QuickFix.Fields.TradeCondition val = new QuickFix.Fields.TradeCondition();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.TradeCondition val) 
                { 
                    this.TradeCondition = val;
                }
                
                public QuickFix.Fields.TradeCondition Get(QuickFix.Fields.TradeCondition val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.TradeCondition val) 
                { 
                    return IsSetTradeCondition();
                }
                
                public bool IsSetTradeCondition() 
                { 
                    return IsSetField(Tags.TradeCondition);
                }
                public QuickFix.Fields.MDEntryOriginator MDEntryOriginator
                { 
                    get 
                    {
                        QuickFix.Fields.MDEntryOriginator val = new QuickFix.Fields.MDEntryOriginator();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MDEntryOriginator val) 
                { 
                    this.MDEntryOriginator = val;
                }
                
                public QuickFix.Fields.MDEntryOriginator Get(QuickFix.Fields.MDEntryOriginator val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MDEntryOriginator val) 
                { 
                    return IsSetMDEntryOriginator();
                }
                
                public bool IsSetMDEntryOriginator() 
                { 
                    return IsSetField(Tags.MDEntryOriginator);
                }
                public QuickFix.Fields.LocationID LocationID
                { 
                    get 
                    {
                        QuickFix.Fields.LocationID val = new QuickFix.Fields.LocationID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.LocationID val) 
                { 
                    this.LocationID = val;
                }
                
                public QuickFix.Fields.LocationID Get(QuickFix.Fields.LocationID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.LocationID val) 
                { 
                    return IsSetLocationID();
                }
                
                public bool IsSetLocationID() 
                { 
                    return IsSetField(Tags.LocationID);
                }
                public QuickFix.Fields.DeskID DeskID
                { 
                    get 
                    {
                        QuickFix.Fields.DeskID val = new QuickFix.Fields.DeskID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.DeskID val) 
                { 
                    this.DeskID = val;
                }
                
                public QuickFix.Fields.DeskID Get(QuickFix.Fields.DeskID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.DeskID val) 
                { 
                    return IsSetDeskID();
                }
                
                public bool IsSetDeskID() 
                { 
                    return IsSetField(Tags.DeskID);
                }
                public QuickFix.Fields.OpenCloseSettlFlag OpenCloseSettlFlag
                { 
                    get 
                    {
                        QuickFix.Fields.OpenCloseSettlFlag val = new QuickFix.Fields.OpenCloseSettlFlag();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.OpenCloseSettlFlag val) 
                { 
                    this.OpenCloseSettlFlag = val;
                }
                
                public QuickFix.Fields.OpenCloseSettlFlag Get(QuickFix.Fields.OpenCloseSettlFlag val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.OpenCloseSettlFlag val) 
                { 
                    return IsSetOpenCloseSettlFlag();
                }
                
                public bool IsSetOpenCloseSettlFlag() 
                { 
                    return IsSetField(Tags.OpenCloseSettlFlag);
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
                public QuickFix.Fields.EarlyTermination EarlyTermination
                { 
                    get 
                    {
                        QuickFix.Fields.EarlyTermination val = new QuickFix.Fields.EarlyTermination();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.EarlyTermination val) 
                { 
                    this.EarlyTermination = val;
                }
                
                public QuickFix.Fields.EarlyTermination Get(QuickFix.Fields.EarlyTermination val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.EarlyTermination val) 
                { 
                    return IsSetEarlyTermination();
                }
                
                public bool IsSetEarlyTermination() 
                { 
                    return IsSetField(Tags.EarlyTermination);
                }
                public QuickFix.Fields.MaxTradeVol MaxTradeVol
                { 
                    get 
                    {
                        QuickFix.Fields.MaxTradeVol val = new QuickFix.Fields.MaxTradeVol();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MaxTradeVol val) 
                { 
                    this.MaxTradeVol = val;
                }
                
                public QuickFix.Fields.MaxTradeVol Get(QuickFix.Fields.MaxTradeVol val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MaxTradeVol val) 
                { 
                    return IsSetMaxTradeVol();
                }
                
                public bool IsSetMaxTradeVol() 
                { 
                    return IsSetField(Tags.MaxTradeVol);
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
                public QuickFix.Fields.SellerDays SellerDays
                { 
                    get 
                    {
                        QuickFix.Fields.SellerDays val = new QuickFix.Fields.SellerDays();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.SellerDays val) 
                { 
                    this.SellerDays = val;
                }
                
                public QuickFix.Fields.SellerDays Get(QuickFix.Fields.SellerDays val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.SellerDays val) 
                { 
                    return IsSetSellerDays();
                }
                
                public bool IsSetSellerDays() 
                { 
                    return IsSetField(Tags.SellerDays);
                }
                public QuickFix.Fields.SettlPriceType SettlPriceType
                { 
                    get 
                    {
                        QuickFix.Fields.SettlPriceType val = new QuickFix.Fields.SettlPriceType();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.SettlPriceType val) 
                { 
                    this.SettlPriceType = val;
                }
                
                public QuickFix.Fields.SettlPriceType Get(QuickFix.Fields.SettlPriceType val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.SettlPriceType val) 
                { 
                    return IsSetSettlPriceType();
                }
                
                public bool IsSetSettlPriceType() 
                { 
                    return IsSetField(Tags.SettlPriceType);
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
                public QuickFix.Fields.QuoteEntryID QuoteEntryID
                { 
                    get 
                    {
                        QuickFix.Fields.QuoteEntryID val = new QuickFix.Fields.QuoteEntryID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.QuoteEntryID val) 
                { 
                    this.QuoteEntryID = val;
                }
                
                public QuickFix.Fields.QuoteEntryID Get(QuickFix.Fields.QuoteEntryID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.QuoteEntryID val) 
                { 
                    return IsSetQuoteEntryID();
                }
                
                public bool IsSetQuoteEntryID() 
                { 
                    return IsSetField(Tags.QuoteEntryID);
                }
                public QuickFix.Fields.MDEntryBuyer MDEntryBuyer
                { 
                    get 
                    {
                        QuickFix.Fields.MDEntryBuyer val = new QuickFix.Fields.MDEntryBuyer();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MDEntryBuyer val) 
                { 
                    this.MDEntryBuyer = val;
                }
                
                public QuickFix.Fields.MDEntryBuyer Get(QuickFix.Fields.MDEntryBuyer val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MDEntryBuyer val) 
                { 
                    return IsSetMDEntryBuyer();
                }
                
                public bool IsSetMDEntryBuyer() 
                { 
                    return IsSetField(Tags.MDEntryBuyer);
                }
                public QuickFix.Fields.MDEntrySeller MDEntrySeller
                { 
                    get 
                    {
                        QuickFix.Fields.MDEntrySeller val = new QuickFix.Fields.MDEntrySeller();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MDEntrySeller val) 
                { 
                    this.MDEntrySeller = val;
                }
                
                public QuickFix.Fields.MDEntrySeller Get(QuickFix.Fields.MDEntrySeller val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MDEntrySeller val) 
                { 
                    return IsSetMDEntrySeller();
                }
                
                public bool IsSetMDEntrySeller() 
                { 
                    return IsSetField(Tags.MDEntrySeller);
                }
                public QuickFix.Fields.NumberOfOrders NumberOfOrders
                { 
                    get 
                    {
                        QuickFix.Fields.NumberOfOrders val = new QuickFix.Fields.NumberOfOrders();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.NumberOfOrders val) 
                { 
                    this.NumberOfOrders = val;
                }
                
                public QuickFix.Fields.NumberOfOrders Get(QuickFix.Fields.NumberOfOrders val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.NumberOfOrders val) 
                { 
                    return IsSetNumberOfOrders();
                }
                
                public bool IsSetNumberOfOrders() 
                { 
                    return IsSetField(Tags.NumberOfOrders);
                }
                public QuickFix.Fields.MDEntryPositionNo MDEntryPositionNo
                { 
                    get 
                    {
                        QuickFix.Fields.MDEntryPositionNo val = new QuickFix.Fields.MDEntryPositionNo();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MDEntryPositionNo val) 
                { 
                    this.MDEntryPositionNo = val;
                }
                
                public QuickFix.Fields.MDEntryPositionNo Get(QuickFix.Fields.MDEntryPositionNo val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MDEntryPositionNo val) 
                { 
                    return IsSetMDEntryPositionNo();
                }
                
                public bool IsSetMDEntryPositionNo() 
                { 
                    return IsSetField(Tags.MDEntryPositionNo);
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
                public QuickFix.Fields.Scope Scope
                { 
                    get 
                    {
                        QuickFix.Fields.Scope val = new QuickFix.Fields.Scope();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.Scope val) 
                { 
                    this.Scope = val;
                }
                
                public QuickFix.Fields.Scope Get(QuickFix.Fields.Scope val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.Scope val) 
                { 
                    return IsSetScope();
                }
                
                public bool IsSetScope() 
                { 
                    return IsSetField(Tags.Scope);
                }
                public QuickFix.Fields.PriceDelta PriceDelta
                { 
                    get 
                    {
                        QuickFix.Fields.PriceDelta val = new QuickFix.Fields.PriceDelta();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.PriceDelta val) 
                { 
                    this.PriceDelta = val;
                }
                
                public QuickFix.Fields.PriceDelta Get(QuickFix.Fields.PriceDelta val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.PriceDelta val) 
                { 
                    return IsSetPriceDelta();
                }
                
                public bool IsSetPriceDelta() 
                { 
                    return IsSetField(Tags.PriceDelta);
                }
                public QuickFix.Fields.NetChgPrevDay NetChgPrevDay
                { 
                    get 
                    {
                        QuickFix.Fields.NetChgPrevDay val = new QuickFix.Fields.NetChgPrevDay();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.NetChgPrevDay val) 
                { 
                    this.NetChgPrevDay = val;
                }
                
                public QuickFix.Fields.NetChgPrevDay Get(QuickFix.Fields.NetChgPrevDay val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.NetChgPrevDay val) 
                { 
                    return IsSetNetChgPrevDay();
                }
                
                public bool IsSetNetChgPrevDay() 
                { 
                    return IsSetField(Tags.NetChgPrevDay);
                }
                public QuickFix.Fields.TradeID TradeID
                { 
                    get 
                    {
                        QuickFix.Fields.TradeID val = new QuickFix.Fields.TradeID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.TradeID val) 
                { 
                    this.TradeID = val;
                }
                
                public QuickFix.Fields.TradeID Get(QuickFix.Fields.TradeID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.TradeID val) 
                { 
                    return IsSetTradeID();
                }
                
                public bool IsSetTradeID() 
                { 
                    return IsSetField(Tags.TradeID);
                }
                public QuickFix.Fields.TradeVolume TradeVolume
                { 
                    get 
                    {
                        QuickFix.Fields.TradeVolume val = new QuickFix.Fields.TradeVolume();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.TradeVolume val) 
                { 
                    this.TradeVolume = val;
                }
                
                public QuickFix.Fields.TradeVolume Get(QuickFix.Fields.TradeVolume val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.TradeVolume val) 
                { 
                    return IsSetTradeVolume();
                }
                
                public bool IsSetTradeVolume() 
                { 
                    return IsSetField(Tags.TradeVolume);
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
                public QuickFix.Fields.SecurityTradingStatus SecurityTradingStatus
                { 
                    get 
                    {
                        QuickFix.Fields.SecurityTradingStatus val = new QuickFix.Fields.SecurityTradingStatus();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.SecurityTradingStatus val) 
                { 
                    this.SecurityTradingStatus = val;
                }
                
                public QuickFix.Fields.SecurityTradingStatus Get(QuickFix.Fields.SecurityTradingStatus val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.SecurityTradingStatus val) 
                { 
                    return IsSetSecurityTradingStatus();
                }
                
                public bool IsSetSecurityTradingStatus() 
                { 
                    return IsSetField(Tags.SecurityTradingStatus);
                }
                public QuickFix.Fields.Duration Duration
                { 
                    get 
                    {
                        QuickFix.Fields.Duration val = new QuickFix.Fields.Duration();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.Duration val) 
                { 
                    this.Duration = val;
                }
                
                public QuickFix.Fields.Duration Get(QuickFix.Fields.Duration val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.Duration val) 
                { 
                    return IsSetDuration();
                }
                
                public bool IsSetDuration() 
                { 
                    return IsSetField(Tags.Duration);
                }
                public QuickFix.Fields.NoReferentialPrices NoReferentialPrices
                { 
                    get 
                    {
                        QuickFix.Fields.NoReferentialPrices val = new QuickFix.Fields.NoReferentialPrices();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.NoReferentialPrices val) 
                { 
                    this.NoReferentialPrices = val;
                }
                
                public QuickFix.Fields.NoReferentialPrices Get(QuickFix.Fields.NoReferentialPrices val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.NoReferentialPrices val) 
                { 
                    return IsSetNoReferentialPrices();
                }
                
                public bool IsSetNoReferentialPrices() 
                { 
                    return IsSetField(Tags.NoReferentialPrices);
                }
                public QuickFix.Fields.TradSesOpenTime TradSesOpenTime
                { 
                    get 
                    {
                        QuickFix.Fields.TradSesOpenTime val = new QuickFix.Fields.TradSesOpenTime();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.TradSesOpenTime val) 
                { 
                    this.TradSesOpenTime = val;
                }
                
                public QuickFix.Fields.TradSesOpenTime Get(QuickFix.Fields.TradSesOpenTime val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.TradSesOpenTime val) 
                { 
                    return IsSetTradSesOpenTime();
                }
                
                public bool IsSetTradSesOpenTime() 
                { 
                    return IsSetField(Tags.TradSesOpenTime);
                }
                public QuickFix.Fields.RLCMsgID RLCMsgID
                { 
                    get 
                    {
                        QuickFix.Fields.RLCMsgID val = new QuickFix.Fields.RLCMsgID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.RLCMsgID val) 
                { 
                    this.RLCMsgID = val;
                }
                
                public QuickFix.Fields.RLCMsgID Get(QuickFix.Fields.RLCMsgID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.RLCMsgID val) 
                { 
                    return IsSetRLCMsgID();
                }
                
                public bool IsSetRLCMsgID() 
                { 
                    return IsSetField(Tags.RLCMsgID);
                }
                public QuickFix.Fields.MicrosecondOrderTimestamp MicrosecondOrderTimestamp
                { 
                    get 
                    {
                        QuickFix.Fields.MicrosecondOrderTimestamp val = new QuickFix.Fields.MicrosecondOrderTimestamp();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MicrosecondOrderTimestamp val) 
                { 
                    this.MicrosecondOrderTimestamp = val;
                }
                
                public QuickFix.Fields.MicrosecondOrderTimestamp Get(QuickFix.Fields.MicrosecondOrderTimestamp val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MicrosecondOrderTimestamp val) 
                { 
                    return IsSetMicrosecondOrderTimestamp();
                }
                
                public bool IsSetMicrosecondOrderTimestamp() 
                { 
                    return IsSetField(Tags.MicrosecondOrderTimestamp);
                }
                public QuickFix.Fields.IndexSeq IndexSeq
                { 
                    get 
                    {
                        QuickFix.Fields.IndexSeq val = new QuickFix.Fields.IndexSeq();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.IndexSeq val) 
                { 
                    this.IndexSeq = val;
                }
                
                public QuickFix.Fields.IndexSeq Get(QuickFix.Fields.IndexSeq val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.IndexSeq val) 
                { 
                    return IsSetIndexSeq();
                }
                
                public bool IsSetIndexSeq() 
                { 
                    return IsSetField(Tags.IndexSeq);
                }
                            public class NoSecurityAltIDGroup : Group
                {
                    public static int[] fieldOrder = {Tags.SecurityAltID, Tags.SecurityAltIDSource, 0};
                
                    public NoSecurityAltIDGroup() 
                      :base( Tags.NoSecurityAltID, Tags.SecurityAltID, fieldOrder)
                    {
                    }
                
                    public override Group Clone()
                    {
                        var clone = new NoSecurityAltIDGroup();
                        clone.CopyStateFrom(this);
                        return clone;
                    }
                
                                    public QuickFix.Fields.SecurityAltID SecurityAltID
                    { 
                        get 
                        {
                            QuickFix.Fields.SecurityAltID val = new QuickFix.Fields.SecurityAltID();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.SecurityAltID val) 
                    { 
                        this.SecurityAltID = val;
                    }
                    
                    public QuickFix.Fields.SecurityAltID Get(QuickFix.Fields.SecurityAltID val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.SecurityAltID val) 
                    { 
                        return IsSetSecurityAltID();
                    }
                    
                    public bool IsSetSecurityAltID() 
                    { 
                        return IsSetField(Tags.SecurityAltID);
                    }
                    public QuickFix.Fields.SecurityAltIDSource SecurityAltIDSource
                    { 
                        get 
                        {
                            QuickFix.Fields.SecurityAltIDSource val = new QuickFix.Fields.SecurityAltIDSource();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.SecurityAltIDSource val) 
                    { 
                        this.SecurityAltIDSource = val;
                    }
                    
                    public QuickFix.Fields.SecurityAltIDSource Get(QuickFix.Fields.SecurityAltIDSource val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.SecurityAltIDSource val) 
                    { 
                        return IsSetSecurityAltIDSource();
                    }
                    
                    public bool IsSetSecurityAltIDSource() 
                    { 
                        return IsSetField(Tags.SecurityAltIDSource);
                    }
                
                }
                public class NoEventsGroup : Group
                {
                    public static int[] fieldOrder = {Tags.EventType, Tags.EventDate, Tags.EventPx, Tags.EventText, 0};
                
                    public NoEventsGroup() 
                      :base( Tags.NoEvents, Tags.EventType, fieldOrder)
                    {
                    }
                
                    public override Group Clone()
                    {
                        var clone = new NoEventsGroup();
                        clone.CopyStateFrom(this);
                        return clone;
                    }
                
                                    public QuickFix.Fields.EventType EventType
                    { 
                        get 
                        {
                            QuickFix.Fields.EventType val = new QuickFix.Fields.EventType();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.EventType val) 
                    { 
                        this.EventType = val;
                    }
                    
                    public QuickFix.Fields.EventType Get(QuickFix.Fields.EventType val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.EventType val) 
                    { 
                        return IsSetEventType();
                    }
                    
                    public bool IsSetEventType() 
                    { 
                        return IsSetField(Tags.EventType);
                    }
                    public QuickFix.Fields.EventDate EventDate
                    { 
                        get 
                        {
                            QuickFix.Fields.EventDate val = new QuickFix.Fields.EventDate();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.EventDate val) 
                    { 
                        this.EventDate = val;
                    }
                    
                    public QuickFix.Fields.EventDate Get(QuickFix.Fields.EventDate val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.EventDate val) 
                    { 
                        return IsSetEventDate();
                    }
                    
                    public bool IsSetEventDate() 
                    { 
                        return IsSetField(Tags.EventDate);
                    }
                    public QuickFix.Fields.EventPx EventPx
                    { 
                        get 
                        {
                            QuickFix.Fields.EventPx val = new QuickFix.Fields.EventPx();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.EventPx val) 
                    { 
                        this.EventPx = val;
                    }
                    
                    public QuickFix.Fields.EventPx Get(QuickFix.Fields.EventPx val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.EventPx val) 
                    { 
                        return IsSetEventPx();
                    }
                    
                    public bool IsSetEventPx() 
                    { 
                        return IsSetField(Tags.EventPx);
                    }
                    public QuickFix.Fields.EventText EventText
                    { 
                        get 
                        {
                            QuickFix.Fields.EventText val = new QuickFix.Fields.EventText();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.EventText val) 
                    { 
                        this.EventText = val;
                    }
                    
                    public QuickFix.Fields.EventText Get(QuickFix.Fields.EventText val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.EventText val) 
                    { 
                        return IsSetEventText();
                    }
                    
                    public bool IsSetEventText() 
                    { 
                        return IsSetField(Tags.EventText);
                    }
                
                }
                public class NoUnderlyingsGroup : Group
                {
                    public static int[] fieldOrder = {Tags.UnderlyingSecurityID, Tags.UnderlyingSecurityIDSource, Tags.UnderlyingSecurityExchange, Tags.UnderlyingPx, Tags.UnderlyingPxType, 0};
                
                    public NoUnderlyingsGroup() 
                      :base( Tags.NoUnderlyings, Tags.UnderlyingSecurityID, fieldOrder)
                    {
                    }
                
                    public override Group Clone()
                    {
                        var clone = new NoUnderlyingsGroup();
                        clone.CopyStateFrom(this);
                        return clone;
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
                    public QuickFix.Fields.UnderlyingPx UnderlyingPx
                    { 
                        get 
                        {
                            QuickFix.Fields.UnderlyingPx val = new QuickFix.Fields.UnderlyingPx();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.UnderlyingPx val) 
                    { 
                        this.UnderlyingPx = val;
                    }
                    
                    public QuickFix.Fields.UnderlyingPx Get(QuickFix.Fields.UnderlyingPx val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.UnderlyingPx val) 
                    { 
                        return IsSetUnderlyingPx();
                    }
                    
                    public bool IsSetUnderlyingPx() 
                    { 
                        return IsSetField(Tags.UnderlyingPx);
                    }
                    public QuickFix.Fields.UnderlyingPxType UnderlyingPxType
                    { 
                        get 
                        {
                            QuickFix.Fields.UnderlyingPxType val = new QuickFix.Fields.UnderlyingPxType();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.UnderlyingPxType val) 
                    { 
                        this.UnderlyingPxType = val;
                    }
                    
                    public QuickFix.Fields.UnderlyingPxType Get(QuickFix.Fields.UnderlyingPxType val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.UnderlyingPxType val) 
                    { 
                        return IsSetUnderlyingPxType();
                    }
                    
                    public bool IsSetUnderlyingPxType() 
                    { 
                        return IsSetField(Tags.UnderlyingPxType);
                    }
                
                }
                public class NoLegsGroup : Group
                {
                    public static int[] fieldOrder = {Tags.LegSymbol, Tags.LegSymbolSfx, Tags.LegSecurityID, Tags.LegSecurityIDSource, Tags.NoLegSecurityAltID, Tags.LegProduct, Tags.LegCFICode, Tags.LegSecurityType, Tags.LegSecuritySubType, Tags.LegMaturityMonthYear, Tags.LegMaturityDate, Tags.LegCouponPaymentDate, Tags.LegIssueDate, Tags.LegRepoCollateralSecurityType, Tags.LegRepurchaseTerm, Tags.LegRepurchaseRate, Tags.LegFactor, Tags.LegCreditRating, Tags.LegInstrRegistry, Tags.LegCountryOfIssue, Tags.LegStateOrProvinceOfIssue, Tags.LegLocaleOfIssue, Tags.LegRedemptionDate, Tags.LegStrikePrice, Tags.LegStrikeCurrency, Tags.LegOptAttribute, Tags.LegContractMultiplier, Tags.LegCouponRate, Tags.LegSecurityExchange, Tags.LegIssuer, Tags.EncodedLegIssuerLen, Tags.EncodedLegIssuer, Tags.LegSecurityDesc, Tags.EncodedLegSecurityDescLen, Tags.EncodedLegSecurityDesc, Tags.LegRatioQty, Tags.LegSide, Tags.LegCurrency, Tags.LegPool, Tags.LegDatedDate, Tags.LegContractSettlMonth, Tags.LegInterestAccrualDate, 0};
                
                    public NoLegsGroup() 
                      :base( Tags.NoLegs, Tags.LegSymbol, fieldOrder)
                    {
                    }
                
                    public override Group Clone()
                    {
                        var clone = new NoLegsGroup();
                        clone.CopyStateFrom(this);
                        return clone;
                    }
                
                                    public QuickFix.Fields.LegSymbol LegSymbol
                    { 
                        get 
                        {
                            QuickFix.Fields.LegSymbol val = new QuickFix.Fields.LegSymbol();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegSymbol val) 
                    { 
                        this.LegSymbol = val;
                    }
                    
                    public QuickFix.Fields.LegSymbol Get(QuickFix.Fields.LegSymbol val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegSymbol val) 
                    { 
                        return IsSetLegSymbol();
                    }
                    
                    public bool IsSetLegSymbol() 
                    { 
                        return IsSetField(Tags.LegSymbol);
                    }
                    public QuickFix.Fields.LegSymbolSfx LegSymbolSfx
                    { 
                        get 
                        {
                            QuickFix.Fields.LegSymbolSfx val = new QuickFix.Fields.LegSymbolSfx();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegSymbolSfx val) 
                    { 
                        this.LegSymbolSfx = val;
                    }
                    
                    public QuickFix.Fields.LegSymbolSfx Get(QuickFix.Fields.LegSymbolSfx val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegSymbolSfx val) 
                    { 
                        return IsSetLegSymbolSfx();
                    }
                    
                    public bool IsSetLegSymbolSfx() 
                    { 
                        return IsSetField(Tags.LegSymbolSfx);
                    }
                    public QuickFix.Fields.LegSecurityID LegSecurityID
                    { 
                        get 
                        {
                            QuickFix.Fields.LegSecurityID val = new QuickFix.Fields.LegSecurityID();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegSecurityID val) 
                    { 
                        this.LegSecurityID = val;
                    }
                    
                    public QuickFix.Fields.LegSecurityID Get(QuickFix.Fields.LegSecurityID val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegSecurityID val) 
                    { 
                        return IsSetLegSecurityID();
                    }
                    
                    public bool IsSetLegSecurityID() 
                    { 
                        return IsSetField(Tags.LegSecurityID);
                    }
                    public QuickFix.Fields.LegSecurityIDSource LegSecurityIDSource
                    { 
                        get 
                        {
                            QuickFix.Fields.LegSecurityIDSource val = new QuickFix.Fields.LegSecurityIDSource();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegSecurityIDSource val) 
                    { 
                        this.LegSecurityIDSource = val;
                    }
                    
                    public QuickFix.Fields.LegSecurityIDSource Get(QuickFix.Fields.LegSecurityIDSource val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegSecurityIDSource val) 
                    { 
                        return IsSetLegSecurityIDSource();
                    }
                    
                    public bool IsSetLegSecurityIDSource() 
                    { 
                        return IsSetField(Tags.LegSecurityIDSource);
                    }
                    public QuickFix.Fields.NoLegSecurityAltID NoLegSecurityAltID
                    { 
                        get 
                        {
                            QuickFix.Fields.NoLegSecurityAltID val = new QuickFix.Fields.NoLegSecurityAltID();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.NoLegSecurityAltID val) 
                    { 
                        this.NoLegSecurityAltID = val;
                    }
                    
                    public QuickFix.Fields.NoLegSecurityAltID Get(QuickFix.Fields.NoLegSecurityAltID val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.NoLegSecurityAltID val) 
                    { 
                        return IsSetNoLegSecurityAltID();
                    }
                    
                    public bool IsSetNoLegSecurityAltID() 
                    { 
                        return IsSetField(Tags.NoLegSecurityAltID);
                    }
                    public QuickFix.Fields.LegProduct LegProduct
                    { 
                        get 
                        {
                            QuickFix.Fields.LegProduct val = new QuickFix.Fields.LegProduct();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegProduct val) 
                    { 
                        this.LegProduct = val;
                    }
                    
                    public QuickFix.Fields.LegProduct Get(QuickFix.Fields.LegProduct val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegProduct val) 
                    { 
                        return IsSetLegProduct();
                    }
                    
                    public bool IsSetLegProduct() 
                    { 
                        return IsSetField(Tags.LegProduct);
                    }
                    public QuickFix.Fields.LegCFICode LegCFICode
                    { 
                        get 
                        {
                            QuickFix.Fields.LegCFICode val = new QuickFix.Fields.LegCFICode();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegCFICode val) 
                    { 
                        this.LegCFICode = val;
                    }
                    
                    public QuickFix.Fields.LegCFICode Get(QuickFix.Fields.LegCFICode val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegCFICode val) 
                    { 
                        return IsSetLegCFICode();
                    }
                    
                    public bool IsSetLegCFICode() 
                    { 
                        return IsSetField(Tags.LegCFICode);
                    }
                    public QuickFix.Fields.LegSecurityType LegSecurityType
                    { 
                        get 
                        {
                            QuickFix.Fields.LegSecurityType val = new QuickFix.Fields.LegSecurityType();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegSecurityType val) 
                    { 
                        this.LegSecurityType = val;
                    }
                    
                    public QuickFix.Fields.LegSecurityType Get(QuickFix.Fields.LegSecurityType val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegSecurityType val) 
                    { 
                        return IsSetLegSecurityType();
                    }
                    
                    public bool IsSetLegSecurityType() 
                    { 
                        return IsSetField(Tags.LegSecurityType);
                    }
                    public QuickFix.Fields.LegSecuritySubType LegSecuritySubType
                    { 
                        get 
                        {
                            QuickFix.Fields.LegSecuritySubType val = new QuickFix.Fields.LegSecuritySubType();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegSecuritySubType val) 
                    { 
                        this.LegSecuritySubType = val;
                    }
                    
                    public QuickFix.Fields.LegSecuritySubType Get(QuickFix.Fields.LegSecuritySubType val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegSecuritySubType val) 
                    { 
                        return IsSetLegSecuritySubType();
                    }
                    
                    public bool IsSetLegSecuritySubType() 
                    { 
                        return IsSetField(Tags.LegSecuritySubType);
                    }
                    public QuickFix.Fields.LegMaturityMonthYear LegMaturityMonthYear
                    { 
                        get 
                        {
                            QuickFix.Fields.LegMaturityMonthYear val = new QuickFix.Fields.LegMaturityMonthYear();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegMaturityMonthYear val) 
                    { 
                        this.LegMaturityMonthYear = val;
                    }
                    
                    public QuickFix.Fields.LegMaturityMonthYear Get(QuickFix.Fields.LegMaturityMonthYear val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegMaturityMonthYear val) 
                    { 
                        return IsSetLegMaturityMonthYear();
                    }
                    
                    public bool IsSetLegMaturityMonthYear() 
                    { 
                        return IsSetField(Tags.LegMaturityMonthYear);
                    }
                    public QuickFix.Fields.LegMaturityDate LegMaturityDate
                    { 
                        get 
                        {
                            QuickFix.Fields.LegMaturityDate val = new QuickFix.Fields.LegMaturityDate();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegMaturityDate val) 
                    { 
                        this.LegMaturityDate = val;
                    }
                    
                    public QuickFix.Fields.LegMaturityDate Get(QuickFix.Fields.LegMaturityDate val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegMaturityDate val) 
                    { 
                        return IsSetLegMaturityDate();
                    }
                    
                    public bool IsSetLegMaturityDate() 
                    { 
                        return IsSetField(Tags.LegMaturityDate);
                    }
                    public QuickFix.Fields.LegCouponPaymentDate LegCouponPaymentDate
                    { 
                        get 
                        {
                            QuickFix.Fields.LegCouponPaymentDate val = new QuickFix.Fields.LegCouponPaymentDate();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegCouponPaymentDate val) 
                    { 
                        this.LegCouponPaymentDate = val;
                    }
                    
                    public QuickFix.Fields.LegCouponPaymentDate Get(QuickFix.Fields.LegCouponPaymentDate val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegCouponPaymentDate val) 
                    { 
                        return IsSetLegCouponPaymentDate();
                    }
                    
                    public bool IsSetLegCouponPaymentDate() 
                    { 
                        return IsSetField(Tags.LegCouponPaymentDate);
                    }
                    public QuickFix.Fields.LegIssueDate LegIssueDate
                    { 
                        get 
                        {
                            QuickFix.Fields.LegIssueDate val = new QuickFix.Fields.LegIssueDate();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegIssueDate val) 
                    { 
                        this.LegIssueDate = val;
                    }
                    
                    public QuickFix.Fields.LegIssueDate Get(QuickFix.Fields.LegIssueDate val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegIssueDate val) 
                    { 
                        return IsSetLegIssueDate();
                    }
                    
                    public bool IsSetLegIssueDate() 
                    { 
                        return IsSetField(Tags.LegIssueDate);
                    }
                    public QuickFix.Fields.LegRepoCollateralSecurityType LegRepoCollateralSecurityType
                    { 
                        get 
                        {
                            QuickFix.Fields.LegRepoCollateralSecurityType val = new QuickFix.Fields.LegRepoCollateralSecurityType();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegRepoCollateralSecurityType val) 
                    { 
                        this.LegRepoCollateralSecurityType = val;
                    }
                    
                    public QuickFix.Fields.LegRepoCollateralSecurityType Get(QuickFix.Fields.LegRepoCollateralSecurityType val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegRepoCollateralSecurityType val) 
                    { 
                        return IsSetLegRepoCollateralSecurityType();
                    }
                    
                    public bool IsSetLegRepoCollateralSecurityType() 
                    { 
                        return IsSetField(Tags.LegRepoCollateralSecurityType);
                    }
                    public QuickFix.Fields.LegRepurchaseTerm LegRepurchaseTerm
                    { 
                        get 
                        {
                            QuickFix.Fields.LegRepurchaseTerm val = new QuickFix.Fields.LegRepurchaseTerm();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegRepurchaseTerm val) 
                    { 
                        this.LegRepurchaseTerm = val;
                    }
                    
                    public QuickFix.Fields.LegRepurchaseTerm Get(QuickFix.Fields.LegRepurchaseTerm val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegRepurchaseTerm val) 
                    { 
                        return IsSetLegRepurchaseTerm();
                    }
                    
                    public bool IsSetLegRepurchaseTerm() 
                    { 
                        return IsSetField(Tags.LegRepurchaseTerm);
                    }
                    public QuickFix.Fields.LegRepurchaseRate LegRepurchaseRate
                    { 
                        get 
                        {
                            QuickFix.Fields.LegRepurchaseRate val = new QuickFix.Fields.LegRepurchaseRate();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegRepurchaseRate val) 
                    { 
                        this.LegRepurchaseRate = val;
                    }
                    
                    public QuickFix.Fields.LegRepurchaseRate Get(QuickFix.Fields.LegRepurchaseRate val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegRepurchaseRate val) 
                    { 
                        return IsSetLegRepurchaseRate();
                    }
                    
                    public bool IsSetLegRepurchaseRate() 
                    { 
                        return IsSetField(Tags.LegRepurchaseRate);
                    }
                    public QuickFix.Fields.LegFactor LegFactor
                    { 
                        get 
                        {
                            QuickFix.Fields.LegFactor val = new QuickFix.Fields.LegFactor();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegFactor val) 
                    { 
                        this.LegFactor = val;
                    }
                    
                    public QuickFix.Fields.LegFactor Get(QuickFix.Fields.LegFactor val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegFactor val) 
                    { 
                        return IsSetLegFactor();
                    }
                    
                    public bool IsSetLegFactor() 
                    { 
                        return IsSetField(Tags.LegFactor);
                    }
                    public QuickFix.Fields.LegCreditRating LegCreditRating
                    { 
                        get 
                        {
                            QuickFix.Fields.LegCreditRating val = new QuickFix.Fields.LegCreditRating();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegCreditRating val) 
                    { 
                        this.LegCreditRating = val;
                    }
                    
                    public QuickFix.Fields.LegCreditRating Get(QuickFix.Fields.LegCreditRating val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegCreditRating val) 
                    { 
                        return IsSetLegCreditRating();
                    }
                    
                    public bool IsSetLegCreditRating() 
                    { 
                        return IsSetField(Tags.LegCreditRating);
                    }
                    public QuickFix.Fields.LegInstrRegistry LegInstrRegistry
                    { 
                        get 
                        {
                            QuickFix.Fields.LegInstrRegistry val = new QuickFix.Fields.LegInstrRegistry();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegInstrRegistry val) 
                    { 
                        this.LegInstrRegistry = val;
                    }
                    
                    public QuickFix.Fields.LegInstrRegistry Get(QuickFix.Fields.LegInstrRegistry val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegInstrRegistry val) 
                    { 
                        return IsSetLegInstrRegistry();
                    }
                    
                    public bool IsSetLegInstrRegistry() 
                    { 
                        return IsSetField(Tags.LegInstrRegistry);
                    }
                    public QuickFix.Fields.LegCountryOfIssue LegCountryOfIssue
                    { 
                        get 
                        {
                            QuickFix.Fields.LegCountryOfIssue val = new QuickFix.Fields.LegCountryOfIssue();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegCountryOfIssue val) 
                    { 
                        this.LegCountryOfIssue = val;
                    }
                    
                    public QuickFix.Fields.LegCountryOfIssue Get(QuickFix.Fields.LegCountryOfIssue val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegCountryOfIssue val) 
                    { 
                        return IsSetLegCountryOfIssue();
                    }
                    
                    public bool IsSetLegCountryOfIssue() 
                    { 
                        return IsSetField(Tags.LegCountryOfIssue);
                    }
                    public QuickFix.Fields.LegStateOrProvinceOfIssue LegStateOrProvinceOfIssue
                    { 
                        get 
                        {
                            QuickFix.Fields.LegStateOrProvinceOfIssue val = new QuickFix.Fields.LegStateOrProvinceOfIssue();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegStateOrProvinceOfIssue val) 
                    { 
                        this.LegStateOrProvinceOfIssue = val;
                    }
                    
                    public QuickFix.Fields.LegStateOrProvinceOfIssue Get(QuickFix.Fields.LegStateOrProvinceOfIssue val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegStateOrProvinceOfIssue val) 
                    { 
                        return IsSetLegStateOrProvinceOfIssue();
                    }
                    
                    public bool IsSetLegStateOrProvinceOfIssue() 
                    { 
                        return IsSetField(Tags.LegStateOrProvinceOfIssue);
                    }
                    public QuickFix.Fields.LegLocaleOfIssue LegLocaleOfIssue
                    { 
                        get 
                        {
                            QuickFix.Fields.LegLocaleOfIssue val = new QuickFix.Fields.LegLocaleOfIssue();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegLocaleOfIssue val) 
                    { 
                        this.LegLocaleOfIssue = val;
                    }
                    
                    public QuickFix.Fields.LegLocaleOfIssue Get(QuickFix.Fields.LegLocaleOfIssue val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegLocaleOfIssue val) 
                    { 
                        return IsSetLegLocaleOfIssue();
                    }
                    
                    public bool IsSetLegLocaleOfIssue() 
                    { 
                        return IsSetField(Tags.LegLocaleOfIssue);
                    }
                    public QuickFix.Fields.LegRedemptionDate LegRedemptionDate
                    { 
                        get 
                        {
                            QuickFix.Fields.LegRedemptionDate val = new QuickFix.Fields.LegRedemptionDate();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegRedemptionDate val) 
                    { 
                        this.LegRedemptionDate = val;
                    }
                    
                    public QuickFix.Fields.LegRedemptionDate Get(QuickFix.Fields.LegRedemptionDate val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegRedemptionDate val) 
                    { 
                        return IsSetLegRedemptionDate();
                    }
                    
                    public bool IsSetLegRedemptionDate() 
                    { 
                        return IsSetField(Tags.LegRedemptionDate);
                    }
                    public QuickFix.Fields.LegStrikePrice LegStrikePrice
                    { 
                        get 
                        {
                            QuickFix.Fields.LegStrikePrice val = new QuickFix.Fields.LegStrikePrice();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegStrikePrice val) 
                    { 
                        this.LegStrikePrice = val;
                    }
                    
                    public QuickFix.Fields.LegStrikePrice Get(QuickFix.Fields.LegStrikePrice val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegStrikePrice val) 
                    { 
                        return IsSetLegStrikePrice();
                    }
                    
                    public bool IsSetLegStrikePrice() 
                    { 
                        return IsSetField(Tags.LegStrikePrice);
                    }
                    public QuickFix.Fields.LegStrikeCurrency LegStrikeCurrency
                    { 
                        get 
                        {
                            QuickFix.Fields.LegStrikeCurrency val = new QuickFix.Fields.LegStrikeCurrency();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegStrikeCurrency val) 
                    { 
                        this.LegStrikeCurrency = val;
                    }
                    
                    public QuickFix.Fields.LegStrikeCurrency Get(QuickFix.Fields.LegStrikeCurrency val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegStrikeCurrency val) 
                    { 
                        return IsSetLegStrikeCurrency();
                    }
                    
                    public bool IsSetLegStrikeCurrency() 
                    { 
                        return IsSetField(Tags.LegStrikeCurrency);
                    }
                    public QuickFix.Fields.LegOptAttribute LegOptAttribute
                    { 
                        get 
                        {
                            QuickFix.Fields.LegOptAttribute val = new QuickFix.Fields.LegOptAttribute();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegOptAttribute val) 
                    { 
                        this.LegOptAttribute = val;
                    }
                    
                    public QuickFix.Fields.LegOptAttribute Get(QuickFix.Fields.LegOptAttribute val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegOptAttribute val) 
                    { 
                        return IsSetLegOptAttribute();
                    }
                    
                    public bool IsSetLegOptAttribute() 
                    { 
                        return IsSetField(Tags.LegOptAttribute);
                    }
                    public QuickFix.Fields.LegContractMultiplier LegContractMultiplier
                    { 
                        get 
                        {
                            QuickFix.Fields.LegContractMultiplier val = new QuickFix.Fields.LegContractMultiplier();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegContractMultiplier val) 
                    { 
                        this.LegContractMultiplier = val;
                    }
                    
                    public QuickFix.Fields.LegContractMultiplier Get(QuickFix.Fields.LegContractMultiplier val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegContractMultiplier val) 
                    { 
                        return IsSetLegContractMultiplier();
                    }
                    
                    public bool IsSetLegContractMultiplier() 
                    { 
                        return IsSetField(Tags.LegContractMultiplier);
                    }
                    public QuickFix.Fields.LegCouponRate LegCouponRate
                    { 
                        get 
                        {
                            QuickFix.Fields.LegCouponRate val = new QuickFix.Fields.LegCouponRate();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegCouponRate val) 
                    { 
                        this.LegCouponRate = val;
                    }
                    
                    public QuickFix.Fields.LegCouponRate Get(QuickFix.Fields.LegCouponRate val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegCouponRate val) 
                    { 
                        return IsSetLegCouponRate();
                    }
                    
                    public bool IsSetLegCouponRate() 
                    { 
                        return IsSetField(Tags.LegCouponRate);
                    }
                    public QuickFix.Fields.LegSecurityExchange LegSecurityExchange
                    { 
                        get 
                        {
                            QuickFix.Fields.LegSecurityExchange val = new QuickFix.Fields.LegSecurityExchange();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegSecurityExchange val) 
                    { 
                        this.LegSecurityExchange = val;
                    }
                    
                    public QuickFix.Fields.LegSecurityExchange Get(QuickFix.Fields.LegSecurityExchange val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegSecurityExchange val) 
                    { 
                        return IsSetLegSecurityExchange();
                    }
                    
                    public bool IsSetLegSecurityExchange() 
                    { 
                        return IsSetField(Tags.LegSecurityExchange);
                    }
                    public QuickFix.Fields.LegIssuer LegIssuer
                    { 
                        get 
                        {
                            QuickFix.Fields.LegIssuer val = new QuickFix.Fields.LegIssuer();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegIssuer val) 
                    { 
                        this.LegIssuer = val;
                    }
                    
                    public QuickFix.Fields.LegIssuer Get(QuickFix.Fields.LegIssuer val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegIssuer val) 
                    { 
                        return IsSetLegIssuer();
                    }
                    
                    public bool IsSetLegIssuer() 
                    { 
                        return IsSetField(Tags.LegIssuer);
                    }
                    public QuickFix.Fields.EncodedLegIssuerLen EncodedLegIssuerLen
                    { 
                        get 
                        {
                            QuickFix.Fields.EncodedLegIssuerLen val = new QuickFix.Fields.EncodedLegIssuerLen();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.EncodedLegIssuerLen val) 
                    { 
                        this.EncodedLegIssuerLen = val;
                    }
                    
                    public QuickFix.Fields.EncodedLegIssuerLen Get(QuickFix.Fields.EncodedLegIssuerLen val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.EncodedLegIssuerLen val) 
                    { 
                        return IsSetEncodedLegIssuerLen();
                    }
                    
                    public bool IsSetEncodedLegIssuerLen() 
                    { 
                        return IsSetField(Tags.EncodedLegIssuerLen);
                    }
                    public QuickFix.Fields.EncodedLegIssuer EncodedLegIssuer
                    { 
                        get 
                        {
                            QuickFix.Fields.EncodedLegIssuer val = new QuickFix.Fields.EncodedLegIssuer();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.EncodedLegIssuer val) 
                    { 
                        this.EncodedLegIssuer = val;
                    }
                    
                    public QuickFix.Fields.EncodedLegIssuer Get(QuickFix.Fields.EncodedLegIssuer val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.EncodedLegIssuer val) 
                    { 
                        return IsSetEncodedLegIssuer();
                    }
                    
                    public bool IsSetEncodedLegIssuer() 
                    { 
                        return IsSetField(Tags.EncodedLegIssuer);
                    }
                    public QuickFix.Fields.LegSecurityDesc LegSecurityDesc
                    { 
                        get 
                        {
                            QuickFix.Fields.LegSecurityDesc val = new QuickFix.Fields.LegSecurityDesc();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegSecurityDesc val) 
                    { 
                        this.LegSecurityDesc = val;
                    }
                    
                    public QuickFix.Fields.LegSecurityDesc Get(QuickFix.Fields.LegSecurityDesc val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegSecurityDesc val) 
                    { 
                        return IsSetLegSecurityDesc();
                    }
                    
                    public bool IsSetLegSecurityDesc() 
                    { 
                        return IsSetField(Tags.LegSecurityDesc);
                    }
                    public QuickFix.Fields.EncodedLegSecurityDescLen EncodedLegSecurityDescLen
                    { 
                        get 
                        {
                            QuickFix.Fields.EncodedLegSecurityDescLen val = new QuickFix.Fields.EncodedLegSecurityDescLen();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.EncodedLegSecurityDescLen val) 
                    { 
                        this.EncodedLegSecurityDescLen = val;
                    }
                    
                    public QuickFix.Fields.EncodedLegSecurityDescLen Get(QuickFix.Fields.EncodedLegSecurityDescLen val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.EncodedLegSecurityDescLen val) 
                    { 
                        return IsSetEncodedLegSecurityDescLen();
                    }
                    
                    public bool IsSetEncodedLegSecurityDescLen() 
                    { 
                        return IsSetField(Tags.EncodedLegSecurityDescLen);
                    }
                    public QuickFix.Fields.EncodedLegSecurityDesc EncodedLegSecurityDesc
                    { 
                        get 
                        {
                            QuickFix.Fields.EncodedLegSecurityDesc val = new QuickFix.Fields.EncodedLegSecurityDesc();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.EncodedLegSecurityDesc val) 
                    { 
                        this.EncodedLegSecurityDesc = val;
                    }
                    
                    public QuickFix.Fields.EncodedLegSecurityDesc Get(QuickFix.Fields.EncodedLegSecurityDesc val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.EncodedLegSecurityDesc val) 
                    { 
                        return IsSetEncodedLegSecurityDesc();
                    }
                    
                    public bool IsSetEncodedLegSecurityDesc() 
                    { 
                        return IsSetField(Tags.EncodedLegSecurityDesc);
                    }
                    public QuickFix.Fields.LegRatioQty LegRatioQty
                    { 
                        get 
                        {
                            QuickFix.Fields.LegRatioQty val = new QuickFix.Fields.LegRatioQty();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegRatioQty val) 
                    { 
                        this.LegRatioQty = val;
                    }
                    
                    public QuickFix.Fields.LegRatioQty Get(QuickFix.Fields.LegRatioQty val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegRatioQty val) 
                    { 
                        return IsSetLegRatioQty();
                    }
                    
                    public bool IsSetLegRatioQty() 
                    { 
                        return IsSetField(Tags.LegRatioQty);
                    }
                    public QuickFix.Fields.LegSide LegSide
                    { 
                        get 
                        {
                            QuickFix.Fields.LegSide val = new QuickFix.Fields.LegSide();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegSide val) 
                    { 
                        this.LegSide = val;
                    }
                    
                    public QuickFix.Fields.LegSide Get(QuickFix.Fields.LegSide val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegSide val) 
                    { 
                        return IsSetLegSide();
                    }
                    
                    public bool IsSetLegSide() 
                    { 
                        return IsSetField(Tags.LegSide);
                    }
                    public QuickFix.Fields.LegCurrency LegCurrency
                    { 
                        get 
                        {
                            QuickFix.Fields.LegCurrency val = new QuickFix.Fields.LegCurrency();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegCurrency val) 
                    { 
                        this.LegCurrency = val;
                    }
                    
                    public QuickFix.Fields.LegCurrency Get(QuickFix.Fields.LegCurrency val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegCurrency val) 
                    { 
                        return IsSetLegCurrency();
                    }
                    
                    public bool IsSetLegCurrency() 
                    { 
                        return IsSetField(Tags.LegCurrency);
                    }
                    public QuickFix.Fields.LegPool LegPool
                    { 
                        get 
                        {
                            QuickFix.Fields.LegPool val = new QuickFix.Fields.LegPool();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegPool val) 
                    { 
                        this.LegPool = val;
                    }
                    
                    public QuickFix.Fields.LegPool Get(QuickFix.Fields.LegPool val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegPool val) 
                    { 
                        return IsSetLegPool();
                    }
                    
                    public bool IsSetLegPool() 
                    { 
                        return IsSetField(Tags.LegPool);
                    }
                    public QuickFix.Fields.LegDatedDate LegDatedDate
                    { 
                        get 
                        {
                            QuickFix.Fields.LegDatedDate val = new QuickFix.Fields.LegDatedDate();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegDatedDate val) 
                    { 
                        this.LegDatedDate = val;
                    }
                    
                    public QuickFix.Fields.LegDatedDate Get(QuickFix.Fields.LegDatedDate val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegDatedDate val) 
                    { 
                        return IsSetLegDatedDate();
                    }
                    
                    public bool IsSetLegDatedDate() 
                    { 
                        return IsSetField(Tags.LegDatedDate);
                    }
                    public QuickFix.Fields.LegContractSettlMonth LegContractSettlMonth
                    { 
                        get 
                        {
                            QuickFix.Fields.LegContractSettlMonth val = new QuickFix.Fields.LegContractSettlMonth();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegContractSettlMonth val) 
                    { 
                        this.LegContractSettlMonth = val;
                    }
                    
                    public QuickFix.Fields.LegContractSettlMonth Get(QuickFix.Fields.LegContractSettlMonth val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegContractSettlMonth val) 
                    { 
                        return IsSetLegContractSettlMonth();
                    }
                    
                    public bool IsSetLegContractSettlMonth() 
                    { 
                        return IsSetField(Tags.LegContractSettlMonth);
                    }
                    public QuickFix.Fields.LegInterestAccrualDate LegInterestAccrualDate
                    { 
                        get 
                        {
                            QuickFix.Fields.LegInterestAccrualDate val = new QuickFix.Fields.LegInterestAccrualDate();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.LegInterestAccrualDate val) 
                    { 
                        this.LegInterestAccrualDate = val;
                    }
                    
                    public QuickFix.Fields.LegInterestAccrualDate Get(QuickFix.Fields.LegInterestAccrualDate val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.LegInterestAccrualDate val) 
                    { 
                        return IsSetLegInterestAccrualDate();
                    }
                    
                    public bool IsSetLegInterestAccrualDate() 
                    { 
                        return IsSetField(Tags.LegInterestAccrualDate);
                    }
                                    public class NoLegSecurityAltIDGroup : Group
                    {
                        public static int[] fieldOrder = {Tags.LegSecurityAltID, Tags.LegSecurityAltIDSource, 0};
                    
                        public NoLegSecurityAltIDGroup() 
                          :base( Tags.NoLegSecurityAltID, Tags.LegSecurityAltID, fieldOrder)
                        {
                        }
                    
                        public override Group Clone()
                        {
                            var clone = new NoLegSecurityAltIDGroup();
                            clone.CopyStateFrom(this);
                            return clone;
                        }
                    
                                            public QuickFix.Fields.LegSecurityAltID LegSecurityAltID
                        { 
                            get 
                            {
                                QuickFix.Fields.LegSecurityAltID val = new QuickFix.Fields.LegSecurityAltID();
                                GetField(val);
                                return val;
                            }
                            set { SetField(value); }
                        }
                        
                        public void Set(QuickFix.Fields.LegSecurityAltID val) 
                        { 
                            this.LegSecurityAltID = val;
                        }
                        
                        public QuickFix.Fields.LegSecurityAltID Get(QuickFix.Fields.LegSecurityAltID val) 
                        { 
                            GetField(val);
                            return val;
                        }
                        
                        public bool IsSet(QuickFix.Fields.LegSecurityAltID val) 
                        { 
                            return IsSetLegSecurityAltID();
                        }
                        
                        public bool IsSetLegSecurityAltID() 
                        { 
                            return IsSetField(Tags.LegSecurityAltID);
                        }
                        public QuickFix.Fields.LegSecurityAltIDSource LegSecurityAltIDSource
                        { 
                            get 
                            {
                                QuickFix.Fields.LegSecurityAltIDSource val = new QuickFix.Fields.LegSecurityAltIDSource();
                                GetField(val);
                                return val;
                            }
                            set { SetField(value); }
                        }
                        
                        public void Set(QuickFix.Fields.LegSecurityAltIDSource val) 
                        { 
                            this.LegSecurityAltIDSource = val;
                        }
                        
                        public QuickFix.Fields.LegSecurityAltIDSource Get(QuickFix.Fields.LegSecurityAltIDSource val) 
                        { 
                            GetField(val);
                            return val;
                        }
                        
                        public bool IsSet(QuickFix.Fields.LegSecurityAltIDSource val) 
                        { 
                            return IsSetLegSecurityAltIDSource();
                        }
                        
                        public bool IsSetLegSecurityAltIDSource() 
                        { 
                            return IsSetField(Tags.LegSecurityAltIDSource);
                        }
                    
                    }
                }
                public class NoReferentialPricesGroup : Group
                {
                    public static int[] fieldOrder = {Tags.ReferentialPxType, Tags.ReferentialState, Tags.ReferentialPx, Tags.ContextId, Tags.TransactTime, 0};
                
                    public NoReferentialPricesGroup() 
                      :base( Tags.NoReferentialPrices, Tags.ReferentialPxType, fieldOrder)
                    {
                    }
                
                    public override Group Clone()
                    {
                        var clone = new NoReferentialPricesGroup();
                        clone.CopyStateFrom(this);
                        return clone;
                    }
                
                                    public QuickFix.Fields.ReferentialPxType ReferentialPxType
                    { 
                        get 
                        {
                            QuickFix.Fields.ReferentialPxType val = new QuickFix.Fields.ReferentialPxType();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.ReferentialPxType val) 
                    { 
                        this.ReferentialPxType = val;
                    }
                    
                    public QuickFix.Fields.ReferentialPxType Get(QuickFix.Fields.ReferentialPxType val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.ReferentialPxType val) 
                    { 
                        return IsSetReferentialPxType();
                    }
                    
                    public bool IsSetReferentialPxType() 
                    { 
                        return IsSetField(Tags.ReferentialPxType);
                    }
                    public QuickFix.Fields.ReferentialState ReferentialState
                    { 
                        get 
                        {
                            QuickFix.Fields.ReferentialState val = new QuickFix.Fields.ReferentialState();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.ReferentialState val) 
                    { 
                        this.ReferentialState = val;
                    }
                    
                    public QuickFix.Fields.ReferentialState Get(QuickFix.Fields.ReferentialState val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.ReferentialState val) 
                    { 
                        return IsSetReferentialState();
                    }
                    
                    public bool IsSetReferentialState() 
                    { 
                        return IsSetField(Tags.ReferentialState);
                    }
                    public QuickFix.Fields.ReferentialPx ReferentialPx
                    { 
                        get 
                        {
                            QuickFix.Fields.ReferentialPx val = new QuickFix.Fields.ReferentialPx();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.ReferentialPx val) 
                    { 
                        this.ReferentialPx = val;
                    }
                    
                    public QuickFix.Fields.ReferentialPx Get(QuickFix.Fields.ReferentialPx val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.ReferentialPx val) 
                    { 
                        return IsSetReferentialPx();
                    }
                    
                    public bool IsSetReferentialPx() 
                    { 
                        return IsSetField(Tags.ReferentialPx);
                    }
                    public QuickFix.Fields.ContextId ContextId
                    { 
                        get 
                        {
                            QuickFix.Fields.ContextId val = new QuickFix.Fields.ContextId();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.ContextId val) 
                    { 
                        this.ContextId = val;
                    }
                    
                    public QuickFix.Fields.ContextId Get(QuickFix.Fields.ContextId val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.ContextId val) 
                    { 
                        return IsSetContextId();
                    }
                    
                    public bool IsSetContextId() 
                    { 
                        return IsSetField(Tags.ContextId);
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
                
                }
            }
        }
    }
}
