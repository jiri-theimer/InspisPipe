using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    public class f21ReplyUnit
    {
        [DataMember]
        public int PID { get; set; }

        [DataMember]
        public int f19ID { get; set; }

        [DataMember]
        public int f18ID { get; set; }

        [DataMember]
        public string f21Name { get; set; }

        [DataMember]
        public string f21Description { get; set; }

        [DataMember]
        public string f21MinValue { get; set; }

        [DataMember]
        public string f21MaxValue { get; set; }

        [DataMember]
        public bool f21IsCommentAllowed { get; set; }

        [DataMember]
        public int f21Ordinal { get; set; }
    }
}
