using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    public class Result_PragoDataTask : Result
    {
        [DataMember]
        public List<PragoDataTask> Data { get; set; }
    }
}