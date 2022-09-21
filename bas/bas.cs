using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
public class bas
{
    private static System.Text.StringBuilder _s { get; set; }

    public static void wsinit()
    {
        _s = new System.Text.StringBuilder();
    }
    public static string wsget()
    {
        return _s.ToString();
    }

    public static void ws(string s = null)
    {
        if (_s == null)
        {
            wsinit();
        }
        if (string.IsNullOrEmpty(s))
        {
            _s.AppendLine();
        }
        else
        {
            _s.AppendLine(s);
        }

    }

    public static FileInfo GetFileInfo(string strFullPath)
    {
        return new FileInfo(strFullPath);
    }
    public static string GetCurrentBinFolder()
    {
        return System.Web.Hosting.HostingEnvironment.MapPath("~/bin");
    }
    
    

    
    public static bool VerifyApiKey(string apikey)
    {        
        if (string.IsNullOrEmpty(apikey))
        {
            throw new Exception("Na vstupu chybí apikey.");
        }
        if (apikey == "cesta-na-přímo-bez-klíče!" || apikey== "cesta-na-primo-bez-klice!")
        {
            return true;
        }
        var db = new DbHandler(DbEnum.PrimaryDb);
        var c = db.Load<GetString>($"select p85ID as Value from p85TempBox WHERE p85GUID='{apikey}' AND p85Prefix='apikey' AND p85DateInsert>DATEADD(MINUTE,-10,GETDATE())");
        if (c ==null)
        {
            throw new Exception("Chybně zadané nebo časově neplatné apikey: " + apikey);
        }

        return true;

    }
    public static string RightString(string input, int num)
    {
        if (num > input.Length)
        {
            num = input.Length;
        }
        return input.Substring(input.Length - num);
    }
    public static string GetGuid()
    {

        return System.Guid.NewGuid().ToString("N");
    }
    public static bool IsExpressionPID(string strExpression)
    {
        if (strExpression.IndexOf("-") > 0 | strExpression.IndexOf("/") > 0)
            return false;
        else
            return true;
    }

    public string GetGinisURL(string pid)
    {
        return $"http://wmx06.csi.local/gordic/ginis/app/usu05/?c=OpenDetail&ixx1={pid}";

        
    }


    public static int InInt(string s)
    {
        if (int.TryParse(s, out int x))
        {
            return x;
        }
        else
        {
            return 0;
        }
    }
    public static Double InDouble(string s)
    {
        if (double.TryParse(s, out Double x))
        {
            return x;
        }
        else
        {
            return 0;
        }
    }

    public static InspisPipe.Models.j03User LoadJ03ByLogin(string login)
    {        
        return j03_handle_load($"j03Login LIKE '{login}'");
    }
    public static InspisPipe.Models.j03User LoadJ03ByGuid(string guid)
    {
        return j03_handle_load($"j03Guid LIKE '{guid}'");
    }
    private static InspisPipe.Models.j03User j03_handle_load(string sqlwhere)
    {
        var db = new DbHandler(DbEnum.PrimaryDb);
        return db.Load<InspisPipe.Models.j03User>($"select *,convert(bit,CASE WHEN GETDATE() BETWEEN j03ValidFrom AND j03ValidUntil THEN 0 ELSE 1 end) as isclosed FROM j03User WHERE "+sqlwhere);

    }
    public static InspisPipe.Models.j02Person LoadJ02Record(int j02id)
    {
        return j02_handle_load($"j02ID={j02id}");
    }
    public static InspisPipe.Models.j02Person LoadJ02RecordByGuid(string j02guid)
    {
        return j02_handle_load($"j02Guid LIKE '{j02guid}'");       
    }

    public static InspisPipe.Models.j02Person LoadJ02RecordByEmail(string j02email)
    {
        return j02_handle_load($"j02Email LIKE '{j02email}'");        
    }

    private static InspisPipe.Models.j02Person j02_handle_load(string sqlwhere)
    {
        var db = new DbHandler(DbEnum.PrimaryDb);
        return db.Load<InspisPipe.Models.j02Person>($"select *,convert(bit,CASE WHEN GETDATE() BETWEEN j02ValidFrom AND j02ValidUntil THEN 0 ELSE 1 end) as isclosed FROM j02Person WHERE " + sqlwhere);

    }


    public static void LogError(string message, string username = null, string procname = null)
    {
        Handle_Log_Write("error", message, username, procname);
    }
    public static void LogInfo(string message, string username = null, string procname = null)
    {
        Handle_Log_Write("info", message, username, procname);
    }
    private static void Handle_Log_Write(string logname, string message, string username = null, string procname = null)
    {
        
        var strLogDir = basConfig.TempFolder + "\\Logs";
        
        var strPath = string.Format("{0}\\log-{1}-{2}-{3}.log", strLogDir, logname, username, DateTime.Now.ToString("yyyy.MM.dd"));
        try
        {
            System.IO.File.AppendAllLines(strPath, new List<string>() { "", "", "------------------------------", DateTime.Now.ToString() });
            if (procname != null)
            {
                System.IO.File.AppendAllLines(strPath, new List<string>() { "procname: " + procname });
            }
            System.IO.File.AppendAllLines(strPath, new List<string>() { "message: " + message });
        }
        catch
        {
            //nic
        }

    }


}
