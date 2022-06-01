using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    public class f19Question
    {
        [DataMember]
        public int PID { get; set; }

        [DataMember]
        public int f18ID { get; set; }

        [DataMember]
        public int f23ID { get; set; }

        [DataMember]
        public int x24ID { get; set; }

        [DataMember]
        public int f19Ordinal { get; set; }

        [DataMember]
        public string f19Name { get; set; }

        [DataMember]
        public string f19SupportingText { get; set; }

        [DataMember]
        public string f19Hint { get; set; }

        [DataMember]
        public bool f19IsMultiselect { get; set; }

        [DataMember]
        public bool f19IsRequired { get; set; }

        [DataMember]
        public string TextBox_MinValue { get; set; }

        [DataMember]
        public string TextBox_MaxValue { get; set; }

        [DataMember]
        public string f19LinkURL { get; set; }

        [DataMember]
        public int f19CHLMaxAnswers { get; set; }
    }
}
