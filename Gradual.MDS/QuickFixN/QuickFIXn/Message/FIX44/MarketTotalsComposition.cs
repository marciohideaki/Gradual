// This is a generated file.  Don't edit it directly!

using QuickFix.Fields;
namespace QuickFix
{
    namespace FIX44 
    {
        public class MarketTotalsComposition : Message
        {
            public const string MsgType = "UTOTC";

            public MarketTotalsComposition() : base()
            {
                this.Header.SetField(new QuickFix.Fields.MsgType("UTOTC"));
            }

            public MarketTotalsComposition(
                    QuickFix.Fields.TotNoRelatedSym aTotNoRelatedSym,
                    QuickFix.Fields.LastFragment aLastFragment
                ) : this()
            {
                this.TotNoRelatedSym = aTotNoRelatedSym;
                this.LastFragment = aLastFragment;
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
            public QuickFix.Fields.IndexID IndexID
            { 
                get 
                {
                    QuickFix.Fields.IndexID val = new QuickFix.Fields.IndexID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.IndexID val) 
            { 
                this.IndexID = val;
            }
            
            public QuickFix.Fields.IndexID Get(QuickFix.Fields.IndexID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.IndexID val) 
            { 
                return IsSetIndexID();
            }
            
            public bool IsSetIndexID() 
            { 
                return IsSetField(Tags.IndexID);
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
            public class NoRelatedSymGroup : Group
            {
                public static int[] fieldOrder = {Tags.Symbol, Tags.SecurityDesc, Tags.NoSecurityGroups, 0};
            
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
                public QuickFix.Fields.NoSecurityGroups NoSecurityGroups
                { 
                    get 
                    {
                        QuickFix.Fields.NoSecurityGroups val = new QuickFix.Fields.NoSecurityGroups();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.NoSecurityGroups val) 
                { 
                    this.NoSecurityGroups = val;
                }
                
                public QuickFix.Fields.NoSecurityGroups Get(QuickFix.Fields.NoSecurityGroups val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.NoSecurityGroups val) 
                { 
                    return IsSetNoSecurityGroups();
                }
                
                public bool IsSetNoSecurityGroups() 
                { 
                    return IsSetField(Tags.NoSecurityGroups);
                }
                            public class NoSecurityGroupsGroup : Group
                {
                    public static int[] fieldOrder = {Tags.SecurityGroupTMR, 0};
                
                    public NoSecurityGroupsGroup() 
                      :base( Tags.NoSecurityGroups, Tags.SecurityGroupTMR, fieldOrder)
                    {
                    }
                
                    public override Group Clone()
                    {
                        var clone = new NoSecurityGroupsGroup();
                        clone.CopyStateFrom(this);
                        return clone;
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
                
                }
            }
        }
    }
}
