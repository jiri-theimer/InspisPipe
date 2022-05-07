using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace InspisPipe.Controllers
{
    public class GetPidDokumentuFromCJController : InspisPipe.Controllers.BaseApiController
    {
        // GET: GetPidDokumentuFromCJ
        public string Get(string login, string cislojednaci)
        {
            this.InhaleLogin(login);
            var cj = new GinisCJ(cislojednaci);

            var lis = _gin.NajdiDokumentPodleCj(_d1, _d2,cj.DenikCJ,cj.RokCJ,cj.PoradoveCisloCJ);

            if (lis.Count() > 0)
            {
                return lis[0].IdDokumentu;
            }
            

            return null;


        }
    }
}