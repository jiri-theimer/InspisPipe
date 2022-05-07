using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspisPipe.Controllers
{
    public class SeznamDokumentuSpisuController : InspisPipe.Controllers.BaseApiController
    {
        // GET: SeznamDokumentuSpisu
        public List<GinisDocument> Get(string login, string pid_spis)
        {
            this.InhaleLogin(login);

            return _gin.SeznamDokumentuSpisu(_d1, _d2, pid_spis);


        }
    }
}