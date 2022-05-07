using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspisPipe.Models
{
    public class Sso2MembViewModel
    {
        public string Login { get; set; }
        public string Message { get; set; }
        public string DestUrl { get; set; }

        public string MembershipUser { get; set; }
        public string WinUser { get; set; }

        public string HelpUrl { get; set; }
    }
}