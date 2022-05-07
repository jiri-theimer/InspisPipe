using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InspisPipe.Controllers
{
    public class _RecoveryAccountController : InspisPipe.Controllers.BaseApiController
    {
        

        // GET api/<controller>/5
        public int Get(string apikey,string login, string email, string newpassword)  //vrací: 1=účet je v pořádku, 2=založen nový účet, 0=chyba
        {
            bas.VerifyApiKey(apikey);

          
            return basMemberShip.RecoveryAccount(login, email, newpassword);

           
        }

       
    }
}