using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    public class Result_Attachments : Result
    {
        [DataMember]
        public List<BinaryAttachment> Attachments { get; set; }
    }
}