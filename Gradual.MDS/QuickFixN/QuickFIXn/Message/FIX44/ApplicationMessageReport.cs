// This is a generated file.  Don't edit it directly!

using QuickFix.Fields;
namespace QuickFix
{
    namespace FIX44 
    {
        public class ApplicationMessageReport : Message
        {
            public const string MsgType = "BY";

            public ApplicationMessageReport() : base()
            {
                this.Header.SetField(new QuickFix.Fields.MsgType("BY"));
            }

            public ApplicationMessageReport(
                    QuickFix.Fields.ApplReportID aApplReportID,
                    QuickFix.Fields.ApplReportType aApplReportType,
                    QuickFix.Fields.ApplReqType aApplReqType,
                    QuickFix.Fields.ApplReqID aApplReqID,
                    QuickFix.Fields.ApplRespID aApplRespID
                ) : this()
            {
                this.ApplReportID = aApplReportID;
                this.ApplReportType = aApplReportType;
                this.ApplReqType = aApplReqType;
                this.ApplReqID = aApplReqID;
                this.ApplRespID = aApplRespID;
            }

            public QuickFix.Fields.ApplReportID ApplReportID
            { 
                get 
                {
                    QuickFix.Fields.ApplReportID val = new QuickFix.Fields.ApplReportID();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ApplReportID val) 
            { 
                this.ApplReportID = val;
            }
            
            public QuickFix.Fields.ApplReportID Get(QuickFix.Fields.ApplReportID val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ApplReportID val) 
            { 
                return IsSetApplReportID();
            }
            
            public bool IsSetApplReportID() 
            { 
                return IsSetField(Tags.ApplReportID);
            }
            public QuickFix.Fields.ApplReportType ApplReportType
            { 
                get 
                {
                    QuickFix.Fields.ApplReportType val = new QuickFix.Fields.ApplReportType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ApplReportType val) 
            { 
                this.ApplReportType = val;
            }
            
            public QuickFix.Fields.ApplReportType Get(QuickFix.Fields.ApplReportType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ApplReportType val) 
            { 
                return IsSetApplReportType();
            }
            
            public bool IsSetApplReportType() 
            { 
                return IsSetField(Tags.ApplReportType);
            }
            public QuickFix.Fields.ApplReqType ApplReqType
            { 
                get 
                {
                    QuickFix.Fields.ApplReqType val = new QuickFix.Fields.ApplReqType();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.ApplReqType val) 
            { 
                this.ApplReqType = val;
            }
            
            public QuickFix.Fields.ApplReqType Get(QuickFix.Fields.ApplReqType val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.ApplReqType val) 
            { 
                return IsSetApplReqType();
            }
            
            public bool IsSetApplReqType() 
            { 
                return IsSetField(Tags.ApplReqType);
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
            public QuickFix.Fields.NoApplIDs NoApplIDs
            { 
                get 
                {
                    QuickFix.Fields.NoApplIDs val = new QuickFix.Fields.NoApplIDs();
                    GetField(val);
                    return val;
                }
                set { SetField(value); }
            }
            
            public void Set(QuickFix.Fields.NoApplIDs val) 
            { 
                this.NoApplIDs = val;
            }
            
            public QuickFix.Fields.NoApplIDs Get(QuickFix.Fields.NoApplIDs val) 
            { 
                GetField(val);
                return val;
            }
            
            public bool IsSet(QuickFix.Fields.NoApplIDs val) 
            { 
                return IsSetNoApplIDs();
            }
            
            public bool IsSetNoApplIDs() 
            { 
                return IsSetField(Tags.NoApplIDs);
            }
            public class NoApplIDsGroup : Group
            {
                public static int[] fieldOrder = {Tags.RefApplID, Tags.RefApplLastSeqNum, Tags.ApplRespError, 0};
            
                public NoApplIDsGroup() 
                  :base( Tags.NoApplIDs, Tags.RefApplID, fieldOrder)
                {
                }
            
                public override Group Clone()
                {
                    var clone = new NoApplIDsGroup();
                    clone.CopyStateFrom(this);
                    return clone;
                }
            
                            public QuickFix.Fields.RefApplID RefApplID
                { 
                    get 
                    {
                        QuickFix.Fields.RefApplID val = new QuickFix.Fields.RefApplID();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.RefApplID val) 
                { 
                    this.RefApplID = val;
                }
                
                public QuickFix.Fields.RefApplID Get(QuickFix.Fields.RefApplID val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.RefApplID val) 
                { 
                    return IsSetRefApplID();
                }
                
                public bool IsSetRefApplID() 
                { 
                    return IsSetField(Tags.RefApplID);
                }
                public QuickFix.Fields.RefApplLastSeqNum RefApplLastSeqNum
                { 
                    get 
                    {
                        QuickFix.Fields.RefApplLastSeqNum val = new QuickFix.Fields.RefApplLastSeqNum();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.RefApplLastSeqNum val) 
                { 
                    this.RefApplLastSeqNum = val;
                }
                
                public QuickFix.Fields.RefApplLastSeqNum Get(QuickFix.Fields.RefApplLastSeqNum val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.RefApplLastSeqNum val) 
                { 
                    return IsSetRefApplLastSeqNum();
                }
                
                public bool IsSetRefApplLastSeqNum() 
                { 
                    return IsSetField(Tags.RefApplLastSeqNum);
                }
                public QuickFix.Fields.ApplRespError ApplRespError
                { 
                    get 
                    {
                        QuickFix.Fields.ApplRespError val = new QuickFix.Fields.ApplRespError();
                        GetField(val);
                        return val;
                    }
                    set { SetField(value); }
                }
                
                public void Set(QuickFix.Fields.ApplRespError val) 
                { 
                    this.ApplRespError = val;
                }
                
                public QuickFix.Fields.ApplRespError Get(QuickFix.Fields.ApplRespError val) 
                { 
                    GetField(val);
                    return val;
                }
                
                public bool IsSet(QuickFix.Fields.ApplRespError val) 
                { 
                    return IsSetApplRespError();
                }
                
                public bool IsSetApplRespError() 
                { 
                    return IsSetField(Tags.ApplRespError);
                }
            
            }
        }
    }
}
