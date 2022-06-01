using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace InspisWS
{
    [ServiceContract(Namespace = "InspisWS.v1")]
    public interface ILists
    {
        /// <summary>
        /// Typ akce
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/a10EventType")]
        IEnumerable<a10EventType> a10EventType();

        /// <summary>
        /// Aplikační role, Enita [j04UserRole]
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/j04UserRole")]
        IEnumerable<j04UserRole> j04UserRole();

        /// <summary>
        /// Kraje (regiony), Enita [a05Region]
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/a05Region")]
        IEnumerable<a05Region> a05Region();

        /// <summary>
        /// Typy zřizovatelů škol, Enita [a09FounderType]
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/a09FounderType")]
        IEnumerable<a09FounderType> a09FounderType();

        /// <summary>
        /// Právní formy institucí, Enita [a21InstitutionLegalType]
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/a21InstitutionLegalType")]
        IEnumerable<a21InstitutionLegalType> a21InstitutionLegalType();

        /// <summary>
        /// Inspektoráty, Enita [a04Inspectorate]
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/a04Inspectorate")]
        IEnumerable<a04Inspectorate> a04Inspectorate();

        /// <summary>
        /// Instituce, entita [a03Institution]
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/a03Institution")]
        IEnumerable<a03Institution> a03Institution();

        /// <summary>
        /// Vztah osob ke školám, Enita [a39InstitutionPerson]
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/a39InstitutionPerson")]
        IEnumerable<a39InstitutionPerson> a39InstitutionPerson();

        /// <summary>
        /// Uživatelský účet + personální profil, inner join entit [j03User] + [j02Person]
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/PersonProfile")]
        IEnumerable<PersonProfile> PersonProfile();

        /// <summary>
        /// Uživatelský účet + personální profil, inner join entit [j03User] + [j02Person]
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/PersonProfileISet?activeInISet={activeInISet}")]
        IEnumerable<PersonProfile> PersonProfileISet(bool activeInISet);
    }
}
