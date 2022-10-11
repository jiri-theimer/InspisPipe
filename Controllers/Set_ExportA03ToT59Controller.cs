using InspisPipe.Models;
using InspisPipe.SetIntegration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;

namespace InspisPipe.Controllers
{
    public class Set_ExportA03ToT59Controller : BaseApiController
    {
        public string Get(int a06id,string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return "Na vstupu chybí query!";
            }
            if (query.Trim().Length<3)
            {
                return "Velikost query musí být alespoň 3 znaky!";
            }

            var db = new DbHandler(DbEnum.PrimaryDb);
            string s = "SELECT a.*";
            s += ",a05.a05name,a05.a05UIVCode,a06.a06Name,a21.a21Name,a09.a09name,a09.a09UIVCode,a05.a05UIVCode,a70.a70Code,a28.a28Name";
            s += ",CASE WHEN GETDATE() BETWEEN a.a03ValidFrom AND a.a03ValidUntil THEN 0 ELSE 1 end as isclosed";
            s += " FROM a03Institution a";

            s += " LEFT OUTER JOIN a05Region a05 ON a.a05id=a05.a05id LEFT OUTER JOIN a09FounderType a09 on a.a09id=a09.a09id LEFT OUTER JOIN a06InstitutionType a06 ON a.a06ID=a06.a06ID";
            s += " LEFT OUTER JOIN a21InstitutionLegalType a21 ON a.a21ID=a21.a21ID LEFT OUTER JOIN a70SIS a70 ON a.a70ID=a70.a70ID";
            s += " LEFT OUTER JOIN a28SchoolType a28 ON a.a28ID=a28.a28ID";
            //s += " WHERE GETDATE() BETWEEN a.a03ValidFrom AND a.a03ValidUntil";
            s += " WHERE 1=1";
            if (a06id > 0)
            {
                s += " AND a.a06ID=" + a06id.ToString();
            }

            s += " AND (a.a03Name like '%'+@ss+'%' OR a.a03Email LIKE '%'+@ss+'%' OR a.a03City LIKE '%'+@ss+'%' OR a.a03Street LIKE '%'+@ss+'%'";
            s += " OR a.a03ICO like @ss+'%' OR a.a03REDIZO like '%'+@ss+'%')";

            

            var lisA03 = db.GetList<a03Institution>(s, new {ss=query});

            
            List<SubjectInfo> fields = new List<SubjectInfo>();

            foreach (var rec in lisA03)
            {
                var c = new SubjectInfo() { ContactPersonName = rec.a03DirectorFullName, AdrStreet = rec.a03Street, AdrZipCode = rec.a03PostCode, AdrCity = rec.a03City, Ico=rec.a03ICO, Name=rec.a03Name, AdrRegionUIV=rec.a05UIVCode };
                c.IsClosed = rec.isclosed;
                if (rec.a06ID == 1)
                {
                    c.SubjectA03Id = rec.a03ID;
                    c.Redizo = Convert.ToInt32(rec.a03REDIZO);
                    c.FounderTypeId = rec.a09ID;
                    
                }

                fields.Add(c);

            }

            using (WSEPisClient client = new SetIntegration.WSEPisClient("WSHttpBinding_IWSEPis"))
            {
                var arr = fields.ToArray();

                var result = client.SynchronizeSchools(arr);

                if (result.Success)
                {
                    return "OK, rows: " + fields.Count();
                }
                else
                {
                    return fields.Count().ToString() + " (fields count), error: " + result.ErrorMessage;
                }
            }
        }
    }
}
