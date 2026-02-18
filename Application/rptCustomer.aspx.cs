using System;
using System.Data;
using ChappalBLL;
using System.Web.Services;
using System.Web.Script.Services;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public partial class rptCustomer : System.Web.UI.Page
{
    PartyController party = new PartyController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtFromDate.Text = CookieContext.WorkingDate.ToString("dd-MMM-yyyy");
            this.txtToDate.Text = CookieContext.WorkingDate.ToString("dd-MMM-yyyy");
            LoadCustomer();
        }
    }
    private void LoadCustomer()
    {
        DataTable dtCustomer = party.GetParty(5);
        ddlCustomer.DataSource = dtCustomer;
        ddlCustomer.DataTextField = "Name";
        ddlCustomer.DataValueField = "PartyID";
        ddlCustomer.DataBind();
    }
    [WebMethod]
    [ScriptMethod]
    public static string ShowLedgerRport(string CustomerID, string FromDate, string ToDate, int ReportType)
    {
        ReportController report = new ReportController();
        DataSet dsReport = report.GetLedgerReport(Convert.ToInt32(CustomerID), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), ReportType);
        return JsonConvert.SerializeObject(dsReport, Formatting.None);
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