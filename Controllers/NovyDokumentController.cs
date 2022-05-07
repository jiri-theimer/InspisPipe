using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspisPipe.Controllers
{
    public class NovyDokumentController : InspisPipe.Controllers.BaseApiController
    {
        // GET: NovyDokument
        public string Get(string login, string typdokumentu, string pid_spis, string vec)
        {
            //vrací PID souboru dokumentu

            this.InhaleLogin(login);

            return _gin.NovyDokument(typdokumentu, pid_spis, vec);


        }
    }
}