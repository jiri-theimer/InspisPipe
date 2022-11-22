using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;


namespace InspisPipe.Controllers
{
    public class _ValidateUserController : InspisPipe.Controllers.BaseApiController
    {
        public bool Get(string apikey, string login, string password)
        {
            HttpContext.Current.Request.Url.ToString();
            
            password = HttpUtility.UrlDecode(password);
            
            if (string.IsNullOrEmpty(login))
            {
                return false;
            }
            bas.VerifyApiKey(apikey);


            if (basMemberShip.ValidateUser(login, password))
            {
                return true;
            }
            else
            {
                var recJ03 = bas.LoadJ03ByLogin(login);
                if (recJ03==null || !recJ03.j03IsDomainAccount)
                {
                    return false;
                }

                return ValidateUserInDomain(login, password);   //alternativně pokus o ověření uživatele ve windows doméně

            }




        }

        private bool ValidateUserInDomain(string login, string password)    //ověření uživatele ve windows doméně
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
                if (providerAD.ValidateUser(login, password))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {

                throw new Exception("Chyba při pokusu o ověření uživatele ve windows doméně: " + e.Message.ToString());
            }
        }
    }
}