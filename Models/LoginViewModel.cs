using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspisPipe.Models
{
    public class LoginViewModel:BaseViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SourceUrl { get; set; }


        public string Browser_UserAgent { get; set; }
        public int Browser_AvailWidth { get; set; }
        public int Browser_AvailHeight { get; set; }
        public int Browser_InnerWidth { get; set; }
        public int Browser_InnerHeight { get; set; }
        public string Browser_DeviceType { get; set; }
        public string Browser_Host { get; set; }
    }
}