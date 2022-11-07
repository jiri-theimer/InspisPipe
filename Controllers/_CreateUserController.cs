using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace InspisPipe.Controllers
{
    public class _CreateUserController : InspisPipe.Controllers.BaseApiController
    {
        // GET api/<controller>
        public string Get(string apikey,string login, string email, string password)
        {
            

            //založí membership uživatele
            bas.VerifyApiKey(apikey);

            basMemberShip.CreateUser(login, email, password);

            return login;





        }
    }
}