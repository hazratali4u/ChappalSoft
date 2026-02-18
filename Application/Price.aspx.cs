using System;
using System.Data;
using ChappalBLL;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;

public partial class Price : System.Web.UI.Page
{
    CategoryController category = new CategoryController();
    ItemPriceController itemprice = new ItemPriceController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadItem();
        }
    }

    private void LoadItem()
    {
        DataTable dtItem = itemprice.GetItemPrice(1);
        gvPrice.DataSource = dtItem;
        gvPrice.DataBind();
    }

    protected void gvPrice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string ItemPrice = DataBinder.Eval(e.Row.DataItem, "ItemPrice").ToString();
            TextBox txtPrice = (TextBox)e.Row.FindControl("txtPrice");
            txtPrice.Text = ItemPrice;

            string ItemWSPrice = DataBinder.Eval(e.Row.DataItem, "ItemWSPrice").ToString();
            TextBox txtWSPrice = (TextBox)e.Row.FindControl("txtWSPrice");
            txtWSPrice.Text = ItemWSPrice;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        LoadItem();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        bool flag = true;
        foreach (GridViewRow gvr in gvPrice.Rows)
        {
            TextBox txtPrice = (TextBox)gvr.Cells[3].FindControl("txtPrice");
            TextBox txtWSPrice = (TextBox)gvr.Cells[5].FindControl("txtWSPrice");

            int Price = 0;
            int WSPrice = 0;
            if (txtPrice.Text.Length > 0)
            {
                Price = Convert.ToInt32(txtPrice.Text);
            }
            if(txtWSPrice.Text.Length > 0)
            {
                WSPrice = Convert.ToInt32(txtWSPrice.Text);
            }
            if (itemprice.InsertUpdateItemPrice(Convert.ToInt32(gvr.Cells[0].Text), Price, WSPrice, CookieContext.UserID, CookieContext.DBServer) > 0)
            {
                flag = true;
            }
            else
            {
                flag = false;
                break;
            }
        }
        if(flag)
        {
            lblError.ForeColor = System.Drawing.Color.Green;
            lblError.Text = "Item Price saved Sucessfully!";
        }
        else
        {
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Text = "Some error occured.";
        }
    }
}