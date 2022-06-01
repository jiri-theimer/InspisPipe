using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    public class Result_f21ReplyUnit : Result
    {
        [DataMember]
        public List<f21ReplyUnit> Data { get; set; }
    }
}
