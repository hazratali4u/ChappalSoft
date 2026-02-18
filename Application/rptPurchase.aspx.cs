using System;
using System.Data;
using ChappalBLL;
using System.Web.Services;
using System.Web.Script.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public partial class rptPurchase : System.Web.UI.Page
{
    CategoryController category = new CategoryController();
    ItemController item = new ItemController();    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtFromDate.Text = CookieContext.WorkingDate.ToString("dd-MMM-yyyy");
            this.txtToDate.Text = CookieContext.WorkingDate.ToString("dd-MMM-yyyy");
            LoadCategory();
            LoadItem();
        }
    }
    private void LoadCategory()
    {
        DataTable dtCategory = category.GetCategory(3);
        ddlCategory.DataSource = dtCategory;
        ddlCategory.DataTextField = "Name";
        ddlCategory.DataValueField = "CategoryID";
        ddlCategory.DataBind();
    }
    private void LoadItem()
    {
        DataTable dtItem = item.GetItem(4);
        ddlItem.DataSource = dtItem;
        ddlItem.DataTextField = "Name";
        ddlItem.DataValueField = "ItemID";
        ddlItem.DataBind();

        hfItemIDs.Value = GetJson(dtItem);
    }

    [WebMethod]
    [ScriptMethod]
    public static string ShowRport(string CategoryID,string ItemID,string FromDate, string ToDate,int ReportType)
    {
        ReportController report = new ReportController();
        DataTable dtReport = report.GetPurchaseReport(Convert.ToInt32(CategoryID), Convert.ToInt32(ItemID), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), ReportType);
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