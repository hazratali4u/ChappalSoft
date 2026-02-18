using System;
using System.Web;

/// <summary>
/// Summary description for CookieContext
/// </summary>
public static class CookieContext
{
    public static int UserID
    {
        get
        {
            var c = HttpContext.Current.Request.Cookies["CookieContext"];            
            return c == null ? 0 : Convert.ToInt32(c.Values["UserID"]);
        }
    }

    public static int RoleID
    {
        get
        {
            var c = HttpContext.Current.Request.Cookies["CookieContext"];
            return c == null ? 0 : Convert.ToInt32(c.Values["RoleID"]);
        }
    }

    public static string EmployeeName
    {
        get
        {
            var c = HttpContext.Current.Request.Cookies["CookieContext"];
            return c == null ? string.Empty : c.Values["EmployeeName"].ToString();
        }
    }

    public static DateTime WorkingDate
    {
        get
        {
            var c = HttpContext.Current.Request.Cookies["CookieContext"];
            return c == null
                ? DateTime.MinValue
                : DateTime.Parse(c.Values["WorkingDate"]);
        }
    }

    public static int DBServer
    {
        get
        {
            var c = System.Configuration.ConfigurationManager.AppSettings["DBServer"];
            return c == null ? 1 : Convert.ToInt32(c);
        }
    }
}