using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

public static class basConfig
{
    public static string TempFolder
    {
        get
        {
            return @ConfigurationManager.AppSettings["TempFolder"];
        }
    }
    public static string Url_PIPE
    {
        get
        {
            return @ConfigurationManager.AppSettings["Url_PIPE"];
        }
    }
    public static string Url_INSPIS { get
        {
            return @ConfigurationManager.AppSettings["Url_INSPIS"];
        }
    }
    public static string Url_SET
    {
        get
        {
            return @ConfigurationManager.AppSettings["Url_SET"];
        }
    }
    public static string Url_PORTAL
    {
        get
        {
            return @ConfigurationManager.AppSettings["Url_PORTAL"];
        }
    }
    public static string Url_ELEARNING
    {
        get
        {
            return @ConfigurationManager.AppSettings["Url_ELEARNING"];
        }
    }
}