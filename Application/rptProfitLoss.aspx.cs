using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using ChappalBLL;

public partial class rptProfitLoss : System.Web.UI.Page
{
    ShopController shop = new ShopController();
    ReportController report = new ReportController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            GetShopData();
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
    protected void btnGetData_Click(object sender, EventArgs e)
    {
        DataTable dtStock = report.GetProfitLossReport(1, Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text));
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(dtStock);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "showPreview",
          string.Format("showPrintPreview({0});", json), true);
    }
}