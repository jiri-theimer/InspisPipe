using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspisPipe.Controllers
{
    public class SeznamDokumentuController : InspisPipe.Controllers.BaseApiController
    {
        
        public List<GinisDocument> Get(string login, bool Spis)
        {
            this.InhaleLogin(login);

            return _gin.SeznamDokumentu(_d1, _d2,Spis);

            
        }
    }
}