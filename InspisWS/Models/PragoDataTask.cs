using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    public class PragoDataTask
    {
        [DataMember]
        public int TaskID { get; set; }

        [DataMember]
        public string UserID { get; set; }

        [DataMember]
        public string RoleID { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime CreateDT { get; set; }
    }
}