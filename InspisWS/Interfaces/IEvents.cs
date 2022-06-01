using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace InspisWS
{
    [ServiceContract]
    public interface IEvents
    {
        /// <summary>
        /// Vrací signaturu akce
        /// </summary>
        [OperationContract]
        string LoadLinkerSignature(string HashIsKey, int a01ID);

        /// <summary>
        /// Vraci seznam vazeb akce-formular
        /// </summary>
        /// <param name="f06ID">ID formulare</param>
        /// <param name="a01ID">ID akce</param>
        /// <param name="a03ID">ID instituce</param>
        [OperationContract]
        IEnumerable<a11EventForm> GetListEventForm(string HashIsKey, int f06ID, int a01ID, int a03ID);

        /// <summary>
        /// Nacteni instance a11EventForm
        /// </summary>
        /// <param name="a11ID">a11ID</param>
        [OperationContract]
        a11EventForm GetEventForm(string HashIsKey, int a11ID);

        /// <summary>
        /// Zapsat komentar k akci
        /// </summary>
        /// <param name="a01ID">ID akce</param>
        /// <param name="comment">Komentar k ulozeni</param>
        [OperationContract]
        Result SaveCommentOnly(string HashIsKey, int a01ID, string comment);

        /// <summary>
        /// Seznam akci dle zadaneho filtru
        /// </summary>
        [OperationContract]
        IEnumerable<a01Event> GetList(string HashIsKey, int b02ID, int a10ID, int a08ID, int a03ID, int j70ID, string a03REDIZO);

        /// <summary>
        /// Zalozeni nove akce
        /// </summary>
        /// <param name="a10ID">ID akce</param>
        /// <param name="a08ID">Typ akce</param>
        /// <param name="f06IDs">Formulare v akci, muze byt null</param>
        [OperationContract]
        Result Create(int a10ID, int a08ID, int[] f06IDs);

        /// <summary>
        /// Zalozeni nove akce vcetne vazby na skolu
        /// </summary>
        /// <param name="a10ID">ID akce</param>
        /// <param name="a08ID">Typ akce</param>
        /// <param name="f06IDs">Formulare v akci, muze byt null</param>
        [OperationContract]
        Result CreateInline(string HashIsKey, int a10ID, int a08ID, string REDIZO, string IZO, int[] f06IDs);

        /// <summary>
        /// Vraci instanci akce
        /// </summary>
        /// <param name="signature">signature</param>
        [OperationContract]
        a01Event GetBySignature(string HashIsKey, string signature);

        /// <summary>
        /// Vraci instanci akce
        /// </summary>
        /// <param name="a01ID">a01ID</param>
        [OperationContract]
        a01Event Get(string HashIsKey, int a01ID);

        /// <summary>
        /// Vraci seznam moznych workflow kroku v danem kontextu akce
        /// </summary>
        /// <param name="b02ID">b02ID</param>
        [OperationContract]
        IEnumerable<b06WorkflowStep> GetAvailableWorkflowSteps(int b02ID);

        /// <summary>
        /// Posune akci ve workflow
        /// </summary>
        [OperationContract]
        Result RunWorkflowStep(string HashIsKey, int a01ID, int b06ID, string comment);
    }
}
