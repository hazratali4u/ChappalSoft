using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using ChappalBLL;

public partial class Home : System.Web.UI.Page
{
    DashboardController dashboard = new DashboardController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            DataTable dtDay = new DataTable();
            dtDay.Columns.Add("DayName", typeof(string));

            DataRow dr = dtDay.NewRow();
            dr["DayName"] = DateTime.Now.DayOfWeek.ToString();
            dtDay.Rows.Add(dr);

            DataRow dr2 = dtDay.NewRow();
            dr2["DayName"] = DateTime.Now.AddDays(-1).DayOfWeek.ToString();
            dtDay.Rows.Add(dr2);

            DataRow dr3 = dtDay.NewRow();
            dr3["DayName"] = DateTime.Now.AddDays(-2).DayOfWeek.ToString();
            dtDay.Rows.Add(dr3);

            DataRow dr4 = dtDay.NewRow();
            dr4["DayName"] = DateTime.Now.AddDays(-3).DayOfWeek.ToString();
            dtDay.Rows.Add(dr4);

            DataRow dr5 = dtDay.NewRow();
            dr5["DayName"] = DateTime.Now.AddDays(-4).DayOfWeek.ToString();
            dtDay.Rows.Add(dr5);

            DataRow dr6 = dtDay.NewRow();
            dr6["DayName"] = DateTime.Now.AddDays(-5).DayOfWeek.ToString();
            dtDay.Rows.Add(dr6);

            DataRow dr7 = dtDay.NewRow();
            dr7["DayName"] = DateTime.Now.AddDays(-6).DayOfWeek.ToString();
            dtDay.Rows.Add(dr7);

            hfDays.Value = GetJson(dtDay);
            DataSet dsData = dashboard.GetDashboardData(1);
            hf7DaysPurchase.Value = GetJson(dsData.Tables[0]);
            hf7DaysSale.Value = GetJson(dsData.Tables[1]);
            hf7DaysExpense.Value = GetJson(dsData.Tables[2]);
        }
    }
    public static string GetJson(DataTable dt)
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        serializer.MaxJsonLength = Int32.MaxValue;
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row = null;

        foreach (DataRow dr in dt.Rows)
        {
            row = dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col]);
            rows.Add(row);
        }
        return serializer.Serialize(rows);
    }
}