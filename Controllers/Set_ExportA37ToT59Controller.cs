using InspisPipe.Models;
using InspisPipe.SetIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InspisPipe.Controllers
{
    public class Set_ExportA37ToT59Controller : BaseApiController
    {
        public string Get(int a05id)
        {
            var db = new DbHandler(DbEnum.PrimaryDb);
            string s = "SELECT a.*,a03.a03Name,a03.a03REDIZO,a17.a17Name,a17.a17UIVCode";
            s += " FROM a37InstitutionDepartment a INNER JOIN a03Institution a03 ON a.a03ID=a03.a03ID LEFT OUTER JOIN a17DepartmentType a17 ON a.a17ID=a17.a17ID";
            s += " WHERE GETDATE() BETWEEN a.a37ValidFrom AND a.a37ValidUntil";
            if (a05id > 0)
            {
                s += " AND a03.a05ID=" + a05id.ToString();
            }

            var lisA37 = db.GetList<a37InstitutionDepartment>(s);

            s = "SELECT a.*,a37.a37Name,a37.a37IZO,a18.a18Name,a18.a18Code,a18.a18Code+' - '+a18.a18Name as a18CodePlusName,a37.a37IZO+' - '+a37.a37Name as a37IzoPlusName,a37.a03ID";
            s += " FROM a19DomainToInstitutionDepartment a INNER JOIN a37InstitutionDepartment a37 ON a.a37ID=a37.a37ID INNER JOIN a18DepartmentDomain a18 ON a.a18ID=a18.a18ID";
            s += " WHERE GETDATE() BETWEEN a37.a37ValidFrom AND a37.a37ValidUntil";
            if (a05id > 0)
            {
                s += " AND a37.a03ID IN (select a03ID FROM a03Institution WHERE a05ID=" + a05id.ToString()+")";
            }
            var lisA19 = db.GetList<a19DomainToInstitutionDepartment>(s);

            List<SchoolIzoDefinition> fields = new List<SchoolIzoDefinition>();

            foreach (var recA37 in lisA37)
            {
                var c = new SchoolIzoDefinition() { Izo=Convert.ToInt32(recA37.a37IZO), SubjectA03Id=recA37.a03ID, SubjectA37Id=recA37.a37ID, TypeCode=recA37.a17UIVCode };

                var qry = lisA19.Where(p => p.a37ID == recA37.a37ID);
                if (qry.Count() > 0)
                {
                    var a18ids = new List<int>();
                    foreach (var recA19 in qry)
                    {
                        a18ids.Add(recA19.a18ID);
                    }
                    c.FieldIds = a18ids.ToArray();                    
                }

                fields.Add(c);

            }

            using (WSEPisClient client = new SetIntegration.WSEPisClient("WSHttpBinding_IWSEPis"))
            {
                var arr = fields.ToArray();
                
                var result = client.SynchronizeSchoolIZO(arr);

                if (result.Success)
                {
                    return "1";
                }
                else
                {
                    return fields.Count().ToString() + " (fields count), error: " + result.ErrorMessage;
                }
            }
        }
    }
}
