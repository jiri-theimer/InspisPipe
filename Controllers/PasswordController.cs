using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InspisPipe.Models;

namespace InspisPipe.Controllers
{
    public class PasswordController : BaseController
    {
        

        public ActionResult ConfirmRecovery(string guid)
        {
            var v = new BaseViewModel();
            v.AddMainButton("Přihlášení do systému", Url.Action("Index", "Login"));
            v.AddMainButton("Spuštění testu",basConfig.Url_SET + "/Login.aspx?Type=PasswordReset", "Certifikované a školní testování",true);
            v.AddMainButton("Výsledky testu",basConfig.Url_SET + "/Vysledky/Default.aspx", "Certifikované a školní testování",true);
            v.AddMainButton("Vytvořit nový účet", Url.Action("Index", "CreateUser",true));

            if (string.IsNullOrEmpty(guid))
            {
                v.ShowErrorMessasge("Na vstupu chybí GUID!");return View(v);
            }            
            if (System.IO.File.Exists(basConfig.TempFolder + "\\" + guid + ".old"))
            {
                v.ShowWarningMessage("Tato žádost o obnovení hesla byla již dříve zpracována.<hr>Pro nové heslo musíte vygenerovat novou žádost!"); return View(v);
            }
            string strFile = basConfig.TempFolder + "\\" + guid + ".txt";
            if (!System.IO.File.Exists(strFile))
            {
                v.ShowErrorMessasge("Na serveru nelze najít vyplněnou žádost o obnovu hesla!"); return View(v);
            }
            var fi = bas.GetFileInfo(strFile);
            if (fi.CreationTime.AddMinutes(30) < DateTime.Now)
            {
                v.ShowWarningMessage("Tato žádost o obnovení přihlašovacího hesla je starší než 30 minut.<hr>Musíte vygenerovat novou žádost."); return View(v);
            }
            var arr = System.IO.File.ReadAllText(strFile).Split('|').ToList();            
            string strNewPwd = basMemberShip.RecoveryPassword(arr[0]);
            if (basMemberShip.ErrorMessage != null)
            {
                v.ShowErrorMessasge(basMemberShip.ErrorMessage);return View(v);
            }

            handle_send_new_password(arr[0], strNewPwd,v);

            return View(v);
        }

        private void handle_send_new_password(string login,string newpwd, BaseViewModel v)
        {
            var recJ03 = bas.LoadJ03ByLogin(login);
            var recJ02 = bas.LoadJ02Record(recJ03.j02ID);
            bas.wsinit();
            bas.ws("Dobrý den,");
            bas.ws($"v systému InspIS DATA došlo k obnovení přihlašovacího hesla k vašemu účtu [{login}].");
            bas.ws();bas.ws($"Nové heslo je: {newpwd}");
            bas.ws();bas.ws();bas.ws("Toto je automatické upozornění, na tuto zprávu prosím neodpovídejte.");
            bas.ws();bas.ws("Systém InspIS DATA");

            var cMail = new SendMail() { MessageGuid = bas.GetGuid() };
            if (!cMail.SendMessage(bas.wsget(), "Potvrzení nového hesla v aplikaci InspIS DATA", recJ02.j02Email))
            {
                v.ShowWarningMessage($"Potvrzení o novém heslu bylo založeno, ale při odesílání e-mail zprávy došlo k chybě:<hr>" + cMail.ErrorMessage);
            }
            else
            {               
                v.ShowInfoMessasge($"Nové heslo bylo vygenerováno. Informace o heslu byla odeslána na e-mail adresu: {recJ02.j02Email}.");
                
            }
        }

        public ActionResult Recovery()
        {
            var v = new PasswordRecoveryViewModel();
            if (HttpContext.Request.IsAuthenticated && !string.IsNullOrEmpty(HttpContext.User.Identity.Name))
            {
                v.IsNotPossible = true; //uživatel je přihlášený
                v.ShowWarningMessage("Obnova zapomenutého hesla funguje pouze v případě, že jste odhlášený z aplikace.");
            }
            else
            {
                v.ShowInfoMessasge("Zadejte uživatelský učet, pro který chcete obnovit heslo.<hr>Na e-mail adresu účtu bude zaslána žádost o obnovení přihlašovacího hesla.");
            }


            RefreshStateRecovery(v);
            return View(v);
        }

        [HttpPost]
        public ActionResult Recovery(PasswordRecoveryViewModel v)
        {
            RefreshStateRecovery(v);

            if (string.IsNullOrEmpty(v.LoginEmail))
            {
                v.ShowErrorMessasge("Musíte zadat přihlašovací jméno/e-mail!"); return View(v);
            }
            
            var recJ03 = bas.LoadJ03ByLogin(v.LoginEmail);
            if (recJ03 == null)
            {
                v.ShowErrorMessasge($"Přihlašovací jméno {v.LoginEmail} neexistuje!"); return View(v);
            }
            if (recJ03.j02ID == 0)
            {
                v.ShowErrorMessasge($"Uživatelský účet bez osobního profilu!"); return View(v);
            }
            var recJ02 = bas.LoadJ02Record(recJ03.j02ID);
            if (recJ02.isclosed || recJ03.isclosed)
            {
                v.ShowErrorMessasge($"Uživatelský účet nebo osobní profil byl uzavřen!"); return View(v);
            }
            
            var cMail = new SendMail() { MessageGuid = bas.GetGuid() };
            if (!cMail.SendMessage(GetPwdRecoveryMessage(recJ03, cMail.MessageGuid), "Žádost o obnovení přihlašovacího hesla v aplikaci InspIS DATA", recJ02.j02Email))
            {
                v.ShowWarningMessage($"Žádost o obnovení hesla byla založena, ale při odesílání potvrzovací e-mail zprávy došlo k chybě:<hr>" + cMail.ErrorMessage);
            }
            else
            {
                System.IO.File.WriteAllText(basConfig.TempFolder + "\\" + cMail.MessageGuid + ".txt", recJ03.j03Login + "|" + recJ02.j02Email);
                v.ShowInfoMessasge($"Žádost o potvrzení obnovy hesla byla odeslána na e-mail adresu: {recJ02.j02Email}.");
                v.IsNotPossible = true;
            }



            return View(v);
        }

        private void RefreshStateRecovery(PasswordRecoveryViewModel v)
        {
            v.AddMainButton("Přihlášení do systému",Url.Action("Index","Login"),"InspIS DATA",true);
            v.AddMainButton("Spuštění testu",basConfig.Url_SET + "/Login.aspx?Type=PasswordReset", "Certifikované a školní testování",true);
            v.AddMainButton("Výsledky testu",basConfig.Url_SET + "/Vysledky/Default.aspx", "Certifikované a školní testování",true);
            v.AddMainButton("Vytvořit nový účet", Url.Action("Index", "CreateUser"),null,true);
        }

        private string GetPwdRecoveryMessage(j03User recJ03, string guid)
        {
            bas.wsinit();
            bas.ws("Dobrý den,");
            bas.ws($"v systému InspIS DATA byla zaregistrována žádost o vytvoření nového hesla k vašemu účtu [{recJ03.j03Login}].");
            bas.ws(); bas.ws("Pro odeslání žádosti klikněte prosím na následující odkaz:");                        
            bas.ws(basConfig.Url_PIPE + "/Password/ConfirmRecovery?guid=" + guid);
            bas.ws(); bas.ws("V případě, že chcete zachovat Vaše stávající heslo, na uvedený odkaz neklikejte a tuto zprávu ignorujte.");
            bas.ws(); bas.ws("Toto je automatické upozornění, na tuto zprávu prosím neodpovídejte.");
            bas.ws(); bas.ws("Systém InspIS DATA");

            return bas.wsget();
        }
        
    }
}