using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    public class a01Event
    {
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime DateInsert { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime DateUpdate { get; set; }

        [DataMember]
        public string UserInsert { get; set; }

        [DataMember]
        public string UserUpdate { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime ValidFrom { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime ValidUntil { get; set; }

        [DataMember]
        public bool IsClosed { get; set; }

        [DataMember]
        public int PID { get; set; }

        [DataMember]
        public int a03ID { get; set; }

        [DataMember]
        public int a10ID { get; set; }
        
        [DataMember]
        public int a08ID { get; set; }
        
        [DataMember]
        public int b02ID { get; set; }
        
        [DataMember]
        public int j03ID_Creator { get; set; }
        
        [DataMember]
        public int j02ID_Issuer { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime? a01DateFrom { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime? a01DateUntil { get; set; }

        [DataMember]
        public string b02Name { get; set; }

        [DataMember]
        public string a01Signature { get; set; }
    }
}
