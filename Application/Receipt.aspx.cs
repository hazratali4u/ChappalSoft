using System;
using System.Data;
using ChappalBLL;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Linq;

public partial class Receipt : System.Web.UI.Page
{
    PartyController party = new PartyController();
    ReceiptController receipt = new ReceiptController();
    SaleController Sale = new SaleController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hfUserID.Value = CookieContext.UserID.ToString();
            hfWorkingDate.Value = CookieContext.WorkingDate.ToString();
            LoadCustomer();     
        }
    }

    private void LoadCustomer()
    {
        DataTable dtCustomer = party.GetParty(4);
        DataTable distinctTable = dtCustomer.AsEnumerable()
                                    .GroupBy(row => row["PartyID"])
                                    .Select(g => g.First())
                                    .CopyToDataTable();
        ddlCustomer.DataSource = distinctTable;
        ddlCustomer.DataTextField = "Name";
        ddlCustomer.DataValueField = "PartyID";
        ddlCustomer.DataBind();
        Session.Add("dtCustomer", dtCustomer);
    }

    private void GetReceipt()
    {
        DataTable dtReceipt = receipt.GetReceipt(1, Convert.ToInt32(ddlCustomer.SelectedValue), CookieContext.WorkingDate);
        gvReceipt.DataSource = dtReceipt;
        gvReceipt.DataBind();
    }

    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtCustomer = (DataTable)Session["dtCustomer"];
        lblTotal.Text = string.Empty;
        GetReceipt();
        if (ddlCustomer.SelectedValue == "0")
        {
            gvCustomer.DataSource = null;
            gvCustomer.DataBind();
        }
        else
        {            
            int total = 0;
            DataRow[] filteredRows = dtCustomer.Select("PartyID = " + ddlCustomer.SelectedValue);
            DataTable filteredTable = dtCustomer.Clone();
            foreach (DataRow row in filteredRows)
            {
                filteredTable.ImportRow(row);
                total += Convert.ToInt32(row["LedgerAmount"]);
            }
            gvCustomer.DataSource = filteredTable;
            gvCustomer.DataBind();
            if(total > 0)
            {
                lblTotal.Text = "Total Amount:" + total.ToString();
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        int amount = 0;
        try
        {
            amount = Convert.ToInt32(txtAmount.Text);
        }
        catch (Exception ex)
        {
            amount = 0;
        }
        if(amount ==0)
        {
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Text = "Amount can not be zero.";
            return;
        }
        if(ddlCustomer.SelectedValue == "0")
        {
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Text = "Select Customer.";
            return;
        }
        int realizeAmount = 0;
        int OfferAmount = Convert.ToInt32(txtAmount.Text);
        string RecordID = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        foreach (GridViewRow gvr in gvCustomer.Rows)
        {
            if (Convert.ToInt32(gvr.Cells[3].Text) >= OfferAmount)
            {
                realizeAmount += OfferAmount;
                receipt.InsertUpdateReceipt(RecordID,Convert.ToInt32(ddlCustomer.SelectedValue), Convert.ToInt32(gvr.Cells[0].Text), Convert.ToInt32(ddlPaymentMode.SelectedValue), OfferAmount,txtRemarks.Text, Convert.ToInt32(hfUserID.Value), 1,Convert.ToDateTime(hfWorkingDate.Value), CookieContext.DBServer);                
                //Update Sale
                Sale.UpdateSale(Convert.ToInt32(gvr.Cells[0].Text), OfferAmount, 1);
                OfferAmount = Convert.ToInt32(gvr.Cells[3].Text) - OfferAmount;
                break;
            }
            else if (Convert.ToInt32(gvr.Cells[3].Text) <= OfferAmount)
            {
                realizeAmount += Convert.ToInt32(gvr.Cells[3].Text);
                receipt.InsertUpdateReceipt(RecordID, Convert.ToInt32(ddlCustomer.SelectedValue), Convert.ToInt32(gvr.Cells[0].Text), Convert.ToInt32(ddlPaymentMode.SelectedValue), Convert.ToInt32(gvr.Cells[3].Text), txtRemarks.Text, Convert.ToInt32(hfUserID.Value), 1,Convert.ToDateTime(hfWorkingDate.Value), CookieContext.DBServer);
                //Update Sale
                Sale.UpdateSale(Convert.ToInt32(gvr.Cells[0].Text), Convert.ToInt32(gvr.Cells[3].Text), 1);
                OfferAmount = OfferAmount - Convert.ToInt32(gvr.Cells[3].Text);                
            }
        }
        //Advance
        if (realizeAmount < Convert.ToInt32(txtAmount.Text))
        {
            receipt.InsertUpdateReceipt(RecordID, Convert.ToInt32(ddlCustomer.SelectedValue), 0, Convert.ToInt32(ddlPaymentMode.SelectedValue), OfferAmount,"Advance", Convert.ToInt32(hfUserID.Value), 1,Convert.ToDateTime(hfWorkingDate.Value), CookieContext.DBServer);
        }
        LoadCustomer();
        ClearControls();
        lblError.ForeColor = System.Drawing.Color.Green;
        lblError.Text = "Receipt saved successfully.";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearControls();
    }

    private void ClearControls()
    {
        ddlPaymentMode.SelectedValue = "1";
        txtAmount.Text = string.Empty;
        txtRemarks.Text = string.Empty;
        ddlCustomer.SelectedValue = "0";
        ddlCustomer_SelectedIndexChanged(null, null);
    }

    protected void gvReceipt_RowEditing(object sender, GridViewEditEventArgs e)
    {
        string RecordID = gvReceipt.Rows[e.NewEditIndex].Cells[0].Text;
        if(receipt.InsertUpdateReceipt(RecordID, 0, 0, 0, 0, string.Empty, Convert.ToInt32(hfUserID.Value), 2, Convert.ToDateTime(hfWorkingDate.Value), CookieContext.DBServer) >0)
        {
            ClearControls();
            lblError.ForeColor = System.Drawing.Color.Green;
            lblError.Text = "Receipt deleted successfully.";
        }
        else
        {
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Text = "Some error occured.";
        }
    }
}