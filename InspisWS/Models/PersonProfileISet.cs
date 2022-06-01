using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    public class PersonProfileISet : PersonProfile
    {
        [DataMember]
        public bool ActiveInISet { get; set; }
    }
}