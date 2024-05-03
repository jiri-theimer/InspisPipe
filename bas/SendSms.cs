using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;



public class SendSms
{
    public const string _Login = "admin";
    public const string _Pwd = "8xMe95x6vB7iBIX";
    private string _Error { get; set; }

    public string GetError()
    {
        return _Error;
    }

    

    private string GetSQL1(string strAppend = null)
    {
        string s = "SELECT a.*,case when a.x44DatetimeProcessed is not null then a.x44DatetimeProcessed else a.x44DateInsert end as MessageTime,";
        s += " FROM x44SmsLog a";
        s += " " + strAppend;

        return s;

    }
    public InspisPipe.Models.x44SmsLog Load(int x44id)
    {
        var db = new DbHandler(DbEnum.PrimaryDb);

        return db.Load<InspisPipe.Models.x44SmsLog>(GetSQL1($" WHERE a.x44ID={x44id}"));
    }

    public string SendLoginVerifyMessage(HttpClient httpclient, InspisPipe.Models.j02Person recJ02, InspisPipe.Models.j03User recJ03)
    {
        var rnd = new Random();
        var strCode = bas.RightString($"0000{rnd.Next(1, 9999)}", 4);
        var strMessage = $"Autorizační kód {strCode} pro přihlášení do systému InspIS DATA ({recJ03.j03Login})";


        var s = SendMessage(httpclient, recJ02.j02ID, recJ02.j02Mobile, strMessage, 502, recJ02.j02ID);
        if (GetError() == null)
        {
            
            UpdateSmsVerifyCode(recJ03.j03ID, strCode);
            return s;
        }
        else
        {
            return s;
        }
    }

    public string SendMessage(HttpClient httpclient, int j02id, string phonenumber, string message, int x29id, int datapid)
    {
        _Error = null;
        string strBody = System.Web.HttpUtility.UrlEncode(message);
        string strURL = $"http://{_Login}:{_Pwd}@192.168.1.54/values.xml?Cmd=SMS&Nmr={phonenumber}&Text={strBody}";
        
        int intX44ID = InsertMessage2Log(j02id, phonenumber, message, x29id, datapid);
        if (intX44ID == 0)
        {
            return "intX44ID=0";
        }
        try
        {
            if (httpclient == null)
            {
                httpclient = new HttpClient();
            }
            using (var request = new HttpRequestMessage(new HttpMethod("GET"), strURL))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{_Login}:{_Pwd}")));

                HttpResponseMessage response = httpclient.SendAsync(request).Result;
                string strResult = response.Content.ReadAsStringAsync().Result;
                if (strResult.Contains("QueueAddResponse"))
                {
                    UpdateLogResult(intX44ID, strResult, 3);
                }
                else
                {
                    _Error = strResult;
                    UpdateLogError(intX44ID, strResult);
                }



                return strResult;

            }
        }
        catch (Exception ex)
        {
            _Error = ex.Message;
            UpdateLogError(intX44ID, ex.Message);
            return ex.Message;
        }



    }

    public bool UpdateSmsVerifyCode(int j03id, string strSmsCode)
    {
        var db = new DbHandler(DbEnum.PrimaryDb);
        return db.RunSql("UPDATE j03User set j03SmsVerifyCode=@s,j03SmsVerifyCreate=GETDATE() WHERE j03ID=@pid", new { s = strSmsCode, pid = j03id });
    }
    public bool ClearSmsVerifyCode(int j03id)
    {
        var db = new DbHandler(DbEnum.PrimaryDb);
        return db.RunSql("UPDATE j03User set j03SmsVerifyCode=null,j03SmsVerifyCreate=null WHERE j03ID=@pid", new { pid = j03id });
    }


    private bool UpdateLogResult(int x44id, string strResult, int intStatus)
    {
        var db = new DbHandler(DbEnum.PrimaryDb);
        return db.RunSql("UPDATE x44SmsLog set x44Result=@result,x44Status=@status WHERE x44ID=@pid", new { pid = x44id, result = strResult, status = intStatus });
    }
    private bool UpdateLogError(int x44id, string strError)
    {
        var db = new DbHandler(DbEnum.PrimaryDb);
        return db.RunSql("UPDATE x44SmsLog set x44ErrorMessage=@err,x44Status=2 WHERE x44ID=@pid", new { pid = x44id, err = strError });
    }

    private int InsertMessage2Log(int j02id, string phonenumber, string message, int x29id, int datapid)
    {
        var rec = new InspisPipe.Models.x44SmsLog() { j02ID = j02id, x44Number = phonenumber, x44Body = message, x44Status = InspisPipe.Models.x44StateFlag.InQueque, x44DataPID = datapid, x29ID = x29id };
        
        var db = new DbHandler(DbEnum.PrimaryDb);


        bool b=db.RunSql("INSERT INTO x44SmsLog(j02ID,x44Status,x29ID,x44DataPID,x44Number,x44Body,x44ErrorMessage,x44Result) VALUES(@j02id,@status,@x29id,@datapid,@number,@body,@err,@result)", new { j02id = rec.j02ID,status=rec.x44Status,x29id=rec.x29ID,datapid=rec.x44DataPID,number=rec.x44Number,body=rec.x44Body,err=rec.x44ErrorMessage,result=rec.x44Result });

        if (b)
        {
            return db.Load<GetInteger>("select top 1 x44ID as Value FROM x44SmsLog order by x44ID DESC").Value;
        }

        return 0;



    }

}
