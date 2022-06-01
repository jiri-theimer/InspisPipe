using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(PersonProfileISet))]
    public class PersonProfile
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
        public DateTime j03ValidFrom { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime j03ValidUntil { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime j02ValidFrom { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime j02ValidUntil { get; set; }

        [DataMember]
        public int j03ID { get; set; }

        [DataMember]
        public int j02ID { get; set; }

        [DataMember]
        public int j04ID { get; set; }

        [DataMember]
        public int a04ID { get; set; }

        [DataMember]
        public string j03Login { get; set; }

        [DataMember]
        public string j02Email { get; set; }

        [DataMember]
        public bool j03IsDomainAccount { get; set; }

        [DataMember]
        public string j02PID { get; set; }
        
        [DataMember]
        public string j03MembershipUserId { get; set; }

        [DataMember]
        public string j02TitleBeforeName { get; set; }

        [DataMember]
        public string j02FirstName { get; set; }

        [DataMember]
        public string j02LastName { get; set; }

        [DataMember]
        public string j02TitleAfterName { get; set; }

        [DataMember]
        public string j02IsInvitedPerson { get; set; }
    }
}
