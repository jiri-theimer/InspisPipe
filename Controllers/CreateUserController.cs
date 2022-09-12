using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using InspisPipe.Models;

namespace InspisPipe.Controllers
{
    public class CreateUserController : BaseController
    {
        public ActionResult Index()
        {
            var v = new CreateUserViewModel() { EmailAddress = "@", VerifyEmail = "@" };

            RefreshStateIndex(v);

            return View(v);
        }

        [HttpPost]
        public ActionResult Index(CreateUserViewModel v, string oper)
        {
            RefreshStateIndex(v);

            if (!string.IsNullOrEmpty(oper))
            {
                return View(v);
            }
            string vzorec = new Crypto().Decrypt(v.LastCaptchaFormulaHashed);
            string spravna_odpoved = Calculator(vzorec).ToString();
            if (!(spravna_odpoved == v.CaptchaAnswer || spravna_odpoved == "+" + v.CaptchaAnswer))
            {
                v.ShowErrorMessasge($"Zadali jste špatný výsledek.<hr>Správný výsledek pro [{vzorec}] je: {spravna_odpoved}");
                return View(v);
            }

            if (string.IsNullOrEmpty(v.EmailAddress) || string.IsNullOrEmpty(v.VerifyEmail) || string.IsNullOrEmpty(v.Firstname) || string.IsNullOrEmpty(v.Lastname))
            {
                v.ShowErrorMessasge("E-mail, jméno a příjmení a jsou povinná pole k vyplnění!"); return View(v);
            }
            if (v.EmailAddress.Length < 5)
            {
                v.ShowErrorMessasge("Příliš krátká e-mail adresa."); return View(v);
            }
            if (v.EmailAddress.ToLower() != v.VerifyEmail.ToLower())
            {
                v.ShowErrorMessasge("Nesouhlasí e-mail adresa s ověřením."); return View(v);
            }

            string strPwd = basMemberShip.GetRandomPassword();
            if (!basMemberShip.ValidatBeforeCreate(v.EmailAddress, strPwd, strPwd))
            {
                v.ShowErrorMessasge(basMemberShip.ErrorMessage); return View(v);
            }

            int intJ04ID = bas.InInt(@ConfigurationManager.AppSettings["CreateUser_j04ID"]);
            if (intJ04ID == 0)
            {
                v.ShowErrorMessasge("V konfiguraci chybí definice j04ID."); return View(v);
            }
            var recJ03 = bas.LoadJ03ByLogin(v.EmailAddress);
            if (recJ03 != null)
            {
                v.ShowErrorMessasge("V systému již existuje uživatel s tímto přihlašovacím jménem (e-mail)."); return View(v);
            }
            var recJ02 = bas.LoadJ02RecordByEmail(v.EmailAddress);
            if (recJ02 != null)
            {
                v.ShowErrorMessasge("V systému již existuje osobní profil s touto e-mail adresou."); return View(v);
            }
            string strGUID = bas.GetGuid();
            var db = new DbHandler(DbEnum.PrimaryDb);
            if (!db.RunSql("INSERT INTO j02Person(j02Email,j02FirstName,j02LastName,j02TitleBeforeName,j02DateUpdate,j02UserInsert,j02UserUpdate,j02Guid) VALUES(@email,@firstname,@lastname,@title,GETDATE(),'pipe','pipe',@guid)", new { email = v.EmailAddress, firstname = v.Firstname, lastname = v.Lastname, title = v.Title, guid = strGUID }))
            {
                v.ShowErrorMessasge(db.GetLastError()); return View(v);
            }
            recJ02 = bas.LoadJ02RecordByGuid(strGUID);
            if (!db.RunSql("INSERT INTO j03User(j04ID,j02ID,j03Login,j03DateUpdate,j03UserInsert,j03UserUpdate,j03Guid) VALUES(@j04id,@j02id,@login,GETDATE(),'pipe','pipe',@guid)", new { j04id = intJ04ID, j02id = recJ02.j02ID, login = v.EmailAddress, guid = strGUID }))
            {
                v.ShowErrorMessasge(db.GetLastError()); return View(v);
            }

            recJ03 = bas.LoadJ03ByLogin(v.EmailAddress);

            var cMail = new SendMail() { MessageGuid = strGUID };
            if (!cMail.SendMessage(GetNewUserRequestMessage(recJ03, recJ02), "Žádost o potvrzení nového účtu v aplikaci InspIS DATA", recJ02.j02Email))
            {
                v.ShowWarningMessage($"Žádost o uživatelský účet byla založena, ale při odesílání potvrzovací e-mail zprávy došlo k chybě:<hr>" + cMail.ErrorMessage);
            }
            else
            {
                v.ShowInfoMessasge($"Potvrzovací zpráva byla odeslána na vaší e-mail adresu: {recJ02.j02Email}.<hr>Heslo k novému účtu obdržíte po potvrzení e-mail zprávy.");
            }

            v.IsFinished = true;
            return View(v);
        }


        private void RefreshStateIndex(CreateUserViewModel v)
        {
            v.AddMainButton("Přihlášení do systému",Url.Action("Index","Login"), "InspIS DATA",true);
            v.AddMainButton("InspIS SET", basConfig.Url_SET, "Systém elektronického testování");
            v.AddMainButton("InspIS PORTÁL", basConfig.Url_PORTAL, "Portál informací o školách");

            v.AddMainButton("Zapomenuté heslo", Url.Action("Recovery", "Password"),null,true);

            v.captcha = new CaptchaSupport();

        }


        private double Calculator(string expr)
        {
            expr = expr.Replace(" ", "").Replace("--", "").Replace("DROP", "").Replace("DELETE", "");
            var db = new DbHandler(DbEnum.PrimaryDb);
            return db.Load<GetDouble>($"select {expr} as Value").Value;
        }

        private string GetNewUserRequestMessage(j03User recJ03, j02Person recJ02)
        {
            bas.wsinit();
            bas.ws("Dobrý den,");
            bas.ws($"kliknutím na následující odkaz potvrdíte vytvoření uživatelského účtu pro uživatele {recJ03.j03Login} v systému InspIS DATA.");            
            bas.ws(basConfig.Url_PIPE + "/CreateUser/Confirm?guid=" + recJ03.j03Guid);
            bas.ws(); bas.ws("Heslo k novému účtu obdržíte po jeho vytvoření.");
            bas.ws(); bas.ws("Toto je automatické upozornění, na tuto zprávu prosím neodpovídejte.");
            bas.ws(); bas.ws("Systém InspIS DATA");

            return bas.wsget();
        }

        public ActionResult Confirm(string guid)
        {
            var v = new CreateUserConfirmViewModel() { IsNotPossible = true };
            v.AddMainButton("Přihlášení do systému", Url.Action("Index", "Login"));
            v.AddMainButton("Zapomenuté heslo", Url.Action("Recovery", "Password"));
            v.AddMainButton("Vytvořit nový účet", Url.Action("Index", "CreateUser"));

            v.RecJ03 = bas.LoadJ03ByGuid(guid);
            if (v.RecJ03 == null)
            {
                v.ShowErrorMessasge("Pro tento GUID neexistuje uživatelský účet!"); return View(v);
            }
            if (v.RecJ03.j03MembershipUserId != null)
            {
                v.ShowErrorMessasge("Tento účet byl již dříve aktivován."); return View(v);
            }

            string strPwd = basMemberShip.GetRandomPassword();
            if (!basMemberShip.ValidatBeforeCreate(v.RecJ03.j03Login, strPwd, strPwd))
            {
                v.ShowErrorMessasge(basMemberShip.ErrorMessage); return View(v);
            }

            v.IsNotPossible = false;
            basMemberShip.CreateUser(v.RecJ03.j03Login, v.RecJ03.j03Login, strPwd);
            var strMembershipID = basMemberShip.GetUserID(v.RecJ03.j03Login);
            var db = new DbHandler(DbEnum.PrimaryDb);
            db.RunSql("UPDATE j03User set j03PasswordLastChange=GETDATE(),j03MembershipUserId=@s WHERE j03Login LIKE @login", new { s = strMembershipID, login = v.RecJ03.j03Login });

            v.RecJ03 = bas.LoadJ03ByLogin(v.RecJ03.j03Login);
            v.RecJ02 = bas.LoadJ02Record(v.RecJ03.j02ID);

            var cMail = new SendMail();
            if (!cMail.SendMessage(GetNewUserConfirmMessage(v.RecJ03, v.RecJ02, strPwd), $"Aktivace uživatele {v.RecJ03.j03Login} v aplikaci InspIS DATA", v.RecJ02.j02Email))
            {
                v.ShowWarningMessage($"Žádost o uživatelský účet byla založena, ale při odesílání potvrzovací e-mail zprávy došlo k chybě:<hr>" + cMail.ErrorMessage);
            }
            else
            {
                v.ShowInfoMessasge($"Na vaší e-mailovou adresu byla odeslána zpráva s přístupovými údaji do systému InspIS DATA.");

            }

            return View(v);
        }

        private string GetNewUserConfirmMessage(j03User recJ03, j02Person recJ02, string pwd)
        {
            bas.wsinit();
            bas.ws("Dobrý den,");
            bas.ws($"uživateli [{recJ03.j03Login}] byl v systému InspIS DATA aktivován účet s následujícími údaji:");
            bas.ws(); bas.ws($"Přihlašovací jméno: {recJ03.j03Login}");
            bas.ws($"Heslo: {pwd}");
            bas.ws(); bas.ws("Heslo lze změnit po přihlášení do systému na stránce [Můj profil] -> [Změnit přístupové heslo].");

            bas.ws(); bas.ws("Toto je automatické upozornění, na tuto zprávu prosím neodpovídejte.");
            bas.ws(); bas.ws("Systém InspIS DATA");

            return bas.wsget();
        }
    }
}