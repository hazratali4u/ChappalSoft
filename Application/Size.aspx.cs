using System;
using System.Data;
using ChappalBLL;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class Size : System.Web.UI.Page
{
    SizeController size = new SizeController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadUser();
        }
    }

    private void LoadUser()
    {
        DataTable dtUser = size.GetSize(1);
        gvSize.DataSource = dtUser;
        gvSize.DataBind();
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
                    if (size.InsertUpdateSize(0, Convert.ToInt32(txtName.Text),1) > 0)
                    {
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "Size saved Sucessfully!";
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
                    if (size.InsertUpdateSize(Convert.ToInt32(hfSizeID.Value), Convert.ToInt32(txtName.Text),2) > 0)
                    {
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "Size updated Sucessfully!";
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
        txtSearch.Focus();
    }

    protected void gvSize_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        divView.Visible = false;
        divAdd.Visible = true;
        hfSizeID.Value = gvSize.Rows[e.NewEditIndex].Cells[0].Text;
        txtName.Text = gvSize.Rows[e.NewEditIndex].Cells[1].Text;
        btnSave.Text = "Update";
    }

    protected void gvSize_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        lblError2.Text = string.Empty;
        try
        {
            int TypeID = 3;
            if(gvSize.Rows[e.RowIndex].Cells[2].Text == "Inactive")
            {
                TypeID = 4;
            }
            if (size.InsertUpdateSize(Convert.ToInt32(gvSize.Rows[e.RowIndex].Cells[0].Text), 0, TypeID) > 0)
            {
                LoadUser();
                lblError2.ForeColor = System.Drawing.Color.Green;
                lblError2.Text = "Size deleted Sucessfully!";
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

    protected void gvSize_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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