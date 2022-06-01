using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    public class a03Institution
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
        public string a03Name { get; set; }

        [DataMember]
        public a06KeyEnum a06ID { get; set; }

        [DataMember]
        public int a05ID { get; set; }

        [DataMember]
        public int a09ID { get; set; }

        [DataMember]
        public int a21ID { get; set; }

        [DataMember]
        public int a03ID_Founder { get; set; }

        [DataMember]
        public string a03REDIZO { get; set; }

        [DataMember]
        public string a03ICO { get; set; }

        [DataMember]
        public string a03FounderCode { get; set; }

        [DataMember]
        public string a03City { get; set; }

        [DataMember]
        public string a03Street { get; set; }

        [DataMember]
        public string a03PostCode { get; set; }
    }

    [DataContract]
    public enum a06KeyEnum
    {
        [EnumMember]
        NotSpecified = 0,
        [EnumMember]
        Skola = 1,
        [EnumMember]
        Zrizovatel = 2,
    }
}
