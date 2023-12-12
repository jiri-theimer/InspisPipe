
using System;
using System.Web;


namespace InspisPipe.Controllers
{
    public class DatovkaErrorLogController : InspisPipe.Controllers.BaseApiController
    {
        // GET: Datovka
        public string Get()
        {
            string strPath = $"{basConfig.LogsFolder}\\log-error-datovka-{DateTime.Now.ToString("yyyy.MM.dd")}.log";
            if (System.IO.File.Exists(strPath))
            {
                string s= System.IO.File.ReadAllText(strPath);
                
                return s;
            }
            else
            {
                return "Log je prázdný";
            }

            




            //string strID = _gin.NajdiEsuIco("25722034");
            //return _gin.OdeslatDatovku("CSI0X00ADYIQ", "CSI00C0UHD39", "CSI0SE02IO9F","df6w7m5","Nazdar v pondělí 2");        //pro produkční server

            //return _gin.OdeslatDatovku("CSI0X009BYCR", "CSI0SE02EGNH", "df6w7m5");    //pro test server
        }
    }
}