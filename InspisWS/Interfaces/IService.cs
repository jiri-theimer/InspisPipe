using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace InspisWS
{
    /// <summary>
    /// Obsluzne sluzby - testovani uzivatelskeho jmena, testovani dostupnosti WS
    /// </summary>
    [ServiceContract(Namespace = "InspisWS.v1")]
    public interface IService
    {
        /// <summary>
        /// Testovaci metoda
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/Test")]
        bool Test();

        /// <summary>
        /// Vraci true pokud se podarilo uzivatele overit
        /// </summary>
        /// <returns>PID uzivatele</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/CheckUser")]
        bool CheckUser();

        /*[OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetEncryptedKey")]
        string GetEncryptedKey();*/
    }
}
