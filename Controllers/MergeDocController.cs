using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Aspose.Words;
using System.IO;
using System.Data;
using System.Text;

namespace InspisPipe.Controllers
{
    public class MergeDocController : InspisPipe.Controllers.BaseApiController
    {
        public string Get(int x31id,string docfilename, int rec_pid, string rec_prefix,string destformat)
        {
            if (rec_pid==0 || string.IsNullOrEmpty(rec_prefix))
            {
                return "rec_pid or rec_prefix missing.";
            }
            var strPath = basConfig.TempFolder + "\\" + docfilename;
            if (!File.Exists(strPath))
            {
                return $"TEMP soubor {docfilename} neexistuje.";
            }

            var db = new DbHandler(DbEnum.PrimaryDb);
            var dt = basDocxMerge.GetDataTable4MailMerge(rec_prefix, rec_pid, db);
            var recX31 = db.Load<InspisPipe.Models.x31Report>($"select * from x31Report WHERE x31ID={x31id}");
            if (recX31 == null)
            {
                return "recX31 is null.";
            }
            var tabregions = new List<InspisPipe.Models.MergeDocRegion>();

            if (!string.IsNullOrEmpty(recX31.x31DocSqlSourceTabs))
            {
                recX31.x31DocSqlSourceTabs = recX31.x31DocSqlSourceTabs.Replace("ß", "€");
                recX31.x31DocSqlSourceTabs = recX31.x31DocSqlSourceTabs.Replace("\r\n\r\n", "ß");
                var blocks = recX31.x31DocSqlSourceTabs.Split('ß').ToList();

                foreach(string s in blocks)
                {

                    var lis = s.Split('|').ToList();
                    if (lis.Count() > 0)
                    {
                        var reg = new Models.MergeDocRegion() { RegionName = lis[0], SqlData = lis[1] };
                        if (lis.Count() > 2)
                        {
                            reg.SqlNoData = lis[2];
                        }
                        tabregions.Add(reg);
                    }
                }
            }

            basDocxMerge.SetLicense();
            

            var doc = new Document();
            try
            {
                doc = new Document(strPath);
            }catch(Exception ex)
            {
                return ex.Message;
            }

            var dstDoc = doc.Clone();
            dstDoc.Sections.Clear();

            foreach (DataRow dbrow in dt.Rows)
            {
                var rowDoc = doc.Clone();
                rowDoc.MailMerge.Execute(dbrow);

                if (tabregions.Count() > 0)
                {
                    MergeInnerTable(rowDoc, rec_pid, tabregions,db);
                }
                

                AppendDoc(dstDoc, rowDoc);
            }
            
            
           

            string strDestFileName = docfilename.Replace(bas.GetFileInfo(strPath).Extension,"").Replace(".","")+"_"+DateTime.Now.ToString("ddMMyyHHmm") + "." + destformat;

            if (basDocxMerge.SaveDocument(dstDoc, basConfig.TempFolder + "\\"+ strDestFileName, destformat))
            {
                return strDestFileName;
            }
            else
            {
                return "Chyba v [basDocxMerge.SaveDocument]";
            }


        }

        private void AppendDoc(Document dstDoc,Document srcDoc)
        {
            for(int i = 0; i < srcDoc.Sections.Count; i++)
            {
                Section s = (Section)dstDoc.ImportNode(srcDoc.Sections[i], true);
                dstDoc.Sections.Add(s);
            }
        }

        private void MergeInnerTable(Document rowDoc,int rec_pid, List<InspisPipe.Models.MergeDocRegion> regions,DbHandler db)
        {
            foreach(var reg in regions)
            {
                
                string strSqlData= reg.SqlData.Replace("#pid#", rec_pid.ToString()).Replace("@pid", rec_pid.ToString());
                var dt = db.GetDataTable(strSqlData);
                
                if (dt.Rows.Count==0 && reg.SqlNoData==null && dt.Columns.Count>0)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (reg.SqlNoData == null)
                        {
                            if (db.GetLastError() != null)
                            {
                                reg.SqlNoData = $"SELECT '{db.GetLastError().Replace("'","")}' as " + dt.Columns[i].ColumnName;
                            }
                            else
                            {
                                reg.SqlNoData = "SELECT NULL as " + dt.Columns[i].ColumnName;
                            }
                            
                        }
                        else
                        {
                            reg.SqlNoData += ",NULL as " + dt.Columns[i].ColumnName;
                        }
                    }                    
                }
                if (dt.Rows.Count == 0 && dt.Columns.Count>0 && reg.SqlNoData !=null)
                {
                    dt = db.GetDataTable(reg.SqlNoData);
                    
                }

                dt.TableName = reg.RegionName;
                rowDoc.MailMerge.ExecuteWithRegions(dt);

            }

        }
    }
}
