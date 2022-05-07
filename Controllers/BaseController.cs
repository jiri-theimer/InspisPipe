using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InspisPipe.Models;

namespace InspisPipe.Controllers
{
    public class BaseController : Controller
    {
        

        public BaseController()
        {
            
         
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            

            base.OnActionExecuting(filterContext);
        }




       
    }
}