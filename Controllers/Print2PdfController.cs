using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspisPipe.Controllers
{
    public class Print2PdfController : InspisPipe.Controllers.BaseApiController
    {
        // GET: Print2Pdf
        public bool Get(string strSourceDocFile, string strDestPdfFile)
        {

            var xx = new VB.ExportDocToPDF();
            return xx.DoExport(strSourceDocFile, strDestPdfFile, "profile002.ini");
            
        }
    }
}