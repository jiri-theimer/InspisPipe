using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspisPipe.Models
{
    public class PasswordRecoveryViewModel:BaseViewModel
    {
        public string LoginEmail { get; set; }
        public bool IsNotPossible { get; set; }
    }
}