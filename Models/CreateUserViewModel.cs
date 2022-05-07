using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspisPipe.Models
{
    public class CreateUserViewModel:BaseViewModel
    {
        public string EmailAddress { get; set; }
        public string VerifyEmail { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public CaptchaSupport captcha { get; set; }
        public string CaptchaAnswer { get; set; }
        public string LastCaptchaFormulaHashed { get; set; }

        public bool IsFinished { get; set; }
    }
}