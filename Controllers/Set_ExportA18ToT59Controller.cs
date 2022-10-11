using Aspose.Words;
using InspisPipe.Models;
using InspisPipe.SetIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;

namespace InspisPipe.Controllers
{
    public class Set_ExportA18ToT59Controller : BaseApiController
    {
        public string Get()
        {
            
            var db = new DbHandler(DbEnum.PrimaryDb);
            string s = "SELECT a.*,case when a.a18SubCode2 is not null then a.a18SubCode1+a.a18SubCode2 end as Skupina,case when LEN(a.a18Code)>=5 then LEFT(a.a18Code,5) end as Obor";
            s += " FROM a18DepartmentDomain a";
            s += " WHERE GETDATE() BETWEEN a.a18ValidFrom AND a.a18ValidUntil";

            var lisA18 = db.GetList<a18DepartmentDomain>(s);
            

            List<FieldDefinition> fields = new List<FieldDefinition>();

            foreach(var rec in lisA18)
            {
                var c = new FieldDefinition() { A18Id = rec.a18ID, Code = rec.a18Code, Name = rec.a18Name, SubCode1 = rec.a18SubCode1, SubCode2 = rec.a18SubCode2, SubCode3 = rec.a18SubCode3, SubCode4 = rec.a18SubCode4 };
                fields.Add(c);
            }

            using (WSEPisClient client = new SetIntegration.WSEPisClient("WSHttpBinding_IWSEPis"))
            {
                
                var arr = fields.ToArray();
                var result = client.SynchronizeFields(arr);

                if (result.Success)
                {
                    return "1";
                }
                else
                {
                    return fields.Count().ToString()+ " (fields count), error: "+ result.ErrorMessage;
                }
            }

                


        }
    }
}
