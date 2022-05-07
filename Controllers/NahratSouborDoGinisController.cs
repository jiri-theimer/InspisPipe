using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace InspisPipe.Controllers
{
    public class NahratSouborDoGinisController : InspisPipe.Controllers.BaseApiController
    {
        // GET: NahratSouborDoGinis
        public string Get(string login,string pid,string tempfilename,string typvazby, string popissouboru)
        {
            this.InhaleLogin(login);

            string tempfullpath = @ConfigurationManager.AppSettings["TempFolder"] + "\\" + tempfilename;

            return _gin.NahratSouborDoGinis(pid, tempfullpath, typvazby, popissouboru);


        }
    }
}