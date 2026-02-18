using System;
using System.Data;
using ChappalBLL;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;

public partial class Party : System.Web.UI.Page
{
    PartyController party = new PartyController();
    SaleController Sale = new SaleController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadParty();
        }
    }

    private void LoadParty()
    {
        DataTable dtCustomer = party.GetParty(1);
        gvCustomer.DataSource = dtCustomer;
        gvCustomer.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        if (txtName.Text.Length > 0)
        {
            int OpeningBalance = 0;
            if (txtOpeningBalance.Text.Length > 0)
            {
                try
                {
                    OpeningBalance = Convert.ToInt32(txtOpeningBalance.Text);
                }
                catch (Exception ex)
                {
                    OpeningBalance = 0;
                }
            }
            try
            {
                if (btnSave.Text == "Save")
                {
                    if (party.InsertUpdateParty(0, txtName.Text, txtAddress.Text, txtContact.Text, Convert.ToInt32(rblType.SelectedValue), OpeningBalance, CookieContext.UserID, 1, CookieContext.DBServer) > 0)
                    {                        
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "party saved Sucessfully!";
                        LoadParty();
                        Clear();
                        divView.Visible = true;
                        divAdd.Visible = false;
                    }
                    else
                    {
                        lblError.ForeColor = System.Drawing.Color.Red;
                        lblError.Text = "Some error occured.";
                    }
                }
                else
                {
                    if (party.InsertUpdateParty(Convert.ToInt32(hfPartyID.Value), txtName.Text, txtAddress.Text, txtContact.Text, Convert.ToInt32(rblType.SelectedValue), OpeningBalance, CookieContext.UserID, 2, CookieContext.DBServer) > 0)
                    {
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "party updated Sucessfully!";
                        LoadParty();
                        Clear();
                        divView.Visible = true;
                        divAdd.Visible = false;
                        txtOpeningBalance.Enabled= true;
                    }
                    else
                    {
                        lblError.ForeColor = System.Drawing.Color.Red;
                        lblError.Text = "Some error occured.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.ForeColor = System.Drawing.Color.Red;
                lblError.Text = ex.Message;
            }
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        lblError2.Text = string.Empty;
        divView.Visible = false;
        divAdd.Visible = true;
        txtName.Focus();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        divView.Visible = true;
        divAdd.Visible = false;
        txtOpeningBalance.Enabled = true;
        txtSearch.Focus();
    }

    protected void gvCustomer_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        txtOpeningBalance.Enabled = false;
        divView.Visible = false;
        divAdd.Visible = true;
        hfPartyID.Value = gvCustomer.Rows[e.NewEditIndex].Cells[0].Text;
        txtName.Text = gvCustomer.Rows[e.NewEditIndex].Cells[1].Text.Replace("&nbsp;", "");
        txtAddress.Text = gvCustomer.Rows[e.NewEditIndex].Cells[2].Text.Replace("&nbsp;", "");
        txtContact.Text = gvCustomer.Rows[e.NewEditIndex].Cells[3].Text.Replace("&nbsp;", "");
        txtOpeningBalance.Text = gvCustomer.Rows[e.NewEditIndex].Cells[5].Text.Replace("&nbsp;", "");
        rblType.SelectedValue = "1";
        if (gvCustomer.Rows[e.NewEditIndex].Cells[6].Text == "Supplier")
        {
            rblType.SelectedValue = "2";
        }
        rblType.Enabled = false;
        btnSave.Text = "Update";
    }

    protected void gvCustomer_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        lblError2.Text = string.Empty;
        try
        {
            int TypeID = 3;
            if (gvCustomer.Rows[e.RowIndex].Cells[4].Text == "Inactive")
            {
                TypeID = 4;
            }
            if (party.InsertUpdateParty(Convert.ToInt32(gvCustomer.Rows[e.RowIndex].Cells[0].Text), null, null, null, 0, 0, CookieContext.UserID, TypeID, CookieContext.DBServer) > 0)
            {
                LoadParty();
                lblError2.ForeColor = System.Drawing.Color.Green;
                lblError2.Text = "party deleted Sucessfully!";
            }
            else
            {
                lblError2.ForeColor = System.Drawing.Color.Red;
                lblError2.Text = "Some error occured.";
            }
        }
        catch (Exception ex)
        {
            lblError2.ForeColor = System.Drawing.Color.Red;
            lblError2.Text = ex.Message;
        }
    }

    protected void gvCustomer_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton btnDelete = (LinkButton)e.Row.FindControl("btnDelete");
            string status = DataBinder.Eval(e.Row.DataItem, "Status").ToString();
            if (status == "Active")
            {
                btnDelete.Text = "Inactive";
            }
            else if (status == "Inactive")
            {
                btnDelete.Text = "Active";
            }
        }
    }

    private void Clear()
    {
        txtName.Text = string.Empty;
        txtSearch.Text = string.Empty;
        rblType.Enabled = true;
        txtSearch.Focus();
    }

    protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtOpeningBalance.Enabled = true;
        if (rblType.SelectedItem.Value == "2")
        {
            txtOpeningBalance.Enabled = false;
        }
    }
}