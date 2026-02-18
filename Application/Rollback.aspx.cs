using System;
using System.Data;
using ChappalBLL;
using System.Web.UI.WebControls;

public partial class Rollback : System.Web.UI.Page
{
    RollbackController rollback = new RollbackController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtFromDate.Text = CookieContext.WorkingDate.ToString("dd-MMM-yyyy");
        }
    }

    protected void btnGetDate_Click(object sender, EventArgs e)
    {
        GetRollbackData();
    }
    private void GetRollbackData()
    {
        lblError.Text = string.Empty;
        gvSale.Visible = false;
        gvSale.DataSource = null;
        gvSale.DataBind();

        gvPurchase.Visible = false;
        gvPurchase.DataSource = null;
        gvPurchase.DataBind();

        gvReceipt.Visible = false;
        gvPurchase.DataSource = null;
        gvReceipt.DataBind();

        DataTable dtRollback = rollback.GetRollbackData(Convert.ToInt32(ddlType.SelectedItem.Value), Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtFromDate.Text));
        if (dtRollback.Rows.Count > 0)
        {
            if (ddlType.SelectedValue == "1")
            {
                gvSale.Visible = true;
                gvSale.DataSource = dtRollback;
                gvSale.DataBind();
            }
            else if (ddlType.SelectedValue == "2")
            {
                gvPurchase.Visible = true;
                gvPurchase.DataSource = dtRollback;
                gvPurchase.DataBind();
            }
            else if (ddlType.SelectedValue == "3")
            {
                gvReceipt.Visible = true;
                gvReceipt.DataSource = dtRollback;
                gvReceipt.DataBind();
            }
        }
    }
    protected void btnRollback_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        bool flag = true;
        int count = 0;
        if (ddlType.SelectedValue == "1")
        {
            foreach (GridViewRow gvr in gvSale.Rows)
            {
                CheckBox cbInvoice = (CheckBox)gvr.Cells[1].FindControl("cbInvoice");
                if(cbInvoice.Checked)
                {
                    if(rollback.RollbackData(1, Convert.ToInt32(gvr.Cells[0].Text),string.Empty,Convert.ToDateTime(txtFromDate.Text), CookieContext.UserID, CookieContext.DBServer) > 0)
                    {
                        count++;                        
                    }
                    else
                    {
                        flag = false;
                        break;
                    }
                }
            }
        }
        else if (ddlType.SelectedValue == "2")
        {
            foreach (GridViewRow gvr in gvPurchase.Rows)
            {
                CheckBox cbInvoice = (CheckBox)gvr.Cells[1].FindControl("cbInvoice");
                if (cbInvoice.Checked)
                {
                    if (rollback.RollbackData(2, Convert.ToInt32(gvr.Cells[0].Text),string.Empty, Convert.ToDateTime(txtFromDate.Text), CookieContext.UserID, CookieContext.DBServer) > 0)
                    {
                        count++;
                    }
                    else
                    {
                        flag = false;
                        break;
                    }
                }
            }
        }
        else if (ddlType.SelectedValue == "3")
        {
            foreach (GridViewRow gvr in gvReceipt.Rows)
            {
                CheckBox cbInvoice = (CheckBox)gvr.Cells[1].FindControl("cbInvoice");
                if (cbInvoice.Checked)
                {
                    if (rollback.RollbackData(3,0, gvr.Cells[0].Text, Convert.ToDateTime(txtFromDate.Text), CookieContext.UserID, CookieContext.DBServer) > 0)
                    {
                        count++;
                    }
                    else
                    {
                        flag = false;
                        break;
                    }
                }
            }
        }
        if (count > 0)
        {
            if(flag)
            {
                lblError.ForeColor = System.Drawing.Color.Green;
                lblError.Text = "Data Rollbacked Sucessfully!";
                GetRollbackData();
            }
            else
            {
                lblError.ForeColor = System.Drawing.Color.Red;
                lblError.Text = "Some error occured!";
            }
        }
    }
}