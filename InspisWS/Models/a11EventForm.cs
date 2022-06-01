using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    public class a11EventForm
    {
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime DateInsert { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime DateUpdate { get; set; }

        [DataMember]
        public string UserInsert { get; set; }

        [DataMember]
        public string UserUpdate { get; set; }

        [DataMember]
        public int PID { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime ValidFrom { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime ValidUntil { get; set; }

        [DataMember]
        public bool IsClosed { get; set; }

        [DataMember]
        public int a01ID { get; set; }

        [DataMember]
        public int f06ID { get; set; }

        [DataMember]
        public bool a11IsInProcessing { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime? a11ProcessingStart { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime? a11ProcessingLast { get; set; }
    }
}
