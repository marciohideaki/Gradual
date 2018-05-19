using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace CommServerProviderSample
{
    [ProtoContract]
    [Serializable]
    public class SimpleProviderPublishing
    {
        [ProtoMember(1)]
        public string Instrumento { get; set; }

        [ProtoMember(2)]
        public DateTime TimeStampSinal { get; set; }

        [ProtoMember(3)]
        public string SinalDescription { get; set; }
    }
}
