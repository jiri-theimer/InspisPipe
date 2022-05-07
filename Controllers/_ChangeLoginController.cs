using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace InspisPipe.Controllers
{
    public class _ChangeLoginController : InspisPipe.Controllers.BaseApiController
    {
        // GET: _ChangeLogin
        public bool Get(string apikey,string userid, string newlogin)
        {
            bas.VerifyApiKey(apikey);

            
            var db = new DbHandler(DbEnum.MembershipDb);

            return db.RunSql($"exec dbo.UpdateUserName '/','{userid}','{newlogin}'");



        }
    }
}