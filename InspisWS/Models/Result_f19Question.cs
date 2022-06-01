using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    public class Result_f19Question : Result
    {
        [DataMember]
        public List<f19Question> Data { get; set; }
    }
}
