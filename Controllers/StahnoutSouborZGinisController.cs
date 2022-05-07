using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspisPipe.Controllers
{
    public class StahnoutSouborZGinisController : InspisPipe.Controllers.BaseApiController
    {
        // GET: StahnoutSouborZGinis
        //výchozí typvazby: elektronicky-obraz
        public GinisFile Get(string login, string pid_dokument, string pid_soubor, string typvazby)
        {
            this.InhaleLogin(login);

            return _gin.StahnoutSouborZGinis(pid_dokument, pid_soubor, typvazby);

            


        }
    }
}