using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    public class Result_f06Form : Result
    {
        [DataMember]
        public f06Form Data { get; set; }
    }
}
