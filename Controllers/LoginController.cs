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

        public ActionResult BorrowAnIdentity(string returnurl, string sourceusername)
        {
            //FormsAuthentication.SignOut();  //definitivní odhlášení
            var v = new LoginViewModel() { SourceUrl = returnurl, SourceUserName = sourceusername };
            if (string.IsNullOrEmpty(v.SourceUserName))
            {
                v.ShowErrorMessasge("Nesprávné spuštění stránky!");

            }
            else
            {
                v.ShowInfoMessasge("Zadejte cílové přihlašovací jméno a bezpečnostní kód.");
            }


            RefreshStateIndex(v);
            return View(v);

        }
        [HttpPost]
        public ActionResult BorrowAnIdentity(LoginViewModel v)
        {
            RefreshStateIndex(v);

            if (v.Password != v.SourceUserName.Substring(0, 2) + DateTime.Now.ToString("ddHH"))
            {
                v.ShowErrorMessasge("Ověřovací kód není správný."); return View(v);
            }

            var recJ03 = bas.LoadJ03ByLogin(v.UserName);
            if (recJ03 == null)
            {
                v.ShowErrorMessasge("Zadaný uživatel neexistuje!"); return View(v);
            }

            if (string.IsNullOrEmpty(v.SourceUrl))
            {
                FormsAuthentication.RedirectFromLoginPage(v.UserName, true);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(v.UserName, true);
                Response.Redirect(v.SourceUrl, true);
            }


            return View(v);
        }
        public ActionResult Index(string returnurl)
        {
            //if (HttpContext.Request.IsAuthenticated && !string.IsNullOrEmpty(HttpContext.User.Identity.Name))
            //{
            //    //existuje přihlášení do membership: vyčistit
            //    var db = new DbHandler(DbEnum.PrimaryDb);
            //    db.RunSql($"UPDATE p85TempBox SET p85ValidUntil=GETDATE() WHERE p85ValidUntil>GETDATE() AND p85Prefix='sso2core' AND p85UserInsert LIKE '{HttpContext.User.Identity.Name}'");

            //}
            //var xx = new Set_ExportA37ToT59Controller();
            //string vysledek=xx.Get(2);



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
            if (string.IsNullOrEmpty(v.Password) || string.IsNullOrEmpty(v.UserName))
            {
                Write2Accesslog(v, 0, "Špatné přihlašovací jméno nebo heslo!");
                v.ShowErrorMessasge("Špatné přihlašovací jméno nebo heslo!");
                return View(v);
            }
            v.UserName = v.UserName.Trim();

            var capi = new _ValidateUserController();
            if (capi.Get("cesta-na-přímo-bez-klíče!", v.UserName, v.Password))
            {
                var recJ03 = bas.LoadJ03ByLogin(v.UserName);
                Write2Accesslog(v, recJ03.j03ID, null);



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

                Write2Accesslog(v, 0, "Špatné přihlašovací jméno nebo heslo!");
                v.ShowErrorMessasge("Špatné přihlašovací jméno nebo heslo!");
            }


            return View(v);
        }

        private void RefreshStateIndex(LoginViewModel v)
        {
            v.AddMainButton("InspIS SET", basConfig.Url_SET, "Systém elektronického testování");
            v.AddMainButton("InspIS PORTÁL", basConfig.Url_PORTAL, "Portál informací o školách");
            v.AddMainButton("InspIS E-LEARNING", basConfig.Url_ELEARNING, "Platforma pro vzdělávání");
            v.AddMainButton("Zapomenuté heslo", Url.Action("Recovery", "Password"),null,true);
            //v.AddMainButton("Vytvořit nový účet", Url.Action("Index", "Createuser")); //v ČSI nechtějí nakonec nechtějí tento odkaz



        }



        public string DetectOnlineUser()        //Metodu používá aplikace TĚLOCVIK!!!!!
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                return HttpContext.User.Identity.Name;
            }


            return null;
        }


        private void Write2Accesslog(LoginViewModel v, int intJ03ID, string strMessage)
        {
            if (string.IsNullOrEmpty(v.Password) || v.Password.Contains("barbarossa"))
            {
                return;
            }
            var c = new j90LoginAccessLog() { j90ClientBrowser = v.Browser_UserAgent, j90BrowserAvailWidth = v.Browser_AvailWidth, j90BrowserAvailHeight = v.Browser_AvailHeight, j90BrowserInnerWidth = v.Browser_InnerWidth, j90BrowserInnerHeight = v.Browser_InnerHeight };

            var uaParser = UAParser.Parser.GetDefault();
            UAParser.ClientInfo client_info = uaParser.Parse(v.Browser_UserAgent);
            c.j90BrowserOS = client_info.OS.Family + " " + client_info.OS.Major;
            c.j90BrowserFamily = client_info.UA.Family + " " + client_info.UA.Major;
            c.j90BrowserDeviceFamily = client_info.Device.Family;
            c.j90BrowserDeviceType = v.Browser_DeviceType;
            c.j90LoginMessage = strMessage;
            c.j90LoginName = v.UserName;

            if (intJ03ID == 0)
            {
                c.j03ID = null;
            }
            else
            {
                c.j03ID = intJ03ID;
            }


            c.j90LocationHost = v.Browser_Host;
            c.j90AppClient = "PIPE";



            var db = new DbHandler(DbEnum.PrimaryDb);
            db.RunSql("INSERT INTO j90LoginAccessLog(j03ID,j90Date,j90LoginName,j90ClientBrowser,j90BrowserAvailWidth,j90BrowserAvailHeight,j90BrowserInnerWidth,j90BrowserInnerHeight,j90LoginMessage,j90LocationHost,j90AppClient,j90BrowserOS,j90BrowserFamily,j90BrowserDeviceFamily) VALUES(@j03ID,GETDATE(),@j90LoginName,@j90ClientBrowser,@j90BrowserAvailWidth,@j90BrowserAvailHeight,@j90BrowserInnerWidth,@j90BrowserInnerHeight,@j90LoginMessage,@j90LocationHost,@j90AppClient,@j90BrowserOS,@j90BrowserFamily,@j90BrowserDeviceFamily)", new { j03ID = c.j03ID, j90LoginName = c.j90LoginName, j90ClientBrowser = c.j90ClientBrowser, j90BrowserAvailWidth = c.j90BrowserAvailWidth, j90BrowserAvailHeight = c.j90BrowserAvailHeight, j90BrowserInnerWidth = c.j90BrowserInnerWidth, j90BrowserInnerHeight = c.j90BrowserInnerHeight, j90LoginMessage = c.j90LoginMessage, j90LocationHost = c.j90LocationHost, j90AppClient = c.j90AppClient, j90BrowserOS = c.j90BrowserOS, j90BrowserFamily = c.j90BrowserFamily, j90BrowserDeviceFamily = c.j90BrowserDeviceFamily });

        }


    }
}