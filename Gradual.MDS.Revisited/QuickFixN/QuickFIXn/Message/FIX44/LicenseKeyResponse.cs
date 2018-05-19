// This is a generated file.  Don't edit it directly!

using QuickFix.Fields;
namespace QuickFix
{
    namespace FIX44 
    {
        public class LicenseKeyResponse : Message
        {
            public const string MsgType = "ULRP";

            public LicenseKeyResponse() : base()
            {
                this.Header.SetField(new QuickFix.Fields.MsgType("ULRP"));
            }

            public LicenseKeyResponse(
                    QuickFix.Fields.LicenseRequestID aLicenseRequestID,
                    QuickFix.Fields.LicenseRequestStatus aLicenseRequestStatus
                ) : this()
            {
                this.LicenseRequestID = aLicenseRequestID;
                this.LicenseRequestStatus = aLicenseRequestStatus;
            }

            public QuickFix.Fields.LicenseRequestID LicenseRequestID
            { 
                get 
                {
                    QuickFix.Fields.LicenseRequestID val = new QuickFix.Fields.LicenseRequestID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.LicenseRequestID val) 
            { 
                this.LicenseRequestID = val;
            }
            
            public QuickFix.Fields.LicenseRequestID Get(QuickFix.Fields.LicenseRequestID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.LicenseRequestID val) 
            { 
                return IsSetLicenseRequestID();
            }
            
            public bool IsSetLicenseRequestID() 
            { 
                return IsSetField(Tags.LicenseRequestID);
            }
            public QuickFix.Fields.LicenseRequestStatus LicenseRequestStatus
            { 
                get 
                {
                    QuickFix.Fields.LicenseRequestStatus val = new QuickFix.Fields.LicenseRequestStatus();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.LicenseRequestStatus val) 
            { 
                this.LicenseRequestStatus = val;
            }
            
            public QuickFix.Fields.LicenseRequestStatus Get(QuickFix.Fields.LicenseRequestStatus val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.LicenseRequestStatus val) 
            { 
                return IsSetLicenseRequestStatus();
            }
            
            public bool IsSetLicenseRequestStatus() 
            { 
                return IsSetField(Tags.LicenseRequestStatus);
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
            public QuickFix.Fields.MarketDate MarketDate
            { 
                get 
                {
                    QuickFix.Fields.MarketDate val = new QuickFix.Fields.MarketDate();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.MarketDate val) 
            { 
                this.MarketDate = val;
            }
            
            public QuickFix.Fields.MarketDate Get(QuickFix.Fields.MarketDate val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.MarketDate val) 
            { 
                return IsSetMarketDate();
            }
            
            public bool IsSetMarketDate() 
            { 
                return IsSetField(Tags.MarketDate);
            }
            public QuickFix.Fields.TraderCode TraderCode
            { 
                get 
                {
                    QuickFix.Fields.TraderCode val = new QuickFix.Fields.TraderCode();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.TraderCode val) 
            { 
                this.TraderCode = val;
            }
            
            public QuickFix.Fields.TraderCode Get(QuickFix.Fields.TraderCode val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.TraderCode val) 
            { 
                return IsSetTraderCode();
            }
            
            public bool IsSetTraderCode() 
            { 
                return IsSetField(Tags.TraderCode);
            }
            public QuickFix.Fields.NoUserRoleIDs NoUserRoleIDs
            { 
                get 
                {
                    QuickFix.Fields.NoUserRoleIDs val = new QuickFix.Fields.NoUserRoleIDs();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoUserRoleIDs val) 
            { 
                this.NoUserRoleIDs = val;
            }
            
            public QuickFix.Fields.NoUserRoleIDs Get(QuickFix.Fields.NoUserRoleIDs val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoUserRoleIDs val) 
            { 
                return IsSetNoUserRoleIDs();
            }
            
            public bool IsSetNoUserRoleIDs() 
            { 
                return IsSetField(Tags.NoUserRoleIDs);
            }
            public QuickFix.Fields.UserAssignedIdentifier UserAssignedIdentifier
            { 
                get 
                {
                    QuickFix.Fields.UserAssignedIdentifier val = new QuickFix.Fields.UserAssignedIdentifier();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.UserAssignedIdentifier val) 
            { 
                this.UserAssignedIdentifier = val;
            }
            
            public QuickFix.Fields.UserAssignedIdentifier Get(QuickFix.Fields.UserAssignedIdentifier val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.UserAssignedIdentifier val) 
            { 
                return IsSetUserAssignedIdentifier();
            }
            
            public bool IsSetUserAssignedIdentifier() 
            { 
                return IsSetField(Tags.UserAssignedIdentifier);
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
            public QuickFix.Fields.NoTradeIDs NoTradeIDs
            { 
                get 
                {
                    QuickFix.Fields.NoTradeIDs val = new QuickFix.Fields.NoTradeIDs();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoTradeIDs val) 
            { 
                this.NoTradeIDs = val;
            }
            
            public QuickFix.Fields.NoTradeIDs Get(QuickFix.Fields.NoTradeIDs val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoTradeIDs val) 
            { 
                return IsSetNoTradeIDs();
            }
            
            public bool IsSetNoTradeIDs() 
            { 
                return IsSetField(Tags.NoTradeIDs);
            }
            public QuickFix.Fields.NoBroadCastIDs NoBroadCastIDs
            { 
                get 
                {
                    QuickFix.Fields.NoBroadCastIDs val = new QuickFix.Fields.NoBroadCastIDs();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoBroadCastIDs val) 
            { 
                this.NoBroadCastIDs = val;
            }
            
            public QuickFix.Fields.NoBroadCastIDs Get(QuickFix.Fields.NoBroadCastIDs val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoBroadCastIDs val) 
            { 
                return IsSetNoBroadCastIDs();
            }
            
            public bool IsSetNoBroadCastIDs() 
            { 
                return IsSetField(Tags.NoBroadCastIDs);
            }
            public QuickFix.Fields.NoNewsAgencyIDs NoNewsAgencyIDs
            { 
                get 
                {
                    QuickFix.Fields.NoNewsAgencyIDs val = new QuickFix.Fields.NoNewsAgencyIDs();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoNewsAgencyIDs val) 
            { 
                this.NoNewsAgencyIDs = val;
            }
            
            public QuickFix.Fields.NoNewsAgencyIDs Get(QuickFix.Fields.NoNewsAgencyIDs val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoNewsAgencyIDs val) 
            { 
                return IsSetNoNewsAgencyIDs();
            }
            
            public bool IsSetNoNewsAgencyIDs() 
            { 
                return IsSetField(Tags.NoNewsAgencyIDs);
            }
            public QuickFix.Fields.NoSupervisedIDs NoSupervisedIDs
            { 
                get 
                {
                    QuickFix.Fields.NoSupervisedIDs val = new QuickFix.Fields.NoSupervisedIDs();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoSupervisedIDs val) 
            { 
                this.NoSupervisedIDs = val;
            }
            
            public QuickFix.Fields.NoSupervisedIDs Get(QuickFix.Fields.NoSupervisedIDs val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoSupervisedIDs val) 
            { 
                return IsSetNoSupervisedIDs();
            }
            
            public bool IsSetNoSupervisedIDs() 
            { 
                return IsSetField(Tags.NoSupervisedIDs);
            }
            public class NoUserRoleIDsGroup : Group
            {
                public static int[] fieldOrder = {Tags.UserRoleID, 0};
            
                public NoUserRoleIDsGroup() 
                  :base( Tags.NoUserRoleIDs, Tags.UserRoleID, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoUserRoleIDsGroup();
                    clone.CopyStateFrom(this);
                    return clone;
                }
            
                            public QuickFix.Fields.UserRoleID UserRoleID
                { 
                    get 
                    {
                        QuickFix.Fields.UserRoleID val = new QuickFix.Fields.UserRoleID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.UserRoleID val) 
                { 
                    this.UserRoleID = val;
                }
                
                public QuickFix.Fields.UserRoleID Get(QuickFix.Fields.UserRoleID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.UserRoleID val) 
                { 
                    return IsSetUserRoleID();
                }
                
                public bool IsSetUserRoleID() 
                { 
                    return IsSetField(Tags.UserRoleID);
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
            public class NoTradeIDsGroup : Group
            {
                public static int[] fieldOrder = {Tags.TradeID, 0};
            
                public NoTradeIDsGroup() 
                  :base( Tags.NoTradeIDs, Tags.TradeID, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoTradeIDsGroup();
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
            
            }
            public class NoBroadCastIDsGroup : Group
            {
                public static int[] fieldOrder = {Tags.BroadCastID, 0};
            
                public NoBroadCastIDsGroup() 
                  :base( Tags.NoBroadCastIDs, Tags.BroadCastID, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoBroadCastIDsGroup();
                    clone.CopyStateFrom(this);
                    return clone;
                }
            
                            public QuickFix.Fields.BroadCastID BroadCastID
                { 
                    get 
                    {
                        QuickFix.Fields.BroadCastID val = new QuickFix.Fields.BroadCastID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.BroadCastID val) 
                { 
                    this.BroadCastID = val;
                }
                
                public QuickFix.Fields.BroadCastID Get(QuickFix.Fields.BroadCastID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.BroadCastID val) 
                { 
                    return IsSetBroadCastID();
                }
                
                public bool IsSetBroadCastID() 
                { 
                    return IsSetField(Tags.BroadCastID);
                }
            
            }
            public class NoNewsAgencyIDsGroup : Group
            {
                public static int[] fieldOrder = {Tags.NewsAgencyID, 0};
            
                public NoNewsAgencyIDsGroup() 
                  :base( Tags.NoNewsAgencyIDs, Tags.NewsAgencyID, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoNewsAgencyIDsGroup();
                    clone.CopyStateFrom(this);
                    return clone;
                }
            
                            public QuickFix.Fields.NewsAgencyID NewsAgencyID
                { 
                    get 
                    {
                        QuickFix.Fields.NewsAgencyID val = new QuickFix.Fields.NewsAgencyID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.NewsAgencyID val) 
                { 
                    this.NewsAgencyID = val;
                }
                
                public QuickFix.Fields.NewsAgencyID Get(QuickFix.Fields.NewsAgencyID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.NewsAgencyID val) 
                { 
                    return IsSetNewsAgencyID();
                }
                
                public bool IsSetNewsAgencyID() 
                { 
                    return IsSetField(Tags.NewsAgencyID);
                }
            
            }
            public class NoSupervisedIDsGroup : Group
            {
                public static int[] fieldOrder = {Tags.SupervisedID, 0};
            
                public NoSupervisedIDsGroup() 
                  :base( Tags.NoSupervisedIDs, Tags.SupervisedID, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoSupervisedIDsGroup();
                    clone.CopyStateFrom(this);
                    return clone;
                }
            
                            public QuickFix.Fields.SupervisedID SupervisedID
                { 
                    get 
                    {
                        QuickFix.Fields.SupervisedID val = new QuickFix.Fields.SupervisedID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.SupervisedID val) 
                { 
                    this.SupervisedID = val;
                }
                
                public QuickFix.Fields.SupervisedID Get(QuickFix.Fields.SupervisedID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.SupervisedID val) 
                { 
                    return IsSetSupervisedID();
                }
                
                public bool IsSetSupervisedID() 
                { 
                    return IsSetField(Tags.SupervisedID);
                }
            
            }
        }
    }
}
