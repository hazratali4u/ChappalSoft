using System;
using System.Data;
using ChappalBLL;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class User : System.Web.UI.Page
{
    UserController user = new UserController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadUser();
        }
    }

    private void LoadUser()
    {
        DataTable dtUser = user.GetUser(txtUsername.Text, txtPassword.Text, 2, CookieContext.DBServer);
        gvUsers.DataSource = dtUser;
        gvUsers.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        if (txtUsername.Text.Length > 0 && txtPassword.Text.Length > 0 && txtEmployeeName.Text.Length > 0)
        {
            try
            {
                if (btnSave.Text == "Save")
                {
                    if (user.InsertUpdateUser(0,txtEmployeeName.Text, txtUsername.Text, txtPassword.Text, Convert.ToInt32(rblRole.SelectedValue), CookieContext.UserID, 1, CookieContext.DBServer) > 0)
                    {
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "User saved Sucessfully!";
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
                    if (user.InsertUpdateUser(Convert.ToInt32(hfUserID.Value),txtEmployeeName.Text, txtUsername.Text, txtPassword.Text, Convert.ToInt32(rblRole.SelectedValue), CookieContext.UserID, 2, CookieContext.DBServer) > 0)
                    {
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "User updated Sucessfully!";
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
        txtEmployeeName.Focus();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        divView.Visible = true;
        divAdd.Visible = false;
        txtSearch.Focus();
    }

    protected void gvUsers_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        divView.Visible = false;
        divAdd.Visible = true;
        hfUserID.Value = gvUsers.Rows[e.NewEditIndex].Cells[0].Text;
        txtEmployeeName.Text = gvUsers.Rows[e.NewEditIndex].Cells[1].Text;
        txtUsername.Text = gvUsers.Rows[e.NewEditIndex].Cells[2].Text;
        txtPassword.Text = gvUsers.Rows[e.NewEditIndex].Cells[5].Text;
        if (gvUsers.Rows[e.NewEditIndex].Cells[3].Text == "Admin")
        {
            rblRole.SelectedValue = "1";
        }
        else
        {
            rblRole.SelectedValue = "2";
        }
        btnSave.Text = "Update";
    }

    protected void gvUsers_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        lblError2.Text = string.Empty;
        try
        {
            int TypeID = 3;
            if(gvUsers.Rows[e.RowIndex].Cells[3].Text == "Inactive")
            {
                TypeID = 4;
            }
            if (user.InsertUpdateUser(Convert.ToInt32(gvUsers.Rows[e.RowIndex].Cells[0].Text),null, null, null, 0, CookieContext.UserID, TypeID, CookieContext.DBServer) > 0)
            {
                LoadUser();
                lblError2.ForeColor = System.Drawing.Color.Green;
                lblError2.Text = "User deleted Sucessfully!";
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

    protected void gvUsers_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
        txtUsername.Text = string.Empty;
        txtPassword.Text = string.Empty;
        rblRole.SelectedValue = "2";
        txtSearch.Focus();
    }
}