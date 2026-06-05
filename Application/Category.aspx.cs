using System;
using System.Data;
using ChappalBLL;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class Category : System.Web.UI.Page
{
    CategoryController category = new CategoryController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadUser();
        }
    }

    private void LoadUser()
    {
        DataTable dtUser = category.GetCategory(1);
        gvCategory.DataSource = dtUser;
        gvCategory.DataBind();
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
                    if (category.InsertUpdateCategory(0, txtName.Text, 1) > 0)
                    {
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "Category saved Sucessfully!";
                        LoadUser();
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
                    if (category.InsertUpdateCategory(Convert.ToInt32(hfCategoryID.Value), txtName.Text, 2) > 0)
                    {
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "Category updated Sucessfully!";
                        LoadUser();
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
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        divView.Visible = true;
        divAdd.Visible = false;
        btnSave.Text = "Save";
        Clear();
        txtSearch.Focus();
    }

    protected void gvCategory_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        divView.Visible = false;
        divAdd.Visible = true;
        hfCategoryID.Value = gvCategory.Rows[e.NewEditIndex].Cells[0].Text;
        txtName.Text = gvCategory.Rows[e.NewEditIndex].Cells[1].Text;
        btnSave.Text = "Update";
    }

    protected void gvCategory_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        lblError2.Text = string.Empty;
        try
        {
            int TypeID = 3;
            if(gvCategory.Rows[e.RowIndex].Cells[2].Text == "Inactive")
            {
                TypeID = 4;
            }
            if (category.InsertUpdateCategory(Convert.ToInt32(gvCategory.Rows[e.RowIndex].Cells[0].Text), null, TypeID) > 0)
            {
                LoadUser();
                lblError2.ForeColor = System.Drawing.Color.Green;
                lblError2.Text = "Category deleted Sucessfully!";
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

    protected void gvCategory_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
    }
}