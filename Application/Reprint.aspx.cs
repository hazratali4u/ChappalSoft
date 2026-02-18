using System;
using System.Data;
using ChappalBLL;
using System.Web.Services;
using System.Web.Script.Services;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class Reprint : System.Web.UI.Page
{
    ReportController report = new ReportController();
    ShopController shop = new ShopController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtFromDate.Text = CookieContext.WorkingDate.ToString("dd-MMM-yyyy");
            this.txtToDate.Text = CookieContext.WorkingDate.ToString("dd-MMM-yyyy");

            DataTable dtShop = shop.GetShop(1);
            if (dtShop.Rows.Count > 0)
            {
                hfInvoiceFooterNote.Value = dtShop.Rows[0]["InvoiceFooterNote"].ToString();
                hfInvoiceFooterNoteShort.Value = dtShop.Rows[0]["InvoiceFooterNoteShort"].ToString();
                hfAddress.Value = dtShop.Rows[0]["Address"].ToString();
                hfAddressShort.Value = dtShop.Rows[0]["AddressShort"].ToString();
                hfPhone.Value = dtShop.Rows[0]["ConactNumber"].ToString();
            }
        }
    }

    private void GetRollbackData()
    {
        gvSale.DataSource = null;
        gvSale.DataBind();        
        DataTable dtRollback = report.GetSalesReport(0,0, Convert.ToDateTime(txtFromDate.Text),Convert.ToDateTime(txtToDate.Text),6);
        if (dtRollback.Rows.Count > 0)
        {
            gvSale.DataSource = dtRollback;
            gvSale.DataBind();
        }
    }

    protected void btnGetInvoices_Click(object sender, EventArgs e)
    {
        GetRollbackData();
    }

    protected void gvSale_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton btnPrint = (LinkButton)e.Row.FindControl("btnPrint");
            string SaleID = DataBinder.Eval(e.Row.DataItem, "SaleID").ToString();
            btnPrint.OnClientClick = "return printInvoice(" + SaleID + ");";
        }
    }

    [WebMethod]
    [ScriptMethod]
    public static string GetInvoice(int SaleID)
    {
        SaleController Sale = new SaleController();
        DataTable dt = new DataTable();
        if (SaleID > 0)
        {
            dt = Sale.GetSale(SaleID, CookieContext.DBServer);
        }
        return GetJson(dt);
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