using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspisPipe.Controllers
{
    public class _DecryptMembershipCookieController : InspisPipe.Controllers.BaseApiController
    {        
        public string Get(string expr)
        {
            
            var cc = System.Web.Security.FormsAuthentication.Decrypt(expr);
           
            return cc.Name;
        }
    }
}