using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    public class Result_GetForm : Result
    {
        [DataMember]
        public f06Form Form { get; set; }

        [DataMember]
        public List<f18FormSegment> Segments { get; set; }

        [DataMember]
        public List<f19Question> Questions { get; set; }

        [DataMember]
        public List<f21ReplyUnit> ReplyUnits { get; set; }

        [DataMember]
        public List<f32FilledValue> FilledValues { get; set; }
    }
}
