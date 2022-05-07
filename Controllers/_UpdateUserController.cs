using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspisPipe.Controllers
{
    public class _UpdateUserController : InspisPipe.Controllers.BaseApiController
    {
        // GET: _UpdateUser
        public bool Get(string apikey,string login, string email, bool isactual)
        {
            bas.VerifyApiKey(apikey);

            return basMemberShip.UpdateUser(login, email, isactual);
        }
    }
}