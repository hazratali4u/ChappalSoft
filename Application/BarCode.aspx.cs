using System;
using System.Data;
using ChappalBLL;
using System.Web.Services;
using System.Web.Script.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZXing;
using ZXing.Rendering;
using ZXing.QrCode;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

public partial class BarCode : System.Web.UI.Page
{
    CategoryController category = new CategoryController();
    ItemController item = new ItemController();
    ColorController color = new ColorController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCategory();
            LoadItem();
            LoadColor();
        }
    }
    private void LoadCategory()
    {
        DataTable dtCategory = category.GetCategory(3);
        ddlCategory.DataSource = dtCategory;
        ddlCategory.DataTextField = "Name";
        ddlCategory.DataValueField = "CategoryID";
        ddlCategory.DataBind();
    }
    private void LoadItem()
    {
        DataTable dtItem = item.GetItem(2);
        ddlItem.DataSource = dtItem;
        ddlItem.DataTextField = "Name";
        ddlItem.DataValueField = "ItemID";
        ddlItem.DataBind();
    }
    private void LoadColor()
    {
        DataTable dtColor = color.GetColor(3);
        ddlColor.DataSource = dtColor;
        ddlColor.DataTextField = "Name";
        ddlColor.DataValueField = "ColorID";
        ddlColor.DataBind();
        Session.Add("dtColor", dtColor);
    }

    [WebMethod]
    [ScriptMethod]
    public static string GenerateSticker(string ItemID,string ItemName,string ColorID,string ColorName)
    {
        if (ColorID == "0")
        {
            DataTable dtColor = (DataTable) HttpContext.Current.Session["dtColor"];
            BarcodeWriter barcode = new BarcodeWriter();
            barcode.Format = BarcodeFormat.CODE_128;
            barcode.Renderer = new BitmapRenderer()
            {
                TextFont = new Font("Arial", 11f, FontStyle.Bold)
            };

            var qrCodeWriter = new BarcodeWriterPixelData();
            qrCodeWriter.Options = new QrCodeEncodingOptions
            {
                Margin = 0
            };

            DataTable dtReport = new DataTable();
            dtReport.Columns.Add("Image", typeof(string));
            dtReport.Columns.Add("ItemName", typeof(string));
            dtReport.Columns.Add("ColorName", typeof(string));
            foreach (DataRow drColor in dtColor.Rows)
            {
                if (drColor["ColorID"].ToString() != "0")
                {
                    var barcodeInBitmap = barcode.Write(ItemID + ":" + drColor["ColorID"].ToString());
                    var ms = new MemoryStream();
                    barcodeInBitmap.Save(ms, ImageFormat.Png);

                    var bytearray = ms.ToArray();
                    var base64Data = Convert.ToBase64String(ms.ToArray());
                    DataRow dr = dtReport.NewRow();
                    dr["Image"] = base64Data;
                    dr["ItemName"] = ItemName;
                    dr["ColorName"] = drColor["Name"].ToString();
                    dtReport.Rows.Add(dr);
                }
            }
            return GetJson(dtReport);
        }
        else
        {
            BarcodeWriter barcode = new BarcodeWriter();
            barcode.Format = BarcodeFormat.CODE_128;
            barcode.Renderer = new BitmapRenderer()
            {
                TextFont = new Font("Arial", 11f, FontStyle.Bold)
            };

            var qrCodeWriter = new BarcodeWriterPixelData();
            qrCodeWriter.Options = new QrCodeEncodingOptions
            {
                Margin = 0
            };

            var barcodeInBitmap = barcode.Write(ItemID + ":" + ColorID);
            var ms = new MemoryStream();
            barcodeInBitmap.Save(ms, ImageFormat.Png);

            var bytearray = ms.ToArray();
            var base64Data = Convert.ToBase64String(ms.ToArray());

            DataTable dtReport = new DataTable();
            dtReport.Columns.Add("Image", typeof(string));
            dtReport.Columns.Add("ItemName", typeof(string));
            dtReport.Columns.Add("ColorName", typeof(string));

            DataRow dr = dtReport.NewRow();
            dr["Image"] = base64Data;
            dr["ItemName"] = ItemName;
            dr["ColorName"] = ColorName;
            dtReport.Rows.Add(dr);
            return GetJson(dtReport);
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
}