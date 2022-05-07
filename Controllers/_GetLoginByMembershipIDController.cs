using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspisPipe.Controllers
{
    public class _GetLoginByMembershipIDController : InspisPipe.Controllers.BaseApiController
    {
        // GET: GetLoginByMembershipID
        public string Get(string apikey,string membershipid)
        {
            bas.VerifyApiKey(apikey);

            return basMemberShip.GetLoginByMembershipID(membershipid);
        }
    }
}