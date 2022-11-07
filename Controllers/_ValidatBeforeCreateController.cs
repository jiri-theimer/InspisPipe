using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspisPipe.Controllers
{
    public class _ValidateBeforeCreateController : InspisPipe.Controllers.BaseApiController
    {
        // GET: _ValidatBeforeCreate
        public string Get(string apikey,string login, string password,string verify)
        {
            bas.VerifyApiKey(apikey);
            
            if (basMemberShip.ValidatBeforeCreate(login, password, verify))
            {
                return login;
            }
            else
            {
                return basMemberShip.ErrorMessage;
            }

            
        }
    }
}