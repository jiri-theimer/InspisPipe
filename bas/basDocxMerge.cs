using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aspose.Words;
using System.Data;

public static class basDocxMerge
{
    public static void SetLicense()
    {
        var lic = new Aspose.Words.License();
        lic.SetLicense(bas.GetCurrentBinFolder() + "\\Aspose_Words.lic");
    }

    public static DataTable GetDataTable4MailMerge(string prefix, int pid,DbHandler db)
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SELECT ");
        switch (prefix)
        {
            case "a01":
                sb.Append("a.*,a08.*,a10.*,b02.*,issuer.*,a21.a21Name,a05.a05Name");
                sb.Append(",a03.a03ICO,a03.a03REDIZO,a03.a03Name,a03.a03City,a03.a03DateInsert,a03.a03UserInsert,a03.a03ValidFrom,a03.a03ValidUntil,a03.a03Street,a03.a03PostCode,a03.a03Phone,a03.a03Mobile,a03.a03Fax,a03.a03Email,a03.a03Web,a03.a03DateUpdate,a03.a03UserUpdate,a03.a03ID_Founder,a03.a03IsTestRecord,a03.a03FounderCode,a03.a03DirectorFullName");
                sb.Append(",zri.a03Name as founder_name,zri.a03FounderCode as founder_code,a04.*");
                sb.Append(",left(a03.a03PostCode,3)+' '+RIGHT(a03.a03PostCode,2) as a03PostCode_32,left(a04.a04PostCode,3)+' '+RIGHT(a04.a04PostCode,2) as a04PostCode_32");
                sb.Append(" FROM a01Event a INNER JOIN a10EventType a10 ON a.a10ID=a10.a10ID");
                sb.Append(" LEFT OUTER JOIN a03Institution a03 ON a.a03ID=a03.a03ID");
                sb.Append(" LEFT OUTER JOIN a08Theme a08 ON a.a08ID=a08.a08ID");
                sb.Append(" LEFT OUTER JOIN b02WorkflowStatus b02 ON a.b02ID=b02.b02ID");
                sb.Append(" LEFT OUTER JOIN j02Person issuer ON a.j02ID_Issuer=issuer.j02ID");
                sb.Append(" LEFT OUTER JOIN a21InstitutionLegalType a21 ON a03.a21ID=a21.a21ID");
                sb.Append(" LEFT OUTER JOIN a03Institution zri on a03.a03ID_Founder=zri.a03ID");
                sb.Append(" LEFT OUTER JOIN a05Region a05 ON a03.a05ID=a05.a05ID");
                sb.Append(" LEFT OUTER JOIN (select * FROM a04Inspectorate WHERE a04IsRegional=1) a04 ON a03.a05ID=a04.a05ID");
                break;
            case "a03":
                sb.Append("a.a03ICO,a.a03REDIZO,a.a03Name,a.a03City,a.a03DateInsert,a.a03UserInsert,a.a03ValidFrom,a.a03ValidUntil,a.a03Street,a.a03PostCode,a.a03Phone,a.a03Mobile,a.a03Fax,a.a03Email,a.a03Web,a.a03DateUpdate,a.a03UserUpdate,a.a03ID_Founder,a.a03IsTestRecord,a.a03FounderCode,a.a03DirectorFullName");
                sb.Append(",a05.a05name,a09.a09name,a06.a06Name,a21.a21Name,zri.a03Name as founder_name,zri.a03FounderCode as founder_code");
                sb.Append(" FROM a03Institution a LEFT OUTER JOIN a05Region a05 ON a.a05id=a05.a05id");
                sb.Append(" LEFT OUTER JOIN a09FounderType a09 on a.a09id=a09.a09id");
                sb.Append(" LEFT OUTER JOIN a06InstitutionType a06 ON a.a06ID=a06.a06ID");
                sb.Append(" LEFT OUTER JOIN a21InstitutionLegalType a21 ON a.a21ID=a21.a21ID");
                sb.Append(" LEFT OUTER JOIN a03Institution zri on a.a03ID_Founder=zri.a03ID");
                break;
            case "h04":
                sb.Append("a.*,h07.*,a01.*,j02.*,h05.h05Name");
                sb.Append(",a03.a03ICO,a03.a03REDIZO,a03.a03Name,a03.a03City,a03.a03Street,a03.a03PostCode,a03.a03Phone,a03.a03Mobile,a03.a03Fax,a03.a03Email,a03.a03Web,a03.a03FounderCode,a03.a03DirectorFullName");
                sb.Append(" FROM h04ToDo a INNER JOIN h07ToDoType h07 ON a.h07ID=h07.h07ID INNER JOIN a01Event a01 ON a.a01ID=a01.a01ID INNER JOIN j02Person j02 ON a.j02ID_Owner=j02.j02ID LEFT OUTER JOIN h05ToDoStatus h05 ON a.h05ID=h05.h05ID");
                sb.Append(" LEFT OUTER JOIN a03Institution a03 ON a01.a03ID=a03.a03ID");
                break;
            case "a11":
                sb.Append("a.*,a01.*,f06.*,a25.*,a37.*,k01.*,issuer_a01.*");
                sb.Append(" FROM a11EventForm a INNER JOIN f06Form f06 ON a.f06ID=f06.f06ID INNER JOIN a01Event a01 ON a.a01ID=a01.a01ID");
                sb.Append(" LEFT OUTER JOIN a25EventFormGroup a25 ON a.a25ID=a25.a25ID");
                sb.Append(" LEFT OUTER JOIN a37InstitutionDepartment a37 ON a.a37ID=a37.a37ID");
                sb.Append(" LEFT OUTER JOIN k01Teacher k01 ON a.k01ID=k01.k01ID");
                sb.Append(" LEFT OUTER JOIN j02Person issuer_a01 ON a01.j02ID_Issuer=issuer_a01.j02ID");
                break;
            case "j02":
                sb.Append("a.*,j07.j07Name,j03.j03Login,j03.j03LangIndex,j04.*");
                sb.Append(" FROM j02Person a LEFT OUTER JOIN j07Position j07 on a.j07ID=j07.j07ID LEFT OUTER JOIN j03User j03 ON a.j02ID=j03.j02ID LEFT OUTER JOIN j04UserRole j04 ON j03.j04ID=j04.j04ID");
                break;
            case "j03":
                sb.Append("a.*,j07.j07Name,j02.*,j04.*");
                sb.Append(" FROM j03User a INNER JOIN j04UserRole j04 ON a.j04ID=j04.j04ID LEFT OUTER JOIN j02Person j02 ON a.j02ID=j02.j02ID LEFT OUTER JOIN j07Position j07 on j02.j07ID=j07.j07ID");
                break;
        }
        sb.Append(" WHERE a." + prefix + "ID=" + pid.ToString());
        return db.GetDataTable(sb.ToString());
    }

    public static bool SaveDocument(Document doc,string destfullpath,string destformat)
    {
        switch (destformat.ToLower())
        {            
            case "docx":
                doc.Save(destfullpath, SaveFormat.Docx);
                break;
            case "doc":
                doc.Save(destfullpath, SaveFormat.Doc);
                break;
            case "odt":
                doc.Save(destfullpath, SaveFormat.Odt);
                break;
            case "rtf":
                doc.Save(destfullpath, SaveFormat.Odt);
                break;
            default:
                doc.Save(destfullpath, SaveFormat.Pdf);
                break;
        }

        if (System.IO.File.Exists(destfullpath))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
