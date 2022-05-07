using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspisPipe.Controllers
{
    public class SeznamSouboruController : InspisPipe.Controllers.BaseApiController
    {
        // GET: SeznamSouboru
        public List<GinisFile> Get(string login, string pid_dokument)
        {
            this.InhaleLogin(login);

            return _gin.SeznamSouboru(pid_dokument);


        }
    }
}