using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using ChappalBLL;

public partial class ExpenseHead : System.Web.UI.Page
{
    ExpenseController expense = new ExpenseController();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadExpenseHead();
        }
    }

    private void LoadExpenseHead()
    {
        DataTable dtUser = expense.GetExpenseHead(1);
        gvExpenseHead.DataSource = dtUser;
        gvExpenseHead.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        if (txtName.Text.Length > 0 )
        {
            try
            {
                if (btnSave.Text == "Save")
                {
                    if (expense.InsertUpdateExpenseHead(0, txtName.Text, CookieContext.UserID, 1) > 0)
                    {
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "expense head saved sucessfully!";
                        LoadExpenseHead();
                        Clear();
                        divView.Visible = true;
                        divAdd.Visible = false;
                    }
                    else
                    {
                        lblError.ForeColor = System.Drawing.Color.Red;
                        lblError.Text = "some error occured.";
                    }
                }
                else
                {
                    if (expense.InsertUpdateExpenseHead(Convert.ToInt32(hfExpenseHeadID.Value), txtName.Text, CookieContext.UserID, 2) > 0)
                    {
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "Expense Head updated Sucessfully!";
                        LoadExpenseHead();
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
        txtName.Focus();
        btnSave.Text = "Save";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        divView.Visible = true;
        divAdd.Visible = false;
        txtSearch.Focus();
    }

    protected void gvExpenseHead_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        divView.Visible = false;
        divAdd.Visible = true;
        hfExpenseHeadID.Value = gvExpenseHead.Rows[e.NewEditIndex].Cells[0].Text;
        txtName.Text = gvExpenseHead.Rows[e.NewEditIndex].Cells[1].Text;
        btnSave.Text = "Update";
    }

    protected void gvExpenseHead_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        lblError2.Text = string.Empty;
        try
        {
            int TypeID = 3;
            if(gvExpenseHead.Rows[e.RowIndex].Cells[2].Text == "Inactive")
            {
                TypeID = 4;
            }
            if (expense.InsertUpdateExpenseHead(Convert.ToInt32(gvExpenseHead.Rows[e.RowIndex].Cells[0].Text), string.Empty, CookieContext.UserID, TypeID) > 0)
            {
                LoadExpenseHead();
                lblError2.ForeColor = System.Drawing.Color.Green;
                if (TypeID == 3)
                {
                    lblError2.Text = "Expense Head Inactive Sucessfully!";
                }
                else
                {
                    lblError2.Text = "Expense Head Active Sucessfully!";
                }
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

    protected void gvExpenseHead_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
        txtSearch.Focus();
        btnSave.Text = "Save";
    }
}