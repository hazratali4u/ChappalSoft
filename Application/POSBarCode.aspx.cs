using System;
using System;
using System.Data;
using ChappalBLL;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using System.Web;

public partial class POSBarCode1 : System.Web.UI.Page
{
    CategoryController category = new CategoryController();
    ItemController item = new ItemController();
    ColorController color = new ColorController();
    SizeController size = new SizeController();
    ShopController shop = new ShopController();
    PartyController party = new PartyController();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hfUserID.Value = CookieContext.UserID.ToString();
            hfWorkingDate.Value = CookieContext.WorkingDate.ToString();
            LoadCustomer();
            LoadItem();
            LoadColor();
            LoadSize();
            DataTable dtShop = shop.GetShop(1);
            if(dtShop.Rows.Count > 0)
            {
                hfInvoiceFooterNote.Value = dtShop.Rows[0]["InvoiceFooterNote"].ToString();
                hfInvoiceFooterNoteShort.Value = dtShop.Rows[0]["InvoiceFooterNoteShort"].ToString();
                hfAddress.Value = dtShop.Rows[0]["Address"].ToString();
                hfAddressShort.Value = dtShop.Rows[0]["AddressShort"].ToString();
                hfPhone.Value = dtShop.Rows[0]["ConactNumber"].ToString();                
            }
        }
    }

    private void LoadCustomer()
    {
        DataTable dtCustomer = party.GetParty(6);
        ddlCustomer.DataSource = dtCustomer;
        ddlCustomer.DataTextField = "Name";
        ddlCustomer.DataValueField = "PartyID";
        ddlCustomer.DataBind();
    }

    private void LoadItem()
    {
        DataTable dtItem = item.GetItem(2);
        hfItemIDs.Value = GetJson(dtItem);        
    }

    private void LoadColor()
    {
        DataTable dtColor = color.GetColor(2);
        hfColorIDs.Value = GetJson(dtColor);
    }

    private void LoadSize()
    {
        DataTable dtSize = size.GetSize(2);
        hfSizeIDs.Value = GetJson(dtSize);
    }

    [WebMethod]
    [ScriptMethod]
    public static string LoadStock()
    {
        ItemController item = new ItemController();
        DataTable dtStock = item.GetItem(3, CookieContext.WorkingDate);
        return GetJson(dtStock);
    }

    [WebMethod]
    [ScriptMethod]
    public static string SaveOrder(string dtItems, int SaleType,int PaymentMode,int CustomerID, int GrossAmount,int Discount,int AmountPaid,int UserID,string WorkingDate, int IsPrinted)
    {
        if (dtItems.Length > 0)
        {
            DataTable dt = new DataTable();
            SaleController Sale = new SaleController();
            DataTable dtItemDetail = new DataTable();
            dtItemDetail = (DataTable)JsonConvert.DeserializeObject(dtItems, (typeof(DataTable)));
            if (dtItemDetail.Rows.Count > 0)
            {
                int SaleID = Sale.InsertSale(SaleType, CustomerID, PaymentMode, GrossAmount, Discount, AmountPaid, dtItemDetail, UserID, Convert.ToDateTime(WorkingDate), Convert.ToBoolean(IsPrinted), CookieContext.DBServer);
                if (SaleID > 0)
                {
                    dt = Sale.GetSale(SaleID, CookieContext.DBServer);
                }
                return GetJson(dt);
            }
            else
            {
                return string.Empty;
            }
        }
        return string.Empty;
    }

    [WebMethod]
    [ScriptMethod]
    public static string InsertCustomer(string Name, string Address, string ContactNo)
    {        
        DataTable dt = new DataTable();
        PartyController party = new PartyController();
        int CustomerID = party.InsertCustomer(Name, Address, ContactNo, CookieContext.UserID, CookieContext.DBServer);
        return CustomerID.ToString();
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
}