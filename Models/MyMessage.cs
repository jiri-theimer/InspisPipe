using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspisPipe.Models
{
    public class MyMessage
    {
        public string MessageText { get; set; }
        public string MessageHeader { get; set; }
        public string MessageStyle { get; set; } = "danger";
    }
}