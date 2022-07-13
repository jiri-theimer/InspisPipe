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
        string smtpserver = "smtp.mycorecloud.net";
        int port = 25;
        bool enablessl = false;
        string login = "smtp-marktime";
        string emailaddress = "noreply@marktime.net";
        string pwd = "Oomaidee3Ais";
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

