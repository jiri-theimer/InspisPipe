using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspisPipe.Controllers
{
    public class GetPidSpisuFromZnackaController : InspisPipe.Controllers.BaseApiController
    {
        // GET: GetPidSpisuFromZnacka
            public string Get(string login, string znacka)
            {
                this.InhaleLogin(login);

                var lis = _gin.NajdiSpisPodleZnacky(_d1, _d2, znacka);

                if (lis.Count() > 0)
                {
                    return lis.First().IdSpisu;
                }

                return null;


            }
    }
}