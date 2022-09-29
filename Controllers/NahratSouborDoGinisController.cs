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
            bas.LogInfo("Před InhaleLogin | typvazby: " + typvazby + ", popissouboru: " + popissouboru + ", pid: " + pid + ", login: " + login);


            this.InhaleLogin(login);

            string tempfullpath = @ConfigurationManager.AppSettings["TempFolder"] + "\\" + tempfilename;

            bas.LogInfo("tempfullpath: " + tempfullpath+ ", typvazby: "+ typvazby+ ", popissouboru: "+ popissouboru+ ", pid: "+ pid+ ", login: "+ login);

            return _gin.NahratSouborDoGinis(pid, tempfullpath, typvazby, popissouboru);


        }
    }
}