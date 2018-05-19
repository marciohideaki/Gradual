// This is a generated file.  Don't edit it directly!

using QuickFix.Fields;
namespace QuickFix
{
    namespace FIX44 
    {
        public class AllocationInstruction : Message
        {
            public const string MsgType = "J";

            public AllocationInstruction() : base()
            {
                this.Header.SetField(new QuickFix.Fields.MsgType("J"));
            }

            public AllocationInstruction(
                    QuickFix.Fields.AllocID aAllocID,
                    QuickFix.Fields.AllocTransType aAllocTransType,
                    QuickFix.Fields.AllocType aAllocType,
                    QuickFix.Fields.AllocNoOrdersType aAllocNoOrdersType,
                    QuickFix.Fields.Side aSide,
                    QuickFix.Fields.Symbol aSymbol,
                    QuickFix.Fields.SecurityID aSecurityID,
                    QuickFix.Fields.SecurityIDSource aSecurityIDSource,
                    QuickFix.Fields.Quantity aQuantity,
                    QuickFix.Fields.AvgPx aAvgPx,
                    QuickFix.Fields.TradeDate aTradeDate
                ) : this()
            {
                this.AllocID = aAllocID;
                this.AllocTransType = aAllocTransType;
                this.AllocType = aAllocType;
                this.AllocNoOrdersType = aAllocNoOrdersType;
                this.Side = aSide;
                this.Symbol = aSymbol;
                this.SecurityID = aSecurityID;
                this.SecurityIDSource = aSecurityIDSource;
                this.Quantity = aQuantity;
                this.AvgPx = aAvgPx;
                this.TradeDate = aTradeDate;
            }

            public QuickFix.Fields.AllocID AllocID
            { 
                get 
                {
                    QuickFix.Fields.AllocID val = new QuickFix.Fields.AllocID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AllocID val) 
            { 
                this.AllocID = val;
            }
            
            public QuickFix.Fields.AllocID Get(QuickFix.Fields.AllocID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AllocID val) 
            { 
                return IsSetAllocID();
            }
            
            public bool IsSetAllocID() 
            { 
                return IsSetField(Tags.AllocID);
            }
            public QuickFix.Fields.AllocTransType AllocTransType
            { 
                get 
                {
                    QuickFix.Fields.AllocTransType val = new QuickFix.Fields.AllocTransType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AllocTransType val) 
            { 
                this.AllocTransType = val;
            }
            
            public QuickFix.Fields.AllocTransType Get(QuickFix.Fields.AllocTransType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AllocTransType val) 
            { 
                return IsSetAllocTransType();
            }
            
            public bool IsSetAllocTransType() 
            { 
                return IsSetField(Tags.AllocTransType);
            }
            public QuickFix.Fields.AllocType AllocType
            { 
                get 
                {
                    QuickFix.Fields.AllocType val = new QuickFix.Fields.AllocType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AllocType val) 
            { 
                this.AllocType = val;
            }
            
            public QuickFix.Fields.AllocType Get(QuickFix.Fields.AllocType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AllocType val) 
            { 
                return IsSetAllocType();
            }
            
            public bool IsSetAllocType() 
            { 
                return IsSetField(Tags.AllocType);
            }
            public QuickFix.Fields.SecondaryAllocID SecondaryAllocID
            { 
                get 
                {
                    QuickFix.Fields.SecondaryAllocID val = new QuickFix.Fields.SecondaryAllocID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.SecondaryAllocID val) 
            { 
                this.SecondaryAllocID = val;
            }
            
            public QuickFix.Fields.SecondaryAllocID Get(QuickFix.Fields.SecondaryAllocID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.SecondaryAllocID val) 
            { 
                return IsSetSecondaryAllocID();
            }
            
            public bool IsSetSecondaryAllocID() 
            { 
                return IsSetField(Tags.SecondaryAllocID);
            }
            public QuickFix.Fields.RefAllocID RefAllocID
            { 
                get 
                {
                    QuickFix.Fields.RefAllocID val = new QuickFix.Fields.RefAllocID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.RefAllocID val) 
            { 
                this.RefAllocID = val;
            }
            
            public QuickFix.Fields.RefAllocID Get(QuickFix.Fields.RefAllocID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.RefAllocID val) 
            { 
                return IsSetRefAllocID();
            }
            
            public bool IsSetRefAllocID() 
            { 
                return IsSetField(Tags.RefAllocID);
            }
            public QuickFix.Fields.AllocCancReplaceReason AllocCancReplaceReason
            { 
                get 
                {
                    QuickFix.Fields.AllocCancReplaceReason val = new QuickFix.Fields.AllocCancReplaceReason();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AllocCancReplaceReason val) 
            { 
                this.AllocCancReplaceReason = val;
            }
            
            public QuickFix.Fields.AllocCancReplaceReason Get(QuickFix.Fields.AllocCancReplaceReason val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AllocCancReplaceReason val) 
            { 
                return IsSetAllocCancReplaceReason();
            }
            
            public bool IsSetAllocCancReplaceReason() 
            { 
                return IsSetField(Tags.AllocCancReplaceReason);
            }
            public QuickFix.Fields.AllocIntermedReqType AllocIntermedReqType
            { 
                get 
                {
                    QuickFix.Fields.AllocIntermedReqType val = new QuickFix.Fields.AllocIntermedReqType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AllocIntermedReqType val) 
            { 
                this.AllocIntermedReqType = val;
            }
            
            public QuickFix.Fields.AllocIntermedReqType Get(QuickFix.Fields.AllocIntermedReqType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AllocIntermedReqType val) 
            { 
                return IsSetAllocIntermedReqType();
            }
            
            public bool IsSetAllocIntermedReqType() 
            { 
                return IsSetField(Tags.AllocIntermedReqType);
            }
            public QuickFix.Fields.AllocLinkID AllocLinkID
            { 
                get 
                {
                    QuickFix.Fields.AllocLinkID val = new QuickFix.Fields.AllocLinkID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AllocLinkID val) 
            { 
                this.AllocLinkID = val;
            }
            
            public QuickFix.Fields.AllocLinkID Get(QuickFix.Fields.AllocLinkID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AllocLinkID val) 
            { 
                return IsSetAllocLinkID();
            }
            
            public bool IsSetAllocLinkID() 
            { 
                return IsSetField(Tags.AllocLinkID);
            }
            public QuickFix.Fields.AllocLinkType AllocLinkType
            { 
                get 
                {
                    QuickFix.Fields.AllocLinkType val = new QuickFix.Fields.AllocLinkType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AllocLinkType val) 
            { 
                this.AllocLinkType = val;
            }
            
            public QuickFix.Fields.AllocLinkType Get(QuickFix.Fields.AllocLinkType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AllocLinkType val) 
            { 
                return IsSetAllocLinkType();
            }
            
            public bool IsSetAllocLinkType() 
            { 
                return IsSetField(Tags.AllocLinkType);
            }
            public QuickFix.Fields.BookingRefID BookingRefID
            { 
                get 
                {
                    QuickFix.Fields.BookingRefID val = new QuickFix.Fields.BookingRefID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.BookingRefID val) 
            { 
                this.BookingRefID = val;
            }
            
            public QuickFix.Fields.BookingRefID Get(QuickFix.Fields.BookingRefID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.BookingRefID val) 
            { 
                return IsSetBookingRefID();
            }
            
            public bool IsSetBookingRefID() 
            { 
                return IsSetField(Tags.BookingRefID);
            }
            public QuickFix.Fields.AllocNoOrdersType AllocNoOrdersType
            { 
                get 
                {
                    QuickFix.Fields.AllocNoOrdersType val = new QuickFix.Fields.AllocNoOrdersType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AllocNoOrdersType val) 
            { 
                this.AllocNoOrdersType = val;
            }
            
            public QuickFix.Fields.AllocNoOrdersType Get(QuickFix.Fields.AllocNoOrdersType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AllocNoOrdersType val) 
            { 
                return IsSetAllocNoOrdersType();
            }
            
            public bool IsSetAllocNoOrdersType() 
            { 
                return IsSetField(Tags.AllocNoOrdersType);
            }
            public QuickFix.Fields.NoOrders NoOrders
            { 
                get 
                {
                    QuickFix.Fields.NoOrders val = new QuickFix.Fields.NoOrders();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoOrders val) 
            { 
                this.NoOrders = val;
            }
            
            public QuickFix.Fields.NoOrders Get(QuickFix.Fields.NoOrders val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoOrders val) 
            { 
                return IsSetNoOrders();
            }
            
            public bool IsSetNoOrders() 
            { 
                return IsSetField(Tags.NoOrders);
            }
            public QuickFix.Fields.PreviouslyReported PreviouslyReported
            { 
                get 
                {
                    QuickFix.Fields.PreviouslyReported val = new QuickFix.Fields.PreviouslyReported();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.PreviouslyReported val) 
            { 
                this.PreviouslyReported = val;
            }
            
            public QuickFix.Fields.PreviouslyReported Get(QuickFix.Fields.PreviouslyReported val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.PreviouslyReported val) 
            { 
                return IsSetPreviouslyReported();
            }
            
            public bool IsSetPreviouslyReported() 
            { 
                return IsSetField(Tags.PreviouslyReported);
            }
            public QuickFix.Fields.ReversalIndicator ReversalIndicator
            { 
                get 
                {
                    QuickFix.Fields.ReversalIndicator val = new QuickFix.Fields.ReversalIndicator();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ReversalIndicator val) 
            { 
                this.ReversalIndicator = val;
            }
            
            public QuickFix.Fields.ReversalIndicator Get(QuickFix.Fields.ReversalIndicator val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ReversalIndicator val) 
            { 
                return IsSetReversalIndicator();
            }
            
            public bool IsSetReversalIndicator() 
            { 
                return IsSetField(Tags.ReversalIndicator);
            }
            public QuickFix.Fields.MatchType MatchType
            { 
                get 
                {
                    QuickFix.Fields.MatchType val = new QuickFix.Fields.MatchType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.MatchType val) 
            { 
                this.MatchType = val;
            }
            
            public QuickFix.Fields.MatchType Get(QuickFix.Fields.MatchType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.MatchType val) 
            { 
                return IsSetMatchType();
            }
            
            public bool IsSetMatchType() 
            { 
                return IsSetField(Tags.MatchType);
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
            public QuickFix.Fields.NoExecs NoExecs
            { 
                get 
                {
                    QuickFix.Fields.NoExecs val = new QuickFix.Fields.NoExecs();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoExecs val) 
            { 
                this.NoExecs = val;
            }
            
            public QuickFix.Fields.NoExecs Get(QuickFix.Fields.NoExecs val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoExecs val) 
            { 
                return IsSetNoExecs();
            }
            
            public bool IsSetNoExecs() 
            { 
                return IsSetField(Tags.NoExecs);
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
            public QuickFix.Fields.DeliveryForm DeliveryForm
            { 
                get 
                {
                    QuickFix.Fields.DeliveryForm val = new QuickFix.Fields.DeliveryForm();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.DeliveryForm val) 
            { 
                this.DeliveryForm = val;
            }
            
            public QuickFix.Fields.DeliveryForm Get(QuickFix.Fields.DeliveryForm val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.DeliveryForm val) 
            { 
                return IsSetDeliveryForm();
            }
            
            public bool IsSetDeliveryForm() 
            { 
                return IsSetField(Tags.DeliveryForm);
            }
            public QuickFix.Fields.PctAtRisk PctAtRisk
            { 
                get 
                {
                    QuickFix.Fields.PctAtRisk val = new QuickFix.Fields.PctAtRisk();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.PctAtRisk val) 
            { 
                this.PctAtRisk = val;
            }
            
            public QuickFix.Fields.PctAtRisk Get(QuickFix.Fields.PctAtRisk val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.PctAtRisk val) 
            { 
                return IsSetPctAtRisk();
            }
            
            public bool IsSetPctAtRisk() 
            { 
                return IsSetField(Tags.PctAtRisk);
            }
            public QuickFix.Fields.NoInstrAttrib NoInstrAttrib
            { 
                get 
                {
                    QuickFix.Fields.NoInstrAttrib val = new QuickFix.Fields.NoInstrAttrib();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoInstrAttrib val) 
            { 
                this.NoInstrAttrib = val;
            }
            
            public QuickFix.Fields.NoInstrAttrib Get(QuickFix.Fields.NoInstrAttrib val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoInstrAttrib val) 
            { 
                return IsSetNoInstrAttrib();
            }
            
            public bool IsSetNoInstrAttrib() 
            { 
                return IsSetField(Tags.NoInstrAttrib);
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
            public QuickFix.Fields.Quantity Quantity
            { 
                get 
                {
                    QuickFix.Fields.Quantity val = new QuickFix.Fields.Quantity();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.Quantity val) 
            { 
                this.Quantity = val;
            }
            
            public QuickFix.Fields.Quantity Get(QuickFix.Fields.Quantity val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.Quantity val) 
            { 
                return IsSetQuantity();
            }
            
            public bool IsSetQuantity() 
            { 
                return IsSetField(Tags.Quantity);
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
            public QuickFix.Fields.AvgParPx AvgParPx
            { 
                get 
                {
                    QuickFix.Fields.AvgParPx val = new QuickFix.Fields.AvgParPx();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AvgParPx val) 
            { 
                this.AvgParPx = val;
            }
            
            public QuickFix.Fields.AvgParPx Get(QuickFix.Fields.AvgParPx val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AvgParPx val) 
            { 
                return IsSetAvgParPx();
            }
            
            public bool IsSetAvgParPx() 
            { 
                return IsSetField(Tags.AvgParPx);
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
            public QuickFix.Fields.AvgPxPrecision AvgPxPrecision
            { 
                get 
                {
                    QuickFix.Fields.AvgPxPrecision val = new QuickFix.Fields.AvgPxPrecision();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AvgPxPrecision val) 
            { 
                this.AvgPxPrecision = val;
            }
            
            public QuickFix.Fields.AvgPxPrecision Get(QuickFix.Fields.AvgPxPrecision val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AvgPxPrecision val) 
            { 
                return IsSetAvgPxPrecision();
            }
            
            public bool IsSetAvgPxPrecision() 
            { 
                return IsSetField(Tags.AvgPxPrecision);
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
            public QuickFix.Fields.AutoAcceptIndicator AutoAcceptIndicator
            { 
                get 
                {
                    QuickFix.Fields.AutoAcceptIndicator val = new QuickFix.Fields.AutoAcceptIndicator();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.AutoAcceptIndicator val) 
            { 
                this.AutoAcceptIndicator = val;
            }
            
            public QuickFix.Fields.AutoAcceptIndicator Get(QuickFix.Fields.AutoAcceptIndicator val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.AutoAcceptIndicator val) 
            { 
                return IsSetAutoAcceptIndicator();
            }
            
            public bool IsSetAutoAcceptIndicator() 
            { 
                return IsSetField(Tags.AutoAcceptIndicator);
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
            public QuickFix.Fields.TotalAccruedInterestAmt TotalAccruedInterestAmt
            { 
                get 
                {
                    QuickFix.Fields.TotalAccruedInterestAmt val = new QuickFix.Fields.TotalAccruedInterestAmt();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TotalAccruedInterestAmt val) 
            { 
                this.TotalAccruedInterestAmt = val;
            }
            
            public QuickFix.Fields.TotalAccruedInterestAmt Get(QuickFix.Fields.TotalAccruedInterestAmt val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TotalAccruedInterestAmt val) 
            { 
                return IsSetTotalAccruedInterestAmt();
            }
            
            public bool IsSetTotalAccruedInterestAmt() 
            { 
                return IsSetField(Tags.TotalAccruedInterestAmt);
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
            public QuickFix.Fields.LegalConfirm LegalConfirm
            { 
                get 
                {
                    QuickFix.Fields.LegalConfirm val = new QuickFix.Fields.LegalConfirm();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.LegalConfirm val) 
            { 
                this.LegalConfirm = val;
            }
            
            public QuickFix.Fields.LegalConfirm Get(QuickFix.Fields.LegalConfirm val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.LegalConfirm val) 
            { 
                return IsSetLegalConfirm();
            }
            
            public bool IsSetLegalConfirm() 
            { 
                return IsSetField(Tags.LegalConfirm);
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
            public QuickFix.Fields.TotNoAllocs TotNoAllocs
            { 
                get 
                {
                    QuickFix.Fields.TotNoAllocs val = new QuickFix.Fields.TotNoAllocs();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TotNoAllocs val) 
            { 
                this.TotNoAllocs = val;
            }
            
            public QuickFix.Fields.TotNoAllocs Get(QuickFix.Fields.TotNoAllocs val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TotNoAllocs val) 
            { 
                return IsSetTotNoAllocs();
            }
            
            public bool IsSetTotNoAllocs() 
            { 
                return IsSetField(Tags.TotNoAllocs);
            }
            public QuickFix.Fields.LastFragment LastFragment
            { 
                get 
                {
                    QuickFix.Fields.LastFragment val = new QuickFix.Fields.LastFragment();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.LastFragment val) 
            { 
                this.LastFragment = val;
            }
            
            public QuickFix.Fields.LastFragment Get(QuickFix.Fields.LastFragment val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.LastFragment val) 
            { 
                return IsSetLastFragment();
            }
            
            public bool IsSetLastFragment() 
            { 
                return IsSetField(Tags.LastFragment);
            }
            public QuickFix.Fields.NoAllocs NoAllocs
            { 
                get 
                {
                    QuickFix.Fields.NoAllocs val = new QuickFix.Fields.NoAllocs();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoAllocs val) 
            { 
                this.NoAllocs = val;
            }
            
            public QuickFix.Fields.NoAllocs Get(QuickFix.Fields.NoAllocs val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoAllocs val) 
            { 
                return IsSetNoAllocs();
            }
            
            public bool IsSetNoAllocs() 
            { 
                return IsSetField(Tags.NoAllocs);
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
            public class NoOrdersGroup : Group
            {
                public static int[] fieldOrder = {Tags.ClOrdID, Tags.OrderID, Tags.SecondaryOrderID, Tags.SecondaryClOrdID, Tags.ListID, Tags.NoNested2PartyIDs, Tags.OrderQty, Tags.OrderAvgPx, Tags.OrderBookingQty, 0};
            
                public NoOrdersGroup() 
                  :base( Tags.NoOrders, Tags.ClOrdID, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoOrdersGroup();
                    clone.CopyStateFrom(this);
                    return clone;
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
                public QuickFix.Fields.NoNested2PartyIDs NoNested2PartyIDs
                { 
                    get 
                    {
                        QuickFix.Fields.NoNested2PartyIDs val = new QuickFix.Fields.NoNested2PartyIDs();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.NoNested2PartyIDs val) 
                { 
                    this.NoNested2PartyIDs = val;
                }
                
                public QuickFix.Fields.NoNested2PartyIDs Get(QuickFix.Fields.NoNested2PartyIDs val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.NoNested2PartyIDs val) 
                { 
                    return IsSetNoNested2PartyIDs();
                }
                
                public bool IsSetNoNested2PartyIDs() 
                { 
                    return IsSetField(Tags.NoNested2PartyIDs);
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
                public QuickFix.Fields.OrderAvgPx OrderAvgPx
                { 
                    get 
                    {
                        QuickFix.Fields.OrderAvgPx val = new QuickFix.Fields.OrderAvgPx();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.OrderAvgPx val) 
                { 
                    this.OrderAvgPx = val;
                }
                
                public QuickFix.Fields.OrderAvgPx Get(QuickFix.Fields.OrderAvgPx val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.OrderAvgPx val) 
                { 
                    return IsSetOrderAvgPx();
                }
                
                public bool IsSetOrderAvgPx() 
                { 
                    return IsSetField(Tags.OrderAvgPx);
                }
                public QuickFix.Fields.OrderBookingQty OrderBookingQty
                { 
                    get 
                    {
                        QuickFix.Fields.OrderBookingQty val = new QuickFix.Fields.OrderBookingQty();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.OrderBookingQty val) 
                { 
                    this.OrderBookingQty = val;
                }
                
                public QuickFix.Fields.OrderBookingQty Get(QuickFix.Fields.OrderBookingQty val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.OrderBookingQty val) 
                { 
                    return IsSetOrderBookingQty();
                }
                
                public bool IsSetOrderBookingQty() 
                { 
                    return IsSetField(Tags.OrderBookingQty);
                }
                            public class NoNested2PartyIDsGroup : Group
                {
                    public static int[] fieldOrder = {Tags.Nested2PartyID, Tags.Nested2PartyIDSource, Tags.Nested2PartyRole, Tags.NoNested2PartySubIDs, 0};
                
                    public NoNested2PartyIDsGroup() 
                      :base( Tags.NoNested2PartyIDs, Tags.Nested2PartyID, fieldOrder)
                    {
                    }
                
                    public override Group Clone()
                    {
                        var clone = new NoNested2PartyIDsGroup();
                        clone.CopyStateFrom(this);
                        return clone;
                    }
                
                                    public QuickFix.Fields.Nested2PartyID Nested2PartyID
                    { 
                        get 
                        {
                            QuickFix.Fields.Nested2PartyID val = new QuickFix.Fields.Nested2PartyID();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.Nested2PartyID val) 
                    { 
                        this.Nested2PartyID = val;
                    }
                    
                    public QuickFix.Fields.Nested2PartyID Get(QuickFix.Fields.Nested2PartyID val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.Nested2PartyID val) 
                    { 
                        return IsSetNested2PartyID();
                    }
                    
                    public bool IsSetNested2PartyID() 
                    { 
                        return IsSetField(Tags.Nested2PartyID);
                    }
                    public QuickFix.Fields.Nested2PartyIDSource Nested2PartyIDSource
                    { 
                        get 
                        {
                            QuickFix.Fields.Nested2PartyIDSource val = new QuickFix.Fields.Nested2PartyIDSource();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.Nested2PartyIDSource val) 
                    { 
                        this.Nested2PartyIDSource = val;
                    }
                    
                    public QuickFix.Fields.Nested2PartyIDSource Get(QuickFix.Fields.Nested2PartyIDSource val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.Nested2PartyIDSource val) 
                    { 
                        return IsSetNested2PartyIDSource();
                    }
                    
                    public bool IsSetNested2PartyIDSource() 
                    { 
                        return IsSetField(Tags.Nested2PartyIDSource);
                    }
                    public QuickFix.Fields.Nested2PartyRole Nested2PartyRole
                    { 
                        get 
                        {
                            QuickFix.Fields.Nested2PartyRole val = new QuickFix.Fields.Nested2PartyRole();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.Nested2PartyRole val) 
                    { 
                        this.Nested2PartyRole = val;
                    }
                    
                    public QuickFix.Fields.Nested2PartyRole Get(QuickFix.Fields.Nested2PartyRole val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.Nested2PartyRole val) 
                    { 
                        return IsSetNested2PartyRole();
                    }
                    
                    public bool IsSetNested2PartyRole() 
                    { 
                        return IsSetField(Tags.Nested2PartyRole);
                    }
                    public QuickFix.Fields.NoNested2PartySubIDs NoNested2PartySubIDs
                    { 
                        get 
                        {
                            QuickFix.Fields.NoNested2PartySubIDs val = new QuickFix.Fields.NoNested2PartySubIDs();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.NoNested2PartySubIDs val) 
                    { 
                        this.NoNested2PartySubIDs = val;
                    }
                    
                    public QuickFix.Fields.NoNested2PartySubIDs Get(QuickFix.Fields.NoNested2PartySubIDs val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.NoNested2PartySubIDs val) 
                    { 
                        return IsSetNoNested2PartySubIDs();
                    }
                    
                    public bool IsSetNoNested2PartySubIDs() 
                    { 
                        return IsSetField(Tags.NoNested2PartySubIDs);
                    }
                                    public class NoNested2PartySubIDsGroup : Group
                    {
                        public static int[] fieldOrder = {Tags.Nested2PartySubID, Tags.Nested2PartySubIDType, 0};
                    
                        public NoNested2PartySubIDsGroup() 
                          :base( Tags.NoNested2PartySubIDs, Tags.Nested2PartySubID, fieldOrder)
                        {
                        }
                    
                        public override Group Clone()
                        {
                            var clone = new NoNested2PartySubIDsGroup();
                            clone.CopyStateFrom(this);
                            return clone;
                        }
                    
                                            public QuickFix.Fields.Nested2PartySubID Nested2PartySubID
                        { 
                            get 
                            {
                                QuickFix.Fields.Nested2PartySubID val = new QuickFix.Fields.Nested2PartySubID();
                                GetField(val);
                                return val;
                            }
                            set { SetField(value); }
                        }
                        
                        public void Set(QuickFix.Fields.Nested2PartySubID val) 
                        { 
                            this.Nested2PartySubID = val;
                        }
                        
                        public QuickFix.Fields.Nested2PartySubID Get(QuickFix.Fields.Nested2PartySubID val) 
                        { 
                            GetField(val);
                            return val;
                        }
                        
                        public bool IsSet(QuickFix.Fields.Nested2PartySubID val) 
                        { 
                            return IsSetNested2PartySubID();
                        }
                        
                        public bool IsSetNested2PartySubID() 
                        { 
                            return IsSetField(Tags.Nested2PartySubID);
                        }
                        public QuickFix.Fields.Nested2PartySubIDType Nested2PartySubIDType
                        { 
                            get 
                            {
                                QuickFix.Fields.Nested2PartySubIDType val = new QuickFix.Fields.Nested2PartySubIDType();
                                GetField(val);
                                return val;
                            }
                            set { SetField(value); }
                        }
                        
                        public void Set(QuickFix.Fields.Nested2PartySubIDType val) 
                        { 
                            this.Nested2PartySubIDType = val;
                        }
                        
                        public QuickFix.Fields.Nested2PartySubIDType Get(QuickFix.Fields.Nested2PartySubIDType val) 
                        { 
                            GetField(val);
                            return val;
                        }
                        
                        public bool IsSet(QuickFix.Fields.Nested2PartySubIDType val) 
                        { 
                            return IsSetNested2PartySubIDType();
                        }
                        
                        public bool IsSetNested2PartySubIDType() 
                        { 
                            return IsSetField(Tags.Nested2PartySubIDType);
                        }
                    
                    }
                }
            }
            public class NoExecsGroup : Group
            {
                public static int[] fieldOrder = {Tags.TradeID, Tags.LastQty, Tags.ExecID, Tags.SecondaryExecID, Tags.LastPx, Tags.LastParPx, Tags.LastCapacity, 0};
            
                public NoExecsGroup() 
                  :base( Tags.NoExecs, Tags.TradeID, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoExecsGroup();
                    clone.CopyStateFrom(this);
                    return clone;
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
            
            }
            public class NoInstrAttribGroup : Group
            {
                public static int[] fieldOrder = {Tags.InstrAttribType, Tags.InstrAttribValue, 0};
            
                public NoInstrAttribGroup() 
                  :base( Tags.NoInstrAttrib, Tags.InstrAttribType, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoInstrAttribGroup();
                    clone.CopyStateFrom(this);
                    return clone;
                }
            
                            public QuickFix.Fields.InstrAttribType InstrAttribType
                { 
                    get 
                    {
                        QuickFix.Fields.InstrAttribType val = new QuickFix.Fields.InstrAttribType();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.InstrAttribType val) 
                { 
                    this.InstrAttribType = val;
                }
                
                public QuickFix.Fields.InstrAttribType Get(QuickFix.Fields.InstrAttribType val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.InstrAttribType val) 
                { 
                    return IsSetInstrAttribType();
                }
                
                public bool IsSetInstrAttribType() 
                { 
                    return IsSetField(Tags.InstrAttribType);
                }
                public QuickFix.Fields.InstrAttribValue InstrAttribValue
                { 
                    get 
                    {
                        QuickFix.Fields.InstrAttribValue val = new QuickFix.Fields.InstrAttribValue();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.InstrAttribValue val) 
                { 
                    this.InstrAttribValue = val;
                }
                
                public QuickFix.Fields.InstrAttribValue Get(QuickFix.Fields.InstrAttribValue val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.InstrAttribValue val) 
                { 
                    return IsSetInstrAttribValue();
                }
                
                public bool IsSetInstrAttribValue() 
                { 
                    return IsSetField(Tags.InstrAttribValue);
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
            public class NoAllocsGroup : Group
            {
                public static int[] fieldOrder = {Tags.AllocAccount, Tags.AllocAcctIDSource, Tags.MatchStatus, Tags.AllocPrice, Tags.AllocQty, Tags.IndividualAllocID, Tags.ProcessCode, Tags.NoNestedPartyIDs, Tags.NotifyBrokerOfCredit, Tags.AllocHandlInst, Tags.AllocText, Tags.EncodedAllocTextLen, Tags.EncodedAllocText, Tags.Commission, Tags.CommType, Tags.CommCurrency, Tags.FundRenewWaiv, Tags.AllocAvgPx, Tags.AllocNetMoney, Tags.SettlCurrAmt, Tags.AllocSettlCurrAmt, Tags.SettlCurrency, Tags.AllocSettlCurrency, Tags.SettlCurrFxRate, Tags.SettlCurrFxRateCalc, Tags.AllocAccruedInterestAmt, Tags.AllocInterestAtMaturity, Tags.NoMiscFees, Tags.NoClearingInstructions, Tags.ClearingFeeIndicator, Tags.AllocSettlInstType, Tags.SettlDeliveryType, Tags.StandInstDbType, Tags.StandInstDbName, Tags.StandInstDbID, 0};
            
                public NoAllocsGroup() 
                  :base( Tags.NoAllocs, Tags.AllocAccount, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoAllocsGroup();
                    clone.CopyStateFrom(this);
                    return clone;
                }
            
                            public QuickFix.Fields.AllocAccount AllocAccount
                { 
                    get 
                    {
                        QuickFix.Fields.AllocAccount val = new QuickFix.Fields.AllocAccount();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.AllocAccount val) 
                { 
                    this.AllocAccount = val;
                }
                
                public QuickFix.Fields.AllocAccount Get(QuickFix.Fields.AllocAccount val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.AllocAccount val) 
                { 
                    return IsSetAllocAccount();
                }
                
                public bool IsSetAllocAccount() 
                { 
                    return IsSetField(Tags.AllocAccount);
                }
                public QuickFix.Fields.AllocAcctIDSource AllocAcctIDSource
                { 
                    get 
                    {
                        QuickFix.Fields.AllocAcctIDSource val = new QuickFix.Fields.AllocAcctIDSource();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.AllocAcctIDSource val) 
                { 
                    this.AllocAcctIDSource = val;
                }
                
                public QuickFix.Fields.AllocAcctIDSource Get(QuickFix.Fields.AllocAcctIDSource val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.AllocAcctIDSource val) 
                { 
                    return IsSetAllocAcctIDSource();
                }
                
                public bool IsSetAllocAcctIDSource() 
                { 
                    return IsSetField(Tags.AllocAcctIDSource);
                }
                public QuickFix.Fields.MatchStatus MatchStatus
                { 
                    get 
                    {
                        QuickFix.Fields.MatchStatus val = new QuickFix.Fields.MatchStatus();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.MatchStatus val) 
                { 
                    this.MatchStatus = val;
                }
                
                public QuickFix.Fields.MatchStatus Get(QuickFix.Fields.MatchStatus val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.MatchStatus val) 
                { 
                    return IsSetMatchStatus();
                }
                
                public bool IsSetMatchStatus() 
                { 
                    return IsSetField(Tags.MatchStatus);
                }
                public QuickFix.Fields.AllocPrice AllocPrice
                { 
                    get 
                    {
                        QuickFix.Fields.AllocPrice val = new QuickFix.Fields.AllocPrice();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.AllocPrice val) 
                { 
                    this.AllocPrice = val;
                }
                
                public QuickFix.Fields.AllocPrice Get(QuickFix.Fields.AllocPrice val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.AllocPrice val) 
                { 
                    return IsSetAllocPrice();
                }
                
                public bool IsSetAllocPrice() 
                { 
                    return IsSetField(Tags.AllocPrice);
                }
                public QuickFix.Fields.AllocQty AllocQty
                { 
                    get 
                    {
                        QuickFix.Fields.AllocQty val = new QuickFix.Fields.AllocQty();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.AllocQty val) 
                { 
                    this.AllocQty = val;
                }
                
                public QuickFix.Fields.AllocQty Get(QuickFix.Fields.AllocQty val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.AllocQty val) 
                { 
                    return IsSetAllocQty();
                }
                
                public bool IsSetAllocQty() 
                { 
                    return IsSetField(Tags.AllocQty);
                }
                public QuickFix.Fields.IndividualAllocID IndividualAllocID
                { 
                    get 
                    {
                        QuickFix.Fields.IndividualAllocID val = new QuickFix.Fields.IndividualAllocID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.IndividualAllocID val) 
                { 
                    this.IndividualAllocID = val;
                }
                
                public QuickFix.Fields.IndividualAllocID Get(QuickFix.Fields.IndividualAllocID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.IndividualAllocID val) 
                { 
                    return IsSetIndividualAllocID();
                }
                
                public bool IsSetIndividualAllocID() 
                { 
                    return IsSetField(Tags.IndividualAllocID);
                }
                public QuickFix.Fields.ProcessCode ProcessCode
                { 
                    get 
                    {
                        QuickFix.Fields.ProcessCode val = new QuickFix.Fields.ProcessCode();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.ProcessCode val) 
                { 
                    this.ProcessCode = val;
                }
                
                public QuickFix.Fields.ProcessCode Get(QuickFix.Fields.ProcessCode val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.ProcessCode val) 
                { 
                    return IsSetProcessCode();
                }
                
                public bool IsSetProcessCode() 
                { 
                    return IsSetField(Tags.ProcessCode);
                }
                public QuickFix.Fields.NoNestedPartyIDs NoNestedPartyIDs
                { 
                    get 
                    {
                        QuickFix.Fields.NoNestedPartyIDs val = new QuickFix.Fields.NoNestedPartyIDs();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.NoNestedPartyIDs val) 
                { 
                    this.NoNestedPartyIDs = val;
                }
                
                public QuickFix.Fields.NoNestedPartyIDs Get(QuickFix.Fields.NoNestedPartyIDs val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.NoNestedPartyIDs val) 
                { 
                    return IsSetNoNestedPartyIDs();
                }
                
                public bool IsSetNoNestedPartyIDs() 
                { 
                    return IsSetField(Tags.NoNestedPartyIDs);
                }
                public QuickFix.Fields.NotifyBrokerOfCredit NotifyBrokerOfCredit
                { 
                    get 
                    {
                        QuickFix.Fields.NotifyBrokerOfCredit val = new QuickFix.Fields.NotifyBrokerOfCredit();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.NotifyBrokerOfCredit val) 
                { 
                    this.NotifyBrokerOfCredit = val;
                }
                
                public QuickFix.Fields.NotifyBrokerOfCredit Get(QuickFix.Fields.NotifyBrokerOfCredit val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.NotifyBrokerOfCredit val) 
                { 
                    return IsSetNotifyBrokerOfCredit();
                }
                
                public bool IsSetNotifyBrokerOfCredit() 
                { 
                    return IsSetField(Tags.NotifyBrokerOfCredit);
                }
                public QuickFix.Fields.AllocHandlInst AllocHandlInst
                { 
                    get 
                    {
                        QuickFix.Fields.AllocHandlInst val = new QuickFix.Fields.AllocHandlInst();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.AllocHandlInst val) 
                { 
                    this.AllocHandlInst = val;
                }
                
                public QuickFix.Fields.AllocHandlInst Get(QuickFix.Fields.AllocHandlInst val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.AllocHandlInst val) 
                { 
                    return IsSetAllocHandlInst();
                }
                
                public bool IsSetAllocHandlInst() 
                { 
                    return IsSetField(Tags.AllocHandlInst);
                }
                public QuickFix.Fields.AllocText AllocText
                { 
                    get 
                    {
                        QuickFix.Fields.AllocText val = new QuickFix.Fields.AllocText();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.AllocText val) 
                { 
                    this.AllocText = val;
                }
                
                public QuickFix.Fields.AllocText Get(QuickFix.Fields.AllocText val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.AllocText val) 
                { 
                    return IsSetAllocText();
                }
                
                public bool IsSetAllocText() 
                { 
                    return IsSetField(Tags.AllocText);
                }
                public QuickFix.Fields.EncodedAllocTextLen EncodedAllocTextLen
                { 
                    get 
                    {
                        QuickFix.Fields.EncodedAllocTextLen val = new QuickFix.Fields.EncodedAllocTextLen();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.EncodedAllocTextLen val) 
                { 
                    this.EncodedAllocTextLen = val;
                }
                
                public QuickFix.Fields.EncodedAllocTextLen Get(QuickFix.Fields.EncodedAllocTextLen val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.EncodedAllocTextLen val) 
                { 
                    return IsSetEncodedAllocTextLen();
                }
                
                public bool IsSetEncodedAllocTextLen() 
                { 
                    return IsSetField(Tags.EncodedAllocTextLen);
                }
                public QuickFix.Fields.EncodedAllocText EncodedAllocText
                { 
                    get 
                    {
                        QuickFix.Fields.EncodedAllocText val = new QuickFix.Fields.EncodedAllocText();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.EncodedAllocText val) 
                { 
                    this.EncodedAllocText = val;
                }
                
                public QuickFix.Fields.EncodedAllocText Get(QuickFix.Fields.EncodedAllocText val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.EncodedAllocText val) 
                { 
                    return IsSetEncodedAllocText();
                }
                
                public bool IsSetEncodedAllocText() 
                { 
                    return IsSetField(Tags.EncodedAllocText);
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
                public QuickFix.Fields.AllocAvgPx AllocAvgPx
                { 
                    get 
                    {
                        QuickFix.Fields.AllocAvgPx val = new QuickFix.Fields.AllocAvgPx();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.AllocAvgPx val) 
                { 
                    this.AllocAvgPx = val;
                }
                
                public QuickFix.Fields.AllocAvgPx Get(QuickFix.Fields.AllocAvgPx val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.AllocAvgPx val) 
                { 
                    return IsSetAllocAvgPx();
                }
                
                public bool IsSetAllocAvgPx() 
                { 
                    return IsSetField(Tags.AllocAvgPx);
                }
                public QuickFix.Fields.AllocNetMoney AllocNetMoney
                { 
                    get 
                    {
                        QuickFix.Fields.AllocNetMoney val = new QuickFix.Fields.AllocNetMoney();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.AllocNetMoney val) 
                { 
                    this.AllocNetMoney = val;
                }
                
                public QuickFix.Fields.AllocNetMoney Get(QuickFix.Fields.AllocNetMoney val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.AllocNetMoney val) 
                { 
                    return IsSetAllocNetMoney();
                }
                
                public bool IsSetAllocNetMoney() 
                { 
                    return IsSetField(Tags.AllocNetMoney);
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
                public QuickFix.Fields.AllocSettlCurrAmt AllocSettlCurrAmt
                { 
                    get 
                    {
                        QuickFix.Fields.AllocSettlCurrAmt val = new QuickFix.Fields.AllocSettlCurrAmt();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.AllocSettlCurrAmt val) 
                { 
                    this.AllocSettlCurrAmt = val;
                }
                
                public QuickFix.Fields.AllocSettlCurrAmt Get(QuickFix.Fields.AllocSettlCurrAmt val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.AllocSettlCurrAmt val) 
                { 
                    return IsSetAllocSettlCurrAmt();
                }
                
                public bool IsSetAllocSettlCurrAmt() 
                { 
                    return IsSetField(Tags.AllocSettlCurrAmt);
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
                public QuickFix.Fields.AllocSettlCurrency AllocSettlCurrency
                { 
                    get 
                    {
                        QuickFix.Fields.AllocSettlCurrency val = new QuickFix.Fields.AllocSettlCurrency();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.AllocSettlCurrency val) 
                { 
                    this.AllocSettlCurrency = val;
                }
                
                public QuickFix.Fields.AllocSettlCurrency Get(QuickFix.Fields.AllocSettlCurrency val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.AllocSettlCurrency val) 
                { 
                    return IsSetAllocSettlCurrency();
                }
                
                public bool IsSetAllocSettlCurrency() 
                { 
                    return IsSetField(Tags.AllocSettlCurrency);
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
                public QuickFix.Fields.AllocAccruedInterestAmt AllocAccruedInterestAmt
                { 
                    get 
                    {
                        QuickFix.Fields.AllocAccruedInterestAmt val = new QuickFix.Fields.AllocAccruedInterestAmt();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.AllocAccruedInterestAmt val) 
                { 
                    this.AllocAccruedInterestAmt = val;
                }
                
                public QuickFix.Fields.AllocAccruedInterestAmt Get(QuickFix.Fields.AllocAccruedInterestAmt val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.AllocAccruedInterestAmt val) 
                { 
                    return IsSetAllocAccruedInterestAmt();
                }
                
                public bool IsSetAllocAccruedInterestAmt() 
                { 
                    return IsSetField(Tags.AllocAccruedInterestAmt);
                }
                public QuickFix.Fields.AllocInterestAtMaturity AllocInterestAtMaturity
                { 
                    get 
                    {
                        QuickFix.Fields.AllocInterestAtMaturity val = new QuickFix.Fields.AllocInterestAtMaturity();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.AllocInterestAtMaturity val) 
                { 
                    this.AllocInterestAtMaturity = val;
                }
                
                public QuickFix.Fields.AllocInterestAtMaturity Get(QuickFix.Fields.AllocInterestAtMaturity val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.AllocInterestAtMaturity val) 
                { 
                    return IsSetAllocInterestAtMaturity();
                }
                
                public bool IsSetAllocInterestAtMaturity() 
                { 
                    return IsSetField(Tags.AllocInterestAtMaturity);
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
                public QuickFix.Fields.NoClearingInstructions NoClearingInstructions
                { 
                    get 
                    {
                        QuickFix.Fields.NoClearingInstructions val = new QuickFix.Fields.NoClearingInstructions();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.NoClearingInstructions val) 
                { 
                    this.NoClearingInstructions = val;
                }
                
                public QuickFix.Fields.NoClearingInstructions Get(QuickFix.Fields.NoClearingInstructions val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.NoClearingInstructions val) 
                { 
                    return IsSetNoClearingInstructions();
                }
                
                public bool IsSetNoClearingInstructions() 
                { 
                    return IsSetField(Tags.NoClearingInstructions);
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
                public QuickFix.Fields.AllocSettlInstType AllocSettlInstType
                { 
                    get 
                    {
                        QuickFix.Fields.AllocSettlInstType val = new QuickFix.Fields.AllocSettlInstType();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.AllocSettlInstType val) 
                { 
                    this.AllocSettlInstType = val;
                }
                
                public QuickFix.Fields.AllocSettlInstType Get(QuickFix.Fields.AllocSettlInstType val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.AllocSettlInstType val) 
                { 
                    return IsSetAllocSettlInstType();
                }
                
                public bool IsSetAllocSettlInstType() 
                { 
                    return IsSetField(Tags.AllocSettlInstType);
                }
                public QuickFix.Fields.SettlDeliveryType SettlDeliveryType
                { 
                    get 
                    {
                        QuickFix.Fields.SettlDeliveryType val = new QuickFix.Fields.SettlDeliveryType();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.SettlDeliveryType val) 
                { 
                    this.SettlDeliveryType = val;
                }
                
                public QuickFix.Fields.SettlDeliveryType Get(QuickFix.Fields.SettlDeliveryType val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.SettlDeliveryType val) 
                { 
                    return IsSetSettlDeliveryType();
                }
                
                public bool IsSetSettlDeliveryType() 
                { 
                    return IsSetField(Tags.SettlDeliveryType);
                }
                public QuickFix.Fields.StandInstDbType StandInstDbType
                { 
                    get 
                    {
                        QuickFix.Fields.StandInstDbType val = new QuickFix.Fields.StandInstDbType();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.StandInstDbType val) 
                { 
                    this.StandInstDbType = val;
                }
                
                public QuickFix.Fields.StandInstDbType Get(QuickFix.Fields.StandInstDbType val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.StandInstDbType val) 
                { 
                    return IsSetStandInstDbType();
                }
                
                public bool IsSetStandInstDbType() 
                { 
                    return IsSetField(Tags.StandInstDbType);
                }
                public QuickFix.Fields.StandInstDbName StandInstDbName
                { 
                    get 
                    {
                        QuickFix.Fields.StandInstDbName val = new QuickFix.Fields.StandInstDbName();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.StandInstDbName val) 
                { 
                    this.StandInstDbName = val;
                }
                
                public QuickFix.Fields.StandInstDbName Get(QuickFix.Fields.StandInstDbName val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.StandInstDbName val) 
                { 
                    return IsSetStandInstDbName();
                }
                
                public bool IsSetStandInstDbName() 
                { 
                    return IsSetField(Tags.StandInstDbName);
                }
                public QuickFix.Fields.StandInstDbID StandInstDbID
                { 
                    get 
                    {
                        QuickFix.Fields.StandInstDbID val = new QuickFix.Fields.StandInstDbID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.StandInstDbID val) 
                { 
                    this.StandInstDbID = val;
                }
                
                public QuickFix.Fields.StandInstDbID Get(QuickFix.Fields.StandInstDbID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.StandInstDbID val) 
                { 
                    return IsSetStandInstDbID();
                }
                
                public bool IsSetStandInstDbID() 
                { 
                    return IsSetField(Tags.StandInstDbID);
                }
                            public class NoNestedPartyIDsGroup : Group
                {
                    public static int[] fieldOrder = {Tags.NestedPartyID, Tags.NestedPartyIDSource, Tags.NestedPartyRole, Tags.NoNestedPartySubIDs, 0};
                
                    public NoNestedPartyIDsGroup() 
                      :base( Tags.NoNestedPartyIDs, Tags.NestedPartyID, fieldOrder)
                    {
                    }
                
                    public override Group Clone()
                    {
                        var clone = new NoNestedPartyIDsGroup();
                        clone.CopyStateFrom(this);
                        return clone;
                    }
                
                                    public QuickFix.Fields.NestedPartyID NestedPartyID
                    { 
                        get 
                        {
                            QuickFix.Fields.NestedPartyID val = new QuickFix.Fields.NestedPartyID();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.NestedPartyID val) 
                    { 
                        this.NestedPartyID = val;
                    }
                    
                    public QuickFix.Fields.NestedPartyID Get(QuickFix.Fields.NestedPartyID val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.NestedPartyID val) 
                    { 
                        return IsSetNestedPartyID();
                    }
                    
                    public bool IsSetNestedPartyID() 
                    { 
                        return IsSetField(Tags.NestedPartyID);
                    }
                    public QuickFix.Fields.NestedPartyIDSource NestedPartyIDSource
                    { 
                        get 
                        {
                            QuickFix.Fields.NestedPartyIDSource val = new QuickFix.Fields.NestedPartyIDSource();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.NestedPartyIDSource val) 
                    { 
                        this.NestedPartyIDSource = val;
                    }
                    
                    public QuickFix.Fields.NestedPartyIDSource Get(QuickFix.Fields.NestedPartyIDSource val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.NestedPartyIDSource val) 
                    { 
                        return IsSetNestedPartyIDSource();
                    }
                    
                    public bool IsSetNestedPartyIDSource() 
                    { 
                        return IsSetField(Tags.NestedPartyIDSource);
                    }
                    public QuickFix.Fields.NestedPartyRole NestedPartyRole
                    { 
                        get 
                        {
                            QuickFix.Fields.NestedPartyRole val = new QuickFix.Fields.NestedPartyRole();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.NestedPartyRole val) 
                    { 
                        this.NestedPartyRole = val;
                    }
                    
                    public QuickFix.Fields.NestedPartyRole Get(QuickFix.Fields.NestedPartyRole val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.NestedPartyRole val) 
                    { 
                        return IsSetNestedPartyRole();
                    }
                    
                    public bool IsSetNestedPartyRole() 
                    { 
                        return IsSetField(Tags.NestedPartyRole);
                    }
                    public QuickFix.Fields.NoNestedPartySubIDs NoNestedPartySubIDs
                    { 
                        get 
                        {
                            QuickFix.Fields.NoNestedPartySubIDs val = new QuickFix.Fields.NoNestedPartySubIDs();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.NoNestedPartySubIDs val) 
                    { 
                        this.NoNestedPartySubIDs = val;
                    }
                    
                    public QuickFix.Fields.NoNestedPartySubIDs Get(QuickFix.Fields.NoNestedPartySubIDs val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.NoNestedPartySubIDs val) 
                    { 
                        return IsSetNoNestedPartySubIDs();
                    }
                    
                    public bool IsSetNoNestedPartySubIDs() 
                    { 
                        return IsSetField(Tags.NoNestedPartySubIDs);
                    }
                                    public class NoNestedPartySubIDsGroup : Group
                    {
                        public static int[] fieldOrder = {Tags.NestedPartySubID, Tags.NestedPartySubIDType, 0};
                    
                        public NoNestedPartySubIDsGroup() 
                          :base( Tags.NoNestedPartySubIDs, Tags.NestedPartySubID, fieldOrder)
                        {
                        }
                    
                        public override Group Clone()
                        {
                            var clone = new NoNestedPartySubIDsGroup();
                            clone.CopyStateFrom(this);
                            return clone;
                        }
                    
                                            public QuickFix.Fields.NestedPartySubID NestedPartySubID
                        { 
                            get 
                            {
                                QuickFix.Fields.NestedPartySubID val = new QuickFix.Fields.NestedPartySubID();
                                GetField(val);
                                return val;
                            }
                            set { SetField(value); }
                        }
                        
                        public void Set(QuickFix.Fields.NestedPartySubID val) 
                        { 
                            this.NestedPartySubID = val;
                        }
                        
                        public QuickFix.Fields.NestedPartySubID Get(QuickFix.Fields.NestedPartySubID val) 
                        { 
                            GetField(val);
                            return val;
                        }
                        
                        public bool IsSet(QuickFix.Fields.NestedPartySubID val) 
                        { 
                            return IsSetNestedPartySubID();
                        }
                        
                        public bool IsSetNestedPartySubID() 
                        { 
                            return IsSetField(Tags.NestedPartySubID);
                        }
                        public QuickFix.Fields.NestedPartySubIDType NestedPartySubIDType
                        { 
                            get 
                            {
                                QuickFix.Fields.NestedPartySubIDType val = new QuickFix.Fields.NestedPartySubIDType();
                                GetField(val);
                                return val;
                            }
                            set { SetField(value); }
                        }
                        
                        public void Set(QuickFix.Fields.NestedPartySubIDType val) 
                        { 
                            this.NestedPartySubIDType = val;
                        }
                        
                        public QuickFix.Fields.NestedPartySubIDType Get(QuickFix.Fields.NestedPartySubIDType val) 
                        { 
                            GetField(val);
                            return val;
                        }
                        
                        public bool IsSet(QuickFix.Fields.NestedPartySubIDType val) 
                        { 
                            return IsSetNestedPartySubIDType();
                        }
                        
                        public bool IsSetNestedPartySubIDType() 
                        { 
                            return IsSetField(Tags.NestedPartySubIDType);
                        }
                    
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
                public class NoClearingInstructionsGroup : Group
                {
                    public static int[] fieldOrder = {Tags.ClearingInstruction, 0};
                
                    public NoClearingInstructionsGroup() 
                      :base( Tags.NoClearingInstructions, Tags.ClearingInstruction, fieldOrder)
                    {
                    }
                
                    public override Group Clone()
                    {
                        var clone = new NoClearingInstructionsGroup();
                        clone.CopyStateFrom(this);
                        return clone;
                    }
                
                                    public QuickFix.Fields.ClearingInstruction ClearingInstruction
                    { 
                        get 
                        {
                            QuickFix.Fields.ClearingInstruction val = new QuickFix.Fields.ClearingInstruction();
                            GetField(val);
                            return val;
                        }
                        set { SetField(value); }
                    }
                    
                    public void Set(QuickFix.Fields.ClearingInstruction val) 
                    { 
                        this.ClearingInstruction = val;
                    }
                    
                    public QuickFix.Fields.ClearingInstruction Get(QuickFix.Fields.ClearingInstruction val) 
                    { 
                        GetField(val);
                        return val;
                    }
                    
                    public bool IsSet(QuickFix.Fields.ClearingInstruction val) 
                    { 
                        return IsSetClearingInstruction();
                    }
                    
                    public bool IsSetClearingInstruction() 
                    { 
                        return IsSetField(Tags.ClearingInstruction);
                    }
                
                }
            }
        }
    }
}
