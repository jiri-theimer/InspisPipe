﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace InspisWS
{
    [Serializable]
    [DataContract]
    public class Result_f32FilledValue : Result
    {
        [DataMember]
        public List<f32FilledValue> Data { get; set; }
    }
}
