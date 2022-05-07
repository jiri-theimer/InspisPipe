using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspisPipe.Models
{
    public class j03User
    {
        public int j03ID { get; set; }
        public string j03Login { get; set; }
        public int j02ID { get; set; }
        public int j04ID { get; set; }
        public string j03MembershipUserId { get; set; }
        public bool j03IsDomainAccount { get; set; }
        public bool j03IsSystemAccount { get; set; }
        public DateTime j03ValidFrom { get; set; }
        public DateTime j03ValidUntil { get; set; }
        public bool isclosed { get; set; }

        public string j03Guid { get; set; }
    }
}