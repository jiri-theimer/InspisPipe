using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;

namespace InspisPipe.Controllers
{
    public class BaseApiController : ApiController
    {
        protected DateTime _d1 { get; set; }
        protected DateTime _d2 { get; set; }
        protected GinisHelper _gin { get; set; }

        public BaseApiController()
        {
            _d1 = new DateTime(2010, 1, 1);
            _d2 = new DateTime(DateTime.Now.Year + 1, 1, 1);
            
        }

        protected void InhaleLogin(string login)
        {
            _gin = new GinisHelper(login);
        }
    }
}