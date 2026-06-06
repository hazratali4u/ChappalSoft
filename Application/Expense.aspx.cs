using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using ChappalBLL;

public partial class Expense : System.Web.UI.Page
{
    ExpenseController expense = new ExpenseController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            LoadExpenseHead();
            LoadExpense();
        }
    }

    private void LoadExpense()
    {
        DataTable dtUser = expense.GetExpense(1);
        gvExpense.DataSource = dtUser;
        gvExpense.DataBind();
    }
    private void LoadExpenseHead()
    {
        DataTable dtData = expense.GetExpenseHead(2);
        ddlHead.DataSource = dtData;
        ddlHead.DataTextField = "Name";
        ddlHead.DataValueField = "ExpenseHeadID";
        ddlHead.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        if (txtAmount.Text.Length > 0 )
        {
            try
            {
                if (btnSave.Text == "Save")
                {
                    if (expense.InsertUpdateExpense(0, Convert.ToInt32(ddlHead.SelectedValue), Convert.ToDateTime(txtFromDate.Text), Convert.ToDecimal(txtAmount.Text), txtRemarks.Text, CookieContext.UserID, 1)> 0)
                    {
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "Expense saved Sucessfully!";
                        LoadExpense();
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
                    if (expense.InsertUpdateExpense(Convert.ToInt32(hfExpenseID.Value), Convert.ToInt32(ddlHead.SelectedValue), Convert.ToDateTime(txtFromDate.Text), Convert.ToDecimal(txtAmount.Text), txtRemarks.Text, CookieContext.UserID, 2) > 0)
                    {
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "Expense updated Sucessfully!";
                        LoadExpense();
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
        txtAmount.Focus();
        btnSave.Text = "Save";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        divView.Visible = true;
        divAdd.Visible = false;
        txtSearch.Focus();
    }

    protected void gvExpense_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        divView.Visible = false;
        divAdd.Visible = true;
        hfExpenseID.Value = gvExpense.Rows[e.NewEditIndex].Cells[0].Text;
        ddlHead.SelectedValue = gvExpense.Rows[e.NewEditIndex].Cells[1].Text;
        txtFromDate.Text = Convert.ToDateTime(gvExpense.Rows[e.NewEditIndex].Cells[2].Text).ToString("dd-MMM-yyyy");
        txtAmount.Text = gvExpense.Rows[e.NewEditIndex].Cells[4].Text;        
        txtRemarks.Text = gvExpense.Rows[e.NewEditIndex].Cells[5].Text.Replace("&nbsp;", "");        
        btnSave.Text = "Update";
    }

    protected void gvExpense_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        lblError2.Text = string.Empty;
        try
        {
            if (expense.InsertUpdateExpense(Convert.ToInt32(gvExpense.Rows[e.RowIndex].Cells[0].Text), 0, DateTime.Now, 0, string.Empty, CookieContext.UserID, 3) > 0)
            {
                LoadExpense();
                lblError2.ForeColor = System.Drawing.Color.Green;
                lblError2.Text = "Expense Deleted Sucessfully!";
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
    
    private void Clear()
    {
        txtAmount.Text = string.Empty;
        txtRemarks.Text = string.Empty;        
        txtSearch.Text = string.Empty;
        txtSearch.Focus();
        btnSave.Text = "Save";
    }
}