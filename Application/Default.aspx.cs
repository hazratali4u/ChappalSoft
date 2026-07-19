using ChappalBLL;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Web;

public partial class Default : System.Web.UI.Page
{
    UserController user = new UserController();    
    DayCloseController dayclose = new DayCloseController();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        try
        {
            if (txtUsername.Text.Length > 0 && txtPassword.Text.Length > 0)
            {

                DataTable dtUser = user.GetUser(txtUsername.Text, txtPassword.Text,1, CookieContext.DBServer);
                if (dtUser.Rows.Count > 0)
                {
                    HttpCookie userCookie = new HttpCookie("CookieContext");                    
                    userCookie.Values["UserID"] = dtUser.Rows[0]["UserID"].ToString();
                    userCookie.Values["EmployeeName"] = dtUser.Rows[0]["EmployeeName"].ToString();
                    userCookie.Values["RoleID"] = dtUser.Rows[0]["RoleID"].ToString();                    
                    userCookie.Expires = DateTime.Now.AddHours(10);
                    userCookie.HttpOnly = true;
                    Response.Cookies.Add(userCookie);
                    InsertDayClose(Convert.ToDateTime(dtUser.Rows[0]["WorkingDate"]), Convert.ToInt32(dtUser.Rows[0]["UserID"]));
                    Response.Redirect("Home.aspx");
                }
                else
                {
                    lblError.Text = "Invalid Username or Password!";
                }
            }
        }
        catch (Exception ex)
        {
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Text = ex.Message;
        }
    }
    private void InsertDayClose(DateTime WorkingDate,int UserID)
    {
        dayclose.InsertDayClose(WorkingDate, UserID, CookieContext.DBServer);
        HttpCookie cookie = Request.Cookies["CookieContext"] ?? new HttpCookie("CookieContext");
        if (CookieContext.DBServer == 1)
        {
            cookie.Values["WorkingDate"] = System.DateTime.Now.ToShortDateString();
        }
        else
        {
            cookie.Values["WorkingDate"] = System.DateTime.Now.AddHours(12).ToShortDateString();
        }
        cookie.Expires = DateTime.Now.AddHours(10);
        cookie.HttpOnly = true;
        Response.Cookies.Add(cookie);
    }
}