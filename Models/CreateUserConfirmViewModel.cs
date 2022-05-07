using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspisPipe.Models
{
    public class CreateUserConfirmViewModel:BaseViewModel
    {
        public j03User RecJ03 { get; set; }
        public j02Person RecJ02 { get; set; }
        public bool IsNotPossible { get; set; }
    }
}