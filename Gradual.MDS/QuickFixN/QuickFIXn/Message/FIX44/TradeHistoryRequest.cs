// This is a generated file.  Don't edit it directly!

using QuickFix.Fields;
namespace QuickFix
{
    namespace FIX44 
    {
        public class TradeHistoryRequest : Message
        {
            public const string MsgType = "UTHQ";

            public TradeHistoryRequest() : base()
            {
                this.Header.SetField(new QuickFix.Fields.MsgType("UTHQ"));
            }

            public TradeHistoryRequest(
                    QuickFix.Fields.MDReqID aMDReqID,
                    QuickFix.Fields.SubscriptionRequestType aSubscriptionRequestType
                ) : this()
            {
                this.MDReqID = aMDReqID;
                this.SubscriptionRequestType = aSubscriptionRequestType;
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
            public QuickFix.Fields.SubscriptionRequestType SubscriptionRequestType
            { 
                get 
                {
                    QuickFix.Fields.SubscriptionRequestType val = new QuickFix.Fields.SubscriptionRequestType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.SubscriptionRequestType val) 
            { 
                this.SubscriptionRequestType = val;
            }
            
            public QuickFix.Fields.SubscriptionRequestType Get(QuickFix.Fields.SubscriptionRequestType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.SubscriptionRequestType val) 
            { 
                return IsSetSubscriptionRequestType();
            }
            
            public bool IsSetSubscriptionRequestType() 
            { 
                return IsSetField(Tags.SubscriptionRequestType);
            }
            public QuickFix.Fields.NoMDEntryTypes NoMDEntryTypes
            { 
                get 
                {
                    QuickFix.Fields.NoMDEntryTypes val = new QuickFix.Fields.NoMDEntryTypes();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoMDEntryTypes val) 
            { 
                this.NoMDEntryTypes = val;
            }
            
            public QuickFix.Fields.NoMDEntryTypes Get(QuickFix.Fields.NoMDEntryTypes val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoMDEntryTypes val) 
            { 
                return IsSetNoMDEntryTypes();
            }
            
            public bool IsSetNoMDEntryTypes() 
            { 
                return IsSetField(Tags.NoMDEntryTypes);
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
            public QuickFix.Fields.SecurityUpdatesSince SecurityUpdatesSince
            { 
                get 
                {
                    QuickFix.Fields.SecurityUpdatesSince val = new QuickFix.Fields.SecurityUpdatesSince();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.SecurityUpdatesSince val) 
            { 
                this.SecurityUpdatesSince = val;
            }
            
            public QuickFix.Fields.SecurityUpdatesSince Get(QuickFix.Fields.SecurityUpdatesSince val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.SecurityUpdatesSince val) 
            { 
                return IsSetSecurityUpdatesSince();
            }
            
            public bool IsSetSecurityUpdatesSince() 
            { 
                return IsSetField(Tags.SecurityUpdatesSince);
            }
            public class NoMDEntryTypesGroup : Group
            {
                public static int[] fieldOrder = {Tags.MDEntryType, 0};
            
                public NoMDEntryTypesGroup() 
                  :base( Tags.NoMDEntryTypes, Tags.MDEntryType, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoMDEntryTypesGroup();
                    clone.CopyStateFrom(this);
                    return clone;
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
            
            }
            public class NoRelatedSymGroup : Group
            {
                public static int[] fieldOrder = {Tags.SecurityID, Tags.SecurityIDSource, Tags.SecurityExchange, Tags.StartSequence, Tags.EndSequence, 0};
            
                public NoRelatedSymGroup() 
                  :base( Tags.NoRelatedSym, Tags.SecurityID, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoRelatedSymGroup();
                    clone.CopyStateFrom(this);
                    return clone;
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
                public QuickFix.Fields.StartSequence StartSequence
                { 
                    get 
                    {
                        QuickFix.Fields.StartSequence val = new QuickFix.Fields.StartSequence();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.StartSequence val) 
                { 
                    this.StartSequence = val;
                }
                
                public QuickFix.Fields.StartSequence Get(QuickFix.Fields.StartSequence val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.StartSequence val) 
                { 
                    return IsSetStartSequence();
                }
                
                public bool IsSetStartSequence() 
                { 
                    return IsSetField(Tags.StartSequence);
                }
                public QuickFix.Fields.EndSequence EndSequence
                { 
                    get 
                    {
                        QuickFix.Fields.EndSequence val = new QuickFix.Fields.EndSequence();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.EndSequence val) 
                { 
                    this.EndSequence = val;
                }
                
                public QuickFix.Fields.EndSequence Get(QuickFix.Fields.EndSequence val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.EndSequence val) 
                { 
                    return IsSetEndSequence();
                }
                
                public bool IsSetEndSequence() 
                { 
                    return IsSetField(Tags.EndSequence);
                }
            
            }
        }
    }
}
