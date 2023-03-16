using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspisPipe.Controllers
{
    public class DatovkaController : InspisPipe.Controllers.BaseApiController
    {
        // GET: Datovka
        public string Get(string login)
        {
            this.InhaleLogin(login);

            //string strID = _gin.NajdiEsuIco("25722034");
            return _gin.OdeslatDatovku("CSI0X00ADYIQ", "CSI0SE02IO9F", "df6w7m5");        //pro produkční server

            //return _gin.OdeslatDatovku("CSI0X009BYCR", "CSI0SE02EGNH", "df6w7m5");    //pro test server
        }
    }
}