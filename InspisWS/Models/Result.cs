using System;
using System.Runtime.Serialization;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(f06Form))]
    [KnownType(typeof(Result_f18FormSegment))]
    [KnownType(typeof(Result_f19Question))]
    [KnownType(typeof(Result_f21ReplyUnit))]
    [KnownType(typeof(Result_GetForm))]
    [KnownType(typeof(Result_PragoDataTask))]
    public class Result
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}
