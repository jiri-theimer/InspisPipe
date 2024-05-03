using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InspisPipe.Models
{
    public enum j04RelationFlagEnum
    {
        NoRelation = 1,     //Aplikační role bez omezení uživatele podle jeho vztahu k instituci nebo k inspektorátu
        A03 = 2,            //Aplikační role s omezením na příslušnost uživatele k instituci
        A05 = 3             //Aplikační role s omezením na příslušnost osoby uživatele ke kraji podle inspektorátu

    }
    public enum j04PortalFaceFlagEnum       //Vztah role k PORTÁLu
    {
        CSI = 1,        //ČŠI uživatel
        School = 2,     //škola
        Founder = 3,    //zřizovatel
        Anonymous = 4   //veřejnost
    }
    public enum j04UaFlag
    {
        None = 0,
        Employee = 1       //vyjadřuje vztah zaměstnavatel -> zaměstnanec
    }
    public enum j04TwoFactorVerifyFlagEnum
    {
        None = 0,
        AlwaysAfterLogin = 1,
        IfChangedUserAgend = 2
    }
    public enum j04DashboardTabFlagEnum
    {
        None = 0,
        Standard = 1
    }
    public class j04UserRole
    {
        public bool isclosed { get; set; }
        public int j04ID { get; set; }
        public string j04Name { get; set; }
        public string j04RoleValue { get; set; }
        public string j04Aspx_PersonalPage { get; set; }
        public string j04Aspx_PersonalPage_Mobile { get; set; }

        public j04RelationFlagEnum j04RelationFlag { get; set; }
        public j04PortalFaceFlagEnum j04PortalFaceFlag { get; set; }

        public bool j04IsAllowedAllEventTypes { get; set; }     //true: Možnost zakládat i číst všechny typy akcí
        public bool j04IsAllowInSchoolAdmin { get; set; }       //true: Role dostupná ve správě školních účtů
        public int j04ElearningDuration { get; set; }
        public bool j04IsElearningNeeded { get; set; }

        public string j04ViewUrl_Page { get; set; } //nové CORE pole - výchozí stránka role
        public string j04DefaultWidgets_Inspector { get; set; } //tovární sada widgetů na osobní stránku inspektora
        public string j04DefaultWidgets_School { get; set; } //tovární sada widgetů na osobní stránku školy
        public j04TwoFactorVerifyFlagEnum j04TwoFactorVerifyFlag { get; set; }    //nastavení 2-faktorového ověření
        public j04DashboardTabFlagEnum j04DashboardTabFlag { get; set; }    //nastavení záložky dashboard v osobní stránce
        public int j04PasswordExpirationDays { get; set; }
    }
}