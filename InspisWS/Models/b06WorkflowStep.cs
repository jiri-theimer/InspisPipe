using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    public class b06WorkflowStep
    {
        [DataMember]
        public int PID { get; set; }

        [DataMember]
        public string b06Name { get; set; }
    }
}
