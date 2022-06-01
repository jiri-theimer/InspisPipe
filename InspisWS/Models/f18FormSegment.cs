using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    public class f18FormSegment
    {
        [DataMember]
        public int PID { get; set; }

        [DataMember]
        public int f06ID { get; set; }

        [DataMember]
        public int f18ParentID { get; set; }

        [DataMember]
        public string f18Name { get; set; }

        [DataMember]
        public string f18Text { get; set; }

        [DataMember]
        public string f18SupportingText { get; set; }

        [DataMember]
        public string f18Description { get; set; }

        [DataMember]
        public int f18Ordinal { get; set; }
    }
}
