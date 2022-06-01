using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Security;

namespace InspisWS
{
    public abstract class BaseSvc
    {
        private string _baseurl { get; set; }        
        private string _login { get; set; }
        
        public string Token { get; set; }
        public BaseSvc()
        {
            _baseurl = "https://tinspiscore.csicr.cz/api";
            _login = null;
           
        }

        private void CheckUserNameAndToken()
        {
            if (!string.IsNullOrEmpty(_login) && !string.IsNullOrEmpty(this.Token))
            {
                return; //již dříve ověřeno
            }

            if (!ServiceSecurityContext.Current.PrimaryIdentity.IsAuthenticated)
            {
                // ziskat username z hlavicky
                var headers = WebOperationContext.Current.IncomingRequest.Headers;
                if (!headers.AllKeys.Contains("username") || !headers.AllKeys.Contains("password"))
                {                    
                    throw new System.Security.Authentication.AuthenticationException("Zkontrolujte, zdali zadávané přihlašovací údaje jsou platné. Vyzkoušejte se s těmito přihlašovacími údaji přihlásit do systému InspIS.");
                }
                string login = headers["username"];
                string pwd = headers["password"];
                this.Token = LoadToken(login, pwd);
                if (!string.IsNullOrEmpty(this.Token))
                {
                    _login = login;                    
                }
            }
            else
            {
                _login = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            }
        }


        private string LoadToken(string login,string pwd)
        {
                                    
            var request = new HttpRequestMessage(HttpMethod.Post, _baseurl + $"/Login?login={login}&password={pwd}");
            
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.SendAsync(request).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    return null;
                }
            }

            
        }

        

        protected string GetApiPlainResult(string urlocas)
        {
            CheckUserNameAndToken();

            var request = new HttpRequestMessage(HttpMethod.Get, _baseurl + "/"+ urlocas);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this.Token);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;

                if (response.IsSuccessStatusCode)
                {
                    
                    return response.Content.ReadAsStringAsync().Result;

                }
            }


            return null;
        }

        

    }
}