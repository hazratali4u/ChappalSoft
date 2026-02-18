using System;
using System.Data;
using ChappalBLL;
using System.Web.Services;
using System.Web.Script.Services;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public partial class rptDocument : System.Web.UI.Page
{
    PartyController party = new PartyController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtFromDate.Text = CookieContext.WorkingDate.ToString("dd-MMM-yyyy");
            this.txtToDate.Text = CookieContext.WorkingDate.ToString("dd-MMM-yyyy");
            LoadCustomer();
            LoadSupplier();
        }
    }
    private void LoadCustomer()
    {
        DataTable dtCustomer = party.GetParty(7);
        ddlCustomer.DataSource = dtCustomer;
        ddlCustomer.DataTextField = "Name";
        ddlCustomer.DataValueField = "PartyID";
        ddlCustomer.DataBind();
    }
    private void LoadSupplier()
    {
        DataTable dtSupplier = party.GetParty(8);
        ddlSupplier.DataSource = dtSupplier;
        ddlSupplier.DataTextField = "Name";
        ddlSupplier.DataValueField = "PartyID";
        ddlSupplier.DataBind();
    }
    [WebMethod]
    [ScriptMethod]
    public static string ShowDocumentReport(string PartyID, string FromDate, string ToDate, int ReportType,int ViewType)
    {
        ReportController report = new ReportController();
        DataTable dtReport = report.GetDocumentReport(Convert.ToInt32(PartyID), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), ReportType, ViewType);
        return GetJson(dtReport);
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