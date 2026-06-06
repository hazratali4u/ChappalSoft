using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using ChappalBLL;

public partial class rptExpense : System.Web.UI.Page
{
    ExpenseController expense = new ExpenseController();
    ShopController shop = new ShopController();
    ReportController report = new ReportController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            GetShopData();
            LoadExpenseHead();
        }
    }

    private void GetShopData()
    {
        DataTable dtShop = shop.GetShop(1);
        hfShopName.Value = dtShop.Rows[0]["Name"].ToString();
        hfAddress.Value = dtShop.Rows[0]["Address"].ToString();
        hfPhone.Value = dtShop.Rows[0]["ConactNumber"].ToString();
        hfContactPerson.Value = dtShop.Rows[0]["ConactPerson"].ToString();
    }
    private void LoadExpenseHead()
    {
        DataTable dtHead = expense.GetExpenseHead(2);
        DataRow drSelectAll = dtHead.NewRow();
        drSelectAll["Name"] = "---All---";
        drSelectAll["ExpenseHeadID"] = 0;
        dtHead.Rows.InsertAt(drSelectAll, 0);

        ddlExpenseHead.DataSource = dtHead;
        ddlExpenseHead.DataTextField = "Name";
        ddlExpenseHead.DataValueField = "ExpenseHeadID";
        ddlExpenseHead.DataBind();
    }
    protected void btnGetData_Click(object sender, EventArgs e)
    {
        DataTable dtStock = report.GetExpenseReport(Convert.ToInt32(rblReportType.SelectedValue), Convert.ToInt32(ddlExpenseHead.SelectedValue), Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text));
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(dtStock);
        if (rblReportType.SelectedValue == "1")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showPreview",
              string.Format("showPrintPreview({0});", json), true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showPreviewDetail",
              string.Format("showPrintPreviewDetail({0});", json), true);
        }
    }
}