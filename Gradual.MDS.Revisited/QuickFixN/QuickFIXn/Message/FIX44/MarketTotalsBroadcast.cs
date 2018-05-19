// This is a generated file.  Don't edit it directly!

using QuickFix.Fields;
namespace QuickFix
{
    namespace FIX44 
    {
        public class MarketTotalsBroadcast : Message
        {
            public const string MsgType = "UTOT";

            public MarketTotalsBroadcast() : base()
            {
                this.Header.SetField(new QuickFix.Fields.MsgType("UTOT"));
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
            public class NoMDEntriesGroup : Group
            {
                public static int[] fieldOrder = {Tags.MDEntryType, Tags.Symbol, Tags.MDEntryDate, Tags.MDEntryTime, Tags.GrossTradeAmt, Tags.TotalVolumeTraded, Tags.TotalNumOfTrades, 0};
            
                public NoMDEntriesGroup() 
                  :base( Tags.NoMDEntries, Tags.MDEntryType, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoMDEntriesGroup();
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
                public QuickFix.Fields.TotalVolumeTraded TotalVolumeTraded
                { 
                    get 
                    {
                        QuickFix.Fields.TotalVolumeTraded val = new QuickFix.Fields.TotalVolumeTraded();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.TotalVolumeTraded val) 
                { 
                    this.TotalVolumeTraded = val;
                }
                
                public QuickFix.Fields.TotalVolumeTraded Get(QuickFix.Fields.TotalVolumeTraded val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.TotalVolumeTraded val) 
                { 
                    return IsSetTotalVolumeTraded();
                }
                
                public bool IsSetTotalVolumeTraded() 
                { 
                    return IsSetField(Tags.TotalVolumeTraded);
                }
                public QuickFix.Fields.TotalNumOfTrades TotalNumOfTrades
                { 
                    get 
                    {
                        QuickFix.Fields.TotalNumOfTrades val = new QuickFix.Fields.TotalNumOfTrades();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.TotalNumOfTrades val) 
                { 
                    this.TotalNumOfTrades = val;
                }
                
                public QuickFix.Fields.TotalNumOfTrades Get(QuickFix.Fields.TotalNumOfTrades val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.TotalNumOfTrades val) 
                { 
                    return IsSetTotalNumOfTrades();
                }
                
                public bool IsSetTotalNumOfTrades() 
                { 
                    return IsSetField(Tags.TotalNumOfTrades);
                }
            
            }
        }
    }
}
