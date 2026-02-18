using System;
using System.Data;
using ChappalBLL;

public partial class Shop : System.Web.UI.Page
{
    ShopController shop = new ShopController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetShop();
        }
    }
    private void GetShop()
    {
        lblError.Text = string.Empty;
        try
        {
            DataTable dtShop = shop.GetShop(1);
            if (dtShop.Rows.Count > 0)
            {
                txtShopName.Text = dtShop.Rows[0]["Name"].ToString();
                txtAddress.Text = dtShop.Rows[0]["Address"].ToString();
                txtShortAddress.Text = dtShop.Rows[0]["AddressShort"].ToString();
                txtContactPerson.Text = dtShop.Rows[0]["ConactPerson"].ToString();
                txtConactNo.Text = dtShop.Rows[0]["ConactNumber"].ToString();
                txtInvoiceFooterNote.Text = dtShop.Rows[0]["InvoiceFooterNote"].ToString();
                txtInvoiceFooterNoteShort.Text = dtShop.Rows[0]["InvoiceFooterNoteShort"].ToString();
                btnSave.Text = "Update";
            }
        }
        catch (Exception ex)
        {
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Text = ex.Message;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {        
        lblError.Text = string.Empty;
        if (txtShopName.Text.Length > 0)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    if (shop.UpdateShop(1, txtShopName.Text, txtAddress.Text,txtShortAddress.Text, txtContactPerson.Text, txtConactNo.Text, txtInvoiceFooterNote.Text,txtInvoiceFooterNoteShort.Text, CookieContext.UserID, CookieContext.DBServer) > 0)
                    {
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "Shop Updated Sucessfully!";
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        GetShop();
    }
}