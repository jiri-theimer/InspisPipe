using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    public class f32FilledValue
    {
        [DataMember]
        public int PID { get; set; }

        [DataMember]
        public int f19ID { get; set; }

        [DataMember]
        public int f21ID { get; set; }

        [DataMember]
        public string f32Comment { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}
