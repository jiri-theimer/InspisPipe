using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspisPipe.Models
{
    public class j02Person
    {
        public int j02ID { get; set; }
        public string j02Email { get; set; }
        public string j02FirstName { get; set; }
        public string j02LastName { get; set; }
        public DateTime j02ValidFrom { get; set; }
        public DateTime j02ValidUntil { get; set; }
        public bool isclosed { get; set; }
        public string j02Guid { get; set; }
    }
}