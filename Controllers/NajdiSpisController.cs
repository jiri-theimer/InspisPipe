using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InspisPipe.Controllers
{
    public class NajdiSpisController : InspisPipe.Controllers.BaseApiController
    {
        
        


        // GET api/<controller>

        // GET api/<controller>/5
        public List<GinisDocument> Get(string login,string pid)
        {
            //pid je třeba zjistit z čísla jednacího, metody: GetPidDokumentuFromCJ

            this.InhaleLogin(login);

            return _gin.NajdiSpis(_d1, _d2, pid);

            //var lis = new List<GinisDocument>();
            //lis.Add(new GinisDocument() { IdDokumentu = "1", IdSpisu = "EEE" });
            //lis.Add(new GinisDocument() { IdDokumentu = "2", IdSpisu = "WERQEWQ" });
            //return lis;
        }

        
    }
}