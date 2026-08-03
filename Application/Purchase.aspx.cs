using System;
using System.Data;
using ChappalBLL;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public partial class Purchase : System.Web.UI.Page
{
    CategoryController category = new CategoryController();
    ItemController item = new ItemController();
    PartyController party = new PartyController();
    ColorController color = new ColorController();
    SizeController size = new SizeController();
    PurchaseController purchase = new PurchaseController();
    DataTable dtItems;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hfUserID.Value = CookieContext.UserID.ToString();
            hfWorkingDate.Value = CookieContext.WorkingDate.ToString();
            LoadDocNo();
            LoadVendor();
            LoadCategory();
            LoadItem();
            LoadColor();
            LoadSize();
            CreateTable();
        }
    }

    private void LoadDocNo()
    {
        DataTable dtDocNo = purchase.GetPurchase(1,0, CookieContext.DBServer);
        ddlDocNo.DataSource = dtDocNo;
        ddlDocNo.DataTextField = "DocNo";
        ddlDocNo.DataValueField = "PurchaseID";
        ddlDocNo.DataBind();
        Session.Add("dtDocNo", dtDocNo);
    }

    private void LoadItem()
    {
        DataTable dtItem = item.GetItem(2);
        ddlItem.DataSource = dtItem;
        ddlItem.DataTextField = "Name";
        ddlItem.DataValueField = "ItemID";
        ddlItem.DataBind();
        Session.Add("dtItem", dtItem);
        hfItemIDs.Value = GetJson(dtItem);
    }

    private void LoadVendor()
    {
        DataTable dtVendor = party.GetParty(3);
        ddlSupplier.DataSource = dtVendor;
        ddlSupplier.DataTextField = "Name";
        ddlSupplier.DataValueField = "PartyID";
        ddlSupplier.DataBind();
    }

    private void LoadCategory()
    {
        DataTable dtCategory = category.GetCategory(3);
        ddlCategory.DataSource = dtCategory;
        ddlCategory.DataTextField = "Name";
        ddlCategory.DataValueField = "CategoryID";
        ddlCategory.DataBind();
    }

    private void LoadColor()
    {
        DataTable dtColor = color.GetColor(2);
        ddlColor.DataSource = dtColor;
        ddlColor.DataTextField = "Name";
        ddlColor.DataValueField = "ColorID";
        ddlColor.DataBind();
    }

    private void LoadSize()
    {
        DataTable dtSize = size.GetSize(2);
        gvSize.DataSource = dtSize;
        gvSize.DataBind();

        if (gvSize.Rows.Count > 0)
        {
            TextBox txt = gvSize.Rows[0].FindControl("txtQuantity") as TextBox;
            if (txt != null)
            {
                txt.Focus();
            }
        }

        if(hfSizeQtyJson.Value.Length >0)
        {
            List<SizeQty> sizeQtyList = JsonConvert.DeserializeObject<List<SizeQty>>(hfSizeQtyJson.Value);
            foreach (var item in sizeQtyList)
            {
                int sizeId = item.Size;
                int quantity = item.Qty;

                foreach(GridViewRow gvr in gvSize.Rows)
                {
                    if(gvr.Cells[0].Text == sizeId.ToString())
                    {
                        TextBox txtQuantity = (TextBox)gvr.FindControl("txtQuantity");
                        txtQuantity.Text = quantity.ToString();
                    }
                }
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        LoadItem();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        int GrommAmount = 0;
        int Discount = 0;
        int NetAmount = 0;
        if (ddlSupplier.Items.Count == 0)
        {
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Text = "Supplier not found.";
            return;
        }

        if (gvItem.Rows.Count > 0)
        {
            DataTable dtItemDetail = new DataTable();
            dtItemDetail.Columns.Add("ItemID", typeof(int));
            dtItemDetail.Columns.Add("ColorID", typeof(int));
            dtItemDetail.Columns.Add("SizeID", typeof(int));
            dtItemDetail.Columns.Add("Quantity", typeof(int));
            dtItemDetail.Columns.Add("Price", typeof(int));
            dtItemDetail.Columns.Add("Amount", typeof(int));
            dtItemDetail.Columns.Add("StockDate", typeof(DateTime));            
            dtItems = (DataTable)Session["dtItems"];
            foreach(DataRow dr in dtItems.Rows)
            {
                if (dr["SizeQtyJson"].ToString().Length > 0)
                {
                    List<SizeQty> sizeQtyList = JsonConvert.DeserializeObject<List<SizeQty>>(dr["SizeQtyJson"].ToString());
                    foreach (var item in sizeQtyList)
                    {
                        int sizeId = item.Size;
                        int quantity = item.Qty;
                        DataRow drDetail = dtItemDetail.NewRow();
                        drDetail["ItemID"] = dr["ItemID"];
                        drDetail["ColorID"] = dr["ColorID"];
                        drDetail["SizeID"] = item.Size;
                        drDetail["Quantity"] = item.Qty;
                        drDetail["Price"] = dr["Price"];
                        drDetail["Amount"] = item.Qty * Convert.ToInt32(dr["Price"]);
                        drDetail["StockDate"] = CookieContext.WorkingDate;
                        dtItemDetail.Rows.Add(drDetail);
                    }
                }                
            }
            GrommAmount = dtItemDetail.Rows.Cast<DataRow>().Sum(dr => int.Parse(dr["Amount"].ToString()));
            NetAmount = GrommAmount - Discount;
            bool flag = true;
            if (ddlDocNo.SelectedValue == "0")
            {                flag = purchase.InsertPurchase(txtPurchaseNo.Text, Convert.ToInt32(ddlSupplier.SelectedValue), GrommAmount, Discount, NetAmount, dtItemDetail, Convert.ToInt32(hfUserID.Value),Convert.ToDateTime(hfWorkingDate.Value), CookieContext.DBServer);
                if (flag)
                {
                    lblError.ForeColor = System.Drawing.Color.Green;
                    lblError.Text = "Purchase Saved successfully.";
                }
                else
                {
                    lblError.ForeColor = System.Drawing.Color.Red;
                    lblError.Text = "Some error occured.";
                }
            }
            else
            {
                flag = purchase.UpdatePurchase(Convert.ToInt32(ddlDocNo.SelectedValue), txtPurchaseNo.Text, Convert.ToInt32(ddlSupplier.SelectedValue), GrommAmount, Discount, NetAmount, dtItemDetail, Convert.ToInt32(hfUserID.Value), CookieContext.DBServer);
                if (flag)
                {
                    lblError.ForeColor = System.Drawing.Color.Green;
                    lblError.Text = "Purchase updated successfully.";
                }
                else
                {
                    lblError.ForeColor = System.Drawing.Color.Red;
                    lblError.Text = "Some error occured.";
                }
            }
            if(flag)
            {
                ClearDetailControls();
                ClearMasterControls();
                LoadDocNo();
            }
        }
        else
        {
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Text = "No item found.";
        }
    }

    protected void lnkRemove_Click(object sender, EventArgs e)
    {

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        if (txtQty.Text.Length > 0 && txtPrice.Text.Length > 0)
        {
            dtItems = (DataTable)Session["dtItems"];
            if (btnAdd.Text == "Add")
            {
                if (!ItemExist())
                {
                    int qty=0, price = 0;
                    DataRow dr = dtItems.NewRow();
                    dr["ItemID"] = ddlItem.SelectedValue;
                    dr["ItemName"] = ddlItem.SelectedItem.Text;
                    dr["ColorID"] = ddlColor.SelectedValue;
                    //dr["SizeID"] = ddlSize.SelectedValue;
                    dr["Color"] = ddlColor.SelectedItem.Text;
                    dr["Size"] = "";
                    dr["Quantity"] = txtQty.Text;
                    if(txtQty.Text.Length > 0)
                    {
                        qty = Convert.ToInt32(txtQty.Text);
                    }
                    dr["Price"] = txtPrice.Text;
                    if(txtPrice.Text.Length > 0)
                    {
                        price = Convert.ToInt32(txtPrice.Text);
                    }
                    dr["Amount"] = (qty * price).ToString();
                    dr["SizeQtyJson"] = hfSizeQtyJson.Value;
                    dtItems.Rows.Add(dr);
                    gvItem.DataSource = dtItems;
                    gvItem.DataBind();
                    ClearDetailControls();
                    Session.Add("dtItems", dtItems);
                }
                else
                {
                    lblError.ForeColor = System.Drawing.Color.Red;
                    lblError.Text = "Item with same color and size exists.";
                    ddlItem.Focus();
                }
            }
        }
        else
        {
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Text = "Quantity and Price required.";
        }
    }

    private void CreateTable()
    {
        dtItems = new DataTable();
        dtItems.Columns.Add("ItemID", typeof(int));
        dtItems.Columns.Add("ItemName", typeof(string));
        dtItems.Columns.Add("ColorID", typeof(int));
        dtItems.Columns.Add("SizeID", typeof(int));
        dtItems.Columns.Add("Color", typeof(string));
        dtItems.Columns.Add("Size", typeof(string));
        dtItems.Columns.Add("Quantity", typeof(int));
        dtItems.Columns.Add("Price", typeof(int));
        dtItems.Columns.Add("Amount", typeof(int));
        dtItems.Columns.Add("StockDate", typeof(DateTime));
        dtItems.Columns.Add("SizeQtyJson", typeof(string));
        Session.Add("dtItems", dtItems);
    }

    private void ClearMasterControls()
    {
        txtPurchaseNo.Text = string.Empty;
        txtPrice.Text = string.Empty;
        dtItems = (DataTable)Session["dtItems"];
        dtItems.Rows.Clear();
        gvItem.DataSource = dtItems;
        gvItem.DataBind();
        Session.Add("dtItems", dtItems);
    }

    private void ClearDetailControls()
    {
        txtQty.Text = string.Empty;
        txtAmount.Text = string.Empty;
        hfSizeQtyJson.Value = string.Empty;
        btnAdd.Text = "Add";
    }

    private bool ItemExist()
    {
        bool flag = false;
        dtItems = (DataTable)Session["dtItems"];
        foreach(DataRow dr in dtItems.Rows)
        {
            if(dr["ItemID"].ToString() == ddlItem.SelectedValue && dr["ColorID"].ToString() == ddlColor.SelectedValue)
            {
                flag = true;
                break;
            }
        }
        return flag;
    }

    protected void gvItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        dtItems = (DataTable)Session["dtItems"];
        if (dtItems.Rows.Count > 0)
        {
            dtItems.Rows.RemoveAt(e.RowIndex);
            gvItem.DataSource = dtItems;
            gvItem.DataBind();
            ClearDetailControls();
            Session.Add("dtItems", dtItems);
        }
    }

    public static string GetJson(DataTable dt)
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        serializer.MaxJsonLength = Int32.MaxValue;
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row = null;

        foreach (DataRow dr in dt.Rows)
        {
            row = dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col]);
            rows.Add(row);
        }
        return serializer.Serialize(rows);
    }

    protected void ddlDocNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(ddlDocNo.SelectedValue == "0")
        {
            ClearDetailControls();
            ClearMasterControls();
        }
        else
        {
            DataTable dtDocNo = (DataTable)Session["dtDocNo"];
            foreach(DataRow dr in dtDocNo.Rows)
            {
                if(dr["PurchaseID"].ToString() == ddlDocNo.SelectedValue)
                {
                    ddlSupplier.SelectedValue = dr["SupplierID"].ToString();
                    txtPurchaseNo.Text = dr["PurchaseNo"].ToString();
                }
            }
            dtItems = purchase.GetPurchase(2, Convert.ToInt32(ddlDocNo.SelectedValue),1);
            Session.Add("dtItems", dtItems);
            gvItem.DataSource = dtItems;
            gvItem.DataBind();
        }
    }

    protected void lnkSize_Click(object sender, EventArgs e)
    {
        ddlDocNo.Enabled = false;
        ddlSupplier.Enabled = false;
        txtPurchaseNo.Enabled = false;
        ddlCategory.Enabled = false;
        ddlItem.Enabled = false;
        btnAdd.Enabled = false;
        btnSave.Enabled = false;
        btnCancel.Enabled = false;
        lblNameSize.InnerHtml= "Item: " + ddlItem.SelectedItem.Text;
        lblIColoeSize.InnerHtml = "Color: " + ddlColor.SelectedItem.Text;
        Session.Add("ItemID", ddlItem.SelectedValue);
        LoadSize();
        dvSize.Visible = true;
    }

    protected void btnCancelSize_Click(object sender, EventArgs e)
    {
        ddlDocNo.Enabled = true;
        ddlSupplier.Enabled = true;
        txtPurchaseNo.Enabled = true;
        ddlCategory.Enabled = true;
        ddlItem.Enabled = true;
        btnAdd.Enabled = true;
        btnSave.Enabled = true;
        btnCancel.Enabled = true;
        dvSize.Visible = false;
        ddlCategory_SelectedIndexChanged(null, null);
        ddlItem.SelectedValue = Session["ItemID"].ToString();
    }

    protected void btnDoneSize_Click(object sender, EventArgs e)
    {
        int Qty = 0;
        foreach(GridViewRow gvr in gvSize.Rows)
        {
            TextBox txtQuantity = (TextBox)gvr.Cells[2].FindControl("txtQuantity");
            if(txtQuantity.Text.Length > 0)
            {
                Qty += Convert.ToInt32(txtQuantity.Text);
            }
        }
        if (Qty > 0)
        {
            AddSizeQty();
            txtQty.Text = Qty.ToString();
            btnCancelSize_Click(null, null);
            txtPrice.Focus();
        }
        else
        {
            string rawMessage = "Must enter size quantity!";
            string safeMessage = rawMessage.Replace("'", "\\'");
            string script = "alert('" + safeMessage + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", script, true);
        }
        ddlCategory_SelectedIndexChanged(null, null);
        ddlItem.SelectedValue = Session["ItemID"].ToString();
    }

    private void AddSizeQty()
    {
        List<SizeQty> sizeQtyList = new List<SizeQty>();
        foreach (GridViewRow row in gvSize.Rows)
        {
            int sizeId = Convert.ToInt32(row.Cells[0].Text);
            TextBox txtQuantity = row.FindControl("txtQuantity") as TextBox;
            int qty = 0;            
            if (txtQuantity.Text.Length > 0)
            {
                qty = Convert.ToInt32(txtQuantity.Text);
                sizeQtyList.Add(new SizeQty
                {
                    Size = sizeId,
                    Qty = qty
                });
            }
        }
        string jsonResult = JsonConvert.SerializeObject(sizeQtyList);
        hfSizeQtyJson.Value = jsonResult;
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItem.Items.Clear();
        int categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
        var dtItem = (DataTable)Session["dtItem"];
        if (categoryId == 0)
        {
            foreach (DataRow dr in dtItem.Rows)
            {
                ddlItem.Items.Add(new ListItem(dr["Name"].ToString(), dr["ItemID"].ToString()));
            }
        }
        else
        {
            DataRow[] filteredRows = dtItem.Select("CategoryID = " + categoryId);
            foreach (DataRow dr in filteredRows)
            {
                ddlItem.Items.Add(new ListItem(dr["Name"].ToString(), dr["ItemID"].ToString()));
            }
        }
    }
}
public class SizeQty
{
    public int Size { get; set; }
    public int Qty { get; set; }
}