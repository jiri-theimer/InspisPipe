using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using InspisPipe.Models;

namespace InspisPipe.Controllers
{
    public class LoginController : BaseController
    {               

        public ActionResult Index(string returnurl)
        {
            //if (HttpContext.Request.IsAuthenticated && !string.IsNullOrEmpty(HttpContext.User.Identity.Name))
            //{
            //    //existuje přihlášení do membership: vyčistit
            //    var db = new DbHandler(DbEnum.PrimaryDb);
            //    db.RunSql($"UPDATE p85TempBox SET p85ValidUntil=GETDATE() WHERE p85ValidUntil>GETDATE() AND p85Prefix='sso2core' AND p85UserInsert LIKE '{HttpContext.User.Identity.Name}'");

            //}
            
            
            FormsAuthentication.SignOut();  //definitivní odhlášení
            var v = new LoginViewModel() { SourceUrl = returnurl };

            v.ShowInfoMessasge("Zadejte svoje přihlašovací údaje nebo vyberte požadovanou akci z pravého menu této stránky.");

            RefreshStateIndex(v);
            return View(v);
        }

        [HttpPost]       
        public ActionResult Index(LoginViewModel v)
        {
            RefreshStateIndex(v);
            var capi = new _ValidateUserController();
            if (capi.Get("cesta-na-přímo-bez-klíče!", v.UserName, v.Password))
            {
                if (string.IsNullOrEmpty(v.SourceUrl))
                {
                    FormsAuthentication.RedirectFromLoginPage(v.UserName, true);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(v.UserName, true);
                    Response.Redirect(v.SourceUrl, true);
                }
                
                
            }
            else
            {
                v.ShowErrorMessasge("Špatné přihlašovací jméno nebo heslo!");
            }
            
           
            return View(v);
        }

        private void RefreshStateIndex(LoginViewModel v)
        {
            v.AddMainButton("InspIS SET", basConfig.Url_SET,"Systém elektronického testování");
            v.AddMainButton("InspIS PORTÁL", basConfig.Url_PORTAL, "Portál informací o školách");
            v.AddMainButton("InspIS E-LEARNING", basConfig.Url_ELEARNING, "Platforma pro vzdělávání");
            v.AddMainButton("Zapomenuté heslo", Url.Action("Recovery", "Password"));
            v.AddMainButton("Vytvořit nový účet", Url.Action("Index", "Createuser"));
            
            

        }



        public string DetectOnlineUser()        //Metodu používá aplikace TĚLOCVIK!!!!!
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                return HttpContext.User.Identity.Name;
            }


            return null;
        }


    }
}