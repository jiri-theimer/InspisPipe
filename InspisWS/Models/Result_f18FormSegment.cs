using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    public class Result_f18FormSegment : Result
    {
        [DataMember]
        public List<f18FormSegment> Data { get; set; }
    }
}
