using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace InspisWS
{
    [ServiceContract(Namespace = "InspisWS.v1")]
    public interface IPragodata
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetTaskList?State={State}&FromDate={FromDate}&ToDate={ToDate}")]
        Result_PragoDataTask GetTaskList(string State, DateTime FromDate, DateTime ToDate);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ChangeState?TaskID={TaskID}&State={State}")]
        Result ChangeState(int TaskID, string State);

        /// <summary>
        /// Uživatelský účet + personální profil, inner join entit [j03User] + [j02Person]
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/PersonProfile")]
        IEnumerable<PersonProfileISet> PersonProfile();
    }
}
