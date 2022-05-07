using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.DirectoryServices.AccountManagement;


namespace InspisPipe.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";


            


            return View();
        }


        public ActionResult Environment()
        {
            return View();
        }

        public ActionResult AD()
        {
            ViewBag.Message = null;

            try
            {


                if (UserPrincipal.Current != null)
                {
                    ViewBag.UserPrincipalName = UserPrincipal.Current.UserPrincipalName;
                    ViewBag.EmailAddress = UserPrincipal.Current.EmailAddress;
                    ViewBag.DisplayName = UserPrincipal.Current.DisplayName;
                }
                
                ViewBag.IdentityName = User.Identity.Name;

            }
            catch(Exception e)
            {
                ViewBag.Message1 = "ERROR UserPrincipal: " + e.Message;
            }

            
            



            return View();
        }

        [HttpPost]
        public ActionResult AD(Models.LoginViewModel v)
        {

            var configAD = new System.Collections.Specialized.NameValueCollection();
            configAD.Add("name", "AspNetActiveDirectoryMembershipProvider");
            configAD.Add("connectionStringName", "ADConnString");
            configAD.Add("connectionUsername", "ADQueryUser");
            configAD.Add("connectionPassword", "Password123!");
            configAD.Add("enableSearchMethods", "true");
            configAD.Add("attributeMapUsername", "sAMAccountName");
            configAD.Add("applicationName", "/");

            var providerAD = new System.Web.Security.ActiveDirectoryMembershipProvider();
            try
            {
                providerAD.Initialize("ADProvider", configAD);
                if (providerAD.ValidateUser(v.UserName, v.Password))
                {
                    ViewBag.Message2 = "ADProvider info: Ověření uživatele v doméně je v pořádku.";
                }
                else
                {
                    ViewBag.Message2 = "ADProvider info: Heslo a Login se nepodařilo ověřit.";
                }
                
            }
            catch (Exception e)
            {
                ViewBag.Message2 = "ERROR ADProvider: " + e.Message;
            }

            return View(v);
        }

    }

   
}
