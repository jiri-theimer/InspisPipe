using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace InspisWS
{
    [ServiceContract(Namespace = "InspisWS.v1")]
    public interface IForms
    {
        /// <summary>
        /// Ulozeni infa o publikovatelnosti
        /// </summary>
        [OperationContract]
        Result PublishQuestion(string HashIsKey, int a11ID, int f19ID, bool publish);

        /// <summary>
        /// Ulozeni odpovedi
        /// </summary>
        [OperationContract]
        Result SaveAnswers(string HashIsKey, int a11ID, List<f32FilledValue> odpovedi);

        /// <summary>
        /// Vycisteni odpovedi
        /// </summary>
        [OperationContract]
        Result ClearAnswers(string HashIsKey, int a11ID, int f19ID);

        /// <summary>
        /// Vraci kompletni set dat potrebny pro vytvoreni formulare
        /// </summary>
        [OperationContract]
        Result_GetForm GetForm(string HashIsKey, int a11id);

        /// <summary>
        /// Vraci ID formulare na zaklade ID akce
        /// </summary>
        [OperationContract]
        int GetFormIdFromEvent(string HashIsKey, int a11id);

        /// <summary>
        /// Instance formulare
        /// </summary>
        [OperationContract]
        Result_f06Form f06Form_Get(int f06id);

        /// <summary>
        /// Seznam segmentu pro dany formular
        /// </summary>
        [OperationContract]
        Result_f18FormSegment f18FormSegment_GetList(int f06id);

        /// <summary>
        /// Seznam vsech otazek pro dany formular
        /// </summary>
        [OperationContract]
        Result_f19Question f19Question_GetList(int f06id);

        /// <summary>
        /// Seznam jednotek odpovedi pro dany formular
        /// </summary>
        [OperationContract]
        Result_f21ReplyUnit f21ReplyUnit_GetList(int f06id);

        /// <summary>
        /// Seznam jiz vyplnenych odpovedi k dane akci
        /// </summary>
        [OperationContract]
        Result_f32FilledValue f32FilledValue_GetList(string HashIsKey, int a11id);
    }
}
