using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.DirectoryServices.AccountManagement;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace InspisPipe.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            var client = new InspisPipe.InspisWS.Events();

            //ViewBag.testapi = client.LoadLinkerSignature("Ag44wNQL34Aj/2FVjcdynQ==", 772846);
            //ViewBag.testapi = client.GetListEventForm("Ag44wNQL34Aj/2FVjcdynQ==",2069735, 772846,38045);
            var lis = client.a10EventType();
            ViewBag.testapi = lis.First().PID.ToString() + ": " + lis.First().a10Name + ", počet: " + lis.Count().ToString();

            //pokusapi_post();


            return View();
        }

        private async Task<bool> ValidateUser(string login, string password)        //volání PIPE api služby
        {

            var httpclient = new HttpClient();

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://tinspiscore.csicr.cz/pipe/api/_ValidateUser?apikey=cesta-na-přímo-bez-klíče!&login=" + login + "&password=" + password))
            {
                HttpResponseMessage response = await httpclient.SendAsync(request);
                string strJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(strJson);

            }


        }

        private string pokusapi_post()
        {
            string url = "https://tinspiscore.csicr.cz";
            HttpContent content = null;

            var client = new HttpClient();

            client.BaseAddress = new Uri(url);
            
            HttpResponseMessage response = client.PostAsync("api/Login?login=lamos&password=123.456aA", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
                
            }

            return null;
        }

        private string pokusapi_get()
        {
            string url = "https://tinspiscore.csicr.cz";

            var client = new HttpClient();
            
            client.BaseAddress = new Uri(url);
            
            HttpResponseMessage response = client.GetAsync("api/Anonym/Ping").Result;
            if (response.IsSuccessStatusCode)
            {
                string strJson = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<string>(strJson);
            }
            else
            {
                return "chyba";
            }
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
