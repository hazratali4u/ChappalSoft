using System;
using System.Data;
using ChappalBLL;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class Color : System.Web.UI.Page
{
    ColorController color = new ColorController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadUser();
        }
    }

    private void LoadUser()
    {
        DataTable dtUser = color.GetColor(1);
        gvColor.DataSource = dtUser;
        gvColor.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        if (txtName.Text.Length > 0 )
        {
            try
            {
                string selectedColor = colorPicker.Value;
                if (btnSave.Text == "Save")
                {
                    if (color.InsertUpdateColor(0, txtName.Text, selectedColor,cbShowName.Checked, 1) > 0)
                    {
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "Color saved Sucessfully!";
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
                    if (color.InsertUpdateColor(Convert.ToInt32(hfColorID.Value), txtName.Text, selectedColor,cbShowName.Checked, 2) > 0)
                    {
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "Color updated Sucessfully!";
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

    protected void gvColor_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        divView.Visible = false;
        divAdd.Visible = true;
        hfColorID.Value = gvColor.Rows[e.NewEditIndex].Cells[0].Text;
        txtName.Text = gvColor.Rows[e.NewEditIndex].Cells[1].Text;
        colorPicker.Value = gvColor.Rows[e.NewEditIndex].Cells[3].Text;
        cbShowName.Checked = Convert.ToBoolean(gvColor.Rows[e.NewEditIndex].Cells[4].Text);
        btnSave.Text = "Update";
    }

    protected void gvColor_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        lblError2.Text = string.Empty;
        try
        {
            int TypeID = 3;
            if(gvColor.Rows[e.RowIndex].Cells[2].Text == "Inactive")
            {
                TypeID = 4;
            }
            if (color.InsertUpdateColor(Convert.ToInt32(gvColor.Rows[e.RowIndex].Cells[0].Text), null,null,false, TypeID) > 0)
            {
                LoadUser();
                lblError2.ForeColor = System.Drawing.Color.Green;
                lblError2.Text = "Color deleted Sucessfully!";
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

    protected void gvColor_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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