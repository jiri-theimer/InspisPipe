using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace InspisPipe.Controllers
{
    public class SsoController : Controller
    {
        // GET: Sso
        public ActionResult Index()
        {
            if (HttpContext.Request.LogonUserIdentity.IsAuthenticated)
            {
                ViewBag.WinUser = HttpContext.Request.LogonUserIdentity.Name;
            }
            else
            {
                ViewBag.WinUser = "----";
            }

            ViewBag.UserDomainName = Environment.UserDomainName;


            



            if (HttpContext.Request.IsAuthenticated)
            {
                ViewBag.MembershipUser = HttpContext.User.Identity.Name;
            }
            else
            {
                ViewBag.MembershipUser = "----";
            }
            

            return View();
        }

       
    }
}