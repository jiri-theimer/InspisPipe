using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspisPipe.Controllers
{
    public class SeznamTypuDokumentuController : InspisPipe.Controllers.BaseApiController
    {
        // GET: SeznamTypuDokumentu
        public List<GinisDocumentType> Get(string login)
        {
            this.InhaleLogin(login);

            return _gin.SeznamTypuDokumentu();


        }
    }
}