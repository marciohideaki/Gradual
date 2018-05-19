// This is a generated file.  Don't edit it directly!

using QuickFix.Fields;
namespace QuickFix
{
    namespace FIX44 
    {
        public class QuoteRequestReject : Message
        {
            public const string MsgType = "AG";

            public QuoteRequestReject() : base()
            {
                this.Header.SetField(new QuickFix.Fields.MsgType("AG"));
            }

            public QuoteRequestReject(
                    QuickFix.Fields.QuoteReqID aQuoteReqID,
                    QuickFix.Fields.QuoteRequestRejectReason aQuoteRequestRejectReason
                ) : this()
            {
                this.QuoteReqID = aQuoteReqID;
                this.QuoteRequestRejectReason = aQuoteRequestRejectReason;
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
            public QuickFix.Fields.QuoteReqID QuoteReqID
            { 
                get 
                {
                    QuickFix.Fields.QuoteReqID val = new QuickFix.Fields.QuoteReqID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.QuoteReqID val) 
            { 
                this.QuoteReqID = val;
            }
            
            public QuickFix.Fields.QuoteReqID Get(QuickFix.Fields.QuoteReqID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.QuoteReqID val) 
            { 
                return IsSetQuoteReqID();
            }
            
            public bool IsSetQuoteReqID() 
            { 
                return IsSetField(Tags.QuoteReqID);
            }
            public QuickFix.Fields.QuoteID QuoteID
            { 
                get 
                {
                    QuickFix.Fields.QuoteID val = new QuickFix.Fields.QuoteID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.QuoteID val) 
            { 
                this.QuoteID = val;
            }
            
            public QuickFix.Fields.QuoteID Get(QuickFix.Fields.QuoteID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.QuoteID val) 
            { 
                return IsSetQuoteID();
            }
            
            public bool IsSetQuoteID() 
            { 
                return IsSetField(Tags.QuoteID);
            }
            public QuickFix.Fields.PrivateQuote PrivateQuote
            { 
                get 
                {
                    QuickFix.Fields.PrivateQuote val = new QuickFix.Fields.PrivateQuote();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.PrivateQuote val) 
            { 
                this.PrivateQuote = val;
            }
            
            public QuickFix.Fields.PrivateQuote Get(QuickFix.Fields.PrivateQuote val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.PrivateQuote val) 
            { 
                return IsSetPrivateQuote();
            }
            
            public bool IsSetPrivateQuote() 
            { 
                return IsSetField(Tags.PrivateQuote);
            }
            public QuickFix.Fields.RFQReqID RFQReqID
            { 
                get 
                {
                    QuickFix.Fields.RFQReqID val = new QuickFix.Fields.RFQReqID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.RFQReqID val) 
            { 
                this.RFQReqID = val;
            }
            
            public QuickFix.Fields.RFQReqID Get(QuickFix.Fields.RFQReqID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.RFQReqID val) 
            { 
                return IsSetRFQReqID();
            }
            
            public bool IsSetRFQReqID() 
            { 
                return IsSetField(Tags.RFQReqID);
            }
            public QuickFix.Fields.QuoteRequestRejectReason QuoteRequestRejectReason
            { 
                get 
                {
                    QuickFix.Fields.QuoteRequestRejectReason val = new QuickFix.Fields.QuoteRequestRejectReason();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.QuoteRequestRejectReason val) 
            { 
                this.QuoteRequestRejectReason = val;
            }
            
            public QuickFix.Fields.QuoteRequestRejectReason Get(QuickFix.Fields.QuoteRequestRejectReason val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.QuoteRequestRejectReason val) 
            { 
                return IsSetQuoteRequestRejectReason();
            }
            
            public bool IsSetQuoteRequestRejectReason() 
            { 
                return IsSetField(Tags.QuoteRequestRejectReason);
            }
            public QuickFix.Fields.NoRelatedSym NoRelatedSym
            { 
                get 
                {
                    QuickFix.Fields.NoRelatedSym val = new QuickFix.Fields.NoRelatedSym();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoRelatedSym val) 
            { 
                this.NoRelatedSym = val;
            }
            
            public QuickFix.Fields.NoRelatedSym Get(QuickFix.Fields.NoRelatedSym val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoRelatedSym val) 
            { 
                return IsSetNoRelatedSym();
            }
            
            public bool IsSetNoRelatedSym() 
            { 
                return IsSetField(Tags.NoRelatedSym);
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
            public class NoRelatedSymGroup : Group
            {
                public static int[] fieldOrder = {Tags.Symbol, Tags.SecurityID, Tags.SecurityIDSource, Tags.SecurityExchange, Tags.AgreementDesc, Tags.AgreementID, Tags.AgreementDate, Tags.AgreementCurrency, Tags.TerminationType, Tags.StartDate, Tags.EndDate, Tags.DeliveryType, Tags.MarginRatio, Tags.NoUnderlyings, Tags.PrevClosePx, Tags.QuoteRequestType, Tags.QuoteType, Tags.TradingSessionID, Tags.TradingSessionSubID, Tags.TradeOriginationDate, Tags.Side, Tags.QtyType, Tags.OrderQty, Tags.CashOrderQty, Tags.OrderPercent, Tags.RoundingDirection, Tags.RoundingModulus, Tags.SettlType, Tags.DaysToSettlement, Tags.FixedRate, Tags.SettlDate, Tags.SettlDate2, Tags.OrderQty2, Tags.Currency, Tags.NoStipulations, Tags.Account, Tags.AcctIDSource, Tags.AccountType, Tags.NoLegs, Tags.NoQuoteQualifiers, Tags.QuotePriceType, Tags.OrdType, Tags.ExpireTime, Tags.TransactTime, Tags.Spread, Tags.BenchmarkCurveCurrency, Tags.BenchmarkCurveName, Tags.BenchmarkCurvePoint, Tags.BenchmarkPrice, Tags.BenchmarkPriceType, Tags.BenchmarkSecurityID, Tags.BenchmarkSecurityIDSource, Tags.PriceType, Tags.Price, Tags.Price2, Tags.YieldType, Tags.Yield, Tags.YieldCalcDate, Tags.YieldRedemptionDate, Tags.YieldRedemptionPrice, Tags.YieldRedemptionPriceType, Tags.NoPartyIDs, 0};
            
                public NoRelatedSymGroup() 
                  :base( Tags.NoRelatedSym, Tags.Symbol, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoRelatedSymGroup();
                    clone.CopyStateFrom(this);
                    return clone;
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
                public QuickFix.Fields.PrevClosePx PrevClosePx
                { 
                    get 
                    {
                        QuickFix.Fields.PrevClosePx val = new QuickFix.Fields.PrevClosePx();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.PrevClosePx val) 
                { 
                    this.PrevClosePx = val;
                }
                
                public QuickFix.Fields.PrevClosePx Get(QuickFix.Fields.PrevClosePx val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.PrevClosePx val) 
                { 
                    return IsSetPrevClosePx();
                }
                
                public bool IsSetPrevClosePx() 
                { 
                    return IsSetField(Tags.PrevClosePx);
                }
                public QuickFix.Fields.QuoteRequestType QuoteRequestType
                { 
                    get 
                    {
                        QuickFix.Fields.QuoteRequestType val = new QuickFix.Fields.QuoteRequestType();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.QuoteRequestType val) 
                { 
                    this.QuoteRequestType = val;
                }
                
                public QuickFix.Fields.QuoteRequestType Get(QuickFix.Fields.QuoteRequestType val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.QuoteRequestType val) 
                { 
                    return IsSetQuoteRequestType();
                }
                
                public bool IsSetQuoteRequestType() 
                { 
                    return IsSetField(Tags.QuoteRequestType);
                }
                public QuickFix.Fields.QuoteType QuoteType
                { 
                    get 
                    {
                        QuickFix.Fields.QuoteType val = new QuickFix.Fields.QuoteType();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.QuoteType val) 
                { 
                    this.QuoteType = val;
                }
                
                public QuickFix.Fields.QuoteType Get(QuickFix.Fields.QuoteType val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.QuoteType val) 
                { 
                    return IsSetQuoteType();
                }
                
                public bool IsSetQuoteType() 
                { 
                    return IsSetField(Tags.QuoteType);
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
                public QuickFix.Fields.NoQuoteQualifiers NoQuoteQualifiers
                { 
                    get 
                    {
                        QuickFix.Fields.NoQuoteQualifiers val = new QuickFix.Fields.NoQuoteQualifiers();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.NoQuoteQualifiers val) 
                { 
                    this.NoQuoteQualifiers = val;
                }
                
                public QuickFix.Fields.NoQuoteQualifiers Get(QuickFix.Fields.NoQuoteQualifiers val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.NoQuoteQualifiers val) 
                { 
                    return IsSetNoQuoteQualifiers();
                }
                
                public bool IsSetNoQuoteQualifiers() 
                { 
                    return IsSetField(Tags.NoQuoteQualifiers);
                }
                public QuickFix.Fields.QuotePriceType QuotePriceType
                { 
                    get 
                    {
                        QuickFix.Fields.QuotePriceType val = new QuickFix.Fields.QuotePriceType();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.QuotePriceType val) 
                { 
                    this.QuotePriceType = val;
                }
                
                public QuickFix.Fields.QuotePriceType Get(QuickFix.Fields.QuotePriceType val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.QuotePriceType val) 
                { 
                    return IsSetQuotePriceType();
                }
                
                public bool IsSetQuotePriceType() 
                { 
                    return IsSetField(Tags.QuotePriceType);
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
                public QuickFix.Fields.Price2 Price2
                { 
                    get 
                    {
                        QuickFix.Fields.Price2 val = new QuickFix.Fields.Price2();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.Price2 val) 
                { 
                    this.Price2 = val;
                }
                
                public QuickFix.Fields.Price2 Get(QuickFix.Fields.Price2 val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.Price2 val) 
                { 
                    return IsSetPrice2();
                }
                
                public bool IsSetPrice2() 
                { 
                    return IsSetField(Tags.Price2);
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
                public class NoLegsGroup : Group
                {
                    public static int[] fieldOrder = {Tags.LegSymbol, Tags.LegSecurityID, Tags.LegSecurityIDSource, Tags.LegSecurityExchange, 0};
                
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
                
                }
                public class NoQuoteQualifiersGroup : Group
                {
                    public static int[] fieldOrder = {Tags.QuoteQualifier, 0};
                
                    public NoQuoteQualifiersGroup() 
                      :base( Tags.NoQuoteQualifiers, Tags.QuoteQualifier, fieldOrder)
                    {
                    }
                
                    public override Group Clone()
                    {
                        var clone = new NoQuoteQualifiersGroup();
                        clone.CopyStateFrom(this);
                        return clone;
                    }
                
                                    public QuickFix.Fields.QuoteQualifier QuoteQualifier
                    { 
                        get 
                        {
                            QuickFix.Fields.QuoteQualifier val = new QuickFix.Fields.QuoteQualifier();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.QuoteQualifier val) 
                    { 
                        this.QuoteQualifier = val;
                    }
                    
                    public QuickFix.Fields.QuoteQualifier Get(QuickFix.Fields.QuoteQualifier val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.QuoteQualifier val) 
                    { 
                        return IsSetQuoteQualifier();
                    }
                    
                    public bool IsSetQuoteQualifier() 
                    { 
                        return IsSetField(Tags.QuoteQualifier);
                    }
                
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
            }
        }
    }
}
