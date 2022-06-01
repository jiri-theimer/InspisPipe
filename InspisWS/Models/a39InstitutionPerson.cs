﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    public class a39InstitutionPerson
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
        public int a03ID { get; set; }

        [DataMember]
        public int j02ID { get; set; }

        [DataMember]
        public int j04ID_Explicit { get; set; }
    }
}
