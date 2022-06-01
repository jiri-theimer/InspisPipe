using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace InspisWS
{
    [ServiceContract(Namespace = "InspisWS.v1")]
    public interface IUploads
    {
        /// <summary>
        /// Ulozeni odpovedi na fileupload otazku
        /// </summary>
        [OperationContract]
        Result SaveFile(string HashIsKey, int a11ID, int f19ID, string fileName, string contentType, byte[] fileData);

        /// <summary>
        /// Maze jiz ulozenou prilohu
        /// </summary>
        [OperationContract]
        Result DeleteFile(string HashIsKey, int o27ID);

        /// <summary>
        /// Seznam uploadovanych souboru k dane otazce
        /// </summary>
        [OperationContract]
        Result_Attachments GetFiles(string HashIsKey, int a11id, int f19id);
    }
}
