﻿using System;
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
    }
}