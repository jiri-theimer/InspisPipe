using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspisPipe.Controllers
{
    public class _RecoveryPasswordController : InspisPipe.Controllers.BaseApiController
    {
        // GET: _RecoveryPassword
        public string Get(string apikey,string login, string explicitpassword = null)
        {
            

            bas.VerifyApiKey(apikey);
            
            return basMemberShip.RecoveryPassword(login, explicitpassword);

         
        }
    }
}