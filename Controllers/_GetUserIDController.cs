using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InspisPipe.Controllers
{
    public class _GetUserIDController : InspisPipe.Controllers.BaseApiController
    {
        

        // GET api/<controller>/5
        public string Get(string apikey,string login)
        {
            bas.VerifyApiKey(apikey);

            return basMemberShip.GetUserID(login);
        }

        
    }
}