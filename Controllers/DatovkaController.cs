using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspisPipe.Controllers
{
    public class DatovkaController : InspisPipe.Controllers.BaseApiController
    {
        // GET: Datovka
        public string Get(string apikey,string login, string ginis_doc_pid, string ginis_file_pid, string id_esu, string id_ds, string message_subject)
        {
            if (string.IsNullOrEmpty(login))
            {
                return "login missing";
            }
            if (string.IsNullOrEmpty(ginis_doc_pid) || string.IsNullOrEmpty(ginis_file_pid) || string.IsNullOrEmpty(id_esu) || string.IsNullOrEmpty(id_ds))
            {
                return "missing input";
            }

            bas.VerifyApiKey(apikey);

            this.InhaleLogin(login);

            if (!string.IsNullOrEmpty(message_subject))
            {
                message_subject = HttpUtility.UrlDecode(message_subject);
            }
            
            return _gin.OdeslatDatovku(ginis_doc_pid, ginis_file_pid, id_esu, id_ds, message_subject);

            //string strID = _gin.NajdiEsuIco("25722034");
            //return _gin.OdeslatDatovku("CSI0X00ADYIQ", "CSI00C0UHD39", "CSI0SE02IO9F","df6w7m5","Nazdar v pondělí 2");        //pro produkční server

            //return _gin.OdeslatDatovku("CSI0X009BYCR", "CSI0SE02EGNH", "df6w7m5");    //pro test server
        }
    }
}