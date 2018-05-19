// This is a generated file.  Don't edit it directly!

using QuickFix.Fields;
namespace QuickFix
{
    namespace FIX44 
    {
        public class ApplicationRawDataReporting : Message
        {
            public const string MsgType = "URDR";

            public ApplicationRawDataReporting() : base()
            {
                this.Header.SetField(new QuickFix.Fields.MsgType("URDR"));
            }

            public ApplicationRawDataReporting(
                    QuickFix.Fields.ApplReqID aApplReqID,
                    QuickFix.Fields.ApplRespID aApplRespID,
                    QuickFix.Fields.ApplID aApplID,
                    QuickFix.Fields.ApplResendFlag aApplResendFlag,
                    QuickFix.Fields.RawDataLength aRawDataLength,
                    QuickFix.Fields.RawData aRawData,
                    QuickFix.Fields.TotNumReports aTotNumReports
                ) : this()
            {
                this.ApplReqID = aApplReqID;
                this.ApplRespID = aApplRespID;
                this.ApplID = aApplID;
                this.ApplResendFlag = aApplResendFlag;
                this.RawDataLength = aRawDataLength;
                this.RawData = aRawData;
                this.TotNumReports = aTotNumReports;
            }

            public QuickFix.Fields.ApplReqID ApplReqID
            { 
                get 
                {
                    QuickFix.Fields.ApplReqID val = new QuickFix.Fields.ApplReqID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ApplReqID val) 
            { 
                this.ApplReqID = val;
            }
            
            public QuickFix.Fields.ApplReqID Get(QuickFix.Fields.ApplReqID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ApplReqID val) 
            { 
                return IsSetApplReqID();
            }
            
            public bool IsSetApplReqID() 
            { 
                return IsSetField(Tags.ApplReqID);
            }
            public QuickFix.Fields.ApplRespID ApplRespID
            { 
                get 
                {
                    QuickFix.Fields.ApplRespID val = new QuickFix.Fields.ApplRespID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ApplRespID val) 
            { 
                this.ApplRespID = val;
            }
            
            public QuickFix.Fields.ApplRespID Get(QuickFix.Fields.ApplRespID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ApplRespID val) 
            { 
                return IsSetApplRespID();
            }
            
            public bool IsSetApplRespID() 
            { 
                return IsSetField(Tags.ApplRespID);
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
            public QuickFix.Fields.RawDataLength RawDataLength
            { 
                get 
                {
                    QuickFix.Fields.RawDataLength val = new QuickFix.Fields.RawDataLength();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.RawDataLength val) 
            { 
                this.RawDataLength = val;
            }
            
            public QuickFix.Fields.RawDataLength Get(QuickFix.Fields.RawDataLength val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.RawDataLength val) 
            { 
                return IsSetRawDataLength();
            }
            
            public bool IsSetRawDataLength() 
            { 
                return IsSetField(Tags.RawDataLength);
            }
            public QuickFix.Fields.RawData RawData
            { 
                get 
                {
                    QuickFix.Fields.RawData val = new QuickFix.Fields.RawData();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.RawData val) 
            { 
                this.RawData = val;
            }
            
            public QuickFix.Fields.RawData Get(QuickFix.Fields.RawData val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.RawData val) 
            { 
                return IsSetRawData();
            }
            
            public bool IsSetRawData() 
            { 
                return IsSetField(Tags.RawData);
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
            public QuickFix.Fields.NoApplSeqNums NoApplSeqNums
            { 
                get 
                {
                    QuickFix.Fields.NoApplSeqNums val = new QuickFix.Fields.NoApplSeqNums();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoApplSeqNums val) 
            { 
                this.NoApplSeqNums = val;
            }
            
            public QuickFix.Fields.NoApplSeqNums Get(QuickFix.Fields.NoApplSeqNums val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoApplSeqNums val) 
            { 
                return IsSetNoApplSeqNums();
            }
            
            public bool IsSetNoApplSeqNums() 
            { 
                return IsSetField(Tags.NoApplSeqNums);
            }
            public class NoApplSeqNumsGroup : Group
            {
                public static int[] fieldOrder = {Tags.ApplSeqNum, Tags.ApplLastSeqNum, Tags.RawDataOffset, Tags.RawDataLength, 0};
            
                public NoApplSeqNumsGroup() 
                  :base( Tags.NoApplSeqNums, Tags.ApplSeqNum, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoApplSeqNumsGroup();
                    clone.CopyStateFrom(this);
                    return clone;
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
                public QuickFix.Fields.RawDataOffset RawDataOffset
                { 
                    get 
                    {
                        QuickFix.Fields.RawDataOffset val = new QuickFix.Fields.RawDataOffset();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.RawDataOffset val) 
                { 
                    this.RawDataOffset = val;
                }
                
                public QuickFix.Fields.RawDataOffset Get(QuickFix.Fields.RawDataOffset val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.RawDataOffset val) 
                { 
                    return IsSetRawDataOffset();
                }
                
                public bool IsSetRawDataOffset() 
                { 
                    return IsSetField(Tags.RawDataOffset);
                }
                public QuickFix.Fields.RawDataLength RawDataLength
                { 
                    get 
                    {
                        QuickFix.Fields.RawDataLength val = new QuickFix.Fields.RawDataLength();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.RawDataLength val) 
                { 
                    this.RawDataLength = val;
                }
                
                public QuickFix.Fields.RawDataLength Get(QuickFix.Fields.RawDataLength val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.RawDataLength val) 
                { 
                    return IsSetRawDataLength();
                }
                
                public bool IsSetRawDataLength() 
                { 
                    return IsSetField(Tags.RawDataLength);
                }
            
            }
        }
    }
}
