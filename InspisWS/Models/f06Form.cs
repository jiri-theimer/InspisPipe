using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace InspisWS
{
    public enum UserLockFlagEnum
    {
        NoLockOffer = 1,
        LockOnlyIfValid = 2,
        LockWhenever = 3,
        NotSpecified = 0
    }

    [Serializable]
    [DataContract]
    public class f06Form
    {
        [DataMember]
        public int PID { get; set; }
        
        [DataMember]
        public string f06Name { get; set; }

        [DataMember]
        public string f06Description { get; set; }

        [DataMember]
        public string f06Hint { get; set; }

        [DataMember]
        public UserLockFlagEnum f06UserLockFlag { get; set; }

        [DataMember]
        public bool f06IsA01PeriodStrict { get; set; }

        [DataMember]
        public bool f06IsA01ClosedStrict { get; set; }

        [DataMember]
        public bool IsClosed { get; set; }
    }
}
