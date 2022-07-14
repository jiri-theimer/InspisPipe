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
        public bool Get(string sourcedocpath, string destpdfpath)
        {

            var xx = new VB.ExportDocToPDF();
            return xx.DoExport(sourcedocpath, destpdfpath, "profile002.ini");
            
        }
    }
}