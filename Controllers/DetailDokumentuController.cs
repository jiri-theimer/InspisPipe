using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspisPipe.Controllers
{
    public class DetailDokumentuController : InspisPipe.Controllers.BaseApiController
    {
        // GET: DetailDokumentu
        public GinisDocument Get(string login, string pid)
        {
            this.InhaleLogin(login);

            return _gin.DetailDokumentu(pid);

            


        }
    }
}