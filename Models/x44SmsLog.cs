using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspisPipe.Models
{
    public enum x44StateFlag
    {
        NotSpecified = 0,
        InQueque = 1,
        Error = 2,
        Proceeded = 3,
        Stopped = 4
    }

    public class x44SmsLog
    {
        public int x44ID { get; set; }
        public int j02ID { get; set; }

        public string x44Number { get; set; }
        public string x44Body { get; set; }
        public x44StateFlag x44Status { get; set; }
        public bool x44IsProcessed { get; set; }
        public int x29ID { get; set; }
        public int x44DataPID { get; set; }
        public DateTime? x44DatetimeProcessed { get; set; }
        public string x44ErrorMessage { get; set; }
        public string x44MessageGuid { get; set; }
        public string x44Result { get; set; }
    }
}