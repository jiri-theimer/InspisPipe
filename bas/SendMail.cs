using System;
using System.Net.Mail;
using System.Text;
using System.Collections.Generic;
using System.Linq;


public class SendMail
{
    public string ErrorMessage { get; set; }
    public string MessageGuid { get; set; }
    public bool IsBodyHtml { get; set; }
    private List<Attachment> _attachments;
    public bool SendMessage(string strBody, string strSubject, string strTo)
    {
        var db = new DbHandler(DbEnum.PrimaryDb);
        var recJ40 = db.Load<InspisPipe.Models.j40MailAccount>("select top 1 a.* FROM j40MailAccount a WHERE a.j40UsageFlag=2 AND GETDATE() BETWEEN a.j40ValidFrom AND a.j40ValidUntil ORDER BY a.j40Ordinary");

        string smtpserver = recJ40.j40SmtpHost;
        int port = recJ40.j40SmtpPort;
        bool enablessl = recJ40.j40SmtpEnableSsl;
        string login = recJ40.j40SmtpLogin;
        string emailaddress = recJ40.j40SmtpEmail;
        string pwd = recJ40.j40SmtpPassword;
        if (string.IsNullOrEmpty(this.MessageGuid))
        {
            this.MessageGuid = bas.GetGuid();
        }        

        var m = new MailMessage() { Body = strBody, IsBodyHtml = this.IsBodyHtml, Subject = strSubject };
        m.From = new MailAddress(emailaddress);

        strTo = strTo.Replace(";", ",");
        var tos = strTo.Split(',').ToList();
        foreach (string s in tos)
        {
            m.To.Add(s);
        }
        if (_attachments != null && _attachments.Count() > 0)
        {
            foreach (var att in _attachments)
            {
                m.Attachments.Add(att);
            }
        }

        using (SmtpClient client = new SmtpClient(smtpserver, port))
        {
            client.Credentials = new System.Net.NetworkCredential(login, pwd);

            m.BodyEncoding = System.Text.Encoding.UTF8;
            m.SubjectEncoding = System.Text.Encoding.UTF8;
            m.Headers.Add("Message-ID", this.MessageGuid);
            m.IsBodyHtml = this.IsBodyHtml;

            
            client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;    //odeslanou zprávu uložit na serveru jako EML soubor
            client.PickupDirectoryLocation = basConfig.TempFolder;
            
            client.Send(m); //nejdříve uložit eml soubor do temp složky
            

            client.DeliveryMethod = SmtpDeliveryMethod.Network; //nyní opravdu odeslat
            client.EnableSsl = enablessl;


            try
            {
                client.Send(m);

            }
            catch (Exception e)
            {
                this.ErrorMessage = e.Message;
            }

        }

        if (this.ErrorMessage != null)
        {
            return false;
        }
        else
        {
            return true;
        }



    }

    public void AddAttachment(string fullpath, string displayname, string contenttype = null)
    {
        if (_attachments == null) _attachments = new List<Attachment>();
        var att = new Attachment(fullpath);
        att.Name = displayname;
        if (contenttype != null)
        {
            att.ContentType = new System.Net.Mime.ContentType(contenttype);
        }
        _attachments.Add(att);
    }
}

