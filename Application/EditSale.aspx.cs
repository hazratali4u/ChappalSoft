using System;
using System.Data;
using ChappalBLL;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;

public partial class EditSale : System.Web.UI.Page
{
    ReportController report = new ReportController();
    SaleController sale = new SaleController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime WorkingDate = CookieContext.WorkingDate;
            this.txtFromDate.Text = WorkingDate.ToString("dd-MMM-yyyy");
            this.txtToDate.Text = WorkingDate.ToString("dd-MMM-yyyy");            
        }
    }

    private void GetRollbackData()
    {
        gvSale.DataSource = null;
        gvSale.DataBind();        
        DataTable dtRollback = report.GetSalesReport(0,0, Convert.ToDateTime(txtFromDate.Text),Convert.ToDateTime(txtToDate.Text),7);
        if (dtRollback.Rows.Count > 0)
        {
            gvSale.DataSource = dtRollback;
            gvSale.DataBind();
        }
    }

    protected void btnGetInvoices_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        DateTime dt;
        if (DateTime.TryParse(txtFromDate.Text, out dt))
        {
            if (dt < DateTime.Today.AddMonths(-1))
            {
                lblError.Text = "Please select From Date within the last 1 month only..";
                return;
            }
        }
        GetRollbackData();
    }    

    protected void gvSale_RowEditing(object sender, GridViewEditEventArgs e)
    {
        int SaleID = Convert.ToInt32(gvSale.Rows[e.NewEditIndex].Cells[0].Text);
        hfSaleID.Value = SaleID.ToString();
        dvView.Visible = false;
        dvEdit.Visible = true;
        string TimeStamp = gvSale.Rows[e.NewEditIndex].Cells[1].Text;
        hfDate.Value = TimeStamp;
        string InvoiceNo = gvSale.Rows[e.NewEditIndex].Cells[2].Text;
        string CustomerName = gvSale.Rows[e.NewEditIndex].Cells[3].Text;
        string dayName = Convert.ToDateTime(TimeStamp).DayOfWeek.ToString();
        string firstChar = dayName[0].ToString();
        int dummyinvoiceno = 1718 + SaleID;
        lblInvoiceNo.Text = "Invoice #: " + InvoiceNo + firstChar + dummyinvoiceno.ToString();
        lblCustomerName.Text = "Customer Name: " + CustomerName;
        lblDate.Text = "Date: " + Convert.ToDateTime(TimeStamp).ToString("dd-MMM-yyyy");
        LoadInvoiceDetail(SaleID);
    }

    private void LoadInvoiceDetail(int SaleID)
    {
        DataTable dtDetail = sale.GetSaleDetail(SaleID);
        if(dtDetail.Rows.Count > 0)
        {            
            gvDetail.DataSource = dtDetail;
            gvDetail.DataBind();
        }
    }

    protected void btnDone_Click(object sender, EventArgs e)
    {
        int Qty = 0;
        int QtyNew = 0;
        int Price = 0;
        int PriceNew = 0;
        DataTable dtItem = new DataTable();
        dtItem.Columns.Add("ItemID", typeof(int));
        dtItem.Columns.Add("ColorID", typeof(int));
        dtItem.Columns.Add("SizeID", typeof(int));
        dtItem.Columns.Add("Price", typeof(int));
        dtItem.Columns.Add("Quantity", typeof(int));
        dtItem.Columns.Add("QuantityStock", typeof(int));
        dtItem.Columns.Add("Amount", typeof(int));
        dtItem.Columns.Add("StockDate", typeof(DateTime));
        foreach (GridViewRow gvr in gvDetail.Rows)
        {
            Qty = 0;
            QtyNew = 0;
            Price = 0;
            PriceNew = 0;
            TextBox txtQuantity = (TextBox)gvr.Cells[6].FindControl("txtQuantity");
            Qty = Convert.ToInt32(gvr.Cells[5].Text);
            if (txtQuantity.Text.Length > 0)
            {
                QtyNew = Convert.ToInt32(txtQuantity.Text);
            }
            TextBox txtPrice = (TextBox)gvr.Cells[4].FindControl("txtPrice");
            Price = Convert.ToInt32(gvr.Cells[11].Text);
            if (txtPrice.Text.Length > 0)
            {
                PriceNew = Convert.ToInt32(txtPrice.Text);
            }

            DataRow dr = dtItem.NewRow();
            dr["ItemID"] = gvr.Cells[8].Text;
            dr["ColorID"] = gvr.Cells[9].Text;
            dr["SizeID"] = gvr.Cells[10].Text;
            dr["Price"] = PriceNew;
            dr["Quantity"] = QtyNew;
            dr["QuantityStock"] = Qty - QtyNew;
            dr["Amount"] = QtyNew * PriceNew;
            dr["StockDate"] = CookieContext.WorkingDate;
            dtItem.Rows.Add(dr);
        }
        if (dtItem.Rows.Count > 0)
        {
            RollbackController rollBack = new RollbackController();
            if (rollBack.RollbackData(1, Convert.ToInt32(hfSaleID.Value), "", Convert.ToDateTime(hfDate.Value), CookieContext.UserID, CookieContext.DBServer) > 0)
            {
                if (sale.UpdateSale(Convert.ToInt32(hfSaleID.Value), dtItem, CookieContext.UserID, Convert.ToDateTime(hfDate.Value), CookieContext.DBServer))
                {
                    dvView.Visible = true;
                    dvEdit.Visible = false;
                }
            }
        }
    }

    protected void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string ItemQuantity = DataBinder.Eval(e.Row.DataItem, "Quantity").ToString();
            TextBox txtQuantity = (TextBox)e.Row.FindControl("txtQuantity");
            txtQuantity.Text = ItemQuantity;

            string ItemPrice = DataBinder.Eval(e.Row.DataItem, "Price").ToString();
            TextBox txtPrice = (TextBox)e.Row.FindControl("txtPrice");
            txtPrice.Text = ItemPrice;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        dvView.Visible = true;
        dvEdit.Visible = false;
    }
}