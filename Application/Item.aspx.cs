using System;
using System.Data;
using ChappalBLL;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;

public partial class Item : System.Web.UI.Page
{
    CategoryController category = new CategoryController();
    ItemController item = new ItemController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCategory();
            LoadItem();
        }
    }

    private void LoadCategory()
    {
        DataTable dtCategory = category.GetCategory(2);
        ddlCategory.DataSource = dtCategory;
        ddlCategory.DataTextField = "Name";
        ddlCategory.DataValueField = "CategoryID";
        ddlCategory.DataBind();
    }

    private void LoadItem()
    {
        DataTable dtItem = item.GetItem(1);
        gvItem.DataSource = dtItem;
        gvItem.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        if (ddlCategory.Items.Count == 0)
        {
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Text = "Category not found.";
            return;
        }
        if (txtName.Text.Length > 0)
        {
            try
            {
                if (btnSave.Text == "Save")
                {
                    if (item.InsertUpdateItem(0, Convert.ToInt32(ddlCategory.SelectedValue), txtName.Text, 1) > 0)
                    {
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "Item saved Sucessfully!";
                        LoadItem();
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
                    if (item.InsertUpdateItem(Convert.ToInt32(hfItemID.Value), Convert.ToInt32(ddlCategory.SelectedValue), txtName.Text, 2) > 0)
                    {
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "Item updated Sucessfully!";
                        LoadItem();
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
        txtSearch.Focus();
    }

    protected void gvItem_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        divView.Visible = false;
        divAdd.Visible = true;
        hfItemID.Value = gvItem.Rows[e.NewEditIndex].Cells[0].Text;
        txtName.Text = gvItem.Rows[e.NewEditIndex].Cells[2].Text;
        ddlCategory.SelectedValue = gvItem.Rows[e.NewEditIndex].Cells[4].Text;
        btnSave.Text = "Update";
    }

    protected void gvItem_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        lblError2.Text = string.Empty;
        try
        {
            int TypeID = 3;
            if (gvItem.Rows[e.RowIndex].Cells[3].Text == "Inactive")
            {
                TypeID = 4;
            }
            if (item.InsertUpdateItem(Convert.ToInt32(gvItem.Rows[e.RowIndex].Cells[0].Text), 0, null, TypeID) > 0)
            {
                LoadItem();
                lblError2.ForeColor = System.Drawing.Color.Green;
                lblError2.Text = "Item deleted Sucessfully!";
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

    protected void gvItem_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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