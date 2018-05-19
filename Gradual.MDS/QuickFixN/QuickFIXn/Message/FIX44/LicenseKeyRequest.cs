// This is a generated file.  Don't edit it directly!

using QuickFix.Fields;
namespace QuickFix
{
    namespace FIX44 
    {
        public class LicenseKeyRequest : Message
        {
            public const string MsgType = "ULRQ";

            public LicenseKeyRequest() : base()
            {
                this.Header.SetField(new QuickFix.Fields.MsgType("ULRQ"));
            }

            public LicenseKeyRequest(
                    QuickFix.Fields.LicenseRequestID aLicenseRequestID,
                    QuickFix.Fields.ControllerType aControllerType,
                    QuickFix.Fields.Password aPassword,
                    QuickFix.Fields.ForceLogin aForceLogin,
                    QuickFix.Fields.InterfaceVersion aInterfaceVersion,
                    QuickFix.Fields.SubUnitID aSubUnitID
                ) : this()
            {
                this.LicenseRequestID = aLicenseRequestID;
                this.ControllerType = aControllerType;
                this.Password = aPassword;
                this.ForceLogin = aForceLogin;
                this.InterfaceVersion = aInterfaceVersion;
                this.SubUnitID = aSubUnitID;
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
            public QuickFix.Fields.ControllerType ControllerType
            { 
                get 
                {
                    QuickFix.Fields.ControllerType val = new QuickFix.Fields.ControllerType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ControllerType val) 
            { 
                this.ControllerType = val;
            }
            
            public QuickFix.Fields.ControllerType Get(QuickFix.Fields.ControllerType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ControllerType val) 
            { 
                return IsSetControllerType();
            }
            
            public bool IsSetControllerType() 
            { 
                return IsSetField(Tags.ControllerType);
            }
            public QuickFix.Fields.ControllerID ControllerID
            { 
                get 
                {
                    QuickFix.Fields.ControllerID val = new QuickFix.Fields.ControllerID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ControllerID val) 
            { 
                this.ControllerID = val;
            }
            
            public QuickFix.Fields.ControllerID Get(QuickFix.Fields.ControllerID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ControllerID val) 
            { 
                return IsSetControllerID();
            }
            
            public bool IsSetControllerID() 
            { 
                return IsSetField(Tags.ControllerID);
            }
            public QuickFix.Fields.Password Password
            { 
                get 
                {
                    QuickFix.Fields.Password val = new QuickFix.Fields.Password();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.Password val) 
            { 
                this.Password = val;
            }
            
            public QuickFix.Fields.Password Get(QuickFix.Fields.Password val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.Password val) 
            { 
                return IsSetPassword();
            }
            
            public bool IsSetPassword() 
            { 
                return IsSetField(Tags.Password);
            }
            public QuickFix.Fields.Channel Channel
            { 
                get 
                {
                    QuickFix.Fields.Channel val = new QuickFix.Fields.Channel();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.Channel val) 
            { 
                this.Channel = val;
            }
            
            public QuickFix.Fields.Channel Get(QuickFix.Fields.Channel val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.Channel val) 
            { 
                return IsSetChannel();
            }
            
            public bool IsSetChannel() 
            { 
                return IsSetField(Tags.Channel);
            }
            public QuickFix.Fields.ForceLogin ForceLogin
            { 
                get 
                {
                    QuickFix.Fields.ForceLogin val = new QuickFix.Fields.ForceLogin();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ForceLogin val) 
            { 
                this.ForceLogin = val;
            }
            
            public QuickFix.Fields.ForceLogin Get(QuickFix.Fields.ForceLogin val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ForceLogin val) 
            { 
                return IsSetForceLogin();
            }
            
            public bool IsSetForceLogin() 
            { 
                return IsSetField(Tags.ForceLogin);
            }
            public QuickFix.Fields.InterfaceVersion InterfaceVersion
            { 
                get 
                {
                    QuickFix.Fields.InterfaceVersion val = new QuickFix.Fields.InterfaceVersion();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.InterfaceVersion val) 
            { 
                this.InterfaceVersion = val;
            }
            
            public QuickFix.Fields.InterfaceVersion Get(QuickFix.Fields.InterfaceVersion val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.InterfaceVersion val) 
            { 
                return IsSetInterfaceVersion();
            }
            
            public bool IsSetInterfaceVersion() 
            { 
                return IsSetField(Tags.InterfaceVersion);
            }
            public QuickFix.Fields.NewPassword NewPassword
            { 
                get 
                {
                    QuickFix.Fields.NewPassword val = new QuickFix.Fields.NewPassword();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NewPassword val) 
            { 
                this.NewPassword = val;
            }
            
            public QuickFix.Fields.NewPassword Get(QuickFix.Fields.NewPassword val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NewPassword val) 
            { 
                return IsSetNewPassword();
            }
            
            public bool IsSetNewPassword() 
            { 
                return IsSetField(Tags.NewPassword);
            }
            public QuickFix.Fields.SubUnitID SubUnitID
            { 
                get 
                {
                    QuickFix.Fields.SubUnitID val = new QuickFix.Fields.SubUnitID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.SubUnitID val) 
            { 
                this.SubUnitID = val;
            }
            
            public QuickFix.Fields.SubUnitID Get(QuickFix.Fields.SubUnitID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.SubUnitID val) 
            { 
                return IsSetSubUnitID();
            }
            
            public bool IsSetSubUnitID() 
            { 
                return IsSetField(Tags.SubUnitID);
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
