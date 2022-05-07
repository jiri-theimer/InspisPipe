using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspisPipe.Controllers
{
    public class _DeleteUserController : InspisPipe.Controllers.BaseApiController
    {
        // GET: _DeleteUser
        public bool Get(string apikey,string login)
        {
            bas.VerifyApiKey(apikey);

            return basMemberShip.DeleteUser(login);
        }
    }
}