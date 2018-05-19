using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace CommServerProviderSample
{
    [ProtoContract]
    [Serializable]
    public class SimpleProviderSubscriptionRequest
    {
        [ProtoMember(1)]
        public string Instrumento { get; set; }
    }
}
